Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text.RegularExpressions
Imports NodaTime

Public Class Utils
    Public Shared Function ExtractCountry(ByVal loc As String) As String
        loc = " " & loc & " "
        Dim countries As IEnumerable(Of String) = File.ReadLines(HttpContext.Current.Server.MapPath("~/App_Data/Countries.txt"))
        For Each country As String In countries
            If (country = "Israel") Then
                Dim a = 1
            End If
            If loc.Contains(" " & country & " ") Then
                Return country
            End If
        Next
        Return "Unknown"
    End Function

    Public Shared Function Tenrary(Of T)(condition As Boolean, y As T, n As T) As T
        If condition Then
            Return y
        Else
            Return n
        End If
    End Function

    Public Shared Function StringifyPeriod(ByVal ts As Period) As String
        If ts.Years > 0 Then
            Return String.Format("{0} Year{1}, {2} Month{3}", ts.Years, Tenrary(ts.Years = 1, "", "s"), ts.Months, Tenrary(ts.Months = 1, "", "s"))
        ElseIf ts.Months > 0 Then
            Return String.Format("{0} Month{1}, {2} Weeks{3}, {4} Day{5}", ts.Months, Tenrary(ts.Months = 1, "", "s"), ts.Weeks, Tenrary(ts.Weeks = 1, "", "s"), ts.Days, Tenrary(ts.Days = 1, "", "s"))
        ElseIf ts.Weeks > 0 Then
            Return String.Format("{0} Week{1}, {2} Day{3}", ts.Weeks, Tenrary(ts.Weeks = 1, "", "s"), ts.Days, Tenrary(ts.Days = 1, "", "s"))
        ElseIf ts.Days > 0 Then
            Return String.Format("{0} Day{1}, {2} Hour{3}", ts.Days, Tenrary(ts.Days = 1, "", "s"), ts.Hours, Tenrary(ts.Hours = 1, "", "s"))
        ElseIf ts.Hours > 0 Then
            Return String.Format("{0} Hour{1}, {2} Minute{3}", ts.Hours, Tenrary(ts.Hours = 1, "", "s"), ts.Minutes, Tenrary(ts.Minutes = 1, "", "s"))
        ElseIf ts.Minutes > 0 Then
            Return String.Format("{0} Minute{1}", ts.Minutes, Tenrary(ts.Minutes = 1, "", "s"))
        End If
        Return Nothing
    End Function


    Public Shared Function GetGeoPos(ByVal address As String) As GeoPos
        Try
            Dim doc As XDocument = XDocument.Load("http://maps.googleapis.com/maps/api/geocode/xml?sensor=true_or_false&address=" & HttpUtility.UrlEncode(address))
            If doc.Elements.First.Elements.First.Value = "OK" Then
                Dim response As XElement = doc.Elements.Where(Function(x) x.Name.LocalName.Equals("GeocodeResponse")).First()
                Dim a As XElement = response.Elements.Where(Function(x) x.Name.LocalName.Equals("result")).First().Elements.Where(Function(x) x.Name.LocalName.Equals("geometry")).First().Elements.Where(Function(x) x.Name.LocalName.Equals("location")).First()
                Dim b As String = response.Elements.Where(Function(x) x.Name.LocalName.Equals("result")).First().Elements.Where(Function(x) x.Name.LocalName.Equals("formatted_address")).First().Value
                Dim lat As Double = Double.Parse(a.Elements.Where(Function(x) x.Name.LocalName.Equals("lat")).First().Value)
                Dim lon As Double = Double.Parse(a.Elements.Where(Function(x) x.Name.LocalName.Equals("lng")).First().Value)
                Dim loc As GeoPos = New GeoPos(lat, lon)
                loc.Address = b
                Return loc
            End If
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class
