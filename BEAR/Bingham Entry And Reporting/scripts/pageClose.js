/* With Firefox you have to set manually under about:config
 * dom.allow_scripts_to_close_windows to true
 */
function ExitApplication() 
{
    if(confirm("Exit Application?")) {
        window.close();
    }
}

/*
Function to call to allow JavaScript to save the document.
closeIt() is called by the <body onbeforeunload="closeIt()"> event
causing the form to be saved when the browser is closed or 
navigated away from.
*/
function closeIt() 
{
     document.form1.submit();
}


function UnloadPage() 
{
     document.form1.submit();
}

