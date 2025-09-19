let dataTable;
function updateDataTable(starDate = null, endDate = null)  {
    if (dataTable) {
        dataTable.destroy();
    }

    let ajaxUrl = "/Transactions/GetTransactions";
    if (starDate) {
        ajaxUrl = `/Transactions/GetTransactionsByDate?startDate=${starDate}&endDate=${endDate}`;
    }

    dataTable = $('#transactionsTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/2.3.2/i18n/es-ES.json',
        },
        responsive: true,
        "ajax": {
            "url": ajaxUrl,
            "type": "GET",
            "datatype": "json"
        },
        buttons: [
            'excelHtml5',
            'pdfHtml5',
            'csvHtml5',
        ],
        ordering: false,
        "columns": [
            {
                data: "service", render: function (data, typeTransaction, row) {
                    const serviceName = data;
                    const typeService = row.typeTransaction;
                    let colorClass = "text-bg-primary";
                    let textBadge = "";

                    switch (typeService) {
                        case 1:
                            textBadge = "Ingreso"; 
                            break;
                        case 2:
                            textBadge = "Gasto"; 
                            colorClass = "text-bg-danger";
                            break;
                    }

                    return `
                                <div>
                                    ${serviceName}<br>
                                    <span class="badge ${colorClass}">${textBadge}</span>
                                </div>
                            `;
                }
            },
            { "data": "comment" },
            {
                data: "totalAmount", render: function (data) {
                    const formatoCRC = new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC' });
                    return `${formatoCRC.format(data)}`;
                }
            },
            { "data": "date" },
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

    $('#export_excel').on('click', function (e) {
        e.preventDefault();
        dataTable.button(0).trigger();
    });

    $('#export_pdf').on('click', function (e) {
        e.preventDefault();
        dataTable.button(1).trigger();
    });

    $('#export_csv').on('click', function (e) {
        e.preventDefault();
        dataTable.button(2).trigger();
    });
}

$(function () {
    const subheader = createSubheader({
        title: 'Transacciones',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            'Transacciones'
        ],
        buttonText: 'Agregar Transacción',
        buttonAction: '/Transactions/Create',
        buttonTooltip: 'Crea una nueva transacción'
    });
    document.querySelector('#subheader-container').appendChild(subheader);

    updateDataTable();
});

$('#filterButton').on('click', function () {
    const startDateInput = $('#startDate').val();
    const endDateInput = $('#endDate').val();
    updateDataTable(startDateInput, endDateInput);
});

$('#resetButton').on('click', function () {
    $('#startDate').val('');
    $('#endDate').val('');
    updateDataTable();
});

function Delete(id) {
    Swal.fire({
        title: 'Eliminar transacción',
        text: '¿Estás seguro de que deseas eliminar esta transacción?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#5369D3',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'No, cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Transactions/Delete?id=' + id,
                type: 'Delete',
                success: function (data) {
                    if (data) {
                        Swal.fire({
                            title: '¡Eliminado!',
                            text: 'La transacción ha sido eliminada correctamente.',
                            icon: 'success',
                            confirmButtonText: 'Aceptar',
                        }).then(() => {
                            updateDataTable();
                        });
                    } else {
                        Swal.fire(
                            '!Algo salió mal!',
                            'La transacción no ha sido eliminado.',
                            'error'
                        );
                    }
                }
            });
        }
    })
}