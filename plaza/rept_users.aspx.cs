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
    public partial class rept_users : System.Web.UI.Page
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
                string method_date = Session["method_date"].ToString();
                if (method_date == "month")
                {
                    l_descript.Text = "Затраты по счетам ";
                    month_report();
                }
                else if (method_date == "period")
                {
                    l_descript.Text = "Затраты по счетам ";
                    period_report();
                }
            }
        }
        void month_report()
        {
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            string checked_cats = Request.QueryString["checked_cats"];
            if (checked_cats != "")
            {
                string[] array_cats = checked_cats.Split(',');
                checked_cats = "";
                for (int i = 0; i < array_cats.Length; i++)
                {
                    checked_cats += "\'";
                    checked_cats += array_cats[i];
                    checked_cats += "\',";
                }
                if (checked_cats.Length > 0)
                { checked_cats = checked_cats.Substring(0, checked_cats.Length - 1); }
                l_descript.Text += "за " + month_12[Int32.Parse(month) - 1] + " " + year + " года";
            }

            DataSet ds = new DataSet();
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q_report = "";
            if (checked_cats == "")
            {
                q_report = "select create_login, sum(value_con) as value_con from consumptions " +
            "WHERE month(consumptions.data_create) = " + month + " and year(consumptions.data_create) = " + year +
            " GROUP BY bil_con";// Получим нужные данные в запросе
            }
            else
            {
                q_report = "select create_login, sum(value_con) as value_con from consumptions " +
            "WHERE month(consumptions.data_create) = " + month + " and year(consumptions.data_create) = " + year +
            " and create_login in (" + checked_cats + ") GROUP BY create_login";// Получим нужные данные в запросе
            }
            OleDbDataAdapter da = new OleDbDataAdapter(q_report, ole_con);
            da.Fill(ds);
            print_report(ds);
        }

        void period_report()
        {
            string month = Request.QueryString["month"];
            string year = Request.QueryString["year"];
            string checked_cats = Request.QueryString["checked_cats"];
            if (checked_cats != "")
            {
                string[] array_cats = checked_cats.Split(',');
                checked_cats = "";
                for (int i = 0; i < array_cats.Length; i++)
                {
                    checked_cats += "\'";
                    checked_cats += array_cats[i];
                    checked_cats += "\',";
                }
                if (checked_cats.Length > 0)
                { checked_cats = checked_cats.Substring(0, checked_cats.Length - 1); }

            }
            DataSet ds = new DataSet();
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            string q_report = "select create_login, sum(value_con) as value_con from consumptions";//создаем запрос по частям
            if (month != "" && year == "")
            {
                l_descript.Text += "за период с " + month;
                q_report += " where data_create >= '" + month + "'";
            }
            else if (month == "" && year != "")
            {
                l_descript.Text += "за период по" + year;
                q_report += " where data_create <= '" + year + "'";
            }
            else if (month != "" && year != "")
            {
                l_descript.Text += "за период с " + month + " по " + year;
                q_report += " where data_create between '" + month + "' and '" + year + "'";
            }

            if (checked_cats != "")
            { q_report += " and create_login in (" + checked_cats + ")"; }
            q_report += " GROUP BY create_login";
            OleDbDataAdapter da = new OleDbDataAdapter(q_report, ole_con);
            da.Fill(ds);
            print_report(ds);
        }

        void print_report(DataSet dset)
        {
            rv.Reset();
            rv.ProcessingMode = ProcessingMode.Local;
            LocalReport lr = rv.LocalReport;
            lr.ReportPath = "rep_bil.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "ds_bils_rep";
            rds.Value = dset.Tables[0];
            lr.DataSources.Add(rds);
        }
    }
}