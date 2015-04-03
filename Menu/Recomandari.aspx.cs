using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Recomandari : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl recomandari = (HtmlGenericControl)Master.FindControl("RecomandariHyperLink");
        recomandari.Attributes["class"] += "active";
    }
}