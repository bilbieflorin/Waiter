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

        if (!IsPostBack)
        {
            potriviri_ = new Hashtable();

            Comanda comanda = Session["comanda"] as Comanda;
            if (comanda != null)
            {
                potriviriComandaCarousel.Visible = true;

                List<Preparat> preparate = comanda.ListaItem.Values.Cast<ItemComanda>().Select(itemComanda => itemComanda.Preparat).ToList();
               
                IEnumerator<Preparat> enumeratorPreparate = preparate.GetEnumerator();
                
                foreach (Preparat preparat in preparate)
                {
                    List<Preparat> potriviri = DatabaseFunctions.getPotriviriPreparat(preparat);
                    foreach (Preparat potrivire in potriviri)
                    {
                        bool potrivire_valida = true;

                        enumeratorPreparate.Reset();
                        while (potrivire_valida && enumeratorPreparate.MoveNext())
                        {
                            if (enumeratorPreparate.Current.Tip.Equals(potrivire.Tip))
                                potrivire_valida = false;
                        }

                        if (potrivire_valida && !potriviri_.ContainsKey(potrivire.Id))
                            potriviri_.Add(potrivire.Id, potrivire);
                    }
                    
                }

                carouselRepeater.DataSource = potriviri_.Values;
                carouselRepeater.DataBind();
            }
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

    protected void carouselItemImageClick(object sender, EventArgs e)
    {
        LinkButton target_image = sender as LinkButton;

        if (target_image.CommandName.Equals("Id"))
        {
            int id_preparat = Convert.ToInt32(target_image.CommandArgument);

            Preparat preparat = potriviri_[id_preparat] as Preparat;

            ModalItemTitle.Text = preparat.Denumire;
            ModalItemImage.ImageUrl = preparat.PathImagine;
            string ingrediente = " ";

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
            ButtonComanda.CommandArgument = id_preparat + "";
            ModalUpdatePanel.Update();
        }
    }

    protected void buttonComandaClick(object sender, EventArgs e)
    {
        if (Session["user"] == null)
        {
            Session["error"] = "Trebuie sa fi autentificat pentru a putea comanda";
            Response.Redirect("../../Web_Forms/User_actions/Login.aspx");
        }
        else
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
            int id_preparat = Convert.ToInt32(order_button.CommandArgument);
            ItemComanda item = new ItemComanda(potriviri_[id_preparat] as Preparat, 1);
            comanda.addItemComanda(item);
            Session["comanda"] = comanda;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal('hide')", true);
            Label badge = Master.FindControl("Badge") as Label;
            UpdatePanel update_badge = Master.FindControl("BadgeUpdatePanel") as UpdatePanel;
            badge.Text = comanda.NumarPreparate + "";
            update_badge.Update();

            bindComandaListView();
        }
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

    private static Hashtable potriviri_;
}