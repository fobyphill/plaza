<%@ Page Title="Категории затрат" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cats.aspx.cs" 
    Inherits="plaza.cats" %>
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
    <div class="onerow maincontent">
        <div class="mar10ver">
            <div class="onerow border1">
                <asp:Panel BackColor="White" ID="p_tv" runat="server" ScrollBars="Vertical">
                    <asp:TreeView ID="tv" Width="276px" Height="276px" runat="server" 
                        OnSelectedNodeChanged="tv_SelectedNodeChanged">
                        <NodeStyle BorderColor="Black" ForeColor="Black" />
                        <SelectedNodeStyle BackColor="#16dbdb" />
                    </asp:TreeView>
                    </asp:Panel>
            </div>
            <div class="onerow">
                    <asp:Label ID="l_cats"  runat="server" Text="Категория"></asp:Label>
            </div>
        </div>
        <asp:Panel ID="p_add_edit" runat="server">
            <div class="onerow"> 
                
                <asp:ImageButton CssClass="checkbox_checked" ID="ib_show_hide" ImageUrl="img/double_checkbox_blue.png" Width="20px" runat="server" OnClick="ib_show_hide_Click" />
                <asp:Label ID="l_collapse" runat="server" Text="Свернуть все"></asp:Label>
                    
                <div class="mar10ver">
                <asp:TextBox CssClass="border1" ID="tb_parent_cat" Width="292" runat="server"></asp:TextBox>
                <asp:Label ID="l_parent_cat" runat="server" Text="Родительская категория"></asp:Label>
                    <asp:Button CssClass="bluebutton mar5hor" ID="b_win_cats" runat="server" Text="Открыть дерево" />
                </div>
                 <div class="mar10ver">
                 <asp:TextBox CssClass="border1" ID="tb_cat" Width="292" runat="server"></asp:TextBox>
                <asp:Label ID="l_cat" runat="server"  Text="Категория затрат"></asp:Label>
                </div>
                 <div class="mar10ver">
                <asp:TextBox CssClass="border1" ID="tb_descript" Width="290" runat="server" Height="72px"
                     TextMode="MultiLine"></asp:TextBox>
                <asp:Label ID="l_descript" runat="server" Text="Описание"></asp:Label>
                     </div>
            </div>
        </asp:Panel>
       
        <div class="onerow">
        <asp:Button CssClass="bluebutton" Height="30px" ID="b_add_cat" runat="server" Text="Добавить" 
            OnClick="b_add_cat_Click" />
            </div>
       <div class="mar10hor onerow">
            <asp:Button CssClass="greenbutton" Height="30" ID="b_change" runat="server" Text="Изменить" OnClick="b_change_Click" />
           </div>
        <div class="onerow">
                    <asp:Button CssClass="redbutton" Height="30px" ID="b_delete" runat="server" Text="Удалить" OnClick="b_delete_Click"/>
       </div>
        <ajaxToolkit:ModalPopupExtender TargetControlID="b_inv" PopupControlID="p_modal_confirm" 
                ID="mpe" runat="server" DropShadow="True"></ajaxToolkit:ModalPopupExtender>
        <div class="invis"><asp:Button ID="b_inv" runat="server" Text="Невидимка" /></div>
        

</div>
    <asp:Panel CssClass="modalwin" ID="p_modal_confirm" runat="server">
                Вы уверены, что желаете удалить категорию?<br /><br />
                <asp:Button ID="b_yes" runat="server" Text="Да" OnClick="b_yes_Click" />
                <asp:Button ID="b_no" runat="server" Text="Нет" />
            </asp:Panel>
        
    <asp:Panel ScrollBars="Vertical" Width="300" ID="p_modal_cats" CssClass="modalwin" runat="server">
        <asp:TreeView NodeStyle-ForeColor="White"  ID="tv_parent" runat="server" OnSelectedNodeChanged="tv_parent_SelectedNodeChanged"></asp:TreeView>
        <asp:Button ID="b_close_cats" runat="server" Text="Закрыть" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="mde_parent" TargetControlID ="b_win_cats" PopupControlID="p_modal_cats" CancelControlID="b_close_cats" runat="server"></ajaxToolkit:ModalPopupExtender>
        </div>
</asp:Content>
