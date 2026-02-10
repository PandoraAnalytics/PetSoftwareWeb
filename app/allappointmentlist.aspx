<%@ Page Title="Calendar List" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="allappointmentlist.aspx.cs" Inherits="Breederapp.allappointmentlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table#ContentPlaceHolder1_tblDynamic tr th {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
            vertical-align: top;
            min-height: 76px;
        }

        .date_row {
            min-height: 40px;
        }

            .date_row ul li {
                list-style: none;
                margin-bottom: 3px;
            }

                .date_row .appoin, .date_row .appoin:hover {
                    font-size: 12px;
                    list-style: none;
                    border: solid 1px #efefef;
                    padding: 3px 8px;
                    border-radius: 5px;
                    display: inline-block;
                    color: #333;
                    background: #f8f8f8;
                    font-weight: normal;
                }

                    .date_row .event , .date_row .event:hover {
                        font-size: 12px;
                        list-style: none;
                        border: solid 1px #efefef;
                        padding: 3px 8px;
                        border-radius: 5px;
                        display: inline-block;
                        color: #333;
                        background: #fff6ec;
                        font-weight: normal;
                    }

                        .date_row ul li a img {
                              border-radius: 50%;
    width: 24px;
    height: 24px;
    display: block;
                        }

        .notcurrentmonth {
            background-color: #f9fbfd !important;
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Calendar %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
        </div>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form_input"></asp:DropDownList>&nbsp;
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form_input"></asp:DropDownList>&nbsp;
                        <%-- <asp:LinkButton ID="btnApply" CssClass="search_link" runat="server" OnClick="btnApply_Click">--%>
                         <asp:LinkButton ID="btnApply" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                             <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                         </asp:LinkButton>
                    </div>
                </div>
                <div id="maintablediv">
                    <asp:Table ID="tblDynamic" runat="server" class="table datatable">
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
