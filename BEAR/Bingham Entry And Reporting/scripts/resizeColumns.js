function pageLoad(args){
    //  get all of the th elements from the gridview
    _ths = $get('searchResults').getElementsByTagName('TH');
    
    //  if the grid has at least one th element
    if(_ths.length > 1){
    
        for(i = 0; i < _ths.length; i++){
            //  determine the widths
            _ths[i].style.width = Sys.UI.DomElement.getBounds(_ths[i]).width + 'px';
        
            //  attach the mousemove and mousedown events
            if(i < _ths.length - 1){
                $addHandler(_ths[i], 'mousemove', _onMouseMove);
                $addHandler(_ths[i], 'mousedown', _onMouseDown);
            }
        }

        //  add a global mouseup handler            
        $addHandler(document, 'mouseup', _onMouseUp);
        //  add a global selectstart handler
        $addHandler(document, 'selectstart', _onSelectStart);
    }       
}


function _onMouseMove(args){    
    if(_isResizing){
        
        //  determine the new width of the header
        var bounds = Sys.UI.DomElement.getBounds(_element); 
        var width = args.clientX - bounds.x;
        
        //  we set the minimum width to 1 px, so make
        //  sure it is at least this before bothering to
        //  calculate the new width
        if(width > 1){
        
            //  get the next th element so we can adjust its size as well
            var nextColumn = _element.nextSibling;
            var nextColumnWidth;
            if(width < _toNumber(_element.style.width)){
                //  make the next column bigger
                nextColumnWidth = _toNumber(nextColumn.style.width) + _toNumber(_element.style.width) - width;
            }
            else if(width > _toNumber(_element.style.width)){
                //  make the next column smaller
                nextColumnWidth = _toNumber(nextColumn.style.width) - (width - _toNumber(_element.style.width));
            }   
            
            //  we also don't want to shrink this width to less than one pixel,
            //  so make sure of this before resizing ...
            if(nextColumnWidth > 1){
                _element.style.width = width + 'px';
                nextColumn.style.width = nextColumnWidth + 'px';
            }
        }
    }   
    else{
        //  get the bounds of the element.  If the mouse cursor is within
        //  2px of the border, display the e-cursor -> cursor:e-resize
        var bounds = Sys.UI.DomElement.getBounds(args.target);
        if(Math.abs((bounds.x + bounds.width) - (args.clientX)) <= 2) {
            args.target.style.cursor = 'e-resize';
        }  
        else{
            args.target.style.cursor = '';
        }          
    }         
}


function _onMouseDown(args){
    //  if the user clicks the mouse button while
    //  the cursor is in the resize position, it means
    //  they want to start resizing.  Set _isResizing to true
    //  and grab the th element that is being resized
    if(args.target.style.cursor == 'e-resize') {
        _isResizing = true;
        _element = args.target;               
    }                    
} 
function _onMouseUp(args){
    //  the user let go of the mouse - so
    //  they are done resizing the header.  Reset
    //  everything back
    if(_isResizing){
        
        //  set back to default values
        _isResizing = false;
        _element = null;
        
        //  make sure the cursor is set back to default
        for(i = 0; i < _ths.length; i++){   
            _ths[i].style.cursor = '';
        }
    }
} 
function _onSelectStart(args){
    // Don't allow selection during drag
    if(_isResizing){
        args.preventDefault();
        return false;
    }
}    

function _toNumber(m) {
    //  helper function to peel the px off of the widths
    return new Number(m.replace('px', ''));
}