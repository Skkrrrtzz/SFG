function lastPurchaseFileUpload() {
  const fileInput = $("#excelFileInput")[0];

  $("#uploadButton").prop("disabled", true); // Disable the button while the file is being uploaded
  $("#fileInputContainer").show(); // Show the file input

  if (!fileInput.files || !fileInput.files[0]) {
    // Check if a file was selected
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Please select a file to upload.",
      toast: true,
      position: "top-end",
      showConfirmButton: false,
      timer: 3000,
    });
    $("#uploadButton").prop("disabled", false); // Re-enable the button
    return;
  } else {
    $("#loadingSpinner").removeClass("d-none"); // Show the loading spinner
    const file = fileInput.files[0]; // Get the uploaded file

    $("#fileName").text(file.name); // Show the file name
    const formData = new FormData(); // Create a FormData object
    formData.append("file", file); // Append the file to FormData
    $("#fileInputContainer").hide(); // Hide the file input container
    $.ajax({
      type: "POST",
      url: UploadLastPurchaseInfo,
      data: formData,
      contentType: false,
      processData: false,
      success: function (response) {
        $("#loadingSpinner").addClass("d-none"); // Hide the loading spinner

        if (response.success) {
          // Display a SweetAlert2 success message
          Swal.fire({
            icon: "success",
            title: "Success",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });

          // Reset the file input field
          $("#excelFileInput").val(""); // Clear the file input value
          $("#uploadLastPurchase").modal("hide"); // Close the modal
          $("#fileInputContainer").show(); // Show the file input
        } else {
          // Display a SweetAlert2 error message
          Swal.fire({
            icon: "error",
            title: "Error",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });
          // Reset the file input field
          $("#excelFileInput").val(""); // Clear the file input value
          $("#fileInputContainer").show(); // Show the file input
        }
        $("#uploadButton").prop("disabled", false); // Re-enable the button
      },
      error: function (xhr, status, error) {
        console.error("Error uploading file:", error);
        $("#loadingSpinner").addClass("d-none"); // Hide the loading spinner
      },
    });
  }
}

function quotationFileUpload() {
  const fileInput = $("#quotationExcel")[0]; // Access the native DOM element

  $("#uploadQuotationButton").prop("disabled", true); // Disable the button while the file is being uploaded
  $("#quotationInputContainer").show(); // Show the file input container

  if (!fileInput.files || !fileInput.files[0]) {
    // Check if a file was selected
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Please select a file to upload.",
      toast: true,
      position: "top-end",
      showConfirmButton: false,
      timer: 3000,
    });
    $("#uploadQuotationButton").prop("disabled", false); // Re-enable the button
    return;
  } else {
    $("#qloadingSpinner").removeClass("d-none"); // Show the loading spinner
    const file = fileInput.files[0]; // Get the uploaded file

    $("#quotationfileName").text(file.name); // Show the file name
    const formData = new FormData(); // Create a FormData object
    formData.append("file", file); // Append the file to FormData
    $("#quotationInputContainer").hide(); // Hide the file input container
    $.ajax({
      type: "POST",
      url: UploadQuotation,
      data: formData,
      contentType: false,
      processData: false,
      success: function (response) {
        $("#qloadingSpinner").addClass("d-none"); // Hide the loading spinner

        if (response.success) {
          // Display a SweetAlert2 success message
          Swal.fire({
            icon: "success",
            title: "Success",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });

          // Reset the file input field
          $("#quotationExcel").val(""); // Clear the file input value
          $("#uploadQuotation").modal("hide"); // Close the modal
          $("#quotationInputContainer").show(); // Show the file input
        } else {
          // Display a SweetAlert2 error message
          Swal.fire({
            icon: "error",
            title: "Error",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });
          // Reset the file input field
          $("#quotationExcel").val(""); // Clear the file input value
          $("#quotationInputContainer").show(); // Show the file input
        }
        $("#uploadQuotationButton").prop("disabled", false); // Re-enable the button
      },
      error: function (xhr, status, error) {
        console.error("Error uploading file:", error);
        $("#qloadingSpinner").addClass("d-none"); // Hide the loading spinner
      },
    });
  }
}

