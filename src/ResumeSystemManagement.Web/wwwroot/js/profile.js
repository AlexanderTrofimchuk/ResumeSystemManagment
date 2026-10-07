const configElement = document.getElementById('profile-config');
const profileConfig = configElement ? JSON.parse(configElement.textContent) : null;
const pendingSaves = new Set();
let saveTimer;
let saveInFlight;

function getProfileAttributes(sectionSelector) {
    return [...document.querySelectorAll(`${sectionSelector} [data-attribute-id]`)]
        .map(element => ({
            id: Number(element.dataset.recordId),
            userId: element.dataset.userId,
            attributeId: Number(element.dataset.attributeId),
            title: element.dataset.title,
            typeName: element.dataset.typeName,
            value: element.type === 'checkbox' ? String(element.checked) : element.value,
            dropdownOptions: null,
            version: Number(element.dataset.version)
        }));
}

async function saveProfileSection(sectionName) {
    const isInfo = sectionName === 'info';
    const response = await fetch(isInfo ? profileConfig.urls.saveInfo : profileConfig.urls.saveMe, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            ...(isInfo ? { 'RequestVerificationToken': getProfileAntiForgeryToken() } : {})
        },
        body: JSON.stringify(getProfileAttributes(isInfo ? '#section-info' : '#section-me'))
    });

    if (!response.ok) {
        throw new Error(`Profile ${sectionName} save failed (${response.status}).`);
    }
}

function getProfileAntiForgeryToken() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    if (!token) {
        throw new Error('The anti-forgery token was not found.');
    }
    return token;
}

function scheduleProfileSave(sectionName, delay = 700) {
    pendingSaves.add(sectionName);
    clearTimeout(saveTimer);
    saveTimer = setTimeout(flushProfileSaves, delay);
}

async function flushProfileSaves() {
    if (saveInFlight) return saveInFlight;

    saveInFlight = processPendingProfileSaves();
    try {
        await saveInFlight;
    } finally {
        saveInFlight = null;
    }
}

async function processPendingProfileSaves() {
    let failed = false;

    while (pendingSaves.size) {
        const sectionName = pendingSaves.values().next().value;
        pendingSaves.delete(sectionName);
        try {
            await saveProfileSection(sectionName);
        } catch (error) {
            console.error(profileConfig.messages.saveFailed, error);
            pendingSaves.add(sectionName);
            failed = true;
            break;
        }
    }

    if (pendingSaves.size) {
        clearTimeout(saveTimer);
        saveTimer = setTimeout(flushProfileSaves, failed ? 3000 : 200);
    }
}

function initializeProfileAttributeFields(section) {
    const sectionName = section.id === 'section-info' ? 'info' : 'me';
    section.querySelectorAll('.text-editor').forEach(element => {
        const editor = new EasyMDE({ element });
        editor.codemirror.on('change', () => scheduleProfileSave(sectionName));
    });
    section.querySelectorAll('.date-picker').forEach(element => {
        flatpickr(element, {
            dateFormat: 'Y-m-d',
            onChange: () => scheduleProfileSave(sectionName)
        });
    });
    section.querySelectorAll('.period-picker').forEach(element => {
        flatpickr(element, {
            mode: 'range',
            dateFormat: 'Y-m-d',
            onChange: () => scheduleProfileSave(sectionName)
        });
    });
}

function handleProfileAttributeInput(event) {
    if (!event.target.matches('[data-attribute-id]')) return;
    scheduleProfileSave(event.currentTarget.id === 'section-info' ? 'info' : 'me');
}

function startProfileAutoSave() {
    ['section-me', 'section-info'].forEach(sectionId => {
        const section = document.getElementById(sectionId);
        section.addEventListener('input', handleProfileAttributeInput);
        section.addEventListener('change', handleProfileAttributeInput);
        initializeProfileAttributeFields(section);
    });
}

async function openCreateSalesforceForm() {
    const container = document.getElementById('createSalesForceFormContainer');
    try {
        const response = await fetch(profileConfig.urls.createForm);
        if (!response.ok) {
            throw new Error(`Loading the Salesforce form failed (${response.status}).`);
        }

        container.innerHTML = await response.text();
        const modal = container.querySelector('#createSalesForceModal');
        if (!modal) throw new Error('The Salesforce form was not returned by the server.');
        bootstrap.Modal.getOrCreateInstance(modal).show();
    } catch (error) {
        console.error('Could not open the Salesforce form.', error);
        window.alert(profileConfig.messages.loadSalesforceFailed);
    }
}

