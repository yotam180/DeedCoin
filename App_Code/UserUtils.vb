Imports Microsoft.VisualBasic
Imports LiteDB

Public Class UserUtils

    Public Shared Function CurrentLoggedInUser() As User
        If HttpContext.Current.Session("UserID") Is Nothing Then
            Return Nothing
        End If
        Using db As LiteDatabase = New LiteDatabase(HttpContext.Current.Server.MapPath("~/App_Data/Database.accdb"))
            Dim tblUsers As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
            Return tblUsers.FindById(Integer.Parse(HttpContext.Current.Session("UserID")))
        End Using
    End Function

    Public Shared Sub LoggedInOnly(Response As HttpResponse, Session As HttpSessionState)
        If Session("UserID") Is Nothing Then
            Response.Redirect("/Login.aspx")
        End If
    End Sub

    Public Shared Sub VerifiedOnly(Response As HttpResponse, Session As HttpSessionState, Server As HttpServerUtility)
        If Session("UserID") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tbl.FindById(Integer.Parse(Session("UserID")))
                If Not user.EmailVerified Then
                    Response.Redirect("/EmailVerification.aspx")
                End If
            End Using
        End If
    End Sub

    Public Shared Sub AdminOnly(Response As HttpResponse, Session As HttpSessionState, Server As HttpServerUtility)
        If Session("UserID") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tbl.FindById(Integer.Parse(Session("UserID")))
                If user.UserLevel < 2 Then
                    Response.Redirect("/Default.aspx")
                End If
            End Using
        End If
    End Sub

    Public Shared Sub EliteAdminOnly(Response As HttpResponse, Session As HttpSessionState, Server As HttpServerUtility)
        If Session("UserID") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tbl.FindById(Integer.Parse(Session("UserID")))
                If user.UserLevel < 3 Then
                    Response.Redirect("/Default.aspx")
                End If
            End Using
        End If
    End Sub

    Public Shared Sub LoggedNotVerifiedOnly(Response As HttpResponse, Session As HttpSessionState, Server As HttpServerUtility)
        If Session("UserID") Is Nothing Then
            Response.Redirect("/Default.aspx")
        Else
            Using db As LiteDatabase = New LiteDatabase(Server.MapPath("~/App_Data/Database.accdb"))
                Dim tbl As LiteCollection(Of User) = db.GetCollection(Of User)("Users")
                Dim user As User = tbl.FindById(Integer.Parse(Session("UserID")))
                If user.EmailVerified Then
                    Response.Redirect("/Default.aspx") ' TODO: Redirect to user panel or something
                End If
            End Using
        End If
    End Sub

    Public Shared Sub LoggedOutOnly(Response As HttpResponse, Session As HttpSessionState)
        If Not Session("UserID") Is Nothing Then
            Response.Redirect("/MyZone.aspx") ' TODO: Redirect to user panel or something
        End If
    End Sub
End Class

Public Class LoggedInUsersOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.LoggedInOnly(Response, Session)
    End Sub
End Class

Public Class AnonymousUsersOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.LoggedOutOnly(Response, Session)
    End Sub
End Class

Public Class VerifiedUsersOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.VerifiedOnly(Response, Session, Server)
    End Sub
End Class

Public Class LoggedNotVerifiedUsersOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.LoggedNotVerifiedOnly(Response, Session, Server)
    End Sub
End Class

Public Class AdministratorOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.AdminOnly(Response, Session, Server)
    End Sub
End Class

Public Class EliteAdministratorOnly
    Inherits Page
    Public Overridable Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        UserUtils.EliteAdminOnly(Response, Session, Server)
    End Sub
End Class