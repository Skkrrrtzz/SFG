function getQueryStringParameter(name) {
  let urlParams = new URLSearchParams(window.location.search);
  return urlParams.get(name);
}
let qC = getQueryStringParameter("quotationCode");
console.log(qC);

$("#SRFQTbl").DataTable({
  responsive: true,
});
function uploadExcel(file) {
  // $.ajax({
  //   type: "POST",
  //   url: UploadRFQInfo,
  //   data: file,
  //   dataType: "json",
  //   success: function (response) {
  //     console.log(response);
  //     if (response.success) {
  //       // Display success message using SweetAlert
  //       Swal.fire({
  //         icon: "success",
  //         title: "Success",
  //         text: response.message,
  //         toast: true,
  //         position: "top-end",
  //         showConfirmButton: false,
  //         timer: 3000,
  //       });
  //       // window.location.reload();
  //     } else {
  //       // Display error message using SweetAlert
  //       Swal.fire({
  //         icon: "error",
  //         title: "Error",
  //         text: response.message,
  //         toast: true,
  //         position: "top-end",
  //         showConfirmButton: false,
  //         timer: 3000,
  //       });
  //     }
  //   },
  // });

  fetch(UploadRFQInfo, {
    method: "POST",
    body: file,
  })
    .then((response) => response.json())
    .then((response) => {
      console.log(response);
      if (response.success) {
        // Display success message using SweetAlert
        Swal.fire({
          icon: "success",
          title: "Success",
          text: response.message,
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
        });
        // window.location.reload();
      } else {
        // Display error message using SweetAlert
        Swal.fire({
          icon: "error",
          title: "Error",
          text: response.message,
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
        });
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      // Display error message using SweetAlert
      Swal.fire({
        icon: "error",
        title: "Error",
        text: "Failed to upload RFQ info.",
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
      });
    });
}
function checkExcel() {
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

    // $.ajax({
    //   type: "POST",
    //   url: CheckRFQInfo,
    //   data: formData,
    //   processData: false,
    //   contentType: false,
    //   success: function (response) {
    //     console.log(response);
    //     if (response.success) {
    //       // Display success message using SweetAlert
    //       Swal.fire({
    //         icon: "success",
    //         title: "Success",
    //         text: response.message,
    //         toast: true,
    //         position: "top-end",
    //         showCancelButton: true,
    //         confirmButtonColor: "#3085d6",
    //         confirmButtonText: "Submit",
    //         cancelButtonText: "Cancel",
    //       })
    //         .then((result) => {
    //           if (result.isConfirmed) {
    //             Swal.fire({
    //               title: "Loading...",
    //               html: '<div class="m-2" id="loading-spinner"><div class="loader3"><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div></div></div>',
    //               showCancelButton: false,
    //               showConfirmButton: false,
    //               allowOutsideClick: false,
    //               allowEscapeKey: false,
    //             });
    //             uploadExcel(formData);
    //           }
    //         })
    //         .catch((err) => {});
    //     } else {
    //       // Display error message using SweetAlert
    //       Swal.fire({
    //         icon: "warning",
    //         title: "Warning",
    //         text: response.message,
    //         toast: true,
    //         position: "top-end",
    //       });
    //     }
    //   },
    //   error: function (xhr, status, error) {
    //     // Display error message using SweetAlert
    //     Swal.fire({
    //       icon: "error",
    //       title: "Error",
    //       text: "An error occurred while uploading the file.",
    //       toast: true,
    //       position: "top-end",
    //       showConfirmButton: false,
    //       timer: 3000,
    //     });
    //     console.error("Error:", error);
    //   },
    // });
    fetch(CheckRFQInfo, {
      method: "POST",
      body: formData,
      processData: false,
      contentType: false,
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
              Swal.fire({
                title: "Loading...",
                html: '<div class="m-2" id="loading-spinner"><div class="loader3"><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div><div class="circle1"></div></div></div>',
                showCancelButton: false,
                showConfirmButton: false,
                allowOutsideClick: false,
                allowEscapeKey: false,
              });
              uploadExcel(formData);
            }
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
      })
      .catch((error) => {
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
      });
  }
}
