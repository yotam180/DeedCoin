<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UploadImage.aspx.vb" Inherits="UploadImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function readURL(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#ContentPlaceHolder1_Image1').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }
        $(document).ready(function () {
            $("#ContentPlaceHolder1_FileUpload1").change(function () {
                readURL(this);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 100%; table-layout: fixed; text-align: center" border="0">
        <tr>
            <td style="width: 20%; text-align: center;"></td><td>
                <br />
                <h2>Upload a new Profile Picture</h2>
                <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" ></asp:Image>
                <br />
                Select file:
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <br /><br />
                <asp:LinkButton ID="btnUpload" runat="server" TabIndex="3">
                    <div class="button_div" style="left: 30%; width: 40%; height: 20px; position: relative;">Upload</div>
                </asp:LinkButton>
            </td><td style="width: 20%; text-align: center;"></td>
        </tr>
    </table>
</asp:Content>

