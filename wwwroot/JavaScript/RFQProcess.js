function getQueryStringParameter(name) {
  var urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
var qC = getQueryStringParameter("quotationCode");
console.log(qC);

$("#SRFQTbl").DataTable({
  responsive: true,
});

function uploadExcel() {
  const excelFile = $("#excelFileInput")[0];

  if (!excelFile.files || !excelFile.files[0]) {
    // Check if a file was selected
    Swal.fire({
      icon: "warning",
      title: "Warning",
      text: "Please select a file to upload.",
      toast: true,
      position: "top-end",
      showConfirmButton: false,
      timer: 3000,
    });
    return;
  } else {
    const file = excelFile.files[0];
    const formData = new FormData(); // Create a FormData object
    formData.append("file", file); // Append the file to FormData

    $.ajax({
      type: "POST",
      url: UploadRFQInfo,
      data: formData,
      processData: false,
      contentType: false,
      success: function (response) {
        if (response.success) {
          // Display success message using SweetAlert
          Swal.fire({
            icon: "success",
            title: "Success",
            text: response.message,
            toast: true,
            position: "top-end",
            timer: 3000,
            showConfirmButton: false,
          });
        } else {
          // Display error message using SweetAlert
          Swal.fire({
            icon: "warning",
            title: "Warning",
            text: response.message,
            toast: true,
            position: "top-end",
          });
        }
      },
      error: function (xhr, status, error) {
        // Display error message using SweetAlert
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "An error occurred while uploading the file.",
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
        });
        console.error("Error:", error);
      },
    });
  }
}
