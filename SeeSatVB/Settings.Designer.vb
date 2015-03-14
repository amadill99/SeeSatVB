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
        Me.TBLat = New System.Windows.Forms.TextBox()
        Me.LLat = New System.Windows.Forms.Label()
        Me.LElev = New System.Windows.Forms.Label()
        Me.TBElev = New System.Windows.Forms.TextBox()
        Me.LLong = New System.Windows.Forms.Label()
        Me.TBLong = New System.Windows.Forms.TextBox()
        Me.LDescr = New System.Windows.Forms.Label()
        Me.TBLName = New System.Windows.Forms.TextBox()
        Me.LTZoffset = New System.Windows.Forms.Label()
        Me.TBTZoffset = New System.Windows.Forms.TextBox()
        Me.BCancel = New System.Windows.Forms.Button()
        Me.BExit = New System.Windows.Forms.Button()
        Me.TBLatDisp = New System.Windows.Forms.TextBox()
        Me.TBLongDisp = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TBLat
        '
        Me.TBLat.Location = New System.Drawing.Point(151, 20)
        Me.TBLat.Name = "TBLat"
        Me.TBLat.Size = New System.Drawing.Size(100, 20)
        Me.TBLat.TabIndex = 0
        '
        'LLat
        '
        Me.LLat.AutoSize = True
        Me.LLat.Location = New System.Drawing.Point(27, 23)
        Me.LLat.Name = "LLat"
        Me.LLat.Size = New System.Drawing.Size(83, 13)
        Me.LLat.TabIndex = 1
        Me.LLat.Text = "Latitude (-south)"
        '
        'LElev
        '
        Me.LElev.AutoSize = True
        Me.LElev.Location = New System.Drawing.Point(27, 116)
        Me.LElev.Name = "LElev"
        Me.LElev.Size = New System.Drawing.Size(91, 13)
        Me.LElev.TabIndex = 3
        Me.LElev.Text = "Elevation (meters)"
        '
        'TBElev
        '
        Me.TBElev.Location = New System.Drawing.Point(151, 113)
        Me.TBElev.Name = "TBElev"
        Me.TBElev.Size = New System.Drawing.Size(100, 20)
        Me.TBElev.TabIndex = 3
        '
        'LLong
        '
        Me.LLong.AutoSize = True
        Me.LLong.Location = New System.Drawing.Point(27, 68)
        Me.LLong.Name = "LLong"
        Me.LLong.Size = New System.Drawing.Size(88, 13)
        Me.LLong.TabIndex = 5
        Me.LLong.Text = "Longitude (-west)"
        '
        'TBLong
        '
        Me.TBLong.Location = New System.Drawing.Point(151, 65)
        Me.TBLong.Name = "TBLong"
        Me.TBLong.Size = New System.Drawing.Size(100, 20)
        Me.TBLong.TabIndex = 2
        '
        'LDescr
        '
        Me.LDescr.AutoSize = True
        Me.LDescr.Location = New System.Drawing.Point(27, 142)
        Me.LDescr.Name = "LDescr"
        Me.LDescr.Size = New System.Drawing.Size(60, 13)
        Me.LDescr.TabIndex = 7
        Me.LDescr.Text = "Description"
        '
        'TBLName
        '
        Me.TBLName.Location = New System.Drawing.Point(93, 139)
        Me.TBLName.Name = "TBLName"
        Me.TBLName.Size = New System.Drawing.Size(158, 20)
        Me.TBLName.TabIndex = 4
        '
        'LTZoffset
        '
        Me.LTZoffset.AutoSize = True
        Me.LTZoffset.Location = New System.Drawing.Point(27, 169)
        Me.LTZoffset.Name = "LTZoffset"
        Me.LTZoffset.Size = New System.Drawing.Size(119, 13)
        Me.LTZoffset.TabIndex = 8
        Me.LTZoffset.Text = "Time zone offset (-west)"
        '
        'TBTZoffset
        '
        Me.TBTZoffset.Location = New System.Drawing.Point(191, 166)
        Me.TBTZoffset.Name = "TBTZoffset"
        Me.TBTZoffset.Size = New System.Drawing.Size(60, 20)
        Me.TBTZoffset.TabIndex = 5
        '
        'BCancel
        '
        Me.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BCancel.Location = New System.Drawing.Point(30, 214)
        Me.BCancel.Name = "BCancel"
        Me.BCancel.Size = New System.Drawing.Size(75, 23)
        Me.BCancel.TabIndex = 6
        Me.BCancel.Text = "Cancel"
        Me.BCancel.UseVisualStyleBackColor = True
        '
        'BExit
        '
        Me.BExit.Location = New System.Drawing.Point(176, 214)
        Me.BExit.Name = "BExit"
        Me.BExit.Size = New System.Drawing.Size(75, 23)
        Me.BExit.TabIndex = 7
        Me.BExit.Text = "Save/Exit"
        Me.BExit.UseVisualStyleBackColor = True
        '
        'TBLatDisp
        '
        Me.TBLatDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBLatDisp.Enabled = False
        Me.TBLatDisp.Location = New System.Drawing.Point(151, 46)
        Me.TBLatDisp.Name = "TBLatDisp"
        Me.TBLatDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBLatDisp.TabIndex = 12
        Me.TBLatDisp.TabStop = False
        '
        'TBLongDisp
        '
        Me.TBLongDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TBLongDisp.Enabled = False
        Me.TBLongDisp.Location = New System.Drawing.Point(151, 91)
        Me.TBLongDisp.Name = "TBLongDisp"
        Me.TBLongDisp.Size = New System.Drawing.Size(100, 13)
        Me.TBLongDisp.TabIndex = 13
        Me.TBLongDisp.TabStop = False
        '
        'UserSettings
        '
        Me.AcceptButton = Me.BExit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BCancel
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.TBLongDisp)
        Me.Controls.Add(Me.TBLatDisp)
        Me.Controls.Add(Me.BExit)
        Me.Controls.Add(Me.BCancel)
        Me.Controls.Add(Me.TBTZoffset)
        Me.Controls.Add(Me.LTZoffset)
        Me.Controls.Add(Me.LDescr)
        Me.Controls.Add(Me.TBLName)
        Me.Controls.Add(Me.LLong)
        Me.Controls.Add(Me.TBLong)
        Me.Controls.Add(Me.LElev)
        Me.Controls.Add(Me.TBElev)
        Me.Controls.Add(Me.LLat)
        Me.Controls.Add(Me.TBLat)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UserSettings"
        Me.Text = "User Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TBLat As System.Windows.Forms.TextBox
    Friend WithEvents LLat As System.Windows.Forms.Label
    Friend WithEvents LElev As System.Windows.Forms.Label
    Friend WithEvents TBElev As System.Windows.Forms.TextBox
    Friend WithEvents LLong As System.Windows.Forms.Label
    Friend WithEvents TBLong As System.Windows.Forms.TextBox
    Friend WithEvents LDescr As System.Windows.Forms.Label
    Friend WithEvents TBLName As System.Windows.Forms.TextBox
    Friend WithEvents LTZoffset As System.Windows.Forms.Label
    Friend WithEvents TBTZoffset As System.Windows.Forms.TextBox
    Friend WithEvents BCancel As System.Windows.Forms.Button
    Friend WithEvents BExit As System.Windows.Forms.Button
    Friend WithEvents TBLatDisp As System.Windows.Forms.TextBox
    Friend WithEvents TBLongDisp As System.Windows.Forms.TextBox

   
End Class
