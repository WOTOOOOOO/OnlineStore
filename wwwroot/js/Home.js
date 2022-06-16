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