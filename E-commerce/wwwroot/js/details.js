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

document.getElementById('openMenuBtn').addEventListener('click', function () {
    // Mostra il menù laterale
    document.getElementById('offCanvasMenu').classList.add('open');
    // Attiva l'overlay sfumato
    document.getElementById('bodyOverlay').classList.add('active');
});

document.getElementById('closeMenuBtn').addEventListener('click', function () {
    // Chiudi il menù laterale
    document.getElementById('offCanvasMenu').classList.remove('open');
    // Rimuovi l'overlay sfumato
    document.getElementById('bodyOverlay').classList.remove('active');
});

document.getElementById('bodyOverlay').addEventListener('click', function () {
    // Chiudi il menù laterale e l'overlay se clicchi fuori dal menù
    document.getElementById('offCanvasMenu').classList.remove('open');
    document.getElementById('bodyOverlay').classList.remove('active');
});
