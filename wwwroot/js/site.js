const URLBase = window.location.origin;

// - collection Utils
function disableButton(id) {
    document.getElementById(id).disableButton;
}

// use : (Order : OrderCart)
function toggleShow(Id) {
    document.getElementById(Id).classList.toggle("show");
}

// use : (Home : Create, Edit, Index)
function updateCategory() {
    var categories = document.getElementsByClassName("form-check-input");
    var categoryInput = document.getElementById("category");
    var finalCategory = 0;
    for (let i = 0; i < categories.length; i++) {
        if (categories[i].checked) {
            finalCategory = finalCategory | categories[i].value;
        }
    }
    categoryInput.value = finalCategory;
    categoryInput.dispatchEvent(new Event('change'));
}
// - collection Home
function filterPopup() {
    var filterCard = document.getElementById("filter");
    if (filterCard.style.maxHeight == "0px") {
        filterCard.style.maxHeight = "800px";
        filterCard.style.opacity = "1";
    }
    else {
        filterCard.style.maxHeight = "0px";
        filterCard.style.opacity = "0";
    }
}

// desc : filter events 
$("#category").change(function () {
    let cat = $("#category").val();
    let str = $("#searchString").val();
    $.get("Home/Filter?categories=" + cat + "&searchString=" + str, function (r) {
        $("#items").html(r);
    });
});

$("#searchString").keyup(function () {
    let cat = $("#category").val();
    let str = $("#searchString").val();
    $.get("Home/Filter?categories=" + cat + "&searchString=" + str, function (r) {
        $("#items").html(r);
    });
});

// desc : file uploads | use : Create, Edit
$("#customFile").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    const fileReader = new FileReader();
    fileReader.readAsDataURL($(this).prop('files')[0]);
    fileReader.addEventListener("load", function () {
        $("#preview").css({ "display": "block" });
        $("#preview").html('<img style="height : 100%; width : 100%;" src="' + this.result + '" />');
    });
});

// desc : delete product
function deleteProduct(Id) {
    let cat = $("#category").val();
    let str = $("#searchString").val();
    $.post("Home/Delete?Id=" + Id + "&categories=" + cat + "&searchString=" + str, function (r) {
        $("#items").html(r);
    });
}

// desc : add to cart
function addProduct(Id) {
    let cat = $("#category").val();
    let str = $("#searchString").val();
    $.post("Home/Add?Id=" + Id + "&categories=" + cat + "&searchString=" + str, function (r) {
        $("#items").html(r);
    });
}

// - collection Profile
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


// - collection Order
function removeCartItem(Id) {
    $.post("RemoveFromCart?Id=" + Id, function (r) {
        $("#cartItemList").html(r);
    });
}


// - collection Admin

function makeAdmin(Id) {
    $.get(URLBase + "/Admin/MakeAdmin?Id=" + Id, function (r) {
        $("#allUsersList").html(r);
    });
}

function removeAdmin(Id) {
    $.get(URLBase + "/Admin/DeleteAdmin?Id=" + Id, function (r) {
        $("#allUsersList").html(r);
    });
}

// - collection Account

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