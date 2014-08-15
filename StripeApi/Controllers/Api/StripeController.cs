using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace StripeApi.Controllers.Api
{
    

    public class CustomerObjectRequest
    {
        public string CardToken { get; set; }
    }

    public class PaymentRequest
    {
        public string CustomerToken { get; set; }
        public decimal Amount { get; set; }
    }


    public class StripeController : ApiController
    {
        private string stripeMerchantSecretKey = "sk_test_mhw8Qvphf7iYcUnj3XrVWMky";

        [HttpPost]
        [Route("api/stripe/createcustomer")]
        public string CreateCustomerObject(CustomerObjectRequest cardTokenRequest)
        {
            var api = new StripeClient(stripeMerchantSecretKey);
            //var card = new 
            dynamic result = api.CreateCustomer(new Stripe.CreditCardToken(cardTokenRequest.CardToken));
            if (!result.IsError)
            {
                return result.Id;
            }
            return "";
        }

        [HttpPost]
        [Route("api/stripe/makepayment")]
        public string MakePayment(PaymentRequest paymentRequest)
        {
            var api = new StripeClient(stripeMerchantSecretKey);
            //var card = new 
            dynamic result = api.CreateCharge(paymentRequest.Amount, "GBP", paymentRequest.CustomerToken, "Its for sundries and arbitrary bribes");
            if (!result.IsError && result.Paid)
            {
                return result.Id;
            }
            return "";
        }
    }
}
