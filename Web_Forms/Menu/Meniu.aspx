<%@ Page Title="" Language="C#" MasterPageFile="../Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Meniu.aspx.cs" Inherits="Meniu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">





    <%--<button type="button" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal">
        Launch demo modal
    </button>


    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                </div>
                <div class="modal-body">
                    ...
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
    </div>--%>


    <asp:ScriptManager EnablePartialRendering="true" ID="MeniuScriptManager" runat="server"></asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="MeniuUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:ListView ID="MeniuListView" runat="server" OnPagePropertiesChanging="MeniuListView_PagePropertiesChanging">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>

                        <div class="row">
                            <div class="col-md-1 col-sm-1 col-xs-2" style="/*border: 1px solid blue;*/">
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-8 container-fluid" style="/*border: 1px solid black;*/">

                                <!-- Item -->
                                <div class="panel <%--div-md-sm-panel div-xs-panel--%>" style="background-color: mediumaquamarine; /*border: 1px solid yellow;*/">
                                    <div class="row">
                                        <!-- Imagine -->
                                        <div class="col-md-5 col-sm-5 div-md-center-item div-sm-center-item div-xs-center-item" style="/*border: 1px solid purple;*/">
                                            <div style="height: 100%; width: 100%; white-space: nowrap; /*background-color: red;*/">
                                                <span style="height: 100%; vertical-align: middle;"></span>
                                                <asp:ImageButton ID="ImageButton1" runat="server"
                                                    AlternateText="ImaginePreparat"
                                                    ImageUrl='<%# Eval("PathImagine") %>'
                                                    OnClick="MeniuListItem_ImagineClick"
                                                    CommandName="DisplayIndex"
                                                    CommandArgument="<%# Container.DisplayIndex %>"
                                                    CssClass="img-thumbnail img-responsive img-md-center-item img-sm-center-item img-xs-center-item"
                                                    Style="width: 178px; max-height: 125px;" />
                                            </div>
                                        </div>
                                        <div class="col-md-7 col-sm-7 div-md-center-item div-sm-center-item div-xs-center-item" style="/*border: 1px solid pink;*/">
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
                            <div class="col-md-1 col-sm-1 col-xs-2" style="/*border: 1px solid red;*/"></div>
                        </div>

                        <%--<!-- Div entry -->
                            <div class="container-fluid" style="width: 90%; height: 150px; background-color: mediumpurple;">
                                
                                <!-- Div imagine -->
                                <div class="pull-left" style="height: 100%; width: 185px; white-space: nowrap;">
                                    <span style="display: inline-block; height: 100%; vertical-align: middle;"></span>
                                    <asp:ImageButton ID="ImageButton1" runat="server"
                                        AlternateText="ImaginePreparat"
                                        ImageUrl='<%# Eval("PathImagine") %>'
                                        OnClick="MeniuListView_DetaliiPreparat"
                                        CommandName="DisplayIndex"
                                        CommandArgument="<%# Container.DisplayIndex %>"
                                        CssClass="img-thumbnail"
                                        style="vertical-align: middle; max-width: 178px; max-height: 136px;" />
                                </div>

                                <div class="pull-right">
                                    <%# Eval("Denumire") %>
                                </div>

                            </div>--%>

                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No data
                    </EmptyDataTemplate>
                </asp:ListView>
                <asp:DataPager ID="MeniuDataPager" runat="server" PagedControlID="MeniuListView" PageSize="5">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Link" />
                    </Fields>
                </asp:DataPager>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Bootstrap Modal Dialog -->
    <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title text-center">
                                <asp:Label ID="lblModalTitle" runat="server" Text=""></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body" style="text-align:center;">
                            <div>
                                <asp:Image ID="MeniuModalImage" runat="server"
                                    Style="width: 90%; height: auto;" />
                            </div>
                            

                            <asp:Label ID="lblModalBody" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>

