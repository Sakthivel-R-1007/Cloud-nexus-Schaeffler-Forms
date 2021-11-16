$(document).ready(function () {

	initializeValidation();

	$("#btnreset").click(function () {
		$("#ResetPassForm").submit();
	});
});

function initializeValidation() {
	$("#ResetPassForm").validate({
		errorPlacement: function (error, element) {
			$("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
		},
		rules: {
			"user.NewPassword": {
				required: true,
				minlength: 8,
				pwcheck: true
			},
			"user.ConfirmPassword": {
				required: true,
				minlength: 8,
				equalTo: "#Password"
			}
		},
		messages: {
			"user.NewPassword": {
				minlength: "NewPassword must be at least 8 characters"
			},
			"user.ConfirmPassword": {
				minlength: "Confirm Password must be at least 8 characters",
				equalTo: "Confirm password and new password should be same"
			}
		}
	});

	$.validator.addMethod("pwcheck", function (value) {
		return /[!@#$%^&*()_]+/.test(value) && /[a-z]/.test(value) && /[0-9]/.test(value) && /[A-Z]/.test(value);
	}, "Password should contain atleast one number and one special character and one upper case and one lower case character");

	$("#password").on("change", function () {
		$("span[id='" + $(this).attr("name") + "-error']").html('');
	});
}