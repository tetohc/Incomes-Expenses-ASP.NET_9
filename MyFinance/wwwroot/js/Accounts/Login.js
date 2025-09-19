$("#btn-singIn").on("click", function (e) {
    e.preventDefault();
    const $btn = $(this);

    if ($("#formLogin").valid()) {
        $btn.prop("disabled", true);
        $btn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Iniciando sesión...');

        $.ajax({
            url: "/Accounts/SignIn",
            type: "POST",
            data: $("#formLogin").serialize(),
        }).done(function (response) {
            if (response.result) {
                Swal.fire({
                    icon: 'success',
                    text: response.message,
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function () {
                    window.location.href = '/Home/Index';
                });
            } else {
                showError(response.message);
            }
        }).fail(function () {
            showError("Ha ocurrido un error al iniciar sesión.");
        }).always(function () {
            $btn.prop("disabled", false);
            $btn.html('Iniciar sesión');
        });
    } else {
        showError("Por favor, complete todos los campos requeridos.", icon = "warning");
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