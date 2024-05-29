<?php

include __DIR__ . '/../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;

// Path to the Excel file
$excelFilePath = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/excel/Production Portal Timeline (JLP & PNP).xlsx';

// Read the Excel file using PhpSpreadsheet
$spreadsheet = IOFactory::load($excelFilePath);

// Convert the Excel file to HTML
$writer = IOFactory::createWriter($spreadsheet, 'Html');
$excelHtml = $writer->generateHTMLAll();

// Output the HTML
echo $excelHtml;
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_FILES['excelFile']) && isset($_FILES['imageFile'])) {
        // Handle Excel file
        $excelFile = $_FILES['excelFile'];
        $excelFileName = $excelFile['name'];
        $excelFileTmpPath = $excelFile['tmp_name'];

        // Validate Excel file extension and format
        $allowedExcelExtensions = ['xls', 'xlsx'];
        $excelFileExt = strtolower(pathinfo($excelFileName, PATHINFO_EXTENSION));

        if (!in_array($excelFileExt, $allowedExcelExtensions)) {
            echo "Only Excel files (XLS or XLSX) are allowed.";
            exit;
        }

        // Read the Excel file content
        $excelFileContent = file_get_contents($excelFileTmpPath);

        // Handle Image file
        $imageFile = $_FILES['imageFile'];
        $imageFileName = $imageFile['name'];
        $imageFileTmpPath = $imageFile['tmp_name'];

        // Validate Image file extension and format (you can add more formats if needed)
        $allowedImageExtensions = ['jpg', 'jpeg', 'png', 'gif'];
        $imageFileExt = strtolower(pathinfo($imageFileName, PATHINFO_EXTENSION));

        if (!in_array($imageFileExt, $allowedImageExtensions)) {
            echo "Only JPG, JPEG, PNG, and GIF images are allowed.";
            exit;
        }

        // Prepare the SQL statement for Excel file
        $stmt = $pdo->prepare("INSERT INTO data_files (emp_id, file_name, file_content, file_type, file_path, file_size, file_saved_date) VALUES (?, ?, ?, ?, ?, ?, ?)");

        // Bind the parameters to the prepared statement
        $empId = 1; // Replace with the actual employee ID
        $fileType = $file['type'];
        $fileSize = $file['size'];
        $fileSavedDate = date('Y-m-d H:i:s');

        // Specify the target directory to save the file
        $targetDirectory = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/excel/'; // Use forward slashes for the directory path
        $filePath = $targetDirectory . $fileName;

        $stmt->bindParam(1, $empId);
        $stmt->bindParam(2, $fileName);
        $stmt->bindValue(3, $fileContent, PDO::PARAM_LOB); // Use bindValue for file content
        $stmt->bindParam(4, $fileType);
        $stmt->bindParam(5, $filePath);
        $stmt->bindParam(6, $fileSize);
        $stmt->bindParam(7, $fileSavedDate);
        // Execute the statement to insert file details into the database
        if ($stmt->execute()) {
            // Move the uploaded file to the target directory
            if (move_uploaded_file($fileTmpPath, $filePath)) {
                echo "File uploaded and stored in the database and folder successfully.";
            } else {
                echo "Error moving file to the folder.";
            }
        } else {
            echo "Error uploading file to the database.";
        }
        // Prepare the SQL statement for Image file
        $stmtImage = $pdo->prepare("INSERT INTO image_files (emp_id, file_name, file_content, file_type, file_path, file_size, file_saved_date) VALUES (?, ?, ?, ?, ?, ?, ?)");

        // Bind the parameters to the prepared statement
        $stmtImage->bindParam(1, $empId);
        $stmtImage->bindParam(2, $imageFileName);
        $stmtImage->bindValue(3, file_get_contents($imageFileTmpPath), PDO::PARAM_LOB); // Use bindValue for image content
        $stmtImage->bindParam(4, $imageFile['type']);

        // Specify the target directory to save the image
        $targetImageDirectory = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/images/'; // Use forward slashes for the directory path
        $imageFilePath = $targetImageDirectory . $imageFileName;

        $stmtImage->bindParam(5, $imageFilePath);
        $stmtImage->bindParam(6, $imageFile['size']);
        $stmtImage->bindParam(7, $fileSavedDate);

        // Execute the statement to insert image details into the database
        if ($stmtImage->execute()) {
            // Move the uploaded image to the target directory
            if (move_uploaded_file($imageFileTmpPath, $imageFilePath)) {
                echo "Excel file and Image uploaded and stored in the database and folder successfully.";
            } else {
                echo "Error moving image to the folder.";
            }
        } else {
            echo "Error uploading image to the database.";
        }
    }
}
