
$(document).ready(function() {
            //     // Disable the save button initially
            $("#saveChangesBtn").prop("disabled", true);

            //     // Create an array to store the edited data
            let editedData = [];
            let curMonth = "<?php echo $currentMonthName; ?>";
            let nextMonth = "<?php echo $nextMonthName; ?>";
            let wwk = "<?php foreach ($weekNumbers as $week) {

                            echo $sunday = $dateRanges[$week]['Sunday'];
                            echo $saturday = $dateRanges[$week]['Saturday'];
                        }
                        ?>";
            //     // Add event listener for detecting changes in the table cells
            $("tbody td").on("input", function() {
                // Enable the save button when any cell is edited
                $("#saveChangesBtn").prop("disabled", false);

                // Get the edited cell's content
                let editedCellValue = $(this).text();

                // Get the ID attribute of the edited cell (which corresponds to the product name)
                let productId = $(this).attr("id");

                // Check if the edited cell's value is within the current month, week, and date range
                if (isWithinDateRange(editedCellValue, curMonth, wwk)) {
                    // Push the edited data to the array
                    editedData.push({
                        curMonth: curMonth,
                        nextMonth: nextMonth,
                        wwk: wwk,
                        product: productId,
                        value: editedCellValue
                    });
                } else {
                    // Display a message or handle the case when the value is not within the desired range
                    alert("Invalid input. The value does not match the specified criteria.");
                }
            });
            console.log(editedData);
            // // Add event listener for saving changes
            $("#saveChangesBtn").click(function() {
                // Check if any data has been edited
                if (editedData.length > 0) {
                    // Send the edited data to the PHP script using AJAX
                    $.ajax({
                        url: "../controller/commands.php",
                        method: "POST",
                        data: {
                            data: editedData
                        },
                        success: function(response) {
                            // Handle the success response
                            console.log(response);
                            alert("Data updated successfully!");
                        },
                        error: function(xhr, status, error) {
                            // Handle the error response
                            console.log(xhr.responseText);
                            alert("Error: " + xhr.responseText);
                        }
                    });
                } else {
                    alert("No changes to save.");
                }
            });
        });
