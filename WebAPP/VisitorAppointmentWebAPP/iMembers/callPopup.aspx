<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="callPopup.aspx.cs" Inherits="MBSERPs.iMembers.callPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
          <h1>
            Customer
            <small>Auto Identification</small>
          </h1>
          <ol class="breadcrumb">
            <li><a href="../dashboard/home.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
            <li><a href="#">Customer</a></li>
            <li class="active">Call Popup</li>
          </ol>
        </section>
    <!-- Main content -->
    <section class="content">
    <div class="row">
        <center>
        <div class="col-md-6">
            <!-- general form elements -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <h3 class="box-title">
                        Call Info</h3>
                </div>
                <!-- /.box-header -->

                <div class="box-body">

                                 <div class="form-group">
                      <label for="txt_CType">Customer Type</label>                      
                      <%--<input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">--%>
                       <asp:TextBox id="txt_CType"  class="form-control"  runat="server" Text=""  ReadOnly="true"></asp:TextBox>
                      </div>

                                                       <div class="form-group">
                      <label for="txt_CustName">Name</label>                      
                      <%--<input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">--%>
                       <asp:TextBox id="txt_CustName"  class="form-control"  runat="server" Text=""  ReadOnly="true"></asp:TextBox>
                      </div>

                      <label for="txt_Mobile">MobileNo</label>                      
                      <%--<input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">--%>
                       <asp:TextBox id="txt_Mobile"  class="form-control"  runat="server" Text=""  ReadOnly="true"></asp:TextBox>
                      </div>
                                                      <div class="box-footer">

                                            <asp:Label class="label label-danger" Font-Size="Small" ID="lblError" runat="server"
                Text=""></asp:Label>
            <asp:Label class="label label-success" Font-Size="Small" ID="lblSuccess" runat="server" Text=""></asp:Label>

                    <asp:Button ID="btnSubmit" class="btn btn-primary  pull-right" runat="server"
                        Text="Get" OnClick="btnSubmit_Click" />

                        <asp:Button ID="btnCancel" class="btn btn-default pull-right" runat="server"
                        Text="Cancel" OnClick="btnCancel_Click" />
                  </div>


                </div>
            </div>
        </div>
        </center>
    </div>
    </section>
</asp:Content>
