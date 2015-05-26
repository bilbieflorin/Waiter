using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;
using rec_system;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        User u = Session["user"] as User;
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

        //    , label2 = new Label();
        //show_content.Controls.Add(label);
        //show_content.Controls.Add(new HtmlGenericControl("br"));
        //show_content.Controls.Add(label2);
        //label.Text = sha256("1234");
        //label2.Text = sha256("123456789");

        /*
        Dictionary<int, List<int>> dict = DatabaseFunctions.preparateComandateDupaUtilizator();
        foreach (KeyValuePair<int,List<int>> d in dict)
        {
            Label label = new Label();
            label.Text+=d.Key+": ";
            foreach (int i in d.Value)
            {
                label.Text+=i+" ";
            }
            HtmlGenericControl br = new HtmlGenericControl("br");
            show_content.Controls.Add(label);
            show_content.Controls.Add(br);
        }
         * */

        //int[] vecini = new int[50];
        //vecini = RecFunctions.Calculeaza_vecini(3, u.Id);

        //foreach (int vecin in vecini)
        //{
        //    Label label = new Label();
        //    label.Text += vecin + " ";
        //    HtmlGenericControl br = new HtmlGenericControl("br");
        //    show_content.Controls.Add(label);
        //    show_content.Controls.Add(br);
        //}
        List<Preparat> prep = rec_system.RecFunctions.Gaseste_recomandari(u.Id, null);
        foreach (var p in prep)
        {
            Label label = new Label();
            label.Text += p.toString() + " " + prep.Count;
            HtmlGenericControl br = new HtmlGenericControl("br");
            show_content.Controls.Add(label);
            show_content.Controls.Add(br);
        }
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