Imports LiteDB

Partial Class RecoverPassword
    Inherits AnonymousUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)


    End Sub

    Public Sub Recover(sender As Object, e As EventArgs) Handles btnRecover.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            For Each usr In usrTbl.Find(Function(x) x.Email.Equals(txtEmail.Text))
                Dim notificationText = "<h2>Dear {name}!</h2>A password recovery request has been submitted for your email.<br/>If it wasn't you, just delete this message and continue.<br/>Your password: <b>{pass}</b>"
                Dim fnot = notificationText.Replace("{name}", usr.FirstName & usr.LastName).Replace("{pass}", usr.Password)
                Notifier.Notify(usr.Email, fnot, "DeedCoin password recovery")
            Next
        End Using
    End Sub

End Class
