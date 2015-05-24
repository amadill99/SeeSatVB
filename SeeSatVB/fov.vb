Imports System.Drawing.Drawing2D

Public Class FOV
    ' methods and properties for a field of view
    Private Shared polygon(12) As PointF   'the array of points defining the ellipse or arc beziers
    Private Shared rotation As Double    'the orientation +/- 90 degrees
    Private Shared altazm As New fov_t       'alt, azm, angle in radians
    Private Shared radec As New fov_t        'ra, dec, angle in radians
    Private Shared dimensions As New xyz_t   'x axis, y axis, angle in radians (y and z ignored if iscircle)
    Private Shared ctl(6) As xyz_t          ' the control points for the FOV
    Private Shared iscircle As Boolean   'draw an ellipse instead of a rectangle
    Public Shared show As Boolean       'show the fov
    Public Shared isdirty As Boolean    'needs to be refreshed
    Public Shared track As Boolean      'track with star field ie use ra/dec
    Public Shared rotate As Boolean     'rotate the fov with azimuth - true for an equitorial mount
    Private Shared pen As Pen            'the pen we draw with
    Private Shared cosrot, sinrot, sinalt, cosalt, sinazm, cosazm As Double
    Private Shared magic As Double = 0.5522847498   ' 3 / 4 * (Math.Sqrt(2) - 1) - magic number for bezier arcs

    Public Shared Function initialize() As Boolean
        'set up the parameters
        pen = New Pen(SatWindow.gColor, SatWindow.sPen)

        rotation = 0 * DefConst.DE2RA
        iscircle = True
        show = True
        track = True
        rotate = False

        ' for testing
        '80 mm lens
        dimensions.x = 25 * DefConst.DE2RA
        dimensions.y = 15 * DefConst.DE2RA
        dimensions.z = rotation
        'arbitrary spot
        altazm.alde = 20 * DefConst.DE2RA
        altazm.azra = 45 * DefConst.DE2RA
        altazm.rot = 0
        'rigel
        radec.azra = 78.634468 * DefConst.DE2RA
        radec.alde = -8.201641 * DefConst.DE2RA
        radec.rot = 0

        Return True
    End Function

    Private Shared Sub calc()
        ' calculate the endpoints of the fov using the values in the alt/azm structure
        ' uses spherical trig to calculate the coordinates
        Dim work As New xyz_t
        Dim Z, deltaHgt, deltaWdth, deltaPhi, deltaLambda As Double

        ' initialize the control points
        work.x = 0
        work.y = 0
        work.z = 0
        For ndx = 0 To ctl.Length - 1
            ctl(ndx) = work.Copy
        Next

        sinalt = Math.Sin(DefConst.PIO2 - altazm.alde)
        cosalt = Math.Cos(DefConst.PIO2 - altazm.alde)
        sinazm = Math.Sin(altazm.azra)
        cosazm = Math.Cos(altazm.azra)
        Z = SatWindow.WSIZE

        If iscircle Then
            'http://www.angusj.com/delphitips/ellipses.php
            'http://www.codeguru.com/cpp/g-m/gdi/article.php/c131/Drawing-Rotated-and-Skewed-Ellipses.htm

            'no rotation in a circle
            sinrot = Math.Sin(0)
            cosrot = Math.Cos(0)
            ' pick width and height in the y axis
            deltaWdth = dimensions.y / 2
            'deltaHgt = dimensions.y / 2

            'top
            Proj2Sphere(work, deltaWdth, DefConst.PIO2, Z)
            transform(work, 0)
            transform(work, 12) 'close back to the origin
            'bottom
            work.y *= -1
            transform(work, 6)
            'right
            Proj2Sphere(work, deltaWdth, 0, Z)
            transform(work, 3)
            'left
            work.x *= -1
            transform(work, 9)

            'top mid right
            deltaPhi = Math.Acos(Math.Cos(deltaWdth * magic) * Math.Cos(deltaWdth))
            deltaLambda = Math.Atan2(deltaWdth, deltaWdth * magic)
            Proj2Sphere(work, deltaPhi, deltaLambda, Z)
            transform(work, 1)
            'bmr
            work.y *= -1
            transform(work, 5)
            'bml
            work.x *= -1
            transform(work, 7)
            'tml
            work.y *= -1
            transform(work, 11)

            'right mid top
            'we could just reverse the x and y components here for a circle
            deltaPhi = Math.Acos(Math.Cos(deltaWdth) * Math.Cos(deltaWdth * magic))
            deltaLambda = Math.Atan2(deltaWdth * magic, deltaWdth)
            Proj2Sphere(work, deltaPhi, deltaLambda, Z)
            transform(work, 2)
            'rmb
            work.y *= -1
            transform(work, 4)
            'lmb
            work.x *= -1
            transform(work, 8)
            'lmt
            work.y *= -1
            transform(work, 10)
        Else
            If rotate Then
                altazm.rot = dimensions.z - altazm.azra
            Else
                altazm.rot = dimensions.z
            End If

            sinrot = Math.Sin(altazm.rot)
            cosrot = Math.Cos(altazm.rot)
            ' pick width in the y axis
            deltaWdth = dimensions.y / 2
            deltaHgt = dimensions.x / 2

            'top right
            deltaPhi = Math.Acos(Math.Cos(deltaWdth) * Math.Cos(deltaHgt))
            deltaLambda = Math.Atan2(deltaHgt, deltaWdth)
            Proj2Sphere(work, deltaPhi, deltaLambda, Z)
            transform(work, 0)
            transform(work, 12) 'close back to the origin
            'bottom right
            work.y *= -1
            transform(work, 3)
            'bottom left
            work.x *= -1
            transform(work, 6)
            'top left
            work.y *= -1
            transform(work, 9)

            'mid top right
            deltaPhi = Math.Acos(Math.Cos(deltaWdth * magic) * Math.Cos(deltaHgt))
            deltaLambda = Math.Atan2(deltaHgt, deltaWdth * magic)
            Proj2Sphere(work, deltaPhi, deltaLambda, Z)
            transform(work, 11)
            'mbr
            work.y *= -1
            transform(work, 4)
            'mbl
            work.x *= -1
            transform(work, 5)
            'mtl
            work.y *= -1
            transform(work, 10)

            'mid right top
            deltaPhi = Math.Acos(Math.Cos(deltaWdth) * Math.Cos(deltaHgt * magic))
            deltaLambda = Math.Atan2(deltaHgt * magic, deltaWdth)
            Proj2Sphere(work, deltaPhi, deltaLambda, Z)
            transform(work, 1)
            'mrb
            work.y *= -1
            transform(work, 2)
            'mlb
            work.x *= -1
            transform(work, 7)
            'mlt
            work.y *= -1
            transform(work, 8)
        End If

    End Sub

    ' not used
    Private Shared Sub arcCtlPts(ByVal p1 As Integer, ByVal pc As Integer, ByVal p4 As Integer, ByVal p2 As Integer, ByVal p3 As Integer)
        ' from http://itc.ktu.lt/itc354/Riskus354.pdf
        ' parameters to routine are references into the control pts array
        ' calculates the bezier control points that aproximate an arc from p1 through pc to p4
        ' NOT through pc but centered on pc :-p

        Dim H, C, theta, R As Double

        ' calculate the height and chord of the arc

        If ctl(pc).y = 0 Then
            H = ctl(pc).x - ctl(p1).x
            C = ctl(p1).y * 2
        Else
            H = ctl(pc).y - ctl(p1).y
            C = ctl(p1).x * 2
        End If

        If H <> 0 Then
            ' theta is the angle between the tangent and the chord
            R = (C ^ 2 + 4 * H ^ 2) / 8 * H
            theta = 2 * Math.Asin(C / (2 * R))
            ' now R becomes the distance between the origin and the centerpoint
            R = Math.Sqrt((C / 2) ^ 2 + H ^ 2)

            If ctl(pc).y = 0 Then
                ctl(p2).x = ctl(p1).x + magic * R * Math.Sin(theta)
                ctl(p2).y = ctl(p1).y + magic * R * Math.Cos(theta)
                ctl(p3).x = ctl(p4).x - magic * R * Math.Sin(theta)
                ctl(p3).y = ctl(p4).y - magic * R * Math.Cos(theta)
            Else
                ctl(p2).x = ctl(p1).x - magic * R * Math.Cos(theta)
                ctl(p2).y = ctl(p1).y - magic * R * Math.Sin(theta)
                ctl(p3).x = ctl(p4).x + magic * R * Math.Cos(theta)
                ctl(p3).y = ctl(p4).y + magic * R * Math.Sin(theta)
            End If
            ' is it -
        End If

    End Sub

    Private Shared Sub Proj2Sphere(ByRef work As xyz_t, ByVal phi As Double, ByVal lambda As Double, ByVal Z As Double)
        ' 0 <= phi <= 2pi, 0 <= lambda <= pi
        work.x = Z * Math.Cos(lambda) * Math.Sin(phi)
        work.y = Z * Math.Sin(lambda) * Math.Sin(phi)
        work.z = Z * Math.Cos(phi)
    End Sub

    Private Shared Sub transform(ByVal pt As xyz_t, id As Integer)

        Dim work As New xyz_t
        work = pt.Copy

        'rotate the field of view
        work.zrot(sinrot, cosrot)
        'then swing to the altitude
        work.yrot(sinalt, cosalt)
        'then swing to the azimuth
        'work.zrot(sinazm, cosazm)
        'reverse the arguments because the y axis is north
        work.zrot(cosazm, sinazm)

        If SatWindow.VIEWSTEREO Then
            ' do the stereo projection http://en.wikipedia.org/wiki/Stereographic_projection
            polygon(id).X = CSng(SatWindow.WSIZE * (work.x / (SatWindow.WSIZE + work.z)))
            polygon(id).Y = CSng(SatWindow.WSIZE * (work.y / (SatWindow.WSIZE + work.z)))
        Else
            ' else it is just flat
            polygon(id).X = CSng(work.x)
            polygon(id).Y = CSng(work.y)
        End If

    End Sub

    Private Shared Function altazm2screenxy(ByVal alt As Double, ByVal azm As Double) As PointF
        ' return a drawing point from an alt azm
        Dim coord As New PointF
        Dim dist As Double

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

        gr.DrawBeziers(pen, polygon)
        'gr.DrawLines(pen, polygon)

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
