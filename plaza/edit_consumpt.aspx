<%@ Page Title="Редактирование записей о затратах" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="edit_consumpt.aspx.cs" Inherits="plaza.edit_consumpt2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="maindiv">
    <asp:Panel ID="p_menu" CssClass="onerow mymenu" runat="server">
       <div class="mar10ver"><a href="consumptions.aspx">Управление затратами</a></div>
        <div class="mar10ver"><a href="plans.aspx">Планирование затрат</a></div>
       <div class="mar10ver"> <a href="cats.aspx">Категории затрат</a></div>
       <div class="mar10ver"> <a href="bils.aspx">Счета</a></div>
       <div class="mar10ver"> <a href="users.aspx">Пользователи</a></div>
       <div class="mar10ver"> <a href="reports.aspx">Отчеты</a></div>
     </asp:Panel>
    <asp:Panel ID="p_main" CssClass=" onerow maincontent" runat="server">
        <div class="mar10ver">
            <div class="onerow border1">
                <asp:Panel BackColor="white" ID="p_tv" runat="server" ScrollBars="Vertical">
                    <asp:TreeView ID="tv" Width="276px" Height="276px" runat="server">
                        <NodeStyle BorderColor="Black" ForeColor="Black" />
                        <SelectedNodeStyle BackColor="#16dbdb" />
                    </asp:TreeView>
                    </asp:Panel>
            </div>
            <div class="onerow">
                    <asp:Label ID="l_cat"  runat="server" Text="Категория"></asp:Label>
            </div>
        </div>
       <div class ="mar10ver">
           <asp:ImageButton CssClass="checkbox_checked" ID="ib_show_hide" ImageUrl="img/double_checkbox_blue.png" Width="20px" 
               runat="server" OnClick="ib_show_hide_Click" />
                <asp:Label ID="l_collapse" runat="server" Text="Свернуть все"></asp:Label>
       </div>
       <div class ="mar10ver">
           <asp:TextBox CssClass="border1" ID="tb_data" runat="server" TextMode="Date" Width="292px"></asp:TextBox>
           <asp:Label ID="l_data" runat="server" Text="Дата затрат"></asp:Label>
       </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_value" CssClass="border1" runat="server" Width="292px"></asp:TextBox>
            <asp:Label ID="l_value" runat="server" Text="Cумма"></asp:Label>
        </div>
       <div class="mar10ver">
           <asp:DropDownList ID="ddl_bils" CssClass="border1" Width="296px" runat="server"></asp:DropDownList>
           <asp:Label ID="l_bil" runat="server" Text="Счет"></asp:Label>
       </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_descript" CssClass="border1" TextMode="MultiLine" runat="server" Height="55px" 
                Width="292px"></asp:TextBox> 
            <asp:Label ID="l_descript" runat="server" Text="Комментарий"></asp:Label>
        </div>
       <div class="mar10ver">
         <asp:Button ID="b_save" CssClass="bluebutton" Height="30px" runat="server" Text="Сохранить" OnClick="b_save_Click" />
            <asp:Button ID="b_cancel" CssClass="greenbutton mar10hor" Height="30px" runat="server" Text="Отмена" OnClick="b_cancel_Click" />
        </div>
    </asp:Panel>
  </div>
</asp:Content>
