<?php

try {
    include_once 'db.php';
    $mastersched_sql = "SELECT * FROM `master_schedule` WHERE `month_names` IN (MONTHNAME(CURDATE()), MONTHNAME(CURDATE() + INTERVAL 1 MONTH))";
    // Prepare the SQL statement
    $stmt = $pdo->prepare($mastersched_sql);

    // Execute the statement
    $stmt->execute();

    // Initialize an associative array to store data grouped by product name
    $productsData = [];

    // Fetch the results as an associative array
    $results = $stmt->fetchAll(PDO::FETCH_ASSOC);

    // Group the data by product name
    foreach ($results as $row) {
        $productName = $row['product_names'];
        if (!isset($productsData[$productName])) {
            $productsData[$productName] = [];
        }
        $productsData[$productName][] = $row;
    }
    $products = array('JLP', 'FLIPPER', 'MTP', 'IONIZER', 'RCMTP', 'HIGH MAG FORCE', 'JTP', 'OLB', 'PNP I/O', 'PNP Transfer');
    // Output the HTML table
    foreach ($products as $product) {
        if (isset($productsData[$product])) {
            // Output the table rows with data
            echo '<tr>';
            echo '<td rowspan="7" class="fw-bold h3">' . $product . '</td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>Prod Build Qty</th>';
            foreach ($productsData[$product] as $row) {
                echo '<td>' . (isset($row['prod_Build_Qty']) ? $row['prod_Build_Qty'] : '') . '</td>';
            }
            echo '</tr>';
            echo '<tr>';
            echo '<th>No.</th>';
            foreach ($productsData[$product] as $row) {
                echo '<td>' . (isset($row['product_No']) ? $row['product_No'] : '') . '</td>';
            }
            echo '</tr>';
            echo '<tr>';
            echo '<th>Shipment Qty</th>';
            foreach ($productsData[$product] as $row) {
                $shipQty = isset($row['ship_Qty']) ? $row['ship_Qty'] : '';
                echo '<td name="ship_qty" id="ship_qty" ' . (($shipQty !== '' && $shipQty !== '0') ? 'class="bg-pink"' : '') . '>';
                echo $shipQty;
                echo '</td>';
            }
            echo '</tr>';
            echo '<tr>';
            echo '<th>BOH/EOH</th>';
            foreach ($productsData[$product] as $row) {
                echo '<td name="boh_eoh">' . (isset($row['BOH_EOH']) ? $row['BOH_EOH'] : '') . '</td>';
            }
            echo '</tr>';
            echo '<tr>';
            echo '<th>Actual Batch Output</th>';
            foreach ($productsData[$product] as $row) {
                echo '<td name="act_batch_output">' . (isset($row['actual_Batch_Output']) ? $row['actual_Batch_Output'] : '') . '</td>';
            }
            echo '</tr>';
            echo '<tr>';
            echo '<th>Delay</th>';
            foreach ($productsData[$product] as $row) {
                $delay = isset($row['delay']) ? $row['delay'] : '';
                echo '<td name="delay" ' . (($delay !== '' && $delay < 0) ? 'class="text-danger fw-bold"' : '') . '>';
                echo $delay;
                echo '</td>';
            }
            echo '</tr>';
        } else {
            // Output a blank row for products with no data
            echo '<tr>';
            echo '<td rowspan="7">' . $product . '</td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>Prod Build Qty</th>';
            echo '<td></td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>No.</th>';
            echo '<td></td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>Shipment Qty</th>';
            echo '<td></td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>BOH/EOH</th>';
            echo '<td></td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>Actual Batch Output</th>';
            echo '<td></td>';
            echo '</tr>';
            echo '<tr>';
            echo '<th>Delay</th>';
            echo '<td></td>';
            echo '</tr>';
        }
    }
} catch (PDOException $e) {
    // Handle any errors that occur during the execution
    echo "Error: " . $e->getMessage();
} finally {
    // Close the database connection
    $pdo = null;
}
