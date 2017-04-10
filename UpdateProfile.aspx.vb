Imports LiteDB
Imports System.Drawing


Partial Class UpdateProfile
    Inherits LoggedInUsersOnly

    Public Address As GeoPos

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" Then
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tblUsers.FindById(Integer.Parse(Session("UserID")))
                If user.UserLevel < UserType.Administrator Then
                    txtCoins.Enabled = False
                End If
                If Request.QueryString("user") <> Session("Usrname") Then
                    If user.UserLevel < 2 Then
                        Response.Redirect("/UpdateProfile.aspx")
                    Else
                        Dim us = user
                        user = tblUsers.FindOne(Function(x) x.Username.Equals(Request.QueryString("user")))
                        If user Is Nothing Then
                            Response.Redirect("/UpdateProfile.aspx")
                        End If
                        If user.UserLevel >= us.UserLevel Then
                            Response.Redirect("/UpdateProfile.aspx")
                        End If
                    End If
                End If
                txtUsername.Text = user.Username
                txtCoins.Text = user.DeedCoins
                txtUsername.Enabled = False
                txtFirstName.Text = user.FirstName
                txtLastName.Text = user.LastName
                txtEmail.Text = user.Email
                txtAbout.Text = user.Description
                txtAddress.Text = user.Location
                cbxEmail.Checked = user.EmailNotifications
            End Using
            Session("Address") = ""
        End If
    End Sub

    Public Sub Update_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Dim user As User = tblUsers.FindOne(Function(x) x.Username.Equals(txtUsername.Text))
            If user Is Nothing Then
                Return
            End If

            If txtPassword.Text <> "" Then
                If txtCurrentPassword.Text = "" Then
                    lblError.Text = "You must provide the current password in order to make changes to your password."
                    lblError.Visible = True
                    Return
                ElseIf txtCurrentPassword.Text <> user.Password Then
                    lblError.Text = "The current password is incorrect."
                    lblError.Visible = True
                    Return
                End If
                If txtPassword.Text = txtPasswordConfirm.Text Then
                    user.Password = txtPassword.Text
                Else
                    lblError.Text = "The password is different than password confirmation."
                    lblError.Visible = True
                    Return
                End If
            End If
            If txtFirstName.Text <> user.FirstName Then
                user.FirstName = txtFirstName.Text
            End If
            If txtLastName.Text <> user.LastName Then
                user.LastName = txtLastName.Text
            End If
            If txtEmail.Text <> user.Email Then
                user.Email = txtEmail.Text
            End If
            If txtAbout.Text <> user.Description Then
                user.Description = txtAbout.Text
            End If
            If txtAddress.Text <> user.Location AndAlso Session("Address") <> Nothing Then
                Address = Utils.GetGeoPos(txtAddress.Text)
                user.Position = Address
                user.Location = txtAddress.Text
                user.Country = Utils.ExtractCountry(user.Position.Address)
            End If
            user.EmailNotifications = cbxEmail.Checked
            Dim dc As Integer
            If Integer.TryParse(txtCoins.Text, dc) Then
                user.DeedCoins = dc
            End If
            tblUsers.Update(user)
            Response.Redirect("/Profile.aspx?user=" & txtUsername.Text)
        End Using
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
End Class
