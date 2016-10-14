//test
$(document).ready(function () {

$('a.goTop').click(function(){
    $('html, body').animate({
        scrollTop: $( $.attr(this, 'href') ).offset().top
    }, 1000);
    return false;
});

$('a.goTop').click(function(){
    $('html, body').animate({
        scrollTop: $( $.attr(this, 'href') ).offset().top
    }, 1000);
    return false;
});

$("a[data-toggle='collapse']").click(function () {
    $(".collapse").slideUp();
    $(this).parent().next(".collapse").slideDown();
    return false;
});

});