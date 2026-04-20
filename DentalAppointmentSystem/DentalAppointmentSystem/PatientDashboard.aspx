<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PatientDashboard.aspx.vb" Inherits="DentalAppointmentSystem.PatientDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patient Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Patient Dashboard</h2>
        <asp:Label ID="lblWelcome" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>
        <br /><br />
        <!-- Optional: Links to book/view appointments -->
        <asp:HyperLink ID="hlBook" runat="server" NavigateUrl="BookAppointment.aspx">Book Appointment</asp:HyperLink>
        <br />
        <asp:HyperLink ID="hlView" runat="server" NavigateUrl="VIEWAPPOINTMENTS.aspx">View My Appointments</asp:HyperLink>
    </form>
</body>
</html>