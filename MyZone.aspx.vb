Imports System.Globalization
Imports System.Linq.Expressions
Imports LiteDB

Partial Class MyZone
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim buyTbl = db.GetCollection(Of Purchase)("Purchases")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))

            Dim orgs = orgTbl.Find(Function(x) x.OwnerID.Equals(usr.Id))
            If orgs.Count = 0 Then
                lblOrgs.Text = "You have no organizations"
            Else
                lblOrgs.Text = "<table style='width: 100%; table-layout: fixed' border='1'>"
                For Each o In orgs
                    lblOrgs.Text &= String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td><a href='OrganizationPage.aspx?org={1}'><strong>{2}</strong></a></td></tr>", o.ImageLoc, o.Id, o.OrganizationName)
                Next
                lblOrgs.Text &= "</table>"
            End If

            Dim gigs = gigTbl.Find(Function(x) (Not x.Disabled) AndAlso x.Offerer.Equals(usr.Id))
            If gigs.Count = 0 Then
                lblGigs.Text = "You have no gigs"
            Else
                lblGigs.Text = "<table style='width: 100%; table-layout: fixed' border='1'>"
                For Each o In gigs
                    If o.Type = GigType.OfferProduct Then
                        lblGigs.Text &= String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td><a href='GigPage.aspx?gig={1}'><strong>{2}</strong></a></td></tr>", Null(o.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, o.Title)
                    Else
                        lblGigs.Text &= String.Format("<tr><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td><a href='JobPage.aspx?job={1}'><strong>{2}</strong></a> as <a href='OrganizationPage.aspx?org={3}'>{4}</a></td></tr>", Null(o.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, o.Title, o.OffererOrg, orgTbl.FindById(New BsonValue(o.OffererOrg)).OrganizationName)
                    End If
                Next
                lblGigs.Text &= "</table>"
            End If

            Dim orders = buyTbl.FindAll().Where(Function(x) FilterOrders(x, gigTbl, usr)).OrderBy(Function(x) x.HasBeenDelivered AndAlso x.HasBeenPaid).ThenByDescending(Function(x) x.PurchaseDate)
            If orders.Count = 0 Then
                lblOrders.Text = "You don't have any orders"
            Else
                lblOrders.Text = "<table style='width: 100%; table-layout: fixed' border='1'>"
                For Each o In orders
                    If o.Buyer = usr.Id Then
                        Dim gigo = gigTbl.FindById(o.Proposal)
                        Dim usro = usrTbl.FindById(New BsonValue(gigo.Offerer))
                        lblOrders.Text &= String.Format("<tr style='background-color: {7}'><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td style='text-align: left; font-size: smaller;'><span style='color: gray;'>0x{5} </span> You ordered <a href='PurchasePage.aspx?deal={1}'><strong>{2}</strong></a> from <a href='Profile.aspx?user={3}'><strong>{4}</strong></a> at {6} </td></tr>",
                                                        Null(gigo.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, gigo.Title, usro.Username, usro.FirstName & " " & usro.LastName, o.Id.ToString("X"), New NodaTime.Instant(o.PurchaseDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), Utils.Tenrary(o.HasBeenDelivered AndAlso o.HasBeenPaid, "lightgreen", "transparent"))
                    Else
                        Dim gigo = gigTbl.FindById(o.Proposal)
                        Dim usro = usrTbl.FindById(New BsonValue(o.Buyer))
                        lblOrders.Text &= String.Format("<tr style='background-color: {7}'><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td style='text-align: left; font-size: smaller;'><span style='color: gray;'>0x{5} </span> <a href='Profile.aspx?user={3}'><strong>{4}</strong></a> hired for <a href='PurchasePage.aspx?deal={1}'><strong>{2}</strong></a> at {6} </td></tr>",
                                                        Null(gigo.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, gigo.Title, usro.Username, usro.FirstName & " " & usro.LastName, o.Id.ToString("X"), New NodaTime.Instant(o.PurchaseDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), Utils.Tenrary(o.HasBeenDelivered AndAlso o.HasBeenPaid, "lightgreen", "yellow"))
                    End If
                Next
                lblOrders.Text &= "</table>"
            End If

            Dim jobs = buyTbl.FindAll().Where(Function(x) FilterJobs(x, gigTbl, usr)).OrderBy(Function(x) x.HasBeenPaid AndAlso x.HasBeenDelivered).ThenByDescending(Function(x) x.PurchaseDate)
            If jobs.Count = 0 Then
                lblJobs.Text = "You don't have any jobs"
            Else
                lblJobs.Text = "<table style='width: 100%; table-layout: fixed' border='1'>"
                For Each o In jobs
                    Dim gigo = gigTbl.FindById(o.Proposal)
                    If gigo.Type = GigType.OfferJob Then
                        Dim orgo = orgTbl.FindById(New BsonValue(gigo.OffererOrg))
                        lblJobs.Text &= String.Format("<tr style='background-color: {7}'><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td style='text-align: left; font-size: smaller;'><span style='color: gray;'>0x{5} </span> You hired to <a href='SalaryPage.aspx?deal={1}'><strong>{2}</strong></a> for <a href='OrganizationPage.aspx?org={3}'><strong>{4}</strong></a> at {6} </td></tr>",
                                                        Null(gigo.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, gigo.Title, orgo.Id, orgo.OrganizationName, o.Id.ToString("X"), New NodaTime.Instant(o.PurchaseDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), Utils.Tenrary(o.HasBeenDelivered AndAlso o.HasBeenPaid, "lightgreen", "transparent"))
                    Else
                        Dim usro = usrTbl.FindById(New BsonValue(o.Buyer))
                        lblJobs.Text &= String.Format("<tr style='background-color: {7}'><td style='width: 20%;'><img src='{0}' style='width: 100%; height: auto;' /></td><td style='text-align: left; font-size: smaller;'><span style='color: gray;'>0x{5} </span> <a href='Profile.aspx?user={3}'><strong>{4}</strong></a> ordered <a href='PurchasePage.aspx?deal={1}'><strong>{2}</strong></a> at {6} </td></tr>",
                                                        Null(gigo.ImageURLs, New String() {usr.ProfilePic})(0), o.Id, gigo.Title, usro.Username, usro.FirstName & " " & usro.LastName, o.Id.ToString("X"), New NodaTime.Instant(o.PurchaseDate).ToString("MM/dd/yyyy hh:mm", CultureInfo.CurrentCulture), Utils.Tenrary(o.HasBeenDelivered AndAlso o.HasBeenPaid, "lightgreen", "yellow"))
                    End If
                Next
                lblJobs.Text &= "</table>"
            End If
        End Using
    End Sub

    Public Function FilterOrders(x As Purchase, gigTbl As LiteCollection(Of JobProposal), usr As User) As Boolean
        Dim gig = gigTbl.FindById(x.Proposal)
        If gig.Type = GigType.OfferProduct AndAlso x.Buyer = usr.Id Then
            Return True
        End If
        If gig.Type = GigType.OfferJob AndAlso gig.Offerer = usr.Id Then
            Return True
        End If
        Return False
    End Function

    Public Function FilterJobs(x As Purchase, gigTbl As LiteCollection(Of JobProposal), usr As User) As Boolean
        Dim gig = gigTbl.FindById(x.Proposal)
        If gig.Type = GigType.OfferJob AndAlso x.Buyer = usr.Id Then
            Return True
        End If
        If gig.Type = GigType.OfferProduct AndAlso gig.Offerer = usr.Id Then
            Return True
        End If
        Return False
    End Function

End Class
