function deleteComm(id, userid) {
    $.post("/" + id + "/Removecomm?commeeid=" + userid, function (data) {
        console.log($.parseJSON(data).Status);
        $.parseJSON(data).Status;
    })
    $("#Comm" + id).remove();    
}