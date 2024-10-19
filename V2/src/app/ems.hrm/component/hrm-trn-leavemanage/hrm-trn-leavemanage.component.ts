import { Component, ElementRef, ViewChild  } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';
import { ExcelService } from 'src/app/Service/excel.service';
import { get } from 'jquery';
import  jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-hrm-trn-leavemanage',
  templateUrl: './hrm-trn-leavemanage.component.html',
  styleUrls: ['./hrm-trn-leavemanage.component.scss']
})
export class HrmTrnLeavemanageComponent {
  radioSelected: any;
  leaveperiod: any;
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  reactiveFormsubmit!: FormGroup;
  reactiveFormLeave!: FormGroup;
  reactiveFormpermission!: FormGroup;
  reactiveFormonduty!: FormGroup;
  leavemanage_list: any[] = [];
  permission_list: any[] = [];
  onduty_list: any[] = [];
  branchlist: any[] = [];
  employeelist: any[] = [];
  employeedate_list: any[] = [];
  leave_list: any[] = [];
  leaveavailablelist: any[] = [];
  responsedata: any;
  parameterValue: any;
  parameterValue2: any;
  parameterValue3: any;
  branch_data: any;
  employee_data: any;
  employee_user: any;
  employee_member: any;
  type_name: any;
  type_data: any;
  onduty_user: any;
  day_user: any;
  leave_data: any;

 
  showDateOfJoining: boolean = false;
  showDateOfJoining1: boolean = false;
  showDateOfJoining2: boolean = false;
  joiningDate: any;
  branch_permission: any;
  branch_onduty: any;
  
 
  constructor(private formBuilder: FormBuilder,
    private excelService : ExcelService,
     private route: ActivatedRoute,
     private router: Router, private ToastrService: ToastrService, 
     public service: SocketService) {
   
    }

    ngOnInit(): void {
    
      this.GetOnDutySummary();
      this.GetLeaveManageSummary();
      this.GetPermissionSummary();

      const options: Options = {
        dateFormat: 'd-m-Y',    
      };
      flatpickr('.date-picker', options);
      this.reactiveFormLeave = new FormGroup({
        employeegid: new FormControl({ value: '', disabled: false },[Validators.required]),
        branch_memb: new FormControl('',[Validators.required]),
        type: new FormControl(''),
        apply_date: new FormControl(this.getCurrentDate()),
        employee_date: new FormControl(''),
        leave_datefrom: new FormControl('',[Validators.required]),
        leave_dateto: new FormControl('',[Validators.required]),
        leave_days: new FormControl(''),
        leavetype_gid: new FormControl('',[Validators.required]),
        leave_balance: new FormControl(''),
        reason: new FormControl('',[Validators.required]),
        days_name: new FormControl(''),
        session_leave: new FormControl(''),
        leave_datehalf: new FormControl(''),

        
     });

     this.reactiveFormpermission = new FormGroup({
      employeenamegid: new FormControl({ value: '', disabled: false },[Validators.required]),
      branch_name: new FormControl('',[Validators.required]),
      type: new FormControl(''),
      permissionapply_date: new FormControl(this.getCurrentDate()),
      employee_date: new FormControl(''),
      permission_date: new FormControl('',[Validators.required]),
      from_hrs: new FormControl('',[Validators.required]),
      to_hrs: new FormControl('',[Validators.required]),
      total_duration: new FormControl(''),
      reason_permission: new FormControl('',[Validators.required]),
     });

     this.reactiveFormonduty = new FormGroup({
      employeedetailgid: new FormControl({ value: '', disabled: false },[Validators.required]),
      branch: new FormControl('',[Validators.required]),
      type: new FormControl(''),
      ondutyapply_date: new FormControl(this.getCurrentDate()),
      employee_date: new FormControl(''),
      onduty_date: new FormControl('',[Validators.required]),
      from_hrsod: new FormControl('',[Validators.required]),
      to_hrsod: new FormControl('',[Validators.required]),
      total_durationod: new FormControl(''),
      reason_onduty: new FormControl('',[Validators.required]),
      onduty_period: new FormControl(''),
      session: new FormControl(''),



     });


      var api='HrmTrnLeaveManage/GetBranchDtl'
      this.service.get(api).subscribe((result:any)=>{
      this.branchlist = result.Getbranch_detail;
      
     });

     var api='HrmTrnLeaveManage/GetEmployeeDtl'
      this.service.get(api).subscribe((result:any)=>{
      this.employeelist = result.GetEmployee_dtl;
      
     });

     var api='HrmTrnLeaveManage/GetLeaveAvailableDtl'
      this.service.get(api).subscribe((result:any)=>{
      this.leaveavailablelist = result.GetLeave_Available;
      
     });


      this.reactiveFormLeave.get('leave_datefrom')?.valueChanges.subscribe(() => {
      this.calculateLeaveDays();
     });
  
     this.reactiveFormLeave.get('leave_dateto')?.valueChanges.subscribe(() => {
     this.calculateLeaveDays();
     });

     this.reactiveFormpermission.get('from_hrs')?.valueChanges.subscribe(() => {
      this.calculateDuration();
     });

     this.reactiveFormpermission.get('to_hrs')?.valueChanges.subscribe(() => {
      this.calculateDuration();
     });
  
     this.reactiveFormonduty.get('from_hrsod')?.valueChanges.subscribe(() => {
      this.calculateOnDutyDuration();
     });

     this.reactiveFormonduty.get('to_hrsod')?.valueChanges.subscribe(() => {
      this.calculateOnDutyDuration();
     });
     
    

    }

