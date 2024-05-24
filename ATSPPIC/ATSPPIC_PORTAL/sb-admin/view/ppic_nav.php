<?php include_once '../controller/ppic_dashboard_data.php';
require_once('../controller/common.php');
$emp_name = $_SESSION['Name'];
?>
<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="utf-8" />
  <meta http-equiv="X-UA-Compatible" content="IE=edge" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
  <meta name="description" content="" />
  <meta name="author" content="" />
  <title>ATS - PPIC PORTAL</title>

  <!-- Custom fonts for this template-->
  <link href="../vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
  <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

  <!-- Custom styles for this template-->
  <link href="../css/sb-admin-2.min.css" rel="stylesheet" />
  <link rel="stylesheet" href="../css/style.css">

</head>

<body id="page-top">
  <!-- Page Wrapper -->
  <div id="wrapper">
    <!-- Sidebar -->
    <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
      <!-- Nav Item - Dashboard -->
      <li class="nav-item" id="dashboardLink">
        <a class="nav-link" href="ppic_dashboard.php">
          <i class="fas fa-fw fa-tachometer-alt"></i>
          <span>Dashboard</span>
        </a>
      </li>
      <!-- Divider -->
      <hr class="sidebar-divider" />

      <!-- Heading -->
      <div class="sidebar-heading">Pages</div>
      <!-- Nav Item  -->
      <li class="nav-item" id="salesorderLink">
        <a class="nav-link" href="ppic_open_sales.php">
          <i class="fas fa-clipboard-list"></i>
          <span>Sales Orders</span>
        </a>
      </li>

      <li class="nav-item" id="mschedLink">
        <a class="nav-link" href="ppic_master_schedule.php">
          <i class="fas fa-calendar-check"></i>
          <span>Master Schedule</span>
        </a>
      </li>

      <li class="nav-item" id="dlvryplanLink">
        <a class="nav-link" href="404.php">
          <i class="fas fa-calendar-alt"></i>
          <span>Calendar of Activities</span>
        </a>
      </li>

      <li class="nav-item" id="prLink">
        <a class="nav-link" href="404.php">
          <i class="fas fa-receipt"></i>
          <span>Purchase Requisition</span>
        </a>
      </li>

      <li class="nav-item" id="intstatusLink">
        <a class="nav-link" href="404.php">
          <i class="fas fa-dolly-flatbed"></i>
          <span>Inventory Status</span>
        </a>
      </li>

      <li class="nav-item" id="matstatusLink">
        <a class="nav-link" href="404.php">
          <i class="fas fa-boxes"></i>
          <span>Material Status</span>
        </a>
      </li>

      <li class="nav-item" id="pfcompLink">
        <a class="nav-link" href="404.php">
          <i class="fas fa-chart-bar"></i>
          <span>Pull Flow Compliance</span>
        </a>
      </li>

      <!-- Divider -->
      <hr class="sidebar-divider" />

      <!-- Heading -->
      <div class="sidebar-heading">Addons</div>

      <!-- Divider -->
      <hr class="sidebar-divider d-none d-md-block" />

      <!-- Sidebar Toggler (Sidebar) -->
      <div class="text-center d-none d-md-inline">
        <button class="rounded-circle border-0" id="sidebarToggle"></button>
      </div>

      <!-- Sidebar Message -->
      <!-- <div class="sidebar-card d-none d-lg-flex"></div> -->
    </ul>
    <!-- End of Sidebar -->
    <div id="content-wrapper" class="d-flex flex-column">
      <!-- Topbar -->
      <nav class="navbar navbar-expand navbar-light bg-primary topbar fixed-top">
        <a class="ppic-name d-flex align-items-center justify-content-center" href="ppic_dashboard.php">
          <div class="">
            <img src="/ATS/ATSPROD_PORTAL/assets/images/ATS logoa.png" alt="ATS_logo" class="rounded" width="70" height="45" />
          </div>
          <div class="navbar-brand text-white fw-bold mx-3">PPIC PORTAL</div>
        </a>
        <!-- Sidebar Toggle (Topbar) -->
        <button id="sidebarToggleTop" class="btn btn-link d-md-none btn-light rounded-circle mr-3">
          <i class="fa fa-bars"></i>
        </button>

        <!-- Topbar Navbar -->
        <ul class="navbar-nav ml-auto">

          <!-- Nav Item - Search Dropdown (Visible Only XS) -->
          <li class="nav-item dropdown no-arrow d-sm-none">
            <a class="nav-link dropdown-toggle" href="#" id="searchDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <i class="fas fa-search fa-fw"></i>
            </a>
            <!-- Dropdown - Messages -->
            <div class="dropdown-menu dropdown-menu-right p-3 shadow animated--grow-in" aria-labelledby="searchDropdown">
              <form class="form-inline mr-auto w-100 navbar-search">
                <div class="input-group">
                  <input type="text" class="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2">
                  <div class="input-group-append">
                    <button class="btn btn-primary" type="button">
                      <i class="fas fa-search fa-sm"></i>
                    </button>
                  </div>
                </div>
              </form>
            </div>
          </li>

          <!-- Nav Item - Alerts -->
          <li class="nav-item dropdown no-arrow mx-1">
            <a class="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <i class="fas fa-bell fa-fw"></i>
              <!-- Counter - Alerts -->
              <span class="badge badge-danger badge-counter">0</span>
            </a>
            <!-- Dropdown - Alerts -->
            <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="alertsDropdown">
              <h6 class="dropdown-header">
                Alerts Center
              </h6>
            </div>
          </li>

          <div class="topbar-divider d-none d-sm-block"></div>

          <!-- Nav Item - User Information -->
          <li class="nav-item dropdown no-arrow">
            <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <span class="mr-2 d-none d-lg-inline text-white small"><?php echo $emp_name; ?></span>
              <img class="img-profile rounded-circle" src="../img/undraw_profile.svg">
            </a>
            <!-- Dropdown - User Information -->
            <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
              <a class="dropdown-item" href="#">
                <i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>
                Profile
              </a>
              <a class="dropdown-item" href="#">
                <i class="fas fa-cogs fa-sm fa-fw mr-2 text-gray-400"></i>
                Settings
              </a>
              <a class="dropdown-item" href="#">
                <i class="fas fa-list fa-sm fa-fw mr-2 text-gray-400"></i>
                Activity Log
              </a>
              <div class="dropdown-divider"></div>
              <a class="dropdown-item" data-toggle="modal" data-target="#logoutModal">
                <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                Logout
              </a>
            </div>
          </li>
        </ul>
      </nav>
      <!-- End of Topbar -->
      <!-- Logout Modal-->
      <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
              <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">×</span>
              </button>
            </div>
            <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
            <div class="modal-footer">
              <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
              <a class="btn btn-primary" href="/ATS/ATSPPIC_PORTAL/PPIC_logout.php">Logout</a>
            </div>
          </div>
        </div>
      </div>

      <!-- Bootstrap core JavaScript-->
      <script src="../vendor/jquery/jquery.min.js"></script>
      <script src="../vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

      <!-- Core plugin JavaScript-->
      <script src="../vendor/jquery-easing/jquery.easing.min.js"></script>

      <!-- Custom scripts for all pages-->
      <script src="../js/sb-admin-2.min.js"></script>

      <!-- Page level plugins -->
      <script src="../vendor/chart.js/Chart.min.js"></script>

      <!-- Page level custom scripts -->
      <script>
        document.getElementById('sidebarToggleTop').click();
        // Get the current URL
        var currentUrl = window.location.pathname;
        // Split the URL by '/' and get the last segment
        var urlSegments = currentUrl.split('/');
        var lastSegment = urlSegments[urlSegments.length - 1];

        // Define an array of link IDs
        var linkIds = [
          'dashboardLink',
          'salesorderLink',
          'dlvryplanLink',
          'prLink',
          'intstatusLink',
          'matstatusLink',
          'pfcompLink',
          'mschedLink'
        ];

        // Loop through the link IDs and add the "active" class if their href matches the last segment
        for (var i = 0; i < linkIds.length; i++) {
          var linkId = linkIds[i];
          var navItem = document.getElementById(linkId);
          if (navItem && navItem.querySelector('a').getAttribute('href') === lastSegment) {
            navItem.classList.add('active');
          }
        }
      </script>
</body>

</html>