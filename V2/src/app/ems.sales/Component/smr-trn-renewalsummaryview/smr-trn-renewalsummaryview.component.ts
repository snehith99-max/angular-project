import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';
@Component({
  selector: 'app-smr-trn-renewalsummaryview',
  templateUrl: './smr-trn-renewalsummaryview.component.html',
  styleUrls: ['./smr-trn-renewalsummaryview.component.scss']
})
export class SmrTrnRenewalsummaryviewComponent {
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
  renewal_gid:any;
  responsedata:any;
  renewalsummary_list:any[]=[];
  Viewrenewaldetail_list : any;
  GetTaxSegmentList:any[]=[];

  ngOnInit() {
    const renewal_gid =this.route.snapshot.paramMap.get('renewal_gid');
    this.renewal_gid= renewal_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.renewal_gid,secretKey).toString(enc.Utf8);
     this.GetViewrenewalSummary(deencryptedParam);
     this.GetViewrenewaldetails(deencryptedParam);
  }

  constructor(private router: Router, public NgxSpinnerService:NgxSpinnerService,private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {
    this.quotationviewform = new FormGroup({
      renewal_date: new FormControl(''),
      user_name: new FormControl(''),
      salesorder_gid: new FormControl(''),
      leadbank_state: new FormControl(''),
      country_name: new FormControl(''),
      leadbank_city: new FormControl(''),
      leadbank_address1: new FormControl(''),
      mobile: new FormControl(''),
      email: new FormControl(''),
      leadbankcontact_name: new FormControl(''),
      leadbank_name: new FormControl(''),
      salesorder_date: new FormControl('')
    })
  }
  //view
  GetViewrenewalSummary(renewal_gid: any) {
  
  var url='SmrTrnRenewalsummary/GetViewrenewalSummary'
  let param = {
    renewal_gid : renewal_gid 
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.renewalsummary_list = result.renewalview_list;
    this.quotationviewform.get("renewal_date")?.setValue(this.renewalsummary_list[0].renewal_date);
    this.quotationviewform.get("user_name")?.setValue(this.renewalsummary_list[0].user_name);
    this.quotationviewform.get("so_referencenumber")?.setValue(this.renewalsummary_list[0].so_referencenumber);
    this.quotationviewform.get("salesorder_date")?.setValue(this.renewalsummary_list[0].salesorder_date);
    this.quotationviewform.get("customer_name")?.setValue(this.renewalsummary_list[0].customer_name);
    this.quotationviewform.get("customer_contact_person")?.setValue(this.renewalsummary_list[0].customer_contact_person);
    this.quotationviewform.get("customer_mobile")?.setValue(this.renewalsummary_list[0].customer_mobile);
    this.quotationviewform.get("customer_email")?.setValue(this.renewalsummary_list[0].customer_email);
    this.quotationviewform.get("customer_address")?.setValue(this.renewalsummary_list[0].customer_address);
    this.quotationviewform.get("city")?.setValue(this.renewalsummary_list[0].city);
    this.quotationviewform.get("country_name")?.setValue(this.renewalsummary_list[0].country_name);
    this.quotationviewform.get("state")?.setValue(this.renewalsummary_list[0].state);
  });
}
GetViewrenewaldetails(renewal_gid: any) {
  debugger
 var url='SmrTrnRenewalsummary/GetViewrenewaldetails'
  this.NgxSpinnerService.show()
  let param = {
    renewal_gid : renewal_gid ,
    
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
  this.responsedata=result;
  this.Viewrenewaldetail_list = result.Viewrenewaldetail_list;   
  this.NgxSpinnerService.hide()

  });
}


}

