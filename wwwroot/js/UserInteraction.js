function ClearPDFViewer(iFrame_id) {
    document.getElementById(iFrame_id).innerHTML = "";
}

function FocusToElementWithID(id) {
    var focused_element = document.getElementById(id);
    focused_element.focus();
}