<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="autorize.aspx.cs" Inherits="plaza.Content.temp_auto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Авторизация</title>
    <link href="/Content/mycss.css" rel="stylesheet"/>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="textcenter">
               <h1 ><span class="logo1">Pla</span><span class="logo2">Za</span></h1>
        <asp:Label ID="l_enter" runat="server" Font-Names="Helvetica, sans-serif"><h2>Вход в личный кабинет</h2></asp:Label>
        <asp:TextBox ID="tb_login" runat="server" CssClass="autorise border1" Font-Size="Larger"></asp:TextBox><br />
        <asp:Label ID="l_login" runat="server" Text="Введите логин"></asp:Label>
        <asp:TextBox Font-Size="Larger" ID="tb_pass" runat="server" CssClass="autorise border1 mar10up" TextMode="Password"><h2></h2></asp:TextBox><br />
        <asp:Label ID="l_pass" runat="server" Text="Введите пароль"></asp:Label><br />
        <!--<div style"text-align:left;">
            <asp:ImageButton ImageAlign="Left" ID="ib_show_hide_search" CssClass="checkbox_uncheck likebukva" ImageUrl="~/img/double_checkbox_blue.png" 
                    runat="server" OnClick="ib_show_hide_search_Click" />
            <p align="left">Запомнить меня</p>
        </div>-->
        <asp:Button ID="b_enter" runat="server" Text="Войти" CssClass="bluebutton autorise mar10up" OnClick="b_enter_Click" />
        <asp:Panel ID="p_error" CssClass="diverror" Visible="false" runat="server">
            <asp:Label ForeColor="White" ID="l_error" runat="server" Text="Логин или пароль введены неправильно"></asp:Label>
        </asp:Panel>
            </div>
    </div>
    </form>
</body>
</html>
