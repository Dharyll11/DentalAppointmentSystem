Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Public Class ReceptionistDashboard
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

            Dim sql As String = "SELECT A.AppointmentID, U.FullName, S.ServiceName, A.AppointmentDate, A.AppointmentTime, A.Status, A.DoctorID " &
                                "FROM Appointments A " &
                                "INNER JOIN Users U ON A.UserID = U.UserID " &
                                "INNER JOIN Services S ON A.ServiceID = S.ServiceID " &
                                "WHERE (@Status='All' OR A.Status=@Status) " &
                                "AND (@Search='' OR U.FullName LIKE '%' + @Search + '%') " &
                                "ORDER BY A.AppointmentDate DESC, A.AppointmentTime ASC"

            Dim cmd As New SqlCommand(sql, con)

            cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue)
            cmd.Parameters.AddWithValue("@Search", txtSearch.Text.Trim())

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()

            da.Fill(dt)

            gvAppointments.DataSource = dt
            gvAppointments.DataBind()

            lblTotal.Text = "Total Appointments: " & dt.Rows.Count

        End Using

    End Sub

    Protected Sub ddlStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatus.SelectedIndexChanged
        LoadAppointments()
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadAppointments()
    End Sub

    Protected Sub gvAppointments_RowCommand(sender As Object, e As GridViewCommandEventArgs)

        Dim id As Integer = Convert.ToInt32(e.CommandArgument)
        Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

        Using con As New SqlConnection(conStr)
            con.Open()

            If e.CommandName = "Confirm" Then

                ' --- Keep existing DoctorID, update only Status ---
                Dim selectedDoctorID As Integer
                Dim getDoctorSql As String = "SELECT DoctorID FROM Appointments WHERE AppointmentID=@ID"
                Using getCmd As New SqlCommand(getDoctorSql, con)
                    getCmd.Parameters.AddWithValue("@ID", id)
                    selectedDoctorID = Convert.ToInt32(getCmd.ExecuteScalar())
                End Using

                Dim sql As String = "UPDATE Appointments SET Status='Confirmed', DoctorID=@DoctorID WHERE AppointmentID=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@ID", id)
                    cmd.Parameters.AddWithValue("@DoctorID", selectedDoctorID)
                    cmd.ExecuteNonQuery()
                End Using

            End If

            If e.CommandName = "CancelAppt" Then

                Dim sql As String = "UPDATE Appointments SET Status='Cancelled' WHERE AppointmentID=@ID"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@ID", id)
                    cmd.ExecuteNonQuery()
                End Using

            End If

        End Using

        LoadAppointments()

    End Sub

End Class