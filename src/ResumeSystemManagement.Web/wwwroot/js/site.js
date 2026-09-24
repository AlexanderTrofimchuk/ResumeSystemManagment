function changePasswordIcon(idInput, idIcon, idButton) {
    const button = document.getElementById(idButton);
    if (!button) return;

    button.addEventListener('click', function () {
        const input = document.getElementById(idInput);
        const icon = document.getElementById(idIcon);
        const isPassword = input.type === 'password';

        input.type = isPassword ? 'text' : 'password';
        icon.classList.toggle('bi-eye');
        icon.classList.toggle('bi-eye-slash');
    });
}