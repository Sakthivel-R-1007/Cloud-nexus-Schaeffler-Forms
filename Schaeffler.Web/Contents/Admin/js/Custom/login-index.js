$(document).ready(function () {
	
	IntializeValidation();
	IntializeModelClose();
	loadCaptcha();
	
	$(".reload").click(loadCaptcha);

	$('#ForceLoginPopup').modal('show');
	$('.close-modal').hide();
	$("#yes").on("click", function () {
		$("#ForceLoginForm").submit();
	});

	$("#no").on("click", function () {
		$("#ForceLoginPopup").css("display", "none");
	});

	$(document).on("click", ".submitBtn", function () {
		if ($("#LoginForm").valid()) {
			$("#LoginForm").submit();
		}
	});

	$(".refreshBtn").on("click", function () {
		$.ajax({
			type: "POST",
			url: 'bulky-item-removal-services.aspx/RefreshCaptcha',
			contentType: 'application/json; charset=utf-8',
			dataType: 'json',
			success: function (response) {
				$("#captcha").val('');
				$.ajax({
					url: 'CaptchaHandler.ashx?method=ajax&' + response.d,
					type: 'POST',
					contentType: "image/jpeg",
					success: function (data) {
						document.getElementById('imgCaptcha').src = data;
					},
					error: function (errorText) {
						console.log("Wwoops something went wrong !");
					}
				});
			}
		});
	});
});

function IntializeValidation() {

	$("#LoginForm").validate({
		errorPlacement: function (error, element) {
			$("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
		},
		rules: {
			Email: {
				required: true,
				email: true
			},
			Password: {
				required: true
			},
			Captcha: {
				required: true
			}
		},
		messages: {
			Email: {
				required: "Email is required!"
			},
			Password: {
				required: "Password is required!"
			},
			Captcha: {
				required: "Security Code is required!"
			}

        }
	});
}

function IntializeModelClose() {
	$('a.openModal').click(function (event) {
		$(this).modal({
			fadeDuration: 250,
			showClose: false
		});
		return false;
	});
}

function loadCaptcha() {
	$("#securityCode").val('');
	$.ajax({
		type: "GET",
		url: apppath + '/Admin/Login/GetCaptcha',
		contentType: "image/png",
		success: function (data) {
			$('#imgCaptcha').attr('src', "data:image/png;base64," + data.captchaImage);
			// $("#FTP").html("For testing purpose " + data.code);
		},
		error: function (error, txtStatus) {
			console.log(txtStatus);
		}
	});
}







