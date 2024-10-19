import { Component, DebugEventListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-renewalmanagersummary',
  templateUrl: './smr-trn-renewalmanagersummary.component.html',
  styleUrls: ['./smr-trn-renewalmanagersummary.component.scss']
})
export class SmrTrnRenewalmanagersummaryComponent {

  renewalsummary_list: any[] = [];
  responsedata: any;
  parameterValue1: any;
  company_code: any;
  showOptionsDivId: any; 
  rows: any[] = [];


constructor(public service:SocketService,private router:Router,private route:Router, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {
    
}

ngOnInit(): void {
  this.GetRenewalManagerSummary();
  // this.GetRenewalManagerCount();
  this.renewalsummary_list.sort((a,b) => {
    return new (b.created_date) - new (a.created_date); 
  });
}

GetRenewalManagerSummary(){
  this.NgxSpinnerService.show();
 var url = 'SmrTrnRenewalmanagersummary/GetRenewalManagerSummary'
 this.service.get(url).subscribe((result: any) => {
   $('#renewalsummary_list').DataTable().destroy();
   this.responsedata = result;
   this.renewalsummary_list = this.responsedata.GetRenewalSummary_lists;
   setTimeout(() => {
     $('#renewalsummary_list').DataTable();
   }, 1);

   this.NgxSpinnerService.hide();
 });
}

onadd() {
  this.router.navigate(['/smr/SmrTrnRenewals360'])

}

// GetRenewalManagerCount(){
//   this.NgxSpinnerService.show();
//   var url = 'SmrTrnRenewalmanagersummary/GetRenewalManagerCount'
//   this.service.get(url).subscribe((result: any) => {
//     $('#renewalsummary_list').DataTable().destroy();
//     this.responsedata = result;
//     this.renewalsummary_list = this.responsedata.GetRenewalSummary_lists;
//     console.log(this.renewalsummary_list )
//     setTimeout(() => {
//       $('#renewalsummary_list').DataTable();
//     }, 1);
 
//     this.NgxSpinnerService.hide();
//   });
// }

}

