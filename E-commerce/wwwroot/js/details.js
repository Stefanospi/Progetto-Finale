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


document.addEventListener('DOMContentLoaded', function () {
    var addToCartForm = document.getElementById('addToCartForm');
    if (addToCartForm) {
        addToCartForm.addEventListener('submit', function (event) {
            var selectedSize = document.querySelector('input[name="size"]:checked');
            var alertContainer = document.getElementById('alertContainer');

            // Rimuovi gli alert esistenti (se presenti)
            if (alertContainer) {
                alertContainer.remove();
            }

            // Se la taglia non è selezionata, mostra un alert
            if (!selectedSize) {
                event.preventDefault(); // Impedisce l'invio del form

                // Crea un div per l'alert
                var alertDiv = document.createElement('div');
                alertDiv.id = 'alertContainer';
                alertDiv.classList.add('alert', 'alert-danger', 'playpen-sans', 'mt-3', 'shadow-lg');
                alertDiv.role = 'alert';
                alertDiv.innerHTML = 'Per favore seleziona una taglia prima di aggiungere il prodotto al carrello.';

                // Inserisci l'alert sopra il form
                addToCartForm.parentNode.insertBefore(alertDiv, addToCartForm);
            }
        });
    }
});



document.addEventListener('DOMContentLoaded', function () {
    var deleteButton = document.getElementById('deleteButton');
    var confirmDelete = document.getElementById('confirmDelete');
    var cancelButton = document.getElementById('cancelButton');

    if (deleteButton) {
        deleteButton.addEventListener('click', function () {
            // Mostra il messaggio di conferma con transizione fluida
            console.log('Delete button clicked');
            confirmDelete.style.display = 'block'; // Rendi visibile il div
            setTimeout(function () {
                confirmDelete.classList.add('show');
            }, 10); // Aggiunge una leggera attesa per attivare la transizione
        });
    }

    if (cancelButton) {
        cancelButton.addEventListener('click', function () {
            // Nascondi il messaggio di conferma con transizione fluida
            console.log('Cancel button clicked');
            confirmDelete.classList.remove('show');
            setTimeout(function () {
                confirmDelete.style.display = 'none'; // Nascondi dopo la transizione
            }, 300); // Dura quanto la transizione CSS
        });
    }
});
