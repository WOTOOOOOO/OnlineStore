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

// desc : image upload preview | use : (Home : Create, Edit)
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