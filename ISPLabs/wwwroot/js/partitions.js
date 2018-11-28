const uri = "/api/partition";
let partitons = null;
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
            const tBody = $("#partitions");
            const delicon = "/images/delete.png";
            const editicon = "/images/edit.png";
            const expandicon = "/images/expand.png";
            const addicon = "/images/add.png";
            tBody.empty();
            $.each(data, function (key, item) {
                var table = $("<table class='table'></table>")
                    .addClass("col-lg-12");
                var ctBody = $("<tbody></tbody>");
                $.each(item.categories, function (ckey, citem) {
                    var ctr = $("<tr></tr>")
                        .append($("<th></th>").text(citem.id))
                        .append($("<td></td>").text(citem.name))
                        .append($("<td></td>").text(citem.topicsCount))
                        .append($("<td></td>")
                            .append($("<button></button>")
                                .addClass("btn btn-light clear")
                                .attr("data-toggle", "modal")
                                .attr("data-target", "#edit_cat")
                                .append($("<img></img>")
                                    .attr("src", editicon)
                                    .attr("height", "20")
                                    .attr("width", "20"))
                                .attr("data-id", citem.id)
                                .attr("data-cat", citem.name)
                                .attr("data-pid", item.id)
                                .attr("data-description", citem.description))
                            .append($("<button></button>")
                                .addClass("btn btn-light clear")
                                .attr("data-toggle", "modal")
                                .attr("data-target", "#request_cat_delete")
                                .append($("<img></img>")
                                    .attr("src", delicon)
                                    .attr("height", "20")
                                    .attr("width", "20"))
                                .attr("data-id", citem.id)
                                .attr("data-cat", citem.name)));
                    ctr.appendTo(ctBody);
                });
                table.append(ctBody);
                const tr = $("<div class='row border-bottom p-10' style='width: auto; margin: 0px 10px;'></div>")
                    .append($("<div class= 'col-lg-3'></div>").text(item.id))
                    .append($("<div class= 'col-lg-3'></div>")
                        .append($("<a></a>")
                            .attr("data-toggle", "collapse")
                            .attr("href", "#cats" + item.id)
                            .attr("role", "button")
                            .attr("aria-expanded", false)
                            .attr("aria-controls", "cats" + item.id)
                            .attr("data-id", item.id)
                            .text(item.name)))
                    .append($("<div class= 'col-lg-3'></div>").text(item.categoriesCount))
                    .append($("<div class= 'col-lg-3'></div>")
                        .append($("<button></button>")
                            .addClass("btn btn-light clear")
                            .attr("data-toggle", "modal")
                            .attr("data-target", "#create_cat")
                            .append($("<img></img>")
                                .attr("src", addicon)
                                .attr("height", "20")
                                .attr("width", "20"))
                            .attr("data-pid", item.id))
                        .append($("<button></button>")
                            .addClass("btn btn-light clear")
                            .attr("data-toggle", "modal")
                            .attr("data-target", "#edit_partition")
                            .append($("<img></img>")
                                .attr("src", editicon)
                                .attr("height", "20")
                                .attr("width", "20"))
                            .attr("data-id", item.id)
                            .attr("data-partition", item.name))
                        .append($("<button></button>")
                            .addClass("btn btn-light clear")
                            .attr("data-toggle", "modal")
                            .attr("data-target", "#request_delete")
                            .append($("<img></img>")
                                .attr("src", delicon)
                                .attr("height", "20")
                                .attr("width", "20"))
                            .attr("data-id", item.id)
                            .attr("data-partition", item.name)));
                var extr = $("<div style='margin: 0px 30px;'></div>")
                    .attr("id", "cats" + item.id)
                    .addClass("row collapse bg-light")
                    .append(table);
                tr.appendTo(tBody);
                extr.appendTo(tBody);
            });
            partitons = data;
            $("#load_bar").addClass("collapse");
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