$(function () {
    const subheader = createSubheader({
        title: 'Nuevo Servicio',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            { label: 'Servicios', url: '/Services' },
            'Nuevo servicio'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
});

$("#btn-create").on("click", function (e) {
    e.preventDefault();

    const $btn = $(this);

    if ($("#formCreateService").valid()) {
        $btn.prop("disabled", true);

        $.ajax({
            url: "/Services/Create",
            type: "POST",
            data: $("#formCreateService").serialize(),
        }).done(function (success) {
            if (success) {
                Swal.fire({
                    icon: 'success',
                    text: 'El servicio se ha guardado correctamente.',
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function (result) {
                    window.location.href = '/Services/Index';
                });
            } else {
                showError();
            }
        }).fail(function () {
            showError();
        }).always(function () {
            $btn.prop("disabled", false);
        });
    }
});

function showError() {
    Swal.fire({
        icon: 'error',
        text: 'Ha ocurrido un error al guardar el servicio.',
        showConfirmButton: true,
        confirmButtonText: 'Cerrar'
    });
}