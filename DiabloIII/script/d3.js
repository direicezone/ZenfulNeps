//D3 Tooltip overover
var b = Bnet.D3.Tooltips;
b.registerDataOld = b.registerData;
try {
    b.registerData = function (data) {
        var c = document.body.children, s = c[c.length - 1].src;
        data.params.key = s.substr(0, s.indexOf('?')).substr(s.lastIndexOf('/') + 1);
        this.registerDataOld(data);
    };
} catch(e) {
}

$(function () {
    $("#dialog-hero").dialog({
        autoOpen: false,
        resizable: false,
        width: 450,
        height: 600,
        modal: true,
        buttons: {
            Close: function () {
                $(this).dialog("close");
            }
        }
    });
    
    $("#tabs").tabs();
    $(".opener").click(function () {
        var heroId = $(this).attr('data-heroid');
        $.blockUI({ message: '<h1><img src="images/busy.gif" /> Gathering Hero Data...</h1>' });
        var heroInfo = getHeroInfo(heroId);
        return false;
    });

    $.fn.digits = function() {
        return this.each(function() {
            $(this).text($(this).text().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
        });
    };
    
    function getHeroInfo(heroId) {
        var hero;
        var battleTag = $('[id$="txtBattleTag"]').val();
        var parms = { battleTag: battleTag, heroId: heroId };
        $.ajax({
            type: "POST",
            //async: false,
            url: "D3FollowerItems.aspx/GetHeroInfo",
            data: JSON.stringify(parms),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                hero = data.d;
                var heroInfo = hero;
                $.unblockUI();
                $('#class').html(getClass(heroInfo.className));
                $('#paragonLevel').html(heroInfo.paragonLevel);
                $('#arcaneResit').html(heroInfo.stats.arcaneResist);
                $('#coldResit').html(heroInfo.stats.coldResist);
                $('#fireResit').html(heroInfo.stats.fireResist);
                $('#poisonResit').html(heroInfo.stats.PoisonResist);
                $('#lightningResit').html(heroInfo.stats.lightningResist);
                $('#physicalResist').html(heroInfo.stats.physicalResist);
                $('#armor').html(heroInfo.stats.armor).digits();
                $('#attackSpeed').html(heroInfo.stats.attackSpeed.toFixed(2));
                $('#critChance').html(heroInfo.stats.critChance.toFixed(2) * 100 + '%');
                $('#critDamage').html(heroInfo.stats.critDamage.toFixed(2) * 100 + '%');
                $('#blockChance').html(heroInfo.stats.blockChance * 100 + '%');
                $('#strength').html(heroInfo.stats.strength);
                $('#vitality').html(heroInfo.stats.vitality);
                $('#dexterity').html(heroInfo.stats.dexterity);
                $('#intelligence').html(heroInfo.stats.intelligence);
                $('#life').html(heroInfo.stats.life).digits();
                $('#primaryResource').html(heroInfo.stats.primaryResource);
                $('#secondaryResource').html(heroInfo.stats.secondaryResource);
                $('#thorns').html(heroInfo.stats.thorns);
                $('#blockAmount').html(heroInfo.stats.blockAmountMin + '-' + heroInfo.stats.blockAmountMax);
                $('#lifePerKill').html(heroInfo.stats.lifePerKill);
                $('#lifeOnHit').html(heroInfo.stats.lifeOnHit);
                $('#toughness').html(heroInfo.stats.toughness).digits();
                $('#damage').html(heroInfo.stats.damage).digits();
                $('#goldFind').html(heroInfo.stats.goldFind.toFixed(2) * 100 + '%');
                $('#magicFind').html(heroInfo.stats.magicFind.toFixed(2) * 100 + '%');
                $('#cooldown').html(heroInfo.stats.cooldown.toFixed(4) * 100 + '%');

                $("#dialog-hero").dialog('option', 'title', heroInfo.name);
                $("#dialog-hero").dialog('open');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText);
            }
        });
        return hero;
    }

    function getClass(className) {
        className = className.toLowerCase().replace(/\b[a-z]/g, function (letter) {
            return letter.toUpperCase();
        });
        return className;
    }

});
