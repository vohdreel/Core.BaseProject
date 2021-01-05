// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function BlockPage() {

    $.blockUI.defaults.css = {
        padding: 0,
        margin: 0,
        width: '30%',
        top: '40%',
        left: '35%',
        textAlign: 'center',
        cursor: 'wait'
    };

    $.blockUI({ message: '<h1 style="color:white; text-shadow: 2px 2px 2px black;" >Carregando...</h1>' });
}

$(document).ready(function () {
    $("#msg_box").fadeOut(5000);
});