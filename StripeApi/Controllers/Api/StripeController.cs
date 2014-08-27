using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        [HttpPost]
        [Route("api/stripe/createcustomer")]
        public string CreateCustomerObject(CustomerObjectRequest cardTokenRequest)
        {
            var customerCreateOptions = new StripeCustomerCreateOptions();
            customerCreateOptions.TokenId = cardTokenRequest.CardToken;

            //these are all optional
            customerCreateOptions.Email = "test@opentable.com";
            customerCreateOptions.Description = "I am a tasty customer";
            var metaData = new Dictionary<string, string>();
            metaData.Add("opentableId", "123456");
            customerCreateOptions.Metadata = metaData;

            var customerService = new StripeCustomerService(ConfigurationManager.AppSettings["masterMerchantSecretkey"]);
            try
            {
                StripeCustomer createdCustomer = customerService.Create(customerCreateOptions);
                return createdCustomer.Id;
            }
            catch(StripeException sx)
            {
                //do some logging
                
            }

            return "";
        }

        [HttpPost]
        [Route("api/stripe/makepayment")]
        public string MakePayment(PaymentRequest paymentRequest)
        {
            var chargeOptions = new StripeChargeCreateOptions();
            chargeOptions.Amount = (int)(paymentRequest.Amount * 100);
            chargeOptions.Currency = "GBP";
            chargeOptions.CustomerId = paymentRequest.CustomerToken;

            //optional properties
            chargeOptions.Description = "tasty meal";
            var metaData = new Dictionary<string, string>();
            metaData.Add("opentableBookingId", "998998924");
            chargeOptions.Metadata= metaData;

            var chargeService = new StripeChargeService(ConfigurationManager.AppSettings["masterMerchantSecretkey"]);

            try
            {
                StripeCharge stripeCharge = chargeService.Create(chargeOptions);
                return stripeCharge.Id;
            }
            catch (StripeException sx)
            {
                //do some logging
            }

            return "";
        }
    }
}
