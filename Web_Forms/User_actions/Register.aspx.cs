using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class Web_Forms_User_actions_Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        connection_string_ = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        if (IsPostBack)
        {
            PasswordTextBox.Attributes.Add("value", PasswordTextBox.Text);
        }

    }
    
    protected void emailServerValidate(object source, ServerValidateEventArgs args)
    {
        Regex email_regular_expression = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        if (email_regular_expression.IsMatch(args.Value) && args.Value.Length > 0 && !DatabaseFunctions.checkEmailIfExist(args.Value,connection_string_))
        {
            args.IsValid = true;
            emailformgroup.Attributes["class"] = "form-group";
            emailformgroup.Controls.Remove(emailformgroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            emailformgroup.Attributes["class"] = "form-group has-error has-feedback";
            EmailTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            emailformgroup.Controls.Add(span);
        }

    }
    
    protected void passwordServerValidate(object source, ServerValidateEventArgs args)
    {
        if (args.Value.Length > 3 && args.Value.Length < 20)
        {
            args.IsValid = true;
            passwordformgroup.Attributes["class"] = "form-group";
            passwordformgroup.Controls.Remove(emailformgroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            passwordformgroup.Attributes["class"] = "form-group has-error has-feedback";
            PasswordTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            passwordformgroup.Controls.Add(span);
        }
    }
   
    protected void confirmPasswordServerValidate(object source, ServerValidateEventArgs args)
    {
        if (args.Value.Length > 3 && args.Value.Length < 21 && args.Value == PasswordTextBox.Text)
        {
            args.IsValid = true;
            confirmpasswordformgroup.Attributes["class"] = "form-group";
            confirmpasswordformgroup.Controls.Remove(emailformgroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            confirmpasswordformgroup.Attributes["class"] = "form-group has-error has-feedback";
            PasswordTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            confirmpasswordformgroup.Controls.Add(span);
        }
    }

    protected void registerButtonClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            User user = new User();
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string first_name = (String.IsNullOrEmpty(FirstNameTextBox.Text)) ? null : FirstNameTextBox.Text;
            string last_name = (String.IsNullOrEmpty(LastNameTextBox.Text)) ? null : LastNameTextBox.Text;
            List<string> specifics_list = new List<string>();
            foreach (ListItem specific in SpecificCheckBoxList.Items)
                if (specific.Selected == true)
                    specifics_list.Add(specific.Text);
            if (specifics_list.Count == 0)
                specifics_list = null;
            user.Initialize(-100000, email, password, first_name, last_name, DateTime.Now, specifics_list);
            DatabaseFunctions.insertUser(user, connection_string_);
            Response.Redirect("../../WebForms/Master/Waiter.aspx");
        }
    }

    private string connection_string_;
}