
Partial Class Logout
    Inherits LoggedUsersOnly
    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        Session.Abandon()
        Response.Redirect(Request.UrlReferrer.ToString())
    End Sub
End Class
