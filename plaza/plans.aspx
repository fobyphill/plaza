<%@ Page Title="Планирование затрат" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="plans.aspx.cs" 
    Inherits="plaza.plans" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
        <asp:Panel ID="p_search" runat="server">
             <asp:Label ID="l_date_create" runat="server" Text="Месяц планирования затрат от "></asp:Label>
            <asp:TextBox ID="tb_date_create_begin" runat="server" TextMode="Month" Width="142px"></asp:TextBox><span> до </span>
            <asp:TextBox ID="tb_date_create_end" runat="server" TextMode="Month" Width="142px"></asp:TextBox>
            <asp:Label ID="l_month_error" runat="server" ForeColor="Red" Visible="false"
                Text="Дата введена с ошибкой. Вероятно Вы пользуетесь браузером FireFox или InternetExplorer. Укажите дату в формате ГГГГ-ММ"></asp:Label>
            <p class="mar5ver" />
            <asp:Label ID="l_value" runat="server" Text="Диапазон значений от"></asp:Label>
            <asp:TextBox ID="tb_value_bottom" width="70px" runat="server"></asp:TextBox><span> до </span>
            <asp:TextBox ID="tb_value_top" width="70px" runat="server"></asp:TextBox>
            <asp:Label CssClass="mar40left" ID="l_cats" runat="server" Text="Категория"></asp:Label>
            <asp:Button CssClass="bluebutton" ID="b_show_tree" runat="server" Text="Показать дерево" OnClick="b_show_tree_Click" />
            <asp:Label ID="l_find_cats" Width="350px" runat="server" Text=""></asp:Label>
            <p class="mar5ver" />
            <asp:Label ID="l_bils" runat="server" Text="Счета "></asp:Label>
            <asp:DropDownList ID="ddl_bils" Width="200px" runat="server">
                <asp:ListItem>Все счета</asp:ListItem>
            </asp:DropDownList>
            <asp:Label CssClass="mar40left" ID="L_search" runat="server" Text="Номер записи или комментарий"></asp:Label>
            <asp:TextBox ID="tb_search" runat="server" Width="440px"></asp:TextBox>
            <p class="mar5ver" />
            <asp:DropDownList ID="ddl_user" runat="server">
                <asp:ListItem>Все пользователи</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="l_user" runat="server" Text="Пользователь"></asp:Label>
            <asp:Button CssClass="greenbutton" Width="166px" ID="b_search" runat="server" Text="Найти" OnClick="b_search_Click"  />
            <asp:Button CssClass="redbutton" ID="b_clear" runat="server" Text="Очистить поиск" OnClick="b_clear_Click" />
        </asp:Panel>
        <asp:ImageButton ID="ib_show_hide_search" CssClass="checkbox_checked" ImageUrl="img/double_checkbox.png" 
                    runat="server" OnClick="ib_show_hide_search_Click" />
        <asp:Label ID="l_collapse" runat="server" Text="Скрыть поиск"></asp:Label>
        <asp:Panel Visible="false"  CssClass="diverror" ID="p_error" runat="server">
            <asp:Label ForeColor="White" ID="l_hint_no_1" runat="server" 
            Text="Ни одна запись не была выбрана" Visible="true"></asp:Label>
        </asp:Panel>
        <div class="divhint">
            
            <asp:Label ID="l_click_left" runat="server" 
            Text="Показаны последние 10 записей"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

            
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gv_SelectedIndexChanged" Font-Names="Arial" Font-Size="Small" Width="955px" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chb_header" runat="server" AutoPostBack="True" OnCheckedChanged="chb_header_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chb_table" runat="server" AutoPostBack="True" OnCheckedChanged="chb_table_CheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                </asp:TemplateField>
                <asp:BoundField DataField="id_plan" HeaderText="№" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="35px" />
                </asp:BoundField>
                 <asp:BoundField DataField="data_plan" DataFormatString = "{0:Y}" 
                     HeaderText="Месяц" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px"/>
                </asp:BoundField>
                <asp:BoundField DataField="value_plan" HeaderText="Значение" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="60px"/>
                </asp:BoundField>
                <asp:BoundField DataField="name_cat" HeaderText="Категория" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="120px"/>
                </asp:BoundField>
                <asp:BoundField DataField="bil_plan" HeaderText="Счет" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="130px"/>
                </asp:BoundField>
                <asp:BoundField DataField="descript_plan" HeaderText="Комментарий" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="265px"/>
                </asp:BoundField>
                <asp:BoundField DataField="fam_user" HeaderText="Изменил сотрудник" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="50px"/>
                 </asp:BoundField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#26a9e0" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#26a9e0" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#16dbdb" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#e4e9eb" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
                </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mar10 onerow">
        <asp:Button CssClass="bluebutton"  ID="b_add_con" runat="server" Height="30px" Text="Добавить" 
            OnClick="b_add_con_Click" />
            </div>
       <div class="onerow">
            <asp:Button CssClass="greenbutton" ID="b_change" runat="server" Height="30px" Text="Изменить" OnClick="b_change_Click" />
           </div>
        <div class="mar10hor onerow">
                    <asp:Button CssClass="redbutton" ID="b_delete" runat="server" Height="30px" Text="Удалить" OnClick="b_delete_Click"/>
            
            </div>
            <ajaxToolkit:ModalPopupExtender TargetControlID="b_inv" PopupControlID="p_modal_confirm" 
                ID="mpe" runat="server" DropShadow="True"></ajaxToolkit:ModalPopupExtender>
       
        <div class="invis"><asp:Button ID="b_inv" runat="server" Text="Невидимка" />
            <asp:Button ID="b_inv2" runat="server" Text="Невидимка" />
        </div>
        

</div>
    <asp:Panel CssClass="modalwin" ID="p_modal_confirm" runat="server">
                Вы уверены, что желаете удалить записи о планировании в количестве
                <asp:Label ID="l_count_of_plans" runat="server" Text="Label"></asp:Label>
                &nbsp;шт?<br /><br />
                <asp:Button ID="b_yes" runat="server" Text="Да" OnClick="b_yes_Click" />
                <asp:Button ID="b_no" runat="server" Text="Нет" OnClick="b_no_Click" />
            </asp:Panel>
        </div>
     <asp:Panel CssClass="modalwin" ID="p_modal_cats" ScrollBars="Vertical" runat="server" Width="300">
        <asp:TreeView ID="tv" runat="server" OnSelectedNodeChanged="tv_SelectedNodeChanged" ForeColor="White"></asp:TreeView>
        <asp:Button ID="b_close_cats" runat="server" Text="Закрыть" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender TargetControlID="b_inv2" ID="mpe_cats" PopupControlID="p_modal_cats" CancelControlID="b_close_cats" 
        runat="server"></ajaxToolkit:ModalPopupExtender>
</asp:Content>
