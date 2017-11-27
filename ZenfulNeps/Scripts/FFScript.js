var getKeepers = [];
var myPlayers = [];
$(document).ready(function () {

    var searchTerm = '';
    var searchPosition = -1;
    var keepers = ["Matt Ryan", " Dak Prescott", "Devonta Freeman", "Derek Carr", "Michael Crabtree", "Jay Ajayi", "Isaiah Crowell", "Michael Thomas", "Tevin Coleman"];

    GetKeepers();
    if (getKeepers.length > 0) {
        keepers = getKeepers;
    }
    ApplyKeepers(keepers);

    GetMyPlayers();
    if (myPlayers.length > 0) {
        //$(".newPlayerRow").remove();
        $.each(myPlayers, function(key, value) {
            $(".myPlayers").append(value);
        });
    }

    $(document).tooltip();

    //remove hyperlinks and keep text
    $('.allPlayers a').contents().unwrap();

    $('.taken').on('click', function (e) {
        var isChecked = $(this).attr('checked') == 'checked';
        var playerType = $(this).closest('table').find('.tablehdr').find("td").text();
        var tds = $(this).closest('tr').find("td");
        var playerName = '';
        var playerTeam = '';
        var playerBye = '';
        if (playerType == 'TEAM DEFENSE / ST') {
            playerName = $.trim(tds.eq(2).text());
            playerTeam = '';
            playerBye = tds.eq(3).text();

        } else {
            playerName = $.trim(tds.eq(2).text());
            playerTeam = tds.eq(3).text();
            playerBye = tds.eq(4).text();
        }
        if (e.shiftKey && isChecked) {
            var bgColor = '#ffffff';
            switch (playerType) {
                case "QUARTERBACKS":
                    bgColor = "#F2A29D";
                    break;
                case "RUNNING BACKS":
                    bgColor = "#B8CAEA";
                    break;
                case "TIGHT ENDS":
                    bgColor = "#F2E09D";
                    break;
                case "WIDE RECEIVERS":
                    bgColor = "#9DF2A7";
                    break;
                case "KICKERS":
                    bgColor = "#E99DF2";
                    break;
                case "TEAM DEFENSE / ST":
                    bgColor = "#CDBE70";
                    break;
                default:
                    bgColor = '#ffffff';
                    break;
            }
            var newRow = "<tr class='newPlayerRow note-td'><td bgcolor='" + bgColor + "' align='center' class='sort2'><a href='#' class='removePlayer'>X</a></td>" +
                "<td bgcolor='" + bgColor + "' class='tdPlayerName sort2'>" +
                playerName + "</td><td bgcolor='" + bgColor + "' class='sort2'>" + playerBye + "</td><td bgcolor='" + bgColor + "' class='sort2'>" +
                playerTeam + "</td><td bgcolor='" + bgColor + "' class='sort2'>" + playerType + "</td></tr>";
            
            $(".myPlayers").append(newRow);
            myPlayers = [];
            $(".newPlayerRow").each(function () {
                myPlayers.push(this.outerHTML);
            });
            SaveMyPlayers();
        }
        
        $(this).closest('tr').find('td').each(function () {
            $(this).removeAttr("bgcolor");
        });;

        $(this).closest('tr')[this.checked ? 'addClass' : 'removeClass']('done');
        var checked = this.checked;
        if (checked) {
            keepers.push(playerName);
        } else {
            keepers = $.grep(keepers, function (value) {
                return value != playerName;
            });
        }
        SaveKeepers(keepers);
    });

    $(document).on('click', '.removePlayer', function (e) {
        var player = $.trim($(this).closest('tr').find(".tdPlayerName").text());
        $(this).closest('tr').remove();

        myPlayers = [];
        $(".newPlayerRow").each(function () {
            myPlayers.push(this.outerHTML);
        });
        SaveMyPlayers();

        var tableRow = $("td").filter(function () {
            return $.trim($(this).text()) == player;
        }).closest("tr");
        var takenSelector = tableRow.find(".taken");
        if ($(takenSelector).attr('checked') == 'checked') {
            $(takenSelector).click();
            $(takenSelector).closest('tr')[this.checked ? 'addClass' : 'removeClass']('done');
        }
        keepers = $.grep(keepers, function (value) {
            return value != player;
        });
        SaveKeepers(keepers);
        return false;
    });

    $('#search-button').on("click", function () {
        var matches = searchAndHighlight($('#search-term').val(), ".allPlayers", 'highlighted', true);
        if (matches > 0) {
            if (searchTerm != $('#search-term').val()) {
                searchPosition = 0;
                searchTerm = $('#search-term').val();
            } else {
                ++searchPosition;
                if (searchPosition > matches - 1) {
                    searchPosition = 0;
                }
            }
            $(window).scrollTop($('.highlighted:eq(' + searchPosition + ')').position().top);
            var onMatch = searchPosition + 1;
            $(".searchResults").text(onMatch + " of " + matches + " Results Found.");
        } else {
            $(".searchResults").text("No Results Found.");
        }
        return false;
    });
});

