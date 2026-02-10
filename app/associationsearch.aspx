<%@ Page Title="Search Association" Language="C#" MasterPageFile="user.Master" AutoEventWireup="true" CodeBehind="associationsearch.aspx.cs" Inherits="Breederapp.associationsearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #assocationrow a {
            color: inherit;
        }

        .assocation_wrp {
            border: 1px solid #ddd;
            padding: 5px 5px 10px 5px;
            border-radius: 5px;
            margin-bottom: 15px;
        }

        .assocation_title {
            font-size: 18px;
            font-weight: 500;
            line-height: 1.6;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            margin-bottom: 8px;
        }

        .assocation_bridertype {
            line-height: 1.5;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            -webkit-box-orient: vertical;
        }

        .assocation_img_wrp {
            position: relative;
            border-radius: 4px;
            margin-bottom: 15px;
        }

        .assocation_date {
            position: absolute;
            bottom: 0px;
            left: 0;
            display: inline-block;
            background: #fff6ec;
            width: 100%;
            padding: 4px 8px;
        }

        .password_rules {
            border-radius: 6px;
            border: 1px solid #ddd;
            padding: 3px;
            background: #eff2f7;
            /*margin-top: 8px;*/
            width: 60px;
            max-width: 100%;
            padding-top: 0px;
            font-size: 14px;
            font-weight: bold;
        }

        .rule {
            color: #777;
        }

            .rule.active {
                color: #000 !important;
                font-weight: 600;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, SearchAssociation %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>

        <input type="hidden" id="hidfilter" runat="server" />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp
                      <asp:Label ID="Label1" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;-&nbsp;
                        <asp:Label ID="Label3" Text="<%$Resources:Resource, Name %>" runat="server" CssClass="form_element"></asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_element form_input" data-validate="required" MaxLength="100"></asp:TextBox>
                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource, TypeofBreed%>" CssClass="form_element"></asp:Label>
                        <asp:DropDownList ID="ddlBreedType" runat="server" CssClass="form_element form_input" DataTextField="namewithbreedname" DataValueField="id" Width="30%"></asp:DropDownList>&nbsp;&nbsp;
                        <asp:Button ID="btnApply" CssClass="form_element form_search_button" Text="<%$Resources:Resource, Search %>" runat="server" OnClick="btnApply_Click" />&nbsp;
                        <asp:LinkButton ID="lnkBack" CssClass="form_element" runat="server" OnClick="lnkBack_Click" Text="<%$Resources:Resource, Back %>"></asp:LinkButton>
                        <%--<asp:TextBox ID="TextBox1" runat="server" CssClass="form_input" MaxLength="40" placeholder="Name"></asp:TextBox>
                        <asp:LinkButton ID="btnApply1" CssClass="search_link" runat="server" OnClick="btnApply1_Click">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>--%>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">

            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div id="assocationrow" class="row form_row">
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js?10"></script>

    <script>
        var filter = '';
        var sty = '';
        var NoAssociationAvailable = '<%=  Resources.Resource.Noassociationavailable %>';
        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hidfilter').val();         
            process(1);
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetAllAssociation", pageIndex, filter, '', getAssociateDetails_Success);
        }

        function getAssociateDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();

                    var contents = '<div style="class="col-lg-12 col-md-12 col-sm-12 col-xs-12"><div class="assocation_wrp">';

                    var isownercheck = record.find("isowner").text();
                    var joindt = record.find("procesed_entrydate").text();
                    var requestdt = record.find("procesed_submitdate").text();
                    var ownertext = null;

                    if (isownercheck == 1)
                        contents += '<div class="assocation_title">' + record.find("name").text() + ' <span class="password_rules">Owner</span> </div>';
                    else
                        contents += '<div class="assocation_title">' + record.find("name").text() + '</div>';

                    contents += '<div class="assocation_bridertype">' + record.find("breednames").text() + '</div>';

                    if (joindt.length > 0)
                        contents += "<td><div class='float-end'>Member Since " + joindt + "</div></td>";
                    else if (requestdt.length > 0)
                        contents += "<td><div class='float-end'>Request Sent On " + requestdt + "</div></td>";
                    else {
                        if (isownercheck != 1)
                            contents += "<td><div class='btn btn_edit float-end'><a href='addmember.aspx?aid=" + id + "'  data-toggle='tooltip'  title='Send Join Request'>JOIN<i ></i></a></div></td>";
                    }

                    contents += '<div class="clearfix"></div>';
                    contents += '</div>';
                    contents += '</div>';

                    $('#assocationrow').append(contents);
                });
            }
            else {
                $('#assocationrow').html("<h6>" + NoAssociationAvailable + "</h6>");
            }
        }
    
    </script>
</asp:Content>
