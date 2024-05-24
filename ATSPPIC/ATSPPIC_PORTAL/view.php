<?php include_once 'ATSPPIC_header.php';
require_once 'db.php'; ?>
<!DOCTYPE html>
<html>

<head>
    <title>Upload and View Excel File</title>
    <style>
        .container {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-top: 50px;
        }

        .file-viewer {
            width: 600px;
            height: 400px;
            border: 1px solid #ccc;
            overflow: auto;
            margin-bottom: 20px;
        }
    </style>
</head>

<body>
    <div class="container-fluid">
        <h2>Upload and View</h2>
        <form action="upload.php" method="POST" enctype="multipart/form-data">
            <div class="row">
                <div class="col-sm">
                    <label for="excelFile" class="form-label">Upload Excel File:</label>
                    <input type="file" class="form-control" id="excelFile" name="excelFile" placeholder="Excel" aria-label="Excel">
                </div>
                <div class="col-sm">
                    <label for="imageFile" class="form-label">Upload Image File:</label>
                    <input type="file" class="form-control" id="imageFile" name="imageFile" placeholder="Image" aria-label="Image">
                </div>
                <div class="col-sm d-flex justify-content-start p-3">
                    <button type="submit" class="btn btn-primary btn-sm">Upload</button>
                </div>
            </div>
        </form>
    </div>

    <div class="container-fluid">
        <h2>Data Files</h2>
        <div class="table-responsive">
            <table id="dataFilesTable" class="table table-hover compact display table-responsive-sm">
                <thead class="table-info">
                    <tr>
                        <th>File ID</th>
                        <th>Employee ID</th>
                        <th>File Name</th>
                        <th>File Size</th>
                        <th>Image</th>
                        <th>Image Size</th>
                        <th>Date updated</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>
    <div class="modal fade" id="imagePreviewModal" tabindex="-1" aria-labelledby="imagePreviewModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="imagePreviewModalLabel">Image View</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <img id="imagePreview" src="" alt="Uploaded Image" class="img-fluid">
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function() {
            // Initialize DataTables and store it in the dataTable variable
            var dataTable = $('#dataFilesTable').DataTable({
                ajax: {
                    url: 'upload.php',
                    method: 'GET',
                    dataSrc: ''
                },
                deferRender: true,
                fixedColumns: true,
                columns: [{
                        data: null,
                        targets: 0,
                        orderable: false,
                        render: function(data, type, row) {
                            return '<input type="checkbox" class="select-checkbox" data-id="' + row.ID + '">';
                        }
                    },
                    {
                        data: 'ID'
                    },
                    {
                        data: 'emp_id'
                    },
                    {
                        data: 'file_name'
                    },
                    {
                        data: 'file_size'
                    },
                    {
                        data: 'image_name'
                    },
                    {
                        data: 'image_size'
                    },
                    {
                        data: 'file_saved_date',
                        render: function(data, type, row) {
                            return '<span style="font-weight: bold;">' + data + '</span>';
                        }
                    },
                    {
                        data: null,
                        orderable: false,
                        render: function(data, type, row) {
                            return '<button type="button" class="btn btn-primary mx-1 view-btn" data-id="' + row.ID + '" data-bs-toggle="modal" data-bs-target="#imagePreviewModal"><i class="fa-regular fa-eye"></i></button>' +
                                '<button class="btn btn-danger delete-btn" data-id="' + row.ID + '"><i class="fa-solid fa-trash"></i></button>';
                        }
                    }
                ]
            });

            // Handle image preview
            $('#dataFilesTable tbody').on('click', '.view-btn', function() {
                var dataId = $(this).data('id');
                var imageUrl = 'files_data/images/' + dataId;
                // Show the image in a custom modal-like popup
                $('#imagePreview').attr('src', imageUrl);
                $('#imagePreviewModal').modal('show');
            });

            $(document).ready(function() {
                $('.delete-btn').on('click', function(e) {
                    e.preventDefault();
                    var dataId = $(this).data('id');
                    console.log(dataId);
                    // Display confirmation dialog using SweetAlert2
                    Swal.fire({
                        icon: 'warning',
                        title: 'Are you sure?',
                        text: 'You are about to delete this row: ' + dataId,
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Delete',
                        cancelButtonText: 'Cancel'
                    }).then(function(result) {
                        if (result.isConfirmed) {
                            // Make an AJAX request to delete the row
                            $.ajax({
                                url: 'upload.php',
                                method: 'POST',
                                data: {
                                    id: dataId,
                                    delete: true
                                },
                                success: function(response) {
                                    // Handle success and error responses
                                    console.log(response); // Check the value of the response
                                    // Show success message using SweetAlert2
                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Delete Successful',
                                        text: 'The data has been deleted successfully!',
                                        showConfirmButton: false,
                                        timer: 1000
                                    }).then(function() {
                                        dataTable.reload(); // Reload the DataTable
                                    });
                                },
                                error: function() {
                                    // Handle the case when the AJAX request encounters an error
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Delete Error!',
                                        text: 'An error occurred during deletion. Please try again.',
                                    });
                                }
                            });
                        }
                    });
                });
            });
        });
    </script>
</body>

</html>