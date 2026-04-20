Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Globalization

Public Class BookingHandler

    Public AllSlots As New List(Of String) From {
        "09:00 AM", "10:00 AM", "11:00 AM", "12:00 PM",
        "01:00 PM", "02:00 PM", "03:00 PM", "04:00 PM", "05:00 PM"
    }

    Private ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString
        End Get
    End Property

    ' -----------------------------
    ' Get available time slots
    ' -----------------------------
    Public Function GetAvailableSlots(doctorID As Integer, selectedDate As Date) As Dictionary(Of String, Boolean)
        Dim slots As New Dictionary(Of String, Boolean)
        Using con As New SqlConnection(ConnectionString)
            con.Open()
            For Each slot As String In AllSlots
                Dim slotTime As DateTime = DateTime.ParseExact(slot, "hh:mm tt", CultureInfo.InvariantCulture)
                Dim slotDateTime As DateTime = selectedDate.Date.AddHours(slotTime.Hour).AddMinutes(slotTime.Minute)
                If slotDateTime <= DateTime.Now Then Continue For

                Dim sql As String = "SELECT COUNT(*) FROM Appointments WHERE DoctorID=@DoctorID AND AppointmentDate=@Date AND AppointmentTime=@Time AND Status='Booked'"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@DoctorID", doctorID)
                    cmd.Parameters.AddWithValue("@Date", selectedDate.Date)
                    cmd.Parameters.AddWithValue("@Time", slotTime.TimeOfDay)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    slots.Add(slot, count < 2)
                End Using
            Next
        End Using
        Return slots
    End Function

    ' -----------------------------
    ' Book appointment
    ' -----------------------------
    Public Function BookAppointment(userID As Integer,
                                    doctorID As Integer,
                                    serviceID As Integer,
                                    selectedDate As Date,
                                    selectedTime As String,
                                    notes As String) As String

        Dim slotTime As DateTime = DateTime.ParseExact(selectedTime, "hh:mm tt", CultureInfo.InvariantCulture)
        Dim slotDateTime As DateTime = selectedDate.Date.AddHours(slotTime.Hour).AddMinutes(slotTime.Minute)

        If slotDateTime <= DateTime.Now Then Return "Cannot book past time!"

        Using con As New SqlConnection(ConnectionString)
            con.Open()

            ' Check if slot full
            Dim checkSql As String = "SELECT COUNT(*) FROM Appointments WHERE DoctorID=@DoctorID AND AppointmentDate=@Date AND AppointmentTime=@Time AND Status='Booked'"
            Using checkCmd As New SqlCommand(checkSql, con)
                checkCmd.Parameters.AddWithValue("@DoctorID", doctorID)
                checkCmd.Parameters.AddWithValue("@Date", selectedDate.Date)
                checkCmd.Parameters.AddWithValue("@Time", slotTime.TimeOfDay)
                If Convert.ToInt32(checkCmd.ExecuteScalar()) >= 2 Then Return "Slot already full!"
            End Using

            ' Get service duration
            Dim duration As Integer
            Dim durationSql As String = "SELECT DurationMinutes FROM Services WHERE ServiceID=@ServiceID"
            Using durationCmd As New SqlCommand(durationSql, con)
                durationCmd.Parameters.AddWithValue("@ServiceID", serviceID)
                duration = Convert.ToInt32(durationCmd.ExecuteScalar())
            End Using

            ' Compute EndTime
            Dim endTime As DateTime = slotDateTime.AddMinutes(duration)

            ' Insert appointment
            Dim insertSql As String = "INSERT INTO Appointments (UserID, DoctorID, ServiceID, AppointmentDate, AppointmentTime, EndTime, Notes, Status, CreatedAt) " &
                                      "VALUES (@UserID,@DoctorID,@ServiceID,@Date,@Time,@EndTime,@Notes,'Booked',GETDATE())"
            Using cmd As New SqlCommand(insertSql, con)
                cmd.Parameters.AddWithValue("@UserID", userID)
                cmd.Parameters.AddWithValue("@DoctorID", doctorID)
                cmd.Parameters.AddWithValue("@ServiceID", serviceID)
                cmd.Parameters.AddWithValue("@Date", selectedDate.Date)
                cmd.Parameters.AddWithValue("@Time", slotDateTime.TimeOfDay)
                cmd.Parameters.AddWithValue("@EndTime", endTime.TimeOfDay)
                cmd.Parameters.AddWithValue("@Notes", notes)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        Return "Booked successfully!"
    End Function

End Class