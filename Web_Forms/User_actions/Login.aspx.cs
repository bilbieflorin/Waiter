using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Web_Forms_User_actions_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void emailServerValidate(object source, ServerValidateEventArgs args)
    {
        Regex email_regular_expression = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        if (email_regular_expression.IsMatch(args.Value) && args.Value.Length > 0)
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
}