function SaveKeepers(keepers) {
    var data = {};
    data.Keepers = keepers;
    $.ajax({
        type: "POST",
        url: "SaveKeepers",
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //return false;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function GetKeepers() {
    var data = {};
    $.ajax({
        type: "GET",
        url: "GetKeepers",
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.Keepers.length > 0) {
                getKeepers = result.Keepers;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function SaveMyPlayers() {
    var data = {};
    data.MyPlayers = myPlayers;
    $.ajax({
        type: "POST",
        url: "SaveMyPlayers",
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //return false;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function GetMyPlayers() {
    var data = {};
    $.ajax({
        type: "GET",
        url: "GetMyPlayers",
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.MyPlayers.length > 0) {
                myPlayers = result.MyPlayers;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}

function ApplyKeepers(keepers) {
    $.each(keepers, function(index, value) {
        var tableRow = $("td").filter(function () {
            return $.trim($(this).text()) == value;
        }).closest("tr");
        var takenSelector = tableRow.find(".taken");
        $(takenSelector).click();
        $(tableRow).find('td').each(function () {
            $(this).removeAttr("bgcolor");
        });;
        $(takenSelector).closest('tr')['addClass']('done');
    });
}

function searchAndHighlight(searchTerm, selector, highlightClass, removePreviousHighlights) {
    if(searchTerm) {
        var selector = selector || "body",                             //use body as selector if none provided
            searchTermRegEx = new RegExp("("+searchTerm+")","gi"),
            matches = 0,
            helper = {};
        helper.doHighlight = function(node, searchTerm){
            if(node.nodeType === 3) {
                if(node.nodeValue.match(searchTermRegEx)){
                    matches++;
                    var tempNode = document.createElement('span');
                    tempNode.innerHTML = node.nodeValue.replace(searchTermRegEx, "<span class='" + highlightClass + "'>$1</span>");
                    node.parentNode.insertBefore(tempNode, node );
                    node.parentNode.removeChild(node);
                }
            }
            else if(node.nodeType === 1 && node.childNodes && !/(style|script)/i.test(node.tagName)) {
                $.each(node.childNodes, function(i,v){
                    helper.doHighlight(node.childNodes[i], searchTerm);
                });
            }
        };
        if(removePreviousHighlights) {
            $('.'+ highlightClass).each(function(i,v){
                var $parent = $(this).parent();
                $(this).contents().unwrap();
                $parent.get(0).normalize();
            });
        }
 
        $.each($(selector).children(), function(index,val){
            helper.doHighlight(this, searchTerm);
        });
        return matches;
    }
    if (removePreviousHighlights) {
        $('.' + highlightClass).each(function (i, v) {
            var $parent = $(this).parent();
            $(this).contents().unwrap();
            $parent.get(0).normalize();
        });
    }
    return false;
}

