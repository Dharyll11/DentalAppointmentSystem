<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="DentalAppointmentSystem.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patient Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Patient Login</h2>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <br /><br />

        Email:
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
        <br /><br />

        Password:
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <br /><br />

        <asp:Button ID="btnLogin" runat="server" Text="Login" />
    </form>
</body>
</html>