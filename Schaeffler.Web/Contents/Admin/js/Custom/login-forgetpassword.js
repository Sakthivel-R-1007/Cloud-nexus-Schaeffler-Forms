$(document).ready(function () {

	IntializeValidation();
	$(document).on("click", "#btnForgetPass", submitForgetPass);
});

function IntializeValidation() {

	$("#ForgotPasswordForm").validate({
		errorPlacement: function (error, element) {
			$("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
		},
		rules: {
			"user.Email": {
				required: true,
				email: true
			}
		}
	});
}



function submitForgetPass() {
	var form = "#ForgotPasswordForm";

	if ($(form).valid()) {
		$.ajax({
			dataType: "json",
			url: $(form).attr("action"),
			data: $(form).serialize(),
			type: $(form).attr("method"),
			beforeSend: function () {
				$("#btnForgetPass").hide();
				$("#messageContainer").html("Please Wait...")
			},
			complete: function () {
				$("#btnForgetPass").show();
			},
			success: function (d) {
				if (d !== null && d.Success) {
					$('#user_Email').val("");
					$("#messageContainer").html("An email has been sent to you.")
				}
				else {
					$("#messageContainer").html(d.Error)
				}
			},
			failure: function (e) {
				console.log(e);
			}
		});
	}
}

