import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
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

interface IWatsapp {
  //sourceedit_name: any;
  leadbank_gid: string;
  created_date: string;
  customer_name: string;

  mobile: string;
  created_by: string;

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
}

@Component({
  selector: 'app-smr-rpt-sales360',
  templateUrl: './smr-rpt-sales360.component.html',
  styleUrls: ['./smr-rpt-sales360.component.scss']
})
export class SmrRptSales360Component {

  @ViewChild('Inbox') tableRef!: ElementRef;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  customer_gid: any;
  subscription!: Subscription;
  currentDayName: string;
  fromDate: any; toDate: any;

  search: string = '';
  response_data: any;
  mailmanagement: any[] = [];
  reactiveForm!: FormGroup;
  parameterValue1: any;
  mailsummary_list: any;
  mail: any;
  from_mail: any;
  to_mail: any;
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
  contactForm !: FormGroup;
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
  customercontact_gid: any;
  absURL: any;
  whatsappMessagetemplatelist: any[] = [];
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
  saledsorder_gid: any;
  parameterValue: any;
  editcustomer_list: any[] = [];
  sending_domain: any;
  receiving_domain: any;
  schedulesummary_list: any[] = [];
  leadbank_name: any;
  reactiveFormfollow!: FormGroup;
  myleads!: IMyleads;
  enquiry2campaign_gid: any;
  ScheduleType = [

    { type: 'Meeting', },

  ];

  // for chart
  
  vendor_gid: any;
  vencount_list : any [] =[];
  vendordetailslist: any[]=[];
  GetPurchase_list : any[]=[];
  flag2: boolean = false;
  invoice_count : any;
  so_count: any;
  quotation_count : any;
  purchasechart: any = {};
  paymentchart:any={};
  purchase_months : any;
  paymentchartflag:boolean=false;
  Getpaymentchart_list : any[]=[]
  series_Value: any;
  labels_value: any;
  cancelled_payment: any;
  approved_payment: any;
  completed_payment: any;
  customergid: any;


  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {
    this.leadbank = {} as IWatsapp;
    this.mailform = {} as IMailform;
    this.myleads = {} as IMyleads;
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });

  }

  ngOnInit(): void {
    debugger
    const secretKey = 'storyboard';
    this.customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    const lspage =this.route.snapshot.paramMap.get('lspage');
    this.customer_gid = this.customer_gid;
    this.lspage= lspage;
    const deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.lspage,secretKey).toString(enc.Utf8);
    this.customergid = deencryptedParam;
    this.lspage = deencryptedParam2;
    this.Getcustomerdetails(deencryptedParam);
    this.Getenquirydetails(deencryptedParam);
    this.Getquotedetails(deencryptedParam);
    this.Getorderdetails(deencryptedParam);
    this.GetCount(deencryptedParam);
    this.Getinvoicedetails(deencryptedParam);

    // charts
    this.GetPurchaseStatus(deencryptedParam)
  this.getpaymentchart(deencryptedParam)



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
        //let a = time.toLocaleString('en-US', { hour: 'numeric', hour12: true });
        let NewTime = hour + ":" + minuts + ":" + seconds
        // console.log(NewTime);
        this.rxTime = time;
      });

    this.reactiveForm = new FormGroup({

      customer_name: new FormControl(this.leadbank.customer_name, [
        Validators.required,
      ]),

      mobile: new FormControl(''),
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
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''),
      leadbank_gid: new FormControl(''),
      to_mail: new FormControl(''),
      mail_from: new FormControl('')
    });

    this.contactForm = new FormGroup({
      customer_name: new FormControl(''),
      customer_id: new FormControl(''),
      customer_gid: new FormControl(''),
      source: new FormControl(''),
      emailid: new FormControl(''),
      mobile: new FormControl(''),
      designation: new FormControl(''),
      region: new FormControl(''),
      customercontact_name: new FormControl(''),
      leadbank_gid: new FormControl('')
    })

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
    });


    this.reactiveFormfollow.patchValue({
      leadbank_gid: deencryptedParam,
      // mail_from: this.to_mail,

    });


  }

// purchase count
GetPurchaseStatus(deencryptedParam : any) {
  var url = 'SmrTrnSales360/GetSalestatus';
  let param ={
    customer_gid : this.customergid
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.GetPurchase_list = this.responsedata.salescount;
   
    if (this.GetPurchase_list.length > 0) {
      this.flag2 = true;
    }
    this.so_count = this.GetPurchase_list.map((entry: { so_count: any }) => entry.so_count)
    this.invoice_count = this.GetPurchase_list.map((entry: { invoice_count: any }) => entry.invoice_count)
    this.quotation_count = this.GetPurchase_list.map((entry: { quotation_count: any }) => entry.quotation_count)
    this.purchase_months = this.GetPurchase_list.map((entry: { Months: any }) => entry.Months)
  
    this.purchasechart = {
      chart: {
        type: 'bar',
        height: 300,
        width: '100%',
        background: 'White',
        foreColor: '#0F0F0F',
        fontFamily: 'inherit',
        toolbar: {
          show: false,
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
        categories: this.purchase_months,
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
          name: 'Sales Order',
          data: this.so_count,
          color: '#3D9DD9',
        },
        {
          name: 'Quotation',
          data: this.quotation_count,
          color: '#f08080',
        },
        {
          name: 'Invoice',
          data: this.invoice_count,
          color: '#9EBF95',
        },
      ],

      legend: {
        position: "top",
        offsetY: 5
      }
    };
  });
}

