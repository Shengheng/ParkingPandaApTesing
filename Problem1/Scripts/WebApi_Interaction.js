$(function () {
    $(document).ready(function () {
        //$('#btn_logout').click(function () {           
        //    $(".form-control").val('');
        //});
        //$("#btn_save").click(function () {
        //    if ($("#mainBody_txt_currentPwd").val().length != 0) {
        //        if($("#mainBody_txt_changePwd").val().length === 0 ||
        //            $("#mainBody_txt_reenterPwd").val().length === 0) {
        //            $("#mainBody_lbl_notify").val() = "Please provide new password";                   
        //        }
        //    }
        //});
        
        //if ($("#mainBody_lbl_loggedUser").text().length === 0) {
        //    $("#login").show();
        //    $("#logged").hide();
        //    $("#user").hide();
        //    $("#save").hide();

        //}
        //else {
        //    $("#login").hide();
        //    $("#logged").show();
        //    $("##user").show();
        //    $("#save").show();
        //}
        if ($("#mainBody_lbl_notify").text() === "Account successfully updated") {
            $("#mainBody_lbl_notify").css('color', 'blue');
        }
    });
});