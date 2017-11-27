
$(document).ready(function () {

    var grainBill = [];

    GetGrainList();

    $('.addGrain').click(function () {
        var grainId = $("[id$='grainList'] option:selected").val();
        var grainName = $("[id$='grainList'] option:selected").text();
        var grainAmount = $("[id$='grainAmount']").val();
        $("[id$='grainAmount']").css({ "background-color": "white" });
        if (grainAmount > 0 && grainId > 0) {
            var returnppg = GetPPG(grainId);
            var returnLovi = GetLovi(grainId);
            grainBill.push({ id: grainId, amount: grainAmount, ppg: returnppg, lovi: returnLovi });
            var ingRow = '<tr>'
                            + '<td>'
                                + '<a href="#" class="deleteGrain" data-Id="' + grainId + '"><img src="../../Images/delete.png" Title="Remove Grain" class="deleteImage" /></a>'
                            + '</td>'
                            + '<td>'
                                + '<span>'
                                    + parseFloat(grainAmount).toFixed(2) + ' lbs.'
                                + '</span>'
                            + '</td>'
                            + '<td>'
                                + '<label>'
                                    + grainName
                                + '</label>'
                            + '</td>'
                        + '</tr>';

            $('#ing-list tbody').append(ingRow);
            $("[id$='grainList']").val(0);
            $("[id$='grainAmount']").val('');
            SetGrainBillTotal();
        } else {
            alert('Please enter an amount and choose a Fermentable.');
            if (grainAmount < 1) {
                $("[id$='grainAmount']").focus();
                $("[id$='grainAmount']").css({ "background-color": "yellow" });
            } else {
                $("[id$='grainList']").focus();
            }
        }
        return false;
    });

    $('.deleteGrain').live('click', function () {
        var id = $(this).attr('data-Id');
        var foundIndex = -1;
        for (var i = 0; i < grainBill.length; i++) {
            if (grainBill[i].id == id) {
                foundIndex = i;
            }
        }
        grainBill.splice(foundIndex, 1);
        $(this).closest('tr').remove();
        SetGrainBillTotal();
        return false;
    });

    $('#calculate').live('click', function () {
        $('.eff-results').stop().css('background-color', $('.color').css('backgroundColor')).animate({ backgroundColor: '#FFFFFF' }, 1500);
        var totalPE = 0;
        for (var i = 0; i < grainBill.length; i++) {
            totalPE = (parseFloat(totalPE) + (parseFloat(grainBill[i].amount) * parseFloat(grainBill[i].ppg))).toFixed(2);
        }
        var startGravity = $('#startGravity').val();
        var amountWort = $('#amountWort').val();
        var gravityPoints = ((parseFloat(startGravity) - 1) * 1000).toFixed(2);
        var extratedPoints = parseFloat(gravityPoints) * parseFloat(amountWort);
        var mashEfficiency = ((parseFloat(extratedPoints) / parseFloat(totalPE)) * 100).toFixed(2) + '%';

        var sfPercentGravity = ((((totalPE * .75) / amountWort) / 1000) + 1).toFixed(3);

        if (mashEfficiency != 'NaN%' & mashEfficiency != 'Infinity%') {
            $('.your-efficiency').html(mashEfficiency);
            $('.sf-efficiency').html(sfPercentGravity);
        }
        return false;
    });

    function SetGrainBillTotal() {
        var billTotal = 0;
        for (var i = 0; i < grainBill.length; i++) {
            billTotal = (parseFloat(billTotal) + parseFloat(grainBill[i].amount)).toFixed(2);
        }
        var srm = '';
        var color = '';
        if ($('#amountWort').val() != '') {
            srm = GetGrainBillSRM($('#amountWort').val())
            color = GetColor(parseFloat(srm).toFixed(1));
            $('.color').css('background-color', 'rgb(' + color + ')');
            $('.color-srm').html(srm + '&deg; SRM');
        }
        $('.grain-bill').html(billTotal + ' lbs.');

    }

    function GetGrainBillSRM(amountWort) {
        var mcu = 0;
        for (var i = 0; i < grainBill.length; i++) {
            mcu = mcu + grainBill[i].lovi * grainBill[i].amount;
        }
        var srm = (1.4922 * (Math.pow((mcu / amountWort), .6859))).toFixed(2);
        return srm;
    }

    function GetPPG(id) {
        var ppg = '';
        var data = new Object();
        data.id = id;
        $.ajax({
            type: "POST",
            async: false,
            url: '../Brewing/GetPPG',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ppg = msg.PPG;
            },
            error: function (msg) { alert('error: ' + msg.responseText); }
        });
        return ppg;
    }

    function GetLovi(id) {
        var lovi = '';
        var data = new Object();
        data.id = id;
        $.ajax({
            type: "POST",
            async: false,
            url: '../Brewing/GetLovi',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                lovi = msg.Lovi;
            },
            error: function (msg) { alert('error: ' + msg.responseText); }
        });
        return lovi;
    }

    function GetColor(srm) {
        var rgb = '';
        var data = new Object();
        data.srm = srm;
        $.ajax({
            type: "POST",
            async: false,
            url: '../Brewing/GetColor',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                rgb = msg.RGB;
            },
            error: function (msg) { alert('error: ' + msg.responseText); }
        });
        return rgb;
    }

    function GetGrainList() {
        $.ajax({
            type: "GET",
            url: '../Brewing/GetGrainList',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $('#grainAutoCom').autocomplete({
                    source: msg.GrainList,
                    select: function (event, ui) {
                        $('#grainList option').each(function () {
                            if ($(this).text() == ui.item.label) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        $('#grainAutoCom').val('');
                        if ($('#grainAmount').val() != '') {
                            $('.addGrain').trigger('click');
                        }
                        return false;
                    }
                });
            },
            error: function (msg) { alert('error: ' + msg.responseText); }
        });
    }

});