<?php
date_default_timezone_set("Asia/Manila");

$conn = mysqli_connect("localhost", "root", "") or die("Could not connect");
mysqli_select_db($conn, "ewip") or die("could not connect database");

$operator_eff = 0;
$technicians_eff = 0;
$ATT_OVERALL = 0;
$ALL_EFF = 0;
$all_abs = 0;
$main = 0;
$cable = 0;
$month = date("F");
$date = date("Y-m-d");
$year = date('Y');
function getAllAttendanceData($conn, $interval)
{
  $data = array();

  if ($interval === 'daily') {

    $totalMP_sql = mysqli_query($conn, "SELECT user_ID, emp_name, department FROM user WHERE (department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394','13640','13351', '5555','12379','13695','13347','13105','11156','13778')) OR (Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947', '2023', '11742')) GROUP BY emp_name, department ORDER BY emp_name;");
    $total_MP = mysqli_num_rows($totalMP_sql);

    $totalPresent_sql = "SELECT DATE, SUM(CASE WHEN department = 'CABLE ASSY' THEN count ELSE 0 END) AS cable_count, SUM(CASE WHEN department = 'Production Main' THEN count ELSE 0 END) AS production_count, SUM(count) AS total_count FROM ( SELECT DATE, 'CABLE ASSY' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department = 'CABLE ASSY' AND WEEK(DATE) =  WEEK(NOW()) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695','13347','13105','11156','13778') GROUP BY DATE UNION SELECT DATE, 'Production Main' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND WEEK(DATE) =  WEEK(NOW()) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY DATE ) AS subquery GROUP BY DATE;";

    $dailyData = array();

    $result = mysqli_query($conn, $totalPresent_sql);
    while ($row = mysqli_fetch_assoc($result)) {
      $dailyData[$row['DATE']] = round($row['total_count'] / $total_MP * 100);
    }
    return $dailyData;
  } elseif ($interval === 'weekly') {
    $weekNumber = date('W');

    $totalMP_sql = mysqli_query($conn, "SELECT user_ID, emp_name, department FROM user WHERE (department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394','13640','13351', '5555','12379','13695','13347','13105','11156','13778')) OR (Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947', '2023', '11742')) GROUP BY emp_name, department ORDER BY emp_name;");
    $total_MP = mysqli_num_rows($totalMP_sql);

    $start_date = date('Y-m-d', strtotime("2023-W$weekNumber-1")); // Get the start date (Monday) of the week
    $end_date = date('Y-m-d', strtotime("2023-W$weekNumber-5"));   // Get the end date (Friday) of the week

    $total_working_days = 0;
    $current_date = $start_date;

    // while ($current_date <= $end_date) {
    //   $day_of_week = date('N', strtotime($current_date));
    //   // Consider weekdays (Monday to Friday) as working days
    //   if ($day_of_week >= 1 && $day_of_week <= 5) {
    //     $total_working_days++;
    //   }
    //   $current_date = date('Y-m-d', strtotime($current_date . ' +1 day'));
    // }

    $totalPresent_sql = mysqli_query($conn, "SELECT DATE,COUNT(DISTINCT CASE WHEN (WEEK(DATE) = WEEK(NOW()) OR WEEK(DATE) = WEEK(NOW()) - 1) THEN DATE END) AS present_days, SUM(CASE WHEN department = 'CABLE ASSY' THEN count ELSE 0 END) AS cable_count, SUM(CASE WHEN department = 'Production Main' THEN count ELSE 0 END) AS production_count, SUM(count) AS total_count FROM ( SELECT DATE, 'CABLE ASSY' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department = 'CABLE ASSY'AND (WEEK(DATE) = WEEK(NOW()) OR WEEK(DATE) = WEEK(NOW()) - 1) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695','13347','13105','11156','13778') GROUP BY DATE UNION SELECT DATE, 'Production Main' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND (WEEK(DATE) = WEEK(NOW()) OR WEEK(DATE) = WEEK(NOW()) - 1) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY DATE ) AS subquery GROUP BY WEEK(DATE);");

    $weeklyData = array();

    while ($row = mysqli_fetch_assoc($totalPresent_sql)) {
      $date = $row['DATE'];
      $weekNumber = date('W', strtotime($date));
      $number_of_days = $row['present_days'];
      $attendance_count = $row['total_count'];

      if ($total_MP !== 0 && $number_of_days > 0) {
        $attendance_rate = round(($attendance_count / ($total_MP * $number_of_days)) * 100);
      } else {
        $attendance_rate = 0; // To avoid division by zero error
      }

      $weeklyData[$weekNumber] = $attendance_rate;
    }

    return $weeklyData;
  } elseif ($interval === 'monthly') {

    $totalMP_sql = mysqli_query($conn, "SELECT user_ID, emp_name, department FROM user WHERE (department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394', '13351', '5555','12379','13695','13347','13105','11156','13778')) OR (Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947', '2023', '11742')) GROUP BY emp_name, department ORDER BY emp_name;");
    $total_MP = mysqli_num_rows($totalMP_sql);

    $today = date('Y-m-d');
    $year = date('Y', strtotime($today));
    $lastDayOfYear = date('Y-12-t', strtotime("$year-01-01"));

    $currentDate = date('Y-m-01', strtotime('-1 month')); // Start from the current month
    $endOfYear = false;

    $monthlyAttendanceRates = array();

    while (!$endOfYear) {
      $totalPresent_sql = mysqli_query($conn, "SELECT DATE, COUNT(DISTINCT DATE) AS present_days,SUM(CASE WHEN department = 'CABLE ASSY' THEN count ELSE 0 END) AS cable_count, SUM(CASE WHEN department = 'Production Main' THEN count ELSE 0 END) AS production_count, SUM(count) AS total_count FROM ( SELECT DATE, 'CABLE ASSY' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department = 'CABLE ASSY'AND MONTH(DATE) = MONTH('$currentDate') AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695','13347','13105','11156','13778') GROUP BY DATE UNION SELECT DATE, 'Production Main' AS department, COUNT(*) AS count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main')AND MONTH(DATE) = MONTH('$currentDate') AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND Emp_ID NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY DATE ) AS subquery;");

      $attendance_count = 0;
      $row = mysqli_fetch_assoc($totalPresent_sql);
      $attendance_count = $row['total_count'];
      $number_of_days = $row['present_days'];

      // Calculate the attendance rate for the current month
      if ($total_MP !== 0 && $number_of_days > 0) {
        $attendance_rate = round(($attendance_count / ($total_MP * $number_of_days)) * 100);
      } else {
        $attendance_rate = 0; // To avoid division by zero error
      }
      // Store the monthly attendance rate in the array
      $monthName = date('F', strtotime($currentDate)); // Get the full month name
      $monthlyAttendanceRates[$monthName] = array(
        'attendance_rate' => $attendance_rate,
        'number_of_days' => $number_of_days
      );

      // Move to the next month
      $currentDate = date('Y-m-d', strtotime('+1 month', strtotime($currentDate)));

      if ($currentDate > $lastDayOfYear) {
        $endOfYear = true;
      }
    }

    return $monthlyAttendanceRates;
  }
  return $data;
}
function getAttendanceData($conn, $type, $interval)
{
  $data = array();

  // Modify the SQL queries to fetch the data based on the interval
  if ($interval === 'daily') {

    if ($type === 'cable') {
      $operator_sql = mysqli_query($conn, "SELECT user_ID, emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394', '13351', '5555','12379','13695','13347','13105','11156','13778') GROUP BY emp_name ORDER BY emp_name");
      $operators = mysqli_num_rows($operator_sql);

      $sql = "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department = 'CABLE ASSY' AND WEEK(DATE)=WEEK(NOW()) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695','13347','13105','11156','13778') GROUP BY DATE ORDER BY DATE";
    } elseif ($type === 'main') {
      $technician_sql = mysqli_query($conn, "SELECT user_ID, emp_name, username FROM user WHERE Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY emp_name ORDER BY emp_name");
      $operators = mysqli_num_rows($technician_sql);

      $sql = "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND WEEK(DATE)=WEEK(NOW()) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY DATE ORDER BY DATE";
    }
    $dailyData = array();
    $result = mysqli_query($conn, $sql);
    while ($row = mysqli_fetch_assoc($result)) {
      $dailyData[$row['DATE']] = round($row['count'] / $operators * 100);
    }
    return $dailyData;
  } elseif ($interval === 'weekly') {

    if ($type === 'cable') {
      $operator_sql = mysqli_query($conn, "SELECT user_ID, emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394', '13351', '5555','12379','13695','13347','13105','11156','13778') GROUP BY emp_name ORDER BY emp_name");
      $operators = mysqli_num_rows($operator_sql);

      $sql = "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department = 'CABLE ASSY' AND (WEEK(DATE) = WEEK(NOW()) OR WEEK(DATE) = WEEK(NOW()) - 1) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695','13347','13105','11156','13778') GROUP BY DATE ORDER BY DATE";
    } elseif ($type === 'main') {
      $technician_sql = mysqli_query($conn, "SELECT user_ID, emp_name, username FROM user WHERE Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY emp_name ORDER BY emp_name");
      $operators = mysqli_num_rows($technician_sql);

      $sql = "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND (WEEK(DATE) = WEEK(NOW()) OR WEEK(DATE) = WEEK(NOW()) - 1) AND DAYOFWEEK(DATE) BETWEEN 2 AND 6 AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('4444', '13472', '947', '2023', '11742') GROUP BY DATE ORDER BY DATE";
    }

    $weeklyData = array();
    $workingDaysPerWeek = array(); // To store the total working days per week

    $result = mysqli_query($conn, $sql);
    while ($row = mysqli_fetch_assoc($result)) {
      $date = $row['DATE'];
      $weekNumber = date('W', strtotime($date));

      // Increment the count of working days for the current week
      if (!isset($workingDaysPerWeek[$weekNumber])) {
        $workingDaysPerWeek[$weekNumber] = 1;
      } else {
        $workingDaysPerWeek[$weekNumber]++;
      }

      // Increment the count of attendance for the current week
      if ($operators !== 0) {
        $weeklyData[$weekNumber] = isset($weeklyData[$weekNumber]) ? $weeklyData[$weekNumber] + ($row['count'] / $operators) : ($row['count'] / $operators);
      } else {
        $weeklyData[$weekNumber] = 0; // To avoid division by zero error
      }
    }

    // Calculate the average attendance rate for each week
    foreach ($weeklyData as $weekNumber => $totalAttendance) {
      $weeklyData[$weekNumber] = number_format(($totalAttendance / $workingDaysPerWeek[$weekNumber]) * 100, 2);
    }

    return $weeklyData;
  }
  return $data;
}
// Call the getAttendanceData function to retrieve the data for cable department
$cableDailyData = getAttendanceData($conn, 'cable', 'daily');
$cableWeeklyData = getAttendanceData($conn, 'cable', 'weekly');

