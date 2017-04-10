' This is completely C#. Take my word on that. Not lying.
' Yeah
' Not lying...
Imports Microsoft.VisualBasic
Imports LiteDB
Imports NodaTime

Public Module DB_Stuff
    Public Class User
        <BsonId>
        Public Property Id As Integer ' Ignore the missing semicolons. Still C#.
        Public Property Username As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property Password As String
        Public Property Email As String
        Public Property EmailVerified As Boolean
        Public Property VerificationCode As Integer
        Public Property JoinDate As Long
        Public Property LastLogin As Long
        Public Property DeedCoins As Double
        Public Property Exp As Integer
        Public Property ProfilePic As String
        Public Property Description As String
        Public Property Location As String
        Public Property Position As GeoPos
        Public Property ProfileViews As Integer
        Public Property LastProfileViewer As Integer
        Public Property Country As String
        Public Property UserLevel As UserType
        Public Property EmailNotifications As Boolean = False
    End Class ' Also ignore the missing parentheses. Still C#, don't be so suspicious...


    Public Class Organization
        <BsonId>
        Public Property Id As Integer
        Public Property OrganizationName As String
        Public Property Approved As Boolean
        Public Property Rejected As Boolean
        Public Property RequestText As String
        Public Property RequestMonthlyUsers As Integer
        Public Property MonthlyPoints As Single
        Public Property Points As Single
        Public Property Address As String
        Public Property Position As GeoPos
        Public Property Description As String
        Public Property OwnerID As Integer ' Link User.Id
        Public Property ImageLoc As String
        Public Property OpeningHours As String
        Public Property CreationDate As Long
        Public Property Images As List(Of String)
        Public Property Audience As String
        Public Property LastUpdatedMonth As Integer = 0
        Public Property LastUpdatedYear As Integer = 0
    End Class

    Public Class JobProposal
        <BsonId>
        Public Property Id As Integer
        Public Property Title As String
        Public Property ShortDescription As String
        Public Property Offerer As Integer? ' Link User.Id
        Public Property OffererOrg As Integer? ' Link Organization.Id
        Public Property Type As GigType
        Public Property Price As Double
        Public Property Description As String
        Public Property VideoURL As String
        Public Property ImageURLs As String()
        Public Property Disabled As Boolean = False
    End Class

    Public Class Purchase
        <BsonId>
        Public Property Id As Integer
        Public Property PurchaseDate As Long
        Public Property Buyer As Integer ' Link User.Id
        Public Property Transfer As Double
        Public Property HasBeenDelivered As Boolean
        Public Property HasBeenPaid As Boolean
        Public Property PaymentDate As Long
        Public Property Rating As Integer
        Public Property Proposal As Integer
    End Class

    Public Class Comment
        <BsonId>
        Public Property Id As Integer
        Public Property Writer As Integer ' Link User.Id
        Public Property ProposalId As Integer ' Link JobProposal.Id
        Public Property Content As String
        Public Property WriteDate As Long
    End Class

    Public Class Notification
        <BsonId>
        Public Property Id As Integer
        Public Property Receiver As Integer ' Link User.Id
        Public Property Content As String
        Public Property NotificationDate As Long
        Public Property Link As String
        Public Property ImageURL As String
        Public Property Popped As Boolean
        Public Property Seen As Boolean
    End Class

    Class Message
        <BsonId>
        Public Property Id As Integer
        Public Property Sender As Integer
        Public Property Reciever As Integer
        Public Property Content As String
        Public Property WriteDate As Long
    End Class

    Public Enum GigType
        OfferJob
        OfferProduct
    End Enum

    Public Enum UserType
        Regular = 1
        Moderator = 2
        Administrator = 3
    End Enum

    Public Class GeoPos
        Public Property Lat() As Double
        Public Property Lon() As Double
        Public Property Address As String
        Public Sub New(lat As Double, lon As Double)
            Me.Lat = lat
            Me.Lon = lon
        End Sub
        Public Sub New() ' Also, this is how you do constructors in C#. Yeahhhhhh....
            Lat = 0
            Lon = 0
        End Sub
    End Class
End Module ' Ok f@ck this, you got me, this is not C#.. BUTIDON'TGIVEAFUCK <3
