using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Comanda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl comanda = (HtmlGenericControl)Master.FindControl("ComandaHyperLink");
        comanda.Attributes["class"] += "active";
    }
}