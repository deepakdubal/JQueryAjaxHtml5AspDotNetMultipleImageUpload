<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="AddImage.aspx.cs" Inherits="AddImage" %>

<!DOCTYPE>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="js/jquery-1.11.1.js"></script>
    <style type="text/css">
        #Preview canvas {
          padding:5px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div id="MainDiv" style="display: block;">

            <table class="auto-style1">
                <colgroup></colgroup>
                <colgroup></colgroup>
                <colgroup></colgroup>
                <tr>
                    <td class="auto-style2" colspan="2">
                        Folder Name 
                    </td>
                    <td class="auto-style2">  <input id="FolderName" type="text" value="Deepak" /></td>
                </tr>
                  <tr runat="server" id="divUpload">
                    <td colspan="3">
                        <input id="Button2" type="button" value="Upload" style="display: none" />
                        <span class="btn btn-success fileinput-button">
                            <span>Choose Images...</span>
                            <asp:FileUpload ID="myFile0" runat="server" multiple="multiple" data-maxwidth="100" data-maxheight="100" data-maxwidthUpload="800" data-maxheightUpload="600" />
                        </span>

                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="Preview">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <input id="Button1" type="button" value="Start Upload" onclick="return form_Submit();" style="display: none" />
                        <asp:Button ID="Button3" runat="server" Text="Submit" OnClick="Button2_Click" Style="display: none;" />

                    </td>
                </tr>

                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <div style="display: none;" id="UpdateProgress1">

                <div class="progressbarCss" id="divProgress">
                </div>
                <div class="style1" style="background-color: rgb(255, 255, 255); border: 5px solid rgb(123, 123, 123); padding: 5px; position: fixed; top: 0px; left: 600px;" id="loading">
                    <table cellspacing="1" cellpadding="5">
                        <tbody>
                            <tr>
                                <td>
                                    <img src="images/LodingCart.gif" alt="" /></td>
                                <td>
                                    <div class="loder">
                                        Please wait your images is being uploaded...
                                    </div>
                                    <div class="success">
                                        Please wait your information is being Submited...
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>
        <div id="MessageDiv" style="display: none;">
            The File APIs are not fully supported in this browser. We recommend you to use latest browsers like Google Chrome, Mozila Firefox, Safari or Internet Explore Version 10 or higher
        </div>
    </form>
    <script src="js/UploadImages.js"></script>
</body>
</html>

