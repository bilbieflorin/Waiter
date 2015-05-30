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

                    <div id="potriviriComandaCarousel" runat="server" class="carousel slide" data-ride="carousel" style="width: 70%; height: 220px; margin: auto;" visible="false">
                        <div class="carousel-inner" role="listbox">
                            <asp:Repeater ID="carouselRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                        <asp:LinkButton runat="server"
                                            ID="carouselItemLinkButton"
                                            OnClick="carouselItemImageClick"
                                            CommandName="Id"
                                            CommandArgument='<%# Eval("Id") %>'>
                                            <img
                                            alt="ImaginePreparat"
                                            src='<%# Eval("PathImagine") %>'
                                            class="img-thumbnail img-responsive"
                                            style="width: 275px; max-height: 200px;" />
                                        <div class="carousel-caption" style="max-width: 275px">
                                            <h3><%# Eval("Denumire") %></h3>
                                        </div>
                                        </asp:LinkButton>

                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <a class="left carousel-control" href="#potriviriComandaCarousel" role="button" data-slide="prev">
                            <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                            <%--<span class="sr-only">Previous</span>--%>
                        </a>
                        <a class="right carousel-control" href="#potriviriComandaCarousel" role="button" data-slide="next">
                            <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                            <%--<span class="sr-only">Next</span>--%>
                        </a>
                    </div>

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

    <!-- Bootstrap Modal Dialog -->
    <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="ModalUpdatePanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title text-center">
                                <asp:Label ID="ModalItemTitle" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body" style="text-align: center;">
                            <div>
                                <asp:Image ID="ModalItemImage" runat="server"
                                    Style="width: 90%; height: auto;" />
                            </div>

                            <%--<asp:TextBox ID="ModalItemBody" runat="server"
                                ReadOnly="true"
                                BorderStyle="None"
                                TextMode="MultiLine"
                                Style="width: 100%; height: 100%; resize: none;">
                            </asp:TextBox>--%>
                            <p id="ModalItemBody" runat="server"></p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="ButtonComanda" OnClick="buttonComandaClick" runat="server" CommandName="IdPreparat" CssClass="btn btn-primary" Text="Comanda" />
                            <button class="btn btn-default" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>

