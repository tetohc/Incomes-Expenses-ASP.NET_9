$(function () {
    const subheader = createSubheader({
        title: 'Registro de usuario',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            { label: 'Usuarios', url: '/Accounts/Users' },
            'Nuevo usuario'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
})

function togglePassword(fieldId) {
    const input = document.getElementById(fieldId);
    const icon = document.getElementById("icon" + fieldId);
    if (input.type === "password") {
        input.type = "text";
        icon.classList.remove("bi-eye");
        icon.classList.add("bi-eye-slash");
    } else {
        input.type = "password";
        icon.classList.remove("bi-eye-slash");
        icon.classList.add("bi-eye");
    }
}

$("#btn-create").on("click", function (e) {
    e.preventDefault();
    const $btn = $(this);

    if ($("#formCreateUser").valid()) {
        $btn.prop("disabled", true);
        $btn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...');

        $.ajax({
            url: "/Accounts/Register",
            type: "POST",
            data: $("#formCreateUser").serialize(),
        }).done(function (response) {
            if (response.result) {
                Swal.fire({
                    icon: 'success',
                    text: response.message,
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function (result) {
                    window.location.href = '/Accounts/Users';
                });
            } else {
                showError(response.message);
            }
        }).fail(function () {
            showError("Ocurrió un error inesperado.");
        }).always(function () {
            $btn.prop("disabled", false);
            $btn.html("Guardar");
        });
    }
});

function showError(message, icon = 'error') {
    Swal.fire({
        icon,
        text: message,
        showConfirmButton: true,
        confirmButtonText: 'Cerrar'
    });
}