﻿$(document).ready(function () {

	IntializeValidation();
	$(".numeric").numeric({ allowMinus: false, allowDecSep: false, maxDecimalPlaces: 0 });

	$(document).on("click", ".submitBtn", function () {

		
		if ($("#contactForm").valid()) {
			$("#contactForm").submit();
		}
	});
});

function IntializeValidation() {

	$.validator.addMethod("noSpace", function (value, element) {
		return value.indexOf(" ") < 0 && value !== "";
	}, "Space not allowed");

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
				required: true
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
			"BrandService[0].Name": {
				required: function () {
					return $('[name="BrandService[0].Id"]:checked').val() == 1;
				}
			},
			"BrandService[1].Name": {
				required: function () {
					return $('[name="BrandService[1].Id"]:checked').val() == "2";
				}
			},
			"BrandService[2].Name": {
				required: function () {
					return $('[name="BrandService[2].Id"]:checked').val() == "3";
				}
			},
			"BrandService[3].Name": {
				required: function () {
					return $('[name="BrandService[3].Id"]:checked').val() == "4";
				}
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
			"InformationType[0].Name": {
				required: function () {
					return $('[name="InformationType[0].Id"]:checked').val() == 1;
				}
			},
			"InformationType[1].Name": {
				required: function () {
					return $('[name="InformationType[1].Id"]:checked').val() == 2;
				}
			},
			"InformationType[2].Name": {
				required: function () {
					return $('[name="InformationType[2].Id"]:checked').val() == 3;
				}
			},
			"InformationType[3].Name": {
				required: function () {
					return $('[name="InformationType[3].Id"]:checked').val() == 4;
				}
			}
		},
		messages: {
			FullName: {
				required: 'Bagian ini diperlukan'
			},
			Email: {
				required: 'Bagian ini diperlukan',
				email: 'Silahkan isi alamat email ',
				noSpace: 'Silahkan isi alamat email '
			},
			CompanyName: {
				required: 'Bagian ini diperlukan'
			},
			PhoneNumber: {
				required: 'Silahkan masukkan nomor telepon yang benar'
			},
			BrandServices: {
				required: 'Bagian ini diperlukan'
			},
			"BrandService[0].Name": {
				required: 'Bagian ini diperlukan'
			},
			"BrandService[1].Name": {
				required: 'Bagian ini diperlukan'
			},
			"BrandService[2].Name": {
				required: 'Bagian ini diperlukan'
			},
			"BrandService[3].Name": {
				required: 'Bagian ini diperlukan'
			},
			VehicleTypes: {
				required: 'Bagian ini diperlukan'
			},
			InformationTypes: {
				required: 'Bagian ini diperlukan'
			},
			"InformationType[0].Name": {
				required: 'Bagian ini diperlukan'
			},
			"InformationType[1].Name": {
				required: 'Bagian ini diperlukan'
			},
			"InformationType[2].Name": {
				required: 'Bagian ini diperlukan'
			},
			"InformationType[3].Name": {
				required: 'Bagian ini diperlukan'
			}

		}
	});
}







