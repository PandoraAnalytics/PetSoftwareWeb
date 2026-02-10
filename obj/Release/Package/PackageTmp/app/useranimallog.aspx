<%@ Page Title="" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="useranimallog.aspx.cs" Inherits="Breederapp.useranimallog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, ActivityLog %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hidfilter" runat="server" />
                    <input type="hidden" id="hdsort" runat="server" />                  
                    <table class="table datatable">
                        <thead>                           
                            <th width="30%">
                                <asp:Label ID="Label3" Text="<%$Resources:Resource, Category %>" runat="server"></asp:Label></th>                          
                            <th width="20%">
                                <asp:Label ID="Label8" Text="<%$Resources:Resource, Date %>" runat="server"></asp:Label></th>
                            <th width="48%">
                                <asp:Label ID="Label9" Text="<%$Resources:Resource, Description %>" runat="server"></asp:Label></th>
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

            filter = $('#ContentPlaceHolder1_hidfilter').val();
            var totalcount = gettotalcount("GetAnimalLogCount", filter);
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
            getdata3("GetAnimalLogDetails", pageIndex, filter, '', getAnimalLogDetails_Success);
        }

        function getAnimalLogDetails_Success(data) {
            $('.datatable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);
                    var id = record.find("securedid").text();
                    var contents = '<tr>';
                    contents += '<td>' + record.find("category").text() + '</td>';
                    contents += '<td>' + record.find("procesed_datetime").text() + '</td>';

                    var descValue = record.find("description").text();
                    var keyValue = record.find("key").text();
                    var descMessage = '';

                    if (keyValue != '' && keyValue.length > 0) {
                        switch (keyValue) {
                            case 'EDITBASICINFO':
                                descMessage = '<%=  Resources.Resource.Log_BasicInfoMessage %>';
                                break;

                            case 'EDITPARENT':
                                descMessage = '<%=  Resources.Resource.Log_ParentInfoMessage %>';
                                break;

                            case 'EDITOTHERINFO':
                                descMessage = '<%=  Resources.Resource.Log_OtherInfoMessage %>';
                                break;

                            case 'ADDGALLERY':
                                descMessage = '<%=  Resources.Resource.Log_GallerAddMessage %>';

                                break;

                            case 'DELETEGALLERY':
                                descMessage = '<%=  Resources.Resource.Log_GalleryDeleteMessage %>';

                                break;

                            case 'ADDAPPOINTMENT':
                                descMessage = '<%=  Resources.Resource.Log_AppointmentAddMessage %>' + " " + descValue;;

                                //descMessage = "Appointment has been booked from " + descValue;

                                break;

                            case 'TODOAPPOINTMENT':
                                descMessage = '<%=  Resources.Resource.Log_AppointmentToDoMessage %>' + " (" + descValue + ")";

                                //descMessage = "Appointment TODO: (" + descValue + ")";
                                break;

                            case 'ADDNOTE':
                                descMessage = '<%=  Resources.Resource.Log_NotesAddMessage %>' + " (" + descValue + ")";

                                //descMessage = "Notes has been added. (" + descValue + ")";
                                break;

                            case 'EDITNOTE':
                                descMessage = '<%=  Resources.Resource.Log_NotesEditMessage %>' + " (" + descValue + ")";

                                //descMessage = "Notes has been edited. (" + descValue + ")";
                                break;

                            case 'ADDFOOD':
                                descMessage = descValue + " " + '<%=  Resources.Resource.Log_FoodAddMessage %>';

                                //descMessage = descValue + " has been added.";
                                break;

                            case 'EDITFOOD':
                                descMessage = descValue + " " + '<%=  Resources.Resource.Log_FoodEditMessage %>';

                                //descMessage = descValue + " has been edited.";
                                break;

                            case 'ADDCERTIFICATE':
                                descMessage = descValue + " " + '<%=  Resources.Resource.Log_CertificateAddMessage %>';

                                //descMessage = descValue + " has been added";
                                break;

                            case 'TRANSFERREQUESTSEND':
                                descMessage = '<%=  Resources.Resource.Log_TransferRequestSentMessage %>' + " " + descValue;

                                //descMessage = "Transfer request has been sent to " + descValue;
                                break;

                            case 'TRANSFERREQUESTACCEPT':
                                descMessage = '<%=  Resources.Resource.Log_TransferRequestAcceptMessage %>';

                                //descMessage = "Transfer request has been accepted";
                                break;

                            case 'TRANSFERREQUESTREJECT':
                                descMessage = '<%=  Resources.Resource.Log_TransferRequestRejectMessage %>';

                                //descMessage = "Transfer request has been rejected";
                                break;

                            case 'Default':
                                break;
                        }
                    }

                    contents += '<td>' + descMessage + '</td>';
                    contents += '</tr>';
                    $('.datatable > tbody:last').append(contents);
                });
            }
            else {
                $('.datatable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }
      
    </script>
</asp:Content>