    // <-- Leave -->
    get branch_memb() {
      return this.reactiveFormLeave.get('branch_memb')!;
    }
    get employeegid() {
      return this.reactiveFormLeave.get('employeegid')!;
    }
    get leave_datefrom() {
      return this.reactiveFormLeave.get('leave_datefrom')!;
    }
    get leave_dateto() {
      return this.reactiveFormLeave.get('leave_dateto')!;
    }
    get leave_type() {
      return this.reactiveFormLeave.get('leavetype_gid')!;
    }
    get reason() {
      return this.reactiveFormLeave.get('reason')!;
    }
   

// <-- Permission -->
    get branch_name() {
      return this.reactiveFormpermission.get('branch_name')!;
    }
    get employeenamegid() {
      return this.reactiveFormpermission.get('employeenamegid')!;
    }
    get permission_date() {
      return this.reactiveFormpermission.get('permission_date')!;
    }
    get from_hrs() {
      return this.reactiveFormpermission.get('from_hrs')!;
    }
    get to_hrs() {
      return this.reactiveFormpermission.get('to_hrs')!;
    }
    get reason_permission() {
      return this.reactiveFormpermission.get('reason_permission')!;
    }

    // <-- OnDuty -->
    get branch() {
      return this.reactiveFormonduty.get('branch')!;
    }
    get employeedetailgid() {
      return this.reactiveFormonduty.get('employeedetailgid')!;
    }
    get onduty_date() {
      return this.reactiveFormonduty.get('onduty_date')!;
    }
    get from_hrsod() {
      return this.reactiveFormonduty.get('from_hrsod')!;
    }
    get to_hrsod() {
      return this.reactiveFormonduty.get('to_hrsod')!;
    }
    get reason_onduty() {
      return this.reactiveFormonduty.get('reason_onduty')!;
    }

