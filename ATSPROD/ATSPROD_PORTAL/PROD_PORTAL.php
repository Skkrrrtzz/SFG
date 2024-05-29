<?php include 'ATS_Prod_Header.php' ?>
<?php include 'PROD_navbar.php' ?>
<?php require_once 'PROD_dashboard.php' ?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>PRODUCTION PORTAL</title>

    <!-- include the Chart.js library -->
    <script src="./assets/js/Chart.js"></script>
    <script src="./assets/js/Chartjsannotation.js"></script>
    <script src="assets/js/chartjs-plugin-datalabels.min.js"></script>
    <script src="./assets/js/exceljs.min.js"></script>
    <style>
        .card {
            background-color: #f8f9fa;
        }

        .card-title {
            font-size: 1.5rem;
        }

        .card-text.text-md-start.text-success .variable {
            display: inline-block;
            margin-left: 10px;
        }

        .custom-bg-cable {
            background-color: rgba(107, 114, 142, 1);
        }

        .custom-bg-main {
            background-color: rgba(64, 66, 88, 1);
        }

        .no-underline {
            text-decoration: none;
        }

        .input-group-text {
            padding-right: 10px;
        }

        .rounded-table {
            border-collapse: separate;
            border-spacing: 0;
            border-radius: 10px;
            overflow: hidden;
        }

        .rounded-table th,
        .rounded-table td {
            padding: 6px;
        }

        .dropdown-toggle::after {
            display: none !important;
        }

        .chart-container {
            position: relative;
            margin: auto;
            height: auto;
            width: auto;
        }

        /* @media (max-width: 767px) {
            .col-6 {
                width: 100%;
            }

            #all_att_chart {
                height: 300px;
                width: 100%;
            }

            #eff_chart {
                height: 300px;
                width: 100%;
            }

            #yield_chart {
                height: 300px;
                width: 100%;
            }

            #Leadtime {
                height: 300px;
                width: 100%;
            }

            #skill_matrix_main {
                height: 300px;
                width: 100%;
            }

            #skill_matrix_cable {
                height: 300px;
                width: 100%;
            }
        } */
    </style>
</head>

