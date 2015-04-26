Imports System.Drawing.Drawing2D

Public Class FOV
    ' methods and properties for a field of view
    Private Shared rect As Rectangle     'the display object in user coords
    Private Shared polygon(3) As PointF   'the array of points if not iscircle
    Private Shared rotation As Double    'the orientation +/- 90 degrees
    Private Shared altazm As New fov_t       'alt, azm, angle in radians
    Private Shared radec As New fov_t        'ra, dec, angle in radians
    Private Shared dimensions As New xyz_t   'x axis, y axis, angle in radians (y and z ignored if iscircle)
    Private Shared iscircle As Boolean   'draw an ellipse instead of a rectangle
    Public Shared show As Boolean       'show the fov
    Private Shared track As Boolean      'track with star field ie use ra/dec
    Private Shared rotate As Boolean     'rotate the fov with azimuth - false for an equitorial mount
    Private Shared pen As Pen            'the pen we draw with

    Public Shared Function initialize() As Boolean
        'set up the parameters
        pen = New Pen(SatWindow.gColor, SatWindow.sPen)

        rotation = 0
        iscircle = False
        show = True
        track = True
        rotate = True

        ' for testing
        '80 mm lens
        dimensions.x = 15 * DefConst.DE2RA
        dimensions.y = 30 * DefConst.DE2RA
        dimensions.z = rotation
        'arbitrary spot
        altazm.alde = 15 * DefConst.DE2RA
        altazm.azra = 90 * DefConst.DE2RA
        altazm.rot = 0
        'rigel
        radec.azra = 78.634468 * DefConst.DE2RA
        radec.alde = -8.201641 * DefConst.DE2RA
        radec.rot = 0

        Return True
    End Function

    Private Shared Sub calc()
        ' calculate the endpoints of the fov using the values in the alt/azm structure
        Dim work As New xyz_t
        Dim origin As New xyz_t
        Dim porigin As PointF
        Dim width, height, angle, cosangle, sinangle As Double

        porigin = altazm2screenxy(altazm.alde, altazm.azra)
        origin.x = porigin.X
        origin.y = porigin.Y
        origin.z = 0
        work.z = 0

        ' scale to our window size
        'dist = CInt(Fix(WSIZE * Math.Cos(azel.phi)))
        width = (Math.Atan(dimensions.x) * SatWindow.WSIZE)
        height = (Math.Atan(dimensions.y) * SatWindow.WSIZE)         ' * Math.Sin(altazm.alde))

        If iscircle Then
            'http://www.angusj.com/delphitips/ellipses.php
            'http://www.codeguru.com/cpp/g-m/gdi/article.php/c131/Drawing-Rotated-and-Skewed-Ellipses.htm
            'rect = New Rectangle
            'rect.Location = altazm2screenxy(altazm.x + dimensions.x / 2, altazm.y + dimensions.x / 2)
            'center.alde = altazm2screenxy(altazm.alde + dimensions.x / 2, altazm.azra)
            rect.Width = CInt(width)
            rect.Height = CInt(width)   'width again because it is a circle
        Else

            angle = dimensions.z - altazm.azra ' + Math.Atan(height / width)
            sinangle = Math.Sin(angle)
            cosangle = Math.Cos(angle)

            ' top right
            work.x = width / 2
            work.y = height / 2
            work.zrot(sinangle, cosangle)
            work.add(origin)
            polygon(0).X = CSng(work.x)
            polygon(0).Y = CSng(work.y)

            ' bottom right
            'angle = dimensions.z + altazm.azra + Math.Atan(height / width)
            work.x = width / 2
            work.y = -height / 2
            work.zrot(sinangle, cosangle)
            work.add(origin)
            polygon(1).X = CSng(work.x)
            polygon(1).Y = CSng(work.y)

            'bottom left
            'angle = dimensions.z + altazm.azra + Math.Atan(height / width)
            work.x = -width / 2
            work.y = -height / 2
            work.zrot(sinangle, cosangle)
            work.add(origin)
            polygon(2).X = CSng(work.x)
            polygon(2).Y = CSng(work.y)

            'top left
            'angle = dimensions.z + altazm.azra + Math.Atan(height / width)
            work.x = -width / 2
            work.y = height / 2
            work.zrot(sinangle, cosangle)
            work.add(origin)
            polygon(3).X = CSng(work.x)
            polygon(3).Y = CSng(work.y)

        End If

    End Sub

    Private Shared Function altazm2screenxy(ByVal alt As Double, ByVal azm As Double) As PointF
        ' return a drawing point from an alt azm
        Dim coord As New PointF
        Dim dist As Double

        dist = CInt(Fix(SatWindow.WSIZE * Math.Cos(alt)))

        If SatWindow.VIEWSTEREO Then  ' sterographic projection
            coord.Y = CSng(SatWindow.WYFLIP * SatWindow.WSIZE * Math.Cos(azm) * Math.Tan((DefConst.PIO2 - alt) / 2))
            coord.X = CSng(SatWindow.WXFLIP * SatWindow.WSIZE * Math.Sin(azm) * Math.Tan((DefConst.PIO2 - alt) / 2))
        Else                ' This is a simple projection that compresses the area below 30 deg
            coord.Y = CSng(Fix(SatWindow.WYFLIP * dist * Math.Cos(azm)))
            coord.X = CSng(Fix(SatWindow.WXFLIP * dist * Math.Sin(azm)))
            'coord.Y = CInt(Fix(dist * Math.Sin(azm)))
        End If



        Return coord

    End Function

    Public Shared Sub draw(ByVal gr As Graphics)
        ' draw the fov on the screen
        calc()
        If iscircle Then
            gr.DrawEllipse(pen, rect)
            gr.DrawLine(pen, rect.Location, New Point(rect.Location.X + 500, rect.Location.Y + 500))
            gr.DrawLine(pen, rect.Location, New Point(rect.Location.X + 250, rect.Location.Y - 250))
        Else
            gr.DrawPolygon(pen, polygon)
        End If
    End Sub

    Private Shared Sub altazm2radec()
        AstroGR.AltAzmtoRaDec(altazm.alde, altazm.azra, radec.azra, radec.alde)
    End Sub

    Private Shared Sub radec2altazm()
        AstroGR.RaDecToAltAzm(radec.azra, radec.alde, altazm.alde, altazm.azra)
    End Sub

End Class


' field of view
Public Class fov_t
    Public alde As Double   'angle (altitude/declination)
    Public azra As Double   'angle (azmimuth/ra)
    Public rot As Double    'angle (rotation)
    Public Function Clone() As fov_t
        Return DirectCast(Me.MemberwiseClone(), fov_t)
    End Function
End Class
