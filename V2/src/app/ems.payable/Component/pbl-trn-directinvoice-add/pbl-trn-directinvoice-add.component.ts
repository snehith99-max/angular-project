import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';

interface CollapseState {
  [key: string]: boolean;
}




@Component({
  selector: 'app-pbl-trn-directinvoice-add',
  templateUrl: './pbl-trn-directinvoice-add.component.html',
  styleUrls: ['./pbl-trn-directinvoice-add.component.scss']
})
export class PblTrnDirectinvoiceAddComponent {
  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
  };

  vendorError: any;
  sam: boolean = false;
  formBuilder: any;
  marks: number = 0;
  arrowfst: boolean = false;
  arrowOne: boolean = false;
  GetTaxSegmentList: any;
  mdlTaxSegment: any;
  scrap: number = 0;
  taxSegmentFlag: any;
  showNoTaxSegments: boolean | undefined;
  taxsegment_gid: any;
  taxGids: string[] = [];
  taxseg_taxgid: string | undefined;
  taxseg_tax_amount = 0;
  tax_amount: any;
  tax_prefix: any;
  taxseg_tax: any;
  totTaxAmountPerQty: any;
  Cmntaxsegment_gid: any;
  GetproductsCode: any;
  filteredPOProductList1: any[] = [];
  GetDraftinvoice: any[] = [];
  prod_name: any;
  selectedProductGID: any;
  mdlproduct_total: any;
  total_amount_including_tax: any;
  txtvendordetails: any;
  txtbillto: any;
  txtshipto: any;
  tax_prefix2: any;
  productremarks: any;
  currency_code1: any
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };
  tax1: any;
  tax2: any;
  selectedValue: string = '';
  InvoiceaddForm: FormGroup | any;
  product_list: any[]=[];
  additional_amount: any;
  branch_list: any[]=[];
  producttype_list: any[]=[];
  vendor_list:any []=[];
  Getproductgroup: any;
  mdlDispatchadd: any;
  currency_list: any[]=[];
  currency_list1: any[]=[];
  productsummary_list: any[]=[];
  Getadditional_list: any[]=[];
  Getdiscount_list: any[]=[];
  netamount: any;
  overall_tax: any;
  tax_list: any[]=[];
  tax4_list: any[]=[];
  delivery_days: number = 0;
  // payment_day : number = 0;
  productcode_list: any[]=[];
  productgroup_list: any[]=[];
  terms_list: any[] = [];
  GetPurchaseTypedropDown: any[] = [];
  productform: FormGroup | any;
  responsedata: any;
  productunit_list: any;
  mdlProductName: any;
  mdlTerms: any;
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
  discountamount: any;
  totalamount: number = 0;
  addoncharge: number = 0;
  deductionamount: number = 0;
  additionalamount: number = 0; 
  extra_deductionamount: number = 0;
  extra_additionalamount: number = 0;
  POdiscountamount: number = 0;
  frieghtcharges: number = 0;
  forwardingCharges: number = 0;
  insurancecharges: number = 0;
  roundoff: number = 0;
  grandtotal: number = 0;
  taxamount1: number = 0;
  taxamount: number = 0;
  taxpercentage: any;

  taxamount2: number = 0;
  taxamount3: number = 0;
  productamount: number = 0;
  producttotalamount: any;
  parameterValue: string | undefined;
  POProductList1: any[] = [];
  invoice_listsedit: any[] = [];
  productnamelist: any;
  selectedCurrencyCode: any;
  purchasetype: any;
  POadd_list: any;
  total_amount: any;
  insurance_charges: number = 0;
  additional_discount: number = 0;
  freightcharges: number = 0;
  packing_charges: number = 0;
  buybackorscrap: any;
  tax_amount4: number = 0;
  mdlTaxName4: any;
  exchange: any;
  isNewProduct: boolean = false;
  mdlProductcode: any;
  mdlProductunit: any;
  unitprice: number = 0;
  mdlvendoraddress: any;
  mdlemailaddress: any;
  mdlcontactnumber: any;
  mdlcontactperson: any;
  mdlvendorfax: any;

  GetVendord: any;
  producttotal_tax_amount: number =0;
  producttotal_amount: any;
  invoicediscountamount: number = 0;
  allchargeslist: any[] = [];
  searchText: any;
  productsearch: any;
  productcodesearch: any;
  productcodesearch1: any;
  productname: any;
  individualTaxAmounts: any = 0;
  productquantity: number = 0;
  productunitprice: number = 0;
  productdiscount: number = 0;
  tax_name4: number = 0;
  discount_percentage: number = 0;
  taxgid1: any;
  taxname1: any;
  addressdetails:any;
  taxprecentage1: any;
  taxgid2: any;
  taxname2: any;
  taxprecentage2: any;
  taxgid3: any;
  taxname3: any;
  invoice_remarks: any;
  billing_mail: any;
  due_date: any;
  taxprecentage3: any;
  productdiscount_amountvalue: any;
  productdiscounted_precentagevalue: any;
  CurrencyExchangeRate: any;
  purchasetype_list : any[]=[];
  ngOnInit(): void {
    debugger

this.productSearch();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);


    var api = 'PmrTrnDirectInvoice/GetBranchName';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.GetBranchnamedropdown;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;   
      this.InvoiceaddForm.get('branch_name')?.setValue(branchName); 
    });

    var deleteapi = 'PmrTrnDirectInvoice/LoadingaddSummary'
    this.service.get(deleteapi).subscribe((result: any) => {
    });

    var api = 'PmrTrnDirectInvoice/PblGetproducttype';
    this.service.get(api).subscribe((result: any) => {
       this.responsedata = result;
      this.producttype_list = this.responsedata.PblGetproducttype;
    });
    
    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });
   

    var api = 'PmrTrnDirectInvoice/PblGetTax';
    this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.tax_list = result.PblGetTax;
    });
    
    var api = 'PmrMstPurchaseType/GetpurchaseType';
    this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.purchasetype_list = result.GetpurchaseType_List;
    });
    
    var url = 'PmrTrnPurchaseOrder/GetTax4Dtl'
    this.service.get(url).subscribe((result:any)=>{
      this.tax4_list = result.GetTax4Dtl;
     });


    var url = 'PmrTrnDirectInvoice/Discount'
    this.service.get(url).subscribe((result:any)=>{
      this.Getdiscount_list = result.Getdiscount_list;
     });

    var url = 'PmrTrnDirectInvoice/Additional'
    this.service.get(url).subscribe((result:any)=>{
      this.Getadditional_list = result.Getadditional_list;
     });

    var api = 'PmrTrnDirectInvoice/GetVendornamedropdown';
    debugger
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.vendor_list = this.responsedata.GetVendornamedropdown;
    });

    var url = 'PmrTrnDirectInvoice/GetcurrencyCodedropdown';
       this.service.get(url).subscribe((result:any)=>{
      this.currency_list = result.Getcurrencycodedropdown;
      this.mdlCurrencyName = this.currency_list[0].currencyexchange_gid;
         const defaultCurrency = this.currency_list.find((currency: { default_currency: string; }) => currency.default_currency === 'Y');
         const defaultCurrencyExchangeRate = defaultCurrency.exchange_rate;
         if (defaultCurrency) {
           this.mdlCurrencyName = defaultCurrency.currencyexchange_gid;
           this.InvoiceaddForm.get("exchange_rate")?.setValue(defaultCurrencyExchangeRate);
         }
      });
      var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
      this.service.get(api).subscribe((result: any) => {
        this.responsedata = result;
        this.terms_list = this.responsedata.GetTermsandConditions
      });
    var api = 'PmrTrnDirectInvoice/GetPurchaseTypedropDown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.GetPurchaseTypedropDown = result.GetPurchaseTypedropDown;
    });
    this.InvoiceaddForm.get('branch_name')?.valueChanges.subscribe((value: string) => {
      if (value) {
        this.OnChangeBranch();
      }
    });

    this.DIproductsummary();
    var api = 'PmrTrnDirectInvoice/PblGetAllChargesConfig';
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
        this.InvoiceaddForm.get("addoncharge")?.setValue(0);
      }

      if (this.allchargeslist[1].flag == 'Y') {
        this.additional_discount = 0;
      } else {
        this.additional_discount = this.allchargeslist[1].flag;
        this.InvoiceaddForm.get("additional_discount")?.setValue(0);
      }

      if (this.allchargeslist[2].flag == 'Y') {
        this.frieghtcharges = 0;
      } else {
        this.frieghtcharges = this.allchargeslist[2].flag;
        this.InvoiceaddForm.get("freightcharges")?.setValue(0);
      }

      if (this.allchargeslist[3].flag == 'Y') {
        this.forwardingCharges = 0;
      } else {
        this.forwardingCharges = this.allchargeslist[3].flag;
        this.InvoiceaddForm.get("forwardingCharges")?.setValue(0);
      }

      if (this.allchargeslist[4].flag == 'Y') {
        this.insurancecharges = 0;
      } else {
        this.insurancecharges = this.allchargeslist[4].flag;
        this.InvoiceaddForm.get("insurancecharges")?.setValue(0);
      }
    });

  }

  constructor(private fb: FormBuilder,private datePipe: DatePipe,
    private route: ActivatedRoute,
     private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {

    this.InvoiceaddForm = new FormGroup({
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      dispatch_name: new FormControl('', Validators.required),
      invoice_date: new FormControl(this.getCurrentDate(), Validators.required),
      vendor_companyname: new FormControl('', Validators.required),
      vendor_details: new FormControl(''),
      address1: new FormControl(''),
      email_id: new FormControl(''),
      contact_telephonenumber:new FormControl(''),
      shipping_address: new FormControl(''),
      tax_amount4: new FormControl(''),
      payment_date: new FormControl(''),
      purchasetype_name: new FormControl(''),
      vendor_ref_no: new FormControl(''),
      currency_code: new FormControl(''),
      payment_term: new FormControl(''),
      contact_person: new FormControl(''),
      email_address: new FormControl(''),
      contact_number: new FormControl(''),
      currency: new FormControl('', Validators.required),
      exchange_rate: new FormControl(''),
      remarks: new FormControl(''),
      due_date :  new FormControl (this.getCurrentDate()),
      vendor_address: new FormControl(''),
      vendor_fax: new FormControl(''),
      priority_n: new FormControl('N'),
      taxamount1: new FormControl(''),
      buybackorscrap: new FormControl(''),
      payment_terms: new FormControl(''),
      delivery_terms: new FormControl(''),
      freight_terms: new FormControl(''),
      delivery_location: new FormControl(''),
      scrap: new FormControl(''),
      template_content: new FormControl(''),
      delivery_period: new FormControl(''),
      payment_days: new FormControl('', [Validators.required]),
      product_total: new FormControl(''),
      tax_name: new FormControl(''),
      discount_percentage: new FormControl(''),
      qty: new FormControl(''),
      mrp: new FormControl(''),
      unitprice: new FormControl(''),
      productuom_name: new FormControl(''),
      product_remarks: new FormControl(''),
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
      directinvioce_remarks: new FormControl(''),
      roundoff: new FormControl(''),
      ship_via: new FormControl(''),
      invoice_ref_no: new FormControl(''),
      grandtotal: new FormControl('', [Validators.required]),
      additional_discount: new FormControl(''),
      insurance_charges: new FormControl(''),
      freightcharges: new FormControl(''),
      addoncharge: new FormControl(''),
      delivery_days: new FormControl('', [Validators.required]),
      template_name: new FormControl(''),
      total_amount: new FormControl(''),
      packing_charges: new FormControl(''),
      tax_name4: new FormControl(''),
      additionalamount: new FormControl(''),
      deductionamount: new FormControl(''),
      additional_name: new FormControl(''),
      discount_name: new FormControl(''),
      extra_deductionamount: new FormControl(''),
      extra_additionalamount: new FormControl(''),
      dispatch_mode : new FormControl(''),
      invoice_remarks: new FormControl(''),
      billing_mail: new FormControl(''),
      invoice_gid: new FormControl(''),
  
    })
    this.productform = new FormGroup({
      tmppurchaseorderdtl_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      product_remarks: new FormControl(''),
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
      template_content: new FormControl(''),
      producttype_name: new FormControl(''),
      producttype_gid: new FormControl(''),
      productquantity: new FormControl(''),
      productdiscount: new FormControl(''),
      productdiscount_amountvalue: new FormControl(''),
      tax_prefix: new FormControl(''),
      producttotal_amount: new FormControl(''),
      tax_prefix2: new FormControl(''),
    
   
    })

    this.InvoiceaddForm.get('payment_days').valueChanges.subscribe((days: any) => {
      this.calculateDueDate(days);
    });

  }

  calculateDueDate(days : any) {
    debugger
    const currentDate = new Date();
    currentDate.setDate(currentDate.getDate() + Number(days));
    const formattedDueDate = this.datePipe.transform(currentDate, 'dd-MM-yyyy');
    this.InvoiceaddForm.get('due_date').setValue(formattedDueDate);
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get shipping_address() {
    return this.InvoiceaddForm.get('shipping_address')!;
  }
  get vendor_address() {
    return this.InvoiceaddForm.get('vendor_address')!;
  }
  get contact_person() {
    return this.InvoiceaddForm.get('contact_person')!;
  }
  get contact_number() {
    return this.InvoiceaddForm.get('contact_number')!;
  }
  get vendor_fax() {
    return this.InvoiceaddForm.get('vendor_fax')!;
  }
  get email_address() {
    return this.InvoiceaddForm.get('email_address')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }

  get branch_name() {
    return this.InvoiceaddForm.get('branch_name')!;
  }
  get dispatch_name() {
    return this.InvoiceaddForm.get('dispatch_name')!;
  }
  get vendor_companyname() {
    return this.InvoiceaddForm.get('vendor_companyname')!;
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

    return this.InvoiceaddForm.get('payment_days')!;

  };
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }
  get currency_code() {
    return this.InvoiceaddForm.get('currency_code')!;
  }
  get invoice_date() {
    return this.InvoiceaddForm.get('invoice_date')!;
  }
  get invoice_ref_no() {
    return this.InvoiceaddForm.get('invoice_ref_no')!;
  }
get purchase_type(){
  return this.InvoiceaddForm.get("purchasetype_name");
}
  OnChangeBranch() {
    debugger
    let branch_gid = this.InvoiceaddForm.get("branch_name")?.value;
    let param = {
      branch_gid: branch_gid
    }
    var url = 'PmrTrnPurchaseOrder/GetOnChangeBranch';
    this.service.getparams(url, param).subscribe((result: any) => {
      const address1 = result.GetBranch[0].address1;
      const zip_code = result.GetBranch[0].postal_code;
      const city = result.GetBranch[0].city;
      const state = result.GetBranch[0].state;
      result.addressdetails = `${address1}\n${city}\n${state}\n${zip_code}`;
  this.InvoiceaddForm.get("shipping_address")?.setValue(result.addressdetails);

});

}
onclearBranch(){
this.txtshipto='';

}
  OnChangeVendor() {
    debugger
       let vendor_gid = this.InvoiceaddForm.get("vendor_companyname")?.value;
       let param ={
         vendor_gid :vendor_gid
       }
       var url = 'PmrTrnDirectInvoice/GetOnChangeVendor';
   
       this.service.getparams(url, param).subscribe((result: any) => {
         this.GetVendord = result.GetOnChangeVendor;
         
         const emailId = this.GetVendord[0].email_id;
         const contactTelephonenumber = this.GetVendord[0].contact_telephonenumber;
         const gstNo = this.GetVendord[0].gst_no;
         const vendorDetails = `${emailId}\n${contactTelephonenumber}\n${gstNo}`;
         this.InvoiceaddForm.get("vendor_details")?.setValue(vendorDetails);
   
         const address1 = this.GetVendord[0].address1;
          let address2 = this.GetVendord[0].address2;
          const zip_code = this.GetVendord[0].postal_code;
          const city = this.GetVendord[0].city;
        
      
          if (address2 === null || address2 === undefined || address2.trim() === '') {
            this.addressdetails = `${address1}\n${city}\n${zip_code}`;
          }
          else{
            this.addressdetails = `${address1}\n${address2}\n${city}\n${zip_code}`;
          }
      this.InvoiceaddForm.get("address1")?.setValue(this.addressdetails);
     
      // this.InvoiceaddForm.get("shipping_address")?.setValue(this.addressdetails);
         
         this.DIproductsummary();
       });
     }
onclearvendor(){
  this.txtvendordetails = null;
  this.txtbillto = null;
  
}
  GetOnChangeTerms() {
    let termsconditions_gid = this.InvoiceaddForm.value.template_name;
    let param = {
      termsconditions_gid: termsconditions_gid
    }
    var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.InvoiceaddForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
      this.InvoiceaddForm.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
    });

  }

  onInput(event: any) {
    const value = event.target.value;
    const parts = value.split('.');

    const integerPart = parts[0];
    let decimalPart = parts[1] || '';

    decimalPart = decimalPart.slice(0, 2);

    event.target.value = `${integerPart}.${decimalPart}`;
    this.unitprice = event.target.value;
  }

  productSearch() {
    debugger
    let vendor_gid = this.InvoiceaddForm.get("vendor_companyname")?.value;
  
    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
      vendor_gid: vendor_gid
    };
    var api = 'PmrTrnPurchaseOrder/GetProductsearchSummaryPurchase';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.POProductList1 = this.responsedata.GetpurchaseorderProductsearchs;
      this.filteredPOProductList1 = this.POProductList1;
      this.GetTaxSegmentList = this.responsedata.GetTaxSegmentList;
      // this.POProductList1 = this.responsedata.GetProductsearchs;
      // this.filteredPOProductList1 = this.POProductList1;
      // this.GetTaxSegmentList = this.responsedata.GetTaxSegmentListorder;
    });
  }
  

  searchOnChange(event: KeyboardEvent) {

    if (event.key !== 'Enter') {

      this.productSearch();
    }
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
  


  onclearbranch() {
    this.mdlDispatchadd = null;
  }
  onClearCurrency() {
    this.exchange = 0;
  }
  OnClearTax1(i: any) {
    this.POProductList1[i].taxamount1 = 0;
    const subtotal =  this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
    this.POProductList1[i].discount_amount = (subtotal * this.POProductList1[i].discount_persentage) / 100;
    this.POProductList1[i].discount_amount = +(this.POProductList1[i].discount_amount).toFixed(2);
    this.POProductList1[i].total_amount = +(subtotal - this.POProductList1[i].discount_amount + this.POProductList1[i].taxamount1 + this.POProductList1[i].taxamount2).toFixed(2);
    this.POProductList1[i].total_amount = +(this.POProductList1[i].total_amount).toFixed(2);
  }
  OnClearTax2(i: any) {
    this.POProductList1[i].taxamount2 = 0;
    const subtotal =  this.POProductList1[i].unitprice * this.POProductList1[i].quantity;
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
    let tax_gid = this.InvoiceaddForm.get('tax_name4')?.value;   
    this.taxpercentage = this.getDimensionsByFilter(tax_gid);
    let tax_percentage = this.taxpercentage[0].percentage;
    this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
    this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
    this.total_amount = +this.total_amount.toFixed(2);
    // this.grandtotal = ((this.total_amount) + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
    // Convert NaN to 0 for each field
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
  OnChangeTaxAmount2(i: any) {
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
      this.POProductList1[i].totalamount = (parseFloat(this.POProductList1[i].total_amount) + parseFloat(this.POProductList1[i].taxamount2));
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
  finaltotal() {
    debugger;

    // Ensure all variables are valid numbers
    const addoncharge = isNaN(this.addoncharge) ? 0 : +this.addoncharge;
    const freightcharges = isNaN(this.freightcharges) ? 0 : +this.freightcharges;
    const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
    const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
    const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
    const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
    const totalamount = isNaN(this.totalamount) ? 0 : +this.totalamount;
    // Log values for debugging
    console.log("Addon Charge:", addoncharge);
    console.log("Freight Charges:", freightcharges);
    console.log("Packing Charges:", packing_charges);
    console.log("Insurance Charges:", insurance_charges);
    console.log("Round Off:", roundoff);
    console.log("Additional Discount:", additional_discount);
    console.log("Tax Amount4:", tax_amount4);

    // Calculate the grand total
    this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
        packing_charges + insurance_charges + roundoff - additional_discount;

    // Log grand total for debugging
    console.log("Grand Total:", this.grandtotal);

    // Ensure the grand total is a valid number with 2 decimal places
    this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
}



  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'PmrTrnDirectInvoice/PblDeleteProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpinvoicedtl_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
        this.DIproductsummary();
        this.NgxSpinnerService.hide()
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.DIproductsummary();
        this.NgxSpinnerService.hide()
      }
    });
  }


  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Y';
  }
  OnChangeCurrency() {
    let currencyexchange_gid = this.InvoiceaddForm.get("currency_code")?.value;
    let param = {
      currencyexchange_gid: currencyexchange_gid
    }
    var url = 'PmrTrnDirectInvoice/PblGetOnChangeCurrency';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list1 = this.responsedata.PblGetOnchangeCurrency;
      this.InvoiceaddForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
      this.currency_code1 = this.currency_list1[0].currency_code
    });

  }
  onCurrencyCodeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    const selectedCurrencyCode = target.value;
    this.selectedCurrencyCode = selectedCurrencyCode;
    this.InvoiceaddForm.controls.currency_code.setValue(selectedCurrencyCode);
    this.InvoiceaddForm.get("currency_code")?.setValue(this.currency_list[0].currency_code);

  }

  productAddSubmit() {
    debugger
    if (this.InvoiceaddForm.value.vendor_companyname == "" || this.InvoiceaddForm.value.vendor_companyname == null || this.InvoiceaddForm.value.vendor_companyname == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Vendor!');
      return
    }
    if (this.productform.value.productquantity == "" || this.productform.value.productquantity == null || this.productform.value.productquantity== "0") {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Product quantity cannot be zero or empty!');
      return
    } 
    var postapi = 'PmrTrnDirectInvoice/PblPostOnAddproduct';
    this.NgxSpinnerService.show();
  debugger
    let param = {
      product_name: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      productgroup_name: this.productform.value.productgroup_name,
      product_remarks: this.productform.value.product_remarks,
      productquantity: this.productform.value.productquantity,
      unitprice: this.productform.value.unitprice,
      discountprecentage: this.productform.value.productdiscount,
      discount_amount: this.productform.value.productdiscount_amountvalue,
      producttotal_amount: this.productform.value.producttotal_amount,
      exchange_rate: this.CurrencyExchangeRate,
      vendor_gid: this.InvoiceaddForm.value.vendor_companyname,
      taxamount1: this.taxamount1,
      taxamount2: this.taxamount2,
      taxamount3: this.taxamount3,
      taxgid1: this.taxgid1,
      taxgid2: this.taxgid2,
      taxgid3: this.taxgid3,
      taxprecentage1: this.taxprecentage1,
      taxprecentage2: this.taxprecentage2,
      taxprecentage3: this.taxprecentage3,  
      taxsegment_gid: this.taxsegment_gid,
      taxname1:this.tax_prefix, 
    }
  
    this.service.post(postapi, param).subscribe((apiresponse: any) => {
      if (apiresponse.status == false) {
        this.ToastrService.warning(apiresponse.message);
        this.isNewProduct=false;
      }
      else {
        this.ToastrService.success(apiresponse.message);
        this.productform.reset();
        this.POProductList1 = [];
        
        this.DIproductsummary();
        this.isNewProduct=false;
      }
    });
    this.NgxSpinnerService.hide();
    this.productSearch();
  }


  
  onSubmit() {
    debugger
    if (this.productsummary_list == null || this.productsummary_list == undefined 
      
      )
      {
        window.scrollTo({
        top: 0,
      });

      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
   
    var params = {
      POProductList: this.POProductList1,
      invoice_ref_no: this.InvoiceaddForm.value.invoice_ref_no,
      invoice_date: this.InvoiceaddForm.value.invoice_date,
      branch_name: this.InvoiceaddForm.value.branch_name,
      vendor_companyname: this.InvoiceaddForm.value.vendor_companyname,
      vendor_details: this.InvoiceaddForm.value.vendor_details,
      address1: this.InvoiceaddForm.value.address1,      
      employee_name: this.InvoiceaddForm.value.employee_name,
      delivery_terms: this.InvoiceaddForm.value.delivery_terms,
      payment_terms: this.InvoiceaddForm.value.payment_terms,  
      Requestor_details: this.InvoiceaddForm.value.Requestor_details,    
      dispatch_mode: this.InvoiceaddForm.value.dispatch_mode,  
      currency_gid: this.InvoiceaddForm.value.currency_gid,
      currency_code: this.InvoiceaddForm.value.currency_code,
      exchange_rate: this.InvoiceaddForm.value.exchange_rate,
      template_name :this.InvoiceaddForm.value.template_name,
      template_content: this.InvoiceaddForm.value.template_content,
      template_gid: this.InvoiceaddForm.value.template_gid,
      totalamount: this.InvoiceaddForm.value.totalamount,
      addoncharge: this.InvoiceaddForm.value.addoncharge,    
      additional_discount: this.InvoiceaddForm.value.additional_discount,
      freightcharges: this.InvoiceaddForm.value.freightcharges,
      roundoff: this.InvoiceaddForm.value.roundoff,
      grandtotal: this.InvoiceaddForm.value.grandtotal,
      tax_gid: this.InvoiceaddForm.value.tax_gid,
      tax_name1: this.InvoiceaddForm.value.tax_name1,
      tax_name2: this.InvoiceaddForm.value.tax_name2,
      tax_name3: this.InvoiceaddForm.value.tax_name3,
      taxamount1: this.InvoiceaddForm.value.taxamount1,
      taxamount2: this.InvoiceaddForm.value.taxamount2,
      taxamount3: this.InvoiceaddForm.value.taxamount3,
      tax_amount4: this.InvoiceaddForm.value.tax_amount4,
      tax_name4: this.InvoiceaddForm.value.tax_name4,      
      taxsegment_gid: this.Cmntaxsegment_gid,
      shipping_address: this.InvoiceaddForm.value.shipping_address,
      due_date: this.InvoiceaddForm.value.due_date,
      billing_mail: this.InvoiceaddForm.value.billing_mail,
      invoice_remarks: this.InvoiceaddForm.value.invoice_remarks,
      purchase_type : this.InvoiceaddForm.value.purchasetype_name
     
    };
    var api = 'PmrTrnDirectInvoice/PblProductSubmit';
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
        this.router.navigate(['/payable/PmrTrnInvoice']);
      }
      this.NgxSpinnerService.hide()
    });

  }

  GetOnChangeProductsGroupName() {
    let product_gid = this.productform.value.product_name;
    let param = {
      product_gid: product_gid
    };
  
    var url = 'SmrTrnSalesorder/GetOnChangeProductGroupName';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("product_code")?.setValue(result.GetProdGrpName[0].product_code);
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }
  GetOnChangeProductsGroup() {
    debugger
    let productgroup_gid = this.productform.value.productgroup_name;
    let param = {
      productgroup_gid: productgroup_gid
    };
  
    var url = 'SmrTrnSalesorder/GetOnChangeProductGroup';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productgroup_list = result.GetCustomer;
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }

  OnProductCode(event: any) {
    const product_code = this.POProductList1.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        productgroup_name: product_code.productgroup_gid,
        product_remarks: product_code.product_desc
      });
    }
  }
  onProductSelect(event: any): void {
    debugger
  
    const product_name = this.POProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        producttype_name: product_name.producttype_name,
        product_remarks: product_name.product_desc,
      });
    }
    this.isNewProduct = event.producttype_name === 'Services';
  }
  
  prodcutaddtotalcal() {
    debugger
    const product_gid = this.productform.value.product_name;
    const vendor = this.InvoiceaddForm.value.vendor_companyname;
  
    if (vendor != null && vendor != undefined) {
      var producttax = 'PmrTrnPurchaseOrder/GetProductWithTaxSummary';
      let param = {
        product_gid: product_gid,
        vendor_gid: vendor
      };
      this.service.getparams(producttax, param).subscribe((result: any) => {
        this.GetTaxSegmentList = result.GetTaxSegmentListpurchaseorder;
  
  
        this.calculateProductTotal(product_gid);
      });
    } else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Select Vendor!');
      return;
    }
  }
  calculateProductTotal(product_gid: any) {
    debugger
    if (this.GetTaxSegmentList != null && this.GetTaxSegmentList != undefined) {
      const taxSegment = this.GetTaxSegmentList.filter(
        (segment: { product_gid: string; tax_percentage: number }) => segment.product_gid === product_gid
      );
  
      let tax_percentage = 0;
      let taxDetails = [];
  
      if (taxSegment.length > 0) {
        for (let i = 0; i < taxSegment.length; i++) {
          let percentageStr = taxSegment[i].tax_percentage;
          let percentage = parseFloat(percentageStr);
          if (isNaN(percentage)) {
            let numericPart = parseFloat(percentageStr.replace(/[^\d.]/g, ''));
            if (!isNaN(numericPart)) {
              tax_percentage += numericPart;
            }
          } else {
            tax_percentage += percentage;
            taxDetails.push({
              tax_percentage: percentage,
              product_gid: taxSegment[i].product_gid,
              tax_gid: taxSegment[i].tax_gid,
              tax_name: taxSegment[i].tax_name,
              taxsegment_gid: taxSegment[i].taxsegment_gid,
              tax_prefix: taxSegment[i].tax_prefix,
            });
          }
        }
      }
  
      const subtotal =  this.productform.value.unitprice * this.productform.value.productquantity;
      this.productdiscount_amountvalue = (subtotal * this.productform.value.productdiscount) / 100;
      this.productdiscounted_precentagevalue = subtotal - this.productdiscount_amountvalue;
  
      let individualTaxAmounts = taxDetails.map(detail => {
        let tax_amount = (this.productdiscounted_precentagevalue * detail.tax_percentage) / 100;
        return {
          product_gid: detail.product_gid,
          tax_gid: detail.tax_gid,
          tax_percentage: detail.tax_percentage,
          taxsegment_gid: detail.taxsegment_gid,
          tax_name: detail.tax_name,
          tax_amount: tax_amount.toFixed(2),
          tax_prefix:detail.tax_prefix,
        };
      });
  
      this.producttotal_tax_amount = individualTaxAmounts.reduce((sum, taxDetail) => sum + parseFloat(taxDetail.tax_amount), 0);
  
      const total = subtotal - this.productdiscount_amountvalue + this.producttotal_tax_amount;
  
      this.producttotal_amount = total.toFixed(2);
  
      this.individualTaxAmounts = individualTaxAmounts;
  
      if (this.individualTaxAmounts.length > 0) {
        if (this.individualTaxAmounts[0].tax_gid != null && this.individualTaxAmounts[0].tax_gid != undefined) {
          this.taxgid1 = this.individualTaxAmounts[0].tax_gid;
          this.taxname1 = this.individualTaxAmounts[0].tax_name;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.taxsegment_gid = this.individualTaxAmounts[0].taxsegment_gid;
        } else {
          this.taxgid1 = 0;
        }
  
        if (this.individualTaxAmounts[0].tax_amount != null && this.individualTaxAmounts[0].tax_amount != undefined) {
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount1 = 0;
        }
  
        if (this.individualTaxAmounts[0].tax_percentage != null && this.individualTaxAmounts[0].tax_percentage != undefined) {
          this.taxprecentage1 = this.individualTaxAmounts[0].tax_percentage;
        } else {
          this.taxprecentage1 = 0;
        }
      } else {
        this.taxgid1 = 0;
        this.taxamount1 = 0;
        this.taxprecentage1 = 0;
      }
  
      if (this.individualTaxAmounts.length > 1) {
        if (this.individualTaxAmounts[1].tax_gid != null && this.individualTaxAmounts[1].tax_gid != undefined) {
          this.taxgid2 = this.individualTaxAmounts[1].tax_gid;
          this.taxname2 = this.individualTaxAmounts[1].tax_name;
          // this.tax_prefix = this.individualTaxAmounts[1].tax_prefix;
          this.tax_prefix = this.individualTaxAmounts[0].tax_prefix;
          this.tax_prefix2 = this.individualTaxAmounts[1].tax_prefix;
        } else {
          this.taxgid2 = 0;
        }
  
        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
           this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
        this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount2 = 0;
        }
  
        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = 0;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }
  
      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
        } else {
          this.taxgid3 = 0;
        }
  
        if (this.individualTaxAmounts[2].tax_amount != null && this.individualTaxAmounts[2].tax_amount != undefined) {
          this.taxamount3 = this.individualTaxAmounts[2].tax_amount;
        } else {
          this.taxamount3 = 0;
        }
  
        if (this.individualTaxAmounts[2].tax_percentage != null && this.individualTaxAmounts[2].tax_percentage != undefined) {
          this.taxprecentage3 = this.individualTaxAmounts[2].tax_percentage;
        } else {
          this.taxprecentage3 = 0;
        }
      } else {
        this.taxgid3 = 0;
        this.taxamount3 = 0;
        this.taxprecentage3 = 0;
      }
  
    }
  else {
      const subtotal =  this.productform.value.unitprice * this.productform.value.productquantity;
      this.productdiscount_amountvalue = (subtotal * this.productform.value.productdiscount) / 100;
      this.productdiscounted_precentagevalue = subtotal - this.productdiscount_amountvalue;
  
      const total = subtotal - this.productdiscount_amountvalue + this.producttotal_tax_amount;
      this.producttotal_amount = total.toFixed(2);
     
  
      if (this.individualTaxAmounts.length > 0) {
        if (this.individualTaxAmounts[0].tax_gid != null && this.individualTaxAmounts[0].tax_gid != undefined) {
          this.taxgid1 = this.individualTaxAmounts[0].tax_gid;
          this.taxname1 = this.individualTaxAmounts[0].tax_name;
          this.taxsegment_gid = this.individualTaxAmounts[0].taxsegment_gid;
        } else {
          this.taxgid1 = 0;
        }
  
        if (this.individualTaxAmounts[0].tax_amount != null && this.individualTaxAmounts[0].tax_amount != undefined) {
          this.taxamount1 = this.individualTaxAmounts[0].tax_amount;
        } else {
          this.taxamount1 = 0;
        }
  
        if (this.individualTaxAmounts[0].tax_percentage != null && this.individualTaxAmounts[0].tax_percentage != undefined) {
          this.taxprecentage1 = this.individualTaxAmounts[0].tax_percentage;
        } else {
          this.taxprecentage1 = 0;
        }
      } else {
        this.taxgid1 = 0;
        this.taxamount1 = 0;
        this.taxprecentage1 = 0;
      }
  
      if (this.individualTaxAmounts.length > 1) {
        if (this.individualTaxAmounts[1].tax_gid != null && this.individualTaxAmounts[1].tax_gid != undefined) {
          this.taxgid2 = this.individualTaxAmounts[1].tax_gid;
          this.taxname2 = this.individualTaxAmounts[1].tax_name;
        } else {
          this.taxgid2 = 0;
        }
  
        if (this.individualTaxAmounts[1].tax_amount != null && this.individualTaxAmounts[1].tax_amount != undefined) {
          this.taxamount2 = this.individualTaxAmounts[1].tax_amount;
        } else {
          this.taxamount2 = 0;
        }
  
        if (this.individualTaxAmounts[1].tax_percentage != null && this.individualTaxAmounts[1].tax_percentage != undefined) {
          this.taxprecentage2 = this.individualTaxAmounts[1].tax_percentage;
        } else {
          this.taxprecentage2 = 0;
        }
      } else {
        this.taxgid2 = 0;
        this.taxamount2 = 0;
        this.taxprecentage2 = 0;
      }
  
      if (this.individualTaxAmounts.length > 2) {
        if (this.individualTaxAmounts[2].tax_gid != null && this.individualTaxAmounts[2].tax_gid != undefined) {
          this.taxgid3 = this.individualTaxAmounts[2].tax_gid;
          this.taxname3 = this.individualTaxAmounts[2].tax_name;
        } else {
          this.taxgid3 = 0;
        }
  
        if (this.individualTaxAmounts[2].tax_amount != null && this.individualTaxAmounts[2].tax_amount != undefined) {
          this.taxamount3 = this.individualTaxAmounts[2].tax_amount;
        } else {
          this.taxamount3 = 0;
        }
  
        if (this.individualTaxAmounts[2].tax_percentage != null && this.individualTaxAmounts[2].tax_percentage != undefined) {
          this.taxprecentage3 = this.individualTaxAmounts[2].tax_percentage;
        } else {
          this.taxprecentage3 = 0;
        }
      } else {
        this.taxgid3 = 0;
        this.taxamount3 = 0;
        this.taxprecentage3 = 0;
      }
    }
  }
  
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("producraddproductgroup_name").setValue('');
    this.productsearch = null;
    this.productremarks = null;

    
  }
  
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
    this.productremarks = null;
  }




  DIproductsummary() {
    let vendor_gid = this.InvoiceaddForm.get("vendor_companyname")?.value;
    var params = { vendor_gid: vendor_gid, product_gid: "" };
    var api = 'PmrTrnDirectInvoice/GetPblProductSummary';

    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.Pblproductsummary_list;
      
      this.totalamount = this.responsedata.grandtotal.toFixed(2);
      this.producttotalamount = this.responsedata.grandtotal.toFixed(2);
      this.InvoiceaddForm.get("totalamount")?.setValue(this.responsedata.grand_total.toFixed(2));
      this.InvoiceaddForm.get("grandtotal")?.setValue(this.responsedata.grandtotal.toFixed(2));
      this.currency_code1 = "";

      this.productsummary_list.forEach((product: any, index: number) => {
        this.fetchProductSummaryAndTax(product.product_gid, index);
      });
      this.productSearch();
    });
  }

  fetchProductSummaryAndTax(product_gid: string, index: number) {
    if (this.InvoiceaddForm.value.vendor_companyname !== undefined) {
      let vendor_gid = this.InvoiceaddForm.get("vendor_companyname")?.value;
      let param = {
        product_gid: product_gid,
        vendor_gid: vendor_gid
      };

      var api = 'PmrTrnDirectInvoice/GetPblProductSummary';

      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.GetTaxSegmentList = this.responsedata.PblGetTaxSegmentList;
        this.handleTaxSegments(product_gid, this.GetTaxSegmentList, index);
      }, (error) => {
        console.error('Error fetching tax details:', error);
      });
    }
  }

  handleTaxSegments(product_gid: string, taxSegments: any[], index: number) {
    const productTaxSegments = taxSegments.filter((taxSegment: { product_gid: string; }) => taxSegment.product_gid === product_gid);

    if (productTaxSegments.length > 0) {
      this.productsummary_list.forEach((product: { product_gid: string; taxSegments: any[]; }, i: number) => {
        if (product.product_gid === product_gid) {
          product.taxSegments = productTaxSegments;
          this.prodtotalcal(index);
          console.log(this.total_amount_including_tax,'-log' )
        }
      });
    } else {
      console.warn('No tax segments found for product_gid:', product_gid);
    }
  }



  checkDuplicateTaxSegment(taxSegments: any[], currentIndex: number): boolean {

    const currentTaxSegmentId = taxSegments[currentIndex].taxsegment_gid;


    for (let i = 0; i < currentIndex; i++) {
      if (taxSegments[i].taxsegment_gid === currentTaxSegmentId) {
        return true;
      }
    }
    return false;
  }

  removeDuplicateTaxSegments(taxSegments: any[]): any[] {
    const uniqueTaxSegmentsMap = new Map<string, any>();
    taxSegments.forEach(taxSegment => {
      uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
    });
    return Array.from(uniqueTaxSegmentsMap.values());
  }

  onKeyPress(event: any) {
    const key = event.key;
    if (!/^[0-9.]$/.test(key)) {
      event.preventDefault();
    }
  }
  redirecttolist(){
    this.router.navigate(['/payable/PmrTrnInvoice']);
  
  }

  drafts(){
    this.GetpurchaserinvoiceDraft();
  }
  GetpurchaserinvoiceDraft() {
    var url = 'PmrTrnDirectInvoice/GetInvoiceDraftsSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#GetDraftinvoice').DataTable().destroy();
      this.responsedata = result;
       this.GetDraftinvoice = this.responsedata.GetDraftinvoice;
      this.NgxSpinnerService.hide()
      
      setTimeout(() => {
        $('#GetDraftinvoice').DataTable();
      }, 1);


    })

  }
  ondraft() {
    debugger
    if (
      this.InvoiceaddForm.value.vendor_companyname == "" || this.InvoiceaddForm.value.vendor_companyname == null || this.InvoiceaddForm.value.vendor_companyname == undefined &&
      this.InvoiceaddForm.value.branch_name == "" || this.InvoiceaddForm.value.branch_name == null || this.InvoiceaddForm.value.branch_name == undefined) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return
    }
  
   
    var params = {
      POProductList: this.POProductList1,
      invoice_ref_no: this.InvoiceaddForm.value.invoice_ref_no,
      invoice_date: this.InvoiceaddForm.value.invoice_date,
      branch_name: this.InvoiceaddForm.value.branch_name,
      vendor_companyname: this.InvoiceaddForm.value.vendor_companyname,
      vendor_details: this.InvoiceaddForm.value.vendor_details,
      address1: this.InvoiceaddForm.value.address1,      
      employee_name: this.InvoiceaddForm.value.employee_name,
      delivery_terms: this.InvoiceaddForm.value.delivery_terms,
      payment_terms: this.InvoiceaddForm.value.payment_terms,  
      Requestor_details: this.InvoiceaddForm.value.Requestor_details,    
      dispatch_mode: this.InvoiceaddForm.value.dispatch_mode,  
      currency_gid: this.InvoiceaddForm.value.currency_gid,
      currency_code: this.InvoiceaddForm.value.currency_code,
      exchange_rate: this.InvoiceaddForm.value.exchange_rate,
      template_name :this.InvoiceaddForm.value.template_name,
      template_content: this.InvoiceaddForm.value.template_content,
      template_gid: this.InvoiceaddForm.value.template_gid,
      totalamount: this.InvoiceaddForm.value.totalamount,
      addoncharge: this.InvoiceaddForm.value.addoncharge,    
      additional_discount: this.InvoiceaddForm.value.additional_discount,
      freightcharges: this.InvoiceaddForm.value.freightcharges,
      roundoff: this.InvoiceaddForm.value.roundoff,
      grandtotal: this.InvoiceaddForm.value.grandtotal,
      tax_gid: this.InvoiceaddForm.value.tax_gid,
      tax_name1: this.InvoiceaddForm.value.tax_name1,
      tax_name2: this.InvoiceaddForm.value.tax_name2,
      tax_name3: this.InvoiceaddForm.value.tax_name3,
      taxamount1: this.InvoiceaddForm.value.taxamount1,
      taxamount2: this.InvoiceaddForm.value.taxamount2,
      taxamount3: this.InvoiceaddForm.value.taxamount3,
      tax_amount4: this.InvoiceaddForm.value.tax_amount4,
      tax_name4: this.InvoiceaddForm.value.tax_name4,      
      taxsegment_gid: this.Cmntaxsegment_gid,
      shipping_address: this.InvoiceaddForm.value.shipping_address,
      due_date: this.InvoiceaddForm.value.due_date,
      billing_mail: this.InvoiceaddForm.value.billing_mail,
      invoice_remarks: this.InvoiceaddForm.value.invoice_remarks,
      purchase_type : this.InvoiceaddForm.value.purchasetype_name,
     invoice_gid : this.InvoiceaddForm.value.invoice_gid,
    };
    var api = 'PmrTrnDirectInvoice/PblDraftOverallSubmit';
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
        this.router.navigate(['/payable/PblTrnDirectInvoiceAdd']);
        window.location.reload();
      }
      this.NgxSpinnerService.hide()
    });
   
    
  }
  Drafts(invoice_gid: any) {
    debugger
    var url = 'PmrTrnDirectInvoice/GetPmrTrnInvoiceDraft'
       let param = {invoice_gid : invoice_gid}
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata=result;
      this.invoice_listsedit = result.invoice_listsedit;      
      this.InvoiceaddForm.get("branch_gid")?.setValue(this.invoice_listsedit[0].branch_gid);
      this.InvoiceaddForm.get("invoice_ref_no")?.setValue(this.invoice_listsedit[0].invoice_refno);
      this.InvoiceaddForm.get("invoice_gid")?.setValue(this.invoice_listsedit[0].invoice_gid);
      this.InvoiceaddForm.get("invoice_date")?.setValue(this.invoice_listsedit[0].invoice_date);
      this.InvoiceaddForm.get("vendor_companyname")?.setValue(this.invoice_listsedit[0].vendor_gid);
      this.InvoiceaddForm.get("vendor_details")?.setValue(this.invoice_listsedit[0].contactperson_name);
      this.InvoiceaddForm.get("address1")?.setValue(this.invoice_listsedit[0].vendor_address);
      this.InvoiceaddForm.get("shipping_address")?.setValue(this.invoice_listsedit[0].shipping_address);
      // this.InvoiceaddForm.get("currency_code")?.setValue(this.invoice_listsedit[0].currency_code);
      this.InvoiceaddForm.get("exchange_rate")?.setValue(this.invoice_listsedit[0].exchange_rate);
     this.InvoiceaddForm.get("delivery_terms")?.setValue(this.invoice_listsedit[0].delivery_term);
      this.InvoiceaddForm.get("payment_terms")?.setValue(this.invoice_listsedit[0].payment_term);
      this.InvoiceaddForm.get("dispatch_mode")?.setValue(this.invoice_listsedit[0].mode_despatch);
      this.InvoiceaddForm.get("due_date")?.setValue(this.invoice_listsedit[0].payment_date);
      this.InvoiceaddForm.get("billing_mail")?.setValue(this.invoice_listsedit[0].billing_email);
      this.InvoiceaddForm.get("invoice_remarks")?.setValue(this.invoice_listsedit[0].invoice_remarks);
    //  this.InvoiceaddForm.get("template_name")?.setValue(this.invoice_listsedit[0].template_name);
     this.InvoiceaddForm.get("template_content")?.setValue(this.invoice_listsedit[0].termsandconditions);    
      // this.InvoiceaddForm.get("totalamount")?.setValue(this.invoice_listsedit[0].total_amount);

      this.InvoiceaddForm.get("addoncharge")?.setValue(this.invoice_listsedit[0].additionalcharges_amount);
      this.InvoiceaddForm.get("additional_discount")?.setValue(this.invoice_listsedit[0].discount_amount);
       this.InvoiceaddForm.get("freightcharges")?.setValue(this.invoice_listsedit[0].freightcharges);
       this.InvoiceaddForm.get("tax_name4")?.setValue(this.invoice_listsedit[0].tax1_gid);
       this.InvoiceaddForm.get("tax_amount4")?.setValue(this.invoice_listsedit[0].tax_amount);
       this.InvoiceaddForm.get("roundoff")?.setValue(this.invoice_listsedit[0].round_off);
       this.InvoiceaddForm.get("currency_code")?.setValue(this.invoice_listsedit[0].currencyexchange_gid);
       this.InvoiceaddForm.get("purchasetype_name")?.setValue(this.invoice_listsedit[0].purchase_type);

      //  this.InvoiceaddForm.get("grandtotal")?.setValue(this.invoice_listsedit[0].invoice_amount);
      const emailId = this.invoice_listsedit[0].email_id;
      const contactTelephonenumber = this.invoice_listsedit[0].contact_telephonenumber;
      const gstNo = this.invoice_listsedit[0].gst_no;
      const vendorDetails = `${emailId}\n${contactTelephonenumber}`;
      this.InvoiceaddForm.get("vendor_details")?.setValue(vendorDetails); 
       this.DIproductsummary();   
 
    });
    this.DIproductsummary();   
  }




}

