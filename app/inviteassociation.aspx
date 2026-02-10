<%@ Page Title="Invite Member" Language="C#" MasterPageFile="user.master" AutoEventWireup="true" CodeBehind="inviteassociation.aspx.cs" Inherits="Breederapp.inviteassociation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .invitationcode {
            border-radius: 4px;
            border: 1px solid #ddd;
            padding: 8px;
            background: #eff2f7;
            margin-top: 10px;
        }

        .hidden_text {
            width: 0;
            visibility: hidden;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="lblHeading" Text="" runat="server" CssClass="error_class"></asp:Label></h5>

        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,  Anyonecanbecomeamemberusingsharelink %>"></asp:Label>
                        <%-- Anyone can become a member of the association by using this share link. Participants need approval from admin to join this association.--%>
                        <div class="invitationcode">
                            <asp:Label ID="lblLink" runat="server"></asp:Label>
                        </div>
                        <asp:HiddenField ID="hid" runat="server" />
                    </div>
                </div>
                <br />
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <a href="javascript:void(0);" onclick="copylink();" class="add_form_btn">
                            <asp:Label ID="Label5" Text="<%$Resources:Resource, Copylink %>" runat="server"></asp:Label></a>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkReset" runat="server" CssClass="add_form_btn" OnClick="lnkReset_Click">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, Resetlink %>" runat="server"></asp:Label></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function copylink() {
            var copyText = $("#ContentPlaceHolder1_hid").val();

            var textArea = document.createElement("textarea");
            textArea.textContent = copyText;
            document.body.appendChild(textArea);
            textArea.focus();
            textArea.className = "hidden_text";
            textArea.select();

            navigator.clipboard.writeText(copyText).then(function () {
                // document.body.removeChild(textArea);
                alert('Copied');
            }, function (err) {
                document.body.removeChild(textArea);
                alert('error');
            });
        }


        $("#ContentPlaceHolder1_lnkReset").click(function () {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        });
    </script>
</asp:Content>
