import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
interface IPromotionadd {
  loan_gid: string;
  employee_gid: string;
  branch_name: string;
  current_department: string;
  current_designation: string;
  branch_detail: string;
  department_detail: string;
  designation_detail: string;
  effective_date: string;
  reason: string;
}

@Component({
  selector: 'app-hrm-trn-promotionadd',
  templateUrl: './hrm-trn-promotionadd.component.html',
  styleUrls: ['./hrm-trn-promotionadd.component.scss']
})
export class HrmTrnPromotionaddComponent {
  parameterValue: any;
  employeedetail_list: any[] = [];
  Promotionadd!: IPromotionadd;
  reactiveForm: any;
  employeelist: any;
  branchlist: any;
  departmentlist: any;
  designationlist: any;
  responsedata: any;
  employeedata: any;
  showEmployeeDetail: boolean = false;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService){
    this.Promotionadd = {} as IPromotionadd;
    this.reactiveForm = new FormGroup({
      employeegid: new FormControl({ value: '', disabled: false },[Validators.required]),
      branch_detail: new FormControl(this.Promotionadd.branch_detail, [Validators.required,Validators.pattern(/^\S.*$/)]),
      department_detail: new FormControl(this.Promotionadd.department_detail, [Validators.required,Validators.pattern(/^\S.*$/)]),
      designation_detail: new FormControl(this.Promotionadd.designation_detail, [Validators.required,Validators.pattern(/^\S.*$/)]), 
      branch_name: new FormControl(''),
      department_name: new FormControl(''),
      designation_name: new FormControl(''),
      effective_date: new FormControl(this.getCurrentDate()),
      reason: new FormControl('',[Validators.pattern(/^\S.*$/)]),

    });
  }

  ngOnInit(): void {

    var api='HrmTrnPromotionManagement/GetEmployeeNameDtl'
    this.service.get(api).subscribe((result:any)=>{
    this.employeelist = result.GetEmployee_Dtl;
   });
   var api='HrmTrnPromotionManagement/GetBranchNameDtl'
   this.service.get(api).subscribe((result:any)=>{
   this.branchlist = result.GetBranch_Dtl;
  });
  var api='HrmTrnPromotionManagement/GetDepartmentNameDtl'
   this.service.get(api).subscribe((result:any)=>{
   this.departmentlist = result.GetDepartment_Dtl;
  });
  var api='HrmTrnPromotionManagement/GetDesignationNameDtl'
   this.service.get(api).subscribe((result:any)=>{
   this.designationlist = result.GetDesignation_Dtl;
  });


  }

  employeedetail(event: any): void {
    if (event) {
      this.showEmployeeDetail = true;
    } else {
      this.showEmployeeDetail = false;
    }

    let employee_gid = this.reactiveForm.get('employeegid')?.value;

    let param = {
      employee_gid: employee_gid
    }
  
  var url = 'HrmTrnPromotionManagement/GetEmployeeDetail'
 
  this.service.getparams(url, param).subscribe((result: any) => {
  this.responsedata=result;
    this.employeedetail_list = result.GetEmployeeData_Detail;
    this.reactiveForm.get("branch_name")?.setValue(this.employeedetail_list[0].branch_name);
    this.reactiveForm.get("department_name")?.setValue(this.employeedetail_list[0].department_name);
    this.reactiveForm.get("designation_name")?.setValue(this.employeedetail_list[0].designation_name);
  });
  
}

  get employeegid() {
    return this.reactiveForm.get('employeegid')!;
  }
  get branch_detail() {
    return this.reactiveForm.get('branch_detail')!;
  }
 
  get department_detail() {
    return this.reactiveForm.get('department_detail')!;
  }

  get designation_detail() {
    return this.reactiveForm.get('designation_detail')!;
  }
  
  
 onconfirm(): void {
  var url = 'HrmTrnPromotionManagement/PostPromotion'
  this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
    
    if (result.status == false) {
      this.ToastrService.warning(result.message)
    }
    else {
      this.router.navigate(['/hrm/HrmTrnPromotionsummary']);
      this.ToastrService.success(result.message)
    }
  });

 
 }

 

  oncancel(){
    this.router.navigate(['/hrm/HrmTrnPromotionsummary']);
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
 
    return dd + '-' + mm + '-' + yyyy;
  }

}
