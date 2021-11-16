$(document).ready(function () {

    var validator = $("form").validate({
        errorPlacement: function (error, element) {
            $("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
        },
        rules: {
            Password: {
                required: true,
                minlength: 8
            },
            NewPassword: {
                required: true,
                minlength: 8,
                pwcheck: true,
                currentpassword_not_sameas_newpassword: true
            },
            ConfirmPassword: {
                required: true,
                minlength: 8,
                equalTo: "#NewPassword"
            }
        },
        messages: {
            Password: {
                required: "This field is requied"
            },
            NewPassword: {
                required: "This field is requied"
            },
            ConfirmPassword:
            {
                required: "This field is requied",
                equalTo: "ConfirmPassword  and NewPassword should be same"
            }
        }
    });

    $.validator.addMethod("currentpassword_not_sameas_newpassword", function (value, element) {
        return $('#Password').val() != $('#NewPassword').val()
    }, "NewPassword value must not be same as CurrentPassword");



    $.validator.addMethod("pwcheck", function (value) {
        return /[!@#$%^&*()_]+/.test(value) && /[a-z]/.test(value) && /[0-9]/.test(value) && /[A-Z]/.test(value);
    }, "Password should contain atleast one number and one special character and one upper case and one lower case character");


    $("#submit_btn").click(function () {
        $("form").submit();
    });

});