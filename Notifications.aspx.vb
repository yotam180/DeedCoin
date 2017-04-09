Imports LiteDB

Partial Class Notifications
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)


        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim msgTbl = db.GetCollection(Of Notification)("Notifications")
            Dim notifications = msgTbl.FindAll().Where(Function(x) x.Receiver.Equals(Session("UserID"))).OrderByDescending(Function(x) x.NotificationDate).Take(30)
            Dim res = "<table style='width: 100%; table-layout: fixed;' border='0'>"
            For Each noti In notifications
                res &= String.Format("<tr><td style='width: 60px;'><img src='{1}' style='width: 100%; height: auto;' /></td><td style='height: 100%; text-align: left;'><div style='display: inline-block; position: relative; width: 100%; height: 100%; background-color: {3}; cursor: pointer;' onclick='location.href=""{0}"";'>{2}</div></td></tr>", noti.Link, noti.ImageURL, noti.Content, Utils.Tenrary(noti.Seen, "whitesmoke", "lightcyan"))
                noti.Seen = True
                noti.Popped = True
                msgTbl.Update(noti)
            Next
            res &= "</table>"
            lblNotifications.Text = res
        End Using

    End Sub

End Class
