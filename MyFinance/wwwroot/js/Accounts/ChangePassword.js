$(function () {
    const subheader = createSubheader({
        title: 'Cambiar contraseña',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            { label: 'Mi Perfil', url: '/Accounts/Profile' },
            'Cambiar contraseña'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
});

$("#btn-save").on("click", function (e) {
    e.preventDefault();
    const $btn = $(this);

    if ($("#formChangePassword").valid()) {
        $btn.prop("disabled", true);
        $btn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...');

        $.ajax({
            url: "/Accounts/ChangePassword",
            type: "POST",
            data: $("#formChangePassword").serialize(),
        }).done(function (response) {
            if (response.result) {
                Swal.fire({
                    icon: 'success',
                    text: response.message,
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function (result) {
                    window.location.href = '/Accounts/Profile';
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