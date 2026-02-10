<%@ Page Title="Brochure - Manage Brochure" Language="C#" MasterPageFile="main.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="brochurepageadd.aspx.cs" Inherits="Breederapp.brochurepageadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .fstElement {
            width: 30%;
        }

        .form_input {
            width: 50%;
        }

        .top_btn {
            margin-right: 5px;
        }

        .error_class {
            display: inline;
        }

        .password_rules {
            border-radius: 4px;
            border: 1px solid #ddd;
            padding: 8px;
            background: #eff2f7;
            margin-top: 8px;
            width: 400px;
            max-width: 100%;
        }

        .rule {
            color: #777;
            padding-bottom: 8px;
        }

            .rule.active {
                color: #000 !important;
                font-weight: 600;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Managebrochures %>" runat="server" CssClass="error_class"></asp:Label>&nbsp;<asp:Label ID="lblHeaderNM" Text="" runat="server"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Pagename %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100" Width="30%"></asp:TextBox>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.Contenttype %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlContentType" runat="server" CssClass="form_input" data-validate="required" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="ddlContentType_SelectedIndexChanged">
                            <asp:ListItem Text="<%$Resources:Resource, Select %>" Value="0"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, TextEditor %>" Value="1"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, AnimalList %>" Value="2"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, OwnerList %>" Value="3"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource, Sponsorlist %>" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <asp:Panel ID="pnlEditor" runat="server" Visible="false">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label6" Text="<%$Resources:Resource, Enteryourtexthere %>" runat="server" CssClass="form_label"></asp:Label>
                            <%--<asp:TextBox ID="txtEditor" runat="server" CssClass="form_input" MaxLength="2000" data-validate="required maxlength-2000" Rows="5" TextMode="MultiLine"></asp:TextBox>--%>
                            <textarea id="txtEditor" class="txtbody" rows="30" style="width: 100%" runat="server"></textarea>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlSponsor" runat="server" Visible="false">
                    <div class="row form_row">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Sponsortype %></span>&nbsp;*</asp:Label>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:DropDownList ID="ddlSponsorType" runat="server" CssClass="form_input" AutoPostBack="true" data-validate="required" Width="30%" OnSelectedIndexChanged="ddlSponsorType_SelectedIndexChanged">
                                <asp:ListItem Text="<%$Resources:Resource, Select %>" Value="0"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Gold %>" Value="1"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Silver %>" Value="2"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource, Bronze %>" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Sponsors %></span>&nbsp;*</asp:Label>
                            <select id="ddlSponsors" multiple runat="server" class="form_input multipleSelect fstElement" data-validate="required" style="width: 30%;" datatextfield="name" datavaluefield="id"></select>
                        </div>
                    </div>

                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <div class="password_rules">
                                <div class="rule">
                                    <asp:Label ID="Label12" Text="<%$Resources:Resource, BrochureImageSpecification %>" runat="server" CssClass=""></asp:Label>
                                </div>
                                <div class="rule">
                                    <asp:Label ID="Label9" Text="<%$Resources:Resource, Bronzetypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                </div>
                                <div class="rule">
                                    <asp:Label ID="Label10" Text="<%$Resources:Resource, Silvertypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                </div>
                                <div class="rule">
                                    <asp:Label ID="Label11" Text="<%$Resources:Resource, Goldtypeimagesize %>" runat="server" CssClass=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlAnimal" runat="server" Visible="false">
                     <asp:Label ID="Label7" Text="<%$Resources:Resource, Alltheanimalswhoregisterforthisevent %>" runat="server" CssClass="form_label"></asp:Label>
                   <%-- <label><%$Resources:Resource, Alltheanimalswhoregisterforthisevent %></label>--%>
                </asp:Panel>

                <asp:Panel ID="pnlOwner" runat="server" Visible="false">
                     <asp:Label ID="Label8" Text="<%$Resources:Resource, Alltheownerswhoregisterforthisevent %>" runat="server" CssClass="form_label"></asp:Label>
                  <%--  <label><%$Resources:Resource, Alltheownerswhoregisterforthisevent %></label>--%>
                </asp:Panel>
                <br />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnAdd_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script src="https://cdn.tiny.cloud/1/1iz4hnja5hu3l1df2g5kx735wdjg8pe7kdv1v38urcs0kzjd/tinymce/6/tinymce.min.js" referrerpolicy="origin"></script>

    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_ddlSponsors').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
                noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
            });

            $('.multipleSelect').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
                noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
            });

            tinymce.init({
                selector: '#ContentPlaceHolder1_txtEditor',
                height: 300,
                plugins: 'table link image imagetools',
                toolbar: 'casechange checklist code formatpainter permanentpen table image imageupload ',
                toolbar_mode: 'floating',
                encoding: 'xml',
                images_upload_url: 'file_upload_docs_tiny.ashx',
                relative_urls: false,
                remove_script_host: false,
                document_base_url: 'https://pets.software/',
                file_picker_types: 'image',
            });
        });

    </script>
</asp:Content>
