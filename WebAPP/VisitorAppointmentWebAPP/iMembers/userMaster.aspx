<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="userMaster.aspx.cs" Inherits="VisitorAppointmentWebAPP.iMembers.userMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <asp:ScriptManager ID="SM1" runat="server" />
    <!-- Content Header (Page header) -->
    <section class="content-header">
          <h1>
            User
            <small>Account</small>
          </h1>
          
          <ol class="breadcrumb">
            <li><a href="../dashboard/home.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">User</a></li>
            <li class="active">Configuration</li>
          </ol>
    </section>
    <!-- Main content -->
    <%--  <section class="content">--%>
    <div class="content">
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div class="row">
                    <%--Customer Info--%>
                    <div class="col-md-12">
                        <!-- general form elements -->
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    User Add/Edit</h3>
                                <asp:Label ID="lbl_custType" runat="server" ForeColor="Red" Font-Bold="true" Text=""></asp:Label>
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                            
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txt_Name">
                                            Name</label><asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator8"
                                                runat="server" ControlToValidate="txt_Name" ErrorMessage="*" ValidationGroup="SubmitUserAccount"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator7"
                                            runat="server" ControlToValidate="txt_Name" ErrorMessage="*" ValidationGroup="EditUserAccount"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:TextBox ID="txt_Name" class="form-control" runat="server" Text="" TabIndex="1"></asp:TextBox>
                                    </div>
                                <div class="form-group">
                                        <label for="txt_UserName">
                                            User Name</label><asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator3"
                                                runat="server" ControlToValidate="txt_UserName" ErrorMessage="*" ValidationGroup="SubmitUserAccount"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator10"
                                            runat="server" ControlToValidate="txt_UserName" ErrorMessage="*" ValidationGroup="EditUserAccount"></asp:RequiredFieldValidator>
                                        <%-- <asp:CustomValidator runat="server" ID="cusCustom" ControlToValidate="txt_UserName"
                                    OnServerValidate="isUsernameAllreadyExistSeverValidation" ErrorMessage="UserName Alread Exist!!"
                                    EnableClientScript="false" Display="Dynamic" ValidateEmptyText="true" Font-Bold="true"
                                    ForeColor="Red" ValidationGroup="SubmitUserAccount" />--%>
                                        <br />
                                        <asp:TextBox ID="txt_UserName" class="form-control" runat="server" Text="" TabIndex="6"></asp:TextBox>
                                    </div>
                                           <div class="form-group">

                                        <asp:Label ID="lbl_MobileIMENo" runat="server" Text="Mobile IMENo" Font-Bold="true"   Visible="false"></asp:Label>
                                        <br />
                                        <asp:TextBox ID="txt_MobileIMENo" class="form-control" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txt_MobileNo">
                                            Mobile No</label><asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator1"
                                                runat="server" ControlToValidate="txt_MobileNo" ErrorMessage="*" ValidationGroup="SubmitUserAccount"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator9"
                                            runat="server" ControlToValidate="txt_MobileNo" ErrorMessage="*" ValidationGroup="EditUserAccount"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:TextBox ID="txt_MobileNo" class="form-control" runat="server" Text="" TabIndex="2"></asp:TextBox>
                                    </div>
                                      <div class="form-group">
                                        <label for="txt_Pass">
                                            Password</label><asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator5"
                                                runat="server" ControlToValidate="txt_Pass" ErrorMessage="*" ValidationGroup="SubmitUserAccount"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:TextBox ID="txt_Pass" class="form-control" runat="server" TabIndex="7" TextMode="Password"></asp:TextBox>
                                    </div>
                             
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txt_Email">
                                            EMail</label><asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator2"
                                                runat="server" ControlToValidate="txt_Email" ErrorMessage="*" ValidationGroup="SubmitUserAccount"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator Font-Bold="true" ForeColor="Red" ID="RequiredFieldValidator11"
                                            runat="server" ControlToValidate="txt_Email" ErrorMessage="*" ValidationGroup="EditUserAccount"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:TextBox ID="txt_Email" class="form-control" runat="server" Text="" TabIndex="3"></asp:TextBox>
                                    </div>
                    <div class="form-group">
                                        <label for="txt_PassConfirm">
                                            Confirm Password</label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*"
                                            ControlToValidate="txt_PassConfirm" Font-Bold="true" ForeColor="Red" ToolTip="Compare Password is a REQUIRED field"
                                            ValidationGroup="SubmitUserAccount">
                                        </asp:RequiredFieldValidator><asp:CompareValidator ID="CompareValidator1" runat="server"
                                            ControlToValidate="txt_PassConfirm" ControlToCompare="txt_Pass" ValidationGroup="SubmitUserAccount"
                                            ErrorMessage="Password must be same" Font-Bold="true" ForeColor="Red" ToolTip="Password must be the same" />
                                        <br />
                                        <asp:TextBox ID="txt_PassConfirm" class="form-control" runat="server" Text="" TabIndex="8"
                                            TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                  <div class="form-group">
                                        <label for="txt_Name">
                                            Role</label>
                                        <br />
                                        <asp:DropDownList ID="drp_UserRole" runat="server" class="form-control" AutoPostBack="true"
                                            TabIndex="5" OnSelectedIndexChanged="drp_UserRole_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Enabled="true">Receptionist</asp:ListItem>
                                            <asp:ListItem Value="2">Dept Chairman</asp:ListItem>
                                            <asp:ListItem Value="3">Chairman</asp:ListItem>
                                            <asp:ListItem Value="4" Enabled="false">Admin</asp:ListItem>
                                            <%--<asp:ListItem Text="SERVICE SUPERVISER" Value="90"></asp:ListItem>
                                            <asp:ListItem Text="SERVICE ENGINEER" Value="91"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="txt_MobileNo">
                                            is Active</label>
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="chk_isActive" Checked="true" runat="server" />
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
                                        <asp:Button ID="btnEdit" class="btn btn-primary  pull-right" runat="server" Visible="false"
                                            Text="Edit" OnClick="btnEdit_Click" ValidationGroup="EditUserAccount" />
                                        <asp:Button ID="btnAddNew" class="btn btn-primary  pull-right" runat="server" Visible="false"
                                            Text="Add New" OnClick="btnAddNew_Click" />
                                        <asp:LinkButton PostBackUrl="~/dashboard/home.aspx" ID="btnCancel" class="btn btn-primary  pull-right"
                                            runat="server" Visible="true" Text="Cancel" />
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12">
                                 <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearchFilter">
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="txt_PassConfirm">
                                                Filter by</label><br />
                                            <asp:DropDownList ID="drp_FilterBy" runat="server" class="form-control">
                                                <asp:ListItem Selected="True" Text="Username" Value="Username"></asp:ListItem>
                                                <asp:ListItem Text="First name" Value="FirstName"></asp:ListItem>
                                                <asp:ListItem Text="Mobile No" Value="MobileNo"></asp:ListItem>
                                                <asp:ListItem Text="E-Mail ID" Value="MailID"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="form-group">
                                            <label for="txt_PassConfirm">
                                                Search Keyword</label><br />
                                            <asp:TextBox ID="txt_fileterUsername" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="btnSearchFilter" runat="server" Text="Filter" class="btn btn-primary"
                                            OnClick="btnSearchFilter_Click" />
                                    </div>
                                    </asp:Panel>
                                    <div class="form-group">
                                        <asp:DataGrid ID="dg_getUser" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC"
                                            BorderColor="#999999" GridLines="Both" Style="z-index: -1;" class="table table-bordered" OnItemCommand="dg_getOrderHistory_ItemCommand">
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <SelectedItemStyle Font-Bold="false" />
                                            <ItemStyle BackColor="White" />
                                            <PagerStyle Mode="NumericPages" />
                                            <Columns>
                                                <%--0--%>
                                                <asp:BoundColumn DataField="UID" HeaderText="UID" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--1--%>
                                                <asp:BoundColumn DataField="Username" HeaderText="Username">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--2--%>
                                                <asp:BoundColumn DataField="Name" HeaderText="Name">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--3--%>
                                                <asp:BoundColumn DataField="MobileNo" HeaderText="MobileNo">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--4--%>
                                                <asp:BoundColumn DataField="EmailID" HeaderText="EmailID">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--5--%>
                                                <asp:BoundColumn DataField="vcRole" HeaderText="User Role">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--6--%>
                                                <asp:BoundColumn DataField="MobileIMENo" HeaderText="Mobile IMENo" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--7--%>
                                                <asp:BoundColumn DataField="UserRoleID" HeaderText="UserRoleID" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--8--%>
                                                <asp:BoundColumn DataField="isActive" HeaderText="isActive">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>
                                                <%--9--%>
                                               <%--   <asp:BoundColumn DataField="vcAddress" HeaderText="vcAddress" Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                </asp:BoundColumn>--%>
                                                <%--10--%>
                                                <asp:TemplateColumn HeaderText="Act">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Select" CommandName="Select" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Bold="false" ForeColor="White" Width="15px" />
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
