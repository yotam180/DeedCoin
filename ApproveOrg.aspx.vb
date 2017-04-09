Imports LiteDB

Partial Class ApproveOrg
    Inherits EliteAdministratorOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.RequestType = "GET" Then
            If Not Request.UrlReferrer Is Nothing Then
                ViewState("Referrer") = Request.UrlReferrer.ToString()
            End If
            If Request.QueryString("org") Is Nothing Then
                pnlMain.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            Dim orgId As Integer
            If Not Integer.TryParse(Request.QueryString("org"), orgId) Then
                pnlMain.Visible = False
                pnlNotFound.Visible = True
                Return
            End If

            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
                Dim org = orgTbl.FindById(orgId)
                If org Is Nothing Then
                    pnlMain.Visible = False
                    pnlNotFound.Visible = True
                    Return
                End If

                lblName.Text = org.OrganizationName
            End Using
        End If
    End Sub

    Public Sub Approve(sender As Object, e As EventArgs) Handles btnApprove.Click
        If Request.QueryString("org") Is Nothing Then
            pnlMain.Visible = False
            pnlNotFound.Visible = True
            Return
        End If

        Dim orgId As Integer
        If Not Integer.TryParse(Request.QueryString("org"), orgId) Then
            pnlMain.Visible = False
            pnlNotFound.Visible = True
            Return
        End If

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim orgTbl = db.GetCollection(Of Organization)("Organizations")
            Dim usrTbl = db.GetCollection(Of User)("Users")

            Dim org = orgTbl.FindById(orgId)
            If org Is Nothing Then
                pnlMain.Visible = False
                pnlNotFound.Visible = True
                Return
            End If
            Dim usr = usrTbl.FindById(org.OwnerID)
            Dim curusr = usrTbl.FindById(New BsonValue(Session("UserID")))

            org.Approved = True
            org.Rejected = False
            orgTbl.Update(org)
            Notifier.Notify(usr.Id, String.Format("Your organization <a href='OrganizationPage.aspx?org={0}'>{1}</a> has been approved by <a href='Profile.aspx?user={2}'>{3} {4}</a>", org.Id, org.OrganizationName, curusr.Username, curusr.FirstName, curusr.LastName), "OrganizationPage.aspx?org=" & org.Id, org.ImageLoc)
            If ViewState("Referrer") Is Nothing Then
                Response.Redirect("/Default.aspx")
            Else
                Response.Redirect(ViewState("Referrer"))
            End If
        End Using
    End Sub

    Public Sub Cancel(sender As Object, e As EventArgs) Handles btnCancel.Click
        If ViewState("Referrer") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Response.Redirect(ViewState("Referrer"))
        End If
    End Sub

End Class