// Call the getAttendanceData function to retrieve the data for main department
$mainDailyData = getAttendanceData($conn, 'main', 'daily');
$mainWeeklyData = getAttendanceData($conn, 'main', 'weekly');

// ALL ATTENDANCE
$OverAllDailyData = getAllAttendanceData($conn, 'daily');
$OverAllWeeklyData = getAllAttendanceData($conn, 'weekly');
$OverAllMonthlyData = getAllAttendanceData($conn, 'monthly');

$monthlyAttendanceRatesWithDays = array();
$monthlyAttendanceDays = array();
foreach ($OverAllMonthlyData as $monthName => $data) {
  $attendanceRate = $data['attendance_rate'];
  $numberOfDays = $data['number_of_days'];

  $monthlyAttendanceRatesWithDays[$monthName] = $attendanceRate;
  $monthlyAttendanceDays[$monthName] = $numberOfDays;
}

function fetchAttendanceData($conn, $clickedDate)
{
  $attendanceData = array();

  $all_users_sql = "SELECT
        SUM(CASE WHEN Department IN ('Prod Main', 'Production Main') THEN 1 ELSE 0 END) AS main_operators,
        SUM(CASE WHEN department = 'Cable Assy' AND role = 'operator' THEN 1 ELSE 0 END) AS cable_operators
    FROM user
    WHERE username NOT IN ('4444', '13472', '947', '2023', '11742', '13394', '13351', '5555','12379','13695','13347','13105','11156','13778');";
  $result = mysqli_query($conn, $all_users_sql);

  if ($result) {
    $row = mysqli_fetch_assoc($result);
    $mainOperatorsCount = $row['main_operators'];
    $cableOperatorsCount = $row['cable_operators'];
  } else {
    echo "Cannot retrieve users";
  }

  $sql_cable_att = "SELECT Name, Time_In FROM prod_attendance WHERE Department='Cable Assy' AND Emp_ID NOT IN ('5555','13640','12379','13695','13347','13105','11156','13778') AND DATE='$clickedDate' GROUP BY Name";
  $result_cable = mysqli_query($conn, $sql_cable_att);
  $cable_num = mysqli_num_rows($result_cable);

  $cable_abs = $cableOperatorsCount - $cable_num;
  $attendanceData['cable_pres'] = $cable_num;
  $attendanceData['cable_abs'] = $cable_abs;

  while ($sql_result = mysqli_fetch_assoc($result_cable)) {
    $attendanceData['cable'][] = array(
      'Time_In' => $sql_result['Time_In'],
      'Name' => $sql_result['Name']
    );
  }

  $sql_prod_att = "SELECT Name, Time_In FROM prod_attendance WHERE Department IN ('Prod Main','Production Main') AND Emp_ID NOT IN('4444','11742','2023','13472') AND DATE='$clickedDate' GROUP BY Name";
  $result_prod = mysqli_query($conn, $sql_prod_att);
  $prod_num = mysqli_num_rows($result_prod);

  $prod_abs = $mainOperatorsCount - $prod_num;
  $attendanceData['prod_pres'] = $prod_num;
  $attendanceData['prod_abs'] = $prod_abs;

  while ($sql_result = mysqli_fetch_assoc($result_prod)) {
    $attendanceData['prod'][] = array(
      'Time_In' => $sql_result['Time_In'],
      'Name' => $sql_result['Name']
    );
  }

  $sql_abs_att = "SELECT u.emp_name FROM user u LEFT JOIN prod_attendance pa ON u.emp_name = pa.Name AND pa.DATE = '$clickedDate' WHERE pa.Name IS NULL AND u.department IN ('Prod Main', 'Production Main','Cable Assy') AND u.role IN('operator','technician','cable_supervisor') AND u.username NOT IN ('266','2063','13640','13394', '13351', '5555','12379','4444', '13472', '947', '2023', '11742','13695','13347','13105','11156','13778')GROUP BY u.emp_name, u.department ORDER BY u.emp_name";
  $result_abs = mysqli_query($conn, $sql_abs_att);

  while ($sql_result = mysqli_fetch_assoc($result_abs)) {
    $attendanceData['abs'][] = array(
      'Name' => $sql_result['emp_name']
    );
  }

  return $attendanceData;
}

// Handle AJAX request
if (isset($_GET['clickedDate'])) {
  $clickedDate = $_GET['clickedDate'];
  $attendanceData = fetchAttendanceData($conn, $clickedDate);

  // Create a new array with only 'cable' and 'prod' parts
  $response = array(
    'cable' => $attendanceData['cable'],
    'cable_present' => $attendanceData['cable_pres'],
    'cable_abs' => $attendanceData['cable_abs'],
    'prod' => $attendanceData['prod'],
    'prod_present' => $attendanceData['prod_pres'],
    'prod_abs' => $attendanceData['prod_abs'],
    'all_abs' => $attendanceData['abs'],
    'dates' => $clickedDate
  );

  echo json_encode($response);
  exit();
}
function getUpdatedInProcessValues($conn)
{
  // Retrieve the updated value for $total_main_inprocess
  $sql_main_inprocess = mysqli_query($conn, "SELECT ID FROM `prod_dtr` WHERE wo_status='IN-PROCESS' AND description!='INDIRECT ACTIVITY' AND product IN('JLP','PNP','PNP IO','OLB','JTP')");
  $total_main_inprocess = mysqli_num_rows($sql_main_inprocess);

  $sql_main_idl = mysqli_query($conn, "SELECT ID FROM `prod_dtr` WHERE description='INDIRECT ACTIVITY' AND wo_status='INDIRECT' AND Duration='';");
  $total_main_idl = mysqli_num_rows($sql_main_idl);

  // Retrieve the updated value for $total_cable_inprocess
  $sql_cable_inprocess = mysqli_query($conn, "SELECT ID FROM dtr WHERE Duration = '' AND Act_Start !='' AND wo_status='IN-PROCESS' ORDER BY Stations");
  $total_cable_inprocess = mysqli_num_rows($sql_cable_inprocess);

  // Retrieve the updated value for $total_cable_indirect
  $sql_cable_indirect = mysqli_query($conn, "SELECT ID FROM dtr WHERE Duration = '' AND Act_Start !='' AND wo_status IN ('INDIRECT','MH') ORDER BY Stations");
  $total_cable_indirect = mysqli_num_rows($sql_cable_indirect);

  // Return the updated values as an associative array
  $updatedValues = [
    'total_cable_inprocess' => $total_cable_inprocess,
    'total_main_inprocess' => $total_main_inprocess,
    'total_main_idl' => $total_main_idl,
    'total_cable_indirect' => $total_cable_indirect
  ];

  return $updatedValues;
}

// Call the function and retrieve the updated values
$updatedValues = getUpdatedInProcessValues($conn);

// Check if the variable is defined
if (isset($updatedValues) && basename($_SERVER['SCRIPT_FILENAME']) === basename(__FILE__)) {
  // Variable is defined and the script is being accessed directly, use it
  echo json_encode($updatedValues);
  exit();
} else {
}

