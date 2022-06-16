function profileGeneral() {
    var generalCard = document.getElementById("profileGeneralEdit");
    var securityCard = document.getElementById("profileSecuritylEdit");

    securityCard.style.display = "none";
    generalCard.style.display = "flex";
}

function profileSecurity() {
    var generalCard = document.getElementById("profileGeneralEdit");
    var securityCard = document.getElementById("profileSecuritylEdit");

    generalCard.style.display = "none";
    securityCard.style.display = "flex";
}

function ProfileEditsSubmit() {
    var data = $("#profileEditsSubmit").serialize();
    $.post("Edit?" + data, function (r) {
        $("#profileGeneralEdit").html(r);
    })
}

function passwordCheckPopup(trigger) {
    $.get("CheckPasswordPopup?trigger=" + trigger, function (r) {
        $("#passwordCheckOverlay").html(r);
    });
    toggleShow("passwordCheckOverlay");
}

function passwordCheckSubmit(trigger) {
    var password = $("#profileEditPassword").val();
    $.get("CheckPassword?password=" + password, function (r) {
        if (r == false) {
            $.get("PasswordIncorrect?trigger=" + trigger, function (r) {
                $("#passwordCheckOverlay").html(r);
            });
        } else {
            $.get("PasswordCorrect?trigger=" + trigger, function (r) {
                window.location.href = r;
            });
        }
    });
}

function resendMessage(trigger) {
    $.get(URLBase + "/Profile/ResendMessage?trigger=" + trigger, function (r) { });
    var a = 1;
}

function emailChangeSubmit(Id) {
    var newEmail = $("#newEmail").val();
    $.get(URLBase + "/Profile/CheckEmailValidity?newEmail=" + newEmail, function (r) {
        if (r == false) {
            $.get(URLBase + "/Profile/EmailInvalid?Id=" + Id, function (r) {
                $("#emailChangeWrap").html(r);
            });
        } else {
            $.get(URLBase + "/Profile/EmailValid?Id=" + Id + "&newEmail=" + newEmail, function (r) {
                window.location.href = URLBase + r;
            });
        }
    });
}

function passwordChangeSubmit(Id, token) {
    var newPassword = $("#newPassword").val();
    var confirmPassword = $("#confirmPassword").val();
    $.get(URLBase + "/Profile/CheckPasswordValidity?Id=" + Id + "&token=" + token +
        "&newPassword=" + newPassword + "&confirmPassword=" + confirmPassword, function (r) {
        if (r == false) {
            $.get(URLBase + "/Profile/PasswordInValid?Id=" + Id + "&token=" + token +
                "&newPassword=" + newPassword + "&confirmPassword=" + confirmPassword, function (r) {
                $("#passwordChangeWrap").html(r);
            });
        } else {
            $.post(URLBase + "/Profile/PasswordValid?Id=" + Id + "&token=" + token +
                "&newPassword=" + newPassword + "&confirmPassword=" + confirmPassword, function (r) {
                if (r == false) {
                    $.get(URLBase + "/Profile/PasswordChangeUnsuccessful", function (r) {
                        $("#passwordChangeWrap").html(r);
                    });
                } else {
                    $.get(URLBase + "/Profile/PasswordChangeSuccessful", function (r) {
                        window.location.href = URLBase + r;
                    });
                }
            });
        }
    });
}
