Imports LiteDB
Imports Markdig

Partial Class GigPage
    Inherits System.Web.UI.Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim gigId As Integer
                If Request.QueryString("gig") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("gig"), gigId) Then
                    pnlNotFound.Visible = True
                    pnlUser.Visible = False
                    Return
                End If

                Dim gig = gigTbl.FindById(gigId)
                If gig Is Nothing OrElse gig.Disabled Then
                    pnlNotFound.Visible = True
                    pnlUser.Visible = False
                    Return
                End If
                If gig.Type = GigType.OfferJob Then
                    Response.Redirect("JobPage.aspx?job=" & gig.Id)
                End If

                Dim offerer = usrTbl.FindById(Utils.Tenrary(gig.Offerer Is Nothing, 0, CType(gig.Offerer, Integer)))

                lblFullName.Text = gig.Title
                lblSeller.Text = "<a href='/Profile.aspx?user=" & offerer.Username & "'>" & offerer.FirstName & " " & offerer.LastName & "</a> <span style='color: gray; font-size: small;'>(@" & offerer.Username & ")</span>"
                If gig.ImageURLs IsNot Nothing AndAlso gig.ImageURLs.Length > 0 Then
                    Image1.ImageUrl = gig.ImageURLs(0)
                Else
                    Image1.ImageUrl = offerer.ProfilePic
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

                If Session("UserID") Is Nothing OrElse Session("UserID") = offerer.Id Then
                    btnBuy.Visible = False
                End If

                If Session("UserID") Is Nothing OrElse (offerer.Id <> Session("UserID") AndAlso usrTbl.FindById(New BsonValue(Session("UserID"))).UserLevel < UserType.Administrator) Then
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
                Dim gig = gigTbl.FindById(Integer.Parse(Request.QueryString("gig")))
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                If usr.Id <> gig.Offerer AndAlso usr.UserLevel < UserType.Administrator Then
                    Return
                End If
                Response.Redirect("/EditGig.aspx?gig=" & Request.QueryString("gig"))
            Catch ex As Exception
                Utils.Alert("Problem while performing operation.")
            End Try
        End Using
    End Sub

    Public Sub Buy(sender As Object, e As EventArgs) Handles btnBuy.Click
        Response.Redirect("Buy.aspx?gig=" & Request.QueryString("gig"))
    End Sub

End Class
