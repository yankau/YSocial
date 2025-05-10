console.log("app.js loaded");

window.scrollToBottom = (element) => {
    if (element) {
        console.log("scrollToBottom called", element);
        element.scrollTop = element.scrollHeight;
    } else {
        console.log("scrollToBottom: element is null");
    }
};