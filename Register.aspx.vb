Imports LiteDB
Imports System.Drawing
Imports System.Net
Imports System.Net.Mail
Imports System.IO
Imports System.Linq
Imports UserUtils
Imports NodaTime

Partial Class Register
    Inherits AnonymousUsersOnly

    Public Address As GeoPos = Nothing

    Public Sub PageLoad(sender As Object, e As EventArgs) Handles Me.Load
        If Request.RequestType = "GET" Then
            Session("Address") = ""
        End If
    End Sub

    Protected Function CheckUsername(un As TextBox) As Boolean
        If Not Regex.IsMatch(un.Text, "^[A-Za-z0-9_]+$") Then
            un.ToolTip = "Username can only contain letters, numbers and underscores."
            Return False
        ElseIf un.Text.Length < 6 Then
            un.ToolTip = "Username is too short! Must be at least 6 letters."
            Return False
        ElseIf un.Text.Length > 18 Then
            un.ToolTip = "Username is too long! Must be at most 18 letters."
            Return False
        End If
        un.ToolTip = "Username is good!"
        Return True
    End Function

    Protected Function CheckPassword(ps As TextBox) As Boolean
        If ps.Text.Length < 6 Then
            ps.ToolTip = "Password is too short! Must be at least 6 letters."
            Return False
        ElseIf ps.Text.Length > 30 Then
            ps.ToolTip = "Password is too long! Must be at most 18 letters."
            Return False
        ElseIf ps.Text.Contains(txtUsername.Text) AndAlso txtUsername.Text <> "" Then
            ps.ToolTip = "Password can't contain the username."
            Return False
        ElseIf Not Regex.IsMatch(ps.Text, "[0-9]") Then
            ps.ToolTip = "Password must contain at least 1 digit."
            Return False
        ElseIf Not Regex.IsMatch(ps.Text, "[A-Z]") Then
            ps.ToolTip = "Password must contain at least 1 uppercase letter."
            Return False
        ElseIf Not Regex.IsMatch(ps.Text, "[a-z]") Then
            ps.ToolTip = "Password must contain at least 1 lowercase letter."
            Return False
        End If
        ps.ToolTip = "Cool, password looks ok!"
        Return True
    End Function

    Protected Function CheckConfirmPassword(cps As TextBox) As Boolean
        If cps.Text = txtPassword.Text Then
            cps.ToolTip = "Yep Shmep, passwords are the same."
            Return True
        Else
            cps.ToolTip = "Confirmation is not the same as the password"
            Return False
        End If
    End Function

    Protected Shared Function IsEmail(ByVal email As String) As Boolean
        Return Regex.IsMatch(email, "\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase)
    End Function

    Protected Function CheckEmail(em As TextBox) As Boolean
        If IsEmail(em.Text) Then
            em.ToolTip = "Email looks fine."
            Return True
        Else
            em.ToolTip = "This doesn't look like a proper email..."
            Return False
        End If
    End Function

    Public Function ProcessEmailMessage(ByVal msg As String, usr As User) As String
        Return msg.Replace("$username", usr.Username) _
                  .Replace("$password", usr.Password) _
                  .Replace("$email", usr.Email) _
                  .Replace("$firstname", usr.FirstName) _
                  .Replace("$lastname", usr.LastName) _
                  .Replace("$location", usr.Location) _
                  .Replace("$address", usr.Position.Address) _
                  .Replace("$vercode", usr.VerificationCode)
    End Function

    Protected Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        If txtUsername.Text = "" Then
            txtUsername.BackColor = Color.LightYellow
        ElseIf CheckUsername(txtUsername) Then
            txtUsername.BackColor = Color.PaleGreen
        Else
            txtUsername.BackColor = Color.PaleVioletRed
        End If
    End Sub

    Protected Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        If txtPassword.Text = "" Then
            txtPassword.BackColor = Color.LightYellow
        ElseIf CheckPassword(txtPassword) Then
            txtPassword.BackColor = Color.PaleGreen
        Else
            txtPassword.BackColor = Color.PaleVioletRed
        End If
    End Sub

    Protected Sub txtPasswordConfirm_TextChanged(sender As Object, e As EventArgs) Handles txtPasswordConfirm.TextChanged
        If txtPasswordConfirm.Text = "" Then
            txtPasswordConfirm.BackColor = Color.LightYellow
        ElseIf CheckConfirmPassword(txtPasswordConfirm) AndAlso CheckPassword(txtPassword) Then
            txtPasswordConfirm.BackColor = Color.PaleGreen
        Else
            txtPasswordConfirm.BackColor = Color.PaleVioletRed
        End If
    End Sub

    Protected Sub txtEmail_TextChanged(sender As Object, e As EventArgs) Handles txtEmail.TextChanged
        If txtEmail.Text = "" Then
            txtEmail.BackColor = Color.LightYellow
        ElseIf CheckEmail(txtEmail) Then
            txtEmail.BackColor = Color.PaleGreen
        Else
            txtEmail.BackColor = Color.PaleVioletRed
        End If
    End Sub

    Protected Sub txtAddress_TextChanged(Sender As Object, e As EventArgs) Handles txtAddress.TextChanged
        txtAddress.BackColor = Color.Black
        Address = Utils.GetGeoPos(txtAddress.Text)
        If Address Is Nothing Then
            Session("Address") = Nothing
            txtAddress.BackColor = Color.PaleVioletRed
            lblAddress.Text = "No suitable address found."
            lblAddress.Visible = True
        Else
            Session("Address") = Address.Address
            txtAddress.BackColor = Color.PaleGreen
            Dim country As String = Utils.ExtractCountry(Address.Address)
            lblAddress.Text = "Address found: <b>" & Address.Address & "</b> (" & Address.Lat & "; " & Address.Lon & ") - country: " & country
            lblAddress.Visible = True
        End If
    End Sub

    Protected Sub txtFirstName_TextChanged(sender As Object, e As EventArgs) Handles txtFirstName.TextChanged
        If txtFirstName.Text.Length < 1 Then
            txtFirstName.BackColor = Color.LightYellow
        ElseIf txtFirstName.Text.Contains("★") Then
            txtFirstName.BackColor = Color.PaleVioletRed
            txtFirstName.ToolTip = "Name cannot contain a star."
        Else

            txtFirstName.BackColor = Color.PaleGreen
        End If
    End Sub

    Protected Sub txtLastName_TextChanged(sender As Object, e As EventArgs) Handles txtLastName.TextChanged
        If txtLastName.Text.Length < 1 Then
            txtLastName.BackColor = Color.LightYellow
        Else
            txtLastName.BackColor = Color.PaleGreen
        End If
    End Sub

    Protected Sub Register(sender As Object, e As EventArgs) Handles btnRegister.Click
        lblError.Text = ""
        If Not CheckUsername(txtUsername) Then
            lblError.Text &= txtUsername.ToolTip & "<br/>"
        End If
        If Not CheckPassword(txtPassword) Then
            lblError.Text &= txtPassword.ToolTip & "<br/>"
        ElseIf Not CheckConfirmPassword(txtPasswordConfirm) Then
            lblError.Text &= txtPasswordConfirm.ToolTip & "<br/>"
        End If
        If Not CheckEmail(txtEmail) Then
            lblError.Text &= txtEmail.ToolTip & "<br/>"
        End If
        If txtAbout.Text.Length < 30 Then
            lblError.Text &= "About text length must be at least 30 chars.<br/>"
        End If
        If txtFirstName.Text.Length < 1 Then
            lblError.Text &= "You must provide your first name.<br/>"
        ElseIf txtFirstName.Text Like "*★*" Then
            lblError.Text &= txtFirstName.ToolTip & "<br/>"
        End If
        If txtLastName.Text.Length < 1 Then
            lblError.Text &= "You must provide your last name.<br/>"
        End If
        If Session("Address") Is Nothing OrElse Session("Address") = "" Then
            lblError.Text &= "The address you entered could not be recognized.<br/>"
        End If

        If lblError.Text.Length < 1 Then
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim users As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim duplicheck As IEnumerable(Of User) = users.Find(Function(x) x.Username.Equals(txtUsername.Text))
                If duplicheck.Count() = 1 Then
                    lblError.Text = "Username is already taken."
                    lblError.Visible = True
                    txtUsername.BackColor = Color.PaleVioletRed
                    Return
                End If

                Dim geopos As GeoPos = Utils.GetGeoPos(txtAddress.Text)
                Dim newUser As Global.User = New User With
                {
                    .Username = txtUsername.Text,
                    .Password = txtPassword.Text,
                    .DeedCoins = 0,
                    .Email = txtEmail.Text,
                    .Description = txtAbout.Text,
                    .Exp = 0,
                    .FirstName = txtFirstName.Text,
                    .LastName = txtLastName.Text,
                    .JoinDate = SystemClock.Instance.Now.Ticks,
                    .LastLogin = SystemClock.Instance.Now.Ticks,
                    .Location = txtAddress.Text,
                    .Position = geopos,
                    .ProfilePic = Nothing,
                    .EmailVerified = False,
                    .VerificationCode = New Random().Next(1000, 10000),
                    .Country = Utils.ExtractCountry(geopos.Address),
                    .LastProfileViewer = 0,
                    .ProfileViews = 0,
                    .UserLevel = UserType.Regular
                }
                users.Insert(newUser)
                users.Update(newUser) ' Necessary? 

                If newUser.Id = 1 Then
                    newUser.UserLevel = UserType.Administrator
                    users.Update(newUser)
                End If


                'Dim smtp As SmtpClient = New SmtpClient("smtp.gmail.com", 587)
                'smtp.EnableSsl = True
                'smtp.DeliveryMethod = SmtpDeliveryMethod.Network
                'smtp.UseDefaultCredentials = False
                'smtp.Credentials = New NetworkCredential("rodicarogozin@gmail.com", "ROroDIca")
                'Dim msg As MailMessage = New MailMessage(
                '    New MailAddress("rodicarogozin@gmail.com", "DeedCoin"),
                '    New MailAddress(txtEmail.Text)
                ')
                'msg.Subject = "Welcome to DeedCoin!"
                'msg.Body = ProcessEmailMessage(File.ReadAllText(MapPath("App_Data/EmailVerification.txt")), newUser)
                'msg.IsBodyHtml = True
                'smtp.Send(msg)
                Notifier.Notify(newUser.Email, ProcessEmailMessage(File.ReadAllText(MapPath("App_Data/EmailVerification.txt")), newUser), "Welcome to DeedCoin!")


                Session("Username") = newUser.Username
                Session("UserID") = newUser.Id
                Response.Redirect("/EmailVerification.aspx")
            End Using
        Else
            lblError.Visible = True
            txtPassword.Text = ""
            txtPasswordConfirm.Text = ""
        End If
    End Sub
End Class
