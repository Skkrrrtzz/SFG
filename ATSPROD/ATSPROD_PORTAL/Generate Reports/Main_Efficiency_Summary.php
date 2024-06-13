<?php include '../ATS_Prod_Header.php'; ?>
<?php include '../PROD_navbar.php'; ?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Efficiency Summary Main Production</title>
    <script src="../assets/js/exceljs.min.js"></script>
</head>

<body>
    <center>
        <div class="table-responsive text-center mt-2 mb-2">
            <form action="" method="POST">
                <label for="datefrom"><b>FROM:</b></label>
                <input align="center" type="date" name="datefrom" value="">
                <label for="dateto"><b>TO:</b></label>
                <input type="date" name="dateto" value="">
                <div class="m-1">
                    <button class="btn btn-secondary btn-sm mb-1" id="filter" name="filter">View</button>
                    <button class="btn btn-secondary btn-sm mb-1" onclick='myApp.printTable()'>Print/Save</button>
                    <button class="btn btn-success btn-sm mb-1" onclick="exportToExcel()">Export to Excel</button>
                </div>

                <div id="spinner">
                    <span id="loading-text">Loading...</span>
                    <div class="spinner-border" role="status" id="spinner" aria-hidden="true">
                    </div>
                </div>
                <?php
                function OVRPRODUCTEFF($conn, $datefrom, $dateto)
                {
                    $result = [];
                    // QUERY FOR HEADCOUNT
                    $jlp_hc_sql = mysqli_query($conn, "SELECT COUNT(DISTINCT Name) AS total_headcount FROM prod_dtr WHERE Department IN ('Production Main','Prod Main') AND Name != 'TECHNICIAN' AND product='JLP' AND description != 'INDIRECT ACTIVITY' AND Emp_ID NOT IN ('4444','11451','12444','11448') AND DATE BETWEEN '$datefrom' AND '$dateto'");
                    $jlp_hc_row = mysqli_fetch_assoc($jlp_hc_sql);
                    $total_hc_jlp = $jlp_hc_row['total_headcount'];

                    // QUERY FOR EMPLOYEES
                    $jlp_sql_data = mysqli_query($conn, "SELECT Name,Qty_Make,cycle_time,SUM(build_percent/100 *Qty_Make) AS build_percent,output,Stations,Activity,description,Part_No,Duration,Prod_Order_No,batch_no,Code,Act_Start,Act_End,remarks,product FROM prod_dtr WHERE DATE BETWEEN '$datefrom' AND '$dateto' AND Qty_Make > 0 AND product = 'JLP' AND Emp_ID NOT IN ('11451','4444','12444','11448') AND Labor_Type != 'Prod_Reg_ID' GROUP BY Name,description,Activity ORDER BY Name ASC");

                    $array1 = []; // Array to store cycle time values
                    $array2 = []; // Array to store build percent values
                    $result_jlp = [];

                    while ($jlp_row = mysqli_fetch_array($jlp_sql_data)) {
                        $jlp_std = $jlp_row['cycle_time'];
                        $jlp_stations = $jlp_row['Stations'];
                        $jlp = $jlp_row['product'];
                        $jlp_bp = number_format($jlp_row['build_percent'], 2);
                        $jlp_new_output = $jlp_row['output'];
                        $jlp_output = $jlp_bp + $jlp_new_output;

                        if ($jlp_stations == 'FVI MODULE') {
                            $std_cycle_time = "3.00";
                            if ($jlp_output == 1) {
                                $jlp_output = '0.89';
                            }
                        } elseif ($jlp_stations == 'SUB TEST') {
                            $std_cycle_time = "2.00";
                        } else {
                            $std_cycle_time = $jlp_std;
                        }

                        $jlp_detailed_std = $std_cycle_time != 0 ? number_format(8 / $std_cycle_time, 2) : 0;
                        $jlp_detailed_output = $jlp_output != 0 ? $jlp_detailed_std * $jlp_output : 0;

                        $array1[] = $std_cycle_time;
                        $array2[] = $jlp_output;

                        $JLP_STDxOUTPUT = array_sum(array_map(function ($a, $b) {
                            return $a * $b;
                        }, $array1, $array2));

                        $JLP_WHxMP = 8 * $total_hc_jlp;
                        $JLP_TOTALEFF = $JLP_WHxMP != 0 ? number_format(($JLP_STDxOUTPUT / $JLP_WHxMP) * 100, 2) : 0;

                        $result_jlp[] = [
                            'JLP_WHxMP' => $JLP_WHxMP,
                            'JLP_STDxOUTPUT' => $JLP_STDxOUTPUT,
                            'JLP_OVERALLEFF' => $JLP_TOTALEFF,
                            'JLP_TOTALHC' => $total_hc_jlp,
                            'product_jlp' => $jlp, 'jlp_detailed_output' => $jlp_detailed_output
                        ];
                    }
                    $result[] = $result_jlp;
                    // QUERY FOR HEADCOUNT
                    $pnp_hc_sql = mysqli_query($conn, "SELECT COUNT(DISTINCT Name) AS total_headcount FROM prod_dtr WHERE Department = 'Prod Main' AND Name != 'TECHNICIAN' AND product IN ('PNP','PNP IO','PNP TRANSFER') AND description != 'INDIRECT ACTIVITY' AND Emp_ID NOT IN ('11451','12444','11448') AND DATE BETWEEN '$datefrom' AND '$dateto'");
                    $pnp_hc_row = mysqli_fetch_assoc($pnp_hc_sql);
                    $total_hc_pnp = $pnp_hc_row['total_headcount'];

                    // QUERY FOR EMPLOYEES
                    $pnp_sql_data = mysqli_query($conn, "SELECT dtr.Name, SUM(CASE WHEN dtr.Qty_Make > 0 THEN dtr.Qty_Make ELSE module.Qty END) AS Qty_Make, dtr.cycle_time, SUM(dtr.build_percent / 100 * CASE WHEN dtr.Qty_Make > 0 THEN dtr.Qty_Make ELSE module.Qty END) AS build_percent, SUM(dtr.output) AS output, dtr.Stations, dtr.description, dtr.Part_No, SUM(dtr.Duration) AS Duration, dtr.Prod_Order_No, dtr.batch_no, dtr.Code, dtr.Act_Start, dtr.Act_End, dtr.remarks, dtr.product FROM prod_dtr AS dtr LEFT JOIN prod_module AS module ON dtr.description = module.description AND dtr.batch_no = module.batch_no AND dtr.module_id = module.ID WHERE dtr.DATE BETWEEN '$datefrom' AND '$dateto' AND dtr.product IN ('PNP IO', 'PNP TRANSFER', 'PNP') AND dtr.Emp_ID NOT IN ('4444', '11451', '12444', '11448') AND dtr.Labor_Type != 'Prod_Reg_ID' GROUP BY dtr.Name, dtr.description ORDER BY dtr.Name ASC;");

                    $array1 = []; // Array to store cycle time values
                    $array2 = []; // Array to store build percent values
                    $result_pnp = [];

                    while ($pnp_row = mysqli_fetch_array($pnp_sql_data)) {

                        $pnp_std = $pnp_row['cycle_time'];
                        $pnp_bp = $pnp_row['build_percent'] + $pnp_row['output'];
                        $pnp = $pnp_row['product'];

                        $array1[] = $pnp_std;
                        $array2[] = $pnp_bp;

                        $PNP_STDxOUTPUT = array_sum(array_map(function ($a, $b) {
                            return $a * $b;
                        }, $array1, $array2));

                        $PNP_STDxOUTPUT;
                        $PNP_WHxMP = 8 * $total_hc_pnp;
                        $PNP_TOTALEFF = $PNP_WHxMP != 0 ? round(($PNP_STDxOUTPUT / $PNP_WHxMP) * 100) : 0;

                        $result_pnp[] = [
                            'PNP_WHxMP' => $PNP_WHxMP,
                            'PNP_STDxOUTPUT' => $PNP_STDxOUTPUT,
                            'PNP_OVERALLEFF' => $PNP_TOTALEFF,
                            'PNP_TOTALHC' => $total_hc_pnp,
                            'product_pnp' => $pnp
                        ];
                    }
                    $result[] = $result_pnp;
                    return $result;
                }
                function calculateDetailedEfficiency($conn, $datefrom, $dateto)
                {
                    $results = [];
                    $sum_detailed_eff = [];
                    $sum_detailed_output = [];
                    $productList = [];
                    $attendances = [];
                    // QUERY FOR HEADCOUNT
                    $hc_sql = mysqli_query($conn, "SELECT COUNT(DISTINCT Name) AS total_headcount FROM prod_dtr WHERE Department IN ('Production Main','Prod Main') AND Act_End != '' AND Name != 'TECHNICIAN' AND description != 'INDIRECT ACTIVITY' AND Emp_ID NOT IN ('11451','4444','11448') AND DATE BETWEEN '$datefrom' AND '$dateto'");
                    $hc_row = mysqli_fetch_assoc($hc_sql);
                    $total_headcount = $hc_row['total_headcount'];

                    // QUERY FOR EMPLOYEES
                    $wosql_data = mysqli_query($conn, "SELECT dtr.Name,COUNT(DISTINCT dtr.Name) AS attendance,SUM(CASE WHEN dtr.Qty_Make > 0 THEN dtr.Qty_Make ELSE module.Qty END) AS Qty_Make,dtr.cycle_time AS cycle_time,SUM(dtr.build_percent / 100 * CASE WHEN dtr.Qty_Make > 0 THEN dtr.Qty_Make ELSE module.Qty END) AS build_percent,dtr.output,dtr.Stations,dtr.description,dtr.Part_No,(dtr.Duration / 60) AS Duration,dtr.Prod_Order_No,dtr.batch_no,dtr.Code,dtr.Act_Start,dtr.Act_End,dtr.remarks,dtr.product,module.build_percent AS module_build_percent,COALESCE(module.Qty, dtr.Qty_Make) AS dtr_Qty_Make FROM prod_dtr AS dtr LEFT JOIN prod_module AS module ON dtr.description = module.description AND dtr.batch_no = module.batch_no AND dtr.module_id = module.ID WHERE dtr.DATE BETWEEN '$datefrom' AND '$dateto' AND dtr.product IN ('JLP', 'PNP', 'PNP IO') AND dtr.wo_status != 'INDIRECT' AND dtr.Department IN ('Production Main', 'Prod Main') AND dtr.Emp_ID NOT IN ('4444', '11451', '12444', '11448') AND dtr.Labor_Type != 'Prod_Reg_ID' GROUP BY dtr.Name, dtr.description, dtr.batch_no, dtr.Activity, dtr.module_id ORDER BY dtr.Name ASC;");

                    $stmt = mysqli_prepare($conn, "SELECT description FROM bom WHERE prod_type=? AND description!='INDIRECT ACTIVITY' AND level='1'");
                    mysqli_stmt_bind_param($stmt, "s", $prodType);

                    $prodType = 'JLP';

                    mysqli_stmt_execute($stmt);
                    $result = mysqli_stmt_get_result($stmt);

                    $descArray = array();
                    $all_desc = array();

                    while ($row = mysqli_fetch_assoc($result)) {
                        $desc = $row['description'];
                        $descArray[] = $desc;
                    }
                    mysqli_stmt_close($stmt);

                    $array1 = []; // Array to store cycle time values
                    $array2 = []; // Array to store build percent values

                    while ($row = mysqli_fetch_array($wosql_data)) {
                        $name = $row['Name'];
                        $attendance = $row['attendance'];
                        $description = $row['description'];
                        $prod_no = $row['Prod_Order_No'];
                        $product = $row['product'];
                        $batch_no = $row['batch_no'];
                        $code = $row['Code'];
                        $start = $row['Act_Start'];
                        $end = $row['Act_End'];
                        $remarks = $row['remarks'];
                        $wo_qty = $row['Qty_Make'];
                        $stations = $row['Stations'];
                        $part_no = $row['Part_No'];
                        $detailed_duration = $row['Duration'];
                        $std = $row['cycle_time'];
                        $bp = $row['build_percent'];
                        $new_output = $row['output'] + $bp;
                        $module_bp = $row['module_build_percent'];

                        $output = round($new_output, 2);

                        if ($stations == 'FVI MODULE' || $stations == 'FVI MAC') {
                            $std_cycle_time = "3.00";
                            if ($output == 1) {
                                $output = '0.89';
                            }
                        } elseif ($stations == 'SUB TEST') {
                            $std_cycle_time = "2.00";
                        } else {
                            $std_cycle_time = $std;
                        }

                        $detailed_std = $std_cycle_time != 0 ? $std_cycle_time : 0;
                        $std_status = number_format(8 / $detailed_std, 2);
                        $detailed_actual = $detailed_duration != 0 ? number_format($detailed_duration, 2) : 0;
                        $detailed_output = $output != 0 ? $output : 0;
                        $detailed_eff = $detailed_output != 0 ? round(($detailed_std * $detailed_output) / 8 * 100) : 0;

                        // Sum $detailed_eff for each unique name
                        if (!isset($sum_detailed_eff[$name])) {
                            $sum_detailed_eff[$name] = $detailed_eff;
                            $sum_detailed_output[$name] = $output;
                        } else {
                            $sum_detailed_eff[$name] += $detailed_eff;
                            $sum_detailed_output[$name] += $output;
                        }
                        $attendances[$name] = $attendance;
                        $productList[$name] = $product;

                        $array1[] = $std_cycle_time;
                        $array2[] = $output;

                        $STDxOUTPUT = array_sum(array_map(function ($a, $b) {
                            return $a * $b;
                        }, $array1, $array2));
                        $WHxMP = 8 * $total_headcount;
                        $total_efficiency = $WHxMP != 0 ? number_format(($STDxOUTPUT / $WHxMP) * 100, 2) : 0;

                        $results[] = [
                            'name' => $name,
                            'attendance' => $attendance,
                            'description' => $description,
                            'prod_no' => $prod_no,
                            'product' => $product,
                            'batch_no' => $batch_no,
                            'code' => $code,
                            'start' => $start,
                            'end' => $end,
                            'remarks' => $remarks,
                            'wo_qty' => $wo_qty,
                            'stations' => $stations,
                            'part_no' => $part_no,
                            'detailed_duration' => $detailed_duration,
                            'build_percent' => $output,
                            'new_output' => $new_output,
                            'detailed_std' => $detailed_std,
                            'detailed_actual' => $detailed_actual,
                            'detailed_output' => $detailed_output,
                            'detailed_eff' => $detailed_eff,
                            'WHxMP' => $WHxMP,
                            'STDxOUTPUT' => $STDxOUTPUT,
                            'OVERALLEFF' => $total_efficiency,
                            'desc' => $all_desc,
                            'total_bp' => $module_bp,
                            'std_status' => $std_status
                        ];
                    }


                    // Create the final result array with unique names, summed detailed efficiency, and output
                    foreach ($sum_detailed_eff as $name => $sum_eff) {
                        $new_product = isset($productList[$name]) ? $productList[$name] : '';
                        $attendance_ = isset($attendances[$name]) ? "P" : 'A';
                        $results[] = [
                            'name' => $name,
                            'sum_detailed_eff' => $sum_eff,
                            'new_product' => $new_product,
                            'output' => isset($sum_detailed_output[$name]) ? $sum_detailed_output[$name] : null,
                            'attendance' => $attendance_
                        ];
                    }
                    return $results;
                }

                if (isset($_POST['filter'])) {
                    $datefrom = $_POST['datefrom'];
                    $dateto = $_POST['dateto'];

                ?>
                    <div class="container">
                        <div class="row mx-0">
                            <div class="col">
                                <table class="table table-sm table-hover table-bordered display compact " id="table1">
                                    <thead class="table-secondary">
                                        <tr>
                                            <td colspan="32" class="text-dark bg-light">
                                                <h4 style="background-color: #ADD8E6" class="fw-bold">MAIN OVERALL EFFFICIENCY</h4>
                                                <h6 class="fw-bold"> FROM: <?php echo $datefrom . " TO: " . $dateto; ?></h6>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th>Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <?php
                                        $results = OVRPRODUCTEFF($conn, $datefrom, $dateto);
                                        $result_jlp = $results[0]; // Get JLP results
                                        $result_pnp = $results[1]; // Get PNP results
                                        $TOTALHC_jlp = 0;
                                        $WHxMP_jlp = 0;
                                        $OVERALLEFF_jlp = null;
                                        $STDxOUTPUT_jlp = null;
                                        $TOTALHC_pnp = 0;
                                        $WHxMP_pnp = 0;
                                        $OVERALLEFF_pnp = null;
                                        $STDxOUTPUT_pnp = null;
                                        // for future purpose (OLB AND JTP)
                                        $STDxOUTPUT_olb = null;
                                        $TOTALHC_olb = 0;
                                        $STDxOUTPUT_jtp = null;
                                        $TOTALHC_jtp = 0;
                                        $WHxMP_olb = null;
                                        $WHxMP_jtp = null;

                                        foreach ($result_jlp as $results) {
                                            if (isset($results['JLP_STDxOUTPUT']) && isset($results['JLP_WHxMP']) && isset($results['JLP_TOTALHC']) && isset($results['JLP_OVERALLEFF'])) {
                                                $STDxOUTPUT_jlp = $results['JLP_STDxOUTPUT'];
                                                $WHxMP_jlp = $results['JLP_WHxMP'];
                                                $TOTALHC_jlp = $results['JLP_TOTALHC'];
                                                $OVERALLEFF_jlp = $results['JLP_OVERALLEFF'];
                                            }
                                        }
                                        foreach ($result_pnp as $results) {
                                            if (isset($results['PNP_STDxOUTPUT']) && isset($results['PNP_WHxMP']) && isset($results['PNP_TOTALHC']) && isset($results['PNP_OVERALLEFF'])) {
                                                $STDxOUTPUT_pnp = $results['PNP_STDxOUTPUT'];
                                                $WHxMP_pnp = $results['PNP_WHxMP'];
                                                $TOTALHC_pnp = $results['PNP_TOTALHC'];
                                                $OVERALLEFF_pnp = $results['PNP_OVERALLEFF'];
                                            }
                                        }
                                        $OVR_STDxOUTPUT = $STDxOUTPUT_jlp + $STDxOUTPUT_pnp + $STDxOUTPUT_olb + $STDxOUTPUT_jtp;
                                        $OVER_WHxMP = round($WHxMP_jlp + $WHxMP_pnp + $WHxMP_olb + $WHxMP_jtp);

                                        if ($OVR_STDxOUTPUT != 0 && $OVER_WHxMP != 0) {
                                            $OVR_EFF = round($OVR_STDxOUTPUT / $OVER_WHxMP * 100);
                                        } else {
                                            $OVR_EFF = 0;
                                        }
                                        ?>
                                        <tr>
                                            <td>STD X OUTPUT</td>
                                            <td class="fw-bold text-primary"><?php echo $OVR_STDxOUTPUT; ?></td>
                                        </tr>
                                        <tr>
                                            <td>WH X MP</td>
                                            <td class="fw-bold text-primary"><?php echo $OVER_WHxMP; ?></td>
                                        </tr>
                                        <tr>
                                            <td class="fw-bold">Efficiency</td>
                                            <td class="fw-bold text-primary"><?php echo $OVR_EFF . "%"; ?></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table class="table table-sm table-hover table-striped table-bordered display compact " id="table2">
                                    <thead class="table-secondary">
                                        <tr>
                                            <td colspan="32" class="text-dark bg-light">
                                                <h4 style="background-color: #ADD8E6" class="fw-bold">EFFFICIENCY PER EMPLOYEE</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Name</th>
                                            <th>Product</th>
                                            <th>Output</th>
                                            <th>Efficiency</th>
                                            <th>Attendance</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <?php
                                        $results = calculateDetailedEfficiency($conn, $datefrom, $dateto);
                                        foreach ($results as $result) {
                                            if (isset($result['sum_detailed_eff'])) {
                                        ?>
                                                <tr class="text-center">
                                                    <td><?php echo $result['name']; ?></td>
                                                    <td><?php echo $result['new_product']; ?></td> <!-- Access the new_product value -->
                                                    <td><?php echo $result['output']; ?></td>
                                                    <td class="text-primary fw-bolder"><?php echo round($result['sum_detailed_eff']); ?>%</td>
                                                    <td><?php echo $result['attendance']; ?></td>
                                                </tr>
                                        <?php
                                            }
                                        }
                                        ?>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col">
                                <table class="table table-sm table-hover table-bordered text-wrap display compact text-center" id="table4">
                                    <thead class="table-secondary">
                                        <tr>
                                            <td colspan="32" class="text-dark bg-light">
                                                <h4 style="background-color: #ADD8E6" class="fw-bold">EFFICIENCY PER PRODUCT</h4>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Product</th>
                                            <th>HC</th>
                                            <th>STD X OUTPUT</th>
                                            <th>WH X MP</th>
                                            <th>Efficiency</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>JLP</td>
                                            <td><?php echo $TOTALHC_jlp; ?></td>
                                            <td><?php echo $STDxOUTPUT_jlp; ?></td>
                                            <td><?php echo $WHxMP_jlp; ?></td>
                                            <td class="text-primary fw-bolder"><?php echo round($OVERALLEFF_jlp) . "%"; ?></td>
                                        </tr>
                                        <tr>
                                            <td>PNP</td>
                                            <td><?php echo $TOTALHC_pnp; ?></td>
                                            <td><?php echo $STDxOUTPUT_pnp; ?></td>
                                            <td><?php echo $WHxMP_pnp; ?></td>
                                            <td class="text-primary fw-bolder"><?php echo round($OVERALLEFF_pnp) . "%"; ?></td>
                                        </tr>
                                        <tr>
                                            <td>OLB</td>
                                            <td>-</td>
                                            <td>-</td>
                                            <td>-</td>
                                            <td class="text-primary fw-bolder">-</td>
                                        </tr>
                                        <tr>
                                            <td>JTP</td>
                                            <td>-</td>
                                            <td>-</td>
                                            <td>-</td>
                                            <td class="text-primary fw-bolder">-</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="accordion accordion-flush m-1" id="accordionFlushExample">
                        <div class="accordion-item">
                            <h2 class="accordion-header">
                                <button class="accordion-button collapsed" style="background-color: #ADD8E6" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                                    VIEW DETAILS
                                </button>
                            </h2>
                            <div id="flush-collapseOne" class="accordion-collapse collapse" data-bs-parent="#accordionFlushExample">
                                <div class="accordion-body">
                                    <table class="table table-sm table-hover table-striped table-bordered display compact mt-3 fs-6" id="table5">
                                        <thead class="table-secondary">
                                            <tr>
                                                <td colspan="32" class="text-dark bg-light">
                                                    <h4 style="background-color: #ADD8E6" class="fw-bold">DETAILED EFFFICIENCY</h4>
                                                </td>
                                            </tr>
                                            <tr class="">
                                                <th>TECHNICIAN</th>
                                                <th>MODULE</th>
                                                <th>STATION</th>
                                                <th>PROD NO.</th>
                                                <th>PRODUCT</th>
                                                <th>PART NO.</th>
                                                <th>SERIAL NO.</th>
                                                <th>ACTIVITY</th>
                                                <th>STARTED</th>
                                                <th>ENDED</th>
                                                <th>REMARKS</th>
                                                <!-- <th>HRS</th> -->
                                                <th>QTY</th>
                                                <th>OUTPUT</th>
                                                <th>STATUS</th>
                                                <th>STD CYCLE TIME</th>
                                                <th>EFF</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <?php
                                            $results = calculateDetailedEfficiency($conn, $datefrom, $dateto);
                                            foreach ($results as $result) {
                                                if (!isset($result['sum_detailed_eff'])) {
                                            ?>
                                                    <tr class="text-center">
                                                        <td><?php echo $result['name']; ?></td>
                                                        <td><?php echo $result['description']; ?></td>
                                                        <td><?php echo $result['stations']; ?></td>
                                                        <td><?php echo $result['prod_no']; ?></td>
                                                        <td><?php echo $result['product']; ?></td>
                                                        <td><?php echo $result['part_no']; ?></td>
                                                        <td><?php echo $result['batch_no']; ?></td>
                                                        <td><?php echo $result['code']; ?></td>
                                                        <td><?php echo $result['start']; ?></td>
                                                        <td><?php echo $result['end']; ?></td>
                                                        <td><?php echo $result['remarks']; ?></td>
                                                        <!-- <td><?php echo $result['detailed_actual']; ?></td> -->
                                                        <td><?php echo $result['wo_qty']; ?></td>
                                                        <td><?php echo $result['new_output']; ?></td> <!--$result['detailed_output'] . "/" . -->
                                                        <td><?php echo $result['total_bp'] . "%"; ?></td>
                                                        <td><?php echo number_format($result['detailed_std'], 2); ?></td>
                                                        <td class="text-primary fw-bold"><?php echo round($result['detailed_eff']) . "%"; ?></td>
                                                    </tr>
                                            <?php
                                                }
                                            }
                                            ?>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                <?php } ?>
            </form>
        </div>

        <script>
            // Hide the spinner by default
            document.getElementById("spinner").style.display = "none";
            const viewBtn = document.getElementById("filter");
            const spinner = document.getElementById("spinner");
            const table = document.getElementById("efficiencysummary");

            viewBtn.addEventListener("click", async () => {
                spinner.style.display = "block";
                table.style.display = "none";

                const datefrom = document.getElementById("datefrom").value;
                const dateto = document.getElementById("dateto").value;
                const url = `cable_efficiency_summary.php?datefrom=${datefrom}&dateto=${dateto}`;

                try {
                    const response = await fetch(url);
                    const data = await response.text();
                    table.innerHTML += data;
                } catch (error) {
                    console.error(error);
                    alert("Failed to fetch data. Please try again.");
                } finally {
                    spinner.style.display = "none";
                    table.style.display = "table";
                }
            });
        </script>
        <script>
            // function exportTablesToExcel() {
            //     var tables = ['table1', 'table2', 'table3']; // Array of table IDs

            //     var workbook = new ExcelJS.Workbook();

            //     tables.forEach(function(tableID) {
            //         var worksheet = workbook.addWorksheet(tableID);

            //         var table = document.getElementById(tableID);
            //         var rows = table.getElementsByTagName('tr');

            //         for (var i = 0; i < rows.length; i++) {
            //             var row = rows[i];
            //             var cells = row.getElementsByTagName('td');

            //             for (var j = 0; j < cells.length; j++) {
            //                 var cell = cells[j];
            //                 var value = cell.innerText || cell.textContent;

            //                 worksheet.getCell(String.fromCharCode(65 + j) + (i + 1)).value = value;
            //             }
            //         }
            //     });

            //     workbook.xlsx.writeBuffer().then(function(buffer) {
            //         var blob = new Blob([buffer], {
            //             type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            //         });
            //         var url = URL.createObjectURL(blob);
            //         var link = document.createElement('a');
            //         link.href = url;
            //         link.download = 'tables.xlsx';
            //         link.click();
            //         URL.revokeObjectURL(url);
            //     });
            // }
            function exportToExcel() {
                // Create a new Workbook
                var workbook = new ExcelJS.Workbook();

                // Add a new Worksheet
                var worksheet = workbook.addWorksheet('Sheet 1');

                // Get the table elements by their IDs
                var table1 = document.getElementById('table1');
                var table2 = document.getElementById('table2');
                var table3 = document.getElementById('table3');
                var table4 = document.getElementById('table4');
                var table5 = document.getElementById('table5');

                // Set the starting row index in the Worksheet
                var rowIndex = 1;

                // Export Table 1
                rowIndex = exportTableToWorksheet(table1, worksheet, rowIndex);

                // Export Table 2
                rowIndex = exportTableToWorksheet(table2, worksheet, rowIndex);

                // Export Table 3
                rowIndex = exportTableToWorksheet(table3, worksheet, rowIndex);

                // Export Table 4
                rowIndex = exportTableToWorksheet(table4, worksheet, rowIndex);

                // Export Table 5
                rowIndex = exportTableToWorksheet(table5, worksheet, rowIndex);


                // Save the Workbook as an Excel file
                workbook.xlsx.writeBuffer().then(function(buffer) {
                    var blob = new Blob([buffer], {
                        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    });
                    var url = URL.createObjectURL(blob);
                    var a = document.createElement('a');
                    a.href = url;
                    a.download = 'tables.xlsx';
                    a.click();
                });
            }

            function exportTableToWorksheet(table, worksheet, rowIndex) {
                // Get the table header row
                var headerRow = table.rows[0];

                // Create a new Row in the Worksheet for the header
                var excelHeaderRow = worksheet.getRow(rowIndex);

                // Iterate over each cell in the header row
                for (var j = 0; j < headerRow.cells.length; j++) {
                    var cell = headerRow.cells[j];

                    // Add the cell value to the Excel header row and set the font style to bold
                    var excelCell = excelHeaderRow.getCell(j + 1);
                    excelCell.value = cell.innerText;
                    excelCell.font = {
                        bold: true
                    };
                }

                // Increment the row index for data rows
                rowIndex++;

                // Iterate over each data row in the table
                for (var i = 1; i < table.rows.length; i++) {
                    var row = table.rows[i];

                    // Create a new Row in the Worksheet for data
                    var excelRow = worksheet.getRow(rowIndex);

                    // Iterate over each cell in the data row
                    for (var j = 0; j < row.cells.length; j++) {
                        var cell = row.cells[j];

                        // Add the cell value to the Excel Row
                        excelRow.getCell(j + 1).value = cell.innerText;
                    }

                    // Increment the row index
                    rowIndex++;
                }

                // Add an empty row as a separator between tables
                rowIndex++;
                return rowIndex;
            }
        </script>
        <script>
            var myApp = new function() {
                this.printTable = function() {
                    var tab = document.getElementById('efficiencysummary');
                    var win = window.open('', '', 'height=700,width=700');
                    win.document.write(tab.outerHTML);
                    win.document.close();
                    win.print();
                }
            }
        </script>
</body>
</center>

</html>