using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl contact = (HtmlGenericControl)Master.FindControl("ContactHyperLink");
        contact.Attributes["class"] += "active";
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

    protected void commentsServerValidate(object source, ServerValidateEventArgs args)
    {
        if (args.Value.Length > 9)
        {
            args.IsValid = true;
            commentsformgroup.Attributes["class"] = "form-group";
            commentsformgroup.Controls.Remove(emailformgroup.FindControl("glyphicon"));
        }
        else
        {
            args.IsValid = false;
            commentsformgroup.Attributes["class"] = "form-group has-error has-feedback";
            MessageTextBox.Attributes["aria-describedby"] = "inputError2Status";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "glyphicon glyphicon-remove form-control-feedback";
            span.Attributes["aria-hidden"] = "true";
            span.Attributes["id"] = "glyphicon";
            commentsformgroup.Controls.Add(span);
        }
    }
    protected void sendButtonClick(object sender, EventArgs e)
    {
        Page.Validate();
        if(Page.IsValid)
        {
            MailMessage mail = new MailMessage(EmailTextBox.Text, "ipproiect@gmail.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "smtp.sendgrid.net";
            client.Credentials = new NetworkCredential("azure_58228c3bd80b12380caacb5e297e1f68@azure.com", "ySO2BQz26p4dyk1");
            mail.Subject = "Contact";
            mail.Body = MessageTextBox.Text;
            client.Send(mail);
            emailformgroup.Visible = false;
            commentsformgroup.Visible = false;
            SendButton.Visible = false;
            feedbackMessage.Visible = true;
        }
    }
}