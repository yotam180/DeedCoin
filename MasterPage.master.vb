﻿Imports LiteDB
Imports NodaTime

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Public notNum As String = ""
    Public noti As String = "empty"

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Timeout = 5760
        LoggedOutPanel.Visible = Session("UserID") = Nothing
        LoggedInPanel.Visible = Not LoggedOutPanel.Visible
        pnlBurger.Visible = LoggedInPanel.Visible
        pnlNotif.Visible = pnlBurger.Visible
        If LoggedInPanel.Visible Then
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim tblNots = db.GetCollection(Of Notification)("Notifications")
                Dim user As User = tblUsers.FindById(Integer.Parse(Session("UserID")))
                If user Is Nothing Then
                    Response.Redirect("/Logout.aspx")
                End If
                Dim nn = tblNots.Find(Function(x) x.Receiver = user.Id AndAlso Not x.Seen).Count
                If nn > 0 Then
                    noti = "bold"
                    notNum = "(" & nn & ") "
                End If
                Label1.Text = "<u>" & user.Username & "</u>"
                user.LastLogin = SystemClock.Instance.Now.Ticks
                tblUsers.Update(user)
                If user.UserLevel > 1 Then
                    pnlSidebarAdmin.Visible = True
                End If
            End Using
        End If
    End Sub

    Public Sub Find(sender As Object, e As EventArgs) Handles INPUT_8.TextChanged
        Response.Redirect("Search.aspx?cat=gigs&q=" & INPUT_8.Text)
    End Sub

End Class

