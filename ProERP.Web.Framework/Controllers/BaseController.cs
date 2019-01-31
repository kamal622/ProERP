using ProERP.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace ProERP.Web.Framework.Controllers
{
    public abstract class BaseController : Controller
    {
        public enum NotifyType
        {
            Info,
            Warning,
            Success,
            Error
        }

        protected BaseController()
        {

        }

        #region Log Exception
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        protected void LogException(Exception exc)
        {
            //var workContext = EngineContext.Current.Resolve<IWorkContext>();
            //var logger = EngineContext.Current.Resolve<ILogger>();

            //var customer = workContext.CurrentUser;
            //logger.Error(exc.Message, exc);
        }

        #endregion

        #region Notifications

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("ace.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        #region JsonNetResult

        protected internal JsonNetResult JsonNet(object data, JsonRequestBehavior behavior)
        {
            return JsonNet(data, null /* contentType */, null /* contentEncoding */, behavior);
        }

        protected internal virtual JsonNetResult JsonNet(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        #endregion

        #region Send Email

        protected void SendEmail(string[] toEmails, string subject, string[] ccEmails, string body, Stream[] attachments, string[] attachmentFileName)
        {
            using (var client = new SmtpClient())
            {
                using (var email = new MailMessage())
                {
                    email.To.Add(string.Join(",", toEmails));
                    if (ccEmails != null)
                        email.Bcc.Add(string.Join(",", ccEmails));
                    email.Subject = subject;
                    email.Body = body;
                    email.IsBodyHtml = true;
                    if (attachments != null)
                    {
                        for (int i = 0; i < attachments.Length; i++)
                        {
                            var attachment = attachments[i];
                            email.Attachments.Add(new Attachment(attachment, attachmentFileName[i]));
                        }
                    }
                    client.Send(email);
                }
            }
        }

        #endregion

        public string ProcessException(Exception ex)
        {
            string allMessages = ex.GetAllMessages();
            return string.Format("<b>Message: </b><br />{0}<br /><b>Details: </b><br />{1}", allMessages.Replace(Environment.NewLine, "<br />"), ex.StackTrace.Replace(Environment.NewLine, "<br />"));
        }
    }
}
