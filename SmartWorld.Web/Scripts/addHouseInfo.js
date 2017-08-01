
function AddUser() {
    var str = $("#userDiv").html();
    $("#userDiv2").empty().html(str);
    //				$("#userDiv").append(str);
    //				$("#userDiv2").empty();
    $("#delUser").show();
}

function DelUser() {
    var str = "<a href='javascript:void(0)' onclick='AddUser()'>+添加合租人</a>";
    $("#userDiv2").empty().html(str);
    $("#delUser").hide();
}