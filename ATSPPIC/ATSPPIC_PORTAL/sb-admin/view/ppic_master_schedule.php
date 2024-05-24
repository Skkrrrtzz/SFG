<?php require_once 'ppic_nav.php';
include_once '../controller/commands.php';
?>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Master Schedule</title>
    <script src="../js/sweetalert2.all.min.js"></script>
    <style>
        .bg-blue-60 {
            background-color: #b8c4e4;
        }

        .bg-blue-80 {
            background-color: #e0e4f4;
        }

        .bg-gray-60 {
            background-color: #e0dcdc;
        }

        .bg-gray-80 {
            background-color: #f0ecec;
        }

        .bg-pink {
            background-color: #ffcccc;
        }
    </style>
</head>

<body id="master_schedule">
    <div class="container-fluid">
        <!-- <div class="card shadow my-2"> -->
        <div class=" d-flex justify-content-between align-items-center">
            <h4 class="m-0 font-weight-bold text-primary">
                Master Schedule
            </h4>
            <!-- Buttons -->
            <div class="my-0">
                <!-- Edit Button -->
                <button type="button" class="btn btn-sm btn-success edit-all-button" data-toggle="modal" data-target="#masterSchedModal">Add Master Schedule</button>
                <!-- Upload Button -->
                <input type="file" class="form-control-file d-none" id="pdfUpload" accept=".pdf">
                <label for="pdfUpload" class="btn btn-primary btn-sm mt-2" id="uploadButton">Upload PDF</label>
                <!-- View Button -->
                <button type="button" class="btn btn-primary btn-sm" id="viewPdfButton">View PDF</button>
            </div>
        </div>
        <div id="message"></div>
        <div class="m-2" style="height: 600px; overflow-y: auto;">
            <div class=" table-responsive">
                <table class=" table-bordered text-center" id="tableContainer">
                    <thead class="table-dark text-nowrap">
                        <tr>
                            <th rowspan="4">Product</th>
                            <th>Month</th>
                            <td colspan="4" class="bg-blue-80 text-dark" name="monthnow"><?= $currentMonthName; ?></td>
                            <td colspan="4" class="bg-blue-80 text-dark" name="monthnxt"><?= $nextMonthName; ?></td>
                        </tr>
                        <tr>
                            <th>Week</th>
                            <?php foreach ($weekNumbers as $week) : ?>
                                <?php
                                $sunday = $dateRanges[$week]['Sunday'];
                                $isCurrentMonth = (date('F', strtotime($sunday)) === $currentMonthName);
                                $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                ?>
                                <td class="<?= $class; ?> text-dark">
                                    Week <?= $week; ?>
                                </td>
                            <?php endforeach; ?>
                        </tr>
                        <tr>
                            <th>Start Build Plan</th>
                            <?php foreach ($weekNumbers as $week) : ?>
                                <?php
                                $sunday = $dateRanges[$week]['Sunday'];
                                $isCurrentMonth = (date('F', strtotime($sunday)) === $currentMonthName);
                                $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                ?>
                                <td class="<?= $class; ?> text-dark"><?= $sunday; ?>
                                </td>
                            <?php endforeach; ?>
                        </tr>
                        <tr>
                            <th>End Build Date</th>
                            <?php foreach ($weekNumbers as $week) : ?>
                                <?php
                                $saturday = $dateRanges[$week]['Saturday'];
                                $isCurrentMonth = (date('F', strtotime($saturday)) === $currentMonthName);
                                $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                ?>
                                <td class="<?= $class; ?> text-dark"><?= $saturday; ?>
                                </td>
                            <?php endforeach; ?>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
        <!-- </div> -->
    </div>
    <!-- Modal for Viewing PDF -->
    <div class="modal fade" id="pdfModal" tabindex="-1" role="dialog" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen">
            <div class="modal-content">
                <div class="modal-header bg-gray-300 my-0">
                    <h5 class="modal-title fw-bold" id="pdfModalLabel">PDF Viewer</h5>

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <h6 class="" id="uploaded_by"></h6>
                    <iframe id="pdfViewer" src="" frameborder="0" style="width: 100%; height: 100%;"></iframe>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal for editing Forecast -->
    <div class="modal fade" id="masterSchedModal" tabindex="-1" aria-labelledby="masterSchedLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-light">
                    <h5 class="modal-title text-success" id="masterSchedLabel">Add Master Schedule</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <label class="input-group-text" for="productSelect">Products</label>
                            </div>
                            <select class="custom-select" id="productSelect">
                                <option selected>Choose...</option>
                                <?php foreach ($products as $product) { ?>
                                    <option><?php echo $product; ?></option>
                                <?php } ?>
                            </select>
                        </div>
                    </div>
                    <div id="messageContainer"></div>
                    <div class="table-responsive" id="productTable" style="display: none;">
                        <form id="uploadForm" method="post">
                            <input type="hidden" name="emp_name" id="emp_name" value="<?php echo $emp_name; ?>">
                            <input type="hidden" name="product" id="product" value="<?php echo $product; ?>">
                            <input type="hidden" name="curmonth" id="curmonth" value="<?php echo $currentMonthName; ?>">
                            <input type="hidden" name="monthnxt" id="monthnxt" value="<?php echo $nextMonthName; ?>">
                            <table class=" table-bordered text-center">
                                <thead class="table-dark">
                                    <tr>
                                        <th>Month</th>
                                        <td colspan="4" class="bg-blue-80 text-dark" name="monthnow"><?= $currentMonthName; ?></td>
                                        <td colspan="4" class="bg-blue-80 text-dark" name="monthnxt"><?= $nextMonthName; ?></td>
                                    </tr>
                                    <tr>
                                        <th>Week</th>
                                        <?php foreach ($weekNumbers as $week) : ?>
                                            <?php
                                            $sunday = $dateRanges[$week]['Sunday'];
                                            $isCurrentMonth = (date('F', strtotime($sunday)) === $currentMonthName);
                                            $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                            ?>
                                            <td class="<?= $class; ?> text-dark"> Week <?= $week; ?>
                                                <input type="hidden" name="week[]" value="<?php echo $week; ?>">
                                            </td>
                                        <?php endforeach; ?>
                                    </tr>

                                    <tr>
                                        <th>Start Build Plan</th>
                                        <?php foreach ($weekNumbers as $week) : ?>
                                            <?php
                                            $sunday = $dateRanges[$week]['Sunday'];
                                            $isCurrentMonth = (date('F', strtotime($sunday)) === $currentMonthName);
                                            $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                            ?>
                                            <td class="<?= $class; ?> text-dark"><?= $sunday; ?>
                                                <input type="hidden" name="wkstart[]" value="<?php echo $sunday; ?>">
                                            </td>
                                        <?php endforeach; ?>
                                    </tr>
                                    <tr>
                                        <th>End Build Date</th>
                                        <?php foreach ($weekNumbers as $week) : ?>
                                            <?php
                                            $saturday = $dateRanges[$week]['Saturday'];
                                            $isCurrentMonth = (date('F', strtotime($saturday)) === $currentMonthName);
                                            $class = $isCurrentMonth ? 'bg-blue-80' : 'bg-gray-60';
                                            ?>
                                            <td class="<?= $class; ?> text-dark"><?= $saturday; ?>
                                                <input type="hidden" name="wkend[]" value="<?php echo $saturday; ?>">
                                            </td>
                                        <?php endforeach; ?>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th>Prod Build Qty</th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="prod_build_qty">
                                                <input type="number" class="w-75" name="prod_build_qty[]" value="">
                                            </td>
                                        <?php endfor; ?>
                                    </tr>
                                    <tr>
                                        <th id="productHeader"></th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="product_no">
                                                <?php if ($i === 0) { ?>
                                                    <input type="number" class="w-75" name="product_no[]" id="productNoInput" value="" required>
                                                <?php } ?>
                                            </td>
                                        <?php endfor; ?>
                                    </tr>
                                    <tr>
                                        <th>Shipment Qty</th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="ship_qty">
                                                <input type="number" class="w-75" name="ship_qty[]" value="">
                                            </td>
                                        <?php endfor; ?>
                                    </tr>
                                    <tr>
                                        <th>BOH/EOH</th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="boh_eoh">
                                                <?php if ($i === 0) { ?>
                                                    <input type="number" class="w-75" name="boh_eoh[]" value="">
                                                <?php } ?>
                                            </td>
                                        <?php endfor; ?>
                                    </tr>
                                    <tr>
                                        <th>Actual Batch Output</th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="act_batch_output"></td>
                                        <?php endfor; ?>
                                    </tr>
                                    <tr>
                                        <th>Delay</th>
                                        <?php for ($i = 0; $i < $saturdaysCount; $i++) : ?>
                                            <td name="delay"></td>
                                        <?php endfor; ?>
                                    </tr>
                                </tbody>
                            </table>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="ajaxSubmit">Submit</button>
                </div>
                </form>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function() {
            // Initial check when the page is loaded
            checkProductNoInput();

            // Add an input event listener to #productNoInput
            $("#productNoInput").on("input", function() {
                // Call the function to check the value and update the button visibility
                checkProductNoInput();
            });

            // Function to check the value of #productNoInput and update button visibility
            function checkProductNoInput() {
                var productNoInputValue = $("#productNoInput").val();

                if (productNoInputValue === '') {
                    $("#ajaxSubmit").hide();
                } else {
                    $("#ajaxSubmit").show();
                }
            }
            // Handle form submission using AJAX
            $("#ajaxSubmit").click(function() {
                Swal.fire({
                    title: "Are you sure?",
                    text: "You are about to submit the form.",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Submit",
                    cancelButtonText: "Cancel",
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Get the input element
                        var productNoInput = $("#productNoInput");

                        // Check if the input has a value
                        if (productNoInput.value === "") {
                            Swal.fire({
                                icon: "error",
                                title: "Field is blank.",
                                text: "Please fill out the fields.",
                            });
                        } else {
                            // Serialize form data
                            var formData = $("#uploadForm").serialize() + "&add=true";

                            // Make an AJAX POST request
                            $.ajax({
                                type: "POST",
                                url: "../controller/upload_data.php",
                                data: formData,
                                success: function(response) {
                                    $("#messageContainer").html(response);
                                    // Clear form fields
                                    $("#uploadForm")[0].reset();
                                },
                                error: function(xhr, status, error) {
                                    // Handle errors (if any)
                                    console.error(xhr.responseText);
                                }
                            });
                        }
                    }
                });
            });
        });
        $(document).ready(function() {
            function refreshData() {
                $.ajax({
                    url: '../controller/refresh_mstable.php',
                    dataType: 'html', // Expect HTML data
                    success: function(data) {
                        // Replace the table content with the updated data
                        $('#tableContainer tbody').html(data);
                    },
                    error: function() {
                        // Handle errors if the request fails
                        console.log('Error fetching data.');
                    }
                });
            }

            // Refresh data every 5 seconds
            setInterval(refreshData, 5000);

            // Call the function to refresh data when the page loads
            refreshData();
        });
        document.addEventListener("DOMContentLoaded", function() {
            const productSelect = document.getElementById("productSelect");
            const productTable = document.getElementById("productTable");
            const productHeader = document.getElementById("productHeader");

            productSelect.addEventListener("change", function() {
                const selectedProduct = productSelect.value;

                // Set the selected product as a variable
                document.getElementById("product").value = selectedProduct;

                if (selectedProduct === "Choose...") {
                    productTable.style.display = "none";
                } else {
                    productTable.style.display = "block";

                    // Update the table header text with the selected product
                    productHeader.textContent = selectedProduct + " No.";
                }
            });
        });
    </script>
    <script>
        // Javascript for uploading PDF file and Viewing uploaded PDF
        $(document).ready(function() {
            $("#pdfUpload").click(function() {
                $("#pdfUpload").change(function() {
                    var fileInput = document.getElementById('pdfUpload');
                    var file = fileInput.files[0];
                    var empName = '<?php echo $emp_name; ?>';

                    if (file) {
                        var formData = new FormData();
                        formData.append("pdfFile", file);
                        formData.append("empName", empName);

                        $.ajax({
                            type: "POST",
                            url: "../controller/upload_file.php",
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function(response) {
                                $("#message").html(response);
                            }
                        });
                    }
                });
            });

            // Attach a click event handler to the "View PDF" button
            $("#viewPdfButton").click(function() {

                $.ajax({
                    type: "POST",
                    url: "../controller/upload_file.php",
                    data: {
                        empName: '<?php echo $emp_name; ?>',
                        upload: true
                    }, // Pass the parameter as an object
                    success: function(response) {
                        try {
                            // Parse the JSON response
                            var responseData = JSON.parse(response);

                            // Check if responseData has the file_loc property
                            if (responseData.file_loc) {
                                // Combine "Uploaded by:" and the value of uploaded_by
                                var uploadedByText = "Uploaded by: " + responseData.uploaded_by + " - Date: " + responseData.uploaded_date;
                                // Set the text content of the HTML element
                                $("#uploaded_by").text(uploadedByText);
                                // Set the src attribute of the iframe in the modal
                                $("#pdfViewer").attr("src", responseData.file_loc);
                                // Show the modal
                                $("#pdfModal").modal("show");
                            } else {
                                // Handle the case where the PDF URL is not found
                                Swal.fire({
                                    icon: 'error',
                                    title: 'PDF URL not found in the database.'
                                });
                            }
                        } catch (error) {
                            // Handle JSON parsing errors or other issues
                            // console.error(error);
                            Swal.fire({
                                icon: 'error',
                                title: 'No PDF uploaded'
                            });
                        }
                    },
                    error: function() {
                        // Handle AJAX error, if any
                        Swal.fire({
                            icon: 'error',
                            title: 'Error fetching PDF URL from the database.',
                            text: 'Please contact the admin, thanks!',
                        });
                    }
                });
            });
        });
    </script>
</body>

</html>