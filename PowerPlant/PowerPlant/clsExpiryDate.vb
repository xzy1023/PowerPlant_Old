'WO#650 ADD Start
Public Class clsExpiryDate

    Private dteLatestExpiryDate As DateTime
    Private dteEarliestExpiryDate As DateTime
    Private dteLatestProductionDate As DateTime
    Private dteEarliestProductionDate As DateTime
    Private intProductionShifeLifeDays As Int16
    Private intShipShelfLifeDays As Int16

    Public Sub New()
    End Sub

    Public Sub New(ByVal ProductionShelfLifeDays As Int16, ByVal ShipShelfLifeDays As Int16)
        intProductionShifeLifeDays = ProductionShelfLifeDays
        intShipShelfLifeDays = ShipShelfLifeDays
    End Sub

    Public ReadOnly Property LatestExpiryDate() As DateTime
        Get
            Return dteLatestExpiryDate
        End Get
    End Property

    Public ReadOnly Property EarilestExpiryDate() As DateTime
        Get
            Return dteEarliestExpiryDate
        End Get
    End Property
    Public ReadOnly Property LatestProductionDate() As DateTime
        Get
            Return dteLatestProductionDate
        End Get
    End Property

    Public ReadOnly Property EarilestProductionDate() As DateTime
        Get
            Return dteEarliestProductionDate
        End Get
    End Property

    Public WriteOnly Property ProductionShelfLifeDays() As Int16
        Set(value As Int16)
            intProductionShifeLifeDays = value
        End Set
    End Property

    Public WriteOnly Property ShipShelfLifeDays() As Int16
        Set(value As Int16)
            intShipShelfLifeDays = value
        End Set
    End Property


    Public Function IsExpiryDateValid(ByVal dteExpiryDate As DateTime, ByVal dteProductionDate As DateTime) As Boolean
        Try
            dteEarliestExpiryDate = DateAdd(DateInterval.Day, intProductionShifeLifeDays - intShipShelfLifeDays, dteProductionDate)
            dteLatestExpiryDate = DateAdd(DateInterval.Day, intProductionShifeLifeDays, dteProductionDate)
            If dteExpiryDate < dteEarliestExpiryDate Or dteExpiryDate > dteLatestExpiryDate Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsExpiryDateValid" & vbCrLf & ex.Message)
        End Try
    End Function

    Public Function IsProductionDateValid(ByVal dteProductionDate As DateTime) As Boolean
        Try
            'dteEarliestProductionDate = DateAdd(DateInterval.Day, intShipShelfLifeDays - intProductionShifeLifeDays, Today)
            'dteLatestProductionDate = DateAdd(DateInterval.Day, -intProductionShifeLifeDays, Today)
            ' dteEarliestProductionDate = DateAdd(DateInterval.Day, intProductionShifeLifeDays - intShipShelfLifeDays, Today)
            'dteLatestProductionDate = Date
            dteEarliestProductionDate = DateAdd(DateInterval.Day, -intShipShelfLifeDays, Today)
            dteLatestProductionDate = Today
            If dteProductionDate < dteEarliestProductionDate Or dteProductionDate > dteLatestProductionDate Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw New Exception("Error in IsProductionDateValid" & vbCrLf & ex.Message)
        End Try
    End Function
End Class
'WO#650 ADD Stop
