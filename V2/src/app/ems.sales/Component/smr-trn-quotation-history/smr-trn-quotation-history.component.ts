import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-smr-trn-quotation-history',
  templateUrl: './smr-trn-quotation-history.component.html',
  styleUrls: ['./smr-trn-quotation-history.component.scss']
})
export class SmrTrnQuotationHistoryComponent {
  quotationhistoryform : FormGroup | any;
  quotationhistorylist : any;
  responsedata : any;
  quotation_gid : any;
  quotationhistorysummarylist : any;
  quotationproduct_list : any [] = [];
  parameterValue1 : any;
  quotationdata : any;
  customer_gid:any;

  ngOnInit() {
   

    const quotation_gid = this.route.snapshot.paramMap.get('quotation_gid');
    this.quotation_gid = quotation_gid;
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    this.customer_gid = customer_gid;
  
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.quotation_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
  
    let param = {
      quotation_gid: deencryptedParam,
      customer_gid:deencryptedParam2
    }
    var api = 'SmrTrnQuotation/Getquotationhistorysummarydata';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.quotationhistorylist = this.responsedata.quotationhistorysummarylist;
    });
    this.Getquotationhistorydata();
  }

  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private service: SocketService,private ToastrService: ToastrService) {

  this.quotationhistoryform = new FormGroup({
    quotation_referenceno1: new FormControl(''),
    quotation_date: new FormControl('', Validators.required),
    customer_name: new FormControl('', Validators.required),
    quotation_remarks: new FormControl(''),
    quotation_gid: new FormControl(''),
    quote_refno: new FormControl(''),
    quote_date: new FormControl(''),
    quotation_customer_name: new FormControl(''),
    customer_contact_person: new FormControl(''),
    user_firstname: new FormControl(''),
    Grandtotal: new FormControl(''),
    quotation_status: new FormControl(''),
  
  })
}

Getquotationhistorydata() {
  const quotation_gid = this.route.snapshot.paramMap.get('quotation_gid');
  this.quotation_gid = quotation_gid;
  const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
  this.customer_gid = customer_gid;

  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.quotation_gid, secretKey).toString(enc.Utf8);
  const deencryptedParam2= AES.decrypt(this.quotation_gid, secretKey).toString(enc.Utf8);
  

  let param = {
    quotation_gid: deencryptedParam,
    customer_gid:deencryptedParam2
  }

  var api = 'SmrTrnQuotation/Getquotationhistorydata';

  this.service.getparams(api, param).subscribe((result: any) => {
    this.responsedata= result;      
    this.quotationdata = result;

    this.quotationhistoryform.get("quotation_gid")?.setValue(this.quotationdata.quotation_gid);
    this.quotationhistoryform.get("quotation_referenceno1")?.setValue(this.quotationdata.quotation_referenceno1);
    this.quotationhistoryform.get("quotation_date")?.setValue(this.quotationdata.quotation_date);
    this.quotationhistoryform.get("customer_name")?.setValue(this.quotationdata.customer_name);
    this.quotationhistoryform.get("quotation_remarks")?.setValue(this.quotationdata.quotation_remarks);
    this.quotationhistoryform.get("quote_refno")?.setValue(this.quotationdata.quote_refno);
    //this.quotationhistoryform.get("quotation_date")?.setValue(this.quotationdata.quotation_date);
    this.quotationhistoryform.get("customer_name")?.setValue(this.quotationdata.customer_name);
    this.quotationhistoryform.get("customer_contact_person")?.setValue(this.quotationdata.customer_contact_person);
    this.quotationhistoryform.get("user_firstname")?.setValue(this.quotationdata.user_firstname);
    this.quotationhistoryform.get("Grandtotal")?.setValue(this.quotationdata.Grandtotal);
    this.quotationhistoryform.get("quotation_status")?.setValue(this.quotationdata.quotation_status);
 });
}

Details(parameter: string,quotation_gid: string){
  this.parameterValue1 = parameter;
  this.quotation_gid = parameter;

  var url='SmrTrnQuotation/Getquotationproductdetails'
    let param = {
      quotation_gid : quotation_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.quotationproduct_list = result.quotationproduct_list;   
    });
  }

  onview(quotation_gid:any,customer_gid:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const lspage = 'QuoteHistory'
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
    this.router.navigate(['/smr/SmrTrnquotationviewNew',encryptedParam,encryptedParam2,lspage]) 
  }
  
  back(){
    this.router.navigate(['/smr/SmrTrnQuotationSummary'])
  }





}
