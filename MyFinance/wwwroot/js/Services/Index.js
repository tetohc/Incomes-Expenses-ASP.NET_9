let dataTable;

function updateDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }

    dataTable = $('#servicesTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.2/i18n/es-ES.json',
        },
        responsive: true,
        "ajax": {
            "url": "/Services/GetServices",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name" },
            { "data": "typeDisplay" },
            {
                data: "id", class: "text-center", render: function (data) {
                    return `
                        <div class="dropdown">
                            <button class="btn btn-action dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" title="Opciones">
                                 <i class="bi bi-menu-button-wide-fill"></i>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-custom">
                                <li>
                                    <a class="dropdown-item" href="/Services/Edit/${data}">
                                        <i class="bi bi-pencil-square me-2"></i> Editar
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item" href="#" onclick="Delete('${data}')">
                                        <i class="bi bi-trash me-2"></i> Eliminar
                                    </a>
                                </li>
                            </ul>
                        </div>
                    `;
                }
            }
        ]
    });
}

$(function () {
    const subheader = createSubheader({
        title: 'Servicios',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            'Servicios'
        ],
        buttonText: 'Agregar Servicio',
        buttonAction: '/Services/Create',
        buttonTooltip: 'Crea un nuevo servicio'
    });
    document.querySelector('#subheader-container').appendChild(subheader);

    updateDataTable();
});


function Delete(id) {
    Swal.fire({
        title: '¿Estás seguro de que deseas eliminar este servicio?',
        text: 'Las transacciones asociadas a este servicio también desaparecerán.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#5369D3',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'No, cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Services/Delete?id=' + id,
                type: 'Delete',
                success: function (data) {
                    if (data) {
                        Swal.fire({
                            title: '¡Eliminado!',
                            text: 'El servicio ha sido eliminado correctamente.',
                            icon: 'success',
                            confirmButtonText: 'Aceptar',
                        }).then(() => {
                            updateDataTable();
                        });
                    } else {
                        Swal.fire(
                            '!Algo salió mal!',
                            'El servicio no ha sido eliminado.',
                            'error'
                        );
                    }
                }
            });
        }
    })
}