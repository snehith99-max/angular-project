import { ChangeDetectorRef, Component, NgZone } from '@angular/core';
import { FormBuilder,FormControl,FormGroup,Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SelectionModel } from '@angular/cdk/collections';



export class IunAssign {
  GetPurchaseOrder1: any;


}

@Component({
  selector: 'app-smr-trn-raise-purchaseorder',
  templateUrl: './smr-trn-raise-purchaseorder.component.html',
  styleUrls: ['./smr-trn-raise-purchaseorder.component.scss']
})
export class SmrTrnRaisePurchaseorderComponent {
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
  
 
  summary_list1: any[] = [];
  CurObj: IunAssign = new IunAssign();
  pick: Array<any> = [];
  allchargeslist: any[] = [];
  data!: any[];
  branch_gid_key: any;
  encryptedSelectedItems_key: any;
  response_data:any;
  mdlBranchName:any;
  branch_list:any;
  branch_list1:any;
  vendor_list:any;
  mdlVendorName:any;
  mdlBill:any;
  product_name : string = '';
  mdlship:any;
  stockamendsummary : any[] = [];
  POAddForm: FormGroup | any;
  productform: FormGroup | any;
  marks: number = 0;
  unitprice: number = 0;
  exchange:any;
  currency_list:any;
  mdlCurrencyName:any;
  mdlQuotationName:any;
  Quotation_list:any;
  mdlVendorphoneno:any;
  mdlvendoraddress:any;
  mdlemailaddress :any;
  mdlcontactperson :any;
  mdlfax :any;
  POProductList1: any[] = [];
  purchaseorder_list: any[] = [];
  GetPurchaseOrder1: any[] = [];
  GetPurchaseOrdertax: { [key: string]: any[] } = {};
  GetTaxSegmentList: any[] = [];
  tax_list: any[] = [];
  insurance_charges: number = 0;
  additional_discount: number = 0;
  packing_charges: number = 0;
  tax_amount4:number=0;
  invoicediscountamount: number = 0;
  insurancecharges: number = 0;
  forwardingCharges: number = 0;
  frieghtcharges: number = 0;
  addoncharge: number = 0;
  grandtotal: number = 0;
  producttotalamount: number = 0;
  totalamount: number = 0;
  roundoff: number = 0;
  freightcharges : number = 0;
  mdlTaxName4:any;
  tax4_list:any;
  delivery_days : number = 0;
  total_amount: number = 0;
  mdlTerms:any;
  Getemployeelist:any;
  taxpercentage: any;
   responsedata:any;
  currency_code1:any;
  currency_list1:any;
  selection = new SelectionModel<IunAssign>(true, []);
  selectedCurrencyCode:any;
  file!: File;
  terms_list:any;
  totTaxAmountPerQty: any;
  GetVendord:any;
  Cmntaxsegment_gid:any;
  mdlTaxSegment: any;
  taxSegmentFlag: any;
  tax_prefix2: any;
  tax_prefix: any;

  showNoTaxSegments: boolean | undefined;
  taxsegment_gid: any;
 taxGids: string[] = [];
  taxseg_taxgid: string | undefined;
  taxseg_tax_amount= 0;
  tax_amount: number = 0;
  taxseg_tax: any;
  mdlUserName: any;
  Mdlrequsterdtl: any;
  user_list : any;
  taxamount1: number = 0;
  taxamount2: number = 0;
  tstax_name : any;
  discount : any;
  discountAmount : any;
  amount : any;
  discount_amount:any;
  

 // ---------------------------
 productCodes: any[] = []; 
 vendorError: any;
 sam: boolean = false;
   
   arrowfst: boolean = false;
   arrowOne: boolean = false;
   
   GetproductsCode: any;
   prod_name: any;
   selectedProductGID: any;
   totalTaxAmount1: any;
   Getproductgroup: any;
   Getproductdeletetemp: any;
   filteredPOProductList1: any[] = [];
   productdiscount_amountvalue: any;
   productdiscounted_precentagevalue: any;
   producttotal_tax_amount: number=0;
   producttotal_amount: any;
   individualTaxAmounts: any = 0;  
   productquantity: number = 0;
   productunitprice: number = 0;
   productdiscount: number = 0;
   discount_percentage: number = 0;
   discountpercentage: number = 0;
   discountamount: any;
   taxgid1: any;
   taxname1: any;
   taxprecentage1: any;
   taxgid2: any;
   taxname2: any;
   taxprecentage2: any;
   taxgid3: any;
   taxname3: any;
   taxprecentage3: any;
   txtvendordetails: any;
   txtbillto: any;
   txtshipto: any;
   CurrencyExchangeRate: any;
   addressdetails: any;
   

   currency_code: any
   
   tax1: any;
   tax2: any;
   selectedValue: string = '';
  
   product_list: any;
 
   
   producttype_list:any;

