// Function to get the value of a query string parameter by name
function getQueryStringParameter(name) {
  var urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
// Get the value of the pNDesc parameter from the query string
var pNDesc = getQueryStringParameter("pndesc");
// console.log(pNDesc);
// Set the value to an element with ID "pNDesc"
$("#pNDesc").text(pNDesc);
// Split the pNDesc into PartNumber and Description
var parts = pNDesc.split(" - ");
var partNumber = parts[0].trim();

if (partNumber != null) {
  // Send the PartNumber to the server using AJAX
  $.ajax({
    type: "POST",
    url: checkDataUrl,
    data: { PartNumber: partNumber },
    dataType: "JSON",
    success: function (response) {
      var data = response.data;
      // Call the getMRPData function with the response data
      getMRPData(data);
    },
    error: function (xhr, status, error) {
      console.error("Error:", error);
    },
  });
} else {
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
