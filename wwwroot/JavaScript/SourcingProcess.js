﻿// Define data
var data;

// Function to get the value of a query string parameter by name
function getQueryStringParameter(name) {
  var urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
// Get the value of the pNDesc parameter from the query string
var pNDesc = getQueryStringParameter("pndesc");
var uploadSRFQForm =
  "/Sourcing/SourcingRFQForm?partNumber=" + encodeURIComponent(pNDesc);
// console.log(pNDesc);
// Set the value to an element with ID "pNDesc"
$("#pNDesc").text(pNDesc);
// Split the pNDesc into PartNumber and Description
var parts = pNDesc.split(" - ");
var partNumber = parts[0].trim();

if (partNumber != null) {
  Swal.fire({
    title: "Loading...",
    html: '<div class="m-2" id="loading-spinner"><div class="loader3"><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div></div></div>',
    showCancelButton: false,
    showConfirmButton: false,
    allowOutsideClick: false,
    allowEscapeKey: false,
  });
  // Send the PartNumber to the server using AJAX
  $.ajax({
    type: "POST",
    url: checkDataUrl,
    data: { PartNumber: partNumber },
    dataType: "JSON",
    success: function (response) {
      data = response.data;
      // Call the getMRPData function with the response data
      getMRPData(data);
      Swal.close();
    },
    error: function (xhr, status, error) {
      console.error("Error:", error);
    },
  });
} else {
  Swal.close();
  console.error("PartNumber is null");
}
function getMRPData(data) {
  var table = $("#sourcingTbl").DataTable({
    responsive: true,
    data: data,
    columns: [
      { data: "partNumber" },
      { data: "description" },
      { data: "rev" },
      { data: "commodity" },
      { data: "mpn" },
      { data: "manufacturer" },
      { data: "eqpa" },
      { data: "uom" },
      {
        data: "status",
        render: function (data) {
          if (data === "COMMON") {
            return (
              '<span class="badge rounded-pill text-bg-secondary">' +
              data +
              "</span>"
            );
          } else if (data === "UNIQUE") {
            return (
              '<span class="badge rounded-pill text-bg-primary">' +
              data +
              "</span>"
            );
          } else {
            return data;
          }
        },
      },
      { data: "gwrlQty" },
      { data: "lastPurchaseDate" },
      { data: "remarks" },
      {
        data: null,
        render: function (row) {
          return (
            '<a href="#" class="text-primary edit-btn fs-4" data-id="' +
            row.partNumber +
            '" data-bs-toggle="modal" data-bs-target="#editStatusRemarks"><i class="fa-solid fa-pen-to-square"></i></a>'
          );
        },
      },
    ],
  });
  // Handle click event on edit button
  $("#sourcingTbl tbody").on("click", ".edit-btn", function (e) {
    e.preventDefault();

    // Get the data-id attribute value of the clicked button
    var partId = $(this).data("id");

    // Retrieve row data
    var rowData = table.row($(this).closest("tr")).data();

    // Display the current status and remarks in the edit modal
    $("#editStatus").val(rowData.status);
    $("#editRemarks").val(rowData.remarks);
    $("#editId").val(partId);
  });

  // Handle click event on update button
  $("#btnUpdate").on("click", function (e) {
    e.preventDefault();

    // Retrieve updated status and remarks from modal fields
    var updatedStatus = $("#editStatus").val();
    var updatedRemarks = $("#editRemarks").val();
    var partId = $("#editId").val();

    // Update corresponding row in DataTable with new values
    var index = table.column(0).data().indexOf(partId); // Find index of row based on partId
    var rowData = table.row(index).data(); // Retrieve row data
    rowData.status = updatedStatus; // Update status
    rowData.remarks = updatedRemarks; // Update remarks
    table.row(index).data(rowData).draw(); // Redraw the table with updated data

    // Close the modal
    $("#editStatusRemarks").modal("hide");
  });
}
function downloadPdf() {
  // Get the value of the pNDesc parameter from the query string
  //   var pNDesc = getQueryStringParameter("pndesc");

  // Check if pNDesc is valid
  if (!pNDesc) {
    console.error("pNDesc parameter is missing.");
    return;
  }
  // Send a GET request to download the PDF file
  var downloadUrl =
    "/Dashboard/GetExcelFile?pNDesc=" + encodeURIComponent(pNDesc);
  window.location.href = downloadUrl;
}

$("#btnSourcing").on("click", function (e) {
  e.preventDefault();

  $("#projectName").val(pNDesc);
  $("#noItems").val(getSourcingRowsAndSumEqpa().totalEqpaForSourcing);
});

function getSourcingRowsAndSumEqpa() {
  var sourcingRows = []; // Array to store rows with "FOR SOURCING" remarks
  var totalEqpaForSourcing = 0; // Variable to store the total sum of "eqpa" values

  // Get all rows data from the DataTables instance
  var sourcingTbl = $("#sourcingTbl").DataTable();
  var allRowsData = sourcingTbl.rows().data();
  // Iterate over each row data
  allRowsData.each(function (rowData) {
    var pN = rowData["partNumber"].trim(); // Assuming the first column contains part numbers
    var description = rowData["description"].trim();
    var rev = rowData["rev"].trim();
    var commodity = rowData["commodity"].trim();
    var mpn = rowData["mpn"];
    var mfr = rowData["manufacturer"];
    var eqpa = rowData["eqpa"];
    var uom = rowData["uom"].trim();
    var status = rowData["status"].trim();
    var qty = rowData["gwrlQty"];
    var lastPurchaseDate = rowData["lastPurchaseDate"].trim();
    var remarks = rowData["remarks"].trim();
    // Check if the remarks indicate "FOR SOURCING"
    if (remarks === "FOR SOURCING") {
      // Store the row data in the sourcingRows array
      var row = {
        partNumber: pN,
        description: description,
        rev: rev,
        commodity: commodity,
        mpn: mpn,
        mfr: mfr,
        eqpa: eqpa,
        uom: uom,
        status: status,
        qty: qty,
        lastPurchaseDate: lastPurchaseDate,
        remarks: remarks,
      };
      sourcingRows.push(row);
    }

    // Add eqpa to the total sum of "eqpa" values
    totalEqpaForSourcing += eqpa;
  });
  // Return both the array of sourcing rows and the total sum of "eqpa" values
  return {
    sourcingRows: sourcingRows,
    totalEqpaForSourcing: totalEqpaForSourcing,
  };
}
// Handle form submission
$("#rfqForm").on("submit", function (e) {
  e.preventDefault();
  RFQ(); // Call RFQ function
});
function RFQ() {
  // Get the sourcing rows data
  let sourcingRows = getSourcingRowsAndSumEqpa().sourcingRows;

  // Initialize an array to store the sourcing row data
  let sourcingData = [];

  // Iterate over each object in the sourcingRows array
  sourcingRows.forEach((rowData) => {
    // Push each sourcing row data as an object to the sourcingData array
    sourcingData.push({
      customerPartNumber: rowData.partNumber,
      description: rowData.description,
      rev: rowData.rev,
      commodity: rowData.commodity,
      origMPN: rowData.mpn,
      origMFR: rowData.mfr,
      eqpa: rowData.eqpa,
      uom: rowData.uom,
      status: rowData.status,
      qtyperAssy: rowData.qty,
      lastPurchaseDate: rowData.lastPurchaseDate,
      remarks: rowData.remarks,
    });
  });

  // Example of accessing values outside the loop
  let pN = $("#projectName").val();
  let quotation = $("#quotation").val();
  let noItems = $("#noItems").val();
  let reqDate = $("#reqDate").val();
  let reqCompletionDate = $("#reqCompDate").val();

  var customer;
  if ($("#customer").val() === "custom") {
    customer = $("#customInput").val();
  } else {
    customer = $("#customer").val();
  }
  console.log(customer);
  // Construct the data object to be sent in the AJAX request
  let requestData = {
    projectName: pN,
    customer: customer,
    quotationCode: quotation,
    noItems: noItems,
    requestDate: reqDate,
    requiredDate: reqCompletionDate,
    sourcingData: sourcingData, // Pass the sourcing row data array
  };
  var reqData = JSON.stringify(requestData);
  // Make the AJAX request
  $.ajax({
    type: "POST",
    url: uploadRFQUrl,
    data: reqData,
    contentType: "application/json",
    dataType: "JSON",
    success: function (response) {
      console.log(response);
      // Display a SweetAlert2 success message
      Swal.fire({
        icon: "success",
        title: "Success",
        text: response.message,
        timer: 3000,
        showConfirmButton: false,
      }).then(function () {
        window.location.href = uploadSRFQForm;
      });

      $("#createSourcingForm").modal("hide"); // Close the modal
    },
    error: function (xhr, status, error) {
      // Display a SweetAlert2 success message
      Swal.fire({
        icon: "error",
        title: "Failed",
        text: response.message,
        timer: 3000,
        showConfirmButton: false,
      });
      console.error("Error:", error);
    },
  });
}
$("#customer").change(function () {
  var selectedOption = $(this).val();
  if (selectedOption === "custom") {
    $("#customInput").removeClass("d-none");
  } else {
    $("#customInput").addClass("d-none");
    $("#customInput").val("");
  }
});