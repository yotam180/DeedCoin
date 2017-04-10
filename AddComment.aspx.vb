Imports LiteDB
Partial Class AddComment
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)
        Dim gigId As Integer
        If Request.QueryString("id") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("id"), gigId) Then
            Response.Redirect("Default.aspx")
        End If
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim gig = gigTbl.FindById(gigId)
            If gig Is Nothing Then
                Response.Redirect("GigPage.aspx?gig=" & gigId)
            End If
            lblName.Text = gig.Title
        End Using

    End Sub

    Public Sub AddComment(sender As Object, e As EventArgs) Handles btnSend.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_data/Database.accdb"))
            Dim cmtTbl = db.GetCollection(Of Comment)("Comments")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim entry = New Comment With {
                .Content = txtContent.Text.Replace(Environment.NewLine, "<br/>"),
                .ProposalId = Integer.Parse(Request.QueryString("id")),
                .WriteDate = NodaTime.SystemClock.Instance.Now.Ticks,
                .Writer = Session("UserID")
            }
            cmtTbl.Insert(entry)
            cmtTbl.Update(entry)
            Dim curusr = usrTbl.FindById(New BsonValue(Session("UserID")))
            Dim gig = gigTbl.FindById(entry.ProposalId)
            Dim usr = usrTbl.FindById(New BsonValue(gig.Offerer))
            If usr.Id <> curusr.Id Then
                Notifier.Notify(usr.Id, String.Format("<a href='Profile.aspx?user={0}'>{1} {2}</a> has commented on <a href='GigPage.aspx?gig={3}'>{4}</a>", curusr.Username, curusr.FirstName, curusr.LastName, gig.Id, gig.Title), "GigPage.aspx?gig=" & gig.Id & "#c_" & entry.Id, gig.ImageURLs(0))
            End If
            Response.Redirect("GigPage.aspx?gig=" & entry.ProposalId)
        End Using
    End Sub

End Class