   dispatch_list: any;
   productorder_list:any;
   mdlDispatchadd:any;
  
   productsummary_list:any;
   netamount:any;
   overall_tax:any;

   // payment_day : number = 0;
   productcode_list: any;
   productgroup_list: any;

   GetBranch: any [] = [];
  
   productunit_list: any;
   mdlProductName: any;

   mdlProductGroup: any;
   mdlProductUnit: any;
   mdlProductCode: any;

   mdlproducttype: any;
 
   mdlDispatchName: any;

   mdlTaxName1: any;
   mdlTaxName2: any;
   mdlTaxName3: any;
   prototal: number = 0;
   quantity: number = 0;
 
   
   POdiscountamount: number = 0;

   taxamount : number =0;

   productdetails_list: any;
   productSaleseorder_list:any;

   taxamount3: any = 0;
   productamount:number=0;

   parameterValue: string | undefined;

   productnamelist: any;

   POadd_list: any;

   buybackorscrap: any;
  
   mdlProductcode:any;
   mdlProductunit:any;

   mdlvendorfax:any;
 
   file_path:any;
   file_name:any;
   searchText: any;
   productsearch: any;
   productcodesearch: any;
   productcodesearch1: any;
   productname: any;
   product_description:any;
   purchaserequisition_gidenc: any;
   purchaseorder: any;
   salesordergid: any;
   

 // ----------------------------------


  constructor(private formBuilder: FormBuilder,
  private ToastrService: ToastrService,
  private router: ActivatedRoute,
  private route: Router,
  public service: SocketService,
  public NgxSpinnerService: NgxSpinnerService,private cdr: ChangeDetectorRef,private zone: NgZone) {


    this.POAddForm = new FormGroup({
      purchaseorder_gid : new FormControl(''),
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      dispatch_name: new FormControl('', Validators.required),
      po_date: new FormControl(this.getCurrentDate(), Validators.required),
      expected_date: new FormControl(this.getCurrentDate(), Validators.required),
      vendor_companyname: new FormControl('', Validators.required),
      raised_by	: new FormControl(''),    
      email_id: new FormControl(''),
      contact_telephonenumber: new FormControl(''),
      remarks: new FormControl(''),
      contact_person: new FormControl(''),
      email_address: new FormControl(''),
      vendor_phoneno: new FormControl(''),
      poref_no: new FormControl(''),
      disbranch_name: new FormControl(''),
      priority_flag: new FormControl(''),
      Quotation_name: new FormControl(''),
      vendor_address: new FormControl(''),
      shipping_address: new FormControl(''),
      po_covernote: new FormControl(''),
      payment_terms: new FormControl(''),
      freight_terms: new FormControl(''),
      currency_code: new FormControl(''),
      exchange_rate: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      productuom_name: new FormControl(''),
      qty_ordered: new FormControl(''),
      product_price: new FormControl(''),
      discount_percentage  : new FormControl(''),    
      tax_amount    : new FormControl(''),
      total_amount: new FormControl(''),      
      needby_date: new FormControl(''),
      template_name: new FormControl(''),
      template_content: new FormControl(''),
      totalamount: new FormControl(''),
      tax_name4: new FormControl(''),
      tax_amount4: new FormControl(''),
      addoncharge: new FormControl(''),
      freightcharges: new FormControl(''),
      packing_charges: new FormControl(''),
      insurance_charges: new FormControl(''),
      additional_discount: new FormControl(''),
      roundoff: new FormControl(''),
      grandtotal: new FormControl(''),
      totalamount3: new FormControl(''),
      tax_name3: new FormControl(''),
      taxamount2: new FormControl(''),
      tax_name2: new FormControl(''),
      tax_name1: new FormControl(''),
      branch_gid: new FormControl(''),
      vendor_gid: new FormControl(''),
      product_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      tmppurchaseorderdtl_gid : new FormControl(''),
      discount_amount : new FormControl(''),
      tax_percentage : new FormControl(''),
      tax_percentage2 : new FormControl(''),
      tax_percentage3 : new FormControl(''),
      tax_amount2 : new FormControl(''),
      tax_amount3 : new FormControl(''),
      product_totalprice : new FormControl(''),
      excise_percentage : new FormControl(''),
      excise_amount : new FormControl(''),
      product_total : new FormControl(''),
      tax_name : new FormControl(''),
      producttype_gid : new FormControl(''),  
      display_field : new FormControl(''),
      purchaserequisitiondtl_gid : new FormControl(''),
      user_name : new FormControl(''),
      quotation_gid : new FormControl(''),
      buybackcharge : new FormControl(''),
      shipvia : new FormControl(''),
      // products: this.formBuilder.array([]) 
      tstaxsegment_gid : new FormControl(''),
      tstaxsegment_name : new FormControl(''),
      tstax_name : new FormControl(''),
      tstax_gid : new FormControl(''),
      tstax_percentage : new FormControl(''),
      tstax_amount : new FormControl(''),
      tsproduct_name : new FormControl(''),
      tsproduct_gid : new FormControl(''),
      tscost_price : new FormControl(''),
      tsmrp_price : new FormControl(''),
      address1 : new FormControl(''),
      employee_name : new FormControl(''),
      Requestor_details : new  FormControl(''),
      tax_prefix2 : new FormControl(''),
      tax_prefix : new FormControl(''),
      despatch_mode : new FormControl(''),
      taxamount1 : new FormControl(''),
      purchaserequisition_gid:new FormControl(''),
      delivery_terms:new FormControl(''),
      contactperson_name:new FormControl(''),
      

    })

    // this.productform = new FormGroup({
    //   product_gid : new FormControl(''), 
    //   productgroup_name : new FormControl(''), 
    //   product_code : new FormControl(''), 
    //   product_name : new FormControl(''), 
    //   productuom_name : new FormControl(''), 
    //   qty_ordered : new FormControl(''), 
    //   product_price : new FormControl(''), 
    //   discount_percentage : new FormControl(''), 
    //   tax_prefix2 : new FormControl(''), 
    //   tax_prefix : new FormControl(''), 
    //   taxamount2 : new FormControl(''), 
    //   taxamount1 : new FormControl(''), 
    //   total_amount : new FormControl(''), 
    //   unitprice: new FormControl(''), 
    //   productdiscount: new FormControl(''), 
    //   producttotal_amount: new FormControl(''), 
    //   productdiscount_amountvalue: new FormControl(''), 
      
    // })
  
}
getCurrentDate(): string {
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();
 
  return dd + '-' + mm + '-' + yyyy;
}

