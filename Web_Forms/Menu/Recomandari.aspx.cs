using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using db_mapping;
using rec_system;

public partial class Recomandari : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
        else
        {
            HtmlGenericControl recomandari = (HtmlGenericControl)Master.FindControl("RecomandariHyperLink");
            recomandari.Attributes["class"] += "active";
            User user = Session["user"] as User;
            if(IsPostBack)
                recomandari_ = Session["recomandari"] as List<Preparat>;
            else
            {    
                recomandari_ = RecFunctions.Gaseste_recomandari_Collective(user.Id, null,4);
                Session["recomandari"] = recomandari_;
                bindRecomandariListViewData();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$('.nbsp').each(function() {$(this).before($('<span>').html('&nbsp;')); $(this).after($('<span>').html('&nbsp;'));});", true);
        }
    }

    protected void recomandariListItemImagineClick(object sender, ImageClickEventArgs e)
    {
        ImageButton target_image = sender as ImageButton;

        if (target_image.CommandName.Equals("DisplayIndex"))
        {
            int image_index = Convert.ToInt32(target_image.CommandArgument);

            // `imageIndex` e in functie de pozitia in pagina.
            // 0 <= `imageIndex` <= MeniuDataPager.MaximumRows.
            // Luam in calcul pe ce pagina de meniu suntem ca sa calculam corect indexul din lista interna.
            int meniu_index = image_index + RecomandariDataPager.StartRowIndex;

            Debug.Assert(meniu_index < recomandari_.Count, "Index inexistent / meniu e null - ceea ce nu ar trebui sa se intample!");

            Preparat preparat = recomandari_[meniu_index];

            ModalItemTitle.Text = preparat.Denumire;
            ModalItemImage.ImageUrl = preparat.PathImagine;
            string ingrediente="";
            if (preparat.ListaIngrediente.Capacity > 0)
            {
                int i;
                for (i = 0; i < preparat.ListaIngrediente.Count - 1; i++)
                {
                    ingrediente += preparat.ListaIngrediente[i] + ", ";
                }
                ingrediente += preparat.ListaIngrediente[i] + ".";
            }
            else
                ingrediente = "None";
            ModalItemBody.InnerHtml = "Specific: " + preparat.Specific + "<br />" + "Tip: " + preparat.Tip + "<br />" + "Gramaj: " + preparat.Gramaj + "<br />" + "Pret: " + preparat.Pret
                + "<br />Ingrediente: " + ingrediente;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            ButtonComanda.CommandArgument = meniu_index + "";
            ModalUpdatePanel.Update();
        }
    }
    
    protected void recomandariListViewPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        // Set current page startindex, max rows and rebind to false/
        RecomandariDataPager.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

        // Rebind List View
        bindRecomandariListViewData();
    }

    protected void buttonComandaClick(object sender, EventArgs e)
    {
        Comanda comanda = Session["comanda"] as Comanda;
        if (comanda == null)
        {
            User user = Session["user"] as User;
            comanda = new Comanda();
            if (user == null)
                comanda.IdUser = 0;
            else
                comanda.IdUser = user.Id;
            comanda.Data = DateTime.Now;
        }
        Button order_button = sender as Button;
        int index_meniu = Convert.ToInt32(order_button.CommandArgument);
        ItemComanda item = new ItemComanda(recomandari_[index_meniu], 1);
        comanda.addItemComanda(item);
        Session["comanda"] = comanda;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal('hide')", true);
        Label badge = Master.FindControl("Badge") as Label;
        UpdatePanel update_badge = Master.FindControl("BadgeUpdatePanel") as UpdatePanel;
        badge.Text = comanda.NumarPreparate + "";
        update_badge.Update();
    }

    private void bindRecomandariListViewData()
    {
        RecomandariListView.DataSource = recomandari_;
        RecomandariListView.DataBind();
    }

    private List<Preparat> recomandari_;
}