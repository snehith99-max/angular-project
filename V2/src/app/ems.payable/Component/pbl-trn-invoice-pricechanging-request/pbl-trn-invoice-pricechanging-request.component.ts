import { Component, numberAttribute } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormRecord, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { DefaultGlobalConfig, ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FrozenColumn } from 'primeng/table';
import { TabHeadingDirective } from 'ngx-bootstrap/tabs';
import { ReplaySubject } from 'rxjs';
import { __values } from 'tslib';
@Component({
  selector: 'app-pbl-trn-invoice-pricechanging-request',
  templateUrl: './pbl-trn-invoice-pricechanging-request.component.html',
  styleUrls: ['./pbl-trn-invoice-pricechanging-request.component.scss']
})
export class PblTrnInvoicePricechangingRequestComponent {

    InvoiceForm: FormGroup | any;
    showInput: boolean = false;
    inputValue: string = ''
    GrnDetails: any;
    vendor_gid_key: any;
    purchaseorder_gid_key: any;
    producttotalamount:any;
    grn_gid_key: any;
    response_data: any;
    exchange: any;
    addoncharge:number = 0;
    InvoicePurchaseOrderGrnDetails_list: any[] = [];
    GetInvoiceGrnDetails_list: any[] = [];
    InvoiceVendorDetails: any[] = [];
    InvoiceGRNDetails_list: any[] = [];
    InvoiceUserDetails: any[] = [];
    InvoicePurcahseType: any[] = [];
    GetNetamount_list: any[] = [];
    mdlpurchasetype: any;
    responsedata: any;
    net_amount: number = 0;
    qty_delivered: number = 0;
    total_amount: any;
    addon_amount: number = 0;
    totalamount: number = 0;
    tax_amount4: number = 0;
    frieghtcharges: number = 0;
    additional_discount: number = 0;
    buybackorscrap: number = 0;
    freightcharges: number = 0;
    discount_amount: number = 0;
    packing_charges: number = 0;
    insurance_charges: number = 0;
    roundoff: number = 0;
    grandtotal: number = 0;
    exchange_rate: number =0;
    Product_amount: number =0;
    net_price: number=0;
    taxpercentage: any;
    tax4_list:any;
    tax_list:any;
    totalproduct_amount: number =0;
    invoicediscountamount: number = 0;
    insurancecharges: number = 0;
    forwardingCharges: number = 0;
    mdlTaxName4:any;
    allchargeslist: any[] = [];
    GetTaxSegmentList: any;
    mdlTaxSegment: any;
    taxsegment_gid: any;
    prod_name: any;
    taxseg_tax: any;
    tax_amount: any;
    taxseg_tax_amount: any;
    taxGids: any;
    taxseg_taxgid: any;
    totTaxAmountPerQty: any;
    po_tax_amount: any;
    decimalPipe: any;
  
    constructor(private formBuilder: FormBuilder,
      private ToastrService: ToastrService,
      private router: ActivatedRoute,
      private route: Router,
      public service: SocketService,
      public NgxSpinnerService: NgxSpinnerService) {
       
      this.InvoiceForm = new FormGroup({
        inv_ref_no: new FormControl('',),
        invoice_date:  new FormControl(this.getCurrentDate(), Validators.required),
        Vendor_ref_no: new FormControl(''),
        purchasetype_name: new FormControl(''),
        branch_name: new FormControl(''),
        purchaseorder_date: new FormControl(''),
        payment_days: new FormControl(''),
        payment_date: new FormControl(''),
        employee_emailid: new FormControl(''),
        employee_mobileno: new FormControl(''),
        employee_name: new FormControl(''),
        vendor_companyname: new FormControl(''),
        contactperson_name: new FormControl(''),
        contact_telephonenumber: new FormControl(''),
        vendor_address: new FormControl(''),
        fax: new FormControl(''),
        exchange_rate: new FormControl(''),
        currency_code: new FormControl(''),
        product_name: new FormControl(''),
        product_code: new FormControl(''),
        productuom_name: new FormControl(''),
        qty_delivered: new FormControl(''),
        product_price: new FormControl(''),
        discount_amount: new FormControl(''),
        additional_discount: new FormControl(''),
        discount_percentage: new FormControl(''),
        tax_name: new FormControl(''),
        tax_amount: new FormControl(''),
        purchaseorder_gid: new FormControl(''),
        addoncharge: new FormControl(''),
      invoice_remarks : new FormControl(''),
        product_remarks: new FormControl(''),
        tax_name4: new FormControl(''),
        totalamount: new FormControl(''),
        grandtotal: new FormControl(''),
        freightcharges: new FormControl(''),
        packing_charges: new FormControl(''),
        insurance_charges: new FormControl(''),
        roundoff: new FormControl(''),
        tax_amount4: new FormControl(''),    
        priority_n: new FormControl('N', Validators.required),
        vendor_gid: new FormControl(''),
      });
    }
    getCurrentDate(): string {
      const today = new Date();
      const dd = String(today.getDate()).padStart(2, '0');
      const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
      const yyyy = today.getFullYear();
      return yyyy + '-' + mm + '-' + dd;
  }
  
  
  
