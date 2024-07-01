let table = $("#userTbl").DataTable({
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

// Add User Form
$("#addUserForm").on("submit", function (e) {
  e.preventDefault();

  if (this.checkValidity() === false) {
    e.stopPropagation();
    this.classList.add("was-validated");
    return;
  }
  const name = $("#addNameField").val();
  const email = $("#addEmailField").val();
  const encoder = $("#addEncoder").is(":checked");
  const processor = $("#addProcessor").is(":checked");
  const viewer = $("#addViewer").is(":checked");
  const admin = $("#addAdmin").is(":checked");
  const isActive = $("#addIsActive").is(":checked");
  const department = $("#addDeptField").val();

  const UserData = {
    name,
    email,
    department,
    encoder,
    processor,
    viewer,
    admin,
    isActive,
  };
  console.log(UserData);
  fetch("/Users/UsersAccounts?handler=AddUser", {
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
          $("#addNameField").val("");
          $("#addEmailField").val("");
          $("#addDeptField").val("").prop("selected", false);
          $("#addEncoder").prop("checked", false);
          $("#addProcessor").prop("checked", false);
          $("#addViewer").prop("checked", false);
          $("#addAdmin").prop("checked", false);
          $("#addIsActive").prop("checked", false);
          // Close the modal
          $("#addUserModal").modal("hide");
          $(".modal-backdrop").remove();
          table.ajax.reload();
        });
      } else {
        showErrorAlert(response.message);
      }
    })
    .catch((error) => {
      console.error("Error:", error);
      showErrorAlert("An error occurred while adding the user.");
    });
});

// Edit User Form
$("#userTbl tbody").on("click", ".edit-btn", function (e) {
  e.preventDefault();

  selectedId = $(this).data("id");
  console.log(selectedId);
  let rowData = table.row($(this).closest("tr")).data();

  $("#editNameField").val(rowData.name);
  $("#editEmailField").val(rowData.email);
  $("#editDeptField").val(rowData.department);
  $("#editEncoder").prop("checked", rowData.encoder);
  $("#editProcessor").prop("checked", rowData.processor);
  $("#editViewer").prop("checked", rowData.processor);
  $("#editAdmin").prop("checked", rowData.admin);
  $("#editIsActive").prop("checked", rowData.isActive);
});

// Update User
$("#updateBtn").on("click", function (e) {
  e.preventDefault();

  let Id = selectedId;
  let Name = $("#editNameField").val();
  let Email = $("#editEmailField").val();
  let Department = $("#editDeptField").val();
  let Encoder = $("#editEncoder").prop("checked");
  let Processor = $("#editProcessor").prop("checked");
  let Viewer = $("#editViewer").prop("checked");
  let Admin = $("#editAdmin").prop("checked");
  let IsActive = $("#editIsActive").prop("checked");

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
  console.log(UserData);
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

// Delete User
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
