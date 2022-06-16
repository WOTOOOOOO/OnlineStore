function removeCartItem(Id) {
    $.post("RemoveFromCart?Id=" + Id, function (r) {
        $("#cartItemList").html(r);
    });
}