$(document).ready(function () {
    initializeValidation();
    intializepagination();
    $(document).on("click", "#DeleteUser", DeletePopup);
    $(document).on("click", "#btnconfirm_delete", submitDelete);
    $(document).on("click", "#userEdit", EditPopup);
    $(document).on("click", "#editUser", update);
    $(document).on("click", "#addUser", add);
    $(document).on("click", "#addNewBtn", addPopup);

    $(".searchBtn").click(function () {
        $("#UserView_Form").submit();
    })



});


function addPopup() {
    var modal = document.getElementById("add");
    modal.style.display = "block";
    var path = apppath + "/Admin/User/AddPartialView";
    $.ajax({
        url: path,
        dataType: 'html',
        success: function (response) {
            if (response === undefined && response === '') {
                response = 'No Results Found';
            }
            //console.log(response);
            $("#UserAddForm").html('');
            $("#UserEditForm").html('');
            $("#UserAddForm").append(response);
        },
        failure: function (response) {
            console.log(response);
            $("#UserAddForm").append(('Error occured'));
        }
    });
}
function EditPopup() {
    $("#dynamic_container").html('')
    var uniquecode = $(this).attr("data-uniquecode");
    var path = apppath + "/Admin/User/EditPartialView/" + uniquecode;
    $.ajax({
        url: path,
        dataType: 'html',
        success: function (response) {
            if (response === undefined && response === '') {
                response = 'No Results Found';
            }
            //console.log(response);
            $("#UserEditForm").html('');
            $("#UserAddForm").html('');
            $("#UserEditForm").append(response);
        },
        failure: function (response) {
            //console.log(response);
            $("#UserEditForm").append(('Error occured'));
        }
    });
}
function add() {
    $("#UserAddForm").attr({
        action: apppath+ "/Admin/User/Save"
    }).submit();
}
function update() {
    $("#UserEditForm").attr({
        action: apppath + "/Admin/User/Save"
    }).submit();
}

function initializeValidation() {

    $("#UserEditForm").validate({
        errorPlacement: function (error, element) {
            $("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
        },
        rules: {

            Name: {
                required: true
                //remote: {
                //    url: apppath + "/Admin/User/CheckUser",
                //    type: "GET",
                //    async: true,
                //    cache: false,
                //    data: {
                //        Name: function () {
                //            return $("#Name").val();
                //        },
                //        EncDetail: function () {
                //            return $("#GUID").val() || null;
                //        }
                //    },
                //    dataType: 'json'
                //}
            },
            Email: {
                required: true,
                remote: {
                    url: apppath + "/Admin/User/CheckUser",
                    type: "GET",
                    async: true,
                    cache: false,
                    data: {
                        Email: function () {
                            return $("#Email").val();
                        },
                        EncDetail: function () {
                            return $("#GUID").val() || null;
                        }
                    },
                    dataType: 'json'
                }
            },
            ContactNo: {
                required: true
            },
            "role.Id": {
                required: true
            }

        },
        messages: {
            Email: {
                remote: "Email Already Exist"

            }
        }


    });

    $("#UserAddForm").validate({
        errorPlacement: function (error, element) {
            $("span[data-valmsg-for='" + $(element).attr("name") + "']").html($(error));
        },
        rules: {

            Name: {
                required: true
                //remote: {
                //    url: apppath + "/Admin/User/CheckUser",
                //    type: "GET",
                //    async: true,
                //    cache: false,
                //    data: {
                //        Name: function () {
                //            return $("#Name").val();
                //        },
                //        EncDetail: function () {
                //            return $("#GUID").val() || null;
                //        }
                //    },
                //    dataType: 'json'
                //}
            },
            Email: {
                required: true,
                remote: {
                    url: apppath + "/Admin/User/CheckUser",
                    type: "GET",
                    async: true,
                    cache: false,
                    data: {
                        Email: function () {
                            return $("#Email").val();
                        },
                        EncDetail: function () {
                            return $("#GUID").val() || null;
                        }
                    },
                    dataType: 'json'
                }
            },
            ContactNo: {
                required: true
            },
            "role.Id": {
                required: true
            }

        },
        messages: {
            Email: {
                remote: "Email Already Exist"

            }
        }


    });
}


function DeletePopup() {
    $('#btnconfirm_delete').attr('data-value', $(this).attr("data-uniquecode"));
}
function submitDelete() {
    //window.location = apppath + "/Admin/UserAccounts/Delete/" + $("#EncDetail").val();
    // alert($('#btnconfirm_delete').data('value'));
    $("#dynamic_container").html('').append($("<input>", {
        id: "EncDetail",
        name: "EncDetail",
        type: "hidden",
        value: $('#btnconfirm_delete').data('value')
    }));


    $("#Form").attr({
        action: apppath + "/Admin/User/Delete"
    }).submit();
}


function intializepagination() {
    $(".pagination").pagination({
        items: $('.pagination').data('totalitems'),
        itemsOnPage: $('.pagination').data('pagesize'),
        currentPage: $('.pagination').data('pageindex'),
        hrefTextPrefix: "",
        prevText: "&laquo;",
        nextText: "&raquo;",
        cssStyle: "",
        onPageClick: function (pageNumber, event) {
            event.preventDefault();
            $("#PageNum").val(pageNumber);
            $("#UserView_Form").submit();
        }
    });
}
//var pageSize = 15;
//function bindPageItems(e) {
//    //var path = "/Admin/Category/PartialIndex/" + e.PageNumber + "/" + e.Category + "/" + e.SubCategory + "/" + e.SortBy + "/" + e.SortDirection;
//    var path = apppath + "/Admin/NewsRoom/AnnualReport/PartialView/" + e.PageNumber;
//    $.ajax({
//        url: path,
//        dataType: 'html',
//        success: function (response) {
//            if (response === undefined && response == '') {
//                response = '<tr><td colspan="5" class="center">No Results Found</td></tr>';
//            }

//            $(".tblgrey tbody").html(response);
//            var pageIndex = parseInt($('.pagination').attr('data-pageindex'));
//            //console.log(pageIndex);
//            var startItem = (pageIndex == 1 ? 0 : (pageIndex - 1) * pageSize) + 1;
//            var lastItem = startItem + ($(".tblgrey tbody tr").length - 1);
//            $(".page").html("Showing " + startItem + " to " + lastItem + " of " + $('.pagination').data('totalitems') + "entries");

//        },
//        failure: function (response) {
//            //console.log(response);
//            $(".tblgrey tbody").html('<tr><td colspan="5" class="center error">Error occured</td></tr>');
//        }
//    });

//}