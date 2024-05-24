<?php
include_once 'db.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $message = "";

    if (isset($_FILES["pdfFile"])) {
        $file = $_FILES["pdfFile"];
        $fileName = $file["name"];
        $fileSize = $file["size"];
        $fileType = $file["type"];
        $fileTmpName = $file["tmp_name"];

        // Function to convert bytes to a human-readable format (KB, MB, or GB)
        function formatBytes($bytes, $precision = 2)
        {
            $units = array("B", "KB", "MB", "GB");
            $bytes = max($bytes, 0);
            $pow = floor(($bytes ? log($bytes) : 0) / log(1024));
            $pow = min($pow, count($units) - 1);
            $bytes /= (1 << (10 * $pow));
            return round($bytes, $precision) . " " . $units[$pow];
        }

        // Specify the target directory where you want to save the PDF files
        $targetDirectory = '../../files_data/pdf/';
        $targetPath = $targetDirectory . $fileName;
        $formattedFileSize = "";
        // Check if the file is a PDF
        if ($fileType === "application/pdf") {
            // Move the uploaded file to the target directory
            if (move_uploaded_file($fileTmpName, $targetPath)) {
                // File was successfully moved, now store its information in the database
                $query = "INSERT INTO data_files (file_name, file_size, file_type, file_loc, uploaded_by, uploaded_date)
                          VALUES (:fileName, :fileSize, :fileType, :fileLoc, :uploadedBy, NOW())";

                $stmt = $pdo->prepare($query);
                $uploadedBy = $_POST['empName'];
                $formattedFileSize = formatBytes($fileSize);

                $stmt->bindParam(":fileName", $fileName, PDO::PARAM_STR);
                $stmt->bindParam(":fileSize", $formattedFileSize, PDO::PARAM_STR);
                $stmt->bindParam(":fileType", $fileType, PDO::PARAM_STR);
                $stmt->bindParam(":fileLoc", $targetPath, PDO::PARAM_STR);
                $stmt->bindParam(":uploadedBy", $uploadedBy, PDO::PARAM_STR);

                if ($stmt->execute()) {
                    $message = "PDF uploaded successfully! File size: $formattedFileSize";
                } else {
                    $message = "Error uploading PDF: " . $stmt->errorInfo()[2];
                }
            } else {
                $message = "Error moving the uploaded PDF to the target directory.";
            }
        } else {
            $message = "Invalid file format. Please upload a PDF.";
        } // Display the message as a Bootstrap 4 alert
        $alertType = ($message === "PDF uploaded successfully! File size: $formattedFileSize") ? "alert-success" : "alert-danger";
        $messageHTML = '<div class="alert ' . $alertType . ' alert-dismissible fade show m-2" role="alert">
                ' . $message . '
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>';
        echo $messageHTML;
    } elseif (isset($_POST['upload'])) {
        // This is the code for retrieving the PDF URL
        $uploadedBy = $_POST['empName'];

        $query = "SELECT uploaded_date, file_loc,uploaded_by FROM data_files WHERE uploaded_by = :uploadedBy ORDER BY uploaded_date DESC LIMIT 1";
        $stmt = $pdo->prepare($query);
        $stmt->bindParam(":uploadedBy", $uploadedBy, PDO::PARAM_STR);

        if ($stmt->execute()) {
            $row = $stmt->fetch(PDO::FETCH_ASSOC);
            if ($row) {
                // Create an associative array with the values you want to send
                $response = [
                    'uploaded_date' => $row['uploaded_date'],
                    'file_loc' => $row['file_loc'],
                    'uploaded_by' => $row['uploaded_by']
                ];

                // Encode the response as JSON and send it
                echo json_encode($response);
            } else {
                $message = "File location not found in the database.";
            }
        } else {
            $message = "Error fetching file location from the database.";
        }
        echo $message;
    } else {
        $message = "No file was uploaded.";
    }
} else {
    echo $message = "No Connection!";
}
