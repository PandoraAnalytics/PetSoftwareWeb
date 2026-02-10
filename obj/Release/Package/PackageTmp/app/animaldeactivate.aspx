<%@ Page Title="Deactivate Profile" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="animaldeactivate.aspx.cs" Inherits="Breederapp.animaldeactivate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Deactivate %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>

                <input type="radio" id="deactivate" name="delete_group" value="deactivate" runat="server" />&nbsp;&nbsp;&nbsp;
                <label for="ContentPlaceHolder1_deactivate">
                     <asp:Label ID="Label10" Text="<%$Resources:Resource, Deactivateyouranimal %>" runat="server"></asp:Label>
                    <%--Deactivate your animal--%>
                </label>
                <div style="margin-top: 5px; opacity: 0.7">
                     <asp:Label ID="Label1" Text="<%$Resources:Resource, Thisistemporary %>" runat="server"></asp:Label>
                    <%--This is temporary--%>
                        <br />
                     <asp:Label ID="Label2" Text="<%$Resources:Resource, Animalprofileandphotoswillnotberemoved %>" runat="server"></asp:Label>
                    <%--Animal profile and photos will not be removed. Reactivate your animal any time.--%>
                </div>
                <br />
                <br />
                <input type="radio" id="delete" name="delete_group" value="delete" runat="server" />&nbsp;&nbsp;&nbsp;
                <label for="ContentPlaceHolder1_delete"><%--Delete your animal--%>
                     <asp:Label ID="Label6" Text="<%$Resources:Resource, Deleteyouranimal %>" runat="server"></asp:Label>
                </label>
                <div style="margin-top: 5px; opacity: 0.7">
                     <asp:Label ID="Label3" Text="<%$Resources:Resource, Thisispermanent %>" runat="server"></asp:Label>
                    <%--This is permanent--%><br />
                     <asp:Label ID="Label4" Text="<%$Resources:Resource, Theanimalwillnolongerbeavailableandalldatawillbepermanentlydeleted %>" runat="server"></asp:Label>
                    <%--The animal will no longer be available and all data will be permanently deleted. This can't be undone.--%>
                </div>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Label ID="Label7" runat="server" CssClass="form_label"><span><%= Resources.Resource.Reason %></span>&nbsp;*</asp:Label>
                <asp:TextBox ID="txtReason" runat="server" CssClass="form_input" MaxLength="500" data-validate="required maxlength-500" Rows="5" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="row form_row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <asp:Button ID="btnSubmit" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>

    <script src="js/validator.js?100" type="text/javascript"></script>


    <script>
        $("#ContentPlaceHolder1_btnSubmit").click(function () {
            var confirm_msg = '<%=  Resources.Resource.PerformActionMessage %>';
            var answer = confirm(confirm_msg);
            return answer;
        });

    </script>
</asp:Content>