  ngOnInit(): void {
    const purchaseorder = this.router.snapshot.paramMap.get('salesorder_gid');
     const secretKey = 'storyboarderp';
     this.purchaseorder = purchaseorder;
     const deencryptedParam = AES.decrypt(this.purchaseorder, secretKey).toString(enc.Utf8);
     this.POproductsummary(deencryptedParam);
     this.salesordergid= deencryptedParam;
    //  this.GetProductdelivery(deencryptedParam);
  
    
     
  // const encryptedBranchGid = this.router.snapshot.paramMap.get('branch_gid');
  // const encryptedSelectedItems = this.router.snapshot.paramMap.get('purchaserequisition_gid');

  // const secretKey = 'storyboarderp';

  // this.branch_gid_key = encryptedBranchGid;
  // this.encryptedSelectedItems_key = encryptedSelectedItems;

  // const branch_gid1 = AES.decrypt(this.branch_gid_key, secretKey).toString(enc.Utf8);
  // const purchaserequisition_gid1 = AES.decrypt(this.encryptedSelectedItems_key, secretKey).toString(enc.Utf8);


  // this.GetViewPurchaseOrderSummary(purchaserequisition_gid1);
  // this.purchaserequisition_gidenc = AES.decrypt(this.encryptedSelectedItems_key, secretKey).toString(enc.Utf8);
  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options);

