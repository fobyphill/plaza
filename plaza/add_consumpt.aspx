<%@ Page Title="Добавить запись о затратах" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="add_consumpt.aspx.cs" Inherits="plaza.add_consumpt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="maindiv">
<asp:Panel CssClass="onerow mymenu" ID="p_vis" runat="server">
       <div class="mar10ver"><a href="consumptions.aspx">Управление затратами</a></div>
        <div class="mar10ver"><a href="plans.aspx">Планирование затрат</a></div>
       <div class="mar10ver"> <a href="cats.aspx">Категории затрат</a></div>
       <div class="mar10ver"> <a href="bils.aspx">Счета</a></div>
       <div class="mar10ver"> <a href="users.aspx">Пользователи</a></div>
       <div class="mar10ver"> <a href="reports.aspx">Отчеты</a></div>
</asp:Panel>
    <asp:Panel CssClass="onwrow maincontent" ID="p_main" runat="server">
       <div class="mar10ver">
        <div class="onerow "><div class="border1">
            <asp:Panel BackColor="White" ID="p_tv" runat="server" ScrollBars="Vertical">
            <asp:TreeView Width="275px" Height =" 275px" ID="tv" runat="server">
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
        <asp:ImageButton CssClass="checkbox_checked" ID="ib_show_hide" ImageUrl="img/double_checkbox_blue.png" Width="20px" runat="server" 
            OnClick="ib_show_hide_Click" />
                <asp:Label ID="l_collapse" runat="server" Text="Свернуть все"></asp:Label>
    </div>
       <div class="mar10ver">
           <asp:TextBox CssClass="border1" Width="292px" ID="tb_data" runat="server" TextMode="Date"></asp:TextBox>
           <asp:Label ID="l_data" runat="server" Text="Дата"></asp:Label>
       </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_value" CssClass="border1" runat="server" Width="292px"></asp:TextBox>
            <asp:Label ID="l_value" runat="server" Text="Cумма"></asp:Label>
        </div>
       <div class="mar10ver">
           <asp:DropDownList class="border1" ID="ddl_bils" runat="server" Width="294px">
           </asp:DropDownList>
           <asp:Label ID="l_bil" runat="server" Text="Cчет"></asp:Label>
       </div>
        <div class="mar10ver">
            <asp:TextBox ID="tb_descript" CssClass="border1" TextMode="MultiLine" runat="server" Height="55px" 
                Width="292px"></asp:TextBox> 
            <asp:Label ID="l_descript" runat="server" Text="Комментарий"></asp:Label>
        </div>
        <div class="mar10ver">
            <asp:Label ID="l_load_xml" runat="server" Text="Или загрузите XML-файл с записями о затратах для пакетной загрузки данных"></asp:Label><br />
            <asp:FileUpload CssClass="border1" ID="fu" runat="server" Height="21px" style="margin-bottom: 2px" Width="296px" />
            <asp:Button ID="b_upload_file" runat="server" Text="Загрузить" CssClass="mar10hor bluebutton" OnClick="b_upload_file_Click" />
            <asp:Label ID="l_status_file" runat="server" Text=""></asp:Label>
        </div>
       <div class="mar10ver">
         <asp:Button ID="b_save" CssClass="bluebutton" Height="30px" runat="server" Text="Сохранить" OnClick="b_save_Click" />
            <asp:Button ID="b_cancel" CssClass="greenbutton mar10hor" Height="30px" runat="server" Text="Отмена" 
            OnClick="b_cancel_Click" />
        </div>
    </asp:Panel>
</div>
</asp:Content>
