// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
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