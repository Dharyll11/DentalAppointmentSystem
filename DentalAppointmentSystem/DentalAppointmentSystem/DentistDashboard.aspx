<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DentistDashboard.aspx.vb" Inherits="DentalAppointmentSystem.DentistDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Dentist Dashboard</title>
</head>

<body>

<form id="form1" runat="server">

<h2>Dentist Dashboard</h2>

<br />

<asp:GridView 
ID="gvAppointments" 
runat="server"
AutoGenerateColumns="False"
OnRowCommand="gvAppointments_RowCommand">

<Columns>

<asp:BoundField DataField="FullName" HeaderText="Patient Name" />

<asp:BoundField DataField="Service" HeaderText="Service" />

<asp:BoundField DataField="AppointmentDate" HeaderText="Date" />

<asp:BoundField DataField="AppointmentTime" HeaderText="Time" />

<asp:BoundField DataField="Status" HeaderText="Status" />

<asp:TemplateField HeaderText="Action">
<ItemTemplate>

<asp:Button
ID="btnComplete"
runat="server"
Text="Mark Completed"
CommandName="Complete"
CommandArgument='<%# Eval("AppointmentID") %>' />

</ItemTemplate>
</asp:TemplateField>

</Columns>

</asp:GridView>

</form>

</body>
</html>