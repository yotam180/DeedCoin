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

            Dim interactions = buyTbl.FindAll().Select(Function(x) New Tuple(Of Purchase, JobProposal)(x, gigTbl.FindById(x.Proposal))).Where(Function(x) x.Item2.Type = GigType.OfferJob AndAlso x.Item2.OffererOrg = org.Id)
            Dim interactionsNum = interactions.Where(Function(x) x.Item1.HasBeenDelivered AndAlso x.Item1.HasBeenPaid).Count
            Dim rating As Decimal = 0.0
            For Each i In interactions
                If i.Item1.Rating < 1 Then
                    interactionsNum -= 1
                End If
                rating += i.Item1.Rating
            Next
            rating /= interactionsNum

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

            If Not Session("UserID") Is Nothing AndAlso (db.GetCollection(Of User)("Users").FindById(New BsonValue(Session("UserID"))).UserLevel > 1) Then
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

End Class
