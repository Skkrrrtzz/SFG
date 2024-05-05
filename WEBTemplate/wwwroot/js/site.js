



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
            url: "Login?handler=UserLogin",
            method: "GET",
            data: { parapass: password },
            success: function (data) {
                if (data == "loginsucess") {

                    $("#partialModal .modal-body").load("/Role");
                    $("#partialModal").modal('show');


                   // $("#optBU").val([]);

                    //$("#dialog").dialog("open");
                    //showMessage('success');
                    // window.location.href = "/Account/Role";

                 //   $.ajax({
                 //       type: 'GET',
                 //       url: '?handler=LoginSuccess',
                 //       contentType: false,
                 //       processData: false,
                 //       success: function (res) {
                 //           $('#partialModal .modal-body').html(res.html);
                 ///*           $('#partialModal .modal-title').html(title);*/
                 //           $('#partialModal').modal('show');
                 //       },
                 //       error: function (err) {
                 //           console.log(err)
                 //       }
                 //   })


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


    $("#optRole").on("focus",function (e) {
        // var password = $("#txtPassword").val();
        $.ajax({
            url: "Role?handler=CheckRole",
            method: "GET",
            // contentType: 'application/json; charset=utf-8',
            // dataType: "json",
            data: { parabu: $("#optBU").val() },
            success: function (data) {

                //Remove all items in the countriesList
                $("#optRole option").remove();

                //For each item retrieved in the AJAX call...
                $.each(data, function (index, itemData) {
                    //...append that item to the countriesList
                    $("#optRole").append("<option value='" + itemData + "'>" + itemData + "</option>");

                });

                $("#optRole").val([]);
            },
            error: function (xhr, status, error) {
                // Handle errors
                alert("Error " + error);
            }
        })
        e.preventDefault();
    })

});



