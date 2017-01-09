<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CustomCache._Default" Title="Default" %>
<%@ Import Namespace="CustomCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <% foreach (var user in this.Users) { %>
        UserID: <%= this.FastEval(user, "UserID") %><br />
        UserName: <%= this.FastEval(user, "UserName") %><br />
    <% } %>
    
    <hr />
    <asp:Repeater runat="server" ID="rptUsers">
        <ItemTemplate>
            UserID: <%# this.FastEval("UserID") %><br />
            UserName: <%# this.FastEval("UserName") %><br />
        </ItemTemplate>
    </asp:Repeater>
</body>
</html>
