<?php
require_once 'config.php';

$conn = mysqli_connect($db_config['host'], $db_config['user'], $db_config['pass']) or die("Could not connect");
mysqli_select_db($conn, $db_config['name']) or die("Could not connect to database");
