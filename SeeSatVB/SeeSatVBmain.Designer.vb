<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeeSatVBmain
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
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.BSatWindow = New System.Windows.Forms.Button()
        Me.TimerR = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllDefaultFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.McCantsMagFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisualMagFileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StarDataFileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TLEFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UserLocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetNTPTimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetNowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OnStartUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisualToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StereoProjectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilterByMagToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveDefaultsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerS = New System.Windows.Forms.Timer(Me.components)
        Me.DateTimePickerST = New System.Windows.Forms.DateTimePicker()
        Me.CheckBoxUTC = New System.Windows.Forms.CheckBox()
        Me.RadioButtonPM = New System.Windows.Forms.RadioButton()
        Me.RadioButtonRT = New System.Windows.Forms.RadioButton()
        Me.GroupBoxTM = New System.Windows.Forms.GroupBox()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GroupBoxPM = New System.Windows.Forms.GroupBox()
        Me.RadioButtonST = New System.Windows.Forms.RadioButton()
        Me.RadioButtonTO = New System.Windows.Forms.RadioButton()
        Me.NUpDownHr = New System.Windows.Forms.NumericUpDown()
        Me.NUpDownMin = New System.Windows.Forms.NumericUpDown()
        Me.NUpDownSec = New System.Windows.Forms.NumericUpDown()
        Me.LabelHr = New System.Windows.Forms.Label()
        Me.LabelMin = New System.Windows.Forms.Label()
        Me.LabelSec = New System.Windows.Forms.Label()
        Me.GroupBoxTS = New System.Windows.Forms.GroupBox()
        Me.FieldOfViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBoxTM.SuspendLayout()
        Me.GroupBoxPM.SuspendLayout()
        CType(Me.NUpDownHr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUpDownMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NUpDownSec, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxTS.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(15, 112)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Lib Version"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(122, 27)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox1.Size = New System.Drawing.Size(390, 356)
        Me.TextBox1.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(15, 141)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Run"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'BSatWindow
        '
        Me.BSatWindow.Location = New System.Drawing.Point(15, 170)
        Me.BSatWindow.Name = "BSatWindow"
        Me.BSatWindow.Size = New System.Drawing.Size(75, 22)
        Me.BSatWindow.TabIndex = 3
        Me.BSatWindow.Text = "SatWindow"
        Me.BSatWindow.UseVisualStyleBackColor = True
        '
        'TimerR
        '
        Me.TimerR.Interval = 1000
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.OptionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(560, 24)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(37, 20)
        Me.ToolStripMenuItem1.Text = "File"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllDefaultFilesToolStripMenuItem, Me.McCantsMagFileToolStripMenuItem, Me.VisualMagFileToolStripMenuItem1, Me.StarDataFileToolStripMenuItem1, Me.TLEFileToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.FileToolStripMenuItem.Text = "Open"
        '
        'AllDefaultFilesToolStripMenuItem
        '
        Me.AllDefaultFilesToolStripMenuItem.Name = "AllDefaultFilesToolStripMenuItem"
        Me.AllDefaultFilesToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.AllDefaultFilesToolStripMenuItem.Text = "All Default Files"
        '
        'McCantsMagFileToolStripMenuItem
        '
        Me.McCantsMagFileToolStripMenuItem.Name = "McCantsMagFileToolStripMenuItem"
        Me.McCantsMagFileToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.McCantsMagFileToolStripMenuItem.Text = "McCants Mag File"
        '
        'VisualMagFileToolStripMenuItem1
        '
        Me.VisualMagFileToolStripMenuItem1.Name = "VisualMagFileToolStripMenuItem1"
        Me.VisualMagFileToolStripMenuItem1.Size = New System.Drawing.Size(169, 22)
        Me.VisualMagFileToolStripMenuItem1.Text = "Visual Mag File"
        '
        'StarDataFileToolStripMenuItem1
        '
        Me.StarDataFileToolStripMenuItem1.Name = "StarDataFileToolStripMenuItem1"
        Me.StarDataFileToolStripMenuItem1.Size = New System.Drawing.Size(169, 22)
        Me.StarDataFileToolStripMenuItem1.Text = "Star Data File"
        '
        'TLEFileToolStripMenuItem
        '
        Me.TLEFileToolStripMenuItem.Name = "TLEFileToolStripMenuItem"
        Me.TLEFileToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.TLEFileToolStripMenuItem.Text = "TLE File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UserLocationToolStripMenuItem, Me.FieldOfViewToolStripMenuItem, Me.SetNTPTimeToolStripMenuItem, Me.VisualToolStripMenuItem, Me.SaveDefaultsToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'UserLocationToolStripMenuItem
        '
        Me.UserLocationToolStripMenuItem.Name = "UserLocationToolStripMenuItem"
        Me.UserLocationToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.UserLocationToolStripMenuItem.Text = "User Location"
        '
        'SetNTPTimeToolStripMenuItem
        '
        Me.SetNTPTimeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetNowToolStripMenuItem, Me.OnStartUpToolStripMenuItem})
        Me.SetNTPTimeToolStripMenuItem.Name = "SetNTPTimeToolStripMenuItem"
        Me.SetNTPTimeToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.SetNTPTimeToolStripMenuItem.Text = "NTP Time"
        '
        'SetNowToolStripMenuItem
        '
        Me.SetNowToolStripMenuItem.Name = "SetNowToolStripMenuItem"
        Me.SetNowToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SetNowToolStripMenuItem.Text = "Set Now"
        '
        'OnStartUpToolStripMenuItem
        '
        Me.OnStartUpToolStripMenuItem.CheckOnClick = True
        Me.OnStartUpToolStripMenuItem.Name = "OnStartUpToolStripMenuItem"
        Me.OnStartUpToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OnStartUpToolStripMenuItem.Text = "On Start Up"
        '
        'VisualToolStripMenuItem
        '
        Me.VisualToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StereoProjectionToolStripMenuItem, Me.FilterByMagToolStripMenuItem})
        Me.VisualToolStripMenuItem.Name = "VisualToolStripMenuItem"
        Me.VisualToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.VisualToolStripMenuItem.Text = "Visual"
        '
        'StereoProjectionToolStripMenuItem
        '
        Me.StereoProjectionToolStripMenuItem.CheckOnClick = True
        Me.StereoProjectionToolStripMenuItem.Name = "StereoProjectionToolStripMenuItem"
        Me.StereoProjectionToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.StereoProjectionToolStripMenuItem.Text = "Stereo Projection"
        '
        'FilterByMagToolStripMenuItem
        '
        Me.FilterByMagToolStripMenuItem.CheckOnClick = True
        Me.FilterByMagToolStripMenuItem.Name = "FilterByMagToolStripMenuItem"
        Me.FilterByMagToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.FilterByMagToolStripMenuItem.Text = "Filter by Mag"
        '
        'SaveDefaultsToolStripMenuItem
        '
        Me.SaveDefaultsToolStripMenuItem.Name = "SaveDefaultsToolStripMenuItem"
        Me.SaveDefaultsToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.SaveDefaultsToolStripMenuItem.Text = "Save All Defaults"
        '
        'TimerS
        '
        '
        'DateTimePickerST
        '
        Me.DateTimePickerST.CustomFormat = "MM/dd/yyyy - HH:mm:ss"
        Me.DateTimePickerST.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePickerST.Location = New System.Drawing.Point(69, 15)
        Me.DateTimePickerST.Name = "DateTimePickerST"
        Me.DateTimePickerST.Size = New System.Drawing.Size(157, 20)
        Me.DateTimePickerST.TabIndex = 5
        '
        'CheckBoxUTC
        '
        Me.CheckBoxUTC.AutoSize = True
        Me.CheckBoxUTC.Location = New System.Drawing.Point(13, 15)
        Me.CheckBoxUTC.Name = "CheckBoxUTC"
        Me.CheckBoxUTC.Size = New System.Drawing.Size(48, 17)
        Me.CheckBoxUTC.TabIndex = 8
        Me.CheckBoxUTC.Text = "UTC"
        Me.CheckBoxUTC.UseVisualStyleBackColor = True
        '
        'RadioButtonPM
        '
        Me.RadioButtonPM.AutoSize = True
        Me.RadioButtonPM.Location = New System.Drawing.Point(6, 41)
        Me.RadioButtonPM.Name = "RadioButtonPM"
        Me.RadioButtonPM.Size = New System.Drawing.Size(58, 17)
        Me.RadioButtonPM.TabIndex = 9
        Me.RadioButtonPM.Text = "Predict"
        Me.RadioButtonPM.UseVisualStyleBackColor = True
        '
        'RadioButtonRT
        '
        Me.RadioButtonRT.AutoSize = True
        Me.RadioButtonRT.Checked = True
        Me.RadioButtonRT.Location = New System.Drawing.Point(6, 18)
        Me.RadioButtonRT.Name = "RadioButtonRT"
        Me.RadioButtonRT.Size = New System.Drawing.Size(73, 17)
        Me.RadioButtonRT.TabIndex = 10
        Me.RadioButtonRT.TabStop = True
        Me.RadioButtonRT.Text = "Real Time"
        Me.RadioButtonRT.UseVisualStyleBackColor = True
        '
        'GroupBoxTM
        '
        Me.GroupBoxTM.Controls.Add(Me.RadioButtonRT)
        Me.GroupBoxTM.Controls.Add(Me.RadioButtonPM)
        Me.GroupBoxTM.Location = New System.Drawing.Point(15, 309)
        Me.GroupBoxTM.Name = "GroupBoxTM"
        Me.GroupBoxTM.Size = New System.Drawing.Size(89, 70)
        Me.GroupBoxTM.TabIndex = 11
        Me.GroupBoxTM.TabStop = False
        Me.GroupBoxTM.Text = "Time Mode"
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'GroupBoxPM
        '
        Me.GroupBoxPM.Controls.Add(Me.RadioButtonST)
        Me.GroupBoxPM.Controls.Add(Me.RadioButtonTO)
        Me.GroupBoxPM.Location = New System.Drawing.Point(15, 387)
        Me.GroupBoxPM.Name = "GroupBoxPM"
        Me.GroupBoxPM.Size = New System.Drawing.Size(89, 70)
        Me.GroupBoxPM.TabIndex = 12
        Me.GroupBoxPM.TabStop = False
        Me.GroupBoxPM.Text = "Predict Mode"
        '
        'RadioButtonST
        '
        Me.RadioButtonST.AutoSize = True
        Me.RadioButtonST.Checked = True
        Me.RadioButtonST.Location = New System.Drawing.Point(6, 18)
        Me.RadioButtonST.Name = "RadioButtonST"
        Me.RadioButtonST.Size = New System.Drawing.Size(73, 17)
        Me.RadioButtonST.TabIndex = 10
        Me.RadioButtonST.TabStop = True
        Me.RadioButtonST.Text = "Start Time"
        Me.RadioButtonST.UseVisualStyleBackColor = True
        '
        'RadioButtonTO
        '
        Me.RadioButtonTO.AutoSize = True
        Me.RadioButtonTO.Location = New System.Drawing.Point(6, 41)
        Me.RadioButtonTO.Name = "RadioButtonTO"
        Me.RadioButtonTO.Size = New System.Drawing.Size(79, 17)
        Me.RadioButtonTO.TabIndex = 9
        Me.RadioButtonTO.Text = "Time Offset"
        Me.RadioButtonTO.UseVisualStyleBackColor = True
        '
        'NUpDownHr
        '
        Me.NUpDownHr.Location = New System.Drawing.Point(33, 37)
        Me.NUpDownHr.Maximum = New Decimal(New Integer() {8784, 0, 0, 0})
        Me.NUpDownHr.Minimum = New Decimal(New Integer() {8784, 0, 0, -2147483648})
        Me.NUpDownHr.Name = "NUpDownHr"
        Me.NUpDownHr.Size = New System.Drawing.Size(47, 20)
        Me.NUpDownHr.TabIndex = 13
        Me.NUpDownHr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NUpDownMin
        '
        Me.NUpDownMin.Location = New System.Drawing.Point(115, 37)
        Me.NUpDownMin.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.NUpDownMin.Minimum = New Decimal(New Integer() {60, 0, 0, -2147483648})
        Me.NUpDownMin.Name = "NUpDownMin"
        Me.NUpDownMin.Size = New System.Drawing.Size(36, 20)
        Me.NUpDownMin.TabIndex = 14
        Me.NUpDownMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'NUpDownSec
        '
        Me.NUpDownSec.Increment = New Decimal(New Integer() {11, 0, 0, 65536})
        Me.NUpDownSec.Location = New System.Drawing.Point(189, 37)
        Me.NUpDownSec.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.NUpDownSec.Minimum = New Decimal(New Integer() {60, 0, 0, -2147483648})
        Me.NUpDownSec.Name = "NUpDownSec"
        Me.NUpDownSec.Size = New System.Drawing.Size(36, 20)
        Me.NUpDownSec.TabIndex = 15
        Me.NUpDownSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelHr
        '
        Me.LabelHr.AutoSize = True
        Me.LabelHr.Location = New System.Drawing.Point(9, 39)
        Me.LabelHr.Name = "LabelHr"
        Me.LabelHr.Size = New System.Drawing.Size(18, 13)
        Me.LabelHr.TabIndex = 16
        Me.LabelHr.Text = "Hr"
        '
        'LabelMin
        '
        Me.LabelMin.AutoSize = True
        Me.LabelMin.Location = New System.Drawing.Point(85, 39)
        Me.LabelMin.Name = "LabelMin"
        Me.LabelMin.Size = New System.Drawing.Size(24, 13)
        Me.LabelMin.TabIndex = 17
        Me.LabelMin.Text = "Min"
        '
        'LabelSec
        '
        Me.LabelSec.AutoSize = True
        Me.LabelSec.Location = New System.Drawing.Point(157, 39)
        Me.LabelSec.Name = "LabelSec"
        Me.LabelSec.Size = New System.Drawing.Size(26, 13)
        Me.LabelSec.TabIndex = 18
        Me.LabelSec.Text = "Sec"
        '
        'GroupBoxTS
        '
        Me.GroupBoxTS.Controls.Add(Me.DateTimePickerST)
        Me.GroupBoxTS.Controls.Add(Me.LabelSec)
        Me.GroupBoxTS.Controls.Add(Me.CheckBoxUTC)
        Me.GroupBoxTS.Controls.Add(Me.LabelMin)
        Me.GroupBoxTS.Controls.Add(Me.NUpDownHr)
        Me.GroupBoxTS.Controls.Add(Me.LabelHr)
        Me.GroupBoxTS.Controls.Add(Me.NUpDownMin)
        Me.GroupBoxTS.Controls.Add(Me.NUpDownSec)
        Me.GroupBoxTS.Location = New System.Drawing.Point(122, 387)
        Me.GroupBoxTS.Name = "GroupBoxTS"
        Me.GroupBoxTS.Size = New System.Drawing.Size(246, 70)
        Me.GroupBoxTS.TabIndex = 19
        Me.GroupBoxTS.TabStop = False
        '
        'FieldOfViewToolStripMenuItem
        '
        Me.FieldOfViewToolStripMenuItem.Name = "FieldOfViewToolStripMenuItem"
        Me.FieldOfViewToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.FieldOfViewToolStripMenuItem.Text = "Field of View"
        '
        'SeeSatVBmain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(560, 469)
        Me.Controls.Add(Me.GroupBoxTS)
        Me.Controls.Add(Me.GroupBoxPM)
        Me.Controls.Add(Me.GroupBoxTM)
        Me.Controls.Add(Me.BSatWindow)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "SeeSatVBmain"
        Me.Text = "SeeSatVB"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBoxTM.ResumeLayout(False)
        Me.GroupBoxTM.PerformLayout()
        Me.GroupBoxPM.ResumeLayout(False)
        Me.GroupBoxPM.PerformLayout()
        CType(Me.NUpDownHr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUpDownMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NUpDownSec, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxTS.ResumeLayout(False)
        Me.GroupBoxTS.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents BSatWindow As System.Windows.Forms.Button
    Friend WithEvents TimerR As System.Windows.Forms.Timer
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TLEFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents McCantsMagFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualMagFileToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StarDataFileToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllDefaultFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveDefaultsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerS As System.Windows.Forms.Timer
    Friend WithEvents DateTimePickerST As System.Windows.Forms.DateTimePicker
    Friend WithEvents CheckBoxUTC As System.Windows.Forms.CheckBox
    Friend WithEvents RadioButtonPM As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonRT As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBoxTM As System.Windows.Forms.GroupBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents UserLocationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetNTPTimeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBoxPM As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonST As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonTO As System.Windows.Forms.RadioButton
    Friend WithEvents NUpDownHr As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUpDownMin As System.Windows.Forms.NumericUpDown
    Friend WithEvents NUpDownSec As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelHr As System.Windows.Forms.Label
    Friend WithEvents LabelMin As System.Windows.Forms.Label
    Friend WithEvents LabelSec As System.Windows.Forms.Label
    Friend WithEvents GroupBoxTS As System.Windows.Forms.GroupBox
    Friend WithEvents SetNowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OnStartUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisualToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StereoProjectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FilterByMagToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FieldOfViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
