<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HandleAuthorization.aspx.cs" Inherits="WebApi.HandleAuthorization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">

        function onLoad() {
            //var url = getUrlParam("redirectUrl", location.href);
            //location.href = url;
            window.parent.postMessage('ready', '*');
        }

        function getUrlParam(name, url) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(decodeURIComponent(url));

            if (results == null) {
                return "";
            }
            else {
                return results[1];
            }
        }

        window.onload = onLoad;
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            Authorization successful!
        </h1>
    </div>
    </form>
</body>
</html>