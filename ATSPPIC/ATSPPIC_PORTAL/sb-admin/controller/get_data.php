<?php
include_once 'db.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_POST['solve'])) {
        // Get the values sent from JavaScript
        $prod_build_qty = filter_input(INPUT_POST, 'prod_build_qty', FILTER_VALIDATE_INT);
        $product_no = filter_input(INPUT_POST, 'product_no', FILTER_VALIDATE_INT);

        if ($prod_build_qty !== false && $product_no !== false) {
            // Perform calculations and processing here
            $calculate = $product_no + $prod_build_qty;

            // Prepare a response (optional)
            $response = ["message" => "Data received and processed successfully"];

            // Send the response back to JavaScript
            header("Content-Type: application/json");
            echo json_encode($response);
        } else {
            // Handle invalid input data
            header("HTTP/1.1 400 Bad Request");
            echo json_encode(["error" => "Invalid input data"]);
        }
    }
} else {
    // Query to fetch data from sales_order table
    $sql = "SELECT * FROM sales_order";
    $stmt = $pdo->prepare($sql);
    $stmt->execute();

    // Fetch data as an associative array
    $data = $stmt->fetchAll(PDO::FETCH_ASSOC);

    // Return data as JSON response
    echo json_encode($data);
}
