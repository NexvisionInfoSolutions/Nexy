function validateAccountData() {
    return true;
}

var AccTransactionType_CASH = "CASH";
var AccTransactionType_BANK = "CASH";
var AccTransactionType_JOURNAL = "CASH";
var AccTransactionType = AccTransactionType_CASH;
var TransactionType = "CREDIT";
var BaseUrl = '/Accounting/GetBookAccountDetails';
function LoanAccountDetails(bookIdVal) {
    //#ddlAccountBookId
    //#lblBookBalance
    //#txtVoucherNo
    //.account-detail-amount

    $.ajax({
        url: BaseUrl+ '?AccountBookId=' + bookIdVal + '&TransType=' + AccTransactionType,
        type: 'GET',
        //data: { someValue: value },
        success: function (result) {
            $('#lblBookBalance').html(result.ClosingBalance)
            if (TransactionType == "DEBIT")
                $('#txtVoucherNo').val(result.ReceiptVoucherPrefix + result.VoucherNo + result.ReceiptVoucherSuffix)
            else
                $('#txtVoucherNo').val(result.PaymentVoucherPrefix + result.VoucherNo + result.PaymentVoucherSuffix)
        }
    });

    return false;
}

$(document).ready(function () {
    $('#ddlAccountBookId').change(function () {
        LoanAccountDetails($(this).val());
    });
});