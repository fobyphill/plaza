<%@ Page Title="Счета" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="bils.aspx.cs" Inherits="plaza.bils" %>
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
        
        <asp:Panel ID="p_error" runat="server" Visible="false" CssClass="diverror">
            <asp:Label ID="l_error" runat="server" Text="Счет не выбран" ForeColor="White"></asp:Label>
        </asp:Panel>
        <!--sdsd-->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

            
            <div class="mar10 onerow">
            <asp:ListBox ID="lb_bils" CssClass="border1" runat="server" Height="180px" style="margin-top: 0px" Width="192px" AutoPostBack="True" OnSelectedIndexChanged="lb_bils_SelectedIndexChanged"></asp:ListBox>
            <asp:Label ID="l_bils" runat="server" Text="счета"></asp:Label>
            </div>
            <div class="onerow">
                <div class="mar10">
            <asp:TextBox ID="tb_name" CssClass="border1" runat="server"></asp:TextBox>
            <asp:Label ID="l_name" runat="server" Text="Название счета"></asp:Label>
                </div>
                <div class="mar10">
            <asp:TextBox ID="tb_num" CssClass="border1" runat="server"></asp:TextBox>
            <asp:Label ID="l_num" runat="server" Text="Номер счета"></asp:Label>
                </div>
                <div class="mar10">
            <asp:TextBox ID="tb_descript" CssClass="border1" runat="server" Height="110px" 
                TextMode="MultiLine" Width="164px"></asp:TextBox>
            <asp:Label ID="l_desript" runat="server" Text="Описание счета"></asp:Label>
               </div>
            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        <div class="onerow mar10">
        <asp:Button CssClass="bluebutton"  ID="b_add" runat="server" Text="Добавить" 
            OnClick="b_add_Click" />
            </div>
       <div class="onerow mar10">
            <asp:Button CssClass="greenbutton" ID="b_change" runat="server" Text="Изменить" OnClick="b_change_Click" />
           </div>
        <div class="onerow mar10">
                    <asp:Button CssClass="redbutton" ID="b_delete" runat="server" Text="Удалить" OnClick="b_delete_Click"/>
            
            </div>
            <div class="onerow mar10">
                    <asp:Button CssClass="redbutton" ID="b_clear" runat="server" Text="Очистить" OnClick="b_clear_Click" />
            
            </div>
            </div>
            <ajaxtoolkit:modalpopupextender TargetControlID="b_inv" PopupControlID="p_modal_confirm" 
                ID="mpe" runat="server" DropShadow="True"></ajaxtoolkit:modalpopupextender>
       
        <div class="invis"><asp:Button ID="b_inv" runat="server" Text="Невидимка" /></div>
        

</div>
    <asp:Panel CssClass="modalwin" ID="p_modal_confirm" runat="server">
                Вы уверены, что желаете удалить счет?<br /><br />
                <asp:Button ID="b_yes" runat="server" Text="Да" OnClick="b_yes_Click" />
                <asp:Button ID="b_no" runat="server" Text="Нет" OnClick="b_no_Click" />
            </asp:Panel>
</div>
</asp:Content>
