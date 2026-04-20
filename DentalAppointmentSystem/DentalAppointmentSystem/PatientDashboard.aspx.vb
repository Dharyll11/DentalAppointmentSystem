Imports System

Public Class PatientDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Check if user is logged in
        If Session("UserID") Is Nothing Or Session("Role") Is Nothing Then
            ' Not logged in or role not set
            Response.Redirect("Login.aspx")
            Return
        End If

        ' Display welcome message
        lblWelcome.Text = "Welcome, " & Session("FullName").ToString() & "!"

        ' Optional: Role-based redirect logic (for future roles)
        Select Case Session("Role").ToString().ToLower()
            Case "patient"
                ' Already here, do nothing
            Case "receptionist"
                Response.Redirect("ReceptionistDashboard.aspx")
            Case "admin"
                Response.Redirect("AdminDashboard.aspx")
            Case Else
                ' Unknown role, force logout
                Session.Clear()
                Response.Redirect("Login.aspx")
        End Select
    End Sub
End Class