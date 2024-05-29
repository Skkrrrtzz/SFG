<?php include 'ATSPPIC_header.php';
include_once 'dbconnection.php'; ?>
<?php

if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

if (isset($_POST['login_prod'])) {
  $username = $_POST['prod_uname'];
  $password = $_POST['prod_pw'];

  // Perform any necessary validation and sanitization of the username and password here

  $sql = "SELECT * FROM user WHERE username = '$username' AND password = '$password'";
  $result = $conn->query($sql);

  if ($result->num_rows > 0) {
    // Fetch the associative array for each row in the result set
    while ($row = $result->fetch_assoc()) {
      // User authentication successful
      $_SESSION['Name'] = $row["emp_name"];
      $_SESSION['Department'] = $row["department"];
      $_SESSION['Emp_ID'] = $row["username"];
    }
    // You can redirect the user to another page or perform any other desired actions
    header('location:PPIC_PORTAL.php');
  } else {
    // User authentication failed
    wp();
  }
}

$conn->close();
?>
<?php
function wp()
{ ?>
  <script>
    Swal.fire({
      icon: 'warning',
      title: 'Invalid',
      text: 'Invalid Password!',
      showConfirmButton: false,
      timer: 1500
    })
  </script>
<?php
}
function login()
{ ?>
  <script>
    Swal.fire({
      icon: 'success',
      title: 'Login Successfully!',
      showConfirmButton: false,
      timer: 1500
    }).then(function() {
      window.location.href = "sup_cable_main.php";
    });
  </script>
<?php
} ?>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8" />
  <meta http-equiv="X-UA-Compatible" content="IE=edge" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>PORTAL LOGIN</title>
  <link href="https://fonts.googleapis.com/css2?family=Roboto+Slab:wght@300;400;700&display=swap" rel="stylesheet">
</head>
<!-- <style>
  /* Apply Roboto Slab font to specific elements */
  .custom-heading {
    font-family: 'Roboto Slab', serif;
    font-weight: bold;
  }
