<?php
include __DIR__ . '/../vendor/autoload.php';

use PhpOffice\PhpSpreadsheet\IOFactory;
use Box\Spout\Writer\Common\Creator\WriterEntityFactory;
use Box\Spout\Common\Entity\Style\Color;

// Path to the Excel file
$excelFilePath = 'C:/xampp/htdocs/ATS/ATSPPIC_PORTAL/files_data/excel/Production Portal Timeline (JLP & PNP).xlsx';

// Read the Excel file using PhpSpreadsheet
$spreadsheet = IOFactory::load($excelFilePath);
$worksheet = $spreadsheet->getActiveSheet();

// Create an image writer with Spout
$writer = WriterEntityFactory::createXLSXWriter();

// Set background color for cells to white
$whiteColor = new Color(Color::WHITE);
$worksheet->getStyle("A1:{$worksheet->getHighestColumn()}{$worksheet->getHighestRow()}")->getFill()->setFillType(Color::FILL_SOLID)->setStartColor($whiteColor);

// Add cells to the image writer
foreach ($worksheet->getRowIterator() as $row) {
    foreach ($row->getCellIterator() as $cell) {
        $writer->addRow([$cell->getValue()]);
    }
}

// Save the image as PNG
$writer->setTempFolder(sys_get_temp_dir());
$imageFilePath = 'excel_image.png'; // The path to save the image
$writer->saveToFile($imageFilePath);

// Output the image
header('Content-Type: image/png');
readfile($imageFilePath);

// Delete the temporary image file
unlink($imageFilePath);
