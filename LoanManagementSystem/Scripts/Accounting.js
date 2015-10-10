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
            var receiptPrefix = "";
            if (result.ReceiptVoucherPrefix != null)
                receiptPrefix = result.ReceiptVoucherPrefix;

            var receiptSuffix = "";
            if (result.ReceiptVoucherSuffix != null)
                receiptSuffix = result.ReceiptVoucherSuffix;

            var PaymentVoucherPrefix = "";
            if (result.PaymentVoucherPrefix != null)
                PaymentVoucherPrefix = result.PaymentVoucherPrefix;

            var PaymentVoucherSuffix = "";
            if (result.PaymentVoucherSuffix != null)
                PaymentVoucherSuffix = result.PaymentVoucherSuffix;

            $('#lblBookBalance').html(result.ClosingBalance)
            if (TransactionType == "DEBIT")
                $('#txtVoucherNo').html(receiptPrefix + result.VoucherNo + receiptSuffix)
            else
                $('#txtVoucherNo').html(PaymentVoucherPrefix + result.VoucherNo + PaymentVoucherSuffix)
        }
    });

    return false;
}

$(document).ready(function () {
    $('#ddlAccountBookId').change(function () {
        LoanAccountDetails($(this).val());
    });

    LoanAccountDetails($('#ddlAccountBookId').val());
});