<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabControlSettings = New System.Windows.Forms.TabControl()
        Me.LocationTab = New System.Windows.Forms.TabPage()
        Me.TBLongDisp = New System.Windows.Forms.TextBox()
        Me.TBLatDisp = New System.Windows.Forms.TextBox()
        Me.BLocExit = New System.Windows.Forms.Button()
        Me.BLocCancel = New System.Windows.Forms.Button()
        Me.TBTZoffset = New System.Windows.Forms.TextBox()
        Me.LTZoffset = New System.Windows.Forms.Label()
        Me.LDescr = New System.Windows.Forms.Label()
        Me.TBLName = New System.Windows.Forms.TextBox()
        Me.LLong = New System.Windows.Forms.Label()
        Me.TBLong = New System.Windows.Forms.TextBox()
        Me.LElev = New System.Windows.Forms.Label()
        Me.TBElev = New System.Windows.Forms.TextBox()
        Me.LLat = New System.Windows.Forms.Label()
        Me.TBLat = New System.Windows.Forms.TextBox()
        Me.FOVTab = New System.Windows.Forms.TabPage()
        Me.LFOVrotation = New System.Windows.Forms.Label()
        Me.TBFOVRotation = New System.Windows.Forms.TextBox()
        Me.TBFOVDecDisp = New System.Windows.Forms.TextBox()
        Me.TBFOVRADisp = New System.Windows.Forms.TextBox()
        Me.LFOVDec = New System.Windows.Forms.Label()
        Me.TBFOVDec = New System.Windows.Forms.TextBox()
        Me.LFOVRA = New System.Windows.Forms.Label()
        Me.TBFOVRA = New System.Windows.Forms.TextBox()
        Me.TBFOVAzmDisp = New System.Windows.Forms.TextBox()
        Me.TBFOVAltDisp = New System.Windows.Forms.TextBox()
        Me.LFOVAzm = New System.Windows.Forms.Label()
        Me.TBFOVAzm = New System.Windows.Forms.TextBox()
        Me.LFOVAlt = New System.Windows.Forms.Label()
        Me.TBFOVAlt = New System.Windows.Forms.TextBox()
        Me.LFOVheight = New System.Windows.Forms.Label()
        Me.LFOVwidth = New System.Windows.Forms.Label()
        Me.GBFOVradec = New System.Windows.Forms.GroupBox()
        Me.RBFOVradec = New System.Windows.Forms.RadioButton()
        Me.RBFOValtazm = New System.Windows.Forms.RadioButton()
        Me.CBFOVRotate = New System.Windows.Forms.CheckBox()
        Me.CBFOVTrack = New System.Windows.Forms.CheckBox()
        Me.TBFOVHeight = New System.Windows.Forms.TextBox()
        Me.TBFOVWidth = New System.Windows.Forms.TextBox()
        Me.CBFOViscircle = New System.Windows.Forms.CheckBox()
        Me.CBFOVShow = New System.Windows.Forms.CheckBox()
        Me.BFOVExit = New System.Windows.Forms.Button()
        Me.BFOVCancel = New System.Windows.Forms.Button()
        Me.OptionTab = New System.Windows.Forms.TabPage()
        Me.TabControlSettings.SuspendLayout()
        Me.LocationTab.SuspendLayout()
        Me.FOVTab.SuspendLayout()
        Me.GBFOVradec.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlSettings
        '
        Me.TabControlSettings.Controls.Add(Me.LocationTab)
        Me.TabControlSettings.Controls.Add(Me.FOVTab)
        Me.TabControlSettings.Controls.Add(Me.OptionTab)
        Me.TabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlSettings.Location = New System.Drawing.Point(0, 0)
        Me.TabControlSettings.Name = "TabControlSettings"
        Me.TabControlSettings.SelectedIndex = 0
        Me.TabControlSettings.Size = New System.Drawing.Size(402, 354)
        Me.TabControlSettings.TabIndex = 14
        '
        'LocationTab
        '
        Me.LocationTab.BackColor = System.Drawing.SystemColors.Control
        Me.LocationTab.Controls.Add(Me.TBLongDisp)
        Me.LocationTab.Controls.Add(Me.TBLatDisp)
        Me.LocationTab.Controls.Add(Me.BLocExit)
        Me.LocationTab.Controls.Add(Me.BLocCancel)
        Me.LocationTab.Controls.Add(Me.TBTZoffset)
        Me.LocationTab.Controls.Add(Me.LTZoffset)
        Me.LocationTab.Controls.Add(Me.LDescr)
        Me.LocationTab.Controls.Add(Me.TBLName)
        Me.LocationTab.Controls.Add(Me.LLong)
        Me.LocationTab.Controls.Add(Me.TBLong)
        Me.LocationTab.Controls.Add(Me.LElev)
        Me.LocationTab.Controls.Add(Me.TBElev)
        Me.LocationTab.Controls.Add(Me.LLat)
        Me.LocationTab.Controls.Add(Me.TBLat)
        Me.LocationTab.Location = New System.Drawing.Point(4, 22)
        Me.LocationTab.Name = "LocationTab"
        Me.LocationTab.Padding = New System.Windows.Forms.Padding(3)
        Me.LocationTab.Size = New System.Drawing.Size(394, 328)
        Me.LocationTab.TabIndex = 0
        Me.LocationTab.Text = "Location"
        '
        'TBLongDisp
        '
        Me.TBLongDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBLongDisp.Enabled = False
        Me.TBLongDisp.Location = New System.Drawing.Point(202, 134)
        Me.TBLongDisp.Name = "TBLongDisp"
        Me.TBLongDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBLongDisp.TabIndex = 27
        Me.TBLongDisp.TabStop = False
        '
        'TBLatDisp
        '
        Me.TBLatDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBLatDisp.Enabled = False
        Me.TBLatDisp.Location = New System.Drawing.Point(202, 89)
        Me.TBLatDisp.Name = "TBLatDisp"
        Me.TBLatDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBLatDisp.TabIndex = 26
        Me.TBLatDisp.TabStop = False
        '
        'BLocExit
        '
        Me.BLocExit.Location = New System.Drawing.Point(227, 257)
        Me.BLocExit.Name = "BLocExit"
        Me.BLocExit.Size = New System.Drawing.Size(75, 23)
        Me.BLocExit.TabIndex = 23
        Me.BLocExit.Text = "Save/Exit"
        Me.BLocExit.UseVisualStyleBackColor = True
        '
        'BLocCancel
        '
        Me.BLocCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BLocCancel.Location = New System.Drawing.Point(81, 257)
        Me.BLocCancel.Name = "BLocCancel"
        Me.BLocCancel.Size = New System.Drawing.Size(75, 23)
        Me.BLocCancel.TabIndex = 22
        Me.BLocCancel.Text = "Cancel"
        Me.BLocCancel.UseVisualStyleBackColor = True
        '
        'TBTZoffset
        '
        Me.TBTZoffset.Location = New System.Drawing.Point(242, 209)
        Me.TBTZoffset.Name = "TBTZoffset"
        Me.TBTZoffset.Size = New System.Drawing.Size(60, 20)
        Me.TBTZoffset.TabIndex = 20
        '
        'LTZoffset
        '
        Me.LTZoffset.AutoSize = True
        Me.LTZoffset.Location = New System.Drawing.Point(78, 212)
        Me.LTZoffset.Name = "LTZoffset"
        Me.LTZoffset.Size = New System.Drawing.Size(119, 13)
        Me.LTZoffset.TabIndex = 25
        Me.LTZoffset.Text = "Time zone offset (-west)"
        '
        'LDescr
        '
        Me.LDescr.AutoSize = True
        Me.LDescr.Location = New System.Drawing.Point(78, 185)
        Me.LDescr.Name = "LDescr"
        Me.LDescr.Size = New System.Drawing.Size(60, 13)
        Me.LDescr.TabIndex = 24
        Me.LDescr.Text = "Description"
        '
        'TBLName
        '
        Me.TBLName.Location = New System.Drawing.Point(144, 182)
        Me.TBLName.Name = "TBLName"
        Me.TBLName.Size = New System.Drawing.Size(158, 20)
        Me.TBLName.TabIndex = 19
        '
        'LLong
        '
        Me.LLong.AutoSize = True
        Me.LLong.Location = New System.Drawing.Point(78, 111)
        Me.LLong.Name = "LLong"
        Me.LLong.Size = New System.Drawing.Size(88, 13)
        Me.LLong.TabIndex = 21
        Me.LLong.Text = "Longitude (-west)"
        '
        'TBLong
        '
        Me.TBLong.Location = New System.Drawing.Point(202, 108)
        Me.TBLong.Name = "TBLong"
        Me.TBLong.Size = New System.Drawing.Size(100, 20)
        Me.TBLong.TabIndex = 16
        '
        'LElev
        '
        Me.LElev.AutoSize = True
        Me.LElev.Location = New System.Drawing.Point(78, 159)
        Me.LElev.Name = "LElev"
        Me.LElev.Size = New System.Drawing.Size(91, 13)
        Me.LElev.TabIndex = 17
        Me.LElev.Text = "Elevation (meters)"
        '
        'TBElev
        '
        Me.TBElev.Location = New System.Drawing.Point(202, 156)
        Me.TBElev.Name = "TBElev"
        Me.TBElev.Size = New System.Drawing.Size(100, 20)
        Me.TBElev.TabIndex = 18
        '
        'LLat
        '
        Me.LLat.AutoSize = True
        Me.LLat.Location = New System.Drawing.Point(78, 66)
        Me.LLat.Name = "LLat"
        Me.LLat.Size = New System.Drawing.Size(83, 13)
        Me.LLat.TabIndex = 15
        Me.LLat.Text = "Latitude (-south)"
        '
        'TBLat
        '
        Me.TBLat.Location = New System.Drawing.Point(202, 63)
        Me.TBLat.Name = "TBLat"
        Me.TBLat.Size = New System.Drawing.Size(100, 20)
        Me.TBLat.TabIndex = 14
        '
        'FOVTab
        '
        Me.FOVTab.BackColor = System.Drawing.SystemColors.Control
        Me.FOVTab.Controls.Add(Me.LFOVrotation)
        Me.FOVTab.Controls.Add(Me.TBFOVRotation)
        Me.FOVTab.Controls.Add(Me.TBFOVDecDisp)
        Me.FOVTab.Controls.Add(Me.TBFOVRADisp)
        Me.FOVTab.Controls.Add(Me.LFOVDec)
        Me.FOVTab.Controls.Add(Me.TBFOVDec)
        Me.FOVTab.Controls.Add(Me.LFOVRA)
        Me.FOVTab.Controls.Add(Me.TBFOVRA)
        Me.FOVTab.Controls.Add(Me.TBFOVAzmDisp)
        Me.FOVTab.Controls.Add(Me.TBFOVAltDisp)
        Me.FOVTab.Controls.Add(Me.LFOVAzm)
        Me.FOVTab.Controls.Add(Me.TBFOVAzm)
        Me.FOVTab.Controls.Add(Me.LFOVAlt)
        Me.FOVTab.Controls.Add(Me.TBFOVAlt)
        Me.FOVTab.Controls.Add(Me.LFOVheight)
        Me.FOVTab.Controls.Add(Me.LFOVwidth)
        Me.FOVTab.Controls.Add(Me.GBFOVradec)
        Me.FOVTab.Controls.Add(Me.CBFOVRotate)
        Me.FOVTab.Controls.Add(Me.CBFOVTrack)
        Me.FOVTab.Controls.Add(Me.TBFOVHeight)
        Me.FOVTab.Controls.Add(Me.TBFOVWidth)
        Me.FOVTab.Controls.Add(Me.CBFOViscircle)
        Me.FOVTab.Controls.Add(Me.CBFOVShow)
        Me.FOVTab.Controls.Add(Me.BFOVExit)
        Me.FOVTab.Controls.Add(Me.BFOVCancel)
        Me.FOVTab.Location = New System.Drawing.Point(4, 22)
        Me.FOVTab.Name = "FOVTab"
        Me.FOVTab.Padding = New System.Windows.Forms.Padding(3)
        Me.FOVTab.Size = New System.Drawing.Size(394, 328)
        Me.FOVTab.TabIndex = 1
        Me.FOVTab.Text = "Field of View"
        '
        'LFOVrotation
        '
        Me.LFOVrotation.AutoSize = True
        Me.LFOVrotation.Location = New System.Drawing.Point(187, 134)
        Me.LFOVrotation.Name = "LFOVrotation"
        Me.LFOVrotation.Size = New System.Drawing.Size(74, 13)
        Me.LFOVrotation.TabIndex = 48
        Me.LFOVrotation.Text = "Rotation (deg)"
        '
        'TBFOVRotation
        '
        Me.TBFOVRotation.Location = New System.Drawing.Point(267, 130)
        Me.TBFOVRotation.Name = "TBFOVRotation"
        Me.TBFOVRotation.Size = New System.Drawing.Size(35, 20)
        Me.TBFOVRotation.TabIndex = 47
        '
        'TBFOVDecDisp
        '
        Me.TBFOVDecDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBFOVDecDisp.Enabled = False
        Me.TBFOVDecDisp.Location = New System.Drawing.Point(267, 243)
        Me.TBFOVDecDisp.Name = "TBFOVDecDisp"
        Me.TBFOVDecDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBFOVDecDisp.TabIndex = 46
        Me.TBFOVDecDisp.TabStop = False
        '
        'TBFOVRADisp
        '
        Me.TBFOVRADisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBFOVRADisp.Enabled = False
        Me.TBFOVRADisp.Location = New System.Drawing.Point(267, 217)
        Me.TBFOVRADisp.Name = "TBFOVRADisp"
        Me.TBFOVRADisp.Size = New System.Drawing.Size(100, 13)
        Me.TBFOVRADisp.TabIndex = 45
        Me.TBFOVRADisp.TabStop = False
        '
        'LFOVDec
        '
        Me.LFOVDec.AutoSize = True
        Me.LFOVDec.Location = New System.Drawing.Point(52, 243)
        Me.LFOVDec.Name = "LFOVDec"
        Me.LFOVDec.Size = New System.Drawing.Size(87, 13)
        Me.LFOVDec.TabIndex = 44
        Me.LFOVDec.Text = "Delcination (deg)"
        '
        'TBFOVDec
        '
        Me.TBFOVDec.Location = New System.Drawing.Point(146, 240)
        Me.TBFOVDec.Name = "TBFOVDec"
        Me.TBFOVDec.Size = New System.Drawing.Size(100, 20)
        Me.TBFOVDec.TabIndex = 43
        '
        'LFOVRA
        '
        Me.LFOVRA.AutoSize = True
        Me.LFOVRA.Location = New System.Drawing.Point(76, 217)
        Me.LFOVRA.Name = "LFOVRA"
        Me.LFOVRA.Size = New System.Drawing.Size(63, 13)
        Me.LFOVRA.TabIndex = 42
        Me.LFOVRA.Text = "RA (hr/deg)"
        '
        'TBFOVRA
        '
        Me.TBFOVRA.Location = New System.Drawing.Point(146, 214)
        Me.TBFOVRA.Name = "TBFOVRA"
        Me.TBFOVRA.Size = New System.Drawing.Size(100, 20)
        Me.TBFOVRA.TabIndex = 41
        '
        'TBFOVAzmDisp
        '
        Me.TBFOVAzmDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBFOVAzmDisp.Enabled = False
        Me.TBFOVAzmDisp.Location = New System.Drawing.Point(267, 191)
        Me.TBFOVAzmDisp.Name = "TBFOVAzmDisp"
        Me.TBFOVAzmDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBFOVAzmDisp.TabIndex = 40
        Me.TBFOVAzmDisp.TabStop = False
        '
        'TBFOVAltDisp
        '
        Me.TBFOVAltDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBFOVAltDisp.Enabled = False
        Me.TBFOVAltDisp.Location = New System.Drawing.Point(267, 165)
        Me.TBFOVAltDisp.Name = "TBFOVAltDisp"
        Me.TBFOVAltDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBFOVAltDisp.TabIndex = 39
        Me.TBFOVAltDisp.TabStop = False
        '
        'LFOVAzm
        '
        Me.LFOVAzm.AutoSize = True
        Me.LFOVAzm.Location = New System.Drawing.Point(68, 191)
        Me.LFOVAzm.Name = "LFOVAzm"
        Me.LFOVAzm.Size = New System.Drawing.Size(71, 13)
        Me.LFOVAzm.TabIndex = 38
        Me.LFOVAzm.Text = "Azimuth (deg)"
        '
        'TBFOVAzm
        '
        Me.TBFOVAzm.Location = New System.Drawing.Point(146, 188)
        Me.TBFOVAzm.Name = "TBFOVAzm"
        Me.TBFOVAzm.Size = New System.Drawing.Size(100, 20)
        Me.TBFOVAzm.TabIndex = 37
        '
        'LFOVAlt
        '
        Me.LFOVAlt.AutoSize = True
        Me.LFOVAlt.Location = New System.Drawing.Point(70, 165)
        Me.LFOVAlt.Name = "LFOVAlt"
        Me.LFOVAlt.Size = New System.Drawing.Size(69, 13)
        Me.LFOVAlt.TabIndex = 36
        Me.LFOVAlt.Text = "Altitude (deg)"
        '
        'TBFOVAlt
        '
        Me.TBFOVAlt.Location = New System.Drawing.Point(146, 162)
        Me.TBFOVAlt.Name = "TBFOVAlt"
        Me.TBFOVAlt.Size = New System.Drawing.Size(100, 20)
        Me.TBFOVAlt.TabIndex = 35
        '
        'LFOVheight
        '
        Me.LFOVheight.AutoSize = True
        Me.LFOVheight.Location = New System.Drawing.Point(78, 134)
        Me.LFOVheight.Name = "LFOVheight"
        Me.LFOVheight.Size = New System.Drawing.Size(65, 13)
        Me.LFOVheight.TabIndex = 34
        Me.LFOVheight.Text = "Height (deg)"
        '
        'LFOVwidth
        '
        Me.LFOVwidth.AutoSize = True
        Me.LFOVwidth.Location = New System.Drawing.Point(78, 111)
        Me.LFOVwidth.Name = "LFOVwidth"
        Me.LFOVwidth.Size = New System.Drawing.Size(62, 13)
        Me.LFOVwidth.TabIndex = 33
        Me.LFOVwidth.Text = "Width (deg)"
        '
        'GBFOVradec
        '
        Me.GBFOVradec.Controls.Add(Me.RBFOVradec)
        Me.GBFOVradec.Controls.Add(Me.RBFOValtazm)
        Me.GBFOVradec.Location = New System.Drawing.Point(81, 23)
        Me.GBFOVradec.Name = "GBFOVradec"
        Me.GBFOVradec.Size = New System.Drawing.Size(114, 72)
        Me.GBFOVradec.TabIndex = 32
        Me.GBFOVradec.TabStop = False
        Me.GBFOVradec.Text = "Pointing Mode"
        '
        'RBFOVradec
        '
        Me.RBFOVradec.AutoSize = True
        Me.RBFOVradec.Location = New System.Drawing.Point(7, 43)
        Me.RBFOVradec.Name = "RBFOVradec"
        Me.RBFOVradec.Size = New System.Drawing.Size(70, 17)
        Me.RBFOVradec.TabIndex = 1
        Me.RBFOVradec.TabStop = True
        Me.RBFOVradec.Text = "Ra / Dec"
        Me.RBFOVradec.UseVisualStyleBackColor = True
        '
        'RBFOValtazm
        '
        Me.RBFOValtazm.AutoSize = True
        Me.RBFOValtazm.Location = New System.Drawing.Point(7, 19)
        Me.RBFOValtazm.Name = "RBFOValtazm"
        Me.RBFOValtazm.Size = New System.Drawing.Size(68, 17)
        Me.RBFOValtazm.TabIndex = 0
        Me.RBFOValtazm.TabStop = True
        Me.RBFOValtazm.Text = "Alt / Azm"
        Me.RBFOValtazm.UseVisualStyleBackColor = True
        '
        'CBFOVRotate
        '
        Me.CBFOVRotate.AutoSize = True
        Me.CBFOVRotate.Location = New System.Drawing.Point(227, 94)
        Me.CBFOVRotate.Name = "CBFOVRotate"
        Me.CBFOVRotate.Size = New System.Drawing.Size(109, 17)
        Me.CBFOVRotate.TabIndex = 31
        Me.CBFOVRotate.Text = "Rotate (equitorial)"
        Me.CBFOVRotate.UseVisualStyleBackColor = True
        '
        'CBFOVTrack
        '
        Me.CBFOVTrack.AutoSize = True
        Me.CBFOVTrack.Location = New System.Drawing.Point(227, 70)
        Me.CBFOVTrack.Name = "CBFOVTrack"
        Me.CBFOVTrack.Size = New System.Drawing.Size(95, 17)
        Me.CBFOVTrack.TabIndex = 30
        Me.CBFOVTrack.Text = "Track (ra/dec)"
        Me.CBFOVTrack.UseVisualStyleBackColor = True
        '
        'TBFOVHeight
        '
        Me.TBFOVHeight.Location = New System.Drawing.Point(146, 130)
        Me.TBFOVHeight.Name = "TBFOVHeight"
        Me.TBFOVHeight.Size = New System.Drawing.Size(35, 20)
        Me.TBFOVHeight.TabIndex = 29
        '
        'TBFOVWidth
        '
        Me.TBFOVWidth.Location = New System.Drawing.Point(146, 104)
        Me.TBFOVWidth.Name = "TBFOVWidth"
        Me.TBFOVWidth.Size = New System.Drawing.Size(35, 20)
        Me.TBFOVWidth.TabIndex = 28
        '
        'CBFOViscircle
        '
        Me.CBFOViscircle.AutoSize = True
        Me.CBFOViscircle.Location = New System.Drawing.Point(227, 47)
        Me.CBFOViscircle.Name = "CBFOViscircle"
        Me.CBFOViscircle.Size = New System.Drawing.Size(61, 17)
        Me.CBFOViscircle.TabIndex = 27
        Me.CBFOViscircle.Text = "Circular"
        Me.CBFOViscircle.UseVisualStyleBackColor = True
        '
        'CBFOVShow
        '
        Me.CBFOVShow.AutoSize = True
        Me.CBFOVShow.Location = New System.Drawing.Point(227, 23)
        Me.CBFOVShow.Name = "CBFOVShow"
        Me.CBFOVShow.Size = New System.Drawing.Size(77, 17)
        Me.CBFOVShow.TabIndex = 26
        Me.CBFOVShow.Text = "Show FOV"
        Me.CBFOVShow.UseVisualStyleBackColor = True
        '
        'BFOVExit
        '
        Me.BFOVExit.Location = New System.Drawing.Point(227, 278)
        Me.BFOVExit.Name = "BFOVExit"
        Me.BFOVExit.Size = New System.Drawing.Size(75, 23)
        Me.BFOVExit.TabIndex = 25
        Me.BFOVExit.Text = "Save/Exit"
        Me.BFOVExit.UseVisualStyleBackColor = True
        '
        'BFOVCancel
        '
        Me.BFOVCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BFOVCancel.Location = New System.Drawing.Point(81, 278)
        Me.BFOVCancel.Name = "BFOVCancel"
        Me.BFOVCancel.Size = New System.Drawing.Size(75, 23)
        Me.BFOVCancel.TabIndex = 24
        Me.BFOVCancel.Text = "Cancel"
        Me.BFOVCancel.UseVisualStyleBackColor = True
        '
        'OptionTab
        '
        Me.OptionTab.BackColor = System.Drawing.SystemColors.Control
        Me.OptionTab.Location = New System.Drawing.Point(4, 22)
        Me.OptionTab.Name = "OptionTab"
        Me.OptionTab.Size = New System.Drawing.Size(394, 328)
        Me.OptionTab.TabIndex = 2
        Me.OptionTab.Text = "Options"
        '
        'UserSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 354)
        Me.Controls.Add(Me.TabControlSettings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UserSettings"
        Me.Text = "User Settings"
        Me.TabControlSettings.ResumeLayout(False)
        Me.LocationTab.ResumeLayout(False)
        Me.LocationTab.PerformLayout()
        Me.FOVTab.ResumeLayout(False)
        Me.FOVTab.PerformLayout()
        Me.GBFOVradec.ResumeLayout(False)
        Me.GBFOVradec.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControlSettings As System.Windows.Forms.TabControl
    Friend WithEvents LocationTab As System.Windows.Forms.TabPage
    Friend WithEvents TBLongDisp As System.Windows.Forms.TextBox
    Friend WithEvents TBLatDisp As System.Windows.Forms.TextBox
    Friend WithEvents BLocExit As System.Windows.Forms.Button
    Friend WithEvents BLocCancel As System.Windows.Forms.Button
    Friend WithEvents TBTZoffset As System.Windows.Forms.TextBox
    Friend WithEvents LTZoffset As System.Windows.Forms.Label
    Friend WithEvents LDescr As System.Windows.Forms.Label
    Friend WithEvents TBLName As System.Windows.Forms.TextBox
    Friend WithEvents LLong As System.Windows.Forms.Label
    Friend WithEvents TBLong As System.Windows.Forms.TextBox
    Friend WithEvents LElev As System.Windows.Forms.Label
    Friend WithEvents TBElev As System.Windows.Forms.TextBox
    Friend WithEvents LLat As System.Windows.Forms.Label
    Friend WithEvents TBLat As System.Windows.Forms.TextBox
    Friend WithEvents FOVTab As System.Windows.Forms.TabPage
    Friend WithEvents BFOVExit As System.Windows.Forms.Button
    Friend WithEvents BFOVCancel As System.Windows.Forms.Button
    Friend WithEvents CBFOVRotate As System.Windows.Forms.CheckBox
    Friend WithEvents CBFOVTrack As System.Windows.Forms.CheckBox
    Friend WithEvents TBFOVHeight As System.Windows.Forms.TextBox
    Friend WithEvents TBFOVWidth As System.Windows.Forms.TextBox
    Friend WithEvents CBFOViscircle As System.Windows.Forms.CheckBox
    Friend WithEvents CBFOVShow As System.Windows.Forms.CheckBox
    Friend WithEvents LFOVheight As System.Windows.Forms.Label
    Friend WithEvents LFOVwidth As System.Windows.Forms.Label
    Friend WithEvents GBFOVradec As System.Windows.Forms.GroupBox
    Friend WithEvents RBFOVradec As System.Windows.Forms.RadioButton
    Friend WithEvents RBFOValtazm As System.Windows.Forms.RadioButton
    Friend WithEvents LFOVrotation As System.Windows.Forms.Label
    Friend WithEvents TBFOVRotation As System.Windows.Forms.TextBox
    Friend WithEvents TBFOVDecDisp As System.Windows.Forms.TextBox
    Friend WithEvents TBFOVRADisp As System.Windows.Forms.TextBox
    Friend WithEvents LFOVDec As System.Windows.Forms.Label
    Friend WithEvents TBFOVDec As System.Windows.Forms.TextBox
    Friend WithEvents LFOVRA As System.Windows.Forms.Label
    Friend WithEvents TBFOVRA As System.Windows.Forms.TextBox
    Friend WithEvents TBFOVAzmDisp As System.Windows.Forms.TextBox
    Friend WithEvents TBFOVAltDisp As System.Windows.Forms.TextBox
    Friend WithEvents LFOVAzm As System.Windows.Forms.Label
    Friend WithEvents TBFOVAzm As System.Windows.Forms.TextBox
    Friend WithEvents LFOVAlt As System.Windows.Forms.Label
    Friend WithEvents TBFOVAlt As System.Windows.Forms.TextBox
    Friend WithEvents OptionTab As System.Windows.Forms.TabPage

   
End Class
