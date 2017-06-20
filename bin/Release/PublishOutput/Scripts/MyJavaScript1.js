$(function () {
    setInterval(function () {
        var now = new Date();
        var y = now.getFullYear();
        var m = now.getMonth() + 1;
        var d = now.getDate();
        var w = now.getDay();
        var h = now.getHours();
        var mi = now.getMinutes();
        var s = now.getSeconds();
        var ad = y + "-" + m + "-" + d + " " + h + ":" + mi + ":" + s;
        $("#date").val(ad);
    }, 1000);
});

