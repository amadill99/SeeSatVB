﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SatWindow
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
        Me.TimerR = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTipSat = New System.Windows.Forms.ToolTip(Me.components)
        Me.TimerM = New System.Windows.Forms.Timer(Me.components)
        Me.TextBoxS = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TimerR
        '
        Me.TimerR.Enabled = True
        Me.TimerR.Interval = 1000
        '
        'ToolTipSat
        '
        Me.ToolTipSat.AutomaticDelay = 10
        Me.ToolTipSat.ShowAlways = True
        Me.ToolTipSat.UseAnimation = False
        Me.ToolTipSat.UseFading = False
        '
        'TimerM
        '
        Me.TimerM.Enabled = True
        Me.TimerM.Interval = 5000
        '
        'TextBoxS
        '
        Me.TextBoxS.Dock = System.Windows.Forms.DockStyle.Right
        Me.TextBoxS.Enabled = False
        Me.TextBoxS.Location = New System.Drawing.Point(670, 0)
        Me.TextBoxS.Multiline = True
        Me.TextBoxS.Name = "TextBoxS"
        Me.TextBoxS.Size = New System.Drawing.Size(346, 642)
        Me.TextBoxS.TabIndex = 0
        '
        'SatWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Desktop
        Me.ClientSize = New System.Drawing.Size(1016, 642)
        Me.Controls.Add(Me.TextBoxS)
        Me.DoubleBuffered = True
        Me.Name = "SatWindow"
        Me.Text = "SatWindow"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TimerR As System.Windows.Forms.Timer
    Friend WithEvents ToolTipSat As System.Windows.Forms.ToolTip
    Friend WithEvents TimerM As System.Windows.Forms.Timer
    Friend WithEvents TextBoxS As System.Windows.Forms.TextBox
End Class
