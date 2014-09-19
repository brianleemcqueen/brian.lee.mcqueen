/*
Function used with onChange event on the gridView to bold the 
row of a changed row.
*/
function ChangeRow(cssClassName) {
    var obj = window.event.srcElement;
    if(obj.tagName == "INPUT" && obj.type == "text")
    {
        obj = obj.parentElement.parentElement;
        obj.className = cssClassName.toString();   
    }
}

function GiveControlFocus(controlID) {
    document.getElementById(controlID).focus();
    document.getElementById(controlID).select();
}

function ChangeCell(cssClassName) {
    var obj = window.event.srcElement;
    obj.className = cssClassName.toString();   
}

function VerifyNumeric(maxLength, fieldDescription) {
    var obj = window.event.srcElement;
    if(isNaN(obj.value)) 
    {
        alert('Please enter a numeric value');
        GiveControlFocus(window.event.srcElement.id);
    } 
    else if (maxLength != -1) 
    {
        if ( obj.value.length > maxLength ) 
        {
            alert('Value entered for '+ fieldDescription + ' is too long.\nThe maximum length is ' + maxLength);
            GiveControlFocus(window.event.srcElement.id);
        }
    } 
}

function ChangeTextBoxBackgroundColor() {
    var obj = window.event.srcElement;
    obj.style.backgroundColor = '#A7B8CB';
    obj.focus();

}

function RestoreTextBoxBackgroundColor() {
    var obj = window.event.srcElement;
    obj.style.backgroundColor = 'White';
}


//variable that will store the id of the last clicked row
var previousRow;
var firstRunAfterPostBack = true;
function ChangeRowColor(row, rowindex, selectedIndex)
{
//alert(row);
    //If last clicked row and the current clicked row are same
    if (previousRow == row) 
    {
        return;//do nothing
    }
    else if (previousRow != null) 
    {
        //change the color of the previous row back to it previous color
        //The grid has alternating colors
        if(rowindex==0 || rowindex%2 == 0) 
        {
            document.getElementById(previousRow).style.backgroundColor = "White";
        } 
        else 
        {
            document.getElementById(previousRow).style.backgroundColor = "#F7F6F3";
        }
    }
    
    //Set the background color of the selected row
    document.getElementById(row).style.backgroundColor = "#E2DED6";            
    
    if(selectedIndex != -1) 
    {
        if(firstRunAfterPostBack) {
            var gv2 = document.getElementById("GridView2");
            var rowElement = gv2.rows[selectedIndex+1];
            if((selectedIndex+1)%2==0) 
            {
                rowElement.style.backgroundColor = "White";
            } else 
            {
                rowElement.style.backgroundColor = "#F7F6F3";
            }
            firstRunAfterPostBack = false;
        } //else {
            //firstRunAfterPostBack = true;
        //}
    }
    
    //assign the current row id to the previous row id for next row to be clicked
    previousRow = row;
}

