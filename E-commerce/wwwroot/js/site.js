﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    var dropdowns = document.querySelectorAll('.dropdown');

    dropdowns.forEach(function (dropdown) {
        dropdown.addEventListener('mouseenter', function () {
            var menu = dropdown.querySelector('.dropdown-menu');
            dropdown.classList.add('show');
            menu.classList.add('show');
        });

        dropdown.addEventListener('mouseleave', function () {
            var menu = dropdown.querySelector('.dropdown-menu');
            dropdown.classList.remove('show');
            menu.classList.remove('show');
        });
    });
});


// Gestire il click sull'icona di ricerca
document.getElementById('searchIcon').addEventListener('click', function () {
    var searchInput = document.getElementById('searchInput');
    if (searchInput.style.display === 'none') {
        searchInput.style.display = 'block';
        searchInput.focus(); // Porta il focus sul campo di input
    } else {
        searchInput.style.display = 'none';
    }
});

// Gestire il submit della ricerca
document.getElementById('searchInput').addEventListener('keydown', function (event) {
    if (event.key === 'Enter') {
        var query = event.target.value;
        window.location.href = '/Product/Search?query=' + encodeURIComponent(query);
    }
});

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
