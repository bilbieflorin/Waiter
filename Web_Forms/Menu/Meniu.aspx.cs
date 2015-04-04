using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using db_mapping;

public partial class Meniu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        connection_string_ = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 

        HtmlGenericControl meniu = (HtmlGenericControl)Master.FindControl("MeniuHyperLink");
        meniu.Attributes["class"] += "active";

        if (!IsPostBack)
        {
            meniu_ = DatabaseFunctions.getPreparate(connection_string_);
            bindMeniuListViewData();
        }

    }

    protected void MeniuListView_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        // Set current page startindex, max rows and rebind to false/
        MeniuDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

        // Rebind List View
        bindMeniuListViewData();
    }

    private string connection_string_;
    private static List<Preparat> meniu_;

    private void bindMeniuListViewData()
    {
        MeniuListView.DataSource = meniu_;
        MeniuListView.DataBind();
    }
    protected void MeniuListView_DetaliiPreparat(object sender, ImageClickEventArgs e)
    {
        ImageButton targetButton = sender as ImageButton;

        Preparat preparat = meniu_[Convert.ToInt32(targetButton.CommandArgument) + MeniuDataPager.StartRowIndex];

        lblModalTitle.Text = preparat.Denumire;
        lblModalBody.Text = preparat.Pret + " " + preparat.Tip;
        MeniuModalImage.ImageUrl = preparat.PathImagine;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
        upModal.Update();
    }
}