function getDefaultDate()
{
  return isset($_GET['date']) ? $_GET['date'] : date('Y-m-d');
}

$defaultDate = getDefaultDate();
$date = $defaultDate; // Assign the default date initially

// Check if a different date is selected
if (isset($_GET['date']) && $_GET['date'] != $defaultDate) {
  $date = $_GET['date']; // Assign the selected date
}

// function getEfficiency($conn)
// {

//   $cable_efficiency = "SELECT WEEK(record_date) AS week,COUNT(record_date) AS days,SUM(operator_efficiency) AS operator_efficiency ,SUM(technician_efficiency) AS technician_efficiency FROM efficiency_records WHERE MONTH(record_date) = MONTH(NOW()) AND (WEEK(record_date) = WEEK(NOW()) OR WEEK(record_date) = WEEK(NOW()) - 1) AND DAYOFWEEK(record_date) BETWEEN 2 AND 6 GROUP BY WEEK(record_date)";

//   // Execute the query
//   $result = mysqli_query($conn, $cable_efficiency);

//   // Check if the query was successful
//   if (!$result) {
//     die("Query failed: " . mysqli_error($conn));
//   }

//   // Fetch the data and store in an array
//   $dataArray = array();
//   while ($row = mysqli_fetch_assoc($result)) {
//     $wwk = $row['week'];
//     $days = $row['days'];
//     $opr = $row['operator_efficiency'] / $days;
//     $tech = $row['technician_efficiency'] / $days;
//     $ovr_eff = $opr + $tech;
//     $dataArray[] = array(
//       "week_number" => $wwk,
//       "ovr_efficiency" => round($ovr_eff / 2)
//     );
//   }
//   // Output the JSON data
//   return $dataArray;
// }

function getEfficiencyData($conn, $interval)
{
  $data = array();

  // Modify the SQL queries to fetch the data based on the interval
  if ($interval === 'daily') {
    $eff_daily_sql = "SELECT record_date,operator_efficiency,technician_efficiency FROM `efficiency_records` WHERE WEEK(record_date) = WEEK(NOW()) GROUP BY record_date;";
    $result = $conn->query($eff_daily_sql);

    if ($result) {
      while ($row = $result->fetch_assoc()) {
        $record_date = $row['record_date'];
        $cable_dailyEff = $row['operator_efficiency'];
        $main_dailyEff = $row['technician_efficiency'];
        $data[$record_date] = array(
          'cable_dailyEff' => $cable_dailyEff,
          'tech_dailyEff' => $main_dailyEff
        );
      }
      $result->free();
    }
  } elseif ($interval === 'weekly') {
    $eff_weekly_sql = "SELECT WEEK(record_date) AS week_number, COUNT(DISTINCT DATE(record_date)) AS number_of_days_with_value, SUM(operator_efficiency) AS cable_efficiency, SUM(technician_efficiency) AS main_efficiency FROM efficiency_records WHERE MONTH(record_date)=MONTH(NOW()) AND DAYOFWEEK(record_date) BETWEEN 2 AND 6 GROUP BY week_number;";
    $result = $conn->query($eff_weekly_sql);

    if ($result) {
      while ($row = $result->fetch_assoc()) {
        $noofdays = $row['number_of_days_with_value'];
        $cable_eff = number_format($row['cable_efficiency'] / $noofdays, 2);
        $tech_eff = number_format($row['main_efficiency'] / $noofdays, 2);
        $weekNumber = $row['week_number'];

        // Store the data as an array with keys for 'cable_efficiency' and 'tech_efficiency'
        $data[$weekNumber] = array(
          'cable_efficiency' => $cable_eff,
          'tech_efficiency' => $tech_eff
        );
      }
      $result->free();
    }
  }

  return $data;
}

// Call the getEfficiencyData function
$EffDailyData = getEfficiencyData($conn, 'daily');
$EffWeeklyData = getEfficiencyData($conn, 'weekly');


function getWeekNumbersInMonth($month, $year)
{
  $firstDayOfMonth = date("$year-$month-01");
  $lastDayOfMonth = date("Y-m-t", strtotime($firstDayOfMonth));

  $firstWeekNumber = date('W', strtotime($firstDayOfMonth));
  $lastWeekNumber = date('W', strtotime($lastDayOfMonth));

  $weekNumbers = array();

  for ($weekNumber = $firstWeekNumber; $weekNumber <= $lastWeekNumber; $weekNumber++) {
    $weekStartDate = date("Y-m-d", strtotime("{$year}-W{$weekNumber}-1"));
    $weekEndDate = date("Y-m-d", strtotime("{$year}-W{$weekNumber}-5")); // Friday

    if (date('n', strtotime($weekStartDate)) == $month && date('n', strtotime($weekEndDate)) == $month) {
      $weekNumbers[] = $weekNumber;
    }
  }

  return $weekNumbers;
}
function getEfficiency($conn)
{
  // Get the current month and year
  $currentMonth = date('m');
  $currentYear = date('Y');

  // Get the week numbers in the current month
  $weekNumbers = getWeekNumbersInMonth($currentMonth, $currentYear);

  // Initialize an associative array to store efficiency data
  $efficiencyData = array();
  // Loop through each week in $weekNumbers
  foreach ($weekNumbers as $weekNumber) {
    // Query to get efficiency data for the current week
    $efficiencyQuery = "SELECT COUNT(record_date) AS days, SUM(operator_efficiency) AS operator_efficiency,  SUM(technician_efficiency) AS technician_efficiency FROM efficiency_records WHERE MONTH(record_date) = $currentMonth AND YEAR(record_date) = $currentYear AND WEEK(record_date) = $weekNumber AND DAYOFWEEK(record_date) BETWEEN 2 AND 6 GROUP BY WEEK(record_date);";

    // Execute the efficiency query
    $efficiencyResult = mysqli_query($conn, $efficiencyQuery);

    // Check if the query was successful
    if (!$efficiencyResult) {
      die("Query failed: " . mysqli_error($conn));
    }

    // Fetch the efficiency data for the current week
    $row = mysqli_fetch_assoc($efficiencyResult);
    // Check if $row is not null
    if ($row !== null) {
      $days = $row['days'];

      // Check if $days is zero to avoid division by zero or if it's empty
      if ($days === '' || $days === null || $days == 0) {
        $efficiencyData[$weekNumber] = 0; // Set efficiency to 0 or any appropriate default value
      } else {
        $opr = $row['operator_efficiency'] / $days;
        $tech = $row['technician_efficiency'] / $days;
        $ovr_eff = $opr + $tech;
        // Store the efficiency data in the associative array
        $efficiencyData[$weekNumber] = round($ovr_eff / 2);
      }
    } else {
      // Handle the case where $row is null (no results)
      $efficiencyData[$weekNumber] = 0; // Set efficiency to 0 or any appropriate default value
    }
  }

  // Query to get daily efficiency
  $dailyEfficiencyQuery = "SELECT record_date, SUM(operator_efficiency) AS operator_efficiency, SUM(technician_efficiency) AS technician_efficiency FROM efficiency_records WHERE MONTH(record_date) = MONTH(NOW()) AND WEEK(record_date) = WEEK(NOW()) AND DAYOFWEEK(record_date) BETWEEN 2 AND 6 GROUP BY record_date";

  // Execute the daily efficiency query
  $dailyEfficiencyResult = mysqli_query($conn, $dailyEfficiencyQuery);

  // Check if the query was successful
  if (!$dailyEfficiencyResult) {
    die("Query failed: " . mysqli_error($conn));
  }
  $dailyEfficiencyData = array();
  while ($row = mysqli_fetch_assoc($dailyEfficiencyResult)) {
    $date = $row['record_date'];
    $opr_eff = $row['operator_efficiency'];
    $tech_eff = $row['technician_efficiency'];
    $ovr_dailyeff = $opr_eff + $tech_eff;
    // Create an array for each date containing opr_eff and tech_eff
    $dailyEfficiencyData[$date] = round($ovr_dailyeff / 2);
  }

  // Return both weekly and daily efficiency data
  return array(
    "weekly" => $efficiencyData,
    "daily" => $dailyEfficiencyData
  );
}

$efficiencyData = getEfficiency($conn);
function getWeeklyDates($startDate)
{
  $weeklyDates = [];
  $currentDate = $startDate;

  // Get the date of Monday for the given week
  $monday = date('Y-m-d', strtotime('monday this week', strtotime($currentDate)));

  // Get the date of Saturday for the given week
  $saturday = date('Y-m-d', strtotime('friday this week', strtotime($currentDate)));

  // Loop through the days from Monday to Saturday
  while ($monday <= $saturday) {
    $weeklyDates[] = $monday;
    $monday = date('Y-m-d', strtotime('+1 day', strtotime($monday)));
  }

  return $weeklyDates;
}

$weeklyDates = getWeeklyDates($date);
$yearly = date("Y");

$cableData = getWeeklyAttendanceData($conn, 'cable');
$mainData = getWeeklyAttendanceData($conn, 'main');

