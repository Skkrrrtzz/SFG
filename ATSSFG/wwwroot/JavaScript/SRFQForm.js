function getQueryStringParameter(name) {
  let urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
let pNDesc = getQueryStringParameter("pndesc");
let table = $("#SRFQTbl").dataTable({
  responsive: true,
});
// Variable to store the current ID
let currentId = null;

// Function to fetch data for editing
function fetchDataForEditing(id) {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: "/Sourcing/SourcingRFQForm?handler=FindId",
      method: "GET",
      data: { id: id },
      success: resolve,
      error: reject,
    });
  });
}

// Function to populate edit form fields
function populateEditFormFields(data) {
  $("#editCustomerPartnumber").val(data.customerPartNumber);
  $("#editRev").val(data.rev);
  $("#editDescription").val(data.description);
  $("#editOrigMPN").val(data.origMPN);
  $("#editOrigMFR").val(data.origMFR);
  $("#editCommodity").val(data.commodity);
  $("#editEqpa").val(data.eqpa);
  $("#editUoM").val(data.uoM);
  $("#editStatus").val(data.status);
}

// Add click event listener to the edit buttons
$("#SRFQTbl").on("click", ".btnEdit", function (e) {
  e.preventDefault();
  currentId = $(this).data("id"); // Store the current ID
  console.log(currentId);
  fetchDataForEditing(currentId)
    .then((data) => {
      populateEditFormFields(data);
    })
    .catch(() => {
      showInfoAlert("Failed to fetch data for editing.");
    });
});

