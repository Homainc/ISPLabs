let topicUri = "/api/topic"
let msgUri = "/api/message"
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/messageshub")
    .build();

hubConnection.on("Receive" + topic_id, function (msg) {
    $("#msgTmpl").tmpl(msg).appendTo("#messages_container");
});

hubConnection.on("Deleted" + topic_id, function (id) {
    $("#msg" + id).remove();
});

hubConnection.on("Changed" + topic_id, function (id, text) {
    $("#msg_text"+id).text(text);
});

hubConnection.start();

$("#request_delete").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const id = button.data("id");
    $(this).find($("#deleteMessageBtn")).attr("onclick", "deleteMessage(" + id + ")");
});

$("#edit_message").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const id = button.data("id");
    const modal = $(this);
    const text = $("#msg_text"+id).text();
    modal.find($("[name = 'Text']")).val(text);
    modal.find($("#editMessageBtn")).attr("onclick", "editMessage(" + id + ")");
});

function sendMessage() {
    var form = $("#send_msg_form");
    if (form.validate().form()) {
        var text = form.find("#Text");
        hubConnection.invoke("Send", text.val(), topic_id);
        text.val("");
    }
}

function deleteMessage(id) {
    hubConnection.invoke("DeleteMessage", id);
    $("#request_delete").modal('hide');
}

function editMessage(id) {
    var win = $("#edit_message");
    const text = win.find("[name = 'Text']").val();
    hubConnection.invoke("EditMessage", id, text);
    win.modal('hide');
}

function closeTopic(tid, tname) {
    var topic = {
        id: tid,
        name: tname,
        isClosed: true,
    };
    $.ajax({
        type: "PUT",
        url: topicUri + "/" + tid,
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(topic),
        success: function (result) {
            getData();
        },
        error: function (xhr, status, error) {
            alert("Not access!");
        },
    });
}

$(document).ready(function () {
    getData();
});

function getData() {
    $("#topic_container").empty();
    $.ajax({
        type: "GET",
        url: topicUri + "/" + topic_id,
        cache: false,
        success: function (data) {
            $("#topicTmpl").tmpl(data).appendTo("#topic_container");
        }
    });
}