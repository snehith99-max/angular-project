
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

interface CollapseState {
  [key: string]: boolean;
}
@Component({
  selector: 'app-pmr-trn-raise-enquiryadd-new',
  templateUrl: './pmr-trn-raise-enquiryadd-new.component.html',
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class PmrTrnRaiseEnquiryaddNewComponent {
  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
};
PurchaseEnquiryForm: FormGroup | any;
vendorError: any;
enquiry_gid:any;
enquiry_date:any;

sam: boolean = false;
  formBuilder: any;
  marks: number = 0;
  arrowfst: boolean = false;
  arrowOne: boolean = false;
  GetTaxSegmentList: any;
  mdlTaxSegment: any;
  taxSegmentFlag: any;
  showNoTaxSegments: boolean | undefined;
  taxsegment_gid: any;
 taxGids: string[] = [];
  taxseg_taxgid: string | undefined;
  taxseg_tax_amount= 0;
  tax_amount: any;
  taxseg_tax: any;
  totTaxAmountPerQty: any;
  Cmntaxsegment_gid: any;
  GetproductsCode: any;
  prod_name: any;
  selectedProductGID: any;
  totalTaxAmount1: any;
  products_list: any;
  products_list1: any;
  uom_list: any;
  GetVendor_list: any;

  assign_list: any;
  txtaddress1:any;
  txtemail_id:any;
  txtcontact_telephonenumber:any;
  txtcontactperson_name:any;
  txtvendorbranch_name:any;
 
  GetVendorlist: any;
  POproductlist: any;
toggleExpand(section: string) {
    this.isExpanded[section] = !this.isExpanded[section];
}
  currency_code1: any
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '33rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };
  tax1: any;
  tax2: any;
  selectedValue: string = '';
 
  product_list: any;
  branch_list: any;
  producttype_list:any;
  vendor_list: any;
  dispatch_list: any;
  productorder_list:any;
  mdlDispatchadd:any;
  currency_list: any;
  currency_list1: any;
  productsummary_list:any;
  netamount:any;
  overall_tax:any;
  tax_list: any;
  tax4_list: any; 
  delivery_days : number = 0;
  // payment_day : number = 0;
  productcode_list: any;
  productgroup_list: any;
  terms_list: any[] = [];
  productform: FormGroup | any;
  responsedata: any;
  productunit_list: any;
  mdlProductName: any;
  mdlTerms :any;
  mdlProductGroup: any;
  mdlProductUnit: any;
  mdlProductCode: any;
  mdlBranchName: any;
  mdlproducttype: any;
  mdlVendorName: any;
  mdlDispatchName: any;
  mdlCurrencyName: any;
  mdlTaxName1: any;
  mdlTaxName2: any;
  mdlTaxName3: any;
  prototal: number = 0;
  quantity: number = 0;
  discountpercentage: number = 0;
  discountamount:any;
  totalamount: number = 0;
  addoncharge: number = 0;
  POdiscountamount: number = 0;
  frieghtcharges: number = 0;
  forwardingCharges: number = 0;
  insurancecharges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  taxamount1:any;
  taxamount : number =0;
  taxpercentage: any;
  productdetails_list: any;
  productSaleseorder_list:any;
  taxamount2: number=0;
  taxamount3: number = 0;
  productamount:number=0;
  producttotalamount: any;
  parameterValue: string | undefined;
  POProductList1: any[] = [];
  productnamelist: any;
  selectedCurrencyCode: any;
  POadd_list: any;
  total_amount: any;
  insurance_charges: number = 0;
  additional_discount: number = 0;
  freightcharges: any;
  packing_charges: number = 0;
  buybackorscrap: any;
  tax_amount4:number=0;
  mdlTaxName4:any;
  exchange: any;
  mdlProductcode:any;
  mdlProductunit:any;
  unitprice:number=0;
  mdlvendoraddress:any;
  mdlemailaddress:any;
  mdlcontactnumber:any;
  mdlcontactperson:any;
  mdlvendorfax:any;
  GetVendord:any;
  invoicediscountamount: number = 0;
  allchargeslist: any[] = [];
  customer_rating:any;
  show(){
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
      toggleBtn?.addEventListener('click',() =>{
        // Toggle the 'show' class on the collapse content element
        collapseContent?.classList.toggle('show');
      });
  }
 
  ngOnInit(): void {
    this.show();

    this.POproductsummary();
    var api = 'PmrTrnRaiseEnquiry/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list = this.responsedata.GetProductGrp;

    });
   
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
flatpickr('.date-picker', options);
    var api = 'PmrTrnRaiseEnquiry/GetProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.products_list1 = this.responsedata.GetProducts;

    });

    
    var api = 'PmrTrnRaiseEnquiry/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.uom_list = this.responsedata.GetProductUnits;

    });
  
    var url = 'PmrTrnRaiseEnquiry/GetBranchDet';