    calculateLeaveDays() {

        const fromDate = this.reactiveFormLeave.get('leave_datefrom')?.value;
        const toDate = this.reactiveFormLeave.get('leave_dateto')?.value;
        if (fromDate && toDate) {
          const startDate = new Date(fromDate);
          const endDate = new Date(toDate);
          const diffTime = Math.abs(endDate.getTime() - startDate.getTime());
          const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24)) + 1;
          this.reactiveFormLeave.get('leave_days')?.setValue(diffDays.toString());
        }

    }

     calculateDuration() {
      const fromTime = this.reactiveFormpermission.get('from_hrs')?.value;
      const toTime = this.reactiveFormpermission.get('to_hrs')?.value;
      if (fromTime && toTime) {
        const [fromHours, fromMinutes] = fromTime.split(':').map(Number);
        const [toHours, toMinutes] = toTime.split(':').map(Number);
        const startTime = new Date();
        startTime.setHours(fromHours, fromMinutes, 0, 0);
      
        const endTime = new Date();
        endTime.setHours(toHours, toMinutes, 0, 0);
      
        const duration = (endTime.getTime() - startTime.getTime()) / (1000 * 60 * 60); // Convert milliseconds to hours
        this.reactiveFormpermission.get('total_duration')?.setValue(duration.toString());
      }

     
       }

       calculateOnDutyDuration() {
        const fromTime = this.reactiveFormonduty.get('from_hrsod')?.value;
        const toTime = this.reactiveFormonduty.get('to_hrsod')?.value;
        if (fromTime && toTime) {
          const [fromHours, fromMinutes] = fromTime.split(':').map(Number);
          const [toHours, toMinutes] = toTime.split(':').map(Number);
          const startTime = new Date();
          startTime.setHours(fromHours, fromMinutes, 0, 0);
        
          const endTime = new Date();
          endTime.setHours(toHours, toMinutes, 0, 0);
        
          const duration = (endTime.getTime() - startTime.getTime()) / (1000 * 60 * 60); // Convert milliseconds to hours
          this.reactiveFormonduty.get('total_durationod')?.setValue(duration.toString());
        }
  
       
         }

    datejoinFetch(event: any): void {
      if (event) {
        this.showDateOfJoining = true;
      } else {
        this.showDateOfJoining = false;
      }
  
      let employee_gid = this.reactiveFormLeave.get('employeegid')?.value;
  
      let param = {
        employee_gid: employee_gid
      }
    
    var url = 'HrmTrnLeaveManage/GetDateOfJoin'
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.employeedate_list = result.GetDate_Join;
      this.reactiveFormLeave.get("employee_date")?.setValue(this.employeedate_list[0].employee_joiningdate);
     
    });
    var url = 'HrmTrnLeaveManage/GetLeavebalance'
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.leave_list = result.GetLeaveBalance;
     
    });
  }
  
  datejoinFetch1(event: any): void {
    if (event) {
      this.showDateOfJoining1 = true;
    } else {
      this.showDateOfJoining1 = false;
    }

    let employee_gid = this.reactiveFormpermission.get('employeenamegid')?.value;

    let param = {
      employee_gid: employee_gid
    }
  
  var url = 'HrmTrnLeaveManage/GetDateOfJoin'
 
  this.service.getparams(url, param).subscribe((result: any) => {
  this.responsedata=result;
    this.employeedate_list = result.GetDate_Join;
    this.reactiveFormpermission.get("employee_date")?.setValue(this.employeedate_list[0].employee_joiningdate);
   
  });
}

datejoinFetch2(event: any): void {
  if (event) {
    this.showDateOfJoining2 = true;
  } else {
    this.showDateOfJoining2 = false;
  }

  let employee_gid = this.reactiveFormonduty.get('employeedetailgid')?.value;

  let param = {
    employee_gid: employee_gid
  }

var url = 'HrmTrnLeaveManage/GetDateOfJoin'

this.service.getparams(url, param).subscribe((result: any) => {
this.responsedata=result;
  this.employeedate_list = result.GetDate_Join;
  this.reactiveFormonduty.get("employee_date")?.setValue(this.employeedate_list[0].employee_joiningdate);
 
});
}

