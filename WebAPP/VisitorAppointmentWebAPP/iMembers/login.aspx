<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="VisitorAppointmentWebAPP.iMembers.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Sanmar VMS</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"
        name="viewport" />
    <link rel="stylesheet" href="../common/css/bootstrap.css" />
    <link rel="stylesheet" href="../common/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../common/fonts/font-awesome.min.css" />
    <link rel="stylesheet" href="../common/fonts/ionicons.min.css" />
    <link rel="stylesheet" href="../common/css/AdminLTE.css" />
    <link rel="stylesheet" href="../common/css/AdminLTE.min.css" />
</head>
<body class="hold-transition login-page">
    <form id="form1" runat="server">
    <div class="login-box">
        <div class="login-logo">
            <a href="#">Sanmar VMS</a>
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body">
            <p class="login-box-msg">
                Sign in to start your session</p>
            <%--<form action="../../index2.html" method="post">--%>
            <div class="form-group has-feedback">
                <asp:TextBox ID="txtUserName" TabIndex="1" runat="server" Text="" class="form-control"
                    placeholder="Username"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvStartLoc" Style="float: right; margin-right: 300px;"
                    ValidationGroup="login" runat="server" ControlToValidate="txtUserName" ErrorMessage="*"
                    ForeColor="Red" Font-Bold="true">
                </asp:RequiredFieldValidator>
                <%--<input type="email" class="form-control" placeholder="Username">--%>
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <%--<input type="password" class="form-control" placeholder="Password1">--%>
                <asp:TextBox ID="txtPassWord" TabIndex="2" Text="" runat="server" TextMode="Password" class="form-control"
                    placeholder="Password"> </asp:TextBox><%--TextMode="Password"--%>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Style="float: right; margin-right: 300px;"
                    TabIndex="2" ValidationGroup="login" runat="server" ControlToValidate="txtPassWord"
                    ErrorMessage="*" ForeColor="Red" Font-Bold="true"> </asp:RequiredFieldValidator>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>
            <div class="row">
                <%--                <div class="col-xs-8">
                    <div class="checkbox icheck">
                        <label>
                            <input type="checkbox">
                            Remember Me
                        </label>
                    </div>
                </div>--%>
                <!-- /.col -->
                <div class="col-xs-4">
                    <asp:Button ID="btnSubmit" class="btn btn-primary btn-block btn-flat" runat="server"
                        Text="Sign In" OnClick="btnSubmit_Click" />
                    <%--                    <button type="submit" class="btn btn-primary btn-block btn-flat">
                        Sign In</button>--%>
                </div>
                <!-- /.col -->
            </div>
            <%--</form>--%>
            <%--            <div class="social-auth-links text-center">
                <p>
                    - OR -</p>
                <a href="#" class="btn btn-block btn-social btn-facebook btn-flat"><i class="fa fa-facebook">
                </i>Sign in using Facebook</a> <a href="#" class="btn btn-block btn-social btn-google btn-flat">
                    <i class="fa fa-google-plus"></i>Sign in using Google+</a>
            </div>--%>
            <!-- /.social-auth-links -->
            <br />
            <%--<a href="#">I forgot my password</a>--%>
            <br />
            <br />
            <asp:Label class="label label-danger" Font-Size="Small" ID="lblError" runat="server"
                Text=""></asp:Label>
            <asp:Label class="label label-success" Font-Size="Small" ID="lblSuccess" runat="server" Text=""></asp:Label>
            <%--<a href="register.html" class="text-center">Register a new membership</a>--%>
        </div>
        <!-- /.login-box-body -->
    </div>
    </form>
    <!-- jQuery 2.1.4 -->
    <script src="../common/js/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="../common/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="../common/js/icheck.min.js"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });
        });
    </script>
</body>
</html>
