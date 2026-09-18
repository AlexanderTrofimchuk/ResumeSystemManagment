function initEditForm() {
    const modal      = document.getElementById('editAttributeModal');
    const typeSelect = modal.querySelector('select[name="TypeId"]');
    const section    = modal.querySelector('#editDropdownOptionsSection');
    const list       = modal.querySelector('#editDropdownOptionsList');
    const addBtn     = modal.querySelector('#addEditDropdownOption');

    if (!typeSelect || !section || !list || !addBtn) return;

    addBtn.addEventListener('click', () => addOption(list));
    typeSelect.addEventListener('change', () => toggleSection(typeSelect, section, list));
    addClickEventForList(list);
}
function toggleSection(typeSelect,section, list) {
    console.log('toggleSection called, value:', typeSelect.value, 'dropdownTypeId:', typeSelect.dataset.dropdownTypeId);
    if (isDropdown(typeSelect)) {
        section.style.display = 'block';
    } else {
        section.style.display = 'none';
        list.innerHTML = '';
    }
}

function isDropdown(typeSelect) {
    return typeSelect.value == getDropdownOptions(typeSelect);
}

function getDropdownOptions(typeSelect) {
    return  typeSelect.dataset.dropdownTypeId;
}

function reindex(list) {
    list.querySelectorAll('.input-group').forEach((row, i) => {
        const hidden = row.querySelector('input[type="hidden"]');
        const text   = row.querySelector('input[type="text"]');
        if (hidden) hidden.name = `DropdownOptions[${i}].Id`;
        if (text) {
            text.name        = `DropdownOptions[${i}].Value`;
            text.placeholder = `Вариант ${i + 1}`;
        }
    });
}

function addOption(list) {
    const i = list.querySelectorAll('.input-group').length;
    const row = document.createElement('div');
    row.className = 'input-group';
    row.innerHTML = `
            <input type="hidden" name="DropdownOptions[${i}].Id" value="0" />
            <input type="text" class="form-control"
                   name="DropdownOptions[${i}].Value"
                   placeholder="Вариант ${i + 1}"
                   autocomplete="off" />
            <button type="button" class="btn btn-outline-danger remove-edit-option" title="Удалить">
                <i class="bi bi-trash"></i>
            </button>`;
    list.appendChild(row);
    reindex(list);
}

function addClickEventForList(list) {
    list.addEventListener('click', function (e) {
        const btn = e.target.closest('.remove-edit-option');
        if (btn) {
            btn.closest('.input-group').remove();
            reindex(list);
        }
    });
}