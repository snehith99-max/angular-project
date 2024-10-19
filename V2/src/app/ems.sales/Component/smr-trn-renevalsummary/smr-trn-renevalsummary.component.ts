import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-smr-trn-renevalsummary',
  templateUrl: './smr-trn-renevalsummary.component.html',
  styleUrls: ['./smr-trn-renevalsummary.component.scss']
})
export class SmrTrnRenevalsummaryComponent {

  renewalsummary_list: any[] = [];
  renewalsummary_list1: any[] = [];
  renewalsummary_list2: any[] = [];
  renewalsummary_list3: any[] = [];
  parameterValue: any;
  showOptionsDivId: any;
  paramvalue: any;
  setdaysform!: FormGroup;
  responsedata: any;
  Viewrenewaldetail_list: any;
  renewal_gid:any;

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private router: ActivatedRoute,
    private route: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit() {

    this.setdaysform = new FormGroup({
      setdaysproduct: new FormControl(''),
    })

    var Getexpirysapi = 'SmrTrnRenewalsummary/GetSmrTrnRenewalall';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.renewalsummary_list2 = result.renewalsummary_list2;
      setTimeout(() => {
        $('#renewalsummary_list2').DataTable();
      }, 1);
    });
  }

  toggleOptions(product_gid: any) {
    if (this.showOptionsDivId === product_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = product_gid;
    }
  }
  add() {
    this.route.navigate(['/smr/SmrTrnRaiseAgreement'])
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  openModalrenew(parameter: string) {
    this.parameterValue = parameter
  
  }

  ondrop() {
    debugger
 // API endpoint URL
  this.NgxSpinnerService.show()
    var url = 'SmrTrnRenewalInvoiceSummary/GetDroprenewal'
    let param = {
      renewal_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
           
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        window.location.reload();
    }
  });
}
  openModalasssign(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/smr/SmrTrnAssignrenewalagreement',encryptedParam])   
  }
  onadd() {
    this.route.navigate(['/smr/SmrTrnRenewals360'])
  }
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/smr/SmrTrnRenewalsummaryview',encryptedParam])   
  }
  RaisetoOrder(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/smr/SmrTrnRenewaltoInvoice',encryptedParam]) 
  }
  Invoicetotag(params: any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
     this.route.navigate(['/smr/SmrTrnAgreementtoinvoicetag',encryptedParam]) 
  }
  open(renewal_gid: any) {
    this.paramvalue = renewal_gid;
  }
ondelete() {
this.NgxSpinnerService.show()
  var url = 'SmrTrnRenewalsummary/Getdeleterenewal'
  let param = {
    renewal_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if(result.status ==false){
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();
    }
    else{
         
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      window.location.reload();
  }
});
}
GetViewrenewaldetails(renewal_gid: any) {
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
 
  onEdit(renewal_gid:any){
    const key = 'storyboard';
    const param = renewal_gid;
    const renewalgid = AES.encrypt(param,key).toString();
    this.route.navigate(['/smr/SmrRenewalEdit', renewalgid])
  
 }
}



