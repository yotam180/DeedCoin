Imports LiteDB
Imports System.IO

Partial Class UploadImage
    Inherits VerifiedUsersOnly

    Public Overrides Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyBase.Page_Load(sender, e)
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers = db.GetCollection(Of User)("Users")
            Dim user As User = tblUsers.FindById(Integer.Parse(Session("UserID").ToString()))
            If user.ProfilePic <> Nothing Then
                Image1.ImageUrl = user.ProfilePic
            Else
                Image1.ImageUrl = "/Images/profile.jpg"
            End If
            Image1.Style.Add("border-radius", "20px")
            Image1.Style.Add("border", "3px solid gray")
        End Using
    End Sub

    Public Sub Upload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If Session("UserID") <> Nothing Then
            Dim filename As String = "~/UserContent/ProfileImages/_" & Session("UserID").ToString() & "_" & FileUpload1.FileName
            Using fs As FileStream = New FileStream(Server.MapPath(filename), System.IO.FileMode.OpenOrCreate)
                FileUpload1.FileContent.CopyTo(fs)
            End Using
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tblUsers = db.GetCollection(Of User)("Users")
                Dim user As User = tblUsers.FindById(Integer.Parse(Session("UserID").ToString()))
                user.ProfilePic = filename
                tblUsers.Update(user)
            End Using
            Response.Redirect("/Profile.aspx")
        End If
    End Sub

End Class
