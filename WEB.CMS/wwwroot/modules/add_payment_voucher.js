﻿var listPaymentRequest = []
var listDetail = []
var amountEdit = 0
var isRender = false
var supplierIdSearch = 0
var clientIdSearch = 0
var isEdit = false
var isEditView = false
var isSetInput = false
var isSetClient = false
var isSetSupplier = false
var bankingAccountId = 0
var listBankAccount = []
var _add_payment_voucher = {
    Initialization: function () {
        this.validImageTypes = ['image/gif', 'image/jpeg', 'image/png', 'image/gif',
            'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/pdf'];
        this.validImageSize = 10 * 1024 * 1024;
        $('#attachFile').hide()
        $("#client-select").select2({
            theme: 'bootstrap4',
            placeholder: "Tên KH, Điện Thoại, Email",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            maximumSelectionLength: 1,
            ajax: {
                url: "/Contract/ClientSuggestion",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        txt_search: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.clientname + ' - ' + item.email + ' - ' + item.phone,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });
        $("#supplier-select").select2({
            theme: 'bootstrap4',
            placeholder: "Tên NCC, Điện Thoại, Email",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            maximumSelectionLength: 1,
            ajax: {
                url: "/PaymentRequest/GetSuppliersSuggest",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        name: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response, function (item) {
                            return {
                                text: item.name,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });
        $("#paymentRequestCode").select2({
            theme: 'bootstrap4',
            placeholder: "Tìm kiếm mã yêu cầu chi",
            hintText: "Nhập từ khóa tìm kiếm",
            searchingText: "Đang tìm kiếm...",
            closeOnSelect: false,
            ajax: {
                url: "/PaymentVoucher/GetListFilter",
                type: "post",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        jsonData: JSON.stringify(listPaymentRequest),
                        text: params.term,
                    }
                    return query;
                },
                processResults: function (response) {
                    return {
                        results: $.map(response.data, function (item) {
                            return {
                                text: item.paymentCode,
                                id: item.id,
                            }
                        })
                    };
                },
                cache: true
            }
        });
        $('#lblBankAccountRequired').show()
        $('#lblBankNameRequired').show()
        $('#lblBankAccount').hide()
        $('#lblBankName').hide()
        $('#lblSupplier').show()
        $('#divSupplier').show()
        $('#lblCustomer').hide()
        $('#divCustomer').hide()
        $('input').attr('autocomplete', 'off');
        $('#btnDeleteImage').hide()

    },
    FormatNumber: function () {
        var amount = $('#amount').val()
        $('#amount').val(amount.replaceAll(',', '.').replaceAll(',', ''))
        var n = parseFloat($('#amount').val().replace(/\D/g, ''), 10);
        $('#amount').val(isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ','));
    },
    FormatNumberStr: function (amount) {
        var n = parseFloat(amount, 10);
        return isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ',')
    },
    FormatNumberOrder: function (i) {
        var amount = $('#amount_order_' + i).val()
        $('#amount_order_' + i).val(amount.replaceAll(',', '.').replaceAll(',', ''))
        var n = parseFloat($('#amount_order_' + i).val().replace(/\D/g, ''), 10);
        $('#amount_order_' + i).val(isNaN(n) === true ? '' : n.toLocaleString().replaceAll('.', ','));
    },
    GetDataByClientOrSupplier: function (clientId, supplier_id, isEdit = false) {
        let payment_request_type = $('#payment-voucher-type').val()
        if (payment_request_type !== '3') {
            this.GetRequestBySupplierId(supplier_id, isEdit);
        }
        if (payment_request_type === '3') {
            this.GetRequestByClientId(clientId);
        }
    },
    GetRequestBySupplierId: function (supplierId, isEdit = false) {
        supplierIdSearch = supplierId
        listPaymentRequest = []
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentVoucher/GetRequestBySupplierId",
            type: "Post",
            data: { 'supplierId': supplierId, 'paymentVoucherId': $('#paymentVoucherId').val() },
            success: function (result) {
                _global_function.RemoveLoading()
                listPaymentRequest = result.data
                var totalAmount = 0
                $("#body_payment_requests").empty();
                $("#paymentRequestCode").empty()
                for (var i = 0; i < result.data.length; i++) {
                    $('#request-relate-table').find('tbody').append(
                        "<tr id='order_" + i + "'>" +
                        "<td>" +
                        "<label class='check-list number'>" +
                        " <input type='checkbox'  id='order_ckb_" + result.data[i].paymentCode + "' name='order_ckb' onclick='_add_payment_voucher.OnCheckBox(" + i + ");_add_payment_voucher.AddToListDetail(" + i + ")'>" +
                        " <span class='checkmark'></span>" + (i + 1) +
                        "  </label>"
                        + "</td>" +
                        "<td>" +
                        " <a class='blue' href='/PaymentRequest/Detail?paymentRequestId=" + result.data[i].id + "'> " + result.data[i].paymentCode + " </a>"
                        + "</td>" +
                        "<td>" + result.data[i].paymentDateViewStr
                        + "</td>" +
                        "<td>" + result.data[i].userName + "</td>" +
                        "<td class='text-right'>" + _add_payment_voucher.FormatNumberStr(result.data[i].amount) + "</td>" +
                        "</tr>"
                    );
                    totalAmount += result.data[i].amount
                    if (result.data[i].isChecked) {
                        let index = i
                        let code = result.data[i].paymentCode
                        setTimeout(function () {
                            $('#order_ckb_' + code).prop('checked', true)
                        }, 800)
                    }
                }

                $('#request-relate-table').find('tbody').append(
                    "<tr style='font-weight:bold !important;'>" +
                    "<td class='text-right' colspan='4'> Tổng </td>" +
                    "<td>" + _add_payment_voucher.FormatNumberStr(totalAmount) + "</td>" +
                    "</tr>"
                );
                setTimeout(function () {
                    _add_payment_voucher.OnCheckBox();
                }, 1000)
                if (isEditView) {
                    isRender = true
                    setTimeout(function () {
                        _add_payment_voucher.RenderItemChecked();
                    }, 700)
                }
            }
        });
        _add_payment_voucher.GetListBankAccountBySupplierID(supplierId);
    },
    GetRequestByClientId: function (clientId, isEdit = false) {
        clientIdSearch = clientId
        listPaymentRequest = []
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentVoucher/GetRequestByClientId",
            type: "Post",
            data: { 'clientId': clientId, 'paymentVoucherId': $('#paymentVoucherId').val() },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#body_payment_requests").empty();
                listPaymentRequest = result.data
                var totalAmount = 0
                $("#paymentRequestCode").empty()
                for (var i = 0; i < result.data.length; i++) {
                    $('#request-relate-table').find('tbody').append(
                        "<tr id='order_" + i + "'>" +
                        "<td>" +
                        "<label class='check-list number'>" +
                        " <input type='checkbox' id='order_ckb_" + result.data[i].paymentCode + "' name='order_ckb' onclick='_add_payment_voucher.OnCheckBox(" + i + ");_add_payment_voucher.AddToListDetail(" + i + ")'>" +
                        " <span class='checkmark'></span>" + (i + 1) +
                        "  </label>"
                        + "</td>" +
                        "<td>" +
                        " <a class='blue' href='/PaymentRequest/Detail?paymentRequestId=" + result.data[i].id + "'> " + result.data[i].paymentCode + " </a>"
                        + "</td>" +
                        "<td>" + result.data[i].paymentDateViewStr + "</td>" +
                        "<td>" + result.data[i].userName + "</td>" +
                        "<td class='text-right'>" + _add_payment_voucher.FormatNumberStr(result.data[i].amount) + "</td>" +
                        "</tr>"
                    );
                    totalAmount += result.data[i].amount
                    if (result.data[i].isChecked) {
                        let index = i
                        let code = result.data[i].paymentCode
                        setTimeout(function () {
                            $('#order_ckb_' + code).prop('checked', true)
                        }, 800)
                    }
                }
                $('#request-relate-table').find('tbody').append(
                    "<tr style='font-weight:bold !important;'>" +
                    "<td class='text-right' colspan='4'> Tổng </td>" +
                    "<td>" + _add_payment_voucher.FormatNumberStr(totalAmount) + "</td>" +
                    "</tr>"
                );
                setTimeout(function () {
                    _add_payment_voucher.OnCheckBox();
                }, 1000)
                if (isEditView)
                    setTimeout(function () {
                        _add_payment_voucher.RenderItemChecked();
                    }, 700)
            }
        });
        _add_payment_voucher.GetListBankAccountByClientID(clientIdSearch);
    },
    GetListBankAccountBySupplierID: function (supplierId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/GetListBankAccountBySupplierId",
            type: "Post",
            data: {
                'supplierId': supplierId,
            },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#bankingAccount").empty();
                $('#bankingAccount').append(
                    "<option value='0'>Chọn</option>"
                );
                listBankAccount = result.data
                for (var i = 0; i < result.data.length; i++) {
                    $('#bankingAccount').append(
                        "<option value='" + result.data[i].id + "'> " + result.data[i].bankId + " - " + result.data[i].accountNumber + "</option>"
                    );
                }
                if (bankingAccountId !== undefined && bankingAccountId !== null && bankingAccountId !== 0) {
                    $("#bankingAccount").val(bankingAccountId);
                    for (var i = 0; i < listBankAccount.length; i++) {
                        if (listBankAccount[i].id === parseInt(bankingAccountId))
                            $('#bankName').val(listBankAccount[i].accountName)
                    }
                }
            }
        });
    },
    GetListBankAccountByClientID: function (clientId) {
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentRequest/GetListBankAccountByClientID",
            type: "Post",
            data: {
                'clientId': clientId,
            },
            success: function (result) {
                _global_function.RemoveLoading()
                $("#bankingAccount").empty();
                $('#bankingAccount').append(
                    "<option value='0'>Chọn</option>"
                );
                listBankAccount = result.data
                for (var i = 0; i < result.data.length; i++) {
                    $('#bankingAccount').append(
                        "<option value='" + result.data[i].id + "'> " + result.data[i].bankId + " - " + result.data[i].accountNumber + "</option>"
                    );
                }
                if (bankingAccountId !== undefined && bankingAccountId !== null && bankingAccountId !== 0) {
                    $("#bankingAccount").val(bankingAccountId);
                    for (var i = 0; i < listBankAccount.length; i++) {
                        if (listBankAccount[i].id === parseInt(bankingAccountId))
                            $('#bankName').val(listBankAccount[i].accountName)
                    }
                }
            }
        });
    },
    GetAccountName: function () {
        var bankingAccountId = $("#bankingAccount").val();
        if (bankingAccountId !== null && bankingAccountId !== undefined && bankingAccountId !== '') {
            for (var i = 0; i < listBankAccount.length; i++) {
                if (listBankAccount[i].id === parseInt(bankingAccountId))
                    $('#bankName').val(listBankAccount[i].accountName)
            }
        }
    },
    Validate: function () {
        let result = true
        _add_payment_voucher.ClearError()
        if ($('#payment-voucher-type').val() == undefined || $('#payment-voucher-type').val() == null || $('#payment-voucher-type').val() == '') {
            _add_payment_voucher.DisplayError('validate-payment-voucher-type', 'Vui lòng chọn loại nghiệp vụ')
            result = false;
        }
        if ($('#payment-voucher-type').val() === '3' && ($('#client-select').val() == undefined || $('#client-select').val() == null || $('#client-select').val() == '')) {
            _add_payment_voucher.DisplayError('validate-client-select', 'Vui lòng chọn khách hàng')
            result = false;
        }
        if (($('#payment-voucher-type').val() === '1' || $('#payment-voucher-type').val() === '2')
            && ($('#supplier-select').val() == undefined || $('#supplier-select').val() == null || $('#supplier-select').val() == '')) {
            _add_payment_voucher.DisplayError('validate-supplier-select', 'Vui lòng chọn nhà cung cấp')
            result = false;
        }
        if ($('#payment-voucher-pay-type').val() == undefined || $('#payment-voucher-pay-type').val() == null || $('#payment-voucher-pay-type').val() == '') {
            _add_payment_voucher.DisplayError('validate-payment-voucher-pay-type', 'Vui lòng chọn hình thức')
            result = false;
        }
        //if (parseInt($('#payment-voucher-pay-type').val()) == 2 && ($('#bankName').val() == undefined
        //    || $('#bankName').val() == null || $('#bankName').val() == '')) {
        //    _add_payment_voucher.DisplayError('validate-bankingName', 'Vui lòng nhập tên người thụ hưởng')
        //    result = false;
        //}
        if (parseInt($('#payment-voucher-pay-type').val()) == 2 && ($('#bankingAccount').val() == undefined
            || $('#bankingAccount').val() == null || $('#bankingAccount').val() == '' || $('#bankingAccount').val() == 0 || $('#bankingAccount').val() == '0')) {
            _add_payment_voucher.DisplayError('validate-bankingAccount', 'Vui lòng chọn số tài khoản nhận')
            result = false;
        }
        if ($('#content').val() == undefined || $('#content').val() == null || $('#content').val() == '') {
            _add_payment_voucher.DisplayError('validate-content', 'Vui lòng nhập nội dung')
            result = false;
        }
        if (($('#content').val()).length > 500) {
            _add_payment_voucher.DisplayError('validate-content', 'Vui lòng nhập nội dung không quá 500 kí tự')
            result = false;
        }
        if ($('#description').val() !== undefined && $('#description').val() !== null
            && $('#description').val() !== '' && ($('#description').val()).length > 3000) {
            _add_payment_voucher.DisplayError('validate-description', 'Vui lòng nhập ghi chú không quá 3000 kí tự')
            result = false;
        }

        if (!result) return false

        var flag = false
        for (var i = 0; i < listPaymentRequest.length; i++) {
            var checked = $('#order_ckb_' + listPaymentRequest[i].paymentCode).is(":checked")
            if (checked) {
                flag = true
            }
        }
        if (!flag) {
            _msgalert.error('Vui lòng tích chọn phiếu yêu cầu chi');
            return false;
        }

        return true
    },
    AddPaymentVoucher: function () {
        let validate = _add_payment_voucher.Validate()
        if (!validate)
            return;
        let PaymentRequestDetails = []
        for (var i = 0; i < listPaymentRequest.length; i++) {
            var checked = $('#order_ckb_' + listPaymentRequest[i].paymentCode).is(":checked")
            if (checked) {
                PaymentRequestDetails.push({ 'id': listPaymentRequest[i].id, 'paymentCode': listPaymentRequest[i].paymentCode })
            }
        }
        //var formData = new FormData();
        //const file = document.querySelector('input[name=imagefile]').files[0];
        let other_image = [];
        $('#suplier_room_list_image .img_other').each(function () {
            let image_item = $(this);
            other_image.push(image_item.attr('src'));
        });
        let obj = {
            'type': parseInt($('#payment-voucher-type').val()),
            'paymentType': parseInt($('#payment-voucher-pay-type').val()),
            'bankingAccountId': $('#bankingAccount').val() == null || $('#bankingAccount').val() == ''
                || $('#bankingAccount').val() == undefined ? 0 : parseInt($('#bankingAccount').val()),
            'description': $('#description').val(),
            'note': $('#content').val(),
            'clientId': parseInt(($('#client-select').val())),
            'bankName': $('#bankName').val(),
            'bankAccount': $('#bankAccount').val(),
            'sourceAccount': $('#bankingAccountSource').val(),
            'supplierId': parseInt(($('#supplier-select').val())),
            'amount': parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', '')),
            'PaymentRequestDetails': PaymentRequestDetails,
            'OtherImages': other_image
        }
        //formData.append('imagefile', file);
        //formData.append('jsonData', JSON.stringify(obj))
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentVoucher/AddNewJson",
            type: "Post",
            data: obj,
            success: function (result) {
                _global_function.RemoveLoading()
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    _global_function.ConfirmFileUpload($('.attachment-addnew'), result.id)
                    setTimeout(function () { window.location.reload() }, 500)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
    OnChooseType: function () {
        var payment_request_type = $('#payment-voucher-type').val()
        $("#body_payment_requests").empty();
        $('#bankName').val("")
        $("#bankingAccount").empty();
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) != 3) { // thanh toán dịch vụ và khác
            $('#lblSupplier').show()
            $('#divSupplier').show()
            $('#lblCustomer').hide()
            $('#divCustomer').hide()
            var supplier_id = $('#supplier-select').val()
            if (supplier_id !== null && supplier_id !== undefined && supplier_id !== '') {
                this.GetDataByClientOrSupplier(0, parseInt(supplier_id[0]))
            }
        }
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) == 3) {//hoàn trả khách hàng
            $('#lblSupplier').hide()
            $('#divSupplier').hide()
            $('#lblCustomer').show()
            $('#divCustomer').show()
            var client_id = $('#client-select').val()
            if (client_id !== null && client_id !== undefined && client_id !== '') {
                this.GetDataByClientOrSupplier(parseInt(client_id[0]), 0)
            }
        }
    },
    OnChooseTypeEdit: function (client_id, supplier_id, bankAccountId) {
        bankingAccountId = bankAccountId
        isEditView = true
        if ((client_id == undefined || client_id == null || client_id == 0 || client_id == '')
            && (supplier_id == undefined || supplier_id == null || supplier_id == 0 || supplier_id == '')) {
            return
        }
        amountEdit = parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', ''))
        isEdit = true
        if (client_id !== undefined && client_id !== null && client_id !== 0 && client_id !== '') {
            _add_payment_voucher.GetListBankAccountByClientID(client_id);
            setTimeout(function () {
                var newOption = new Option($('#client_name_hide').val(), client_id, true, true);
                $('#client-select').append(newOption).trigger('change');
            }, 500)
        }
        if (supplier_id !== undefined && supplier_id !== null && supplier_id !== 0 && supplier_id !== '') {
            _add_payment_voucher.GetListBankAccountBySupplierID(supplier_id);
            setTimeout(function () {
                var newOption = new Option($('#supplier_name_hide').val(), supplier_id, true, true);
                $('#supplier-select').append(newOption).trigger('change');
            }, 500)
        }

        var payment_request_type = $('#payment-voucher-type').val()
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) == 1) { // thanh toán dịch vụ
            $('#amount').attr('disabled', true)
            $('#amount').addClass('background-disabled')
        }
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) == 3) {//hoàn trả khách hàng
            $('#amount').attr('disabled', true)
            $('#amount').addClass('background-disabled')
        }
        if (client_id !== null && client_id !== undefined && client_id !== '' && client_id !== 0) {
            this.GetDataByClientOrSupplier(client_id, 0, true)
        }
        if (supplier_id !== null && supplier_id !== undefined && supplier_id !== '' && supplier_id !== 0) {
            this.GetDataByClientOrSupplier(0, supplier_id, true)
        }
        var payment_request_type = $('#payment-voucher-type').val()
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) != 3) { // thanh toán dịch vụ và khác
            $('#lblSupplier').show()
            $('#divSupplier').show()
            $('#lblCustomer').hide()
            $('#divCustomer').hide()
        }
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) == 3) {//hoàn trả khách hàng
            $('#lblSupplier').hide()
            $('#divSupplier').hide()
            $('#lblCustomer').show()
            $('#divCustomer').show()
        }
        _add_payment_voucher.OnChoosePaymentType()
    },
    Close: function () {
        $('#create_contract_pay').removeClass('show')
        setTimeout(function () {
            $('#create_contract_pay').remove();
        }, 300);
    },
    EditPaymentVoucher: function () {
        let validate = _add_payment_voucher.Validate()
        if (!validate)
            return;
        let PaymentRequestDetails = []
        for (var i = 0; i < listPaymentRequest.length; i++) {
            var checked = $('#order_ckb_' + listPaymentRequest[i].paymentCode).is(":checked")
            if (checked) {
                PaymentRequestDetails.push({ 'id': listPaymentRequest[i].id })
            }
        }
        //var formData = new FormData();
        //const file = document.querySelector('input[name=imagefile]').files[0];
        let other_image = [];
        $('#suplier_room_list_image .img_other').each(function () {
            let image_item = $(this);
            other_image.push(image_item.attr('src'));
        });
        let obj = {
            'id': parseInt($('#paymentVoucherId').val()),
            'paymentCode': $('#paymentCode').val(),
            'type': parseInt($('#payment-voucher-type').val()),
            'paymentType': parseInt($('#payment-voucher-pay-type').val()),
            'bankingAccountId': $('#bankingAccount').val() == null || $('#bankingAccount').val() == ''
                || $('#bankingAccount').val() == undefined ? 0 : parseInt($('#bankingAccount').val()),
            'attachFiles': $('#attachmentFile').val(),
            'description': $('#description').val(),
            'bankName': $('#bankName').val(),
            'bankAccount': $('#bankAccount').val(),
            'note': $('#content').val(),
            'clientId': parseInt(($('#client-select').val())),
            'supplierId': parseInt(($('#supplier-select').val())),
            'sourceAccount': $('#bankingAccountSource').val(),
            'amount': parseFloat($('#amount').val().replaceAll('.', '').replaceAll(',', '')),
            'paymentRequestDetails': PaymentRequestDetails,
            'OtherImages': other_image
        }
        //formData.append('imagefile', file);
        //formData.append('jsonData', JSON.stringify(obj))
        _global_function.AddLoading()
        $.ajax({
            url: "/PaymentVoucher/Update",
            type: "Post",
            data: obj,
            success: function (result) {
                _global_function.RemoveLoading()
                if (result.isSuccess) {
                    _msgalert.success(result.message);
                    $.magnificPopup.close();
                    _global_function.ConfirmFileUpload($('.attachment-edit'), parseInt($('#paymentVoucherId').val()))
                    setTimeout(function () { window.location.reload() }, 500)
                } else {
                    _msgalert.error(result.message);
                }
            }
        });
    },
    OnChoosePaymentType: function () {
        var pay_type = $('#payment-voucher-pay-type').val()
        if (parseInt(pay_type) != 2) { //thanh toán tiền mặt
            $('#bankingAccount').attr('disabled', true)
            $('#bankName').attr('disabled', true)
            $('#bankAccount').attr('disabled', true)
            $('#lblBankAccountRequired').hide()
            $('#lblBankNameRequired').hide()
            $('#lblBankAccount').show()
            $('#lblBankName').show()
            $('#bankingAccount').val(-1)
        } else {
            $('#bankingAccount').attr('disabled', false)
            $('#bankName').attr('disabled', false)
            $('#bankAccount').attr('disabled', false)
            $('#lblBankAccountRequired').show()
            $('#lblBankNameRequired').show()
            $('#lblBankAccount').hide()
            $('#lblBankName').hide()
        }
        var payment_request_type = $('#payment-voucher-type').val()
        if (payment_request_type !== null && payment_request_type !== '' && (parseInt(payment_request_type) == 1 || parseInt(payment_request_type) == 2)) { // thanh toán dịch vụ
            _add_payment_voucher.GetListBankAccountBySupplierID($('#supplier-select').val())
        }
        if (payment_request_type !== null && payment_request_type !== '' && parseInt(payment_request_type) == 3) { // thanh toán dịch vụ
            _add_payment_voucher.GetListBankAccountByClientID($('#client-select').val());
        }
    },
    OnCheckBox: function (index) {
        var totalAmount = 0
        $("#paymentRequestCode").empty()
        for (var i = 0; i < listPaymentRequest.length; i++) {
            var checked = $('#order_ckb_' + listPaymentRequest[i].paymentCode).is(":checked")
            if (checked) {
                totalAmount += listPaymentRequest[i].amount
                listPaymentRequest[i].isChecked = true
                $('#order_ckb_' + listPaymentRequest[i].paymentCode).prop('checked', true)
            } else {
                listPaymentRequest[i].isChecked = false
                $('#order_ckb_' + listPaymentRequest[i].paymentCode).prop('checked', false)
            }
        }
        $('#amount').val(this.FormatNumberStr(totalAmount))
        setTimeout(_add_payment_voucher.RenderItemChecked(), 500)
    },
    AddToListDetail: function (index) {
        var payment_request_type = $('#payment-voucher-type').val()
        if (payment_request_type !== null && payment_request_type !== '' && (parseInt(payment_request_type) == 1 || parseInt(payment_request_type) == 3)) { // thanh toán dịch vụ
            for (var i = 0; i < listPaymentRequest.length; i++) {
                if (i === index) {
                    listDetail.push(listPaymentRequest[i])
                }
            }
        }
    },
    OnCheckedRequest: function () {
        var requestChoose = $('#paymentRequestCode').val()
        $("#paymentRequestCode").empty()
        if (requestChoose === undefined || requestChoose === null || requestChoose === '') return
        for (var i = 0; i < listPaymentRequest.length; i++) {
            if (requestChoose !== undefined && requestChoose !== null && requestChoose !== ''
                && requestChoose.includes(listPaymentRequest[i].id + '')) {
                listPaymentRequest[i].isChecked = true
                $('#order_ckb_' + listPaymentRequest[i].paymentCode).prop('checked', true)
            } else {
                listPaymentRequest[i].isChecked = false
                $('#order_ckb_' + listPaymentRequest[i].paymentCode).prop('checked', false)
            }
        }
        setTimeout(_add_payment_voucher.OnCheckBox(), 300)
    },
    RenderItemChecked: function () {
        //if (isEditView && isRender) return
        $("#body_payment_requests").empty();
        var totalAmount = 0
        var listChecked = listPaymentRequest.filter(n => n.isChecked)
        var listUnChecked = listPaymentRequest.filter(n => !n.isChecked)
        var requestChoose = $('#paymentRequestCode').val()
        var index = 1
        $("#paymentRequestCode").empty()
        for (var i = 0; i < listChecked.length; i++) {
            $('#request-relate-table').find('tbody').append(
                "<tr id='order_" + index + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox'  id='order_ckb_" + listChecked[i].paymentCode + "' name='order_ckb' onclick='_add_payment_voucher.OnCheckBox(" + i + ");_add_payment_voucher.AddToListDetail(" + i + ")'>" +
                " <span class='checkmark'></span>" + (index) +
                "  </label>"
                + "</td>" +
                "<td>" +
                " <a class='blue' href='/PaymentRequest/Detail?paymentRequestId=" + listChecked[i].id + "'> " + listChecked[i].paymentCode + " </a>"
                + "</td>" +
                "<td>" + listChecked[i].paymentDateViewStr
                + "</td>" +
                "<td>" + listChecked[i].userName + "</td>" +
                "<td>" + _add_payment_voucher.FormatNumberStr(listChecked[i].amount) + "</td>" +
                "</tr>"
            );
            totalAmount += listChecked[i].amount
            if (listChecked[i].isChecked) {
                let index = i
                let code = listChecked[i].paymentCode
                setTimeout(function () {
                    $('#order_ckb_' + code).prop('checked', true)
                }, 300)
            }
            index++
        }
        for (var i = 0; i < listChecked.length; i++) {
            if (requestChoose !== undefined && requestChoose !== null && requestChoose !== ''
                && requestChoose.includes(listPaymentRequest[i].id + '')) {
                continue
            } else {
                if (listChecked[i].isChecked) {
                    let indexCount = i
                    var newOption = new Option(listChecked[indexCount].paymentCode, listChecked[indexCount].id, true, true);
                    $('#paymentRequestCode').append(newOption);
                }
            }
        }
        for (var i = 0; i < listUnChecked.length; i++) {
            $('#request-relate-table').find('tbody').append(
                "<tr id='order_" + index + "'>" +
                "<td>" +
                "<label class='check-list number'>" +
                " <input type='checkbox'  id='order_ckb_" + listUnChecked[i].paymentCode + "' name='order_ckb' onclick='_add_payment_voucher.OnCheckBox(" + i + ");_add_payment_voucher.AddToListDetail(" + i + ")'>" +
                " <span class='checkmark'></span>" + (index
                ) +
                "  </label>"
                + "</td>" +
                "<td>" +
                " <a class='blue' href='/PaymentRequest/Detail?paymentRequestId=" + listUnChecked[i].id + "'> " + listUnChecked[i].paymentCode + " </a>"
                + "</td>" +
                "<td>" + listUnChecked[i].paymentDateViewStr
                + "</td>" +
                "<td>" + listUnChecked[i].userName + "</td>" +
                "<td>" + _add_payment_voucher.FormatNumberStr(listUnChecked[i].amount) + "</td>" +
                "</tr>"
            );
            index++
        }
        $('#request-relate-table').find('tbody').append(
            "<tr style='font-weight:bold !important;'>" +
            "<td class='text-right' colspan='4'> Tổng </td>" +
            "<td>" + _add_payment_voucher.FormatNumberStr(totalAmount) + "</td>" +
            "</tr>"
        );
        _add_payment_voucher.AddItemToInput()
    },
    AddItemToInput: function () {
        if (isSetInput) return
        isSetInput = true
        for (var i = 0; i < listPaymentRequest.length; i++) {
            if (listPaymentRequest[i].isChecked) {
                var newOption = new Option(listPaymentRequest[i].paymentCode, listPaymentRequest[i].id, true, true);
                $('#paymentRequestCode').append(newOption)
            }
        }
        $('#paymentRequestCode').trigger('change');
    },
    OnChangeImage: function () {
        var file = document.querySelector('input[name=imagefile]').files[0]
        if (file !== undefined && file !== null) {
            var fileName = file.name
            if (!(fileName.includes('.jpg') || fileName.includes('.png') || fileName.includes('.jpeg') || fileName.includes('.gif '))) {
                //_msgalert.error('File đính kèm không đúng định dạng ảnh .png, .jpg, .jpeg, gif. Vui lòng kiểm tra lại');
                _add_payment_voucher.DisplayError('validate-imagefile', 'File đính kèm không đúng định dạng ảnh .png, .jpg, .jpeg, gif. Vui lòng kiểm tra lại')
                document.getElementById("imagefile").value = "";
            }
            $('#btnDeleteImage').show()
        }
    },
    OnDeleteImage: function () {
        document.getElementById("imagefile").value = "";
        $('#btnDeleteImage').hide()
    },
    ClearError: function () {
        $(".validate-payment-voucher-type").find('p').remove();
        $(".validate-supplier-select").find('p').remove();
        $(".validate-client-select").find('p').remove();
        $(".validate-amount").find('p').remove();
        $(".validate-payment-voucher-pay-type").find('p').remove();
        $(".validate-bankingAccount").find('p').remove();
        $(".validate-bankingName").find('p').remove();
        $(".validate-content").find('p').remove();
        $(".validate-description").find('p').remove();
        $(".validate-imagefile").find('p').remove();
    },
    DisplayError: function (className, message) {
        $("." + className).find('p').remove();
        $("." + className).append("<p>" + message + "</p>");
        $("." + className).css("color", "red");
        $("." + className).css("font-size", "13px");
        $("." + className).css("margin-top", "3px");
    },
    FileAttachment: function (data_id, type, readonly = false) {
        _global_function.RenderFileAttachment($('.attachment_file'), data_id, type, false, true)
    },
}