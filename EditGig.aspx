<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditGig.aspx.vb" Inherits="EditGig" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var load = function () {
            $(".aboutTxt").bind('input propertychange', function () {
                if ($(this).val().length < 30 && $(this).val() !== "") {
                    $("#lblCharCount").show();
                    $("#lblCharCount").text((30 - $(this).val().length).toString() + " chars to go!");
                } else {
                    $("#lblCharCount").hide();
                }
            });
        };

        $(document).ready(load);
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>Want a change?</h2>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <b>Title:</b><br />
                        <asp:TextBox placeholder="Gig title" CssClass="large_txt" style="font-size: 17px;" ID="txtTitle" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="1"></asp:TextBox>
                        <br />
                        <b>Short description:</b><br />
                        <asp:TextBox placeholder="Short description" CssClass="large_txt" style="font-size: 17px;" ID="txtShortDescription" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="2"></asp:TextBox>
                        <br />
                        <b>Description:</b><br />
                        <asp:TextBox placeholder="Description (markdown friendly)" CssClass="large_txt aboutTxt" style="font-size: 17px;" ID="txtDescription" runat="server" Width="70%" Height="300px" TextMode="MultiLine" Font-Bold="True" AutoPostBack="True" TabIndex="5"></asp:TextBox>
                        <br />
                        <b>Price:</b><br />
                        <asp:TextBox placeholder="Price (in DeedCoins)" CssClass="large_txt" style="font-size: 17px;" ID="txtPrice" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="2"></asp:TextBox>
                        <br />
                        <b>Video URL:</b><br />
                        <asp:TextBox placeholder="Video URL" CssClass="large_txt" style="font-size: 17px;" ID="txtVideo" runat="server" Width="70%" Font-Bold="True" AutoPostBack="True" TabIndex="2"></asp:TextBox>
                        <br />
                        <b>Image URLs:</b><br />
                        <asp:TextBox placeholder="Image URLs (one per line)" CssClass="large_txt aboutTxt" style="font-size: 17px;" ID="txtImages" runat="server" Width="70%" Height="93px" TextMode="MultiLine" Font-Bold="True" AutoPostBack="True" TabIndex="5"></asp:TextBox>
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblError" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></asp:Label>
                <asp:LinkButton ID="btnRegister" runat="server" TabIndex="7">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Update</div>
                </asp:LinkButton>
            </td><td style="width: 20%;"></td>
        </tr>
    </table>
</asp:Content>
