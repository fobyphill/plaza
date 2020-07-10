using Microsoft.Reporting.WebForms;
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
    public partial class rept_complex : System.Web.UI.Page
    {
        string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
        //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=SQLOLEDB;Data Source=finplaza.ru,5108;Initial Catalog=finplaza_;User ID=plaza;Password=ZMxncb7913";
        //"Provider=SQLOLEDB;Data Source=PHILL-ПК\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
        //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
        string[] month_12 = { "январь", "февраль", "март", "апрель", "май",
                                    "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string type_report = Request.QueryString["type"];
                if (type_report == "fast")
                {
                    l_descript.Text = "Затраты и планы по главным категориям ";
                    fast_report();

                }
                else if (type_report == "only_cats")
                {
                    l_descript.Text = "Затраты и планы по выбранным категориям <br />";
                    only_cats_report();
                }
                else
                {
                    l_descript.Text = "Затраты и планы выбранным и дочерним категориям <br />";
                    with_include_report();
                }
            }
        }

        void fast_report()
        {
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            l_descript.Text += "за " + month_12[Int32.Parse(month) - 1] + " " + year + " года";
            DataSet ds = new DataSet();
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q_all_cats = "select name_cat, id_cat, parent_id from cats";//Собираем список из всех категорий
            OleDbDataAdapter da = new OleDbDataAdapter(q_all_cats, ole_con);
            da.Fill(ds);
            DataColumn col_sum_con = new DataColumn("sum_con");
            col_sum_con.DataType = System.Type.GetType("System.Single");
            ds.Tables[0].Columns.Add(col_sum_con);//Добавим столбец
            DataColumn col_sum_plan = new DataColumn("sum_plan");
            col_sum_plan.DataType = System.Type.GetType("System.Single");
            ds.Tables[0].Columns.Add(col_sum_plan);//Добавим второй - сумма планов
            string q_report = "SELECT cats.name_cat, cats.id_cat, cats.parent_id, sum(consumptions.value_con) as sum_con " +
            "FROM cats INNER JOIN consumptions ON consumptions.cat_con=cats.id_cat " +
            "WHERE month(consumptions.data_create) = " + month + " and year(consumptions.data_create) = " + year +
            " GROUP BY cats.name_cat, cats.id_cat, cats.parent_id";// Получим данные о затратах за нужный месяц в запросе
            OleDbCommand com = new OleDbCommand(q_report, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            while (dr.Read())//Вставим данные в ДатаСэт на свои места
            {
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (dr["id_cat"].ToString() == drow["id_cat"].ToString())
                    {
                        drow["sum_con"] = dr["sum_con"];
                        break;
                    }
                }
            }
            dr.Close();
            q_report = "SELECT cats.name_cat, cats.id_cat, cats.parent_id, sum(plans.value_plan) as sum_plan " +
            "FROM cats INNER JOIN plans ON plans.cat_plan=cats.id_cat " +
            "WHERE month(plans.data_plan) = " + month + " and year(plans.data_plan) = " + year +
            " GROUP BY cats.name_cat, cats.id_cat, cats.parent_id";// Получим данные о затратах за нужный месяц в запросе
            com = new OleDbCommand(q_report, ole_con);
            dr = com.ExecuteReader();
            while (dr.Read())//Вставим данные в ДатаСэт на свои места
            {
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (dr["id_cat"].ToString() == drow["id_cat"].ToString())
                    {
                        drow["sum_plan"] = dr["sum_plan"];
                        break;
                    }
                }
            }
            //собираем сумму расходов и планов по главным категориям
            bool flag = true;//
            while (flag)
            {
                flag = false;
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow["parent_id"].ToString() != "0" && drow["sum_con"].ToString() != "")
                    {//если категория не корневая, и если есть деньги, то
                        foreach (DataRow drow_cut in ds.Tables[0].Rows)
                        {//ищем ее родителя
                            if (drow_cut["id_cat"].ToString() == drow["parent_id"].ToString())
                            {
                                float summa = 0;
                                Single.TryParse(drow_cut["sum_con"].ToString(), out summa);
                                summa += float.Parse(drow["sum_con"].ToString());
                                drow_cut["sum_con"] = summa.ToString();
                                drow["sum_con"] = System.DBNull.Value;
                                break;
                            }
                        }
                        flag = true;
                    }
                    if (drow["parent_id"].ToString() != "0" && drow["sum_plan"].ToString() != "")
                    {//если категория не корневая, и если есть деньги, то
                        foreach (DataRow drow_cut in ds.Tables[0].Rows)
                        {//ищем ее родителя
                            if (drow_cut["id_cat"].ToString() == drow["parent_id"].ToString())
                            {
                                float summa = 0;
                                Single.TryParse(drow_cut["sum_plan"].ToString(), out summa);
                                summa += float.Parse(drow["sum_plan"].ToString());
                                drow_cut["sum_plan"] = summa.ToString();
                                drow["sum_plan"] = System.DBNull.Value;
                                break;
                            }
                        }
                        flag = true;
                    }
                }
            }
            // Удаляем все записи, кроме главных категорий
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if (drow["sum_con"].ToString() == "" && drow["sum_plan"].ToString() == "")
                { drow.Delete(); }
                else
                {
                    if (drow["sum_con"].ToString() == "")
                    { drow["sum_con"] = 0; }
                    if (drow["sum_plan"].ToString() == "")
                    { drow["sum_plan"] = 0; }

                }
            }
            ds.AcceptChanges();//применили обновления в ДатаСэт
            //Обновляем отчет
            print_report(ds);
        }

        void only_cats_report()
        {
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            string checked_cats = Request.QueryString["checked_cats"];
            DataSet ds = new DataSet();
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            //формируем запрос в соответствии с выбранным периодом - месяцем или произвольным
            l_descript.Text += "за " + month_12[Int32.Parse(month) - 1] + " " + year + " года";
            string q_report = "SELECT cats.name_cat, cats.id_cat,  sum(consumptions.value_con) as sum_con " +
            "FROM cats left JOIN consumptions ON consumptions.cat_con=cats.id_cat " +
            "and month(consumptions.data_create) = " + month + " and year(consumptions.data_create) = " + year +
            " and cats.id_cat in(" + checked_cats + ") GROUP BY cats.name_cat, cats.id_cat";// Получим нужные данные в запросе
            OleDbDataAdapter da = new OleDbDataAdapter(q_report, ole_con);
            da.Fill(ds); da.Dispose();
            //добавим данные о планировании
            DataColumn col_sum_plan = new DataColumn("sum_plan");
            col_sum_plan.DataType = System.Type.GetType("System.Single");
            ds.Tables[0].Columns.Add(col_sum_plan);//Добавим столбец
            q_report = "SELECT cats.name_cat, cats.id_cat,  sum(plans.value_plan) as sum_plan " +
            "FROM cats INNER JOIN plans ON plans.cat_plan=cats.id_cat " +
            "WHERE month(plans.data_plan) = " + month + " and year(plans.data_plan) = " + year +
            " and cats.id_cat in(" + checked_cats + ") GROUP BY cats.name_cat, cats.id_cat";// Получим данные о планировании в нужный месяц в запросе
            OleDbCommand com = new OleDbCommand(q_report, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                foreach(DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow[1].ToString() == dr[1].ToString())
                    {drow[3] = dr[2];}
                }
            }
            //Удалим лишние категории
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if (drow["sum_con"].ToString() == "" && drow["sum_plan"].ToString() == "")
                { drow.Delete(); }
                else
                {
                    if (drow["sum_con"].ToString() == "")
                    { drow["sum_con"] = 0; }
                    if (drow["sum_plan"].ToString() == "")
                    { drow["sum_plan"] = 0; }

                }
            }
            ds.AcceptChanges();//применили обновления в ДатаСэт
            print_report(ds);
        }

        void with_include_report()
        {
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            string checked_cats = Request.QueryString["checked_cats"];
            string list_cats = Session["list_cats"].ToString();
            string[] array_cats = list_cats.Split(',');
            DataSet ds = new DataSet("ds_rep");
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            //Запросим перечень всех категорий, их айди, родительских айди + суммы затрат в указанный юзером месяц, если затраты были
             l_descript.Text += "за " + month_12[Int32.Parse(month) - 1] + " " + year + " года";
             string q_report = "SELECT cats.name_cat, cats.id_cat, cats.parent_id, sum(consumptions.value_con) as sum_con " +
            "FROM cats LEFT JOIN consumptions ON consumptions.cat_con=cats.id_cat " +
            "and month(consumptions.data_create) = " + month + " and year(consumptions.data_create) = " + year +
            " GROUP BY cats.name_cat, cats.id_cat, cats.parent_id";// Получим нужные данные в запросе
            OleDbDataAdapter da = new OleDbDataAdapter(q_report, ole_con);
            da.Fill(ds);
            //добавим записи о планировании
            DataColumn col_sum_plan = new DataColumn("sum_plan");
            col_sum_plan.DataType = System.Type.GetType("System.Single");
            ds.Tables[0].Columns.Add(col_sum_plan);//Добавим столбец
            q_report = "SELECT cats.name_cat, cats.id_cat, cats.parent_id, sum(plans.value_plan) as sum_plan " +
            "FROM cats LEFT JOIN plans ON plans.cat_plan=cats.id_cat " +
            "and month(plans.data_plan) = " + month + " and year(plans.data_plan) = " + year +
            " GROUP BY cats.name_cat, cats.id_cat, cats.parent_id";
            OleDbCommand com = new OleDbCommand(q_report, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            while(dr.Read())
            {
                foreach(DataRow drow in ds.Tables[0].Rows)
                {
                    if(drow[1].ToString() == dr[1].ToString())
                    {drow[4] = dr[3];}
                }
            }
            //Удалим лишние категории
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                bool flag_own = false;
                for (int i = 0; i < array_cats.Length; i++)
                {
                    if (array_cats[i] == drow[1].ToString())
                    { flag_own = true; break; }
                }
                if (!flag_own)
                { drow.Delete(); }
            }
            ds.AcceptChanges();
            //Укажем старшим категориям ID_par = 0
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                bool flag_parent = false;
                foreach (DataRow drow_parent in ds.Tables[0].Rows)
                {
                    if (drow[2].ToString() == drow_parent[1].ToString())
                    {
                        flag_parent = true;
                        break;
                    }
                }
                if (!flag_parent)
                { drow[2] = 0; }
            }
            //собираем сумму расходов и планов по главным категориям
            bool flag = true;//
            while (flag)
            {
                flag = false;
                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    if (drow["parent_id"].ToString() != "0" && drow["sum_con"].ToString() != "")
                    {
                        foreach (DataRow drow_cut in ds.Tables[0].Rows)
                        {
                            if (drow_cut["id_cat"].ToString() == drow["parent_id"].ToString())
                            {
                                float summa = 0;
                                Single.TryParse(drow_cut["sum_con"].ToString(), out summa);
                                summa += float.Parse(drow["sum_con"].ToString());
                                drow_cut["sum_con"] = summa.ToString();
                                drow["sum_con"] = System.DBNull.Value;
                                break;
                            }
                        }
                        flag = true;
                    }
                    if (drow["parent_id"].ToString() != "0" && drow["sum_plan"].ToString() != "")
                    {
                        foreach (DataRow drow_cut in ds.Tables[0].Rows)
                        {
                            if (drow_cut["id_cat"].ToString() == drow["parent_id"].ToString())
                            {
                                float summa = 0;
                                Single.TryParse(drow_cut["sum_plan"].ToString(), out summa);
                                summa += float.Parse(drow["sum_plan"].ToString());
                                drow_cut["sum_plan"] = summa.ToString();
                                drow["sum_plan"] = System.DBNull.Value;
                                break;
                            }
                        }
                        flag = true;
                    }
                }
            }
            // Удаляем все записи, кроме главных категорий
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                if (drow["sum_con"].ToString() == "" && drow["sum_plan"].ToString() == "")
                { drow.Delete(); }
                else
                {
                    if (drow["sum_con"].ToString() =="")
                    { drow["sum_con"] = 0; }
                    if (drow["sum_plan"].ToString() == "")
                    { drow["sum_plan"] = 0; }
                }
            }
            ds.AcceptChanges();//применили обновления в ДатаСэт
            //Обновляем отчет
            print_report(ds);
        }

        void print_report(DataSet dset)
        {
            rv.Reset();
            rv.ProcessingMode = ProcessingMode.Local;
            LocalReport lr = rv.LocalReport;
            lr.ReportPath = "rep_complex.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dset_complex";
            rds.Value = dset.Tables[0];
            lr.DataSources.Add(rds);
        }
    }
}