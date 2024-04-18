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

// Function to calculate annual forecast for each item
function calculateAnnualForecast() {
  var inputQty = parseFloat($("#addAnnualForecast").val()); // Get the input quantity

  // Validate input
  if (isNaN(inputQty) || inputQty <= 0) {
    alert("Please enter a valid input quantity.");
    return;
  }

  // Loop through each row in the table
  $("#SRFQTbl tbody tr").each(function () {
    var eqpa = parseFloat($(this).find("td:eq(7)").text()); // Get Eqpa value for the current row
    var annualForecast = inputQty * eqpa; // Calculate annual forecast
    $(this).find("td#annualForecast").text(annualForecast); // Update annual forecast cell
  });
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

  $.ajax({
    type: "POST",
    url: ProjectUrl,
    data: { projectName: partNumber },
    success: function (response) {
      alert(response);
    },
    error: function (xhr, status, error) {
      alert("Error: " + error);
    },
  });
}

$("#btnSubmit").click(function (e) {
  e.preventDefault();

  let currentIds = [];
  let annualForecasts = [];
  let isValid = true; // Flag variable to track input validity

  // Loop through each row in the table
  $("#SRFQTbl tbody tr").each(function () {
    let currentId = $(this).find("td:eq(0)").text();
    let annualForecastValue = $(this).find("td#annualForecast").text();
    currentIds.push(currentId);
    annualForecasts.push(annualForecastValue);

    if (isNaN(annualForecastValue) || annualForecastValue <= 0) {
      alert(
        "Please enter a valid input quantity in the Annual Forecast field."
      );
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
      alert("All data saved successfully.");
      project(partNumber);
    },
    error: () => {
      alert("Failed to save data.");
    },
  });
});
