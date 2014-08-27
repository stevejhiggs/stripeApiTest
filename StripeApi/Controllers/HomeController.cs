using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StripeApi.Controllers
{
    public class IndexResponseViewModel
    {
        public string MerchantToken { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index(string merchantToken = "")
        {
            ViewBag.Title = "Home Page";

            return View(new IndexResponseViewModel {  MerchantToken = merchantToken });
        }

        public ActionResult PostConnect(string code)
        {
            var stripeService = new StripeOAuthTokenService(ConfigurationManager.AppSettings["masterMerchantSecretkey"]);
            var stripeTokenOptions = new StripeOAuthTokenCreateOptions() { Code = code, GrantType = "authorization_code", Scope="read_write" };
            StripeOAuthToken response = stripeService.Create(stripeTokenOptions);

            //note, you would actually want to store these details securely against the new merchant, pass back the merchant id and look it up later
            return Redirect("/home/index?merchantToken=" + response.AccessToken);
        }
    }
}
