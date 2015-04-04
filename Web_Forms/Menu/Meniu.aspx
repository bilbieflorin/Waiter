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
                        <ul>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li style="list-style-type: none; margin-top: 5px; margin-bottom: 5px;">
                            
                            <!-- Div entry -->
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

                            </div>
                        </li>
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

    <%--<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel" runat="server"></h4>
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

