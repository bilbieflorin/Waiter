using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Meniu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl meniu = (HtmlGenericControl)Master.FindControl("MeniuHyperLink");
        meniu.Attributes["class"] += "active";
    }
}