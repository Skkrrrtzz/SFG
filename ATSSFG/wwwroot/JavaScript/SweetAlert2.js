function showSuccessAlert(message) {
  return Swal.fire({
    icon: "success",
    title: "Success",
    text: message,
    toast: true,
    position: "top-end",
    timer: 3000,
    showConfirmButton: false,
    didOpen: (toast) => {
      toast.onmouseenter = Swal.stopTimer;
      toast.onmouseleave = Swal.resumeTimer;
    },
  });
}

function showErrorAlert(message) {
  Swal.fire({
    icon: "error",
    title: "Error",
    text: message,
    toast: true,
    position: "top-end",
    timer: 3000,
    showConfirmButton: false,
  });
}

function showWarningAlert(message) {
  return Swal.fire({
    icon: "warning",
    title: "Warning",
    text: message,
    toast: true,
    position: "top-end",
    timer: 3000,
    showConfirmButton: false,
  });
}

function showInfoAlert(message) {
  Swal.fire({
    icon: "info",
    title: "Info",
    text: message,
    toast: true,
    position: "top-end",
    timer: 3000,
    showConfirmButton: false,
  });
}

function showLoading() {
  Swal.fire({
    title: "Loading...",
    timerProgressBar: true,
    allowOutsideClick: false,
    allowEscapeKey: false,
    didOpen: () => {
      Swal.showLoading();
    },
  });
}
function showAlertMiddle(message) {
  Swal.fire({
    icon: "info",
    title: message,
    toast: true,
    position: "center",
    timer: 3000,
    showConfirmButton: false,
  });
}
function showToastwithTimer() {
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
    title: "Please enter a valid input quantity in the Annual Forecast field.",
  });
}
