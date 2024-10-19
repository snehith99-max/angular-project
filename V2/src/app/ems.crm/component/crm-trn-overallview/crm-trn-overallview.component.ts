import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
interface IWatsapp {
  //sourceedit_name: any;
  leadbank_gid: string;
  created_date: string;
  customer_name: string;
  displayName: string;
  mobile: string;
  created_by: string;
  computedDisplayName: string;
  email: string;
  first_letter: string;
  key: string;
  value: string;
  firstName: string;
  lastName: string;
  gender: string;
  identifierValue: string;
  type: string;
  sendtext: string;
  phone: string;

}
interface IMailform {
  leadbank_gid: string;
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  to_mail: any;
}
interface IGmailform {

  leadbank_gid: string;
  gmail_mail_from: string;
  gmail_sub: string;
  //  gmail_to: string;
  gmail_body: string;

  gmail_to_mail: any;
}

interface IMyleads {


  leadbank_gid: string;
  leadbank_gid1: string;
  lead2campaign_gid: string;
  schedule_type: string;
  schedule: string;
  ScheduleRemarks1: string;
  schedule_status: string;
  schedule_date: string;
  schedule_time: string;
  call_response: string;
}

@Component({
  selector: 'app-crm-trn-overallview',
  templateUrl: './crm-trn-overallview.component.html',
  styleUrls: ['./crm-trn-overallview.component.scss']
})

export class CrmTrnOverallviewComponent {
  @ViewChild('Inbox') tableRef!: ElementRef;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  subscription!: Subscription;
  currentDayName: string;
  fromDate: any; toDate: any;
  encryptedParam: any;
  search: string = '';
  response_data: any;
  mailmanagement: any[] = [];
  reactiveForm!: FormGroup;
  parameterValue1: any;
  mailsummary_list: any;
  mail: any;
  reactiveFormdrop!: FormGroup;
  from_mail: any;
  to_mail: any;
  parameter1: any;
  subject: any;
  body_content: any;
  mailcount_list: any[] = [];
  responsedata: any;
  filteredData: any;
  clicktotal_count: any;
  deliverytotal_count: any;
  opentotal_count: any;
  searchTerm: string = '';
  searchResults: string[] = [];
  searchText: any;
  items = [];
  currentPage: number = 1;
  pageSize: number = 50;
  mailevent_list: any;
  whatsappmessage_list: any[] = [];
  whatsapp_list: any[] = [];
  count_list: any[] = [];
  reactiveMessageForm!: FormGroup;
  leadbank!: IWatsapp;
  file: any;

  leadbank_gid: any;
  mailid_list: any[] = [];
  mailform!: IMailform;
  NotesreactiveForm!: FormGroup;
  branchList: any[] = [];
  designation_list: any[] = [];
  country_list2: any[] = [];
  leadorderdetails_list: any[] = [];
  leadquotationdetails_list: any[] = [];
  leadinvoicedetails_list: any[] = [];
  leadcountdetails_list: any[] = [];
  messagecount_list: any;
  message_count: any;
  mail_sent: any;
  sentmailcount_list: any;
  lspage: any;
  leadbasicdetailslist: any[] = [];
  leaddocumentdetail_list: any[] = [];
  id: any;
  mailopen: boolean = true;
  mailreply: boolean = true;
  mailview_list: any;
  body: any;
  created_date: any;
  created_time: any;
  internalnotes: any;
  leadbanknotes_list: any[] = [];
  leadgig: any;
  txtinternal_notes: any;
  documentuploadlist: any[] = [];
  document_upload: any[] = [];
  file1!: FileList;
  file_name: any;
  formDataObject: FormData = new FormData();
  AutoIDkey: any;
  allattchement: any[] = [];
  reactiveFormEdit!: FormGroup;
  to_address: any;
  opencomposemail: boolean = true;
  from_address: any;
  direction: any;
  document_path: any;
  isReadOnly = true;
  orderleadbank_gid: any;
  orderlead2campaign_gid: any;
  orderleadbankcontact_gid: any;
  quotationleadbank_gid: any;
  quotationlead2campaign_gid: any;
  quotationleadbankcontact_gid: any;
  salesorder_gid: any;
  invoice_gid: any;
  quotation_gid: any;
  absURL: any;
  whatsappMessagetemplatelist: any[] = [];
  gmailmailsummary_list: any[] = [];
  outlookmailsummary_list: any[] = [];
  openDiv: boolean = false;
  filetype: string = "";
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  Enquirylist: any;
  enquiryleadbank_gid: any;
  enquirylead2campaign_gid: any;
  enquiryleadbankcontact_gid: any;
  enquiry_gid: any;
  gid_list: any[] = [];
  lead2campaign_gid: any;
  leadbankcontact_gid: any;
  parameterValue: any;
  editcustomer_list: any[] = [];
  sending_domain: any;
  receiving_domain: any;
  schedulesummary_list: any[] = [];
  assignedteamdropdown_list: any[] = [];
  myappointmentlog_list: any[] = [];
  leadbank_name: any;
  reactiveFormfollow!: FormGroup;
  myleads!: IMyleads;
  ScheduleType: any;
  phone_number: any;


