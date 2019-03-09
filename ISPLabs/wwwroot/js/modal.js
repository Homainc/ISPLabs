function initModal() {
    $("button[data-toggle='modal']").click(function (e) { showModal(e.target.dataset.target); });
    $("div.overflow").click(function (e) { hideModal(e.target.dataset.target); });
}
var modalOverflow = document.createElement("div");
modalOverflow.className = "overflow";

function showModal(id) {
    modalOverflow.dataset.target = id;
    let modal = document.getElementById(id); 
    modal.classList = "modal";
    modal.style.marginTop = -modal.offsetHeight / 2 + "px";
    modal.style.marginLeft = -modal.offsetWidth / 2 + "px";
    modal.style.top = "50%";
    document.body.appendChild(modalOverflow);
}
function hideModal(id) {
    let modal = document.getElementById(id);
    modal.style.top = "-100%";
    modalOverflow.remove();
}