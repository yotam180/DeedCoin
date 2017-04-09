Imports LiteDB
Imports NodaTime

Partial Class EditOrganization
    Inherits VerifiedUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)

        If Request.RequestType = "GET" Then
            If Request.QueryString("org") Is Nothing OrElse Session("UserID") Is Nothing Then
                Response.Redirect("/OrganizationPage.aspx?")
                Return
            End If
            Dim orgId As Integer
            If Not Integer.TryParse(Request.QueryString("org"), orgId) Then
                Response.Redirect("/OrganizationPage.aspx?")
                Return
            End If

            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
                Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")

                Dim org = orgTbl.FindById(orgId)
                If org Is Nothing Then
                    Response.Redirect("/OrganizationPage.aspx?org=" & orgId)
                    Return
                End If

                Dim interactions = buyTbl.FindAll().Select(Function(x) New Tuple(Of Purchase, JobProposal)(x, gigTbl.FindById(x.Proposal))).Where(Function(x) x.Item2.Type = GigType.OfferJob AndAlso x.Item2.OffererOrg = org.Id)


                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                If usr Is Nothing Then
                    Response.Redirect("/OrganizationPage.aspx?")
                    Return
                End If

                If usr.UserLevel < UserType.Moderator AndAlso org.OwnerID <> usr.Id Then
                    Response.Redirect("/OrganizationPage.aspx?")
                    Return
                End If

                If usr.UserLevel < UserType.Administrator Then
                    btnUpdate2.Visible = False
                    btnBlock.Visible = False
                End If

                txtOrgName.Text = org.OrganizationName
                txtMonthlyUsers.Text = org.RequestMonthlyUsers.ToString
                txtDescription.Text = org.Description
                txtAddress.Text = org.Address
                txtOpeningHours.Text = org.OpeningHours
                txtAudience.Text = org.Audience
                lblMonthlyCoins.Text = org.MonthlyPoints
                lblCoins.Text = org.Points
                lblSpent.Text = interactions.Where(Function(x) x.Item1.HasBeenPaid).Select(Function(x) x.Item1.Transfer).Sum.ToString & " dC"
                lblInteractions.Text = interactions.Count
                lblMonthlyInteractions.Text = interactions.Where(Function(x) New Instant(x.Item1.PurchaseDate).ToDateTimeUtc.Month = DateTime.Now.Month).Count.ToString
                txtMonthlyCoins.Text = org.MonthlyPoints
                txtCurrentBalance.Text = org.Points
            End Using
        End If
    End Sub

    Public Sub Submit(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim orgId = 0
            If (Not Request.QueryString("org") Is Nothing) AndAlso Integer.TryParse(Request.QueryString("org"), orgId) Then
                Dim org = orgTbl.FindById(orgId)
                If org Is Nothing Then
                    Utils.Alert("Organization not found. Error?")
                    Response.Redirect("/OrganizationPage.aspx?org=" & orgId)
                    Return
                End If

                Dim requestMonthlyUsers As Integer
                If Not Integer.TryParse(txtMonthlyUsers.Text, requestMonthlyUsers) Then
                    Utils.Alert("Monthly users must be a parsable number")
                    Response.Redirect("/OrganizationPage.aspx?org=" & orgId)
                    Return
                End If

                If org.Address <> txtAddress.Text Then
                    org.Address = txtAddress.Text
                    Dim gp = Utils.GetGeoPos(txtAddress.Text)
                    If Not gp Is Nothing Then
                        org.Position = gp
                    End If
                End If
                If org.OrganizationName <> txtOrgName.Text Then
                    org.OrganizationName = txtOrgName.Text
                End If
                If org.RequestMonthlyUsers <> requestMonthlyUsers Then
                    org.RequestMonthlyUsers = requestMonthlyUsers
                End If
                If org.Description <> txtDescription.Text Then
                    org.Description = txtDescription.Text
                End If
                If org.OpeningHours <> txtOpeningHours.Text Then
                    org.OpeningHours = txtOpeningHours.Text
                End If
                If org.Audience <> txtAudience.Text Then
                    org.Audience = txtAudience.Text
                End If
                orgTbl.Update(org)

                Response.Redirect("/OrganizationPage.aspx?org=" & orgId)

            Else
                Utils.Alert("Organization not found. Error?")
                Response.Redirect("/OrganizationPage.aspx")
                Return
            End If
        End Using
    End Sub

    Public Sub Administrate(sender As Object, e As EventArgs) Handles btnUpdate2.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Try
                Dim org = orgTbl.FindById(Integer.Parse(Request.QueryString("org")))
                If org Is Nothing Then
                    Utils.Alert("An error occured with the organization")
                End If
                org.Points = Integer.Parse(txtCurrentBalance.Text)
                org.MonthlyPoints = Integer.Parse(txtMonthlyCoins.Text)
                orgTbl.Update(org)
                Response.Redirect("/OrganizationPage.aspx?org=" & Request.QueryString("org"))
            Catch Exx As Exception
                Utils.Alert("An error occured: " & Exx.Message)
            End Try
        End Using
    End Sub

End Class