async function openAddProfileAttributeModal() {
    const container = document.getElementById('addAttributeModalContainer');
    try {
        let modal = container.querySelector('#modalAddAttribute');
        if (!modal) {
            const response = await fetch(profileConfig.urls.attributeLibrary);
            if (!response.ok) {
                throw new Error(`Loading the attribute library failed (${response.status}).`);
            }

            container.innerHTML = await response.text();
            modal = container.querySelector('#modalAddAttribute');
            if (!modal) throw new Error('The attribute modal was not returned by the server.');
        }

        disableExistingProfileAttributes(modal);
        const addButton = modal.querySelector('#addSelectedAttributes');
        if (!addButton.dataset.handlerAttached) {
            addButton.addEventListener('click', addSelectedProfileAttributes);
            addButton.dataset.handlerAttached = 'true';
        }
        bootstrap.Modal.getOrCreateInstance(modal).show();
    } catch (error) {
        console.error('Could not open the add-attribute modal.', error);
        window.alert('Could not load the attribute library. Please try again.');
    }
}

function disableExistingProfileAttributes(modal) {
    const existingIds = new Set(
        [...document.querySelectorAll('#infoAttributes [data-attribute-id]')]
            .map(element => element.dataset.attributeId)
    );
    modal.querySelectorAll('.attribute-option').forEach(option => {
        option.disabled = existingIds.has(option.value);
    });
}

async function addProfileAttribute(option, modal) {
    const formData = new FormData();
    formData.append(
        '__RequestVerificationToken',
        modal.querySelector('input[name="__RequestVerificationToken"]').value
    );
    formData.append('Id', option.value);
    formData.append('Name', option.dataset.name);
    formData.append('Description', option.dataset.description || '');
    formData.append('TypeName', option.dataset.typeName || '');

    const response = await fetch(profileConfig.urls.addAttribute, {
        method: 'POST',
        body: formData
    });
    if (!response.ok) {
        throw new Error(`Adding attribute ${option.value} failed (${response.status}).`);
    }
}

async function addSelectedProfileAttributes(event) {
    const button = event.currentTarget;
    const modal = button.closest('#modalAddAttribute');
    const selectedOptions = [...modal.querySelectorAll('.attribute-option:checked:not(:disabled)')];
    if (!selectedOptions.length) {
        window.alert(profileConfig.messages.selectAttribute);
        return;
    }

    button.disabled = true;
    try {
        for (const option of selectedOptions) {
            await addProfileAttribute(option, modal);
        }
        window.location.reload();
    } catch (error) {
        console.error('Could not add the selected attributes.', error);
        window.alert(profileConfig.messages.addAttributesFailed);
        button.disabled = false;
    }
}

function updateDeleteSelectedAttributesButton() {
    const infoAttributes = document.getElementById('infoAttributes');
    document.getElementById('deleteSelectedAttributes').disabled =
        !infoAttributes.querySelector('.info-attribute-select:checked');
}

async function deleteSelectedProfileAttributes() {
    const infoAttributes = document.getElementById('infoAttributes');
    const deleteButton = document.getElementById('deleteSelectedAttributes');
    const selected = [...infoAttributes.querySelectorAll('.info-attribute-select:checked')];
    if (!selected.length || !window.confirm(profileConfig.messages.confirmDeleteAttributes)) return;

    deleteButton.disabled = true;
    try {
        await flushProfileSaves();
        const response = await fetch(profileConfig.urls.deleteAttributes, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getProfileAntiForgeryToken()
            },
            body: JSON.stringify(selected.map(checkbox => Number(checkbox.dataset.recordId)))
        });
        if (!response.ok) {
            throw new Error(`Deleting profile attributes failed (${response.status}).`);
        }

        selected.forEach(checkbox => checkbox.closest('.info-attribute').remove());
        if (!infoAttributes.querySelector('[data-attribute-id]')) {
            showEmptyProfileAttributes(infoAttributes);
        }
    } catch (error) {
        console.error('Could not delete the selected profile attributes.', error);
        window.alert(profileConfig.messages.deleteAttributeFailed);
    } finally {
        updateDeleteSelectedAttributesButton();
    }
}

function showEmptyProfileAttributes(container) {
    const emptyState = document.createElement('div');
    emptyState.id = 'infoEmptyState';
    emptyState.className = 'text-center py-5 text-secondary';

    const icon = document.createElement('i');
    icon.className = 'bi bi-collection fs-2 d-block mb-2';
    const text = document.createElement('p');
    text.className = 'mb-0 small';
    text.append(document.createTextNode(profileConfig.messages.noAttributesYet));
    text.append(
        document.createElement('br'),
        document.createTextNode(profileConfig.messages.clickToPickAttributes)
    );
    emptyState.append(icon, text);
    container.append(emptyState);
}

function startProfilePage() {
    if (!profileConfig) return;

    startProfileAutoSave();
    document.getElementById('openCreateSalesForceForm')
        .addEventListener('click', openCreateSalesforceForm);
    document.getElementById('openAddAttributeModal')
        .addEventListener('click', openAddProfileAttributeModal);
    document.getElementById('infoAttributes')
        .addEventListener('change', handleProfileAttributeSelection);
    document.getElementById('deleteSelectedAttributes')
        .addEventListener('click', deleteSelectedProfileAttributes);
}

function handleProfileAttributeSelection(event) {
    if (event.target.matches('.info-attribute-select')) {
        updateDeleteSelectedAttributesButton();
    }
}

startProfilePage();
