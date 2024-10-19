import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-crm-trn-trasieinvoice',
  templateUrl: './crm-trn-trasieinvoice.component.html',
  styleUrls: ['./crm-trn-trasieinvoice.component.scss']
})
export class CrmTrnTrasieinvoiceComponent {
  invoiceaccountinglist: any;
  invoiceaccountingform: FormGroup | any;
  invoiceaccountingduedateControl: any;
  salesorder_gid: any;
  responsedata :any;
  invoiceaccountingdata: any;


  
  ngOnInit() {
   

    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;
  
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
  
    let param = {
      directorder_gid: deencryptedParam
    }
    var api = 'Einvoice/Getproductsummarydata';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.invoiceaccountinglist = this.responsedata.salesinvoiceproduct_list;
    });
    this.GetSalesinvoicedata();
  }

  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService,private ToastrService: ToastrService) {
    
    this.invoiceaccountingform = new FormGroup({
      invoiceaccounting_salesorder_gid: new FormControl(''),
      invoiceaccounting_refno: new FormControl('', Validators.required),
      invoice_date: new FormControl('', Validators.required),
      invoiceaccounting_payterm: new FormControl(''),
      invoiceaccounting_duedate: new FormControl(''),
      invoiceaccounting_orderrefno: new FormControl(''),
      
      invoiceaccounting_orderdate: new FormControl(''),
      invoiceaccounting_customername: new FormControl(''),
      invoiceaccounting_email: new FormControl(''),
      invoiceaccounting_customeraddress: new FormControl(''),
      invoiceaccounting_contactnumber: new FormControl(''),
      invoiceaccounting_contactperson: new FormControl(''),
      invoiceaccounting_branch: new FormControl(''),
      invoiceaccounting_currency: new FormControl(''),
      invoiceaccounting_exchangerate: new FormControl(''),
      invoiceaccounting_salestype: new FormControl(''),

      invoiceaccounting_remarks: new FormControl(''),
      customerproduct_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      description: new FormControl(''),
      qty_invoice: new FormControl(''),
      Vendor_price: new FormControl(''),
      margin_percentage: new FormControl(''),

      orderdetails: new FormControl(''),
      product_price: new FormControl(''),
      final_amount: new FormControl(''),
      invoiceaccounting_net_amount: new FormControl(''),
      invoiceaccounting_overall_tax: new FormControl(''),
      invoiceaccouting_orderdiscountamount: new FormControl(''),
      invoiceaccounting_addon_amount: new FormControl(''),
      invoiceaccounting_ordertotal: new FormControl(''),
      invoiceaccounting_discountamount: new FormControl(''),
      invoiceaccounting_addonamount: new FormControl(''),
      invoiceaccounting_freightcharges: new FormControl(''),
      invoiceaccounting_buybackcharges: new FormControl(''),
      invoiceaccounting_packingcharges: new FormControl(''),
      invoiceaccounting_insurancecharges: new FormControl(''),
      invoiceaccounting_outstandingamount: new FormControl(''),
      invoiceaccounting_roundoff: new FormControl(''),

      invoiceaccounting_grandtotal: new FormControl(''),
      invoiceaccounting_termsandconditions: new FormControl(''),
      invoiceaccounting_taxname: new FormControl(''),
      invoiceaccounting_taxamount: new FormControl(''),
      invoiceaccounting_taxname2: new FormControl(''),
      invoiceaccounting_taxamount2: new FormControl(''),
      invoiceaccounting_taxname3: new FormControl(''),
      invoiceaccounting_taxamount3: new FormControl(''),
    
    })
  }

  get invoiceaccountingformrefnoControl() {
    return this.invoiceaccountingform.get('invoiceaccounting_refno');
  }
  get invoiceaccountingdateControl() {
    return this.invoiceaccountingform.get('invoice_date');
  }

  GetSalesinvoicedata() {
    const salesorder_gid = this.route.snapshot.paramMap.get('directorder_gid');
    this.salesorder_gid = salesorder_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);

    let param = {
      directorder_gid: deencryptedParam
    }

    var api = 'Einvoice/GetSalesinvoicedata';

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata= result;      
      this.invoiceaccountingdata = result;

      this.invoiceaccountingform.get("invoiceaccounting_salesorder_gid")?.setValue(this.invoiceaccountingdata.serviceorder_gid);
      this.invoiceaccountingform.get("invoice_date")?.setValue(this.invoiceaccountingdata.salesorder_date);
      this.invoiceaccountingform.get("invoiceaccounting_refno")?.setValue(this.invoiceaccountingdata.invoice_no);

      this.invoiceaccountingform.get("invoiceaccounting_payterm")?.setValue(this.invoiceaccountingdata.invoiceaccounting_payterm);
      this.invoiceaccountingform.get("invoiceaccounting_duedate")?.setValue(this.invoiceaccountingdata.invoiceaccounting_duedate);
      this.invoiceaccountingform.get("invoiceaccounting_orderrefno")?.setValue(this.invoiceaccountingdata.so_reference);
      this.invoiceaccountingform.get("invoiceaccounting_orderdate")?.setValue(this.invoiceaccountingdata.serviceorder_date);
      this.invoiceaccountingform.get("invoiceaccounting_customername")?.setValue(this.invoiceaccountingdata.customer_name);
      this.invoiceaccountingform.get("invoiceaccounting_email")?.setValue(this.invoiceaccountingdata.email);
      this.invoiceaccountingform.get("invoiceaccounting_customeraddress")?.setValue(this.invoiceaccountingdata.customer_address);
      this.invoiceaccountingform.get("invoiceaccounting_contactnumber")?.setValue(this.invoiceaccountingdata.customer_mobile);
      this.invoiceaccountingform.get("invoiceaccounting_contactperson")?.setValue(this.invoiceaccountingdata.customercontact_name);
      this.invoiceaccountingform.get("invoiceaccounting_branch")?.setValue(this.invoiceaccountingdata.customer_code);
      this.invoiceaccountingform.get("invoiceaccounting_currency")?.setValue(this.invoiceaccountingdata.currency_code);
      this.invoiceaccountingform.get("invoiceaccounting_exchangerate")?.setValue(this.invoiceaccountingdata.exchange_rate);
      this.invoiceaccountingform.get("invoiceaccounting_salestype")?.setValue(this.invoiceaccountingdata.invoiceaccounting_salestype);
      this.invoiceaccountingform.get("invoiceaccounting_remarks")?.setValue(this.invoiceaccountingdata.remarks);      
      this.invoiceaccountingform.get("customerproduct_code")?.setValue(this.invoiceaccountingdata.customerproduct_code);
      this.invoiceaccountingform.get("product_name")?.setValue(this.invoiceaccountingdata.product_name);
      this.invoiceaccountingform.get("description")?.setValue(this.invoiceaccountingdata.description);
      this.invoiceaccountingform.get("qty_quoted")?.setValue(this.invoiceaccountingdata.qty_quoted);
      this.invoiceaccountingform.get("vendoramount")?.setValue(this.invoiceaccountingdata.vendoramount);
      this.invoiceaccountingform.get("discount_amount")?.setValue(this.invoiceaccountingdata.discount_amount);
      this.invoiceaccountingform.get("product_price")?.setValue(this.invoiceaccountingdata.qty_quoted);
      this.invoiceaccountingform.get("orderdetails")?.setValue(this.invoiceaccountingdata.orderdetails);  
      this.invoiceaccountingform.get("net_amount")?.setValue(this.invoiceaccountingdata.invoice_amount);  
      this.invoiceaccountingform.get("invoiceaccounting_taxname")?.setValue(this.invoiceaccountingdata.tax_name1);      
      this.invoiceaccountingform.get("invoiceaccounting_taxamount")?.setValue(this.invoiceaccountingdata.tax_amount1);      
      this.invoiceaccountingform.get("invoiceaccounting_taxname2")?.setValue(this.invoiceaccountingdata.tax_name2);
      this.invoiceaccountingform.get("invoiceaccounting_taxamount2")?.setValue(this.invoiceaccountingdata.tax_amount2);  
      this.invoiceaccountingform.get("invoiceaccounting_taxname3")?.setValue(this.invoiceaccountingdata.tax_name3);
      this.invoiceaccountingform.get("invoiceaccounting_taxamount3")?.setValue(this.invoiceaccountingdata.tax_amount3);    
      
    
      this.invoiceaccountingform.get("final_amount")?.setValue(this.invoiceaccountingdata.final_amount);      
      this.invoiceaccountingform.get("invoiceaccounting_overall_tax")?.setValue(this.invoiceaccountingdata.tax_amount);      
      this.invoiceaccountingform.get("invoiceaccouting_orderdiscountamount")?.setValue(this.invoiceaccountingdata.tax_name2);
      this.invoiceaccountingform.get("invoiceaccounting_addon_amount")?.setValue(this.invoiceaccountingdata.tax_amount2);
      this.invoiceaccountingform.get("invoiceaccounting_ordertotal")?.setValue(this.invoiceaccountingdata.total_amount);
      this.invoiceaccountingform.get("invoiceaccounting_discountamount")?.setValue(this.invoiceaccountingdata.gst_amount);
      this.invoiceaccountingform.get("invoiceaccounting_addonamount")?.setValue(this.invoiceaccountingdata.addonamount);
      this.invoiceaccountingform.get("invoiceaccounting_freightcharges")?.setValue(this.invoiceaccountingdata.freight_charges);
      this.invoiceaccountingform.get("invoiceaccounting_buybackcharges")?.setValue(this.invoiceaccountingdata.buyback_charges);
      this.invoiceaccountingform.get("invoiceaccounting_packingcharges")?.setValue(this.invoiceaccountingdata.packing_charges);
      this.invoiceaccountingform.get("invoiceaccounting_insurancecharges")?.setValue(this.invoiceaccountingdata.insurance_charges);
      this.invoiceaccountingform.get("invoiceaccounting_outstandingamount")?.setValue(this.invoiceaccountingdata.packing_charges);
      this.invoiceaccountingform.get("invoiceaccounting_roundoff")?.setValue(this.invoiceaccountingdata.round_off);
      this.invoiceaccountingform.get("invoiceaccounting_grandtotal")?.setValue(this.invoiceaccountingdata.grand_total);     
      this.invoiceaccountingform.get("invoiceaccounting_termsandconditions")?.setValue(this.invoiceaccountingdata.termsandconditions);
    });
  }

salesinvoicesubmit() {
    console.log(this.invoiceaccountingform)

    var api = 'Einvoice/salesinvoicesubmit';
    this.service.post(api, this.invoiceaccountingform.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.router.navigate(['/einvoice/SalesInvoiceSummary']);
        this.ToastrService.success(result.message)
      }
    },
    );
  }

back() {
  this.router.navigate(['/crm/CrmTrnMarketingManagerSummary']);
  }

  redirecttolist(){
    this.router.navigate(['/einvoice/SalesInvoiceSummary'])
  }

}
