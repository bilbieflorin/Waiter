<%@ Page Title="" Language="C#" MasterPageFile="../Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel-heading page-header text-center"><h1>Contact</h1></div>
    <div class="panel-body">
        <div class="form-group" id="emailformgroup" runat="server">
            <label class="control-label">Adresa email</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="EmailTextBox" placeholder="Indroduceti adresa email" Text=""/>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Adresa de email invalida!" ForeColor="Red" Display="Dynamic" ControlToValidate="EmailTextBox" OnServerValidate="emailServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" id="commentsformgroup" runat="server">
            <label class="control-label">Mesajul dumneavoastra</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="CommentsTextBox" placeholder="Introduceti mesajul" TextMode="MultiLine" Rows="10" Text=""/>
            <asp:CustomValidator ID="Comments" runat="server"  ErrorMessage="Mesajul trebuie sa contina minim 10 caractere!" ForeColor="Red" Display="Dynamic"  ControlToValidate="CommentsTextBox" OnServerValidate="commentsServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Trimite"></asp:Button>
    </div>
</asp:Content>

