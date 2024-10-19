import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
interface IExitRequisitionadd {
  employee_code: string;
  employee_name: string;
  employee_date: string;
  designation: string;
  branch: string;
  department: string;
  apply_date: string;
  relieving_date: string;
  reason: string;
}

@Component({
  selector: 'app-hrm-trn-employeeexitrequisitionadd',
  templateUrl: './hrm-trn-employeeexitrequisitionadd.component.html',
  styleUrls: ['./hrm-trn-employeeexitrequisitionadd.component.scss']
})
export class HrmTrnEmployeeexitrequisitionaddComponent {
  responsedata: any;
  reactiveForm: any;
  employeeexit_list: any;
  selectedBranch: any;
  selectedEmployee: any;
  ExitRequisitionadd!: IExitRequisitionadd;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.ExitRequisitionadd = {} as IExitRequisitionadd;

    this.reactiveForm = new FormGroup({

      employee_code: new FormControl(''),
      employee_name: new FormControl(''),
      employee_date: new FormControl(''),
      branch: new FormControl(''),
      department: new FormControl(''),
      designation: new FormControl(''),
      apply_date: new FormControl(this.getCurrentDate()),
      relieving_date: new FormControl(this.getCurrentDate(),[Validators.required]),
      reason: new FormControl('',[Validators.required]),

    });

    



  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };

    flatpickr('.date-picker', options);


    this.GetExitEmployee();
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
 
    return dd + '-' + mm + '-' + yyyy;
  }

  // Personal Details
  get employee_code() {
    return this.reactiveForm.get('employee_code')!;
  }
  get employee_name() {
    return this.reactiveForm.get('employee_name')!;
  }
  get employee_date() {
    return this.reactiveForm.get('employee_date')!;
  }
  get branch() {
    return this.reactiveForm.get('branch')!;
  }
  get department() {
    return this.reactiveForm.get('department')!;
  }
  get designation() {
    return this.reactiveForm.get('designation')!;
  }
  get apply_date() {
    return this.reactiveForm.get('apply_date')!;
  }
  get relieving_date() {
    return this.reactiveForm.get('relieving_date')!;
  }
  get reason() {
    return this.reactiveForm.get('reason')!;
  }

  GetExitEmployee() {

    var url = 'HrmtrnExitRequisition/GetExitEmployee'
   
    this.service.get(url).subscribe((result: any) => {
    this.responsedata=result;
      this.employeeexit_list = result.GetExitEmployee;
      console.log(this.employeeexit_list)
      console.log(this.employeeexit_list[0].branch_gid)
      debugger;
      this.selectedBranch = this.employeeexit_list[0].branch_gid;
      this.selectedEmployee = this.employeeexit_list[0].user_gid;
      this.reactiveForm.get("employee_code")?.setValue(this.employeeexit_list[0].user_code);
      this.reactiveForm.get("employee_name")?.setValue(this.employeeexit_list[0].employee_name);
      this.reactiveForm.get("employee_date")?.setValue(this.employeeexit_list[0].employee_joiningdate);
      this.reactiveForm.get("branch")?.setValue(this.employeeexit_list[0].branch_name);
      this.reactiveForm.get("department")?.setValue(this.employeeexit_list[0].department_name);
      this.reactiveForm.get("designation")?.setValue(this.employeeexit_list[0].designation_name);
      

      this.reactiveForm.get("user_gid")?.setValue(this.employeeexit_list[0].user_gid);
    
     
    });
  }


  onconfirm(){
    var url = 'HrmtrnExitRequisition/PostExitRequisition'
    this.NgxSpinnerService.show();
  
    this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
      debugger;
      
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.router.navigate(['/hrm/HrmTrnEmployeeexitrequisitionsummary']);
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
    
    });
  }

  oncancel(){
    this.router.navigate(['/hrm/HrmTrnEmployeeexitrequisitionsummary'])
  }

}
