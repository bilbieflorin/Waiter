using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class Web_Forms_Admin_Admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        connection_string_ = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        User user = Session["user"] as User;
        if (user == null || !user.Type.Equals("ADMIN"))
        {
            Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
        }

        if (!IsPostBack)
        {
            ingrediente_ = DatabaseFunctions.getIngrediente(connection_string_);

            ingredienteAdaugate_ = new List<Ingredient>();
            denumiriIngrediente_ = new List<String>();
            denumiriIngrediente_.Add("Ingredient nou");
            foreach (Ingredient ingredient in ingrediente_)
            {
                denumiriIngrediente_.Add(ingredient.Denumire);
            }

            IngredienteDropDownList.DataSource = denumiriIngrediente_;
            IngredienteDropDownList.DataBind();
        }

        IngredienteListView.DataSource = ingredienteAdaugate_;
        IngredienteListView.DataBind();

        IngredientNouTextBox.Visible = IngredienteDropDownList.SelectedIndex == 0;
    }

    protected void adaugaIngredientButtonClick(object sender, EventArgs e)
    {
        int indexIngredient = IngredienteDropDownList.SelectedIndex;
        if (indexIngredient == 0)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.Initialize(-100000, IngredientNouTextBox.Text);
            ingredienteAdaugate_.Add(ingredient);
            IngredientNouTextBox.Text = "";
        }
        else
        {
            indexIngredient -= 1; // balance +1 from "Ingrediente nou"
            ingredienteAdaugate_.Add(ingrediente_[indexIngredient]);
        }
        IngredienteListView.DataBind();
        IngredienteUpdatePanel.Update();
    }

    protected void denumireValidate(object source, ServerValidateEventArgs args)
    {
        if (!args.Value.Equals(""))
        {
            args.IsValid = true;
            denumireFormGroup.Attributes["class"] = "form-group";
            denumireFormGroup.Controls.Remove(denumireFormGroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            denumireFormGroup.Attributes["class"] = "form-group has-error has-feedback";
            DenumirePreparatTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            denumireFormGroup.Controls.Add(span);
        }
    }

    protected void tipValidate(object source, ServerValidateEventArgs args)
    {
        if (!args.Value.Equals(""))
        {
            args.IsValid = true;
            tipFormGroup.Attributes["class"] = "form-group";
            tipFormGroup.Controls.Remove(tipFormGroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            tipFormGroup.Attributes["class"] = "form-group has-error has-feedback";
            TipPreparatTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            tipFormGroup.Controls.Add(span);
        }
    }

    protected void gramajValidate(object source, ServerValidateEventArgs args)
    {
        bool gramajValid = true;
        double gramaj;

        if (args.Value.Length > 0)
        {
            try
            {
                gramaj = Convert.ToDouble(args.Value);
                if (gramaj <= 0)
                {
                    gramajValid = false;
                }
            }
            catch (Exception e)
            {
                gramajValid = false;
            }
        }
        else
        {
            gramajValid = false;
        }

        if (gramajValid)
        {
            args.IsValid = true;
            gramajFormGroup.Attributes["class"] = "form-group";
            gramajFormGroup.Controls.Remove(gramajFormGroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            gramajFormGroup.Attributes["class"] = "form-group has-error has-feedback";
            TipPreparatTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            gramajFormGroup.Controls.Add(span);
        }
    }

    protected void pretValidate(object source, ServerValidateEventArgs args)
    {
        bool pretValid = true;
        double pret;

        if (args.Value.Length > 0)
        {
            try
            {
                pret = Convert.ToDouble(args.Value);
                if (pret <= 0)
                {
                    pretValid = false;
                }
            }
            catch (Exception e)
            {
                pretValid = false;
            }
        }
        else
        {
            pretValid = false;
        }

        if (pretValid)
        {
            args.IsValid = true;
            pretFormGroup.Attributes["class"] = "form-group";
            pretFormGroup.Controls.Remove(pretFormGroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            pretFormGroup.Attributes["class"] = "form-group has-error has-feedback";
            TipPreparatTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            pretFormGroup.Controls.Add(span);
        }
    }

    protected void specificValidate(object source, ServerValidateEventArgs args)
    {
        if (RadioButtonList1.SelectedIndex == -1)
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void adaugaButtonClick(object sender, EventArgs e)
    {
        Page.Validate();
        SpecificValidator.Validate();
        if (Page.IsValid && SpecificValidator.IsValid)
        {
            String pathImagine = "../../Images/" + UploadPoza.FileName;
            UploadPoza.SaveAs(Server.MapPath(pathImagine));

            Preparat preparat = new Preparat();
            String denumire = DenumirePreparatTextBox.Text;
            String tip = TipPreparatTextBox.Text;
            double pret = Convert.ToDouble(PretTextBox.Text);
            double gramaj = Convert.ToDouble(GramajTextBox.Text);
            String specific = RadioButtonList1.SelectedValue;
            preparat.Initialize(
                -100000,
                denumire,
                tip,
                pret,
                pathImagine,
                gramaj,
                specific,
                null,
                DateTime.Now);
            int id = DatabaseFunctions.insertPreparat(preparat, connection_string_);

            foreach (Ingredient ingredient in ingredienteAdaugate_)
            {
                if (ingredient.IsNew)
                {
                    int id_ingredient = DatabaseFunctions.insertIngredient(ingredient, connection_string_);
                    ingredient.Initialize(id_ingredient, ingredient.Denumire);
                }

                DatabaseFunctions.insertPreparatContineIngredient(id, ingredient.Id, connection_string_);
            }

            Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
        }
        
    }

    private String connection_string_;
    private static List<Ingredient> ingrediente_;
    private static List<String> denumiriIngrediente_;
    private static List<Ingredient> ingredienteAdaugate_;
}