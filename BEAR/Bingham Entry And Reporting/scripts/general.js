//randomize images in poster directory at startup.
function loadPoster() 
{
    NumberOfImagesToRotate = 15;
    FirstPart = '<img src="../images/posters/bingham'
    SecondPart = '.jpg" />'
    var r = Math.ceil(Math.random() * NumberOfImagesToRotate);
    document.write(FirstPart + r + SecondPart);
}


function changeScreenSize(w,h)
{
    window.resizeTo( w,h )
}


/*
* Gets a Value from a RadioButtonList 
* and Returns the Value to a TextBox
*/
function RadioButtonToTextBox(id, textBoxId)
{
    var radio = document.getElementsByName(id);
    var textbox = document.getElementById(textBoxId);
    for (var i=0; i < radio.length; i++)
    {
        if (radio[i].checked)
        {
            textbox.value=radio[i].value;
            textbox.focus();
            break;
        }
    }
}

function RadioButtonAppendTextBox(id, textBoxId)
{
    var radio = document.getElementsByName(id);
    var textbox = document.getElementById(textBoxId);
    for (var i=0; i < radio.length; i++)
    {
        if (radio[i].checked)
        {
            if(textbox.value=='') {
                textbox.value=radio[i].value;
                textbox.focus();
                break;
            }
            else
            {
                if(textbox.value.search(radio[i].value)==-1) {
                    textbox.value += ',' + radio[i].value;
                }
                textbox.focus();
                break;
            }
        }
    }
}



function TextBoxOnFocusAllToBlank(textBoxId)
{
    if(document.getElementById(textBoxId).value=="All")
        document.getElementById(textBoxId).value="";
}



// number formatting function
// copyright Stephen Chapman 24th March 2006, 22nd August 2008
// permission to use this function is granted provided
// that this copyright notice is retained intact
//http://javascript.about.com/library/blnumfmt.htm
//num = variable to format
//dec = decimal places
//thou = thousands separation
//pnt = decimal point character
//curr1 = leading currency
//curr2 = following currency
//n1 = front negative sign
//n2 = following negative sign
function formatNumber(num,dec,thou,pnt,curr1,curr2,n1,n2) {
    var x = Math.round(num * Math.pow(10,dec));
    if (x >= 0) 
        n1=n2='';
    var y = (''+Math.abs(x)).split('');
    var z = y.length - dec; 
    if (z<0) 
        z--; 
    for(var i = z; i < 0; i++) 
        y.unshift('0'); 
    if (z<0) 
        z = 1; 
    y.splice(z, 0, pnt); 
    if(y[0] == pnt) 
        y.unshift('0'); 
    while (z > 3) 
    {
        z-=3; 
        y.splice(z,0,thou);
    }
    var r = curr1+n1+y.join('')+n2+curr2;
    return r;
}

function stripCommas(numString) {
    var re = /,/g;
    return numString.replace(re,"");
}