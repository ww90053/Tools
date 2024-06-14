Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginReq);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
var timeOut

function beginReq(sender, args) {

    timeOut =  setTimeout('openModal()', 30);

}
function openModal(sender, args) {

    // shows the Popup 
    $find(ModalProgress).show();
}

function endReq(sender, args) {

    clearTimeout(timeOut);
    //  shows the Popup 
    $find(ModalProgress).hide();
} 