$(function () {
    const subheader = createSubheader({
        title: 'Nueva Transacción',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            { label: 'Transacciones', url: '/Transactions/Index' },
            'Nueva transacción'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);

    setSelectServices();

    // validación personalizada para campo Monto
    $('#TotalAmount').on('blur', function () {
        amountValidate(); 
    });

    // cargar servicios al cambiar el tipo
    $('#Type').on('change', function () {
        getServicesAsync();
    });
});

// Función para validar campo
const amountValidate = () => {
    const errorSpan = $("#totalAmountError");
    const currentError = errorSpan.text();

    const rulesValidation = {
        notNumber: {
            condition: currentError.includes("must be a number") || currentError.includes("not valid"),
            message: "Solo se permiten números en el campo Monto."
        },
        tooLow: {
            condition: currentError.includes("Please enter a value greater than or equal to 1"),
            message: "El monto ingresado debe ser mayor a 1."
        }
    };

    for (const key in rulesValidation) {
        const { condition, message } = rulesValidation[key];
        if (condition) {
            errorSpan.text(message);
            return;
        }
    }
    errorSpan.text("");
}

// Función inicializar Select2 para servicios
function setSelectServices() {
    $('#serviceSelect').select2({
        placeholder: 'Seleccione un servicio',
        width: '100%'
    });
}

// Función para cargar los servicios
async function getServicesAsync() {
    const typeService = $('#Type').val();
    const response = await fetch(`/Services/GetServicesByType?type=${typeService}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const data = await response.json();
        const serviceSelect = $('#serviceSelect');

        serviceSelect.empty(); 
        serviceSelect.append(new Option('Seleccione un servicio', '', true, true));
        data.forEach(service => {
            const option = new Option(service.name, service.id, false, false);
            serviceSelect.append(option);
        });

        serviceSelect.trigger('change.select2');
    }
}

$("#btn-create").on("click", function (e) {
    e.preventDefault();

    const $btn = $(this);

    if ($("#formCreateTransaction").valid()) {
        $btn.prop("disabled", true);

        $.ajax({
            url: "/Transactions/Create",
            type: "POST",
            data: $("#formCreateTransaction").serialize(),
        }).done(function (success) {
            if (success) {
                Swal.fire({
                    icon: 'success',
                    text: 'La transacción se ha guardado correctamente.',
                    showConfirmButton: true,
                    confirmButtonText: 'Continuar'
                }).then(function (result) {
                    window.location.href = '/Transactions/Index';
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
        text: 'Ha ocurrido un error al guardar la transacción.',
        showConfirmButton: true,
        confirmButtonText: 'Cerrar'
    });
}