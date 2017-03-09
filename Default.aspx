<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        $(document).ready(function () {
            setInterval(function () {
                if (window.innerWidth < 694) {
                    $(".heightable").height("131");
                    $(".paddable").css("padding-top", "0");
                }  else if (window.innerWidth < 913) {
                    $(".heightable").height((350 - 913 + window.innerWidth).toString());
                    $(".paddable").css("padding-top", (window.innerWidth * 0.41 - 292.2).toString() + "px");
                } else {
                    $(".heightable").height("350");
                    $(".paddable").css("padding-top", "90px");
                }
            }, 30);
        });
    </script>
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <div class="heightable" style="width: 100%; height: 350px; background-color: rgb(50, 17, 255);">
        <span style="float: right;">
            <img class="heightable" src="Images/community.png" style="max-height: 350px; height: 350px; width: auto;" />
        </span>
        <div id="inspiretext" class="paddable" style="vertical-align: middle; padding-top: 90px; padding-left: 100px; height: 350px;">
        <h1 style="color: yellow; position: absolute; font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 35px; font-style: italic; flex: 1; display: inline;">
            Start with helping,<br />&nbsp;&nbsp;&nbsp;Then profit.
        </h1>
        </div>
    </div>
</asp:Content>

