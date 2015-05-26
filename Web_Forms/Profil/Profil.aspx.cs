using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class Web_Forms_Profil_Profil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        User user = Session["user"] as User;
        if (user == null)
        {
            Response.Redirect("../../Web_Forms/User_actions/Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {  
                specifice_ = DatabaseFunctions.getSpecifice();
                EmailLabel.Text = user.Email;
                LastNameTextBox.Text = user.LastName;
                FirstNameTextBox.Text = user.FirstName;
                SpecificCheckBoxList.DataSource = specifice_;
                SpecificCheckBoxList.DataBind();
                ConfirmPasswordTextBox.Text = "";

                if (user.SpecificsList == null)
                    user.Initialize(
                        user.Id,
                        user.Email,
                        user.Password,
                        user.FirstName,
                        user.LastName,
                        user.JoinDate,
                        user.Type,
                        DatabaseFunctions.getSpecificsForUser(user.Id)
                        );
                specificeAlese_ = user.SpecificsList;

                foreach (String specific in user.SpecificsList)
                {
                    foreach (ListItem specificItem in SpecificCheckBoxList.Items)
                    {
                        if (specificItem.Text.Equals(specific))
                        {
                            specificItem.Selected = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    protected void confirmPasswordServerValidate(object source, ServerValidateEventArgs args)
    {
        User user = Session["user"] as User;

        if (args.Value.Length > 3 && args.Value.Length < 21 && sha256(args.Value).Equals(user.Password))
        {
            args.IsValid = true;
            passwordFormGroup.Attributes["class"] = "form-group";
            passwordFormGroup.Controls.Remove(passwordFormGroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            passwordFormGroup.Attributes["class"] = "form-group has-error has-feedback";
            ConfirmPasswordTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            passwordFormGroup.Controls.Add(span);
        }
    }

    protected void updateButtonClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            User user = Session["user"] as User;
            string first_name = (String.IsNullOrEmpty(FirstNameTextBox.Text)) ? null : FirstNameTextBox.Text;
            string last_name = (String.IsNullOrEmpty(LastNameTextBox.Text)) ? null : LastNameTextBox.Text;
            List<string> specifics_list = new List<string>();
            foreach (ListItem specific in SpecificCheckBoxList.Items)
                if (specific.Selected == true)
                    specifics_list.Add(specific.Text);
            if (specifics_list.Count == 0)
                specifics_list = null;
            if (
                !first_name.Equals(user.FirstName) ||
                !last_name.Equals(user.LastName) ||
                (specifics_list != null ? specifics_list.Count : 0) 
                != (user.SpecificsList != null ? user.SpecificsList.Count : 0))
            {
                user.Initialize(
                    user.Id,
                    user.Email,
                    user.Password,
                    first_name,
                    last_name,
                    user.JoinDate,
                    user.Type,
                    specifics_list
                    );
                DatabaseFunctions.updateUser(user);
                Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
            }
        }
    }

    private string sha256(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        foreach (byte bit in crypto)
        {
            hash += bit.ToString("x2");
        }
        return hash;
    }

    private static List<String> specifice_;
    private static List<String> specificeAlese_;
}