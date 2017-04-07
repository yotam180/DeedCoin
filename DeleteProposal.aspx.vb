Imports LiteDB

Partial Class DeleteProposal
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        ddlRemove.Items.Clear()
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim usrTbl = db.GetCollection(Of User)("Users")
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
            If Request.QueryString("gig") IsNot Nothing Then
                For Each a As JobProposal In gigTbl.Find(Function(x) x.Offerer.Equals(usr.Id) AndAlso x.Type.Equals(GigType.OfferProduct) AndAlso Not x.Disabled)
                    ddlRemove.Items.Add(New ListItem(a.Title, a.Id.ToString))
                Next
            Else
                For Each a As JobProposal In gigTbl.Find(Function(x) x.Offerer.Equals(usr.Id) AndAlso x.Type.Equals(GigType.OfferJob) AndAlso Not x.Disabled)
                    ddlRemove.Items.Add(New ListItem(a.Title, a.Id.ToString))
                Next
            End If
        End Using

    End Sub

    Public Sub Delete(sender As Object, e As EventArgs) Handles btnDelete.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim gig = gigTbl.FindById(Integer.Parse(ddlRemove.SelectedValue))
            gig.Disabled = True
            Dim id = gig.Id
            gigTbl.Update(gig)
            Response.Redirect("GigPage.aspx?gig=" & id)
        End Using
    End Sub

End Class
