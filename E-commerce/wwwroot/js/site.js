// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Chiudi il menu dopo aver cliccato su un link
//document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
//    link.addEventListener('click', function () {
//        var navbarCollapse = document.querySelector('.navbar-collapse');
//        var bsCollapse = new bootstrap.Collapse(navbarCollapse, {
//            toggle: false
//        });
//        bsCollapse.hide();
//    });
//});


// Mostra i prodotti nascosti quando si clicca sul pulsante "Mostra Altro"
document.getElementById('showMoreBtn').addEventListener('click', function () {
    var hiddenProducts = document.querySelectorAll('.hidden-product');
    hiddenProducts.forEach(function (product) {
        product.style.display = 'block'; // Mostra i prodotti nascosti
    });
    document.getElementById('hideProductsBtn').style.display = 'inline-block'; // Mostra il pulsante "Nascondi Prodotti"
    this.style.display = 'none'; // Nasconde il pulsante "Mostra Altro"
});

// Nasconde di nuovo i prodotti quando si clicca sul pulsante "Nascondi Prodotti"
document.getElementById('hideProductsBtn').addEventListener('click', function () {
    var hiddenProducts = document.querySelectorAll('.hidden-product');
    hiddenProducts.forEach(function (product) {
        product.style.display = 'none'; // Nasconde i prodotti
    });
    document.getElementById('showMoreBtn').style.display = 'inline-block'; // Mostra di nuovo il pulsante "Mostra Altro"
    this.style.display = 'none'; // Nasconde il pulsante "Nascondi Prodotti"
});
