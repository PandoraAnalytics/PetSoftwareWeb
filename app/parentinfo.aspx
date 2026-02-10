<%@ Page Title="Breeder - Parent Info" Language="C#" MasterPageFile="breeder.Master" AutoEventWireup="true" CodeBehind="parentinfo.aspx.cs" Inherits="Breederapp.parentinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tree li {
            list-style-type: none;
            margin: 0;
            padding: 10px 5px 0 5px;
            position: relative;
        }

            .tree li a {
            color:#333;
            }

            .tree li::before, .tree li::after {
                content: '';
                left: -16px;
                position: absolute;
                right: auto;
            }

            .tree li::before {
                border-left: 1px solid #999;
                bottom: 50px;
                height: 100%;
                top: 0;
                width: 1px;
            }

            .tree li::after {
                border-top: 1px solid #999;
                height: 20px;
                top: 20px;
                width: 16px;
            }

            .tree li span {
                /*border: 1px solid #999;
                    border-radius: 5px;*/
                display: inline-block;
                padding: 3px 8px;
                text-decoration: none;
            }

            .tree li.parent_li > span {
                cursor: pointer;
            }

        .tree > ul > li::before, .tree > ul > li::after {
            border: 0;
        }

        .tree li:last-child::before {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AddBreedParentDetails %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end ">
            <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" CssClass="edit_profile_link">
                <i class="fa-solid fa-pen-to-square"></i>&nbsp;<asp:Label ID="Label0" runat="server" Text="<%$Resources:Resource, Edit %>"></asp:Label>
            </asp:LinkButton>
        </div>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-8 col-xs-12">
                <asp:Panel ID="panelView" runat="server">
                    <div class="row form_row">
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label2" Text="<%$Resources:Resource, FathersName %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblFathersName" Text="-" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label1" Text="<%$Resources:Resource, MothersName %>" runat="server" CssClass="form_label"></asp:Label>
                            <asp:Label ID="lblMothersName" Text="-" runat="server"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panelEdit" runat="server" Visible="false">
                    <div class="row form_row">
                        <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label3" Text="<%$Resources:Resource, FathersName %>" runat="server" CssClass="form_label"></asp:Label>
                            <input type="text" list="ContentPlaceHolder1_datalist" class="form_input" runat="server" id="txtFathersName" maxlength="50" autocomplete="off" />
                            <datalist runat="server" id="datalist"></datalist>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, MothersName %>" runat="server" CssClass="form_label"></asp:Label>
                            <input type="text" list="ContentPlaceHolder1_datalist" class="form_input" runat="server" id="txtMothersName" maxlength="50" autocomplete="off" />
                            <datalist runat="server" id="datalist1"></datalist>
                        </div>
                    </div>
                    <div class="row form_row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:Button ID="btnAddParents" CssClass="form_button" Text="<%$Resources:Resource, Save %>" runat="server" OnClick="btnSubmit_Click" />&nbsp;
                            <asp:LinkButton ID="btnClose" runat="server" OnClick="btnClose_Click" Text="<%$Resources:Resource, Close %>"></asp:LinkButton>
                        </div>
                    </div>

                </asp:Panel>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">
                <h6>
                    <asp:Label ID="Label6" Text="<%$Resources:Resource, ChildAnimals %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <div id="childtable" class="tree"></div>
            </div>
        </div>
    </div>
    <script src="js/validator.js" type="text/javascript"></script>
    <script src="js/data.js?123"></script>
    <script>
        var filter = '<%= ViewState["id"] %>';
        $(document).ready(function () {
            $('.datatable tbody').html('');
            getdata3("GetAnimalChilds", 1, filter, '', getParents_Success);
        });


        function getParents_Success(data) {
            $('childtable').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var record = records.first();
                var id = record.find("id").text();
                var childarray = findparentid(records, id);
                if (childarray != null && childarray.length > 0) {
                    var tree = '<ul>';
                    tree += '<li>'  + record.find("name").text();
                    tree += '<ul style="margin-left:' + marginleft + 'px">';
                    for (c = 0; c < childarray.length; c++) {
                        tree += buildnode(records, childarray[c]);
                    }
                    tree += '</ul>';

                    tree += '</li>';
                    tree += '</ul>';
                    $('#childtable').html(tree);
                }
                else {
                    $('#childtable').html("-");
                }

            }
            else {
                $('#childtable').html("-");
            }
        }

        var marginleft = 20;
        function buildnode(xmlrecords, childrecord) {
            var temptree = '<li>' + '<a href="parentinfo.aspx?id=' + childrecord.find("id").text() + '">' + childrecord.find("name").text() + '</a>';
            var childarray = findparentid(xmlrecords, childrecord.find("id").text());
            if (childarray != null && childarray.length > 0) {
                temptree += '<ul style="margin-left:' + marginleft + 'px">';
                for (var i = 0; i < childarray.length; i++) {
                    temptree += buildnode(xmlrecords, childarray[i]);
                }
                temptree += '</ul>';
                marginleft += 20;
            }
            temptree += '</li>';


            return temptree;
        }

        function findparentid(xmlrecords, id) {
            var childarray = [];

            xmlrecords.each(function () {
                var record = $(this);
                var fatherid = record.find("fatherid").text();
                var motherid = record.find("motherid").text();
                if (id == fatherid || id == motherid) {
                    if (id == motherid) {
                        var name = record.find("name").text();
                        name += '&nbsp;&nbsp;<i class="fa-solid fa-venus"></i>';
                        record.find("name").text(name);
                    }
                    childarray.push(record);
                }
            });
            return childarray;
        }
    </script>
</asp:Content>
