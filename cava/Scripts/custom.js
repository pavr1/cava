$(document).ready(function () {
    $("#menu-bar-id").click(function () {
        LoadBar();
    });

    $("#menu-experience-id").click(function () {
        LoadExperience();
    });

    $("#menu-kitchen-id").click(function () {
        LoadKitchen();
    });
});

function LoadBar() {
    $.get("/Home/Bar", {}, function (data) {
        $('#main-container').html(data);

        $('#btn-toggle').prop('checked', true).change()
    });
}
function LoadKitchen() {
    $.get("/Home/Kitchen", {}, function (data) {
        $('#main-container').html(data);
    });
}
function LoadExperience() {
    $.get("Home/Experience", {}, function (data) {
        $('#main-container').html(data);
    });
}