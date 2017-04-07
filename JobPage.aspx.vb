Imports LiteDB
Imports Markdig

Partial Class JobPage
    Inherits System.Web.UI.Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim orgTbl = db.GetCollection(Of Organization)("Organizations")

                Dim gigId As Integer
                If Request.QueryString("job") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("job"), gigId) Then
                    pnlNotFound.Visible = True
                    pnlUser.Visible = False
                    Return
                End If

                Dim gig = gigTbl.FindById(gigId)
                If gig Is Nothing Then
                    pnlNotFound.Visible = True
                    pnlUser.Visible = False
                    Return
                End If
                If gig.Type = GigType.OfferProduct Then
                    Response.Redirect("GigPage.aspx?gig=" & gig.Id)
                End If

                Dim offerer = orgTbl.FindById(Utils.Tenrary(gig.OffererOrg Is Nothing, 0, CType(gig.OffererOrg, Integer)))

                lblFullName.Text = gig.Title
                lblSeller.Text = "<a href='/OrganizationPage.aspx?org=" & offerer.Id & "'>" & offerer.OrganizationName & "</a> <span style='color: gray; font-size: small;'>(" & offerer.Position.Address & ")</span>"
                If gig.ImageURLs IsNot Nothing AndAlso gig.ImageURLs.Length > 0 Then
                    Image1.ImageUrl = gig.ImageURLs(0)
                Else
                    Image1.ImageUrl = offerer.ImageLoc
                End If
                lblShortDesc.Text = gig.ShortDescription
                Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
                lblAbout.Text = Markdown.ToHtml(gig.Description, pipeline)
                lblPurchases.Text = "Not yet implemented"
                Dim res As String = ""
                For i As Integer = 0 To gig.ImageURLs.Length - 1
                    res &= "<img src='" & gig.ImageURLs(i) & "' /><br/>"
                Next
                lblImages.Text = res
                lblPrice.Text = gig.Price
                lblPriceShow.Text = gig.Price

                If Session("UserID") Is Nothing OrElse Session("UserID") = offerer.OwnerID OrElse offerer.Points < gig.Price Then
                    btnBuy.Visible = False
                End If

                If Session("UserID") Is Nothing OrElse (offerer.OwnerID <> Session("UserID") AndAlso usrTbl.FindById(New BsonValue(Session("UserID"))).UserLevel < UserType.Administrator) Then
                    btnEdit.Visible = False
                End If

            Catch Exx As Exception
                Utils.Alert("Problem with loading page.")
                Return
            End Try
        End Using
    End Sub

    Public Sub Edit(sender As Object, e As EventArgs) Handles btnEdit.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim gig = gigTbl.FindById(Integer.Parse(Request.QueryString("job")))
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                If usr.Id <> gig.Offerer AndAlso usr.UserLevel < UserType.Administrator Then
                    Return
                End If
                Response.Redirect("/EditJob.aspx?job=" & Request.QueryString("job"))
            Catch ex As Exception
                Utils.Alert("Problem while performing operation.")
            End Try
        End Using
    End Sub

    Public Sub Buy(sender As Object, e As EventArgs) Handles btnBuy.Click
        Response.Redirect("Hire.aspx?job=" & Request.QueryString("job"))
    End Sub

End Class

