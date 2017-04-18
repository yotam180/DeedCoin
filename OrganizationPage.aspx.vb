Imports LiteDB
Imports Markdig

Partial Class OrganizationPage
    Inherits Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim orgName = Request.QueryString("org")
        If orgName Is Nothing Then
            pnlUser.Visible = False
            pnlNotFound.Visible = True
            Return
        End If
        Dim orgId As Integer
        If Not Integer.TryParse(orgName, orgId) Then
            pnlUser.Visible = False
            pnlNotFound.Visible = True
            Return
        End If

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim org = orgTbl.FindById(orgId)
            If org Is Nothing Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            If DateTime.Now.Year <> org.LastUpdatedYear OrElse DateTime.Now.Month <> org.LastUpdatedMonth Then
                org.Points = org.MonthlyPoints + 1
                org.LastUpdatedMonth = DateTime.Now.Month
                org.LastUpdatedYear = DateTime.Now.Year
                orgTbl.Update(org)
            End If

            Dim jobs = gigTbl.FindAll.Where(Function(x) Not x.Disabled AndAlso x.OffererOrg IsNot Nothing AndAlso x.OffererOrg = org.Id)
            If jobs.Count = 0 Then
                lblJobs.Text = "This organization has no jobs available"
            Else
                lblJobs.Text = "<table style='table-layout: fixed; width: 100%;' cellpadding='10px'>"
                For Each job In jobs
                    lblJobs.Text &= RowOf(job.ImageURLs(0), job.Title, job.ShortDescription, "JobPage.aspx?job=" & job.Id)
                Next
                lblJobs.Text &= "</table>"
            End If

            Dim interactions = buyTbl.FindAll().Select(Function(x) New Tuple(Of Purchase, JobProposal)(x, gigTbl.FindById(x.Proposal))).Where(Function(x) x.Item2.Type = GigType.OfferJob AndAlso x.Item2.OffererOrg = org.Id)
            Dim interactionsNum = interactions.Where(Function(x) x.Item1.HasBeenDelivered AndAlso x.Item1.HasBeenPaid).Count
            Dim inn = interactions.Count
            Dim rating As Decimal = 0.0
            For Each i In interactions
                If i.Item1.Rating < 1 Then
                    inn -= 1
                End If
                rating += i.Item1.Rating
            Next
            If inn = 0 Then
                rating = 0
            Else
                rating /= interactionsNum
            End If

            Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            lblAbout.Text = Markdown.ToHtml(org.Description, pipeline)
            lblAddress.Text = org.Position.Address
            lblCash.Text = org.MonthlyPoints
            lblEXP.Text = org.RequestMonthlyUsers
            lblOrgName.Text = org.OrganizationName
            lblInteractions.Text = "Successful interactions - " & interactionsNum ' TODO: implement interactions & interaction counter
            lblOpeningHours.Text = "Opened:" & org.OpeningHours
            lblAudience.Text = "For: " & org.Audience
            lblRating.Text = rating & "&nbsp;&nbsp; <span style=""color: gold"">" & New String("★", Math.Ceiling(rating))
            Image1.ImageUrl = org.ImageLoc

            If org.Rejected Then
                pnlRejected.Visible = True
            ElseIf Not org.Approved Then
                pnlNotApproved.Visible = True
            End If

            Dim owner = db.GetCollection(Of User)("Users").FindById(org.OwnerID)

            If (Not org.Approved) AndAlso (Session("UserID") Is Nothing OrElse (Session("UserID") <> owner.Id AndAlso db.GetCollection(Of User)("Users").FindById(New BsonValue(Session("UserID"))).UserLevel < 2)) Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            If Not Session("UserID") Is Nothing AndAlso ((Not owner Is Nothing AndAlso Session("UserID") = owner.Id) OrElse db.GetCollection(Of User)("Users").FindById(New BsonValue(Session("UserID"))).UserLevel > 1) Then
                btnEdit.Visible = True
            Else
                btnEdit.Visible = False
            End If

            If Not Session("UserID") Is Nothing AndAlso (db.GetCollection(Of User)("Users").FindById(New BsonValue(Session("UserID"))).UserLevel > UserType.Regular) Then
                If Not (org.Approved OrElse org.Rejected) Then
                    pnlApprRej.Visible = True
                ElseIf org.Rejected Then
                    pnlReAppr.Visible = True
                End If
            End If

        End Using

    End Sub

    Public Sub Edit(ByVal sender As Object, ByVal e As EventArgs) Handles btnEdit.Click
        Dim orgId As Integer
        If Request.QueryString("org") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("org"), orgId) Then
            Utils.Alert("Something went wrong2... ")
            Return
        End If
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Dim tblOrgs = db.GetCollection(Of Organization)("Organizations")
            Dim org = tblOrgs.FindById(orgId)
            Dim curuser As User = tblUsers.FindById(Integer.Parse(Session("UserID")))

            If curuser Is Nothing Or org Is Nothing Then
                Utils.Alert("Something went wrong... ")
                Return
            End If

            If Session("UserID") = org.OwnerID OrElse curuser.UserLevel > UserType.Regular Then
                Response.Redirect("/EditOrganization.aspx?org=" & orgId)
            Else
                Utils.Alert("You don't have the permission to edit this organization.")
            End If
        End Using
    End Sub

    Public Sub Message(sender As Object, e As EventArgs) Handles btnMsg.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgName = Request.QueryString("org")
            If orgName Is Nothing Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            Dim orgId As Integer
            If Not Integer.TryParse(orgName, orgId) Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim org = orgTbl.FindById(orgId)
            If org Is Nothing Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            Response.Redirect("Conversation.aspx?to=" & org.OwnerID)
        End Using
    End Sub

    Public Function RowOf(img As String, title As String, desc As String, href As String) As String
        Return String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;'/></td><td><a href='{1}'><h2 style='color: black'>{2}</h2></a><span style='color: darkgray'>{3}</span></td></tr>", img, href, title, desc)
    End Function

End Class
