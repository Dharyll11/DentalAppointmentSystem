Imports System.Data.SqlClient
Imports System.Configuration

Public Class VIEWAPPOINTMENTS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ' ✅ Check if logged in
        If Session("UserID") Is Nothing Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            LoadAppointments()
        End If

    End Sub

    Private Sub LoadAppointments()

        Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

        Using con As New SqlConnection(conStr)
            con.Open()

            ' ✅ FINAL FIXED QUERY (MATCHES YOUR DATABASE)
            Dim sql As String =
                "SELECT a.AppointmentID, " &
                "u.FullName, " &
                "d.FullName AS Dentist, " &
                "s.ServiceName AS Service, " &
                "a.AppointmentDate, " &
                "a.AppointmentTime, " &
                "a.Status " &
                "FROM Appointments a " &
                "INNER JOIN Users u ON a.UserID = u.UserID " &
                "INNER JOIN Doctors d ON a.DoctorID = d.DoctorID " &
                "INNER JOIN Services s ON a.ServiceID = s.ServiceID " &
                "WHERE a.UserID = @UserID " &
                "ORDER BY a.AppointmentDate DESC"

            Dim cmd As New SqlCommand(sql, con)
            cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session("UserID")))

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.HasRows Then
                gvAppointments.DataSource = reader
                gvAppointments.DataBind()
                lblEmpty.Text = ""
            Else
                gvAppointments.DataSource = Nothing
                gvAppointments.DataBind()
                lblEmpty.Text = "No appointments found."
            End If

        End Using

    End Sub

    Protected Sub gvAppointments_RowCommand(sender As Object, e As GridViewCommandEventArgs)

        If e.CommandName = "CancelAppointment" Then

            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim appointmentID As Integer = Convert.ToInt32(gvAppointments.DataKeys(rowIndex).Value)

            Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

            Using con As New SqlConnection(conStr)
                con.Open()

                Dim sql As String = "UPDATE Appointments SET Status='Cancelled' WHERE AppointmentID=@ID"

                Dim cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@ID", appointmentID)
                cmd.ExecuteNonQuery()

            End Using

            LoadAppointments()

        End If

    End Sub

End Class