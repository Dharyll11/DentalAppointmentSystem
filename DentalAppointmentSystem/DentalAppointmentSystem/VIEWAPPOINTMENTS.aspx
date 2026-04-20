<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VIEWAPPOINTMENTS.aspx.vb" Inherits="DentalAppointmentSystem.VIEWAPPOINTMENTS" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>My Appointments</title>
</head>
<body>
<form id="form1" runat="server">

<h2>My Appointments</h2>

<asp:Label ID="lblEmpty" runat="server" ForeColor="Red"></asp:Label>
<br /><br />

<asp:GridView ID="gvAppointments" runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="AppointmentID"
    OnRowCommand="gvAppointments_RowCommand">

    <Columns>

        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
        <asp:BoundField DataField="Dentist" HeaderText="Dentist" />
        <asp:BoundField DataField="Service" HeaderText="Service" />
        <asp:BoundField DataField="AppointmentDate" HeaderText="Date" DataFormatString="{0:MMMM dd, yyyy}" />
        <asp:BoundField DataField="AppointmentTime" HeaderText="Time" />
        <asp:BoundField DataField="Status" HeaderText="Status" />

        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btnCancel" runat="server"
                    Text="Cancel"
                    CommandName="CancelAppointment"
                    CommandArgument='<%# Container.DataItemIndex %>' />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

<br />
<asp:Button ID="btnBack" runat="server"
    Text="Back to Dashboard"
    PostBackUrl="PatientDashboard.aspx" />

</form>
</body>
</html>