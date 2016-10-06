<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true"
    CodeBehind="ParkingPanda.aspx.cs" Inherits="Problem1.ParkingPanda" Async="true" ViewStateMode="Disabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBody" runat="server">
    <div class="row">
        <div id="left" class="col-md-2 col-md-offset-3">
            <div id="login" class="form-group" runat="server">
                <div>
                    <asp:Label Text="Username" runat="server" ID="lbl_userName" />
                    <asp:TextBox runat="server" ID="txt_userName" TextMode="Email" CssClass="form-control" />
                    <asp:RequiredFieldValidator ErrorMessage="The email field is required" ControlToValidate="txt_userName" runat="server" ValidationGroup="vld_login" Display="None"/>
                </div>
                <div>
                    <asp:Label Text="Password" runat="server" ID="lbl_password" />
                    <asp:TextBox runat="server" ID="txt_password" TextMode="Password" CssClass="form-control" />
                    <asp:RequiredFieldValidator ErrorMessage="The password field is required" ControlToValidate="txt_password" runat="server" ValidationGroup="vld_login" Display="None" />
                </div>
                <br />
                <div>
                    <asp:Button Text="Log in" runat="server" ID="btn_Login" OnClick="btn_Login_Click" ValidationGroup="vld_login" CssClass="btn btn-degault" />
                </div>
            </div>

            <div id="logged" runat="server" visible="false">
                <asp:Label Text="Welcome to Parking Panda" runat="server" CssClass="h5" />
                <asp:Label runat="server" ID="lbl_loggedUser" CssClass="text-success text-uppercase h4" />
            </div>
        </div>

        <div id="right" class="col-md-2 col-md-offset-1">
            <div id="notify" class="notification h5">
                <asp:Label Text="Notify" runat="server" Visible="false" ID="lbl_notify" />
                <asp:ValidationSummary runat="server" ValidationGroup="vld_login" ID="vld_group_login" />
                <asp:ValidationSummary runat="server" ValidationGroup="vld_save" ID="vld_group_save" />
            </div>

            <div id="user" class="form-group" runat="server" visible="false">
                <div>
                    <div>
                        <asp:Label Text="First Name" runat="server" ID="lbl_firstName" />
                        <asp:TextBox runat="server" ID="txt_firstName" CssClass="form-control" />
                        <asp:RequiredFieldValidator ErrorMessage="First Name is required" ControlToValidate="txt_firstName" runat="server" ValidationGroup="vld_save" Display="None" />
                    </div>
                    <div>
                        <asp:Label Text="Last Name" runat="server" ID="lbl_lastName" />
                        <asp:TextBox runat="server" ID="txt_lastName" CssClass="form-control" />
                        <asp:RequiredFieldValidator ErrorMessage="Last Name is required" ControlToValidate="txt_lastName" runat="server" ValidationGroup="vld_save" Display="None" />
                    </div>
                </div>
                <div>
                    <asp:Label Text="Email" runat="server" ID="lbl_email" />
                    <asp:TextBox runat="server" ID="txt_email" TextMode="Email" CssClass="form-control" />
                    <asp:CheckBox Text="Enable Email Message" runat="server" ID="cbox_email" />
                    <asp:RequiredFieldValidator ErrorMessage="Email is required" ControlToValidate="txt_email" runat="server" ValidationGroup="vld_save" Display="None"/>
                </div>
                <div>
                    <asp:Label Text="Phone" runat="server" ID="lbl_phone" />
                    <asp:TextBox runat="server" ID="txt_phone" TextMode="Phone" CssClass="form-control" />
                    <asp:CheckBox Text="Enable Text Message" runat="server" ID="cbox_sms" />
                    <asp:RegularExpressionValidator ErrorMessage="PhoneNumber is not valid" ControlToValidate="txt_phone" runat="server"
                        ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$" ValidationGroup="vld_save" Display="None"/>
                    <asp:RequiredFieldValidator ErrorMessage="PhoneNumber is required" ControlToValidate="txt_phone" runat="server" ValidationGroup="vld_save" Display="None"/>

                </div>
                <div>
                    <div>
                        <asp:Label Text="Current Password" runat="server" ID="lbl_currentPwd" />
                        <asp:TextBox runat="server" ID="txt_currentPwd" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div>
                        <asp:Label Text="New Password" runat="server" ID="lbl_changePwd" />
                        <asp:TextBox runat="server" ID="txt_changePwd" TextMode="Password" CssClass="form-control" />
                        <asp:Label Text="Reenter Password" runat="server" ID="lbl_reenterPwd" />
                        <asp:TextBox runat="server" ID="txt_reenterPwd" TextMode="Password" CssClass="form-control" />
                        <asp:CompareValidator ErrorMessage="New passwords are not identity " ControlToValidate="txt_changePwd" ControlToCompare="txt_reenterPwd" 
                            runat="server" ValidationGroup="vld_save" Display="None" />
                    </div>
                </div>
            </div>

            <div id="save" runat="server" visible="false">
                <asp:Button Text="Save" runat="server" ID="btn_save" OnClick="btn_Save_Click" ValidationGroup="vld_save" CssClass="btn btn-degault" />

                <input type="reset" name="btn_cancel" value="Cancel" class="btn btn-default" />
                <br />
                <br />
                <div>
                    <asp:Button Text="LogOut" runat="server" ID="btn_Logout" OnClick="btn_Logout_Click" CssClass="btn btn-default" CausesValidation="false" />
                    <%--  <input type="button" name="logout" id="btn_logout" value="Logout"" class="btn btn-default"  />--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