// Add click event listener to the save button
$("#btnSave").click(function (e) {
  e.preventDefault();

  if (!currentId) {
    showErrorAlert("No ID found for saving.");
    return;
  }

  // Get form data from input fields
  const formData = {
    Id: currentId, // Use the current ID
    CustomerPartNumber: $("#editCustomerPartnumber").val(),
    Rev: $("#editRev").val(),
    Description: $("#editDescription").val(),
    OrigMPN: $("#editOrigMPN").val(),
    OrigMFR: $("#editOrigMFR").val(),
    Commodity: $("#editCommodity").val(),
    Eqpa: $("#editEqpa").val(),
    UoM: $("#editUoM").val(),
    Status: $("#editStatus").val(),
  };

  // Send a POST request to the server with form data
  fetch("/Sourcing/SourcingRFQForm?handler=UpdateId", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
    body: JSON.stringify(formData),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((response) => {
      if (response.success) {
        showSuccessAlert(response.message).then(() => {
          location.reload();
        });
      } else {
        showErrorAlert("Failed to save data.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      showErrorAlert("An error occurred while saving data.");
    });
});

let sourcingTbl = $("#SRFQTbl").DataTable();
let allRowsData = sourcingTbl.rows().data();
// Function to calculate annual forecast for each item
function calculateAnnualForecast() {
  let inputQty = parseFloat($("#addAnnualForecast").val()); // Get the input quantity

  if (isNaN(inputQty) || inputQty <= 0) {
    showInfoAlert("Please enter a valid quantity.");
    return;
  }
  // Loop through each row in the table
  allRowsData.each(function (rowData, index) {
    let eqpa = parseFloat(rowData[7]);
    let annualForecast = inputQty * eqpa;
    rowData[8] = annualForecast;

    // Update the DataTable with the modified rowData
    sourcingTbl.row(index).data(rowData);
  });

  // Redraw the DataTable to reflect the changes
  sourcingTbl.draw();
}
// Add click event listener to the button
$("#btnAddAnnualForecast").click(function () {
  calculateAnnualForecast();
  // close modal
  $("#addAnnualForecastModal").modal("hide");
});

//
function project(partNumber) {
  if (partNumber == null) {
    showAlertMiddle("No ProjectName found.");
    return;
  }
  showLoading();
  captureDivToImage("capture", function (dataURL) {
    // Convert data URL to Blob
    let blob = dataURLtoBlob(dataURL);
    let formData = new FormData();
    formData.append("projectName", partNumber);
    formData.append("image", blob);
    formData.append(
      "__RequestVerificationToken",
      $('input:hidden[name="__RequestVerificationToken"]').val()
    );
    fetch("/Sourcing/SourcingRFQForm?handler=Project", {
      method: "POST",
      body: formData,
    })
      .then((response) => {
        if (response.ok) {
          return showSuccessAlert("Notification Sent to Sourcing Team.");
        } else {
          throw new Error("An error occurred while submitting the form.");
        }
      })
      .then(() => {
        window.location.href = "/Dashboard/Dashboard";
      })
      .catch((error) => {
        console.error("Error:", error);
        showErrorAlert(error.message);
      });
  });
}

// Function to convert data URL to Blob
function dataURLtoBlob(dataURL) {
  let arr = dataURL.split(",");
  let mime = arr[0].match(/:(.*?);/)[1];
  let bstr = atob(arr[1]);
  let n = bstr.length;
  let u8arr = new Uint8Array(n);
  while (n--) {
    u8arr[n] = bstr.charCodeAt(n);
  }
  return new Blob([u8arr], { type: mime });
}

$("#btnSubmit").click(function (e) {
  e.preventDefault();

  let currentIds = [];
  let annualForecasts = [];
  let isValid = true; // Flag variable to track input validity

  // Loop through each row in the table
  allRowsData.each(function (rowData, index) {
    let currentId = rowData[0];
    let annualForecastValue = rowData[8];

    currentIds.push(currentId);
    annualForecasts.push(annualForecastValue);
    if (isNaN(annualForecastValue) || annualForecastValue === "") {
      showInfoAlert(
        "Please enter a valid input quantity in the Annual Forecast field."
      );

      isValid = false; // Set isValid to false if invalid input is detected
      return false; // Exit the loop
    }
  });

  if (!isValid) {
    return; // Exit the function if invalid input is detected
  }
  showLoading();
  const partNumber = $("#projectName").val();

  const formData = new FormData();
  formData.append("ids", JSON.stringify(currentIds));
  formData.append("annualForecasts", JSON.stringify(annualForecasts));
  // Send AJAX request to the controller
  fetch("/Sourcing/SourcingRFQForm?handler=AddAnnualForecast", {
    method: "POST",
    body: JSON.stringify({ ids: currentIds, annualForecasts: annualForecasts }),
    headers: {
      "Content-Type": "application/json",
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
  })
    .then((response) => {
      if (response.ok) {
        showSuccessAlert("All data saved successfully.").then(() => {
          project(partNumber);
        });
      } else {
        throw new Error("Failed to save data.");
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      showErrorAlert(error.message);
    });
});

function captureDivToImage(divId, callback) {
  // Get the jQuery object of the div element by id
  let $div = $("#" + divId);

  // Create a new canvas element
  let canvas = document.createElement("canvas");
  let context = canvas.getContext("2d");

  // Set canvas dimensions based on the size of the div
  canvas.width = 500;
  canvas.height = 400;

  // Render the content of the div onto the canvas
  html2canvas($div.get(0)).then(function (canvas) {
    // Convert canvas to data URL
    let dataURL = canvas.toDataURL("image/png");

    // Invoke the callback function with the image data URL
    callback(dataURL);
  });
}
function uploadExcel(file, projectName) {
  showLoading();

  const formData = new FormData();
  formData.append("file", file);
  formData.append("fileName", projectName);

  fetch("/Sourcing/SourcingRFQForm?handler=UploadRFQExcelFile", {
    method: "POST",
    body: formData,
    headers: {
      RequestVerificationToken: $(
        'input:hidden[name="__RequestVerificationToken"]'
      ).val(),
    },
  })
    .then((response) => response.json())
    .then((response) => {
      if (response.success) {
        // Display success message using SweetAlert
        showSuccessAlert(response.message).then(function () {
          window.location.href = "/Dashboard/Dashboard";
        });
      } else {
        showErrorAlert(response.message);
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      // Display error message using SweetAlert
      showErrorAlert("Failed to upload RFQ info.");
    });
}

function checkExcel() {
  const excelFile = $("#excelFileInput")[0];
  const projectName = $("#projectName").val();

  if (!excelFile.files || !excelFile.files[0]) {
    // Check if a file was selected
    showWarningAlert("Please select a file to upload.");
    return;
  } else {
    const file = excelFile.files[0];
    const formData = new FormData(); // Create a FormData object
    formData.append("file", file); // Append the file to FormData

    fetch("/Sourcing/SourcingRFQForm?handler=CheckingUploadedFile", {
      method: "POST",
      body: formData,
      headers: {
        RequestVerificationToken: $(
          'input:hidden[name="__RequestVerificationToken"]'
        ).val(),
      },
    })
      .then((response) => response.json())
      .then((response) => {
        if (response.success) {
          // Display success message using SweetAlert
          Swal.fire({
            icon: "success",
            title: "Success",
            text: response.message,
            toast: true,
            position: "top-end",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Submit",
            cancelButtonText: "Cancel",
          }).then((result) => {
            if (result.isConfirmed) {
              showLoading();
              uploadExcel(file, projectName);
            }
          });
        } else {
          // Display error message using SweetAlert
          showErrorAlert(response.message);
        }
      })
      .catch((error) => {
        // Display error message using SweetAlert
        showErrorAlert("An error occurred while uploading the file.");
        console.error("Error:", error);
      });
  }
}
