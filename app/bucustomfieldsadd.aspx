<%@ Page Title="Add Other Edit" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucustomfieldsadd.aspx.cs" Inherits="Breederapp.bucustomfieldsadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/fastselect.css" rel="stylesheet" />
    <link href="css/datepicker3.css" rel="stylesheet" />
    <style>
        .fstElement {
            width: 50%;
        }

        .form_input {
            width: 50%;
        }

        .table > tbody > tr > td, .table > tbody > tr > th {
            border: none;
            padding-left: 0;
        }

        .top_btn {
            margin-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">

        <h5 class="float-start">
            <asp:Label ID="Label2" Text="<%$Resources:Resource, AddOtherEdit %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>


        <div class="row form_row">
            <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.Type %></span>&nbsp;*</asp:Label>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form_input" data-validate="required" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label7" Text="<%$Resources:Resource, TypeofBreed %>" runat="server" CssClass="form_label"></asp:Label>
                <select id="ddlBreedType" class="multipleSelect" multiple runat="server" datatextfield="namewithbreedname" datavaluefield="id"></select>
                <span class="rule">
                    <asp:Label ID="Label1" Text="<%$Resources:Resource, Ifyouwanttoapplyforalltypes %>" runat="server" CssClass="form_label"></asp:Label></span>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.Name %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtQTitle" CssClass="form_input" runat="server" data-validate="required" MaxLength="100"></asp:TextBox>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="Label8" Text="<%$Resources:Resource, IsMandatory %>" runat="server" CssClass="form_label"></asp:Label>
                <asp:DropDownList ID="ddlIsMandatory" runat="server" CssClass="form_input">
                    <asp:ListItem Text="<%$Resources:Resource, No %>" Value="1"></asp:ListItem>
                    <asp:ListItem Text="<%$Resources:Resource, Yes %>" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <asp:Panel ID="panelList" runat="server" Visible="false">
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption1" Text="<%$Resources:Resource, Option1 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal1" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption2" Text="<%$Resources:Resource, Option2 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal2" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
            </div>

            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption3" Text="<%$Resources:Resource, Option3 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal3" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption4" Text="<%$Resources:Resource, Option4 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal4" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
            </div>

            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption5" Text="<%$Resources:Resource, Option5 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal5" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption6" Text="<%$Resources:Resource, Option6 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal6" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
            </div>

            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption7" Text="<%$Resources:Resource, Option7 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal7" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblOption8" Text="<%$Resources:Resource, Option8 %>" runat="server" CssClass="form_label"></asp:Label>
                    <asp:TextBox ID="txtVal8" runat="server" CssClass="form_input" MaxLength="250"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="panelRange" runat="server" Visible="false">
            <div class="row form_row">
                <div class="form_label col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblMinValue" runat="server" CssClass="form_label"><span><%= Resources.Resource.MinValue %></span>&nbsp;* </asp:Label>
                    <asp:TextBox ID="txtRangeMinVal" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                </div>
            </div>

            <div class="row form_row">
                <div class="form_label col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="lblMaxValue" runat="server" CssClass="form_label"><span><%= Resources.Resource.MaxValue %></span>&nbsp;* </asp:Label>
                    <asp:TextBox ID="txtRangeMaxVal" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="panelMatrix" runat="server" Visible="false">
            <div class="row form_row">
                <div class="form_label col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.NumberofRows %></span>&nbsp;* 
                    </asp:Label>
                    <asp:TextBox ID="txtRowsValue" runat="server" CssClass="form_input" data-validate="required number" MaxLength="4"></asp:TextBox>
                </div>
            </div>
            <div class="row form_row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <asp:Label ID="Label5" runat="server" CssClass="form_label"><span><%= Resources.Resource.ColumnName %></span>&nbsp;* 
                    </asp:Label>
                    <asp:TextBox ID="txtColumnVal1" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>&nbsp;
                                                <asp:Button ID="btnAddColumn" runat="server" Text="Add More Column" CssClass="form_button top_btn" validate="no" OnClick="btnAddTxtBox_Click" />
                </div>
            </div>

            <asp:Panel ID="pnlMatrixTextBoxes" runat="server">
            </asp:Panel>

        </asp:Panel>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:LinkButton ID="btnBack" runat="server" Text="<%$Resources:Resource, Back %>" CssClass="top_btn" OnClick="btnBack_Click"></asp:LinkButton>
                <asp:Button ID="btnSubmitStay" runat="server" Text="<%$Resources:Resource, SaveAddNewField %>" CssClass="form_button top_btn" OnClick="btnSubmitStay_Click" Visible="false" />
                <asp:Button ID="btnSubmitBack" runat="server" Text="<%$Resources:Resource, SaveBacktoList %>" CssClass="form_button top_btn" OnClick="btnSubmitBack_Click" Visible="false" />

            </div>
        </div>
    </div>

    <script src="js/validator.js?101" type="text/javascript"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <%-- <script src="js/validator.js" type="text/javascript"></script>--%>
    <script src="js/data.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>

    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            $('.multipleSelect').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
               noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
           });
        });
    </script>
</asp:Content>