   this.POproductsummary(deencryptedParam);
  // var api = 'PmrTrnPurchaseOrder/GetBranch';
  // this.service.get(api).subscribe((result: any) => {
  //   this.responsedata = result;
  //   this.branch_list = this.responsedata.GetBranch;
  // });
  var url = 'PmrTrnPurchaseOrder/GetBranch'
  this.service.get(url).subscribe((result: any) => {
    this.branch_list = result.GetBranch;
    const firstBranch = this.branch_list[0];
    const branchName = firstBranch.branch_gid;
    this.POAddForm.get('branch_name')?.setValue(branchName);
  });

  
  var api = 'PmrTrnPurchaseOrder/GetBranch';
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.branch_list1 = this.responsedata.GetBranch;

  });
  var api = 'PmrTrnPurchaseOrder/Getuser';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.user_list = result.Getuser;
    });
  var api = 'PmrTrnPurchaseOrder/GetVendor';
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.vendor_list = this.responsedata.GetVendor;

  });
   
  var url = 'PmrTrnPurchaseOrder/GetTax4Dtl'
  this.service.get(url).subscribe((result:any)=>{
    this.tax4_list = result.GetTax4Dtl;
   });
  var url = 'PmrTrnPurchaseOrder/GetCurrency';
  this.service.get(url).subscribe((result:any)=>{
 this.currency_list = result.GetCurrency;  
 this.mdlCurrencyName = this.currency_list[0].currencyexchange_gid;
    const defaultCurrency = this.currency_list.find((currency: { default_currency: string; }) => currency.default_currency === 'Y');
    const defaultCurrencyExchangeRate = defaultCurrency.exchange_rate;
    if (defaultCurrency) {
      this.mdlCurrencyName = defaultCurrency.currencyexchange_gid;
      this.POAddForm.get("exchange_rate")?.setValue(defaultCurrencyExchangeRate);
    }

 });
 var api = 'PmrTrnPurchaseQuotation/GetTermsandConditions';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.terms_list = this.responsedata.GetTermsandConditions
    });

 var api = 'PmrTrnPurchaseOrder/GetTax';
 this.service.get(api).subscribe((result: any) => {
   this.responsedata = result;
   this.tax_list = result.GetTax;

 });
 this.POAddForm.get('branch_name')?.valueChanges.subscribe((value: string) => {
  if (value) {
    this.OnChangeBranch();
  }
});
  
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
     this.POAddForm.get("addoncharge")?.setValue(0);
   }

   if (this.allchargeslist[1].flag == 'Y') {
     this.additional_discount = 0;
   } else {
     this.additional_discount = this.allchargeslist[1].flag;
     this.POAddForm.get("additional_discount")?.setValue(0);
   }

   if (this.allchargeslist[2].flag == 'Y') {
     this.freightcharges = 0;
   } else {
     this.freightcharges = this.allchargeslist[2].flag;
     this.POAddForm.get("freightcharges")?.setValue(0);
   }

   if (this.allchargeslist[3].flag == 'Y') {
     this.forwardingCharges = 0;
   } else {
     this.forwardingCharges = this.allchargeslist[3].flag;
     this.POAddForm.get("forwardingCharges")?.setValue(0);
   }

   if (this.allchargeslist[4].flag == 'Y') {
     this.insurancecharges = 0;
   } else {
     this.insurancecharges = this.allchargeslist[4].flag;
     this.POAddForm.get("insurancecharges")?.setValue(0);
   }
 });
}

// GetViewPurchaseOrderSummary(purchaserequisition_gid : any) {
   
//   var url='PmrTrnPurchaseorderAddselect/GetRaisePOSummary'
//   let param = {
//     purchaserequisition_gid : purchaserequisition_gid
//   }
//   this.service.getparams(url,param).subscribe((result:any)=>{
//   this.purchaseorder_list = result.GetRaisePO;
//  this.POAddForm.get("branch_name")?.setValue(this.purchaseorder_list[0].branch_name)
// //  this.POAddForm.get("po_date")?.setValue(this.purchaseorder_list[0].purchaserequisition_date);
//  this.POAddForm.get("purchaserequisition_gid")?.setValue(this.purchaseorder_list[0].purchaserequisition_gid);
//   this.POAddForm.get("email_address")?.setValue(this.purchaseorder_list[0].email_id)
//  this.POAddForm.get("vendor_phoneno")?.setValue(this.purchaseorder_list[0].contact_telephonenumber)
//   this.POAddForm.get("address1")?.setValue(this.purchaseorder_list[0].address1)
//   this.POAddForm.get("vendor_address")?.setValue(this.purchaseorder_list[0].vendor_address)
//   this.POAddForm.get("shipping_address")?.setValue(this.purchaseorder_list[0].address1)
//   this.POAddForm.get("employee_name")?.setValue(this.purchaseorder_list[0].requested_by)
//   this.POAddForm.get("Requestor_details")?.setValue(this.purchaseorder_list[0].requested_details)
//   this.POAddForm.get("disbranch_name")?.setValue(this.purchaseorder_list[0].branch_name)
//  this.POAddForm.get("payment_terms")?.setValue(this.purchaseorder_list[0].payment_terms)
//  this.POAddForm.get("po_covernote")?.setValue(this.purchaseorder_list[0].po_covernote)
//  this.POAddForm.get("totalamount")?.setValue(this.purchaseorder_list[0].netamount)
//   this.POAddForm.get("addoncharge")?.setValue(this.purchaseorder_list[0].addon_amount)
//  this.POAddForm.get("additional_discount")?.setValue(this.purchaseorder_list[0].discount_amount)
//   this.POAddForm.get("freightcharges")?.setValue(this.purchaseorder_list[0].freightcharges)
//  this.POAddForm.get("tax_name4")?.setValue(this.purchaseorder_list[0].overalltaxname)
//  this.POAddForm.get("tax_amount4")?.setValue(this.purchaseorder_list[0].overall_tax)
//  this.POAddForm.get("roundoff")?.setValue(this.purchaseorder_list[0].roundoff)
//   this.POAddForm.get("grandtotal")?.setValue(this.purchaseorder_list[0].total_amount)
//   this.POAddForm.get("template_content")?.setValue(this.purchaseorder_list[0].termsandconditions)
// });
// }

get branch_name() {
  return this.POAddForm.get('branch_name')!;
}
get vendor_companyname() {
  return this.POAddForm.get('vendor_companyname')!;
}
get payment_days() {

  return this.POAddForm.get('payment_days')!;

};
onclearBranch(){
  this.txtshipto='';

}

