Imports System.Drawing
Imports LiteDB
Imports System.IO

Partial Class AddOrganization
    Inherits VerifiedUsersOnly

    Public Property Address As GeoPos

    Public Sub PageLoad(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" Then
            Session("Address") = ""
        End If
    End Sub

    Public Function CheckOrganizationName(un As TextBox) As Boolean
        If un.Text.Length > 0 Then
            un.ToolTip = "Seems legit :)"
            Return True
        End If
        un.ToolTip = "You must enter a username"
        Return False
    End Function

    Public Function CheckMonthlyUsers(un As TextBox) As Boolean
        Dim isint As Boolean
        If Integer.TryParse(un.Text, isint) Then
            un.ToolTip = "Seems alright!"
            Return True
        End If
        un.ToolTip = "The number must be numeric"
        Return False
    End Function

    Public Sub txtOrgName_TextChanged(sender As Object, e As EventArgs) Handles txtOrgName.TextChanged
        If Not CheckOrganizationName(txtOrgName) Then
            txtOrgName.BackColor = Color.PaleVioletRed
        Else
            txtOrgName.BackColor = Color.PaleGreen
        End If
    End Sub

    Public Sub txtMUsers_TextChanged(sender As Object, e As EventArgs) Handles txtMonthlyUsers.TextChanged
        If CheckMonthlyUsers(txtMonthlyUsers) Then
            txtMonthlyUsers.BackColor = Color.PaleGreen
        Else
            txtMonthlyUsers.BackColor = Color.PaleVioletRed
        End If
    End Sub

    Public Sub txtAddress_TextChanged(Sender As Object, e As EventArgs) Handles txtAddress.TextChanged
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

    Public Sub Register(sender As Object, e As EventArgs) Handles btnRegister.Click
        ' Handling "Register" button click
        lblError.Text = String.Empty
        If Not CheckOrganizationName(txtOrgName) Then
            lblError.Text &= txtOrgName.ToolTip & "<br/>"
        End If
        If Not CheckMonthlyUsers(txtMonthlyUsers) Then
            lblError.Text &= txtMonthlyUsers.ToolTip & "<br/>"
        End If
        Address = Utils.GetGeoPos(txtAddress.Text)
        If Address Is Nothing Then
            lblError.Text &= "Address was not recognized<br/>"
        End If
        If txtDescription.Text.Length = 0 Then
            lblError.Text &= "You must enter an organization description<br/>"
        End If
        If Not fileImage.HasFile Then
            lblError.Text &= "You must upload an organization image/logo <br/>"
        End If

        If lblError.Text <> "" Then
            lblError.Visible = True
            Return
        End If

        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblOrg = db.GetCollection(Of Organization)("Organizations")
            Dim neworg As Organization = New Organization With
            {
                .Address = txtAddress.Text,
                .Position = Me.Address,
                .Approved = False,
                .Description = txtDescription.Text,
                .ImageLoc = Nothing,
                .MonthlyPoints = 0,
                .OrganizationName = txtOrgName.Text,
                .OwnerID = Session("UserID"),
                .Points = 0,
                .Rejected = False,
                .RequestMonthlyUsers = Integer.Parse(txtMonthlyUsers.Text)
            }
            tblOrg.Insert(neworg)
            tblOrg.Update(neworg)
            lblError.Text = "Organization ID : " & neworg.Id
            File.WriteAllBytes(Server.MapPath("~/UserContent/OrganizationImages/_" & neworg.Id & "_" & fileImage.FileName), fileImage.FileBytes)
            neworg.ImageLoc = "UserContent/OrganizationImages/_" & neworg.Id & "_" & fileImage.FileName
            tblOrg.Update(neworg)
            Response.Redirect("Default.aspx")
        End Using

    End Sub

End Class
