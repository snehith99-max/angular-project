using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ems.subscription.Models
{
    public class MdlCustomerRegister : result
    {
        public List<register_list> register_list { get; set; }

    }

    public class register_list:result
    {
        public string company { get; set; }
        public string contact_person { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string gst_number { get; set; }
        public string address { get; set; }
        public string company_code { get; set; }
        public string fname { get; set; }
        public string company_id { get; set; }
        public string branch { get; set; }
        public string phone_shopicart { get; set; }
        public string signup_title { get; set; }
        public string order_id { get; set; }
        public string payment_link { get; set; }
        public string register_gid { get; set; }
        public string c_code { get; set; }
        public string code { get; set; }


    }
    public class mdlUpdatePayment : result
    {
        public string company_code { get; set; }
        public string c_code { get; set; }
        public string payment_flag { get; set; }
        public string amount { get; set; }
        public string company_name { get; set; }
        public string package { get; set; }
        public string currency { get; set; }
        public string order_id { get; set; }
        public string company { get; set; }
        public string contact_person { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string gst_number { get; set; }
        public string address { get; set; }
        public string fname { get; set; }
        public string company_id { get; set; }
        public string branch { get; set; }
        public string phone_shopicart { get; set; }
        public string signup_title { get; set; }
        public string payment_id { get; set; }
        public string payment_intent_id { get; set; }
        public string client_secret { get; set; }
        public string payment_link_url { get; set; }


    }
    public class MdlStripePaymentLink
    {
        public string id { get; set; }
        public string _object { get; set; }
        public bool active { get; set; }
        public After_Completion after_completion { get; set; }
        public bool allow_promotion_codes { get; set; }
        public object application { get; set; }
        public object application_fee_amount { get; set; }
        public object application_fee_percent { get; set; }
        public Automatic_Tax automatic_tax { get; set; }
        public string billing_address_collection { get; set; }
        public object consent_collection { get; set; }
        public string currency { get; set; }
        public object[] custom_fields { get; set; }
        public Custom_Text custom_text { get; set; }
        public string customer_creation { get; set; }
        public object inactive_message { get; set; }
        public bool livemode { get; set; }
        public object on_behalf_of { get; set; }
        public object payment_intent_data { get; set; }
        public string payment_method_collection { get; set; }
        public object payment_method_types { get; set; }
        public object restrictions { get; set; }
        public object shipping_address_collection { get; set; }
        public object[] shipping_options { get; set; }
        public string submit_type { get; set; }
        public object subscription_data { get; set; }
        public object transfer_data { get; set; }
        public string url { get; set; }
    }
    public class After_Completion
    {
        public Hosted_Confirmation hosted_confirmation { get; set; }
        public string type { get; set; }
    }
    public class Hosted_Confirmation
    {
        public object custom_message { get; set; }
    }
    public class Automatic_Tax
    {
        public bool enabled { get; set; }
        public object liability { get; set; }
    }
    public class Custom_Text
    {
        public object after_submit { get; set; }
        public object shipping_address { get; set; }
        public object submit { get; set; }
        public object terms_of_service_acceptance { get; set; }
    }
    public class mdlDynamicDBCreation : result
    {
        public string otp { get; set; }
    }
    public class customerotpverifyresponse
    {
        public string otp_value { get; set; }
        public bool status { get; set; }
        public string c_code { get; set; }

    }

    public class customerotpverify
    {
        //internal string mobile_number;
        public string message { get; set; }
        public string otp_value { get; set; }
        public bool status { get; set; }

        public string OTP_verify { get; set; }

    }

}