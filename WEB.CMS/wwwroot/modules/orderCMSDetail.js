let input = $('#order_Id').val();
let type = 7;
$(document).ready(function () {
   
    _orderDetail.LoadPackages(input);
    _orderDetail.LoadContractPay(input);
    _orderDetail.LoadBillVAT(input);
    _orderDetail.LoadFile(input, type);
    _orderDetail.LoadPersonInCharge(input);
});
var _orderDetail = {
    LoadOeederDetail: function () {
        _orderDetail.LoadPackages(input);
        _orderDetail.LoadContractPay(input);
        _orderDetail.LoadBillVAT(input);
        _orderDetail.LoadFile(input, type);
        _orderDetail.LoadPersonInCharge(input);
    },

    LoadPackages: function (input) {
        $.ajax({
            url: "/Order/Packages",
            type: "Post",
            data: { orderId: input},
            success: function (result) {
                $('#imgLoading_Packages').hide();
                $('#grid_data_Packages').html(result);
            }
        });
    },
    LoadContractPay: function (input) {
        $.ajax({
            url: "/Order/ContractPay",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_ContractPay').hide();
                $('#grid_data_ContractPay').html(result);
            }
        });
    },
    LoadBillVAT: function (input) {
        $.ajax({
            url: "/Order/BillVAT",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_BillVAT').hide();
                $('#grid_data_BillVAT').html(result);
            }
        });
    },
    LoadListPassenger: function (input) {
        $.ajax({
            url: "/Order/ListPassenger",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_ListPassenger').hide();
                $('#grid_data_ListPassenger').html(result);
            }
        });
    },
    LoadFile: function (input, type) {
        _global_function.RenderFileAttachment($('#grid_data_File'), input,type)
        $('#imgLoading_File').hide();
        /*
        $.ajax({
            url: "/Order/File",
            type: "Post",
            data: { orderId: input, type: type },
            success: function (result) {
                $('#imgLoading_File').hide();
                $('#grid_data_File').html(result);
            }
        });
        */
    },
    LoadPersonInCharge: function (input) {
        $.ajax({
            url: "/Order/PersonInCharge",
            type: "Post",
            data: { orderId: input },
            success: function (result) {
                $('#imgLoading_PersonInCharge').hide();
                $('#grid_data_PersonInCharge').html(result);
            }
        });
    },

}
