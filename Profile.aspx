﻿<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Profile.aspx.vb" Inherits="Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href='//fonts.googleapis.com/css?family=Oleo+Script' rel='stylesheet' type='text/css'>
    <style>
        .icon {
            background-position: 0px -97px, 0px -97px;
            box-sizing: border-box;
            color: rgb(102, 102, 102);
            display: inline-block;
            height: 14px;
            text-align: left;
            vertical-align: middle;
            width: 12px;
            column-rule-color: rgb(102, 102, 102);
            perspective-origin: 6px 7px;
            transform-origin: 6px 7px;
            border: 0px none rgb(102, 102, 102);
            font: normal normal normal normal 13px / 16.9px Arial, "Helvetica Neue", Helvetica, sans-serif;
            list-style: none outside none;
            margin: 0px 3px 0px 0px;
            outline: rgb(102, 102, 102) none 0px;
        }

        #icon_location {
            background: url("/Style/user-profile-sprite.svg") repeat scroll 0px -97px / auto padding-box border-box, rgba(0, 0, 0, 0) none repeat scroll 0px -97px / auto padding-box border-box;
        }
        #icon_memfor {
            background: url("/Style/user-profile-sprite.svg") repeat scroll 0px -63px / auto padding-box border-box, rgba(0, 0, 0, 0) none repeat scroll 0px -63px / auto padding-box border-box;
        }
        #icon_profileviews {
            background: url("/Style/user-profile-sprite.svg") repeat scroll 0px -182px / auto padding-box border-box, rgba(0, 0, 0, 0) none repeat scroll 0px -182px / auto padding-box border-box;
        }
        #icon_lastlogin {
            background: url("/Style/user-profile-sprite.svg") repeat scroll 0px -147px / auto padding-box border-box, rgba(0, 0, 0, 0) none repeat scroll 0px -147px / auto padding-box border-box;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlUser" runat="server" Visible="true">
        <table style="width: 100%; min-height: 100px; table-layout: fixed;" border="0" cellpadding="10">
            <tr>
                <td style="width: 15%;"></td>
                <td style="width: 200px; vertical-align: top; text-align: center;">
                    <asp:LinkButton ID="btnImage" runat="server">
                        <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" ></asp:Image>
                    </asp:LinkButton>
                    <div style="width: 100%; text-align: center; border: 1px solid; border-color: lightgray; border-radius: 10px;">
                        <b><asp:Label ID="lblEXP" runat="server" Text="Label"></asp:Label></b>
                        <span style="color: darkslategray; font-size: small; font-family: 'Arial Rounded MT'">&nbsp;&nbsp;&nbsp;EXP</span>
                    </div>
                    <asp:Panel ID="pnlCash" runat="server">
                        <div style="width: 100%; text-align: center; border: 1px solid; border-color: lightgray; border-radius: 10px;">
                        <b><asp:Label ID="lblCash" runat="server" Text="Label"></asp:Label></b>
                        <span style="color: darkslategray; font-size: small; font-family: 'Arial Rounded MT'">&nbsp;&nbsp;&nbsp;dC</span>
                    </div>
                    </asp:Panel>
                    <div style="width: 100%; text-align: center; border: 1px solid; border-color: lightgray; border-radius: 10px;">
                        <span style="color: darkslategray; font-size: small; font-family: 'Arial Rounded MT'">Rating: &nbsp;&nbsp;&nbsp; </span>                        
                        <b><asp:Label ID="lblRating" runat="server" Text="lblRating"></asp:Label></b>
                    </div>
                </td>
                <td>
                    <table style="width: 100%; height: 100%; position: relative; table-layout: fixed;" border="0" cellpadding="5">
                        <tr>
                            <td colspan="2" style="vertical-align: top;">
                                <h2><asp:Label ID="lblFullName" runat="server" Text="Label"></asp:Label> <span style="color: gray; font-size: medium;">(@<asp:Label ID="lblUsername" runat="server" />)</span></h2>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnEdit" runat="server">Edit</asp:LinkButton><br /><br />
                                <asp:LinkButton ID="btnMessage" runat="server">Message</asp:LinkButton>
                             </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="padding: 5px; width: 90%; height: 200px; overflow-y: auto; background-color: inherit; border: .5px solid lightgray; border-radius: 10px;">
                                    <span style="font-family: 'Oleo Script'; font-size: 16px;"><asp:Label ID="lblAbout" runat="server"></asp:Label></span>
                                </div>
                            </td>
                            <td style="vertical-align: top; color: rgb(102, 102, 102); font: normal normal normal normal 13px / 16.9px Arial, 'Helvetica Neue', Helvetica, sans-serif;">

                                <span class="icon" id="icon_location"></span>
                                <asp:Label ID="lblCountry" runat="server" Text="Label"></asp:Label><br />

                                <span class="icon" id="icon_memfor"></span>
                                <asp:Label ID="lblMemfor" runat="server" Text="Label"></asp:Label><br />

                                <span class="icon" id="icon_profileviews"></span>
                                <asp:Label ID="lblProfileViews" runat="server" Text="Label"></asp:Label><br />

                                <span class="icon" id="icon_lastlogin"></span>
                                <asp:Label ID="lblLastLogin" runat="server" Text="Label"></asp:Label><br />
                             </td>
                        </tr>
                    </table>
                    <h2>Gigs by this user:</h2>
                </td>
                <td style="width: 15%;"></td>
            </tr>
        </table>
        <div style="border: 2px solid black; width: 70%; margin: auto; min-height: 200px;">
            <asp:Label ID="lblGigs" runat="server"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <div style="width: 100%; text-align: center;">
            <h2>Sorry.. This user was not found :(</h2>
        </div>
    </asp:Panel>
</asp:Content>

