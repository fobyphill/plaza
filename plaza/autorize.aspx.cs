using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace plaza.Content
{
    public partial class temp_auto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { Session.Abandon(); }
            Session["flag_add_edit_cat"] = "0";
        }
        protected void ib_show_hide_search_Click(object sender, ImageClickEventArgs e)
        {
            if (ib_show_hide_search.CssClass=="checkbox_checked")
            {ib_show_hide_search.CssClass = "checkbox_uncheck";}
            else { ib_show_hide_search.CssClass = "checkbox_checked"; }
        }

        protected void b_enter_Click(object sender, EventArgs e)
        {
            string con_str = "Provider=SQLOLEDB;Data Source=wpl27.hosting.reg.ru;Initial Catalog=u1078621_plaza;User ID=u1078621_user;Password=zmxncb123456.";
            //"Provider=SQLOLEDB;Data Source=DESKTOP-53LPOHJ\\SQLEXPRESS;Initial Catalog=plaza;Integrated Security=SSPI";
            //"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\plaza.accdb";
            string q_autorize = "select * from users where login_user = '" + tb_login.Text + "' and pass_user = '" + tb_pass.Text + "'";
            OleDbConnection ole_con = new OleDbConnection(con_str);
            ole_con.Open();
            OleDbCommand com = new OleDbCommand(q_autorize, ole_con);
            OleDbDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                Session["login_user"] = dr[2].ToString();
                Session["name_user"] = dr[0].ToString();
                Session["fam_user"] = dr[1].ToString();
                Session["status_user"] = dr[4].ToString();
                if ((string)Session["status_user"] == "a")
                {Response.Redirect("consumptions.aspx");}
                else {Response.Redirect("cons_user.aspx");}

            }
            else { p_error.Visible = true; }
            dr.Close(); ole_con.Close();
        }
    }
}