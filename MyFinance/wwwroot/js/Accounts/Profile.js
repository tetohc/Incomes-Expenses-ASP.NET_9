$(function () {
    const subheader = createSubheader({
        title: 'Mi Perfil',
        breadcrumbItems: [
            { label: 'Inicio', url: '/Home/Index' },
            'Mi Perfil'
        ]
    });
    document.querySelector('#subheader-container').appendChild(subheader);
});