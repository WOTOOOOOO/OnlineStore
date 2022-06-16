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