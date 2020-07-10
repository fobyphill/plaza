using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace plaza
{
    public partial class edit_plan : System.Web.UI.Page
    {
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //    "Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
            //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
            //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
            //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
        string login_user, name_user, fam_user, status_user; // переменные для данных пользователя

        protected void Page_Load(object sender, EventArgs e)
        {
            //Получим данные из сессии
            login_user = (string)Session["login_user"];
            name_user = (string)Session["name_user"];
            fam_user = (string)Session["fam_user"];
            status_user = (string)Session["status_user"];
            if (status_user != "a")
            { Response.Redirect("autorize.aspx"); }

            string id_plan = Request.QueryString["id_plan"];// Получим айди записи из УРЛа
            string count_rec = Request.QueryString["count_rec"];
            if (!Page.IsPostBack)
            {
                string data_plan = "", value_plan = "", cat_plan = "", bil_plan = "", descript_plan = "";
                OleDbConnection ole_con = new OleDbConnection(con_str);
                if (count_rec == "1")
                {
                    //Получим данные записи о планировании
                    string q_plan = "select * from plans where id_plan = " + id_plan;
                    ole_con.Open();
                    OleDbCommand com = new OleDbCommand(q_plan, ole_con);
                    OleDbDataReader dr = com.ExecuteReader();
                    dr.Read();
                    data_plan = dr[1].ToString();
                    value_plan = dr[2].ToString();
                    cat_plan = dr[3].ToString();
                    bil_plan = dr[4].ToString();
                    descript_plan = dr[5].ToString();
                    dr.Close();
                    com.Dispose();
                    ole_con.Close();
                }
                //Заполним поля категорий
                string q_cat = "select * from cats";
                //соединение с БД
                ole_con.Open();
                OleDbCommand com2 = new OleDbCommand(q_cat, ole_con);
                OleDbDataReader dr2 = com2.ExecuteReader();
                while (dr2.Read())
                {
                    if (dr2[3].ToString() == "0")
                    {
                        TreeNode node_cat = new TreeNode(dr2[1].ToString(), dr2[0].ToString());
                        find_child(node_cat);
                        tv.Nodes.Add(node_cat);
                    }
                }
                //закрываем БД
                dr2.Close();
                com2.Dispose();
                if (count_rec == "1")
                {
                    //Выделим нужное значение категории
                    foreach (TreeNode n in tv.Nodes)
                    {
                        if (n.Value.ToString() == cat_plan)
                        {
                            n.Select();
                            break;
                        }
                        select_child(n, cat_plan);
                    }
                }
                //Заполняем комбобокс со счетами
                string q_bil = "select name_bil from bils";
                com2 = new OleDbCommand(q_bil, ole_con);
                dr2 = com2.ExecuteReader();
                ddl_bils.Items.Add("Выберите счет");
                while (dr2.Read())
                {
                    ddl_bils.Items.Add(dr2[0].ToString());
                }
                if (ddl_bils.Items.Count == 0)
                {
                    l_bil.Text = "Создайте счет на странице \"Управление счетами\"";
                    l_bil.CssClass = "hint stress";
                }
                dr2.Close();
                ole_con.Close();
                com2.Dispose();

                if (count_rec == "1")//вывод счета, значения и коммента
                {
                    for (int i = 0; i < ddl_bils.Items.Count; i++)//выделяем счет
                    {
                        if (ddl_bils.Items[i].Text == bil_plan)
                        {
                            ddl_bils.SelectedIndex = i;
                            break;
                        }
                    }
                    tb_value.Text = value_plan;//выводим значение в поле
                    tb_descript.Text = descript_plan;//выводим комментарий
                }
                //заполним комбобокс с годами
                if (count_rec == "1")
                {
                    DateTime dt = DateTime.Parse(data_plan);
                    tb_data_plan.Text = dt.ToString("yyyy-MM");
                }

            }
        }

        protected void ib_show_hide_Click(object sender, ImageClickEventArgs e)
        {
            if (l_collapse.Text == "Развернуть все")
            {
                tv.ExpandAll();
                l_collapse.Text = "Свернуть все";
            }
            else
            {
                tv.CollapseAll();
                l_collapse.Text = "Развернуть все";
            }
        }

        protected void b_save_Click(object sender, EventArgs e)
        {
            string id_plan = Request.QueryString["id_plan"];
            OleDbConnection ole_con = new OleDbConnection(con_str);
            //Формируем запрос на изменение
            string q_update_plan = "update plans set ";
            bool flag = false, flag_error = false;// Флаг. Индикатор запроса и отсутствия ошибки
            //Получим обновленную дату планирования
            if (tb_data_plan.Text != "")
            {
                DateTime dt = new DateTime();
                bool try_parse_date = false;
                try_parse_date = DateTime.TryParse(tb_data_plan.Text, out dt);
                if (try_parse_date)
                { 
                    q_update_plan += "data_plan = '" + dt.ToString("yyyy-MM-dd") + "'";
                    flag = true;
                }
                else
                {
                    flag_error = true;
                    l_period.Text = "Вероятно Ваш браузер Mozilla или InternetExplorer. Укажите месяц в формате ГГГГ-ММ";
                    l_period.CssClass = "stress";
                }
                
                
            }
            
            //Получим ID категории
            if (tv.SelectedValue.ToString() != "")
            {
                if (flag)
                {q_update_plan +=", ";}
                q_update_plan += "cat_plan = ";
                q_update_plan += tv.SelectedValue.ToString();
                flag = true;
            }
            //Получим значение расхода
            if (tb_value.Text != "")
            {
                float value_plan;
                tb_value.Text = tb_value.Text.Replace('.', ',');
                if (!Single.TryParse(tb_value.Text, out value_plan))
                {
                    l_value.Text = "Укажите Корректное число";
                    l_value.Style.Add("color", "red");
                    flag_error = true;
                }
                else
                {
                    if (flag)
                    { q_update_plan += ", "; }
                    tb_value.Text = value_plan.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                    q_update_plan += "value_plan = " + tb_value.Text;
                    flag = true;
                }
            }
            //Получим значение счета
            if (ddl_bils.SelectedIndex > 0)
            {
                if (flag)
                { q_update_plan +=", ";}
                q_update_plan += "bil_plan = '"+ ddl_bils.SelectedItem.Text+"'";
                flag = true;
            }
            //описание плана
            if (tb_descript.Text != "")
            {
                if (flag)
                { q_update_plan +=", ";}
                q_update_plan += "descript_plan = '"+ tb_descript.Text+"'";
                flag = true;
            }
            //Объединим данные в переменной запроса
           /* string q_update_plan = "update plans set  data_plan ='" + data_plan + "', value_plan = "+value_plan+", cat_plan = "+cat_plan+
                ", bil_plan = '"+bil_plan+"', descript_plan = '"+ descript_plan+"', login_user = '"+login_user+"' where id_plan = "+id_plan;*/
            if (!flag_error && flag)
            {
                q_update_plan += "where id_plan in ("+id_plan+")";
                ole_con.Open();
                OleDbCommand com = new OleDbCommand(q_update_plan, ole_con);
                com.ExecuteNonQuery(); //Выполнить изменение данных в БД
                com.Dispose();
                ole_con.Close();
                System.Threading.Thread.Sleep(450);
                Response.Redirect("plans.aspx");

            }
        }

        protected void b_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("plans.aspx");
        }

        void find_child(TreeNode pn)
        {
            string id_con = Request.QueryString["id_con"];
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
            dr.Close();
            com.Dispose();
            ole_con.Close();
        }

        void select_child(TreeNode n, string cat)
        {
            if (n.ChildNodes.Count > 0)
            {
                foreach (TreeNode n_ch in n.ChildNodes)
                {
                    if (n_ch.Value.ToString() == cat)
                    {
                        n_ch.Select();
                        break;
                    }
                    select_child(n_ch, cat);
                }
            }
        }
    }
}