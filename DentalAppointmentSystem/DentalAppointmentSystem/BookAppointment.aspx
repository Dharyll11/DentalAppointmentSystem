<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.vb" Inherits="DentalAppointmentSystem.BookAppointment" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Book Appointment</title>
</head>
<body>
<form id="form1" runat="server">
<h2>Book Appointment</h2>

<!-- Full Name -->
<asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
<br /><br />

<!-- Contact Number -->
<asp:TextBox ID="txtContactNumber" runat="server"></asp:TextBox>
<br /><br />

<!-- Email -->
<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
<br /><br />

<!-- Dentist (DoctorID as Value) -->
<asp:DropDownList ID="ddlDentist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDentist_SelectedIndexChanged">
    <asp:ListItem Text="-- Select Dentist --" Value="0"/>
</asp:DropDownList>
<br /><br />

<!-- Service (Dynamic) -->
<asp:DropDownList ID="ddlService" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlService_SelectedIndexChanged">
    <asp:ListItem Text="-- Select Service --" Value="0"/>
</asp:DropDownList>
<asp:Label ID="lblServiceInfo" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
<br /><br />

<!-- Calendar -->
<asp:Calendar ID="calDate" runat="server"></asp:Calendar>
<br /><br />

<!-- Time -->
<asp:DropDownList ID="ddlTime" runat="server"></asp:DropDownList>
<br /><br />

<!-- Notes -->
<asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
<br /><br />

<asp:Button ID="btnBook" runat="server" Text="Book Appointment"/>
</form>
</body>
</html>