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

@Component({
  selector: 'app-smr-trn-renewaladd',
  templateUrl: './smr-trn-renewaladd.component.html',
  styleUrls: ['./smr-trn-renewaladd.component.scss']
})
export class SmrTrnRenewaladdComponent {
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '40rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    
  };
  

  RenewalAddForm!: FormGroup;
  Viewsalesordersummary_list: any[] = [];
  Viewsalesorderdetail_list: any[] = [];
  
  summary_list1: any[] = [];
  pick: Array<any> = [];
  allchargeslist: any[] = [];
  data!: any[];
  branch_gid_key: any;
  branch_list: any;
  customer_gid:any;
  responsedata: any;
  customer: any;
  salesorder_gid: any;
  renewal_description: any;

   
   constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private router: ActivatedRoute,
    private route: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService,private cdr: ChangeDetectorRef,private zone: NgZone) {
  
  
      this.RenewalAddForm = new FormGroup({
        salesorder_gid : new FormControl(''),
        branch: new FormControl('', Validators.required),
        branch_name: new FormControl(''),
        customer_name: new FormControl(''),
        customer_gid: new FormControl(''),
        customercontact_names: new FormControl(''),
        customercontact_gid: new FormControl(''),
        customer_mobile: new FormControl(''),
        customer_email: new FormControl(''),      
        customer_address: new FormControl(''),
   
        customer_details: new FormControl(''),
        renewal_description: new FormControl(''),
        renewal_date: new FormControl(this.getCurrentDate()),
        currency_code: new FormControl(''),
        exchange_rate: new FormControl(''),
        
       
        
  
      })
      

}
getCurrentDate(): string {
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();
  return dd + '-' + mm + '-' + yyyy;
}



ngOnInit(): void {
  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options); 

  const salesorder_gid = this.router.snapshot.paramMap.get('salesorder_gid');
  this.salesorder_gid = salesorder_gid;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
  const deencryptedParam3 = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
  console.log(deencryptedParam)

  
  
  this.GetViewsalesorderSummary(deencryptedParam);
  this.GetViewsalesorderdetails(deencryptedParam3);

 

}



GetViewsalesorderSummary(salesorder_gid: any) {
  debugger
  var url = 'SmrTrnRenewalsummary/GetViewsalesorderSummary'
  let param = {
    salesorder_gid: salesorder_gid
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.Viewsalesordersummary_list = result.renewalsalesorder_list;
    this.RenewalAddForm.get("termsandconditions")?.setValue(this.Viewsalesordersummary_list[0].termsandconditions);
    this.RenewalAddForm.get("net_amount")?.setValue(this.Viewsalesordersummary_list[0].total_amount);
    this.RenewalAddForm.get("tax_name")?.setValue(this.Viewsalesordersummary_list[0].tax_name);
    this.RenewalAddForm.get("total_amountwithtax")?.setValue(this.Viewsalesordersummary_list[0].total_price);
    this.RenewalAddForm.get("addon_charges")?.setValue(this.Viewsalesordersummary_list[0].addon_charge);
    this.RenewalAddForm.get("additional_discount")?.setValue(this.Viewsalesordersummary_list[0].additional_discount);
    this.RenewalAddForm.get("freight_charges")?.setValue(this.Viewsalesordersummary_list[0].freight_charges);
    this.RenewalAddForm.get("buyback_charges")?.setValue(this.Viewsalesordersummary_list[0].buyback_charges);
    this.RenewalAddForm.get("packing_charges")?.setValue(this.Viewsalesordersummary_list[0].packing_charges);
    this.RenewalAddForm.get("insurance_charges")?.setValue(this.Viewsalesordersummary_list[0].insurance_charges);
    this.RenewalAddForm.get("roundoff")?.setValue(this.Viewsalesordersummary_list[0].roundoff);
    this.RenewalAddForm.get("grandtotal")?.setValue(this.Viewsalesordersummary_list[0].Grandtotal);
    this.RenewalAddForm.get("customer_name")?.setValue(this.Viewsalesordersummary_list[0].customer_name);
    this.RenewalAddForm.get("customer_address")?.setValue(this.Viewsalesordersummary_list[0].customer_address);
    this.RenewalAddForm.get("currency_code")?.setValue(this.Viewsalesordersummary_list[0].currency_code);
    this.RenewalAddForm.get("exchange_rate")?.setValue(this.Viewsalesordersummary_list[0].exchange_rate);
    this.RenewalAddForm.get("branch_name")?.setValue(this.Viewsalesordersummary_list[0].branch_name);
    const emailId = this.Viewsalesordersummary_list[0].customer_email;
    const contactTelephonenumber = this.Viewsalesordersummary_list[0].customer_mobile;
    const gst_no=this.Viewsalesordersummary_list[0].gst_number; 
    const customerDetails = `${emailId}\n${contactTelephonenumber}\n${gst_no}`;
    this.RenewalAddForm.get("customer_details")?.setValue(customerDetails);
    this.RenewalAddForm.get("salesorder_gid")?.setValue(salesorder_gid);
    
  });
   
}

GetViewsalesorderdetails(salesorder_gid: any) {
  var url = 'SmrTrnRenewalsummary/GetViewsalesorderdetails'
  this.NgxSpinnerService.show()
  let param = {
    salesorder_gid: salesorder_gid,
   
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.Viewsalesorderdetail_list = result.renewalsalesorderdetails_list;
    this.NgxSpinnerService.hide()
  });
}

get branch_name() {
  return this.RenewalAddForm.get('branch_name');
}

onSubmit( ) {
  debugger
  
  var params = {
    salesorder_gid: this.RenewalAddForm.value.salesorder_gid,
    renewal_description: this.RenewalAddForm.value.renewal_description,
    renewal_date: this.RenewalAddForm.value.renewal_date,
  }
  var url = 'SmrTrnRenewalsummary/getrenewal'
  this.NgxSpinnerService.show();
  this.service.postparams(url, params).subscribe((result: any) => {

    if (result.status == false) {
      this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message)
    }
    else {
      this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message)
      this.route.navigate(['/smr/SmrTrnRenevalsummary']);
    }
  });



}


  
   

}

