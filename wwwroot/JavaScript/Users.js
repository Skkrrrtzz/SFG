//Initialize DataTable with AJAX
var table = $("#userTbl").DataTable({
  scrollX: true,
  scrollY: "50vh",
  responsive: true,
  ajax: function (data, callback, settings) {
    fetch(GetUsers, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    })
      .then((response) => response.json())
      .then((data) => {
        callback({
          data: data.data,
        });
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
      data: "role",
      render: function (data, type, row) {
        if (data == "Encoder") {
          return (
            '<span class="badge rounded-pill text-bg-success">' +
            data +
            "</span>"
          );
        } else if (data == "Processor") {
          return (
            '<span class="badge rounded-pill text-bg-dark">' + data + "</span>"
          );
        } else if (data == "Viewer") {
          return (
            '<span class="badge rounded-pill text-bg-dark">' + data + "</span>"
          );
        } else {
          return (
            '<span class="badge rounded-pill text-bg-primary">' +
            data +
            "</span>"
          );
        }
        return data;
      },
    },
    {
      data: null,
      render: function (data, type, row) {
        return (
          '<a href="#" class="text-primary edit-btn fs-4" data-id="' +
          row.AccountID +
          '" data-bs-toggle="modal" data-bs-target="#editUserModal" ><i class="bi bi-pencil-square"></i></a>' +
          ' <a href="#" class="text-danger delete-btn fs-4" data-id="' +
          row.AccountID +
          '"><i class="bi bi-trash3-fill"></i></a>'
        );
      },
    },
  ],
});

//ADD BUTTON
$("#addBtn").on("click", function (e) {
  e.preventDefault(); // Prevent default form submission

  // Retrieve the values from the modal inputs
  var name = $("#addUserField").val();
  var email = $("#addEmailField").val();
  var emp_id = $("#addEmpField").val();
  var pass = $("#addPassField").val();
  var dept = $("#addDeptField").val();
  var role = $("#addRoleField").val();
  var pos = $("#addPosField").val();
  // Hide the alert by default
  $("#alert").addClass("d-none");

  // Check if any required field is empty
  if (
    name === "" ||
    email === "" ||
    emp_id === "" ||
    pass === "" ||
    role === ""
  ) {
    // Display error message for empty fields
    $("#alert-text").text("Please fill in all required fields.");
    $("#alert").removeClass("d-none").show();
    return;
  }
  // Check the password length
  // if (pass.length < 8) {
  //     // Display an error message for a password that is less than 8 characters long
  //     $('#alert-text').text('Password must be at least 8 characters long.');
  //     $('#alert').removeClass('d-none').show();
  //     return;
  // }
  // Hide the alert message if all fields are filled
  $("#alert").addClass("d-none");

  // Perform an AJAX request to insert the data into the database
  $.ajax({
    url: AddUser,
    method: "POST",
    data: {
      name: name,
      email: email,
      userName: emp_id,
      password: pass,
      department: dept,
      role: role,
      position: pos,
    },
    success: function (response) {
      // Handle the success response

      if (response.success) {
        // Check the 'success' property in the response
        // Show success message using SweetAlert2
        Swal.fire({
          icon: "success",
          title: "Added Successful",
          text: "The data has been added successfully!",
          showConfirmButton: false,
          timer: 1000,
        }).then(function () {
          // Reset the modal inputs
          $("#addUserField").val("");
          $("#addEmailField").val("");
          $("#addEmpField").val("");
          $("#addPassField").val("");
          $("#addDeptField").val("");
          $("#addRoleField").val("");
          $("#addPosField").val("");
          // Close the modal
          $("#addUserModal").modal("hide");
          $(".modal-backdrop").remove();
          table.ajax.reload(); // Reload the DataTable
        });
      } else {
        // Handle the case when the update failed
        Swal.fire({
          icon: "error",
          title: "Failed",
          text: response.message,
          showConfirmButton: true,
        });
      }
    },
    error: function () {
      // Handle the error case
      console.log("Error occurred during data insertion");
    },
  });
});

