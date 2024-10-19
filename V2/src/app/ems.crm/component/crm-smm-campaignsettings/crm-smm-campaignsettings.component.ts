import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { SelectionModel } from '@angular/cdk/collections';
import { param } from 'jquery';

export class IAssign1 {

  emailaddress: any;
  untaglist: any;


}
interface ICampaingService {
  workspace_id: string;
  whatsapp_accesstoken: string;
  channel_id: string;
  mobile_number: string;
  channel_name: string;
  access_token_edit: string;
  base_url_edit: string;
  whatsapp_id: string;
  channelgroup_id: string
}
interface IShopifyService {
  shopify_accesstoken: string;
  shopify_store_name: string;
  store_month_year: string;
  shopify_accesstokenedit: string;
  shopify_store_nameedit: string;
  store_month_yearedit: string;
  shopify_status: string;
}
interface IEmailService {
  mail_access_token: string;
  mail_base_url: string;
  email_id: string;
  receiving_domain: string;
  sending_domain: string;
  email_username: string;
  email_status: string;

}
interface IGEmailService {
  client_id: string;
  client_secret: string;
  refresh_token: string;
  gmail_address: string;
  gmail_status: string;
  email_id: string;



}
interface IGmailService {
  gclient_id: string;
  gclient_secret: string;
  grefresh_token: string;
  ggmail_address: string;
  ggmail_status: string;
  gemail_id: string;



}
interface IOutlookService {
  outlook_client_id: string;
  outlook_client_secret: string;
  tenant_id: string;
  gmail_status: string;
}
interface IFacebookService {
  facebook_access_token: string;
  facebook_page_id: string;
  facebook_id: string;
  facebook_status: string;


}
interface IInstagramService {
  instagram_access_token: string;
  instagram_id: string;
  instagram_status: string;
  instagram_account_id: string;

}
interface ILinkedinService {
  linkedin_access_token: string;
  linkedin_id: string;
  linkedin_status: string;
}
interface ITelegramService {
  bot_id: string;
  chat_id: string;
  telegram_id: string;
  telegram_status: string;

}
interface ILivechatService {
  livechat_access_token: string;
  livechat_id: string;
  livechat_agentid: string;
  livechat_status: string;

}
interface IClicktocallService {
  clicktocall_access_token: string;
  clicktocall_baseurl: string;
  clicktocall_id: string;
  clicktocall_status: string;

}
interface IGoogleanalyticsService {
  user_url: string;
  page_url: string;
  googleanalytics_id: string;
  googleanalytics_status: string;

}
interface ISmsService {
  sms_user_id: string;
  sms_password: string;
  sms_id: string;
  sms_status: string;
}
interface IIndiaMARTService {
  api_key: string;
  indiamart_id: string;
  indiamart_status: string;

}
interface ICalendarService {
  api_key: string;
  calendar_id: string;
  active_flag: string;

}
interface IPaymentService {
  key1: string;
  key2: string;
  key3: string;
  payment_gateway: string;

}
interface IMintsoftService {
  api_key: string;
  mintsoft_id: string;
  base_url: string;
  mintsoft_status: string;
}
interface IEinvoice {
  einvoiceAutenticationURL: string;
  einvoice_id: string;
  einvoiceIRNGenerate: string;
  gspappid: string;
  gspappsecret: string;
  einvoiceuser_name: string;
  einvoicepwd: string;
  einvoicegstin: string;
  einvoice_Auth: string;
  generateQRURL: string;
  cancleIRN: string;
  einvoice_flag: string;
  einvoice_status: string;
}

export class IAssign {

