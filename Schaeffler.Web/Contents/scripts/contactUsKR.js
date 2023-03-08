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
	}, "유효한 전화 번호를 입력하세요.");

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
			//"BrandService[0].Name": {
			//	required: function () {
			//		return $('[name="BrandService[0].Id"]:checked').val() == 1;
			//	}
			//},
			//"BrandService[1].Name": {
			//	required: function () {
			//		return $('[name="BrandService[1].Id"]:checked').val() == "2";
			//	}
			//},
			//"BrandService[2].Name": {
			//	required: function () {
			//		return $('[name="BrandService[2].Id"]:checked').val() == "3";
			//	}
			//},
			//"BrandService[3].Name": {
			//	required: function () {
			//		return $('[name="BrandService[3].Id"]:checked').val() == "4";
			//	}
			//},
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
			//"InformationType[0].Name": {
			//	required: function () {
			//		return $('[name="InformationType[0].Id"]:checked').val() == 1;
			//	}
			//},
			//"InformationType[1].Name": {
			//	required: function () {
			//		return $('[name="InformationType[1].Id"]:checked').val() == 2;
			//	}
			//},
			//"InformationType[2].Name": {
			//	required: function () {
			//		return $('[name="InformationType[2].Id"]:checked').val() == 3;
			//	}
			//},
			"InformationType[3].Name": {
				required: function () {
					return $('[name="InformationType[3].Id"]:checked').val() == 4;
				}
			}
		},
		messages: {
			FullName: {
				required: '이 필드는 필수 항목입니다.'
			},
			Email: {
				required: '유효한 이메일 주소를 입력하세요.',
				email: '유효한 이메일 주소를 입력하세요.',
				noSpace: '유효한 이메일 주소를 입력하세요.'
			},
			CompanyName: {
				required: '이 필드는 필수 항목입니다.'
			},
			PhoneNumber: {
				required: '유효한 전화 번호를 입력하세요.',
				pnumber: '유효한 전화 번호를 입력하세요.'
			},
			BrandServices: {
				required: '이 필드는 필수 항목입니다.'
			},
			//"BrandService[0].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			//"BrandService[1].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			//"BrandService[2].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			//"BrandService[3].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			VehicleTypes: {
				required: '이 필드는 필수 항목입니다.'
			},
			InformationTypes: {
				required: '이 필드는 필수 항목입니다.'
			},
			//"InformationType[0].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			//"InformationType[1].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			//"InformationType[2].Name": {
			//	required: 'Bagian ini diperlukan'
			//},
			"InformationType[3].Name": {
				required: '必須項目'
			}

		}
	});
}