  gamilreactiveForm!: FormGroup;
  gmailform!: IGmailform;
  gmail_address: any;
  gmailfiles!: File;
  base64EncodedText: any;
  lsmodule_ref: any;
  currencycodelist: any;
  symbolData: any;
  currency_list: any;
  lsmobile: any;
  leadstage_name: any;
  callresponse_list: any;
  MyLeadsCount_List: any[] = [];
  Opportunityschedulesummary_list: any[] = [];
  call_logreport: any;
  windowInterval: any;
  recording_path: any;
  reactiveForm2: any;
  campaign_gid: any;
  done: boolean = false;
  customer_gid: any;
  appointment_gid: any | null;
  appointment_gid1: any;
  searchTextnotes = '';
  enquiry_date: any;
  enquiry_count: any;
  enquiry_amount: any;
  enquiry_first_amount: any;
  enquiry_first_count: any;
  enquiry_flag: boolean = false;
  quotation_date: any;
  quotation_count: any;
  quotation_amount: any;
  quotation_first_amount: any;
  quotation_first_count: any;
  quotation_flag: boolean = false;
  order_date: any;
  order_count: any;
  order_amount: any;
  order_first_amount: any;
  order_first_count: any;
  order_flag: boolean = false;
  invoice_date: any;
  invoice_count: any;
  invoice_amount: any;
  invoice_first_amount: any;
  invoice_first_count: any;
  invoice_flag: boolean = false;
  flag2: boolean = false;
  //   enqiurymonthchart:any={};
  //   quotationmonthchart:any={};
  //  ordermonthchart:any={};
  //  invoicemonthchart:any={};
  invoicsaleschartemonthchart: any = {};
  sales_months: any;
  saleschart: any = {};
  pricesegment_list: any;
  taxsegment_list: any;
  region_list: any;
  country_list: any;
  raise_options: any = 'Customer';
  Getpricesegement_list: any[] = [];
  mail_service: any;
  mail_body: any;
  outlookmail_body:any;
  mail_subject:any;
  outlookmail_subject:any;
  GetCallLogLead_list:any;
  lsshopify_flag:any;
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.displayName.toLowerCase().includes(searchString) || item.value.toLowerCase().includes(searchString);
  }

  matchesSearchnotes(item: any): boolean {
    const searchString = this.searchTextnotes.toLowerCase();
    return item.internal_notes.toLowerCase().includes(searchString) || item.internal_notes.toLowerCase().includes(searchString);
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {
    this.leadbank = {} as IWatsapp;
    this.mailform = {} as IMailform;
    this.gmailform = {} as IGmailform;
    this.myleads = {} as IMyleads;
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });

  }

  ngOnInit(): void {

    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    const appointment_gid = this.route.snapshot.paramMap.get('appointment_gid');
    this.leadbank_gid = leadbank_gid;
    this.appointment_gid = appointment_gid;
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam3 = AES.decrypt(this.appointment_gid, secretKey).toString(enc.Utf8);
    this.leadgig = deencryptedParam;
    this.appointment_gid1 = deencryptedParam3;
    const lspage = this.route.snapshot.paramMap.get('lspage');
    this.lspage = lspage;
    const deencryptedParam1 = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    this.lspage = deencryptedParam1;
    if (this.lspage == 'MM-teleTotal' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-telepending' || this.lspage == 'MM-teleFollowup' ||
      this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleLapsedLead' ||
      this.lspage == 'MM-teleLongestLead' || this.lspage == 'My-teleNew' || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleFollowup' ||
      this.lspage == 'My-teleProspect' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-TeleAll' || this.lspage == "Mycall-schedule" || this.lspage == "MM-teleteamview") {
   
        var url = 'Leadbank360/GetLeadBasicTeleDetails'
      let param = {
        leadbank_gid: this.leadgig
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.leadbasicdetailslist = this.responsedata.leadbasicdetails_list;
        console.log('fgdj', this.leadbasicdetailslist)
        this.to_mail = this.leadbasicdetailslist[0].email;
        this.leadbank_name = this.leadbasicdetailslist[0].leadbankcontact_name;
        this.gmail_address = this.leadbasicdetailslist[0].gmail_address;
        this.lsmobile = this.leadbasicdetailslist[0].mobile;
        this.lsmodule_ref = this.leadbasicdetailslist[0].lsmodule_ref;
        this.customer_gid = this.leadbasicdetailslist[0].customer_gid;
        this.mail_service = this.leadbasicdetailslist[0].mail_service;
        this.lsshopify_flag = this.leadbasicdetailslist[0].lsshopify_flag;
        this.reactiveForm.get("potential_value")?.setValue(this.leadbasicdetailslist[0].potential_value);
        this.reactiveForm.get("customer_name")?.setValue(this.leadbasicdetailslist[0].leadbank_name);

        this.gamilreactiveForm.patchValue({

          leadbank_gid: deencryptedParam,
          gmail_to_mail: this.to_mail,
          gmail_mail_from: this.gmail_address,

        });
        this.reactiveForm.patchValue({
          // leadbank_gid:deencryptedParam,
          to_mail: this.to_mail,

        });

        // console.log(this.responsedata.leadbasicdetails_list,'leadbasicdetails_list');
      });
      this.ScheduleType = [
        { type: 'Meeting' },
        { type: 'Call' },
      ];
    }
    else {
      var url = 'Leadbank360/GetLeadBasicDetails'
      let param = {
        appointment_gid: this.appointment_gid1
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        this.responsedata = result;
        this.leadbasicdetailslist = this.responsedata.leadbasicdetails_list;
        this.to_mail = this.leadbasicdetailslist[0].email;
        this.leadbank_name = this.leadbasicdetailslist[0].leadbankcontact_name;
        this.gmail_address = this.leadbasicdetailslist[0].gmail_address;
        this.lsmobile = this.leadbasicdetailslist[0].mobile;
        this.lsmodule_ref = this.leadbasicdetailslist[0].lsmodule_ref;
        this.customer_gid = this.leadbasicdetailslist[0].customer_gid;
        this.mail_service = this.leadbasicdetailslist[0].mail_service;
        this.lsshopify_flag = this.leadbasicdetailslist[0].lsshopify_flag;
        this.reactiveForm.get("potential_value")?.setValue(this.leadbasicdetailslist[0].potential_value);
        this.reactiveForm.get("customer_name")?.setValue(this.leadbasicdetailslist[0].leadbank_name);
        this.gamilreactiveForm.patchValue({

          leadbank_gid: deencryptedParam,
          gmail_to_mail: this.to_mail,
          gmail_mail_from: this.gmail_address,

        });
        this.reactiveForm.patchValue({
          // leadbank_gid:deencryptedParam,
          to_mail: this.to_mail,

        });
        // console.log(this.responsedata.leadbasicdetails_list,'leadbasicdetails_list');
      });
      this.ScheduleType = [
        { type: 'Meeting' }
      ];
    }
    var api2 = 'Whatsapp/GetMessageTemplatesummary'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.whatsappMessagetemplatelist = result.whatsappMessagetemplatelist;
    });

    // var api2 = 'Leadbank/Getregiondropdown'
    // this.service.get(api2).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.region_list = result.regionname_list;
    // });
    var url = 'SmrTrnCustomerSummary/Getcountry'
    this.service.get(url).subscribe((result: any) => {
      this.country_list = result.Getcountry;
    });
    var api5 = 'Leadbank/Getcountrynamedropdown'
    this.service.get(api5).subscribe((result: any) => {
      this.responsedata = result;
      this.country_list2 = result.country_list;
    });
    var url = 'SmrTrnCustomerSummary/getcurrencydtl'
    this.service.get(url).subscribe((result: any) => {
      this.currency_list = result.getcurrencydtl_list;
    });
    var url = 'SmrMstTaxSegment/GetTaxSegmentSummary'
    this.service.get(url).subscribe((result: any) => {
      this.taxsegment_list = result.TaxSegmentSummary_list;
    });
    var url = 'SmrTrnCustomerSummary/Getpricesegment'
    this.service.get(url).subscribe((result: any) => {
      this.pricesegment_list = result.pricesegment_list;
    });
    //console.log('enwfkj',this.pricesegment_list)
    this.GetMailSummary(deencryptedParam);
    this.GetGmailMailSummary(deencryptedParam);
    this.GetOutlookMailSummary(deencryptedParam);
    this.GetOrderDetailsSummary(deencryptedParam);
    this.GetQuotationDetailsSummary(deencryptedParam);
    this.GetInvoiceDetailsSummary(deencryptedParam);
    this.GetLeadCountDetails(deencryptedParam);
    this.GetWhatsappMessageSummary(deencryptedParam);
    this.GetWhatsappSummary(deencryptedParam);
    this.GetEnquiryDetailsSummary(deencryptedParam);
    this.GetGiddetails(deencryptedParam);
    this.Getshedulesummary(deencryptedParam);
    this.Get360Gmailsummary(deencryptedParam);
    this.getlogreport(deencryptedParam);
    this.GetLeadNotesSummary(deencryptedParam);
    this.GetLeadAppointmentLog(deencryptedParam);
    //this.Getpricesegement(this.customer_gid);
    this.GetLeadDocumentDetails();
    this.viewcalllog();
    let yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    this.fromDate = this.datePipe.transform(yesterday, 'dd-MM-yyyy');
    this.toDate = this.datePipe.transform(new Date(), 'dd-MM-yyyy');
    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);
    this.subscription = timer(0, 1000)
      .pipe(
        map(() => new Date()),
        share()
      )
      .subscribe(time => {
        let hour = this.rxTime.getHours();
        let minuts = this.rxTime.getMinutes();
        let seconds = this.rxTime.getSeconds();
        this.rxTime = time;
      });

    this.reactiveForm = new FormGroup({

      customer_name: new FormControl(this.leadbank.customer_name, [
        Validators.required,
      ]),
      displayName: new FormControl(this.leadbank.displayName, [
        Validators.required,
      ]),
      mobile: new FormControl(''),
      potential_value: new FormControl(''),
      value: new FormControl(this.leadbank.value, [
        Validators.required,
      ]),
      phone: new FormControl(this.leadbank.phone, [
        Validators.required,]),
      gender: new FormControl(''),
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      email: new FormControl(''),
      address1: new FormControl(''),
      leadbank_address1: new FormControl(''),
      leadbank_address2: new FormControl(''),
      country_name: new FormControl(''),
      leadbank_state: new FormControl(''),
      leadbank_city: new FormControl(''),
      leadbank_pin: new FormControl(''),
      address2: new FormControl(''),
      //region_name: new FormControl(null),
      customer_city: new FormControl(''),
      currency: new FormControl(null),
      postal_code: new FormControl(''),
      countryname: new FormControl(null),
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''),
      leadbank_gid: new FormControl(''),
      to_mail: new FormControl(''),
      mail_from: new FormControl(''),
      moving_stage: new FormControl(''),
      contact: new FormControl(''),
      appointment_gid: new FormControl(''),
      taxsegment_name: new FormControl(null),

    });
    this.reactiveFormdrop = new FormGroup({
      drop_remarks: new FormControl(''),
    });
    this.gamilreactiveForm = new FormGroup({


      gmail_sub: new FormControl(this.gmailform.gmail_sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      gmail_body: new FormControl(''),
      leadbank_gid: new FormControl(''),
      gmail_to_mail: new FormControl(''),
      gmail_mail_from: new FormControl(''),
      base64EncodedText: new FormControl(''),
    });
    this.reactiveMessageForm = new FormGroup({
      identifierValue: new FormControl(''),
      type: new FormControl(''),
      sendtext: new FormControl(''),
      template_name: new FormControl(''),
      p_name: new FormControl(''),
      project_id: new FormControl(),
      version: new FormControl(''),
      message_id: new FormControl(''),
      contact_id: new FormControl(''),
    });



    this.NotesreactiveForm = new FormGroup({
      leadgig: new FormControl(''),
      internalnotestext_area: new FormControl(''),
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      document_title: new FormControl(''),
      remarks: new FormControl(''),
      s_no: new FormControl(''),
    });
    this.reactiveFormfollow = new FormGroup({
      schedule_date: new FormControl(this.myleads.schedule_date, [
        Validators.required,
      ]),
      schedule_time: new FormControl(this.myleads.schedule_time, [
        Validators.required,
      ]),
      schedule_type: new FormControl(this.myleads.schedule_type, [
        Validators.required,
      ]),
      schedule_remarks: new FormControl(''),
      ScheduleRemarks1: new FormControl(''),
      leadbank_gid: new FormControl(''),
      lead2campaign_gid: new FormControl(''),
      log_details: new FormControl(''),
      log_legend: new FormControl(''),
      appointment_gid: new FormControl(''),

    });

    this.reactiveForm2 = new FormGroup({
      call_response: new FormControl(this.myleads.call_response, [
        Validators.required,
        // Validators.pattern( '^[a-zA-Z]+$')
      ]),
      leadbank_gid: new FormControl(this.leadgig, []),
      team_gid: new FormControl(''),
      appointment_gid: new FormControl(''),
      pricesegment_gid: new FormControl(null),
      //sales_stages: new FormControl(''),
    });

    this.reactiveForm.patchValue({
      leadbank_gid: deencryptedParam,
      // mail_from: this.to_mail,

    });
    this.reactiveFormfollow.patchValue({
      leadbank_gid: deencryptedParam,
      // mail_from: this.to_mail,

    });

    const options: Options = {
      // enableTime: true,
      dateFormat: 'Y-m-d',

    };
    flatpickr('.date-picker', options);

  }
  GetLeadDocumentDetails() {
    var url = 'Leadbank360/GetLeadDocumentDetails'
    let params = {
      leadbank_gid: this.leadgig
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.leaddocumentdetail_list = this.responsedata.leaddocumentdetails;
    });
  }

  Updatepricesegement() {
    var url = 'Leadbank360/Updatepricesegement'
    let param = {
      customer_gid: this.customer_gid,
      pricesegment_gid: this.reactiveForm2.value.pricesegment_gid
    }

    this.NgxSpinnerService.show();
    this.service.post(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);

      }
    });
    this.NgxSpinnerService.hide();
  }
  Getpricesegement() {
    var url = 'Leadbank360/Getpricesegement';
    this.NgxSpinnerService.show();
    var params = {
      customer_gid: this.customer_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.reactiveForm2.get("pricesegment_gid")?.setValue(result.pricesegment_gid);
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }
  stagechange() {
    if (this.reactiveForm2.value.call_response == 'Prospect') {
      this.done = true

    }
    else {
      this.done = false

    }
  }
  //GetGidDetails
  GetGiddetails(deencryptedParam: any) {
    if (this.lspage == 'MM-teleTotal' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-telepending' || this.lspage == 'MM-teleFollowup' ||
      this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleLapsedLead' ||
      this.lspage == 'MM-teleLongestLead' || this.lspage == 'My-teleNew' || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleFollowup' ||
      this.lspage == 'My-teleProspect' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-TeleAll' || this.lspage == 'Mycall-schedule' || this.lspage == "MM-teleteamview") {
      var url = 'Leadbank360/GetTeleGidDetails'
      let param = {
        leadbank_gid: deencryptedParam
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        // $('#leadorderdetails_list').DataTable().destroy();
        this.responsedata = result;
        this.gid_list = this.responsedata.gid_list;

        this.leadbank_gid = this.gid_list[0].leadbank_gid;
        this.lead2campaign_gid = this.gid_list[0].lead2campaign_gid;
        this.leadbankcontact_gid = this.gid_list[0].leadbankcontact_gid;
        this.leadstage_name = this.gid_list[0].leadstage_name;
        this.campaign_gid = this.gid_list[0].campaign_gid;

        //this.salesorder_gid = this.gid_list[0].salesorder_gid;

        setTimeout(() => {
          $('#leadorderdetails_list').DataTable();
        }, 1);
        // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list'); 
      });
    }
    else {
      var url = 'Leadbank360/GetGidDetails'
      let param = {
        leadbank_gid: this.appointment_gid1
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        // $('#leadorderdetails_list').DataTable().destroy();
        this.responsedata = result;
        this.gid_list = this.responsedata.gid_list;

        this.leadbank_gid = this.gid_list[0].leadbank_gid;
        this.lead2campaign_gid = this.gid_list[0].lead2campaign_gid;
        this.leadbankcontact_gid = this.gid_list[0].leadbankcontact_gid;
        this.leadstage_name = this.gid_list[0].leadstage_name;
        this.campaign_gid = this.gid_list[0].campaign_gid;

        //this.salesorder_gid = this.gid_list[0].salesorder_gid;

        setTimeout(() => {
          $('#leadorderdetails_list').DataTable();
        }, 1);
        // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list'); 
      });
    }
  }

  GetOrderDetailsSummary(deencryptedParam: any) {
    // console.log(deencryptedParam,'testleadbank_gid');
    var url = 'Leadbank360/GetLeadOrderDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#leadorderdetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadorderdetails_list = this.responsedata.leadorderdetailslist;

      this.orderleadbank_gid = this.leadorderdetails_list[0].leadbank_gid;
      this.orderlead2campaign_gid = this.leadorderdetails_list[0].lead2campaign_gid;
      this.orderleadbankcontact_gid = this.leadorderdetails_list[0].leadbankcontact_gid;
      this.salesorder_gid = this.leadorderdetails_list[0].salesorder_gid;

      setTimeout(() => {
        $('#leadorderdetails_list').DataTable();
      }, 1);
      // console.log(this.responsedata.leadorderdetailslist,'leadorderdetails_list'); 
    });
  }

  GetEnquiryDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetEnquiryDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Enquirylist = this.responsedata.Enquiry_list;
      console.log('oeiwj', this.Enquirylist)
      // this.enquiryleadbank_gid = this.Enquirylist[0].leadbank_gid;
      // this.enquirylead2campaign_gid = this.Enquirylist[0].lead2campaign_gid;
      // this.enquiryleadbankcontact_gid = this.Enquirylist[0].leadbankcontact_gid;
      this.enquiry_gid = this.Enquirylist[0].quotation_gid;

      setTimeout(() => {
        $('#Enquirylist').DataTable();
      }, 1);
      // console.log(this.responsedata.leadquotationdetailslist,'leadquotationdetails_list'); 
    });
  }

  GetQuotationDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadQuotationDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      // $('#leadquotationdetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadquotationdetails_list = this.responsedata.leadquotationdetailslist;

      this.quotationleadbank_gid = this.leadquotationdetails_list[0].leadbank_gid;
      this.quotationlead2campaign_gid = this.leadquotationdetails_list[0].lead2campaign_gid;
      this.quotationleadbankcontact_gid = this.leadquotationdetails_list[0].leadbankcontact_gid;
      this.quotation_gid = this.leadquotationdetails_list[0].quotation_gid;

      setTimeout(() => {
        $('#leadquotationdetails_list').DataTable();
      }, 1);
    });
  }
  // GetLeadCountDetails(deencryptedParam: any) {
  //   var url = 'Leadbank360/GetLeadCountDetails'
  //   let param = {
  //     leadbank_gid: deencryptedParam
  //   }
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.leadcountdetails_list = this.responsedata.leadsaleschart;
  //     console.log('om=pk',this.leadcountdetails_list)
  //     this.enquiry_date = this.leadcountdetails_list
  //     .filter((entry: { source: string }) => entry.source === 'ENQUIRY')
  //     .map((entry: { dates: any }) => entry.dates || 0);
  //   this.enquiry_count = this.leadcountdetails_list
  //     .filter((entry: { source: string }) => entry.source === 'ENQUIRY')
  //     .map((entry: { count: number }) => entry.count || 0);
  //   this.enquiry_amount = this.leadcountdetails_list
  //     .filter((entry: { source: string }) => entry.source === 'ENQUIRY')
  //     .map((entry: { amount: string }) => parseInt(entry.amount) || 0)
  //     .reduce((sum, amount) => sum + amount, 0);
  //   if (this.enquiry_date != 0 || this.enquiry_count != 0) {
  //     this.enquiry_flag = true;
  //     this.enquiry_first_amount = this.enquiry_amount.toLocaleString('en-US');
  //     this.enquiry_first_count = this.leadcountdetails_list
  //     .filter((entry: { source: string }) => entry.source === 'ENQUIRY')
  //     .map((entry: { count: string }) =>parseInt(entry.count) || 0)
  //     .reduce((sum, amount) => sum + amount, 0);
  //   }
  //     this.quotation_date = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'QUOTATION')
  //       .map((entry: { dates: any }) => entry.dates || 0);
  //     this.quotation_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'QUOTATION')
  //       .map((entry: { count: number }) => entry.count || 0);
  //     this.quotation_amount = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'QUOTATION')
  //       .map((entry: { amount: string }) => parseInt(entry.amount) || 0)
  //       .reduce((sum, amount) => sum + amount);
  //     if (this.quotation_date != 0 || this.quotation_count != 0) {
  //       this.quotation_flag = true;
  //       this.quotation_first_amount = this.quotation_amount.toLocaleString('en-US');
  //       this.quotation_first_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'QUOTATION')
  //       .map((entry: { count: string }) =>parseInt(entry.count) || 0)
  //       .reduce((sum, amount) => sum + amount, 0);
  //     }
  //     this.order_date = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'ORDER')
  //       .map((entry: { dates: any }) => entry.dates || 0);
  //     this.order_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'ORDER')
  //       .map((entry: { count: number }) => entry.count || 0);
  //     this.order_amount = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'ORDER')
  //       .map((entry: { amount: string }) => parseInt(entry.amount) || 0)
  //       .reduce((sum, amount) => sum + amount);
  //     if (this.order_date != 0 || this.order_count != 0) {
  //       this.order_flag = true;
  //       this.order_first_amount = this.order_amount.toLocaleString('en-US');
  //       this.order_first_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'ORDER')
  //       .map((entry: { count: string }) =>parseInt(entry.count) || 0)
  //       .reduce((sum, amount) => sum + amount, 0);
  //     }
  //     this.invoice_date = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'INVOICE')
  //       .map((entry: { dates: any }) => entry.dates || 0);
  //     this.invoice_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'INVOICE')
  //       .map((entry: { count: number }) => entry.count || 0);
  //     this.invoice_amount = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'INVOICE')
  //       .map((entry: { amount: string }) =>parseInt(entry.amount) || 0)
  //       .reduce((sum, amount) => sum + amount);
  //     if (this.invoice_date != 0 || this.invoice_count != 0) {
  //       this.invoice_flag = true;
  //       this.invoice_first_amount = this.invoice_amount.toLocaleString('en-US');

  //       this.invoice_first_count = this.leadcountdetails_list
  //       .filter((entry: { source: string }) => entry.source === 'INVOICE')
  //       .map((entry: { count: string }) =>parseInt(entry.count) || 0)
  //       .reduce((sum, amount) => sum + amount, 0);
  //     }
  //     this.enqiurymonthchart = {
  //       chart: {
  //         type: 'area',
  //         height: 100,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         fontFamily: 'inherit',
  //         toolbar: {
  //           show: false,
  //         },
  //         sparkline: {
  //           enabled: true
  //         },
  //       },
  //       colors: ['#96bf48'],
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '50%',
  //           borderRadius: 0,
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false,
  //       },
  //       xaxis: {
  //         type: 'datetime',
  //       },
  //       yaxis: {
  //         min: 0,
  //       },
  //       series: [{
  //         name: 'Enquiry',
  //         data: randomizeArray1(this.enquiry_count),
  //       }],
  //       labels: Array.from(this.enquiry_date),
  //     };
  //     this.quotationmonthchart = {
  //       chart: {
  //         type: 'area',
  //         height: 100,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         fontFamily: 'inherit',
  //         toolbar: {
  //           show: false,
  //         },
  //         sparkline: {
  //           enabled: true
  //         },
  //       },
  //       colors: ['#7FC7D9'],
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '50%',
  //           borderRadius: 0,
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false,
  //       },
  //       xaxis: {
  //         type: 'datetime',
  //       },
  //       yaxis: {
  //         min: 0,
  //       },
  //       series: [{
  //         name: 'Quotation',
  //         data: randomizeArray2(this.quotation_count),
  //       }],
  //       labels: Array.from(this.quotation_date),
  //     };
  //     this.ordermonthchart = {
  //       chart: {
  //         type: 'area',
  //         height: 100,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         fontFamily: 'inherit',
  //         toolbar: {
  //           show: false,
  //         },
  //         sparkline: {
  //           enabled: true
  //         },
  //       },
  //       colors: ['#25D366'],
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '50%',
  //           borderRadius: 0,
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false,
  //       },
  //       xaxis: {
  //         type: 'datetime',
  //       },
  //       yaxis: {
  //         min: 0,
  //       },
  //       series: [{
  //         name: 'Order',
  //         data: randomizeArray3(this.order_count),
  //       }],
  //       labels: Array.from(this.order_date),
  //     };
  //     this.invoicemonthchart = {
  //       chart: {
  //         type: 'area',
  //         height: 100,
  //         background: 'White',
  //         foreColor: '#0F0F0F',
  //         fontFamily: 'inherit',
  //         toolbar: {
  //           show: false,
  //         },
  //         sparkline: {
  //           enabled: true
  //         },
  //       },
  //       colors: ['#EF5350'],
  //       plotOptions: {
  //         bar: {
  //           horizontal: false,
  //           columnWidth: '50%',
  //           borderRadius: 0,
  //         },
  //       },
  //       dataLabels: {
  //         enabled: false,
  //       },
  //       xaxis: {
  //         type: 'datetime',
  //       },
  //       yaxis: {
  //         min: 0,
  //       },
  //       series: [{
  //         name: 'Invoice',
  //         data: randomizeArray4(this.invoice_count),
  //       }],
  //       labels: Array.from(this.invoice_date),
  //     };

  //   });
  // }
  GetLeadCountDetails(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadCountDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.response_data = result;
      this.leadcountdetails_list = this.response_data.leadsaleschart;
      if (this.leadcountdetails_list.length > 0) {
        this.flag2 = true;
      }
      this.enquiry_count = this.leadcountdetails_list.map((entry: { enquiry_count: any }) => entry.enquiry_count)
      this.quotation_count = this.leadcountdetails_list.map((entry: { quotation_count: any }) => entry.quotation_count)
      this.order_count = this.leadcountdetails_list.map((entry: { order_count: any }) => entry.order_count)
      this.invoice_count = this.leadcountdetails_list.map((entry: { invoice_count: any }) => entry.invoice_count)
      this.sales_months = this.leadcountdetails_list.map((entry: { Months: any }) => entry.Months)

      this.saleschart = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: true,
          },
        },
        colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.sales_months,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Enquiry',
            data: this.enquiry_count,
            color: '#9EBF95',
          },
          {
            name: 'Quotation',
            data: this.quotation_count,
            color: '#8C8C8C',
          },
          {
            name: 'Order',
            data: this.order_count,
            color: '#F2D377',
          },
          {
            name: 'Invoice',
            data: this.invoice_count,
            color: '#48a363',
          },

        ],
        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
  }
  GetLeadAppointmentLog(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadAppointmentLog'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.myappointmentlog_list = this.responsedata.myappointmentlog_list;
    });
  }
  GetInvoiceDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadInvoiceDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#leadinvoicedetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadinvoicedetails_list = this.responsedata.leadinvoicedetailslist;

      //this.invoice_gid = this.leadinvoicedetails_list[0].invoice_gid;
      setTimeout(() => {
        $('#leadinvoicedetails_list').DataTable();
      }, 1);
      // console.log(this.responsedata.leadinvoicedetailslist,'leadinvoicedetails_list'); 
    });
  }
  Mail_routesgmail() {
    sessionStorage.setItem('CRM_TOMAILID',this.to_mail)
    sessionStorage.setItem('CRM_LEADBANK_GID_ENQUIRY',this.leadgig)
    this.router.navigate(['/crm/CrmSmmMailscompose']);

  }
  Mail_routesoutlook() {
    sessionStorage.setItem('CRM_TOMAILID',this.to_mail)
    sessionStorage.setItem('CRM_LEADBANK_GID_ENQUIRY',this.leadgig)
    this.router.navigate(['/crm/CrmSmmOutlookmailcompose']);

  }

  //////////////////Mail functions Starts///////////////////
  GetMailSummary(deencryptedParam: any) {
    let params = {
      leadbank_gid: deencryptedParam
    }
    var api = 'MailCampaign/GetIndividualMailSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.response_data = result;
      this.mailsummary_list = this.response_data.mailsummary_list;
      this.sending_domain = this.response_data.sending_domain;
      this.receiving_domain = this.response_data.receiving_domain;
      this.reactiveForm.patchValue({
        mail_from: this.sending_domain,
      });
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);
    });
  }
  GetGmailMailSummary(deencryptedParam: any) {
    let params = {
      leadbank_gid: deencryptedParam
    }
    var api = 'GmailCampaign/GmailindividualSenditemSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.response_data = result;
      this.gmailmailsummary_list = this.response_data.gmailsenditemsummary_list;
      this.sending_domain = this.response_data.sending_domain;
      this.receiving_domain = this.response_data.receiving_domain;
      this.reactiveForm.patchValue({
        mail_from: this.sending_domain,
      });
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);
    });
  }
  GetOutlookMailSummary(deencryptedParam: any) {
    let params = {
      leadbank_gid: deencryptedParam
    }
    var api = 'OutlookCampaign/OutlooklindividualSenditemSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.response_data = result;
      this.outlookmailsummary_list = this.response_data.outlooksentMail_list;
      this.sending_domain = this.response_data.sending_domain;
      this.receiving_domain = this.response_data.receiving_domain;
      this.reactiveForm.patchValue({
        mail_from: this.sending_domain,
      });
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);
    });
  }
  popmodal(parameter: string,parameter1:string) {
      this.mail_body = parameter; // Access parameter directly
      this.mail_subject = parameter1; 
    }
    popmodal1(parameter: string,parameter1:string) {
      this.outlookmail_body = parameter;
      this.outlookmail_subject = parameter1; 

       // Access parameter directly
    }
  onbackmail() {
    this.mailopen = true;
    this.mailreply = true

  }
  viewpage(mailmanagement_gid: any) {
    this.mailopen = !this.mailopen;
    var url = 'MailCampaign/GetMailView';
    let param = {
      mailmanagement_gid: mailmanagement_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.mailview_list = result.mailsummary_list;
      this.from_mail = this.mailview_list[0].mail_from;
      this.subject = this.mailview_list[0].sub;
      this.body = this.mailview_list[0].body;
      this.created_date = this.mailview_list[0].created_date;
      this.direction = this.mailview_list[0].direction;
      this.document_path = this.mailview_list[0].document_path;
      this.created_time = this.mailview_list[0].created_time;
      this.to_mail = this.mailview_list[0].to;

    });

  }
  onreply() {
    this.mailreply = !this.mailreply;

  }
  onChange2($event: any): void {
    this.file1 = $event.target.files;

    if (this.file1 != null && this.file1.length !== 0) {
      for (let i = 0; i < this.file1.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.file1[i]);
        this.file_name = this.file1[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.file1[i].name
        });
        //console.log(this.file1[i]);
      }
    }

    //console.log(this.files[i]);
  }
  onChange4(event: any) {
    this.file = event.target.files[0];
  }


  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }
  setFileType(data: string) {
    this.filetype = data;
  }

  popattachments() {
    this.opencomposemail = !this.opencomposemail;
  }

  public onadd(): void {
    // console.log(this.reactiveForm.value)
    this.mailform = this.reactiveForm.value;
    if (this.mailform.sub != null) {
      const allattchement = "" + JSON.stringify(this.allattchement) + "";
      if (this.file1 != null && this.file1 != undefined) {
        this.formDataObject.append("filename", allattchement);
        this.formDataObject.append("mail_from", this.mailform.mail_from);
        this.formDataObject.append("sub", this.mailform.sub);
        this.formDataObject.append("to", this.mailform.to_mail);
        this.formDataObject.append("body", this.mailform.body);
        this.formDataObject.append("bcc", this.mailform.bcc);
        this.formDataObject.append("cc", this.mailform.cc);
        // this.formDataObject.append("reply_to", this.mailform.reply_to);
        this.formDataObject.append("leadbank_gid", this.mailform.leadbank_gid);
        var api7 = 'MailCampaign/MailUpload'
        this.service.post(api7, this.formDataObject).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)

          }
          else {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
            this.ToastrService.success(result.message)
          }
        });
      }
      else {
        var api7 = 'MailCampaign/MailSend'
        //console.log(this.file)
        this.service.post(api7, this.mailform).subscribe((result: any) => {

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
            // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
            this.ToastrService.success(result.message)
          }
          this.responsedata = result;
        });
      }
    }

    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    return;

  }
  get mail_from() {
    return this.reactiveForm.get('mail_from')!;
  }
  get to() {
    return this.reactiveForm.get('to')!;
  }
  get sub() {
    return this.reactiveForm.get('sub')!;
  }
  get reply_to() {
    return this.reactiveForm.get('reply_to')!;
  }
  get cc() {
    return this.reactiveForm.get('cc')!;
  }
  get bcc() {
    return this.reactiveForm.get('bcc')!;
  }
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get address1() {
    return this.reactiveForm.get('address1')!;
  }
  get address2() {
    return this.reactiveForm.get('address2')!;
  }
  // get region_name() {
  //   return this.reactiveForm.get('region_name')!;
  // }
  get currency() {
    return this.reactiveForm.get('currency')!;
  }
  get postal_code() {
    return this.reactiveForm.get('postal_code')!;
  }
  get countryname() {
    return this.reactiveForm.get('countryname')!;
  }
  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  get drop_remarks() {
    return this.reactiveFormdrop.get('drop_remarks')!;
  }

  get gmail_sub() {
    return this.gamilreactiveForm.get('gmail_sub')!;
  }
  get gmail_body() {
    return this.gamilreactiveForm.get('gmail_body')!;
  }

  get call_response() {
    return this.reactiveForm2.get('call_response')!;
  }
  get team_gid() {
    return this.reactiveForm2.get('team_gid')!;
  }




  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    width: '100%',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'insertImage',
        'insertVideo',
        'removeFormat',
        'toggleEditorMode'
      ]
    ]
  };
  config_compose_mail: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '120px',
    minHeight: '0rem',
    width: '1013px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'insertImage',
        'insertVideo',
        'removeFormat',
        'toggleEditorMode'
      ]
    ]

  };

  //////////////////Mail functions Ends///////////////////
  //////////////////////////////gmail starts///////////////

  ongamilChange1(event: any) {

    this.gmailfiles = event.target.files[0];

    let reader = new FileReader();
    reader.onload = (e) => {
      if (e.target && e.target.result) { // Check if e.target exists
        // Cast e.target.result to string
        let base64String = e.target.result as string;
        // Append the base64 content to formDataObject
        // console.log(base64String)
        this.formDataObject.append("gmailfiles", base64String);
      }
    };
    reader.readAsDataURL(this.gmailfiles);
  }

  public onaddmail(): void {
    // console.log(this.gamilreactiveForm.value)
    this.gmailform = this.gamilreactiveForm.value;
    if (this.gmailfiles == null) {
      const originalText =
        'From: ' + this.gmailform.gmail_mail_from + '\r\nTo: ' + this.gmailform.gmail_to_mail + '\r\nSubject:' + this.gmailform.gmail_sub + '\r\nContent-Type: text/html\r\n\r\n' + this.gmailform.gmail_body + '';

      this.base64EncodedText = encodeToBase64(originalText);
      this.gamilreactiveForm.patchValue({

        base64EncodedText: this.base64EncodedText,


      });
      //console.log('Base64 encoded text:',this.base64EncodedText);
      var api7 = 'Leadbank360/Gmailtext'
      this.service.post(api7, this.gamilreactiveForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();


        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();

        }
      });
    }
    else if (this.gmailform.gmail_mail_from != null && this.gmailform.gmail_sub != null && this.gmailform.gmail_to_mail != null) {
      // const allattchement = "" + JSON.stringify(this.allattchement) + "";
      this.formDataObject.append("gmailfiles", this.gmailfiles || null);
      this.formDataObject.append("gmail_mail_from", this.gmailform.gmail_mail_from);
      this.formDataObject.append("gmail_sub", this.gmailform.gmail_sub);
      this.formDataObject.append("gmail_to_mail", this.gmailform.gmail_to_mail);
      this.formDataObject.append("gmail_body", this.gmailform.gmail_body);
      this.formDataObject.append("leadbank_gid", this.gmailform.leadbank_gid);

      this.NgxSpinnerService.show();

      var api7 = 'Leadbank360/Gmailupload'
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();


        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();

        }
      });
      this.NgxSpinnerService.hide();


      return;

    }
  }
  Get360Gmailsummary(deencryptedParam: any) {
    let params = {
      leadbank_gid: deencryptedParam
    }
    var api = 'GmailCampaign/Get360Gmailsummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.response_data = result;
      this.mailsummary_list = this.response_data.gmailtemplatesendsummary_list;
      //this.sending_domain = this.response_data.sending_domain;
      //this.receiving_domain = this.response_data.receiving_domain;
      // this.reactiveForm.patchValue({
      //   mail_from: this.sending_domain,
      // });
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);
    });
  }
  gmailviewpage(gmail_gid: any, deencryptedParam: any) {
    //this.mailopen = !this.mailopen;
    var url = 'MailCampaign/GetMailView';
    let param = {
      gmail_gid: gmail_gid,
      //leadbank_gid:deencryptedParam
    }
    const secretKey = 'storyboarderp';
    //console.log(params)
    gmail_gid = AES.encrypt(param.gmail_gid, secretKey).toString();

    this.router.navigate(['/crm/CrmSmmGmailview', gmail_gid, deencryptedParam])

    // this.service.getparams(url, param).subscribe((result: any) => {
    //   this.mailview_list = result.gmailtemplatesendsummary_list;
    //   this.from_mail = this.mailview_list[0].mail_from;
    //   this.subject = this.mailview_list[0].sub;
    //   this.body = this.mailview_list[0].body;
    //   this.created_date = this.mailview_list[0].created_date;
    //   this.direction = this.mailview_list[0].direction;
    //   this.document_path = this.mailview_list[0].document_path;
    //   this.created_time = this.mailview_list[0].created_time;
    //   this.to_mail = this.mailview_list[0].to;

    // });

  }
  //////////////////////////////gmail ends///////////////


  GetWhatsappMessageSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetWhatsappLeadMessage'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#whatsmessagelist').DataTable().destroy();
      this.responsedata = result;
      this.whatsappmessage_list = this.responsedata.leadwhatsmessagelist;
      this.GetWhatsappSummary(deencryptedParam);
      this.GetWhatsappMessageSummary(deencryptedParam);
    });
  }

  GetWhatsappSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetWhatsappLeadContact'
    let param = {
      leadbank_gid: deencryptedParam
    }
    var url = 'Leadbank360/GetWhatsappLeadContact'
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#whatsnamelist').DataTable().destroy();
      this.responsedata = result;
      this.whatsapp_list = this.responsedata.leadwhatscontactlist;
    });
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    // Check if the clicked element is not the button or notification area
    if (!event.target || !(event.target as HTMLElement).closest('#notification') && !(event.target as HTMLElement).closest('.sampel')) {
      // Toggle the notification off
      this.openDiv = false;
    }
  }

  showResponsiveOutput(gid: string) {
    this.GetWhatsappMessageSummary(gid);
  }

  GetContactCount() {
    var url = 'Whatsapp/GetContactCount'
    this.service.get(url).subscribe((result: any) => {
      $('#count_list').DataTable().destroy();
      this.responsedata = result;
      this.count_list = this.responsedata.contactcount_list;
      //console.log(this.source_list)

    });
  }

  onclose() {
    this.reactiveForm.get("displayName")?.setValue('');
    this.reactiveForm.get("email")?.setValue('');
    this.reactiveForm.get("mobile")?.setValue('');
    this.reactiveForm.get("address1")?.setValue('');
    this.reactiveForm.get("leadbank_gid")?.setValue('');
    this.reactiveForm.get("taxsegment_name")?.setValue('');
  }

  onChange1(event: any) {
    this.file = event.target.files[0];

  }

  isDropdownOpen = false;

  sendMessage() {
    // Add your send message logic here
  }
  poptemplatemodal(parameter: string) {
    this.reactiveMessageForm.get("identifierValue")?.setValue(parameter);

  }
  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }



  get customer_name() {
    return this.reactiveForm.get('customer_name')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get value() {
    return this.reactiveForm.get('mobile')!;
  }
  get firstName() {
    return this.reactiveForm.get('customer_name')!;
  }
  get lastName() {
    return this.reactiveForm.get('mobile')!;
  }
  get displayName() {
    return this.reactiveForm.get('mobile')!;
  }
  get identifierValue() {
    return this.reactiveMessageForm.get('identifierValue')!;
  }
  get document_title() {
    return this.NotesreactiveForm.get('document_title')!;
  }
  get leadbank_pin() {
    return this.reactiveForm.get('leadbank_pin')!;
  }
  get country_name() {
    return this.reactiveForm.get('country_name')!;
  }
  attachments() {
    this.openDiv = !this.openDiv;
  }
  public onsubmit(): void {
    if (this.reactiveForm.value.displayName != null && this.reactiveForm.value.mobile.e164Number != null
      && this.reactiveForm.value.email != null && this.reactiveForm.value.leadbank_address1) {

      this.reactiveForm.value;
      console.log('ewkn', this.reactiveForm.value)
      var url = 'Leadbank360/Getupdatecontactdetails'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message);
          window.location.reload();
          this.reactiveForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message);
          window.location.reload();
          this.reactiveForm.reset();
        }
        this.reactiveForm.reset();
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  public onaddpotentialvalue(): void {
    if (this.reactiveForm.value.potential_value) {
      this.reactiveForm.get('appointment_gid')?.setValue(this.appointment_gid1);
      this.reactiveForm.value;
      var url = 'Leadbank360/PotentialValue'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message);
          window.location.reload();
          this.reactiveForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message);
          window.location.reload();
          this.reactiveForm.reset();
        }
        this.reactiveForm.reset();
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  // Message send //
  public onMessagesent(gid: string, id: string): void {
    this.reactiveMessageForm.value.identifierValue = gid;
    this.reactiveMessageForm.value.contact_id = id;var sendtexts = this.reactiveMessageForm.value.sendtext; 
    if (sendtexts != null && sendtexts != '') {

      const identifierValue = this.reactiveMessageForm?.get('identifierValue')?.value;

      this.reactiveMessageForm.value;

      var url = 'Whatsapp/WhatsappSend'
      this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
          this.reactiveMessageForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          //this.GetWhatsappMessageSummary();
          this.reactiveMessageForm.reset();
        }
        // this.GetSourceSummary();
        this.GetWhatsappMessageSummary(this.leadgig);
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Please Type a Message to Continue')
    }

  }
  public onTemplatesent(id: string, version: string): void {

    this.reactiveMessageForm.get("project_id")?.setValue(id);
    this.reactiveMessageForm.get("version")?.setValue(version);
    let identifierValue = this.reactiveMessageForm.value.identifierValue;
    let project_id = id;
    let param = {
      identifierValue: identifierValue,
      project_id: project_id,
    }

    if (project_id != null) {
      this.reactiveMessageForm.value.param = param

      var url = 'Whatsapp/WhatsappSend'
      this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.reactiveMessageForm.reset();
          //this.GetWhatsappMessageSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.reactiveMessageForm.reset();
        }
        this.GetWhatsappMessageSummary(this.leadgig);
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  onback() {

    window.history.back();
  }

  public onsubmitlog1(): void {
    if (this.reactiveForm2.value.call_response != null) {
      if (this.lspage == 'MM-teleTotal' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-telepending' || this.lspage == 'MM-teleFollowup' ||
        this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleLapsedLead' ||
        this.lspage == 'MM-teleLongestLead' || this.lspage == 'My-teleNew' || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleFollowup' ||
        this.lspage == 'My-teleProspect' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-TeleAll' || this.lspage == "MM-teleteamview") {

        var url = 'Leadbank360/PostteleLeadStage'
        this.service.post(url, this.reactiveForm2.value).pipe().subscribe((result: any) => {
          if (result.status == false) {
            window.location.reload();
            this.ToastrService.warning(result.message)
            this.reactiveForm2.reset();
          }
          else {
            window.location.reload()
            this.ToastrService.success(result.message)

            this.reactiveForm2.reset();
          }

          this.reactiveForm2.reset();
        });
      }
      else {
        this.reactiveForm2.get('appointment_gid')?.setValue(this.appointment_gid1);
        var url = 'Leadbank360/PostLeadStage'
        this.service.post(url, this.reactiveForm2.value).pipe().subscribe((result: any) => {
          if (result.status == false) {
            window.location.reload()
            this.ToastrService.warning(result.message)
            this.reactiveForm2.reset();
          }
          else {
            window.location.reload()
            this.ToastrService.success(result.message)

            this.reactiveForm2.reset();
          }

          this.reactiveForm2.reset();
        });
      }

    }
    else {
      this.ToastrService.warning('Kindly Select Stage!! ')
    }
  }

  ///////////////////////////////////////////////notes 25.07.2024 start ///////////////////////
  GetLeadNotesSummary(deencryptedParam: any) {
    if (this.lspage == 'My-TeleAll' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-teleProspect' || this.lspage == 'My-teleFollowup'
      || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleNew' || this.lspage == 'MM-teleLongestLead' || this.lspage == 'MM-teleLapsedLead'
      || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleFollowup' ||
      this.lspage == 'MM-telepending' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-teleTotal' || this.lspage == "MM-teleteamview") {
      var url = 'Leadbank360/GetTeleLeadNotesSummary'
      let params1 = {
        leadbank_gid: deencryptedParam
      }
      this.service.getparams(url, params1).subscribe((result: any) => {
        this.responsedata = result;
        this.internalnotes = this.responsedata.notes;
        //this.NotesreactiveForm.get("internalnotestext_area")?.setValue(this.internalnotes[0].internal_notes);
        // this.txtinternal_notes = this.internalnotes[0].internal_notes;
      });
    }
    else {
      var url = 'Leadbank360/GetNotesSummary'
      let params1 = {
        leadbank_gid: deencryptedParam
      }
      this.service.getparams(url, params1).subscribe((result: any) => {
        this.responsedata = result;
        this.internalnotes = this.responsedata.notes;
        //this.NotesreactiveForm.get("internalnotestext_area")?.setValue(this.internalnotes[0].internal_notes);
        // this.txtinternal_notes = this.internalnotes[0].internal_notes;
      });
    }
  }
  public Addnotes(): void {
    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.leadbank_gid = leadbank_gid;
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    this.NotesreactiveForm.value.leadgig = deencryptedParam;
    if (this.NotesreactiveForm.value.internalnotestext_area != null &&
      this.NotesreactiveForm.value.internalnotestext_area != "") {
      this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/Notesadd'
      this.service.post(api7, this.NotesreactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.GetLeadNotesSummary(deencryptedParam);
        } else {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.NotesreactiveForm.reset();
          this.GetLeadNotesSummary(deencryptedParam);
        }
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  updatenotes(param: any) {
    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.leadbank_gid = leadbank_gid;
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    //this.NotesreactiveForm.value.leadgig = deencryptedParam;
    if (param.internal_notes != null && param.internal_notes != "") {
      this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/Noteupdate';
      this.service.post(api7, param).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.GetLeadNotesSummary(deencryptedParam);
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetLeadNotesSummary(deencryptedParam);
        }
      });
    }
    else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  deletenotes(param: any) {
    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.leadbank_gid = leadbank_gid;
    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    this.NgxSpinnerService.show();
    var api7 = 'Leadbank360/Notedelete';
    this.service.post(api7, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.GetLeadNotesSummary(deencryptedParam);
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetLeadNotesSummary(deencryptedParam);
      }
    });

  }
  ///////////////////////////////////////////////notes 25.07.2024 end///////////////////////
  myModaladddetails(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue1.product_gid);
  }
  //upload documents
  public ondocumentsubmit(): void {
    debugger
    let formData = new FormData();
    this.document_upload = this.NotesreactiveForm.value;
    let document_title = this.NotesreactiveForm.value.document_title;
    let remarks = this.NotesreactiveForm.value.remarks;
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("leadbank_gid", this.leadgig);
      formData.append("remarks", remarks);
      formData.append("document_title", document_title);
      //this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/LeadDocumentUpload'
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message);
         this.NotesreactiveForm.reset();  
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message);
          this.NotesreactiveForm.reset();       
        }
        this.responsedata = result;
      });

      this.GetLeadDocumentDetails();
    }
     window.location.reload();
  }

  ondocChange2(event: any) {
    this.file = event.target.files[0];
  }


  downloadImage(doctitlelist: any) {
    if (doctitlelist.document_upload != null && doctitlelist.document_upload != "") {
      if (doctitlelist.document_type === 'mylead') {
        saveAs(doctitlelist.document_upload, `${doctitlelist.leadbank_gid}_file`);
      }

      else {
        saveAs(doctitlelist.document_upload, `${doctitlelist.leadbank_gid}_file`);
      }
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }


  }

  Leadstage_update(sales_stages: any) {

    this.reactiveForm2.get('appointment_gid')?.setValue(this.appointment_gid1);
    if (this.reactiveForm2.value.appointment_gid != null) {
      this.reactiveForm2.get('call_response')?.setValue(sales_stages);
      var url = 'Leadbank360/PostLeadStage'
      this.service.post(url, this.reactiveForm2.value).pipe().subscribe((result: any) => {
        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
          this.reactiveForm2.reset();
        }
        else {
          window.location.reload()
          this.ToastrService.success(result.message)

          this.reactiveForm2.reset();
        }

        this.reactiveForm2.reset();
      });
    }
    else {

    }

  }
  ///////////////////////////////enquiry///////////////////////////////////////   
  // Raise Enquiry
  async raiseenquiry() {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.leadgig, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    sessionStorage.setItem('CRM_LEADBANK_GID_ENQUIRY', leadbank_gid);
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    this.router.navigate(['/smr/SmrTrnCustomerraiseenquiry']);
    this.NgxSpinnerService.hide();
  }
  //view enquiry
  viewenquiry(params: any) {
    const secretKey = 'storyboarderp';
    const lspage = this.lspage;
    const enquiry_gid = AES.encrypt(params, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnEnquiryView', enquiry_gid]);
  }
  //enquiry to quotation  
  async raise_enquirytoquotation() {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    console.log(';wem', this.customer_gid)
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    this.router.navigate(['/smr/SmrTrnRaisequote', customer_gid]);
    this.NgxSpinnerService.hide();
  }
  //////////////////////////////quotation//////////////////////////////////////
  //raise quotation
  raisequotation() {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/SmrTrnQuotationaddNew']);
    this.NgxSpinnerService.hide();
  }
  //view quotation
  viewquotation(quotation_gid: any, customer_gid: any) {
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const lspage = 'SrmTrnNewquotationview'
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnquotationviewNew', encryptedParam, encryptedParam2, lspage])
  }
  // quotation to order
  raise_quotationtoorder(quotation_gid: any) {
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    this.router.navigate(['/smr/SmrTrnQuoteToOrder', encryptedParam]);
  }
  //quotation pdf
  quotation_pdf(quotation_gid: any) {
    const api = 'SmrTrnQuotation/GetQuotationRpt';
    this.NgxSpinnerService.show();
    let param = {
      quotation_gid: quotation_gid
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide();
    });
  }

  ///////////////////////////////order////////////////////////////////
  //raise order
  async raiseorder() {
    const secretKey = 'storyboarderp';
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/SmrTrnRaiseSalesOrderNew']);
    this.NgxSpinnerService.hide();
  }
  //raise order to invoice
  raise_ordertoinvoice(salesorder_gid: any) {
    const secretKey = 'storyboard';
    const param = salesorder_gid;
    const salesordergid = AES.encrypt(param, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    this.router.navigate(['/smr/SmrTrnOrderToInvoice', salesordergid])
  }
  //view order
  vieworder(salesorder_gid: any, customer_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const salesordergid = (salesorder_gid);
    const customergid = (customer_gid);
    const lspage1 = "SmrTrnSalesorderview";
    const leadbank_gid1 = "";
    const leadbank_gid = AES.encrypt(leadbank_gid1, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const encryptedParam = AES.encrypt(salesordergid, secretKey).toString();
    const encryptedParam2 = AES.encrypt(customergid, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesorderviewNew', encryptedParam, encryptedParam2, leadbank_gid, lspage]);
  }
  /////////////////////////////////invoice/////////////////////////////////////
  //raise invoice
  async raiseinvoice() {
    debugger
    debugger
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
    console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
    sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/RblTrnDirectinvoice/invoice']);
    this.NgxSpinnerService.hide();
  }
  //view invoice

  viewinvoice(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceview', encryptedParam])
  }
  //invoice pdf
  invoice_pdf(invoice_gid: string) {
    debugger
    const api = 'SmrRptInvoiceReport/GetInvoicePDF';
    let param = {
      invoice_gid: invoice_gid
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      else {
        this.ToastrService.warning(result.message)
      }

    });
  }

  ////////////////////////////////////////////////
  onaddtocustomer() {
    if (!this.reactiveForm.value.mobile) {
      this.reactiveForm.value.mobile = { e164Number: null }; // Or any default value you prefer
    }
    var url = 'Leadbank360/Addtocustomer';
    let param = {
      leadbank_gid: this.leadbank_gid,
      displayName: this.reactiveForm.value.displayName,
      mobile: this.reactiveForm.value.mobile.e164Number,
      email: this.reactiveForm.value.email,
      address1: this.reactiveForm.value.address1,
      taxsegment_name: this.reactiveForm.value.taxsegment_name,
      address2: this.reactiveForm.value.address2,
      customer_city: this.reactiveForm.value.customer_city,
      postal_code: this.reactiveForm.value.postal_code,
      countryname: this.reactiveForm.value.countryname,
      //region_name:this.reactiveForm.value.region_name,
      currency: this.reactiveForm.value.currency,
    };
    console.log('wefioj', param)
    if (this.reactiveForm.value.displayName != null && this.reactiveForm.value.mobile.e164Number != null
      && this.reactiveForm.value.email != null && this.reactiveForm.value.taxsegment_name != null
      && this.reactiveForm.value.address1 != null && this.reactiveForm.value.countryname
      && this.reactiveForm.value.currency != null) {
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == true) {
          this.ToastrService.success(result.message);
          this.customer_gid = result.addtocustomer[0].customer_gid;
          if (this.raise_options === "Enquiry") {
            const secretKey = 'storyboarderp';
            const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
            this.router.navigate(['/smr/SmrTrnEnquiry360', customer_gid, this.appointment_gid]);
          }
          else if (this.raise_options === "Quotation") {
            const secretKey = 'storyboarderp';
            const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
            const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
            console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
            sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
            sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
            this.router.navigate(['/smr/SmrTrnQuotationaddNew']);
          }
          else if (this.raise_options === "Order") {
            const secretKey = 'storyboard';
            const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
            const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
            console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
            sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
            sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
            this.router.navigate(['/smr/SmrTrnRaiseSalesOrderNew']);
          }
          else if (this.raise_options === "Invoice") {
            const secretKey = 'storyboarderp';
            const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
            const appointment_gid = AES.encrypt(this.appointment_gid1, secretKey).toString();
            console.log('riufjiewfnkjwf', appointment_gid, customer_gid)
            sessionStorage.setItem('CRM_APPOINTMENT_GID', appointment_gid);
            sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
            this.router.navigate(['/smr/RblTrnDirectinvoice/invoice']);
          }
          else {
            window.location.reload();
          }
        } else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message);
        }
      });
    }
    else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly fill all mandatory fields");
    }
  }


  
  //Price Segment
  openModalpricesegment(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = this.lspage;
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const leadbank_gid = AES.encrypt(this.leadbank_gid, secretKey).toString();
    const lead2campaign_gid = AES.encrypt(this.lead2campaign_gid, secretKey).toString();
    const leadbankcontact_gid = AES.encrypt(this.leadbankcontact_gid, secretKey).toString();
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/crm/CrmTrnCustomerProductPrice', encryptedParam, leadbank_gid, lead2campaign_gid, leadbankcontact_gid, lspage])
  }
  public onup(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
    }
    else {
      this.ToastrService.warning('Kindly select atleast one file!')
    }
  }

  public onupload(id: string): void {
    this.attachments();
    let formData = new FormData();
    this.reactiveMessageForm.value.contact_id = id;
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("file_type", this.filetype)
      formData.append("contact_id", this.reactiveMessageForm.value.contact_id)
      this.NgxSpinnerService.show();
      var url = 'Whatsapp/waSendDocuments'
      this.service.post(url, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)

        }
        this.GetWhatsappMessageSummary(this.leadgig);
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }


  openmodaleditcustomer(parameter: any, options: any) {
    this.raise_options = options;
    this.Geteditcustomerdetails(parameter);
  }
  Geteditcustomerdetails(leadbank_gid: any) {
    var url = 'Leadbank360/GetEditContactdetails'
    let param = {
      leadbank_gid: leadbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.editcustomer_list = result.contactedit_list;
      console.log('weoklm', this.editcustomer_list)
      this.reactiveForm.get("displayName")?.setValue(this.editcustomer_list[0].displayName);
      this.reactiveForm.get("email")?.setValue(this.editcustomer_list[0].email);
      this.reactiveForm.get("mobile")?.setValue(this.editcustomer_list[0].mobile1);
      this.reactiveForm.get("address1")?.setValue(this.editcustomer_list[0].address1);
      this.reactiveForm.get("leadbank_address1")?.setValue(this.editcustomer_list[0].address1);
      this.reactiveForm.get("leadbank_address2")?.setValue(this.editcustomer_list[0].address2);
      this.reactiveForm.get("leadbank_city")?.setValue(this.editcustomer_list[0].city);
      this.reactiveForm.get("customer_city")?.setValue(this.editcustomer_list[0].city);
      this.reactiveForm.get("leadbank_pin")?.setValue(this.editcustomer_list[0].pincode);
      this.reactiveForm.get("postal_code")?.setValue(this.editcustomer_list[0].pincode);
      this.reactiveForm.get("leadbank_state")?.setValue(this.editcustomer_list[0].state);
      this.reactiveForm.get("country_name")?.setValue(this.editcustomer_list[0].country_gid);
      this.reactiveForm.get("countryname")?.setValue(this.editcustomer_list[0].country_gid);
      this.reactiveForm.get("leadbank_gid")?.setValue(this.editcustomer_list[0].leadbank_gid);
      this.reactiveForm.get("address2")?.setValue(this.editcustomer_list[0].address2);

    });
  }


  get schedule_date() {
    return this.reactiveFormfollow.get('schedule_date')!;
  }
  get schedule_time() {
    return this.reactiveFormfollow.get('schedule_time')!;
  }
  get schedule_type() {
    return this.reactiveFormfollow.get('schedule_type')!;
  }
  Getshedulesummary(leadbank_gid: any) {
    var url = 'MyLead/GetOpportunitylogsummary'
    let param = {
      appointment_gid: this.appointment_gid1
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.Opportunityschedulesummary_list = result.Opportunityschedulesummary_list;

    });
    var url = 'MyLead/GetSchedulelogsummary'
    let param2 = {
      leadbank_gid: leadbank_gid
    }
    this.service.getparams(url, param2).subscribe((result: any) => {
      // this.responsedata=result;
      this.schedulesummary_list = result.schedulesummary_list;
      // console.log(this.callresponse_list[0].branch_gid)

    });

  }
  public schedule(): void {
    //debugger
    if (this.lspage == 'My-TeleAll' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-teleProspect' || this.lspage == 'My-teleFollowup'
      || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleNew' || this.lspage == 'MM-teleLongestLead' || this.lspage == 'MM-teleLapsedLead'
      || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleFollowup' ||
      this.lspage == 'MM-telepending' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-teleTotal' || this.lspage == "MM-teleteamview") {

      if (this.reactiveFormfollow.value.schedule_date != null && this.reactiveFormfollow.value.schedule_time != null) {
        this.reactiveFormfollow.value;
        var url = 'MyLead/PostTeleschedulelog'
        this.service.post(url, this.reactiveFormfollow.value).subscribe((result: any) => {


          if (result.status == false) {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            this.ToastrService.warning(result.message)
            this.ToastrService.warning("false")


            this.reactiveFormfollow.reset();
          }
          else {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            this.reactiveFormfollow.get("schedule_date")?.setValue(null);
            this.reactiveFormfollow.get("schedule_time")?.setValue(null);
            this.ToastrService.success(result.message)


            this.reactiveFormfollow.reset();
          }
          this.reactiveFormfollow.reset();
        });

      }
      else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
    }
    else {
      if (this.reactiveFormfollow.value.schedule_date != null && this.reactiveFormfollow.value.schedule_time != null) {
        this.reactiveFormfollow.get("appointment_gid")?.setValue(this.appointment_gid1);
        var url = 'MyLead/PostNewschedulelog'
        this.service.post(url, this.reactiveFormfollow.value).subscribe((result: any) => {
          if (result.status == false) {
            window.scrollTo({
              top: 0, // Code is used for scroll top after event done
            });
            this.ToastrService.warning(result.message)
            this.ToastrService.warning("false")
            this.reactiveFormfollow.reset();
          }
          else {
            window.scrollTo({
              top: 0, // Code is used for scroll top after event done
            });
            this.reactiveFormfollow.get("schedule_date")?.setValue(null);
            this.reactiveFormfollow.get("schedule_time")?.setValue(null);
            this.ToastrService.success(result.message)


            this.reactiveFormfollow.reset();
          }
          this.reactiveFormfollow.reset();
        });

      }
      else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
    }


  }
  onclickcall(identifierValue: any) {
    this.phone_number = identifierValue
    var phonenumber = this.phone_number.split("+91")
    this.phone_number = phonenumber[1];
    var url = 'clicktocall/Postcall'
    this.service.postparams(url, this.phone_number).subscribe((result: any) => {

    });
  }
  Getleadstadename() {

    if (this.lspage == 'My-TeleAll' || this.lspage == 'My-TeleDrop' || this.lspage == 'My-teleProspect' || this.lspage == 'My-teleFollowup'
      || this.lspage == 'My-teleNewpending' || this.lspage == 'My-teleNew' || this.lspage == 'MM-teleLongestLead' || this.lspage == 'MM-teleLapsedLead'
      || this.lspage == 'MM-teleScheduled' || this.lspage == 'MM-teleDrop' || this.lspage == 'MM-teleProspect' || this.lspage == 'MM-teleFollowup' ||
      this.lspage == 'MM-telepending' || this.lspage == 'MM-teleNew' || this.lspage == 'MM-teleTotal' || this.lspage == "MM-teleteamview") {

      var api8 = 'Leadbank360/GetTeleLeadStage';
      let params2 = {
        leadstage_name: this.gid_list[0].leadstage_name,
        leadbank_gid: this.leadgig
      }

      this.service.getparams(api8, params2).subscribe((result: any) => {
        this.responsedata = result;
        this.callresponse_list = this.responsedata.leadstage_list;
      });

    }
    else {

      var api8 = 'Leadbank360/GetLeadStage';
      let params2 = {
        leadstage_name: this.gid_list[0].leadstage_name,
        leadbank_gid: this.leadgig
      }
      this.service.getparams(api8, params2).subscribe((result: any) => {
        this.responsedata = result;
        this.callresponse_list = this.responsedata.leadstage_list;
      });
    }

  }
  connectcall() {
    let params = {
      phone_number: this.lsmobile,
    }
    this.NgxSpinnerService.show();
    var url = 'clicktocall/customercall'
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)

      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success('Connecting Your Call!! ')

      }
    });

  }
  getlogreport(deencryptedParam: any) {
    this.NgxSpinnerService.show();
    var url = 'Leadbank360/Getcalllogreport'
    let params = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result != null) {
        this.call_logreport = result.call_logreport;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#call_logreport').DataTable();
        }, 1000);
      }
      else {
        clearInterval(this.windowInterval)
      }
    });
  }
  getaudio(uniqueid: any) {
    this.NgxSpinnerService.show();
    var url = 'clicktocall/Getaudioplay'
    let param = {
      uniqueid: uniqueid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.recording_path = this.responsedata.recording_path;
      this.NgxSpinnerService.hide();
    });
  }
  onclosemovestage() {
    this.reactiveForm2.reset();
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    this.NgxSpinnerService.show();
    var url = 'Leadbank360/Getdeletedocuments'
    let param = {
      document_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
 
      }
      this.GetLeadDocumentDetails();

    });
  }
  ViewUploadDoc(document_upload: string, document_title: string) {
    const image = document_upload.split('.net/');
    const page = image[0];
    const url = page.split('?');
    const imageurl = url[0];
    const parts = imageurl.split('.');
    const extension = parts.pop();

    this.service.downloadfile(imageurl, document_title + '.' + extension).subscribe(
      (data: any) => {

        if (data != null) {
          this.service.fileviewer(data);
        }

      });
  }
  downloadFile(document_upload: string, document_title: string): void {

    const image = document_upload.split('.net/');
    const page = image[0];
    const url = page.split('?');
    const imageurl = url[0];
    const parts = imageurl.split('.');
    const extension = parts.pop();

    this.service.downloadfile(imageurl, document_title + '.' + extension).subscribe(
      (data: any) => {
        if (data != null) {
          this.service.filedownload1(data);
        } else {
          this.ToastrService.warning('Error in file download');
        }
      },
    );
  }
  openModallog5(gid: string) {
    this.parameter1 = gid
  }

  OnBin() {
    this.reactiveFormdrop.value.leadbank_gid = this.parameter1;
    this.NgxSpinnerService.show();
    var url1 = 'MarketingManager/GetCampaignMoveToBin'
    this.service.post(url1, this.reactiveFormdrop.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Lead Moved to MyBin')
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success('Lead Moved to MyBin Successfully')
        this.NgxSpinnerService.hide();
        window.location.reload();
      }
    });
  }

  onclosedrop() {
    this.reactiveFormdrop.reset();
  }


  viewcalllog() {
      this.NgxSpinnerService.show();
      let param = {
        leadbank_gid: this.leadgig
      }
      var url = 'Mycalls/GetCallLogLead'
      this.service.getparams(url, param).subscribe((result: any) => {
        $('#GetCallLogLead_list').DataTable().destroy();
        this.responsedata = result;
        this.GetCallLogLead_list = this.responsedata.GetCallLogLead_list;
        this.NgxSpinnerService.hide();
        console.log('inhewudjidojed',this.GetCallLogLead_list)
        setTimeout(() => {
          $('#GetCallLogLead_list').DataTable();
        }, 1);
      })
  }
}
function encodeToBase64(text: string): string {
  return btoa(text);
}
