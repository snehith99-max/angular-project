import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

interface IEmployeePaymentview {

}

@Component({
  selector: 'app-pay-rpt-employeepaymentdetailsview',
  templateUrl: './pay-rpt-employeepaymentdetailsview.component.html',
  styleUrls: ['./pay-rpt-employeepaymentdetailsview.component.scss']
})
export class PayRptEmployeepaymentdetailsviewComponent {
  ViewEmployeeReportSummary_list:any [] = [];
  ViewPromotionHistory_list:any [] = [];
  ViewPaymentDetails_list:any [] = [];
  EmployeePaymentview!: IEmployeePaymentview;
  responsedata: any;
  employee: any;
  employee_gid: any;

  constructor(private formBuilder: FormBuilder,private ToastrService: ToastrService, route:Router,private router:ActivatedRoute,public service :SocketService) {
    this.EmployeePaymentview= {} as IEmployeePaymentview;
   }

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

    const employee_gid =this.router.snapshot.paramMap.get('employee_gid');
    this.employee = employee_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.getViewEmployeePaymentSummary(deencryptedParam);
    this.getViewPromotionHistory(deencryptedParam);
    this.employee_gid=deencryptedParam;

   
  }

  getViewEmployeePaymentSummary(employee_gid: any) {
    var url='PayRptEmployeeHistory/getViewEmployeePaymentSummary'
    let param = {
      employee_gid : employee_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.ViewEmployeeReportSummary_list = result.getViewEmployeeReportSummary;   
    });
}

getViewPromotionHistory(employee_gid: any) {
var url='PayRptEmployeeHistory/getViewPromotionHistory'
let param = {
  employee_gid : employee_gid 
}
this.service.getparams(url,param).subscribe((result:any)=>{
this.responsedata=result;
this.ViewPromotionHistory_list = result.getViewPromotionHistory;   
});
}

getViewPaymentDetails(employee_gid: any) {
  var url='PayRptEmployeeHistory/getViewPaymentDetails'
  let param = {
    employee_gid : employee_gid 
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
  this.responsedata=result;
  this.ViewPaymentDetails_list = result.getViewPaymentDetails;   
  });
  }


}