this.service.get(url).subscribe((result:any)=>{
  this.branch_list = result.GetBranchDet;
   this.PurchaseEnquiryForm.get("branch_gid")?.setValue(this.branch_list[0].branch_gid);
 });

 var url = 'PmrTrnRaiseEnquiry/GetEmployeePerson'
 this.service.get(url).subscribe((result:any)=>{
   this.assign_list = result.GetEmployeePerson;
  });

    this.POproductsummary();
   
    var api = 'PmrTrnPurchaseOrder/Getproducttype';
    this.service.get(api).subscribe((result: any) => {
       this.responsedata = result;
      this.producttype_list = this.responsedata.Getproducttype;
    });
     var api = 'PmrTrnPurchaseOrder/GetVendor';
     this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.vendor_list = this.responsedata.GetVendor;
    });
    var api = 'PmrTrnPurchaseOrder/GetDispatchToBranch';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.dispatch_list = this.responsedata.GetDispatchToBranch;

    });
    var url = 'PmrTrnPurchaseOrder/GetCurrency';
       this.service.get(url).subscribe((result:any)=>{
      this.currency_list = result.GetCurrency;  
      this.mdlCurrencyName = this.currency_list[0].currencyexchange_gid;
         const defaultCurrency = this.currency_list.find((currency: { default_currency: string; }) => currency.default_currency === 'Y');
         const defaultCurrencyExchangeRate = defaultCurrency.exchange_rate;
         if (defaultCurrency) {
           this.mdlCurrencyName = defaultCurrency.currencyexchange_gid;
           this.PurchaseEnquiryForm.get("exchange_rate")?.setValue(defaultCurrencyExchangeRate);
         }
      });
    var api = 'PmrTrnPurchaseOrder/GetTax';
    this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.tax_list = result.GetTax;
    });
    var url = 'PmrTrnPurchaseOrder/GetTax4Dtl'
    this.service.get(url).subscribe((result:any)=>{
      this.tax4_list = result.GetTax4Dtl;
     });
   
    var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.terms_list = this.responsedata.GetTermsandConditions
    });
   
    this.POproductsummary();
    var api = 'PmrMstPurchaseConfig/GetAllChargesConfig';
     this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.allchargeslist = this.responsedata.salesconfigalllist;
      this.addoncharge = this.allchargeslist[0].flag;
      this.invoicediscountamount = this.allchargeslist[1].flag;
      this.frieghtcharges = this.allchargeslist[2].flag;
      this.forwardingCharges = this.allchargeslist[3].flag;
      this.insurancecharges = this.allchargeslist[4].flag;
      if (this.allchargeslist[0].flag == 'Y') {
        this.addoncharge = 0;
      } else {
        this.addoncharge = this.allchargeslist[0].flag;
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.invoicediscountamount = 0;
      } else {
        this.invoicediscountamount = this.allchargeslist[1].flag;
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.frieghtcharges = 0;
      } else {
        this.frieghtcharges = this.allchargeslist[2].flag;
      }

      if (this.allchargeslist[3].flag == 'Y') {
        this.forwardingCharges = 0;
      } else {
        this.forwardingCharges = this.allchargeslist[3].flag;
      }

      if (this.allchargeslist[4].flag == 'Y') {
        this.insurancecharges = 0;
      } else {
        this.insurancecharges = this.allchargeslist[4].flag;
      }
    });
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {

      this.PurchaseEnquiryForm = new FormGroup({
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      dispatch_name: new FormControl('', Validators.required),
    
      po_date: new FormControl(this.getCurrentDate(), Validators.required),
      vendor_companyname: new FormControl('', Validators.required),
      enquiry_date:new FormControl(this.getCurrentDate(), Validators.required),
      enquiry_referencenumber:new FormControl('', Validators.required),
      customer_rating:new FormControl(''),
      closure_date:new FormControl(this.getCurrentDate()),
      enquiry_remarks:new FormControl(''),
      vendor_requirement:new FormControl(''),
      enquiry_gid:new FormControl(''),
      tax_amount4: new FormControl(''),
      currency_code: new FormControl('',Validators.required),
      payment_term: new FormControl(''),
      contact_person: new FormControl(''),
      email_address: new FormControl(''),
      contact_number: new FormControl(''),
      currency: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      remarks: new FormControl(''),
      shipping_address: new FormControl(''),
      vendor_address: new FormControl(''),
      vendor_fax: new FormControl(''),
      priority_n: new FormControl('N'),
      taxamount1: new FormControl(''),
      buybackorscrap: new FormControl(''),
      payment_terms: new FormControl(''),
      freight_terms: new FormControl(''),
      delivery_location: new FormControl(''),
      template_content: new FormControl(''),
      delivery_period: new FormControl(''),
      payment_days: new FormControl('',[Validators.required]),
      product_total: new FormControl(''),
      tax_name: new FormControl(''),
      discount_percentage: new FormControl(''),
      qty: new FormControl(''),
      mrp: new FormControl(''),
      unitprice: new FormControl(''),
      productuom_name: new FormControl(''),
      product_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
      totalamount: new FormControl(''),
      totalamount3: new FormControl(''),
      tax_name3: new FormControl(''),
      taxamount2: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name1: new FormControl(''),
      discountamount: new FormControl(''),
      discountpercentage: new FormControl(''),
      quantity: new FormControl(''),
      display_name: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      priority_remarks: new FormControl(''),
      pocovernote_address: new FormControl(''),
      roundoff: new FormControl(''),
      ship_via: new FormControl(''),
      po_no: new FormControl('',[Validators.required]),
      grandtotal: new FormControl('',[Validators.required]),
      additional_discount: new FormControl(''),
      insurance_charges: new FormControl(''),
      freightcharges: new FormControl(''),
      addoncharge: new FormControl(''),
      delivery_days: new FormControl('',[Validators.required]),
      template_name :new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      tax_name4: new FormControl(''),
     
    })
    this.productform = new FormGroup({
      product_requireddate: new FormControl(this.getCurrentDate(), Validators.required),
      tmppurchaseorderdtl_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      display_name: new FormControl(''),
      productname: new FormControl(''),
      tax_name1: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name3: new FormControl(''),
      remarks: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      unitprice: new FormControl(''),
      quantity: new FormControl(''),
      discount_persentage: new FormControl(''),
      discount_amount: new FormControl(''),
      taxname1: new FormControl(''),
      taxamount1: new FormControl(''),
      taxname2: new FormControl(''),
      taxamount2: new FormControl(''),
      taxname3: new FormControl(''),
      taxamount3: new FormControl(''),
      totalamount: new FormControl(''),
      tax_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      netamount: new FormControl(''),
      overall_tax: new FormControl(''),
      template_content :new FormControl(''),
      producttype_name :new FormControl(''),
      producttype_gid :new FormControl(''),
    })
 
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get shipping_address() {
    return this.PurchaseEnquiryForm.get('shipping_address')!;
  }
  get vendor_address() {
    return this.PurchaseEnquiryForm.get('vendor_address')!;
  }
  get contact_person() {
    return this.PurchaseEnquiryForm.get('contact_person')!;
  }
  get contact_number() {
    return this.PurchaseEnquiryForm.get('contact_number')!;
  }
  get vendor_fax() {
    return this.PurchaseEnquiryForm.get('vendor_fax')!;
  }
  get email_address() {
    return this.PurchaseEnquiryForm.get('email_address')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  
  get branch_name() {
    return this.PurchaseEnquiryForm.get('branch_name')!;
  }
  get dispatch_name() {
    return this.PurchaseEnquiryForm.get('dispatch_name')!;
  }
  get vendor_companyname() {
    return this.PurchaseEnquiryForm.get('vendor_companyname')!;
  }
  get tax_name1() {
    return this.productform.get('tax_name1')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get display_name() {
    return this.productform.get('display_name')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get prodnameControl() {
    return this.productform.get('productgid');
  }
  get priority_remarks() {
    return this.productform.get('priority_remarks')
  }
  get tax() {
    return this.productform.get('tax')!;
  }
  get payment_days() {

    return this.PurchaseEnquiryForm.get('payment_days')!;

  };
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }
  get currency_code() {
    return this.productform.get('currency_code')!;
  }

  get enquiry_referencenumber() {
    return this.productform.get('enquiry_referencenumber')!;
  }
  OnChangeBranch() {
    debugger
    let branch_gid = this.PurchaseEnquiryForm.get("branch_name")?.value;
    let param = {
      branch_gid: branch_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeBranch';
    this.service.getparams(url, param).subscribe((result: any) => {
    this.PurchaseEnquiryForm.get("vendor_address")?.setValue(result.GetBranch[0].address1);
    });

  }
  OnChangeVendor() {
 
    let vendorregister_gid = this.PurchaseEnquiryForm.get("vendor_companyname")?.value;
    let param ={
      vendorregister_gid :vendorregister_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeVendor';
      this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetVendord = result.GetVendor;
      this.productSearch();
       this.PurchaseEnquiryForm.get("contact_number")?.setValue(result.GetVendor[0].contact_telephonenumber);
      this.PurchaseEnquiryForm.get("shipping_address")?.setValue(result.GetVendor[0].contactperson_name);
      this.Cmntaxsegment_gid =result.GetVendor[0].taxsegment_gid;
       //this.PurchaseEnquiryForm.get("vendor_address")?.setValue(result.GetVendor[0].address1);
       this.PurchaseEnquiryForm.get("email_address")?.setValue(result.GetVendor[0].email_id);
       this.PurchaseEnquiryForm.get("vendor_fax")?.setValue(result.GetVendor[0].fax)
       this.PurchaseEnquiryForm.value.vendorregister_gid = result.GetVendor[0].vendorregister_gid  ;
       
      this.POproductsummary();
      
    });

  }


  GetOnChangeTerms() {
    let termsconditions_gid = this.PurchaseEnquiryForm.value.template_name;
    let param = {
      termsconditions_gid: termsconditions_gid
    }
    var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.PurchaseEnquiryForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.PurchaseEnquiryForm.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
    });

    }
 
  onInput(event: any) {
    const value = event.target.value;
    const parts = value.split('.');
    // Keep only the first two parts (before and after the decimal point)
    const integerPart = parts[0];
    let decimalPart = parts[1] || '';
    // Limit the decimal part to 2 digits
    decimalPart = decimalPart.slice(0, 2);
    // Update the input value
    event.target.value = `${integerPart}.${decimalPart}`;
    this.unitprice = event.target.value; // Update the model value if necessary
  }

productSearch() {
  debugger;
 
  let vendorregister_gid = this.PurchaseEnquiryForm.get("vendor_companyname")?.value;
  var params ={
    producttype_gid: this.productform.value.producttype_name,
    product_name: this.productform.value.product_name,
    vendor_gid: vendorregister_gid
  };
  var api = 'PmrTrnRaiseEnquiry/GetProductsearchSummary';
  this.service.getparams(api, params).subscribe((result: any) => {
    this.responsedata = result;
    this.POProductList1 = this.responsedata.GetProductsearch_enq;   
  });
}

searchOnChange(event: KeyboardEvent) {
 
  if (event.key !== 'Enter') {
    
    this.productSearch();
  }
}
productAdd(product_gid:any){
  debugger
  if(this.PurchaseEnquiryForm.value.vendor_companyname==""||this.PurchaseEnquiryForm.value.vendor_companyname == null || this.PurchaseEnquiryForm.value.vendor_companyname == undefined)
   {
    window.scrollTo({
      top: 0, 
    });
    this.ToastrService.warning('Kindly Select Vendor!');
    return
   }
 this.toggleCollapsesection('section3');

  const api = 'PmrTrnRaiseEnquiry/PostOnAddsMultiple';
  this.NgxSpinnerService.show();

  // Set a timer to hide the spinner after 3 seconds
  const spinnerTimer = setTimeout(() => {
    this.NgxSpinnerService.hide();
  },3000);
  var params = {
    POProductList1_enq: this.POProductList1,
    productgroup_name: this.PurchaseEnquiryForm.value.productgroup_name,
    display_field: this.productform.value.display_field,
    product_code: this.productform.value.product_code,
    product_name: this.productform.value.product_name.product_name,
    productuom_name: this.productform.value.productuom_name,
    qty_requested: this.productform.value.qty_requested,
    potential_value: this.productform.value.potential_value,
    product_requireddate: this.productform.value.product_requireddate,
    product_requireddateremarks: this.productform.value.product_requireddateremarks,
    product_gid: this.productform.value.product_name.product_gid,
    productgroup_gid: this.productform.value.productgroup_gid,
    productuom_gid: this.productform.value.productuom_gid,
}
  console.log(params)
  this.service.post(api, params).subscribe((result: any) => {
    clearTimeout(spinnerTimer); // Clear the spinner timer
    if (result.status == false) {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning(result.message);
    } else {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.success(result.message);
      this.POProductList1 = [];
     
      this.POproductsummary();
    }
    this.NgxSpinnerService.hide();
  });


  const toggleBtn = document.getElementById('toggleBtn');
  const collapseContent = document.getElementById('collapseContent');
  toggleBtn?.addEventListener('click', () => {
    collapseContent?.classList.toggle('show');
  });
}
productSubmit() {
  debugger
  if(this.PurchaseEnquiryForm.value.vendor_companyname==""||this.PurchaseEnquiryForm.value.vendor_companyname == null || this.PurchaseEnquiryForm.value.vendor_companyname == undefined)
   {
    window.scrollTo({
      top: 0, 
    });
    this.ToastrService.warning('Kindly Select Vendor!');
    return
   }
 this.toggleCollapsesection('section3');

  const api = 'PmrTrnRaiseEnquiry/PostOnAddsMultiple';
  this.NgxSpinnerService.show();


  var params = {
    POProductList1_enq: this.POProductList1,
    productgroup_name: this.PurchaseEnquiryForm.value.productgroup_name,
    display_field: this.productform.value.display_field,
    product_code: this.productform.value.product_code,
    product_name: this.productform.value.product_name.product_name,
    productuom_name: this.productform.value.productuom_name,
    qty_requested: this.productform.value.qty_requested,
    potential_value: this.productform.value.potential_value,
    product_requireddate: this.productform.value.product_requireddate,
    product_requireddateremarks: this.productform.value.product_requireddateremarks,
    product_gid: this.productform.value.product_name.product_gid,
    productgroup_gid: this.productform.value.productgroup_gid,
    productuom_gid: this.productform.value.productuom_gid,
}
  console.log(params)
  this.service.post(api, params).subscribe((result: any) => {
    
    if (result.status == false) {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning(result.message);
    } else {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.success(result.message);
      this.POProductList1 = [];
     
      this.POproductsummary();
    }
    this.NgxSpinnerService.hide();
  });


  const toggleBtn = document.getElementById('toggleBtn');
  const collapseContent = document.getElementById('collapseContent');
  toggleBtn?.addEventListener('click', () => {
    collapseContent?.classList.toggle('show');
  });
}

prodtotalcal(i: any) {
  this.marks = this.POProductList1[i].quantity;
  const unitPrice = this.POProductList1[i].unitprice;
  const quantity = this.POProductList1[i].quantity;
  const discountPercentage = this.POProductList1[i].discount_persentage;
  
  // Initialize total tax amount
  let totalTaxAmount = 0;

  // Iterate through tax segments of the product
  for (let taxSegment of this.POProductList1[i].taxSegments) {
      // Calculate tax amount for the current tax segment
      const taxAmount = (((quantity * unitPrice) - (quantity * unitPrice * discountPercentage / 100)) * (parseFloat(taxSegment.tax_percentage) / 100));
      
      // Add tax amount to the total tax amount
      totalTaxAmount += taxAmount;

      // Store individual tax amount for each tax segment
      taxSegment.taxAmount = taxAmount.toFixed(2);
  }

  // Calculate total discount amount
  const discountAmount = (quantity * unitPrice * discountPercentage) / 100;

  // Calculate total amount including tax
  const totalAmount = (quantity * unitPrice) - discountAmount + totalTaxAmount;

  // Update values in the product list
  this.POProductList1[i].discount_amount = discountAmount.toFixed(2);
  this.POProductList1[i].totalTaxAmount = totalTaxAmount.toFixed(2);
  this.POProductList1[i].total_amount = totalAmount.toFixed(2);

  // Calculate total tax amount for all tax segments
  const totalTaxAmountForProduct = this.POProductList1[i].taxSegments.reduce((acc: number, cur: { taxAmount: string; }) => acc + parseFloat(cur.taxAmount), 0);
 
  totalTaxAmount=totalTaxAmountForProduct.toFixed(2);
  console.log( totalTaxAmount)
  // Log the total tax amount for debugging
  console.log('Total Tax Amount for Product:', totalTaxAmount);
}

GetOnChangeCustomerName(){
    
  let vendor_gid = this.PurchaseEnquiryForm.value.vendor_companyname.vendor_gid;
  let param ={
    vendor_gid :vendor_gid
  }
  var url = 'PmrTrnRaiseEnquiry/GetVendorDtl';
  this.service.getparams(url,param).subscribe((result: any) => {
    this.GetVendorlist = result.GetVendorlist;   
    this.PurchaseEnquiryForm.get("vendorbranch_name")?.setValue("H.Q");
    this.PurchaseEnquiryForm.get("email_id")?.setValue(result.GetVendorlist[0].email_id);
    this.PurchaseEnquiryForm.get("contactperson_name")?.setValue(result.GetVendorlist[0].contactperson_name);
    this.PurchaseEnquiryForm.get("address1")?.setValue(result.GetVendorlist[0].address1);
    this.PurchaseEnquiryForm.get("contact_telephonenumber")?.setValue(result.GetVendorlist[0].contact_telephonenumber);
   
  });

}
OnClearVendor()
{
  this.txtvendorbranch_name='';
  this.txtcontactperson_name='';
  this.txtcontact_telephonenumber='';
  this.txtemail_id='';
  this.txtaddress1='';


}
  onclearvendor(){
    this.mdlcontactperson = null;
    this.mdlcontactnumber = null;
    this.mdlemailaddress = null;
    this.mdlvendoraddress = null;
    this.mdlvendorfax = null; 
  }
  onclearbranch(){
    this.mdlDispatchadd=null;
  }
  onclearcurrency(){
    this.exchange=null;
  }
  OnClearTax1(i:any) {
    this.POProductList1[i].taxamount1 = 0; 
    const subtotal = this.exchange * this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discount_amount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].discount_amount = +(this.POProductList1[i].discount_amount).toFixed(2);
    this.POProductList1[i].total_amount = +(subtotal - this.POProductList1[i].discount_amount + this.POProductList1[i].taxamount1+this.POProductList1[i].taxamount2).toFixed(2);
    this.POProductList1[i].total_amount = +(this.POProductList1[i].total_amount).toFixed(2);
  }
  OnClearTax2(i:any) {
    this.POProductList1[i].taxamount2 = 0; 
    const subtotal = this.exchange * this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discount_amount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].discount_amount = +(this.POProductList1[i].discount_amount).toFixed(2);
    this.POProductList1[i].total_amount = +(subtotal - this.POProductList1[i].discount_amount + this.POProductList1[i].taxamount2).toFixed(2);
    this.POProductList1[i].total_amount = +(this.POProductList1[i].total_amount).toFixed(2);
  }
  OnChangeTaxAmount1(i: any) {
    let tax_gid = this.POProductList1[i].tax1;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.POProductList1[i].taxamount1 = ((tax_percentage) * (this.POProductList1[i].total_amount) / 100);
    this.POProductList1[i].taxamount1 = +(this.POProductList1[i].taxamount1).toFixed(2);
    if (this.POProductList1[i].taxamount1 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
      this.totalamount = +(this.totalamount).toFixed(2);
    }
    else {
      this.POProductList1[i].totalamount = parseFloat(this.POProductList1[i].total_amount) + parseFloat(this.POProductList1[i].taxamount1);
      this.POProductList1[i].total_amount = +this.POProductList1[i].totalamount.toFixed(2);
    }
  }

  OnChangeTaxAmount4() {
    debugger
    let tax_gid = this.PurchaseEnquiryForm.get('tax_name4')?.value;   
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
    this.total_amount = +this.total_amount.toFixed(2);
    
this.total_amount = isNaN(this.total_amount) ? 0 : this.total_amount;
this.tax_amount4 = isNaN(this.tax_amount4) ? 0 : this.tax_amount4;
this.addoncharge = isNaN(this.addoncharge) ? 0 : this.addoncharge;
this.freightcharges = isNaN(this.freightcharges) ? 0 : this.freightcharges;
this.packing_charges = isNaN(this.packing_charges) ? 0 : this.packing_charges;
this.insurance_charges = isNaN(this.insurance_charges) ? 0 : this.insurance_charges;
this.roundoff = isNaN(this.roundoff) ? 0 : this.roundoff;
this.totalamount = isNaN(this.totalamount) ? 0 : this.totalamount;
this.additional_discount = isNaN(this.additional_discount) ? 0 : this.additional_discount;

// Calculate the grand total
this.grandtotal = this.total_amount + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +
    (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount) - (+this.additional_discount);

    }
  OnClearOverallTax() {
    this.tax_amount4=0;
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
    this.total_amount = +this.total_amount.toFixed(2);
     this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
    this.grandtotal =+this.grandtotal.toFixed(2);
  }
  OnChangeTaxAmount2(i:any) {
    let tax_gid = this.POProductList1[i].tax2;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);  
    let tax_percentage = this.taxpercentage[0].percentage;
    const subtotal = this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discountamount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].totalamount = subtotal - this.POProductList1[i].discountamount;
    this.POProductList1[i].taxamount2 = ((tax_percentage) * (this.POProductList1[i].totalamount) / 100);
    if (this.POProductList1[i].taxamount1 == undefined && this.taxamount2 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    } 
    else {
      this.POProductList1[i].totalamount = (parseFloat(this.POProductList1[i].total_amount)  + parseFloat(this.POProductList1[i].taxamount2));
      this.POProductList1[i].total_amount = +this.POProductList1[i].totalamount.toFixed(2);
    }
  }
  OnChangeTaxAmount3() {
    let tax_gid3 = this.productform.get('tax_name3')?.value;
    this.taxpercentage = this.getDimensionsByFilter(tax_gid3);
    let tax_percentage = this.taxpercentage[0].percentage;
    const subtotal = this.unitprice * this.quantity;
    this.discountamount = (subtotal * this.discountpercentage) / 100;
    this.totalamount = subtotal - this.discountamount;
    this.taxamount3 = ((tax_percentage) * (this.totalamount) / 100);
    if (this.taxamount1 == undefined && this.taxamount2 == undefined && this.taxamount3 == undefined) {
      const subtotal = this.unitprice * this.quantity;
      this.discountamount = (subtotal * this.discountpercentage) / 100;
      this.totalamount = subtotal - this.discountamount;
    }
    else {
      this.totalamount = ((this.totalamount) + (+this.taxamount1) + (+this.taxamount2) + (+this.taxamount3));
    }
  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url ='PmrTrnRaiseEnquiry/GetDeleteEnquiryProductSummary';
    this.NgxSpinnerService.show()
    let param = {
      tmpsalesenquiry_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {

        this.ToastrService.success(result.message)
        this.POproductsummary();
        this.NgxSpinnerService.hide()
      }
    });
  }
  

  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }
  OnChangeCurrency() {
    let currencyexchange_gid = this.PurchaseEnquiryForm.get("currency_code")?.value;
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.GetOnchangeCurrency;
      this.PurchaseEnquiryForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.currency_code1 = this.currency_list1[0].currency_code
    });

  }
  onCurrencyCodeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;
    this.selectedCurrencyCode = selectedCurrencyCode;
    this.PurchaseEnquiryForm.controls.currency_code.setValue(selectedCurrencyCode);
    this.PurchaseEnquiryForm.get("currency_code")?.setValue(this.currency_list[0].currency_code);

  }
  onSubmit(){
    debugger
    if( this.productsummary_list == null || this.productsummary_list == undefined 
      ){
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    if(this.PurchaseEnquiryForm.value.enquiry_referencenumber==""||this.PurchaseEnquiryForm.value.enquiry_referencenumber == null || this.PurchaseEnquiryForm.value.enquiry_referencenumber == undefined &&
    this.PurchaseEnquiryForm.value.enquiry_date==""||this.PurchaseEnquiryForm.value.enquiry_date == null || this.PurchaseEnquiryForm.value.enquiry_date == undefined &&
    this.PurchaseEnquiryForm.value.vendor_companyname==""||this.PurchaseEnquiryForm.value.vendor_companyname == null || this.PurchaseEnquiryForm.value.vendor_companyname == undefined &&
        this.PurchaseEnquiryForm.value.branch_name==""||this.PurchaseEnquiryForm.value.branch_name == null || this.PurchaseEnquiryForm.value.branch_name == undefined &&
        this.PurchaseEnquiryForm.value.currency_code==""||this.PurchaseEnquiryForm.value.currency_code == null || this.PurchaseEnquiryForm.value.currency_code == undefined &&
        this.PurchaseEnquiryForm.value.closure_date==""||this.PurchaseEnquiryForm.value.closure_date == null || this.PurchaseEnquiryForm.value.closure_date == undefined 
        )
    {
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }
    var params = {
      branch_name:this.PurchaseEnquiryForm.value.branch_name,
      branch_gid: this.PurchaseEnquiryForm.value.branch_name.branch_gid,
      enquiry_date:this.PurchaseEnquiryForm.value.enquiry_date,
      enquiry_gid:this.PurchaseEnquiryForm.value.enquiry_gid,
      vendor_companyname:this.PurchaseEnquiryForm.value.vendor_companyname,
      vendorbranch_name: this.PurchaseEnquiryForm.value.vendor_companyname,
      contactperson_name: this.PurchaseEnquiryForm.value.contactperson_name,
      email_id: this.PurchaseEnquiryForm.value.email_id,
      address1: this.PurchaseEnquiryForm.value.shipping_address,
      contact_email: this.PurchaseEnquiryForm.value.contact_email,
      enquiry_referencenumber:this.PurchaseEnquiryForm.value.enquiry_referencenumber,
      contact_number: this.PurchaseEnquiryForm.value.contact_number,
      enquiry_remarks: this.PurchaseEnquiryForm.value.enquiry_remarks,
      contact_address: this.PurchaseEnquiryForm.value.shipping_address,
      vendor_requirement: this.PurchaseEnquiryForm.value.vendor_requirement,
      currency_gid : this.PurchaseEnquiryForm.value.currency_code,
      closure_date: this.PurchaseEnquiryForm.value.closure_date,
      user_firstname: this.PurchaseEnquiryForm.value.user_firstname,
      customer_rating: this.PurchaseEnquiryForm.value.customer_rating,
      customerbranch_name: this.PurchaseEnquiryForm.value.customerbranch_name,
      vendor_gid:this.PurchaseEnquiryForm.value.vendor_companyname,
      contact_telephonenumber:this.PurchaseEnquiryForm.value.contact_number,
  
    }
    var api='PmrTrnRaiseEnquiry/PostVendorEnquiry'
    this.NgxSpinnerService.show()
    
    this.service.postparams(api, params).subscribe((result: any) => {
    
    if (result.status == false) {
      this.NgxSpinnerService.hide()
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning(result.message)
    }
    else {
      this.NgxSpinnerService.hide()
        window.scrollTo({
    top: 0, 
  });
      this.ToastrService.success(result.message)
      this.router.navigate(['/pmr/PmrTrnRaiseEnquiry']);
    }
    this.NgxSpinnerService.hide()
  });

  }
  
  sample(){
    this.sam = !this.sam;
    }

    arrow(){
      this.arrowfst = !this.arrowfst;
      }

      arrowone(){
        this.arrowOne = !this.arrowOne;
        }



    isCollapsed: CollapseState = {
      section1: true,
      section2: true,
      section3: true,
  };

  toggleCollapse(section: string) {
   
      this.isCollapsed[section] = !this.isCollapsed[section];
  }

  toggleCollapsesection(section: string) {
    this.isCollapsed[section] = false;
}


POproductsummary() {
  debugger;
  this.NgxSpinnerService.show();
  var api = 'PmrTrnRaiseEnquiry/GetProductsSummary';
  this.service.get(api).subscribe((result: any) => {
  this.responsedata   = result;
  this.productsummary_list = this.responsedata.productsummarys_list;
  this.NgxSpinnerService.hide();
    
  });
}

fetchProductSummaryAndTax(product_gid: string) {
  if (this.PurchaseEnquiryForm.value.vendor_companyname !== undefined) {
    let vendor_gid = this.PurchaseEnquiryForm.get("vendor_companyname")?.value;
    let param = {
      product_gid: product_gid,
      vendor_gid: vendor_gid
    };

    var api = 'PmrTrnPurchaseOrder/GetProductSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      
     
    }, (error) => {
      console.error('Error fetching tax details:', error);
      
    });
  }
}




}

