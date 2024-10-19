import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

interface IPayrunReport {
  branch_name: string;
  department_name: string;
 
  branch_gid: string;
  department_gid: string;
 
}
@Component({
  selector: 'app-hrm-trn-dailyattendance',
  templateUrl: './hrm-trn-dailyattendance.component.html',
  styleUrls: ['./hrm-trn-dailyattendance.component.scss']
})
export class HrmTrnDailyattendanceComponent {
  departmentlist: any[] = [];
  branchlist: any[] = [];
  reactiveForm!: FormGroup;
  responsedata: any;
  branch_name: any;
  department_name: any;
  PayrunReport: IPayrunReport;
  attendance_list:any[]=[];

  constructor(private formBuilder: FormBuilder, public NgxSpinnerService:NgxSpinnerService,
   private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
      this.PayrunReport = {} as IPayrunReport;

    }

 ngOnInit(): void {

  const options: Options = {
    dateFormat: 'd-m-Y',    
  };
  flatpickr('.date-picker', options); 

    
  this.reactiveForm = new FormGroup({

        date : new FormControl(this.getCurrentDate()),
       department_name : new FormControl(''), 
       branch_name : new FormControl(''), 
        department_gid: new FormControl(''),
    });

  var api='PayRptPayrunSummary/GetBranchDtl'
  this.service.get(api).subscribe((result:any)=>{
  this.branchlist = result.GetBranchDtl;
  //console.log(this.branchlist)
 });

 var api='PayRptPayrunSummary/GetDepartmentDtl'
 debugger;
 this.service.get(api).subscribe((result:any)=>{
 this.departmentlist = result.GetDepartmentDtl;
});

this.search()

}
getCurrentDate(): string {
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();

  return dd + '-' + mm + '-' + yyyy;
}

oncleardepartment(){
  this.department_name = ''
}
onclearbranch(){
  this.branch_name = ''
}

search(){
  debugger 
  var url = 'HrmTrnDailyAttendance/GetDailySummary'
  this.service.getparams(url, this.reactiveForm.value).subscribe((result: any) => {
    this.responsedata = result;
    this.attendance_list = this.responsedata.daily_list1;

    setTimeout(() => {
      $('#attendance_list').DataTable();
    },);
  });
}
}
