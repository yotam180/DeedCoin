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
            Dim org = orgTbl.FindById(orgId)
            If org Is Nothing Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            lblAbout.Text = Markdown.ToHtml(org.Description, pipeline)
            lblAddress.Text = org.Position.Address
            lblCash.Text = org.MonthlyPoints
            lblEXP.Text = org.RequestMonthlyUsers
            lblOpeningHours.Text = "Opening hours - TBI" ' TODO: implement opening hours
            lblOrgName.Text = org.OrganizationName
            lblSince.Text = "Since - TBI" ' TODO: implement since when organization has been operating
            lblInteractions.Text = "Successful interactions - TBI" ' TODO: implement interactions & interaction counter
            Image1.ImageUrl = org.ImageLoc

            If org.Rejected Then
                pnlRejected.Visible = True
            ElseIf Not org.Approved Then
                pnlNotApproved.Visible = True
            End If

            Dim owner = db.GetCollection(Of User)("Users").FindById(org.OwnerID)

            If (Not org.Approved) AndAlso (Session("UserID") Is Nothing OrElse (Session("UserID") <> owner.Id AndAlso db.GetCollection(Of User)("Users").FindById(Session("UserID")).UserLevel < 2)) Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            If Not Session("UserID") Is Nothing AndAlso ((Not owner Is Nothing AndAlso Session("UserID") = owner.Id) OrElse db.GetCollection(Of User)("Users").FindById(Session("UserID")).UserLevel > 1) Then
                btnEdit.Visible = True
                If Not (org.Approved AndAlso org.Rejected) Then
                    pnlApprRej.Visible = True
                ElseIf org.Rejected Then
                    pnlReAppr.Visible = True
                End If
            Else
                btnEdit.Visible = False
            End If
        End Using

    End Sub

End Class
