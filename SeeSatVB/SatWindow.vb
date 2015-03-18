Imports System.Drawing.Drawing2D

Public Class SatWindow

    Public Shared DEBUG As Boolean = False

    ' the following are global
    Public Shared WSIZE As Integer = 10000       ' our absolute drawing surface coordinate system size of half of the x,y axis
    Public Shared WBORDER As Double = 0.05      ' the percentage of width of the border around the graphics area that we clip off
    Public Shared WSCALE As Double = 1          ' the global scale factor
    Public Shared WMAXSCALE As Double = 16      ' maximum scale factor
    Public Shared MJITTER As Integer = 4        ' the maximum amount the mouse can move just sitting there

    ' the projection to use
    'Public Shared VIEWSTEREO As Boolean = False ' use sterographic projection
    Public Shared VIEWSTEREO As Boolean = SeeSatVBmain.my_params.view_stereo ' use sterographic projection

    'Public Shared pbgr As Graphics = SatWindow.CanvasPBox.CreateGraphics
    Public Shared grMatrix As System.Drawing.Drawing2D.Matrix
    Public Shared WXOFFSET As Integer = 0   ' the x and y offsets for the window when it is zoomed
    Public Shared WYOFFSET As Integer = 0
    Public Shared WXROTATE As Single = 0    ' the amount to rotate the screen (before this is implemented we need to work out the matrix math)

    Public Shared CanvasTotal As Rectangle      ' form size
    Public Shared CanvasBounds As RectangleF    ' canvas size
    Public Shared Canvas As Region              ' our drawing canvas
    'Public Shared screenRectangle As Rectangle  ' Screen.PrimaryScreen.Bounds

    Public Shared mouse_xy As Point     ' the current cursor position
    Public Shared mouse_xy_old As Point ' the previous cursor position
    Public Shared mouse_xy_user As Point    ' the user space coordinates of the cursor

    'Public Shared tPen As Integer = 1        ' tiny pen
    'Public Shared sPen As Integer = 2        ' small pen
    'Public Shared mPen As Integer = 3      ' medium pen
    'Public Shared bPen As Integer = 5    ' big pen
    Public Shared tPen As Integer = CInt(WSIZE * 0.001)        ' tiny pen
    Public Shared sPen As Integer = CInt(WSIZE * 0.002)        ' small pen
    Public Shared mPen As Integer = CInt(WSIZE * 0.003)      ' medium pen
    Public Shared bPen As Integer = CInt(WSIZE * 0.005)    ' big pen

    Public Shared gColor As Color = Color.DarkCyan  ' grid color
    Public Shared formColor As Color = Color.Black

    Public Shared sunColor As Color = Color.Yellow
    Public Shared duskColor As Color = Color.Orange
    Public Shared darkColor As Color = Color.DarkRed

    'Public Shared starColorF As Color = Color.DimGray
    'Public Shared starColorM As Color = Color.Gray
    'Public Shared starColorB As Color = Color.LightGray
    Public Shared starColorF As Color = Color.DimGray
    Public Shared starColorM As Color = Color.LightGray
    Public Shared starColorB As Color = Color.LightGray

    Public Shared LIMITMAG As Double = 8 ' smallest mag to display - change to 8
    Public Shared SHOWSTARS As Boolean = True  ' paint routine uses this to decide to display stars

    Public Shared STZ As Integer = CInt(WSIZE * 0.002)  ' 20 - used int plotazel base satellite size
    Public Shared SRZ As Integer = CInt(WSIZE * 0.001)  ' 10 - used int plotazel base star size

    Public Shared SatIsDirty As Boolean = False    ' our graphics area has changed
    Public Shared FirstTime As Boolean = True   ' we haven't drawn anything
    Public Shared StarIsDirty As Boolean = False

    ' these structures hold the graphic representation of our satellites
    ' Sats_I are a list of display coordinates, pens, and an internal index to a circular list or next free element
    Public Structure structSat_I
        Public ndxD As Integer      ' index back to satsD
        Public isActive As Boolean  ' set to false not to display or reuse
        Public p As Pen           ' pen information
        Public r As Rectangle       ' holds the ellipse to paint
    End Structure

    ' Sat_D is initiallized after the tle files are read and we know how many satellites we are dealing with
    ' lookups are a straight index into the array, wasteful of space but fast
    ' isActive is set true when the satellite rises and is set to false when it sets
    Public Structure structSat_D
        Public isActive As Boolean  ' set to false not to display, not initialized, or reuse
        Public ndxI As Integer      ' pointer to (circular) list if just displaying limited number of positions
        Public name As String       ' name of satellite
        Public SatsI() As structSat_I   ' the display list of satellite points
    End Structure

    Public Shared sizeofSatI As Integer = 100        ' initial size of the internal list of sat display points

    Public Shared SatsD() As structSat_D

    ' these structures hold the graphic representation of our stars
    Public Structure structStarD
        Public isActive As Boolean  ' set to false not to display, not initialized, or reuse
        Public mag As Double
        Public p As Pen             ' pen information
        Public r As Rectangle       ' holds the ellipse to paint
    End Structure

    Public Shared StarsD() As structStarD


    Private Sub SatWindow_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint

        SetWindow(e.Graphics)
        'TestWindow(e.Graphics)
        'testTrans(e.Graphics)
        'FillRegionExcludingPath(e)

        TimerR.Enabled = False
        TimerM.Enabled = False

        DrawGrid(e.Graphics)

        TextBoxS.Width = CInt(CanvasTotal.Width - (CanvasBounds.Left + CanvasBounds.Right))

        If SHOWSTARS = True And StarIsDirty = True And FirstTime = False Then
            DrawAllStars(e.Graphics)
            StarIsDirty = False
        End If

        If SatIsDirty = True And FirstTime = False Then
            DrawAllSats(e.Graphics)
            SatIsDirty = False
        End If

        FirstTime = False
        TimerR.Enabled = True
        TimerM.Enabled = True

    End Sub


    Private Sub SatWindow_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        Me.Invalidate()

    End Sub

    Private Sub SetWindow(ByVal gr As Graphics)
        ' set up our drawing window and coordinate system
        ' extern CanvasBounds, WSIZE

        ' set the clipping area to the window minumum less WBORDER %
        SetClip(gr)

        ' set the coordinate system to origin 0,0 in the middle and +y to the top
        SetScale(gr, CInt(CanvasBounds.Width), CInt(CanvasBounds.Height), CInt((WSIZE) / WSCALE), _
                 CInt((-WSIZE) / WSCALE), CInt((WSIZE) / WSCALE), CInt((-WSIZE) / WSCALE))

        'Me.Size = New Size(CInt(Me.Size.Width / WSCALE), CInt(Me.Size.Height / WSCALE))
        grMatrix = gr.Transform

        ' set the pen thicknesses
        'setPens()

        'clear the clipping region
        gr.ResetClip()

    End Sub

    Private Sub setPens()
        ' set our pen sizes
        tPen = 1
        sPen = 2
        mPen = 3
        bPen = 4

    End Sub


    ' set the clipping area
    Private Sub SetClip(ByVal gr As Graphics)

        Dim d As Integer

        CanvasTotal = Me.ClientRectangle
        d = Math.Min(CanvasTotal.Width, CanvasTotal.Height)
        CanvasBounds = New Rectangle(CInt(d * WBORDER), CInt(d * WBORDER), d - CInt(d * WBORDER) * 2, d - CInt(d * WBORDER) * 2)

        gr.Clip = New Region(CanvasBounds)

    End Sub

    ' from http://www.vb-helper.com/howto_net_set_scale.html
    ' Set transformations for the Graphics object
    ' so its coordinate system matches the one specified.

    Private Sub SetScale(ByVal gr As Graphics, ByVal gr_width _
        As Integer, ByVal gr_height As Integer, ByVal left_x As _
        Single, ByVal right_x As Single, ByVal top_y As Single, _
        ByVal bottom_y As Single)
        ' Start from scratch.
        gr.ResetTransform()

        Dim xoff, yoff As Integer

        ' Scale so the viewport's width and height
        ' map to the Graphics object's width and height.
        'Dim bounds As RectangleF = gr.ClipBounds
        gr.ScaleTransform( _
            gr_width / (right_x - left_x), _
            gr_height / (bottom_y - top_y), System.Drawing.Drawing2D.MatrixOrder.Append)

        Dim bounds As RectangleF = gr.ClipBounds

        xoff = CInt(Math.Min(Math.Abs(bounds.Right), Math.Abs(bounds.Left)) * Math.Sign(Math.Min(bounds.Left, bounds.Right)))
        yoff = CInt(Math.Min(Math.Abs(bounds.Top), Math.Abs(bounds.Bottom)) * Math.Sign(Math.Min(bounds.Top, bounds.Bottom)))

        ' Translate (left_x, top_y) to the Graphics
        ' object's origin.
        'gr.TranslateTransform(-left_x - WXOFFSET + CInt(bounds.Left), -top_y - WYOFFSET + CInt(bounds.Bottom))
        gr.TranslateTransform(-left_x - WXOFFSET + xoff, -top_y - WYOFFSET + yoff)

        'gr.RotateTransform(WXROTATE, System.Drawing.Drawing2D.MatrixOrder.Append)
        'gr.RotateTransform(WXROTATE)

    End Sub


    '  This routine adds a star to the diplay list
    '    the values in azel come from the external module xyztop in ASTRO.C
    '
    Public Shared Sub plotstar(ByVal starxy As star_xy, ByRef starE As structStarD)

        ' before these routines are called the routines in AstroVB that establish the observers position have to be called
        ' topos and xyztop - cleanup!

        Dim n As Integer
        Dim e As Integer
        Dim rval As Integer
        Dim dist As Integer

        'C++ TO VB CONVERTER NOTE: 'extern' variable declarations are not required in VB:
        '	extern Integer elsusa
        Dim StarColor As Color
        Dim i As Integer

        Dim p As Pen = New Pen(Color.Black)

        'If starxy.mag = 0.18 Then
        '    'we have rigel
        '    rval = 0
        'End If
        ''Dim OurGr As Graphics = SatWindow.CreateGraphics

        dist = CInt(Fix(WSIZE * Math.Cos(starxy.alt)))

        If VIEWSTEREO Then  ' sterographic projection
            n = CInt(WSIZE * Math.Cos(starxy.azm) * Math.Tan((DefConst.PIO2 - starxy.alt) / 2))
            e = CInt(-WSIZE * Math.Sin(starxy.azm) * Math.Tan((DefConst.PIO2 - starxy.alt) / 2))
        Else                ' simple projection
            n = CInt(Fix(dist * Math.Cos(starxy.azm)))
            e = CInt(Fix(-dist * Math.Sin(starxy.azm)))
        End If

        ' stereographic projections from http://www2.arnes.si/~gljsentvid10/horizon.html
        'x = COS(z) * TAN((90 - a) / 2)
        'y = SIN(z) * TAN((90 - a) / 2)
        'where a is altitude and z is azimuth of the star. These formulas
        'will actually give North along the X axis. 
        '90 - a is simply the Zenith angle of the star, and the Zenith is
        'the centre of your plot.

        Select Case starxy.mag
            Case Is > 8.0
                p.Width = tPen
                p.Color = starColorF
            Case 6.0 To 8.0
                p.Width = tPen
                p.Color = starColorF
            Case 4.0 To 6.0
                p.Width = mPen
                p.Color = starColorF
            Case 2.5 To 4.0
                p.Width = mPen
                p.Color = starColorM
            Case 1 To 2.5
                p.Width = mPen
                p.Color = starColorB
            Case Is < 1
                p.Width = bPen
                p.Color = starColorB
        End Select

        Select Case starxy.mag
            Case Is > 8.0
                i = SRZ
            Case Else
                i = CInt((-starxy.mag + 8) * SRZ)
        End Select

        'StarE = (true, p, New Rectangle(e - CInt(i / 2), n - CInt(i / 2), i, i))

        starE.isActive = True
        starE.mag = starxy.mag
        starE.p = p
        starE.r = New Rectangle(e - CInt(i / 2), n - CInt(i / 2), i, i)

    End Sub


    ' possibly call this from a timer
    ' initialize the display list of stars
    Public Function initStarD() As Integer
        Dim sz, ndx, i, j, rval As Integer

        If IsNothing(SatIO.stars) Then
            Return 0
        End If

        sz = SatIO.stars.Length() - 1       ' always one more than actual number of elements

        ReDim StarsD(sz)     ' destroy the existing array and create anew
        ndx = 1

        For i = 1 To sz
            If SatIO.stars(i).mag < LIMITMAG Then
                Dim starxy As New star_xy
                rval = AstroGR.calcstar(i, starxy)   ' 1 if visible
                If rval = 0 Then
                    Continue For
                End If

                Dim starE As New structStarD
                plotstar(starxy, starE)
                StarsD(ndx) = starE
                ndx += 1
            End If

        Next (i)

        ReDim Preserve StarsD(ndx)

        StarIsDirty = True
        TimerR.Enabled = True
        'Timer1.Start()

        Return (ndx)  ' number of elements read

    End Function

    ' walk throught the star display list and draw them on the screen
    Public Sub DrawAllStars(ByVal gr As Graphics)
        Dim ndx As Integer = 1      ' the 0 element is never used
        Dim last As Integer         ' the last element to be checked

        If StarsD Is Nothing Then
            StarIsDirty = False
            Exit Sub
        End If

        last = StarsD.Length - 1

        For ndx = 1 To last
            If StarsD(ndx).isActive = True Then
                ' need logic in here to determine limiting mag per WSCALE


                DrawOneStar(ndx, gr)
            End If
        Next
        StarIsDirty = False
        TimerR.Enabled = True

    End Sub

    Public Sub DrawOneStar(ByVal ndx As Integer, ByVal gr As Graphics)

        If StarsD(ndx).isActive = False Then ' don't do any more work
            Exit Sub
        End If

        Dim b As Brush
        Dim lmag As Double

        lmag = 4.5 + WSCALE / 4
        'lmag = 0.5

        If StarsD(ndx).mag < lmag Then
            b = New SolidBrush(StarsD(ndx).p.Color)
            gr.DrawLine(StarsD(ndx).p, StarsD(ndx).r.Left, StarsD(ndx).r.Bottom, StarsD(ndx).r.Right, StarsD(ndx).r.Top)
            gr.DrawLine(StarsD(ndx).p, StarsD(ndx).r.Left, StarsD(ndx).r.Top, StarsD(ndx).r.Right, StarsD(ndx).r.Bottom)
            'gr.FillRectangle(b, StarsD(ndx).r)
            gr.FillEllipse(b, StarsD(ndx).r)
        End If

    End Sub

    '  This routine adds the satellite to the display list
    '    the values in azel come from the external module xyztop in ASTRO.C
    '
    Public Shared Sub plotazel(ByVal SatNdx As Integer, ByVal azel As sph_t, ByVal mag As Double, elsusa As Double)

        Dim dist As Integer
        Dim n As Integer
        Dim e As Integer
        Dim rval As Integer

        'C++ TO VB CONVERTER NOTE: 'extern' variable declarations are not required in VB:
        '	extern Integer elsusa
        Dim SatColor As Color
        Dim i As Integer

        Dim p As Pen

        'Dim OurGr As Graphics = SatWindow.CreateGraphics
        If DEBUG Then
            ' set a conditional breakpoint here
            DEBUG = False
        End If

        ' This is a simple projection that compresses the area below 30 deg
        dist = CInt(Fix(WSIZE * Math.Cos(azel.phi)))

        If VIEWSTEREO Then  ' sterographic projection
            n = CInt(WSIZE * Math.Cos(azel.lambda) * Math.Tan((DefConst.PIO2 - azel.phi) / 2))
            e = CInt(-WSIZE * Math.Sin(azel.lambda) * Math.Tan((DefConst.PIO2 - azel.phi) / 2))
        Else                ' simple projection
            n = CInt(Fix(dist * Math.Cos(azel.lambda)))
            e = CInt(Fix(-dist * Math.Sin(azel.lambda)))
        End If

        'n = CInt(Fix(dist * Math.Cos(azel.lambda)))
        'e = CInt(Fix(-dist * Math.Sin(azel.lambda)))

        ' stereographic projections from http://www2.arnes.si/~gljsentvid10/horizon.html
        'x = COS(z) * TAN((90 - a) / 2)
        'y = SIN(z) * TAN((90 - a) / 2)

        'where a is altitude and z is azimuth of the star. These formulas
        'will actually give North along the X axis. 
        '90 - a is simply the Zenith angle of the star, and the Zenith is
        'the centre of your plot.

        Select Case elsusa
            Case Is < -2
                SatColor = darkColor
            Case -2 To 1
                SatColor = duskColor
            Case Is > 1
                SatColor = sunColor
        End Select

        p = New Pen(SatColor, tPen)

        Select Case mag
            Case Is > 8.0
                i = STZ
            Case Else
                i = CInt((-mag + 8) * STZ)
        End Select

        rval = AddDSat(SatNdx, p, New Rectangle(e - CInt(i / 2), n - CInt(i / 2), i, i))

        SatIsDirty = True
        StarIsDirty = True
        SatWindow.TimerR.Enabled = True
        'SatWindow.Timer1.Start()

    End Sub

    ' add a satellite display point to the array
    Public Shared Function AddDSat(ByVal ndx As Integer, ByVal p As Pen, ByVal r As Rectangle) As Integer
        Dim i As Integer = 0    ' return flag
        Dim j As Integer
        Dim sz As Integer = SatsD.Length()  ' one more than the number of elements

        ' not sure if this bit of code is redundant
        If ndx >= sz Then    ' we are adding new element
            ReDim Preserve SatsD(ndx)     ' resize existing array 

            For i = sz To ndx
                SatsD(i).isActive = False
                SatsD(i).ndxI = 1           ' pointer to display list
                SatsD(i).name = SeeSatVBmain.satellites(i).tle0.satname
                ReDim SatsD(i).SatsI(sizeofSatI)
                For j = 1 To sizeofSatI
                    SatsD(i).SatsI(j).isActive = False
                    SatsD(i).SatsI(j).ndxD = i
                Next (j)
            Next (i)
        End If
        ' end of redundancy

        Dim last As Integer = SatsD(ndx).SatsI.Length() - 1

        If SatsD(ndx).isActive = False Then
            For j = 1 To last
                SatsD(ndx).SatsI(j).isActive = False
                SatsD(ndx).SatsI(j).ndxD = ndx
            Next (j)
            SatsD(ndx).isActive = True
            SatsD(ndx).ndxI = 1
        Else
            SatsD(ndx).ndxI = CListNext(SatsD(ndx).ndxI, last)
        End If

        SatsD(ndx).SatsI(SatsD(ndx).ndxI).isActive = True
        SatsD(ndx).SatsI(SatsD(ndx).ndxI).p = p
        SatsD(ndx).SatsI(SatsD(ndx).ndxI).r = r

        AddDSat = SatsD(ndx).ndxI

    End Function

    ' get the next element in a circular list
    Public Shared Function CListNext(ByVal ndx As Integer, ByVal last As Integer) As Integer
        ' get the next element in a circular list
        ndx = ndx + 1
        If ndx > last Then
            ndx = 1
        End If
        CListNext = ndx
    End Function

    ' get the next active element in the display list
    Public Shared Function DListNext(ByRef ndx As Integer) As Boolean
        ' updates ndx
        Dim last As Integer
        Dim rval As Boolean = True    ' return false if looped all the way around the list
        Dim start As Integer = ndx

        last = SatsD.Length() - 1

        ndx = CListNext(ndx, last)

        While SatsD(ndx).isActive = False And ndx <> start
            ndx = CListNext(ndx, last)
        End While

        If ndx = start Then ' we have looped
            rval = False
        End If

        DListNext = rval
    End Function

    ' call from Main after the TLE files are read
    Public Function initSatsD() As Integer
        Dim sz, i, j As Integer
        sz = SeeSatVBmain.satellites.Length() - 1       ' always one more than actual number of elements

        ReDim SatsD(sz)     ' destroy the existing array and create anew

        For i = 1 To sz
            SatsD(i).isActive = False
            SatsD(i).ndxI = 1           ' pointer to display list
            SatsD(i).name = SeeSatVBmain.satellites(i).tle0.satname
            ReDim SatsD(i).SatsI(sizeofSatI)
            For j = 1 To sizeofSatI
                SatsD(i).SatsI(j).isActive = False
                SatsD(i).SatsI(j).ndxD = i
            Next (j)
        Next (i)

        Return (i)  ' number of elements read
    End Function

    ' if adding a new satellite element
    Public Function addSatsE(Optional ByVal ndx As Integer = 0) As Integer
        Dim sz, j As Integer
        Dim sat As New structSat_D

        sz = SatsD.Length()  ' always one more than actual number of elements

        If ndx = 0 Or ndx = sz Then     ' we are adding an element
            ReDim Preserve SatsD(sz)    ' add an element to the array
            ndx = sz
        End If

        sat.isActive = False
        sat.ndxI = 1         ' pointer to display list
        sat.name = SeeSatVBmain.satellites(ndx).tle0.satname  ' for now, change to actual name

        ReDim sat.SatsI(sizeofSatI)
        For j = 1 To sizeofSatI
            sat.SatsI(j).isActive = False
            sat.SatsI(j).ndxD = ndx
        Next (j)

        SatsD(ndx) = sat     ' add to the array

        Return (ndx)  ' position of insertion
    End Function

    ' walk throught the satellite display list and draw them on the screen
    Public Sub DrawAllSats(ByVal gr As Graphics)
        Dim ndx As Integer = 1      ' the 0 element is never used
        Dim start As Integer = 1    ' the first element to be checked

        If SatsD Is Nothing Then
            SatIsDirty = False
            Exit Sub
        End If

        If SatsD(ndx).isActive = True Then
            DrawOneSat(ndx, gr)
        End If

        If DListNext(ndx) = True Then
            start = ndx
            While DListNext(ndx) = True And start <> ndx
                DrawOneSat(ndx, gr)
            End While
        End If

    End Sub

    Public Sub DrawOneSat(ByVal ndx As Integer, ByVal gr As Graphics)

        If SatsD(ndx).isActive = False Then ' don't do any more work
            ndx += 1    'but increment the index
            Exit Sub
        End If

        Dim j, start As Integer
        Dim last As Integer = SatsD(ndx).SatsI.Length() - 1
        Dim b As Brush
        Dim f As Font

        ' Get the bounds of the screen. 
        'screenRectangle = Screen.PrimaryScreen.Bounds

        start = SatsD(ndx).ndxI

        If SatsD(ndx).SatsI(start).isActive = True Then
            'b = New SolidBrush(SatsD(ndx).SatsI(start).p.Color)
            'f = New Font(Me.Font)
            'gr.DrawEllipse(SatsD(ndx).SatsI(start).p, SatsD(ndx).SatsI(start).r)
            'If (screenRectangle.Contains(SatsD(ndx).SatsI(start).r)) Then
            'gr.FillEllipse(b, SatsD(ndx).SatsI(start).r)

            gr.DrawLine(SatsD(ndx).SatsI(start).p, SatsD(ndx).SatsI(start).r.Left, SatsD(ndx).SatsI(start).r.Bottom, _
                        SatsD(ndx).SatsI(start).r.Right, SatsD(ndx).SatsI(start).r.Top)
            gr.DrawLine(SatsD(ndx).SatsI(start).p, SatsD(ndx).SatsI(start).r.Left, SatsD(ndx).SatsI(start).r.Top, _
                        SatsD(ndx).SatsI(start).r.Right, SatsD(ndx).SatsI(start).r.Bottom)
            ' upside down and backwards :-)
            'gr.DrawString(SatsD(ndx).name, Me.Font, b, SatsD(ndx).SatsI(start).r.Right, SatsD(ndx).SatsI(start).r.Top)

            'End If
            j = CListPrev(start, last)
        Else
            Exit Sub
        End If

        While SatsD(ndx).SatsI(j).isActive = True And j <> start
            b = New SolidBrush(SatsD(ndx).SatsI(j).p.Color)
            'gr.DrawEllipse(SatsD(ndx).SatsI(j).p, SatsD(ndx).SatsI(j).r)
            'If (screenRectangle.Contains(SatsD(ndx).SatsI(j).r)) Then
            gr.FillEllipse(b, SatsD(ndx).SatsI(j).r)
            'End If
            j = CListPrev(j, last)
        End While

    End Sub

    ' get the previous element in a circular list
    Public Shared Function CListPrev(ByVal ndx As Integer, ByVal last As Integer) As Integer
        ' get the previous element in a circular list
        ndx = ndx - 1
        If ndx < 1 Then
            ndx = last
        End If
        CListPrev = ndx

    End Function

    ' draw the alt az gridlines
    Public Shared Sub DrawGrid(ByVal gr As Graphics)

        Dim i, tsize As Integer
        Dim p As Pen = New Pen(gColor, mPen)

        gr.DrawEllipse(p, New Rectangle(-WSIZE, -WSIZE, WSIZE * 2, WSIZE * 2))

        If VIEWSTEREO Then  ' sterographic projection
            tsize = CInt(WSIZE * Math.Tan((DefConst.PIO2 - 30.0 * DefConst.DE2RA) / 2))
            gr.DrawEllipse(p, New Rectangle(-tsize, -tsize, tsize * 2, tsize * 2))
            tsize = CInt(WSIZE * Math.Tan((DefConst.PIO2 - 60.0 * DefConst.DE2RA) / 2))
            gr.DrawEllipse(p, New Rectangle(-tsize, -tsize, tsize * 2, tsize * 2))
            'n = CInt(WSIZE * Math.Cos(azel.lambda) * Math.Tan((DefConst.PIO2 - azel.phi) / 2))
            'e = CInt(-WSIZE * Math.Sin(azel.lambda) * Math.Tan((DefConst.PIO2 - azel.phi) / 2))
        Else                ' simple projection
            tsize = CInt(WSIZE * Math.Cos(30.0 * DefConst.DE2RA))
            gr.DrawEllipse(p, New Rectangle(-tsize, -tsize, tsize * 2, tsize * 2))
            tsize = CInt(WSIZE * Math.Cos(60.0 * DefConst.DE2RA))
            gr.DrawEllipse(p, New Rectangle(-tsize, -tsize, tsize * 2, tsize * 2))
            'n = CInt(Fix(dist * Math.Cos(azel.lambda)))
            'e = CInt(Fix(-dist * Math.Sin(azel.lambda)))
        End If


        For i = 0 To 359 Step 30
            RadialLine(gr, i * DefConst.DE2RA)
            If Math.IEEERemainder(i, 90) <> 0 Then
                ' might have to change this for depending on VIEWSTEREO for anything other than the perimeter
                DrawChar(gr, CInt(-WSIZE * 1.04 * Math.Sin(i * DefConst.DE2RA)), CInt(WSIZE * 1.04 * Math.Cos(i * DefConst.DE2RA)), CStr(i), 0.5)
            End If
        Next

        DrawChar(gr, 0, CInt(WSIZE * 1.02), "N")
        DrawChar(gr, 0, -CInt(WSIZE * 1.02), "S")
        DrawChar(gr, CInt(WSIZE * 1.02), 0, "W")
        DrawChar(gr, -CInt(WSIZE * 1.02), 0, "E")

        gr.DrawRectangle(p, New Rectangle(-WSIZE, -WSIZE, WSIZE * 2, WSIZE * 2))

    End Sub

    ' draw text at the x y coordinates
    Public Shared Sub DrawChar(ByVal gr As Graphics, ByVal X As Integer, ByVal Y As Integer, ByVal text As String, Optional ByVal scale As Double = 1)
        Dim b As New SolidBrush(gColor)
        Dim base As Integer = CInt(WSIZE * 0.01)    ' 1 percent of our drawing area
        Dim f As New Font("Arial", CInt(base * 3 * scale))

        Dim bmap As New Bitmap(base * 4, base * 4)
        bmap.MakeTransparent()

        Dim gg As Graphics = Graphics.FromImage(bmap)
        Dim mx As New Matrix(Math.Sign(grMatrix.Elements(0)), 0, 0, Math.Sign(grMatrix.Elements(3)), 0, 0)

        mx.Translate(base * 4 * Math.Sign(grMatrix.Elements(0)), base * 4 * Math.Sign(grMatrix.Elements(3)))
        gg.Transform = mx

        'gg.Clear(formColor)

        gg.DrawString(text, f, b, -20, -20)
        
        gr.DrawImage(bmap, X + base * 2 * Math.Sign(grMatrix.Elements(0)), Y + base * 2 * Math.Sign(grMatrix.Elements(3)))

        gg.Dispose()

    End Sub

    ' draw a radial line
    Public Shared Sub RadialLine(ByVal gr As Graphics, ByVal angle As Double)

        Dim p As Pen = New Pen(gColor, mPen)
        'gr.DrawLine(p, CInt(WSIZE * Math.Sin(angle)), CInt(-WSIZE * Math.Cos(angle)), CInt(-WSIZE * Math.Sin(angle)), CInt(WSIZE * Math.Cos(angle)))
        gr.DrawLine(p, 0, 0, CInt(-WSIZE * Math.Sin(angle)), CInt(WSIZE * Math.Cos(angle)))

    End Sub


    Sub testTrans(ByVal gr As Graphics)

        ' Draw coordinate axes.
        ' make a pen for the axes. 
        ' (Width 0 means a fine line.)
        Dim red_pen As Pen = New Pen(Color.Red, sPen)
        Dim stepsz As Integer = CInt(WSIZE / 10)

        gr.DrawLine(red_pen, -WSIZE, 0, WSIZE, 0)
        For x As Integer = stepsz To stepsz * 10 Step stepsz
            gr.DrawLine(red_pen, x, -3, x, 3)
        Next x
        gr.DrawLine(red_pen, 0, -WSIZE, 0, WSIZE)
        For y As Integer = stepsz To stepsz * 100 Step stepsz
            gr.DrawLine(red_pen, -3, y, 3, y)
        Next y

    End Sub

    Private Sub TestWindow(ByVal gr As Graphics)
        Dim p1, p2, p3 As Pen
        Dim d As Integer
        Dim x As Integer

        d = Math.Min(Me.ClientRectangle.Width, Me.ClientRectangle.Height)
        x = CInt(Me.ClientRectangle.Width / 2 - d / 2)

        p1 = New Pen(Brushes.Navy, sPen)
        p2 = New Pen(Brushes.Navy, mPen)
        p3 = New Pen(Brushes.Navy, bPen)

        gr.DrawEllipse(p1, New Rectangle(-WSIZE, -WSIZE, WSIZE * 2, WSIZE * 2))

        gr.DrawLine(p3, 0, 0, 1000, 1000)
        gr.DrawLine(p2, 1000, 0, 1000, 1000)


    End Sub

    ' search through the display list and get the nearest active sat and return the ID and distance to it
    Private Sub FindNearestSat(ByVal xy As Point, ByRef sndx As Integer, ByRef distance As Integer)
        ' a value of -1 for sndx indicates nothing found

        Dim ndx As Integer = 1      ' the 0 element is never used
        Dim start As Integer = 1    ' the first element to be checked
        Dim faraway As Integer = 10000
        Dim nearest As Integer = faraway  ' start with a really far distance
        Dim thisdist As Integer

        If SatsD Is Nothing Then
            sndx = -1
            distance = faraway
            Exit Sub
        End If

        If SatsD(ndx).isActive = True Then
            nearest = CalcDist(xy, SatsD(ndx).SatsI(SatsD(ndx).ndxI).r.Location)
            sndx = ndx
        End If

        If DListNext(ndx) = True Then
            start = ndx
            While DListNext(ndx) = True And start <> ndx
                thisdist = CalcDist(xy, SatsD(ndx).SatsI(SatsD(ndx).ndxI).r.Location)
                If thisdist < nearest Then
                    nearest = thisdist
                    sndx = ndx
                End If
            End While
        End If

        If nearest = faraway Then
            sndx = -1
            distance = faraway
        Else
            distance = nearest
        End If

    End Sub

    ' returns the distance between two graphics points
    Private Function CalcDist(ByVal xyM As Point, xyS As Point) As Integer

        CalcDist = CInt(Math.Sqrt((xyM.X - xyS.X) ^ 2 + (xyM.Y - xyS.Y) ^ 2))

    End Function
    ' refreshes the screen
    Private Sub TimerR_Tick(sender As Object, e As EventArgs) Handles TimerR.Tick
        'Timer1.Interval = 1000

        If SatIsDirty = True Or StarIsDirty = True Then
            Show_ToolTip()
            Me.Refresh()
        End If

    End Sub

    Private Sub Show_ToolTip()

        Dim distance, sndx As Integer
        Dim mindist As Integer = CInt(WSIZE * 0.02) ' minimum distance to sat

        If mouse_xy <> mouse_xy_old Then        ' the tooltip doesn't work through a me.refresh??? (sometimes will if double buffering is enabled)
            FindNearestSat(mouse_xy_user, sndx, distance)
            If distance < mindist Then
                ToolTipSat.Show(SatsD(sndx).name + vbNewLine + "Mag " + CStr(SeeSatVBmain.satellites(sndx).view.truemag) + vbNewLine _
                    + "Dist " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.azel.r * DefConst.EARTHR2KM, 4)), _
                    Me, mouse_xy.X, mouse_xy.Y, 10000)
                SeeSatVBmain.TextBox1.AppendText(SeeSatVBmain.satellites(sndx).tlename + " - McName: " + SeeSatVBmain.satellites(sndx).tle0.satname + _
                    " - Itnl: " + CStr(SeeSatVBmain.satellites(sndx).tle.intl_desig) + vbNewLine + " - Norad#: " + _
                    CStr(SeeSatVBmain.satellites(sndx).tle.norad_number) + " - ElSet: " + SeeSatVBmain.satellites(sndx).elset + vbNewLine)
                SeeSatVBmain.TextBox1.AppendText(" Time (UTC): " + CStr(AstroGR.Julian2Gregorian(SeeSatVBmain.JDPUB)) + _
                    " (Local):" + CStr(AstroGR.Julian2Gregorian(SeeSatVBmain.JDPUB + SeeSatVBmain.my_loc.tz_offset / DefConst.HRPERDAY)) + vbNewLine)
                SeeSatVBmain.TextBox1.AppendText(" Alt: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.azel.phi * DefConst.RA2DE, 4)) + _
                    " Azm: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.azel.lambda * DefConst.RA2DE, 4)) + _
                    " Dist: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.azel.r * DefConst.EARTHR2KM, 4)) + " km" + vbNewLine)
                SeeSatVBmain.TextBox1.AppendText(" RA: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.radec.lambda * DefConst.RA2DE, 4)) + _
                    " Dec: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.radec.phi * DefConst.RA2DE, 4)) + _
                    " Dist: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.radec.r * DefConst.EARTHR2KM, 4)) + " km" + vbNewLine)
                SeeSatVBmain.TextBox1.AppendText(" Sun angle: " + CStr(SeeSatVBmain.satellites(sndx).view.elsusa) + " Illum: " + _
                    CStr(CInt(SeeSatVBmain.satellites(sndx).view.illum * 100)) + "% Mag: " + CStr(SeeSatVBmain.satellites(sndx).view.truemag) + vbNewLine + vbNewLine)
                DEBUG = True
            End If
            'ToolTipSat.Hide(Me)
            'ToolTipSat.Show("X,Y " + CStr(mouse_xy_user.X) + "," + CStr(mouse_xy_user.Y), Me, mouse_xy.X, mouse_xy.Y, 10000)
            'SeeSatVBmain.TextBox1.AppendText("X,Y " + CStr(mouse_xy_user.X) + "," + CStr(mouse_xy_user.Y) + vbNewLine)
            mouse_xy_old = mouse_xy
        Else
            ToolTipSat.Hide(Me)
        End If

    End Sub

    ' shows the tooltip if the mouse has been stationary
    Private Sub TimerM_Tick(sender As Object, e As EventArgs) Handles TimerM.Tick

        Show_ToolTip()

    End Sub

    Private Sub SatWindow_ResizeEnd(sender As Object, e As EventArgs) Handles MyBase.ResizeEnd

        If FirstTime = False Then
            SatIsDirty = True
            StarIsDirty = True
        End If

        'TextBoxS.Width = CInt(CanvasTotal.Width - (CanvasBounds.Left + CanvasBounds.Right))

    End Sub

    Private Sub SatWindow_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If FirstTime = False Then
            SatIsDirty = True
            StarIsDirty = True
        End If

        TextBoxS.Width = CInt(CanvasTotal.Width - (CanvasBounds.Left + CanvasBounds.Right))

    End Sub

    Private Sub SatWindow_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown

        Dim distance, sndx As Integer
        Dim mindist As Integer = CInt(WSIZE * 0.02) ' minimum distance to sat

        If (e.Button = Windows.Forms.MouseButtons.Left) Then


            FindNearestSat(mouse_xy_user, sndx, distance)
            If distance < mindist Then
                TextBoxS.AppendText(vbNewLine + SeeSatVBmain.satellites(sndx).tlename + " / " + SeeSatVBmain.satellites(sndx).tle0.satname + _
                    " - ElSet: " + SeeSatVBmain.satellites(sndx).elset + vbNewLine)
                TextBoxS.AppendText(" Time (UTC): " + AstroGR.Julian2Gregorian(SeeSatVBmain.JDPUB).ToString("MMM/dd/yyyy HH:mm:ss") + _
                    " (Local):" + AstroGR.Julian2Gregorian(SeeSatVBmain.JDPUB + SeeSatVBmain.my_loc.tz_offset / DefConst.HRPERDAY).ToString("MMM/dd/yyyy HH:mm:ss") + vbNewLine)
                TextBoxS.AppendText(" Alt: " + Parser.DecDegToDMSString(SeeSatVBmain.satellites(sndx).view.azel.phi * DefConst.RA2DE) + _
                    " Azm: " + Parser.DecDegToDMSString(SeeSatVBmain.satellites(sndx).view.azel.lambda * DefConst.RA2DE) + _
                    " Dist: " + CStr(Math.Round(SeeSatVBmain.satellites(sndx).view.azel.r * DefConst.EARTHR2KM, 4)) + " km" + vbNewLine)
                TextBoxS.AppendText(" RA: " + Parser.DecDegToHrString(SeeSatVBmain.satellites(sndx).view.radec.lambda * DefConst.RA2DE) + _
                    " Dec: " + Parser.DecDegToDMSString(SeeSatVBmain.satellites(sndx).view.radec.phi * DefConst.RA2DE))
                TextBoxS.AppendText(" Sun angle: " + CStr(SeeSatVBmain.satellites(sndx).view.elsusa) + " Illum: " + _
                    CStr(CInt(SeeSatVBmain.satellites(sndx).view.illum * 100)) + "% Mag: " + CStr(SeeSatVBmain.satellites(sndx).view.truemag) + vbNewLine)

            End If
        End If
    End Sub

    'Private Sub SatWindow_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
    '    If (e.Button = Windows.Forms.MouseButtons.Right) Then
    '        WSCALE = WSCALE / 2
    '    End If
    '    If (e.Button = Windows.Forms.MouseButtons.Left) And WSCALE < WMAXSCALE Then
    '        WSCALE = WSCALE * 2
    '    End If

    '    WXOFFSET = CInt(((e.Location.X - grMatrix.OffsetX) / grMatrix.Elements(0)))
    '    WYOFFSET = CInt(((e.Location.Y - grMatrix.OffsetY) / grMatrix.Elements(3)))

    '    If WSCALE <= 1 Then
    '        WSCALE = 1
    '        WXOFFSET = 0
    '        WYOFFSET = 0
    '    End If

    '    'SeeSatVBmain.TextBox1.AppendText("Click Xm,Xw " + CStr(e.Location.X) + "," + CStr((e.Location.X - grMatrix.OffsetX) / grMatrix.Elements(0)) _
    '    '  + " Ym,Yw " + CStr(e.Location.Y) + "," + CStr((e.Location.Y - grMatrix.OffsetY) / grMatrix.Elements(3)) + vbNewLine)

    '    ' test the pointtoclient method
    '    'mouse_xy = Me.PointToClient(New Point(e.Location.X, e.Location.Y))
    '    'Me.Cursor = New Cursor(Cursor.Current.Handle)
    '    'SeeSatVBmain.TextBox1.AppendText("PointToClient X,Y " + CStr(mouse_xy.X) + "," + CStr(mouse_xy.Y) + vbNewLine)

    '    SatIsDirty = True
    '    StarIsDirty = True
    'End Sub

    Private Sub SatWindow_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover
        'ToolTipSat.Active = True
        'ToolTipSat.Show("Test", Me)
    End Sub

    Private Sub SatWindow_MouseWheel(sender As Object, e As MouseEventArgs) Handles MyBase.MouseWheel

        If e.Y < CanvasBounds.Top Or e.Y > CanvasBounds.Bottom Or e.X < CanvasBounds.Left Or e.X > CanvasBounds.Right Then
            Exit Sub
        End If

        If e.Delta < 0 Then
            WSCALE -= 1
        End If
        If e.Delta > 0 And WSCALE < WMAXSCALE Then
            WSCALE += 1
        End If

        WXOFFSET = CInt(((e.Location.X - grMatrix.OffsetX) / grMatrix.Elements(0)))
        WYOFFSET = CInt(((e.Location.Y - grMatrix.OffsetY) / grMatrix.Elements(3)))

        If WXOFFSET < -WSIZE Then
            WXOFFSET = -WSIZE
        End If

        If WYOFFSET < -WSIZE Then
            WYOFFSET = -WSIZE
        End If


        If WSCALE <= 1 Then
            WSCALE = 1
            WXOFFSET = 0
            WYOFFSET = 0
        End If

        SatIsDirty = True
        StarIsDirty = True
    End Sub

    Private Sub SatWindow_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        'ToolTipSat.Active = False
        'ToolTipSat.Hide(Me)
        'If (Math.Abs(e.X - mouse_xy.X) > 10 Or Math.Abs(e.Y - mouse_xy.Y) > 10) And Not grMatrix Is Nothing Then ' the mouse has moved more than jitter

        'If (Math.Abs(e.X - mouse_xy.X) > MJITTER Or Math.Abs(e.Y - mouse_xy.Y) > MJITTER) Then ' the mouse has moved more than jitter
        If Math.Sqrt((e.X - mouse_xy.X) ^ 2 + (e.Y - mouse_xy.Y) ^ 2) > MJITTER Then ' the mouse has moved more than jitter
            mouse_xy_old = mouse_xy
            mouse_xy.X = e.X
            mouse_xy.Y = e.Y

            If Not IsNothing(grMatrix) Then
                mouse_xy_user.X = CInt((e.Location.X - grMatrix.OffsetX) / grMatrix.Elements(0))
                mouse_xy_user.Y = CInt((e.Location.Y - grMatrix.OffsetY) / grMatrix.Elements(3))
            End If

        End If

    End Sub


End Class