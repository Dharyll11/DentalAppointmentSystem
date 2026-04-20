Imports System.Data.SqlClient
Imports System.Configuration

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        Dim conStr As String = ConfigurationManager.ConnectionStrings("ClinicDB").ConnectionString

        Using con As New SqlConnection(conStr)
            con.Open()

            ' ✅ Include Role column in SELECT
            Dim sql As String = "SELECT UserID, FullName, Email, Role FROM Users WHERE Email=@Email AND Password=@Password"
            Dim cmd As New SqlCommand(sql, con)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim())

            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' ✅ Save session values
                Session("UserID") = Convert.ToInt32(reader("UserID"))
                Session("UserEmail") = reader("Email").ToString()
                Session("FullName") = reader("FullName").ToString()
                Session("Role") = reader("Role").ToString()

                ' ✅ Role-based redirect
                Select Case reader("Role").ToString().ToLower()
                    Case "admin"
                        Response.Redirect("AdminDashboard.aspx")
                    Case "doctor"
                        Response.Redirect("DentistDashboard.aspx")
                    Case "receptionist"
                        Response.Redirect("ReceptionistDashboard.aspx")
                    Case Else
                        Response.Redirect("PatientDashboard.aspx")
                End Select
            Else
                lblMessage.Text = "Invalid email or password."
            End If
        End Using
    End Sub

End Class