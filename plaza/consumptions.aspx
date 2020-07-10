<%@ Page Title="Управление затратами" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="consumptions.aspx.cs" Inherits="plaza.consumptions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function clear_all()
        {
           // alert('sdasd');
            document.getElementById('MainContent_tb_date_create_begin').value = '';
            document.getElementById('MainContent_tb_date_create_end').value = '';
            document.getElementById('MainContent_tb_date_change_begin').value = '';
            document.getElementById('MainContent_tb_date_change_end').value = '';
            document.getElementById('MainContent_tb_value_bottom').value = '';
            document.getElementById('MainContent_tb_value_top').value = '';
            document.getElementById('MainContent_tb_find_cats').readOnly = false;
            document.getElementById('MainContent_tb_find_cats').value = null;
            document.getElementById('MainContent_tb_find_cats').readOnly = true;
            document.getElementById('MainContent_tb_search').value = '';
            document.getElementById('MainContent_ddl_bils').selectedIndex = 0;
            document.getElementById('MainContent_ddl_user').selectedIndex = 0;
        }
    </script>
   <div class="maindiv">
    <div class= "onerow mymenu">
       <div class="mar10ver"><a href="consumptions.aspx">Управление затратами</a></div>
        <div class="mar10ver"><a href="plans.aspx">Планирование затрат</a></div>
       <div class="mar10ver"> <a href="cats.aspx">Категории затрат</a></div>
       <div class="mar10ver"> <a href="bils.aspx">Счета</a></div>
       <div class="mar10ver"> <a href="users.aspx">Пользователи</a></div>
       <div class="mar10ver"> <a href="reports.aspx">Отчеты</a></div>
   </div>
    <div class="onerow maincontent">
            <ContentTemplate>
                <asp:Panel ID="p_search" runat="server">
             <asp:Label ID="l_date_create" runat="server" Text="Дата создания затрат от "></asp:Label>
            <asp:TextBox ID="tb_date_create_begin" runat="server" TextMode="Date" Width="125px"></asp:TextBox><span> до </span>
            <asp:TextBox ID="tb_date_create_end" runat="server" TextMode="Date" Width="125px"></asp:TextBox>
           <asp:Label CssClass="mar40left" ID="l_date_change" runat="server" Text="Дата изменения затрат от"></asp:Label>
            <asp:TextBox ID="tb_date_change_begin" runat="server" TextMode="Date" Width="125px"></asp:TextBox>
            <span> до </span>
            <asp:TextBox ID="tb_date_change_end" runat="server" TextMode="Date" Width="134px"></asp:TextBox>
            <p class="mar5ver" />
            <asp:Label ID="l_value" runat="server" Text="Диапазон значений от"></asp:Label>
            <asp:TextBox ID="tb_value_bottom" width="70px" runat="server"></asp:TextBox><span> до </span>
            <asp:TextBox ID="tb_value_top" width="70px" runat="server"></asp:TextBox>
            <asp:Label CssClass="mar40left" ID="l_cats" runat="server" Text="Категория"></asp:Label>
                <asp:Button CssClass="bluebutton" ID="b_show_tree" runat="server" Text="Показать дерево" OnClick="b_show_tree_Click" />
            <asp:TextBox ID="tb_find_cats" runat="server" Width="370px" ReadOnly="True"></asp:TextBox>
            <p class="mar5ver" />
            <asp:Label ID="l_bils" runat="server" Text="Счета "></asp:Label>
            <asp:DropDownList ID="ddl_bils" Width="200px" runat="server">
                <asp:ListItem>Все счета</asp:ListItem>
            </asp:DropDownList>
            <asp:Label CssClass="mar40left" ID="L_search" runat="server" Text="Номер записи или комментарий"></asp:Label>
            <asp:TextBox ID="tb_search" runat="server" Width="437px"></asp:TextBox>
            
            <p class="mar5ver" />
            <asp:DropDownList ID="ddl_user" runat="server">
                <asp:ListItem>Все пользователи</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="l_user" runat="server" Text="Пользователь"></asp:Label>
            <asp:Button CssClass="greenbutton" Width="166px" ID="b_search" runat="server" Text="Найти" OnClick="b_search_Click" />
            <asp:Button CssClass="redbutton" ID="b_clear" runat="server" Text="Очистить поиск" OnClick="b_clear_Click"   />
        </asp:Panel>
            </ContentTemplate>
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
        <asp:GridView ID="gv1" Width="955px" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gv1_SelectedIndexChanged" OnSelectedIndexChanging="gv1_SelectedIndexChanging" Font-Names="Arial" Font-Size="Small">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chb_all" runat="server" AutoPostBack="True" OnCheckedChanged="chb_all_CheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chb_table" runat="server" AutoPostBack="True" OnCheckedChanged="chb_table_CheckedChanged" Width="20px" />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="id_con" HeaderText="№">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="15px" />
                </asp:BoundField>
                 <asp:BoundField DataField="data_create" DataFormatString = "{0:dd/MM/yyyy}" 
                     HeaderText="Дата внесения" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="60px"/>
                </asp:BoundField>
                <asp:BoundField DataField="data_change" DataFormatString = "{0:dd/MM/yyyy}" 
                    HeaderText="Дата изменения" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="60px"/>
                </asp:BoundField>
                <asp:BoundField DataField="value_con" HeaderText="Значение" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="60px"/>
                </asp:BoundField>
                <asp:BoundField DataField="name_cat" HeaderText="Категория" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="60px"/>
                </asp:BoundField>
                <asp:BoundField DataField="bil_con" HeaderText="Счет" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="155px"/>
                </asp:BoundField>
                <asp:BoundField DataField="descript_con" HeaderText="Комментарий" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="100px"/>
                </asp:BoundField>
                <asp:BoundField DataField="u_f" HeaderText="Создал сотрудник">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="50px" />
                </asp:BoundField>
                    <asp:BoundField DataField="u_f2" HeaderText="Изменил сотрудник" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="50px"/>
                </asp:BoundField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#26a9e0" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#26a9e0" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <AlternatingRowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#16dbdb" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle  BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#e4e9eb" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
                </ContentTemplate>
        </asp:UpdatePanel>
        <div class="mar10 onerow">
        <asp:Button  ID="b_add_con" runat="server" Text="Добавить" 
            OnClick="b_add_con_Click" CssClass="bluebutton" Height="30px" />
            </div>
       <div class="mar10ver onerow">
            <asp:Button ID="b_change" runat="server" Text="Изменить" OnClick="b_change_Click" CssClass="greenbutton" Height="30px" />
           </div>
        <div class="mar10hor onerow">
                    <asp:Button ID="b_delete" runat="server" Text="Удалить" OnClick="b_delete_Click" CssClass="redbutton" Height="30px"/>
            </div>
            <ajaxToolkit:ModalPopupExtender TargetControlID="b_inv" PopupControlID="p_modal_confirm" 
                ID="mpe" runat="server" DropShadow="True"></ajaxToolkit:ModalPopupExtender>
       
        <div class="invis"><asp:Button ID="b_inv" runat="server" Text="Невидимка" />
            <asp:Button ID="b_inv2" runat="server" Text="Невидимка" />

        </div>
        

    </div>
</div>
    <asp:Panel CssClass="modalwin" ID="p_modal_confirm" runat="server">
                Вы уверены, что желаете удалить записи о затратах в количестве <asp:Label ID="l_count_of_cons" 
                    runat="server" Text=""></asp:Label>
        <br /><br />
                <asp:Button ID="b_yes" runat="server" Text="Да" OnClick="b_yes_Click" />
                <asp:Button ID="b_no" runat="server" Text="Нет" />
            </asp:Panel>
       <asp:Panel CssClass="modalwin" ID="p_modal_cats" ScrollBars="Vertical" runat="server" Width="300">
        <asp:TreeView ID="tv" runat="server" OnSelectedNodeChanged="tv_SelectedNodeChanged" ForeColor="White"></asp:TreeView>
        <asp:Button ID="b_close_cats" runat="server" Text="Закрыть" />
    </asp:Panel>
     <ajaxToolkit:ModalPopupExtender TargetControlID="b_inv2" ID="mpe_cats" PopupControlID="p_modal_cats" CancelControlID="b_close_cats" 
        runat="server"></ajaxToolkit:ModalPopupExtender>
</asp:Content>

