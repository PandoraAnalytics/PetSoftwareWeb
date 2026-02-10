<%@ Page Title="Breed - Breeder Details" Language="C#" MasterPageFile="breeder.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="breederinfo.aspx.cs" Inherits="Breederapp.breederinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .FormatRadioButtonList td {
            padding-bottom:6px;
        }

        .FormatRadioButtonList label {
            padding-left: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Breederdetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>

        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <h6><asp:Label ID="Label8" Text="<%$Resources:Resource, Definitionofbreeder %>" runat="server"></asp:Label></h6>
                 <asp:Label ID="Label10" Text="<%$Resources:Resource, Alldefinitionofbreeder %>" runat="server"></asp:Label>
               <%-- An animal breeder specializes in selectively breeding animals to produce offspring with desired traits. Whether working with dogs, horses, livestock, or other animals, 
                    breeders carefully choose parent animals that possess specific characteristics, such as size, color, temperament, or performance abilities. 
                    By mating these animals, they aim to create offspring that inherit and express those desired traits.--%>
            </div>
        </div>
        <br />
        <asp:Panel ID="panelView" runat="server" Visible="false">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, Breedername %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblBreederName" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Breederemail %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblBreederEmail" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNoData" runat="server" Visible="false">
            <h6><asp:Label ID="Label9" Text="<%$Resources:Resource, Breederinformationisnotavailable %>" runat="server"></asp:Label></h6>
        </asp:Panel>

        <asp:Panel ID="panelEdit" runat="server" Visible="false">
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                     <asp:Label ID="Label12" Text="<%$Resources:Resource, Whoisbreederofthisanimal %>" runat="server"></asp:Label>
                   <%-- Who is breeder of this animal?--%>
                    <br />
                    <asp:RadioButtonList ID="rdbBreederType" runat="server" CssClass="FormatRadioButtonList" RepeatColumns="1" AutoPostBack="true" RepeatDirection="Vertical" OnSelectedIndexChanged="rdbBreederType_SelectedIndexChanged">
                        <asp:ListItem Text="<%$Resources:Resource, Myself %>" Value="1"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Idontknow %>" Value="2"></asp:ListItem>
                        <asp:ListItem Text="<%$Resources:Resource, Iknow %>" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <asp:Panel ID="pnlSearchBreeder" runat="server" Visible="false">
                <asp:Label ID="Label3" Text="<%$Resources:Resource, Searchbreederviaemailaddress %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:TextBox ID="txtSearchBreeder" runat="server" CssClass="form_input" MaxLength="200"></asp:TextBox>
                <a href="javascript:void(0);" onclick="SearchBreeder();">
                    <asp:Label ID="lbl" runat="server" Text="<%$Resources:Resource, Searchbreeder %>"></asp:Label></a>
            </asp:Panel>

            <asp:Panel ID="pnlViewBreederInfo" runat="server" Visible="false" Style="padding-top: 20px;">
                <div class="row">
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label4" Text="<%$Resources:Resource, Breedername %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblViewUserName" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label6" Text="<%$Resources:Resource, Breederemail %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblViewUserEmail" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label13" Text="<%$Resources:Resource, Breedermobile %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:Label ID="lblViewUserMobile" Text="-" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlEditBreederInfo" runat="server" Visible="false" Style="padding-top: 20px;">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                         <asp:Label ID="Label14" Text="<%$Resources:Resource, Emailaddressnotfound %>" runat="server"></asp:Label>
                       <%-- We haven’t found this email address in our database. Please add the details.--%>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label5" Text="<%$Resources:Resource, Breedername %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtEditBreederName" runat="server" CssClass="form_input" MaxLength="200"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row form_row">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Breederemail %>" runat="server" CssClass="form_label"></asp:Label>
                                <asp:TextBox ID="txtEditBreederEmail" runat="server" CssClass="form_input" MaxLength="200"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </asp:Panel>
        <br />
        <asp:Panel ID="pnlSubmitBtn" runat="server" Visible="false" Style="padding-top: 20px;">
            <div class="row form_row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <asp:Button ID="btnSaveBreed" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;                    
                </div>
            </div>
        </asp:Panel>

    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script>

        function SearchBreeder() {
            var emailval = $("#ContentPlaceHolder1_txtSearchBreeder").val();
            var emailaddress = '<%= Resources.Resource.Pleaseenteremailaddrestosearchbreederinfo %>';
            if (emailval.length <= 0)
                alert(emailaddress);
            else
                __doPostBack('search', emailval);
        }
    </script>
</asp:Content>
