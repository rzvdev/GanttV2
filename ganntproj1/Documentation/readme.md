

    Up next v2.0

    In next version:
        -Readme text (Description)
        -Scoll anoying bug

	
## Full and real Bars + DVC   

<!-- full bars -->
    var fullBarRect = new Rectangle(x, y, w, h);          
<!-- real bars -->    
    var rddBarRect = new Rectangle(((x + w) - rw), y - 2, w - (w - rw), h - 8);	
<!-- DVC -->
    grfx.DrawLine(Pens.Red, ((x + w) - rw), y, ((x + w) - rw), y + 8);
##
