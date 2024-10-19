import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-renewalemployee',
  templateUrl: './smr-trn-renewalemployee.component.html',
  styleUrls: ['./smr-trn-renewalemployee.component.scss']
})
export class SmrTrnRenewalemployeeComponent {
  renewalemployee_list: any[] = [];
  responsedata:any;
  campaign_gid:any;
  parameterValue:any;
  employeeGid: any;
  renewal_gid:any;
  renewal_gid_key:any;
  renewal_gid1:any;
  ngOnInit() {
    const campaign_gid =this.route.snapshot.paramMap.get('campaign_gid');
    const renewal_gid1 =this.route.snapshot.paramMap.get('renewal_gid1');
    this.campaign_gid= campaign_gid;
    this.renewal_gid= renewal_gid1;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.campaign_gid,secretKey).toString(enc.Utf8);
    this.renewal_gid = AES.decrypt(this.renewal_gid,secretKey).toString(enc.Utf8);

    this.GetViewrenewaldetails(deencryptedParam);
  }
  constructor(private router: Router, 
    public NgxSpinnerService:NgxSpinnerService,private route: ActivatedRoute, 
    private fb: FormBuilder, private service: SocketService, private ToastrService: ToastrService) {
    
  }
  openModalrenew(campaign_gid: string, employee_gid: string) {
    this.parameterValue = campaign_gid;
    this.employeeGid = employee_gid;
  }
  ononassignemployee() {
    this.NgxSpinnerService.show();
    var url = 'SmrTrnRenewalsummary/Getassignrenewal';
    let param = {
      campaign_gid: this.parameterValue,
      employee_gid: this.employeeGid,
      renewal_gid: this.renewal_gid
    };
    
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        window.location.reload();
      }
    });
  }
  onassign(campaign_gid: any,employee_gid:any,renewal_gid:any)
  {
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const param1 = (employee_gid);
    const param2 = (renewal_gid);
    const campaign_gid1 = AES.encrypt(param,secretKey).toString();
    const employee_gid1 = AES.encrypt(param1,secretKey).toString();
    const renewal_gid1 = AES.encrypt(param2,secretKey).toString();
    this.router.navigate(['/smr/SmrTrnRenewalassign', campaign_gid1,employee_gid1,renewal_gid1]) 
    
  } 
  GetViewrenewaldetails(campaign_gid: any) {
    var url='SmrTrnRenewalteamsummary/Getrenewalemployee'
     this.NgxSpinnerService.show()
     let param = {
      campaign_gid : campaign_gid ,
     }
     this.service.getparams(url,param).subscribe((result:any)=>{
     this.responsedata=result;
     this.renewalemployee_list = result.renewalemployee_list;   
     this.NgxSpinnerService.hide()
     });
   }

}
