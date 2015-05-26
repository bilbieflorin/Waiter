<%@ Page Title="" Language="C#" MasterPageFile="../Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Recomandari.aspx.cs" Inherits="Recomandari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1 class="text-center">Recomandari</h1>
    <h4 class="text-left">Alti utilizatori au preferat si: </h4>
        <div>
        <asp:UpdatePanel ID="MeniuUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:ListView ID="RecomandariListView" runat="server" OnPagePropertiesChanging="recomandariListViewPagePropertiesChanging">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>

                        <div class="row">
                            <div class="col-md-1 col-sm-1 col-xs-2" style="/*border: 1px solid blue; */">
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-8 container-fluid" style="/*border: 1px solid black; */">

                                <!-- Item -->
                                <div class="panel <%--div-md-sm-panel div-xs-panel--%>" style="background-color: #E6B280; /*border: 1px solid yellow; */">
                                    <div class="row">
                                        <!-- Imagine -->
                                        <div class="col-md-5 col-sm-5 div-md-sm-center-item div-xs-center-item" style="/*border: 1px solid purple; */">
                                            <div class="div-md-sm-center-item-image" style="height: 100%; width: 100%; white-space: nowrap; /*background-color: red; */">
                                                <span style="height: 100%; vertical-align: middle;"></span>
                                                <asp:ImageButton ID="ImageButton1" runat="server"
                                                    AlternateText="ImaginePreparat"
                                                    ImageUrl='<%# Eval("PathImagine") %>'
                                                    OnClick="recomandariListItemImagineClick"
                                                    CommandName="DisplayIndex"
                                                    data-toggle="tooltip" 
                                                    data-placement="top" 
                                                    title="Dati click pentru a comanda"
                                                    CommandArgument="<%# Container.DisplayIndex %>"
                                                    CssClass="img-thumbnail img-responsive img-md-sm-center-item img-xs-center-item"
                                                    Style="width: 178px; max-height: 125px;" />
                                            </div>
                                        </div>
                                        <div class="col-md-7 col-sm-7 div-md-sm-center-item div-xs-center-item" style="/*border: 1px solid pink; */">
                                            <h4><%# Eval("Denumire") %></h4>
                                            <div class="div-md-sm-item-details div-xs-item-details">
                                                <p><%# Eval("Tip", "Tip: {0}") %></p>
                                                <p><%# Eval("Specific", "Specific: {0}") %></p>
                                            </div>
                                            <p><%# Eval("Pret", "{0} RON") %></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1 col-sm-1 col-xs-2" style="/*border: 1px solid red; */"></div>
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No data
                    </EmptyDataTemplate>
                </asp:ListView>
                <div class="row-fluid text-center" style="margin-bottom: 10px">
                    <asp:DataPager ID="RecomandariDataPager" runat="server" PagedControlID="RecomandariListView" PageSize="1">
                        <Fields>
                            <asp:NextPreviousPagerField PreviousPageText="Inapoi" 
                                 ShowNextPageButton="false" ShowLastPageButton="false" ShowPreviousPageButton="true"
                                ButtonCssClass="btn btn-primary" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                            <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="btn btn-primary active nbsp" RenderNonBreakingSpacesBetweenControls="false"
                                NumericButtonCssClass="btn btn-default" ButtonCount="1" NextPageText="..." NextPreviousButtonCssClass="none" />
                            <asp:NextPreviousPagerField NextPageText="Inainte" ShowNextPageButton="true"
                                 ShowPreviousPageButton="false" ShowFirstPageButton="false"
                                ButtonCssClass="btn btn-primary" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Bootstrap Modal Dialog -->
    <div class="modal fade" id="myModal"  role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
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

