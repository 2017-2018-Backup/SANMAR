<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="MBSERPs.iMembers.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
          <h1>
            Profile
            <small>User</small>
          </h1>
          <ol class="breadcrumb">
            <li><a href="../dashboard/home.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">Profile</a></li>
            <li class="active">Change Password</li>
          </ol>
        </section>
    <!-- Main content -->
    <section class="content">
    <div class="row">
        <div class="col-md-12">
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        Change Password</h3>
                </div>
                <!-- /.box-header -->

                <div class="box-body">
                    <div class="form-group">
                      <label for="txt_UserId">User Name</label>                      
                      <%--<input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">--%>
                       <asp:TextBox id="txt_UserId"  class="form-control"  runat="server"  ReadOnly="true"></asp:TextBox>
                      </div>
                      <div class="form-group">
                      <label for="txt_CurrentPwd">Current Password</label>
                      <asp:TextBox id="txt_CurrentPwd"  class="form-control"  runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="form-group">
                      <label for="txtNewPwd">New Password</label>
                      <asp:TextBox id="txtNewPwd"  class="form-control"  runat="server" TextMode="Password"></asp:TextBox>
                      
                    </div>
                    <div class="form-group">
                      <label for="txt_ReEnterNewPwd">Re-Enter New Password</label>
                      <asp:TextBox id="txt_ReEnterNewPwd"  class="form-control"  runat="server" TextMode="Password"></asp:TextBox>
                      
                    </div>
                                <div class="box-footer">

                                            <asp:Label class="label label-danger" Font-Size="Small" ID="lblError" runat="server"
                Text=""></asp:Label>
            <asp:Label class="label label-success" Font-Size="Small" ID="lblSuccess" runat="server" Text=""></asp:Label>

                    <asp:Button ID="btnSubmit" class="btn btn-primary  pull-right" runat="server"
                        Text="Submit" OnClick="btnSubmit_Click" />

                        <asp:Button ID="btnCancel" class="btn btn-default pull-right" runat="server"
                        Text="Cancel" OnClick="btnCancel_Click" />
                  </div>
                </div>   
            </div>
        </div>
    </div>
    </section>
</asp:Content>
