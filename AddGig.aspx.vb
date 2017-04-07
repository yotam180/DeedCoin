Imports LiteDB


Partial Class AddGig
    Inherits VerifiedUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)
    End Sub

    ''' <summary>
    ''' This method adds a new gig to the database, based on the submitted form.
    ''' </summary>
    ''' <param name="sender">event sender</param>
    ''' <param name="e">event arguments (not used here)</param>
    Public Sub Add(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' TODO finish later...
        If txtGigName.Text.Length < 2 Then
            lblError.Text = "You must enter a gig name"
            lblError.Visible = True
            Return
        End If
        If txtDescription.Text.Length < 2 Then
            lblError.Text = "You must enter a description"
            lblError.Visible = True
            Return
        End If
        If txtImages.Text.Length < 2 Then
            lblError.Text = "You must enter at least 1 image url"
            lblError.Visible = True
            Return
        End If
        If txtPrice.Text.Length < 2 Then
            lblError.Text = "You must enter a price"
            lblError.Visible = True
            Return
        End If
        If txtShortDescription.Text.Length < 2 Then
            lblError.Text = "You must enter a short description"
            lblError.Visible = True
            Return
        End If

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usrTbl = db.GetCollection(Of User)("Users")
            If Session("UserID") Is Nothing Then
                lblError.Text = "Login problem"
                lblError.Visible = True
                Return
            End If
            Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
            Dim price As Double
            If Not Double.TryParse(txtPrice.Text, price) Then
                lblError.Text = "Price must be number-convertible"
                lblError.Visible = True
                Return
            End If
            Dim entry = New JobProposal With {
                .Description = txtDescription.Text,
                .ImageURLs = txtImages.Text.Split(New Char() {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries),
                .Offerer = usr.Id,
                .OffererOrg = Nothing,
                .Price = price,
                .Title = txtGigName.Text,
                .Type = GigType.OfferProduct,
                .VideoURL = txtVideoURL.Text,
                .ShortDescription = txtShortDescription.Text
            }
            gigTbl.Insert(entry)
            gigTbl.Update(entry)
            Response.Redirect("/GigPage.aspx?gig=" & entry.Id)
        End Using
    End Sub

End Class
