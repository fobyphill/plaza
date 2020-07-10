using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace plaza
{
    public partial class reports : System.Web.UI.Page
    {
        string login_user, status_user; // переменные для данных пользователя
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Зададим параметры пользователя
            login_user = (string)Session["login_user"];
            status_user = (string)Session["status_user"];
            if (status_user != "a")
            { Response.Redirect("autorize.aspx"); }
            if (!Page.IsPostBack)
            {
                p_users.Visible = false; p_bils.Visible = false;
                tb_month.Text = DateTime.Now.ToString("yyyy-MM");//Выведем текущий месяц по умолчанию
                //Выведем все категории в дерево
                string q_cat = "select * from cats";
                OleDbConnection ole_con = new OleDbConnection(con_str);
                ole_con.Open();
                OleDbCommand com = new OleDbCommand(q_cat, ole_con);
                OleDbDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[3].ToString() == "0")
                    {
                        TreeNode node_cat = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        //string parent_id = dr[0].ToString();
                        edit_consumpt2 ec2 = new edit_consumpt2();
                        ec2.find_child(node_cat);
                        tv.Nodes.Add(node_cat);
                    }
                }
                dr.Close(); 
                //вкинем даты
                DateTime date_now = new DateTime();
                date_now = DateTime.Now;
                int mons = DateTime.Now.Month;
                mons--;
                int day = date_now.Day;
                int year_now = date_now.Year;
                DateTime date_before = new DateTime(year_now, mons, day);
                tb_date_to.Text = date_now.ToString("yyyy-MM-dd");
                tb_date_from.Text = date_before.ToString("yyyy-MM-dd");

                //Заполним юзеров
                string q_user = "select name_user, fam_user,login_user, status_user from users";
                com = new OleDbCommand(q_user, ole_con);
                dr = com.ExecuteReader();
                Dictionary<string, string> d_users = new Dictionary<string, string>();
                while(dr.Read())
                {
                    string user_str = dr[0].ToString() + " " + dr[1].ToString();
                    lb_users.Items.Add(new ListItem(user_str, dr[2].ToString()));
                    d_users.Add(dr[2].ToString(), dr[3].ToString());
                }
                Session["d_users"] = d_users;
            }
        }

        protected void b_print_Click(object sender, EventArgs e)
        {
            bool flag_error = false;
            string type_report, month = "", year = "";
            if (rbl_period_choise.SelectedIndex == 0)//если период выбран Месяц, то
            {
                bool try_parse_date = false;
                DateTime tempDate = new DateTime();
                try_parse_date = DateTime.TryParse(tb_month.Text, out tempDate);//передаем месяц и год
                if (try_parse_date)
                {
                    month = tempDate.Month.ToString();
                    year = tempDate.Year.ToString();
                    Session["method_date"] = "month";
                }
                else
                {
                    l_month_error.Visible = true;
                    flag_error = true;
                }
                
            }
            else//если период произвольный - то
            {
                DateTime temp_date;
                if (DateTime.TryParse(tb_date_from.Text, out temp_date))
                { month = temp_date.ToString("yyyy-MM-dd"); }
                if (DateTime.TryParse(tb_date_to.Text, out temp_date))
                {year = temp_date.ToString("yyyy-MM-dd");}
                Session["method_date"] = "period";
            }
            string checked_cats = "";//перечень выделенных категорий
            if (rbl_choice_report.SelectedIndex == 1 && ddl_main_choise.SelectedIndex == 0)
            {
                if (rbl_option.SelectedIndex == 0)
                { type_report = "only_cats"; }
                else { 
                        type_report = "with_include";
                        string list_cats = "";
                        foreach (TreeNode tn in tv.CheckedNodes)
                        {list_cats += find_check_and_child(tn);}
                        list_cats = list_cats.Substring(0, list_cats.Length - 1);
                        Session["list_cats"] = list_cats;
                    }
                foreach (TreeNode tn in tv.CheckedNodes)//Найдем выделенные категории
                {
                    checked_cats += tn.Value.ToString();
                    checked_cats += ",";
                }
                if (checked_cats == "")
                {
                    flag_error = true;
                    
                    l_cats.Text = "Не выбрана ни одна категория";
                    l_cats.CssClass = "stress";
                }
                else { checked_cats = checked_cats.Substring(0, checked_cats.Length - 1); }
            }
            else { type_report = "fast"; }
            if (ddl_main_choise.SelectedIndex == 1)//определим список выделенных категорий для отчета по счетам
            {
                checked_cats = "";
                foreach (ListItem li in lb_bils.Items)
                {
                    if (li.Selected)
                    { 
                        checked_cats += li.Text;
                        checked_cats += ",";
                    }
                }
                if (checked_cats.Length != 0)
                { checked_cats = checked_cats.Substring(0, checked_cats.Length - 1); }
            }
            if (ddl_main_choise.SelectedIndex == 2)//определим список категорий для отчета по пользователям
            {
                checked_cats = "";
                foreach (ListItem li in lb_users.Items)
                {
                    if (li.Selected)
                    {
                        checked_cats += li.Value;
                        checked_cats += ",";
                    }
                }
                if (checked_cats.Length != 0)
                { checked_cats = checked_cats.Substring(0, checked_cats.Length - 1); }
            }
            if (!flag_error)
            {
                int count = 0;
                foreach (ListItem cb in cbl_con_plan.Items)
                {
                    if (cb.Selected == true)
                    { count++; }
                }
                switch (ddl_main_choise.SelectedIndex)
                {
                    case 0:
                        if (count == 1 && cbl_con_plan.SelectedIndex == 0)//если одна галка и она затраты
                        {
                            Response.Write("<script>window.open ('rept.aspx?type=" + type_report +
                            "&month=" + month + "&year=" + year + "&checked_cats=" + checked_cats + "','_blank');</script>");
                        }
                        else if (count == 1 && cbl_con_plan.SelectedIndex == 1)//если одна галка и она планирование
                        {
                            Response.Write("<script>window.open ('rept_plan.aspx?type=" + type_report +
                            "&month=" + month + "&year=" + year + "&checked_cats=" + checked_cats + "','_blank');</script>");
                        }
                        else if (count == 2)//если обе галки - показываем комплексный отчет
                        {
                            Response.Write("<script>window.open ('rept_complex.aspx?type=" + type_report +
                             "&month=" + month + "&year=" + year + "&checked_cats=" + checked_cats + "','_blank');</script>");
                        }
                        else
                        {
                            l_con_plan.Text = "Не выбран тип отчета - по затратам, планам или комплексный";
                            l_con_plan.CssClass = "stress";
                        }
                        break;
                    case 1:
                        Response.Write("<script>window.open ('rept_bils.aspx?type=" + type_report +
                            "&month=" + month + "&year=" + year + "&checked_cats=" + checked_cats + "','_blank');</script>");
                        Session["user_rept"] = "0";
                        break;
                    case 2:
                        Response.Write("<script>window.open ('rept_bils.aspx?type=" + type_report +
                            "&month=" + month + "&year=" + year + "&checked_cats=" + checked_cats + "','_blank');</script>");
                        Session["user_rept"] = "1";
                        break;
                }
                
            }
                
        }

        protected void rbl_choice_report_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbl_choice_report.SelectedIndex == 0)
            { 
                p_custom_report.Visible = false;
                rbl_period_choise.SelectedIndex = 0;
                p_free_period.Visible = false;
                p_fast_report.Visible = true;
            }
            else 
            {
                p_custom_report.Visible = true;
            }
        }
        protected void ib_show_hide_Click(object sender, ImageClickEventArgs e)
        {
            if (l_collapse.Text == "Развернуть все")
            {
                tv.ExpandAll();
                l_collapse.Text = "Свернуть все";
                ib_show_hide.CssClass = "checkbox_checked";
            }
            else
            {
                tv.CollapseAll();
                l_collapse.Text = "Развернуть все";
                ib_show_hide.CssClass = "checkbox_uncheck";
            }
        }
        protected void ib_check_Click(object sender, ImageClickEventArgs e)
        {
            if (l_check.Text == "Отметить все")
            {
                l_check.Text = "Снять все отметки";
                ib_check.CssClass = "checkbox_checked";
                foreach (TreeNode tn in tv.Nodes)
                {
                    tn.Checked = true;
                    check_tree(tn, true);
                }
                
            }
            else
            {
                l_check.Text = "Отметить все";
                ib_check.CssClass = "checkbox_uncheck";
                foreach (TreeNode tn in tv.Nodes)
                {
                    tn.Checked = false;
                    check_tree(tn, false);
                }
            }

        }
        void check_tree(TreeNode n, bool v)
        {
            if (n.ChildNodes.Count > 0)
            {
                foreach (TreeNode tn in n.ChildNodes)
                {
                    tn.Checked = v;
                    check_tree(tn, v);
                }
            }
        }
        string find_check_and_child(TreeNode n)
        {
            string s = n.Value.ToString()+",";
            if (n.ChildNodes.Count > 0)
            {
                foreach (TreeNode tn in n.ChildNodes)
                {
                    s += find_check_and_child(tn);
                }
            }
            return s;
        }
        protected void rbl_period_choise_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbl_period_choise.SelectedIndex == 0)
            {
                p_fast_report.Visible = true;
                p_free_period.Visible = false;
            }
            else
            {
                p_free_period.Visible = true;
                p_fast_report.Visible = false;
            }
        }
        protected void cbl_con_plan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbl_con_plan.Items[1].Selected)
            {
                rbl_period_choise.SelectedIndex = 0;
                var li = rbl_period_choise.Items.FindByValue("1");
                li.Enabled = false;
                p_fast_report.Visible = true;
                p_free_period.Visible = false;
            }
            else
            {
                rbl_period_choise.Items.FindByValue("1").Enabled = true;
            }
        }
        protected void ddl_main_choise_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ddl_main_choise.SelectedIndex)
            {
                case 0:
                    p_consumptions_plans.Visible = true;
                    p_bils.Visible = false;
                    p_users.Visible = false;
                    break;
                case 1:
                    p_consumptions_plans.Visible = false;
                    p_bils.Visible = true;
                    p_users.Visible = false;
                    break;
                case 2:
                    p_consumptions_plans.Visible = false;
                    p_bils.Visible = false;
                    p_users.Visible = true;
                    Dictionary<string, string> d_users = new Dictionary<string, string>();
                    d_users = (Dictionary<string, string>)Session["d_users"];
                    for (int i = 0; i < lb_users.Items.Count; i++ )
                    {
                        if (d_users[lb_users.Items[i].Value] == "a")
                        { lb_users.Items[i].Attributes.Add("style", "background-color:#16dbdb"); }
                    }
                    break;
                    
            }
        }
        protected void ib_bils_Click(object sender, ImageClickEventArgs e)
        {
            if (l_select_all_bils.Text == "Выделить все")
            {
                foreach (ListItem li in lb_bils.Items)
                {
                    li.Selected = true;
                }
                ib_bils.CssClass = "checkbox_checked";
                l_select_all_bils.Text = "Снять отметки";
            }
            else
            {
                foreach (ListItem li in lb_bils.Items)
                {
                    li.Selected = false;
                }
                ib_bils.CssClass = "checkbox_uncheck";
                l_select_all_bils.Text = "Выделить все";
            }
        }

        protected void ib_select_all_users_Click(object sender, ImageClickEventArgs e)
        {
            if (l_select_all_users.Text == "Выделить всех")
            {
                foreach (ListItem li in lb_users.Items)
                {
                    li.Selected = true;
                }
                ib_users.CssClass = "checkbox_checked";
                l_select_all_users.Text = "Снять отметки";
            }
            else
            {
                Dictionary<string, string> d_users = (Dictionary<string, string>)Session["d_users"];
                int i = 0;
                foreach (ListItem li in lb_users.Items)
                {
                    li.Selected = false;
                    if (d_users[lb_users.Items[i].Value] == "a")
                    { lb_users.Items[i].Attributes.Add("style", "background-color:#16dbdb"); }
                    i++;
                }
                ib_users.CssClass = "checkbox_uncheck";
                l_select_all_users.Text = "Выделить всех";
            }
        }

        protected void lb_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = lb_users.SelectedIndex;
            Dictionary<string, string> d_users = (Dictionary<string, string>)Session["d_users"];
            for (int i = 0; i < lb_users.Items.Count; i++)
            {
                if (d_users[lb_users.Items[i].Value] == "a")
                { lb_users.Items[i].Attributes.Add("style", "background-color:#16dbdb"); }
            }
            lb_users.SelectedIndex = sel;

        }
    }
}