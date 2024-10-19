import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
  employee_gid: string;
  branchname: string;
  entityname: string;
  first_name: string;
  user_code: string
  mobile: string;
  user_type:string;

}
@Component({
  selector: 'app-sys-mst-user-edit',
  templateUrl: './sys-mst-user-edit.component.html'
})
export class SysMstUserEditComponent implements OnInit  {
  employee!: IEmployee;
  reactiveForm: FormGroup | any;
  entity_list: any[] = [];
  branch_list: any[] = [];
  Entity: any;
  responsedata: any;
  selectedBranch: any;
  selectedEntity: any;
  entityList: any;
  branchList: any;
  employeegid: any;
  employeeedit_list: any;
  
  @ViewChild('fileInput') fileInput!: ElementRef;
  employee_gid: any;

  constructor(private renderer: Renderer2, private el: ElementRef,public NgxSpinnerService: NgxSpinnerService,public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.employee = {} as IEmployee;
  }
  ngOnInit(): void {
    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    this.employeegid = employee_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employeegid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetViewuserSummary(deencryptedParam)
    this.reactiveForm = new FormGroup({
      first_name: new FormControl(this.employee.first_name, [
        Validators.required,Validators.pattern(/^\S.*$/)
      ]),
      user_code: new FormControl(this.employee.user_code, [
        Validators.required,Validators.pattern(/^\S.*$/)
      ]),
      branchname: new FormControl(this.employee.branchname, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      entityname: new FormControl(this.employee.entityname, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      user_type: new FormControl(this.employee.user_type, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      mobile: new FormControl(this.employee.mobile, [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(15),
      ]),
      employee_gid: new FormControl(''),
    }
    );
    var url = 'Employeelist/Getbranchdropdown';
    this.service.get(url).subscribe((result: any) => {
      this.branchList = result.Getbranchdropdown;
    });
    var api1 = 'Employeelist/Getentitydropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.entity_list = result.Getentitydropdown;
    });
  }
  GetViewuserSummary(employee_gid: any) {
    var url = 'UserManagementSummary/GetViewuserSummary'
    let param = {
      employee_gid : employee_gid 
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employeeedit_list = result.userviewsummary_list;
      console.log(this.employeeedit_list[0].branch_gid)
      this.selectedBranch = this.employeeedit_list[0].branch_gid;
      this.reactiveForm.get("mobile")?.setValue(this.employeeedit_list[0].employee_mobileno);
      this.reactiveForm.get("entityname")?.setValue(this.employeeedit_list[0].entity_name);
      this.selectedEntity = this.employeeedit_list[0].entity_gid;
      this.reactiveForm.get("user_code")?.setValue(this.employeeedit_list[0].user_code);
      this.reactiveForm.get("first_name")?.setValue(this.employeeedit_list[0].user_firstname);
      this.reactiveForm.get("employee_gid")?.setValue(this.employeeedit_list[0].employee_gid);
      this.reactiveForm.get("user_type")?.setValue(this.employeeedit_list[0].user_type);

    });
  }
  get branchname() {
    return this.reactiveForm.get('branchname')!;
  }
  get first_name() {
    return this.reactiveForm.get('first_name')!;
  }
  get user_code() {
    return this.reactiveForm.get('user_code')!;
  }
 get mobile() {
    return this.reactiveForm.get('mobile')!;
  } 
  get user_type() {
    return this.reactiveForm.get('user_type')!;
  } 

  get entityname() {
    return this.reactiveForm.get('entityname')!;
  }
  public validate(): void {
    this.employee = this.reactiveForm.value; 
    if (this.employee.entityname != null && this.employee.branchname != null  && this.employee.first_name != null && this.employee.user_type != null && this.employee.mobile != null) {
      {
        var api7 = 'UserManagementSummary/Updateuserdetails'
        this.NgxSpinnerService.show();
        this.service.post(api7, this.employee).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning("Error while Updating User Detatls")
          }
          else {
            this.route.navigate(['/system/SysMstUsermanagementSummary']);
            this.ToastrService.success("User Updated Successfully")
            this.NgxSpinnerService.hide();
          }
          this.responsedata = result;
        });
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    return;
  }
}

