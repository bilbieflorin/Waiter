using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class ComandaWebPage : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
        else
        {
            HtmlGenericControl comandaHyperLink = (HtmlGenericControl)Master.FindControl("ComandaHyperLink");
            Label badge = Master.FindControl("Badge") as Label;
            badge.Attributes["style"] = "background: #cf262e;";
            comandaHyperLink.Attributes["class"] += "active";
            bindComandaListView();
        }
    }

    protected void deleteItemComanda(object sender, EventArgs e)
    {
        LinkButton remove_Item = sender as LinkButton;
        int id = Convert.ToInt32(remove_Item.CommandArgument);
        Comanda comanda = Session["comanda"] as Comanda;
        comanda.removeItemComanda(comanda.ListaItem[id] as ItemComanda);
        updateBadge();
        ComandaListView.DataBind();
        showTotal();
    }

    protected void adaugaPreparat(object sender, EventArgs e)
    {
        Comanda comanda = Session["comanda"] as Comanda;
        LinkButton adauga = sender as LinkButton;
        int id = Convert.ToInt32(adauga.CommandArgument);
        ItemComanda item = comanda.ListaItem[id] as ItemComanda;
        comanda.removeItemComanda(item);
        item.maresteCantitate();
        comanda.addItemComanda(item);
        Session["comanda"] = comanda;
        updateBadge();
        bindComandaListView();
        showTotal();
    }

    protected void deletePreparat(object sender, EventArgs e)
    {
        Comanda comanda = Session["comanda"] as Comanda;
        LinkButton adauga = sender as LinkButton;
        int id = Convert.ToInt32(adauga.CommandArgument);
        ItemComanda item = comanda.ListaItem[id] as ItemComanda;
        comanda.removeItemComanda(item);
        item.scadeCantitate();
        comanda.addItemComanda(item);
        Session["comanda"] = comanda;
        updateBadge();
        ComandaListView.DataBind();
        showTotal();
    }

    protected void trimitereComandaClick(object sender, EventArgs e)
    {
        Comanda comanda = Session["comanda"] as Comanda;
        if (comanda != null)
        {
            DatabaseFunctions.trimiteComanda(comanda);
            Session["comanda"] = null;
            FormularComanda.Visible = false;
            StatusComanda.Visible = true;
            updateBadge();
        }
        else
            Response.Redirect(Request.RawUrl);
    }

    private void updateBadge()
    {
        Label badge = Master.FindControl("Badge") as Label;
        UpdatePanel update_badge = Master.FindControl("BadgeUpdatePanel") as UpdatePanel;
        Comanda comanda = Session["comanda"] as Comanda;
        if (comanda != null)
            badge.Text = comanda.NumarPreparate + "";
        else
            badge.Text = "0";
        update_badge.Update();

    }

    private void bindComandaListView()
    {
        Comanda comanda_curenta = Session["comanda"] as Comanda;
        if (comanda_curenta != null)
        {
            ComandaListView.DataSource = comanda_curenta.ListaItem.Values;
        }
        ComandaListView.DataBind();
        showTotal();
    }

    private void showTotal()
    {
        Comanda comanda = Session["comanda"] as Comanda;
        if (comanda == null)
            return;
        Total.InnerHtml = "<span class='pull-right' > Total " + comanda.Pret + " RON</span>";
        if (ComandaListView.Items.Any())
        {
            Total.Visible = true;
            TrimitereComanda.Visible = true;
        }
        else
        {
            Total.Visible = false;
            TrimitereComanda.Visible = false;
        }
    }
}