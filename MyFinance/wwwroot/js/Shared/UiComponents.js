function createSubheader({ title, breadcrumbItems, buttonText, buttonAction, buttonTooltip }) {
    if (!Array.isArray(breadcrumbItems) || breadcrumbItems.length < 2 || breadcrumbItems.length > 3) {
        throw new Error('breadcrumbItems debe tener entre 2 y 3 elementos.');
    }

    const container = document.createElement('div');
    container.className = 'subheader py-6 py-lg-8 subheader-transparent container-header gutter-b';

    const breadcrumbHTML = breadcrumbItems.map((item, index) => {
        const isLast = index === breadcrumbItems.length - 1;

        if (isLast && typeof item === 'string') {
            return `<li class="breadcrumb-item active" aria-current="page">${item}</li>`;
        }

        if (typeof item === 'object' && item.label && item.url) {
            return `<li class="breadcrumb-item"><a href="${item.url}" class="text-decoration-none">${item.label}</a></li>`;
        }

        throw new Error(`breadcrumbItems[${index}] debe ser un objeto con label y url, o un string si es el último.`);
    }).join('');

    const buttonHTML = (buttonText && buttonAction && buttonTooltip)
        ? `
      <div class="d-flex justify-content-end mb-2">
        <a class="btn btn-primary" data-bs-toggle="tooltip" data-bs-placement="top" title="${buttonTooltip}" href="${buttonAction}">
          <i class="bi bi-plus-circle me-2"></i> ${buttonText}
        </a>
      </div>
    `
        : '';

    container.innerHTML = `
    <div class="container d-flex align-items-center justify-content-between flex-wrap flex-sm-nowrap">
      <div class="d-flex align-items-center flex-wrap me-1">
        <div class="d-flex align-items-baseline flex-wrap me-5 font-monospace">
          <h2 class="my-1 me-5 text-primary">${title}</h2>
          <nav style="--bs-breadcrumb-divider: '>';" aria-label="breadcrumb">
            <ol class="breadcrumb">
              ${breadcrumbHTML}
            </ol>
          </nav>
        </div>
      </div>
    </div>
    ${buttonHTML}
  `;

    return container;
}

function showError(message, icon = 'error') {
    Swal.fire({
        icon,
        text: message,
        showConfirmButton: true,
        confirmButtonText: 'Cerrar'
    });
}
