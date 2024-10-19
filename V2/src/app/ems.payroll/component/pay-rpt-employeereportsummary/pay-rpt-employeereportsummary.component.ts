import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';

interface ISearchD {
  department_name: string;
  
}
interface ISearchB {
  branch_name: string;
  
}


@Component({
  selector: 'app-pay-rpt-employeereportsummary',
  templateUrl: './pay-rpt-employeereportsummary.component.html',
  styleUrls: ['./pay-rpt-employeereportsummary.component.scss']
})
export class PayRptEmployeereportsummaryComponent {
  branch!: ISearchB;
  department!: ISearchD;
  branch_name: any;
  department_name: any;
  responsedata: any;
  employeehistory_list : any[] = [];
  reactiveForm!: FormGroup;
  branch_list : any[] = [];
  department_list : any[] = [];
  selectedDepartment: any = null;
  selectedBranch: any = null;
 

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private route:Router, private excelService : ExcelService,) {
    this.branch = {} as ISearchB;
    this.department = {} as ISearchD;
    }

    ngOnInit(): void {
debugger;
      const options: Options = {
        dateFormat: 'd-m-Y',    
      };
      flatpickr('.date-picker', options);

      this.department_list = [{ department_name: 'All', department_gid: 'all' }, ...this.department_list];

      this.reactiveForm = new FormGroup({
        branch_name : new FormControl(''),
        department_name : new FormControl(''),
        department_gid: new FormControl(this.department.department_name, [ Validators.required,]),
        branch_gid: new FormControl(this.branch.branch_name, [ Validators.required,]),
      });
      
      var url = 'PayRptEmployeeHistory/GetBranchDetail';
    this.service.get(url).subscribe((result: any) => {
    this.branch_list = result.GetBranchDetail;     
    this.branch_list = [{ branch_name: 'All', branch_gid: 'all' }, ...this.branch_list];
    
   
        });
        const params = {
          branch_name: "all",
          department_name: "all"
        };
       
        const url2 = 'PayRptEmployeeHistory/GetEmployeeHistory'
        this.service.getparams(url2, params).subscribe((result: any) => {
    
          this.responsedata = result;
          this.employeehistory_list = this.responsedata.employeehistory_list;
          setTimeout(() => {
            $('#employeehistory_list').DataTable();
          }, 1);
    
    
        });

    }


    onBranchChange(branch_gid: any) {
      debugger;
      const branchValue = this.reactiveForm.get('branch_name')!.value;
    
      if (branchValue === 'all') {
        this.department_list = [{ department_name: 'All', department_gid: 'all' }];
      } else {
        var url1 = 'PayRptEmployeeHistory/GetDepartmentDetail';
      let param: { branch_gid: any } = {
        branch_gid: branch_gid
    };
      this.service.getparams(url1, param).subscribe((result: any) => {
        debugger;
        this.department_list = [{ department_name: 'All', department_gid: 'all' }];
        this.department_list.push(...result.GetDepartmentDtl); // Add the data to the existing array 
      });
      }
      this.GetEmployeeReportSummary();
    }
    

    openModalview(params: any){
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.route.navigate(['/payroll/PayRptEmployeepaymentdetailsview',encryptedParam])
    }

    GetEmployeeReportSummary() {
debugger;
      let selectedBranch = this.reactiveForm.value.branch_name;
      let selectedDepartment = this.reactiveForm.value.department_name;
    
      if(selectedBranch==null || selectedBranch==""){
        selectedBranch='all'
      }
      if(selectedDepartment==null || selectedDepartment==""){
        selectedDepartment='all'
      }
    
        const params = {
          branch_name: selectedBranch,
          department_name: selectedDepartment
        };
        const url2 = 'PayRptEmployeeHistory/GetEmployeeHistory'
      this.service.getparams(url2, params).subscribe((result: any) => {
  
        this.responsedata = result;
        this.employeehistory_list = this.responsedata.employeehistory_list;
        setTimeout(() => {
          $('#employeehistory_list').DataTable();
        }, 1);
  
  
      });
    }

    exportExcel() :void {

      const Employeereport  = this.employeehistory_list.map(item => ({
        Branch: item.branch_name || '', 
        Department: item.department_name || '',
        EmployeeCode: item.user_code || '',
        EmployeeName: item.employee_name || '',
        Designation: item.designation_name || '',
        EmployeeJoiningDate: item.employee_joiningdate || '',
        TotalSalary: item.total_salary || '',

      }));
    
            
            this.excelService.exportAsExcelFile(Employeereport , 'Employee_Report ');
    }

}
