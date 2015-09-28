function LoadComboBox(DataUrl, ValueField, TextField, SelectedValue, ddlComboBox) {
    $.ajax({
        url: DataUrl,
        type: 'GET',
        //data: { someValue: value },
        //start: function () { $('.action-inprogress').show(); },
        //complete: function () { $('.action-inprogress').hide(); },
        success: function (result) {
            $("#" + ddlComboBox).jqxComboBox('clear');
            // Create a jqxComboBox
            $("#" + ddlComboBox).jqxComboBox({
                source: result, displayMember: TextField, valueMember: ValueField,
                theme: 'energyblue',
                width: '200px',
                height: '25px'
            });
            //$("#" + ddlComboBox).on('change', function (event) {
            //    var args = event.args;
            //    if (args) {
            //        //// index represents the item's index.                          
            //        //var index = args.index;
            //        var item = args.item;
            //        //// get item's label and value.
            //        //var label = item.label;
            //        //var value = item.value;
            //        $("#" + ddlTarget).val(item.value);
            //    }
            //});
            var itemSelect = { label: '-Select-', value: "0" };
            $("#" + ddlComboBox).jqxComboBox('removeItem', "0");
            $("#" + ddlComboBox).jqxComboBox('insertAt', itemSelect, 0);
            var selectedItem = { label: '-Select-', value: "0" };
            if (SelectedValue != null && SelectedValue != "") {
                selectedItem.value = SelectedValue;
            }
            console.log(selectedItem)
            $("#" + ddlComboBox).jqxComboBox('selectItem', selectedItem);
        }
    });
}