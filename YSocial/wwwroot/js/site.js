
window.checkElementExists = function (elementId) {
    console.log('checkElementExists called for element: ' + elementId);
    var exists = !!document.getElementById(elementId);
    console.log('checkElementExists result: ' + exists);
    return exists;
};

window.triggerFileInputClick = function (elementId) {
    console.log('triggerFileInputClick called for element: ' + elementId);
    var element = document.getElementById(elementId);
    if (element) {
        element.click();
        console.log('triggerFileInputClick: Click event triggered for ' + elementId);
    } else {
        console.error('triggerFileInputClick: Element not found: ' + elementId);
    }
};

window.preventDoubleSubmit = function (formId) {
    console.log('preventDoubleSubmit called for form: ' + formId);
    var form = document.getElementById(formId);
    if (form) {
        form.addEventListener('submit', function (e) {
            if (form.classList.contains('submitting')) {
                console.log('preventDoubleSubmit: Form already submitting, preventing');
                e.preventDefault();
                return;
            }
            form.classList.add('submitting');
            console.log('preventDoubleSubmit: Form submission allowed');
        });
    } else {
        console.error('preventDoubleSubmit: Form not found: ' + formId);
    }
};