fetch("/Dashboard/Dashboard?handler=AllRFQProjects")
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    let count = data.data.length;
    $("#rfqCount").text(count);
    viewRFQProjects("#libraryTable", data.data);
  })
  .catch((error) => {
    alert("Error: " + error.message);
  });

fetch("/Dashboard/Dashboard?handler=IncomingRFQProjects")
  .then((response) => {
    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }
    return response.json();
  })
  .then((data) => {
    let count = data.data.length;
    // CHECKING OF ROLES FOR INCOMING RFQ
    //COST ENGR - PROCESSOR IF HAVE PRICES SHOW
    //SOURCING - PROCESSOR IF NO PRICES SHOW
    $("#incoming").text(count);
    viewRFQProjects("#incomingRFQTable", data.data);
  })
  .catch((error) => {
    alert("Error: " + error.message);
  });
// Function to upload a file
function uploadFile(modalId, url, inputId) {
  const fileInput = document.getElementById(inputId);
  // console.log(modalId, url, inputId);
  const file = fileInput.files[0];
  const formData = new FormData();
  formData.append("file", file);

  if (!fileInput.files || !fileInput.files[0]) {
    // Check if a file was selected
    showErrorAlert("Please select a file to upload.");
    $("#" + modalId + "Button").prop("disabled", false); // Re-enable the button
    return;
  } else {
    $.ajax({
      url: url,
      type: "POST",
      data: formData,
      processData: false,
      contentType: false,
      headers: {
        RequestVerificationToken: $(
          'input:hidden[name="__RequestVerificationToken"]'
        ).val(),
      },
      beforeSend: function () {
        $("#" + modalId + "loadingSpinner").removeClass("d-none"); // Show the loading spinner
        $("[id^='" + modalId + "InputContainer']").hide(); // Hide the file input container
        $("#" + modalId + "Button").prop("disabled", true); // Disable the button while the file is being uploaded
      },
      success: function (data) {
        $("#" + modalId + "loadingSpinner").addClass("d-none"); // Hide the loading spinner
        $("#" + modalId + "Button").prop("disabled", false); // Re-enable the button
        $("[id^='" + modalId + "InputContainer']").show(); // Show the file input container
        if (data.success) {
          showSuccessAlert(data.message);
          $("#" + inputId).val(""); // Clear the file input value
          $("#" + modalId).modal("hide"); // Close the modal
        } else {
          showErrorAlert(data.message);
          $("#" + inputId).val("");
        }
      },
      error: function (xhr, status, error) {
        console.error("Error uploading file:", error);
        $("#" + modalId + "loadingSpinner").addClass("d-none"); // Hide the loading spinner
        $("#" + modalId + "Button").prop("disabled", false); // Re-enable the button
        showErrorAlert("An error occurred while uploading the file.");
      },
    });
  }
}

// Uploading Excel Files
$("#uploadLastPurchaseButton").click(function () {
  uploadFile(
    "uploadLastPurchase",
    "/Dashboard/Dashboard?handler=UploadLastPurchaseInfo",
    "lastPurchaseExcel"
  );
});

$("#uploadQuotationButton").click(function () {
  uploadFile(
    "uploadQuotation",
    "/Dashboard/Dashboard?handler=UploadQuotations",
    "quotationExcel"
  );
});

$("#uploadMRPBomButton").click(function () {
  uploadFile(
    "uploadMRPBom",
    "/Dashboard/Dashboard?handler=UploadMRPBOM",
    "MRPBomExcel"
  );
});