onclearvendor(){
  this.mdlVendorName ='';
  this.mdlcontactperson ='';
  this.mdlfax ='';
  this.mdlemailaddress = '';
  this.mdlVendorphoneno ='';
  this.mdlvendoraddress ='';
}

onAnchorClick(event:any){
  debugger
  if(this.file_path !=null && this.file_path != ""){
        const file_name = this.file_name;
        const image = this.file_path.split('.net/');
      const page = image[0];
     const url = page.split('?');
      const imageurl = url[0];
      const parts = imageurl.split('.');
      const extension = parts.pop();

      let params = {
        file_path: imageurl,
      file_name: file_name
      }
      this.service.post('SmrTrnSalesorder/DownloadDocument',params).subscribe(
        (data: any) => {
        if (data != null){
          this.service.fileviewer(data);

        }
      
      });
    }

    else{
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("No file has been uploaded for this SO to PO");
    }
}
removeFile() {
  
  this.file_name = null;
  this.file_path = null;
}



OnChangeBranch() {
   
  let branch_gid = this.POAddForm.get("branch_name")?.value;
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
this.POAddForm.get("shipping_address")?.setValue(result.addressdetails);

});

}
OnChangeVendor() {

   
  let vendor_gid = this.POAddForm.get("vendor_companyname")?.value;
  let param ={
    vendor_gid :vendor_gid
  }
  var url = 'PmrTrnPurchaseOrder/GetOnChangeVendor';
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.productSearch();
    this.GetVendord = result.GetVendor;
    const email_id = result.GetVendor[0].email_id
    const vendor_phoneno = (result.GetVendor[0].contact_telephonenumber);
    const contactperson =(result.GetVendor[0].contactperson_name);
    const vendorDetails = `${contactperson}\n${vendor_phoneno}\n${email_id}`;
    this.Cmntaxsegment_gid =result.GetVendor[0].taxsegment_gid;
     this.POAddForm.get("vendor_address")?.setValue(vendorDetails);
     this.POAddForm.get("email_address")?.setValue(result.GetVendor[0].email_id);
     this.POAddForm.get("address1")?.setValue(result.GetVendor[0].address1)
     this.POAddForm.value.vendorregister_gid = result.GetVendor[0].vendorregister_gid  ;
     
    
    
  });

}



OnChangeVendor1() {
  let vendor_gid = this.POAddForm.get("vendor_companyname")?.value;
  let param = {
    vendor_gid: vendor_gid
  };
  var url = 'PmrTrnPurchaseOrder/GetOnChangeVendor';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.productSearch();
    this.GetVendord = result.GetVendor;
    const email_id = this.GetVendord[0].email_id;
    const vendor_phoneno = this.GetVendord[0].contact_telephonenumber;
    const contactperson = this.GetVendord[0].contactperson_name;
    const gstNo = this.GetVendord[0].gst_no;
    const vendorDetails = `${contactperson}\n${vendor_phoneno}\n${email_id}\n${gstNo}`;
    this.Cmntaxsegment_gid = this.GetVendord[0].taxsegment_gid;
    this.POAddForm.get("vendor_address")?.setValue(vendorDetails);
    this.POAddForm.get("email_address")?.setValue(email_id);
    const address1 = this.GetVendord[0].address1;
    const address2 = this.GetVendord[0].address2 || '';  
    const city = this.GetVendord[0].city;
    const zip_code = this.GetVendord[0].postal_code; 
    let addressDetails = `${address1}\n${city}\n${zip_code}`;
    if (address2.trim() !== '') {
      addressDetails = `${address1}\n${address2}\n${city}\n${zip_code}`;
    }
    this.POAddForm.get("address1")?.setValue(addressDetails);
    // this.POAddForm.get("shipping_address")?.setValue(addressDetails);
    this.POAddForm.value.vendorregister_gid = this.GetVendord[0].vendorregister_gid;   
  });
}

onclearrequestor(){
  this.mdlUserName ='';
  this.Mdlrequsterdtl ='';

  }



GetOnChangeTerms() {
  let termsconditions_gid = this.POAddForm.value.template_name;
  let param = {
    termsconditions_gid: termsconditions_gid
  }
  var url = 'PmrTrnPurchaseQuotation/GetOnChangeTerms';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.POAddForm.get("template_content")?.setValue(result.terms_list[0].termsandconditions);
    this.POAddForm.value.termsconditions_gid = result.terms_list[0].termsconditions_gid
  });

  }

