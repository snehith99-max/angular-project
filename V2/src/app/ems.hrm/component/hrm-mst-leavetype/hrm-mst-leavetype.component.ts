import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface ILeavetype {
  leave_code: string;
  leave_name: string;
}
@Component({
  selector: 'app-hrm-mst-leavetype',
  templateUrl: './hrm-mst-leavetype.component.html',
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class HrmMstLeavetypeComponent {
  // showOptionsDivId: any;
  Leavetype_list: any[] = [];
  consider_list: any[] = [];
  responsedata: any;
  reactiveFormadd!: FormGroup;
  leavetype!: ILeavetype;
  parameterValue: any;
  mdlconsiderlist: any;
  Code_Generation: any;

  showInputField: boolean | undefined;



  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService :NgxSpinnerService) {
    this.leavetype = {} as ILeavetype;
  }
  ngOnInit(): void {

    this.reactiveFormadd = new FormGroup({
      leave_code_auto: new FormControl(''),
      leave_code_manual: new FormControl('', [Validators.required,]),
      leave_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      Status_flag: new FormControl('Y'),
      weekoff_consider: new FormControl('N'),
      holiday_consider: new FormControl('N'),
      carry_forward: new FormControl('N'),
      Accured_type: new FormControl('N'),
      negative_leave: new FormControl('N'),
      Consider_as: new FormControl(''),
      Leave_Days: new FormControl(''),
      Code_Generation: new FormControl('Y'),
    });


    //// Summary Grid//////
    var url = 'LeaveTypeGrade/LeavetypeSummary'

    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.Leavetype_list = this.responsedata.Leavetype_list;
      setTimeout(() => {
        $('#Leavetype_list').DataTable();
      },);
    });

    this.consider_list = [
      { label: 'Leave with pay', value: 'leave with pay' },
      { label: 'Leave without pay', value: 'leave without pay' }
    ];
  }

  get leave_code_manual() {
    return this.reactiveFormadd.get('leave_code_manual')!;
  }
  get leave_name() {
    return this.reactiveFormadd.get('leave_name')!;
  }


  // public onsubmit(): void {

  //   debugger
  //   if (this.reactiveFormadd.value.leave_code != null && this.reactiveFormadd.value.leave_name != '')
  //     if (this.reactiveFormadd.value.Status_flag != null && this.reactiveFormadd.value.weekoff_consider != '')
  //       if (this.reactiveFormadd.value.holiday_consider != null && this.reactiveFormadd.value.carry_forward != '')
  //         if (this.reactiveFormadd.value.Accured_type != null && this.reactiveFormadd.value.negative_leave != '')
  //           if (this.reactiveFormadd.value.Consider_as != null && this.reactiveFormadd.value.Leave_Days != '') {
  //             // for (const control of Object.keys(this.reactiveFormadd.controls)) {
  //             //   this.reactiveFormadd.controls[control].markAsTouched();
  //             // }
  //             this.reactiveFormadd.value;
  //             let param = {
  //               leavetype_code: this.reactiveFormadd.value.leave_code,
  //               leavetype_name: this.reactiveFormadd.value.leave_name,
  //               leavetype_status: this.reactiveFormadd.value.Status_flag,
  //               consider_as: this.reactiveFormadd.value.Consider_as,
  //               weekoff_applicable: this.reactiveFormadd.value.weekoff_consider,
  //               holiday_applicable: this.reactiveFormadd.value.holiday_consider,
  //               carryforward: this.reactiveFormadd.value.carry_forward,
  //               accrud: this.reactiveFormadd.value.Accured_type,
  //               leave_days: this.reactiveFormadd.value.Leave_Days,
  //             }
  //             var url1 = 'LeaveTypeGrade/PostAddleave'

  //             this.service.postparams(url1, param).subscribe((result: any) => {
  //               debugger;
  //               if (result.status == false) {
  //                 this.ToastrService.warning(result.message)

  //               }
  //               else {
  //                 this.reactiveFormadd.get("leave_code")?.setValue(null);
  //                 this.reactiveFormadd.get("leave_name")?.setValue(null);
  //                 this.reactiveFormadd.get("Status_flag")?.setValue(null);
  //                 this.reactiveFormadd.get("weekoff_consider")?.setValue(null);
  //                 this.reactiveFormadd.get("holiday_consider")?.setValue(null);
  //                 this.reactiveFormadd.get("carry_forward")?.setValue(null);
  //                 this.reactiveFormadd.get("Accured_type")?.setValue(null);
  //                 this.reactiveFormadd.get("negative_leave")?.setValue(null);
  //                 this.reactiveFormadd.get("Consider_as")?.setValue(null);
  //                 this.reactiveFormadd.get("Leave_Days")?.setValue(null);

  //                 this.ToastrService.success("Leave Type Added successfully")

  //               }
               
  //             });

  //           }
  //           else {
  //             this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //           }
            
  //   setTimeout(function () {
  //     window.location.reload();
  //   }, 2000); // 2000 milliseconds = 2 seconds
  // }

  ////////Delete popup////////


  onsubmit(){
    debugger
    console.log(this.reactiveFormadd.value)
    this.NgxSpinnerService.show();
    var url = 'LeaveTypeGrade/PostAddleave';
    this.service.post(url, this.reactiveFormadd.value).subscribe((result:any) => {
      if(result.status == true){
      
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
      }
      else {
            
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();           
      }
      this.reactiveFormadd.reset();
      this.ngOnInit();
    })
      
        
  
  }
  openModaldelete(parameter: string) {
    debugger;
    this.parameterValue = parameter

  }
  ondelete() {
    debugger;
    console.log(this.parameterValue);
    var url = 'LeaveTypeGrade/DeleteLeaveType'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }


    });
    setTimeout(function () {
      window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
  }
  toggleInputField() {
    this.showInputField = this.Code_Generation === 'N'; // Show input field only for 'Manual' option
  }


}
