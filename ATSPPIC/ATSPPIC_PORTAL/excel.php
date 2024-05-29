<?php
include __DIR__ . '/../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;
use PhpOffice\PhpSpreadsheet\Writer\Pdf\Mpdf;

// Load Excel file
$inputFileName = $_SERVER['DOCUMENT_ROOT'] . '/ATS/ATSPPIC_PORTAL/files_data/excel/Master SCHEDULE AS OF No. 2023_74_July 10.xlsx';
$spreadsheet = IOFactory::load($inputFileName);

// Create PDF Writer
$writer = new Mpdf($spreadsheet);
$writer->setOrientation(\PhpOffice\PhpSpreadsheet\Worksheet\PageSetup::ORIENTATION_LANDSCAPE);
$writer->setSheetIndex(0);
// Save PDF to a file
$pdfOutput = $_SERVER['DOCUMENT_ROOT'] . '/ATS/ATSPPIC_PORTAL/files_data/pdf/Master_Schedule.pdf';
$writer->save($pdfOutput);

echo 'Excel converted to PDF successfully!';
