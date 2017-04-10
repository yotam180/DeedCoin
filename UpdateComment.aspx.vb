Imports LiteDB

Partial Class UpdateComment
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)
        If Request.HttpMethod = "GET" Then
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim cmtTbl = db.GetCollection(Of Comment)("Comments")
                Dim cmtId As Integer
                If Request.QueryString("id") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("id"), cmtId) Then
                    lblName.Text = " << ERROR: comment not found >>"
                    btnSend.Visible = False
                    Return
                End If
                Dim cmt = cmtTbl.FindById(cmtId)
                If cmt Is Nothing Then
                    lblName.Text = " << ERROR: comment not found >>"
                    btnSend.Visible = False
                    Return
                End If
                If Session("UserID") <> cmt.Writer Then
                    lblName.Text = " << ERROR: You are not allowed to edit this comment >>"
                    btnSend.Visible = False
                    Return
                End If
                lblName.Text = "Edit your comment:"
                txtContent.Text = cmt.Content
            End Using
        End If
    End Sub

    Public Sub Edit(sender As Object, e As EventArgs) Handles btnSend.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim cmtTbl = db.GetCollection(Of Comment)("Comments")
            Dim cmtId As Integer
            If Request.QueryString("id") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("id"), cmtId) Then
                lblName.Text = " << ERROR: comment not found >>"
                btnSend.Visible = False
                Return
            End If
            Dim cmt = cmtTbl.FindById(cmtId)
            If cmt Is Nothing Then
                lblName.Text = " << ERROR: comment not found >>"
                btnSend.Visible = False
                Return
            End If
            If Session("UserID") <> cmt.Writer Then
                lblName.Text = " << ERROR: You are not allowed to edit this comment >>"
                btnSend.Visible = False
                Return
            End If
            cmt.Content = txtContent.Text.Replace(Environment.NewLine, "<br/>")
            cmtTbl.Update(cmt)
            Response.Redirect("GigPage.aspx?gig=" & cmt.ProposalId)
        End Using
    End Sub

End Class
