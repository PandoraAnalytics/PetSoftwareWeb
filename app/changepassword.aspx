<%@ Page Title="Change Password" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="Breederapp.changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form_input {
            width: 30%;
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
            <asp:Label ID="lblChangePassword" Text="<%$Resources:Resource, ChangePassword %>" runat="server" CssClass="error_class"></asp:Label></h5>

        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.OldPassword %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtOldPassword" runat="server" CssClass="form_input" TextMode="Password" MaxLength="30" data-validate="required"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.NewPassword %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form_input" TextMode="Password" MaxLength="20" data-validate="required"></asp:TextBox>
                        <div class="password_rules">
                            <div class="rule" id="rule_min_length">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Minimum6characters %>" runat="server" CssClass=""></asp:Label>
                            </div>
                            <div class="rule" id="rule_isnumberrequired">
                                <asp:Label ID="Label5" Text="<%$Resources:Resource, Atleastonenumber %>" runat="server" CssClass=""></asp:Label>
                            </div>
                            <div class="rule" id="rule_islowercaserequired">
                                <asp:Label ID="Label6" Text="<%$Resources:Resource, Atleastonelowercase %>" runat="server" CssClass=""></asp:Label>
                            </div>
                            <div class="rule" id="rule_isuppercaserequried">
                                <asp:Label ID="Label7" Text="<%$Resources:Resource, Atleastoneuppercase %>" runat="server" CssClass=""></asp:Label>
                            </div>
                            <div class="rule" id="rule_isnumberspecialcharrequired">
                                <asp:Label ID="Label8" Text="<%$Resources:Resource, Atleastonespecialcharacter %>" runat="server" CssClass=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.ReenterPassword %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtNewPassword2" runat="server" CssClass="form_input" TextMode="Password" MaxLength="30" data-validate="required"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnSave" CssClass="form_button" Text="<%$Resources:Resource, ChangePassword %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        var typingTimer1;
        $('#ContentPlaceHolder1_txtNewPassword').on('input propertychange paste', function () {
            clearTimeout(typingTimer1);
            typingTimer1 = setTimeout(validatepassword, 800);
        });

        function validatepassword() {
            $('#rule_min_length').removeClass('active');
            $('#rule_isnumberrequired').removeClass('active');
            $('#rule_islowercaserequired').removeClass('active');
            $('#rule_isuppercaserequried').removeClass('active');
            $('#rule_isnumberspecialcharrequired').removeClass('active');

            var pwd = $.trim($("#ContentPlaceHolder1_txtNewPassword").val());
            if (pwd.length == 0) return;

            $.ajax({
                type: "POST",
                url: "changepassword.aspx/CheckPasswordKeys",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ 'password': pwd }),
                success: function (msg) {
                    if (msg && msg.d && msg.d.length > 0) {
                        var keys = msg.d.split(',');
                        for (var x = 0; x < keys.length; x++) {
                            $('#rule_' + keys[x] + '').addClass('active');
                        }
                    }
                }
            });
        }
    </script>
</asp:Content>
