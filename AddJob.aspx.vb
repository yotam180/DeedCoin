Imports LiteDB
Partial Class AddJob
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs)
        MyBase.Page_Load(sender, e)

        If Request.HttpMethod = "GET" Then
            DropDownList1.Items.Clear()
            DropDownList1.Items.Add(New ListItem("Select your organization", "-1"))
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
                Dim jobTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
                Dim orgs = orgTbl.Find(Function(x) x.Approved AndAlso x.OwnerID = usr.Id).OrderBy(Function(x) x.OrganizationName)
                For Each org As Organization In orgs
                    DropDownList1.Items.Add(New ListItem(org.OrganizationName, org.Id.ToString))
                Next
            End Using
        End If

    End Sub

    Public Sub OrgSelect(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim org = orgTbl.FindById(Integer.Parse(DropDownList1.SelectedValue))
            If org IsNot Nothing Then
                imgOrg.ImageUrl = org.ImageLoc
            End If
        End Using
    End Sub

    Public Sub Add(sender As Object, e As EventArgs) Handles btnAdd.Click
        ' TODO finish later...
        If txtGigName.Text.Length < 2 Then
            lblError.Text = "You must enter a job name"
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
            Dim orgId = Integer.Parse(DropDownList1.SelectedIndex)
            If orgId < 1 Then
                lblError.Text = "You must select an organization to proceed"
                lblError.Visible = True
                Return
            End If
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usrTbl = db.GetCollection(Of User)("Users")
            If Session("UserID") Is Nothing Then
                lblError.Text = "Login problem."
                lblError.Visible = True
                Return
            End If
            Dim usr = usrTbl.FindById(New BsonValue(Session("UserID")))
            Dim price As Double
            If Not Double.TryParse(txtPrice.Text, price) Then
                lblError.Text = "Price must be number-convertible"
                lblError.Visible = true
                Return
            End If
            Dim entry = New JobProposal With {
                .Description = txtDescription.Text,
                .ImageURLs = txtImages.Text.Split(New Char() {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries),
                .Offerer = usr.Id,
                .OffererOrg = orgId,
                .Price = price,
                .Title = txtGigName.Text,
                .Type = GigType.OfferJob,
                .VideoURL = txtVideoURL.Text,
                .ShortDescription = txtShortDescription.Text
            }
            gigTbl.Insert(entry)
            gigTbl.Update(entry)
            Response.Redirect("/JobPage.aspx?job=" & entry.Id)
        End Using
    End Sub

End Class
