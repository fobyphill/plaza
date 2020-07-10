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
    public partial class users : System.Web.UI.Page
    {
        string login_user, name_user, fam_user;
        string status_user; // переменные для данных пользователя
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Зададим параметры пользователя
            login_user = (string)Session["login_user"];
            name_user = (string)Session["name_user"];
            fam_user = (string)Session["fam_user"];
            status_user = (string)Session["status_user"];
            //перебросим пользователя на экран авторизации по окончании сессии
            if (status_user != "a")
            { Response.Redirect("autorize.aspx"); }

            if (!Page.IsPostBack)
            {
                string q_users = "SELECT * from users";

                OleDbConnection ole_con = new OleDbConnection(con_str);
                ole_con.Open();
                OleDbCommand com = new OleDbCommand(q_users, ole_con);
                OleDbDataAdapter da = new OleDbDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);
                //Передаю массив со пользователями в сессию
                string[,] users_data = new string[ds.Tables[0].Rows.Count, 5];
                int i = 0;
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        users_data[i, j] = drow.ItemArray[j].ToString();
                    }
                    i++;
                }
                Session["users_data[,]"] = users_data;

                //Заполняю листбокс логинами
                i = 0;
                OleDbDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    lb_users.Items.Add(dr["login_user"].ToString());
                    if (dr[4].ToString() == "a")
                    { lb_users.Items[i].Attributes.Add("style", "background-color:#16dbdb"); }
                    i++;
                }
                com.Dispose(); ole_con.Close();// закрыть всех
            }
        }

        protected void b_add_Click(object sender, EventArgs e)
        {
            bool flag = false;
            flag = verify_user();//Проверка полей
            //Проверка логина
            for (int i = 0; i < lb_users.Items.Count; i++ )
            {
                if (tb_login.Text == lb_users.Items[i].Text)
                {
                    flag = true;
                    l_login.Text = "Указанный логин уже существует в системе";
                    l_login.CssClass = "stress";
                }
            }
            //Проверка пароля
            if (tb_password.Text != tb_pass2.Text)
            {
                l_password.Text = "Пароли не совпадают";
                l_password.CssClass = "stress";
                flag = true;
            }
            //Формируем и выполняем запрос на добавление
                if (!flag)
                {
                    string q_add = "INSERT INTO users ( name_user, fam_user, login_user, pass_user, status_user ) VALUES ('" + tb_name.Text + "', '"
                        + tb_fam.Text + "', '" + tb_login.Text + "', '" + tb_password.Text + "', '" + rbl_status.SelectedValue.ToString() + "')";
                    exe_query(q_add);
                    System.Threading.Thread.Sleep(450);
                    Response.Redirect(Request.RawUrl);
                }
        }

        protected void b_change_Click(object sender, EventArgs e)
        {
            bool flag = false;
            flag = ver_select_user();
            flag = verify_user();//Проверка полей
            //Проверка пароля
            if (tb_password.Text != tb_pass2.Text)
            {
                l_password.Text = "Пароли не совпадают";
                l_password.CssClass = "stress";
                flag = true;
            }
            //Формируем запрос на изменение и выполняем
            if (!flag)
            {
                string q_edit = "update users set name_user = '" + tb_name.Text + "', fam_user = '" + tb_fam.Text + "', login_user = '" + tb_login.Text +
                    "', pass_user = '" + tb_password.Text + "', status_user = '" + rbl_status.SelectedValue.ToString() + "' where login_user = '" +
                    lb_users.SelectedItem.Text+"'";
                exe_query(q_edit);
                System.Threading.Thread.Sleep(450);
                Response.Redirect(Request.RawUrl);

            }
        }

        protected void b_delete_Click(object sender, EventArgs e)
        {
            if (ver_select_user())
            {
                mpe.Show();
            }
        }

        protected void b_clear_Click(object sender, EventArgs e)
        {
            tb_fam.Text = "";
            tb_login.Text = "";
            tb_name.Text = "";
            tb_password.Text = "";
            lb_users.SelectedIndex = -1;
            rbl_status.SelectedIndex = -1;
        }

        protected void b_yes_Click(object sender, EventArgs e)
        {
            //Проверка, не администратора ли удаляем
            if (lb_users.SelectedItem.Text != "administrator")
            {
                //Перенаправляем планрования на administrarora
                string q_move_plans_admin = "update plans set login_user = 'administrator' where login_user = '"
                    + lb_users.SelectedItem.Text + "'";
                exe_query(q_move_plans_admin);

                //Перенаправляем затраты, созданные этим пользователем на администратора
                string q_move_cons_create_admin = "update consumptions set create_login = 'administrator' where create_login = '"
                + lb_users.SelectedItem.Text + "'";
                exe_query(q_move_cons_create_admin);

                //Перенаправляем затраты, отредактированные этим пользователем
                string q_move_cons_change_admin = "update consumptions set change_login = 'administrator' where change_login = '"
                + lb_users.SelectedItem.Text + "'";
                exe_query(q_move_cons_change_admin);

                //Создаем запрос и удаляем пользователя
                string q_del_user = "delete from users where login_user = '" + lb_users.SelectedItem.Text + "'";
                exe_query(q_del_user);
                System.Threading.Thread.Sleep(450);
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                l_login.Text = "Невозможно удалить пользователя \"administrator\"";
                l_login.CssClass = "stress";
            }


        }

        protected void b_no_Click(object sender, EventArgs e)
        {

        }

        protected void lb_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            label_norm();
            //проверка полей на администратора
            if (lb_users.SelectedItem.Text == "administrator")
            {
                tb_name.Text = " ";
                tb_fam.Enabled = false;
                tb_login.Enabled = false;
                tb_name.Enabled = false;
                rbl_status.Enabled = false;
            }
            else
            {
                tb_fam.Enabled = true;
                tb_login.Enabled = true;
                tb_name.Enabled = true;
                rbl_status.Enabled = true;
            }
            string[,] users_data = (string[,])Session["users_data[,]"];
            for (int i = 0; i < users_data.GetLength(0); i++)
            {
                if (users_data[i, 4] == "a") //перекрашиваем поля листбокса
                { lb_users.Items[i].Attributes.Add("style", "background-color:#16dbdb"); }
                if (lb_users.SelectedItem.Text == users_data[i,2]) // выводим выделенного пользователя
                {
                    tb_login.Text = users_data[i,2];
                    tb_name.Text = users_data[i, 0];
                    tb_fam.Text = users_data[i, 1];
                    tb_password.Text = users_data[i, 3];
                    tb_pass2.Text = users_data[i, 3];
                    rbl_status.SelectedValue = users_data[i, 4];
                }
            }
            /*if (lb_users.SelectedValue.ToString() == "a")
            {
                rbl_status.SelectedValue = "a";
            }
            else { rbl_status.SelectedValue = "u"; }*/
        }
                
        void exe_query(string q)
        {
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q, ole_con);
            com.ExecuteNonQuery();
            ole_con.Close();
        }

        bool verify_user()
                {
                    bool flag = false;
                    if (tb_name.Text == "")
                    {
                        flag = true;
                        l_name.Text = "Не указано имя пользователя";
                        l_name.CssClass = "stress";
                    }
                    if (tb_fam.Text == "")
                    {
                        flag = true;
                        l_fam.Text = "Не указана фамилия пользователя";
                        l_fam.CssClass = "stress";
                    }
                    if (tb_login.Text == "")
                    {
                        flag = true;
                        l_login.Text = "Не указан логин пользователя";
                        l_login.CssClass = "stress";
                    }
                    if (tb_password.Text == "")
                    {
                        flag = true;
                        l_password.Text = "Не указан пароль пользователя";
                        l_password.CssClass = "stress";
                    }
                    if (rbl_status.SelectedIndex == -1)
                    {
                        flag = true;
                        l_status.Text = "Не указан статус пользователя";
                        l_status.CssClass = "stress";
                    }
                    return flag;
                }

        bool ver_select_user()
                {
                    if (lb_users.SelectedIndex == -1)
                    {
                        p_error.Visible = true;
                        return false;
                    }
                    else { return true; }
                }

        void label_norm()
        {
            l_login.Text = "Логин пользователя";
            l_login.CssClass = "norm";
            l_name.Text = "Имя пользователя";
            l_name.CssClass = "norm";
            l_password.Text = "Пароль пользователя";
            l_password.CssClass = "norm";
            l_status.Text = "Права пользователя";
            l_status.CssClass = "norm";
            l_fam.Text = "Фамилия пользователя";
            l_fam.CssClass = "norm";
            p_error.Visible = false;
        }

        protected void ib_show_hide_pass_Click(object sender, ImageClickEventArgs e)
        {
            if (tb_password.TextMode == TextBoxMode.Password)
            {
                tb_password.TextMode = TextBoxMode.SingleLine;
                l_collapse.Text = "Скрыть пароли";
                ib_show_hide_pass.CssClass = "checkbox_checked";
            }
            else
            {
                tb_password.TextMode = TextBoxMode.Password;
                l_collapse.Text = "Показать пароли";
                ib_show_hide_pass.CssClass = "checkbox_unckeck";

            }
        }

    }
}