    ngOnInit(): void {
      debugger
  
      
      const vendor_gid1 = this.router.snapshot.paramMap.get('vendor_gid2');
      const purchaseorder_gid1 = this.router.snapshot.paramMap.get('purchaseorder_gid1');
      const grn_gid = this.router.snapshot.paramMap.get('grn_gid3');
  
      const secretKey = 'storyboarderp';
  
      this.vendor_gid_key = vendor_gid1;
      this.purchaseorder_gid_key = purchaseorder_gid1;
      this.grn_gid_key = grn_gid;
  
      const vendor_gid_1 = AES.decrypt(this.vendor_gid_key, secretKey).toString(enc.Utf8);
      const purchaseorder_gid_1 = AES.decrypt(this.purchaseorder_gid_key, secretKey).toString(enc.Utf8);;
      const grn_gid1 = AES.decrypt(this.grn_gid_key, secretKey).toString(enc.Utf8);
  
      this.GetGRNDetailsInvoiceSummary(purchaseorder_gid_1);
      this.GetVendorDetailsSummary(vendor_gid_1);
      this.GetPurchaseDetailsSummary(grn_gid1);
      this.GetPurchaseDetailsSummary1(grn_gid1);
      this.GetVendorUserDetails();
      this.GetPurchaseTypeDetails();
      this.finaltotal();
    }
    
