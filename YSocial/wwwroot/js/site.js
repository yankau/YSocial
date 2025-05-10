window.preventDoubleSubmit = function(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener('submit', function() {
            const submitButton = form.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.disabled = true;
            }
        });
    }
};