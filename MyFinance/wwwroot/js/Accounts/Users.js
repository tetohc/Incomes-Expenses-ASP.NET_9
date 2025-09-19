let dataTable;

$(function () {
    const subheader = createSubheader({
        title: 'Usuarios',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            'Usuarios'
        ],
        buttonText: 'Registrar usuario',
        buttonAction: '/Accounts/Register',
        buttonTooltip: 'Crea un nuevo usuario'
    });
    document.querySelector('#subheader-container').appendChild(subheader);

    updateDataTable();
});

function updateDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }

    dataTable = $('#transactionsTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.2/i18n/es-ES.json',
        },
        responsive: true,
        "ajax": {
            "url": '/Accounts/GetUsers',
            "type": "GET",
            "datatype": "json"
        },
        ordering: false,
        "columns": [
            { "data": "fullName" },
            { "data": "email" },
            {
                data: "id", class: "text-center", render: function (data) {
                    return `
                        <div class="dropdown">
                            <button class="btn btn-action dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" title="Opciones">
                                <i class="bi bi-menu-button-wide-fill"></i>
                            </button>
                            <ul class="dropdown-menu">
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

function Delete(id) {
    Swal.fire({
        title: 'Eliminar usuario',
        text: '¿Estás seguro de que deseas eliminar este usuario?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#5369D3',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'No, cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Accounts/Delete?id=' + id,
                type: 'Delete',
                success: function (data) {
                    if (data) {
                        Swal.fire({
                            title: '¡Eliminado!',
                            text: 'El usuario ha sido eliminada correctamente.',
                            icon: 'success',
                            confirmButtonText: 'Aceptar',
                        }).then(() => {
                            updateDataTable();
                        });
                    } else {
                        Swal.fire(
                            '!Algo salió mal!',
                            'El usuario no ha sido eliminado.',
                            'error'
                        );
                    }
                }
            });
        }
    })
}