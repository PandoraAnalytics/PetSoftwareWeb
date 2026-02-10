<%@ Page Title="" Language="C#" MasterPageFile="breeder.master" AutoEventWireup="true" CodeBehind="pedigreereport.aspx.cs" Inherits="Breederapp.pedigreereport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .person {
            border: 1px solid #ddd;
            padding: 10px;
            min-width: 100px;
            background-color: #FFFFFF;
            display: inline-block;
            border-radius: 5px;
        }

            .person.female {
                border-color: #F45B69;
            }

            .person.male {
                border-color: #456990;
            }

            .person div {
                text-align: center;
            }

            .person .name a {
                color: #333;
            }

            .person .parentDrop, .person .spouseDrop, .person .childDrop {
                border: 1px dashed #000000;
                width: auto;
                min-width: 80px;
                min-height: 80px;
                display: inline-block;
                vertical-align: top;
                position: relative;
                padding-top: 15px;
            }

                .person .parentDrop > span,
                .person .spouseDrop > span,
                .person .childDrop > span {
                    position: absolute;
                    top: 2px;
                    left: 2px;
                    font-weight: bold;
                }

        .parentDrop > .person,
        .spouseDrop > .person,
        .childDrop > .person {
            margin-top: 20px;
        }

        /* Tree */
        .tree ul {
            padding-top: 20px;
            position: relative;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
        }

        .tree li {
            display: table-cell;
            text-align: center;
            list-style-type: none;
            position: relative;
            padding: 20px 5px 0 5px;
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
        }



            /*We will use ::before and ::after to draw the connectors*/
            .tree li::before, .tree li::after {
                content: '';
                position: absolute;
                top: 0;
                right: 50%;
                border-top: 1px solid #ccc;
                width: 50%;
                height: 20px;
            }

            .tree li::after {
                right: auto;
                left: 50%;
                border-left: 1px solid #ccc;
            }

            /*We need to remove left-right connectors from elements without 
any siblings*/
            .tree li:only-child::after, .tree li:only-child::before {
                display: none;
            }

            /*Remove space from the top of single children*/
            .tree li:only-child {
                padding-top: 0;
            }

            /*Remove left connector from first child and 
right connector from last child*/
            .tree li:first-child::before, .tree li:last-child::after {
                border: 0 none;
            }
            /*Adding back the vertical connector to the last nodes*/
            .tree li:last-child::before {
                border-right: 1px solid #ccc;
                border-radius: 0 5px 0 0;
                -webkit-border-radius: 0 5px 0 0;
                -moz-border-radius: 0 5px 0 0;
            }

            .tree li:first-child::after {
                border-radius: 5px 0 0 0;
                -webkit-border-radius: 5px 0 0 0;
                -moz-border-radius: 5px 0 0 0;
            }

        /*Time to add downward connectors from parents*/
        .tree ul ul::before {
            content: '';
            position: absolute;
            top: -10px;
            left: 50%;
            border-left: 1px solid #ccc;
            width: 0;
            height: 30px;
        }

        .tree li .parent {
            transition: all 0.5s;
            -webkit-transition: all 0.5s;
            -moz-transition: all 0.5s;
            margin-top: 10px;
        }

        .tree li .family {
            position: relative;
        }

            .tree li .family .spouse {
                position: absolute;
                top: 0;
                left: 50%;
                margin-left: 95px;
            }

                .tree li .family .spouse::before {
                    content: '';
                    position: absolute;
                    top: 50%;
                    left: -10px;
                    border-top: 1px solid #ccc;
                    border-bottom: 1px solid #ccc;
                    width: 10px;
                    height: 3px;
                }

        /*Time for some hover effects*/
        /*We will apply the hover effect the the lineage of the element also*/
        .tree li .child:hover,
        .tree li .child:hover + .parent .person,
        .tree li .parent .person:hover,
        .tree li .child:hover + .parent .person + ul li .child,
        .tree li .parent .person:hover + ul li .child,
        .tree li .child:hover + .parent .person + ul li .parent .person,
        .tree li .parent .person:hover + ul li .parent .person {
            background: #c8e4f8;
            color: #000;
            border: 1px solid #94a0b4;
        }
            /*Connector styles on hover*/
            .tree li .child:hover + .parent::before,
            .tree li .child:hover + .parent .person + ul li::after,
            .tree li .parent .person:hover + ul li::after,
            .tree li .child:hover + .parent .person + ul li::before,
            .tree li .parent .person:hover + ul li::before,
            .tree li .child:hover + .parent .person + ul::before,
            .tree li .parent .person:hover + ul::before,
            .tree li .child:hover + .parent .person + ul ul::before,
            .tree li .parent .person:hover + ul ul::before {
                border-color: #94a0b4;
            }

        .form_horizontal {
            overflow-y:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form_horizontal">
        <h6 class="float-start">
            <asp:Label ID="Label11" Text="<%$Resources:Resource, Pedigree %>" runat="server" CssClass="error_class"></asp:Label></h6>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-end">
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div>
                    <div class="tree">
                        <ul>
                            <li>
                                <div class="family">
                                    <div class="person child">
                                        <div class="name">
                                            <asp:Label ID="lblName" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div id="pedigree"></div>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <script src="js/data.js?123"></script>
    <script>
        var filter = '<%= ViewState["id"] %>';

        $(document).ready(function () {
            process();
        });

        function process() {
            $('.datatable tbody').html('');
            getdata3("GetAnimalParents", 1, filter, '', getDetails_Success);
        }

        var arr = [];
        var maindata = {};

        var parentc = '<div class="family"><div class="parent"><ul><li><div class="person child male"><div class="name">#fname#</div></div>#fchild#</li><li><div class="person child female"><div class="name">#mname#</div></div>#mchild#</li></ul></div></div>';

        function getDetails_Success(data) {
            $('#pedigree').html('');
            var xmldata = data.d;
            if (xmldata != null && xmldata.length > 2) {
                var xmlDoc = $.parseXML(xmldata);
                var xml = $(xmlDoc);
                var records = xml.find("Table");
                records.each(function () {
                    var record = $(this);

                    var temp = {
                        id: record.find("id").text(),
                        name: record.find("name").text(),
                        fatherid: isNaN(parseInt(record.find("fatherid").text())) ? 0 : parseInt(record.find("fatherid").text()),
                        motherid: isNaN(parseInt(record.find("motherid").text())) ? 0 : parseInt(record.find("motherid").text()),
                    };
                    arr.push(temp);
                });

                console.log(maindata);

                if (arr.length > 0) {
                    maindata["id"] = arr[0].id;
                    maindata["name"] = arr[0].name;
                    maindata["father"] = {};
                    maindata["mother"] = {};
                    var fid = parseInt(arr[0].fatherid);
                    if (fid > 0) maindata["father"] = getFatherNode(fid);

                    var mid = parseInt(arr[0].motherid);
                    if (mid > 0) maindata["mother"] = getMotherNode(mid);

                    var content = writenode(maindata);
                    $('#pedigree').html(content);
                }
            }
        }

        function getFatherNode(fid) {
            var temp = {};
            arr.forEach(function (fam) {
                if (parseInt(fam.id) == fid) {
                    temp["id"] = fam.id;
                    temp["name"] = fam.name;
                    temp["father"] = {};
                    temp["mother"] = {};
                    var fid1 = parseInt(fam.fatherid);
                    if (fid1 > 0) temp["father"] = getFatherNode(fid1);

                    var mid1 = parseInt(fam.motherid);
                    if (mid1 > 0) temp["mother"] = getMotherNode(mid1);
                }
            });
            return temp;
        }

        function getMotherNode(mid) {
            var temp = {};
            arr.forEach(function (fam) {
                if (parseInt(fam.id) == mid) {
                    temp["id"] = fam.id;
                    temp["name"] = fam.name;
                    temp["father"] = {};
                    temp["mother"] = {};

                    var fid1 = parseInt(fam.fatherid);
                    if (fid1 > 0) temp["father"] = getFatherNode(fid1);

                    var mid1 = parseInt(fam.motherid);
                    if (mid1 > 0) temp["mother"] = getMotherNode(mid1);
                }
            });
            return temp;
        }

        function writenode(data) {
            var temp = '';
            if (data.father.id && parseInt(data.father.id) > 0 ||
               data.mother.id && parseInt(data.mother.id) > 0
               ) {

                console.log(data.id + ' ' + data.father.id)

                var fath = '-';
                var fchild = '';
                if (data.father.id && parseInt(data.father.id) > 0) {
                    fath = '<a href="pedigreereport.aspx?id=' + data.father.id + '">' + data.father.name + '</a>';

                    fchild = writenode(data.father)
                }
                var moth = '-';
                var mchild = '';
                if (data.mother.id && parseInt(data.mother.id) > 0) {
                    moth = '<a href="pedigreereport.aspx?id=' + data.mother.id + '">' + data.mother.name + '</a>';
                    mchild = writenode(data.mother)
                }
                var temp = parentc.replace("#fname#", fath);
                temp = temp.replace("#fchild#", fchild);
                temp = temp.replace("#mname#", moth);
                temp = temp.replace("#mchild#", mchild);
            }
            return temp;
        }
    </script>
</asp:Content>
