<?php
// Database connection details
$dbHost = 'localhost';
$dbUser = 'root';
$dbPass = '';
$dbName = 'ats_ppic_portal';
require_once __DIR__ . '/../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Worksheet\Drawing;
// Create a new PDO instance
$pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUser, $dbPass);

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_FILES['excelFile'])) {
        $file = $_FILES['excelFile'];
        $fileName = $file['name'];
        $fileTmpPath = $file['tmp_name'];

        // Validate file extension and format
        $allowedExtensions = ['xls', 'xlsx'];
        $fileExt = strtolower(pathinfo($fileName, PATHINFO_EXTENSION));
        // Read the file content
        $fileContent = file_get_contents($fileTmpPath);

        if (!in_array($fileExt, $allowedExtensions)) {
            echo "Only Excel files (XLS or XLSX) are allowed.";
            exit;
        }

        // Prepare the SQL statement
        $stmt = $pdo->prepare("INSERT INTO data_files (emp_id, file_name, file_content, file_type, file_path, file_size, file_saved_date) VALUES (?, ?, ?, ?, ?, ?, ?)");

        // Bind the parameters to the prepared statement
        $empId = 1; // Replace with the actual employee ID
        $fileType = $file['type'];
        $fileSize = $file['size'];
        $fileSavedDate = date('Y-m-d H:i:s');

        // Specify the target directory to save the file
        $targetDirectory = 'C:\xampp\htdocs\ATS\ATSPPIC_PORTAL\files_data\excel\\';
        $filePath = $targetDirectory . $fileName;

        $stmt->bindParam(1, $empId);
        $stmt->bindParam(2, $fileName);
        $stmt->bindParam(3, $fileContent, PDO::PARAM_LOB);
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
    }
}

// Download file if requested
if ($_SERVER['REQUEST_METHOD'] === 'GET' && isset($_GET['file_id'])) {
    $fileId = $_GET['file_id'];

    // Retrieve file details from the database
    $stmt = $pdo->prepare("SELECT file_name, file_path FROM data_files WHERE id = ?");
    $stmt->bindParam(1, $fileId);
    $stmt->execute();
    $fileData = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($fileData) {
        $fileName = $fileData['file_name'];
        $filePath = $fileData['file_path'];

        // Set appropriate headers for file download
        header("Content-type: application/octet-stream");
        header("Content-Disposition: attachment; filename=\"$fileName\"");
        header("Content-length: " . filesize($filePath));
        header("Pragma: no-cache");
        header("Expires: 0");

        // Read and output the file content
        readfile($filePath);
        exit;
    } else {
        echo "File not found.";
    }
}

?>

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

        .file-item {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
        }

        .file-item a {
            margin-left: 10px;
        }
    </style>
</head>

<body>
    <div class="container">
        <h2>Upload and View Excel File</h2>
        <form action="" method="POST" enctype="multipart/form-data">
            <input type="file" name="excelFile">
            <input type="submit" value="Upload">
        </form>

        <?php
        // Display a list of uploaded files with download and view buttons
        $stmt = $pdo->query("SELECT id, file_name, file_path FROM data_files");
        $files = $stmt->fetchAll(PDO::FETCH_ASSOC);

        if ($files) {
            echo "<h3>Uploaded Files:</h3>";
            foreach ($files as $file) {
                $fileId = $file['id'];
                $fileName = $file['file_name'];
                $filePath = $file['file_path'];

                echo "<div class='file-item'>";
                echo "<p>$fileName</p>";
                echo "<a href=\"?file_id=$fileId\">Download</a>";
                echo "<a href=\"?view_file_id=$fileId\">View</a>";
                echo "</div>";
            }
        }
        ?>


    </div>
</body>

</html>