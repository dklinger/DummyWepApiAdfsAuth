<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="WebApi.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function custom_OnLoad()
        {
            alert(window.parent.document);
        }

        window.onload = custom_OnLoad;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>
        Das ist ein Test!
    </h1>
    </div>
    </form>
</body>
</html>