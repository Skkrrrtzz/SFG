//Initialize DataTable with AJAX
var table = $("#userTbl").DataTable({
  scrollX: true,
  scrollY: "50vh",
  responsive: true,
  ajax: function (data, callback, settings) {
    fetch("/Users/UsersAccounts?handler=DisplayUsers", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        callback({ data: data.data }); // Assuming the server returns { data: [...] }
      })
      .catch((error) => {
        console.error("Error fetching data:", error);
      });
  },
  columns: [
    { data: "id" },
    { data: "name" },
    { data: "email" },
    { data: "department" },
    {
      data: "encoder",
      render: function (data, type, row) {
        let icon = row.encoder
          ? '<i class="fa-solid fa-circle-check text-success fs-5"></i>'
          : '<i class="fa-solid fa-circle-xmark text-danger fs-5"></i>';
        return icon;
      },
    },
    {
      data: "processor",
      render: function (data, type, row) {
        let icon = row.processor
          ? '<i class="fa-solid fa-circle-check text-success fs-5"></i>'
          : '<i class="fa-solid fa-circle-xmark text-danger fs-5"></i>';
        return icon;
      },
    },
    {
      data: "viewer",
      render: function (data, type, row) {
        let icon = row.viewer
          ? '<i class="fa-solid fa-circle-check text-success fs-5"></i>'
          : '<i class="fa-solid fa-circle-xmark text-danger fs-5"></i>';
        return icon;
      },
    },
    {
      data: "admin",
      render: function (data, type, row) {
        let icon = row.admin
          ? '<i class="fa-solid fa-circle-check text-success fs-5"></i>'
          : '<i class="fa-solid fa-circle-xmark text-danger fs-5"></i>';
        return icon;
      },
    },
    {
      data: "isActive",
      render: function (data, type, row) {
        let status = row.isActive ? "Active" : "InActive";
        let badgeClass = row.isActive ? "bg-success" : "bg-danger";
        return (
          '<span class="badge ' +
          badgeClass +
          ' rounded-pill">' +
          status +
          "</span>"
        );
      },
    },
    {
      data: null,
      render: function (data, type, row) {
        return (
          '<button type="button" class="btn btn-sm bg-main3 edit-btn me-2" data-id="' +
          row.id +
          '" data-bs-toggle="modal" data-bs-target="#editUserModal"><i class="fa-solid fa-pen-to-square fs-5"></i></button>' +
          '<button type="button" class="btn btn-sm bg-main3 delete-btn me-2" data-id="' +
          row.id +
          '"><i class="fa-solid fa-trash fs-5"></i></button>'
        );
      },
    },
  ],
});

//ADD BUTTON
// $("#addBtn").on("click", function (e) {
//   e.preventDefault(); // Prevent default form submission

//   // Retrieve the values from the modal inputs
//   var name = $("#addUserField").val();
//   var email = $("#addEmailField").val();
//   var emp_id = $("#addEmpField").val();
//   var pass = $("#addPassField").val();
//   var dept = $("#addDeptField").val();
//   var role = $("#addRoleField").val();
//   var pos = $("#addPosField").val();
//   // Hide the alert by default
//   $("#alert").addClass("d-none");

//   // Check if any required field is empty
//   if (
//     name === "" ||
//     email === "" ||
//     emp_id === "" ||
//     pass === "" ||
//     role === ""
//   ) {
//     // Display error message for empty fields
//     $("#alert-text").text("Please fill in all required fields.");
//     $("#alert").removeClass("d-none").show();
//     return;
//   }
//   // Check the password length
//   // if (pass.length < 8) {
//   //     // Display an error message for a password that is less than 8 characters long
//   //     $('#alert-text').text('Password must be at least 8 characters long.');
//   //     $('#alert').removeClass('d-none').show();
//   //     return;
//   // }
//   // Hide the alert message if all fields are filled
//   $("#alert").addClass("d-none");

//   // Perform an AJAX request to insert the data into the database
//   $.ajax({
//     url: AddUser,
//     method: "POST",
//     data: {
//       name: name,
//       email: email,
//       userName: emp_id,
//       password: pass,
//       department: dept,
//       role: role,
//       position: pos,
//     },
//     success: function (response) {
//       // Handle the success response

//       if (response.success) {
//         // Check the 'success' property in the response
//         // Show success message using SweetAlert2
//         showSuccessAlert("The data has been added successfully!").then(
//           function () {
//             // Reset the modal inputs
//             $("#addUserField").val("");
//             $("#addEmailField").val("");
//             $("#addEmpField").val("");
//             $("#addPassField").val("");
//             $("#addDeptField").val("");
//             $("#addRoleField").val("");
//             $("#addPosField").val("");
//             // Close the modal
//             $("#addUserModal").modal("hide");
//             $(".modal-backdrop").remove();
//             table.ajax.reload(); // Reload the DataTable
//           }
//         );
//       } else {
//         // Handle the case when the update failed
//         showErrorAlert("Failed to add data");
//         // Swal.fire({
//         //   icon: "error",
//         //   title: "Failed",
//         //   text: response.message,
//         //   showConfirmButton: true,
//         // });
//       }
//     },
//     error: function () {
//       // Handle the error case
//       console.log("Error occurred during data insertion");
//     },
//   });
// });

