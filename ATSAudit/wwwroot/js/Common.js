// $(document).ajaxError(function(event, jqxhr, settings, exception) {
//     if (jqxhr.status === 401) {
//         // Redirect to the login page
//         window.location.href = encodeURIComponent(window.location.pathname)
//     }
// });

$(document).on("ajaxComplete", (event, jqxhr, settings, exception) => {
    if (jqxhr.status == 401) {
        window.location.href = "https://localhost:7103/Login";
    }
});