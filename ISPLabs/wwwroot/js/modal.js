$(document).ready(function () {
    initModal();
});
function initModal() {
    $(document).delegate('[data-toggle="modal"]', 'click', function () { toggleModal(this); });
    $(document).delegate('.overflow', 'click', function () { toggleModal(this); });
    $(document).delegate('.modal', 'show.modal', function (obj) { showModal(this.id) })
    $(document).delegate('.modal', 'hide.modal', function (obj) { hideModal(this.id) })
}
var modalOverflow = document.createElement("div");
modalOverflow.className = "overflow";
document.body.appendChild(modalOverflow);

function toggleModal(obj) {
    let id = obj.dataset.target;
    let modal = document.getElementById(id);
    if (modal.style.display == "block") {
        $(modal).trigger('hide.modal',obj);
    }
    else {
        $(modal).trigger('show.modal',obj);
    }
}

function hideModal(id) {
    let modal = document.getElementById(id);
    modalOverflow.style.display = "none";
    modal.style.display = "none";
    modal.style.opacity = '0';
    modalOverflow.style.opacity = '0';
}

function showModal(id) {
    modalOverflow.dataset.target = id;
    let modal = document.getElementById(id);
    modalOverflow.style.display = "block";
    modal.style.display = "block";
    modal.style.marginTop = '-' + modal.offsetHeight / 2 + "px";
    modal.style.marginLeft = '-' + modal.offsetWidth / 2 + "px";
    modal.style.opacity = '1';
    modalOverflow.style.opacity = '0.5';
}