<body class="bg-dark-subtle">
    <div class="row g-0 text-center" id="dashboard">
        <!-- <div class="col-sm-6 col-md-8 bg-dark-subtle"> -->
        <div class="col-sm-6 col-md-8 col-lg-8 bg-dark-subtle">
            <div class="row m-1">
                <div class="container d-flex justify-content-between">
                    <div class="d-inline-flex">
                        <div class="input-group">
                            <span class="input-group-text fw-bolder"><i class="fa-solid fa-calendar-day"></i></span>
                            <input type="date" id="date" name="date" class="form-control" value="<?php echo getDefaultDate(); ?>
                            ">
                            <input type="submit" id="filter" name="filter" value="View" class="btn btn-primary m-1">
                        </div>
                    </div>
                    <div class="d-flex flex-row-reverse">
                        <button type="button" onclick="printCharts('dashboard')" class="btn btn-secondary m-1">Print</button>
                        <button type="button" onclick="saveCharts('dashboard')" class="btn btn-secondary m-1">Save</button>
                    </div>
                </div>
                <div class="col-sm-6 p-2">
                    <div class="card shadow">
                        <div class="card-header bg-primary-subtle d-flex flex-row align-items-center justify-content-between">
                            <h5 class="fw-bold"> Attendance Rate </h5> <!-- <?php echo $fullMonthName = date('F'); ?> -->
                            <div class="dropdown">
                                <a class="dropdown-toggle text-dark" href="#" role="button" id="dropdownMenu1" data-bs-toggle="dropdown" data-bs-auto-close="inside" aria-haspopup="true" aria-expanded="false">
                                    <i class="bi bi-three-dots-vertical"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-right shadow" aria-labelledby="dropdownMenu1">
                                    <label class="dropdown-item" for="toggleAllWeekly">
                                        <input type="checkbox" id="toggleAllWeekly" class="form-check-input" onchange="toggleAllInterval()" />
                                        Weekly
                                    </label>
                                    <label class="dropdown-item" for="toggleAllMonthly">
                                        <input type="checkbox" id="toggleAllMonthly" class="form-check-input" onchange="toggleAllInterval()" />
                                        Monthly
                                    </label>
                                    <div class="dropdown-divider"></div>
                                    <div class="dropdown-item" type="button" data-bs-toggle="modal" data-bs-target="#AllAttendance">More info <i class="fas fa-info-circle"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <h4><i class="fa fa-users"></i>Today: <?php echo $ATT_OVERALL . "%", " Absent ", $all_abs ?></h4>
                            <div class="chart-container">
                                <canvas id="all_att_chart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 p-2">
                    <div class="card shadow">
                        <div class="card-header bg-primary-subtle d-flex flex-row align-items-center justify-content-between">
                            <h5 class="fw-bold"> Efficiency Rate</h5> <!-- <?php echo $fullMonthName = date('F'); ?> -->
                            <div class="dropdown">
                                <a class="dropdown-toggle text-dark" href="#" role="button" id="dropdownMenu2" data-bs-toggle="dropdown" data-bs-auto-close="inside" aria-haspopup="true" aria-expanded="false">
                                    <i class="bi bi-three-dots-vertical"></i>
                                </a>
                                <div class="dropdown-menu dropdown-menu-right shadow" aria-labelledby="dropdownMenu1">
                                    <label class="dropdown-item" for="toggleDailyEff">
                                        <input type="checkbox" id="toggleDailyEff" class="form-check-input" onchange="toggleEfficiency()" />Daily
                                    </label>
                                    <label class="dropdown-item" for="toggleMonthEff">
                                        <input type="checkbox" id="toggleMonthlyEff" class="form-check-input" onchange="toggleEfficiency()" />Monthly
                                    </label>
                                    <div class="dropdown-divider"></div>
                                    <div class="dropdown-item" type="button" data-bs-toggle="modal" data-bs-target="#AllEff">More info <i class="fas fa-info-circle"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <h4><i class="fa fa-line-chart"></i>Today: <?php echo $ALL_EFF . "%"; ?></h4>
                            <div class="chart-container">
                                <canvas id="eff_chart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- <div class="col-sm col-sm-4 p-2">
                    <div class="card">
                        <div class="card-header bg-primary-subtle">
                            <h5 class="fw-bold" type="button" data-bs-toggle="modal" data-bs-target="#staticBackdrop">Production Yield <i class="fas fa-info-circle"></i></h5>
                        </div>
                        <div class="card-body">
                            <h4><i class="fa-solid fa-chart-column"></i></i><?php echo $yield, "%" ?></h4>
                            <div class="chart-container">
                                <canvas id="yield_chart"></canvas>
                            </div>
                        </div>
                    </div>
                </div> -->
                <div class="col-sm-6 p-2 h-100">
                    <div class="card shadow">
                        <div class="card-header bg-primary-subtle">
                            <!-- Button trigger modal -->
                            <h5 class="fw-bold text-center" type="button" data-bs-toggle="modal" data-bs-target="#staticBackdrop">Lead Time <i class="fas fa-info-circle"></i></h5>
                        </div>
                        <div class="card-body">
                            <!-- <h4><i class="fa-solid fa-clock"></i></h4> -->
                            <div class="chart-container">
                                <canvas id="Leadtime" height="180px"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 p-2">
                    <div class="card shadow">
                        <div class="card-header bg-primary-subtle">
                            <a class="no-underline text-dark" href="Generate Reports/module_build_status.php">
                                <h5 class="fw-bold text-center">Build Status <i class="fas fa-info-circle"></i></h5>
                            </a>
                        </div>
                        <div class="card-body">
                            <!-- <h4><i class="fa-solid fa-bars-progress"></i></h4> -->
                            <div class="chart-container">
                                <canvas id="buildStatusChart" height="180px"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- <div class="col-sm col-md-4 bg-dark-subtle"> -->
        <div class="col-sm-6 col-md-4 col-lg-4 bg-dark-subtle">
            <div class="container-fluid m-2 mx-0" id="WIP">
                <div class="row">
                    <div class="col-sm">
                        <div class="card shadow">
                            <div class="card-header bg-primary-subtle">
                                <!-- <a class="no-underline text-dark" href="Generate Reports/cable_wip.php?linkTitle=CABLE">
                                    <h5 class="fw-bold text-start fs-4 "><i class="fa-solid fa-bars-progress"></i> CABLE</h5>
                                </a> -->
                                <h4 class="fw-bold" id="modalLink" type="button" data-bs-toggle="modal" data-bs-target="#CableWIPModal">
                                    CABLE WIP <i class="fas fa-info-circle"></i>
                                </h4>
                            </div>

                            <div class="card-body">
                                <h3 class="text-start fs-4"> <span class="badge text-bg-success"><i class="fa-solid fa-play"></i> IN-PROCESS</span> <span class="float-end" id="total_cable_inprocess"><?php echo $updatedValues['total_cable_inprocess']; ?></span></h3>
                                <h3 class="text-start fs-4"> <span class="badge text-bg-secondary"><i class="fas fa-hourglass-end"></i> INDIRECT</span> <span class="float-end" id="total_cable_indirect"><?php echo $updatedValues['total_cable_indirect']; ?></span></h3>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm">
                        <div class="card shadow">
                            <div class="card-header bg-primary-subtle">
                                <!-- <a class="no-underline text-dark" href="Generate Reports/module_wip.php?linkTitle=MAIN">
                                    <h5 class=" fw-bold text-start fs-4 "><i class=" fa-solid fa-bars-progress"></i> MAIN</h5>
                                </a> -->
                                <h4 class="fw-bold" id="modalLink" type="button" data-bs-toggle="modal" data-bs-target="#MainWIPModal">
                                    MAIN WIP <i class="fas fa-info-circle"></i>
                                </h4>
                            </div>
                            <div class="card-body">
                                <h3 class="text-start fs-4"> <span class="badge text-bg-success"><i class="fa-solid fa-play"></i> IN-PROCESS</span> <span class="float-end" id="total_main_inprocess"><?php echo $updatedValues['total_main_inprocess']; ?></span></h3>
                                <h3 class="text-start fs-4"> <span class="badge text-bg-secondary"><i class="fas fa-hourglass-end"></i> INDIRECT</span> <span class="float-end" id="total_main_idl"><?php echo $updatedValues['total_main_idl']; ?></span></h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container-fluid m-2 mx-0">
                <div class="col-sm col-md p-1">
                    <div class="card shadow bg-light">
                        <div class="card-header bg-primary-subtle">
                            <!-- Button trigger modal -->
                            <h5 class="fw-bold" type="button" data-bs-toggle="modal" data-bs-target="#MainMatrixModal">Main Production Skill <i class="fas fa-info-circle"></i></h5>
                        </div>
                        <div class="card-body">
                            <div class="chart-container">
                                <canvas id="skill_matrix_main" height="180px"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm col-md p-1">
                    <div class="card shadow bg-light">
                        <div class="card-header bg-primary-subtle">
                            <!-- Button trigger modal -->
                            <h5 class="fw-bold" type="button" data-bs-toggle="modal" data-bs-target="#CableMatrixModal">Cable Production Skill <i class="fas fa-info-circle"></i></h5>
                        </div>
                        <div class="card-body">
                            <div class="chart-container">
                                <canvas id="skill_matrix_cable" height="180px"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <!--Attendance Cable and Main Modal -->
    <div class="modal fade" id="AllAttendance" tabindex="-1" aria-labelledby="AllAttendanceLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class=" modal-content">
                <div class="modal-header bg-primary-subtle">
                    <h1 class="modal-title fs-5" id="AllAttendanceLabel">Attendance Rate Cable and Main</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="card">
                        <div class="card-body">
                            <div class="">
                                <input type="checkbox" id="toggleWeekly" />
                                <label for=" toggleWeekly">Weekly</label>
                                <canvas id="att_chart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Efficiency Cable and Main Modal -->
    <div class="modal fade" id="AllEff" tabindex="-1" aria-labelledby="AllEffLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class=" modal-content">
                <div class="modal-header bg-primary-subtle">
                    <h1 class="modal-title fs-5" id="AllEffLabel">Efficiency Rate Cable and Main</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="card">
                        <div class="card-body">
                            <div class="">
                                <input type="checkbox" id="toggleWeeklyEff" />
                                <label for=" toggleWeeklyEff">Weekly</label>
                                <canvas id="eff_chart_per"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--Main Matrix Modal -->
    <div class="modal fade" id="MainMatrixModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="MatrixModal" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h1 class="modal-title fs-5 text-white" id="MatrixModal">Main Production Skill</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body py-0 my-0">
                    <div class="position-relative">
                        <table class="table table-bordered fs-6 table-sm border-dark table-hover display compact" id="MainMatrix">
                            <thead class="sticky-top">
                                <tr class="text-center">
                                    <th class="text-bg-dark" colspan="4">TECHNICIANS</th>
                                    <th class="bg-primary-subtle" colspan="11">JLP</th>
                                    <th class="bg-info-subtle" colspan="2">MATRIX</th>
                                    <th class="bg-secondary" colspan="2">TEST</th>
                                </tr>
                                <tr class="text-center">
                                    <th class="text-bg-dark">No.</th>
                                    <th class="text-bg-dark">NAME</th>
                                    <th class="text-bg-dark">EMP ID</th>
                                    <th class="text-bg-dark">TECH LEVEL</th>
                                    <th class="text-black bg-primary-subtle">CDA</th>
                                    <th class="text-black bg-primary-subtle">CDM</th>
                                    <th class="text-black bg-primary-subtle">TSL</th>
                                    <th class="text-black bg-primary-subtle">FA</th>
                                    <th class="text-black bg-primary-subtle">TXP</th>
                                    <th class="text-black bg-primary-subtle">AC</th>
                                    <th class="text-black bg-primary-subtle">FC</th>
                                    <th class="text-black bg-primary-subtle">MTP</th>
                                    <th class="text-black bg-primary-subtle">ION</th>
                                    <th class="text-black bg-primary-subtle">FLIP</th>
                                    <th class="text-black bg-primary-subtle">INT</th>
                                    <th class="text-black bg-info-subtle">SUB ASSY</th>
                                    <th class="text-black bg-info-subtle">INT</th>
                                    <th class="text-black bg-secondary">SUB TEST</th>
                                    <th class="text-black bg-secondary">F-TEST</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php
                                // Query the database to retrieve data
                                $matrix_query = "SELECT * FROM prod_skills_matrix";
                                $matrix_result = mysqli_query($conn, $matrix_query);
                                $lvl_query = "SELECT TECH_LVL, COUNT(TECH_LVL) AS LVL FROM `prod_skills_matrix` GROUP BY TECH_LVL ASC;";
                                $lvl_result = mysqli_query($conn, $lvl_query);
                                // Check if the query was successful
                                if ($matrix_result && $lvl_query) {

                                    // Generate the table rows dynamically
                                    while ($row = mysqli_fetch_assoc($matrix_result)) {
                                        // Increment the count based on the technician level
                                        $technicianLevel = $row['TECH_LVL'];
                                        $number = filter_var($technicianLevel, FILTER_SANITIZE_NUMBER_INT);
                                        $trimmedNumber = ($number !== false) ? intval($number) : '';

                                        echo "<tr class='text-center'>";
                                        echo "<td>" . $row['ID'] . "</td>";
                                        echo "<td>" . $row['Name'] . "</td>";
                                        echo "<td>" . $row['Emp_ID'] . "</td>";

                                        echo "<td class='" . (($trimmedNumber == 1) ? 'text-bg-warning bg-gradient' : (($trimmedNumber == 2) ? 'text-bg-primary bg-gradient' : (($trimmedNumber == 3) ? 'text-bg-success bg-gradient' : 'text-bg-secondary bg-gradient'))) . "'>" . $row['TECH_LVL'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['CDA'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['CDM'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['TSL'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['FA'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['TXP'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['AC'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['FC'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['MTP'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['ION'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['FLIP'] . "</td>";
                                        echo "<td class='bg-primary-subtle'>" . $row['INTEGRATION'] . "</td>";
                                        echo "<td class='text-black bg-info-subtle'>" . $row['PNP_SUB_ASSY'] . "</td>";
                                        echo "<td class='text-black bg-info-subtle'>" . $row['PNP_INT'] . "</td>";
                                        echo "<td class='text-black bg-secondary'>" . $row['SUB_TEST'] . "</td>";
                                        echo "<td class='text-black bg-secondary'>" . $row['FINAL_TEST'] . "</td>";
                                        echo "</tr>";
                                    }
                                    mysqli_free_result($matrix_result);
                                } else {
                                    // Handle the case when the query fails
                                    echo "Error: " . mysqli_error($conn);
                                }
                                ?>
                            </tbody>
                        </table>
                        <?php
                        // Initialize an array to store the level counts
                        $levelCounts = array();

                        // Fetch the results and store the counts in the array
                        while ($row = mysqli_fetch_assoc($lvl_result)) {
                            $levelCounts[$row['TECH_LVL']] = $row['LVL'];
                        } ?>
                        <!-- Display the level counts -->
                        <div class='row text-start mx-0'>
                            <div class="col">
                                <span class='badge text-bg-warning fs-4'>LEVEL 1: <?php echo $levelCounts['LVL 1']; ?></span>
                                <span class='badge text-bg-primary fs-4'>LEVEL 2: <?php echo $levelCounts['LVL 2']; ?></span>
                                <span class='badge text-bg-success fs-4'>LEVEL 3: <?php echo $levelCounts['LVL 3']; ?></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" id="exportMainMatrix">Export to Excel</button>
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Cable Matrix Modal -->
    <div class="modal fade" id="CableMatrixModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h1 class="modal-title fs-5 text-white" id="staticBackdropLabel">Cable Production Skill</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body py-0 my-0">
                    <div class="position-relative">
                        <table class="table table-bordered fs-6 table-sm border-dark table-hover display compact" id="CableMatrix">
                            <thead class="sticky-top">
                                <tr class="text-center">
                                    <th class="text-bg-dark">No.</th>
                                    <th class="text-bg-dark">NAME</th>
                                    <th class="text-bg-dark">EMP ID</th>
                                    <th class="text-bg-dark">SKILL LEVEL</th>
                                    <th class="text-black bg-warning">Manual Cutting</th>
                                    <th class="text-black bg-warning">Manual Stripping</th>
                                    <th class="text-black bg-warning">Manual Crimping</th>
                                    <th class="text-black bg-success-subtle">Semi-Auto Wire Crimp</th>
                                    <th class="text-black bg-success-subtle">Machine set Up</th>
                                    <th class="text-black bg-warning-subtle">Soldering</th>
                                    <th class="text-black bg-warning-subtle">Molding</th>
                                    <th class="text-black bg-primary-subtle">Wire Harnessing</th>
                                    <th class="text-black bg-primary-subtle">Final Assembly</th>
                                    <th class="text-black bg-info">Machine Change-over</th>
                                    <th class="text-black bg-info-subtle">Labelling</th>
                                    <th class="text-black bg-info-subtle">Electrical Testing </th>
                                    <th class="text-black bg-info-subtle">Visual Inspection</th>
                                    <th class="text-black bg-info-subtle">Pre Blocking</th>
                                    <th class="text-black bg-info-subtle">Taping</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php
                                // Query the database to retrieve data
                                $matrix_query = "SELECT * FROM prod_skills_matrix_cable";
                                $matrix_result = mysqli_query($conn, $matrix_query);
                                $lvl_query = "SELECT SKILL_LVL, COUNT(SKILL_LVL) AS LVL FROM `prod_skills_matrix_cable` GROUP BY SKILL_LVL ASC;";
                                $lvl_result = mysqli_query($conn, $lvl_query);
                                // Check if the query was successful
                                if ($matrix_result && $lvl_query) {

                                    // Generate the table rows dynamically
                                    while ($row = mysqli_fetch_assoc($matrix_result)) {
                                        // Increment the count based on the technician level
                                        $technicianLevel = $row['SKILL_LVL'];
                                        $number = filter_var($technicianLevel, FILTER_SANITIZE_NUMBER_INT);
                                        $trimmedNumber = ($number !== false) ? intval($number) : '';

                                        echo "<tr class='text-center'>";
                                        echo "<td>" . $row['ID'] . "</td>";
                                        echo "<td>" . $row['Name'] . "</td>";
                                        echo "<td>" . $row['Emp_ID'] . "</td>";

                                        echo "<td class='" . (($trimmedNumber == 1) ? 'text-bg-warning bg-gradient' : (($trimmedNumber == 2) ? 'text-bg-primary bg-gradient' : (($trimmedNumber == 3) ? 'text-bg-success bg-gradient' : 'text-bg-secondary bg-gradient'))) . "'>" . $row['SKILL_LVL'] . "</td>";
                                        echo "<td class='bg-warning' >" . $row['MCUTTING'] . "</td>";
                                        echo "<td class='bg-warning' >" . $row['MSTRIPPING'] . "</td>";
                                        echo "<td class='bg-warning' >" . $row['MCRIMPING'] . "</td>";
                                        echo "<td class='bg-success-subtle' >" . $row['SAWC'] . "</td>";
                                        echo "<td class='bg-success-subtle' >" . $row['MsU'] . "</td>";
                                        echo "<td class='bg-warning-subtle' >" . $row['SOLDERING'] . "</td>";
                                        echo "<td class='bg-warning-subtle' >" . $row['MOLDING'] . "</td>";
                                        echo "<td class='bg-primary-subtle' >" . $row['WHARNESS'] . "</td>";
                                        echo "<td class='bg-primary-subtle' >" . $row['FINALASSY'] . "</td>";
                                        echo "<td class='bg-info' >" . $row['MCO'] . "</td>";
                                        echo "<td class='bg-info-subtle' >" . $row['LABELLING'] . "</td>";
                                        echo "<td class='bg-info-subtle' >" . $row['ETESTING'] . "</td>";
                                        echo "<td class='text-black bg-info-subtle' >" . $row['VI'] . "</td>";
                                        echo "<td class='text-black bg-info-subtle' >" . $row['PB'] . "</td>";
                                        echo "<td class='text-black bg-info-subtle'>" . $row['TAPING'] . "</td>";
                                        echo "</tr>";
                                    }
                                    mysqli_free_result($matrix_result);
                                } else {
                                    // Handle the case when the query fails
                                    echo "Error: " . mysqli_error($conn);
                                }
                                ?>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" id="exportCableMatrix">Export to Excel</button>
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal CABLE -->
    <div class="modal fade" id="CableWIPModal" tabindex="-1" aria-labelledby="CableModal" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-primary-subtle">
                    <h5 class="modal-title text-center" id="CableModal">CABLE WORK IN PROGRESS</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <table class="table table-sm table-bordered table-striped table-hover display compact rounded-table" id="eclipse1">
                            <thead>
                                <tr class="table-primary text-center">
                                    <th>STATIONS</th>
                                    <th>OPERATOR</th>
                                    <th>PROD NO</th>
                                    <th>PART NO</th>
                                    <th>QTY</th>
                                    <th>ACTIVITY</th>
                                    <th>REMARKS</th>
                                    <th>STARTED</th>
                                    <th>ONGOING(minutes)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php
                                $categories = [
                                    'IN-PROCESS' => [
                                        'name' => 'DIRECT LABOR',
                                        'color' => 'text-success'
                                    ],
                                    'INDIRECT' => [
                                        'name' => 'INDIRECT LABOR',
                                        'color' => 'text-warning'
                                    ],
                                    'MH' => [
                                        'name' => 'MATERIAL HANDLER',
                                        'color' => 'text-warning'
                                    ]
                                ];

                                foreach ($categories as $category => $data) {
                                    $categoryName = $data['name'];
                                    $color = $data['color'];
                                    $query = "SELECT Name, Stations, Station_No, Part_No, Prod_Order_No, Act_Start, Qty_Make, Activity, remarks, NOW() as timer
                          FROM dtr
                          WHERE Duration = '' AND Act_Start != '' AND wo_status = '$category'
                          ORDER BY Stations";

                                    $result = mysqli_query($conn, $query);

                                    echo "<tr>
                        <th colspan='9' class='text-center $color bg-secondary-subtle'>$categoryName</th>
                      </tr>";

                                    while ($row = mysqli_fetch_array($result)) {
                                        $now = $row['timer'];
                                        $Start = strtotime($row['Act_Start']);
                                        $End = strtotime($now);
                                        $Duration = ($End - $Start) / 60;
                                        $total = number_format($Duration);

                                        echo "<tr class='fw-bold text-center table-bordered'>";
                                        echo "<td>" . $row['Stations'] . " &nbsp; " . $row['Station_No'] . "</td>";
                                        echo "<td>" . $row['Name'] . "</td>";
                                        echo "<td>" . $row['Prod_Order_No'] . "</td>";
                                        echo "<td>" . $row['Part_No'] . "</td>";
                                        echo "<td>" . $row['Qty_Make'] . "</td>";
                                        echo "<td>" . $row['Activity'] . "</td>";
                                        echo "<td>" . $row['remarks'] . "</td>";
                                        echo "<td>" . $row['Act_Start'] . "</td>";
                                        echo "<td>" . $total . "</td>";
                                        echo "</tr>";
                                    }
                                }
                                ?>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal MAIN -->
    <div class="modal fade" id="MainWIPModal" tabindex="-1" aria-labelledby="MainModal" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header bg-primary-subtle">
                    <h5 class="modal-title text-center" id="MainModal">MAIN WORK IN PROGRESS</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive ">
                        <table class="table table-bordered table-striped table-hover display compact rounded-table" id="eclipse2">
                            <thead>
                                <tr class="table-primary text-center">
                                    <th>TECHNICIAN</th>
                                    <th>DESCRIPTION</th>
                                    <th>PRODUCT</th>
                                    <th>PROD NO</th>
                                    <th>BATCH</th>
                                    <th>QTY</th>
                                    <th>ACTIVITY</th>
                                    <th>REMARKS</th>
                                    <th>STARTED</th>
                                    <th>ONGOING(minutes)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <?php
                                $categories = [
                                    'DIRECT LABOR' => [
                                        'status' => 'IN-PROCESS',
                                        'color' => 'text-success'
                                    ],
                                    'INDIRECT LABOR' => [
                                        'status' => 'INDIRECT',
                                        'color' => 'text-warning'
                                    ]
                                ];

                                foreach ($categories as $category => $data) {
                                    $status = $data['status'];
                                    $color = $data['color'];

                                    $query = "SELECT Name, Stations, batch_no, description, Prod_Order_No, product, Act_Start, Qty_Make, Activity, remarks, NOW() as timer FROM prod_dtr WHERE Act_Start != '' AND Act_End='' AND wo_status = '$status' ORDER BY Stations, product";

                                    $result = mysqli_query($conn, $query);

                                    echo "<tr class='bg-success-subtle'>
                        <th colspan='10' class='text-center $color bg-secondary-subtle'>$category</th>
                      </tr>";

                                    while ($row = mysqli_fetch_array($result)) {
                                        $now = $row['timer'];
                                        $Start = strtotime($row['Act_Start']);
                                        $End = strtotime($now);
                                        $Duration = ($End - $Start) / 60;
                                        $total = number_format($Duration);

                                        echo "<tr class='fw-bold text-center'>";
                                        echo "<td>" . $row['Name'] . "</td>";
                                        echo "<td>" . $row['description'] . "</td>";
                                        echo "<td>" . $row['product'] . "</td>";
                                        echo "<td>" . $row['Prod_Order_No'] . "</td>";
                                        echo "<td>" . $row['batch_no'] . "</td>";
                                        echo "<td>" . $row['Qty_Make'] . "</td>";
                                        echo "<td>" . $row['Activity'] . "</td>";
                                        echo "<td>" . $row['remarks'] . "</td>";
                                        echo "<td>" . $row['Act_Start'] . "</td>";
                                        echo "<td>" . $total . "</td>";
                                        echo "</tr>";
                                    }
                                }
                                ?>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--ATTENDANCE TODAY MODAL -->
    <div class="modal fade" id="AllAttModal" tabindex="-1" aria-labelledby="AllAttModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h1 class="modal-title fs-5" id="AllAttModalLabel">Attendance Login Time</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-4">
                                    <input type="hidden" id="clickedDateInput" name="clickedDate">
                                    <h4>CABLE <span id="cablePresentBadge" class="badge rounded-pill text-bg-success">Present: </span>
                                        <span id="cableAbsentBadge" class="badge rounded-pill text-bg-warning">Absent: </span>
                                    </h4>
                                    <table class="table table-hover fw-bold rounded-table border border-2" id="cableTable">
                                        <thead class="text-bg-primary">
                                            <tr>
                                                <th>Name</th>
                                                <th>Time In</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-4">
                                    <h4>MAIN <span id="prodPresentBadge" class="badge rounded-pill text-bg-success">Present: </span>
                                        <span id="prodAbsentBadge" class="badge rounded-pill text-bg-warning">Absent: </span>
                                    </h4>
                                    <table class="table table-hover fw-bold rounded-table border border-2" id="mainTable">
                                        <thead class="text-white bg-primary">
                                            <tr>
                                                <th>Name</th>
                                                <th>Time In</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-4">
                                    <h4 class="m-1">Absent on <span id="datesBadge" class="badge rounded-pill text-bg-success"></h4>
                                    <table class="table table-hover fw-bold rounded-table border border-2" id="absTable">
                                        <thead class="text-white bg-primary">
                                            <tr>
                                                <th>Name</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <?php include 'Dashboard_Charts.php'; ?>


    <script>
        function printCharts(divId) {
            var div = document.getElementById("dashboard");
            html2canvas(div).then(function(canvas) {
                var dataURL = canvas.toDataURL("image/png");
                var img = new Image();
                img.src = dataURL;
                img.onload = function() {
                    var printWindow = window.open();
                    printWindow.document.write('<html><head><title>Print Chart</title></head><body>');
                    printWindow.document.write('<img src="' + dataURL + '" width="' + img.width + '" height="' + img.height + '">');
                    printWindow.document.write('</body></html>');
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                };
            });
        }

        function saveCharts(divId) {
            var div = document.getElementById(divId);
            html2canvas(div).then(function(canvas) {
                var dataURL = canvas.toDataURL("image/png");
                var link = document.createElement("a");
                link.download = divId + ".png";
                link.href = dataURL;
                link.click();
            });
        }
    </script>

    <script>
        $(document).ready(function() {
            $('#filter').click(function(e) {
                e.preventDefault(); // Prevent form submission

                var selectedDate = $('#date').val();

                // Redirect to PHP page with the selected date as a parameter
                window.location.href = 'PROD_PORTAL.php?date=' + selectedDate;
            });
        });

        // Function to update the values of $total_cable_inprocess and $total_main_inprocess using AJAX
        function updateWIP() {
            $.ajax({
                url: "PROD_dashboard.php",
                dataType: "json",
                success: function(data) {
                    var totalCableInProcess = data.total_cable_inprocess;
                    var totalMainInProcess = data.total_main_inprocess;
                    var totalCableInDirect = data.total_cable_indirect;

                    // Update the content of the respective elements
                    $("#total_cable_inprocess").text(totalCableInProcess);
                    $("#total_main_inprocess").text(totalMainInProcess);
                    $("#total_cable_indirect").text(totalCableInDirect);
                }
            });
        }
        // Function to refresh the tables using AJAX
        function refreshTables() {
            $.ajax({
                url: "",
                dataType: "html",
                success: function(data) {
                    // Replace the existing tables with the updated ones
                    var updatedTable1 = $(data).find("#eclipse1");
                    var updatedTable2 = $(data).find("#eclipse2");
                    $("#eclipse1").html(updatedTable1);
                    $("#eclipse2").html(updatedTable2);
                },
                error: function(xhr, status, error) {
                    console.error('AJAX error: ' + status + ', ' + error);
                }
            });
        }
        setInterval(function() {
            updateWIP();
            refreshTables();
        }, 5000);

        // Function to export Table 1 to Excel
        function exportTable1ToExcel() {
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet('Table 1');

            const table = document.getElementById('MainMatrix');
            const headerRows = table.querySelectorAll('thead tr');
            const dataRows = table.querySelectorAll('tbody tr');

            // Add header rows to worksheet
            headerRows.forEach(row => {
                const rowData = Array.from(row.querySelectorAll('th')).map(th => th.textContent);
                const headerRow = worksheet.addRow(rowData);
                headerRow.font = {
                    bold: true
                };
            });

            // Add table data to worksheet
            dataRows.forEach(row => {
                const rowData = Array.from(row.querySelectorAll('td')).map(td => td.textContent);
                worksheet.addRow(rowData);
            });

            // Save the workbook as Excel file
            workbook.xlsx.writeBuffer().then(buffer => {
                const blob = new Blob([buffer], {
                    type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                });
                const url = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = 'MainMatrix.xlsx';
                a.click();
            });
        }



        // Function to export Table 2 to Excel
        function exportTable2ToExcel() {
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet('Table 2');

            const table = document.getElementById('CableMatrix');
            const rows = table.querySelectorAll('tr');

            // Add table headers to worksheet
            const headerRow = worksheet.addRow(Array.from(rows[0].querySelectorAll('th')).map(th => th.textContent));
            headerRow.font = {
                bold: true
            };

            // Add table data to worksheet
            for (let i = 1; i < rows.length; i++) {
                const row = rows[i];
                const rowData = Array.from(row.querySelectorAll('td')).map(td => td.textContent);
                worksheet.addRow(rowData);
            }

            // Save the workbook as Excel file
            workbook.xlsx.writeBuffer().then(buffer => {
                const blob = new Blob([buffer], {
                    type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                });
                const url = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = 'CableMatrix.xlsx';
                a.click();
            });
        }

        // Attach click event listeners to export buttons
        document.getElementById('exportMainMatrix').addEventListener('click', exportTable1ToExcel);
        document.getElementById('exportCableMatrix').addEventListener('click', exportTable2ToExcel);
    </script>
</body>
<footer class="bg-dark">
    <div class="container-fluid pt-3 pb-3 ">
        <div class="fw-bold text-white">
            ATS © 2023 <a href="https://pimes.com.ph">P. IMES Corp.</a>
        </div>
    </div>
</footer>

</html>