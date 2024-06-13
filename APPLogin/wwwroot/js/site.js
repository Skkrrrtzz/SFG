// #region Function

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
function ShowMessage(paramode) {
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

    var table = $('#tblPending').DataTable({ searching: false, paging: false, info: false });
    $('#tblPending tbody').on('dblclick', 'tr', function () {
        var rowData = table.row(this).data();
        console.log(rowData[3]);
    });


    $("#txtPassword").focus();

    $('#txtPassword').on("keypress", function (e) {
        if (e.key === "Enter") {
            $("#loading").fadeIn();
            $("#btnLogin").trigger("click");
        }
    });

    $("#btnLogin").on("click", function (e) {
        $.ajax({
            url: "Login?handler=UserLogin",
            method: "GET",
            data: { parapass: $("#txtPassword").val() },
            dataType: 'json',
            success: function (data) {
                //console.log(data);
                if (data.success) {
                    window.location.href = "/Menu";
                }
                else {
                    $("#loading").fadeOut();
                    $("#txtPassword").select();
                    ShowMessage('error');
                }
            },
            error: DialogError
        });
        e.preventDefault();
    });

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

    // #region Menu
    //$("#tblPending tbody tr").on("dblclick", function (e) {
    //    var rowData = $(this).find('td').map(function () {
    //        return $(this).text();
    //    }).get();
    //    //alert(rowData[3]);
    //    window.location.href = "http://192.168.0.188:8083";

    //    e.preventDefault();
    //})



    // #endregion Menu
});