"use strict";

function qr_style(value)
{
    var url = $('#qrmenu_style').val();
    $.ajax({
        type: 'get',
        url: url,
        data: {style: value},
        beforeSend: function(response)
        {
            $('.loading').fadeIn();
        },
        dataType: 'json',
        success: function(response){ 
            $('.loading').fadeOut();
            $('.qr-code-main').load(' .qr-code-main');
        }
    });
}

function qr_color(ev) {
    const color = ev.target.value
    const r = parseInt(color.substr(1,2), 16)
    const g = parseInt(color.substr(3,2), 16)
    const b = parseInt(color.substr(5,2), 16)
    const f_color = r+','+g+','+b;
    var url = $('#qrmenu_style').val();
    $.ajax({
        type: 'get',
        url: url,
        data: {color: f_color},
        dataType: 'json',
        beforeSend: function(response)
        {
            $('.loading').fadeIn();
        },
        success: function(response){ 
            $('.loading').fadeOut();
            $('.qr-code-main').load(' .qr-code-main');
        }
    });
}

function qr_bgcolor(ev) {
    const color = ev.target.value
    const r = parseInt(color.substr(1,2), 16)
    const g = parseInt(color.substr(3,2), 16)
    const b = parseInt(color.substr(5,2), 16)
    const f_color = r+','+g+','+b;
    var url = $('#qrmenu_style').val();
    $.ajax({
        type: 'get',
        url: url,
        data: {bgcolor: f_color},
        dataType: 'json',
        beforeSend: function(response)
        {
            $('.loading').fadeIn();
        },
        success: function(response){ 
            $('.loading').fadeOut();
            $('.qr-code-main').load(' .qr-code-main');
        }
    });
}

function qr_size()
{
    var size = $('#qr_size').val();
    var url = $('#qrmenu_style').val();
    $.ajax({
        type: 'get',
        url: url,
        data: {size: size},
        dataType: 'json',
        beforeSend: function(response)
        {
            $('.loading').fadeIn();
        },
        success: function(response){ 
            $('.loading').fadeOut();
            $('.qr-code-main').load(' .qr-code-main');
        }
    });
}

function qr_margin()
{
    var margin = $('#qr_margin').val();
    var url = $('#qrmenu_style').val();
    $.ajax({
        type: 'get',
        url: url,
        data: {margin: margin},
        dataType: 'json',
        beforeSend: function(response)
        {
            $('.loading').fadeIn();
        },
        success: function(response){ 
            $('.loading').fadeOut();
            $('.qr-code-main').load(' .qr-code-main');
        }
    });
}


function downloadPng(){
    var img = new Image();
    img.onload = function (){
        var canvas = document.createElement("canvas");
        canvas.width = img.naturalWidth;
        canvas.height = img.naturalHeight;
        var ctxt = canvas.getContext("2d");
        ctxt.fillStyle = "#fff";
        ctxt.fillRect(0, 0, canvas.width, canvas.height);
            ctxt.drawImage(img, 0, 0);
        var a = document.createElement("a");
        a.href = canvas.toDataURL("image/png");
        a.download = "qrcode.png"
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    };
    var innerSvg = document.querySelector(".qr-code-main svg");
    var svgText = (new XMLSerializer()).serializeToString(innerSvg);
    img.src = "data:image/svg+xml;utf8," + encodeURIComponent(svgText);
}