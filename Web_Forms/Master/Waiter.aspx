<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Waiter.aspx.cs" Inherits="Web_Forms_Master_Waiter" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-heading page-header text-center">
        <h1>Bine ati venit!</h1>
    </div>
    <div class="panel-body" style="background:inherit">
        <p class="text-center text-info" style="font-size:large">Ma grabesc vreau la <a href="../../Web_Forms/Menu/Meniu.aspx">meniu</a>!</p>
        <a runat="server" id="LoginButton" class="btn btn-primary btn-lg pull-left xs-center" href="../../Web_Forms/User_actions/Login.aspx" role="button">Autentificare</a>
        <a runat="server" id="RegisterButton" class="btn btn-primary btn-lg pull-right xs-center" href="../../Web_Forms/User_actions/Register.aspx" role="button">Inregistrare</a>
    </div>
</asp:Content>