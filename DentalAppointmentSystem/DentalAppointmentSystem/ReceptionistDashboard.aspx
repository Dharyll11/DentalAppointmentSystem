<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReceptionistDashboard.aspx.vb" Inherits="DentalAppointmentSystem.ReceptionistDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Receptionist Dashboard</title>
</head>

<body>

<form id="form1" runat="server">

<h2>Receptionist Dashboard</h2>

Search Patient:
<asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
<asp:Button ID="btnSearch" runat="server" Text="Search"/>

<br /><br />

Filter Status:

<asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true">
<asp:ListItem Text="All" Value="All"/>
<asp:ListItem Text="Booked" Value="Booked"/>
<asp:ListItem Text="Confirmed" Value="Confirmed"/>
<asp:ListItem Text="Cancelled" Value="Cancelled"/>
</asp:DropDownList>

<br /><br />

<asp:Label ID="lblTotal" runat="server" Font-Bold="true"></asp:Label>

<br /><br />

<asp:GridView 
ID="gvAppointments" 
runat="server" 
AutoGenerateColumns="False"
OnRowCommand="gvAppointments_RowCommand">

<Columns>

<asp:BoundField DataField="AppointmentID" HeaderText="ID" />

<asp:BoundField DataField="FullName" HeaderText="Patient" />

<asp:BoundField DataField="ServiceName" HeaderText="Service" />

<asp:BoundField DataField="AppointmentDate" HeaderText="Date" />

<asp:BoundField DataField="AppointmentTime" HeaderText="Time" />

<asp:BoundField DataField="Status" HeaderText="Status" />

<asp:TemplateField HeaderText="Actions">
<ItemTemplate>

<asp:Button 
ID="btnConfirm" 
runat="server" 
Text="Confirm"
CommandName="Confirm"
CommandArgument='<%# Eval("AppointmentID") %>' />

<asp:Button 
ID="btnCancel" 
runat="server" 
Text="Cancel"
CommandName="CancelAppt"
CommandArgument='<%# Eval("AppointmentID") %>' />

</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</form>
</body>
</html>