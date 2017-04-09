Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text.RegularExpressions
Imports NodaTime
Imports System.Net.Mail
Imports System.Net
Imports System.Runtime.CompilerServices
Imports LiteDB
Imports SendGrid
Imports SendGrid.Helpers.Mail

Public Module StrUtil

    Public Function Relevancy(s As String, d As String) As Integer
        Dim dwrds = d.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries).Select(Function(x) x.ToUpper()).Distinct()
        Dim swrds = s.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries).Select(Function(x) x.ToUpper()).Distinct()
        Dim pts = 0
        For Each word In dwrds
            For Each sword In swrds
                If sword = word Then
                    pts += 30
                Else
                    Dim distance = word.DistanceTo(sword)
                    If distance < 5 Then
                        pts += 10 - distance
                    End If
                End If
            Next
        Next
        Return pts
    End Function

    Public Function Null(Of T)(def As T, alt As T) As T
        If def Is Nothing Then
            Return alt
        Else
            Return def
        End If
    End Function

    <Extension()>
    Public Function DistanceTo(s As String, t As String) As Integer ' TODO: check if right.
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length

        Dim d As Integer(,) = New Integer(n, m) {}

        ' Step 1
        If n = 0 Then
            Return m
        End If

        If m = 0 Then
            Return n
        End If

        ' Step 2
        Dim ii As Integer = 0
        While ii <= n
            d(ii, 0) = System.Math.Max(System.Threading.Interlocked.Increment(ii), ii - 1)
        End While

        Dim jj As Integer = 0
        While jj <= m
            d(0, jj) = System.Math.Max(System.Threading.Interlocked.Increment(jj), jj - 1)
        End While

        ' Step 3
        For i As Integer = 1 To n
            'Step 4
            For j As Integer = 1 To m
                ' Step 5
                Dim cost As Integer = If((t(j - 1) = s(i - 1)), 0, 1)

                ' Step 6
                d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + cost)
            Next
        Next
        ' Step 7
        Return d(n, m)
    End Function
End Module

Public Class Utils

    Public Shared Sub Alert(ByVal msg As String)
        HttpContext.Current.Response.Write(Escape("<script>(function(){var a=window.onload||function(){};window.onload=function(){a();alert('" + msg + "');};})();</script>"))
    End Sub

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

    Public Shared Function Tenrary(Of T)(ByVal condition As Boolean, ByVal y As T, ByVal n As T) As T
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
            Return String.Format("{0} Month{1}, {2} Week{3}, {4} Day{5}", ts.Months, Tenrary(ts.Months = 1, "", "s"), ts.Weeks, Tenrary(ts.Weeks = 1, "", "s"), ts.Days, Tenrary(ts.Days = 1, "", "s"))
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

    Public Shared Function Escape(s As String) As String
        Return s.Replace("""", "\""").Replace("'", "\'")
    End Function

End Class


Public Class Notifier
    Public Shared Sub NotifyEmail(email As String, content As String, subject As String)
        Dim template = File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/EmailNotificationTemplate.html"))
        Dim fcontent = template.Replace("{message}", content)

        'Dim smtp As SmtpClient = New SmtpClient("smtp.gmail.com", 587)
        'smtp.EnableSsl = True
        'smtp.DeliveryMethod = SmtpDeliveryMethod.Network
        'smtp.UseDefaultCredentials = False
        'smtp.Credentials = New NetworkCredential("rodicarogozin@gmail.com", "ROroDIca")
        'Dim msg As MailMessage = New MailMessage(
        '    New MailAddress("rodicarogozin@gmail.com", "DeedCoin"),
        '    New MailAddress(email)
        ')
        'msg.Subject = subject
        'msg.Body = fcontent
        'msg.IsBodyHtml = True
        Dim msg = New SendGridMessage
        msg.SetFrom(New EmailAddress("noreply@deedcoin.net", "DeedCoin"))
        Dim recipients = New List(Of EmailAddress)
        msg.AddTo(New EmailAddress(email))
        msg.SetSubject(subject)
        msg.AddContent(MimeType.Html, fcontent)

        Dim client = New SendGridClient(Environment.GetEnvironmentVariable("EMAIL_API"))
        client.SendEmailAsync(msg)
    End Sub

    Public Shared Sub Notify(user As Integer, msg As String, link As String, img As String)
        Using db = New LiteDatabase(HttpContext.Current.Server.MapPath("~/App_Data/Database.accdb"))
            Dim notification = New Notification With {
                .Content = msg,
                .ImageURL = img,
                .Link = link,
                .Receiver = user,
                .NotificationDate = SystemClock.Instance.Now.Ticks,
                .Popped = False,
                .Seen = False
            }
            Dim msgTbl = db.GetCollection(Of Notification)("Notifications")
            msgTbl.Insert(notification)
            msgTbl.Update(notification)
        End Using
    End Sub
End Class