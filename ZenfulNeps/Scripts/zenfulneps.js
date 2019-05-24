
$(document).ready(function () {
    var rssCount = $('#rssFeedCount').val();
    for (i = 0; i < rssCount; i++) {
        GetRssFeed(i);
    }
});

function GetRssFeed(iteration) {
    var h3 = '#rssFeedH3' + iteration;
    var titleHyper = '#rssFeedTitle' + iteration;
    var divDesc = '#rssFeed' + iteration;
    var fieldSet = '#fieldSet' + iteration;
    $.ajax({
        type: "GET",
        url: "../ZenfulNeps/GetRssFeed",
        async: true,
        data: { rssIteration: iteration },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $(h3).html(result.Heading);
            $(divDesc).html(result.Description);
            $(titleHyper).text(result.Title);
            $(titleHyper).attr("href", result.Link);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            $(h3).hide();
            $(titleHyper).hide();
            $(divDesc).hide();
            $(fieldSet).hide();
            //alert(xhr.responseText);
        }
    });
}