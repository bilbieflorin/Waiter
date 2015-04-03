using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String ConnectionString = System.Web.Configuration.WebConfigurationManager.
            ConnectionStrings["ConnectionString"].ConnectionString;

        //List<Preparat> lista_preparate = DatabaseFunctions.getPreparate(ConnectionString);
        //foreach (Preparat preparat in lista_preparate)
        //{
        //    Label continut_preparat = new Label();
        //    HtmlGenericControl br = new HtmlGenericControl("br");
        //    continut_preparat.Text = preparat.toString();
        //    show_content.Controls.Add(continut_preparat);
        //    show_content.Controls.Add(br);
        //}

        List<Users> list_users = DatabaseFunctions.getUsers(ConnectionString);
        foreach (Users user in list_users)
        {
            Label continut_users = new Label();
            HtmlGenericControl br = new HtmlGenericControl("br");
            continut_users.Text = user.toString();
            show_content.Controls.Add(continut_users);
            show_content.Controls.Add(br);
        }

    }
}