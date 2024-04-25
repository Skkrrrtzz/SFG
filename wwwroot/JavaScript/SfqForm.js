var table = $("#SRFQTbl").dataTable({
  responsive: true,
});
// Variable to store the current ID
let currentId = null;

// Function to fetch data for editing
function fetchDataForEditing(id) {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: FindIdUrl,
      method: "GET",
      data: { id: id },
      success: resolve,
      error: reject,
    });
  });
}

// Function to save data
function saveData(formData) {
  return new Promise((resolve, reject) => {
    $.ajax({
      url: UpdateUrl,
      method: "POST",
      data: formData,
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
$(".btnEdit").click(function (e) {
  e.preventDefault();
  currentId = $(this).data("id"); // Store the current ID
  fetchDataForEditing(currentId)
    .then((data) => {
      populateEditFormFields(data);
    })
    .catch(() => {
      alert("Failed to fetch data for editing.");
    });
});

// Add click event listener to the save button
$("#btnSave").click(function (e) {
  e.preventDefault();
  if (!currentId) {
    alert("No ID found for saving.");
    return;
  }

  const formData = {
    id: currentId, // Use the current ID
    customerPartNumber: $("#editCustomerPartnumber").val(),
    rev: $("#editRev").val(),
    description: $("#editDescription").val(),
    origMPN: $("#editOrigMPN").val(),
    origMFR: $("#editOrigMFR").val(),
    commodity: $("#editCommodity").val(),
    eqpa: $("#editEqpa").val(),
    uoM: $("#editUoM").val(),
    status: $("#editStatus").val(),
  };

  saveData(formData)
    .then(() => {
      Swal.fire({
        title: "Message",
        text: "Data saved successfully.",
        icon: "success",
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 2000,
      }).then((result) => {
        location.reload();
      });
    })
    .catch(() => {
      Swal.fire({
        title: "Message",
        text: "Failed to save data.",
        icon: "error",
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 2000,
      });
    });
});
var sourcingTbl = $("#SRFQTbl").DataTable();
var allRowsData = sourcingTbl.rows().data();
// Function to calculate annual forecast for each item
function calculateAnnualForecast() {
  var inputQty = parseFloat($("#addAnnualForecast").val()); // Get the input quantity

  // Validate input
  if (isNaN(inputQty) || inputQty <= 0) {
    Swal.fire({
      title: "Message",
      text: "Please enter a valid input quantity.",
      icon: "error",
      toast: true,
      position: "top-end",
      showConfirmButton: false,
      timer: 2000,
    });
    return;
  }

  // Loop through each row in the table
  allRowsData.each(function (rowData, index) {
    var eqpa = parseFloat(rowData[7]); // Assuming eqpa is stored in the 8th column
    var annualForecast = inputQty * eqpa;
    rowData[8] = annualForecast; // Assuming annual forecast should be stored in the 9th column

    // Update the DataTable with the modified rowData
    sourcingTbl.row(index).data(rowData);
  });

  // Redraw the DataTable to reflect the changes
  sourcingTbl.draw();
}
// Add click event listener to the button
$("#btnAddAnnualForecast").click(function () {
  calculateAnnualForecast();
});

//
function project(partNumber) {
  if (partNumber == null) {
    alert("No ProjectName found.");
    return;
  }
  Swal.fire({
    title: "Loading...",
    html: '<div class="m-2" id="loading-spinner"><div class="loader3"><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div></div></div>',
    showCancelButton: false,
    showConfirmButton: false,
    allowOutsideClick: false,
    allowEscapeKey: false,
  });
  captureDivToImage("capture", function (dataURL) {
    // Convert data URL to Blob
    var blob = dataURLtoBlob(dataURL);

    // Create FormData object
    var formData = new FormData();
    formData.append("projectName", partNumber);
    formData.append("image", blob); // No need to specify file name here

    // Make the AJAX request with both the projectName and imageData
    $.ajax({
      type: "POST",
      url: ProjectUrl,
      data: formData,
      processData: false,
      contentType: false,
      success: function (response) {
        // Display a SweetAlert2 success message
        Swal.fire({
          icon: "success",
          title: "Notification Sent to Sourcing Team",
          toast: true,
          position: "top-end",
          timer: 3000,
          showConfirmButton: false,
        }).then(function () {
          Swal.close();
          window.location.href = "/Dashboard/Dashboard";
        });
      },
      error: function (xhr, status, error) {
        Swal.close();
        alert("Error: " + error);
      },
    });
  });
}

// Function to convert data URL to Blob
function dataURLtoBlob(dataURL) {
  var arr = dataURL.split(",");
  var mime = arr[0].match(/:(.*?);/)[1];
  var bstr = atob(arr[1]);
  var n = bstr.length;
  var u8arr = new Uint8Array(n);
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
    if (isNaN(annualForecastValue)) {
      // Display a SweetAlert2 success message
      Swal.fire({
        icon: "info",
        title:
          "Please enter a valid input quantity in the Annual Forecast field.",
        toast: true,
        position: "top-end",
        timer: 3000,
        showConfirmButton: false,
      });

      isValid = false; // Set isValid to false if invalid input is detected
      return false; // Exit the loop
    }
  });

  if (!isValid) {
    return; // Exit the function if invalid input is detected
  }
  const partNumber = $("#projectName").val();
  // Send AJAX request to the controller
  $.ajax({
    url: AddAFUrl,
    method: "POST",
    data: {
      ids: currentIds,
      annualForecasts: annualForecasts,
    },
    success: () => {
      // Display a SweetAlert2 success message
      Swal.fire({
        icon: "success",
        title: "All data saved successfully.",
        toast: true,
        position: "top-end",
        timer: 3000,
        showConfirmButton: false,
      });
      project(partNumber);
    },
    error: () => {
      alert("Failed to save data.");
    },
  });
});

function captureDivToImage(divId, callback) {
  // Get the jQuery object of the div element by id
  var $div = $("#" + divId);

  // Create a new canvas element
  var canvas = document.createElement("canvas");
  var context = canvas.getContext("2d");

  // Set canvas dimensions based on the size of the div
  canvas.width = $div.width();
  canvas.height = $div.height();

  // Render the content of the div onto the canvas
  html2canvas($div.get(0)).then(function (canvas) {
    // Convert canvas to data URL
    var dataURL = canvas.toDataURL("image/png");

    // Invoke the callback function with the image data URL
    callback(dataURL);
  });
}
