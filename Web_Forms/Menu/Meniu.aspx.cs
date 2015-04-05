using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    protected void MeniuListItem_ImagineClick(object sender, ImageClickEventArgs e)
    {
        ImageButton targetImage = sender as ImageButton;

        if (targetImage.CommandName.Equals("DisplayIndex"))
        {
            int imageIndex = Convert.ToInt32(targetImage.CommandArgument);

            // `imageIndex` e in functie de pozitia in pagina.
            // 0 <= `imageIndex` <= MeniuDataPager.MaximumRows.
            // Luam in calcul pe ce pagina de meniu suntem ca sa calculam corect indexul din lista interna.
            int meniuIndex = imageIndex + MeniuDataPager.StartRowIndex;

            Debug.Assert(meniuIndex < meniu_.Count, "Index inexistent / meniu e null - ceea ce nu ar trebui sa se intample!");

            Preparat preparat = meniu_[meniuIndex];

            ModalItemTitle.Text = preparat.Denumire;
            ModalItemImage.ImageUrl = preparat.PathImagine;
            string ingrediente =" ";
            int i;
            for(i=0; i < preparat.ListaIngrediente.Count-1; i++)
            {
                ingrediente+=preparat.ListaIngrediente[i]+", ";
            }
            ingrediente += preparat.ListaIngrediente[i]+".";
            ModalItemBody.InnerHtml = "Specific: " + preparat.Specific + "<br />" + "Tip: " + preparat.Tip + "<br />" + "Gramaj: " + preparat.Gramaj + "<br />" + "Pret: " + preparat.Pret
                + "<br />Ingrediente: "+ingrediente;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            ModalUpdatePanel.Update();
        }
    }
}