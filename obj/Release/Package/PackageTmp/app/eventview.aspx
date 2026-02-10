<%@ Page Title="Event View" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="eventview.aspx.cs" Inherits="Breederapp.eventview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .default_banner {
            background-color: #f3f3f3;
            height: 250px;
            border-radius: 5px;
            position: relative;
            text-align: center;
            background-size: cover;
            background-repeat: no-repeat;
            border: 1px solid #ddd;
            background-image: url('../app/images/default_banner_image.png');
        }

        .overlay {
            position: absolute;
            top: 0;
            left: 0;
            background-color: #333;
            z-index: 200;
            right: 0;
            width: 100%;
            opacity: 0.4;
            bottom: 0;
        }

        .default_banner h3 {
            position: absolute;
            bottom: 10px;
            left: 10px;
            color: #fff;
            z-index: 999;
        }

        .photos {
            /*margin-bottom: 25px;*/
        }

            .photos li {
                display: inline-block;
                padding: 10px;
                position: relative;
            }

                .photos li .photowrapper {
                    display: inline-block;
                    /* min-width: 120px; */
                    border: solid 1px #ddd;
                    text-align: center;
                    padding: 2px 2px;
                    color: #333;
                    border-radius: 5px;
                    height: 100px;
                    width: auto;
                }

                .photos li .photo {
                    width: 130px;
                    background-repeat: no-repeat;
                    background-size: contain;
                    background-repeat: no-repeat;
                    background-position: center;
                    height: auto !important;
                    height: 100%;
                    min-height: 100%;
                }

                .photos li .cross {
                    position: absolute;
                    top: 0px;
                    right: 0px;
                    border: 0;
                    border-radius: 50%;
                    min-width: 0;
                    background-color: #d64651;
                    color: #fff;
                    padding: 1px 6px;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="default_banner" runat="server" id="default_banner">
                    <div class="overlay">&nbsp;</div>
                    <h3>
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    </h3>
                </div>
            </div>
        </div>
        <br />
        <div class="registration_section">
            <div class="row form_row">
                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-12">
                    <div>
                        <h5>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, Clickheretoregisterforthisevent%>"></asp:Label></h5>
                    </div>
                    <div>
                        <i class="fa-solid fa-heart" style="color: #d64651;"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRegisteredCount" runat="server"></asp:Label>&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, Peoplehaveregisteredforthisevent%>"></asp:Label>
                    </div>
                </div>
                <div class="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                    <asp:Button ID="btnRegisterEvent" CssClass="form_button" Text="<%$Resources:Resource, Register%>" runat="server" OnClick="btnRegister_Click" Visible="false" />
                    <%--  <asp:Button ID="btnRegisterEvent2" CssClass="form_button" Text="<%$Resources:Resource, AlreadyRegistered%>" runat="server" Enabled="false" Visible="false" />--%>
                    <asp:Button ID="btnDeRegisterEvent" CssClass="form_button" Text="DeRegister" runat="server" Enabled="true" Visible="false" OnClick="btnDeRegisterEvent_Click" />
                </div>
            </div>
        </div>
        <br />
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource, Description%>" CssClass="error_class"></asp:Label></h6>
                <asp:Label ID="lblDescription" runat="server">-</asp:Label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, DateTimeVenue%>" CssClass="error_class"></asp:Label></h6>
                <i class="fa-solid fa-calendar-days"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDate" runat="server">-</asp:Label>
                &nbsp;&nbsp;&nbsp;
                <i class="fa-sharp fa-solid fa-location-pin"></i>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblVenue" runat="server">-</asp:Label>
            </div>
        </div>
        <br />
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource, RuleandCondition%>" CssClass="error_class"></asp:Label></h6>

                <asp:Literal ID="litRules" runat="server"></asp:Literal>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <h6>
                    <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource, Brochurelist%>" CssClass="error_class"></asp:Label></h6>
                <ul class="photos">
                    <asp:Repeater ID="rptrBrochure" runat="server">
                        <ItemTemplate>
                            <li>
                                <a href='exportbrochure.aspx?bid=<%#Eval("new_id")%>' target="_blank" class='cross' title="Click here to open."><i class='fa fa-external-link'></i></a>
                                <a href="exportbrochure.aspx?bid=<%#Eval("new_id")%>" target="doc" class="photowrapper" title="Click here to open.">
                                    <div class="photo" style='<%# (Eval("name").ToString() != "") ? "background-image:url(images/document.png)": string.Format("background-image:url(docs/{0})", Eval("name")) %>'>
                                        &nbsp;
                                    </div>
                                    <br />
                                    <%#  Eval("name").ToString().Substring(Eval("name").ToString().IndexOf("_") + 1) %>
                                </a>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            <br />
                            &nbsp;
                        </FooterTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>

        <asp:Panel ID="panelView" runat="server">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <ul class="photos">
                        <asp:Repeater ID="repEventPhotos" runat="server" OnItemCommand="repeaterFiles_ItemCommand">
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="lnkdelete" runat="server" CssClass="cross" OnClientClick="return confirm_delete();" CommandName="delete" CommandArgument='<%# Eval("file") %>' data-toggle='tooltip' data-placement='top' data-original-title='Delete' Text="<i class='fa fa-times'></i>"></asp:LinkButton>                                    
                                    <a href='<%# string.Format("../app/viewdocument.aspx?file={0}", Eval("file")) %>' target="_blank">
                                        <img src='../images/image_loading.gif' class="lazy-img photo" data-src='<%# Eval("file") %>' alt="photo" />
                                       <%-- <br />
                                        &nbsp;<%#  Eval("file").ToString().Substring(Eval("file").ToString().IndexOf("_") + 1) %> &nbsp;--%>
                                    </a>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                <br />
                                &nbsp;
                            </FooterTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </asp:Panel>
    </div>
    <script src="js/data.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            loadLazyImages();
        });
        function confirm_delete() {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        }

    </script>
</asp:Content>
