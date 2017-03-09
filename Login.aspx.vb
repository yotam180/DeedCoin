Imports UserUtils
Imports LiteDB
Imports NodaTime

Partial Class Login
    Inherits AnonymousUsersOnly

    Public Sub Login_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            For Each usr As User In tbl.Find(Function(x) x.Username.Equals(txtUsername.Text) AndAlso x.Password.Equals(txtPassword.Text))
                ' Found
                Session("UserID") = usr.Id
                Session("Username") = usr.Username
                usr.LastLogin = SystemClock.Instance.Now.Ticks
                tbl.Update(usr)
                Response.Redirect("/Default.aspx")
                Return
            Next
            ' Not found
            lblState.Text = "Username or password are incorrect."
            lblState.Visible = True
        End Using
    End Sub

End Class
