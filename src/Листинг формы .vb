
Private Sub UserForm_Initialize()
   
    For i = 1 To 12
        ComboBox1.AddItem MonthName(i, False)
    Next i
    ComboBox1.Value = MonthName(month(Date), False)
    
    
    For i = year(Date) - 2 To year(Date) + 1
        ComboBox2.AddItem i
    Next i
    ComboBox2.Value = year(Date)
End Sub

Private Sub CommandButton1_Click()
    Dim monthNum As Integer
    monthNum = ComboBox1.ListIndex + 1
    GenerateReportSimple monthNum, ComboBox2.Value
    Unload Me
End Sub
