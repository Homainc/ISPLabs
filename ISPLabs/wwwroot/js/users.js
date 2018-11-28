const uri = "/api/user";
let users = null;
function getCount(data) {
    const el = $("#counter");
    el.text("(" + data + ")");
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
            const tBody = $("#users");
            const delicon = "/images/delete.png"
            const editicon = "/images/edit.png"
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<th scope='row'></th>").text(item.id))
                    .append($("<td></td>").text(item.role))
                    .append($("<td></td>").text(item.login))
                    .append($("<td></td>").text(item.email))
                    .append($("<td></td>").text(item.password))
                    .append($("<td></td>").text(item.registrationDate))
                    .append($("<td></td>")
                        .append($("<button type='button' class='btn btn-light clear' data-toggle='modal' data-target='#edit_user' data-user=" + item.id + "></button>")
                            .append($("<img src=" + editicon + " height = '20' width = '20' />")))
                        .append($("<button type='button' class='btn btn-light clear' data-toggle='modal' data-target='#request_delete' data-user=" + item.login + "></button>")
                            .attr("data-id", item.id)
                            .append($("<img src=" + delicon + " height = '20' width = '20' />")))
                    );
                tr.appendTo(tBody);
            });
            users = data;
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