onInput(event: any) {
  const value = event.target.value;
  const parts = value.split('.');
  const integerPart = parts[0];
  let decimalPart = parts[1] || '';
  // Limit the decimal part to 2 digits
  decimalPart = decimalPart.slice(0, 2);
  // Update the input value
  event.target.value = `${integerPart}.${decimalPart}`;
  this.unitprice = event.target.value; 
}
OnChangeCurrency() {
  let currencyexchange_gid = this.POAddForm.get("currency_code")?.value;
  let param = {
    currencyexchange_gid: currencyexchange_gid
  }
  var url = 'PmrTrnPurchaseOrder/GetOnChangeCurrency';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.currency_list1 = this.responsedata.GetOnchangeCurrency;
    this.POAddForm.get("exchange_rate")?.setValue(this.currency_list1[0].exchange_rate);
    this.currency_code1 = this.currency_list1[0].currency_code
  });

}
onCurrencyCodeChange(event: Event) {
  const target = event.target as HTMLSelectElement;
  const selectedCurrencyCode = target.value;
  this.selectedCurrencyCode = selectedCurrencyCode;
  this.POAddForm.controls.currency_code.setValue(selectedCurrencyCode);
  this.POAddForm.get("currency_code")?.setValue(this.currency_list[0].currency_code);

}

onclearcurrency(){
  this.exchange=0;
}



onChange2(event: any) {
  this.file = event.target.files[0];
}




get qty_ordered() {
  return this.productform.get('qty_ordered')!;
}

POproductsummary(salesorder_gid : any) {
  let param = {
    salesorder_gid : salesorder_gid
  }
  const url = 'SmrTrnSO2PO/GetSO2POSummary';
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.GetPurchaseOrder1 = this.responsedata.GetPurchaseOrder1;
    this.file_name = result.file_name;
    this.file_path = result.file_path;

    
  });
}

Calculation(data: any, i: number) {
   
  const qtyOrdered = parseFloat(this.productform.get(`qty_ordered_${i}`)?.value || '0');
  const productPrice = parseFloat(this.productform.get(`product_price_${i}`)?.value || '0');
  const discountPercentage = parseFloat(this.productform.get(`discount_percentage_${i}`)?.value || '0');
 

  let totalAmount = qtyOrdered * productPrice;

  if (discountPercentage > 0) {
    totalAmount -= (totalAmount * discountPercentage) / 100;
  }
debugger
// -------------------------------------------------------------------------------------
let taxsum1 = 0;
if (this.GetPurchaseOrdertax[data.product_gid]) {
  const taxes = this.GetPurchaseOrdertax[data.product_gid];

  // Initialize the tax amounts
  let taxamt1 = 0;
  let taxamt2 = 0;

  taxes.forEach((tax: any, index: number) => {
    taxsum1 += parseFloat(tax.tstax_percentage);


    if (index === 0) {
      taxamt1 = (totalAmount * parseFloat(tax.tstax_percentage)) / 100;
    } else if (index === 1) {
      taxamt2 = (totalAmount * parseFloat(tax.tstax_percentage)) / 100;
    }
  });


  this.productform.get(`taxamount1`)?.setValue(taxamt1);
  this.productform.get(`taxamount2`)?.setValue(taxamt2);

  if (taxes.length <= 1) {
    this.productform.get(`taxamount2`)?.setValue('');
  }
}


// ------------------------------------------------------------------



  let taxsum = 0;
  if (this.GetPurchaseOrdertax[data.product_gid]) {
    this.GetPurchaseOrdertax[data.product_gid].forEach((tax: any) => {
      taxsum +=  parseFloat(tax.tstax_percentage);
      
    });
  }
  
  let totalAmount1 = taxsum;

  this.productform.get(`tstaxAmount`)?.setValue(totalAmount1.toFixed(2));
  let overallproducttotal = (totalAmount * taxsum)/100;
  overallproducttotal = (totalAmount + overallproducttotal)


  this.discountAmount = Number(productPrice) * Number(qtyOrdered)
  this.discount = (this.discountAmount * Number(discountPercentage)) / 100;
  this.amount = (this.discountAmount - this.discount)
  
  this.productform.get(`total_amount_${i}`)?.setValue(overallproducttotal.toFixed(2));
  this.productform.get(`discount_amount_${i}`)?.setValue(this.discount.toFixed(2));

 

  let sum = 0;
  for (let j = 0; j < this.GetPurchaseOrder1.length; j++) {
    const totalAmountForRow = parseFloat(this.productform.get(`total_amount_${j}`)?.value || '0');
    sum += totalAmountForRow;
  }
debugger
  this.POAddForm.get('totalamount')?.setValue(sum.toFixed(2));
  this.productform.get(`discount_amount_${i}`)?.setValue(this.discount.toFixed(2))

  this.finaltotal();
}

