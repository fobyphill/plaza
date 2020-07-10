<%@ Page Title="Добавить запись о планировании" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="add_plan.aspx.cs" Inherits="plaza.add_plan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="maindiv">
    <div class="onerow mymenu">
       <div class="mar10ver"><a href="consumptions.aspx">Управление затратами</a></div>
        <div class="mar10ver"><a href="plans.aspx">Планирование затрат</a></div>
       <div class="mar10ver"> <a href="cats.aspx">Категории затрат</a></div>
       <div class="mar10ver"> <a href="bils.aspx">Счета</a></div>
       <div class="mar10ver"> <a href="users.aspx">Пользователи</a></div>
       <div class="mar10ver"> <a href="reports.aspx">Отчеты</a></div>
   </div>
   <div class=" onerow maincontent">
       <div class="mar10ver">
        <div class="onerow "><div class="border1">
            <asp:Panel BackColor="White" ID="p_tv" runat="server" ScrollBars="Vertical">
            <asp:TreeView Width="275px" Height =" 275px" ID="tv" runat="server" style="margin-right: 20px">
                <NodeStyle BorderColor="Black" ForeColor="Black" />
                <SelectedNodeStyle BackColor="#16dbdb" />
            </asp:TreeView>
                </asp:Panel>
                </div>
        </div>
        <div class="onerow">
            <asp:Label ID="l_cat" runat="server" Text="Категория"></asp:Label>
        </div>
    </div>
    <div class="mar10ver">
        <asp:ImageButton ID="ib_show_hide" CssClass="checkbox_checked" ImageUrl="img/double_checkbox_blue.png" Width="20px" runat="server" OnClick="ib_show_hide_Click" />
                <asp:Label ID="l_collapse" runat="server" Text="Свернуть все"></asp:Label>
    </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_value" CssClass="border1" runat="server" Width="310px"></asp:TextBox>
            <asp:Label ID="l_value" runat="server" Text="Cумма"></asp:Label>
        </div>
       <div class ="mar10ver">
           <asp:TextBox CssClass="border1" ID="tb_data_plan" runat="server" TextMode="Month" Width="310px"></asp:TextBox>
           <asp:Label ID="l_period" runat="server" Text="Период планируемых затрат"></asp:Label>
       </div>
       <div class="mar10ver">
           <asp:DropDownList class="border1" ID="ddl_bils" runat="server" Width="314px">
           </asp:DropDownList>
           <asp:Label ID="l_bil" runat="server" Text="Cчет"></asp:Label>
       </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_descript" CssClass="border1" TextMode="MultiLine" runat="server" Height="55px" Width="310px"></asp:TextBox> 
            <asp:Label ID="l_descript" runat="server" Text="Комментарий"></asp:Label>
        </div>
       <div class="mar10ver">
         <asp:Button CssClass="bluebutton" Height="30px" ID="b_save" runat="server" Text="Сохранить" OnClick="b_save_Click" />
            <asp:Button CssClass="greenbutton" Height="30px" ID="b_cancel" runat="server" Text="Отмена" 
            OnClick="b_cancel_Click" />
        </div>
    </div>
     </div>
</asp:Content>
