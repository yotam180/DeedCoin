﻿Imports LiteDB
Partial Class EditGig
    Inherits LoggedInUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        If Request.HttpMethod = "POST" Then
            Return
        End If

        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
            Dim usrTbl = db.GetCollection(Of User)("Users")

            Dim gigId As Integer
            If Request.QueryString("gig") Is Nothing OrElse Not Integer.TryParse(Request.QueryString("gig"), gigId) Then
                Response.Redirect("/GigPage.aspx?")
                Return
            End If
            Dim gig = gigTbl.FindById(gigId)
            If gig Is Nothing Then
                Response.Redirect("/GigPage.aspx?gig=" & gigId)
                Return
            End If

            Dim userId As Integer
            If Session("UserID") Is Nothing Or Not Integer.TryParse(Session("UserID"), userId) Then
                Response.Redirect("/GigPage.aspx?")
            End If
            Dim usr = usrTbl.FindById(userId)
            If usr Is Nothing OrElse (usr.Id <> gig.Offerer AndAlso usr.UserLevel < UserType.Moderator) Then
                Response.Redirect("/GigPage.aspx?")
            End If

            txtDescription.Text = gig.Description
            txtImages.Text = String.Join("\n", gig.ImageURLs)
            txtPrice.Text = gig.Price
            txtShortDescription.Text = gig.ShortDescription
            txtTitle.Text = gig.Title
            txtVideo.Text = gig.VideoURL
        End Using

    End Sub

    Public Sub Update(sender As Object, e As EventArgs) Handles btnRegister.Click
        Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Try
                Dim gigTbl = db.GetCollection(Of JobProposal)("Proposals")
                Dim gig = gigTbl.FindById(Integer.Parse(Request.QueryString("gig")))
                gig.Description = txtDescription.Text
                gig.ImageURLs = txtImages.Text.Split(Environment.NewLine)
                IO.File.WriteAllText(Server.MapPath("~/App_Data/Log.txt"), txtImages.Text)
                gig.Price = Integer.Parse(txtPrice.Text)
                gig.ShortDescription = txtShortDescription.Text
                gig.VideoURL = txtVideo.Text
                gig.Title = txtTitle.Text
                gigTbl.Update(gig)
                Response.Redirect("/GigPage.aspx?gig=" & Request.QueryString("gig"))
            Catch ex As Exception
                Utils.Alert("Error while performing action: " & ex.Message)
                Return
            End Try
        End Using
    End Sub

End Class
