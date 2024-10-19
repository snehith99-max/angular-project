import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

@Component({
  selector: 'app-smr-trn-sales-all360',
  templateUrl: './smr-trn-sales-all360.component.html',
  styleUrls: ['./smr-trn-sales-all360.component.scss']
})
export class SmrTrnSalesAll360Component {

  NotesreactiveForm! : FormGroup;
  reactiveMessageForm! : FormGroup;
  reactiveForm! : FormGroup;
  enquiry_first_count: any;
  quotationmonthchart: any;
  order_first_count: any;
  order_first_amount: any;
  ordermonthchart: any;
  quotation_first_count: any;
  enquiry_first_amount: any;
  quotation_first_amount: any;
  enqiurymonthchart: any;
  invoice_first_count: any;
  invoice_first_amount: any;
  invoicemonthchart: any;
  leadstage_name: any;
  searchTextnotes= '';
  customergid: any;
  invoice_gid: any;
  salesorder_gid: any;
  customer_gid: any;
  quotation_gid: any;
  responsedata: any;
  enquiry_flag: boolean = false;
  quotation_flag: boolean = false;
  order_flag: boolean = false;
  invoice_flag: boolean = false;
  leadbasicdetailslist: any[]=[];
  leaddocumentdetail_list: any[]=[];
  internalnotes: any[]=[];
  Enquirylist: any[]=[];
  leadquotationdetails_list: any[]=[];
  leadorderdetails_list: any[]=[];
  leadinvoicedetails_list: any[]=[];
  leadcountdetails_list: any[]=[];
  enquiry_date:any;
  enquiry_count:any;
  enquiry_amount:any;
  quotation_date:any;
  customername:any;
  quotation_count:any;
  quotation_amount:any;
  order_amount:any;
  order_count:any;
  order_date:any;
  invoice_amount:any;
  invoice_date:any;
  invoice_count:any;
  sales_months:any;
  enquirycustomercontact_gid:any;
  enquiry_gid:any;
  enquirycustomer_gid:any;
  parameterValue:any;
  flag2: boolean = false;
  saleschart: any = {};
  file: any;
  document_upload: any[] = [];
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
    
  ];
  SearchCountryField = SearchCountryField;

  matchesSearchnotes(item: any): boolean {
    const searchString = this.searchTextnotes.toLowerCase();
    return item.internal_notes.toLowerCase().includes(searchString) || item.internal_notes.toLowerCase().includes(searchString);
  }
  constructor(private route: ActivatedRoute,
    private router :Router,
    private service : SocketService,
    private NgxSpinnerService:NgxSpinnerService,
    private ToastrService: ToastrService
  ){}

  ngOnInit() :void{  
    debugger  
    const key = 'storyboard';
    this.customergid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.customer_gid = AES.decrypt(this.customergid,key).toString(enc.Utf8);
    this.GetCustomerDetails(this.customer_gid);
    this.GetCustomerCountDetails(this.customer_gid);
    this.GetCustomerNotesSummary(this.customer_gid);
    this.GetEnquiryDetailsSummary(this.customer_gid);
    this.GetQuotationDetailsSummary(this.customer_gid);
    this.GetOrderDetailsSummary(this.customer_gid);
    this.GetInvoiceDetailsSummary(this.customer_gid);
    this.GetDocumentSummary(this.customer_gid);

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

    
  }
  GetDocumentSummary(customer_gid: any){
    let param = { customer_gid: customer_gid};
    var url = 'SmrTrnSales360/SmrGetDocumentDetails';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leaddocumentdetail_list = this.responsedata.SmrDocumentList;
    });
  }
  GetCustomerDetails(customer_gid:any){
    let param = { customer_gid: customer_gid}
    var getapi = 'SmrTrnSales360/GetCustomerDetails';
    this.service.getparams(getapi,param).subscribe((result:any)=>{
      this.leadbasicdetailslist = result.overalllist;
      this.customername = result.overalllist[0].customer_name
    })
  }
  GetCustomerCountDetails(customer_gid:any){
    debugger
    var url = 'SmrTrnSales360/GetCustomerCountDetails'
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadcountdetails_list = this.responsedata.customerchart;
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
  get email() {
    return this.reactiveForm.get('email')!;
  }
  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  
  GetEnquiryDetailsSummary(customer_gid:any){
    debugger
    var url = 'SmrTrnSales360/GetEnquiryDetails'
    let param = {
      leadbank_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Enquirylist = this.responsedata.SmrEnquiry_list;

      this.enquirycustomer_gid = this.Enquirylist[0].customer_gid;      
      this.enquirycustomercontact_gid = this.Enquirylist[0].customercontact_gid;
      this.enquiry_gid = this.Enquirylist[0].quotation_gid;

      setTimeout(() => {
        $('#Enquirylist').DataTable();
      }, 1);
    });
  }
  GetQuotationDetailsSummary(customer_gid: any) {
    var url = 'SmrTrnSales360/GetSmrQuotationDetails'
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {     
      this.responsedata = result;
      this.leadquotationdetails_list = this.responsedata.SmrQuotationList;     
      this.quotation_gid = this.leadquotationdetails_list[0].quotation_gid;
      setTimeout(() => {
        $('#leadquotationdetails_list').DataTable();
      }, 1);
    });
  }
  GetOrderDetailsSummary(customer_gid:any){
    var url = 'SmrTrnSales360/GetSmrOrderDetails'
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.leadorderdetails_list = this.responsedata.SmrOrderList;
      this.salesorder_gid = this.leadorderdetails_list[0].salesorder_gid;
console.log('feytwuisj', this.leadorderdetails_list )
      setTimeout(() => {
        $('#leadorderdetails_list').DataTable();
      }, 1);
    });
  }
  GetInvoiceDetailsSummary(customer_gid: any) {
    var url = 'SmrTrnSales360/GetSmrInvoiceDetails'
    let param = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#leadinvoicedetails_list').DataTable().destroy();
      this.responsedata = result;
      this.leadinvoicedetails_list = this.responsedata.SmrInvoiceList;

      this.invoice_gid = this.leadinvoicedetails_list[0].invoice_gid;
      setTimeout(() => {
        $('#leadinvoicedetails_list').DataTable();
      }, 1);
    });
  }
  onback(){
    window.history.back();
  }
  onaddpotentialvalue(){
  }
  openmodaleditcustomer(customergid:any){
    debugger
    const key = 'storyboarderp';
    const param = customergid;
    const lspath = 'Sales360'
    const customer_gid = AES.encrypt(param,key).toString();
    const lspage = AES.encrypt(lspath,key).toString();
    this.router.navigate(['/smr/SmrMstCustomerEdit',customer_gid,lspage]);
  }
  openModaladdtocustomer(){
  }
  Getleadstadename(){
  }
  openModalpricesegment(customer_gid:any){
  }
  ViewUploadDoc(document_upload:any,document_title:any){
    debugger
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
  downloadFile(document_upload:any,document_title:any){
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
  openModaldelete(document_gid:any){
    this.parameterValue = document_gid
  }
  Addnotes(){
    debugger
    this.NotesreactiveForm.value.customer_gid = this.customer_gid;
    if (this.NotesreactiveForm.value.internalnotestext_area != null &&
      this.NotesreactiveForm.value.internalnotestext_area != "") {
      this.NgxSpinnerService.show();
      var api7 = 'SmrTrnSales360/Notesadd'
      this.service.post(api7, this.NotesreactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.GetCustomerNotesSummary(this.customer_gid);
        } else {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
          this.NotesreactiveForm.reset();
          this.GetCustomerNotesSummary(this.customer_gid);
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
  GetCustomerNotesSummary(customer_gid:any){
   var url = 'SmrTrnSales360/SmrGetNotesSummary'
    let params1 = {
      customer_gid: customer_gid
    }
    this.service.getparams(url, params1).subscribe((result: any) => {
      this.responsedata = result;
      this.internalnotes = this.responsedata.SmrNotes;
    });
  }
  updatenotes(data:any){
    const secretKey = 'storyboarderp';
    // const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    const customer_gid = data.customer_gid;
    // const deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    //this.NotesreactiveForm.value.leadgig = deencryptedParam;
    if (data.internal_notes != null && data.internal_notes != "") {
      this.NgxSpinnerService.show();
      var api7 = 'SmrTrnSales360/SmrNoteupdate';
      this.service.post(api7, data).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.GetCustomerNotesSummary(this.customer_gid);
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetCustomerNotesSummary(this.customer_gid);
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
  deletenotes(data:any){
    const secretKey = 'storyboarderp';
    const leadbank_gid = this.route.snapshot.paramMap.get('leadbank_gid');
    this.customer_gid = leadbank_gid;
    const deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    this.NgxSpinnerService.show();
    var api7 = 'SmrTrnSales360/SmrNotedelete';
    this.service.post(api7, data).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.GetCustomerNotesSummary(this.customer_gid);
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.GetCustomerNotesSummary(this.customer_gid);
      }
    });
  }
  raiseenquiry(){
    debugger
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const leadbank_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    sessionStorage.setItem('CRM_LEADBANK_GID_ENQUIRY', leadbank_gid);          
    this.router.navigate(['/smr/SmrTrnCustomerraiseenquiry']);
    this.NgxSpinnerService.hide();  
  }
  raisequotation(){   
    debugger 
    const secretKey = 'storyboarderp'; 
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/SmrTrnQuotationaddNew']);   
  } 
  PrintPDF(quotation_gid:any){
    const api = 'SmrTrnQuotation/GetQuotationRpt';
    this.NgxSpinnerService.show()
    let param = {
      quotation_gid: quotation_gid
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  onraiseorders(quotation_gid:any){
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnQuoteToOrder', encryptedParam]);
  }
  onviewquote(quotation_gid:any,customer_gid:any){
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const lspage = 'SrmTrnNewquotationview'
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnquotationviewNew', encryptedParam, encryptedParam2, lspage])
  }
  raiseorder(){ 
    debugger
    const secretKey = 'storyboarderp';
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/SmrTrnRaiseSalesOrderNew']);
    this.NgxSpinnerService.hide();   
  }
  RaisetoOrder(salesorder_gid:any){ 
    const key = 'storyboard';
    const param = salesorder_gid;
    const salesordergid = AES.encrypt(param, key).toString();
    this.router.navigate(['/smr/SmrTrnOrderToInvoice', salesordergid])   
  }
  onview(salesorder_gid:any,customer_gid:any){   
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
  raiseinvoice(){
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const customer_gid = AES.encrypt(this.customer_gid, secretKey).toString();
    sessionStorage.setItem('CRM_CUSTOMER_GID_QUOTATION', customer_gid);
    this.router.navigate(['/smr/RblTrnDirectinvoice/invoice']);
    this.NgxSpinnerService.hide();
  }
  viewinvoice(invoice_gid:any){
    const secretKey = 'storyboarderp';
    const param = (invoice_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceview', encryptedParam])
  }
  PrintPDFINV(invoice_gid:any){
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
  viewenquiry(params:any){
    const secretKey = 'storyboarderp';
    const enquiry_gid = AES.encrypt(params, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnEnquiryView', enquiry_gid]);
  }
  raise_enquirytoquotation(enquiry_gid:any) {
    debugger
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = enquiry_gid;
    const enquirygid = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnRaisequote', enquirygid]);
    this.NgxSpinnerService.hide();
  }
  ondocChange2(event: any) {
    this.file = event.target.files[0];
  }
  CustomerPDF(){
    this.NgxSpinnerService.show();
    var url = 'CustomerStatement/GetCustomerStatementPDF'; 
    let param = { customer_gid : this.customer_gid }
    this.service.getparams(url,param).subscribe((result:any)=>{
      if(result.status == true){
        this.service.filedownload1(result);
        this.NgxSpinnerService.hide();
      }
      else{
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }
  ondocumentsubmit(){
    let formData = new FormData();
    this.document_upload = this.NotesreactiveForm.value;
    let document_title = this.NotesreactiveForm.value.document_title;
    let remarks = this.NotesreactiveForm.value.remarks;
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("customer_gid", this.customer_gid);
      formData.append("remarks", remarks);
      formData.append("document_title", document_title);
      var api7 = 'Leadbank360/LeadDocumentUpload'
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          window.location.reload();
        }
        else {
          this.ToastrService.success(result.message);
          window.location.reload();
        }
        this.responsedata = result;
      });
    }
  }
}
const randomizeArray1= (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }
  return array;
};
const randomizeArray2= (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }
  return array;
};
const randomizeArray3= (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }
  return array;
};
const randomizeArray4= (arg: number[]): number[] => {
  const array = arg.slice();
  let currentIndex = array.length, temporaryValue, randomIndex;

  while (0 !== currentIndex) {
    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;

    temporaryValue = array[currentIndex];
    array[currentIndex] = array[randomIndex];
    array[randomIndex] = temporaryValue;
  }
  return array;
};