<?php
include_once '../db.php';
require_once '../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Shared\Date;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Check if a file was uploaded
    if (isset($_FILES['excelFile'])) {
        $response = array();

        // Define allowed file extensions
        $allowedExtensions = array('xlsx', 'xls', 'csv');

        // Get the uploaded file's extension
        $fileExtension = strtolower(pathinfo($_FILES['excelFile']['name'], PATHINFO_EXTENSION));

        // Check if the file extension is allowed
        if (in_array($fileExtension, $allowedExtensions)) {
            // Define the target directory for uploads
            $targetDirectory = 'C:\xampp\htdocs\ATS\ATSPPIC_PORTAL\files_data\excel';

            // Generate a unique filename for the uploaded file
            $targetFile = $targetDirectory . '/' . uniqid() . '.' . $fileExtension;
            // Move the uploaded file to the target directory
            if (move_uploaded_file($_FILES['excelFile']['tmp_name'], $targetFile)) {
                // Load the uploaded Excel file
                $spreadsheet = IOFactory::load($targetFile);

                // Get the first worksheet in the Excel file
                $worksheet = $spreadsheet->getActiveSheet();
                // Prepare the SQL statement for execution
                $insertQuery = "INSERT INTO sales_order (Docu_status, Int_number, Docu_number, CV_code, CV_name, Posting_date, Row_Del_date, Item_no, Item_Service_description, Customer_part_no, CV_Cat_No, Qty, Inventory_UoM, Purchasing_UoM, Open_Qty, WH_Code, Price_Currency, Distribution_rule, Unit_price, Orig_Amt, Open_Amt, First_Name, Last_Name, Sales_Emp_name, Remarks, Pay_Terms_Code, Payment_Terms_Code, Ref_Number, BP_Reference_No, Customer_PO_No, Delivered_Qty, Addl_txt, Free_txt, Contact_Person) VALUES (:Docu_status, :Int_number, :Docu_number, :CV_code, :CV_name, :Posting_date, :Row_Del_date, :Item_no, :Item_Service_description, :Customer_part_no, :CV_Cat_No, :Qty, :Inventory_UoM, :Purchasing_UoM, :Open_Qty, :WH_Code, :Price_Currency, :Distribution_rule, :Unit_price, :Orig_Amt, :Open_Amt, :First_Name, :Last_Name, :Sales_Emp_name, :Remarks, :Pay_Terms_Code, :Payment_Terms_Code, :Ref_Number, :BP_Reference_No, :Customer_PO_No, :Delivered_Qty, :Addl_txt, :Free_txt, :Contact_Person)";

                $stmt = $pdo->prepare($insertQuery);

                foreach ($worksheet->getRowIterator() as $row) {
                    $cellIterator = $row->getCellIterator();
                    $cellIterator->setIterateOnlyExistingCells(FALSE);

                    // Create an associative array to hold the data
                    $rowData = array();

                    foreach ($cellIterator as $cell) {
                        $value = $cell->getValue();
                        // Check if the cell contains a date in "MM/DD/YYYY" format
                        if (preg_match('/^\d{1,2}\/\d{1,2}\/\d{4}$/', $value)) {
                            // Attempt to parse the date using strtotime
                            $timestamp = strtotime($value);

                            if ($timestamp !== false) {
                                // Convert the timestamp to a MySQL-compatible date format
                                $formattedDate = date('Y-m-d', $timestamp);

                                // Add the formatted date to the rowData array for both posting_date and row_del_date
                                $rowData[] = $formattedDate;
                            } else {
                                // Handle date parsing errors
                                $rowData[] = 'Invalid Date'; // Placeholder value or handle as needed
                            }
                        } // Check if the cell contains a date in Excel numerical format
                        else if (is_numeric($value) && Date::isDateTime($cell)) {
                            // Convert the Excel numerical date to a Unix timestamp
                            $excelBaseDate = Date::excelToTimestamp($value);
                            // Convert the Unix timestamp to a MySQL-compatible date format
                            $formattedDate = date('Y-m-d', $excelBaseDate);
                            // Add the formatted date to the rowData array for both posting_date and row_del_date
                            $rowData[] = $formattedDate;
                        } else {
                            // If it doesn't look like a date, retain the original value
                            $rowData[] = $value;
                        }
                    }

                    // Define a unique identifier (e.g., Int_number) in your database table
                    $uniqueIdentifier = $rowData[':Int_number']; // Change to the appropriate column name

                    // Check for duplicate data based on the unique identifier
                    $checkDuplicateQuery = "SELECT Int_number FROM sales_order WHERE Int_number = :Int_number";
                    $checkDuplicateStmt = $pdo->prepare($checkDuplicateQuery);
                    $checkDuplicateStmt->bindParam(':Int_number', $uniqueIdentifier);
                    $checkDuplicateStmt->execute();

                    if ($checkDuplicateStmt->rowCount() === 0) {
                        // No duplicate data found, proceed with insertion
                        // Execute the SQL statement and handle errors
                        if ($stmt->execute(array_combine(
                            array(
                                ':Docu_status', ':Int_number', ':Docu_number', ':CV_code', ':CV_name', ':Posting_date', ':Row_Del_date',
                                ':Item_no', ':Item_Service_description', ':Customer_part_no', ':CV_Cat_No', ':Qty', ':Inventory_UoM', ':Purchasing_UoM', ':Open_Qty', ':WH_Code', ':Price_Currency', ':Distribution_rule', ':Unit_price', ':Orig_Amt', ':Open_Amt', ':First_Name', ':Last_Name', ':Sales_Emp_name', ':Remarks', ':Pay_Terms_Code', ':Payment_Terms_Code', ':Ref_Number', ':BP_Reference_No', ':Customer_PO_No', ':Delivered_Qty', ':Addl_txt', ':Free_txt', ':Contact_Person'
                            ),
                            $rowData
                        ))) {
                            // Debugging: Print out the rowData
                            $response = [
                                'success' => true,
                                'message' => 'File uploaded and processed successfully.'
                            ];
                            // Print the inserted data
                            // print_r($rowData);
                        } else {
                            // There was an error with the query
                            $response = [
                                'success' => false,
                                'message' => 'Error executing the SQL query: ' . implode(', ', $stmt->errorInfo())
                            ];
                        }
                    } else {
                        // Duplicate data found, you can choose to skip or handle it as needed
                        $response = [
                            'success' => false,
                            'message' => 'Duplicate data found for Int_number: ' . $uniqueIdentifier
                        ];
                    }
                }
            } else {
                $response = [
                    'success' => false,
                    'message' => 'Error uploading the file.'
                ];
            }
        } else {
            $response = [
                'success' => false,
                'message' => 'Upload empty or file type is not xlsx, xls, or csv!'
            ];
        }
    }
} else {
    // No file was uploaded, handle accordingly
    $response = [
        'success' => false,
        'message' => 'No data!'
    ];
}
echo json_encode($response);
