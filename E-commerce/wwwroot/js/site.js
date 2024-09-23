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
        setTimeout(function () {
            product.style.opacity = 1; // Applica l'animazione di fade-in
        }, 100); // Ritardo per applicare l'animazione
    });
    document.getElementById('hideProductsBtn').style.display = 'inline-block'; // Mostra il pulsante "Nascondi Prodotti"
    this.style.display = 'none'; // Nasconde il pulsante "Mostra Altro"
});

// Nascondi di nuovo i prodotti quando si clicca su "Nascondi Prodotti"
document.getElementById('hideProductsBtn').addEventListener('click', function () {
    var hiddenProducts = document.querySelectorAll('.hidden-product');
    hiddenProducts.forEach(function (product) {
        product.style.opacity = 0; // Applica l'animazione di fade-out
        setTimeout(function () {
            product.style.display = 'none'; // Nasconde i prodotti dopo l'animazione
        }, 500); // Il tempo di attesa deve corrispondere alla durata della transizione CSS
    });
    document.getElementById('showMoreBtn').style.display = 'inline-block'; // Mostra di nuovo il pulsante "Mostra Altro"
    this.style.display = 'none'; // Nasconde il pulsante "Nascondi Prodotti"
});


// Funzione per aggiungere la classe "visible" quando l'elemento entra nel viewport
function revealOnScroll() {
    var elements = document.querySelectorAll('.scroll-fade-in');
    var windowHeight = window.innerHeight;

    elements.forEach(function (el) {
        var elementTop = el.getBoundingClientRect().top;
        if (elementTop < windowHeight - 100) {
            el.classList.add('visible');
        }
    });
}

// Aggiungi l'evento scroll
window.addEventListener('scroll', revealOnScroll);

// Avvia la funzione inizialmente
revealOnScroll();



