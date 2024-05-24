<?php
$host = 'localhost';
$dbuser = 'root';
$dbpassword = '';
$dbname = 'ats_ppic';

try {
    $pdo = new PDO("mysql:host=$host;dbname=$dbname", $dbuser, $dbpassword);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
    die("Connection Failed: " . $e->getMessage());
}