  emailaddress: any;
  taglist: any;


}
@Component({
  selector: 'app-crm-smm-campaignsettings',
  templateUrl: './crm-smm-campaignsettings.component.html',
  styleUrls: ['./crm-smm-campaignsettings.component.scss']
})
export class CrmSmmCampaignsettingsComponent {
  file!: File;
  services: any;
  parameterValue: any;
  leadbank_list: any[] = [];
  parameterValue1: any;
  reactiveFormFacebook!: FormGroup;
  reactiveFormInstagram!: FormGroup;
  reactiveFormShopify!: FormGroup;
  reactiveForm: any;
  reactiveFormEmail!: FormGroup;
  emailaddress: any;
  reactiveFormWhatsapp!: FormGroup;
  pick: Array<any> = [];
  reactiveFormLinkedin!: FormGroup;
  reactiveFormTelegram!: FormGroup;
  reactiveFormGmail!: FormGroup;
  reactiveformrazorpay!: FormGroup;
  reactiveFormcalendar!: FormGroup
  reactiveFormlivechat!: FormGroup;
  reactiveFormmintsoft!: FormGroup;
  reactiveFormCompany!: FormGroup;
  reactiveFormeinvoice!: FormGroup;
  reactiveFormclicktocall!: FormGroup;
  reactiveFormGEmail!: FormGroup;
  reactiveFormOutlook!: FormGroup;
  useGmail: boolean = true;
  reactiveFormCustomerType!: FormGroup
  campaignserv_list: any[] = [];
  campaignser_list: any[] = [];
  unassigncustomer_list: any[] = [];
  mintsoft_list: any[] = [];
  einvoice_list: any[] = [];
  facebookcampaignservicelist: any[] = [];
  instagramcampaignservicelist: any[] = [];
  customertype_list: any[] = [];
  campaignservice_list: any[] = [];
  shopifycampaignservice_list: any[] = [];
  CurObj: IAssign = new IAssign();
  CurObj1: IAssign1 = new IAssign1();
  mailtemplateview_list: any;
  access_token: any;
  EmailService!: IEmailService;
  responsedata: any;
  GEmailService!: IGEmailService;
  Mintsoft!: IMintsoftService;
  Einvoice!: IEinvoice;
  OutlookService!: IOutlookService;
  CampaingService!: ICampaingService;
  ShopifyService!: IShopifyService;
  FacebookService!: IFacebookService;
  InstagramService!: IInstagramService;
  LinkedinService!: ILinkedinService;
  TelegramService!: ITelegramService;
  LivechatService!: ILivechatService;
  ClicktocallService!: IClicktocallService;
  GmailService!: IGmailService;
  CalendarService!: ICalendarService;
  linkedincampaignservicelist: any[] = [];
  telegramcampaignservicelist: any[] = [];
  untaglist: any[] = [];
  taglist: any[] = [];
  livechatservicelist: any[] = [];
  clicktocalllist: any[] = [];
  googleanalyticsservice_list: any[] = [];
  googleanalyticsService!: IGoogleanalyticsService;
  editingcalendarEnabled: any;
  calendarservicelist: any;
  datas: any;
  data: any;
  shopifystatus: any;
  whatsappstatus: any;
  einvoice: any;
  emailstatus: any;
  gmailstatus: any;
  facebookstatus: any;
  linkedinstatus: any;
  telegramstatus: any;
  instagramstatus: any;
  livechatstatus: any;
  mintsoft: any;
  Company_code: any;
  companylist: any;
  Company_list: any;
  currency_list: any;
  company: any;
  companylogo!: File;
  welcomelogo!: File;
  companylogo1: any;
  welcomelogo1: any;
  selectedcurrency: any;
  country_list2: any;
  selectedcountry: any;
  reactiveFormwelcome: any;
  module_list: any;
  clicktocallstatus: any;
  mail_toggle: any;
  submodule_list: any;
  selectedmodulename: any;
  modulesummary_list: any;
  selectedscreenname: any;
  switchToGmail: boolean = true; // Initial state
  switchToExchange: boolean = false; // Initial state
  gmailservice_list: any[] = [];
  outlookservice_list: any[] = [];
  gmailcampaignservice_list: any[] = [];
  reactiveFormgoogleanalytics!: FormGroup;
  googleanalyticsstatus: any;
  reactiveFormSms!: FormGroup;
  SmsService!: ISmsService;
  smsstatus: any;
  smscampaignservicelist: any;
  oldValue: any;
  client_ids: any;
  assigncustomer_list: any[] = [];
  editingEnabled: any;
  selection = new SelectionModel<IAssign>(true, []);
  selection1 = new SelectionModel<IAssign1>(true, []);
  /////
  reactiveFormindiamart!: FormGroup;
  IndiaMARTService!: IIndiaMARTService;
  indiamartstatus: any;
  storedindiamartvalue: any;
  editingindiamartEnabled: any;
  indiamartcampaignservicelist: any;
  storedapikey: any;
  editingindidamartEnabled: any;
  currentTab: any = 'company';
  page_id: any;
  account_id: any;
  activeflag: any;
  paymentgatewayservice_list: any;
  payment_gateway: any;
  paymentService!: IPaymentService;
  selectedOption: string = '';
  enable_kot: any;
  employee_list: any[] = [];
  fileInputs: any;
  fileInputs1: any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.CampaingService = {} as ICampaingService;
    this.ShopifyService = {} as IShopifyService;
    this.EmailService = {} as IEmailService;
    this.GEmailService = {} as IGEmailService;
    this.OutlookService = {} as IOutlookService;
    this.FacebookService = {} as IFacebookService;
    this.LinkedinService = {} as ILinkedinService;
    this.TelegramService = {} as ITelegramService;
    this.LivechatService = {} as ILivechatService;
    this.InstagramService = {} as IInstagramService;
    this.ClicktocallService = {} as IClicktocallService;
    this.googleanalyticsService = {} as IGoogleanalyticsService;
    this.SmsService = {} as ISmsService;
    this.IndiaMARTService = {} as IIndiaMARTService;
    this.CalendarService = {} as ICalendarService;
    this.GmailService = {} as IGmailService;
    this.paymentService = {} as IPaymentService;
    this.Mintsoft = {} as IMintsoftService;
    this.Einvoice = {} as IEinvoice
  }

  ngOnInit(): void {
    // Form values for Add popup/////
    const options: Options = {
      // enableTime: true,
      dateFormat: 'Y-m-d',

    };
    //this.googleanalyticsstatus=this.reactiveFormgoogleanalytics.value.googleanalytics_status;
    this.GetpaymentgatewaySummary();
    this.GetWhatsappSummary();
    this.GetShopifySummary();
    this.GetMailSummary();
    this.GetGMailSummary();
    this.GetFacebookServiceSummary();
    this.GetMailmanagementSummary();
    this.GetInstagramServiceSummary();
    this.GetLinkedinServiceSummary();
    this.GetTelegramServiceSummary();
    this.GetCustomerTypeSummary();
    this.GetLivechatServiceSummary();
    this.GetCompanySummary();
    this.Getcurrency();
    this.GetCountry();
    this.GetModuleNameSummary();
    // this.GetModuleSummery();
    this.GetClicktocallSummary();
    this.GetGoogleanalyticsSummary();
    this.GetSmsServiceSummary();
    this.GetIndiaMARTServiceSummary();
    this.GetCalendarserviceSummary();
    this.GetOutlookSumary();
    this.GetMintSoftSummary();
    this.GetEinvoiceSummary();

    this.reactiveFormindiamart = new FormGroup({
      api_key: new FormControl(this.IndiaMARTService.api_key, [
        Validators.required,
      ]),
      indiamart_id: new FormControl(),
      indiamart_status: new FormControl()
    });
    this.reactiveFormShopify = new FormGroup({
      shopify_accesstoken: new FormControl(this.ShopifyService.shopify_accesstoken, [
        Validators.required,
      ]),
      shopify_store_name: new FormControl(this.ShopifyService.shopify_store_name, [
        Validators.required,
      ]),
      store_month_year: new FormControl(this.ShopifyService.store_month_year, [
        Validators.required,
      ]),
      shopify_id: new FormControl(),
      shopify_status: new FormControl()
    });
    this.reactiveFormWhatsapp = new FormGroup({

      whatsapp_accesstoken: new FormControl(this.CampaingService.whatsapp_accesstoken, [

        Validators.required

      ]),

      workspace_id: new FormControl(this.CampaingService.workspace_id, [
        Validators.required
      ]),
      channel_id: new FormControl(this.CampaingService.channel_id, [

        Validators.required
      ]),
      channelgroup_id: new FormControl(this.CampaingService.channelgroup_id, [

        Validators.required
      ]),
      mobile_number: new FormControl(this.CampaingService.mobile_number, [
        Validators.required
      ]),
      channel_name: new FormControl(this.CampaingService.channel_name, [

        Validators.required
      ]),
      whatsapp_id: new FormControl(),
      whatsapp_status: new FormControl()

    });
    this.reactiveformrazorpay = new FormGroup({
      key1: new FormControl(this.paymentService.key1, [
        Validators.required,
      ]),
      key2: new FormControl(this.paymentService.key2, [
        Validators.required,
      ]),
      key3: new FormControl(this.paymentService.key3, [
        Validators.required,
      ]),
      payment_gateway: new FormControl()
    });
    this.reactiveFormSms = new FormGroup({
      sms_user_id: new FormControl(this.SmsService.sms_user_id, [
        Validators.required,
      ]),
      sms_password: new FormControl(this.SmsService.sms_password, [
        Validators.required,
      ]),
      sms_id: new FormControl(),
      sms_status: new FormControl()
    });
    this.reactiveFormCompany = new FormGroup({
      company_code: new FormControl(),
      company_name: new FormControl(),

      company_phone: new FormControl(),
      company_mail: new FormControl(),
      contact_person: new FormControl('', Validators.required),
      company_address: new FormControl(),
      contact_person_phone: new FormControl(),
      contact_person_mail: new FormControl(),
      company_state: new FormControl(),
      country_name: new FormControl('', Validators.required),
      sequence_reset: new FormControl(),
      currency_code: new FormControl(),
      currency: new FormControl(),
      company_gid: new FormControl(),
      country_gid: new FormControl(),
      company_address1: new FormControl(),

    })


    this.reactiveFormEmail = new FormGroup({
      mail_access_token: new FormControl(this.EmailService.mail_access_token, [
        Validators.required,
      ]),
      mail_base_url: new FormControl(this.EmailService.mail_base_url, [
        Validators.required,
      ]),
      email_id: new FormControl(),
      receiving_domain: new FormControl(this.EmailService.receiving_domain, [
        Validators.required,
      ]),
      sending_domain: new FormControl(this.EmailService.sending_domain, [
        Validators.required,
      ]),
      email_username: new FormControl(this.EmailService.email_username, [
        Validators.required,
      ]),
      email_status: new FormControl(),
      mail_service: new FormControl(),


    });
    this.reactiveFormGEmail = new FormGroup({
      client_id: new FormControl(this.GEmailService.client_id, [
        Validators.required,
      ]),
      client_secret: new FormControl(this.GEmailService.client_secret, [
        Validators.required,
      ]),
      refresh_token: new FormControl(this.GEmailService.refresh_token, [
        Validators.required,
      ]),
      gmail_address: new FormControl(this.GEmailService.gmail_address, [Validators.required, Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      gmail_status: new FormControl(),
      email_id: new FormControl(),
      mail_service: new FormControl(),



    });
    this.reactiveFormGmail = new FormGroup({
      gclient_id: new FormControl(this.GmailService.gclient_id, [
        Validators.required,
      ]),
      gclient_secret: new FormControl(this.GmailService.gclient_secret, [
        Validators.required,
      ]),
      grefresh_token: new FormControl(this.GmailService.grefresh_token, [
        Validators.required,
      ]),
      ggmail_address: new FormControl(this.GmailService.ggmail_address, [
        Validators.required,
      ]),
      ggmail_status: new FormControl(),
      gemail_id: new FormControl(),
      mail_service: new FormControl(),


    });
    this.reactiveFormOutlook = new FormGroup({
      outlook_client_id: new FormControl(this.OutlookService.outlook_client_id, [
        Validators.required,
      ]),
      outlook_client_secret: new FormControl(this.OutlookService.outlook_client_secret, [
        Validators.required,
      ]),
      tenant_id: new FormControl(this.OutlookService.tenant_id, [
        Validators.required,
      ]),
      gmail_status: new FormControl(),
      mail_service: new FormControl(),


    });
    this.reactiveFormFacebook = new FormGroup({
      facebook_access_token: new FormControl(this.FacebookService.facebook_access_token, [
        Validators.required,
      ]),
      facebook_page_id: new FormControl(this.FacebookService.facebook_page_id, [
        Validators.required,
      ]),
      facebook_id: new FormControl(),
      facebook_status: new FormControl()
    });
    this.reactiveFormInstagram = new FormGroup({
      instagram_access_token: new FormControl(this.InstagramService.instagram_access_token, [
        Validators.required,
      ]),
      instagram_account_id: new FormControl(this.InstagramService.instagram_account_id, [
        Validators.required,
      ]),

      instagram_id: new FormControl(),
      instagram_status: new FormControl()
    });
    this.reactiveFormLinkedin = new FormGroup({
      linkedin_access_token: new FormControl(this.LinkedinService.linkedin_access_token, [
        Validators.required,
      ]),

      linkedin_id: new FormControl(),
      linkedin_status: new FormControl(),

    });
    this.reactiveFormTelegram = new FormGroup({
      bot_id: new FormControl(this.TelegramService.bot_id, [
        Validators.required,
      ]),
      chat_id: new FormControl(this.TelegramService.chat_id, [
        Validators.required,
      ]),
      telegram_id: new FormControl(),
      telegram_status: new FormControl()
    });
    this.reactiveFormCustomerType = new FormGroup({
      corporate_gid: new FormControl(),
      corporate_name: new FormControl(),
      retailer_gid: new FormControl(),
      retailer_name: new FormControl(),
      distributor_gid: new FormControl(),
      distributor_name: new FormControl(),
    });
    this.reactiveFormlivechat = new FormGroup({
      livechat_access_token: new FormControl(this.LivechatService.livechat_access_token, [
        Validators.required,
      ]),
      livechat_agentid: new FormControl(this.LivechatService.livechat_agentid, [
        Validators.required,
      ]),
      livechat_id: new FormControl(),
      livechat_status: new FormControl()
    });
    this.reactiveFormmintsoft = new FormGroup({
      api_key: new FormControl(this.Mintsoft.api_key, [
        Validators.required,
      ]),
      base_url: new FormControl(this.Mintsoft.base_url, [
        Validators.required,
      ]),
      mintsoft_id: new FormControl(),
      mintsoft_status: new FormControl(),
      mintsoft_flag: new FormControl()
    });
    this.reactiveFormwelcome = new FormGroup({
      module_name: new FormControl(),
      module_gid: new FormControl(),
      Module_name: new FormControl(),
      Module_gid: new FormControl(),
      // module_names: new FormControl(),
      // Module_names: new FormControl(),

    })
    this.reactiveFormclicktocall = new FormGroup({
      clicktocall_access_token: new FormControl(this.ClicktocallService.clicktocall_access_token, [
        Validators.required,
      ]),
      clicktocall_baseurl: new FormControl(this.ClicktocallService.clicktocall_baseurl, [
        Validators.required,
      ]),
      clicktocall_id: new FormControl(),
      clicktocall_status: new FormControl()
    });
    this.reactiveFormcalendar = new FormGroup({
      api_key: new FormControl(this.CalendarService.api_key, [
        Validators.required,
      ]),
      calendar_id: new FormControl(),
      active_flag: new FormControl()
    });
    this.reactiveFormgoogleanalytics = new FormGroup({
      user_url: new FormControl(this.googleanalyticsService.user_url, [
        Validators.required,
      ]),
      page_url: new FormControl(this.googleanalyticsService.page_url, [
        Validators.required,
      ]),
      googleanalytics_id: new FormControl(),
      googleanalytics_status: new FormControl()
    });

    this.reactiveFormeinvoice = new FormGroup({
      einvoice_status: new FormControl(),
      einvoice_id: new FormControl(),
      einvoiceAutenticationURL: new FormControl(this.Einvoice.einvoiceAutenticationURL),
      einvoiceIRNGenerate: new FormControl(this.Einvoice.einvoiceIRNGenerate),
      gspappid: new FormControl(this.Einvoice.gspappid),
      gspappsecret: new FormControl(this.Einvoice.gspappsecret),
      einvoiceuser_name: new FormControl(this.Einvoice.einvoiceuser_name),
      einvoicepwd: new FormControl(this.Einvoice.einvoicepwd),
      einvoicegstin: new FormControl(this.Einvoice.einvoicegstin),
      einvoice_Auth: new FormControl(this.Einvoice.einvoice_Auth),
      generateQRURL: new FormControl(this.Einvoice.generateQRURL),
      cancleIRN: new FormControl(this.Einvoice.cancleIRN),
      einvoice_flag: new FormControl(),
    });
    flatpickr('.date-picker', options);

  }
  //////new ui//////

  showTab(tab: string) {
    this.currentTab = tab;
  }

  GetCalendarserviceSummary() {

    var api = 'CampaignService/GetCalendarSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.calendarservicelist = this.responsedata.calendarservice_list;

      // Store the original password before modifying it
      const originalvalue = this.calendarservicelist[0].api_key;

      this.calendarservicelist.forEach((item: any) => {
        item.api_key = originalvalue;

        let hiddenapi_key = this.calendarhiddenapikey(item.api_key);
        this.reactiveFormcalendar.get("api_key")?.setValue(hiddenapi_key);
        this.reactiveFormcalendar.get("active_flag")?.setValue(item.active_flag);
        this.reactiveFormcalendar.get("calendar_id")?.setValue(item.calendar_id);
        this.activeflag = item.active_flag;
      });

      this.reactiveFormcalendar.get('api_key')?.disable(); // Disable the password field by default
      this.storedapikey = originalvalue;
    });
  }
  calendarapikeyEdit(apikeyInput: HTMLInputElement) {
    this.editingcalendarEnabled = true;
    this.reactiveFormcalendar.get('api_key')?.enable();
    apikeyInput.focus();
  }
  calendarhiddenapikey(apikey: string): string {
    if (apikey.length >= 6) {
      return apikey.substring(0, apikey.length - 6) + 'xxxxxx';
    } else {
      return apikey;
    }
  }
  oncalendarUpdate() {
    if (this.reactiveFormcalendar.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateCalendarService';
      let hiddenapikey = this.hiddenapikey(this.reactiveFormcalendar.get('api_key')?.value || '');
      if (hiddenapikey === this.reactiveFormcalendar.get('v')?.value) {
        let formData = { ...this.reactiveFormcalendar.value };
        formData.api_key = this.storedapikey;
        this.service.post(url, formData).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetCalendarserviceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetCalendarserviceSummary();
          }
        });
      } else {
        this.service.post(url, this.reactiveFormcalendar.value).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetCalendarserviceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetCalendarserviceSummary();
          }
        });
      }
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  onemailupdate() {

    if (this.reactiveFormEmail.status === 'VALID') {
      this.NgxSpinnerService.show();
      this.reactiveFormEmail.get("mail_service")?.setValue('Sparkpost');   
      var url = 'CampaignService/UpdateEmailService'
      this.service.post(url, this.reactiveFormEmail.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetMailSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetMailSummary();

        }

      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  GetpaymentgatewaySummary() {

    var api2 = 'CampaignService/GetpaymentgatewaySummary'
    this.service.get(api2).subscribe((result: any) => {
      this.reactiveformrazorpay.get("key1")?.setValue(result.key1 || null);
      this.reactiveformrazorpay.get("key2")?.setValue(result.key2 || null);
      this.reactiveformrazorpay.get("key3")?.setValue(result.key3 || null);
      this.reactiveformrazorpay.get("payment_gateway")?.setValue(result.payment_gateway || null);
      this.payment_gateway = result.payment_gateway;

    });

  }
  onrazorpaykeyupdate() {
    this.NgxSpinnerService.show();
    var url = 'CampaignService/updatepaymentgatewayservice'
    this.service.post(url, this.reactiveformrazorpay.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetpaymentgatewaySummary();

      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetpaymentgatewaySummary();

      }
    });


  }
  GetSmsServiceSummary() {
    ;
    var api = 'CampaignService/GetSmsServiceSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.smscampaignservicelist = this.responsedata.smscampaignservice_list;

      // Store the original password before modifying it
      const originalPassword = this.smscampaignservicelist[0].sms_password;

      this.smscampaignservicelist.forEach((item: any) => {
        // Reset the original password for each item before applying the transformation
        item.sms_password = originalPassword;

        let hiddenPassword = this.hideLastFourDigits(item.sms_password);
        this.reactiveFormSms.get("sms_user_id")?.setValue(item.sms_user_id);
        this.reactiveFormSms.get("sms_password")?.setValue(hiddenPassword);
        this.reactiveFormSms.get("sms_status")?.setValue(item.sms_status);
        this.reactiveFormSms.get("sms_id")?.setValue(item.sms_id);
        this.smsstatus = item.sms_status;
      });

      this.reactiveFormSms.get('sms_password')?.disable(); // Disable the password field by default
      this.oldValue = originalPassword;
    });
  }
  enablePasswordEdit(passwordInput: HTMLInputElement) {
    this.editingEnabled = true;
    this.reactiveFormSms.get('sms_password')?.enable(); // Enable the password field
    passwordInput.focus(); // Focus on the password input field
  }
  hideLastFourDigits(password: string): string {
    if (password.length >= 4) {
      // Replace last 4 characters with 'xxxx'
      return password.substring(0, password.length - 4) + 'xxxx';
    } else {
      // If password is less than 4 characters, return it as is
      return password;
    }
  }
  onsmsupdate() {
    if (this.reactiveFormSms.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateSmsService';
      // Get the hidden password
      let hiddenPassword = this.hideLastFourDigits(this.reactiveFormSms.get('sms_password')?.value || '');
      // Check if hidden password and form value are the same
      if (hiddenPassword === this.reactiveFormSms.get('sms_password')?.value) {
        // Include the original sms_password from the form
        let formData = { ...this.reactiveFormSms.value };
        formData.sms_password = this.oldValue;
        this.service.post(url, formData).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetSmsServiceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetSmsServiceSummary();
          }
          window.location.reload();
        });
      } else {
        // Pass the reactiveFormSms value directly
        this.service.post(url, this.reactiveFormSms.value).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetSmsServiceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetSmsServiceSummary();
          }
          window.location.reload();
        });
      }
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  ongmailupdate() {

    this.NgxSpinnerService.show();
    this.reactiveFormGEmail.get("mail_service")?.setValue('Gmail');   
    var url = 'CampaignService/UpdategmailService'
    this.service.post(url, this.reactiveFormGEmail.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetMailSummary();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetMailSummary();

      }
      window.location.reload();
    });
  }

  toggleTabContent(event: any) {
    this.useGmail = event.target.checked;
  }
  onlinkedinupdate() {

    if (this.reactiveFormLinkedin.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateLinkedinService'
      this.service.post(url, this.reactiveFormLinkedin.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetLinkedinServiceSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetLinkedinServiceSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  ontelegramupdate() {

    if (this.reactiveFormTelegram.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateTelegramService'
      this.service.post(url, this.reactiveFormTelegram.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetTelegramServiceSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetTelegramServiceSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  onlivechatupdate() {

    if (this.reactiveFormlivechat.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateLivechatService'
      this.service.post(url, this.reactiveFormlivechat.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetLivechatServiceSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetLivechatServiceSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  public toggleswitch(): void {

  }
  onclicktocallupdate() {

    if (this.reactiveFormclicktocall.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateCicktocallService'
      this.service.post(url, this.reactiveFormclicktocall.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetClicktocallSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetClicktocallSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  get einvoiceAutenticationURL() {
    return this.reactiveFormeinvoice.get('einvoiceAutenticationURL')!;
  }
  get einvoiceIRNGenerate() {
    return this.reactiveFormeinvoice.get('einvoiceIRNGenerate')!;
  }
  get gspappid() {
    return this.reactiveFormeinvoice.get('gspappid')!;
  }
  get gspappsecret() {
    return this.reactiveFormeinvoice.get('gspappsecret')!;
  }

  get einvoiceuser_name() {
    return this.reactiveFormeinvoice.get('einvoiceuser_name')!;
  }
  get einvoicepwd() {
    return this.reactiveFormeinvoice.get('einvoicepwd')!;
  }
  get einvoicegstin() {
    return this.reactiveFormeinvoice.get('einvoicegstin')!;
  }

  get einvoice_Auth() {
    return this.reactiveFormeinvoice.get('einvoice_Auth')!;
  }

  get generateQRURL() {
    return this.reactiveFormeinvoice.get('generateQRURL')!;
  }

  get cancleIRN() {
    return this.reactiveFormeinvoice.get('cancleIRN')!;
  }






  get bot_id() {
    return this.reactiveFormTelegram.get('bot_id')!;
  }
  get linkedin_access_token() {
    return this.reactiveFormLinkedin.get('linkedin_access_token')!;
  }
  get chat_id() {
    return this.reactiveFormTelegram.get('chat_id')!;
  }
  get facebook_access_token() {
    return this.reactiveFormFacebook.get('facebook_access_token')!;
  }
  get facebook_page_id() {
    return this.reactiveFormFacebook.get('facebook_page_id')!;
  }
  get shopify_accesstoken() {
    return this.reactiveFormShopify.get('shopify_accesstoken')!;
  }
  get shopify_store_name() {
    return this.reactiveFormShopify.get('shopify_store_name')!;
  }
  get store_month_year() {
    return this.reactiveFormShopify.get('store_month_year')!;
  }
  get shopify_status() {
    return this.reactiveFormShopify.get('shopify_status')!;
  }
  get workspace_id() {
    return this.reactiveFormWhatsapp.get('workspace_id')!;
  }
  get whatsapp_accesstoken() {
    return this.reactiveFormWhatsapp.get('whatsapp_accesstoken')!;
  }
  get mobile_number() {
    return this.reactiveFormWhatsapp.get('mobile_number')!;
  }
  get channelgroup_id() {
    return this.reactiveFormWhatsapp.get('channelgroup_id')!;
  }
  get channel_id() {
    return this.reactiveFormWhatsapp.get('channel_id')!;
  }
  get channel_name() {
    return this.reactiveFormWhatsapp.get('channel_name')!;
  }
  get mail_access_token() {
    return this.reactiveFormEmail.get('mail_access_token')!;
  }
  get mail_base_url() {
    return this.reactiveFormEmail.get('mail_base_url')!;
  }
  get receiving_domain() {
    return this.reactiveFormEmail.get('receiving_domain')!;
  } get sending_domain() {
    return this.reactiveFormEmail.get('sending_domain')!;
  }
  get email_username() {
    return this.reactiveFormEmail.get('email_username')!;
  }
  get corporate_name() {
    return this.reactiveFormCustomerType.get('corporate_name')!;
  }
  get retailer_name() {
    return this.reactiveFormCustomerType.get('retailer_name')!;
  }

  get linkedin_status() {
    return this.reactiveFormLinkedin.get('linkedin_status')!;
  }
  get instagram_access_token() {
    return this.reactiveFormInstagram.get('instagram_access_token')!;
  }
  get instagram_account_id() {
    return this.reactiveFormInstagram.get('instagram_account_id')!;
  }
  get instagram_status() {
    return this.reactiveFormInstagram.get('instagram_status')!;
  }
  get telegram_status() {
    return this.reactiveFormTelegram.get('telegram_status')!;
  }
  get facebook_status() {
    return this.reactiveFormFacebook.get('facebook_status')!;
  }
  get whatsapp_status() {
    return this.reactiveFormWhatsapp.get('whatsapp_status')!;
  }
  get email_status() {
    return this.reactiveFormEmail.get('email_status')!;
  }
  get gmail_status() {
    return this.reactiveFormGEmail.get('gmail_status')!;
  }

  get distributor_name() {
    return this.reactiveFormCustomerType.get('distributor_name')!;
  }
  get livechat_agentid() {
    return this.reactiveFormlivechat.get('livechat_agentid')!;
  }
  get mintsoftapi_key() {
    return this.reactiveFormmintsoft.get('api_key')!;
  }
  get base_url() {
    return this.reactiveFormmintsoft.get('base_url')!;
  }
  get livechat_access_token() {
    return this.reactiveFormlivechat.get('livechat_access_token')!;
  }
  get company_code() {
    return this.reactiveFormCompany.get('company_code')!;
  }

  get company_name() {
    return this.reactiveFormCompany.get('company_name')!;
  }
  get company_phone() {
    return this.reactiveFormCompany.get('company_phone')!;
  }
  get company_mail() {
    return this.reactiveFormCompany.get('company_mail')!;
  }

  get contact_person() {
    return this.reactiveFormCompany.get('contact_person')!;
  }

  get company_address() {
    return this.reactiveFormCompany.get('company_address')!;
  }

  get contact_person_phone() {
    return this.reactiveFormCompany.get('contact_person_phone')!;
  }

  get contact_person_mail() {
    return this.reactiveFormCompany.get('contact_person_mail')!;
  }

  get company_state() {
    return this.reactiveFormCompany.get('company_state')!;
  }
  get country_name() {
    return this.reactiveFormCompany.get('country_name')!;
  }

  get sequence_reset() {
    return this.reactiveFormCompany.get('sequence_reset')!;
  }

  get currency() {
    return this.reactiveFormCompany.get('currency')!;
  }
  get country_gid() {
    return this.reactiveFormCompany.get('country_gid')!;
  }
  get company_address1() {
    return this.reactiveFormCompany.get('company_address')!;
  }

  get module_name() {
    return this.reactiveFormwelcome.get('module_name')!;
  }
  get module_gid() {
    return this.reactiveFormwelcome.get('module_gid')!;
  }
  get Module_name() {
    return this.reactiveFormwelcome.get('Module_name')!;
  }
  get Module_gid() {
    return this.reactiveFormwelcome.get('Module_gid')!;
  }
  // get module_name() {
  //   return this.reactiveFormwelcome.get('module_name')!;
  // }
  get clicktocall_baseurl() {
    return this.reactiveFormclicktocall.get('clicktocall_baseurl')!;
  }
  get clicktocall_access_token() {
    return this.reactiveFormclicktocall.get('clicktocall_access_token')!;
  }
  get client_id() {
    return this.reactiveFormGEmail.get('client_id')!;
  }
  get client_secret() {
    return this.reactiveFormGEmail.get('client_secret')!;
  }
  get refresh_token() {
    return this.reactiveFormGEmail.get('refresh_token')!;
  }
  get gmail_address() {
    return this.reactiveFormGEmail.get('gmail_address')!;
  }
  get gclient_id() {
    return this.reactiveFormGmail.get('gclient_id')!;
  }
  get gclient_secret() {
    return this.reactiveFormGmail.get('gclient_secret')!;
  }
  get grefresh_token() {
    return this.reactiveFormGmail.get('grefresh_token')!;
  }
  get ggmail_address() {
    return this.reactiveFormGmail.get('ggmail_address')!;
  }
  get user_url() {
    return this.reactiveFormgoogleanalytics.get('user_url')!;
  }
  get page_url() {
    return this.reactiveFormgoogleanalytics.get('page_url')!;
  }
  get sms_user_id() {
    return this.reactiveFormSms.get('sms_user_id')!;
  }
  get sms_password() {
    return this.reactiveFormSms.get('sms_password')!;
  }
  get api_key() {
    return this.reactiveFormindiamart.get('api_key')!;
  }
  get outlook_client_id() {
    return this.reactiveFormOutlook.get('outlook_client_id')!;
  }
  get outlook_client_secret() {
    return this.reactiveFormOutlook.get('outlook_client_secret')!;
  }
  get tenant_id() {
    return this.reactiveFormOutlook.get('tenant_id')!;
  }
  get key1() {
    return this.reactiveformrazorpay.get('key1')!;
  }
  get key2() {
    return this.reactiveformrazorpay.get('key2')!;
  }
  get key3() {
    return this.reactiveformrazorpay.get('key3')!;
  }
  onshopifyupdate() {

    if (this.reactiveFormShopify.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateShopifyService'
      this.service.post(url, this.reactiveFormShopify.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetShopifySummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetShopifySummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }



  }

  onwhatsappupdate() {
    if (this.reactiveFormWhatsapp.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateWhatsappService'
      this.service.post(url, this.reactiveFormWhatsapp.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetWhatsappSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetWhatsappSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  GetWhatsappSummary() {
    var api = 'CampaignService/GetWhatsappSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignservice_list = this.responsedata.campaignservice_list;

      this.reactiveFormWhatsapp.get("whatsapp_accesstoken")?.setValue(this.campaignservice_list[0].access_token);
      this.reactiveFormWhatsapp.get("workspace_id")?.setValue(this.campaignservice_list[0].workspace_id);
      this.reactiveFormWhatsapp.get("channel_id")?.setValue(this.campaignservice_list[0].channel_id);
      this.reactiveFormWhatsapp.get("whatsapp_id")?.setValue(this.campaignservice_list[0].s_no);
      this.reactiveFormWhatsapp.get("mobile_number")?.setValue(this.campaignservice_list[0].mobile_number);
      this.reactiveFormWhatsapp.get("channel_name")?.setValue(this.campaignservice_list[0].channel_name);
      this.reactiveFormWhatsapp.get("whatsapp_status")?.setValue(this.campaignservice_list[0].whatsapp_status);
      this.reactiveFormWhatsapp.get("channelgroup_id")?.setValue(this.campaignservice_list[0].channelgroup_id);
      this.whatsappstatus = this.campaignservice_list[0].whatsapp_status;
    });
  }

  GetShopifySummary() {
    var api = 'CampaignService/GetShopifySummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignserv_list = this.responsedata.shopifycampaignservice_list;
      this.reactiveFormShopify.get("shopify_store_name")?.setValue(this.campaignserv_list[0].shopify_store_name);
      this.reactiveFormShopify.get("store_month_year")?.setValue(this.campaignserv_list[0].store_month_year);
      this.reactiveFormShopify.get("shopify_accesstoken")?.setValue(this.campaignserv_list[0].shopify_access_token);
      this.reactiveFormShopify.get("shopify_id")?.setValue(this.campaignserv_list[0].s_no);
      this.reactiveFormShopify.get("shopify_status")?.setValue(this.campaignserv_list[0].shopify_status);
      this.shopifystatus = this.campaignserv_list[0].shopify_status;
    });
  }


  GetCompanySummary() {
    this.NgxSpinnerService.show();
    var api = 'CampaignService/GetCompanySummary'
    this.service.get(api).subscribe((result: any) => {
      debugger
      this.responsedata = result;
      //console.log('ekwndkjwnk',this.responsedata.Company_list[0].company_logo)
      this.Company_list = this.responsedata.Company_list;
      this.companylogo1 = this.responsedata.Company_list[0].company_logo;
      this.welcomelogo1 = this.responsedata.Company_list[0].welcome_logo;
      this.reactiveFormCompany.get("company_code")?.setValue(this.Company_list[0].company_code);
      this.reactiveFormCompany.get("company_name")?.setValue(this.Company_list[0].company_name);
      this.reactiveFormCompany.get("company_phone")?.setValue(this.Company_list[0].company_phone);
      this.reactiveFormCompany.get("company_mail")?.setValue(this.Company_list[0].company_mail);
      this.reactiveFormCompany.get("contact_person")?.setValue(this.Company_list[0].contact_person);
      this.reactiveFormCompany.get("company_address")?.setValue(this.Company_list[0].company_address);
      this.reactiveFormCompany.get("contact_person_mail")?.setValue(this.Company_list[0].contact_person_mail);
      this.reactiveFormCompany.get("contact_person_phone")?.setValue(this.Company_list[0].contact_person_phone);
      this.reactiveFormCompany.get("company_state")?.setValue(this.Company_list[0].company_state);
      this.reactiveFormCompany.get("country_name")?.setValue(this.Company_list[0].country_name);
      this.reactiveFormCompany.get("sequence_reset")?.setValue(this.Company_list[0].sequence_reset);
      this.reactiveFormCompany.get("company_gid")?.setValue(this.Company_list[0].company_gid);
      this.reactiveFormCompany.get("company_address1")?.setValue(this.Company_list[0].company_address1);
      this.NgxSpinnerService.hide();

    });
  }
  onfacebookkeyadd() {

    if (this.reactiveFormFacebook.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/PostFacebookkeys'
      this.service.post(url, this.reactiveFormFacebook.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetFacebookServiceSummary();
          this.reactiveFormFacebook.reset();

        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetFacebookServiceSummary();
          this.reactiveFormFacebook.reset();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  onclose() {
    this.reactiveFormFacebook.reset();
  }
  GetFacebookServiceSummary() {

    var api = 'CampaignService/GetFacebookServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.facebookcampaignservicelist = this.responsedata.facebookcampaignservice_list;
      this.facebookstatus = this.facebookcampaignservicelist[0].facebook_status;
    });
  }
  GetMailmanagementSummary() {

    var api = 'CampaignService/GetMailmanagementSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.gmailcampaignservice_list = this.responsedata.gmailcampaignservice_list;
      this.facebookstatus = this.facebookcampaignservicelist[0].facebook_status;
    });
  }
  openModaldelete(parameter: string) {
    this.page_id = parameter

  }
  openModaldelete1(parameter: string) {
    this.client_ids = parameter

  }
  openModaltag(parameter: string) {
    this.emailaddress = parameter
    this.GetEmployeeMailsTag(this.emailaddress);


  }
  openModaluntag(parameter: string) {
    this.emailaddress = parameter
    this.GetEmployeeMailsUnTag(this.emailaddress);

  }
  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.untaglist.length;
    return numSelected === numRows;
  }
  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.untaglist.forEach((row: IAssign1) => this.selection1.select(row));
  }
  ondelete() {
    var url = 'CampaignService/deleteaccesstoken'
    let param = {
      page_id: this.page_id
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)

      }
      this.GetFacebookServiceSummary();



    });
  }

  onfacebookupdate() {

    this.NgxSpinnerService.show();
    var url = 'CampaignService/updatefacebookkeys'
    this.service.post(url, this.reactiveFormFacebook.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetFacebookServiceSummary();
        this.reactiveFormFacebook.reset();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetFacebookServiceSummary();
        this.reactiveFormFacebook.reset();
      }
    });



  }

  Getcurrency() {
    var api = 'CampaignService/Getcurrency'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.currency_list = this.responsedata.currency_list;
      this.reactiveFormCompany.get("currency")?.setValue(this.currency_list[0].currency);
      this.reactiveFormCompany.get("currency_code")?.setValue(this.currency_list[0].currency_code);

    });

  }
  GetCountry() {
    var api = 'CampaignService/GetCountry'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.country_list2 = this.responsedata.country_list2;
      // this.reactiveFormCompany.get("country_gid")?.setValue(this.country_list2[0].country_gid);
      // this.reactiveFormCompany.get("country_name")?.setValue(this.country_list2[0].country_name);

    });

  }

  GetMailSummary() {
    var api = 'CampaignService/GetMailSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.campaignser_list = this.responsedata.mailcampaignservice_list;
      this.reactiveFormEmail.get("mail_access_token")?.setValue(this.campaignser_list[0].mail_access_token);
      this.reactiveFormEmail.get("mail_base_url")?.setValue(this.campaignser_list[0].mail_base_url);
      this.reactiveFormEmail.get("email_id")?.setValue(this.campaignser_list[0].s_no);
      this.reactiveFormEmail.get("receiving_domain")?.setValue(this.campaignser_list[0].receiving_domain);
      this.reactiveFormEmail.get("sending_domain")?.setValue(this.campaignser_list[0].sending_domain);
      this.reactiveFormEmail.get("email_username")?.setValue(this.campaignser_list[0].email_username);
      this.reactiveFormEmail.get("email_status")?.setValue(this.campaignser_list[0].email_status);
      this.gmailstatus = this.campaignser_list[0].email_status;
    });
  }
  GetGMailSummary() {
    var api = 'CampaignService/GetGMailSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.gmailservice_list = this.responsedata.gmailcampaignservice_list;
      this.reactiveFormGEmail.get("client_id")?.setValue(this.gmailservice_list[0].client_id);
      this.reactiveFormGEmail.get("email_id")?.setValue(this.gmailservice_list[0].s_no);
      this.reactiveFormGEmail.get("client_secret")?.setValue(this.gmailservice_list[0].client_secret);
      this.reactiveFormGEmail.get("refresh_token")?.setValue(this.gmailservice_list[0].refresh_token);
      this.reactiveFormGEmail.get("gmail_address")?.setValue(this.gmailservice_list[0].gmail_address);
      this.reactiveFormGEmail.get("gmail_status")?.setValue(this.gmailservice_list[0].gmail_status);
      this.gmailstatus = this.gmailservice_list[0].gmail_status;
      if (this.gmailstatus == null || this.gmailstatus == '') {
        this.GetOutlookSumary();
      }
    });
  }
  GetOutlookSumary() {
    var api = 'CampaignService/GetOutlookSummary'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.outlookservice_list = this.responsedata.outlookcampaignservice_list;
      this.reactiveFormOutlook.get("outlook_client_id")?.setValue(this.outlookservice_list[0].client_id);
      this.reactiveFormOutlook.get("outlook_client_secret")?.setValue(this.outlookservice_list[0].client_secret);
      this.reactiveFormOutlook.get("tenant_id")?.setValue(this.outlookservice_list[0].tenant_id);
      this.gmailstatus = this.outlookservice_list[0].outlook_status;
      if (this.gmailstatus == null || this.gmailstatus == '') {
        this.GetGMailSummary();
      }
    });
  }

  GetEmployeeMailsTag(emailaddress: any) {
    this.NgxSpinnerService.show();
    let param = {
      emailaddress: emailaddress
    }
    var url = 'CampaignService/GetEmployeeMailsTag';
    this.service.getparams(url, param).subscribe((apiresponse: any) => {
      $('#taglist').DataTable().destroy();
      this.taglist = apiresponse.taglist;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#taglist').DataTable();
      }, 1);

    });
  }
  GetEmployeeMailsUnTag(emailaddress: any) {
    this.NgxSpinnerService.show();
    let param = {
      emailaddress: emailaddress
    }
    var url = 'CampaignService/GetEmployeeMailsUnTag';
    this.service.getparams(url, param).subscribe((apiresponse: any) => {
      $('#untaglist').DataTable().destroy();
      this.untaglist = apiresponse.untaglist;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#untaglist').DataTable();
      }, 1);

    });
  }
  GetLinkedinServiceSummary() {
    var api = 'CampaignService/GetLinkedinServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.linkedincampaignservicelist = this.responsedata.linkedincampaignservice_list;
      this.reactiveFormLinkedin.get("linkedin_access_token")?.setValue(this.linkedincampaignservicelist[0].linkedin_access_token);
      this.reactiveFormLinkedin.get("linkedin_id")?.setValue(this.linkedincampaignservicelist[0].linkedin_id);
      this.reactiveFormLinkedin.get("linkedin_status")?.setValue(this.linkedincampaignservicelist[0].linkedin_status);
      this.linkedinstatus = this.linkedincampaignservicelist[0].linkedin_status;
    });
  }
  GetTelegramServiceSummary() {
    var api = 'CampaignService/GetTelegramServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.telegramcampaignservicelist = this.responsedata.telegramcampaignservice_list;
      this.reactiveFormTelegram.get("bot_id")?.setValue(this.telegramcampaignservicelist[0].bot_id);
      this.reactiveFormTelegram.get("chat_id")?.setValue(this.telegramcampaignservicelist[0].chat_id);
      this.reactiveFormTelegram.get("telegram_id")?.setValue(this.telegramcampaignservicelist[0].telegram_id);
      this.reactiveFormTelegram.get("telegram_status")?.setValue(this.telegramcampaignservicelist[0].telegram_status);
      this.telegramstatus = this.telegramcampaignservicelist[0].telegram_status;
    });
  }
  GetLivechatServiceSummary() {
    var api = 'CampaignService/GetLivechatServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.livechatservicelist = this.responsedata.livechatservice_list;
      this.reactiveFormlivechat.get("livechat_agentid")?.setValue(this.livechatservicelist[0].livechat_agentid);
      this.reactiveFormlivechat.get("livechat_access_token")?.setValue(this.livechatservicelist[0].livechat_access_token);
      this.reactiveFormlivechat.get("livechat_id")?.setValue(this.livechatservicelist[0].livechat_id);
      this.reactiveFormlivechat.get("livechat_status")?.setValue(this.livechatservicelist[0].livechat_status);
      this.livechatstatus = this.livechatservicelist[0].livechat_status;
    });
  }

  GetCustomerTypeSummary() {

    var api = 'CampaignService/GetCustomerTypeSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.customertype_list = this.responsedata.customertype_list;

      this.reactiveFormCustomerType.get("corporate_gid")?.setValue(this.customertype_list[0].customertype_gid);
      this.reactiveFormCustomerType.get("corporate_name")?.setValue(this.customertype_list[0].customer_type);
      this.reactiveFormCustomerType.get("retailer_gid")?.setValue(this.customertype_list[1].customertype_gid);
      this.reactiveFormCustomerType.get("retailer_name")?.setValue(this.customertype_list[1].customer_type);
      this.reactiveFormCustomerType.get("distributor_gid")?.setValue(this.customertype_list[2].customertype_gid);
      this.reactiveFormCustomerType.get("distributor_name")?.setValue(this.customertype_list[2].customer_type);

    });
  }
  GetClicktocallSummary() {

    var api = 'CampaignService/GetClicktocallSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.clicktocalllist = this.responsedata.clicktocall_list;
      this.reactiveFormclicktocall.get("clicktocall_baseurl")?.setValue(this.clicktocalllist[0].clicktocall_baseurl);
      this.reactiveFormclicktocall.get("clicktocall_access_token")?.setValue(this.clicktocalllist[0].clicktocall_access_token);
      this.reactiveFormclicktocall.get("clicktocall_id")?.setValue(this.clicktocalllist[0].clicktocall_id);
      this.reactiveFormclicktocall.get("clicktocall_status")?.setValue(this.clicktocalllist[0].clicktocall_status);
      this.clicktocallstatus = this.clicktocalllist[0].clicktocall_status;
    });
  }
  GetIndiaMARTServiceSummary() {

    var api = 'CampaignService/GetIndiaMARTServiceSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.indiamartcampaignservicelist = this.responsedata.indiamartcampaignservice_list;

      // Store the original password before modifying it
      const originalvalue = this.indiamartcampaignservicelist[0].api_key;

      this.indiamartcampaignservicelist.forEach((item: any) => {
        // Reset the original password for each item before applying the transformation
        item.api_key = originalvalue;

        let hiddenapi_key = this.hiddenapikey(item.api_key);
        this.reactiveFormindiamart.get("api_key")?.setValue(hiddenapi_key);
        this.reactiveFormindiamart.get("indiamart_status")?.setValue(item.indiamart_status);
        this.reactiveFormindiamart.get("indiamart_id")?.setValue(item.indiamart_id);
        this.indiamartstatus = item.indiamart_status;
      });

      this.reactiveFormindiamart.get('api_key')?.disable(); // Disable the password field by default
      this.storedapikey = originalvalue;
    });
  }
  enableapikeyEdit(apikeyInput: HTMLInputElement) {
    this.editingindidamartEnabled = true;
    this.reactiveFormindiamart.get('api_key')?.enable();
    apikeyInput.focus();
  }
  hiddenapikey(apikey: string): string {
    if (apikey.length >= 6) {
      return apikey.substring(0, apikey.length - 6) + 'xxxxxx';
    } else {
      return apikey;
    }
  }
  onindiamartUpdate() {
    if (this.reactiveFormindiamart.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateIndiaMARTService';
      // Get the hidden password
      let hiddenapikey = this.hiddenapikey(this.reactiveFormindiamart.get('api_key')?.value || '');
      // Check if hidden password and form value are the same
      if (hiddenapikey === this.reactiveFormindiamart.get('api_key')?.value) {
        // Include the original sms_password from the form
        let formData = { ...this.reactiveFormindiamart.value };
        formData.api_key = this.storedapikey;
        this.service.post(url, formData).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetIndiaMARTServiceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetIndiaMARTServiceSummary();
          }
          window.location.reload();
        });
      } else {
        this.service.post(url, this.reactiveFormindiamart.value).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
            this.GetIndiaMARTServiceSummary();
          } else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
            this.GetIndiaMARTServiceSummary();
          }
          window.location.reload();
        });
      }
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  GetGoogleanalyticsSummary() {

    var api2 = 'CampaignService/GetGoogleanalyticsserviceSummary'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.googleanalyticsservice_list = this.responsedata.googleanalyticsservice_list;
      this.reactiveFormgoogleanalytics.get("user_url")?.setValue(this.googleanalyticsservice_list[0].user_url || null);
      this.reactiveFormgoogleanalytics.get("page_url")?.setValue(this.googleanalyticsservice_list[0].page_url || null);
      this.reactiveFormgoogleanalytics.get("googleanalytics_status")?.setValue(this.googleanalyticsservice_list[0].googleanalytics_status || null);
      this.reactiveFormgoogleanalytics.get("googleanalytics_id")?.setValue(this.googleanalyticsservice_list[0].googleanalytics_id || null);
      this.googleanalyticsstatus = this.googleanalyticsservice_list[0].googleanalytics_status;

      //console.log('hello',this.googleanalyticsstatus)
    });

    //console.log('hello',this.googleanalyticsstatus)

  }
  oncustomertypeupdate() {
    this.NgxSpinnerService.show();
    //console.log(this.reactiveFormCustomerType.value);

    if (this.reactiveFormCustomerType.value.corporate_name != null &&
      this.reactiveFormCustomerType.value.corporate_name != "" &&
      this.reactiveFormCustomerType.value.retailer_name != null &&
      this.reactiveFormCustomerType.value.retailer_name != "" &&
      this.reactiveFormCustomerType.value.distributor_name != null &&
      this.reactiveFormCustomerType.value.distributor_name != "") {

      var url = 'CampaignService/UpdateCustomerType'
      this.service.post(url, this.reactiveFormCustomerType.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetShopifySummary();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetShopifySummary();
        }
      });
    }
    else {
      this.NgxSpinnerService.hide();
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Fields !! ')
    }
  }
  onChange1(event: any) {

    this.companylogo = event.target.files[0];

  }

  onChange2(event: any) {

    this.welcomelogo = event.target.files[0];

  }

  public onsubmit(): void {
    debugger
    if (this.reactiveFormCompany.status == "VALID") {
      if (!this.companylogo && !this.welcomelogo) {
        this.NgxSpinnerService.show();
        var url = 'CampaignService/PostCompanyDetailsForm'
        this.service.post(url, this.reactiveFormCompany.value).subscribe((result: any) => {
          if (result.status == false) {
            window.scrollTo({
              top: 0, // Code is used for scroll top after event done
            });
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            window.scrollTo({
              top: 0, // Code is used for scroll top after event done
            });
            this.GetCompanySummary();
            this.Getcurrency();
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message);
          }
        });
      }
      else if (this.companylogo && this.welcomelogo) {
        this.NgxSpinnerService.show();
        this.company = this.reactiveFormCompany.value;
        let formData = new FormData();
        formData.append("company_logo", this.companylogo);
        formData.append("welcome_logo", this.welcomelogo);
        formData.append("company_code", this.company.company_code);
        formData.append("company_name", this.company.company_name);
        formData.append("company_phone", this.company.company_phone);
        formData.append("company_mail", this.company.company_mail);
        formData.append("contact_person", this.company.contact_person);
        formData.append("company_address", this.company.company_address);
        formData.append("company_address1", this.company.company_address1);
        formData.append("contact_person_mail", this.company.contact_person_mail);
        formData.append("contact_person_phone", this.company.contact_person_phone);
        formData.append("company_state", this.company.company_state);
        formData.append("country_name", this.company.country_name);
        formData.append("sequence_reset", this.company.sequence_reset);
        formData.append("company_gid", this.company.company_gid);

        // Call the service to send the form data
        var api7 = 'CampaignService/PostCompanyDetails';
        this.service.postfile(api7, formData).subscribe((result: any) => {
          this.NgxSpinnerService.hide();

          if (result.status === false) {
            this.fileInputs = null;
            this.fileInputs1 = null;
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
          } else {
            this.NgxSpinnerService.hide();
            this.GetCompanySummary();
            this.Getcurrency();
            this.ToastrService.success(result.message);
            window.location.reload();
          }

          // this.responsedata = result;
        });
      }
      else if (this.companylogo) {
        this.NgxSpinnerService.show();
        this.company = this.reactiveFormCompany.value;
        let formData = new FormData();
        formData.append("company_logo", this.companylogo);
        formData.append("company_code", this.company.company_code);
        formData.append("company_name", this.company.company_name);
        formData.append("company_phone", this.company.company_phone);
        formData.append("company_mail", this.company.company_mail);
        formData.append("contact_person", this.company.contact_person);
        formData.append("company_address", this.company.company_address);
        formData.append("company_address1", this.company.company_address1);
        formData.append("contact_person_mail", this.company.contact_person_mail);
        formData.append("contact_person_phone", this.company.contact_person_phone);
        formData.append("company_state", this.company.company_state);
        formData.append("country_name", this.company.country_name);
        formData.append("sequence_reset", this.company.sequence_reset);
        formData.append("company_gid", this.company.company_gid);

        // Call the service to send the form data
        var api7 = 'CampaignService/PostCompanyDetails';
        this.service.postfile(api7, formData).subscribe((result: any) => {
          this.NgxSpinnerService.hide();

          if (result.status === false) {
            this.fileInputs = null;
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
          } else {
            this.fileInputs = null;
            this.NgxSpinnerService.hide();
            this.GetCompanySummary();
            this.Getcurrency();
            this.ToastrService.success(result.message);
            window.location.reload();
          }

          // this.responsedata = result;
        });
      }
      else if (this.welcomelogo) {
        this.NgxSpinnerService.show();
        this.company = this.reactiveFormCompany.value;
        let formData = new FormData();
        formData.append("welcome_logo", this.welcomelogo);
        formData.append("company_code", this.company.company_code);
        formData.append("company_name", this.company.company_name);
        formData.append("company_phone", this.company.company_phone);
        formData.append("company_mail", this.company.company_mail);
        formData.append("contact_person", this.company.contact_person);
        formData.append("company_address", this.company.company_address);
        formData.append("company_address1", this.company.company_address1);
        formData.append("contact_person_mail", this.company.contact_person_mail);
        formData.append("contact_person_phone", this.company.contact_person_phone);
        formData.append("company_state", this.company.company_state);
        formData.append("country_name", this.company.country_name);
        formData.append("sequence_reset", this.company.sequence_reset);
        formData.append("company_gid", this.company.company_gid);

        // Call the service to send the form data
        var api7 = 'CampaignService/PostCompanyDetails';
        this.service.postfile(api7, formData).subscribe((result: any) => {
          this.NgxSpinnerService.hide();

          if (result.status === false) {
            this.fileInputs1 = null;
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message);
          } else {
            this.fileInputs1 = null;
            this.NgxSpinnerService.hide();
            this.GetCompanySummary();
            this.Getcurrency();
            this.ToastrService.success(result.message);
          }

          // this.responsedata = result;
        });
      }
      else {
        this.ToastrService.warning('Both Company Logo and Welcome Logo must be provided.');
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !!');
    }
  }

  GetModuleNameSummary() {

    var api = 'CampaignService/GetModuleNameSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.module_list = this.responsedata.module_list;
      this.reactiveFormwelcome.get("module_gid")?.setValue(this.module_list.module_gid);
      this.reactiveFormwelcome.get("module_name")?.setValue(this.module_list.module_name);

    });
  }

  OnChangeScreenNameSummary() {

    let module = this.reactiveFormwelcome.get("module_name")?.value
    if (module != '' || module != null) {
      let param = {
        module_gid: module
      }
      var api = 'CampaignService/GetScreenNameSummary'
      this.service.getparams(api, param).subscribe((result: any) => {

        this.responsedata = result;
        this.submodule_list = this.responsedata.submodule_list;
        this.reactiveFormwelcome.get("Module_name")?.setValue(this.submodule_list.Module_name);
        this.reactiveFormwelcome.get("Module_gid")?.setValue(this.submodule_list.Module_gid);


      });
    }
    else {
      this.ToastrService.warning('select module name')
    }
  }

  updatemodulename() {

    var url = 'CampaignService/updatemodulename'
    this.service.post(url, this.reactiveFormwelcome.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetShopifySummary();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetShopifySummary();

      }
      window.location.reload();
    });




  }

  GetModuleSummery() {
    var api = 'CampaignService/GetModuleSummery'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.modulesummary_list = this.responsedata.modulesummary_list;
      // this.reactiveFormwelcome.get("module_gid")?.setValue(this.module_list.module_gid);
      this.reactiveFormwelcome.get("module_name")?.setValue(this.modulesummary_list[0].module_names);
      this.reactiveFormwelcome.get("Module_name")?.setValue(this.modulesummary_list[1].module_names);

    });
  }
  onGoogleAnalyticsUpdate() {

    if (this.reactiveFormgoogleanalytics.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/updategoogleanalyticsservice'
      this.service.post(url, this.reactiveFormgoogleanalytics.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetGoogleanalyticsSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetGoogleanalyticsSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  oninstakeyadd() {

    if (this.reactiveFormInstagram.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/PostInstakeys'
      this.service.post(url, this.reactiveFormInstagram.value).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetInstagramServiceSummary();
          this.reactiveFormInstagram.reset();

        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetInstagramServiceSummary();
          this.reactiveFormInstagram.reset();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }

  GetInstagramServiceSummary() {

    var api = 'CampaignService/GetInstaServiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.instagramcampaignservicelist = this.responsedata.instagramcampaignservice_list;
      this.instagramstatus = this.instagramcampaignservicelist[0].instagram_status;
    });
  }
  openModalinstadelete(parameter: string) {
    this.account_id = parameter

  }
  oninstadelete() {
    var url = 'CampaignService/deleteinstaaccesstoken'
    let param = {
      instagram_account_id: this.account_id
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)

      }
      this.GetInstagramServiceSummary();



    });
  }

  oninstagramupdate() {

    this.NgxSpinnerService.show();
    var url = 'CampaignService/updateinstakeys'
    this.service.post(url, this.reactiveFormInstagram.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetInstagramServiceSummary();
        this.reactiveFormInstagram.reset();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetInstagramServiceSummary();
        this.reactiveFormInstagram.reset();
      }
    });



  }
  onoutlookupdate() {
    debugger
    if (this.reactiveFormOutlook.status === 'VALID') {
      this.NgxSpinnerService.show();
      this.reactiveFormOutlook.get("mail_service")?.setValue('Outlook');   
      var url = 'CampaignService/UpdateoutlookService'
      this.service.post(url, this.reactiveFormOutlook.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetMailSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetMailSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  onmailmanagementkeyadd() {

    if (this.reactiveFormGmail.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/Postmailmanagementkeys'
      this.service.post(url, this.reactiveFormGmail.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetMailmanagementSummary();
          this.reactiveFormGmail.reset();

        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetMailmanagementSummary();
          this.reactiveFormGmail.reset();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  ondelete1() {
    var url = 'CampaignService/deletemailkey'
    let param = {
      s_no: this.client_ids
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)

      }
      this.GetMailmanagementSummary();



    });
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.taglist.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.taglist.forEach((row: IAssign) => this.selection.select(row));
  }
  tagemployee() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.taglist = this.pick;
    this.CurObj.emailaddress = this.emailaddress;

    if (this.CurObj.taglist.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly Select Atleast One Record to Tag Employee.");
      return;
    }

    else {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/TagemptoGmail';
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result === false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning('Error Occurred While CusEmployeetomer Tag to Gmail.');
          this.GetMailmanagementSummary();
          // this.GetEmployeeMailsTag();
          // this.GetEmployeeMailsUnTag();
        } else {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success('Inbox Mail Tag to Employee Successfully !!');
          this.GetMailmanagementSummary();
          // this.GetEmployeeMailsTag();
          // this.GetEmployeeMailsUnTag();
          // Delay navigation to ensure toastr message is shown
        }
      });
      this.selection.clear();
    }
  }
  untagemployee() {
    debugger;
    this.pick = this.selection1.selected
    this.CurObj1.untaglist = this.pick;
    this.CurObj.emailaddress = this.emailaddress;

    if (this.CurObj1.untaglist.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly Select Atleast One Record to UnTag Employee.");
      return;
    }

    else {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UnTagempGmail';
      this.service.post(url, this.CurObj1).subscribe((result: any) => {
        if (result === false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning('Error Occurred While Employee UnTag to Gmail.');
          this.GetMailmanagementSummary();
          // this.GetEmployeeMailsTag();
          // this.GetEmployeeMailsUnTag();
        } else {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success('Inbox Mail UnTag to Employee Successfully !!');
          this.GetMailmanagementSummary();
          // this.GetEmployeeMailsTag();
          // this.GetEmployeeMailsUnTag();
          // Delay navigation to ensure toastr message is shown
        }
      });
      this.selection.clear();
    }
  }
  onenablekotupdate() {
    this.NgxSpinnerService.show();
    let param = {
      selectedOption: this.selectedOption
    }
    var url = 'CampaignService/enablekotscreen';
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
      }
    });

  }
  getkotscreensum() {
    this.NgxSpinnerService.show();
    var url = 'CampaignService/Getkotscreensum';
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.selectedOption = result.enable_kot;
      this.GetEmployeeSummary();
    });
  }
  GetEinvoiceSummary() {
    debugger
    var api = 'CampaignService/GetEinvoiceSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.einvoice_list = this.responsedata.einvoice_list;
      this.reactiveFormeinvoice.get("einvoice_id")?.setValue(this.einvoice_list[0].id);
      this.reactiveFormeinvoice.get("einvoiceAutenticationURL")?.setValue(this.einvoice_list[0].einvoiceAutenticationURL);
      this.reactiveFormeinvoice.get("einvoiceIRNGenerate")?.setValue(this.einvoice_list[0].einvoiceIRNGenerate);
      this.reactiveFormeinvoice.get("gspappid")?.setValue(this.einvoice_list[0].gspappid);
      this.reactiveFormeinvoice.get("gspappsecret")?.setValue(this.einvoice_list[0].gspappsecret);
      this.reactiveFormeinvoice.get("einvoiceuser_name")?.setValue(this.einvoice_list[0].einvoiceuser_name);
      this.reactiveFormeinvoice.get("einvoicepwd")?.setValue(this.einvoice_list[0].einvoicepwd);
      this.reactiveFormeinvoice.get("einvoicegstin")?.setValue(this.einvoice_list[0].einvoicegstin);
      this.reactiveFormeinvoice.get("einvoice_Auth")?.setValue(this.einvoice_list[0].einvoice_Auth);
      this.reactiveFormeinvoice.get("generateQRURL")?.setValue(this.einvoice_list[0].generateQRURL);
      this.reactiveFormeinvoice.get("cancleIRN")?.setValue(this.einvoice_list[0].cancleIRN);
      this.einvoice = this.einvoice_list[0].einvoice_flag;
    });
  }
  onEinvoiceUpdate() {

    this.NgxSpinnerService.show();
    var url = 'CampaignService/UpdateEinvoice'
    this.service.post(url, this.reactiveFormeinvoice.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.GetEinvoiceSummary();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetEinvoiceSummary();

      }
      window.location.reload();
    });



  }
  GetMintSoftSummary() {
    debugger
    var api = 'CampaignService/GetMintSoftSummary'
    this.service.get(api).subscribe((result: any) => {

      this.responsedata = result;
      this.mintsoft_list = this.responsedata.mintsoft_list;
      this.reactiveFormmintsoft.get("api_key")?.setValue(this.mintsoft_list[0].api_key);
      this.reactiveFormmintsoft.get("base_url")?.setValue(this.mintsoft_list[0].base_url);
      this.reactiveFormmintsoft.get("mintsoft_id")?.setValue(this.mintsoft_list[0].id);
      this.mintsoft = this.mintsoft_list[0].mintsoft_flag
    });
  }
  onMintsoftUpdate() {

    if (this.reactiveFormmintsoft.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'CampaignService/UpdateMintSoft'
      this.service.post(url, this.reactiveFormmintsoft.value).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetMintSoftSummary();
        }
        else {

          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetMintSoftSummary();

        }
        window.location.reload();
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  GetEmployeeSummary() {
    var api1 = 'UserManagementSummary/GetUserSummary'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.employee_list = this.responsedata.usersummary_list;

    });
  }
}