finaltotal() {
 
    const addoncharge = isNaN(this.addoncharge) ? 0 : +this.addoncharge;
  const freightcharges = isNaN(this.freightcharges) ? 0 : +this.freightcharges;
  const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
  const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
  const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
  const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
  const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
  const totalamount = isNaN(parseFloat(this.POAddForm.get('totalamount')?.value || '0')) ? 0 : +parseFloat(this.POAddForm.get('totalamount')?.value || '0');
  
  console.log("Addon Charge:", addoncharge);
  
  this.grandtotal = totalamount + tax_amount4 + addoncharge + freightcharges +
  packing_charges + insurance_charges + roundoff - additional_discount; 
  
  this.grandtotal = isNaN(this.grandtotal) ? 0 : +(this.grandtotal).toFixed(2);
  this.POAddForm.get('grandtotal')?.setValue(this.grandtotal.toFixed(2));
}

OnChangeTaxAmount4() {
        
  
  let tax_gid = this.POAddForm.get('tax_name4')?.value;   
  this.taxpercentage = this.getDimensionsByFilter(tax_gid);
  console.log(this.taxpercentage);
  let tax_percentage = this.taxpercentage[0].percentage;
  console.group(tax_percentage);

  this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
  this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
  this.total_amount = +this.total_amount.toFixed(2);
  this.grandtotal = ((this.total_amount) + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));

  }
  getDimensionsByFilter(id: any) {
    return this.tax_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
  }
OnClearOverallTax() {
  this.tax_amount4=0;
 
  this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
  this.total_amount = +this.total_amount.toFixed(2);
  
   this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
  this.grandtotal =+this.grandtotal.toFixed(2);
}



productSearch() {
  debugger;
  let vendorregister_gid = this.POAddForm.get("vendor_companyname")?.value;
  let salesorder_gid = this.salesordergid;
  let param = {
    salesorder_gid: salesorder_gid,
    vendor_gid: vendorregister_gid
  }
  const url = 'SmrTrnSO2PO/GetSO2POproducttax';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.GetPurchaseOrder1 = this.responsedata.GetPurchaseOrder1;

  
   });
}

  


onKeyPress(event: any) {
  const key = event.key;
  if (!/^[0-9.]$/.test(key)) {
    event.preventDefault();
  }
}







