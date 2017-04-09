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
            Dim entry = New Comment With {
                .Content = txtContent.Text,
                .ProposalId = Integer.Parse(Request.QueryString("id")),
                .WriteDate = NodaTime.SystemClock.Instance.Now.Ticks,
                .Writer = Session("UserID")
            }
            cmtTbl.Insert(entry)
            cmtTbl.Update(entry)
            Response.Redirect("GigPage.aspx?gig=" & entry.ProposalId)
        End Using
    End Sub

End Class
