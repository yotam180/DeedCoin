Imports System.Globalization
Imports LiteDB, NodaTime

Partial Class Conversation
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim msgTbl = db.GetCollection(Of Message)("Messages")

            Dim curuser = usrTbl.FindById(New BsonValue(Session("UserID")))
            Dim messengers = msgTbl.Find(Function(x) x.Sender = curuser.Id OrElse x.Reciever = curuser.Id).OrderByDescending(Function(x) x.WriteDate).Select(Function(x) Utils.Tenrary(x.Sender = curuser.Id, x.Reciever, x.Sender)).Distinct
            For Each msngr In messengers
                Dim pp = usrTbl.FindById(msngr)
                lblContacts.Text &= String.Format("<div style='width: 100%; height: 50px; border=1px'><img src=""{0}"" style='height: 100%; width: auto;'/><a href='Conversation.aspx?to={1}'><strong>{2} {3}</strong></a><br/>", Null(pp.ProfilePic, "~/Images/profile.jpg").Substring(2), pp.Id, pp.FirstName, pp.LastName)
            Next

            Dim usrId As Integer
            If Request.QueryString("to") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("to"), usrId) Then
                lblMsg.Text = "<strong>Error: bad query string</strong>"
                btnSend.Visible = False
                Return
            End If
            Dim usr = usrTbl.FindById(usrId)
            If usr Is Nothing Then
                lblMsg.Text = "<strong>Error: user not found</strong>"
                btnSend.Visible = False
                Return
            End If
            lblMsg.Text = ""
            lblContacts.Text = ""
            Dim msgs = msgTbl.Find(Function(x) (x.Sender = curuser.Id AndAlso x.Reciever = usr.Id) OrElse (x.Sender = usr.Id AndAlso x.Reciever = curuser.Id)).OrderByDescending(Function(x) x.WriteDate).Take(30).Reverse

            For Each msg In msgs
                Dim rec As User
                Dim sen As User
                If msg.Reciever = curuser.Id Then
                    rec = curuser
                    sen = usr
                Else
                    sen = curuser
                    rec = usr
                End If
                lblMsg.Text &= String.Format("<strong><a href='Profile.aspx?user={0}'>{1} {2}</a> says... <span style='font-size: smaller; color: gray'>{3}</span></strong><br/>{4}<hr/>", sen.Username, sen.FirstName, sen.LastName, New Instant(msg.WriteDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), msg.Content)
            Next
            lblName.Text = usr.FirstName & " " & usr.LastName
        End Using
    End Sub

    Public Sub SendMessage(sender As Object, e As EventArgs) Handles btnSend.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim msgTbl = db.GetCollection(Of Message)("Messages")
            Dim entry = New Message With {
                .Sender = Session("UserID"),
                .Reciever = Integer.Parse(Request.QueryString("to")),
                .Content = txtMsg.Text.Replace(Environment.NewLine, "<br/>"),
                .WriteDate = SystemClock.Instance.Now.Ticks
            }
            Dim usr = usrTbl.FindById(entry.Sender)
            msgTbl.Insert(entry)
            msgTbl.Update(entry)
            txtMsg.Text = ""
            Notifier.Notify(entry.Reciever, String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> has sent you a message", usr.Username, usr.FirstName, usr.LastName), "Conversation.aspx?to=" & entry.Sender, usr.ProfilePic)
            Response.Redirect("Conversation.aspx?to=" & Request.QueryString("to"))
        End Using
    End Sub
End Class
