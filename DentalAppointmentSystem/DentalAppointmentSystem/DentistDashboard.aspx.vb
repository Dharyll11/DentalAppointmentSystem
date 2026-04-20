Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DentistDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserID") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            LoadAppointments()
        End If

    End Sub

    Private Sub LoadAppointments()

        Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

        Using con As New SqlConnection(conStr)
            con.Open()

            ' ✅ Load appointments assigned to logged-in dentist
            ' Show both Booked and Confirmed
            Dim sql As String = "SELECT a.AppointmentID, u.FullName, s.ServiceName AS Service, " &
                                "a.AppointmentDate, a.AppointmentTime, a.Status, a.Notes " &
                                "FROM Appointments a " &
                                "INNER JOIN Users u ON a.UserID = u.UserID " &
                                "INNER JOIN Services s ON a.ServiceID = s.ServiceID " &
                                "WHERE a.DoctorID = @DoctorID AND a.Status IN ('Booked','Confirmed') " &
                                "ORDER BY a.AppointmentDate ASC, a.AppointmentTime ASC"

            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@DoctorID", Session("UserID")) ' DoctorID ng dentist login
                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                gvAppointments.DataSource = dt
                gvAppointments.DataBind()
            End Using

        End Using

    End Sub

    Protected Sub gvAppointments_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        If e.CommandName = "Complete" Then

            Dim appointmentID As Integer = Convert.ToInt32(e.CommandArgument)
            Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

            Using con As New SqlConnection(conStr)
                con.Open()

                Dim sql As String = "UPDATE Appointments SET Status='Completed' WHERE AppointmentID=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@ID", appointmentID)
                    cmd.ExecuteNonQuery()
                End Using

            End Using

            LoadAppointments()

        End If

    End Sub

End Class