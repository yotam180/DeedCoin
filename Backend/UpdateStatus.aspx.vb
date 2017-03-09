Imports LiteDB

Partial Class Backend_UpdateStatus
    Inherits System.Web.UI.Page

    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim usr As User = UserUtils.CurrentLoggedInUser()
        If usr Is Nothing OrElse usr.UserLevel < UserType.Administrator Then
            Response.Write("ERROR UserType too low")
            Return
        End If
        Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsrs = db.GetCollection(Of User)("Users")
            Dim target As User = Nothing
            For Each u As User In tblUsrs.Find(Function(x) x.Username.Equals(Request.Form("user")))
                target = u
            Next
            If target Is Nothing Then
                Response.Write("ERROR User not found")
                Return
            End If
            If target.Id = usr.Id Then
                Response.Write("ERROR Cannot change own status")
                Return
            End If
            Try
                target.UserLevel = CType(Integer.Parse(Request.Form("level")), UserType)
            Catch
                Response.Write("ERROR Type not found")
                Return
            End Try
            tblUsrs.Update(target)
            Response.Write("OK")
        End Using
    End Sub
End Class