$cableYearlyData = getYearlyAttendanceData($conn, 'cable');
$mainYearlyData = getYearlyAttendanceData($conn, 'main');
// ATTENDANCE
// SELECT DATA FROM user TABLE
$technician_sql = mysqli_query($conn, "SELECT user_ID, emp_name, username FROM user WHERE Department IN ('Prod Main', 'Production Main') AND username NOT IN ('4444', '13472', '947','11742','2023') GROUP BY emp_name ORDER BY emp_name");
$technicians = mysqli_num_rows($technician_sql);

$operator_sql = mysqli_query($conn, "SELECT user_ID, emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394', '13351', '5555','12379','13695') GROUP BY emp_name ORDER BY emp_name");
$operators = mysqli_num_rows($operator_sql);

// SELECT PRESENT DATA FROM prod_attendance TABLE
$present_opr_sql = mysqli_query($conn, "SELECT * FROM `prod_attendance` WHERE Department='CABLE ASSY' AND DATE ='$date' AND Emp_ID NOT IN ('5555', '13640', '13394', '13351','12379','13695') ORDER BY `DATE`");
$present_operators = mysqli_num_rows($present_opr_sql);

$present_tech_sql = mysqli_query($conn, "SELECT * FROM `prod_attendance` WHERE Department IN ('Production Main', 'Prod Main') AND DATE ='$date' AND Emp_ID NOT IN ('4444', '13472', '947','2023','11742') ORDER BY `DATE`");
$present_technicians = mysqli_num_rows($present_tech_sql);

function getWeeklyAttendanceData($conn, $type)
{
  // Retrieve weekly attendance data based on the selected date or default date
  $weeklyData = array();

  // Modify the SQL queries to fetch the weekly data for the respective type (cable or main)
  if ($type === 'cable') {
    $operator_sql = mysqli_query($conn, "SELECT user_ID,emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394','13351','5555','13695') GROUP BY emp_name ORDER BY emp_name");
    $operators = mysqli_num_rows($operator_sql);

    $weekly_sql = mysqli_query($conn, "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department = 'CABLE ASSY' AND WEEK(DATE) = WEEK(NOW()) AND Emp_ID NOT IN ('5555', '13640','13394', '13351','12379','13695') GROUP BY DATE ORDER BY DATE");
  } elseif ($type === 'main') {
    $technician_sql = mysqli_query($conn, "SELECT user_ID,emp_name,username FROM user WHERE Department IN ('Prod Main','Production Main') AND username NOT IN ('4444','13472','947','2023','11742') GROUP BY emp_name ORDER BY emp_name");
    $technicians = mysqli_num_rows($technician_sql);

    $weekly_sql = mysqli_query($conn, "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND WEEK(DATE) = WEEK(NOW()) AND Emp_ID NOT IN ('4444', '13472', '947','2023','11742') GROUP BY DATE ORDER BY DATE");
  }

  // Fetch and store the weekly attendance data in an array
  while ($row = mysqli_fetch_assoc($weekly_sql)) {
    if ($type === 'cable') {
      $weeklyData[] = number_format($row['count'] / $operators * 100, 2);
    } elseif ($type === 'main') {
      $weeklyData[] = number_format($row['count'] / $technicians * 100, 2);
    }
  }

  // Return the array
  return $weeklyData;
}

function getYearlyAttendanceData($conn, $type)
{
  // Retrieve yearly attendance data based on the selected date or default date
  $yearlyData = array();

  // Modify the SQL queries to fetch the yearly data for the respective type (cable or main)
  if ($type === 'cable') {
    $operator_sql = mysqli_query($conn, "SELECT user_ID,emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394','13351','5555') GROUP BY emp_name ORDER BY emp_name");
    $operators = mysqli_num_rows($operator_sql);

    $yearly_sql = mysqli_query($conn, "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department = 'CABLE ASSY' AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('5555', '13640','13394', '13351','12379') GROUP BY MONTH(DATE) ORDER BY MONTH(DATE)");
  } elseif ($type === 'main') {
    $technician_sql = mysqli_query($conn, "SELECT user_ID,emp_name,username FROM user WHERE Department IN ('Prod Main','Production Main') AND username NOT IN ('4444','13472','947','11742') GROUP BY emp_name ORDER BY emp_name");
    $technicians = mysqli_num_rows($technician_sql);

    $yearly_sql = mysqli_query($conn, "SELECT DATE, COUNT(*) as count FROM prod_attendance WHERE Department IN ('Production Main', 'Prod Main') AND YEAR(DATE) = YEAR(NOW()) AND Emp_ID NOT IN ('4444', '13472', '947') GROUP BY MONTH(DATE) ORDER BY MONTH(DATE)");
  }

  // Fetch and store the yearly attendance data in an array
  while ($row = mysqli_fetch_assoc($yearly_sql)) {
    if ($type === 'cable') {
      $yearlyData[] = number_format($row['count'] / $operators * 100, 2);
    } elseif ($type === 'main') {
      $yearlyData[] = number_format($row['count'] / $technicians * 100, 2);
    }
  }

  // Return the array
  return $yearlyData;
}
$main = number_format(($present_technicians / $technicians) * 100, 2); // FORMULA FOR MAIN ATTENDANCE
$cable = number_format(($present_operators / $operators) * 100, 2); // FORMULA FOR CABLE ATTENDANCE

$all_present = $present_technicians + $present_operators; // TOTAL PRESENT USERS
$all = $operators + $technicians; // TOTAL USERS
$all_abs = $all - $all_present; // TOTAL ABSENT

// OVERALL ATTENDANCE PERCENTAGE
if ($main != 0 && $cable != 0) {
  $OVERALL_ATT = ($all_present / $all) * 100;
} else {
  $OVERALL_ATT = 0;
}
$ATT_OVERALL = round($OVERALL_ATT);

// SELECT CABLE ASSY USERS FROM USER TABLE
$sql_total_operator = mysqli_query($conn, "SELECT user_ID,emp_name FROM user WHERE department = 'Cable Assy' AND role = 'operator' AND username NOT IN ('13394','13351','5555','12379') GROUP BY emp_name ORDER BY emp_name");
$total_operator = mysqli_num_rows($sql_total_operator);

// Initialize a variable to hold the total actual time for all names
$total_actual_time_all_names = 0;
$total_std_time_all_names = 0;
// Initialize an array to hold the total actual time and detailed actual time for each name
$name_totals = array();
// CABLE EFFICIENCY SUMMARY
while ($row = mysqli_fetch_array($sql_total_operator)) {
  $name = $row['emp_name'];

  //GET THE ACTUAL DIRECT LABOR HOURS BASED ON ACTUAL PROCESSED PART per EMPLOYEE
  $sql_result = mysqli_query($conn, "SELECT dtr.Name,dtr.DATE,dtr.Duration AS total_actual_time,dtr.Qty_Make,dtr.Duration/dtr.Qty_Make AS detailed_total_act,dtr.wo_id,dtr.Part_No,dtr.Prod_Order_No,cable_cycletime.cycle_time,(CASE WHEN dtr.Duration/dtr.Qty_Make < cable_cycletime.cycle_time THEN dtr.Duration/dtr.Qty_Make ELSE cable_cycletime.cycle_time END) AS Std_time FROM (SELECT Name,DATE,SUM(Duration) AS Duration,Qty_Make,wo_id,Part_No,Stations,Code,Station_No,Labor_Type,Act_Start,Prod_Order_No FROM dtr WHERE DATE BETWEEN '$date' and '$date' AND Name = '$name' AND Emp_ID NOT IN ('13394','13351','12379') AND Qty_Make > 0 AND wo_status = 'IN-PROCESS' AND Stations!='FG TRANSACTION' AND Labor_Type='Reg_DL' AND Department = 'Cable Assy' GROUP BY Name, Prod_Order_No,Stations) AS dtr LEFT JOIN cable_cycletime ON dtr.Part_No = cable_cycletime.Part_No AND cable_cycletime.station=dtr.Stations GROUP BY dtr.Name, dtr.Prod_Order_No,dtr.Stations ORDER BY dtr.Name, dtr.Act_Start ASC");

  // Initialize a variable to hold the running total and detailed actual time for each Name
  $total_actual_time_by_name = 0;
  $detailed_actual_time = '';
  $total_std_time_by_name = 0;
  $total_actual_time = 0;
  $QTY_ = 0;


  // Loop through the result set and calculate the total actual time and detailed actual time for each Name
  while ($row = mysqli_fetch_assoc($sql_result)) {
    $name = $row['Name'];
    $std_time = $row['Std_time'];
    $qty_make = $row['Qty_Make'];
    $detailed_actual_time = $row['detailed_total_act'];
    $actual_time = $row['total_actual_time'] / 60;
    $total_actual_time += $actual_time;
    $total_actual_time_by_name += $actual_time;
    if ($std_time != 0) {
      $total_time = $std_time * $qty_make / 60;
    } else {
      $total_time = number_format($detailed_actual_time * $qty_make / 60, 2);
    }
    $total_std_time_by_name += $total_time;
    //$QTY_ += number_format($std_time * $qty_make / 60, 2);
  }
  $name_totals[$name] = array(
    'total_actual_time' => number_format($total_actual_time_by_name, 2),
    'total_std_time' => $total_std_time_by_name,
    'detailed_actual_time' => rtrim($detailed_actual_time, '; ')
  );

  // Add the actual_time value to the running total for all names
  $total_actual_time_all_names += $total_actual_time_by_name;
  $total_std_time_all_names += $total_std_time_by_name;
}
if ($total_std_time_all_names && $total_actual_time_all_names != 0) {
  $operator_eff = round(($total_std_time_all_names / $total_actual_time_all_names) * 100, 2);
} else {
  $operator_eff = 0;
}
// Get current hour in 24-hour format
$current_hour = date('H');
// Check if current hour is between 16 (4 PM) and 17 (5 PM)
if ($operator_eff >= 98 && $current_hour >= 16 && $current_hour < 17) {
  // Save the operator efficiency value for later insertion
  $efficiency_to_insert = $operator_eff;
} else {
  // Set to a default value (you can adjust as needed)
  $efficiency_to_insert = 0;
}
// $sql = mysqli_query($conn, "SELECT Name,Qty_Make,cycle_time,SUM(build_percent/100 *Qty_Make) AS build_percent,Stations,description,Part_No,Duration,Prod_Order_No,batch_no,Code,Act_Start,Act_End,remarks,product FROM prod_dtr WHERE DATE BETWEEN '$date' AND '$date' AND Qty_Make > 0 AND product = 'JLP' AND Emp_ID NOT IN ('11451','4444') AND Act_End != '' AND Labor_Type != 'Prod_Reg_ID' GROUP BY Name,description ORDER BY Name ASC;");

