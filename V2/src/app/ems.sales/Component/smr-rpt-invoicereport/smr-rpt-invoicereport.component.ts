
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-smr-rpt-invoicereport',
  templateUrl: './smr-rpt-invoicereport.component.html',
  styleUrls: ['./smr-rpt-invoicereport.component.scss']
})
export class SmrRptInvoicereportComponent {

  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/

  invoicesummarylist: any;
  response_data: any;
  invoicerefnoform!:FormGroup;
  company_code: any;
  shopifyInvoice:any;
  parameterValue2: any;
  parameterValue: any;
  paramvalues:any;
  creditnoteForm : FormGroup | any;
  cancelirnForm : FormGroup | any;
  responsedata: any;
  showOptionsDivId: any;
  lspage: any;
  filter_list: any[]=[];
  total_list: number=0;
  currentPage: number = 1;
  itemsPerPage: number = 200;
  sortBy: string = '';
  sortOrder: boolean = true; 
  itemsPerPageOptions: number[] = [200, 500,1000, 1500];
  noResultsMessage:any;
  parameter: any;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
      this.cancelirnForm = new FormGroup({
        invoice_refno_cancel: new FormControl(''),
        invoice_date_cancel: new FormControl(''),
        invoice_custname_cancel: new FormControl(''),
        invoice_amount_cancel: new FormControl(''),
        irn_no_cancel: new FormControl(''),
        invoice_gid: new FormControl(''),
  
      })
  
