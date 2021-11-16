$(document).ready(function () {

	IntializeValidation();
	$(".numeric").numeric({ allowMinus: false, allowDecSep: false, maxDecimalPlaces: 0 });

	$(document).on("click", ".submitBtn", function () {

		
		if ($("#contactForm").valid()) {
			$(".submitBtn").hide();			
			$("#contactForm").submit();
		}
	});

	$(function () {
		$("#PhoneNumber").keypress(function (e) {
			var keyCode = e.keyCode || e.which;
			var regex = /^[0-9-+]*$/;

			//Validate TextBox value against the Regex.
			var isValid = regex.test(String.fromCharCode(keyCode));
			if (!isValid) {
			}
			return isValid;
		});
	});

});

function IntializeValidation() {

	$.validator.addMethod("noSpace", function (value, element) {
		return value.indexOf(" ") < 0 && value !== "";
	}, "Space not allowed");

	$.validator.addMethod("pnumber", function (value, element) {	
		var isValid = false;
		var regex = /^[0-9-+]*$/;
		isValid = regex.test(value);
		return isValid;
	}, "Silahkan masukkan nomor telepon yang benar");

	$("#contactForm").validate({
		errorPlacement: function (error, element) {
			$("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
		},
		ignore: [],
		rules: {
			FullName: {
				required: true
			},
			Email: {
				required: true,
				email: true,
				noSpace: true,
			},
			CompanyName: {
				required: true
			},
			Country: {
				required: true
			},
			PhoneNumber: {
				required: true,
				pnumber: true
			},
			BrandServices: {//Produncts
				required: function (element) {
					if ($('input.BrandServiceClass:checkbox:checked').length == 0) {
						return true;
					}
					return false;
				},
				minlength: 1
			},
		
			VehicleTypes: {//Produncts
				required: function (element) {
					if ($('input.vehicleTypesclass:checkbox:checked').length == 0) {
						return true;
					}
					return false;
				},
				minlength: 1
			},
			InformationTypes: {//Produncts
				required: function (element) {
					if ($('input.InformationTypesclass:checkbox:checked').length == 0) {
						return true;
					}
					return false;
				},
				minlength: 1
			},
		
			"InformationType[3].Name": {
				required: function () {
					return $('[name="InformationType[3].Id"]:checked').val() == 4;
				}
			}
		},
		messages: {
			FullName: {
				required: 'This field is required.'
			},
			Email: {
				required: 'Please enter a valid email address.',
				email: 'Please enter a valid email address.',
				noSpace: 'Please enter a valid email address.'
			},
			CompanyName: {
				required: 'This field is required.'
			},
			"Country": {
				required: 'This field is required.'
			},
			PhoneNumber: {
				required: 'Please enter a valid phone number. ',
				pnumber: 'Please enter a valid phone number. '
			},
			BrandServices: {
				required: 'This field is required.'
			},
			
			VehicleTypes: {
				required: 'This field is required.'
			},
			InformationTypes: {
				required: 'This field is required.'
			},
			
			"InformationType[3].Name": {
				required: 'This field is required.'
			}

		}
	});
}