//payment chart
getpaymentchart(deencryptedParam : any){
  var url = 'SmrTrnSales360/GetPaymentCount';
  let param ={
    customer_gid : this.customergid
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.Getpaymentchart_list= this.responsedata.paymentcounts_list;
    if ( this.Getpaymentchart_list.length === 0) {
      this.paymentchartflag = true;
    }
    this.cancelled_payment =  Number ( this.Getpaymentchart_list[0].cancelled_payment);
    this.approved_payment =  Number(this.Getpaymentchart_list[0].approved_payment);
    this.completed_payment =  Number(this.Getpaymentchart_list[0].completed_payment);
    this.series_Value = [ this.approved_payment,this.completed_payment ,this.cancelled_payment  ];
    this.labels_value = ['Payment Approved','Payment Completed','Payment Cancelled'];
    this.paymentchart = {
      series: this.series_Value,
      labels: this.labels_value,
      chart: {
        width: 430,
        type: "pie"
      },
      colors: ['#E2D686','#26C485','#F96666'], // Update colors as needed
      fill: {
        type: "solid"
      },
    };

  });
  
}

  Getcustomerdetails(deencryptedParam: any) {
    debugger
    var url = 'SmrTrnSales360/GetCustomerDetails'
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadbasicdetailslist = this.responsedata.overalllist;
    });
  }

  Getenquirydetails(deencryptedParam: any) {
    var url = 'SmrTrnSales360/Get360EnquiryDetails'
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Enquirylist = this.responsedata.Getenquirydetailslist;
    });
  }

  Getquotedetails(deencryptedParam: any) {
    var url = 'SmrTrnSales360/GetQuotationDetails'
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadquotationdetails_list = this.responsedata.getquotelist;
    });
  }

  Getorderdetails(deencryptedParam: any) {
    var url = 'SmrTrnSales360/GetSalesOrderDetails'
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadorderdetails_list = this.responsedata.orderdetaillist;
    });
  }

  Getinvoicedetails(deencryptedParam: any) {
    var url = 'SmrTrnSales360/GetInvoiceDetails'
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadinvoicedetails_list = this.responsedata.invoicedetaillist;
    });
  }

  GetCount(deencryptedParam: any) {
    var url = 'SmrTrnSales360/GetCountandAmount';
    let param = {
      customer_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadcountdetails_list = this.responsedata.tilescountlist;

    });
  }

  // openModalUpdate(parameter: string) {
  //   debugger
  //   this.parameterValue1 = parameter;   
  // this.contactForm.get("customer_gid")?.setValue(this.parameterValue1);
  // }

  GetInvoiceDetailsSummary(deencryptedParam: any) {
    var url = 'Leadbank360/GetLeadInvoiceDetails'
    let param = {
      leadbank_gid: deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#leadinvoicedetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadinvoicedetails_list = this.responsedata.leadinvoicedetailslist;

      this.invoice_gid = this.leadinvoicedetails_list[0].invoice_gid;
      setTimeout(() => {
        $('#leadinvoicedetails_list').DataTable();
      }, 1);

    });
  }



  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }

  popattachments() {
    this.opencomposemail = !this.opencomposemail;
  }
  onback() {
    debugger
    if (this.lspage == 'SmrRptSalesreport') {
      this.router.navigate(['/smr/SmrRptSalesreport']);

    }
    if (this.lspage == 'SmrRptSalesreport') {
      this.router.navigate(['/smr/SmrRptSalesreport']);

    }
    else {
      this.router.navigate(['/smr/SmrTrnSalesLedger'])
    }

  }

  public Update(): void {

  }
  openModaladdtocustomer() {
    // this.parameterValue = parameter
  }

  onaddtocustomer() {
    console.log(this.leadbank_gid);
    var url = 'Leadbank360/Addtocustomer'
    let param = {
      leadbank_gid: this.leadbank_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.Status == true) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        window.location.reload()
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        window.location.reload()
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
    });
  }

  onclose() { }
  onsubmit() {
    debugger
    var url = 'SmrTrnSales360/InactiveCustomer'

    this.NgxSpinnerService.show();
    this.service.post(url, this.contactForm.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
        this.contactForm.reset()

      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
        this.contactForm.reset()

      }
    });
  }
  onupload(param: any) { }
  onChange1(param: any) { }

}

