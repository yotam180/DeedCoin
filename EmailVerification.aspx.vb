Imports LiteDB
Imports System.Drawing
Imports UserUtils

Partial Class EmailVerification
    Inherits LoggedNotVerifiedUsersOnly

    'Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
    '    VerifiedOnly(Response, Session, Server)
    'End Sub

    Public Sub btnVerify_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Try
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tbl.FindById(Integer.Parse(Session("UserID")))
                If user.VerificationCode.ToString() = txtCode.Text Then
                    user.EmailVerified = True
                    tbl.Update(user)
                    txtCode.BackColor = Color.PaleGreen
                    lblState.Visible = True
                    lblState.Text = "Great! Your email is verified. You can now navigate through the website freely."
                    ' Response.Write("<script>setTimeout(function(){location.href='/Default.aspx';}, 1000)</script>")
                Else
                    lblState.Visible = True
                    lblState.Text = "No... This doesn't seem correct..."
                    txtCode.BackColor = Color.PaleVioletRed
                End If
            End Using
        Catch ex As Exception
            txtCode.BackColor = Color.PaleVioletRed
        End Try
    End Sub

End Class
