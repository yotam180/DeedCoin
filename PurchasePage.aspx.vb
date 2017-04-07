﻿Imports LiteDB
Imports NodaTime

Partial Class PurchasePage
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)
        If Session("UserID") Is Nothing Then
            Response.Redirect("Default.aspx")
            Return
        End If
        Dim dealId As Integer
        If Request.QueryString("deal") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("deal"), dealId) Then
            Response.Redirect("Default.aspx")
            Return
        End If
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")

            Dim buy = buyTbl.FindById(dealId)
            Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
            Dim gig = gigTbl.FindById(buy.Proposal)
            Dim offerer = usrTbl.FindById(New BsonValue(gig.Offerer))


            lblName.Text = gig.Title
            lblPrice.Text = gig.Price
            lblSeller.Text = String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> (@{3})", offerer.Username, offerer.FirstName, offerer.LastName, offerer.Username)
            lblBDate.Text = New Instant(usr.JoinDate).ToString()
            lblAmount.Text = buy.Transfer & " dC"
            lblBuyer.Text = String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> (@{3})", usr.Username, usr.FirstName, usr.LastName, usr.Username)
            If gig.ImageURLs Is Nothing OrElse gig.ImageURLs.Length < 1 Then
                imgPrc.ImageUrl = offerer.ProfilePic
            Else
                imgPrc.ImageUrl = gig.ImageURLs(0)
            End If
            lblWorkDone.Text = Utils.Tenrary(buy.HasBeenDelivered, "Yes", "No")
            lblPaid.Text = Utils.Tenrary(buy.HasBeenPaid, "Yes, at" & New Instant(buy.PaymentDate).ToString, "No")

            pnlApprovePayment.Visible = False
            pnlApproveWork.Visible = False
            If Not buy.HasBeenDelivered AndAlso usr.Id = offerer.Id Then
                pnlApproveWork.Visible = True
            End If
            If buy.HasBeenDelivered AndAlso Not buy.HasBeenPaid AndAlso usr.Id = buy.Buyer Then
                pnlApprovePayment.Visible = True
            End If
        End Using
    End Sub

    Public Sub ApproveWork(sender As Object, e As EventArgs) Handles btnApprove.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim buy = buyTbl.FindById(Integer.Parse(Request.QueryString("deal")))
                buy.HasBeenDelivered = True
                buyTbl.Update(buy)
                Response.Redirect("/PurchasePage.aspx?deal=" & Request.QueryString("deal"))
            Catch ex As Exception
                Utils.Alert("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub Pay(sender As Object, e As EventArgs) Handles btnPay.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim payer = usrTbl.FindById(New BsonValue(Session("UserID")))
                Dim buy = buyTbl.FindById(Integer.Parse(Request.QueryString("deal")))
                Dim seller = usrTbl.FindById(New BsonValue(gigTbl.FindById(buy.Proposal).Offerer))
                If payer.DeedCoins < buy.Transfer Then
                    IO.File.WriteAllText(Server.MapPath("App_Data/Log.txt"), "Not enough cash")
                    btnPay.Text = "<div style='width: 100%; text-align: center'>Not enough cash</div>"
                    Return
                End If
                payer.DeedCoins -= buy.Transfer
                seller.DeedCoins += buy.Transfer
                usrTbl.Update(payer)
                usrTbl.Update(seller)
                buy.HasBeenPaid = True
                buyTbl.Update(buy)
                Response.Redirect("/PurchasePage.aspx?deal=" & Request.QueryString("deal"))
            Catch ex As Exception
                IO.File.WriteAllText(Server.MapPath("App_Data/Log.txt"), ex.Message)
                Utils.Alert("Error: " & ex.Message)
            End Try
        End Using
    End Sub

End Class