      this.creditnoteForm = new FormGroup({
        invoice_refno_creditnote: new FormControl(''),
        invoice_date_creditnote: new FormControl(''),
        invoice_custname_creditnote: new FormControl(''),
        invoice_amount_creditnote: new FormControl(''),
        irn_no_creditnote: new FormControl(''),
        invoice_gid: new FormControl(''),
  
      })
  }

  ngOnInit(): void {
    this.invoicerefnoform = new FormGroup({
      new_invoicerefno:new FormControl('',Validators.required),
      Old_invoicerefno:new FormControl(''),
      invoice_gid:new FormControl('')
    })
    this.Getsalesinvoice();
      
    var url = 'SmrRptInvoiceReport/GetShopifyInvoice'
    this.service.get(url).subscribe((result: any) => {
      this.shopifyInvoice = result;
    });
  

  

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  get new_invoicerefno(){
    return this.invoicerefnoform.get('new_invoicerefno')!;
  }
  Mail(params : string)
  {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceMail',encryptedParam])
  }
  Getsalesinvoice() {
    var api = 'SmrRptInvoiceReport/SaleinvoiceSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.invoicesummarylist = this.response_data.salesinvoicesummary_list || [];
      this.filter_list = this.invoicesummarylist;
      this.total_list = this.invoicesummarylist.length;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#invoice').DataTable();
      }, 1);
    });
  }
  get page_list(): any[] {
    const startIndex =(this.currentPage -1 ) * this.itemsPerPage
    const endIndex = startIndex + this.itemsPerPage;
    return this.filter_list.slice(startIndex, endIndex);
  }
  sort(field: string) {
    if (this.sortBy === field) {
      this.sortOrder = !this.sortOrder; // Toggle sort order
    } else {
      this.sortBy = field;
      this.sortOrder = true; // Default to ascending
    }
    this.filter_list.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return this.sortOrder ? -1 : 1;
      } else if (a[field] > b[field]) {
        return this.sortOrder ? 1 : -1;
      } else {
        return 0;
      }
    });
  }
  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }
  
  get endIndex(): number {
    return Math.min(this.startIndex + this.itemsPerPage, this.total_list);
  }
  onItemsPerPageChange(): void {
    this.currentPage = 1;
  }
  search(event: Event) {
    const input = event.target as HTMLInputElement;
    const query = input?.value.trim().toLowerCase(); // Trim whitespace and convert to lowercase
    if (!query) {
      this.filter_list = this.invoicesummarylist;
    } else {
      this.filter_list = this.invoicesummarylist.filter((item: any) => {
        // Convert all properties to lowercase before comparing
        const so_referencenumber = item.so_referencenumber.toLowerCase();
        const invoice_refno = item.invoice_refno.toLowerCase();
        const customer_name = item.customer_name.toLowerCase();
        const invoice_status = item.invoice_status.toLowerCase();
  
        // Filter items based on the main list properties
        const matchesMainList = 
        so_referencenumber.includes(query) ||
        customer_name.includes(query) ||
        invoice_refno.includes(query) ||
        invoice_status.includes(query);
  

        return matchesMainList;
      });
    }
    this.total_list = this.filter_list.length; 
    this.currentPage = 1; 
    if (this.total_list === 0) {
      
      this.noResultsMessage = "No matching records found";
    } else {
      this.noResultsMessage = "";
    }
  }
  pageChanged(event: any): void {
    this.currentPage = event.page;
  }

  onadd() {
    const lspage = "Invoice";

    this.router.navigate(['/smr/RblTrnDirectinvoice', lspage])
  }

  onraiseinvoice() {

    this.router.navigate(['/smr/SmrTrnRaiseSalesorder2invoice'])
  }
  collectpayment() {

    this.router.navigate(['/smr/SmrTrnReceiptsummary'])
  }


  editinvoice(params: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const invoice_gid = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceedit', invoice_gid])
  }
  editinvoiceRefno(params:any){
    this.paramvalues = params
    this.invoicerefnoform.get('Old_invoicerefno')?.setValue(this.paramvalues.invoice_refno);
    this.invoicerefnoform.get('invoice_gid')?.setValue(this.paramvalues.invoice_gid);
  }
  oninvoicerefnosubmit(){
    var url = "SmrRptInvoiceReport/changeinvoicerefno";
    this.service.postparams(url,this.invoicerefnoform.value).subscribe((result:any)=>{
      if(result.status == true){
        this.ToastrService.success(result.message);
        this.Getsalesinvoice()
        this.invoicerefnoform.reset()
      }
      else{
        this.ToastrService.warning(result.message)
        this.invoicerefnoform.reset()
      }
    });
  }
  onclose(){
    this.invoicerefnoform.reset()
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  oncancelinvoice(){
    var url="SmrRptInvoiceReport/cancelinvoice";
    let param = {
      invoice_gid : this.parameterValue.invoice_gid,
      salesorder_gid :this.parameterValue.invoice_reference,
      invoice_amount:this.parameterValue.invoice_amount
    }
    this.service.postparams(url,param).subscribe((result:any)=>{
      if(result.status==true){
        this.ToastrService.success(result.message)
        this.Getsalesinvoice()
      }
      else{
        this.ToastrService.warning(result.message)
      }
    });
  }
  viewinvoice(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceview', encryptedParam])
  }
  Delivery(salesorder_gid: any,invoice_gid:any) {
    const secretKey = 'storyboarderp';
    const param = (salesorder_gid);
    const param1=(invoice_gid )
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const encryptedParam1 = AES.encrypt(param1, secretKey).toString();

    this.router.navigate(['/ims/ImsTrnRaiseDeliveryorder', encryptedParam,encryptedParam1])
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  PrintPDF(invoice_gid: string) {
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
  // IRN

  myModaladddetails(parameter: string) {
    this.parameterValue2 = parameter
    this.cancelirnForm.get("invoice_refno_cancel")?.setValue(this.parameterValue2.invoice_refno);
    this.cancelirnForm.get("invoice_date_cancel")?.setValue(this.parameterValue2.invoice_date);
    this.cancelirnForm.get("invoice_custname_cancel")?.setValue(this.parameterValue2.customer_name);
    this.cancelirnForm.get("invoice_amount_cancel")?.setValue(this.parameterValue2.invoice_amount);
    this.cancelirnForm.get("irn_no_cancel")?.setValue(this.parameterValue2.irn);
    this.cancelirnForm.get("invoice_gid")?.setValue(this.parameterValue2.invoice_gid);




  }
  creditnoteModaldetails(parameter: string) {
    this.parameterValue = parameter
    this.creditnoteForm.get("invoice_refno_creditnote")?.setValue(this.parameterValue.invoice_refno);
    this.creditnoteForm.get("invoice_date_creditnote")?.setValue(this.parameterValue.invoice_date);
    this.creditnoteForm.get("invoice_custname_creditnote")?.setValue(this.parameterValue.customer_name);
    this.creditnoteForm.get("invoice_amount_creditnote")?.setValue(this.parameterValue.invoice_amount);
    this.creditnoteForm.get("irn_no_creditnote")?.setValue(this.parameterValue.irn);
    this.creditnoteForm.get("invoice_gid")?.setValue(this.parameterValue.invoice_gid);

 }
//  Cancel IRN
 onsubmit() {

  var url = 'Einvoice/PostCancelIRN'
      this.service.postparams(url, this.cancelirnForm.value).pipe().subscribe(result => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.Getsalesinvoice();
        }
        else {
          this.ToastrService.success("Update successfully")
          this.Getsalesinvoice();
        }
      });
  }
  onClose() {
    this.cancelirnForm.reset();
  };
  // Credit note IRN
  back(){
    this.creditnoteForm.reset();
  }
  creditnotesubmit() {
    var url = 'Einvoice/PostCreditNote'
        this.service.postparams(url, this.creditnoteForm.value).pipe().subscribe(result => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
           this.Getsalesinvoice();
          }
          else {
            this.ToastrService.success("IRN Cancelled Sucessfully")
            this.Getsalesinvoice();
          }
        });
    }
    // Generate IRN
    einvoice(params: any) {
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      this.router.navigate(['/smr/SmrTrnEinvoice', encryptedParam])
    }
}

