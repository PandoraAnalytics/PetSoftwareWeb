<%@ Page Title="Upcoming Events" Language="C#" MasterPageFile="main.Master" AutoEventWireup="true" CodeBehind="upcomingevents.aspx.cs" EnableEventValidation="false" Inherits="Breederapp.upcomingevents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css?101" rel="stylesheet" />
    <style>
        #eventrow a {
            color: inherit;
        }

        .event_wrp {
            border: 1px solid #ddd;
            padding-bottom: 10px;
            border-radius: 5px;
            margin-bottom: 15px;
        }


        .event_img_wrp {
            position: relative;
            border-radius: 4px;
            margin-bottom: 15px;
        }

        .event_img {
            width: 100%;
            object-fit: contain;
            max-height: 163px;
            height: 163px;
            background: #fff6ec;
            /* border-radius: 4px; */
            margin-bottom: 10px;
        }

        .event_txt_wrp {
            padding: 0 8px;
        }

        .event_title {
            font-size: 18px;
            font-weight: 500;
            line-height: 1.33;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            margin-bottom: 8px;
        }

        .event_venue {
            line-height: 1.5;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            -webkit-box-orient: vertical;
        }

        .event_date {
            position: absolute;
            bottom: 0px;
            left: 0;
            display: inline-block;
            background: #fff6ec;
            width: 100%;
            padding: 4px 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, Events %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="float-end">
            <asp:Panel ID="panelAddEvent" runat="server" Visible="false">
                <a href="eventsadd.aspx" class="add_form_btn btnborder">+&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, AddNew %>"></asp:Label>
                </a>
            </asp:Panel>
        </div>
        <div class="clearfix"></div>
        <br />
        <input type="hidden" id="hidfilter" runat="server" />

        <div class="row">
            <div class="col-lg-3 col-md-3 col-sm-4 col-xs-12">
                <h6><i class="fa-solid fa-filter"></i>&nbsp;
                         <asp:Label ID="Label6" Text="<%$Resources:Resource, FilterBy %>" runat="server"></asp:Label></h6>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource, Month %>" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form_input" Width="50%"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource, Year %>" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form_input" Width="50%"></asp:DropDownList>
                    </div>
                </div>

                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource, SelectAnimal%>" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlSelectAnimal" runat="server" CssClass="form_input" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectAnimal_SelectedIndexChanged" DataTextField="breedname" DataValueField="id" Width="50%"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource, SelectBreed%>" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlSelectBreed" runat="server" CssClass="form_input" DataTextField="name" DataValueField="id" Width="50%"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Button ID="btnApply" CssClass="form_button" Text="<%$Resources:Resource, Apply %>" runat="server" OnClick="btnApply_Click" />&nbsp;
                    </div>
                </div>
            </div>
            <div class="col-lg-9 col-md-9 col-sm-8 col-xs-12">
                <div id="eventrow" class="row form_row">
                </div>
            </div>
        </div>
    </div>

    <script src="js/data.js?10"></script>
    <script type="text/javascript" src="js/bootstrap-datepicker.js?101"></script>

    <script>
        var filter = '';
        var NoUpcomingEvents = '<%=  Resources.Resource.Noupcomingevents %>';

        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });

            filter = $('#ContentPlaceHolder1_hidfilter').val();
            process(1);
        });

        function process(pageIndex) {
            $('.datatable tbody').html('');
            getdata3("GetEventDetails", pageIndex, filter, '', getAnimalNotesDetails_Success);
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
                    var contents = '<div class="col-lg-4 col-md-4 col-sm-4 col-xs-12"><a href="eventview.aspx?id=' + id + '"><div class="event_wrp">';
                    var img = '';                    
                    if (record.find("banner_image").text().length > 0) {
                        img = '<td><img src="../images/image_loading.gif" class="lazy-img event_img" data-src="' + record.find(" banner_image").text() + '" class="equipment_img alt="x"></td>';                        
                    }
                    else {                       
                        img = '<td><img src="../app/images/default_banner_image.png" class="event_img" alt="x"></td>';
                    }
                    contents += '<div class="event_img_wrp">';
                    contents += img;
                    contents += '<span class="event_date">' + record.find("procesed_date").text() + '</span>';
                    contents += '</div>';


                    contents += '<div class="event_txt_wrp">';
                    contents += '<div class="event_title">' + record.find("title").text() + '</div>';

                    var venue = record.find("venue").text();
                    if (!venue || venue.length == 0) venue = '-';
                    contents += ' <div class="event_venue">' + venue + '</div>';


                    contents += '<div>';
                    contents += '<div class="float-start">' + record.find("procesed_time").text() + '</div>';

                    var heart = (parseInt(record.find("myregistrationcount").text()) > 0) ? '<i class="fa-solid fa-heart"  style="color:#d64651;"></i>' : '<i class="fa-solid fa-heart"></i>';

                    contents += '<div class="float-end">' + heart + '&nbsp;' + record.find("registrationcount").text() + '</div>';
                    contents += '<div class="clearfix"></div>';
                    contents += '</div>';
                    contents += '</div>';
                    contents += '</div></a></div>';


                    $('#eventrow').append(contents);
                });
                loadLazyImages();
            }
            else {
                $('#eventrow').html("<h6>" + NoUpcomingEvents + "</h6>");
            }
        }

    </script>
</asp:Content>
