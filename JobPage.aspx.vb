﻿Imports System.Globalization
Imports LiteDB
Imports Markdig

Partial Class JobPage
    Inherits System.Web.UI.Page

    Public gid As String

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim cmtTbl = db.GetCollection(Of Comment)("Comments")

                Dim gigId As Integer
                If Request.QueryString("job") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("job"), gigId) Then
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
                gid = gig.Id
                lblShortDesc.Text = gig.ShortDescription
                Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
                lblAbout.Text = Markdown.ToHtml(gig.Description, pipeline)
                lblPurchases.Text = buyTbl.Find(Function(x) x.Proposal = gig.Id).Count
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

                If Session("UserID") Is Nothing Then
                    pnlAddComment.Visible = False
                End If

                If Session("UserID") Is Nothing OrElse (offerer.OwnerID <> Session("UserID") AndAlso usrTbl.FindById(New BsonValue(Session("UserID"))).UserLevel < UserType.Moderator) Then
                    btnEdit.Visible = False
                End If

                Dim comments = cmtTbl.Find(Function(x) x.ProposalId = gig.Id).OrderByDescending(Function(x) x.WriteDate)
                For Each cmt In comments
                    Dim writer = usrTbl.FindById(cmt.Writer)
                    lblComments.Text &= String.Format("<div style='display: inline-block; width: 80%; border: 3px solid gray; border-radius: 10px; padding: 10px; min-height: 50px; background-color: whitesmoke; text-align: left;'><h3><a href='{0}'>{1} {2}</a> says...&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size: smaller; color: gray'>{3}</span></h3>{4}</div>", writer.Username, writer.FirstName, writer.LastName, New NodaTime.Instant(cmt.WriteDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), cmt.Content)
                Next

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

