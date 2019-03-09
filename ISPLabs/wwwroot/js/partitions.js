const uri = "/api/partition";
const catUri = "/api/category";


$(document).ready(function () {
    getData();
});

function getData() {
    $("#load_bar").removeClass("collapse");
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            $("#partitions").empty();
            $("#partitionTmpl").tmpl(data).appendTo($("#partitions"));            
        },
    });
}

$("#request_delete").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const pname = button.data("partition");
    const id = button.data("id");
    $("#deleting_partition").text(pname);
    const modal = $(this);
    modal.find($("#deletePartitionBtn")).attr("onclick", "deletePartition(" + id + ")");
});

function deletePartition(id) {
    $.ajax({
        type: "DELETE",
        url: uri + "/" + id,
        success: function (result) {
            getData();
            $("#request_delete").modal('hide');
        }
    });
}

function createPartition() {
    var part = {
        name: $("#Name").val(),
    }
    if ($("#create_form").validate().form())
        $.ajax({
            type: "POST",
            url: uri,
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(part),
            success: function (result) {
                $("#create_partition").modal('hide');
                getData();
            },
            error: function (xhr, status, error) {
                alert("Duplicate name");
            },
        });
}


function editPartition(id) {
    var part = {
        name: $("#edit_form").find($("[name = 'Name']")).val(),
    };
    if ($("#edit_form").validate().form())
        $.ajax({
            type: "PUT",
            url: uri + "/" + id,
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(part),
            success: function (result) {
                $("#edit_partition").modal('hide');
                getData();
            },
            error: function (xhr, status, error) {
                alert("Duplicate name");
            },
        });
}

$("#edit_partition").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const pname = button.data("partition");
    const id = button.data("id");
    const modal = $(this);
    modal.find($("[name = 'Name']")).val(pname);
    modal.find($("#editPartitionBtn")).attr("onclick", "editPartition(" + id + ")");
});

$("#create_cat").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const pid = button.data("pid");
    const modal = $(this);
    modal.find($("[name = 'Id']")).val(pid);
});


function createCat() {
    var cf = $("#create_cat_form");
    var cat = {
        partitionId: cf.find($("[name = 'Id']")).val(),
        name: cf.find($("[name = 'Name']")).val(),
        description: cf.find($("[name = 'Description']")).val(),
    };
    if (cf.validate().form())
        $.ajax({
            type: "POST",
            url: catUri,
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(cat),
            success: function (result) {
                $("#create_cat").modal('hide');
                getData();
            },
            error: function (xhr, status, error) {
                alert("Duplicate name");
            },
        });
}


$("#request_cat_delete").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const cname = button.data("cat");
    const id = button.data("id");
    $("#deleting_cat").text(cname);
    const modal = $(this);
    modal.find($("#deleteCatBtn")).attr("onclick", "deleteCat(" + id + ")");
});

function deleteCat(id) {
    $.ajax({
        type: "DELETE",
        url: catUri + "/" + id,
        success: function (result) {
            getData();
            $("#request_cat_delete").modal('hide');
        }
    });
}


function editCat(id, pid) {
    var form = $("#edit_cat_form");
    var cat = {
        partitionId: pid,
        name: form.find($("[name = 'Name']")).val(),
        description: form.find($("[name = 'Description']")).val(),
    };
    if (form.validate().form())
        $.ajax({
            type: "PUT",
            url: catUri + "/" + id,
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(cat),
            success: function (result) {
                $("#edit_cat").modal('hide');
                getData();
            },
            error: function (xhr, status, error) {
                alert("Duplicate name");
            },
        });
}

$("#edit_cat").on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    const cname = button.data("cat");
    const id = button.data("id");
    const pid = button.data("pid");
    const description = button.data("description");
    const modal = $(this);
    modal.find($("[name = 'Name']")).val(cname);
    modal.find($("[name = 'Description']")).val(description);
    modal.find($("#editCatBtn")).attr("onclick", "editCat(" + id + "," + pid + ")");
});


function expandPartition(id) {
    let td = $("#td" + id);
    td.toggleClass("p-10");
    td.find($(".category-row")).toggleClass("h-0");
}