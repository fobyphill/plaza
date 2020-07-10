using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace plaza
{
    public partial class cats : System.Web.UI.Page
    {
        string login_user, name_user, fam_user, status_user; // переменные для данных пользователя

        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Получение данных из сессии и возврат на страницу авторизации при окончании сессии
            login_user = (string)Session["login_user"];
            name_user = (string)Session["name_user"];
            fam_user = (string)Session["fam_user"];
            status_user = (string)Session["status_user"];
           if (status_user !="a")
            { Response.Redirect("autorize.aspx"); }
            if (!Page.IsPostBack)
            {
                //создадим массив из айди категорий и описаний
                
                string q_decript = "select id_cat, descript_cat from cats";
                OleDbDataReader dr = my_query(q_decript);
                int i = 0;
                while (dr.Read())
                { i++; }
                dr.Close();
                string[,] descript_cats = new string[i,2];
                i = 0;
                dr = my_query(q_decript);
                while (dr.Read())
                {
                    descript_cats[i, 0] = dr[0].ToString();
                    descript_cats[i, 1] = dr[1].ToString();
                    i++;
                }
                dr.Close();
                Session["descript_cats"] = descript_cats;

               string q_cat = "select * from cats";
                dr = my_query(q_cat);
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
                //Заполним всплывающее окно категориями
                OleDbConnection ole_con = new OleDbConnection(con_str);
                ole_con.Open();
                OleDbCommand com = new OleDbCommand(q_cat, ole_con);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[3].ToString() == "0")
                    {
                        TreeNode node_cat = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        find_child(node_cat);
                        tv_parent.Nodes.Add(node_cat);
                    }
                }
                dr.Close();
            }
        }

        protected void b_add_cat_Click(object sender, EventArgs e)
        {
            bool flag_add = false; // Если флаг включается, значит некорректные данные
            //Получим ID категории
            string q_max_id = "SELECT Max(id_cat) FROM cats;";
            OleDbDataReader dr = my_query(q_max_id);
            dr.Read();
            int id = Int32.Parse(dr[0].ToString());
            id++;
             string id_cat = id.ToString();
             string id_parent_cat = "";
            dr.Close();

            //Получим ID Родительской категории
            if (tb_parent_cat.Text == "")
            { id_parent_cat = "0"; }
            else
            {
                foreach (TreeNode n in tv.Nodes)
                {
                    if (n.Text == tb_parent_cat.Text)
                    {
                        Session["id_parent_cat"] = n.Value.ToString();
                        break;
                    }
                    else
                    {
                        find_parent_cat(n, tb_parent_cat.Text);
                        if (Session["id_parent_cat"] != null)
                        { break; }
                    }
                }
                id_parent_cat = (string)Session["id_parent_cat"];
                Session["id_parent_cat"] = null;
            }
            if (l_cat.Text == "")
            {
                l_cat.Text = "Заполните поле \"Категория\"";
                flag_add = true;
            }
            if (id_parent_cat == null)
            { l_parent_cat.Text = "Укажите категорию корректно";
            flag_add = true;
            }

            //Проверка дублей внутри на одном уровне
            if (tv.SelectedNode == null)
            {
                foreach (TreeNode n in tv.Nodes)
                {
                    if (n.Text == tb_cat.Text)
                    {
                        flag_add = true;
                        l_cat.Text = "На этом уровне уже есть категория с таким названием";
                        l_cat.CssClass = "stress";
                        break;
                    }
                }
            }
            else if (tv.SelectedNode.Parent == null)
            {
                foreach (TreeNode n in tv.Nodes)
                {
                    if (n.Text == tb_cat.Text)
                    {
                        flag_add = true;
                        l_cat.Text = "На этом уровне уже есть категория с таким названием";
                        l_cat.CssClass = "stress";
                        break;
                    }
                }
            }
            else
            {
                TreeNode nod = tv.SelectedNode.Parent;
                foreach (TreeNode n in nod.ChildNodes)
                {
                    if (n.Text == tb_cat.Text)
                    {
                        flag_add = true;
                        l_cat.Text = "На этом уровне уже есть категория с таким названием";
                        l_cat.CssClass = "stress";
                        break;
                    }
                }
            }

            
            //запрос на добавление данных
            string ex_add = "INSERT INTO cats ( id_cat, name_cat, descript_cat, parent_id ) VALUES (" + id_cat + ", '" + tb_cat.Text + "', '"
                +tb_descript.Text +"', " + id_parent_cat + ")";

            //Вносим изменение в БД
            if (!flag_add)
            {
                querry_execute(ex_add);
                System.Threading.Thread.Sleep(450);
                Response.Redirect(Request.RawUrl);
            }


        }
        
        protected void b_change_Click(object sender, EventArgs e)
        {
            bool flag_edit = false;
            string id_cat = "";
            //Получим ID категории
            if (tv.SelectedNode == null)
            { 
                l_cats.Text = "Ни одна категория не выбрана";
                flag_edit = true;
            }
            else {id_cat = tv.SelectedNode.Value.ToString(); }
            
            //Получим ID Родительской категории
            string id_parent_cat;
            if (tb_parent_cat.Text == "")
            { id_parent_cat = "0"; }
            else
            {
                foreach (TreeNode n in tv.Nodes)
                {
                    if (n.Text == tb_parent_cat.Text)
                    {
                        Session["id_parent_cat"] = n.Value.ToString();
                        break;
                    }
                    else
                    {
                        find_parent_cat(n, tb_parent_cat.Text);
                        if (Session["id_parent_cat"] != null)
                        { break; }
                    }
                }
                id_parent_cat = (string)Session["id_parent_cat"];
                Session["id_parent_cat"] = null;
            }
            if (l_cat.Text == "")
            {
                l_cat.Text = "Заполните поле \"Категория\"";
                flag_edit = true;
            }
            if (id_parent_cat == null)
            {
                l_parent_cat.Text = "Укажите корректно родительскую категорию";
                flag_edit = true;
            }
            //Сформируем запрос на изменение данных
            string ex_edit = "update cats set name_cat = '" + tb_cat.Text + "', descript_cat = '" + tb_descript.Text + "', parent_id = " 
                + id_parent_cat + " where id_cat = " + id_cat;
            //вносим изменение данных
            if (!flag_edit)
            {
                querry_execute(ex_edit);
                System.Threading.Thread.Sleep(450);
                Response.Redirect(Request.RawUrl);
            }

        }

        protected void b_delete_Click(object sender, EventArgs e)
        {
            bool flag_del = false;
            if (tv.SelectedNode == null)
            {
                l_cat.Text = "Не выбрана ни одна категория";
                l_cat.CssClass = "stress";
                flag_del = true;
            }
            else if (tv.SelectedNode.ChildNodes.Count != 0)
            {
                l_cat.Text = "Категория не может быть удалена, т.к. имеет дочерние категории";
                l_cat.CssClass = "stress";
                flag_del = true;
            }
            else
            {
                //Проверка, есть ли записи в выбранной на удаление категории
                string q_count_cons = "select count(*) from consumptions where cat_con = " + tv.SelectedNode.Value.ToString();
                OleDbDataReader dr = my_query(q_count_cons);
                dr.Read();
                int count_cons = Int32.Parse(dr[0].ToString());
                dr.Close();
                if (count_cons > 0)
                {
                    l_cat.Text = "В выбранной категории есть записи о расходах в количестве " + count_cons.ToString() + " шт.<br />";
                    l_cat.CssClass = "stress";
                    flag_del = true;
                }
                string q_count_plans = "SELECT count(*) from plans where cat_plan = " + tv.SelectedNode.Value.ToString();
                dr = my_query(q_count_plans);
                dr.Read();
                count_cons = Int32.Parse(dr[0].ToString());
                if (count_cons > 0)
                {
                    if (l_cat.Text == "Категория затрат")
                    {l_cat.Text = "";}
                    l_cat.Text += "В выбранной категории есть записи о планировании в количестве " + count_cons.ToString() + "шт.";
                    l_cat.CssClass = "stress";
                    flag_del = true;
                }
            }
            if (!flag_del)
            {
                mpe.Show();
            }
        }

        protected void b_yes_Click(object sender, EventArgs e)
        {
            string ex_del_cat = "delete from cats where id_cat = " + tv.SelectedNode.Value.ToString();
            querry_execute(ex_del_cat);
            System.Threading.Thread.Sleep(450);
            Response.Redirect(Request.RawUrl);

        }

        protected void tv_SelectedNodeChanged(object sender, EventArgs e)
        {
            //Указываем категорию
                tb_cat.Text = tv.SelectedNode.Text;
                l_cat.Text = "Категория затрат";
                l_cat.CssClass = "norm";
            //Выведем описание категории
                string[,] descript_cats = (string[,])Session["descript_cats"];
                for (int i = 0; i < descript_cats.Length; i++ )
                {
                    if (descript_cats[i, 0] == tv.SelectedNode.Value.ToString())
                    {
                        tb_descript.Text = descript_cats[i,1];
                        break;
                    }
                }
                //Указываем родительскую категорию
                if (tv.SelectedNode.Parent == null)
                {
                    tb_parent_cat.Text = "";
                }
                else
                {
                    TreeNode pn = tv.SelectedNode.Parent;
                    tb_parent_cat.Text = pn.Text;
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
        }// конец процедуры поиска категории

        void find_child(TreeNode pn)
        {
            //соединились с БД
           // string con_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q_cat = "select * from cats";
            OleDbCommand com = new OleDbCommand(q_cat, ole_con);
            com.CommandType = CommandType.Text;
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

        OleDbDataReader my_query(string q)//Процедура запроса данных из БД
        {
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q, ole_con);
            com.CommandType = CommandType.Text;//тип команды - текст
            OleDbDataReader dr = com.ExecuteReader();
            return dr;

        }

        void find_parent_cat(TreeNode n, string cat) //процедура нахождения категории
        {
            if (n.ChildNodes.Count > 0)
            {
                foreach (TreeNode n_ch in n.ChildNodes)
                {
                    if (n_ch.Text == cat)
                    {
                        Session["id_parent_cat"] = n_ch.Value.ToString();
                        break;
                    }
                    else {
                        find_parent_cat(n_ch, cat);
                        if (Session["id_parent_cat"] != null)
                        { break; }
                        }

                }

            }
        }

    void querry_execute(string q_e)
        {
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q_e, ole_con);
            com.CommandType = CommandType.Text;
            com.ExecuteNonQuery();
            ole_con.Close();
        }

    protected void tv_parent_SelectedNodeChanged(object sender, EventArgs e)
    {
        tb_parent_cat.Text = tv_parent.SelectedNode.Text;
    }
    }
}