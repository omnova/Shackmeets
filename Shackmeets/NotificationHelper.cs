using Shackmeets.Models;
using Shackmeets.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Shackmeets
{
  public class NotificationHelper
  {
    private AppSettings appSettings;
    private IChattyService chattyService;
    
    public NotificationHelper(AppSettings appSettings, IChattyService chattyService)
    {
      this.appSettings = appSettings;
      this.chattyService = chattyService;
    }

    public void SendEmail(string to, string subject, string body)
    {
      var client = new SmtpClient("smtp.shackmeets.com");

      client.UseDefaultCredentials = true;
      //client.Credentials = new NetworkCredential("username", "password");

      var mailMessage = new MailMessage();

      mailMessage.From = new MailAddress("noreply@shackmeets.com");
      mailMessage.To.Add(to);
      mailMessage.Subject = subject;
      mailMessage.Body = body;

      try
      {
        client.Send(mailMessage);
      }
      catch (Exception e)
      {
        
      }
    }

    public void SendEmail2()
    {
     
    }

    public void SendShackMessage(string to, string subject, string body)
    {
      string shackmeetsUsername = this.appSettings.Chatty.Username;
      string shackmeetsPassword = this.appSettings.Chatty.Password;

      if (this.chattyService.SendMessage(shackmeetsUsername, shackmeetsPassword, to, subject, body))
      {

      }
    }

    public string BuildShackmeetAnnouncementPostText(Meet meet)
    {
      string text =
          @"*[b{Shackmeet Announcement - " + meet.Name + "}b]*" + Environment.NewLine
        + Environment.NewLine
        + @"y{" + meet.OrganizerUsername + "}y has suggested a shackmeet for *[" + meet.EventDate.ToShortDateString() + "]* in *[";

      if (!string.IsNullOrEmpty(meet.LocationState))
        text += meet.LocationState + ", " + meet.LocationCountry;
      else
        text += meet.LocationCountry;

      text += @"]*!\" + Environment.NewLine;
      text += Environment.NewLine;
      text += meet.Description + Environment.NewLine;
      text += Environment.NewLine;
      text += "Click here for all the details: " + "";

      return text;
    }

    public string BuildShackmeetReminderPostText(Meet meet)
    {
      string text =
          @"*[g{Shackmeet Reminder - " + meet.Name + "}g]*" + Environment.NewLine
        + Environment.NewLine
        + @"y{" + meet.OrganizerUsername + "}y has suggested a shackmeet for *[" + meet.EventDate.ToShortDateString() + "]* in *[";

      if (!string.IsNullOrEmpty(meet.LocationState))
        text += meet.LocationState + ", " + meet.LocationCountry;
      else
        text += meet.LocationCountry;

      text += @"]*!\" + Environment.NewLine;
      text += Environment.NewLine;
      text += meet.Description + Environment.NewLine;
      text += Environment.NewLine;
      text += "Click here for all the details: " + "";

      return text;
    }

    public string BuildShackmeetAnnouncementNotificationText(Meet meet)
    {
      string text = "Shackmeet Announcement - " + meet.Name + Environment.NewLine + Environment.NewLine;
      text += meet.OrganizerUsername + " has suggested a shackmeet for " + meet.EventDate.ToShortDateString() + " in ";

      if (!string.IsNullOrEmpty(meet.LocationState))
        text += meet.LocationState + ", " + meet.LocationCountry;
      else
        text += meet.LocationCountry;

      text += "!" + Environment.NewLine + Environment.NewLine;
      text += meet.Description + Environment.NewLine + Environment.NewLine;
      text += "Click here for all the details: " + "";

      return text;
    }

    public string BuildShackmeetReminderNotificationText(Meet meet)
    {
      string text = "Shackmeet Reminder - " + meet.Name + Environment.NewLine + Environment.NewLine;
      text += meet.OrganizerUsername + " has suggested a shackmeet for " + meet.EventDate.ToShortDateString() + " in ";

      if (!string.IsNullOrEmpty(meet.LocationState))
        text += meet.LocationState + ", " + meet.LocationCountry;
      else
        text += meet.LocationCountry;

      text += "!" + Environment.NewLine + Environment.NewLine;
      text += meet.Description + Environment.NewLine + Environment.NewLine;
      text += "Click here for all the details: " + "";

      return text;
    }

    public string BuildShackmeetUpdateNotificationText(Meet meet)
    {
      string text = "Shackmeet Updated - " + meet.Name + Environment.NewLine + Environment.NewLine;
      text += meet.OrganizerUsername + " has updated the details of the shackmeet occurring on " + meet.EventDate.ToShortDateString() + " in ";

      if (!string.IsNullOrEmpty(meet.LocationState))
        text += meet.LocationState + ", " + meet.LocationCountry;
      else
        text += meet.LocationCountry;

      text += "!" + Environment.NewLine + Environment.NewLine;
      text += meet.Description + Environment.NewLine + Environment.NewLine;
      text += "Click here for all the details: " + "";

      return text;
    }

    private string StripShackTags(string text)
    {
      return text;
    }

    private string ConvertShackTagsToHtmlTags(string text)
    {
      // Todo: Improve efficiency

      //text = text.Replace(Environment.NewLine, "<br>");

      // Open tags
      text = text.Replace("r{",  "<span class=\"shacktag-red\">");   
      text = text.Replace("g{",  "<span class=\"shacktag-green\">"); 
      text = text.Replace("b{",  "<span class=\"shacktag-blue\">");  
      text = text.Replace("y{",  "<span class=\"shacktag-yellow\">");
      text = text.Replace("l[",  "<span class=\"shacktag-lime\">");  
      text = text.Replace("n[",  "<span class=\"shacktag-orange\">");
      text = text.Replace("p[",  "<span class=\"shacktag-pink\">");  
      text = text.Replace("e[",  "<span class=\"shacktag-olive\">"); 
      text = text.Replace("/[",  "<span class=\"shacktag-italic\">");
      text = text.Replace("b[",  "<span class=\"shacktag-bold\">");  
      text = text.Replace("*[",  "<span class=\"shacktag-bold\">");  
      text = text.Replace("q[",  "<span class=\"shacktag-quote\">"); 
      text = text.Replace("s[",  "<span class=\"shacktag-sample\">");   
      text = text.Replace("_[",  "<span class=\"shacktag-underline\">");
      text = text.Replace("-[",  "<span class=\"shacktag-strike\">");   
      text = text.Replace("o[",  "<span class=\"shacktag-spoiler\">");  
      text = text.Replace("/{{", "<span class=\"shacktag-code\">");

      // Close tags
      text = text.Replace("}r",  "</span>");
      text = text.Replace("}g",  "</span>");
      text = text.Replace("}b",  "</span>");
      text = text.Replace("}y",  "</span>");
      text = text.Replace("]l",  "</span>");
      text = text.Replace("]n",  "</span>");
      text = text.Replace("]p",  "</span>");
      text = text.Replace("]e",  "</span>");
      text = text.Replace("]/",  "</span>");
      text = text.Replace("]b",  "</span>");
      text = text.Replace("]*",  "</span>");
      text = text.Replace("]q",  "</span>");
      text = text.Replace("]s",  "</span>");
      text = text.Replace("]_",  "</span>");
      text = text.Replace("]-",  "</span>");
      text = text.Replace("]o",  "</span>");
      text = text.Replace("}}/", "</span>");

      return text;
    }

    private string RemoveShackTags(string text)
    {
      // Todo: Improve efficiency

      // Open tags
      text = text.Replace("r{",  string.Empty);
      text = text.Replace("g{",  string.Empty);
      text = text.Replace("b{",  string.Empty);
      text = text.Replace("y{",  string.Empty);
      text = text.Replace("l[",  string.Empty);
      text = text.Replace("n[",  string.Empty);
      text = text.Replace("p[",  string.Empty);
      text = text.Replace("e[",  string.Empty);
      text = text.Replace("/[",  string.Empty);
      text = text.Replace("b[",  string.Empty);
      text = text.Replace("*[",  string.Empty);
      text = text.Replace("q[",  string.Empty);
      text = text.Replace("s[",  string.Empty);
      text = text.Replace("_[",  string.Empty);
      text = text.Replace("-[",  string.Empty);
      text = text.Replace("o[",  string.Empty);
      text = text.Replace("/{{", string.Empty);

      // Close tags
      text = text.Replace("}r",  string.Empty);
      text = text.Replace("}g",  string.Empty);
      text = text.Replace("}b",  string.Empty);
      text = text.Replace("}y",  string.Empty);
      text = text.Replace("]l",  string.Empty);
      text = text.Replace("]n",  string.Empty);
      text = text.Replace("]p",  string.Empty);
      text = text.Replace("]e",  string.Empty);
      text = text.Replace("]/",  string.Empty);
      text = text.Replace("]b",  string.Empty);
      text = text.Replace("]*",  string.Empty);
      text = text.Replace("]q",  string.Empty);
      text = text.Replace("]s",  string.Empty);
      text = text.Replace("]_",  string.Empty);
      text = text.Replace("]-",  string.Empty);
      text = text.Replace("]o",  string.Empty);
      text = text.Replace("}}/", string.Empty);

      return text;
    }
  }
}
