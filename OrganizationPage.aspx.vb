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
            lblSince.Text = db.GetCollection(Of User)("Users").FindById(org.OwnerID).Username ' "Since - TBI" ' TODO: implement since when organization has been operating
            lblInteractions.Text = "Successful interactions - TBI" ' TODO: implement interactions & interaction counter
            Image1.ImageUrl = org.ImageLoc
        End Using

    End Sub

End Class
