using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace plaza
{
    public partial class SiteMaster : MasterPage
    {
        string name_user, fam_user;
        protected void Page_Load(object sender, EventArgs e)
        {
            name_user = (string)Session["name_user"];
            fam_user = (string)Session["fam_user"];
            if (name_user != null)
            { 
                l_user.Text = name_user + " " + fam_user;
                b_exit.Visible = true;
            }
        }

        protected void b_exit_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("autorize.aspx");
        }
    }
}