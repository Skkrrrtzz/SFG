$(document).on("ajaxComplete", (event, jqxhr, settings, exception) => {
    if (jqxhr.status == 401) {
        window.location.href = "https://localhost:7103/Login?isLoggedOut=true";
    }
});

$(document).on("ajaxError", (event, jqxhr, settings, exception) => {
    // $('.toast').val(exception);
    $('.toast').show();
});