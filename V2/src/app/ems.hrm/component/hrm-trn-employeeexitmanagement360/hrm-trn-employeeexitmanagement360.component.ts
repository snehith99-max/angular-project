import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc, format } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ReplaySubject } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-hrm-trn-employeeexitmanagement360',
  templateUrl: './hrm-trn-employeeexitmanagement360.component.html',
  styleUrls: ['./hrm-trn-employeeexitmanagement360.component.scss']
})
export class HrmTrnEmployeeexitmanagement360Component {
  GetLeaveDetails_list: any[] = [];
  Initiateapproval_list: any[] = [];
  GetInitiateApproval_list: any[] = [];
  GetSalaryDetails_list: any[] = [];
  GetEmployee_list: any[] = [];
  GetAddition_list: any[] = [];
  GetOther_list: any[] = [];
  GetEmployeename_list: any[] = [];
  GetAssetCustodian_list: any[] = [];
  GetDeduction_list: any[] = [];
  exitemployee_gid1: any;
  exitemployee_gid: any;
  employee_gid_summary: any;
  salary_gid: any;
  exitform!: FormGroup;
  config = {
    uiColor: '#2599FF !important',
    toolbarGroups: [

      { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
      { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },

      { name: 'styles' },
      { name: 'colors' },

      { name: 'insert' },],
    skin: 'kama',
    resize_enabled: false,
    removePlugins: 'elementspath,save,magicline',
    extraPlugins: 'divarea,emoji,smiley,justify,indentblock,colordialog',

    colorButton_foreStyle: {
      element: 'font',
      attributes: { 'color': '#(color)' }
    },
    height: 188,
    removeDialogTabs: 'image:advanced;link:advanced',
    removeButtons: 'Subscript,Superscript,Anchor,Source,Table',
    format_tags: 'p;h1;h2;h3;pre;div'
  }
  constructor(private route: Router,
    private router: ActivatedRoute,
    public service: SocketService,
    private ToastrService: ToastrService,
  ) { }

  ngOnInit(): void {
    debugger
    const exitemployee_gid1 = this.router.snapshot.paramMap.get('exitemployee_gid1')
    this.exitemployee_gid1 = exitemployee_gid1;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.exitemployee_gid1, secretKey).toString(enc.Utf8);
    this.exitemployee_gid = deencryptedParam;
    this.GetEmployeeDetails(this.exitemployee_gid);
    this.GetLeaveDetails();
    this.GetInitiateApproval(this.exitemployee_gid);
    this.GetInitiateApprovalSummary(this.exitemployee_gid);
    this.GetSalaryDetailsSummary(this.exitemployee_gid);

    this.exitform = new FormGroup({
      editor_content: new FormControl(''),
    });



  }

  GetEmployeeDetails(exitemployee_gid: any) {
    let param = { exitemployee_gid: exitemployee_gid }
    var api = 'ExitManagement/GetEmployeeDetails';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.GetEmployee_list = result.GetEmployee_list;
    });
  }
  GetLeaveDetails() {
    var api1 = 'ExitManagement/GetLeaveDetails';
    this.service.get(api1).subscribe((result: any) => {
      this.GetLeaveDetails_list = result.GetLeaveDetails_list;
    });
  }
  GetInitiateApproval(exitemployee_gid: any) {
    let param = { exitemployee_gid: exitemployee_gid }
    var api2 = 'ExitManagement/GetInitiateApproval';
    this.service.getparams(api2, param).subscribe((result: any) => {
      this.Initiateapproval_list = result.Initiateapproval_list;
    });
  }
  GetInitiateApprovalSummary(exitemployee_gid: any) {
    let param = { exitemployee_gid: exitemployee_gid }
    var api3 = 'ExitManagement/GetInitiateApprovalSummary';
    this.service.getparams(api3, param).subscribe((result: any) => {
      this.GetInitiateApproval_list = result.GetInitiateApproval_list;
    });
  }
  GetSalaryDetailsSummary(exitemployee_gid: any) {
    debugger
    let param = { exitemployee_gid: exitemployee_gid }
    var api4 = 'ExitManagement/GetSalaryDetailsSummary';
    this.service.getparams(api4, param).subscribe((result: any) => {
      this.GetSalaryDetails_list = result.GetSalaryDetails_list || [];
      this.GetAddition_list = result.GetAddition_list || [];
      this.GetDeduction_list = result.GetDeduction_list || [];
      this.GetOther_list = result.GetOther_list || [];
      this.GetEmployeename_list = result.GetEmployeename_list || [];
      this.exitform.get('editor_content')?.setValue(result.GetEmployeename_list[0].template_content);
      this.GetAssetCustodianSummary();
    });
  }
  GetAssetCustodianSummary() {
    debugger
    this.employee_gid_summary = this.GetEmployeename_list[0].employee_gid;
    let param = {
      employee_gid: this.employee_gid_summary,
    }
    var api5 = 'ExitManagement/GetAssetCustodianSummary';
    this.service.getparams(api5, param).subscribe((result: any) => {
      this.GetAssetCustodian_list = result.GetAssetCustodian_list;
    });
  }

  getAdditionsDetails(salary_gid: any) {
    return this.filterBySalaryGid(this.GetAddition_list, salary_gid);
  }

  getDeductionDetails(salary_gid: any) {
    return this.filterBySalaryGid(this.GetDeduction_list, salary_gid);
  }

  getOtherDetails(salary_gid: any) {
    return this.filterBySalaryGid(this.GetOther_list, salary_gid);
  }

  filterBySalaryGid(items: any[], salaryGid: any): any[] {
    return items.filter(item => item.salary_gid === salaryGid);
  }

  OnSumbit() {
    debugger
    var api6 = 'ExitManagement/PostInitiateApproval';
    let param = {
      exitemployee_gid: this.exitemployee_gid,
      manager_name: this.Initiateapproval_list[0].manager_name,
    }
    this.service.post(api6, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
        this.GetInitiateApprovalSummary(this.exitemployee_gid);
      }

    });
  }
  OnAllSumbit() {
    debugger
    var api7 = 'ExitManagement/Post360Submit';
    let param ={
     editor_content : this.exitform.value.editor_content,
     exitemployee_gid: this.exitemployee_gid,
    }
    this.service.post(api7, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
      }
      else {
        window.scrollTo({
          top: 0,
        });        
        this.route.navigate(['/hrm/HrmExitManagment']);
        this.ToastrService.success(result.message);
      }
    });
  }
}

