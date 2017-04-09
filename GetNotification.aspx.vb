Imports LiteDB

Partial Class GetNotification
    Inherits System.Web.UI.Page
    Public Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Using db = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim usrTbl = db.GetCollection(Of User)("Users")
                Dim msgTbl = db.GetCollection(Of Notification)("Notifications")
                Dim usr = usrTbl.FindById(Integer.Parse(Null(Session("UserID"), "-1")))
                Dim msg = msgTbl.FindOne(Function(x) x.Receiver = usr.Id AndAlso Not x.Popped)
                If msg Is Nothing Then
                    Throw New Exception
                End If
                msg.Popped = True
                msgTbl.Update(msg)
                Response.Write(msg.Content)
            End Using
        Catch ex As Exception
            Response.Write("EMPTY")
        End Try

    End Sub
End Class
