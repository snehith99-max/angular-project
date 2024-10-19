import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

interface IDePromotion {
  employee_name: string;
  projectname: string;
  reason: string;
  employee_gid: string;
  employee_nameedit: string;
  project_nameedit: string;
  reason_edit: string;
  penality_gid: string;
}

@Component({
  selector: 'app-hrm-trn-depromotedetailssummary',
  templateUrl: './hrm-trn-depromotedetailssummary.component.html',
  styleUrls: ['./hrm-trn-depromotedetailssummary.component.scss']
})
export class HrmTrnDepromotedetailssummaryComponent {
  employee: any;
  employee_edit: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  // showOptionsDivId: any;
  responsedata: any;
  depromotion_list: any[] = [];
  employeelist: any[] = [];
  depromotion!: IDePromotion;
  parameterValue1: any;
  parameterValue: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, private router: Router, public service: SocketService) {
    this.depromotion = {} as IDePromotion;
  }

  ngOnInit(): void {
  this.GetDePromoteSummary();
  this.reactiveForm = new FormGroup({
    employee_name: new FormControl(this.depromotion.employee_name, [Validators.required]),
    projectname: new FormControl(this.depromotion.projectname, [Validators.required, Validators.pattern(/^\S.*$/)]),
    fine_date: new FormControl(this.getCurrentDate()),
    reason: new FormControl(this.depromotion.reason, [Validators.pattern(/^\S.*$/)]),
    penality_gid: new FormControl(''),
    employee_gid: new FormControl(''),
  });
 
  this.reactiveFormEdit = new FormGroup({
    employee_nameedit: new FormControl(this.depromotion.employee_nameedit, [Validators.required]),
    project_nameedit: new FormControl(this.depromotion.project_nameedit, [Validators.required, Validators.pattern(/^\S.*$/)]),
    reason_edit: new FormControl(this.depromotion.reason_edit, [Validators.pattern(/^\S.*$/)]),
    penality_gid: new FormControl(''),
    employee_gid: new FormControl(''),
  });

  // var api='HrmTrnDepromote/GetEmployeeDetail'
  // this.service.get(api).subscribe((result:any)=>{
  // this.employeelist = result.GetEmployeeUserData_Detail;
  // //console.log(this.employeelist)
  // });
}
GetDePromoteSummary() {
  var url = 'HrmTrnDepromote/GetDePromoteSummary'
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.depromotion_list = this.responsedata.DePromotionsummary_list;
    setTimeout(() => {
      $('#depromotion_list').DataTable();
    },);
  });
}



get employee_name() {
  return this.reactiveForm.get('employee_name')!;
}
get project_gid() {
  return this.reactiveForm.get('project_gid')!;
}

get employee_nameedit() {
  return this.reactiveFormEdit.get('employee_nameedit')!;
}
get project_nameedit() {
  return this.reactiveFormEdit.get('project_nameedit')!;
}


public onsubmit(): void {
  if (this.reactiveForm.value.employee_name != null && this.reactiveForm.value.employee_name != '')
  if (this.reactiveForm.value.projectname != null && this.reactiveForm.value.projectname != '') {
    this.reactiveForm.value;
  var url = 'HrmTrnDePromote/PostDePromotion'
  this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.GetDePromoteSummary();
    }
    else {
      this.reactiveForm.get("penality_gid")?.setValue(null);
      this.reactiveForm.get("employee_name")?.setValue(null);
      this.reactiveForm.get("reason")?.setValue(null);
      this.reactiveForm.get("projectname")?.setValue(null);
      this.ToastrService.success(result.message)
      this.GetDePromoteSummary();
    }
  });
}
}

onclose(){
}

onclose1(){
}

getCurrentDate(): string {
  const today = new Date();
  const dd = String(today.getDate()).padStart(2, '0');
  const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
  const yyyy = today.getFullYear();

  return dd + '-' + mm + '-' + yyyy;
}

history(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmTrnDePromotionhistory',encryptedParam])
}

openModaledit(parameter: string) {
  debugger;
  this.parameterValue1 = parameter
  this.reactiveFormEdit.get("employee_nameedit")?.setValue(this.parameterValue1.employee_name);
  this.reactiveFormEdit.get("project_nameedit")?.setValue(this.parameterValue1.projectname);
  this.reactiveFormEdit.get("reason_edit")?.setValue(this.parameterValue1.reason);
  this.reactiveFormEdit.get("penality_gid")?.setValue(this.parameterValue1.penality_gid);
  this.reactiveFormEdit.get("employee_nameedit")?.setValue(this.parameterValue1.employee_gid);
}

// public onupdate(): void {
//   if (this.reactiveFormEdit.value.employee_nameedit != null && this.reactiveFormEdit.value.employee_nameedit != '') {
//     for (const control of Object.keys(this.reactiveFormEdit.controls)) {
//       this.reactiveFormEdit.controls[control].markAsTouched();
//     }
//     this.reactiveFormEdit.value;

//     var url = 'HrmTrnDePromote/getUpdatedDePromotion'
//     this.service.postparams(url, this.reactiveFormEdit.value).pipe().subscribe(result => {
//       this.responsedata = result;
//       if (result.status == false) {
//         this.ToastrService.warning(result.message)
//         this.GetDePromoteSummary();
//       }
//       else {
//         this.ToastrService.success(result.message)
//         this.GetDePromoteSummary();
//       }
//     });
//     setTimeout(function () {
//       window.location.reload();
//     }, 2000);
//   }
// }

 ////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter
}

ondelete(){
  console.log(this.parameterValue);
    var url = 'HrmTrnDePromote/Deletedepromotion'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      $('#depromotion_list').DataTable().destroy();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
        this.GetDePromoteSummary();
      }
      
    });

}

}
