window.onscroll = window_onscroll;

function window_onscroll()
{
    if (document.all) {
        options.style.top = document.documentElement.scrollTop;
        //options.style.left = document.documentElement.scrollLeft;
    }
}



function CreateDataDivCookie(){
    var strCook = document.cookie;
    
    if(strCook.indexOf("!~")!=0){
        var intS = strCook.indexOf("!~");
        var intE = strCook.indexOf("~!");
        var strPos = strCook.substring(intS+2,intE);
        document.getElementById("data").scrollTop = strPos;
    }
    if(strCook.indexOf("!#")!=0){
        var intS = strCook.indexOf("!#");
        var intE = strCook.indexOf("#!");
        var strPos = strCook.substring(intS+2,intE);
        document.getElementById("data").scrollLeft = strPos;
    }
}


function SetDataDivPosition(){
    var intY = document.getElementById("data").scrollTop;
    var intX = document.getElementById("data").scrollLeft;
    
    var date = new Date();
    date.setTime(date.getTime() + (100*24*60*60*1000) );
    var expires = "; expires="+date.toDateString();
    
    //document.title = intX; // for debugging to see the scroll position in the title.
    document.cookie = "yPos=!~" + intY + "~!" + expires;
    document.cookie = "xPos=!#" + intX + "#!" + expires;
}


function ResetDataDivPosition() {
    document.cookie = "yPos=!~0~!";
    document.cookie = "xPos=!#0#!";
}



function getScrollBottom(p_oElem) {
    return p_oElem.scrollHeight - p_oElem.scrollTop - p_oElem.clientHeight;
}


