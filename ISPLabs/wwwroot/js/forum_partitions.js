const uri = "/api/partition"

$(document).ready(function () {
    getData();
});

function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            $("#partTmpl").tmpl(data).appendTo("#partitions_container");
        }
    });
}