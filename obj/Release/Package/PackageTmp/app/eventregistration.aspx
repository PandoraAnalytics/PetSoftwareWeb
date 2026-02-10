<%@ Page Title="Event Registration" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="eventregistration.aspx.cs" Inherits="Breederapp.eventregistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link href="css/fastselect.css" rel="stylesheet" />
    <link href="css/bootstrap-timepicker.min.css" rel="stylesheet" />
    <style>
        .form_input {
            width: 50%;
        }

        .customtable tr td {
            border: 0 !important;
        }

            .customtable tr td label {
                font-weight: normal;
                margin-left: 8px;
            }

        .msg {
            border: 1px solid #fff6ec;
            padding: 8px;
            border-radius: 5px;
            background: #fff6ec;
            font-weight: 600;
        }

        .form_input, .fstElement {
            width: 45%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="lblEventRegistationTitle" Text="<%$Resources:Resource, EventRegistration %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 msg">
                        <asp:Label ID="Label6" Text="<%$Resources:Resource, EventRegistrationMessage %>" runat="server"></asp:Label>&nbsp;
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, Name%>" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, EmailAddress%>" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource, Phone%>" CssClass="form_label"></asp:Label>
                        <asp:Label ID="lblPhone" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label2" Text="<%$Resources:Resource, SelectAnimal%>" runat="server" CssClass="form_label"></asp:Label>
                        <select id="ddlAnimal" multiple runat="server" class="form_input multipleSelect" data-validate="required" datatextfield="animalname" datavaluefield="id">
                        </select>
                    </div>
                </div>
                <asp:Panel ID="pnlCustomFieldsEdit" runat="server">
                </asp:Panel>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnRegisterEvent" CssClass="form_button" Text="<%$Resources:Resource, Register%>" runat="server" OnClick="btnRegisterEvent_Click" />&nbsp;
                        <asp:LinkButton ID="lnkBack" runat="server" Text="<%$Resources:Resource, Back%>" OnClick="lnkBack_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/bootstrap-timepicker.min.js"></script>
    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            $(".time-picker").timepicker({ showInputs: false, showMeridian: false });
        });
        $('.multipleSelect').fastselect({
            placeholder: '<%=  Resources.Resource.ChooseOption %>',
            noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
        });
    </script>

</asp:Content>
