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
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			},
			Email: {
				required: 'กรุณาใส่อีเมล์ที่ถูกต้อง',
				email: 'กรุณาใส่อีเมล์ที่ถูกต้อง',
				noSpace: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			},
			CompanyName: {
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			},
			PhoneNumber: {
				required: 'กรุณาใส่หมายเลขโทรศัพท์ที่ถูกต้อง',
				pnumber: 'กรุณาใส่หมายเลขโทรศัพท์ที่ถูกต้อง'
			},
			BrandServices: {
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			},
			//"BrandService[0].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			//"BrandService[1].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			//"BrandService[2].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			//"BrandService[3].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			VehicleTypes: {
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			},
			InformationTypes: {
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			},
			//"InformationType[0].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			//"InformationType[1].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			//"InformationType[2].Name": {
			//	required: 'กรุณาใส่อีเมล์ที่ถูกต้อง'
			//},
			"InformationType[3].Name": {
				required: 'ต้องระบุข้อมูลในส่วนนี้'
			}

		}
	});
}







