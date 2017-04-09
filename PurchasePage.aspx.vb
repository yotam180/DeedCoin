Imports LiteDB
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
            Dim buyer = usrTbl.FindById(buy.Buyer)
            If gig.Type = GigType.OfferJob Then
                Response.Redirect("SalaryPage.aspx?deal=" & buy.Id)
            End If
            Dim offerer = usrTbl.FindById(New BsonValue(gig.Offerer))



            lblName.Text = gig.Title
            lblPrice.Text = gig.Price
            lblSeller.Text = String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> (@{3})", offerer.Username, offerer.FirstName, offerer.LastName, offerer.Username)
            lblBDate.Text = New Instant(usr.JoinDate).ToString()
            lblAmount.Text = buy.Transfer & " dC"
            lblBuyer.Text = String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> (@{3})", buyer.Username, buyer.FirstName, buyer.LastName, buyer.Username)
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
            If buy.HasBeenDelivered AndAlso buy.HasBeenPaid Then
                lblRating.Text = "Rating: <span style=""color: gold"">" & New String("★", buy.Rating) & "</span>"
            End If
        End Using
    End Sub

    Public Sub ApproveWork(sender As Object, e As EventArgs) Handles btnApprove.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim buy = buyTbl.FindById(Integer.Parse(Request.QueryString("deal")))
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                Dim gig = gigTbl.FindById(buy.Proposal)
                buy.HasBeenDelivered = True
                buyTbl.Update(buy)
                Notifier.Notify(buy.Buyer, String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> has finished working on <a href='GigPage.aspx?gig={3}'>{4}</a>. Go pay them now!", usr.Username, usr.FirstName, usr.LastName, gig.Id, gig.Title), "PurchasePage.aspx?deal=" & buy.Id, Null(gig.ImageURLs, New String() {"Images/profile.jpg"})(0))
                Response.Redirect("/PurchasePage.aspx?deal=" & Request.QueryString("deal"))
            Catch ex As Exception
                Utils.Alert("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Public Sub Pay(sender As Object, e As EventArgs) Handles btnPay.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim rate As Integer = 1
                Integer.TryParse(rating.SelectedValue, rate)
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim payer = usrTbl.FindById(New BsonValue(Session("UserID")))
                Dim buy = buyTbl.FindById(Integer.Parse(Request.QueryString("deal")))
                Dim gig = gigTbl.FindById(buy.Proposal)
                Dim seller = usrTbl.FindById(New BsonValue(gigTbl.FindById(buy.Proposal).Offerer))
                If payer.DeedCoins < buy.Transfer Then
                    IO.File.WriteAllText(Server.MapPath("App_Data/Log.txt"), "Not enough cash")
                    btnPay.Text = "<div style='width: 100%; text-align: center'>Not enough cash</div>"
                    Return
                End If
                payer.DeedCoins -= buy.Transfer
                payer.Exp += buy.Transfer
                seller.DeedCoins += buy.Transfer
                seller.Exp += buy.Transfer * rate
                usrTbl.Update(payer)
                usrTbl.Update(seller)
                buy.HasBeenPaid = True
                buy.Rating = rate
                buyTbl.Update(buy)
                Notifier.Notify(seller.Id, String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> has paid for your gig, <a href='GigPage.aspx?gig={3}'>{4}</a>", payer.Username, payer.FirstName, payer.LastName, gig.Id, gig.Title), "PurchasePage.aspx?deal=" & buy.Id, Null(gig.ImageURLs, New String() {"Images/profile.jpg"})(0))
                Response.Redirect("/PurchasePage.aspx?deal=" & Request.QueryString("deal"))
            Catch ex As Exception
                IO.File.WriteAllText(Server.MapPath("App_Data/Log.txt"), ex.Message)
                Utils.Alert("Error: " & ex.Message)
            End Try
        End Using
    End Sub

End Class
