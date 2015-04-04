using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        //List<User> list_users = DatabaseFunctions.getUsers(ConnectionString);
        //foreach (User user in list_users)
        //{
        //    Label continut_users = new Label();
        //    HtmlGenericControl br = new HtmlGenericControl("br");
        //    continut_users.Text = user.toString();
        //    show_content.Controls.Add(continut_users);
        //    show_content.Controls.Add(br);
        //}
        Label label = new Label(), label2 = new Label();
        show_content.Controls.Add(label);
        show_content.Controls.Add(new HtmlGenericControl("br"));
        show_content.Controls.Add(label2);
        label.Text = sha256("1234");
        label2.Text = sha256("123456789");
    }

     private string sha256(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        foreach (byte bit in crypto)
        {
            hash += bit.ToString("x2");
        }
        return hash;
    }
}