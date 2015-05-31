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

        //// Potriviri additions
        if (!IsPostBack)
        {
            potriviri_ = new Hashtable();
            potriviriKeyedByPreparate_ = new Hashtable();
            potriviriPreparateCache_ = new Hashtable();

            Comanda comanda = Session["comanda"] as Comanda;
            if (comanda != null && comanda.ListaItem.Count > 0)
            {
                potriviriDiv.Visible = true;

                List<Preparat> preparate = comanda.ListaItem.Values.Cast<ItemComanda>().Select(itemComanda => itemComanda.Preparat).ToList();
               
                IEnumerator<Preparat> preparate_enumerator = preparate.GetEnumerator();
                
                foreach (Preparat preparat in preparate)
                {
                    List<Preparat> potriviri = DatabaseFunctions.getPotriviriPreparat(preparat);
                    foreach (Preparat potrivire in potriviri)
                    {
                        bool potrivire_valida = true;

                        preparate_enumerator.Reset();
                        while (preparate_enumerator.MoveNext())
                        {
                            if (preparate_enumerator.Current.Id == potrivire.Id)
                            {
                                potrivire_valida = false;
                                break;
                            }

                            if (preparate_enumerator.Current.Tip.Equals(potrivire.Tip))
                            {
                                potrivire_valida = false;
                                break;
                            }
                        }

                        if (potrivire_valida)
                        {
                            if (!potriviri_.ContainsKey(potrivire.Id))
                                potriviri_.Add(potrivire.Id, potrivire);

                            List<int> id_potriviri = null;
                            if (potriviriKeyedByPreparate_.ContainsKey(preparat.Id))
                            {
                                id_potriviri = potriviriKeyedByPreparate_[preparat.Id] as List<int>;
                                id_potriviri.Add(potrivire.Id);
                            }
                            else
                            {
                                id_potriviri = new List<int>();
                                id_potriviri.Add(potrivire.Id);
                                potriviriKeyedByPreparate_[preparat.Id] = id_potriviri;
                            }
                        }
                    }
                    potriviriPreparateCache_.Add(preparat.Id, potriviri);
                }

                carouselRepeater.DataSource = potriviri_.Values;
                carouselRepeater.DataBind();
                if (carouselRepeater.Items.Count == 0)
                    potriviriDiv.Visible = false;
            }
        }
        ////
    }

    protected void deleteItemComanda(object sender, EventArgs e)
    {
        LinkButton remove_Item = sender as LinkButton;
        int id = Convert.ToInt32(remove_Item.CommandArgument);
        Comanda comanda = Session["comanda"] as Comanda;
        comanda.removeItemComanda(comanda.ListaItem[id] as ItemComanda);
        updateBadge();
        ComandaListView.DataBind();

        //// Potriviri additions
        potriviri_.Clear();
        potriviriKeyedByPreparate_.Clear();

        List<Preparat> preparate = comanda.ListaItem.Values.Cast<ItemComanda>().Select(itemComanda => itemComanda.Preparat).ToList();

        IEnumerator<Preparat> preparate_enumerator = preparate.GetEnumerator();

        foreach (Preparat preparat in preparate)
        {
            List<Preparat> potriviri = null;
            if (potriviriPreparateCache_.ContainsKey(preparat.Id))
                potriviri = potriviriPreparateCache_[preparat.Id] as List<Preparat>;
            else
            {
                potriviri = DatabaseFunctions.getPotriviriPreparat(preparat);
                potriviriPreparateCache_.Add(preparat.Id, potriviri);
            }

            foreach (Preparat potrivire in potriviri)
            {
                bool potrivire_valida = true;

                preparate_enumerator.Reset();
                while (preparate_enumerator.MoveNext())
                {
                    if (preparate_enumerator.Current.Id == potrivire.Id)
                    {
                        potrivire_valida = false;
                        break;
                    }

                    if (preparate_enumerator.Current.Tip.Equals(potrivire.Tip))
                    {
                        potrivire_valida = false;
                        break;
                    }
                }

                if (potrivire_valida)
                {
                    if (!potriviri_.ContainsKey(potrivire.Id))
                        potriviri_.Add(potrivire.Id, potrivire);

                    List<int> id_potriviri = null;
                    if (potriviriKeyedByPreparate_.ContainsKey(preparat.Id))
                    {
                        id_potriviri = potriviriKeyedByPreparate_[preparat.Id] as List<int>;
                        id_potriviri.Add(potrivire.Id);
                    }
                    else
                    {
                        id_potriviri = new List<int>();
                        id_potriviri.Add(potrivire.Id);
                        potriviriKeyedByPreparate_[preparat.Id] = id_potriviri;
                    }
                }
            }
        }

        carouselRepeater.DataSource = potriviri_.Values;
        carouselRepeater.DataBind();
        if (carouselRepeater.Items.Count == 0)
            potriviriDiv.Visible = false;
        else
            potriviriDiv.Visible = true;

        // Cod vechi: tine in viata potrivirile comune cu alte preparate insa nu am luat in calcul ca 
        // daca nu mai sunt potriviri, la stergere de preparat e posibil sa se revalideze unele potriviri..
        // ..asa ca am tuflit din nou toata logica (vezi mai sus)
        // lasat pentru verificari teste
        /*
        if (potriviriKeyedByPreparate_ != null && potriviriKeyedByPreparate_.ContainsKey(id))
        {
            List<int> id_potriviri_item_sters = potriviriKeyedByPreparate_[id] as List<int>;
            foreach (int id_potrivire_candidata_la_sters in id_potriviri_item_sters)
            {
                bool potrivire_valida = false;

                foreach (int id_preparat in potriviriKeyedByPreparate_.Keys)
                {
                    if (id_preparat == id)
                        continue;

                    List<int> id_potriviri = potriviriKeyedByPreparate_[id_preparat] as List<int>;
                    foreach (int id_potrivire in id_potriviri)
                    {
                        if (id_potrivire == id_potrivire_candidata_la_sters)
                        {
                            potrivire_valida = true;
                            break;
                        }
                    }

                    if (potrivire_valida)
                        break;
                }

                if (!potrivire_valida)
                    potriviri_.Remove(id_potrivire_candidata_la_sters);
            }

            potriviriKeyedByPreparate_.Remove(id);

            carouselRepeater.DataSource = potriviri_.Values;
            carouselRepeater.DataBind();
            if (carouselRepeater.Items.Count == 0)
                potriviriDiv.Visible = false;
        }
         */

        if (potriviriPreparateCache_ != null && potriviriPreparateCache_.ContainsKey(id))
            potriviriPreparateCache_.Remove(id);
        ////

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

            //// Potriviri additions
            potriviri_.Clear();
            potriviriKeyedByPreparate_.Clear();

            List<Preparat> preparate = comanda.ListaItem.Values.Cast<ItemComanda>().Select(itemComanda => itemComanda.Preparat).ToList();

            IEnumerator<Preparat> preparate_enumerator = preparate.GetEnumerator();

            foreach (Preparat preparat in preparate)
            {
                List<Preparat> potriviri = null;
                if (potriviriPreparateCache_.ContainsKey(preparat.Id))
                    potriviri = potriviriPreparateCache_[preparat.Id] as List<Preparat>;
                else
                {
                    potriviri = DatabaseFunctions.getPotriviriPreparat(preparat);
                    potriviriPreparateCache_.Add(preparat.Id, potriviri);
                }

                foreach (Preparat potrivire in potriviri)
                {
                    bool potrivire_valida = true;

                    preparate_enumerator.Reset();
                    while (preparate_enumerator.MoveNext())
                    {
                        if (preparate_enumerator.Current.Id == potrivire.Id)
                        {
                            potrivire_valida = false;
                            break;
                        }

                        if (preparate_enumerator.Current.Tip.Equals(potrivire.Tip))
                        {
                            potrivire_valida = false;
                            break;
                        }
                    }

                    if (potrivire_valida)
                    {
                        if (!potriviri_.ContainsKey(potrivire.Id))
                            potriviri_.Add(potrivire.Id, potrivire);

                        List<int> id_potriviri = null;
                        if (potriviriKeyedByPreparate_.ContainsKey(preparat.Id))
                        {
                            id_potriviri = potriviriKeyedByPreparate_[preparat.Id] as List<int>;
                            id_potriviri.Add(potrivire.Id);
                        }
                        else
                        {
                            id_potriviri = new List<int>();
                            id_potriviri.Add(potrivire.Id);
                            potriviriKeyedByPreparate_[preparat.Id] = id_potriviri;
                        }
                    }
                }
            }

            carouselRepeater.DataSource = potriviri_.Values;
            carouselRepeater.DataBind();
            if (carouselRepeater.Items.Count == 0)
                potriviriDiv.Visible = false;
            ////

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

    // Variabilele au fost adaugate in functie de cum am avut nevoie.
    // E posibil sa nu mai fie toate necesare avand in vedere codul comentat de la stergere preaprat (vezi ami sus).
    private static Hashtable potriviri_; // key: id_preparat (id potrivire) value: preparat (potrivirea)
    private static Hashtable potriviriKeyedByPreparate_; // key: id_preparat value: id_potriviri
    private static Hashtable potriviriPreparateCache_; // key: id_preparat value: preparate (potriviri)
}