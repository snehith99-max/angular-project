import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-smr-trn-quotationview-new',
  templateUrl: './smr-trn-quotationview-new.component.html',
  styleUrls: ['./smr-trn-quotationview-new.component.scss']
})
export class SmrTrnQuotationviewNewComponent {
  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '40rem',
    minHeight: '5rem',
    width: '750px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  quotationviewform:FormGroup|any;
  quotation:any;
  lspage: any;
  responsedata:any;
  quotationsummary_list:any;
  Viewquotationdetail_list : any;
  so_tax_amount :any;
  customer: any;
  GetTaxSegmentList:any[]=[];
  termsandconditions: string | any;

  ngOnInit() {
    const quotation_gid =this.route.snapshot.paramMap.get('quotation_gid');
    this.quotation= quotation_gid;
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    this.customer = customer_gid
    const lspage =this.route.snapshot.paramMap.get('lspage');
    this.lspage= lspage;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.quotation,secretKey).toString(enc.Utf8);
    const deencryptedParam3 = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);

    const deencryptedParam2 = AES.decrypt(this.lspage,secretKey).toString(enc.Utf8);
    this.lspage = deencryptedParam2;
    this.GetViewQuotationSummary(deencryptedParam);
    this.GetViewquotationdetails(deencryptedParam,deencryptedParam3);
  }

  constructor(private router: Router, public NgxSpinnerService:NgxSpinnerService,private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {
    this.quotationviewform = new FormGroup({
      quotation_gid: new FormControl(''),
      quotation_refno: new FormControl(''),
      quotation_date: new FormControl(''),
      branch: new FormControl(''),
      customer_name: new FormControl(''),
      contact_person: new FormControl(''),
      contact_number: new FormControl(''),
      customer_address: new FormControl(''),
      customer_email: new FormControl(''),
      sales_contact_person: new FormControl(''),
      freight_terms: new FormControl(''),
      payment_terms: new FormControl(''),
      currency: new FormControl(''),
      exchange_rate: new FormControl(''),
      remarks: new FormControl(''),

      termsandconditions: new FormControl(''),
      net_amount: new FormControl(''),
      tax_name: new FormControl(''),
      total_amountwithtax: new FormControl(''),
      addon_charges: new FormControl(''),
      additional_discount: new FormControl(''),
      freight_charges: new FormControl(''),
      buyback_charges: new FormControl(''),
      packing_charges: new FormControl(''),
      insurance_charges: new FormControl(''),
      roundoff: new FormControl(''),
      grandtotal: new FormControl('')
    })
  }

   
  //view
GetViewQuotationSummary(quotation_gid: any) {
  
  var url='SmrTrnQuotation/GetViewQuotationSummary'
  let param = {
    quotation_gid : quotation_gid 
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.quotationsummary_list = result.SO_list;
    this.termsandconditions = this.quotationsummary_list[0].termsandconditions;
    this.quotationviewform.get("net_amount")?.setValue(this.quotationsummary_list[0].total_amount);
    this.quotationviewform.get("tax_name")?.setValue(this.quotationsummary_list[0].tax_name);
    this.quotationviewform.get("total_amountwithtax")?.setValue(this.quotationsummary_list[0].total_price);
    this.quotationviewform.get("addon_charges")?.setValue(this.quotationsummary_list[0].addon_charge);
    this.quotationviewform.get("additional_discount")?.setValue(this.quotationsummary_list[0].additional_discount);
    this.quotationviewform.get("freight_charges")?.setValue(this.quotationsummary_list[0].freight_charges);
    this.quotationviewform.get("buyback_charges")?.setValue(this.quotationsummary_list[0].buyback_charges);
    this.quotationviewform.get("packing_charges")?.setValue(this.quotationsummary_list[0].packing_charges);
    this.quotationviewform.get("insurance_charges")?.setValue(this.quotationsummary_list[0].insurance_charges);
    this.quotationviewform.get("roundoff")?.setValue(this.quotationsummary_list[0].roundoff);
    this.quotationviewform.get("grandtotal")?.setValue(this.quotationsummary_list[0].Grandtotal);

  });
}
onclickgoback(){
  window.history.back()
}
GetViewquotationdetails(quotation_gid: any,customer_gid:any) {
  
  
 var url='SmrTrnQuotation/GetViewquotationdetails'
  this.NgxSpinnerService.show()
  let param = {
    quotation_gid : quotation_gid ,
    customer_gid: customer_gid
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
  this.responsedata=result;
  this.Viewquotationdetail_list = result.Sq_list;   
  this.NgxSpinnerService.hide()

  this.Viewquotationdetail_list.forEach((product: any) => {
    this.fetchProductSummaryAndTax(product.product_gid,quotation_gid,customer_gid);
  });
  });
}

fetchProductSummaryAndTax(product_gid: any,quotation_gid:any,customer_gid:any) {
  debugger
      let param = {
      product_gid: product_gid,
      quotation_gid: quotation_gid,
      customer_gid:customer_gid
    };

    var api = 'SmrTrnQuotation/GetViewquotationdetails';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetTaxSegmentList = result.GetTaxSegmentListView;

      // Handle tax segments for the current product
      this.handleTaxSegments(product_gid, this.GetTaxSegmentList);

     
    }, (error) => {
      console.error('Error fetching tax details:', error);
       
    });
  
}

// Method to handle tax segments for the current product
handleTaxSegments(product_gid: string ,taxSegments: any[]) {
  // Find tax segments for the current product_gid
  const productTaxSegments = taxSegments.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);

  if (productTaxSegments.length > 0) {
    // Assign tax segments to the current product
    this.Viewquotationdetail_list.forEach((product: { product_gid: string; taxSegments: any[]; }) => {
      if (product.product_gid === product_gid) {
        product.taxSegments = productTaxSegments;
      }
    });
  } else {
    // No tax segments found for the current product
    console.warn('No tax segments found for product_gid:', product_gid);
  }
}

// Inside your component class
checkDuplicateTaxSegment(taxSegments: any[], currentIndex: number): boolean {
  // Extract the taxsegment_gid of the current tax segment
  const currentTaxSegmentId = taxSegments[currentIndex].taxsegment_gid;

  // Check if the current tax segment exists before the current index in the array
  for (let i = 0; i < currentIndex; i++) {
      if (taxSegments[i].taxsegment_gid === currentTaxSegmentId) {
          // Duplicate found
          return true;
      }
  }

  // No duplicates found
  return false;
}
// Inside your component class
removeDuplicateTaxSegments(taxSegments: any[]): any[] {
  const uniqueTaxSegmentsMap = new Map<string, any>();
  taxSegments.forEach(taxSegment => {
      uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
  });
  // Convert the Map back to an array
  return Array.from(uniqueTaxSegmentsMap.values());
}
onback(){
  window.history.back();
}
}
