$(function () {
    const subheader = createSubheader({
        title: 'Actualizar Servicio',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            { label: 'Servicios', url: '/Services/Index' },
            'Actualizar servicio'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
});

$("#btn-update").on("click", function (e) {
    e.preventDefault();

    const $btn = $(this);
    const $spinner = $btn.find(".spinner-border");
    const $btnText = $btn.find(".btn-text");

    if ($("#formEditService").valid()) {
        $btn.prop("disabled", true);
        $spinner.removeClass("d-none");
        $btnText.html('Actualizando...');

        $.ajax({
            url: "/Services/Edit",
            type: "POST",
            data: $("#formEditService").serialize(),
        }).done(function (success) {
            if (success) {
                Swal.fire({
                    icon: 'success',
                    text: 'El servicio se ha actualizado correctamente.',
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function () {
                    window.location.href = '/Services';
                });
            } else {
                showError();
            }
        }).fail(function () {
            showError();
        }).always(function () {
            $btn.prop("disabled", false);
            $spinner.addClass("d-none");
            $btnText.html('<i class="bi bi-save"></i> Guardar cambios');
        });
    }
});

function showError() {
    Swal.fire({
        icon: 'error',
        text: 'Ha ocurrido un error al actualizar el servicio.',
        showConfirmButton: true,
        confirmButtonText: 'Cerrar'
    });
}