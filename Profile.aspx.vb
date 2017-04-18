Imports LiteDB
Imports NodaTime
Imports Markdig

Partial Class Profile
    Inherits Page


    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim user As User
        Dim username As String = Request.QueryString("user")
        If username = Nothing OrElse username = "" Then
            username = Session("Username")
        End If
        If username = Nothing OrElse username = "" Then
            username = "yotam180"
        End If
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers = db.GetCollection(Of User)("Users")
            Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim users As IEnumerable(Of User) = tblUsers.Find(Function(x) x.Username.Equals(username))
            If users.Count() = 0 Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            user = users.First() ' Found our user.
            '''''''''''''''''''''''''''''''''''''''''''
            If Session("UserID") <> Nothing AndAlso Session("UserID") <> user.LastProfileViewer Then
                user.ProfileViews += 1
                user.LastProfileViewer = Session("UserID")
                tblUsers.Update(user)
            End If

            Dim interactions = buyTbl.FindAll().Select(Function(x) New Tuple(Of Purchase, JobProposal)(x, gigTbl.FindById(x.Proposal))).Where(Function(x) x.Item2.Type = GigType.OfferProduct AndAlso x.Item2.Offerer = user.Id)
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
                rating = rating / inn
            End If

            Dim jobs = gigTbl.FindAll.Where(Function(x) Not x.Disabled AndAlso x.Type = GigType.OfferProduct AndAlso x.Offerer = user.Id)
            If jobs.Count = 0 Then
                lblGigs.Text = "This user has no gigs available"
            Else
                lblGigs.Text = "<table style='table-layout: fixed; width: 100%;' cellpadding='10px'>"
                For Each job In jobs
                    lblGigs.Text &= RowOf(job.ImageURLs(0), job.Title, job.ShortDescription, "JobPage.aspx?job=" & job.Id)
                Next
                lblGigs.Text &= "</table>"
            End If

            ''''''''''''''''''''''''''''''''''''''''''' Now let's do the cool stuff 8D
            lblFullName.Text = user.FirstName & " " & user.LastName
            If user.UserLevel > 2 Then
                lblFullName.Text = "<span style=""color: gold"">★</span> " & lblFullName.Text
            ElseIf user.UserLevel > 1 Then
                lblFullName.Text = "<span style=""color: royalblue"">♦</span> " & lblFullName.Text
            End If
            lblUsername.Text = user.Username
            lblEXP.Text = user.Exp.ToString()
            Dim pipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
            lblAbout.Text = Markdown.ToHtml(user.Description, pipeline)
            lblCountry.Text = user.Country
            lblProfileViews.Text = user.ProfileViews & " profile view" & Utils.Tenrary(user.ProfileViews = 1, "", "s")
            lblRating.Text = rating & "&nbsp;&nbsp; <span style=""color: gold"">" & New String("★", Math.Ceiling(rating))


            Dim memfor As Period = Period.Between(New Instant(user.JoinDate).InUtc().LocalDateTime, SystemClock.Instance.Now.InUtc().LocalDateTime, PeriodUnits.AllDateUnits Or PeriodUnits.AllTimeUnits)
            Dim memforstr As String = Utils.StringifyPeriod(memfor)
            If memforstr <> Nothing Then
                lblMemfor.Text = "Member for " & memforstr
            Else
                lblMemfor.Text = "Right now joined!"
            End If

            Dim lstlgn As Period = Period.Between(New Instant(user.LastLogin).InUtc().LocalDateTime, SystemClock.Instance.Now.InUtc().LocalDateTime, PeriodUnits.AllDateUnits Or PeriodUnits.AllTimeUnits)
            Dim lstlgnstr As String = Utils.StringifyPeriod(lstlgn)
            If lstlgnstr <> Nothing Then
                lblLastLogin.Text = "Last seen " & lstlgnstr & " ago"
            Else
                lblLastLogin.Text = "~Now online~"
            End If

            If user.ProfilePic <> Nothing Then
                Image1.ImageUrl = user.ProfilePic
            Else
                Image1.ImageUrl = "/Images/profile.jpg"
            End If
            Image1.Style.Add("border-radius", "20px")
            Image1.Style.Add("border", "3px solid gray")

            Dim curuser As User = Nothing
            If Session("UserID") <> Nothing Then
                curuser = tblUsers.FindById(Double.Parse(Session("UserID")))
            End If

            If Session("UserID") = user.Id OrElse (Not curuser Is Nothing AndAlso curuser.UserLevel > UserType.Regular) Then
                Image1.Style.Add("cursor", "pointer")
                btnEdit.Visible = True
                pnlUser.Visible = True
                lblCash.Text = user.DeedCoins
            Else
                Image1.Style.Remove("cursor")
                Image1.Style.Add("cursor", "default")
                btnEdit.Visible = False
                pnlCash.Visible = False
            End If
        End Using
    End Sub

    Public Sub Image_Click(sender As Object, e As EventArgs) Handles btnImage.Click
        Dim user As User
        Dim username As String = Request.QueryString("user")
        If username = Nothing OrElse username = "" Then
            username = Session("Username")
        End If
        If username = Nothing OrElse username = "" Then
            username = "yotam180"
        End If
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Dim users As IEnumerable(Of User) = tblUsers.Find(Function(x) x.Username.Equals(username))
            If users.Count() = 0 Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            user = users.First() ' Found our user.
            If Session("UserID") = user.Id Then
                Response.Redirect("/UploadImage.aspx?user=" & username)
            End If
        End Using
    End Sub

    Public Sub Edit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim user As User
        Dim username As String = Request.QueryString("user")
        If username = Nothing OrElse username = "" Then
            username = Session("Username")
        End If
        If username = Nothing OrElse username = "" Then
            username = "yotam180"
        End If
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Dim users As IEnumerable(Of User) = tblUsers.Find(Function(x) x.Username.Equals(username))
            If users.Count() = 0 Then
                pnlUser.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            user = users.First() ' Found our user.
            Dim curuser As User = tblUsers.FindById(Integer.Parse(Session("UserID")))

            If Session("UserID") = user.Id OrElse (Not curuser Is Nothing AndAlso curuser.UserLevel > UserType.Regular) Then
                Response.Redirect("/UpdateProfile.aspx?user=" & username)
            End If
        End Using
    End Sub

    Public Sub Message(sender As Object, e As EventArgs) Handles btnMessage.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim usr = usrTbl.FindOne(Function(x) x.Username.Equals(Request.QueryString("user")))
            Response.Redirect("Conversation.aspx?to=" & usr.Id)
        End Using
    End Sub

    Public Function RowOf(img As String, title As String, desc As String, href As String) As String
        Return String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;'/></td><td><a href='{1}'><h2 style='color: black'>{2}</h2></a><span style='color: darkgray'>{3}</span></td></tr>", img, href, title, desc)
    End Function

End Class