leaveavailableFetch(event: any): void {
  if (event) {
    this.showDateOfJoining2 = true;
  } else {
    this.showDateOfJoining2 = false;
  }
  let employee_gid = this.reactiveFormLeave.get('employeegid')?.value;
  
  let param = {
    employee_gid: employee_gid
  }
  var url = 'HrmTrnLeaveManage/GetLeaveAvailable'
   
  this.service.getparams(url, param).subscribe((result: any) => {
  this.responsedata=result;
  this.leave_list = result.GetLeave_Available;
   
  });

}

 getCurrentDate(): string {
      const today = new Date();
      const dd = String(today.getDate()).padStart(2, '0');
      const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
      const yyyy = today.getFullYear();
   
      return dd + '-' + mm + '-' + yyyy;
    }

    
    GetLeaveManageSummary() {
      var url = 'HrmTrnLeaveManage/GetLeaveManageSummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.leavemanage_list = this.responsedata.leavemanagelist;
        setTimeout(() => {
          $('#leavemanage_list').DataTable();
        }, );
  
  
      });
    }

    leavesubmit(){
      if (this.reactiveFormLeave.value.leave_datefrom != null && this.reactiveFormLeave.value.leave_datefrom != '') {
        for (const control of Object.keys(this.reactiveFormLeave.controls)) {
          this.reactiveFormLeave.controls[control].markAsTouched();
        }
        this.reactiveFormLeave.value;
        var url = 'HrmTrnLeaveManage/LeaveSubmit'
        this.service.post(url, this.reactiveFormLeave.value).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            
            this.ToastrService.success(result.message)
          }
        });
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
    }

    permissionsubmit(){
      if (this.reactiveFormpermission.value.permission_date != null && this.reactiveFormpermission.value.permission_date != '') {
        for (const control of Object.keys(this.reactiveFormpermission.controls)) {
          this.reactiveFormpermission.controls[control].markAsTouched();
        }
        this.reactiveFormpermission.value;
        var url = 'HrmTrnLeaveManage/PermissionSubmit'
        this.service.post(url, this.reactiveFormpermission.value).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            
            this.ToastrService.success(result.message)
          }
        });
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
    }

    GetPermissionSummary() {
      var url = 'HrmTrnLeaveManage/GetPermissionSummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.permission_list = this.responsedata.permissionnamelist;
        setTimeout(() => {
          $('#permission_list').DataTable();
        }, );
  
  
      });
    }

    GetOnDutySummary() {
      var url = 'HrmTrnLeaveManage/GetOnDutySummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.onduty_list = this.responsedata.ondutynamelist;
        setTimeout(() => {
          $('#onduty_list').DataTable();
        }, );
  
  
      });
    }

    OnDutySubmit(){
      if (this.reactiveFormonduty.value.onduty_date != null && this.reactiveFormonduty.value.onduty_date != '') {
        for (const control of Object.keys(this.reactiveFormonduty.controls)) {
          this.reactiveFormonduty.controls[control].markAsTouched();
        }
        this.reactiveFormonduty.value;
        var url = 'HrmTrnLeaveManage/OnDutySubmit'
        this.service.post(url, this.reactiveFormonduty.value).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            
            this.ToastrService.success(result.message)
          }
        });
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
    }




  openModaldelete(parameter: string){
    this.parameterValue = parameter
  }

  ondelete(){
 console.log(this.parameterValue);
      var url = 'HrmTrnLeaveManage/DeleteLeaveManage'
      this.service.getid(url, this.parameterValue).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
        }
        this.GetLeaveManageSummary();
  
      });
  }

  openModaldelete2(parameter: string){
    this.parameterValue2 = parameter
  }

  ondelete2(){
    console.log(this.parameterValue2);
    var url = 'HrmTrnLeaveManage/DeletePermission'
    this.service.getid(url, this.parameterValue2).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.GetPermissionSummary();

    });
  }

  openModaldelete3(parameter: string){
    this.parameterValue3 = parameter
  }

  ondelete3(){
    console.log(this.parameterValue3);
    var url = 'HrmTrnLeaveManage/DeleteOnDuty'
    this.service.getid(url, this.parameterValue3).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.GetOnDutySummary();

    });
  }
  
  back(){
    this.router.navigate(['/hrm/HrmTrnLeavemanagesummary'])
  }


}
