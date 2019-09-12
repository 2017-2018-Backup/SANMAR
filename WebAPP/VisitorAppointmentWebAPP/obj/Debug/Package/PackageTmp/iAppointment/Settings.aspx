<%@ Page Title="" Language="C#" MasterPageFile="~/master/admin.Master" AutoEventWireup="true"
    CodeBehind="Settings.aspx.cs" Inherits="VisitorAppointmentWebAPP.iAppointment.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="COntent2" runat="server">
    <asp:ScriptManager ID="SM1" runat="server" />
    <div class="content">
        <div class="col-sm-12">
            <div class="box box-primary">
                <div class="box-body">
                    <div class="row">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <h1 class="box-title" style="font-size: 15px !important; font-weight: 300 !important">
                                            <b>Chairman</b></h1>
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            General Tab</label>&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="rdoGen1" Text="Show" GroupName="General" Checked="true"
                                            OnCheckedChanged="rdoGen1_CheckedChanged" />
                                        &nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="rdoGen2" Text="Hide" GroupName="General" OnCheckedChanged="rdoGen2_CheckedChanged" />
                                        <%--  <asp:RadioButtonList RepeatDirection="Horizontal" runat="server" ID="CGeneral">
                                            <asp:ListItem Selected="True" Text="Show" Value="Show"></asp:ListItem>
                                            <asp:ListItem Text="Hide" Value="Hide"></asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            Urgent Tab</label>&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="RadioButton1" Text="Show" GroupName="Urgent"
                                            Checked="true" OnCheckedChanged="RadioButton1_CheckedChanged" />
                                        &nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="RadioButton2" Text="Hide" GroupName="Urgent" />
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <h1 class="box-title" style="font-size: 15px !important; font-weight: 300 !important">
                                            <b>Deputy Chairman</b></h1>
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            General Tab</label>&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="rdoDGen1" Text="Show" GroupName="GeneralD" Checked="true" />
                                        &nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="rdoDGen2" Text="Hide" GroupName="GeneralD" />
                                    </div>
                                    <div class="form-group">
                                        <label>
                                            Urgent Tab</label>&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="RadioButton3" Text="Show" GroupName="UrgentD"
                                            Checked="true" />
                                        &nbsp; &nbsp;
                                        <asp:RadioButton runat="server" ID="RadioButton4" Text="Hide" GroupName="UrgentD" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
