Imports LiteDB
Imports NodaTime

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session.Timeout = 5760
        LoggedOutPanel.Visible = Session("UserID") = Nothing
        LoggedInPanel.Visible = Not LoggedOutPanel.Visible
        If LoggedInPanel.Visible Then
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tblUsers.FindById(Integer.Parse(Session("UserID")))
                If user Is Nothing Then
                    Response.Redirect("/Logout.aspx")
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

End Class

