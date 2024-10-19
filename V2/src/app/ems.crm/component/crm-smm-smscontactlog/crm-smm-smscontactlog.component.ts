import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-crm-smm-smscontactlog',
  templateUrl: './crm-smm-smscontactlog.component.html',
  styleUrls: ['./crm-smm-smscontactlog.component.scss']
})
export class CrmSmmSmscontactlogComponent {
  responsedata: any;
  customerdetail_list:any;
  phone_number:any;
  individualsmslog:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
  }
  ngOnInit(): void {
  
    this.GetSmsCampaignSummary();
  }


  GetSmsCampaignSummary() {
    this.NgxSpinnerService.show();
    var api = 'SmsCampaign/SmsLeadCustomerDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.customerdetail_list = this.responsedata.smsleadcustomerdetails_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#customerdetail_list').DataTable();
      }, 1);
    });

  }

  getindividuallog(default_phone:any){
    debugger
    console.log(default_phone)
    let param ={
      phone_number :default_phone,
    }
    var api = 'SmsCampaign/Getindividuallog'
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#individualsmslog').DataTable().destroy();
      this.responsedata = result;
      this.individualsmslog = this.responsedata.individualsmslog;
      this.phone_number = this.individualsmslog[0].phone_number;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#individualsmslog').DataTable();
      }, 1);


    });
  }
}
