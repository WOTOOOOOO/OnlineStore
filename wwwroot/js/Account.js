function loginSubmit() {
    var data = $("#loginForm").serialize();
    $.post(URLBase + "/Account/LoginResult?" + data, function (r) {
        if (r == false) {
            $.get(URLBase + "/Account/LoginUnsuccessful?" + data, function (r) {
                $("#loginFormComponent").html(r);
            });
        } else {
            $.get(URLBase + "/Account/LoginSuccessful", function (r) {
                window.location.href = URLBase + r;
            });
        }
    });
}

function registerSubmit() {
    var data = $("#registerForm").serialize();
    var Email = $("#userEmail").val();
    $.post(URLBase + "/Account/RegisterResult?" + data, function (r) {
        if (r == false) {
            $.get(URLBase + "/Account/RegisterUnsuccessful?" + data, function (r) {
                $("#registerFormComponent").html(r);
            });
        } else {
            $.get(URLBase + "/Account/RegisterSuccessful?Email=" + Email, function (r) {
                window.location.href = URLBase + r;
            });
        }
    });
}

function resendEmailVerification(Email) {
    $.get(URLBase + "/Account/ResendMessage/" + Email, function (r) {});
}