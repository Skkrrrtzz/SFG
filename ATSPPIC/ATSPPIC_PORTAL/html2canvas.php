<!DOCTYPE html>
<html>

<head>
    <title>Excel to Image</title>
    <style>
        table {
            border-collapse: collapse;
        }

        td {
            border: 1px solid black;
            padding: 5px;
        }
    </style>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.3.2/html2canvas.min.js"></script>
</head>

<body>
    <div id="excelTable">
        <?php include('excel.php'); ?>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function() {
            html2canvas(document.getElementById('excelTable')).then(function(canvas) {
                var imageData = canvas.toDataURL();
                var img = new Image();
                img.src = imageData;
                document.body.appendChild(img);
            });
        });
    </script>
</body>

</html>