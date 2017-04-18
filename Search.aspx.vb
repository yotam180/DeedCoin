Imports LiteDB

Partial Class Search
    Inherits System.Web.UI.Page

    Public Property c1 As String = "lightgray"
    Public Property c2 As String = "lightgray"
    Public Property c3 As String = "lightgray"
    Public Property c4 As String = "lightgray"

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.HttpMethod = "GET" Then
            Dim q = Request.QueryString("q")
            Dim cat = Request.QueryString("cat")
            txtSearch.Text = q
            Search(q, cat)
        End If
    End Sub

    Public Sub Search_click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Response.Redirect("Search.aspx?cat=" & Request.QueryString("cat") & "&q=" & txtSearch.Text)
    End Sub

    Public Sub bc1(sender As Object, e As EventArgs) Handles btnUsers.Click
        Response.Redirect("Search.aspx?cat=users&q=" & Null(Request.QueryString("q"), ""))
    End Sub

    Public Sub bc2(sender As Object, e As EventArgs) Handles btnGigs.Click
        Response.Redirect("Search.aspx?cat=gigs&q=" & Null(Request.QueryString("q"), ""))
    End Sub

    Public Sub bc3(sender As Object, e As EventArgs) Handles btnJobs.Click
        Response.Redirect("Search.aspx?cat=jobs&q=" & Null(Request.QueryString("q"), ""))
    End Sub

    Public Sub bc4(sender As Object, e As EventArgs) Handles btnOrgs.Click
        Response.Redirect("Search.aspx?cat=orgs&q=" & Null(Request.QueryString("q"), ""))
    End Sub

    Public Sub Search(q As String, cat As String)
        lblRes.Text = "<table style='table-layout: fixed; width: 100%;' cellpadding='10px'>"
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            If cat = "jobs" Then
                c4 = "skyblue"
                Dim tbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim src = tbl.FindAll().Where(Function(x) (Not x.Disabled) AndAlso x.Type = GigType.OfferJob)
                If q <> "" Then
                    src = src.Where(Function(x) Relevancy(q, String.Format("{0} {1} {2}", x.Title, x.ShortDescription, x.Description)) > 0).OrderByDescending(Function(x) Relevancy(q, String.Format("{0} {1} {2}", x.Title, x.ShortDescription, x.Description))).ThenBy(Function(x) x.Title)
                End If
                For Each res In src
                    lblRes.Text &= RowOf(Null(Null(res.ImageURLs, New String() {"~/Images/profile.jpg"})(0), "~/Images/profile.jpg"), res.Title & " (" & res.Price & " dC)", res.ShortDescription, "JobPage.aspx?job=" & res.Id)
                Next
            ElseIf cat = "gigs" Then
                c3 = "skyblue"
                Dim tbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim src = tbl.FindAll().Where(Function(x) (Not x.Disabled) AndAlso x.Type = GigType.OfferProduct)
                If q <> "" Then
                    src = src.Where(Function(x) Relevancy(q, String.Format("{0} {1} {2}", x.Title, x.ShortDescription, x.Description)) > 0).OrderByDescending(Function(x) Relevancy(q, String.Format("{0} {1} {2}", x.Title, x.ShortDescription, x.Description))).ThenBy(Function(x) x.Title)
                End If
                For Each res In src
                    lblRes.Text &= RowOf(Null(Null(res.ImageURLs, New String() {"~/Images/profile.jpg"})(0), "~/Images/profile.jpg"), res.Title & " (" & res.Price & " dC)", res.ShortDescription, "GigPage.aspx?gig=" & res.Id)
                Next
            ElseIf cat = "orgs" Then
                c2 = "skyblue"
                Dim tbl = db.GetCollection(Of Organization)("Organizations")
                Dim src = tbl.Find(Function(x) x.Approved)
                If q <> "" Then
                    src = src.Where(Function(x) Relevancy(q, String.Format("{0} {1}", x.OrganizationName, x.Description)) > 0).OrderByDescending(Function(x) Relevancy(q, String.Format("{0} {1}", x.OrganizationName, x.Description))).ThenBy(Function(x) x.OrganizationName)
                End If
                For Each res In src
                    lblRes.Text &= RowOf(Null(res.ImageLoc, "~/Images/profile.jpg"), res.OrganizationName, res.Description, "OrganizationPage.aspx?org=" & res.Id)
                Next
            Else
                c1 = "skyblue"
                Dim tbl = db.GetCollection(Of User)("Users")
                Dim src = tbl.FindAll()
                If q <> "" Then
                    src = src.Where(Function(x) Relevancy(q, String.Format("{0} {1} {2} {3}", x.Username, x.FirstName, x.LastName, x.Description)) > 0).OrderByDescending(Function(x) Relevancy(q, String.Format("{0} {1} {2} {3}", x.Username, x.FirstName, x.LastName, x.Description))).ThenBy(Function(x) x.Username)
                End If
                For Each res In src
                    lblRes.Text &= RowOf(Null(res.ProfilePic, "~/Images/profile.jpg").Substring(2), String.Format("{0} {1} <span style='font-size: smaller; color: gray'>(@{2})", res.FirstName, res.LastName, res.Username), res.Description, "Profile.aspx?user=" & res.Username)
                Next
            End If
        End Using
        If lblRes.Text = "<table style='table-layout: fixed; width: 100%;' cellpadding='10px'>" Then
            lblRes.Text = "<div style='width: 100%; text-align: center;'><h3>Sorry, your search query didn't return any results!</h3></div>"
        Else
            lblRes.Text &= "</table>"
        End If
    End Sub

    Public Function RowOf(img As String, title As String, desc As String, href As String) As String
        Return String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;'/></td><td><a href='{1}'><h2 style='color: black'>{2}</h2></a><span style='color: darkgray'>{3}</span></td></tr>", img, href, title, desc)
    End Function
End Class
