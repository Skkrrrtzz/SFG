<?php
require_once 'ppic_nav.php';
?>
<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <style>
        thead,
        td {
            font-size: 13px;
            padding: 4px;
        }
    </style>
</head>

<body>
    <div id="dashboard_nav" class="mx-2 mt-1">
        <!-- Page Heading -->
        <div class="d-sm-flex align-items-center justify-content-between">
            <h3 class="mb-0 text-gray-800 fw-bold">Dashboard</h3>
            <!-- <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i class="fas fa-download fa-sm text-white-50"></i> Generate Report</a> -->
        </div>
        <!-- Content Row -->
        <div class="row">
            <div class="col-xl-3 col-md-6 mb-2">
                <div class="card border-left-warning shadow h-100">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-warning text-uppercase">
                                    Open Sales Orders
                                </div>
                                <div class="h4 mb-0 font-weight-bold text-gray-800">
                                    <?php echo $openSalesCount; ?>
                                </div>
                                <div class="h6 mb-0 font-weight-bold text-gray-800">
                                    <i class="fas fa-dollar-sign"></i><?php echo $openSalesPrice; ?>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-box-open fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-md-6 mb-2">
                <div class="card border-left-success shadow h-100">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-success text-uppercase">
                                    Closed Sales Orders
                                </div>
                                <div class="h4 mb-0 font-weight-bold text-gray-800">
                                    <?php echo $closedSalesCount; ?>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-box fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-md-6 mb-2">
                <div class="card border-left-danger shadow h-100">
                    <div class="card-body">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="text-xs font-weight-bold text-danger text-uppercase">
                                    Delayed Sales Orders
                                </div>
                                <div class="row no-gutters align-items-center">
                                    <div class="col-auto">
                                        <div class="h4 mb-0 mr-3 font-weight-bold text-gray-800">
                                            <?php echo $delayedSalesCount; ?>
                                        </div>
                                    </div>
                                    <div class="col-auto">
                                        <!-- <div class="progress progress-sm mr-2">
                                            <div class="progress-bar bg-info" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div> -->
                                    </div>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-exclamation-triangle fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xl-3 col-md-6 mb-2">
                <div class="card border-left-warning shadow h-100">
                    <div class="card-body" type="button" data-toggle="modal" data-target="#openSalesModal">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="h6 font-weight-bold text-warning text-uppercase">
                                    Delivery this Month of <?php echo date("F"); ?>
                                </div>
                                <div class="h4 mb-0 font-weight-bold text-gray-800">
                                    <?php echo $openSalesdelThisMonth; ?>
                                </div>
                                <div class="h6 mb-0 font-weight-bold text-gray-800">
                                    <i class="fas fa-dollar-sign"></i><?php echo $openSalesdelThisMonthPrice; ?>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-calendar-check fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-3 col-md-6 mb-2">
                <div class="card border-left-success shadow h-100">
                    <div class="card-body" type="button" data-toggle="modal" data-target="#closedSalesModal">
                        <div class="row no-gutters align-items-center">
                            <div class="col mr-2">
                                <div class="h6 font-weight-bold text-success text-uppercase">
                                    Delivered this Month of <?php echo date("F"); ?>
                                </div>
                                <div class="h4 mb-0 font-weight-bold text-gray-800">
                                    <?php echo $closedSalesdelThisMonth; ?>
                                </div>
                                <div class="h6 mb-0 font-weight-bold text-gray-800">
                                    <i class="fas fa-dollar-sign"></i><?php echo $closedSalesdelThisMonthPrice; ?>
                                </div>
                            </div>
                            <div class="col-auto">
                                <i class="fas fa-shipping-fast fa-2x text-gray-300"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xl-8 col-lg-7">
                <div class="card shadow mb-2">
                    <div class="card-header">
                        <h6 class="m-0 font-weight-bold text-primary">Delivered Items</h6>
                    </div>
                    <div class="m-2">
                        <div class="chart-area">
                            <canvas id="myAreaChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-4 col-lg-5">
                <div class="card shadow mb-2">
                    <div class="card-header">
                        <h6 class="m-0 font-weight-bold text-primary">Open Sales</h6>
                    </div>
                    <div class="m-4">
                        <div class="chart-pie">
                            <canvas id="myPieChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top">
        <i class="fas fa-chevron-up"></i>
    </a>
    <!--Open Delivery this Month Modal -->
    <div class="modal fade" id="openSalesModal" tabindex="-1" aria-labelledby="openSalesModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="exampleModalLabel">Delivery this Month of <?php echo date("F"); ?></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive h-100">
                        <table class="table table-bordered table-sm">
                            <thead class="thead-dark ">
                                <tr>
                                    <th>No.</th>
                                    <th>BP Reference No.</th>
                                    <th>Posting Date</th>
                                    <th>Row Delivery Date</th>
                                    <th>Commit Date</th>
                                    <th>Actual Delivery Date</th>
                                    <th>Customer Part No</th>
                                    <th>Item/Service Description</th>
                                    <th>Qty</th>
                                    <th>Open Qty</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody class="text-nowrap">
                                <?php
                                $rowNumber = 1; // Initialize the row number counter

                                while ($row_open_delry_date = $stmt_del_date_open->fetch(PDO::FETCH_ASSOC)) {
                                    echo '<tr>';
                                    echo '<td>' . $rowNumber . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['BP_Reference_No']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Posting_date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Row_Del_date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Commit_Date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Actual_Del_Date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Customer_part_no']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Item_Service_description']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Qty']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_open_delry_date['Open_Qty']) . '</td>';
                                    echo '<td>';
                                    $docuStatus = $row_open_delry_date['Docu_status'];
                                    if ($docuStatus === 'O') {
                                        echo '<span class="badge badge-warning fw-bold">OPEN</span>';
                                    } elseif ($docuStatus === 'C') {
                                        echo '<span class="badge badge-success fw-bold">CLOSED</span>';
                                    } else {
                                        // Handle other cases if needed
                                        echo htmlspecialchars($docuStatus);
                                    }
                                    echo '</td>';
                                    echo '</tr>';
                                    $rowNumber++; // Increment the row number counter
                                }
                                ?>
                            </tbody>

                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Closed Delivered this Month Modal -->
    <div class="modal fade" id="closedSalesModal" tabindex="-1" aria-labelledby="closedSalesModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="exampleModalLabel">Delivered this Month of <?php echo date("F"); ?></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="table-responsive">
                        <table class="table table-bordered table-sm">
                            <thead class="thead-dark">
                                <tr>
                                    <th>No.</th>
                                    <th>BP Reference No.</th>
                                    <th>Posting Date</th>
                                    <th>Row Delivery Date</th>
                                    <th>Commit Date</th>
                                    <th>Actual Delivery Date</th>
                                    <th>Customer Part No</th>
                                    <th>Item/Service Description</th>
                                    <th>Qty</th>
                                    <th>Open Qty</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody class="text-nowrap">
                                <?php
                                $rowNumber = 1; // Initialize the row number counter

                                while ($row_closed_delvrd_date = $stmt_del_date_closed->fetch(PDO::FETCH_ASSOC)) {
                                    echo '<tr>';
                                    echo '<td>' . $rowNumber . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['BP_Reference_No']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Posting_date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Row_Del_date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Commit_Date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Actual_Del_Date']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Customer_part_no']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Item_Service_description']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Qty']) . '</td>';
                                    echo '<td>' . htmlspecialchars($row_closed_delvrd_date['Open_Qty']) . '</td>';
                                    echo '<td>';
                                    $docuStatus = $row_closed_delvrd_date['Docu_status'];
                                    if ($docuStatus === 'O') {
                                        echo '<span class="badge badge-warning fw-bold">OPEN</span>';
                                    } elseif ($docuStatus === 'C') {
                                        echo '<span class="badge badge-success fw-bold">CLOSED</span>';
                                    } else {
                                        // Handle other cases if needed
                                        echo htmlspecialchars($docuStatus);
                                    }
                                    echo '</td>';
                                    echo '</tr>';
                                    $rowNumber++; // Increment the row number counter
                                }
                                ?>
                            </tbody>

                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/chart-area.js"></script>
    <script>
        const openSalesCount = <?php echo $openSalesCount; ?>;
        const closedSalesCount = <?php echo $closedSalesCount; ?>;
        const delayedSalesCount = <?php echo $delayedSalesCount; ?>;
    </script>
    <script src="../js/chart-pie.js"></script>

</body>

</html>