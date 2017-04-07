﻿Imports LiteDB

Partial Class Hire_pg
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        If Request.RequestType = "GET" Then
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Try
                    Dim gig = db.GetCollection(Of JobProposal)("Proposals").FindById(Integer.Parse(Request.QueryString("job")))
                    lblName.Text = gig.Title
                    lblPrice.Text = gig.Price
                Catch ex As Exception
                    Response.Redirect("JobPage.aspx?job=" & Request.QueryString("job"))
                End Try
            End Using
            If Not Request.UrlReferrer Is Nothing Then
                ViewState("Referrer") = Request.UrlReferrer.ToString()
            End If
        End If

    End Sub

    Public Sub Cancel(sender As Object, e As EventArgs) Handles btnCancel.Click
        If ViewState("Referrer") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Response.Redirect(ViewState("Referrer"))
        End If
    End Sub

    Public Sub Buy(sender As Object, e As EventArgs) Handles btnBuy.Click
        Try
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                Dim gig = gigTbl.FindById(Integer.Parse(Request.QueryString("job")))
                Dim entry = New Purchase With {
                    .Buyer = usr.Id,
                    .HasBeenDelivered = False,
                    .HasBeenPaid = False,
                    .PaymentDate = 0,
                    .PurchaseDate = NodaTime.SystemClock.Instance.Now.Ticks,
                    .Rating = 0,
                    .Transfer = gig.Price,
                    .Proposal = gig.Id
                }
                buyTbl.Insert(entry)
                buyTbl.Update(entry)
                Response.Redirect("/SalaryPage.aspx?deal=" & entry.Id.ToString)
            End Using
        Catch ex As Exception
            Utils.Alert("Problem: " & ex.Message)
        End Try
    End Sub

End Class
