<?php

if (!isset($_SESSION['Emp_ID']) && $_SESSION['Department'] !== 'DPIC') {
    header('Location: /ATS/ATSPROD_PORTAL/ATS_Prod_Home.php');
    exit();
}
