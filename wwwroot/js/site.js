// col Utils
function disableButton(id) {
    document.getElementById(id).disableButton;
}

// use (Order : OrderCart)
function toggleOrderDropDown() {
    document.getElementById("orderDropDownCont").classList.toggle("show");
}

// use (Home : Create, Edit, Index)
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
// col (Home : Index)
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

// col (Profile : Profile)
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

function passwordCheckPopup(trigger) {
    var overlay = document.getElementById("overlay");
    var nextWindow = document.getElementById("profileEditTrigger");
    if (trigger == 1) {
        nextWindow.value = 1;
    }
    else if (trigger == 2) {
        nextWindow.value = 2;
    }
    else {
        nextWindow.value = 0;
    }
    if (overlay.style.visibility == "hidden") {
        overlay.style.visibility = "visible";
        overlay.style.opacity = "1";
    }
    else {
        overlay.style.opacity = "0";
        overlay.style.visibility = "hidden";
    }
}

// file uploads (Home : Create, Edit)
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