    showTextBox(event: Event) {
      const target = event.target as HTMLInputElement;
      this.showInput = target.value === 'Y';
    }
   
  
    // get invoice_date() {
    //   return this.InvoiceForm.get('invoice_date')!;
    // }
    GetGRNDetailsInvoiceSummary(purchaseorder_gid_1: any) {
      debugger
      var api = 'PblInvoiceGrnDetails/GetGrnAmountDetails'
      this.NgxSpinnerService.show()
      let param = {
        purchaseorder_gid: purchaseorder_gid_1
      }
      this.service.getparams(api, param).subscribe((result: any) => {
        this.response_data = result;
        this.InvoicePurchaseOrderGrnDetails_list = result.GetInvoicePurchaseOrderGrnDetails_list;
        this.InvoiceForm.get("currency_code")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].currency_code);
        this.InvoiceForm.get("fax")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].fax);
        this.InvoiceForm.get("branch_name")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].branch_name);
        this.InvoiceForm.get("purchaseorder_gid")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].purchaseorder_gid);
        this.InvoiceForm.get("addoncharge")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].addon_amount);
        this.InvoiceForm.get("additional_discount")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].discount_amount);
        this.InvoiceForm.get("freightcharges")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].freightcharges);
        this.InvoiceForm.get("packing_charges")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].packing_charges);
        this.InvoiceForm.get("buybackorscrap")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].buybackorscrap);
        this.InvoiceForm.get("roundoff")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].roundoff);
        this.InvoiceForm.get("insurance_charges")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].insurance_charges);
        this.InvoiceForm.get("exchange_rate")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].exchange_rate);
        this.InvoiceForm.get("purchaseorder_date")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].purchaseorder_date);
        this.InvoiceForm.get("payment_days")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].payment_days);
        this.InvoiceForm.get("tax_name4")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].tax_name);
        this.InvoiceForm.get("tax_amount4")?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].tax_amount);
        this.InvoiceForm.get('tax_name4')?.setValue(this.InvoicePurchaseOrderGrnDetails_list[0].tax_gid);
        this.mdlTaxName4 = this.InvoicePurchaseOrderGrnDetails_list[0].tax_gid;
  const purchaseorder_date = this.InvoiceForm.get('purchaseorder_date')?.value
  const payment_days = this.InvoiceForm.get('payment_days')?.value
  
        // this.calculate();
        this.cal_datetime(purchaseorder_date,payment_days);
      });
      this.NgxSpinnerService.hide()
    }
  
    GetVendorDetailsSummary(vendor_gid_1: any) {
      debugger
      var api2 = 'PblInvoiceGrnDetails/GetVendorDetails'
      this.NgxSpinnerService.show()
      let param = {
        vendor_gid: vendor_gid_1
      }
      this.service.getparams(api2, param).subscribe((result: any) => {
        this.response_data = result;
        this.InvoiceVendorDetails = result.GetInvoiceVendorDetails_list;
        this.InvoiceForm.get("vendor_address")?.setValue(this.InvoiceVendorDetails[0].vendor_address);
  
        this.InvoiceForm.get("employee_mobileno")?.setValue(this.InvoiceVendorDetails[0].contact_telephonenumber);
        this.InvoiceForm.get("employee_name")?.setValue(this.InvoiceVendorDetails[0].employee_name);
        this.InvoiceForm.get("vendor_companyname")?.setValue(this.InvoiceVendorDetails[0].vendor_companyname);
        this.InvoiceForm.get("contactperson_name")?.setValue(this.InvoiceVendorDetails[0].contactperson_name);
        this.InvoiceForm.get("contact_telephonenumber")?.setValue(this.InvoiceVendorDetails[0].contact_telephonenumber);
  
  
        this.InvoiceForm.get("vendor_gid")?.setValue(this.InvoiceVendorDetails[0].vendor_gid);
  
      });
      this.NgxSpinnerService.hide()
    }
    get tax_name4() {
      return this.InvoiceForm.get('tax_name4')!;
    }
  
    GetPurchaseDetailsSummary(grn_gid1: any) {
      this.NgxSpinnerService.show();
      var api3 = 'PblInvoiceGrnDetails/GetPurchaseOrderDetails'
      
      let param = {
        grn_gid: grn_gid1
      }
      this.service.getparams(api3, param).subscribe((result: any) => {
        this.response_data = result;
        this.GetTaxSegmentList = result.GetInvTaxSegmentList;
        this.InvoiceGRNDetails_list = result.GetInvoiceGrnDetails_list;
      console.log(result.GetInvoiceGrnDetails_list)
  this.InvoiceForm.get("qty_delivered")?.setValue(this.InvoiceGRNDetails_list[0].qty_delivered);
  this.InvoiceForm.get("product_price")?.setValue(this.InvoiceGRNDetails_list[0].product_price);
  this.InvoiceForm.get("Product_amount")?.setValue(this.InvoiceGRNDetails_list[0].total_amount);
  this.InvoiceForm.get("tax_amount")?.setValue(this.InvoiceGRNDetails_list[0].tax_amount);
  this.InvoiceForm.get("discount_amount")?.setValue(this.InvoiceGRNDetails_list[0].discount_amount);
  this.InvoiceForm.get("discount_percentage")?.setValue(this.InvoiceGRNDetails_list[0].discount_percentage);
  this.InvoiceForm.get("taxsegment_gid")?.setValue(this.InvoiceGRNDetails_list[0].taxsegment_gid);
  this.InvoiceForm.get("taxsegmenttax_gid")?.setValue(this.InvoiceGRNDetails_list[0].taxsegmenttax_gid); 
  this.po_tax_amount =result.GetInvTaxSegmentList[0].tax_amount  ;
  this.GetTaxSegmentList = result.GetInvTaxSegmentList;
  
  debugger
  for (let product of this.InvoiceGRNDetails_list) {
    // Filter tax segments based on the current product's product_gid
    const taxSegmentsForProduct = this.GetTaxSegmentList.filter((taxSegment: { product_gid: any; }) => taxSegment.product_gid === product.product_gid);
   
      // Map tax segments to include tax_name, tax_percentage, tax_gid, and taxsegment_gid
      const mappedTaxSegments = taxSegmentsForProduct.map((taxSegment: { tax_name: any; tax_percentage: any; tax_gid: any; taxsegment_gid: any; }) => ({
        tax_name: taxSegment.tax_name,
        tax_percentage: taxSegment.tax_percentage,
        tax_gid: taxSegment.tax_gid,
        taxsegment_gid: taxSegment.taxsegment_gid
    }));
  
    // Get unique tax_gids and taxsegment_gids for the product
    const uniqueTaxGids = [...new Set(mappedTaxSegments.map((segment: { tax_gid: any; }) => segment.tax_gid))];
    const uniqueTaxSegmentGids = [...new Set(mappedTaxSegments.map((segment: { taxsegment_gid: any; }) => segment.taxsegment_gid))];
  
    // Assign the mapped tax segments and unique tax_gids and taxsegment_gids to the current product
    product.taxSegments = mappedTaxSegments;
    product.tax_gids = uniqueTaxGids;
    product.taxsegment_gids = uniqueTaxSegmentGids;
    product.tax_gids_string = uniqueTaxGids.join(', ');
  
     // Initialize total tax amount for the product
     let totalTaxAmountPerQty = 0;
  debugger
     // Iterate over tax segments associated with the current product
     for (let taxSegment of product.taxSegments) {
         // Calculate tax amount for the current tax segment
         const taxAmount = parseFloat(product.unitprice) * parseFloat(taxSegment.tax_percentage) / 100;
          
         
         
         // Assign tax amount to the tax segment object for display (optional)
         taxSegment.taxAmount = taxAmount.toFixed(2);
         // Add tax amount to the total tax amount for the product
         totalTaxAmountPerQty += taxAmount;
     }
  
  this.totTaxAmountPerQty = totalTaxAmountPerQty.toFixed(2);
   this.NgxSpinnerService.hide();
  }
  
  });
    
   
      debugger
      var url = 'PblInvoiceGrnDetails/GetTax4Dtl'
      this.service.get(url).subscribe((result:any)=>{
        this.tax4_list = result.GetTaxfourDtl;
       });
      this.NgxSpinnerService.hide();
  
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
    removeDuplicateTaxSegments(taxSegments: any[]): any[] {
      const uniqueTaxSegmentsMap = new Map<string, any>();
      taxSegments.forEach(taxSegment => {
          uniqueTaxSegmentsMap.set(taxSegment.tax_name, taxSegment);
      });
      // Convert the Map back to an array
      return Array.from(uniqueTaxSegmentsMap.values());
    }
    
    GetPurchaseDetailsSummary1(grn_gid1: any) {
      debugger
      var api12 = 'PblInvoiceGrnDetails/GetInvoiceNetAmount'
  
      let param = {
        grn_gid: grn_gid1
      }
      this.service.getparams(api12, param).subscribe((result: any) => {
        this.response_data = result;
        this.GetNetamount_list = result.GetNetamount_list;
        debugger
  this.InvoiceForm.get("totalamount")?.setValue(this.GetNetamount_list[0].netamount);
  
      });}
    GetVendorUserDetails() {
      this.NgxSpinnerService.show()
      var api4 = 'PblInvoiceGrnDetails/GetVendorUserDetails'
  
      this.service.get(api4).subscribe((result: any) => {
        this.InvoiceUserDetails = result.GetVendorUserDetails_list;
      });
      this.NgxSpinnerService.hide()
    }
    GetPurchaseTypeDetails() {
      this.NgxSpinnerService.show()
      var api5 = 'PblInvoiceGrnDetails/GetPurchaseTyepDetails'
  
      this.service.get(api5).subscribe((result: any) => {
        this.InvoicePurcahseType = result.GetPurchaseType_list;
      });
      this.NgxSpinnerService.hide()
    }
    calculate() {
      debugger
  
  
      this.InvoiceGRNDetails_list.forEach((item) => {
        item.qty_delivered = Number(item.qty_delivered);
        item.product_price = Number(item.product_price);
        item.discount_amount = item.discount_amount ? Number(item.discount_amount) : 0;
        item.tax_amount = item.tax_amount ? Number(item.tax_amount) : 0;
        if (item.discount_percentage) {
          const discountPercentage = Number(item.discount_percentage) / 100;
          item.discount_amount = item.qty_delivered * item.product_price * discountPercentage;
          item.discount_amount=  item.discount_amount.toFixed(2);
        }
        let total_amount = (item.qty_delivered * item.product_price) - item.discount_amount + item.tax_amount;
        // this.total_amount = item.total_amount
  
        item.total_amount = total_amount.toFixed(2) ;   
        
        this.totalproduct_amount = item.total_amount;
      });
    }
    netamount_cal() {
      debugger
      this.net_amount = this.InvoiceGRNDetails_list.reduce(
        (total, item) => {
          return total + item.total_amount;
        }, 0);
    }
    close() {
      this.route.navigate(['/payable/PmrTrnInvoiceAddselect'])
    }
    changeprice(){
      
    }
    onSubmit() {
  
      this.NgxSpinnerService.show();
      
      if(this.InvoiceForm.value.inv_ref_no==""||this.InvoiceForm.value.inv_ref_no == null || this.InvoiceForm.value.inv_ref_no == undefined &&
      this.InvoiceForm.value.Vendor_ref_no==""||this.InvoiceForm.value.Vendor_ref_no == null || this.InvoiceForm.value.Vendor_ref_no == undefined &&
         this.InvoiceForm.value.purchasetype_name==""||this.InvoiceForm.value.purchasetype_name == null || this.InvoiceForm.value.purchasetype_name == undefined
         )
      {
       window.scrollTo({
         top: 0, 
       });
       this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
       this.NgxSpinnerService.hide();
       return
      }
     
      var params = {
        
        inv_ref_no: this.InvoiceForm.value.inv_ref_no,
        invoice_date: this.InvoiceForm.value.invoice_date,
        invoice_gid: this.InvoiceForm.value.invoice_gid,
        branch_name: this.InvoicePurchaseOrderGrnDetails_list[0].branch_name,
        Vendor_ref_no :  this.InvoiceForm.value.Vendor_ref_no,
        purchaseorder_gid: this.InvoicePurchaseOrderGrnDetails_list[0].purchaseorder_gid,
        purchaseorder_date: this.InvoicePurchaseOrderGrnDetails_list[0].purchaseorder_date,
        payment_days: this.InvoicePurchaseOrderGrnDetails_list[0].payment_days,
        payment_date: this.InvoiceForm.value.payment_date,
              
        employee_emailid: this.InvoiceUserDetails[0].employee_emailid,
        employee_mobileno: this.InvoiceUserDetails[0].employee_mobileno,
        employee_name: this.InvoiceUserDetails[0].employee_name,
        vendor_companyname: this.InvoiceVendorDetails[0].vendor_companyname,
        // vendor_gid: this.InvoiceVendorDetails[0].vendor_gid,
        contactperson_name: this.InvoiceVendorDetails[0].contactperson_name,
        contact_telephonenumber: this.InvoiceVendorDetails[0].contact_telephonenumber,
        exchange_rate: this.InvoicePurchaseOrderGrnDetails_list[0].exchange_rate,
        // currency_code: this.InvoicePurchaseOrderGrnDetails_list[0].currency_code,
        // product_name: this.InvoiceGRNDetails_list[0].product_name,
        // product_code: this.InvoiceGRNDetails_list[0].product_code,
        // productuom_name: this.InvoiceGRNDetails_list[0].productuom_name,
        // qty_delivered: this.InvoiceGRNDetails_list[0].qty_delivered,
        // product_price: this.InvoiceGRNDetails_list[0].product_price,
        // discount_amount1: this.InvoiceGRNDetails_list[0].discount_amount,
        // discount_percentage: this.InvoiceGRNDetails_list[0].discount_percentage,
        // tax_name: this.InvoiceGRNDetails_list[0].tax_name,
        // tax_amount: this.InvoiceGRNDetails_list[0].tax_amount,
        grn_gid : this.InvoiceGRNDetails_list[0].grn_gid,
        taxsegment_gid : this.InvoiceGRNDetails_list[0].taxsegment_gid,
        taxsegmenttax_gid : this.InvoiceGRNDetails_list[0].taxsegmenttax_gid,
        invoice_remarks: this.InvoiceForm.value.invoice_remarks,
        priority_n: this.InvoiceForm.value.priority_n,
        purchasetype_name: this.InvoiceForm.value.purchasetype_name,
        product_remarks: this.InvoiceForm.value.product_remarks,
        totalamount: this.InvoiceForm.value.totalamount,
        tax_name4: this.InvoiceForm.value.tax_name4,
        tax_amount4: this.InvoiceForm.value.tax_amount4,
        addoncharge: this.InvoiceForm.value.addoncharge,
        freightcharges:this.InvoiceForm.value.freightcharges,
        packing_charges: this.InvoiceForm.value.packing_charges,
        insurance_charges: this.InvoiceForm.value.insurance_charges,
        additional_discount: this.InvoiceForm.value.additional_discount,
        roundoff: this.InvoiceForm.value.roundoff,
        grandtotal: this.InvoiceForm.value.grandtotal,
        
      }
      this.NgxSpinnerService.show();
      var api6 = 'PblInvoiceGrnDetails/PostOverAllSubmit'
      this.service.postparams(api6, params).pipe().subscribe((result: any) => {
          this.response_data = result;
          if (result.status == false) {
            window.scrollTo({
              top: 0, 
            });
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            window.scrollTo({
              top: 0, 
            });
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
            this.route.navigate(['/payable/PmrTrnInvoice']);
          }
        });
        this.NgxSpinnerService.hide();
    }
   
    OnChangeTaxAmount4() {
      debugger
      let tax_gid = this.InvoiceForm.get('tax_name4')?.value;   
      this.taxpercentage = this.getDimensionsByFilter(tax_gid);
      let tax_percentage = this.taxpercentage[0].percentage;
      this.tax_amount4 = +(tax_percentage * this.producttotalamount / 100).toFixed(2);
      this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
      this.total_amount = +this.total_amount.toFixed(2);
      this.grandtotal = ((this.total_amount) + (+this.tax_amount4) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
     
      }
    OnClearOverallTax() {
      this.tax_amount4=0;
     
      this.total_amount = +((+this.producttotalamount) + (+this.tax_amount4));  
      this.total_amount = +this.total_amount.toFixed(2);
      
       this.grandtotal = ((this.total_amount) + (+this.addoncharge) + (+this.freightcharges) +  (+this.packing_charges) + (+this.insurance_charges) + (+this.roundoff) + (+this.totalamount)  - (+this.additional_discount));
      this.grandtotal =+this.grandtotal.toFixed(2);
    }
  
    // finaltotal() {
    //   debugger
    //   const exchange_rate = isNaN(this.exchange_rate) ? 0: this.exchange_rate;
    //   const addon_amount = isNaN(this.addon_amount) ? 0 : +this.addon_amount;
    //   const buybackorscrap = isNaN(this.buybackorscrap) ? 0 : +this.buybackorscrap;
    //   const freightcharges = isNaN(this.freightcharges) ? 0 : +this.freightcharges;
    //   const discount_amount = isNaN(this.discount_amount) ? 0 : +this.discount_amount;
    //   const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
    //   const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
    //   const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
    //   const netamount = isNaN(this.net_amount) ? 0 : +this.net_amount;
  
    //   this.grandtotal = ((addon_amount) + (buybackorscrap) + (freightcharges) + (insurance_charges) + (roundoff) - (discount_amount) + (packing_charges) + (netamount));
    //   this.grandtotal = +(this.grandtotal).toFixed(2)
  
    // }
     getDimensionsByFilter(id: any) {
      return this.tax4_list.filter((x: { tax_gid: any; }) => x.tax_gid === id);
    }
  
    finaltotal() {
      const addoncharge = isNaN(this.addoncharge) ? 0 : +this.addoncharge;
      const producttotalamount = isNaN(this.producttotalamount) ? 0 : +this.producttotalamount;
      this.producttotalamount = producttotalamount.toFixed(2);
      const frieghtcharges = isNaN(this.frieghtcharges) ? 0 : +this.frieghtcharges;
      const packing_charges = isNaN(this.packing_charges) ? 0 : +this.packing_charges;
      const insurance_charges = isNaN(this.insurance_charges) ? 0 : +this.insurance_charges;
      const roundoff = isNaN(this.roundoff) ? 0 : +this.roundoff;
      const additional_discount = isNaN(this.additional_discount) ? 0 : +this.additional_discount;
      const tax_amount4 = isNaN(this.tax_amount4) ? 0 : +this.tax_amount4;
      this.grandtotal = ((producttotalamount) + (tax_amount4) +(addoncharge) + (frieghtcharges) + (packing_charges) + (insurance_charges) + (roundoff) - (additional_discount));
      this.grandtotal = +(this.grandtotal).toFixed(2);
      //  const grandtotal1 = this.decimalPipe.transform(this.grandtotal, '1.2-2');
    }
  
    
    cal_datetime(purchaseorder_date: any ,payment_days: any): void 
    {
  const parts= purchaseorder_date.split('-')
  const fullYear = '20' + parts[2];
  const  purchaseorderdate = new Date(+fullYear, +parts[1] -1, +parts[0]);
  purchaseorderdate.setDate(purchaseorderdate.getDate() + parseInt(payment_days));
  
  const dueDate = [
    ('0' + purchaseorderdate.getDate()).slice(-2), // Add leading zero, if needed
    ('0' + (purchaseorderdate.getMonth() + 1)).slice(-2), // Month is 0-indexed, add leading zero, if needed
    purchaseorderdate.getFullYear().toString().substr(-2),
  ].join('-');
  this.InvoiceForm.get('payment_date')?.setValue(dueDate);
    }
  
}
