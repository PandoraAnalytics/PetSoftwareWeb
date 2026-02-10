<%@ Page Title="Breed - Notes List" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="noteslist.aspx.cs" Inherits="Breederapp.NotesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5>
            <asp:Label ID="Label11" Text="<%$Resources:Resource, NoteList %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
                <asp:LinkButton ID="lnk" CssClass="add_form_btn btnborder" runat="server" OnClick="lnk_Click">
                    <i class="fa-solid fa-user-plus"></i>&nbsp;
                    <asp:Label ID="Label2" Text="<%$Resources:Resource, AddNotes %>" runat="server"></asp:Label>
                </asp:LinkButton>
            </div>
        </div>
        <br />
        <div class="dt_ctrl_panal" style="display: none;">
            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <ul class="dt_filters pull-left">
                        <li>
                            <label><span class="glyphicon glyphicon-search"></span>&nbsp;<asp:Label ID="Label3" Text="<%$Resources:Resource, Filters %>" runat="server"></asp:Label></label>
                        </li>
                        <li><asp:Label ID="Label4" Text="<%$Resources:Resource, Site %>" runat="server"></asp:Label>                          
                            <asp:TextBox ID="txtName" runat="server" CssClass="form_element" MaxLength="40"></asp:TextBox>
                        </li>
                        <li>
                            <asp:Button ID="btnApply" runat="server" Text="<%$Resources:Resource, Apply %>" CssClass="form_submit_btn submit_btn_sm" OnClick="btnApply_Click" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdsort" runat="server" />
                    <table class="table datatable">
                        <thead>
                            <th width="20%">
                                <asp:Label ID="Label1" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                            <th width="60%">
                                <asp:Label ID="Label16" Text="<%$Resources:Resource, Description %>" runat="server"></asp:Label>
                            </th>
                            <th width="10%">
                                <asp:Label ID="Label18" Text="<%$Resources:Resource, TotalFiles %>" runat="server"></asp:Label></th>
                            <th width="10%">&nbsp;</th>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="dt_footer">
                        <div class="pagination float-end">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js?123"></script>
    <script src="js/jquery.bootpag.min.js"></script>
    <script>
        var filter = '';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';

        $(document).ready(function () {
            filter = $('#ContentPlaceHolder1_hdfilter').val();
            var totalcount = gettotalcount("GetAnimalNotesCount", filter);
            process(1);
            $('.pagination').bootpag({
                total: totalcount,
                page: 1,
                maxVisible: 5,
                leaps: true,
                firstLastUse: true,
                first: '←',
                last: '→',
                wrapClass: 'pagination',
                activeClass: 'active',
                disabledClass: 'disabled',
                nextClass: 'next',
                prevClass: 'prev',
                lastClass: 'last',
                firstClass: 'first'
            }).on("page", function (event, num) {
                process(num);
            });
        });

        function process(pageIndex) {
            
            $('.datatable tbody').html('');           
            getdata3("GetAnimalNotesDetails", pageIndex, filter, '', getAnimalNotesDetails_Success);
        }

        function getAnimalNotesDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();

                    var animalid = record.find("animalid").text();

                    var contents = '<tr>';
                    contents += '<td>' + record.find("procesed_date").text() + '</td>';
                    var desc = record.find("description").text();
                    if (desc.length > 255) desc = desc.substring(0, 255) + '...';
                    contents += '<td style="word-break: break-all;">' + desc + '</td>';
                    contents += '<td>' + record.find("filescount").text() + '</td>';
                    contents += "<td><div class='btn-group btn-group-sm dt_action_btns'><a href='notesdetails.aspx?id=" + id + "' class='btn btn_edit' data-toggle='tooltip' data-placement='top' data-original-title='Edit' title='Edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' onclick='deleteRecord(\"" + id + "\")' class='btn btn_delete' data-toggle='tooltip' data-placement='top' data-original-title='Delete' title='Delete'><i class='fa fa-trash'></i></a></div></td>";
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        function deleteRecord(id) {
            deleteData("DeleteAnimalNotes", id);
            process(1);
        }
    </script>
</asp:Content>