// EDIT BUTTON
$("#userTbl tbody").on("click", ".edit-btn", function (e) {
  e.preventDefault();

  selectedId = $(this).data("id");

  var rowData = table.row($(this).closest("tr")).data();

  $("#editNameField").val(rowData.name);
  $("#editEmailField").val(rowData.email);
  $("#editUserNameField").val(rowData.userName);
  $("#editPassField").val(rowData.password);
  $("#editDeptField").val(rowData.department);
  $("#editRoleField").val(rowData.role);
  $("#editPosField").val(rowData.position);
});

// UPDATE BUTTON
$("#updateBtn").on("click", function (e) {
  e.preventDefault(); // Prevent default form submission

  var id = selectedId;
  var updateName = $("#editNameField").val();
  var updateEMP = $("#editUserNameField").val();
  var updateEmail = $("#editEmailField").val();
  var updatePass = $("#editPassField").val();
  var updateDept = $("#editDeptField").val();
  var updateRole = $("#editRoleField").val();
  var updatePosition = $("#editPosField").val();
  // Make an AJAX request to update the values in the database
  $.ajax({
    url: EditUser,
    method: "POST",
    data: {
      id: id,
      name: updateName,
      username: updateEMP,
      email: updateEmail,
      password: updatePass,
      department: updateDept,
      role: updateRole,
      position: updatePosition,
    },
    success: function (response) {
      if (response.success) {
        Swal.fire({
          title: "Message",
          text: response.message,
          icon: "success",
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
        });

        // Reset the input fields
        $("#editNameField").val("");
        $("#editUserNameField").val("");
        $("#editEmailField").val("");
        $("#editPassField").val("");
        $("#editDeptField").val("");
        $("#editRoleField").val("");
        $("#editPosField").val("");
        // Close the modal
        $("#editUserModal").modal("hide");
        $(".modal-backdrop").remove();
        table.ajax.reload();
      } else {
        // Display an error message
        Swal.fire("Error", response.message, "error");
      }
    },
  });
});

// DELETE BUTTON
$("#userTbl tbody").on("click", ".delete-btn", function (e) {
  e.preventDefault();

  var userId = $(this).data("id");

  // Show a confirmation dialog
  Swal.fire({
    title: "Are you sure?",
    text: "You will not be able to recover this user!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    confirmButtonText: "Confirm",
    cancelButtonText: "Cancel",
  }).then((result) => {
    if (result.isConfirmed) {
      // Perform the delete operation using AJAX
      $.ajax({
        url: DeleteUser,
        type: "POST",
        data: { Id: userId },
        success: function (response) {
          if (response.success) {
            Swal.fire({
              title: "Message",
              text: `${response.message}`,
              icon: "success",
              toast: true,
              position: "top-end",
              showConfirmButton: false,
              timer: 5000,
            });
            // Reload the DataTable
            table.ajax.reload();
          } else {
            // Display an error message
            Swal.fire(
              "Error",
              `${response.message} User ID: ${response.userid}`,
              "error"
            );
          }
        },
        error: function () {
          // Display a generic error message
          Swal.fire(
            "Error",
            "An error occurred while deleting the user.",
            "error"
          );
        },
      });
    }
  });
});

// Toggles password visibility
$("#toggleAddPassword").click(function () {
  // Toggle the password visibility
  let passwordAddField = $("#addPassField");
  let iconAdd = $(this).find("i");

  if (passwordAddField.attr("type") === "password") {
    passwordAddField.attr("type", "text");
    iconAdd.removeClass("bi bi-eye-slash-fill").addClass("bi bi-eye-fill");
  } else {
    passwordAddField.attr("type", "password");
    iconAdd.removeClass("bi bi-eye-fill").addClass("bi bi-eye-slash-fill");
  }
});

$("#toggleEditPassword").click(function () {
  let passwordEditField = $("#editPassField");
  let iconEdit = $(this).find("i");

  if (passwordEditField.attr("type") === "password") {
    passwordEditField.attr("type", "text");
    iconEdit.removeClass("bi bi-eye-slash-fill").addClass("bi bi-eye-fill");
  } else {
    passwordEditField.attr("type", "password");
    iconEdit.removeClass("bi bi-eye-fill").addClass("bi bi-eye-slash-fill");
  }
});
