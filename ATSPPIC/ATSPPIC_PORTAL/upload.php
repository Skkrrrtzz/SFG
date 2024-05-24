<?php
include_once 'db.php';

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
        function formatFileSize($sizeInBytes)
        {
            $units = ['B', 'KB', 'MB', 'GB', 'TB'];

            $unitIndex = 0;
            while ($sizeInBytes >= 1024 && $unitIndex < count($units) - 1) {
                $sizeInBytes /= 1024;
                $unitIndex++;
            }

            return round(
                $sizeInBytes,
                2
            ) . ' ' . $units[$unitIndex];
        }

        // Prepare the SQL statement for both Excel and Image file
        $stmt = $pdo->prepare("INSERT INTO data_files (emp_id, file_name, file_content, file_type, file_path, file_size, image_name, image_type, image_size, image_content, file_saved_date) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

        // Bind the parameters to the prepared statement
        $empId = 1; // Replace with the actual employee ID
        $fileType = $excelFile['type'];
        $excelfileSize = $excelFile['size'];
        $excelSize = formatFileSize($excelfileSize);
        $imageType = $imageFile['type'];
        $imageSize = $imageFile['size'];
        $imgSize = formatFileSize($imageSize);
        $fileSavedDate = date('Y-m-d H:i:s');

        // Specify the target directories to save the files
        $targetExcelDirectory = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/excel/'; // Use forward slashes for the directory path
        $targetImageDirectory = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/images/'; // Use forward slashes for the directory path
        $filePath = $targetExcelDirectory . $excelFileName;
        $imageFilePath = $targetImageDirectory . $imageFileName;

        $stmt->bindParam(1, $empId);
        $stmt->bindParam(2, $excelFileName);
        $stmt->bindValue(3, $excelFileContent, PDO::PARAM_LOB); // Use bindValue for Excel file content
        $stmt->bindParam(4, $fileType);
        $stmt->bindParam(5, $filePath);
        $stmt->bindParam(6, $excelSize);
        $stmt->bindParam(7, $imageFileName);
        $stmt->bindParam(8, $imageType);
        $stmt->bindParam(9, $imgSize);
        $stmt->bindValue(10, file_get_contents($imageFileTmpPath), PDO::PARAM_LOB); // Use bindValue for image content
        $stmt->bindParam(11, $fileSavedDate);

        // Execute the statement to insert details into the database
        if ($stmt->execute()) {
            // Move the uploaded files to the target directories
            if (move_uploaded_file($excelFileTmpPath, $filePath) && move_uploaded_file($imageFileTmpPath, $imageFilePath)) {
                echo "Excel file and Image uploaded and stored in the database and folders successfully.";
            } else {
                echo "Error moving files to the folders.";
            }
        } else {
            echo "Error uploading files to the database.";
        }
    } elseif (isset($_POST['delete'])) {
        // Handle delete operation
        $id = $_POST['id'];

        // Prepare the DELETE statement
        $stmt = $pdo->prepare("DELETE FROM data_files WHERE ID = :id");

        $stmt->bindParam(':id', $id);

        try {
            // Execute the DELETE statement
            $stmt->execute();
            echo 'success';
        } catch (PDOException $e) {
            echo 'error';
        }
    }
} else {
    // Query to fetch data from cable_cycletime table
    $sql = "SELECT * FROM data_files";
    $result = mysqli_query($conn, $sql);

    // Fetch data as an associative array
    $data = mysqli_fetch_all($result, MYSQLI_ASSOC);

    // Close the database connection
    mysqli_close($conn);

    // Return data as JSON response
    echo json_encode($data);
}
