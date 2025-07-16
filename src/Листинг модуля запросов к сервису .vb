Sub GenerateReportSimple(month As Integer, year As Integer)
    Dim ws As Worksheet
    Set ws = ThisWorkbook.Sheets(1)
    ws.Cells.Clear
    
    Dim http As Object, csvData As String
    Set http = CreateObject("MSXML2.XMLHTTP")
    
    http.Open "GET", "http://localhost:5113/api/Reports/monthly/csv?month=" & month & "&year=" & year, False
    http.Send
    
    If http.Status = 200 Then
        csvData = http.responseText
        Dim lines As Variant, i As Integer, j As Integer
        lines = Split(csvData, vbLf)
        
        For i = 0 To UBound(lines)
            If Len(lines(i)) > 0 Then
                Dim fields As Variant
                fields = Split(lines(i), ",")
                For j = 0 To UBound(fields)
                     ws.Cells(i + 1, j + 1).Value = fields(j)
                Next j
            End If
        Next i
    Else
        MsgBox "Eror HTTP: " & http.Status & " - " & http.statusText, vbCritical
    End If
End Sub
