<%@ Page Title="Add Event" Language="C#" MasterPageFile="main.master" AutoEventWireup="true" CodeBehind="eventsadd.aspx.cs" Inherits="Breederapp.eventsadd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/datepicker3.css?101" rel="stylesheet" />
    <link href="css/fastselect.css" rel="stylesheet" />
    <link rel="stylesheet" href="js/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css" type="text/css" />
    <style>
        .fstElement {
            width: 30%;
            display: inline-block;
            top: 10px;
            margin-top: -15px;
        }

        .form_input {
            width: 65%;
        }

        .default_banner {
            background-color: #f3f3f3;
            height: 250px;
            border-radius: 5px;
            position: relative;
            text-align: center;
            background-size: cover;
            background-repeat: no-repeat;
            border: 1px solid #ddd;
            background-image: url('images/default_banner_image.png');
        }

        .upload-btn-wrapper {
            position: absolute;
            bottom: 10px;
            left: 10px;
            overflow: hidden;
            display: inline-block;
        }

        .upload_banner_text {
            border: 1px solid gray;
            color: gray;
            background-color: #f8cb94;
            padding: 5px 5px;
            border-radius: 5px;
            font-weight: bold;
            font-size: 12px;
            font-weight: 600;
            cursor: default;
        }

        .upload-btn-wrapper input[type=file] {
            font-size: 100px;
            position: absolute;
            left: 0;
            top: 0;
            opacity: 0;
        }

        #nexteventlist tr td {
            padding: 6px 0;
            vertical-align: top;
        }

        #nexteventlist .event_img {
            width: 100%;
            object-fit: cover;
            height: 90px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h5 class="float-start">
            <asp:Label ID="Label5" Text="<%$Resources:Resource, AddEvent %>" runat="server" CssClass="error_class"></asp:Label></h5>
        <div class="clearfix"></div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="default_banner" runat="server" id="default_banner">
                    <div>
                        <div class="upload-btn-wrapper">
                            <button class="upload_banner_text">
                                <asp:Label ID="Label10" Text="<%$Resources:Resource, UploadBanner %>" runat="server" CssClass="table-success"></asp:Label></button>
                            <input type="file" name="myfile" id="banner_upload" accept="image/*" />
                        </div>
                        <input type="hidden" runat="server" id="hid_bannerimage" />
                    </div>
                </div>
                <span><%= Resources.Resource.RecommendedSize %></span>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-8 col-md-8 col-sm-6 col-xs-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_class"></asp:Label>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label4" runat="server" CssClass="form_label"><span><%= Resources.Resource.EventTitle %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEventTitle" runat="server" CssClass="form_input" data-validate="required" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label12" Text="<%$Resources:Resource, Timezone %>" runat="server" CssClass="form_label"></asp:Label>
                        <asp:DropDownList ID="ddlTimezone" runat="server" CssClass="form_input" DataValueField="name" DataTextField="timezone">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label3" runat="server" CssClass="form_label"><span><%= Resources.Resource.Date %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off" Width="33%"></asp:TextBox>
                        &nbsp;
                        <asp:DropDownList ID="ddlStartTime" runat="server" CssClass="form_input" data-validate="required" Width="30%"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label9" runat="server" CssClass="form_label"><span><%= Resources.Resource.EndsOn %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form_input date-picker" data-validate="required date" MaxLength="10" autocomplete="off" Width="33%"></asp:TextBox>
                        &nbsp;
                        <asp:DropDownList ID="ddlEndTime" runat="server" CssClass="form_input" data-validate="required" Width="30%"></asp:DropDownList>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label11" runat="server" CssClass="form_label"><span><%= Resources.Resource.Visible %></span>&nbsp;*</asp:Label>
                        <select id="ddlVisible1" multiple runat="server" class="form_input multipleSelect" data-validate="required">
                            <option value="" text="<%$Resources:Resource, VisibletoALL %>"></option>
                            <option value="1" text="All Pet Breeders"></option>
                            <option value="2" text="All Pet Owners"></option>
                        </select>

                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label2" runat="server" CssClass="form_label"><span><%= Resources.Resource.Description %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form_input" MaxLength="2000" data-validate="required maxlength-2000" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label1" runat="server" CssClass="form_label"><span><%= Resources.Resource.SelectAnimal %></span>&nbsp;*</asp:Label>
                        <asp:DropDownList ID="ddlSelectAnimal" runat="server" CssClass="form_input" data-validate="required" AutoPostBack="true" DataTextField="breedname" DataValueField="id" Width="33%" OnSelectedIndexChanged="ddlSelectAnimal_SelectedIndexChanged"></asp:DropDownList>
                        &nbsp;
                        <select id="ddlSelectBreed" multiple runat="server" class="form_input multipleSelect" data-validate="required" style="width: 30%;" datatextfield="name" datavaluefield="id"></select>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label6" runat="server" CssClass="form_label"><span><%= Resources.Resource.EventVenue %></span>&nbsp;*</asp:Label>
                        <asp:TextBox ID="txtEventVenue" runat="server" CssClass="form_input" MaxLength="100" data-validate="required"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12  col-xs-12">
                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource, RuleandCondition%>" CssClass="form_label"></asp:Label>
                        <asp:TextBox ID="txtRuleandCondition" runat="server" CssClass="form_input" MaxLength="1000" data-validate="maxlength-1000" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <input type="hidden" id="filenames" runat="server" />
                        <div id="uploader" style="width: 100%; display: inline-block;">
                            <p>
                                <asp:Label ID="lblhtml" Text="<%$Resources:Resource, YourbrowserdoesnothaveFlash %>" runat="server"></asp:Label>
                            </p>
                        </div>
                        <span class="rule">
                            <asp:Label ID="lblFileUploadvalidation" Text="<%$Resources:Resource, Max2MBpng %>" runat="server"></asp:Label></span>
                    </div>
                </div>
                <div class="row form_row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnCreateEvent" CssClass="form_button" Text="<%$Resources:Resource, CreateEvent %>" runat="server" OnClick="btnCreateEvent_Click" />&nbsp;
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                <br />
                <h6>
                    <asp:Label ID="Label8" Text="<%$Resources:Resource, OtherEvents %>" runat="server" CssClass="error_class"></asp:Label></h6>
                <input type="hidden" id="hidfilter" runat="server" />
                <table id="nexteventlist" width="100%">
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>
    <script src="js/validator.js?100" type="text/javascript"></script>
    <script type="text/javascript" src="js/bootstrap-datepicker.js?101"></script>
    <script type="text/javascript" src="js/fastselect.standalone.min.js"></script>
    <script type="text/javascript" src="js/plupload/plupload.full.min.js"></script>
    <script type="text/javascript" src="js/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script src="js/data.js?123"></script>


    <script>
        $(document).ready(function () {
            $(".date-picker").datepicker({ format: '<%= this.DateFormatSmall %>' });
            $('#ContentPlaceHolder1_ddlSelectBreed').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseBreedType %>',
                noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
            });

            $('.multipleSelect').fastselect({
                placeholder: '<%=  Resources.Resource.ChooseOption %>',
                noResultsText: '<%=  Resources.Resource.NoRecordsFound %>',
            });
            getEvents();
        });

        $('#banner_upload').change(function () {
            var f = $(this);
            var fid = $(f).attr('id');

            var fileData = $(f).prop("files")[0];
            console.log(fileData);
            var formData = new window.FormData();
            formData.append("file", fileData);
            formData.append("extns", "image");

            var id = 'per_lbl_' + fid;
            $('#' + id).remove();

            $.ajax({
                url: 'file_upload_docs.ashx',
                data: formData,
                processData: false,
                contentType: false,
                type: 'POST',
                xhr: function () {
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (evt) {
                        if (evt.lengthComputable) {
                            var percentComplete = evt.loaded / evt.total;
                            if (!isNaN(percentComplete)) {
                                percentComplete = percentComplete * 100;
                                if (percentComplete > 100) percentComplete = 100;
                                var id = 'per_lbl_' + fid;
                                $('#' + id).remove();
                                $(f).after("<span id='" + id + "'>Uploaded: " + parseFloat(percentComplete).toFixed(2) + "%</span>");
                            }
                        }
                    }, false);
                    return xhr;
                },
                success: function (data) {
                    var hid = '#ContentPlaceHolder1_hid_bannerimage';
                    $(hid).val(data);
                    //alert("The file has been uploaded successfully.");
                    data = "url('docs/" + data + "')";
                    // alert(data);
                    $(".default_banner").css("background-image", data);
                },
                error: function (evt, error) {
                    var fileuploadingerror = '<%= Resources.Resource.Problemuploadingthefile %>';
                    if (evt.responseText && evt.responseText.length > 0) alert(evt.responseText);
                    else alert(fileuploadingerror);
                }
            });
        });

    </script>

    <script>
        var Noupcomingevents = '<%=  Resources.Resource.Noupcomingevents %>';

        function getEvents() {
            $('#nexteventlist tbody').html('');
            var filter = $('#ContentPlaceHolder1_hidfilter').val();
            getdata3("GetEventDetails", 1, filter, '', getAnimalNotesDetails_Success);
        }

        function getAnimalNotesDetails_Success(data) {
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");

                var maincontents = '<tr><td width="100%">';

                records.each(function (index) {
                    var record = $(this);
                    var id = record.find("securedid").text();

                    var contents = '<table width="100%" class="event"><tr>';
                    if (record.find("banner_image").text().length > 0) {
                        contents += '<td width="100%" colspan="2"><div><img src="docs/' + record.find("banner_image").text() + '" class="event_img" alt="x"></div></td>';
                    }
                    else {
                        contents += '<td width="100%" colspan="2"><div><img src="images/default_banner_image.png" class="event_img" alt="x"></div></td>';
                    }
                    contents += '</tr><tr>';
                    contents += '<td width="50%"><a href="eventview.aspx?id=' + id + '" class="eventlink" style="">' + record.find("title").text() + '</td>';
                    contents += '<td width="50%">' + record.find("procesed_datetime").text() + '</td>';
                    contents += '</tr><tr>';
                    contents += '<td width="100%" colspan="2">' + record.find("venue").text() + '</td>';
                    contents += '</tr></table>';

                    maincontents += contents;
                });
                maincontents += '</td></tr>';
                $('#nexteventlist tbody').html(maincontents);
            }
            else {
                $('#nexteventlist tbody').html("<tr><td colspan='6'>" + Noupcomingevents + "</td></tr>");
            }


        }
    </script>
    <script>
        var temp;
        $("#uploader").pluploadQueue({
            runtimes: 'html5,flash,browserplus,silverlight,gears',
            url: 'file_upload_docs.ashx',
            max_files: 5,
            rename: true,
            dragdrop: true,

            filters: {
                max_file_size: '2mb',
                mime_types: [
                    { title: "Image files", extensions: "jpg,gif,png,jpeg" },
                    { title: "PDF files", extensions: "pdf" },
                ]
            },

            flash_swf_url: 'js/plupload/Moxie.swf',
            silverlight_xap_url: 'js/plupload/Moxie.xap',
            init: {
                FileUploaded: function (up, file, info) {
                    var val = $("#ContentPlaceHolder1_filenames").val();
                    temp = val;
                    if (val.length > 0) {
                        temp += ",";
                    }
                    temp += info.response;
                    $("#ContentPlaceHolder1_filenames").empty();
                    $("#ContentPlaceHolder1_filenames").val(temp);
                }
            },
        });
    </script>

</asp:Content>
