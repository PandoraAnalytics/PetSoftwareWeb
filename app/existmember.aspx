<%@ Page Title="Association - Exist Member" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="existmember.aspx.cs" Inherits="Breederapp.existmember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Exitmember %>" runat="server" CssClass="error_class"></asp:Label></h5>
      
        <div class="clearfix"></div>
        <br />
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                    <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblExitDate" runat="server" CssClass="form_label"><span><%= Resources.Resource.ExitDate %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtExitDate" runat="server" CssClass="form_input date-picker" data-validate="required date" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="lblExitReason" runat="server" CssClass="form_label"><span><%= Resources.Resource.ExitReason %></span>&nbsp;*</asp:Label>
                            <asp:TextBox ID="txtExitReason" runat="server" TextMode="MultiLine" CssClass="form_input" MaxLength="500" Rows="8" Width="100%" data-validate="required"></asp:TextBox>
                        </div>
                    </div>
                   
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSave_Click" />&nbsp;
                           
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                </div>
            </div>
       
    </div>

    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>

    <script>
        $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });

       
    </script>
</asp:Content>
