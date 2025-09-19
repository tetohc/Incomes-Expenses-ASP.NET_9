$(function () {
    const subheader = createSubheader({
        title: 'Inicio',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            'Inicio'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
});

