<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="editCustomer.aspx.cs" Inherits="MBSERPs.iMembers.editCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/jscript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&libraries=places&components=country:in&location=13.114631,80.166137&radius=500000"></script>
    <script type="text/javascript">

        function initialize() {
            var toAreaVar = "<%=txt_Location.ClientID %>";
            //alert('initialize');
            //alert(document.getElementById(toAreaVar).value);
            var defaultBounds = new google.maps.LatLngBounds(
                                new google.maps.LatLng(10.675401, 79.129200),
                                  new google.maps.LatLng(10.830893, 79.136753));

            var options = {
                bounds: defaultBounds,
                componentRestrictions: { country: 'in' }
            };

            //var fromAreaVar = "Pattaravakkam Railway Station, Periyakulam, Sidco Industrial Estate, Ambattur, Chennai, Tamil Nadu 600053, India";

            //var v1 = new google.maps.places.Autocomplete(document.getElementById(fromAreaVar), options);

            var v2 = new google.maps.places.Autocomplete(document.getElementById(toAreaVar), options);

            //document.getElementById(fromAreaVar).value = v1.get.getPlace().name;
            if (document.getElementById(toAreaVar).value == '') {
                document.getElementById(toAreaVar).value = v2.getPlace();
            }
            if (document.getElementById(toAreaVar).value == 'undefined') {
                document.getElementById(toAreaVar).value = '';
            }
        }


        google.maps.event.addDomListener(window, 'load', initialize);

        function callback(response, status) {
            //alert('callback');
            switch (status) {
                case google.maps.DistanceMatrixStatus.OK:
                    var origins = response.originAddresses;
                    var destinations = response.destinationAddresses;

                    for (var i = 0; i < origins.length; i++) {
                        var results = response.rows[i].elements;
                        for (var j = 0; j < results.length; j++) {
                            var element = results[j];
                            var hdfDistanceVar = "<%=txt_KMToCustomerHouse.ClientID %>";
                            var duration = element.duration.text;
                            var from = origins[i];
                            var to = destinations[j];

                            if (element.status != 'NOT_FOUND') {
                                var distance = parseFloat(element.distance.text);

                                document.getElementById(hdfDistanceVar).value = distance;
                            }
                        }
                    }
                    return true;

                case google.maps.DistanceMatrixStatus.INVALID_REQUEST:
                    alert('Invalid Input Request.');
                    return false;

                case google.maps.DistanceMatrixStatus.MAX_ELEMENTS_EXCEEDED:
                    alert('Request Exceeds 100 Elements.');
                    return false;

                case google.maps.DistanceMatrixStatus.MAX_DIMENSIONS_EXCEEDED:
                    alert('More Than 25 Origins Or Destinations Are Not Allowed.');
                    return false;

                case google.maps.DistanceMatrixStatus.DistanceMatrixStatus.OVER_QUERY_LIMIT:
                    alert('You Have Reached The Maximum Query Limit. Please Try After Some Hours');
                    return false;

                case google.maps.DistanceMatrixStatus.DistanceMatrixStatus.REQUEST_DENIED:
                    alert('Request Is Denied.');
                    return false;

                case google.maps.DistanceMatrixStatus.DistanceMatrixStatus.UNKNOWN_ERROR:
                    alert('Server Error. Please Try Again.');
                    return false;

                default:
                    alert('Error. Please Try Again With Some Other Area.');
                    return false;
            }
        }

        function findKm() {
            //alert('findKmstart');
            var fromAreaVar = "<%=txt_Location.ClientID %>";
            var fromArea = document.getElementById(fromAreaVar).value;
            var toArea = 'Pattaravakkam Railway Station, Periyakulam, Sidco Industrial Estate, Ambattur, Chennai, Tamil Nadu 600053, India';

            var service = new google.maps.DistanceMatrixService();
            service.getDistanceMatrix(
                      {
                          origins: [fromArea],
                          destinations: [toArea],
                          travelMode: google.maps.TravelMode.WALKING,
                          unitSystem: google.maps.UnitSystem.METRIC,
                          avoidHighways: false,
                          avoidTolls: false
                      }, isCompleted = callback);

            if (fromArea != '') {
                alert('Processing...');
            }
            return false;
        }     
    </script>
    <script type="text/javascript">
        function ModifyEnterKeyPressAsTab() {

            if (window.event && window.event.keyCode == 13) {

                window.event.keyCode = 9;
            }
        }
    </script>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                $('#<%=txt_MembershipcardRegiDt.ClientID%>').daterangepicker({
                    startDate: moment().add('days', -100),
                    endDate: moment().add('days', 45),
                    singleDatePicker: true,
                    showDropdowns: true,
                    timePicker: false,                    
                    format: 'DD/MM/YYYY',
                    dateLimit: { days: 45 }

                });
            });
            $(function () {
                $('#<%=txt_MembershipcardExpireDt.ClientID%>').daterangepicker({
                    startDate: moment().add('days', 330),
                    endDate: moment().add('days', 400),
                    singleDatePicker: true,
                    showDropdowns: true,
                    timePicker: false,                    
                    format: 'DD/MM/YYYY',
                    dateLimit: { days: 45 }

                });
            });
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <asp:ScriptManager ID="SM1" runat="server" />
    <!-- Content Header (Page header) -->
    <section class="content-header">
          <h1>
            Customer
            <small>Edit</small>
          </h1>
          
          <ol class="breadcrumb">
            <li><a href="../dashboard/home.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">Customer</a></li>
            <li class="active">Edit</li>
          </ol>
    </section>
    <!-- Main content -->
    <div class="content">
        <div class="row">
            <%--Customer Info--%>
            <div class="col-md-12">
                <!-- general form elements -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Customer Info</h3>
                        <asp:Label ID="lbl_custType" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                    </div>
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnEdit">
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="col-md-4">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btn_getCust">
                                    <div class="form-group">
                                        <label for="txt_MobileNo">
                                            Mobile No.</label>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator8"
                                            runat="server" ControlToValidate="txt_MobileNo" ErrorMessage="Error, Input Required!!"
                                            ValidationGroup="GETCustomerInfo"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator2"
                                            runat="server" ControlToValidate="txt_MobileNo" ErrorMessage="Error, Input Required!!"
                                            ValidationGroup="SUBMITBUTTON"></asp:RequiredFieldValidator>
                                        <%--     <asp:CompareValidator ID="cv" runat="server" ControlToValidate="txt_MobileNo" Type="Integer"
                                    Operator="DataTypeCheck" ErrorMessage="Value must be an Number!" ValidationExpression="^\d+?$"
                                    Font-Bold="true" ForeColor="Red" ValidationGroup="GETCustomerInfo" />--%>
                                        <asp:Button ID="btn_getCust" runat="server" Text="GET CUSTOMER" class="btn btn-link  pull-right"
                                            OnClick="btn_getCust_Click" ValidationGroup="GETCustomerInfo" />
                                        <asp:TextBox ID="txt_MobileNo" class="form-control" runat="server" Text=""></asp:TextBox>
                                    </div>
                                </asp:Panel>
                                <div class="form-group">
                                    <label for="txt_Name">
                                        Name</label>
                                    <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator3"
                                        runat="server" ControlToValidate="txt_Name" ErrorMessage="Error, Input Required!!"
                                        ValidationGroup="SUBMITBUTTON"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txt_Name" class="form-control" runat="server" Text="Balamurugan"
                                        onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_AltMobileNo">
                                        Alternate Mobile No/ Landline No.</label>
                                    <asp:TextBox ID="txt_AltMobileNo" class="form-control" runat="server" Text="04442071983"
                                        onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_Name">
                                        Email ID</label>
                                    <asp:TextBox ID="txt_Email" class="form-control" runat="server" Text="baluclick@gmail.com"
                                        onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_SourceType">
                                        Source Type</label>
                                    <asp:DropDownList ID="drp_SourceType" runat="server" class="form-control" onkeydown="ModifyEnterKeyPressAsTab();">
                                        <asp:ListItem Selected="True" Text="TV AD" Value="71"></asp:ListItem>
                                        <asp:ListItem Text="PAPER AD" Value="72"></asp:ListItem>
                                        <asp:ListItem Text="FRIENDS" Value="73"></asp:ListItem>
                                        <asp:ListItem Text="RELATIONS" Value="74"></asp:ListItem>
                                        <asp:ListItem Text="OTHERS" Value="75"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="txt_Location">
                                        Location</label>
                                    <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator1"
                                        runat="server" ControlToValidate="txt_Location" ErrorMessage="Error, Input Required!!"
                                        ValidationGroup="GETKM"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator4"
                                        runat="server" ControlToValidate="txt_Location" ErrorMessage="Error, Input Required!!"
                                        ValidationGroup="SUBMITBUTTON"></asp:RequiredFieldValidator>
                                    <asp:Button ID="Button1" runat="server" Text="GET DISTANCE" class="btn btn-link  pull-right"
                                        OnClientClick="findKm();" ValidationGroup="GETKM" />
                                    <asp:TextBox ID="txt_Location" class="form-control" runat="server" Text="" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_KMToCustomerHouse">
                                        Distance</label>
                                    <asp:TextBox ID="txt_KMToCustomerHouse" class="form-control" runat="server" Text=""
                                        onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_Address">
                                        Address</label>
                                    <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator5"
                                        runat="server" ControlToValidate="txt_Address" ErrorMessage="Error, Input Required!!"
                                        ValidationGroup="SUBMITBUTTON"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txt_Address" TextMode="MultiLine" Rows="4" class="form-control"
                                        runat="server" Text="Customer Address" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_Name">
                                        Landmark</label>
                                    <asp:TextBox ID="txt_Landmark" class="form-control" runat="server" Text="Near School"
                                        onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txt_Name">
                                        Need Company Offer By SMS/Mail</label>
                                    <br />
                                    <asp:RadioButton ID="rd_isPromYes" Text="Yes" runat="server" GroupName="isProm" ClientIDMode="Static"
                                        onkeydown="ModifyEnterKeyPressAsTab();" />
                                    <asp:RadioButton ID="rd_isPromNo" Text="No" runat="server" GroupName="isProm" ClientIDMode="Static"
                                        onkeydown="ModifyEnterKeyPressAsTab();" />
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="txt_Location">
                                        Customer Photo</label>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Image ID="img_CustomerPhoto" runat="server" Height="150px" BorderStyle="Ridge"
                                                    AlternateText="IMAGE NOT AVAILABLE" ForeColor="Orange" />
                                            </td>
                                            <td>
                                                <asp:FileUpload runat="server" ID="imgCustSnap" CssClass="lblRed" class="form-control" />
                                                <br />
                                                <asp:Button ID="btn_UploadPhotos" runat="server" Text="Upload Photo" class="btn btn-primary  pull-left"
                                                    OnClick="btn_UploadPhotos_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="form-group">
                                    <label for="rd_isMembershipcardYes">
                                        isMembershipcard Yes/No</label>
                                    <asp:RadioButton ID="rd_isMembershipcardYes" Text="Yes" runat="server" GroupName="isMembershipcard"
                                        AutoPostBack="true" OnCheckedChanged="rd_isPromYes_NO_CheckedChanged"></asp:RadioButton>
                                    <asp:RadioButton ID="rd_isMembershipcardNo" Text="No" runat="server" GroupName="isMembershipcard"
                                        AutoPostBack="true" OnCheckedChanged="rd_isPromYes_NO_CheckedChanged"></asp:RadioButton>
                                </div>
                                <div id="Div_isMem" runat="server">
                                    <div class="form-group">
                                        <label for="drp_MembercardType">
                                            Card Type</label>
                                        <asp:DropDownList ID="drp_MembercardType" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Text="GOLD CARD" Value="136"></asp:ListItem>
                                            <asp:ListItem Text="PLATINUM CARD" Value="137"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="txt_MembershipcardNumber">
                                            Card Number</label>
                                        <asp:TextBox ID="txt_MembershipcardNumber" MaxLength="20" class="form-control" runat="server"
                                            Text="" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="txt_MembershipcardBardCodeNo">
                                            BarCode Number</label>
                                        <asp:TextBox ID="txt_MembershipcardBardCodeNo" MaxLength="15" class="form-control"
                                            runat="server" Text="" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                    </div>
                                          <div class="form-group">
                                        <label for="txt_MembershipcardRegiDt">
                                            Membership Registered Date</label>
                                        <asp:TextBox ID="txt_MembershipcardRegiDt" MaxLength="15" class="form-control"
                                            runat="server" Text="" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                    </div>
                                          <div class="form-group">
                                        <label for="txt_MembershipcardExpireDt">
                                            Membership Expire Date</label>
                                        <asp:TextBox ID="txt_MembershipcardExpireDt" MaxLength="15" class="form-control"
                                            runat="server" Text="" onkeydown="ModifyEnterKeyPressAsTab();"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <div class="form-group">
                                    <br />
                                    <asp:Label class="label label-danger pull-left" Font-Size="Small" ID="lblError" runat="server"
                                        Text=""></asp:Label>
                                    <asp:Label class="label label-success pull-left" Font-Size="Small" ID="lblSuccess"
                                        runat="server" Text=""></asp:Label>
                                    <asp:Button ID="btnBook" class="btn btn-primary  pull-right" runat="server" Visible="true"
                                        CausesValidation="true" Text="Add" OnClick="btnBook_Click" ValidationGroup="SubmitUserAccount" />
                                    <asp:Button ID="btnEdit" class="btn btn-primary  pull-right" runat="server" Text="Edit"
                                        OnClick="btnEdit_Click" />
                                    <asp:Button ID="btnAddNew" class="btn btn-primary  pull-right" runat="server" Visible="false"
                                        Text="Add New" OnClick="btnAddNew_Click" />
                                    <asp:Button ID="btnCancel" class="btn btn-default  pull-right" runat="server" Text="Cancel"
                                        PostBackUrl="../dashboard/home.aspx" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
