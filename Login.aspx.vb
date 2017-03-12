Imports UserUtils
Imports LiteDB
Imports NodaTime

Partial Class Login
    Inherits AnonymousUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" AndAlso Not (Request.UrlReferrer Is Nothing) Then
            ViewState("Referrer") = Request.UrlReferrer.ToString()
        End If
    End Sub

    Public Sub Login_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            For Each usr As User In tbl.Find(Function(x) x.Username.Equals(txtUsername.Text) AndAlso x.Password.Equals(txtPassword.Text))
                ' Found
                Session("UserID") = usr.Id
                Session("Username") = usr.Username
                usr.LastLogin = SystemClock.Instance.Now.Ticks
                tbl.Update(usr)
                If ViewState("Referrer") Is Nothing Then
                    Response.Redirect("/Default.aspx")
                Else
                    Response.Redirect(ViewState("Referrer").ToString())
                End If
                Return
            Next
            ' Not found
            lblState.Text = "Username or password are incorrect."
            lblState.Visible = True
        End Using
    End Sub

End Class
