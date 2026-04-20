Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Globalization

Public Class BookAppointment
    Inherits System.Web.UI.Page

    Dim booking As New BookingHandler()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then
            Response.Redirect("Login.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            calDate.TodaysDate = DateTime.Today
            LoadDentists()
            LoadServices()
        End If
    End Sub

    ' -----------------------------
    ' Load dentists
    ' -----------------------------
    Private Sub LoadDentists()
        ddlDentist.Items.Clear()
        ddlDentist.Items.Add(New ListItem("-- Select Dentist --", "0"))

        Dim connStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

        Using con As New SqlConnection(connStr)
            Dim sql As String = "SELECT DoctorID, FullName FROM Doctors"
            Using cmd As New SqlCommand(sql, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        ddlDentist.Items.Add(New ListItem(reader("FullName").ToString(), reader("DoctorID").ToString()))
                    End While
                End Using
            End Using
        End Using
    End Sub

    ' -----------------------------
    ' Load services
    ' -----------------------------
    Private Sub LoadServices()
        ddlService.Items.Clear()
        ddlService.Items.Add(New ListItem("-- Select Service --", "0"))

        Dim connStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString
        Using con As New SqlConnection(connStr)
            Dim sql As String = "SELECT ServiceID, ServiceName, Price, DurationMinutes FROM Services"
            Using cmd As New SqlCommand(sql, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim text As String = reader("ServiceName").ToString() & " ₱" & Convert.ToDecimal(reader("Price")).ToString("N0") & " – " & reader("DurationMinutes").ToString() & " mins"
                        ddlService.Items.Add(New ListItem(text, reader("ServiceID").ToString()))
                    End While
                End Using
            End Using
        End Using
    End Sub

    ' -----------------------------
    ' Display selected service info
    ' -----------------------------
    Protected Sub ddlService_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlService.SelectedIndexChanged
        If ddlService.SelectedIndex = 0 Then
            lblServiceInfo.Text = ""
            Return
        End If

        Dim connStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString
        Using con As New SqlConnection(connStr)
            Dim sql As String = "SELECT ServiceName, Price, DurationMinutes FROM Services WHERE ServiceID=@ID"
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@ID", ddlService.SelectedValue)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        lblServiceInfo.Text = "Selected: " & reader("ServiceName").ToString() & " ₱" & Convert.ToDecimal(reader("Price")).ToString("N0") & " – " & reader("DurationMinutes").ToString() & " mins"
                    End If
                End Using
            End Using
        End Using
    End Sub

    ' -----------------------------
    ' Load slots for selected dentist and date
    ' -----------------------------
    Protected Sub LoadAvailableSlots()
        ddlTime.Items.Clear()
        ddlTime.Items.Add(New ListItem("-- Select Time --", ""))

        If ddlDentist.SelectedIndex = 0 Or calDate.SelectedDate = DateTime.MinValue Then Exit Sub

        Dim doctorID As Integer = Convert.ToInt32(ddlDentist.SelectedValue)
        Dim slotStatus = booking.GetAvailableSlots(doctorID, calDate.SelectedDate)

        For Each slot As String In booking.AllSlots
            Dim item As New ListItem(slot, slot)

            ' Check if slot full
            If slotStatus.ContainsKey(slot) AndAlso slotStatus(slot) = False Then
                item.Enabled = False
                item.Text &= " (FULL)"
            End If

            ' Disable past time today
            Dim slotTime As DateTime = DateTime.ParseExact(slot, "hh:mm tt", CultureInfo.InvariantCulture)
            Dim slotDateTime As DateTime = calDate.SelectedDate.Date.AddHours(slotTime.Hour).AddMinutes(slotTime.Minute)
            If calDate.SelectedDate = DateTime.Today AndAlso slotDateTime <= DateTime.Now Then
                item.Enabled = False
            End If

            ddlTime.Items.Add(item)
        Next
    End Sub

    Protected Sub calDate_SelectionChanged(sender As Object, e As EventArgs) Handles calDate.SelectionChanged
        LoadAvailableSlots()
    End Sub

    Protected Sub ddlDentist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDentist.SelectedIndexChanged
        LoadAvailableSlots()
    End Sub

    ' -----------------------------
    ' Book appointment
    ' -----------------------------
    Protected Sub btnBook_Click(sender As Object, e As EventArgs) Handles btnBook.Click
        If txtFullName.Text.Trim() = "" Then ShowAlert("Enter Full Name") : Exit Sub
        If txtContactNumber.Text.Trim() = "" Then ShowAlert("Enter Contact Number") : Exit Sub
        If calDate.SelectedDate = DateTime.MinValue Then ShowAlert("Select date") : Exit Sub
        If ddlDentist.SelectedIndex = 0 Then ShowAlert("Select dentist") : Exit Sub
        If ddlService.SelectedIndex = 0 Then ShowAlert("Select service") : Exit Sub
        If ddlTime.SelectedIndex = 0 Then ShowAlert("Select time") : Exit Sub

        Dim userID As Integer = Convert.ToInt32(Session("UserID"))
        Dim doctorID As Integer = Convert.ToInt32(ddlDentist.SelectedValue)
        Dim serviceID As Integer = Convert.ToInt32(ddlService.SelectedValue)

        Dim result As String = booking.BookAppointment(userID, doctorID, serviceID, calDate.SelectedDate, ddlTime.SelectedItem.Text, txtNotes.Text.Trim())

        ShowAlert(result)

        If result = "Booked successfully!" Then
            LoadAvailableSlots()
        End If
    End Sub

    Private Sub ShowAlert(msg As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('" & msg.Replace("'", "") & "');", True)
    End Sub

    ' -----------------------------
    ' Disable past dates and after 5PM
    ' -----------------------------
    Protected Sub calDate_DayRender(sender As Object, e As DayRenderEventArgs) Handles calDate.DayRender
        If e.Day.Date < DateTime.Today Then
            e.Day.IsSelectable = False
            e.Cell.ForeColor = Drawing.Color.Gray
        End If
        If e.Day.Date = DateTime.Today AndAlso DateTime.Now.Hour >= 17 Then
            e.Day.IsSelectable = False
            e.Cell.ForeColor = Drawing.Color.Gray
        End If
    End Sub
End Class