calculateProductTotal1(product: any) {
  debugger
      const { qty_ordered, product_price, product_gid, discount_percentage,tax_percentage,tax_percentage2 } = product
  
  
      const total = qty_ordered * product_price;
      const discounttotal = (total * discount_percentage) / 100;
  
      product.productdiscount_amountvalue = discounttotal.toFixed(2);
      const sam = total - discounttotal;
      const tax1= sam * tax_percentage/100;
      product.taxamount1 = tax1.toFixed(2);
      const tax2= sam * tax_percentage2/100;
      product.taxamount2 = tax2.toFixed(2);
      const totalproduct =sam+tax1+tax2;  
      product.producttotal_amount = totalproduct.toFixed(2);

      this.updateNetAmount();
        }
        updateNetAmount() {
          let netAmount = 0;
          this.GetPurchaseOrder1.forEach(product => {
            netAmount += parseFloat(product.producttotal_amount) || 0;
          });
          this.POAddForm.get("totalamount")?.setValue(netAmount.toFixed(2));
        }

        isAllSelected() {
          const numSelected = this.selection.selected.length;
          const numRows = this.GetPurchaseOrder1.length;
          return numSelected === numRows;
        }
        masterToggle() {
          this.isAllSelected() ?
            this.selection.clear() :
            this.GetPurchaseOrder1.forEach((row: IunAssign) => this.selection.select(row));
        }
        onsubmit() {
          debugger;     
     
          this.pick = this.selection.selected;
          this.CurObj.GetPurchaseOrder1 = this.pick;
                
          if (this.CurObj.GetPurchaseOrder1.length === 0) {
            this.ToastrService.warning("Select at least one Product");
            return;
          }
          if (this.file != null && this.file != undefined)
          {
            const formData = new FormData();
      formData.append('file', this.file,this.file.name);
      formData.append('Posummary_list', JSON.stringify(this.CurObj.GetPurchaseOrder1));
      formData.append('branch_name', this.POAddForm.value.branch_name);
      formData.append('branch_gid', this.POAddForm.value.branch_gid);
      formData.append('poref_no', this.POAddForm.value.poref_no);
      formData.append('po_date', this.POAddForm.value.po_date);
      formData.append('expected_date', this.POAddForm.value.expected_date);
      formData.append('vendor_gid', this.POAddForm.value.vendor_gid);
      formData.append('vendor_companyname', this.POAddForm.value.vendor_companyname);
      formData.append('contact_telephonenumber', this.POAddForm.value.contact_telephonenumber);
      formData.append('vendor_address', this.POAddForm.value.vendor_address);
      formData.append('shipping_address', this.POAddForm.value.shipping_address);
      formData.append('address1', this.POAddForm.value.address1);
      formData.append('employee_name', this.POAddForm.value.employee_name);
      formData.append('delivery_terms', this.POAddForm.value.delivery_terms);
      formData.append('payment_terms', this.POAddForm.value.payment_terms);
      formData.append('Requestor_details', this.POAddForm.value.Requestor_details);
      formData.append('despatch_mode', this.POAddForm.value.despatch_mode);
      formData.append('currency_gid', this.POAddForm.value.currency_gid);
      formData.append('currency_code', this.POAddForm.value.currency_code);
      formData.append('exchange_rate', this.POAddForm.value.exchange_rate);
      formData.append('po_covernote', this.POAddForm.value.po_covernote);
      formData.append('template_name', this.POAddForm.value.template_name);
      formData.append('template_content', this.POAddForm.value.template_content);
      formData.append('template_gid', this.POAddForm.value.template_gid);
      formData.append('totalamount', this.POAddForm.value.totalamount);
      formData.append('addoncharge', this.POAddForm.value.addoncharge);
      formData.append('additional_discount', this.POAddForm.value.additional_discount);
      formData.append('freightcharges', this.POAddForm.value.freightcharges);
      formData.append('roundoff', this.POAddForm.value.roundoff);
      formData.append('grandtotal', this.POAddForm.value.grandtotal);
      formData.append('purchaserequisition_gid', this.POAddForm.value.purchaserequisition_gid);
      formData.append('tax_gid', this.POAddForm.value.tax_gid);
      formData.append('tax_amount4', this.POAddForm.value.tax_amount4);
      formData.append('tax_name4', this.POAddForm.value.tax_name4);
      formData.append('contactperson_name', this.POAddForm.value.contactperson_name);
      formData.append('email_id', this.POAddForm.value.email_id);
      formData.append('taxsegment_gid', this.Cmntaxsegment_gid);

    
      this.NgxSpinnerService.show();
      const url = 'SmrTrnSO2PO/PostPurchaseOrderfileupload';
      this.service.post(url, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status === false) {
          this.ToastrService.warning(result.message);
        } else {
          this.ToastrService.success(result.message);
          this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
        }
      });

    }
          else{
          var params = {       
            Posummary_list: this.CurObj.GetPurchaseOrder1, 
            branch_name: this.POAddForm.value.branch_name,
            branch_gid: this.POAddForm.value.branch_gid,    
            poref_no: this.POAddForm.value.poref_no,
            po_date: this.POAddForm.value.po_date,
            expected_date: this.POAddForm.value.expected_date,
            vendor_gid: this.POAddForm.value.vendor_gid,
            vendor_companyname: this.POAddForm.value.vendor_companyname,
            contact_telephonenumber: this.POAddForm.value.contact_telephonenumber,
            vendor_address: this.POAddForm.value.vendor_address,
            shipping_address: this.POAddForm.value.shipping_address,
            address1: this.POAddForm.value.address1,      
            employee_name: this.POAddForm.value.employee_name,
            delivery_terms: this.POAddForm.value.delivery_terms,
            payment_terms: this.POAddForm.value.payment_terms,  
            Requestor_details: this.POAddForm.value.Requestor_details,    
            despatch_mode: this.POAddForm.value.despatch_mode,  
            currency_gid: this.POAddForm.value.currency_gid,
            currency_code: this.POAddForm.value.currency_code,
            exchange_rate: this.POAddForm.value.exchange_rate,
            po_covernote: this.POAddForm.value.po_covernote,
            template_name: this.POAddForm.value.template_name,
            template_content: this.POAddForm.value.template_content,
            template_gid: this.POAddForm.value.template_gid,
            totalamount: this.POAddForm.value.totalamount,
            addoncharge: this.POAddForm.value.addoncharge,    
            additional_discount: this.POAddForm.value.additional_discount,
            freightcharges: this.POAddForm.value.freightcharges,
            roundoff: this.POAddForm.value.roundoff,
            grandtotal: this.POAddForm.value.grandtotal,
            purchaserequisition_gid: this.POAddForm.value.purchaserequisition_gid,
            tax_gid: this.POAddForm.value.tax_gid,
            tax_amount4: this.POAddForm.value.tax_amount4,
            tax_name4: this.POAddForm.value.tax_name4,
            contactperson_name: this.POAddForm.value.contactperson_name,
            email_id: this.POAddForm.value.email_id,
            taxsegment_gid: this.Cmntaxsegment_gid,
 
          };
          this.NgxSpinnerService.show();    
           const url = 'SmrTrnSO2PO/PostOverallSubmit';    
           this.service.post(url, params).subscribe((result: any) => {
            this.NgxSpinnerService.hide();        
            if (result.status === false) {
              this.ToastrService.warning(result.message);
            } else {
              this.ToastrService.success(result.message);
              this.route.navigate(['/smr/SmrTrnSalesorderSummary']);
            }
          });     
        

          this.selection.clear();
        }
      }      
}