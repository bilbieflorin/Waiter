<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Web_Forms_Admin_Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel-heading page-header text-center"><h1>Adaugare Preparat</h1></div>
    <div class="panel-body">
        <div id="denumireFormGroup" class="form-group" runat="server">
            <label class="control-label" for="DenumirePreparatTextBox">Denumire preparat</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="DenumirePreparatTextBox" placeholder="Introduceti denumirea preparatului" />
            <asp:CustomValidator ID="DenumireValidator" runat="server" ErrorMessage="Trebuie introdusa o denumire!" ForeColor="Red" Display="Dynamic" ControlToValidate="DenumirePreparatTextBox" OnServerValidate="denumireValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" runat="server">
            <label class="control-label" for="UploadPoza">Poza preparat</label>
            <div>
                <asp:FileUpload ID="UploadPoza" CssClass="form-control" runat="server" Height="39px" Width="309px"/>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UploadPoza" ErrorMessage="Trebuie sa introduci o poza!" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Numai jpg/jpeg/png/bmp file is allowed!" ValidationExpression="^.+(.jpg|.jpeg|.png|.bmp)$" ControlToValidate="UploadPoza" Display="Dynamic" ForeColor="Red"> </asp:RegularExpressionValidator>--%>
            </div>
        </div>
         <div id="tipFormGroup" class="form-group" runat="server">
            <label class="control-label" for="TipPreparatTextBox">Tip preparat</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="TipPreparatTextBox" placeholder="Introduceti tipul preparatului" />
            <asp:CustomValidator ID="TipValidator" runat="server" ErrorMessage="Trebuie specificat un tip!" ForeColor="Red" Display="Dynamic" ControlToValidate="TipPreparatTextBox" OnServerValidate="tipValidate" ValidateEmptyText="true"></asp:CustomValidator>
         </div>
        <div id="gramajFormGroup" class="form-group" runat="server">
            <label class="control-label" for="GramajTextBox">Gramaj</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="GramajTextBox" placeholder="Introduceti gramajul preparatului" />
            <asp:CustomValidator ID="GramajValidator" runat="server" ErrorMessage="Gramaj invalid!" ForeColor="Red" Display="Dynamic" ControlToValidate="GramajTextBox" OnServerValidate="gramajValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div id="pretFormGroup" class="form-group" runat="server">
            <label class="control-label" for="PretTextBox">Pret</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="PretTextBox" placeholder="Introduceti pretul preparatului" />
            <asp:CustomValidator ID="PretValidator" runat="server" ErrorMessage="Pret invalid!" ForeColor="Red" Display="Dynamic" ControlToValidate="PretTextBox" OnServerValidate="pretValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" runat="server">
            <label class="control-label">Ingrediente</label>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="IngredienteUpdatePanel">
                <ContentTemplate>
                    <ul class="list-group">
                        <asp:ListView runat="server" ID="IngredienteListView">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="list-group-item">
                                    <asp:Label runat="server" Text='<%# Eval("denumire") %>' />
                                </li>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <li class="list-group-item"></li>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </ul>
                    <asp:DropDownList ID="IngredienteDropDownList" runat="server" Width="241px" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="IngredientNouTextBox" placeholder="Denumire" Visible="false"/>
                    <asp:Button ID="Button1" runat="server" OnClick="adaugaIngredientButtonClick" CssClass="btn btn-primary" Text="Adauga ingredient" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <div class="form-group">
            <label class="control-label">Specific preferat</label>
            <asp:RadioButtonList runat="server" ID="RadioButtonList1" DataSourceID="SqlDataSource1" DataTextField="DENUMIRE_SPECIFIC" DataValueField="DENUMIRE_SPECIFIC">
            </asp:RadioButtonList>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT [DENUMIRE_SPECIFIC] FROM [SPECIFIC] ORDER BY [DENUMIRE_SPECIFIC]"></asp:SqlDataSource>
            <asp:CustomValidator ID="SpecificValidator" runat="server" ErrorMessage="Alege un specific!" ForeColor="Red" Display="Dynamic" OnServerValidate="specificValidate"></asp:CustomValidator>
        </div>
        <asp:Button ID="AdaugaButton" OnClick="adaugaButtonClick" CssClass="btn btn-primary" runat="server" Text="Adauga"></asp:Button>
    </div>
</asp:Content>

