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
import { NgxSpinnerService } from 'ngx-spinner';

interface ILeaveManageSummaryReport {

}

@Component({
  selector: 'app-hrm-trn-leavemanagesummary',
  templateUrl: './hrm-trn-leavemanagesummary.component.html',
  styleUrls: ['./hrm-trn-leavemanagesummary.component.scss']
})
export class HrmTrnLeavemanagesummaryComponent {
  file!: File;
  reactiveForm!: FormGroup;
  LeaveManageSummaryReport!: ILeaveManageSummaryReport;
  responsedata: any;
  leavemanage_list: any[] = [];
  permission_list: any[] = [];
  onduty_list: any[] = [];
  branchlist: any[] = [];
  departmentlist: any[] = [];
  leavelist: any[] = [];
  leavemanagereport_list: any[] = [];
  type_name: any;
  branch_name: any;
  department_name: any;
  parameterValue: any;
  parameterValue2: any;
  parameterValue3: any;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService,) {
    this.LeaveManageSummaryReport = {} as ILeaveManageSummaryReport;
    }

    ngOnInit(): void {
      this.GetLeaveManageSummary();
      this.GetPermissionSummary();
      this.GetOnDutySummary();

      const options: Options = {
        dateFormat: 'd-m-Y',    
      };
      flatpickr('.date-picker', options);
      this.reactiveForm = new FormGroup({

        branch_company : new FormControl('',[Validators.required]),
        department_data : new FormControl('',[Validators.required]),
        date : new FormControl('',[Validators.required]),
        type: new FormControl('',[Validators.required]),
      });

      var api='HrmTrnLeaveManage/GetBranchDtl'
      this.service.get(api).subscribe((result:any)=>{
      this.branchlist = result.Getbranch_detail;
      
     });
     var api='HrmTrnLeaveManage/GetDepartmentDtl'
     this.service.get(api).subscribe((result:any)=>{
     this.departmentlist = result.Getdepartment_detail;
     
    });



    }

    get branch_company() {
      return this.reactiveForm.get('branch_company')!;
    }
    get department_data() {
      return this.reactiveForm.get('department_data')!;
    }
    get date() {
      return this.reactiveForm.get('date')!;
    }
    get type() {
      return this.reactiveForm.get('type')!;
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

    GetLeaveManage(){
      var url = 'HrmTrnLeaveManage/GetLeaveManage';
      let params={
       branch: this.reactiveForm.value.branch_name,
       department:this.reactiveForm.value.department_name,
       fromdate:this.reactiveForm.value.date,
       leavetype:this.reactiveForm.value.type
      }
      this.service.getparams(url,params).subscribe((result:any)=>{
        this.leavemanage_list = result.leavemanagelist;
      });
      
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

    addleavemanage(){
      this.route.navigate(['/hrm/HrmTrnLeavemanage'])

    }

    importexcel(){
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        window.scrollTo({
          top: 0,
        });
        formData.append("file", this.file, this.file.name);
        var api = 'HrmTrnLeaveManage/PermissionImport'
        this.NgxSpinnerService.show();
        this.service.postfile(api, formData).subscribe((result: any) => {
          this.NgxSpinnerService.hide();
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            
          }
          else {
            this.ToastrService.success(result.message)
           
          }
          this.GetPermissionSummary();
        });
      }
    }

    importexcel1(){
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        window.scrollTo({
          top: 0,
        });
        formData.append("file", this.file, this.file.name);
        var api = 'HrmTrnLeaveManage/LeaveImport'
        this.NgxSpinnerService.show();
        this.service.postfile(api, formData).subscribe((result: any) => {
          this.NgxSpinnerService.hide();
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            
          }
          else {
            this.ToastrService.success(result.message)
           
          }
          this.GetLeaveManageSummary();
        });
      }
    }

    onChange1(event: any): void {
      this.file = event.target.files[0];
    }

    onChange2(event: any): void {
      this.file = event.target.files[0];
    }


    downloadfileformat(){
      debugger;
      let link = document.createElement("a");
      link.download = "Permission Details";
      window.location.href = "http://" + environment.host + "/Templates/Permission Details.xls";
      link.click();
    }

    downloadfileformat1(){
      debugger;
      let link = document.createElement("a");
      link.download = "Leave Details";
      window.location.href = "http://" + environment.host + "/Templates/Leave Details.xls";
      link.click();
    }

}