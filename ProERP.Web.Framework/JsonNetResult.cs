using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ProERP.Web.Framework
{
    public class JsonNetResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        /// 
        /// <returns>
        /// The content encoding.
        /// </returns>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// 
        /// <returns>
        /// The type of the content.
        /// </returns>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// 
        /// <returns>
        /// The data.
        /// </returns>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.
        /// </summary>
        /// 
        /// <returns>
        /// A value that indicates whether HTTP GET requests from the client are allowed.
        /// </returns>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of data.
        /// </summary>
        /// 
        /// <returns>
        /// The maximum length of data.
        /// </returns>
        public int? MaxJsonLength { get; set; }

        /// <summary>
        /// Gets or sets the recursion limit.
        /// </summary>
        /// 
        /// <returns>
        /// The recursion limit.
        /// </returns>
        public int? RecursionLimit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.JsonResult"/> class.
        /// </summary>
        public JsonNetResult()
        {
            this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param><exception cref="T:System.ArgumentNullException">The <paramref name="context"/> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JsonRequest_GetNotAllowed");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            var serializedObject = JsonConvert.SerializeObject(Data);
            response.Write(serializedObject);

            //JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            //if (this.MaxJsonLength.HasValue)
            //    scriptSerializer.MaxJsonLength = this.MaxJsonLength.Value;
            //if (this.RecursionLimit.HasValue)
            //    scriptSerializer.RecursionLimit = this.RecursionLimit.Value;
            //response.Write(scriptSerializer.Serialize(this.Data));
        }
    }

    //public class JsonResponse {
    //    public object Data { get; set; }
    //    public string Message { get; set; }
    //    public ResponseStatus Status { get; set; }
    //}

    //public enum ResponseStatus
    //{
    //    Success = 1,
    //    Warning = 2,
    //    Error = 3
    //}
}
