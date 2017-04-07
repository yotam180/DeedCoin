Imports LiteDB
Imports System.Linq.Expressions
Delegate Sub XX(s As String)
Partial Class Admin
    Inherits AdministratorOnly

    Public Function SortType(sort As String) As Tuple(Of String, Boolean)
        Return New Tuple(Of String, Boolean)(sort.Replace("*", ""), sort.Contains("*"))
    End Function

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" Then
            lblUsers.Text = GetUsers(1)
            txtPageNumUsr.Text = "1"
            ViewState("OrgDisp") = "Queue"
        End If
    End Sub

    Public Sub btnMUsers_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMUsers.Click
        pnlUsers.Visible = True
        pnlOrganizations.Visible = False
        lblUsers.Text = GetUsers(1)
        txtPageNumUsr.Text = "1"
    End Sub

    Public Sub OrgAll(sender As Object, e As EventArgs) Handles btnAllOrg.Click
        Dim prs = SortType(radioSort.SelectedValue)
        lblOrgs.Text = GetOrganizations(Nothing, prs.Item1, prs.Item2, 1)
        ViewState("OrgDisp") = "All"
    End Sub

    Public Sub OrgQueue(sender As Object, e As EventArgs) Handles btnOrgQueue.Click
        lblOrgs.Text = GetOrganizationsQueue(1)
        ViewState("OrgDisp") = "Queue"
    End Sub

    Public Sub OrgApproved(sender As Object, e As EventArgs) Handles btnActiveOrg.Click
        Dim prs = SortType(radioSort.SelectedValue)
        lblOrgs.Text = GetOrganizations(Function(x) x.Approved, prs.Item1, prs.Item2, 1)
        ViewState("OrgDisp") = "Approved"
    End Sub

    Public Sub OrgRejected(sender As Object, e As EventArgs) Handles btnRejectedOrg.Click
        Dim prs = SortType(radioSort.SelectedValue)
        lblOrgs.Text = GetOrganizations(Function(x) x.Rejected, prs.Item1, prs.Item2, 1)
        ViewState("OrgDisp") = "Rejected"
    End Sub

    Public Sub btnMAdmins_Click(sender As Object, e As EventArgs) Handles btnMAdmins.Click
        pnlUsers.Visible = False
        pnlOrganizations.Visible = False
    End Sub

    Public Sub btnMOrganizations_Click(sender As Object, e As EventArgs) Handles btnMOrganization.Click
        pnlUsers.Visible = False
        pnlOrganizations.Visible = True
        lblOrgs.Text = GetOrganizationsQueue(1)
        ViewState("OrgDisp") = "Queue"
    End Sub

    Public Sub btnNextUsr_Click(sender As Object, e As EventArgs) Handles btnNextUsr.Click
        Dim i As Integer
        If Integer.TryParse(txtPageNumUsr.Text, i) Then
            txtPageNumUsr.Text = (i + 1).ToString()
            lblUsers.Text = GetUsers(i + 1)
        End If
    End Sub

    Public Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim i As Integer
        If Integer.TryParse(txtPageNum.Text, i) Then
            Dim prs = SortType(radioSort.SelectedValue)
            txtPageNum.Text = (i + 1).ToString()
            If ViewState("OrgDisp") = "Queue" Then
                lblOrgs.Text = GetOrganizationsQueue(Math.Max(1, i + 1))
            ElseIf ViewState("OrgDisp") = "Approved" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Approved, prs.Item1, prs.Item2, Math.Max(1, i + 1))
            ElseIf ViewState("OrgDisp") = "Rejected" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Rejected, prs.Item1, prs.Item2, Math.Max(1, i + 1))
            Else
                lblOrgs.Text = GetOrganizations(Nothing, radioSort.SelectedValue, Math.Max(1, i + 1))
            End If
        End If
    End Sub

    Public Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        Dim i As Integer
        If Integer.TryParse(txtPageNum.Text, i) Then
            Dim prs = SortType(radioSort.SelectedValue)
            txtPageNum.Text = (Math.Max(1, i - 1)).ToString()
            If ViewState("OrgDisp") = "Queue" Then
                lblOrgs.Text = GetOrganizationsQueue(Math.Max(1, i - 1))
            ElseIf ViewState("OrgDisp") = "Approved" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Approved, prs.Item1, prs.Item2, Math.Max(1, i - 1))
            ElseIf ViewState("OrgDisp") = "Rejected" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Rejected, prs.Item1, prs.Item2, Math.Max(1, i - 1))
            Else
                lblOrgs.Text = GetOrganizations(Nothing, prs.Item1, prs.Item2, Math.Max(1, i - 1))
            End If
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

    Public Sub txtPageNumber_TextChange(sender As Object, e As EventArgs) Handles txtPageNum.TextChanged
        Dim i As Integer
        If Integer.TryParse(txtPageNumUsr.Text, i) Then
            Dim prs = SortType(radioSort.SelectedValue)
            txtPageNumUsr.Text = (Math.Max(1, i)).ToString()
            If ViewState("OrgDisp") = "Queue" Then
                lblOrgs.Text = GetOrganizationsQueue(Math.Max(1, i))
            ElseIf ViewState("OrgDisp") = "Approved" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Approved, prs.Item1, prs.Item2, Math.Max(1, i))
            ElseIf ViewState("OrgDisp") = "Rejected" Then
                lblOrgs.Text = GetOrganizations(Function(x) x.Rejected, prs.Item1, prs.Item2, Math.Max(1, i))
            Else
                lblOrgs.Text = GetOrganizations(Nothing, prs.Item1, prs.Item2, Math.Max(1, i))
            End If
        End If
    End Sub

    Public Sub RadioChanged(sender As Object, e As EventArgs) Handles radioSort.SelectedIndexChanged
        txtPageNumber_TextChange(sender, e)
    End Sub

    Public Function GetOrganizations(Optional filter As Expression(Of Func(Of Organization, Boolean)) = Nothing, Optional sorting As String = Nothing, Optional reverse As Boolean = False, Optional pagenum As Integer = 1) As String
        Dim b = New StringBuilder("<table id=""adminTblOrg"" style='width: 100%; table-layout: fixed; border: 2px solid gray;' border='0'>")
        Dim p As XX = AddressOf b.Append
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tbl = db.GetCollection(Of Organization)("Organizations")
            Dim fltr = Utils.Tenrary(Of Expression(Of Func(Of Organization, Boolean)))(filter Is Nothing, Function(x) True, filter)
            Dim res = tbl.Find(fltr)
            If Not sorting Is Nothing Then
                If reverse Then
                    res = res.OrderByDescending(Function(x) x.GetType().GetProperty(sorting).GetValue(x, Nothing)).ThenBy(Function(x) x.OrganizationName)
                Else
                    res = res.OrderBy(Function(x) x.GetType().GetProperty(sorting).GetValue(x, Nothing)).ThenBy(Function(x) x.OrganizationName)
                End If
            Else
                    res = res.OrderBy(Function(x) x.OrganizationName)
            End If
            For Each org In res.Skip(10 * Math.Max(0, pagenum - 1)).Take(10)
                p("<tr><td><a href=""/OrganizationPage.aspx?org=")
                p(org.Id)
                p(""" style=""color: ")
                If org.Approved Then
                    p("green;")
                ElseIf org.Rejected Then
                    p("red;")
                Else
                    p("gray;")
                End If
                p(""">")
                p(org.OrganizationName)
                p("</a><br/><img src=""")
                p(org.ImageLoc)
                p(""" style='width: 50%; height: auto;' />")
                p("</td><td>Owner: <b>")
                Dim owner = db.GetCollection(Of User)("Users").FindById(org.OwnerID)
                If owner Is Nothing Then
                    p("--Not found--")
                Else
                    p(owner.FirstName & " " & owner.LastName & "</b> <a href='/Profile.aspx?user=" & owner.Username & "'><span style='color: gray;'>(@" & owner.Username & ")</span></a>")
                End If
                p("</td><td>Monthly dC: <b>")
                p(org.MonthlyPoints)
                p("</b>")
                p("</td></tr>")

            Next
            p("</table>")
            Return b.ToString()
        End Using
        Return Nothing
    End Function

    Public Function GetOrganizationsQueue(Optional pagenum As Integer = 1) As String
        Dim builder = New StringBuilder("<table id=""adminTblOrg"" style='width: 100%; table-layout: fixed; border: 2px solid gray;' border='0'>")
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            ' db.DropCollection("Organizations")
            Dim tblOrg = db.GetCollection(Of Organization)("Organizations")
            Dim res = tblOrg.Find(Function(X) (X.Approved = False AndAlso X.Rejected = False)).Skip(10 * (Math.Max(0, pagenum - 1))).Take(10)
            For Each org As Organization In res
                builder.Append("<tr><td style=""border: 1px dotted gray;""><a href=""/OrganizationPage.aspx?org=")
                builder.Append(org.Id)
                builder.Append(""">")
                builder.Append(org.OrganizationName)
                builder.Append("</a><br/><img src='")
                builder.Append(org.ImageLoc)
                builder.Append("' style='width: 50%; height: auto;'></img></td><td style=""border: 1px dotted gray;""><a href=""#"" onclick='appr(""")
                builder.Append(org.Id)
                builder.Append(""");'>Approve</a></td><td style=""border: 1px dotted gray;""><a href=""#"" onclick='rej(""")
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
            For Each usr As User In res.OrderBy(Function(x) 3 - x.UserLevel).ThenBy(Function(x) x.Username).Skip(Math.Max(0, (pagenum - 1) * 10)).Take(10)
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