function MRPBomFileUpload() {
  const fileInput = $("#MRPBomExcel")[0]; // Access the native DOM element

  $("#uploadMRPBomButton").prop("disabled", true); // Disable the button while the file is being uploaded
  $("#MRPBomInputContainer").show(); // Show the file input container

  if (!fileInput.files || !fileInput.files[0]) {
    // Check if a file was selected
    Swal.fire({
      icon: "error",
      title: "Error",
      text: "Please select a file to upload.",
      toast: true,
      position: "top-end",
      showConfirmButton: false,
      timer: 3000,
    });
    $("#uploadMRPBomButton").prop("disabled", false); // Re-enable the button
    return;
  } else {
    $("#mrploadingSpinner").removeClass("d-none"); // Show the loading spinner
    const file = fileInput.files[0]; // Get the uploaded file

    $("#MRPBomfileName").text(file.name); // Show the file name
    const formData = new FormData(); // Create a FormData object
    formData.append("file", file); // Append the file to FormData
    $("#MRPBomInputContainer").hide(); // Hide the file input container
    $.ajax({
      type: "POST",
      url: UploadMRPBom,
      data: formData,
      contentType: false,
      processData: false,
      success: function (response) {
        $("#mrploadingSpinner").addClass("d-none"); // Hide the loading spinner

        if (response.success) {
          // Display a SweetAlert2 success message
          Swal.fire({
            icon: "success",
            title: "Success",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });

          // Reset the file input field
          $("#MRPBomExcel").val(""); // Clear the file input value
          $("#uploadMRPBom").modal("hide"); // Close the modal
          $("#MRPBomInputContainer").show(); // Show the file input
        } else {
          // Display a SweetAlert2 error message
          Swal.fire({
            icon: "error",
            title: "Error",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });
          // Reset the file input field
          $("#MRPBomExcel").val(""); // Clear the file input value
          $("#MRPBomInputContainer").show(); // Show the file input
        }
        $("#uploadMRPBomButton").prop("disabled", false); // Re-enable the button
      },
      error: function (xhr, status, error) {
        console.error("Error uploading file:", error);
        $("#mrploadingSpinner").addClass("d-none"); // Hide the loading spinner
      },
    });
  }
}

function sourcingForm() {
  $.ajax({
    type: "GET",
    url: GetPNandDescription,
    dataType: "JSON",
    success: function (response) {
      let selectElement = $("#sourcingFormSelect");
      selectElement.empty();
      $("<option selected disabled>Select BOM</option>").appendTo(
        selectElement
      );

      response.data.forEach((item) => {
        let partNumber = item.partNumber;
        let description = item.description;
        let pndesc = partNumber + " - " + description;
        let option = $("<option></option>").attr("value", pndesc).text(pndesc);
        selectElement.append(option);
      });
    },
    error: function (xhr, status, error) {
      console.error("Error:", error);
    },
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
        render: function (data) {
          if (data === "OPEN") {
            return (
              '<span class="badge rounded-pill text-bg-success">' +
              data +
              "</span>"
            );
          } else if (data === "CLOSED") {
            // Corrected case
            return (
              '<span class="badge rounded-pill text-bg-danger">' +
              data +
              "</span>"
            );
          } else {
            return data;
          }
        },
      },
      {
        data: null,
        render: function (row) {
          return (
            '<a href="#" class="text-primary view-btn fs-4" data-id="' +
            row.quotationCode +
            ' " data-name="' +
            row.projectName +
            '"><i class="fa-solid fa-eye"></i></a> ' +
            ' <a href="#" class="text-success download-btn fs-4" data-id="' +
            row.projectName +
            '"><i class="fa-solid fa-download"></i></a>'
          );
        },
      },
    ],
  });
}

// Attach click event handler to a parent element using event delegation
$("#incomingRFQTable").on("click", ".view-btn", function () {
  let quotationCode = $(this).data("id");
  let url = ViewRFQForm + "?quotationCode=" + quotationCode;
  console.log(url);
  // Redirect to the generated URL
  window.location.href = url;
});

$("#incomingRFQTable").on("click", ".download-btn", function () {
  let projectName = $(this).data("id");
  let url =
    "/Dashboard/DownloadExcelFile?projectName=" +
    encodeURIComponent(projectName);
  console.log(url);
  window.location.href = url;
});

$("#libraryTable").on("click", ".view-btn", function () {
  let quotationCode = $(this).data("name");

  console.log(quotationCode);
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
    "/Dashboard/ViewSourcingForm?pndesc=" + encodeURIComponent(pndesc);
});
