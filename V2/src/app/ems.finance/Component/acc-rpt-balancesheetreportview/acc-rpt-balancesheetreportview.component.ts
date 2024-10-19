import { Component } from '@angular/core';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-acc-rpt-balancesheetreportview',
  templateUrl: './acc-rpt-balancesheetreportview.component.html',
  styleUrls: ['./acc-rpt-balancesheetreportview.component.scss']
})
export class AccRptBalancesheetreportviewComponent {
  account_gid: any;
  finyear: any;
  branch_name: any;
  reactiveform: FormGroup | any;
  responsedata: any;
  LedgerView_list: any[]= [];
  constructor(public service: SocketService,private route: ActivatedRoute){

  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    const deencrypted_gid = this.route.snapshot.paramMap.get('account_gid');
    const deencrypted_finyear = this.route.snapshot.paramMap.get('finyear');
    const deencrypted_branch = this.route.snapshot.paramMap.get('branch');
    this.account_gid= deencrypted_gid;
    this.finyear= deencrypted_finyear;
    this.branch_name= deencrypted_branch;
    const secretKey = 'storyboarderp';
    const account_gid = AES.decrypt(this.account_gid, secretKey).toString(enc.Utf8);
    const finyear = AES.decrypt(this.finyear, secretKey).toString(enc.Utf8);
    const branch_name = AES.decrypt(this.branch_name, secretKey).toString(enc.Utf8);
    
    this.reactiveform = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
    this.GetLeadgerView(account_gid,finyear,branch_name);
  }
  GetLeadgerView(account_gid: any, finyear: any, branch_name: any){
    debugger
    let param = {
      account_gid: account_gid,
      finyear: finyear,
      branch_name: branch_name,
    }
    var url = 'BSIEReports/GetLedgerView'
    this.service.getparams(url,param).subscribe((result: any) =>{
      this.responsedata = result;
      this.LedgerView_list = result.LedgerView_list;
    });
  }
  OnChangeFinancialYear(){

  }
}
