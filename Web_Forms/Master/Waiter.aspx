<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Waiter.aspx.cs" Inherits="Web_Forms_Master_Waiter" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-heading page-header text-center">
        <h1>Bine ati venit!</h1>
    </div>
    <div class="panel-body" style="background:inherit">
        <p class="text-center text-info" style="font-size:large"><a href="../../Web_Forms/Menu/Meniu.aspx">Ma grabesc vreau la meniu</a>!</p>
        <p class="text-center text-info" runat="server" id="RecomandariLink" visible="false" style="font-size:large"><a href="../../Web_Forms/Menu/Recomandari.aspx">Cateva preparate pe care le-ai putea aprecia</a></p>
        <p class="text-center text-info" runat="server" id="ContactLink" visible="false" style="font-size:large"><a href="../../Web_Forms/User_actions/Contact.aspx">Ai sugestii referitoare la serviiciile noastre? Trimite-ne opinia ta!</a></p>
        <a runat="server" id="LoginButton" class="btn btn-primary btn-lg pull-left center-block" href="../../Web_Forms/User_actions/Login.aspx" role="button">Autentificare</a>
        <a runat="server" id="RegisterButton" class="btn btn-primary btn-lg pull-right center-block" href="../../Web_Forms/User_actions/Register.aspx" role="button">Inregistrare</a>
    </div>
</asp:Content>