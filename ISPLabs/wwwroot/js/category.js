﻿const uri = "/api/category"

$(document).ready(function () {
    getData();
});

function getData() {
    $.ajax({
        type: "GET",
        url: uri + "/" + cat_id,
        cache: false,
        success: function (data) {
            $.each(data.topics, function (key, item) {
                let d = new Date(Date.parse(item.lastActivity));
                item.lastActivity = d.toLocaleString();
            });
            $("#category_container").empty();
            $("#catTmpl").tmpl(data).appendTo("#category_container");
        }
    });
}

function createTopic() {
    var cf = $("#create_topic_form");
    var topic = {
        categoryId: cf.find($("[name = 'CategoryId']")).val(),
        name: cf.find($("[name = 'Name']")).val(),
        initialText: cf.find($("[name = 'InitialText']")).val(),
    };
    if (cf.validate().form()) {
        $.ajax({
            type: "POST",
            url: "/api/topic",
            accepts: "application/json",
            contentType: "application/json",
            data: JSON.stringify(topic),
            success: function (result) {
                hideModal("new_topic");
                getData();
            },
            error: function (xhr, status, error) {
                alert("Duplicate name");
            },
        });
    }
}