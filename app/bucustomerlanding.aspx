<%@ Page Title="Welcome to Customer - Breeder" Language="C#" MasterPageFile="bu.Master" AutoEventWireup="true" CodeBehind="bucustomerlanding.aspx.cs" Inherits="Breederapp.bucustomerlanding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="css/jquery.e-calendar.css" />
    <style>
        .profilepic {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            border: 1px solid #ddd;
        }

        #maintable {
            border-top: 1px solid #ddd;
        }

            #maintable tr td {
                vertical-align: middle;
            }

        #appointmenttable tr td, #eventstable tr td {
            padding: 8px 0;
            vertical-align: top;
        }

        .sub_text {
            opacity: 0.7;
            margin-top: 3px;
        }

        .date {
            background: #db6516;
            color: #fff;
            display: inline-block;
            padding: 0 3px;
            border-radius: 4px;
        }

        .moremenu li a, .moremenu li:hover a {
            color: #333;
        }

        .btn_edit, .btn_edit:hover {
            border: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal" style="padding: 10px !important;">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center">
                <h5>
                    <asp:Label ID="lblCustomerName" Text="" runat="server" CssClass="error_class"></asp:Label>
                </h5>
            </div>
        </div>
    </div>
    <br />
   <%-- <div class="form_horizontal">
        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <h6 class="float-start">
                    <asp:Label ID="Label5" Text="<%$Resources:Resource, Appointment %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <div class="float-end ">
                </div>
                <div class="clearfix"></div>
                <input type="hidden" id="hidappfilter" runat="server" />
                  <div style="height: 265px; overflow: auto;">
                    <table id="appointmenttable" width="100%">
                        <tbody></tbody>
                    </table>

                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <h6 class="float-start">
                    <asp:Label ID="Label3" Text="<%$Resources:Resource, Events %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <div class="float-end ">
                </div>
                <div class="clearfix"></div>
                 <div style="height: 265px; overflow: auto;">
                    <table id="eventstable" width="100%">
                        <tbody></tbody>
                    </table>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <div id="calendardiv"></div>
            </div>
        </div>

    </div>--%>
    <br />
    <div class="form_horizontal">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <h6 class="float-start">
                    <asp:Label ID="Label1" Text="Pet List" runat="server" CssClass="error_class"></asp:Label></h6>
                <div class="float-end">
                    <asp:LinkButton ID="lnkAddBreed" runat="server" CssClass="add_form_btn btnborder" OnClick="lnkAddBreed_Click">
                        +&nbsp;<asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
                    </asp:LinkButton>
                </div>
                <div class="clearfix"></div>
                <div class="table-responsive dt_container">
                    <input type="hidden" id="hdfilter" runat="server" />
                    <input type="hidden" id="hdsort" runat="server" />
                    <div class="filter_section">
                        <i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label>&nbsp;
                         <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form_input" DataTextField="breedname" DataValueField="id"></asp:DropDownList>&nbsp;
                        <asp:TextBox ID="txtName" runat="server" CssClass="form_input" MaxLength="40" placeholder="Name"></asp:TextBox>&nbsp;
                        <asp:LinkButton ID="btnApply" CssClass="form_search_button" runat="server" OnClick="btnApply_Click">
                            <asp:Label ID="Label4" Text="<%$Resources:Resource, Search %>" runat="server"></asp:Label>
                        </asp:LinkButton>
                    </div>
                    <table class="table datatable" id="maintable">
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

    <script type="text/javascript" src="js/jquery.e-calendar.js"></script>
    <script>
        var filter = '';
        var actionlist = [];

        var cmonth = new Date().getMonth();
        var cyear = new Date().getFullYear();

        var m_gallery = '<%=  Resources.Resource.Gallery %>';
        var m_appointment = '<%=  Resources.Resource.Appointment %>';
        var m_createappointment = '<%=  Resources.Resource.CreateAppointment %>';
        var m_certificatelist = '<%=  Resources.Resource.CertificateList %>';
        var m_notelist = '<%=  Resources.Resource.NoteList %>';
        var m_foodlist = '<%=  Resources.Resource.FoodList %>';
        var m_view = '<%=  Resources.Resource.View %>';
        var m_activitylog = '<%=  Resources.Resource.ActivityLog %>';
        var NoRecordsFound = '<%=  Resources.Resource.NoRecordsFound %>';
       // var CustomerId = '<%=  EncCustomerId %>';

        $(document).ready(function () {
            var filterjson = {
                filter :  $('#ContentPlaceHolder1_hdfilter').val(),
                customerid : '<%=  ViewState["userid"] %>'
            };

           <%-- var formData = {};
            formData['scheduledateid'] = '<%= ViewState["id"]%>';
            formData['animalid'] = '<%= ViewState["animalid"]%>';
            formData['userid'] = '<%= this.UserId %>';

            var filter = JSON.stringify(formData);--%>

            var totalcount = gettotalcount("GetCustomerAnimalsCount", JSON.stringify(filterjson));
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

            /*processappointment();*/
        });

        function process(pageIndex) {
            var filterjson = {
                filter :  $('#ContentPlaceHolder1_hdfilter').val(),
                customerid : '<%=  ViewState["userid"] %>'
            };

            $('#maintable tbody').html('');
            getdata3("GetCustomerAnimals", pageIndex, JSON.stringify(filterjson), '', getAnimalDetails_Success);
        }

        function getAnimalDetails_Success(data) {
            $('#maintable tbody').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var rowactions = "<ul class='dropdown-menu profile_menu moremenu' aria-labelledby='moremenu'>\
<li><a href='bugallery.aspx?id={id}'>" + m_gallery + "</a></li>\
<li><a href='buappointmentlist.aspx?id={id}'>" + m_appointment + "</a></li>\
<li><a href='bucreateappointment.aspx?id={id}'>" + m_createappointment + "</a></li>\
<li><a href='bucertificateslist.aspx?id={id}'>" + m_certificatelist + "</a></li>\
<li><a href='bunoteslist.aspx?id={id}'>" + m_notelist + "</a></li>\
<li><a href='bufoodlist.aspx?id={id}'>" + m_foodlist + "</a></li>\
<li><a href='buuseranimallog.aspx?id={id}'>" + m_activitylog + "</a></li>\
</ul>";
                records.each(function () {
                    var record = $(this);
                    var id = record.find("id").text();
                    var securedid = record.find("securedid").text();

                    var contents = '<tr>';
                    if (record.find("profilepic_file").text().length > 0) contents += '<td width="10%"><img src="docs/' + record.find("profilepic_file").text() + '" alt="p" class="profilepic" /></td>';
                    else contents += '<td width="10%"><img src="images/' + record.find("breedimage").text() + '" alt="p" class="profilepic"/></td>';

                    contents += '<td width="35%" style="font-weight: 900; font-size: 15px;"> <a href="bubasicdetails.aspx?id=' + securedid + '" class="btn btn_edit"> ' + record.find("name").text() + '</a></td>';
                    contents += '<td width="20%">' + record.find("typename").text() + '</td>';
                    contents += '<td  width="10%">' + record.find("procesed_dob").text() + '</td>';
                    var temp = rowactions;
                    temp = temp.replace(/{id}/g, securedid);

                    contents += "<td width='15%'><div class='btn-group btn-group-sm dt_action_btns'><a href='bubasicdetails.aspx?id=" + securedid + "' class='btn btn_edit'><i class='fa fa-pencil'></i></a><a href='javascript:void(0);' class='dropdown-toggle btn btn_edit' id='moremenu' data-bs-toggle='dropdown' aria-expanded='false'><i class='fa-solid fa-ellipsis'></i></a>" + temp + "</div> </td>";
                    contents += '</tr>';
                    $('#maintable > tbody:last').append(contents);
                });
            }
            else {
                $('#maintable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
            }
        }

        //function getAppointment_Success(data) {
        //    $('#appointmenttable tbody').html('');
        //    var xmldata = data.d;
        //    if (xmldata != null && xmldata.length > 2) {
        //        var xmlDoc = $.parseXML(xmldata);
        //        var xml = $(xmlDoc);
        //        var records = xml.find("Table");
        //        records.each(function () {
        //            var record = $(this);

        //            var contents = '<tr>';
        //            contents += '<td width="35%"><div class="date">' + record.find("procesed_datetime").text() + '</div></td>';
        //            contents += '<td width="55%">' + record.find("profession_name").text() + ' - ' + record.find("contact_name").text() + '<div class="sub_text">' + record.find("animal_name").text() + ' [' + record.find("typename").text() + ']</div></td>';
        //            contents += "<td width='10%'><a href='appointmentview.aspx?id=" + record.find("securedid").text() + "'>" + m_view + "</a></td>";
        //            contents += '</tr>';
        //            $('#appointmenttable > tbody:last').append(contents);

        //            var json = {};
        //            var leadaction_date = new Date(record.find("startdatetime").text());
        //            json['datetime'] = new Date(leadaction_date.getFullYear(), leadaction_date.getMonth(), leadaction_date.getDate(), 0);
        //            json['dataid'] = record.find("securedid").text();

        //            actionlist.push(json);
        //        });

        //        $('#calendardiv').eCalendar({
        //            events: actionlist,
        //            month: cmonth,
        //            year: cyear,
        //            prevMonthClick: function (m, y) {
        //                cmonth = m;
        //                cyear = y;
        //                processappointment();
        //            },
        //            nextMonthClick: function (m, y) {
        //                cmonth = m;
        //                cyear = y;
        //                processappointment();
        //            }
        //        });
        //    }
        //    else {
        //        $('#appointmenttable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
        //    }
        //}

        //function getEvents_Success(data) {
        //    $('#eventstable tbody').html('');
        //    var xmldata = data.d;
        //    if (xmldata != null && xmldata.length > 2) {
        //        var xmlDoc = $.parseXML(xmldata);
        //        var xml = $(xmlDoc);
        //        var records = xml.find("Table");
        //        var chkValue = false;
        //        records.each(function () {
        //            var record = $(this);

        //            if (parseInt(record.find("myregistrationcount").text()) > 0) { // If user registrationcount > 0 then only show the events 

        //                var contents = '<tr>';
        //                contents += '<td width="35%"><div class="date">' + record.find("procesed_datetime").text() + '</div></td>';
        //                contents += '<td width="55%">' + record.find("title").text() + '</td>';
        //                contents += "<td width='10%'><a href='eventview.aspx?id=" + record.find("securedid").text() + "'>" + m_view + "</a></td>";
        //                contents += '</tr>';
        //                $('#eventstable > tbody:last').append(contents);

        //                var json = {};
        //                var leadaction_date = new Date(record.find("startdate").text());
        //                json['datetime'] = new Date(leadaction_date.getFullYear(), leadaction_date.getMonth(), leadaction_date.getDate(), 0);
        //                json['dataid'] = record.find("securedid").text();

        //                actionlist.push(json);
        //                chkValue = true;
        //            }

        //        });

        //        $('#calendardiv').eCalendar({
        //            events: actionlist,
        //            month: cmonth,
        //            year: cyear,
        //            prevMonthClick: function (m, y) {
        //                cmonth = m;
        //                cyear = y;
        //                processappointment();
        //            },
        //            nextMonthClick: function (m, y) {
        //                cmonth = m;
        //                cyear = y;
        //                processappointment();
        //            }
        //        });

        //        if (!chkValue) {
        //            $('#eventstable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
        //        }
        //    }
        //    else {
        //        $('#eventstable tbody').html("<tr><td colspan='6'>" + NoRecordsFound + "</td></tr>");
        //    }
        //}

        //function processappointment() {
        //    actionlist = [];

        //    var obj = {};
        //    obj['month'] = parseInt(cmonth) + 1;
        //    obj['year'] = cyear;
        //    var filter = JSON.stringify(obj);

        //    $('#appointmenttable tbody').html('...');
        //    getdata3("GetAnimalAppointmentDetailsByMonth", -1, filter, '', getAppointment_Success); // -1 page means show all records

        //    $('#eventstable tbody').html('...');
        //    getdata3("GetEventDetailsByMonth", 1, filter, '', getEvents_Success);
        //}

        $('#calendardiv').eCalendar({
            events: [],
            day: new Date().getDate(),
            month: cmonth,
            year: cyear,
            prevMonthClick: function (m, y) {
                cmonth = m;
                cyear = y;
                /*processappointment();*/
            },
            nextMonthClick: function (m, y) {
                cmonth = m;
                cyear = y;
                /* processappointment();*/
            }
        });

        $(document).on('click', '.c-event', function (event) {
            /*event.stopPropagation();
            var id = $(this).attr('data-id');
            if (id) {
                window.location = "appointmentview.aspx?id=" + id;
            }*/
        });
    </script>
</asp:Content>