// // Create an associative array to store the sum of products for each name
// $productSumByNames = array();
// $stdValues = array();
// $buildValues = array();
// $result = 0;
// $total_headcount = mysqli_num_rows($sql);
// while ($row = mysqli_fetch_assoc($sql)) {
//   $name = $row['Name'];
//   $build = $row['build_percent'];
//   $std = $row['cycle_time'];

//   // If the station is FVI or SUB TEST, set the build percent to 1
//   if ($row['Stations'] == 'FVI MODULE' || $row['Stations'] == 'SUB TEST') {
//     $std_cycle_time = "2.00";
//     if ($build == 1) {
//       $build = '0.89';
//     }
//   } else {
//     $std_cycle_time = $std;
//   }

//   $stdValues[$name] = $std_cycle_time;
//   $buildValues[$name][] = $build;
//   $array1[] = $std_cycle_time;
//   $array2[] = $build;

//   // Calculate the product for the current row
//   $product = $std_cycle_time * $build / 8  * 100;

//   // Check if the name already exists in the associative array
//   if (isset($productSumByNames[$name])) {
//     // Add the product to the existing sum for that name
//     $productSumByNames[$name] += $product;
//   } else {
//     // Initialize the sum for the name
//     $productSumByNames[$name] = $product;
//   }

//   if ($total_headcount != 0) {
//     $WHxMP = $total_headcount * 8;
//   } else {
//     $WHxMP = 0;
//   }


//   $result = round(array_sum(array_map(function ($a, $b) {
//     return $a * $b;
//   }, $array1, $array2)));


//   if ($WHxMP && $result != 0) {
//     $technicians_eff = number_format(($result / $WHxMP) * 100, 2);
//   } else {
//     $technicians_eff = 0;
//   }
// }
// foreach ($productSumByNames as $name => $sumProduct) {

//   $std = $stdValues[$name];
//   $build = $buildValues[$name];
//   $buildSum = array_sum($build);

//   $name;
//   $std;
//   $buildSum;
//   $sumProduct;
// }