// EDIT BUTTON
$("#userTbl tbody").on("click", ".edit-btn", function (e) {
  e.preventDefault();

  let selectedId = $(this).data("id");
  let rowData = table.row($(this).closest("tr")).data();

  $("#editNameField").val(rowData.name);
  $("#editEmailField").val(rowData.email);
  $("#editDeptField").val(rowData.department);
  $("#editEncoder").prop("checked", rowData.encoder);
  $("#editProcessor").prop("checked", rowData.processor);
  $("#editViewer").prop("checked", rowData.processor);
  $("#editAdmin").prop("checked", rowData.admin);
  $("#editIsActive").prop("checked", rowData.isActive);
  // UPDATE BUTTON
  $("#updateBtn").on("click", function (e) {
    e.preventDefault();

    let Id = selectedId;
    let Name = $("#editNameField").val();
    let Email = $("#editEmailField").val();
    let Department = $("#editDeptField").val();
    let Encoder = $("#editEncoder").prop("checked") || null;
    let Processor = $("#editProcessor").prop("checked") || null;
    let Viewer = $("#editViewer").prop("checked") || null;
    let Admin = $("#editAdmin").prop("checked") || null;
    let IsActive = $("#editIsActive").prop("checked") || null;

    const UserData = {
      Id,
      Name,
      Email,
      Department,
      Encoder,
      Processor,
      Viewer,
      Admin,
      IsActive,
    };

    fetch("/Users/UsersAccounts?handler=EditUser", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        RequestVerificationToken: $(
          'input:hidden[name="__RequestVerificationToken"]'
        ).val(),
      },
      body: JSON.stringify(UserData),
    })
      .then((response) => response.json())
      .then((response) => {
        if (response.success) {
          showSuccessAlert(response.message).then(function () {
            $("#editNameField").val("");
            $("#editEmailField").val("");
            $("#editDeptField").val("");
            // Close the modal
            $("#editUserModal").modal("hide");
            $(".modal-backdrop").remove();
            table.ajax.reload();
          });
        } else {
          showErrorAlert(response.message);
        }
      })
      .catch((error) => {
        console.error("Error:", error);
        showErrorAlert("An error occurred while updating the user.");
      });
  });
});

// DELETE BUTTON
$("#userTbl tbody").on("click", ".delete-btn", function (e) {
  e.preventDefault();

  let UserId = $(this).data("id");

  showConfirmButton("Are you sure you want to delete this user?", "info").then(
    (result) => {
      if (result.isConfirmed) {
        fetch("/Users/UsersAccounts?handler=DeleteUser", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            RequestVerificationToken: $(
              'input[name="__RequestVerificationToken"]'
            ).val(),
          },
          body: JSON.stringify(UserId),
        })
          .then((response) => {
            if (!response.ok) {
              throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
          })
          .then((response) => {
            if (response.success) {
              showSuccessAlert(response.message).then(function () {
                table.ajax.reload();
              });
            } else {
              showErrorAlert(response.message);
            }
          })
          .catch((error) => {
            console.error("Error:", error);
            showErrorAlert("An error occurred while deleting the user.");
          });
      }
    }
  );
});

// // Toggles password visibility
// $("#toggleAddPassword").click(function () {
//   // Toggle the password visibility
//   let passwordAddField = $("#addPassField");
//   let iconAdd = $(this).find("i");

//   if (passwordAddField.attr("type") === "password") {
//     passwordAddField.attr("type", "text");
//     iconAdd.removeClass("bi bi-eye-slash-fill").addClass("bi bi-eye-fill");
//   } else {
//     passwordAddField.attr("type", "password");
//     iconAdd.removeClass("bi bi-eye-fill").addClass("bi bi-eye-slash-fill");
//   }
// });

// $("#toggleEditPassword").click(function () {
//   let passwordEditField = $("#editPassField");
//   let iconEdit = $(this).find("i");

//   if (passwordEditField.attr("type") === "password") {
//     passwordEditField.attr("type", "text");
//     iconEdit.removeClass("bi bi-eye-slash-fill").addClass("bi bi-eye-fill");
//   } else {
//     passwordEditField.attr("type", "password");
//     iconEdit.removeClass("bi bi-eye-fill").addClass("bi bi-eye-slash-fill");
//   }
// });
