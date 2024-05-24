<?php

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $batchNo = $_POST['batchNo'];
    $startDate = $_POST['startDate'];
    $startTime = $_POST['startTime'];
    $cycleTimeInDays = $_POST['givenCycletime'];
    $workingHrs = ($_POST['inlineRadioOptions'] == "regWH") ? 7.5 : 12;

    // Calculate start date and time
    $startDateTime = new DateTime($startDate . ' ' . $startTime);
    // Clone startDate 
    $startDateFormatted = clone $startDateTime;
    // Include the start date in the count
    $workingDaysCount = 1;

    // Calculate end date considering weekends
    while ($workingDaysCount < $cycleTimeInDays) {
        // Skip weekends
        $startDateTime->modify('+1 day');
        if ($startDateTime->format('N') < 6) {
            $workingDaysCount++;
        }
    }

    // Calculate end time
    $endDateTime = clone $startDateTime;
    $endDateTime->setTime($workingHrs + $startDateTime->format('H'), $startDateTime->format('i'));

    // Prepare the response as a JSON object
    $response = [
        'batchNo' => $batchNo,
        'startDate' => $startDateFormatted->format('m/d/Y'),
        'startTime' => $startTime,
        'endDate' => $endDateTime->format('m/d/Y'),
        'endTime' => $endDateTime->format('H:i:s'),
        'cycleTimeInDays' => $cycleTimeInDays,
        'workingHrs' => $workingHrs,
    ];

    // print_r($response);

    // Output the JSON response
    echo json_encode($response);
} else {
    // If the request method is not POST, handle it accordingly
    echo "Invalid request method.";
}
