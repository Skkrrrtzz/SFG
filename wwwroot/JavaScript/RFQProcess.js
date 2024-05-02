function getQueryStringParameter(name) {
  var urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
var qC = getQueryStringParameter("quotationCode");
console.log(qC);

$("#SRFQTbl").DataTable({
  responsive: true,
});

$("#uploadBtn").on("click", function () {
  var excelFile = $("#excelFileInput")[0].files[0];
  // Check if a file is selected
  if (!excelFile) {
    alert("Please select a file.");
    return;
  }

  var formData = new FormData();
  formData.append("file", excelFile);

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
      console.error("Error:", error);
    },
  });
});