function sourcingForm() {
  fetch("/Dashboard/Dashboard?handler=PNandDescription", {
    method: "GET",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      let selectElement = $("#sourcingFormSelect");
      selectElement.empty();
      if (data.data.length === 0) {
        $("<option selected disabled>No available MRP BOM</option>").appendTo(
          selectElement
        );
      } else {
        $("<option selected disabled>Select BOM</option>").appendTo(
          selectElement
        );

        data.data.forEach((item) => {
          let partNumber = item.partNumber;
          let description = item.description;
          let pndesc = partNumber + " - " + description;
          let option = $("<option></option>")
            .attr("value", pndesc)
            .text(pndesc);
          selectElement.append(option);
        });
      }
    })
    .catch((error) => {
      console.error("Error:", error);
    });
}
function viewRFQProjects(table, data) {
  let Tbl = $(table).DataTable({
    responsive: true,
    data: data,
    columns: [
      { data: "quotationCode" },
      {
        data: "requestDate",
        render: function (data) {
          return formatDate(data);
        },
      },
      {
        data: "projectName",
        render: function (data) {
          return getPartNumber(data);
        },
      },
      {
        data: "status",
        render: function (data, type, row) {
          let badgeClass;
          if (data === "OPEN") {
            badgeClass = "badge rounded-pill text-bg-success";
          } else if (data === "CLOSED") {
            badgeClass = "badge rounded-pill text-bg-danger";
          } else {
            badgeClass = "";
          }
          return (
            '<span class="' +
            badgeClass +
            '" data-status="' +
            data +
            '">' +
            data +
            "</span>"
          );
        },
      },
      {
        data: null,
        render: function (row) {
          // const isClosed = row.status === "CLOSED";
          // const markBtnDisabled = isClosed ? "disabled" : "";
          if (department === "Cost Engineering") {
            return (
              '<button type="button" class="btn btn-sm btn-primary border-3 border-primary-subtle list-btn me-2" data-id="' +
              row.quotationCode +
              '" data-name="' +
              row.projectName +
              '">View</button>' +
              '<button type="button" class="btn btn-sm btn-danger border-3 border-danger-subtle marked-btn" data-id="' +
              row.quotationCode +
              '" data-name="' +
              row.projectName +
              '"> Mark as Closed</button>'
            );
          } else if (department === "Sourcing") {
            return (
              '<a href="#" class="text-primary view-btn fs-4" data-id="' +
              row.quotationCode +
              '" data-name="' +
              row.projectName +
              '"><i class="fa-solid fa-eye"></i></a> ' +
              '<a href="#" class="text-success download-btn fs-4" data-id="' +
              row.projectName +
              '"><i class="fa-solid fa-download"></i></a>'
            );
          } else {
            return "Viewer kaba ?";
          }
        },
      },
    ],
  });
}

// Attach click event handler to a parent element using event delegation
$("#incomingRFQTable").on("click", ".view-btn", function () {
  let projectName = $(this).data("name");
  // console.log(projectName);
  let url = "/Sourcing/SourcingRFQForm?partNumber=" + projectName;

  window.location.href = url;
});

$("#incomingRFQTable").on("click", ".download-btn", function () {
  let projectName = $(this).data("id");
  let url =
    "/Dashboard/Dashboard?handler=DownloadExcelFile&projectName=" +
    encodeURIComponent(projectName);
  // console.log(url);
  window.location.href = url;
});

$("#incomingRFQTable").on("click", ".marked-btn", function () {
  let quotationCode = $(this).data("id");
  let projectName = $(this).data("name");
  // console.log(quotationCode, projectName);

  const requestData = {
    quotationCode: quotationCode,
    projectName: projectName,
  };

  fetch("/Dashboard/Dashboard?handler=MarkedAsClosed", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
    body: JSON.stringify(requestData),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      console.log(data);
      if (data.success) {
        showSuccessAlert(data.message).then(() => {
          location.reload();
        });
      } else {
        showErrorAlert(data.message);
      }
    })
    .catch((error) => {
      // Handle errors
      console.error("Error:", error);
      showErrorAlert("An error occurred while marking the project as closed.");
    });
});

$("#incomingRFQTable").on("click", ".list-btn", function () {
  let projectName = $(this).data("name");
  let quotation = $(this).data("id");
  let url =
    "/Sourcing/SourcingRFQPrices?projectName=" +
    projectName +
    "&quotationCode=" +
    quotation;

  window.location.href = url;
});

$("#libraryTable").on("click", ".view-btn", function () {
  let quotationCode = $(this).data("name");

  // console.log(quotationCode);
});
// Function to format the date
function formatDate(dateString) {
  let date = new Date(dateString);
  let day = date.getDate();
  let monthNames = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ];
  let monthIndex = date.getMonth();
  let year = date.getFullYear().toString().slice(-2); // Get last two digits of the year
  return day + "-" + monthNames[monthIndex] + "-" + year;
}
// Function to extract part number from projectName
function getPartNumber(projectName) {
  // Split projectName by space and take the first part
  return projectName.split(" ")[0];
}
// Event listener for the change event on the select element
$("#sourcingFormSelect").change(function () {
  // Check if an option other than the default is selected
  if ($(this).val() !== "Select BOM") {
    // Show the View button
    $("#viewPNDesc").removeClass("d-none");
  } else {
    // Hide the View button
    $("#viewPNDesc").addClass("d-none");
  }
});
// Event listener for the click event on the View button
$("#viewPNDesc").click(function () {
  // Get the selected option's value (pndesc)
  let pndesc = $("#sourcingFormSelect").val();

  // Redirect to the ViewSourcingForm action with the modified pNDesc parameter
  window.location.href =
    "/Sourcing/ViewSourcingForm?pndesc=" + encodeURIComponent(pndesc);
});
