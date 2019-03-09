const uri = "/api/user";
let users = null;

function getCount(data) {
    const el = $("#counter");
    el.text("(" + data.length + ")");
}

$(document).ready(function () {
    $("#load_bar").removeClass("collapse");
    getData();
    $("#load_bar").addClass("collapse");
    $("#users_table").removeClass("collapse");
});

function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            $.each(data, function (key, item) {
                let d = new Date(Date.parse(item.registrationDate));
                item.registrationDate = d.toLocaleString();
            });
            $("#users").empty();
            getCount(data);
            $("#userTmpl").tmpl(data).appendTo($("#users"));
        }
    });
}

$('#edit_user').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var id = button.data('user'); // Extract info from data-* attributes
    var modal = $(this);
    $.ajax({
        type: "GET",
        url: uri + "/" + id,
        cache: false,
        success: function (data) {
            modal.find($("[name = 'Id']")).val(data.id);
            modal.find($("[name = 'Login']")).val(data.login);
            modal.find($("[name = 'Email']")).val(data.email);
            modal.find($("[name = 'Password']")).val(data.password);
            modal.find($("[name = 'Role']")).val(data.role);
            modal.find($("[type = 'submit']")).attr('data-id', data.id);
        }
    });
})

$('#request_delete').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var id = button.data('id'); // Extract info from data-* attributes
    var login = button.data('user');
    var modal = $(this);
    modal.find($("#deleting_user")).text(login);
    modal.find($("#deleteUserBtn")).attr("onclick", "deleteUser(" + id + ")");
})

function deleteUser(id) {
    $.ajax({
        type: "DELETE",
        url: uri + "/" + id,
        success: function (result) {
            getData();
            $("#request_delete").modal("hide");
        }
    });
}

function editUser() {
    const user = {
        login: $("#Login").val(),
        email: $("#Email").val(),
        role: $("#Role").val(),
        password: $("#Password").val(),
    }
    if ($("#edit_form").validate().form())
        $.ajax({
            url: uri + "/" + $("#Id").val(),
            type: "PUT",
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(user),
            success: function (result) {
                getData();
                $("#edit_user").modal("hide");
            },
            error: function (xhr, status, error) {
                alert("Dublicate email/login!");
            },
        });
}