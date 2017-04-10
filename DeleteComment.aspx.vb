Imports LiteDB

Partial Class DeleteComment
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim cmtTbl = db.GetCollection(Of Comment)("Comments")
            Dim cmtId As Integer
            If Request.QueryString("id") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("id"), cmtId) Then
                Response.Redirect("Default.aspx")
            End If
            Dim cmt = cmtTbl.FindById(cmtId)
            If cmt Is Nothing Then
                Response.Redirect("Default.aspx")
            End If
            cmtTbl.Delete(Function(x) x.Id = cmt.Id)
            Response.Redirect("GigPage.aspx?gig=" & cmt.ProposalId)
        End Using

    End Sub

End Class
