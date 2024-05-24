<?php
include __DIR__ . '/../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Worksheet\MemoryDrawing;
use Dompdf\Dompdf;
use Dompdf\Options;
use PhpOffice\Imagick\Imagick;

// Path to the Excel file
$excelFilePath = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/excel/Production Portal Timeline (JLP & PNP).xlsx';

// Read the Excel file using PhpSpreadsheet
$spreadsheet = IOFactory::load($excelFilePath);

// Create an instance of Imagick (from PhpOffice\Imagick)
$imagick = new Imagick();

// Set up Dompdf options
$options = new Options();
$options->set('isRemoteEnabled', true);
$dompdf = new Dompdf($options);

// Loop through each worksheet in the Excel file
foreach ($spreadsheet->getAllSheets() as $sheet) {
    // Render the worksheet as an image
    $drawing = new MemoryDrawing();
    $drawing->setName('Sheet Image');
    $drawing->setDescription('Sheet Image');
    $drawing->setResizeProportional(true);
    $drawing->setWidth(600);
    $drawing->setWorksheet($sheet);

    ob_start();
    $drawing->printImage($imagick);
    $imageContent = ob_get_clean();

    // Add the image to Dompdf
    $dompdf->loadHtml($imageContent);
    $dompdf->render();
}

// Output the PDF
$dompdf->stream('excel_to_pdf.pdf', ['Attachment' => false]);
