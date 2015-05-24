<%@ Page Title="" ClientIDMode="AutoID" Language="C#" MasterPageFile="../Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Comanda.aspx.cs" Inherits="ComandaWebPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-heading page-header text-center">
        <h1>Comanda mea</h1>
    </div>
    <div class="panel-body">
        <div id="FormularComanda" runat="server">
            <asp:UpdatePanel runat="server" ID="ListaComandaUpdatePanel" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <ul class="list-group">
                        <asp:ListView runat="server" ID="ComandaListView">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="list-group-item">
                                    <asp:LinkButton runat="server"
                                        CssClass="pull-right"
                                        OnClick="deleteItemComanda"
                                        ID="DeleteItemComanda"
                                        CommandArgument='<%#Eval("Preparat.Id")%>'>
                                    <span class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>
                                    <%#Eval("Preparat.Denumire")%>
                                    <span style="display: block">
                                        <asp:LinkButton runat="server"
                                            ID="DeletePreparat"
                                            OnClick="deletePreparat"
                                            CssClass="btn btn-primary btn-circle"
                                            CommandArgument='<%#Eval("Preparat.Id")%>'>
                                    <span class="glyphicon glyphicon-minus"></span>
                                        </asp:LinkButton>
                                        <%#Eval("Cantitate")%>
                                        <asp:LinkButton runat="server"
                                            CssClass="btn btn-primary btn-circle"
                                            ID="AddPreparat"
                                            OnClick="adaugaPreparat"
                                            CommandArgument='<%#Eval("Preparat.Id")%>'>
                                    <span class="glyphicon glyphicon-plus"></span>
                                        </asp:LinkButton>
                                    </span>
                                </li>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="alert alert-info text-center">
                                    Nici un produs adaugat la comanda
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </ul>
                    <div class="pull-right text-center" id="Total" runat="server" visible="false" style="font-weight: bold"></div>
                    <asp:Button ID="TrimitereComanda" OnClick="trimitereComandaClick" runat="server" Visible="false" Style="clear: both" CssClass="btn btn-primary pull-right" Text="Trimite comanda" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="TrimitereComanda" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div id="StatusComanda" class="alert alert-success text-center" visible="false" runat="server">
            Comanda trimisa!
        </div>
    </div>
</asp:Content>

