using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class MutipleSelectionItem
    {
        //Integer value of a checkbox
        public string Value { get; set; }

        //String name of a checkbox
        public string Text { get; set; }

        //Boolean value to select a checkbox
        //on the list
        public bool IsSelected { get; set; }

        //Object of html tags to be applied
        //to checkbox, e.g.:'new{tagName = "tagValue"}'
        public object Tags { get; set; }
    }

    public class PostedMutipleSelectionItems
    {
        //this array will be used to POST values from the form to the controller
        public string[] AccountIds { get; set; }
    }

    public class MutipleSelectionModel
    {
        public IEnumerable<MutipleSelectionItem> Items { get; set; }
        public IEnumerable<MutipleSelectionItem> SelectedItems { get; set; }
        public PostedMutipleSelectionItems PostedItems { get; set; }
    }
}