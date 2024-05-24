<?php
include_once 'db.php';
require_once '../../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_FILES['excelFile'])) {
        $response = array();
        $allowedExtensions = array('xlsx', 'csv');
        $fileExtension = strtolower(pathinfo($_FILES['excelFile']['name'], PATHINFO_EXTENSION));

        if (in_array($fileExtension, $allowedExtensions)) { // Truncate the order_sales table
            $truncateQuery = "TRUNCATE TABLE `ats_ppic`.`sales_order`;";
            $truncateStmt = $pdo->prepare($truncateQuery);
            if ($truncateStmt->execute()) {
                $targetDirectory = 'C:\xampp\htdocs\ATS\ATSPPIC_PORTAL\files_data\excel';
                $targetFile = $targetDirectory . '/' . uniqid() . '.' . $fileExtension;

                if (move_uploaded_file($_FILES['excelFile']['tmp_name'], $targetFile)) {
                    $spreadsheet = IOFactory::load($targetFile);
                    $worksheet = $spreadsheet->getActiveSheet();
                    $rows = $worksheet->toArray();
                    // Skip the first row (header row) and process the rest
                    $insertQuery = "INSERT INTO sales_order (Docu_status, Int_number, Docu_number, CV_code, CV_name, Posting_date, Row_Del_date, Item_no, Item_Service_description, Customer_part_no, CV_Cat_No, Qty, Inventory_UoM, Purchasing_UoM, Open_Qty, WH_Code, Price_Currency, Distribution_rule, Unit_price, Orig_Amt, Open_Amt, First_Name, Last_Name, Sales_Emp_name, Remarks, Pay_Terms_Code, Payment_Terms_Code, Ref_Number, BP_Reference_No, Customer_PO_No, Delivered_Qty, Addl_txt, Free_txt, Contact_Person, Commit_Date,Actual_Del_Date ) VALUES (:Docu_status, :Int_number, :Docu_number, :CV_code, :CV_name, :Posting_date, :Row_Del_date, :Item_no, :Item_Service_description, :Customer_part_no, :CV_Cat_No, :Qty, :Inventory_UoM, :Purchasing_UoM, :Open_Qty, :WH_Code, :Price_Currency, :Distribution_rule, :Unit_price, :Orig_Amt, :Open_Amt, :First_Name, :Last_Name, :Sales_Emp_name, :Remarks, :Pay_Terms_Code, :Payment_Terms_Code, :Ref_Number, :BP_Reference_No, :Customer_PO_No, :Delivered_Qty, :Addl_txt, :Free_txt, :Contact_Person, :Commit_Date, :Actual_Del_Date)";
                    $stmt = $pdo->prepare($insertQuery);
                    // Check if preparation of the SQL statement failed
                    if (!$stmt) {
                        $response = ['icon' => "error", 'message' => 'Error preparing the SQL statement: ' . $pdo->errorInfo()];
                    } else {
                        for ($rowIndex = 1; $rowIndex < count($rows); $rowIndex++) {
                            $rowData = $rows[$rowIndex];
                            $rowData = array_map('trim', $rowData);
                            // Convert date formats
                            $postingDate = DateTime::createFromFormat('m/d/Y', $rowData[5]);
                            $rowDelDate = DateTime::createFromFormat('m/d/Y', $rowData[6]);
                            $commitDate = DateTime::createFromFormat('m/d/Y', $rowData[34]);
                            $actualDelDate = DateTime::createFromFormat('m/d/Y', $rowData[35]);

                            // Validate if dates are properly formatted
                            if ($postingDate !== false && $rowDelDate !== false) {
                                // Format dates to MySQL format (YYYY-MM-DD)
                                $formattedPostingDate = $postingDate ? $postingDate->format('Y-m-d') : null;
                                $formattedRowDelDate = $rowDelDate ? $rowDelDate->format('Y-m-d') : null;
                                $formattedCommitDate = $commitDate ? $commitDate->format('Y-m-d') : '0000-00-00';
                                $formattedActualDelDate = $actualDelDate ? $actualDelDate->format('Y-m-d') : '0000-00-00';
                                $cleanedUnitPrice = str_replace(',', '', $rowData[18]);
                                $cleanedOrigAmt = str_replace(',', '', $rowData[19]);
                                $cleanedOpenAmt = str_replace(',', '', $rowData[20]);
                                $cleanedDelivered_Qty = str_replace(',', '', $rowData[30]);

                                $params = [
                                    ':Docu_status' => $rowData[0], ':Int_number' => $rowData[1], ':Docu_number' => $rowData[2], ':CV_code' => $rowData[3], ':CV_name' => $rowData[4], ':Posting_date' => $formattedPostingDate, ':Row_Del_date' =>  $formattedRowDelDate, ':Item_no' => $rowData[7], ':Item_Service_description' => $rowData[8], ':Customer_part_no' => $rowData[9], ':CV_Cat_No' => $rowData[10], ':Qty' => $rowData[11], ':Inventory_UoM' => $rowData[12], ':Purchasing_UoM' => $rowData[13], ':Open_Qty' => $rowData[14], ':WH_Code' => $rowData[15], ':Price_Currency' => $rowData[16], ':Distribution_rule' => $rowData[17], ':Unit_price' => $cleanedUnitPrice, ':Orig_Amt' => $cleanedOrigAmt, ':Open_Amt' => $cleanedOpenAmt, ':First_Name' => $rowData[21], ':Last_Name' => $rowData[22], ':Sales_Emp_name' => $rowData[23], ':Remarks' => $rowData[24], ':Pay_Terms_Code' => $rowData[25], ':Payment_Terms_Code' => $rowData[26], ':Ref_Number' => $rowData[27], ':BP_Reference_No' => $rowData[28], ':Customer_PO_No' => $rowData[29], ':Delivered_Qty' => $cleanedDelivered_Qty, ':Addl_txt' => $rowData[31], ':Free_txt' => $rowData[32], ':Contact_Person' => $rowData[33], ':Commit_Date' => $formattedCommitDate, ':Actual_Del_Date' => $formattedActualDelDate
                                ];

                                if (!$stmt->execute($params)) {
                                    // var_dump($rowData);
                                    $response = ['icon' => "error", 'message' => 'Error executing the SQL query: ' . $stmt->errorInfo()];
                                    // Handle or log the error as needed
                                    break; // Exit the loop on the first error encountered
                                }
                            } else {
                                // var_dump($rowData);
                                // Date format is invalid - handle or log the error
                                $response = ['icon' => "error", 'message' => 'Invalid date format in row ' . $rowIndex];
                                // Handle the error as needed
                                break;
                            }
                        }

                        if (empty($response)) {
                            $response = ['icon' => "success", 'message' => 'File uploaded and processed successfully.'];
                            // var_dump($rowData);
                        }
                    }
                } else {
                    $response = ['icon' => "error", 'message' => "Error moving the file."];
                }
            } else {
                $response = ['icon' => "error", 'message' => 'Error truncating the order_sales table: ' . $truncateStmt->errorInfo()];
            }
        } else {
            $response = ['icon' => "warning", 'message' =>  "Upload file type is not xlsx, or csv!"];
        }
    } else {
        $response = ['icon' => "error", 'message' => "Error uploading the file."];
    }
} else {
    $response = ['icon' => "warning", 'message' => "No data!"];
}

echo json_encode($response);
