<!DOCTYPE html>
<html>

<head>
    <title>Load Excel Sheet in Browser using PHPSpreadsheet</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js" integrity="sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz" crossorigin="anonymous"></script>
</head>

<body>
    <div class="container-fluid">
        <br />
        <h3 align="center">Load Excel Sheet in Browser using PHPSpreadsheet</h3>
        <br />
        <form method="post" id="load_excel_form" enctype="multipart/form-data">
            <table class="table">
                <tr>
                    <td width="25%" align="right">Select Excel File</td>
                    <td width="50%"><input type="file" name="select_excel" /></td>
                    <td width="25%"><input type="submit" name="load" class="btn btn-primary" /></td>
                </tr>
            </table>
        </form><span id="message"></span>
        <div id="excel_area"></div>
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
</body>

</html>
<script>
    $(document).ready(function() {
        $('#load_excel_form').on('submit', function(event) {
            event.preventDefault();
            $.ajax({
                url: "new.php",
                method: "POST",
                data: new FormData(this),
                contentType: false,
                cache: false,
                processData: false,
                success: function(data) {
                    $('#excel_area').html(data);
                    $('table').css('width', '100%');
                }
            })
        });
    });
</script>