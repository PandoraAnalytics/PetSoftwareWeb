<%@ Page Title="Edit Association" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="associationedit.aspx.cs" Inherits="Breederapp.associationedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <style>
        .fstElement {
            width: 50%;
        }

        .form_input {
            width: 50%;
        }

        .top_btn {
            margin-right: 5px;
        }

        .members li {
            width: 33.33%;
            float: left;
            margin: 10px 0;
            list-style: none;
        }

        .linkicon .linkicon:hover {
            color: #d9082d;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label2" Text="<%$Resources:Resource, AssociationEdit %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <%-- <a href="associationsearch.aspx" class="edit_profile_link">&nbsp;&nbsp;<asp:Button ID="btnAccept" CssClass="form_button" Text="<%$Resources:Resource, Accept %>" runat="server" OnClick="btnAccept_Click" />
            </a>--%>
            <a runat="server" id="lnkManageMemeber" href="memberlist.aspx?aid=<%# AssociationId %>" class="add_form_btn btnborder">
                <i class="fa-solid fa-user-plus"></i>&nbsp;&nbsp;<asp:Label ID="Label10" Text="<%$Resources:Resource, ManageMembers %>" runat="server"></asp:Label>
            </a>
        </div>
        <%--<%# string.Format("memberlist.aspx?aid={0}", BABusiness.BusinessSecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey)) %>--%>
        <div class="clearfix"></div>

        <div class="row form_row">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form_input" data-validate="required" MaxLength="100"></asp:TextBox>

            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label7" Text="<%$Resources:Resource, TypeofBreed %>" runat="server" CssClass="form_label"></asp:Label>
                <select id="ddlBreedType" class="multipleSelect" multiple runat="server" datatextfield="namewithbreedname" datavaluefield="id"></select>
                <span class="rule">
                    <asp:Label ID="Label6" Text="<%$Resources:Resource, Ifyouwanttoapplyforalltypes %>" runat="server"></asp:Label></span>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Phone %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form_input" data-validate="required phone" MaxLength="20"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label5" runat="server" CssClass="form_label"><span><%= Resources.Resource.EmailAddress %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtEmailAddress" runat="server" data-validate="required email" CssClass="form_input" MaxLength="255"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="lblMandatory" Text="<%$Resources:Resource, Address %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form_input" TextMode="Multiline" MaxLength="255"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label1" Text="<%$Resources:Resource, Website %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form_input" MaxLength="255"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
                <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />
            </div>
        </div>
        <br />
      <%--  <br />
        <h6>
            <asp:Label ID="Label8" Text="<%$Resources:Resource, Members %>" runat="server" CssClass="error_class"></asp:Label>

            <a href='<%# string.Format("memberlist.aspx?aid={0}", BABusiness.BusinessSecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey)) %>' style="color: #777;">
                <i class="fa-solid fa-user-plus"></i>&nbsp;&nbsp;<asp:Label ID="Label9" Text="<%$Resources:Resource, ManageMembers %>" runat="server"></asp:Label>
            </a>

        </h6>--%>
        <%--<div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <ul class="members">
                    <asp:Repeater ID="repeaterMembers" runat="server">
                        <HeaderTemplate>
                            <li>
                                <div>
                                    <a href='<%# string.Format("assignassociate.aspx?assoid={0}", BABusiness.BusinessSecurity.Encrypt(ViewState["id"].ToString(), Breederapp.PageBase.HashKey)) %>' style="color: #777;">
                                        <i class="fa-solid fa-circle-plus"></i>&nbsp;&nbsp;
                                        <asp:Label ID="Label1" Text="<%$Resources:Resource, AddNewMember %>" runat="server"></asp:Label>
                                    </a>
                                    &nbsp;&nbsp;
                                </div>
                                <span style="opacity: 0.7; margin-top: 3px;">
                                    <%# Eval("email") %> 
                                </span>
                            </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div>
                                    <a href="javascript:void(0);" onclick='<%# string.Format("deleteRecord({0});", Eval("id")) %>'>

                                        <i class="fa-solid fa-circle-minus"></i>
                                    </a>
                                    &nbsp;&nbsp;
                                    <%# Eval("name") %>
                                </div>
                                <span style="opacity: 0.7; margin-top: 3px;">
                                    <%# Eval("email") %> 
                                </span>
                            </li>
                        </ItemTemplate>

                    </asp:Repeater>
                </ul>
                <div class="clearfix"></div>
            </div>
        </div>--%>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        $('.multipleSelect').fastselect({
            placeholder: '<%=  Resources.Resource.ChooseOption %>',
            noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
        });

        function deleteRecord(id) {
            deleteData("RemoveBreederFromAssocation", id);
            var u = window.location.href;
            window.location = u;
        }

    </script>
</asp:Content>
