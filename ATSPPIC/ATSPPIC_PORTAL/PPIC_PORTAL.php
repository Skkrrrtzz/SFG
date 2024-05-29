<?php include 'ATSPPIC_header.php' ?>
<!-- <?php include 'PPIC_navbar.php' ?> -->

<!DOCTYPE html>
<html>

<head>
    <style>
        .topleft {
            position: absolute;
            top: 13%;
            left: 35%;
        }

        .midleft {
            position: absolute;
            top: 35%;
            left: 25%;
        }

        .topright {
            position: absolute;
            top: 13%;
            right: 35%;
        }

        .midright {
            position: absolute;
            top: 35%;
            right: 25%;
        }

        .bottomleft {
            position: absolute;
            bottom: 20%;
            left: 32%;
        }

        .bottomright {
            position: absolute;
            bottom: 20%;
            right: 32%;
        }

        P {
            text-align: center;
            font-family: Verdana, Geneva, Tahoma, sans-serif;
            font-size: 13px;
            font-weight: bold;
        }
    </style>

</head>

<body>

    <div class="position-absolute top-50 start-50 translate-middle">
        <image src="/ATS/ATSPROD_PORTAL/assets/images/ux.png" width="150" height="150">
    </div>
    <div class="position-absolute bottom-0 start-50 translate-middle-x  text-center">
        <button type="button" class="btn btn-danger border-3 border-dark rounded-circle">
            <image src="/ATS/ATSPROD_PORTAL/assets/images/fast-delivery.png" width="100" height="100">
        </button>
        <p>DELIVERY PLAN AND STATUS</p>
    </div>
    <div class="topleft text-center">
        <a href="portal_theme/index.html">
            <button type="button" class="btn btn-info border-3 border-dark rounded-circle">
                <image src="/ATS/ATSPROD_PORTAL/assets/images/document.png" width="100" height="100">
            </button>
        </a>
        <p>OPEN SALES ORDER</p>
    </div>
    <div class="midleft text-center">
        <a href="sb-admin/ppic_dashboard.php">
            <button type="button" class="btn btn-secondary border-3 border-dark rounded-circle">
                <image src="/ATS/ATSPROD_PORTAL/assets/images/sticky.png" width="100" height="100">
            </button>
        </a>
        <p>PURCHASE REQUISITION</p>
    </div>
    <div class="topright text-center">
        <a href="/ATS/ATSPRODsup_cable_main.php">
            <button type="button" class="btn btn-primary border-3 border-dark rounded-circle">
                <image src="/ATS/ATSPROD_PORTAL/assets/images/invent.png" width="100" height="100">
            </button>
        </a>
        <p>INVENTORY STATUS</p>
    </div>
    <div class="midright text-center">
        <button type="button" class="btn btn-success border-3 border-dark rounded-circle">
            <image src="/ATS/ATSPROD_PORTAL/assets/images/checklist.png" width="100" height="100">
        </button>
        <p>PULL FLOW COMPLIANCE</p>
    </div>
    <div class="bottomleft text-center">
        <button type="button" class="btn btn-dark border-3 border-dark rounded-circle">
            <image src="/ATS/ATSPROD_PORTAL/assets/images/audit.png" width="100" height="100">
        </button>
        <p>MASTER SCHEDULE</p>
    </div>
    <div class="bottomright text-center">
        <button type="button" class="btn btn-warning border-3 border-dark rounded-circle">
            <image src="/ATS/ATSPROD_PORTAL/assets/images/data-management.png" width="100" height="100">
        </button>
        <p>MATERIAL STATUS</p>
    </div>
</body>

</html>