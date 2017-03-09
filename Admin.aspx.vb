Imports LiteDB
Imports System.Linq.Expressions
Partial Class Admin
    Inherits AdministratorOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" Then
            lblUsers.Text = GetUsers(1)
            txtPageNumUsr.Text = "1"
        End If
    End Sub

    Public Sub btnMUsers_Click(sender As Object, e As EventArgs) Handles btnMUsers.Click
        pnlUsers.Visible = True
        pnlOrganizations.Visible = False
        ' lblUsers.Text = GetUsers(1)
        txtPageNumUsr.Text = "1"
    End Sub

    Public Sub btnMAdmins_Click(sender As Object, e As EventArgs) Handles btnMAdmins.Click
        pnlUsers.Visible = False
        pnlOrganizations.Visible = False
    End Sub

    Public Sub btnMOrganizations_Click(sender As Object, e As EventArgs) Handles btnMOrganization.Click
        pnlUsers.Visible = False
        pnlOrganizations.Visible = True
        lblOrgs.Text = GetOrganizations(1)
    End Sub

    Public Sub btnNextUsr_Click(sender As Object, e As EventArgs) Handles btnNextUsr.Click
        Dim i As Integer
        If Integer.TryParse(txtPageNumUsr.Text, i) Then
            txtPageNumUsr.Text = (i + 1).ToString()
            lblUsers.Text = GetUsers(i + 1)
        End If
    End Sub

    Public Sub btnPrevUsr_Click(sender As Object, e As EventArgs) Handles btnPrevUsr.Click
        Dim i As Integer
        If Integer.TryParse(txtPageNumUsr.Text, i) Then
            txtPageNumUsr.Text = (Math.Max(1, i - 1)).ToString()
            lblUsers.Text = GetUsers(Math.Max(1, i - 1))
        End If
    End Sub

    Public Sub txtPageNumberUsr_TextChange(sender As Object, e As EventArgs) Handles txtPageNumUsr.TextChanged
        Dim i As Integer
        If Integer.TryParse(txtPageNumUsr.Text, i) Then
            txtPageNumUsr.Text = (Math.Max(1, i)).ToString()
            lblUsers.Text = GetUsers(Math.Max(1, i))
        End If
    End Sub

    Public Function GetOrganizations() As String
        Return Nothing
    End Function

    Public Function GetOrganizations(Optional pagenum As Integer = 1) As String
        Dim builder = New StringBuilder("<table id=""adminTblOrg"" style='width: 100%; table-layout: fixed; border: 2px solid gray;' border='0'>")
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb") + "; journal=false")
            ' db.DropCollection("Organizations")
            Dim tblOrg = db.GetCollection(Of Organization)("Organizations")
            Dim res = tblOrg.Find(Function(X) True)
            For Each org As Organization In res
                builder.Append("<tr><td style=""border: 1px dotted gray;""><a href=""/Organization.aspx?org=")
                builder.Append(org.OrganizationName)
                builder.Append(""">")
                builder.Append(org.OrganizationName)
                builder.Append("</a><br/><img src='")
                builder.Append(org.ImageLoc)
                builder.Append("' style='width: 50%; height: auto;'></img></td><td style=""border: 1px dotted gray;""><a href=""#"" onclick='approveOrg(")
                builder.Append(org.Id)
                builder.Append(""");'>Approve</a></td><td style=""border: 1px dotted gray;""><a href=""#"" onclick='rejectOrg(")
                builder.Append(org.Id)
                builder.Append(""");'>Reject</a></td></tr>")
            Next
            builder.Append("</table>")
        End Using
        Return builder.ToString()
    End Function

    Public Function GetUsers(Optional pagenum As Integer = 1, Optional exp As Expression(Of Func(Of User, Boolean)) = Nothing) As String
        Dim builder As StringBuilder = New StringBuilder("<table id=""adminTbl"" style='width: 100%; table-layout: fixed; border: 2px solid gray;' border='0'>")
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("App_Data/Database.accdb"))
            Dim tblUsr As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Dim res As IEnumerable(Of User)
            If exp Is Nothing Then
                res = tblUsr.Find(Function(x) True)
            Else
                res = tblUsr.Find(exp)
            End If
            For Each usr As User In res.Skip(Math.Max(0, (pagenum - 1) * 10)).Take(10)
                ' Username + full name column
                builder.Append("<tr><td style=""border: 1px dotted gray;"">")
                If usr.UserLevel > 2 Then
                    builder.Append("<span style=""color: gold"">★</span> ")
                ElseIf usr.UserLevel > 1 Then
                    builder.Append("<span style=""color: royalblue"">♦</span> ")
                End If
                builder.Append(usr.FirstName)
                builder.Append(" ")
                builder.Append(usr.LastName)
                builder.Append(" (<a href=""/Profile.aspx?user=")
                builder.Append(usr.Username)
                builder.Append(""">@")
                builder.Append(usr.Username)
                ' Profile editing column
                builder.Append("</a>)</td><td><a href=""/UpdateProfile.aspx?user=")
                builder.Append(usr.Username)
                ' Status update column
                builder.Append(""">Edit profile</a></td><td>")
                builder.Append(String.Format("<select onchange=""setMod('{3}')"" id=""{3}""><option value=""1"" {0}>Normal</option><option value=""2"" {1}>Editor</option><option value=""3"" {2}>Administrator</option></select></td></tr>" _
                                             , Utils.Tenrary(usr.UserLevel.Equals(UserType.Regular), "selected=""selected""", "") _
                                             , Utils.Tenrary(usr.UserLevel.Equals(UserType.Moderator), "selected=""selected""", "") _
                                             , Utils.Tenrary(usr.UserLevel.Equals(UserType.Administrator), "selected=""selected""", "") _
                                             , "mod_" & usr.Username
                                ))
                builder.Append("</td></tr>")
            Next
        End Using
        builder.Append("</table>")
        Return builder.ToString()
    End Function
End Class
