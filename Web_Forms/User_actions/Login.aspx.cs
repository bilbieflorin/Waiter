using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using db_mapping;

public partial class Web_Forms_User_actions_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        status.Visible = false;
        if (Session["error"] != null)
        {
            errormessage.InnerText = Session["error"].ToString();
            status.Visible = true;
            Session["error"] = null;
        }
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
        if (args.Value.Length > 0)
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

    protected void loginButtonClick(object sender, EventArgs e)
    {
        Page.Validate();
        if (Page.IsValid)
        {
            User user = DatabaseFunctions.checkEmailAndPasswordIfExists(EmailTextBox.Text,sha256(PasswordTextBox.Text));
            if (user.isInitialized())
            {
                Session["user"] = user;
                Response.Redirect("../../Web_Forms/Master/Waiter.aspx");
            }
            else
                status.Visible = true;
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
}