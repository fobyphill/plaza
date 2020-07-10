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
    public partial class consumptions : System.Web.UI.Page
    {
        string login_user, status_user; // переменные для данных пользователя
        //строка подключения
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";


        protected void Page_Load(object sender, EventArgs e)
        {
            //Зададим параметры пользователя
            login_user = (string)Session["login_user"];
            status_user = (string)Session["status_user"];
            if (status_user != "a")
            { Response.Redirect("autorize.aspx"); }

            if (!Page.IsPostBack)
            {
                string q_table = "select top 10 * from cons_output order by data_create desc";
                string q_bils = "select name_bil from bils";
                string q_users = "select login_user, status_user from users";
                //СОздаем объект Оле - соединение с БД
                OleDbConnection ole_con = new OleDbConnection(con_str);
                ole_con.Open();
                //Выполняем запрос. Результат - массив в формате "Команда"
                OleDbCommand com = new OleDbCommand(q_table, ole_con);
                OleDbDataReader dr = com.ExecuteReader();
                //заполняем таблицу данными
                gv1.DataSource = dr;
                gv1.DataBind();
                //Заполним-ка комбобоксы "счета" и "пользователи" данными
                com = new OleDbCommand(q_bils, ole_con);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    ddl_bils.Items.Add(dr[0].ToString());
                }
                com = new OleDbCommand(q_users, ole_con);
                dr = com.ExecuteReader();
                int i = 1;
                while (dr.Read())
                {
                    ddl_user.Items.Add(dr[0].ToString());

                    if (dr[1].ToString() == "a")
                    {
                        ddl_user.Items[i].Attributes.Add("style", "background-color:Yellow");
                    }
                    i++;
                }
                ole_con.Close();
                //Загрузим дерево категорий
                string q_cat = "select * from cats";
                OleDbConnection sql_con = new OleDbConnection(con_str);
                ole_con.Open();
                com = new OleDbCommand(q_cat, ole_con);
                dr = com.ExecuteReader();
                //Заполним поля категорий
                while (dr.Read())
                {
                    if (dr[3].ToString() == "0")
                    {
                        TreeNode node_cat = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        find_child(node_cat);
                        tv.Nodes.Add(node_cat);
                    }
                }
                dr.Close();
            }

        }

        protected void b_add_con_Click(object sender, EventArgs e)
        {
            Response.Redirect("add_consumpt.aspx");
        }

        protected void b_change_Click(object sender, EventArgs e)
        {
            //считаем количество записей и скидываем в строку их айдишки
            int count_rec = 0;
            string id_con = "";
            foreach (GridViewRow gvr in gv1.Rows)
            {
                CheckBox chb = (CheckBox)gvr.FindControl("chb_table");
                if (chb.Checked)
                {
                    count_rec++;
                    id_con += gvr.Cells[1].Text + ",";
                }
            }
            

            if (count_rec > 0)
            {
                id_con = id_con.Substring(0, id_con.Length - 1);
                //id_con = gv1.Rows[gv1.SelectedIndex].Cells[1].Text; строка из прошлого. Забирает айди из выделенной строки
                Response.Redirect("edit_consumpt.aspx?id_con=" + id_con + "&count_rec="+count_rec);
            }
            else
            {
                p_error.Visible = true;
            }

        }

        protected void b_delete_Click(object sender, EventArgs e)
        {
            int count_rec = 0;
            string id_con = "";
            foreach (GridViewRow gvr in gv1.Rows)
            {
                CheckBox chb = (CheckBox)gvr.FindControl("chb_table");
                if (chb.Checked)
                {
                    count_rec++;
                    id_con += gvr.Cells[1].Text + ",";
                }
            }
            


            if (count_rec >0)
            {
                id_con = id_con.Substring(0, id_con.Length - 1);
                l_count_of_cons.Text = count_rec.ToString();
                Session["id_con"] = id_con;
                mpe.Show(); 

            }
            else
            {
                p_error.Visible = true;
            }
        }

        protected void b_yes_Click(object sender, EventArgs e)
        {
           // string id_con = gv1.Rows[gv1.SelectedIndex].Cells[1].Text;//Получил ID выбранного расхода
            string id_con = Session["id_con"].ToString();
            string q_con = "delete from consumptions where id_con in (" + id_con+")";
            //Создал запрос с нужным расходом.

            //Соединяюсь с БД
            //string con_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q_con, ole_con);
            com.ExecuteNonQuery();
            com.Dispose();
            ole_con.Close();
            System.Threading.Thread.Sleep(500);
            Response.Redirect("consumptions.aspx");
        }

        protected void b_search_Click(object sender, EventArgs e)
        {
            string q_find_cons = "select * from cons_output where"; //основной запрос
            bool flag = false;
            //Начало блока всех условий
            //блок даты затрат
            if (tb_date_create_begin.Text == "" && tb_date_create_end.Text != "")
            {
                flag = true;
                DateTime dt = new DateTime();
                dt = DateTime.Parse(tb_date_create_end.Text);
                q_find_cons += " data_create <= '" + dt.ToString("yyyy-MM-dd") + "'";
            }
            if (tb_date_create_begin.Text != "" && tb_date_create_end.Text == "")
            {
                flag = true;
                DateTime dtb = new DateTime();
                dtb = DateTime.Parse(tb_date_create_begin.Text);
                q_find_cons += " data_create >= '" + dtb.ToString("yyyy-MM-dd") + "'";
            }
            if (tb_date_create_begin.Text != "" && tb_date_create_end.Text != "")
            {
                flag = true;
                DateTime dtb = new DateTime();
                dtb = DateTime.Parse(tb_date_create_begin.Text);
                DateTime dte = new DateTime();
                dte = DateTime.Parse(tb_date_create_end.Text);
                if (dtb > dte)//меняем местами даты, если пользователь перепутал кра
                {
                    DateTime temp_date = new DateTime();
                    temp_date = dte;
                    dte = dtb;
                    dtb = temp_date;
                    string temp_data = tb_date_create_begin.Text;
                    tb_date_create_begin.Text = tb_date_create_end.Text;
                    tb_date_create_end.Text = temp_data;
                }
                q_find_cons += " data_create between '" + dtb.ToString("yyyy-MM-dd") + "' and '" + dte.ToString("yyyy-MM-dd") + "'";
            }
            //блок даты изменения затрат
            if (tb_date_change_begin.Text == "" && tb_date_change_end.Text != "")
            {
                DateTime dt = new DateTime();
                dt = DateTime.Parse(tb_date_change_end.Text);
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " data_change <= '" + dt.ToString("yyyy-MM-dd") + "'";
                flag = true;
            }
            if (tb_date_change_begin.Text != "" && tb_date_change_end.Text == "")
            {
                DateTime dtb = new DateTime();
                dtb = DateTime.Parse(tb_date_change_begin.Text);
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " data_change >= '" + dtb.ToString("yyyy-MM-dd") + "'";
                flag = true;
            }
            if (tb_date_change_begin.Text != "" && tb_date_change_end.Text != "")
            {
                DateTime dtb = new DateTime();
                dtb = DateTime.Parse(tb_date_change_begin.Text);
                DateTime dte = new DateTime();
                dte = DateTime.Parse(tb_date_change_end.Text);
                if (dtb > dte)//меняем местами даты, если пользователь перепутал края
                {
                    DateTime temp_date = new DateTime();
                    temp_date = dte;
                    dte = dtb;
                    dtb = temp_date;
                    string temp_data = tb_date_change_begin.Text;
                    tb_date_change_begin.Text = tb_date_change_end.Text;
                    tb_date_change_end.Text = temp_data;
                }
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " data_change between '" + dtb.ToString("yyyy-MM-dd") + "' and '" + dte.ToString("yyyy-MM-dd") + "'";
                flag = true;

            }
            //блок значений
            if (tb_value_bottom.Text == "" && tb_value_top.Text != "")
            {
                if (!try_parse_val(tb_value_top.Text))
                {
                    l_hint_no_1.Text = "Указано некорректное значение верхнего порога затрат. Введите число.";
                    l_hint_no_1.Visible = true;
                }
                else
                {
                    if (flag) { q_find_cons += " and"; }
                    q_find_cons += " value_con <= " + tb_value_top.Text;
                    flag = true;
                }
            }
            if (tb_value_bottom.Text != "" && tb_value_top.Text == "")
            {
                if (!try_parse_val(tb_value_bottom.Text))
                {
                    l_hint_no_1.Text = "Указано некорректное значение нижнего порога затрат. Введите число.";
                    l_hint_no_1.Visible = true;
                }
                else
                {
                    if (flag) { q_find_cons += " and"; }
                    q_find_cons += " value_con >= " + tb_value_bottom.Text;
                    flag = true;
                }
            }
            if (tb_value_bottom.Text != "" && tb_value_top.Text != "")
            {
                if (!try_parse_val(tb_value_bottom.Text) || !try_parse_val(tb_value_top.Text))
                {
                    l_hint_no_1.Text = "Указано некорректное значение затрат. Введите число.";
                    l_hint_no_1.Visible = true;
                }
                else
                {
                    if (flag) { q_find_cons += " and"; }
                    float value_top = float.Parse(tb_value_top.Text);
                    float value_bottom = float.Parse(tb_value_bottom.Text);
                    if (value_top < value_bottom)
                    {
                        string temp_str = tb_value_top.Text;
                        tb_value_top.Text = tb_value_bottom.Text;
                        tb_value_bottom.Text = temp_str;
                    }
                    q_find_cons += " value_con between " + tb_value_bottom.Text + " and " + tb_value_top.Text;
                    flag = true;
                }
            }
            //блок категорий
            if (tb_find_cats.Text != "")
            {
                
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " name_cat in (" + tb_find_cats.Text + ")";
                flag = true;
            }
            //блок счетов
            if (ddl_bils.SelectedIndex > 0)
            {
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " bil_con = '" + ddl_bils.SelectedItem.Text + "'";
                flag = true;
            }
            //блок пользователей
            if (ddl_user.SelectedIndex > 0)
            {
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " (cr_login = '" + ddl_user.SelectedItem.Text + "' or ch_login = '" + ddl_user.SelectedItem.Text + "')";
                flag = true;
            }
            //блок айди + комменты
            if (try_parse_val(tb_search.Text))
            {
                int id = Int32.Parse(tb_search.Text);
                q_find_cons = "select * from cons_output where id_con = " + id;
                flag = true;
            }
            else if (tb_search.Text.Length > 0)
            {
                if (flag) { q_find_cons += " and"; }
                q_find_cons += " descript_con like '%" + tb_search.Text + "%'";
                flag = true;
            }
            //Конец блока всех условий
            //вывод таблицы
            string q_table = "select top 10 * from cons_output ";//order by data_create desc";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand();
            if (flag)
            {
                com = new OleDbCommand(q_find_cons, ole_con);
                l_click_left.Text = "Показаны результаты поиска";
            }
            else
            {
                com = new OleDbCommand(q_table, ole_con);
                l_click_left.Text = "Показаны последние 10 записей";
            }

            OleDbDataReader dr = com.ExecuteReader();
            gv1.DataSource = null;
            gv1.DataBind();
            gv1.DataSource = dr;
            gv1.DataBind();
            ole_con.Close();
        }

        bool try_parse_val(string value)
        {
            float val_digit;
            value = value.Replace('.', ',');
            bool res = Single.TryParse(value, out val_digit);
            return res;
        }

        protected void b_clear_Click(object sender, EventArgs e)
        {
            tb_date_create_begin.Text = "";
            tb_date_create_end.Text = "";
            tb_date_change_begin.Text = "";
            tb_date_change_end.Text = "";
            tb_value_bottom.Text = "";
            tb_value_top.Text = "";
            tb_find_cats.Text = "";
            ddl_bils.SelectedIndex = 0;
            ddl_user.SelectedIndex = 0;
            tb_search.Text = "";
        }

        protected void ib_show_hide_search_Click(object sender, ImageClickEventArgs e)
        {
            if (p_search.Visible == true)
            {
                p_search.Visible = false;
                l_collapse.Text = "Показать поиск";
                ib_show_hide_search.CssClass = "checkbox_uncheck";
            }
            else
            {
                p_search.Visible = true;
                l_collapse.Text = "Скрыть поиск";
                ib_show_hide_search.CssClass = "checkbox_checked";
            }
        }

        protected void gv1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void gv1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void chb_table_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gv1.Rows.Count; i++)
            {
                CheckBox chb = (CheckBox)gv1.Rows[i].FindControl("chb_table");
                if (chb.Checked)
                {
                    gv1.Rows[i].BackColor = System.Drawing.Color.FromArgb(22, 219, 219);
                }
                else if (i % 2 != 0)
                { gv1.Rows[i].BackColor = System.Drawing.Color.White; }
                else { gv1.Rows[i].BackColor = System.Drawing.Color.FromArgb(227, 234, 235); }

            }
        }

        protected void chb_all_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb_all = (CheckBox)gv1.HeaderRow.FindControl("chb_all");
            for (int i = 0; i < gv1.Rows.Count; i++)
            {
                CheckBox chb = (CheckBox)gv1.Rows[i].FindControl("chb_table");
                if (chb_all.Checked)
                {
                    gv1.Rows[i].BackColor = System.Drawing.Color.FromArgb(22, 219, 219);
                    chb.Checked = true;
                }
                else if (i % 2 != 0)
                { 
                    gv1.Rows[i].BackColor = System.Drawing.Color.White;
                    chb.Checked = false;
                }
                else 
                { 
                    gv1.Rows[i].BackColor = System.Drawing.Color.FromArgb(227, 234, 235); 
                    chb.Checked = false;
                }
             }
        }

        protected void tv_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (tb_find_cats.Text == "")
            { tb_find_cats.Text ="'"+ tv.SelectedNode.Text+"'"; }
            else
            {
                tb_find_cats.Text += ",'";
                tb_find_cats.Text += tv.SelectedNode.Text;
                tb_find_cats.Text += "'";
            }
        }

        void find_child(TreeNode pn)
        {
            //соединились с БД
            string q_cat = "select * from cats";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q_cat, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            //Забираем данные с нода
            string p_i = pn.Value.ToString();

            while (dr.Read())
            {
                if (dr[3].ToString() == p_i)
                {
                    TreeNode n = new TreeNode(dr[1].ToString(), dr[0].ToString());
                    find_child(n);
                    pn.ChildNodes.Add(n);
                }
            }
            dr.Close(); com.Dispose(); ole_con.Close();
        }

        protected void b_show_tree_Click(object sender, EventArgs e)
        {
            mpe_cats.Show();
        }
    }
}