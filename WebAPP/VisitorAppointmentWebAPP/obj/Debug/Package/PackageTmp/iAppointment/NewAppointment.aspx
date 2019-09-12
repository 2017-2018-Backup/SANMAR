<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="NewAppointment.aspx.cs" Inherits="VisitorAppointmentWebAPP.iAppointment.NewAppointment"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function targetMeBlank() {
            document.forms[0].target = "_blank";
        }

        $(document).ready(function () {
            var MaxLength = 75;
            $('#txt_Message').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <asp:ScriptManager ID="SM1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
        <ContentTemplate>
            <section class="content-header">
          <h1>
            <%--Visitor--%>
            <small><b>Visitor Appointment </b>No.</small>
            <%--<small style="Color:Black"> - Visitor No :--%>
            <small>
            <asp:TextBox ID="txt_SlNo" Width="50px" BackColor="Transparent" BorderColor="Transparent" class="form-control inline" style="Color:Black; font-weight:bold" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            Date: <asp:TextBox ID="txt_Date" Width="100px" class="form-control inline" style="Color:Black; font-weight:bold" BackColor="Transparent" BorderColor="Transparent" runat="server" Text="" ReadOnly="true"></asp:TextBox>
            Time: <asp:TextBox ID="txt_DateTime" Width="100px" class="form-control inline" style="Color:Black; font-weight:bold" BackColor="Transparent" BorderColor="Transparent" runat="server" Text="" ReadOnly="true"></asp:TextBox>
             </small>
          </h1>      
          
          <%--<ol class="breadcrumb">
            <li class="active"><i class="fa fa-calendar"></i> VMS</li>
            <li class="active">Appointment</li>
            <li class="active">New</li> 
          </ol>--%>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Main content -->
    <%--  <section class="content">--%>
    <div class="content">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
            <ContentTemplate>
                <div class="col-sm-12">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        <label for="txt_FromName">
                                            <%--style="font-size: smaller;"--%>
                                            Name of the Person
                                            <%--(with Organisation Details if required)--%>
                                        </label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                            ControlToValidate="txt_FromName" Font-Bold="true" ForeColor="Red" ToolTip="From is a REQUIRED field"
                                            ValidationGroup="SUBMITBUTTON"></asp:RequiredFieldValidator>
                                        <asp:TextBox ID="txt_FromName" class="form-control" runat="server" Text="" TabIndex="1"></asp:TextBox>
                                        <div>
                                            <asp:CheckBox ID="chkExternal" runat="server" Text="External Visitor" AutoPostBack="true"
                                                OnCheckedChanged="chkExternal_CheckedChanged" />
                                            <label for="txt_Organization">
                                            </label>
                                            <asp:TextBox ID="txtOrganization" Visible="false" class="form-control" runat="server"
                                                Text=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--<div class="form-group">
                                        <label for="txt_FromName">
                                            Message Type
                                        </label>
                                        <asp:DropDownList ID="drp_MessageType" runat="server" class="form-control" TabIndex="4">
                                            <asp:ListItem Value="1" Enabled="true">Waiting To See You</asp:ListItem>
                                            <asp:ListItem Value="2">To See You</asp:ListItem>
                                            <asp:ListItem Value="3">Talk To You</asp:ListItem>
                                            <asp:ListItem Value="4">Called</asp:ListItem>
                                            <asp:ListItem Value="5">To Inform</asp:ListItem>
                                            <asp:ListItem Value="6">To Check</asp:ListItem>
                                            <asp:ListItem Value="7">For Information</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>--%>
                                </div>
                                <div class="col-sm-2">
                                    <div class="form-group">
                                        <label for="txt_To">
                                            Executive to Meet
                                        </label>
                                        <asp:DropDownList ID="drp_To" runat="server" class="form-control" TabIndex="2">
                                        </asp:DropDownList>
                                    </div>
                                    <%-- <div class="form-group">
                                        <label for="txt_Address">
                                            Message</label>
                                        <asp:TextBox ID="txt_Message" TextMode="MultiLine" Rows="2" class="form-control"
                                            TabIndex="5" runat="server" Text="" MaxLength="75"></asp:TextBox>
                                        <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ControlToValidate="txt_Message"
                                            ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,75}$" runat="server"
                                            ErrorMessage="Maximum 75 characters allowed."></asp:RegularExpressionValidator>
                                    </div>--%>
                                </div>
                                <div class="col-sm-2">
                                    <div class="form-group">
                                        <label for="txt_Name">
                                            Message Priority</label>
                                        <br />
                                        <asp:DropDownList ID="drp_MeggasePriority" runat="server" class="form-control" TabIndex="3">
                                            <asp:ListItem Selected="True" Text="General" Value="General"></asp:ListItem>
                                            <asp:ListItem Text="Urgent" Value="Urgent"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-2">
                                    <div class="form-group">
                                        <label for="txt_FromName">
                                            Message Type
                                        </label>
                                        <asp:DropDownList ID="drp_MessageType" runat="server" class="form-control" TabIndex="4">
                                            <asp:ListItem Value="1" Enabled="true">Waiting To See You</asp:ListItem>
                                            <asp:ListItem Value="2">To See You</asp:ListItem>
                                            <asp:ListItem Value="3">Talk To You</asp:ListItem>
                                            <asp:ListItem Value="4">Called</asp:ListItem>
                                            <asp:ListItem Value="5">To Inform</asp:ListItem>
                                            <asp:ListItem Value="6">To Check</asp:ListItem>
                                            <asp:ListItem Value="7">For Information</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        <label for="txt_Address">
                                            Message</label>
                                        <asp:TextBox ID="txt_Message" TextMode="MultiLine" Rows="2" class="form-control"
                                            TabIndex="5" runat="server" Text="" MaxLength="75"></asp:TextBox>
                                        <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Red" ControlToValidate="txt_Message"
                                            ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{0,75}$" runat="server"
                                            ErrorMessage="Maximum 75 characters allowed."></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <%-- <br />--%>
                                        <asp:Button ID="btnBook" class="btn btn-primary  pull-left" runat="server" Visible="true"
                                            Text="Create Appointment" OnClick="btnBook_Click" ValidationGroup="SUBMITBUTTON"
                                            TabIndex="6" />
                                        <asp:Button ID="btnCancel" class="btn btn-default  pull-left" runat="server" Visible="true"
                                            Text="Cancel" OnClick="btnCancel_Click" />
                                    </div>
                                    <%--   </div>
                                <div class="col-md-6">--%>
                                    <span />
                                    <asp:UpdatePanel ID="pnl3" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <asp:Label class="label label-danger pull-left" Font-Size="Small" ID="lblError" runat="server"
                                                    Text=""></asp:Label>
                                                <asp:Label class="label label-success pull-left" Font-Size="Small" ID="lblSuccess"
                                                    runat="server" Text=""></asp:Label>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="msgLabel" EventName="Tick" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </span>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Timer ID="msgLabel" runat="server" Interval="5000" Enabled="false" OnTick="msgLabel_Tick">
        </asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
            <ContentTemplate>
                <div class="row">
                    <%--Customer Info--%>
                    <div class="col-sm-12">
                        <!-- general form elements -->
                        <br />
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h1 class="box-title" style="font-size: 15px !important; font-weight: 300 !important">
                                    <b>Appointment Status</b></h1>
                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                            </div>
                            <div class="box-body">
                                <div class="col-sm-12">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearchFilter">
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txt_PassConfirm">
                                                    Filter by</label><br />
                                                <asp:DropDownList ID="drp_FilterBy" runat="server" class="form-control">
                                                    <asp:ListItem Selected="True" Text="Visitor Name" Value="Visitor Name"></asp:ListItem>
                                                    <asp:ListItem Text="Visiting To" Value="Visiting To"></asp:ListItem>
                                                    <asp:ListItem Text="Read Status" Value="Read Status"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                                <label for="txt_fileterStr">
                                                    Search Keyword</label><br />
                                                <asp:TextBox ID="txt_fileterStr" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-1">
                                            <%-- <label for="btnSearchFilter">
                                                Action</label>--%>
                                            <br />
                                            <asp:Button ID="btnSearchFilter" runat="server" Text="Filter" class="btn btn-primary"
                                                OnClick="btnSearchFilter_Click" />
                                        </div>
                                        <div class="col-sm-5">
                                            <%--<label for="Button1">
                                                Message Status</label>
                                            <br />
                                            <asp:Button ID="Button1" class="btn btn-danger" runat="server" Visible="true" Text="Unread" />
                                            <asp:Button ID="Button2" class="btn btn-success" runat="server" Visible="true" Text="Read" />
                                            <asp:Button ID="Button3" class="btn btn-warning" runat="server" Visible="true" Text="Carry Forward" />
                                            <asp:Button ID="Button4" class="btn btn-info" runat="server" Visible="true" Text="Message Response" />--%>
                                        </div>
                                    </asp:Panel>
                                    <div class="form-group">
                                        <asp:DataGrid ID="dg_getAppoint" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                            BorderColor="#999999" GridLines="Both" Style="z-index: -1;" AllowPaging="true"
                                            AllowCustomPaging="true" PageSize="20" OnPageIndexChanged="dg_getAppoint_PageIndexChanged"
                                            class="table table-bordered" OnItemDataBound="dg_getAppoint_ItemDataBound" OnSortCommand="dg_getAppoint_SortCommand"
                                            AllowSorting="True" OnItemCommand="dg_getAppoint_ItemCommand">
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <SelectedItemStyle Font-Bold="false" />
                                            <ItemStyle BackColor="White" />
                                            <PagerStyle Mode="NumericPages" />
                                            <Columns>
                                                <%--0--%>
                                                <asp:BoundColumn DataField="msgID" HeaderText="UID" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--1--%>
                                                <asp:BoundColumn DataField="TodayID" HeaderText="Sl. No" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--2--%>
                                                <asp:BoundColumn DataField="VisitorName" SortExpression="VisitorName" HeaderText="Visitor Name">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--3--%>
                                                <asp:BoundColumn DataField="ToUserName" SortExpression="ToUserName" HeaderText="To Visit">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--4--%>
                                                <asp:BoundColumn DataField="MessageContent" SortExpression="MessageContent" HeaderText="Message Content">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--5--%>
                                                <asp:BoundColumn DataField="msgStatusDesc" HeaderText="Message Status" SortExpression="msgStatusDesc">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--6--%>
                                                <asp:BoundColumn DataField="FromUserName" SortExpression="FromUserName" HeaderText="Created By">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--7--%>
                                                <asp:BoundColumn DataField="CreatedDateTime" SortExpression="CreatedDateTime" HeaderText="Created DateTime">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--8--%>
                                                <asp:BoundColumn DataField="isActive" HeaderText="isActive" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--9--%>
                                                <asp:BoundColumn DataField="msgStatusID" HeaderText="msgStatusID" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--10--%>
                                                <asp:BoundColumn DataField="ResponseString" SortExpression="ResponseString" HeaderText="Response"
                                                    Visible="true">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--11--%>
                                                <asp:BoundColumn DataField="ResponseTime" SortExpression="ResponseTime" HeaderText="Response Time"
                                                    Visible="true">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Edit" Text="Edit" HeaderText=""
                                                    Visible="true" />
                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="Close" Text="Close" HeaderText=""
                                                    Visible="true" />
                                                <asp:BoundColumn DataField="PostponeTime" SortExpression="PostponeTime" HeaderText="Postpone Time"
                                                    Visible="false" />
                                                <asp:BoundColumn DataField="VisitorMessage" SortExpression="VisitorMessage" HeaderText="Visitor Message"
                                                    Visible="true">
                                                    <ItemStyle Wrap="true" Width="10%" />
                                                </asp:BoundColumn>
                                                <%--12--%>
                                                <%--<asp:TemplateColumn HeaderText="Act">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Select" CommandName="Select" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>--%>
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="false" ForeColor="White" Width="15px" />
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Timer ID="Timer1" runat="server" Interval="2000" OnTick="Timer1_Tick">
        </asp:Timer>
        <%--<asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick">
        </asp:Timer>--%>
    </div>
</asp:Content>
