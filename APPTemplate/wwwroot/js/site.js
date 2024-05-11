﻿// #region Function
function DialogError(xhr) {
    $("#loading").fadeOut();
    var responseJson = JSON.parse(xhr.responseText);
    var responseText;
    $("#dialogerror").html("");
    try {
        $("#dialogerror").append("<div><b>Exception</b><hr />" + responseJson.errorMessage + "<br /><br /></div>");

    } catch (e) {
        responseText = xhr.responseText;
        $("#dialogerror").html(responseText);
    }

    $("#dialogerror").dialog({
        title: "ERROR",
        autoOpen: true,
        show: {
            effect: "fade",
            duration: 320
        },
        hide: {
            effect: "fade",
            duration: 300
        },
        width: 300,
        //buttons: {
        //    Close: function () {
        //        $(this).dialog('close');
        //    }
        //}
    });
}
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
}

//#endregion Function

$(function () {
    // #region Login

    $('#myList a').on('click', function (e) { 
        e.preventDefault()
        $(this).tab('show')
    })

    $("#ulBU .list-group-item").on('click', function (e) {
        $("#ulBU .list-group-item").removeClass("active");
        $(e.target).addClass("active");
    });

    $("#ulRole .list-group-item").on('click',function (e) {
        $("#ulRole .list-group-item").removeClass("active");
        $(e.target).addClass("active");
    });

    $("#btnLogin").on("click", function (e) {
        $.ajax({
            url: "Login?handler=UserLogin",
            method: "GET",
            data: { parapass: $("#txtPassword").val() },
            success: function (data) {
                if (JSON.parse(data)["Success"] === true) {
                    $("#partialModal .modal-body").load("/Role");
                    $("#partialModal").modal('show');
                }
                else {
                    // $("#stkRole").hide();
                    $("#txtPassword").select();
                    showMessage('error');
                }
            }
        })
        e.preventDefault();
    })
    $("#optRole").on("focus", function (e) {
        $.ajax({
            url: "Role?handler=CheckRole",
            method: "GET",
            // contentType: 'application/json; charset=utf-8',
            // dataType: "json",
            data: { parabu: $("#optBU option:selected").text() },
            success: function (data) {
                //Remove all items in the countriesList
                $("#optRole option").remove();

                //For each item retrieved in the AJAX call...
                $.each(data, function (index, itemData) {
                    //Append that item to the countriesList
                    $("#optRole").append("<option value='" + itemData + "'>" + itemData + "</option>");
                });

                $("#optRole").val([]);
            }
        })
        e.preventDefault();
    })
    $("#btnNext").on("click", function (e) {
        var varbu = $("#optBU option:selected").text();
        var varbuid = $("#optBU option:selected").val();
        var varrole = $("#optRole option:selected").text();
        $.ajax({
            url: "Role?handler=StartPage",
            method: "GET",
            data: { parabu: varbu, parabuid: varbuid, pararole: varrole },
            success: function (data) {
                if (JSON.parse(data)["currentrole"] === 'REQUESTOR') {
                    window.location.href = "/Requestor";
                }
                if (JSON.parse(data)["currentrole"] === 'APPROVER') {
                    window.location.href = "/Approver";
                }
                if (JSON.parse(data)["currentrole"] === 'NOTER') {
                    window.location.href = "/Noter";
                }
            }
        })
        e.preventDefault();
    })

    // #endregion Login
});