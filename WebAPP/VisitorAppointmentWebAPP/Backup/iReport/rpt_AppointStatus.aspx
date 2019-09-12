<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="rpt_AppointStatus.aspx.cs" Inherits="VisitorAppointmentWebAPP.iReport.rpt_AppointStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function targetMeBlank() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <asp:ScriptManager ID="SM1" runat="server" />
    <section class="content-header">
          <h1>
            
            <small>Visitor Appointment Status</small>
          </h1>
          
          <ol class="breadcrumb">
            <li class="active"><i class="fa fa-calendar"></i> VMS</li>
            <li class="active">Report</a></li>
            <li class="active">Appointment Status</li>
                      
            </li>
          </ol>
    </section>
    <!-- Main content -->
    <%--  <section class="content">--%>
    <div class="content">
        <div class="row">
            <%--Customer Info--%>
            <div class="col-md-12">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">
                            Report</h3>
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                    </div>
                    <div class="box-body">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearchFilter">
                                <div class="col-md-3">
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
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txt_fileterStr">
                                            Search Keyword</label><br />
                                        <asp:TextBox ID="txt_fileterStr" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <%--<label for="btnSearchFilter">
                                        Action</label>--%>
                                    <br />
                                    <asp:Button ID="btnSearchFilter" runat="server" Text="Filter" class="btn btn-primary"
                                        OnClick="btnSearchFilter_Click" />
                                </div>
                                <div class="col-md-5">
                                    <%--<label for="Button1">
                                        Message Status</label>
                                    <br />
                                    <asp:Button ID="Button1" class="btn btn-danger" runat="server" Visible="true" Text="Unread" />
                                    <asp:Button ID="Button2" class="btn btn-success" runat="server" Visible="true" Text="Read" />
                                    <asp:Button ID="Button3" class="btn btn-warning" runat="server" Visible="true" Text="Carry Forward" />
                                     <asp:Button ID="Button4" class="btn btn-info" runat="server" Visible="true"
                                Text="Message Response" />--%>
                                </div>
                            </asp:Panel>
                            <div class="form-group">
                                <asp:DataGrid ID="dg_getAppoint" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                    BorderColor="#999999" GridLines="Both" Style="z-index: -1;" class="table table-bordered"
                                    OnItemDataBound="dg_getAppoint_ItemDataBound" AllowPaging="true" AllowCustomPaging="true"
                                    AllowSorting="true" OnSortCommand="dg_getAppoint_SortCommand"
                                        PageSize="20" OnPageIndexChanged="dg_getAppoint_PageIndexChanged">
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
                                        <asp:BoundColumn DataField="msgStatusDesc" SortExpression="msgStatusDesc" HeaderText="Message Status">
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
                                        <asp:BoundColumn DataField="ResponseString" SortExpression="ResponseString" HeaderText="Response" Visible="true">
                                            <ItemStyle Wrap="true" />
                                        </asp:BoundColumn>

                                        <%--11--%>
                                        <asp:BoundColumn DataField="ResponseTime" SortExpression="ResponseTime" HeaderText="Response Time" Visible="true">
                                            <ItemStyle Wrap="true" />
                                        </asp:BoundColumn>

                                        <%--10--%>
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
                    
                    <div class="box-footer">
                        <div class="form-group">
                            <br />
                            <asp:Label class="label label-danger pull-left" Font-Size="Small" ID="lblError" runat="server"
                                Text=""></asp:Label>
                            <asp:Label class="label label-success pull-left" Font-Size="Small" ID="lblSuccess"
                                runat="server" Text=""></asp:Label>                            
                        </div>
                    </div>
               
                </div>
            </div>
        </div>
    </div>
</asp:Content>
