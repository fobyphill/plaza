using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace plaza
{
    public partial class edit_consumpt2 : System.Web.UI.Page
    {
        string login_user, name_user, fam_user, status_user; // переменные для данных пользователя
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //  "Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            login_user = (string)Session["login_user"];
            name_user = (string)Session["name_user"];
            fam_user = (string)Session["fam_user"];
            status_user = (string)Session["status_user"];
            if (login_user == null)
            { Response.Redirect("autorize.aspx"); }
            if (!Page.IsPostBack)// Запускаем эту программу только в первый раз
            {
                if (status_user == "u")
                {
                    p_menu.Visible = false;
                    p_main.CssClass = "maincontent_user";
                }
                string id_con = Request.QueryString["id_con"];
                string cat_id = "", data_create = "", value = "", bil_con = "", descript = "";
                int count_rec = Int32.Parse(Request.QueryString["count_rec"]);
                if (status_user == "u" && !ver_user(id_con))//защита от попыток юзера исправить чужую запись
                { Response.Redirect("autorise.aspx"); }
                //Получим данные расхода
                OleDbConnection ole_con = new OleDbConnection(con_str);
                if (count_rec == 1)
                {
                    string q_con = "select * from consumptions where id_con = " + id_con;
                    ole_con.Open();
                    OleDbCommand com = new OleDbCommand(q_con, ole_con);
                    OleDbDataReader dr = com.ExecuteReader();
                    dr.Read();
                    id_con = dr[0].ToString();//Заново получил ID расхода
                    data_create = dr[1].ToString(); //Получили дату создания расхода
                    value = dr[3].ToString();// Получили значение величины расхода
                    cat_id = dr[4].ToString();// Получили номер категории расхода
                    bil_con = dr[5].ToString();
                    descript = "";// Получаем значение описания расхода
                    if (dr[6].ToString() != "")
                    {
                        descript = dr[6].ToString();
                    }
                    dr.Close();
                    com.Dispose();
                    ole_con.Close();
                }
                    //Все необходимые данные расхода получены

                    // Заполняем категории
                    string q_cat = "select * from cats";
                    ole_con.Open();
                    OleDbCommand com2 = new OleDbCommand(q_cat, ole_con);
                    OleDbDataReader dr2 = com2.ExecuteReader();
                    while (dr2.Read())
                    {
                        if (dr2[3].ToString() == "0")
                        {
                            TreeNode node_cat = new TreeNode(dr2[1].ToString(), dr2[0].ToString());
                            //string parent_id = dr[0].ToString();
                            find_child(node_cat);
                            tv.Nodes.Add(node_cat);
                        }
                    }
                    dr2.Close();
                    //выеделение текущей категории
                    if (count_rec == 1)
                    {
                        foreach (TreeNode n in tv.Nodes)
                        {
                            if (n.Value.ToString() == cat_id)
                            {
                                n.Select();
                                break;
                            }
                            select_child(n, cat_id);
                        }

                        tb_data.Text = DateTime.Parse(data_create).ToString("yyyy-MM-dd");

                        tb_value.Text = value;//Выводим значения расхода
                        tb_descript.Text = descript;//Выводим комментарий
                    }
                    //заполняем поля счетов
                    string q_bil = "select name_bil from bils";
                    com2 = new OleDbCommand(q_bil, ole_con);
                    dr2 = com2.ExecuteReader();
                    ddl_bils.Items.Add("Выберите счет");
                    while (dr2.Read())
                    {
                        ddl_bils.Items.Add(dr2[0].ToString());
                    }
                    //выделим счет данного расхода
                    if (count_rec == 1)
                    {
                        for (int i = 0; i < ddl_bils.Items.Count; i++)
                        {
                            if (ddl_bils.Items[i].Text == bil_con)
                            {
                                ddl_bils.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    //Если счетов не задано, извещаем пользователя
                    if (ddl_bils.Items.Count == 0)
                    {
                        l_bil.Text = "Создайте счет на странице <a href='bils.aspx'>Управление счетами</a>";
                        l_bil.CssClass = "hint stress";
                    }

                    //Итак. все значения вывели. 
                    ole_con.Close();
                }
        }

        protected void b_save_Click(object sender, EventArgs e)
        {
            string id_con = Request.QueryString["id_con"];//получили id записи
            bool flag = true;

            OleDbConnection ole_con = new OleDbConnection(con_str);
            //Формируем запрос на изменение
            string q_update_con = "UPDATE consumptions SET data_change ='";
            //Получим дату изменения
            string data_change = DateTime.Now.ToString("yyyy-MM-dd");
            q_update_con += data_change;
            q_update_con += "'";

            //Получим ID категории
            if (tv.SelectedValue !="")
            {
                q_update_con += ", cat_con = ";
                q_update_con += tv.SelectedValue.ToString();
            }
            //Получим измененную дату создания заказа
            if (tb_data.Text != "")
            {
                DateTime dt = new DateTime();
                dt = DateTime.Parse(tb_data.Text);
                q_update_con += ",";
                q_update_con += " data_create = '";
                q_update_con += dt.ToString("yyyy-MM-dd");
                q_update_con += "'";
            }
            //Получим значение расхода
            if (tb_value.Text != "")
            {
                float value;
                tb_value.Text = tb_value.Text.Replace('.', ',');
                if (!Single.TryParse(tb_value.Text, out value))
                {
                    l_value.Text = "Укажите Корректное число";
                    l_value.Style.Add("color", "red");
                    flag = false;
                }
                else
                {
                    tb_value.Text = value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                    q_update_con += ",";
                    q_update_con += " value_con = ";
                    q_update_con += tb_value.Text;
                }
            }
            //Получим значение счета
            if (ddl_bils.SelectedIndex != 0)
            {
                q_update_con += ",";
                q_update_con += " bil_con = '";
                q_update_con += ddl_bils.SelectedItem.Text;
                q_update_con += "'";
            }
            //Получим комментарий
            if (tb_descript.Text != "")
            {
                q_update_con += ",";
                q_update_con += " descript_con = '";
                q_update_con += tb_descript.Text;
                q_update_con += "'";
            }
            //Объединим данные в переменной запроса
          /*  string q_update_con = "update consumptions set data_create='" + data_create + "', data_change ='" +
            data_change + "', value_con = " + tb_value.Text + ", cat_con = " + num_cat +
            ", bil_con='" + bil + "', descript_con = '" + tb_descript.Text + "', change_login = '" +
            login_user + "' where id_con = " + id_con;*/

            if (flag)
            {
                q_update_con += " where id_con in (";
                q_update_con += id_con;
                q_update_con += ")";
                ole_con.Open();
                OleDbCommand com = new OleDbCommand(q_update_con, ole_con);
                com.ExecuteNonQuery(); //Выполнить изменение данных в БД
                com.Dispose();
                ole_con.Close();
                System.Threading.Thread.Sleep(450);
                if (status_user == "a")
                { Response.Redirect("consumptions.aspx"); }
                else
                { Response.Redirect("cons_user.aspx"); }
            }
           
        }

        protected void b_cancel_Click(object sender, EventArgs e)
        {
            if (status_user == "a")
            { Response.Redirect("consumptions.aspx"); }
            else
            { Response.Redirect("cons_user.aspx"); }
        }
       public void find_child(TreeNode pn)
        {
            //соединились с БД
           // string con_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q_cat = "select * from cats";
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

        bool ver_user(string id)// проверка пользователя и даты заказа
        {
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q = "select create_login, data_change from consumptions where id_con = " + id;
            OleDbCommand com = new OleDbCommand(q, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                string dt = DateTime.Now.ToShortDateString();
                if (dr[0].ToString() == login_user && DateTime.Parse(dr[1].ToString()) == DateTime.Parse(dt))
                { return true; }
            }
            return false;

        }
    }
}