function OVRPRODUCTEFF($conn, $date)
{
  $result = [];
  // QUERY FOR HEADCOUNT
  $jlp_hc_sql = mysqli_query($conn, "SELECT COUNT(DISTINCT Name) AS total_headcount FROM prod_dtr WHERE Department = 'Prod Main' AND Act_End != '' AND Name != 'TECHNICIAN' AND product='JLP' AND description != 'INDIRECT ACTIVITY' AND Emp_ID NOT IN ('4444','11451') AND DATE BETWEEN '$date' AND '$date'");
  $jlp_hc_row = mysqli_fetch_assoc($jlp_hc_sql);
  $total_hc_jlp = $jlp_hc_row['total_headcount'];

  // QUERY FOR EMPLOYEES
  $jlp_sql_data = mysqli_query(
    $conn,
    "SELECT Name,Qty_Make,cycle_time,SUM(build_percent/100 *Qty_Make) AS build_percent,Stations,description,Part_No,Duration,Prod_Order_No,batch_no,Code,Act_Start,Act_End,remarks,product FROM prod_dtr WHERE DATE BETWEEN '$date' AND '$date' AND Qty_Make > 0 AND product = 'JLP' AND Emp_ID NOT IN ('11451','4444') AND Act_End != '' AND Labor_Type != 'Prod_Reg_ID' GROUP BY Name,description ORDER BY Name ASC"
  );

  $array1 = []; // Array to store cycle time values
  $array2 = []; // Array to store build percent values
  $result_jlp = [];

  while ($jlp_row = mysqli_fetch_array($jlp_sql_data)) {
    $jlp_std = $jlp_row['cycle_time'];
    $jlp_stations = $jlp_row['Stations'];
    $jlp = $jlp_row['product'];
    $jlp_bp = number_format($jlp_row['build_percent'], 2);
    $jlp_output = $jlp_bp;

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
      'product_jlp' => $jlp, 'jlp_detailed_output' => $jlp_detailed_output //thiss error
    ];
  }
  $result[] = $result_jlp;
  // QUERY FOR HEADCOUNT
  $pnp_hc_sql = mysqli_query($conn, "SELECT COUNT(DISTINCT Name) AS total_headcount FROM prod_dtr WHERE Department = 'Prod Main' AND Act_End != '' AND Name != 'TECHNICIAN' AND product='PNP' AND description != 'INDIRECT ACTIVITY' AND Emp_ID != '11451' AND DATE BETWEEN '$date' AND '$date'");
  $pnp_hc_row = mysqli_fetch_assoc($pnp_hc_sql);
  $total_hc_pnp = $pnp_hc_row['total_headcount'];

  // QUERY FOR EMPLOYEES
  $pnp_sql_data = mysqli_query($conn, "SELECT Name,Qty_Make,cycle_time,SUM(build_percent/100 *Qty_Make) AS build_percent,Stations,description,Part_No,Duration,Prod_Order_No,batch_no,Code,Act_Start,Act_End,remarks,product FROM prod_dtr WHERE DATE BETWEEN '$date' AND '$date' AND Qty_Make > 0 AND product = 'PNP' AND Emp_ID NOT IN ('4444','11451') AND Labor_Type != 'Prod_Reg_ID' GROUP BY Name,description ORDER BY Name ASC");

  $array1 = []; // Array to store cycle time values
  $array2 = []; // Array to store build percent values
  $result_pnp = [];

  while ($pnp_row = mysqli_fetch_array($pnp_sql_data)) {

    $pnp_std = $pnp_row['cycle_time'];
    $pnp_bp = $pnp_row['build_percent'];
    $pnp = $pnp_row['product'];

    $array1[] = $pnp_std;
    $array2[] = $pnp_bp;

    $PNP_STDxOUTPUT = array_sum(array_map(function ($a, $b) {
      return $a * $b;
    }, $array1, $array2));

    $PNP_STDxOUTPUT;
    $PNP_WHxMP = 8 * $total_hc_pnp;
    $PNP_TOTALEFF = $PNP_WHxMP != 0 ? number_format(($PNP_STDxOUTPUT / $PNP_WHxMP) * 100, 2) : 0;

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
$results = OVRPRODUCTEFF($conn, $date, $date);
$result_jlp = $results[0]; // Get JLP results 
$TOTALHC_jlp = null;
$WHxMP_jlp = 0;
$OVERALLEFF_jlp = null;
$STDxOUTPUT_jlp = null;
$TOTALHC_pnp = null;
$WHxMP_pnp = 0;
$OVERALLEFF_pnp = null;
$STDxOUTPUT_pnp = null;
// for future purpose (OLB AND JTP)
$STDxOUTPUT_olb = null;
$STDxOUTPUT_jtp = null;
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
$OVR_STDxOUTPUT = $STDxOUTPUT_jlp + $STDxOUTPUT_pnp + $STDxOUTPUT_olb + $STDxOUTPUT_jtp;
$OVER_WHxMP = round($WHxMP_jlp + $WHxMP_pnp + $WHxMP_olb + $WHxMP_jtp);

if ($OVR_STDxOUTPUT != 0 && $OVER_WHxMP != 0) {
  $OVR_EFF = round($OVR_STDxOUTPUT / $OVER_WHxMP * 100);
} else {
  $OVR_EFF = 0;
}

$WHxMP_Cable = $total_actual_time_all_names; // CABLE PRESENT & WH
$WHxMP_Main = $OVER_WHxMP; // MAIN PRESENT & WH
$STDxOUTPUT_OVERALL = number_format($OVR_STDxOUTPUT + $total_std_time_all_names, 2); // STD X OUTPUT OVR
$WHxMP_OVERALL = $WHxMP_Main + $WHxMP_Cable; // WORKING HR X MP
$STDxOUTPUT_OVERALL;
// FORMULA EFFICIENCY OVERALL
if ($STDxOUTPUT_OVERALL != 0 && $WHxMP_OVERALL != 0) {
  $ALL_EFF = number_format(($STDxOUTPUT_OVERALL / $WHxMP_OVERALL) * 100, 2);
} else {
  $ALL_EFF = 0;
}

// YIELD FORMULA


// $inspect_sql = mysqli_query($conn, "SELECT date_updated, Stations, COUNT(*) AS inspected_count FROM prod_module WHERE Stations IN ('SUB TEST', 'FINAL TESTS', 'FVI MODULE', 'FVI MAC') AND date_updated BETWEEN '2023-05-18' AND '2023-05-18' AND description != 'INDIRECT ACTIVITY' AND wo_status = 'IDLE' GROUP BY date_updated, Stations ORDER BY date_updated ASC");

// while ($row = mysqli_fetch_assoc($inspect_sql)) {
//   $date_updated = $row['date_updated'];
//   $stations = $row['Stations'];
//   $inspected_count = $row['inspected_count'];
//   // echo "date: $date_updated, Count: $inspected_count, station: $stations<br>"; // for debugging purposes
//   $defects = 0;
//   $date_values[$date_updated] = $date_updated;
//   $inspected_counts[$date_updated] = $inspected_count;
//   // Store the station value in the array
//   $station_values[$date_updated][] = $stations;

//   if ($inspected_count && $defects != 0) {
//     $yield = number_format((($inspected_count - $defects) / $inspected_count) * 100, 2);
//   } else {
//     $yield = 100;
//   }
//   $yield_values[$date_updated] = $yield;
// }
// // Now you can access the station values outside the loop
// foreach ($station_values as $date => $stations) {
//   $date;
//   foreach ($stations as $station_value) {
//     echo "Station: $station_value<br>";
//   }
// }

// Array to store the yield,date,inspected values for each date

$station_values = [];
$inspected_count = 0; // Set default value for inspected count
$defects = 0; // Set default value for defects
$yield = 0;
$inspected = 0;

$inspect_sql = mysqli_query($conn, "SELECT date_updated, Stations, COUNT(*) AS inspected_count FROM prod_module WHERE Stations IN ('SUB TEST', 'FINAL TESTS', 'FVI MODULE', 'FVI MAC') AND date_updated BETWEEN '$date' AND '$date' AND description != 'INDIRECT ACTIVITY' AND wo_status = 'IDLE' GROUP BY date_updated, Stations ORDER BY date_updated ASC");

while ($row = mysqli_fetch_assoc($inspect_sql)) {
  $date_updated = $row['date_updated'];
  $stations = $row['Stations'];
  $inspected_count = $row['inspected_count'];
  $inspected += $inspected_count;

  // Calculate the yield value
  if ($inspected != 0) {
    $yield = number_format((($inspected - $defects) / $inspected) * 100, 2);
  } else {
    $yield = 0;
  }

  // Store the station value in the array
  $station_values[$date_updated][] = array(
    'station' => ($stations != '') ? $stations : "0",
    'count' => $inspected_count,
    'yield' => $yield
  );
}



// LEAD TIME FORMULA
$leadtime_sql = "SELECT batch_no, date_received, DATEDIFF(NOW(), date_received) - (2 * (DATEDIFF(NOW(), date_received) DIV 7)) - CASE WHEN DAYOFWEEK(NOW()) = 1 THEN 1 WHEN DAYOFWEEK(NOW()) = 7 THEN 2 ELSE 0 END AS No_Days FROM `prod_module` WHERE product = 'JLP' AND description != 'INDIRECT ACTIVITY' AND work_station NOT IN ('WS11', 'WS11.1', 'WS3.1', 'WS2.1', 'WS12', 'WS13') GROUP BY batch_no ORDER BY batch_no ASC";
$leadtime_result = $conn->query($leadtime_sql);

$dateReceived = array();
$batchNumbers = array();
$noDays = array();

// Process the data from the database and store it in arrays
if ($leadtime_result->num_rows > 0) {
  while ($row = $leadtime_result->fetch_assoc()) {
    $dateReceived[] = date('Y-m-d', strtotime($row['date_received']));
    $batchNumbers[] = $row['batch_no'];
    $noDays[] = $row['No_Days'];
  }
}


?>
<?php
$mainmatrix_query = "SELECT
  SUM(CASE WHEN CDA = 1 THEN 1 ELSE 0 END) AS CDA_Count_1,
  SUM(CASE WHEN CDA = 2 THEN 1 ELSE 0 END) AS CDA_Count_2,
  SUM(CASE WHEN CDA = 3 THEN 1 ELSE 0 END) AS CDA_Count_3,
  COUNT(CASE WHEN CDA IN (1, 2, 3) THEN Name END) AS CDA_Total,
  SUM(CASE WHEN CDM = 1 THEN 1 ELSE 0 END) AS CDM_Count_1,
  SUM(CASE WHEN CDM = 2 THEN 1 ELSE 0 END) AS CDM_Count_2,
  SUM(CASE WHEN CDM = 3 THEN 1 ELSE 0 END) AS CDM_Count_3,
  COUNT(CASE WHEN CDM IN (1, 2, 3) THEN Name END) AS CDM_Total,
  SUM(CASE WHEN TSL = 1 THEN 1 ELSE 0 END) AS TSL_Count_1,
  SUM(CASE WHEN TSL = 2 THEN 1 ELSE 0 END) AS TSL_Count_2,
  SUM(CASE WHEN TSL = 3 THEN 1 ELSE 0 END) AS TSL_Count_3,
  COUNT(CASE WHEN TSL IN (1, 2, 3) THEN Name END) AS TSL_Total,
  SUM(CASE WHEN FA = 1 THEN 1 ELSE 0 END) AS FA_Count_1,
  SUM(CASE WHEN FA = 2 THEN 1 ELSE 0 END) AS FA_Count_2,
  SUM(CASE WHEN FA = 3 THEN 1 ELSE 0 END) AS FA_Count_3,
  COUNT(CASE WHEN FA IN (1, 2, 3) THEN Name END) AS FA_Total,
  SUM(CASE WHEN TXP = 1 THEN 1 ELSE 0 END) AS TXP_Count_1,
  SUM(CASE WHEN TXP = 2 THEN 1 ELSE 0 END) AS TXP_Count_2,
  SUM(CASE WHEN TXP = 3 THEN 1 ELSE 0 END) AS TXP_Count_3,
  COUNT(CASE WHEN TXP IN (1, 2, 3) THEN Name END) AS TXP_Total,
  SUM(CASE WHEN AC = 1 THEN 1 ELSE 0 END) AS AC_Count_1,
  SUM(CASE WHEN AC = 2 THEN 1 ELSE 0 END) AS AC_Count_2,
  SUM(CASE WHEN AC = 3 THEN 1 ELSE 0 END) AS AC_Count_3,
  COUNT(CASE WHEN AC IN (1, 2, 3) THEN Name END) AS AC_Total,
  SUM(CASE WHEN FC = 1 THEN 1 ELSE 0 END) AS FC_Count_1,
  SUM(CASE WHEN FC = 2 THEN 1 ELSE 0 END) AS FC_Count_2,
  SUM(CASE WHEN FC = 3 THEN 1 ELSE 0 END) AS FC_Count_3,
  COUNT(CASE WHEN FC IN (1, 2, 3) THEN Name END) AS FC_Total,
  SUM(CASE WHEN MTP = 1 THEN 1 ELSE 0 END) AS MTP_Count_1,
  SUM(CASE WHEN MTP = 2 THEN 1 ELSE 0 END) AS MTP_Count_2,
  SUM(CASE WHEN MTP = 3 THEN 1 ELSE 0 END) AS MTP_Count_3,
  COUNT(CASE WHEN MTP IN (1, 2, 3) THEN Name END) AS MTP_Total,
  SUM(CASE WHEN ION = 1 THEN 1 ELSE 0 END) AS ION_Count_1,
  SUM(CASE WHEN ION = 2 THEN 1 ELSE 0 END) AS ION_Count_2,
  SUM(CASE WHEN ION = 3 THEN 1 ELSE 0 END) AS ION_Count_3,
  COUNT(CASE WHEN ION IN (1, 2, 3) THEN Name END) AS ION_Total,
  SUM(CASE WHEN FLIP = 1 THEN 1 ELSE 0 END) AS FLIP_Count_1,
  SUM(CASE WHEN FLIP = 2 THEN 1 ELSE 0 END) AS FLIP_Count_2,
  SUM(CASE WHEN FLIP = 3 THEN 1 ELSE 0 END) AS FLIP_Count_3,
  COUNT(CASE WHEN FLIP IN (1, 2, 3) THEN Name END) AS FLIP_Total,
  SUM(CASE WHEN INTEGRATION = 1 THEN 1 ELSE 0 END) AS INTEGRATION_Count_1,
  SUM(CASE WHEN INTEGRATION = 2 THEN 1 ELSE 0 END) AS INTEGRATION_Count_2,
  SUM(CASE WHEN INTEGRATION = 3 THEN 1 ELSE 0 END) AS INTEGRATION_Count_3,
  COUNT(CASE WHEN INTEGRATION IN (1, 2, 3) THEN Name END) AS INTEGRATION_Total,
  SUM(CASE WHEN PNP_SUB_ASSY = 1 THEN 1 ELSE 0 END) AS PNP_SUB_ASSY_Count_1,
  SUM(CASE WHEN PNP_SUB_ASSY = 2 THEN 1 ELSE 0 END) AS PNP_SUB_ASSY_Count_2,
  SUM(CASE WHEN PNP_SUB_ASSY = 3 THEN 1 ELSE 0 END) AS PNP_SUB_ASSY_Count_3,
  COUNT(CASE WHEN PNP_SUB_ASSY IN (1, 2, 3) THEN Name END) AS PNP_SUB_ASSY_Total,
  SUM(CASE WHEN PNP_INT = 1 THEN 1 ELSE 0 END) AS PNP_INT_Count_1,
  SUM(CASE WHEN PNP_INT = 2 THEN 1 ELSE 0 END) AS PNP_INT_Count_2,
  SUM(CASE WHEN PNP_INT = 3 THEN 1 ELSE 0 END) AS PNP_INT_Count_3,
  COUNT(CASE WHEN PNP_INT IN (1, 2, 3) THEN Name END) AS PNP_INT_Total,
  SUM(CASE WHEN OLB_MAIN = 1 THEN 1 ELSE 0 END) AS OLB_MAIN_Count_1,
  SUM(CASE WHEN OLB_MAIN = 2 THEN 1 ELSE 0 END) AS OLB_MAIN_Count_2,
  SUM(CASE WHEN OLB_MAIN = 3 THEN 1 ELSE 0 END) AS OLB_MAIN_Count_3,
  COUNT(CASE WHEN OLB_MAIN IN (1, 2, 3) THEN Name END) AS OLB_MAIN_Total,
  SUM(CASE WHEN ABLP = 1 THEN 1 ELSE 0 END) AS ABLP_Count_1,
  SUM(CASE WHEN ABLP = 2 THEN 1 ELSE 0 END) AS ABLP_Count_2,
  SUM(CASE WHEN ABLP = 3 THEN 1 ELSE 0 END) AS ABLP_Count_3,
  COUNT(CASE WHEN ABLP IN (1, 2, 3) THEN Name END) AS ABLP_Total,
  SUM(CASE WHEN OLB_F_INT = 1 THEN 1 ELSE 0 END) AS OLB_F_INT_Count_1,
  SUM(CASE WHEN OLB_F_INT = 2 THEN 1 ELSE 0 END) AS OLB_F_INT_Count_2,
  SUM(CASE WHEN OLB_F_INT = 3 THEN 1 ELSE 0 END) AS OLB_F_INT_Count_3,
  COUNT(CASE WHEN OLB_F_INT IN (1, 2, 3) THEN Name END) AS OLB_F_INT_Total,
  SUM(CASE WHEN SUB_TEST = 1 THEN 1 ELSE 0 END) AS SUB_TEST_Count_1,
  SUM(CASE WHEN SUB_TEST = 2 THEN 1 ELSE 0 END) AS SUB_TEST_Count_2,
  SUM(CASE WHEN SUB_TEST = 3 THEN 1 ELSE 0 END) AS SUB_TEST_Count_3,
  COUNT(CASE WHEN SUB_TEST IN (1, 2, 3) THEN Name END) AS SUB_TEST_Total,
  SUM(CASE WHEN FINAL_TEST = 1 THEN 1 ELSE 0 END) AS FINAL_TEST_Count_1,
  SUM(CASE WHEN FINAL_TEST = 2 THEN 1 ELSE 0 END) AS FINAL_TEST_Count_2,
  SUM(CASE WHEN FINAL_TEST = 3 THEN 1 ELSE 0 END) AS FINAL_TEST_Count_3,
  COUNT(CASE WHEN FINAL_TEST IN (1, 2, 3) THEN Name END) AS FINAL_TEST_Total
FROM prod_skills_matrix";

$cablematrix_query = "SELECT
  SUM(CASE WHEN MCUTTING = 1 THEN 1 ELSE 0 END) AS MCUTTING_Count_1,
  SUM(CASE WHEN MCUTTING = 2 THEN 1 ELSE 0 END) AS MCUTTING_Count_2,
  SUM(CASE WHEN MCUTTING = 3 THEN 1 ELSE 0 END) AS MCUTTING_Count_3,
  COUNT(CASE WHEN MCUTTING IN (1, 2, 3) THEN Name END) AS MCUTTING_Total,
  SUM(CASE WHEN MSTRIPPING = 1 THEN 1 ELSE 0 END) AS MSTRIPPING_Count_1,
  SUM(CASE WHEN MSTRIPPING = 2 THEN 1 ELSE 0 END) AS MSTRIPPING_Count_2,
  SUM(CASE WHEN MSTRIPPING = 3 THEN 1 ELSE 0 END) AS MSTRIPPING_Count_3,
  COUNT(CASE WHEN MSTRIPPING IN (1, 2, 3) THEN Name END) AS MSTRIPPING_Total,
  SUM(CASE WHEN MCRIMPING = 1 THEN 1 ELSE 0 END) AS MCRIMPING_Count_1,
  SUM(CASE WHEN MCRIMPING = 2 THEN 1 ELSE 0 END) AS MCRIMPING_Count_2,
  SUM(CASE WHEN MCRIMPING = 3 THEN 1 ELSE 0 END) AS MCRIMPING_Count_3,
  COUNT(CASE WHEN MCRIMPING IN (1, 2, 3) THEN Name END) AS MCRIMPING_Total,
  SUM(CASE WHEN SAWC = 1 THEN 1 ELSE 0 END) AS SAWC_Count_1,
  SUM(CASE WHEN SAWC = 2 THEN 1 ELSE 0 END) AS SAWC_Count_2,
  SUM(CASE WHEN SAWC = 3 THEN 1 ELSE 0 END) AS SAWC_Count_3,
  COUNT(CASE WHEN SAWC IN (1, 2, 3) THEN Name END) AS SAWC_Total,
  SUM(CASE WHEN MsU = 1 THEN 1 ELSE 0 END) AS MsU_Count_1,
  SUM(CASE WHEN MsU = 2 THEN 1 ELSE 0 END) AS MsU_Count_2,
  SUM(CASE WHEN MsU = 3 THEN 1 ELSE 0 END) AS MsU_Count_3,
  COUNT(CASE WHEN MsU IN (1, 2, 3) THEN Name END) AS MsU_Total,
  SUM(CASE WHEN SOLDERING = 1 THEN 1 ELSE 0 END) AS SOLDERING_Count_1,
  SUM(CASE WHEN SOLDERING = 2 THEN 1 ELSE 0 END) AS SOLDERING_Count_2,
  SUM(CASE WHEN SOLDERING = 3 THEN 1 ELSE 0 END) AS SOLDERING_Count_3,
  COUNT(CASE WHEN SOLDERING IN (1, 2, 3) THEN Name END) AS SOLDERING_Total,
  SUM(CASE WHEN MOLDING = 1 THEN 1 ELSE 0 END) AS MOLDING_Count_1,
  SUM(CASE WHEN MOLDING = 2 THEN 1 ELSE 0 END) AS MOLDING_Count_2,
  SUM(CASE WHEN MOLDING = 3 THEN 1 ELSE 0 END) AS MOLDING_Count_3,
  COUNT(CASE WHEN MOLDING IN (1, 2, 3) THEN Name END) AS MOLDING_Total,
  SUM(CASE WHEN WHARNESS = 1 THEN 1 ELSE 0 END) AS WHARNESS_Count_1,
  SUM(CASE WHEN WHARNESS = 2 THEN 1 ELSE 0 END) AS WHARNESS_Count_2,
  SUM(CASE WHEN WHARNESS = 3 THEN 1 ELSE 0 END) AS WHARNESS_Count_3,
  COUNT(CASE WHEN WHARNESS IN (1, 2, 3) THEN Name END) AS WHARNESS_Total,
  SUM(CASE WHEN FINALASSY = 1 THEN 1 ELSE 0 END) AS FINALASSY_Count_1,
  SUM(CASE WHEN FINALASSY = 2 THEN 1 ELSE 0 END) AS FINALASSY_Count_2,
  SUM(CASE WHEN FINALASSY = 3 THEN 1 ELSE 0 END) AS FINALASSY_Count_3,
  COUNT(CASE WHEN FINALASSY IN (1, 2, 3) THEN Name END) AS FINALASSY_Total,
  SUM(CASE WHEN MCO = 1 THEN 1 ELSE 0 END) AS MCO_Count_1,
  SUM(CASE WHEN MCO = 2 THEN 1 ELSE 0 END) AS MCO_Count_2,
  SUM(CASE WHEN MCO = 3 THEN 1 ELSE 0 END) AS MCO_Count_3,
  COUNT(CASE WHEN MCO IN (1, 2, 3) THEN Name END) AS MCO_Total,
  SUM(CASE WHEN LABELLING = 1 THEN 1 ELSE 0 END) AS LABELLING_Count_1,
  SUM(CASE WHEN LABELLING = 2 THEN 1 ELSE 0 END) AS LABELLING_Count_2,
  SUM(CASE WHEN LABELLING = 3 THEN 1 ELSE 0 END) AS LABELLING_Count_3,
  COUNT(CASE WHEN LABELLING IN (1, 2, 3) THEN Name END) AS LABELLING_Total,
  SUM(CASE WHEN ETESTING = 1 THEN 1 ELSE 0 END) AS ETESTING_Count_1,
  SUM(CASE WHEN ETESTING = 2 THEN 1 ELSE 0 END) AS ETESTING_Count_2,
  SUM(CASE WHEN ETESTING = 3 THEN 1 ELSE 0 END) AS ETESTING_Count_3,
  COUNT(CASE WHEN ETESTING IN (1, 2, 3) THEN Name END) AS ETESTING_Total,
  SUM(CASE WHEN VI = 1 THEN 1 ELSE 0 END) AS VI_Count_1,
  SUM(CASE WHEN VI = 2 THEN 1 ELSE 0 END) AS VI_Count_2,
  SUM(CASE WHEN VI = 3 THEN 1 ELSE 0 END) AS VI_Count_3,
  COUNT(CASE WHEN VI IN (1, 2, 3) THEN Name END) AS VI_Total,
  SUM(CASE WHEN PB = 1 THEN 1 ELSE 0 END) AS PB_Count_1,
  SUM(CASE WHEN PB = 2 THEN 1 ELSE 0 END) AS PB_Count_2,
  SUM(CASE WHEN PB = 3 THEN 1 ELSE 0 END) AS PB_Count_3,
  COUNT(CASE WHEN PB IN (1, 2, 3) THEN Name END) AS PB_Total,
  SUM(CASE WHEN TAPING = 1 THEN 1 ELSE 0 END) AS TAPING_Count_1,
  SUM(CASE WHEN TAPING = 2 THEN 1 ELSE 0 END) AS TAPING_Count_2,
  SUM(CASE WHEN TAPING = 3 THEN 1 ELSE 0 END) AS TAPING_Count_3,
  COUNT(CASE WHEN TAPING IN (1, 2, 3) THEN Name END) AS TAPING_Total
FROM prod_skills_matrix_cable";

// Execute the query
$main_lvl_statement = mysqli_prepare($conn, $mainmatrix_query);
mysqli_stmt_execute($main_lvl_statement);
$main_lvl_result = mysqli_stmt_get_result($main_lvl_statement);

$cable_lvl_statement = mysqli_prepare($conn, $cablematrix_query);
mysqli_stmt_execute($cable_lvl_statement);
$cable_lvl_result = mysqli_stmt_get_result($cable_lvl_statement);
// Fetch the row from the query result
$main_row = mysqli_fetch_assoc($main_lvl_result);
$cable_row = mysqli_fetch_assoc($cable_lvl_result);

// Define the Level arrays
$mainlevel1Values = array();
$mainlevel2Values = array();
$mainlevel3Values = array();
$maintargetValues = array('4', '4', '4', '4', '4', '4', '4', '6', '5', '4', '6', '8');

$cablelevel1Values = array();
$cablelevel2Values = array();
$cablelevel3Values = array();

// Iterate over the column names and retrieve the Level values
$main_columns = array('FC', 'CDM', 'TSL', 'AC', 'CDA', 'TXP', 'FA', 'INTEGRATION', 'PNP_SUB_ASSY', 'PNP_INT', 'SUB_TEST', 'FINAL_TEST');

$cable_columns = array('MCUTTING', 'MSTRIPPING', 'MCRIMPING', 'SAWC', 'MsU', 'SOLDERING', 'MOLDING', 'WHARNESS', 'FINALASSY', 'MCO', 'LABELLING', 'ETESTING', 'VI', 'PB', 'TAPING');

foreach ($main_columns as $column) {
  $mainlevel1Values[$column] = $main_row[$column . '_Count_1'];
  $mainlevel2Values[$column] = $main_row[$column . '_Count_2'];
  $mainlevel3Values[$column] = $main_row[$column . '_Count_3'];
}
foreach ($cable_columns as $column) {
  $cablelevel1Values[$column] = $cable_row[$column . '_Count_1'];
  $cablelevel2Values[$column] = $cable_row[$column . '_Count_2'];
  $cablelevel3Values[$column] = $cable_row[$column . '_Count_3'];
}

$maindescriptionMap = [
  'ASSY, FACILITY CABINET, JLP-G3' => 'Facility Cabinet',
  'ASSY CART DOCKING MECH-JLP G3' => 'CDM',
  'LIFT ASSY,TRAY STACK-JLP G3' => 'LIFT',
  'ASSY, CONVEYOR JLP-G3' => 'Assy Conveyor',
  'CONVEYOR DRIVE ASSY,JLP G3' => 'CDA',
  'TRAY TRANSPORT ASSY,JLP G3' => 'Tray Transport',
  'FRAME ASSY, JLP G3' => 'Frame Assy',
  'JLP GEN 3, MAIN ASSEMBLY' => 'Final Integration',
  'Matrix Sub-Assy',
  'Matrix Integration',
  'Sub Test',
  'Final Test'
];

$renamedColumns = array_map(function ($column) use ($maindescriptionMap) {
  return isset($maindescriptionMap[$column]) ? $maindescriptionMap[$column] : $column;
}, $main_columns);

$shortcutNames = [];

foreach ($maindescriptionMap as $shortcut) {
  $shortcutNames[] = $shortcut;
}
// // Output the Level arrays for testing
// echo "Level 1:\n";
// print_r($level1Values);

// echo "Level 2:\n";
// print_r($level2Values);

// echo "Level 3:\n";
// print_r($level3Values);

// echo "Target: \n";
// print_r($targetValues);

// BUILD STATUS
$build_status_query = "SELECT
    psa.batch_no,
    psa.description,
    IFNULL(psa.sub_count, 0) AS sub_count,
    ROUND(IFNULL(psa.subassy /7,0)) AS sub_bp,
    IFNULL(psa.subassy, 0) AS subassy,
    IFNULL(pma.main_count, 0) AS main_count,
    IFNULL(pma.mainassy, 0) AS mainassy,
    IFNULL(pt.test_count, 0) AS test_count,
	  IFNULL(pt.testing, 0) AS testing,
    ROUND((IFNULL(psa.subassy, 0) + IFNULL(pma.mainassy, 0) + IFNULL(pt.testing, 0)) / 9) AS total_count
FROM
    (SELECT
        pm.batch_no,
        pm.description,
        COUNT(pm.description) AS sub_count,
        ROUND(SUM(pm.build_percent), 2) AS subassy
    FROM
        prod_module pm
    WHERE
        pm.product = 'JLP'
        AND pm.description NOT IN ('PANEL ASSY, JLP G3', 'INDIRECT ACTIVITY')
        AND pm.work_station NOT IN ('WS11', 'WS11.1', 'WS12', 'WS13', '')
    GROUP BY
        pm.batch_no) AS psa
LEFT JOIN
    (SELECT
        pm.batch_no,
        COUNT(pm.description) AS main_count,
        SUM(pm.build_percent) AS mainassy
    FROM
        prod_module pm
    WHERE
        pm.product = 'JLP'
        AND pm.description = 'JLP GEN 3, MAIN ASSEMBLY'
    GROUP BY
        pm.batch_no) AS pma
ON
    psa.batch_no = pma.batch_no
LEFT JOIN
    (SELECT
        pm.batch_no,
        COUNT(pm.description) AS test_count,
     	SUM(pm.build_percent) AS testing
    FROM
        prod_module pm
    WHERE
        pm.product = 'JLP'
        AND pm.description != 'JLP GEN 3, MAIN ASSEMBLY'
        AND pm.work_station = ''
    GROUP BY
        pm.batch_no) AS pt
ON
    psa.batch_no = pt.batch_no;";

// Prepare the statement
$stmt = $conn->prepare($build_status_query);

// Execute the statement
$stmt->execute();

// Fetch the data
$build_status_data = array();
$build_status_result = $stmt->get_result();
while ($build_status_row = $build_status_result->fetch_assoc()) {
  $build_status_data[] = $build_status_row;
}
?>
