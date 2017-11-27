// <copyright file="CompanionPlants.cs" >
// Copyright William H Hamilton (c) 2007, 2008 All Right Reserved

$(document).ready(function () {
    var plantList = GetPlantList();
    $('#searchValue').autocomplete({
        source: plantList
    });
});

function GetPlantList() {
    var plants;
    $.ajax({
        type: "GET",
        async: false,
        url: '../CompanionPlants/GetPlantList',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            plants = msg.PlantList;
        },
        error: function (msg) { alert('error: ' + msg.responseText); }
    });
    return plants;
}