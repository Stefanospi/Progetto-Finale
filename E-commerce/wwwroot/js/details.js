// Script per gestire il toggle della descrizione
document.addEventListener("DOMContentLoaded", function () {
    var toggleDescription = document.getElementById('toggleDescription');
    var content = document.getElementById('descriptionContent');
    var icon = document.getElementById('toggleIcon');

    toggleDescription.addEventListener('click', function () {
        if (content.classList.contains('open')) {
            content.classList.remove('open');
            icon.textContent = '+';
        } else {
            content.classList.add('open');
            icon.textContent = '-';
        }
    });
});