</style> -->
<!-- <style>
  html,
  body {
    padding: 0px;
    margin: 0px;
    background: #f2f2f2;
    font-family: "Raleway", sans-serif;
    color: #FFF;
    height: 100%;
  }

  .containers {
    min-height: 300px;
    max-width: 350px;
    margin: 40px auto;
    background: #FFF;
    border-radius: 4px;
    box-shadow: 0px 2px 3px rgba(0, 0, 0, 0.2);
    display: flex;
    flex-direction: column;
    overflow: hidden;
    -webkit-animation: hi 0.5s;
    animation: hi 0.5s;
    transform: translateZ(0px);

  }

  .containers * {
    box-sizing: border-box;
  }

  .pages {
    flex: 1;
    white-space: nowrap;
    position: relative;
    transition: all 0.4s;
    display: flex;
  }

  .pages .page {
    min-width: 100%;
    padding: 20px 15px;
    padding-top: 0px;
    background: linear-gradient(to left, #955DFF, #6FAAFF);
  }

  .pages .page:nth-of-type(1) .input {
    transform: translateX(-100%) scale(0.5);
  }

  .pages .page:nth-of-type(2) .input {
    transform: translateX(100%) scale(0.5);
  }

  .pages .page .input {
    transition: all 1s;
    opacity: 0;
    transition-delay: 0s;
  }

  .pages .page.signup {
    background: linear-gradient(to left, #6FAAFF, #955DFF);
  }

  .pages .page .title {
    margin-bottom: 10px;
    font-size: 14px;
    position: relative;
    line-height: 14px;
  }

  .pages .page .title i {
    vertical-align: text-bottom;
    font-size: 19px;
  }

  .pages .page .input {
    margin-top: 20px;
  }

  .pages .page input.text {
    background: #F6F7F9;
    border: none;
    border-radius: 4px;
    width: 100%;
    height: 40px;
    line-height: 40px;
    padding: 0px 10px;
    color: rgba(0, 0, 0, 0.5);
    outline: none;
  }

  .pages .page input[type=submit] {
    background: rgba(0, 0, 0, 0.5);
    color: #F6F7F9;
    height: 40px;
    line-height: 40px;
    width: 100%;
    border: none;
    border-radius: 4px;
    font-weight: 600;
  }

  .tabs {
    max-height: 50px;
    height: 50px;
    display: flex;
    background: #FFF;
  }

  .tabs .tab {
    flex: 1;
    color: #5D708A;
    text-align: center;
    line-height: 50px;
    transition: all 0.2s;
  }

  .tabs .tab .text {
    font-size: 14px;
    transform: scale(1);
    transition: all 0.2s;
  }

  input[type=radio] {
    display: none;
  }

  input[type=radio]:nth-of-type(1):checked~.tabs .tab:nth-of-type(1) {
    box-shadow: inset -3px 2px 5px rgba(0, 0, 0, 0.25);
    color: #3F4C7F;
  }

  input[type=radio]:nth-of-type(1):checked~.tabs .tab:nth-of-type(1) .text {
    transform: scale(0.9);
  }

  input[type=radio]:nth-of-type(1):checked~.pages {
    transform: translateX(0%);
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input {
    opacity: 1;
    transform: translateX(0%);
    transition: all 0.5s;
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input:nth-child(1) {
    transition-delay: 0.2s;
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input:nth-child(2) {
    transition-delay: 0.4s;
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input:nth-child(3) {
    transition-delay: 0.6s;
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input:nth-child(4) {
    transition-delay: 0.8s;
  }

  input[type=radio]:nth-of-type(1):checked~.pages .page:nth-of-type(1) .input:nth-child(5) {
    transition-delay: 1s;
  }

  input[type=radio]:nth-of-type(2):checked~.tabs .tab:nth-of-type(2) {
    box-shadow: inset 3px 2px 5px rgba(0, 0, 0, 0.25);
    color: #3F4C7F;
  }

  input[type=radio]:nth-of-type(2):checked~.tabs .tab:nth-of-type(2) .text {
    transform: scale(0.9);
  }

  input[type=radio]:nth-of-type(2):checked~.pages {
    transform: translateX(-100%);
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input {
    opacity: 1;
    transform: translateX(0%);
    transition: all 0.5s;
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input:nth-child(1) {
    transition-delay: 0.2s;
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input:nth-child(2) {
    transition-delay: 0.4s;
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input:nth-child(3) {
    transition-delay: 0.6s;
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input:nth-child(4) {
    transition-delay: 0.8s;
  }

  input[type=radio]:nth-of-type(2):checked~.pages .page:nth-of-type(2) .input:nth-child(5) {
    transition-delay: 1s;
  }

  @-webkit-keyframes hi {
    from {
      transform: translateY(50%) scale(0, 0);
      opacity: 0;
    }
  }

  @keyframes hi {
    from {
      transform: translateY(50%) scale(0, 0);
      opacity: 0;
    }
  }

  .bg-myblue {
    --my-blue: #2706F0;
    background-color: var(--my-blue);
  }
</style> -->

<body>
  <div class="container-fluid w-75 mt-2">
    <div class="card shadow">
      <div class="row mx-0">
        <div class="col-sm">
          <img src="/ATS/ATSPROD_PORTAL/assets/images/Mobile login-rafiki.png" alt="login" class="img-fluid">
        </div>
        <div class="col-sm bg-primary d-flex align-items-center justify-content-center"> <!-- Added flex classes -->
          <div class="card m-2 bg-white">
            <form class="m-2" action="" method="POST">
              <h4 class="m-2 text-dark-subtle fw-bold text-center"> Log In </h4> <!-- Added text-center class -->
              <div class="input-group mb-3">
                <span class="input-group-text"><i class="fa fa-user"></i></span>
                <div class="form-floating">
                  <input type="text" class="form-control" id="username" name="prod_uname" placeholder="Enter Username" required>
                  <label for="username">Enter Username</label>
                </div>
              </div>
              <div class="input-group mb-3">
                <span class="input-group-text"><i class="fa fa-lock"></i></span>
                <div class="form-floating password-input">
                  <input type="password" class="form-control" name="prod_pw" id="password" placeholder="Password" required>
                  <label for="password">Enter Password</label>
                </div>
                <span class="input-group-text" id="togglePassword"><i class="fa-regular fa-eye-slash"></i></span>
              </div>
              <div class="form-check mt-3">
                <input type="checkbox" checked="checked" name="remember" class="form-check-input">
                <label class="form-check-label">Remember me</label>
              </div>
              <button class="btn btn-primary w-100 p-2" type="submit" name="login_prod">Log In</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>


  <script>
    function showForm(formId) {
      const productionForm = document.getElementById("productionForm");
      const ppicForm = document.getElementById("ppicForm");

      if (formId === "productionForm") {
        productionForm.style.display = "block";
        ppicForm.style.display = "none";
      } else if (formId === "ppicForm") {
        productionForm.style.display = "none";
        ppicForm.style.display = "block";
      }
    }
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#password');

    togglePassword.addEventListener('click', function(e) {
      // toggle the type attribute
      const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
      password.setAttribute('type', type);
      // toggle the icon
      this.querySelector('i').classList.toggle('fa-eye');
      this.querySelector('i').classList.toggle('fa-eye-slash');
    });
    $(document).ready(function() {
      // Set the default active button to "btnFeatures"
      $("#btnFeatures").addClass("active");
      $("#collapseFeatures").addClass("show");

      $(".btn").click(function() {
        // If the clicked button is already active, do nothing
        if ($(this).hasClass("active")) return;

        $(".btn").removeClass("active");
        $(this).addClass("active");

        // Hide all content
        $(".collapse").removeClass("show");

        // Show the clicked content
        const target = $(this).attr("data-bs-target");
        $(target).addClass("show");
      });
    });
  </script>



  <!-- <div class="containers">
    <input id="signin" type="radio" name="tab" checked="checked" />
    <input id="register" type="radio" name="tab" />

    <div class="pages">
      <div class="page" id="loginForm">
        <form action="" method="POST">
          <h4 class="p-2 my-2 fw-bold"><i class="fa fa-gear fs-2"></i> PRODUCTION PORTAL</h4>
          <div class="input">
            <div class="title"><i class="fa-solid fa-user"></i> USERNAME</div>
            <input class="text" type="text" name="prod_uname" placeholder="Enter Username"" />
          </div>
          <div class=" input">
            <div class="title"><i class="fa-solid fa-lock"></i> PASSWORD</div>
            <input class="text" type="password" name="prod_pw" placeholder="Enter Password" />
          </div>
          <div class="input">
            <button class="btn btn-dark w-100 fw-bold" type="submit" name="login_prod">LOGIN</button>
          </div>
        </form>
      </div>

      <div class="page signup">
        <h4 class="p-2 my-2 fw-bold"><i class="fa-solid fa-calendar"></i> PPIC PORTAL</h4>
        <div class="input">
          <div class="title"><i class="fa-solid fa-user"></i> USERNAME</div>
          <input class="text" type="text" name="ppic_uname" placeholder="Enter Username" />
        </div>
        <div class="input">
          <div class="title"><i class="fa fa-lock"></i> PASSWORD</div>
          <input class="text" type="password" name="ppic_pw" placeholder="Enter Password" />
        </div>
        <div class="input">
          <button class="btn btn-dark w-100 fw-bold" type="submit" name="login_ppic">LOGIN</button>
        </div>
      </div>
    </div>
    <div class="tabs">
      <label class="tab text fw-bold" for="signin">PRODUCTION</label>
      <label class="tab text fw-bold" for="register">PPIC</label>
    </div>
  </div>
  <script>
    // Demo or didn't happen
    var signin = document.querySelector("#signin");
    var register = document.querySelector("#register");
    setTimeout(function() {
      register.checked = true;
    }, 1000);
    setTimeout(function() {
      signin.checked = true;
    }, 2000);
  </script> -->
</body>
<!--
<footer>
  <div class="fixed-bottom text-center bg-myblue shadow-lg border-top">
    <p>ATS PRODUCTION. © 2023</p>
  </div>

</footer>-->

</html>