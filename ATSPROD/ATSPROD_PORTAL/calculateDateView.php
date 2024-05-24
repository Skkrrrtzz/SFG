<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="/ATS/ATSPROD_PORTAL/assets/css/bootstrap.min.css">
    <link rel="stylesheet" href="/ATS/ATSPROD_PORTAL/assets/css/fontawesome-6.3.0/css/all.min.css">
</head>

<body>
    <div class="container m-5">
        <form class="row g-3 align-items-center" id="dateForm" action="cal_date.php" method="post">
            <div class="col-md-2">
                <label for="batchNo" class="form-label">Batch Number</label>
                <input type="text" class="form-control" name="batchNo" id="batchNo">
            </div>
            <div class="col-md-2">
                <label for="givenCycletime" class="form-label">Cycle Time (Days)</label>
                <input type="text" class="form-control" name="givenCycletime" id="givenCycletime">
            </div>
            <div class="col-md-2">
                <label for="startDate" class="form-label">Start Date</label>
                <input type="date" class="form-control" name="startDate" id="startDate">
            </div>
            <div class="col-md-2">
                <label for="startTime" class="form-label">Start Time</label>
                <input type="time" class="form-control" name="startTime" id="startTime">
            </div>
            <div class="col-md-2 d-flex justify-content-between text-nowrap">
                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="inlineRadioOptions" id="regWH" value="regWH">
                    <label class="form-check-label" for="regWH">Regular WH</label>
                </div>

                <div class="form-check form-check-inline">
                    <input class="form-check-input" type="radio" name="inlineRadioOptions" id="otWH" value="otWH">
                    <label class="form-check-label" for="otWH">OT WH</label>
                </div>
            </div>
            <div class="d-flex justify-content-center text-center col-md-2 my-auto">
                <button type="button" class="btn btn-primary" onclick="calculateDate()">Calculate</button>
            </div>
        </form>
        <div class="table-responsive my-2">
            <table class="table table-bordered w-75">
                <thead class="table-primary">
                    <tr>
                        <th>CYCLE TIME (Days)</th>
                        <th>WORKING HRS</th>
                        <th>START DATE</th>
                        <th>START TIME</th>
                        <th>END DATE</th>
                        <th>END TIME</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>
    <script src="/ATS/ATSPROD_PORTAL/assets/js/jquery-3.6.0.min.js"></script>
    <script src="/ATS/ATSPROD_PORTAL/assets/js/bootstrap.bundle.min.js"></script>

    <script>
        function calculateDate() {
            // Get form data
            var formData = $('#dateForm').serialize();

            $.ajax({
                type: "POST",
                url: "calculateDate.php",
                data: formData,
                success: function(response) {
                    var data = JSON.parse(response);
                    updateTable(data);
                },
                error: function(xhr, status, error) {
                    console.log("Error:", error);
                }
            });
        }

        function updateTable(response) {
            var tableBody = $('tbody');
            tableBody.empty(); // Clear existing rows

            // Convert time to 12-hour format with AM/PM
            var startTime = formatTime(response.startTime);
            var endTime = formatTime(response.endTime);

            // Append a new row using the response properties
            tableBody.append('<tr><td>' + response.cycleTimeInDays + '</td><td>' + response.workingHrs + '</td><td>' + response.startDate + '</td><td>' + startTime + '</td><td>' + response.endDate + '</td><td>' + endTime + '</td></tr>');
        }

        function formatTime(timeString) {
            var time = new Date('2000-01-01T' + timeString); // Use a consistent date to make sure time is parsed correctly
            var hours = time.getHours();
            var minutes = time.getMinutes();
            var ampm = hours >= 12 ? 'PM' : 'AM';

            // Convert to 12-hour format
            hours = hours % 12;
            hours = hours ? hours : 12; // The hour '0' should be '12'

            // Add leading zero to minutes if needed
            minutes = minutes < 10 ? '0' + minutes : minutes;

            return hours + ':' + minutes + ' ' + ampm;
        }
    </script>
</body>

</html>