



$("#dialog").dialog({
    autoOpen: false,
    show: {
        effect: "fade",
        duration: 500
    },
    hide: {
        effect: "fade",
        duration: 500
    }
});


// #region Function
function showMessage(paramode) {
    var toaster = document.getElementById("toastMessage");
    var toasterHeader = document.getElementById("toastHeader");
    var toasterBody = document.getElementById("toastBody");

    if (paramode == 'success') {
        toasterHeader.classList.remove("bg-danger", "bg-success");
        toasterBody.classList.remove("bg-danger-subtle", "bg-danger-subtle");

        toasterHeader.classList.add("bg-success");
        toasterBody.classList.add("bg-success-subtle");

        toasterHeader.innerHTML = 'SUCCESS';
        toasterBody.innerHTML = '<i class="bi bi-check-circle-fill"></i> Password accepted.';

    }

    if (paramode == 'error') {
        toasterHeader.classList.remove("bg-danger", "bg-success");
        toasterBody.classList.remove("bg-danger-subtle", "bg-danger-subtle");

        toasterHeader.classList.add("bg-danger");
        toasterBody.classList.add("bg-danger-subtle");

        toasterHeader.innerHTML = 'ERROR';
        toasterBody.innerHTML = '<i class="bi bi-x-circle-fill"></i> Password denied. Please try again';
    }

    var visibleToast = new bootstrap.Toast(toaster, { 'autohide': true, 'delay': 3000 });
    visibleToast.show();

    // const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)

}


// #endregion


$(function () {
    $("#btnLogin").on("click", function (e) {
        var password = $("#txtPassword").val();
        // $("#dialog").dialog("open");

        $.ajax({
            url: "?handler=UserLogin",
            method: "GET",
            data: { parapass: password },
            success: function (data) {
                if (data == "loginsucess") {
                    // $("#stkRole").show();
                    //const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample)

                    // $.fn.myFunction();
                    $("#dialog").dialog("open");

                    showMessage('success');
                }
                else {
                    // $("#stkRole").hide();
                    $("#txtPassword").select();

                    showMessage('error');
                    //toastBootstrap.show()




                }
            }
        })
        e.preventDefault();
    })
});