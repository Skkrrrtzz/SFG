let table = $("#SRFQTbl").dataTable({
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

  fetch(UpdateUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(formData),
  })
    .then((response) => response.json())
    .then((data) => {
      if (data === true) {
        Swal.fire({
          title: "Message",
          text: "Data saved successfully.",
          icon: "success",
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 2000,
        }).then(() => {
          location.reload();
        });
      } else {
        Swal.fire({
          title: "Message",
          text: "Failed to save data.",
          icon: "error",
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 2000,
        });
      }
    })
    .catch((error) => {
      console.error("Error:", error);
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

let sourcingTbl = $("#SRFQTbl").DataTable();
let allRowsData = sourcingTbl.rows().data();
// Function to calculate annual forecast for each item
function calculateAnnualForecast() {
  let inputQty = parseFloat($("#addAnnualForecast").val()); // Get the input quantity

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
    let eqpa = parseFloat(rowData[7]); // Assuming eqpa is stored in the 8th column
    let annualForecast = inputQty * eqpa;
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
    let blob = dataURLtoBlob(dataURL);

    // Create FormData object
    let formData = new FormData();
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
      // Display a SweetAlert2 warning message
      const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.onmouseenter = Swal.stopTimer;
          toast.onmouseleave = Swal.resumeTimer;
        },
      });
      Toast.fire({
        icon: "info",
        title:
          "Please enter a valid input quantity in the Annual Forecast field.",
      });

      isValid = false; // Set isValid to false if invalid input is detected
      return false; // Exit the loop
    }
  });

  if (!isValid) {
    return; // Exit the function if invalid input is detected
  }
  const partNumber = $("#projectName").val();

  const formData = new FormData();
  formData.append("ids", JSON.stringify(currentIds));
  formData.append("annualForecasts", JSON.stringify(annualForecasts));

  // Send AJAX request to the controller
  fetch(AddAFUrl, {
    method: "POST",
    body: JSON.stringify({ ids: currentIds, annualForecasts: annualForecasts }),
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) => {
      if (response.ok) {
        // Display a SweetAlert2 success message
        Swal.fire({
          icon: "success",
          title: "All data saved successfully.",
          toast: true,
          position: "top-end",
          timer: 3000,
          showConfirmButton: false,
        }).then(function () {
          project(partNumber);
        });
      } else {
        throw new Error("Failed to save data.");
      }
    })
    .catch((error) => {
      alert(error.message);
    });
});

function captureDivToImage(divId, callback) {
  // Get the jQuery object of the div element by id
  let $div = $("#" + divId);

  // Create a new canvas element
  let canvas = document.createElement("canvas");
  let context = canvas.getContext("2d");

  // Set canvas dimensions based on the size of the div
  canvas.width = $div.width();
  canvas.height = $div.height();

  // Render the content of the div onto the canvas
  html2canvas($div.get(0)).then(function (canvas) {
    // Convert canvas to data URL
    let dataURL = canvas.toDataURL("image/png");

    // Invoke the callback function with the image data URL
    callback(dataURL);
  });
}
