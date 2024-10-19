import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup,Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { monthToStr } from 'flatpickr/dist/utils/formatting';
import { ExcelService } from 'src/app/Service/excel.service';

@Component({
  selector: 'app-pay-mst-employeewisepayment',
  templateUrl: './pay-mst-employeewisepayment.component.html',
  styleUrls: ['./pay-mst-employeewisepayment.component.scss']
})
export class PayMstEmployeewisepaymentComponent {
  branch_list:any[]=[];
  Employeewisepaymentform:FormGroup|any;
  employeewisepayment_list:any[]=[];
  responsedata: any;

  constructor(private SocketService: SocketService,
    private excelService : ExcelService,private NgxSpinnerService: NgxSpinnerService,public service: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder) {
  }
  ngOnInit(): void {
    this.Employeewisepaymentform = new FormGroup({ 
      Branch: new FormControl(''),
      Month: new FormControl(''),
      Year: new FormControl(''),
    });

    var api = 'Employeewisepayment/GetBranchdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.Getbranchdropdown;
    });
this.employeewisepaymentsummary()
this.GetEmployeewisepaymentSummary()

}
  
  employeewisepaymentsummary() {
    var api = 'Employeewisepayment/GetEmployeewisepaymentSummary';
    this.service.get(api).subscribe((result: any) => {
      this.employeewisepayment_list = result.Employeewisepaymentlists;
      setTimeout(() => {
        $('#employeewisepayment_list').DataTable();
      }, 1);
  
    });
  }
  GetEmployeewisepaymentSummary(){
    const selectedBranch = this.Employeewisepaymentform.value.Branch || 'null'; 
    const selectyear = this.Employeewisepaymentform.value.Year || 'null';
    const selectmonth = this.Employeewisepaymentform.value.Month || 'null'; 
   
  
    for (const control of Object.keys(this.Employeewisepaymentform.controls)) {
      this.Employeewisepaymentform.controls[control].markAsTouched();
    }
    const params = {
      branch_gid: selectedBranch,  
      year:selectyear,
      month:selectmonth,
     
    };
    const url2 = 'Employeewisepayment/GetEmployeewiseSummarySearch';

    this.service.getparams(url2, params).subscribe((result) => {
      this.responsedata = result;
      this.employeewisepayment_list = this.responsedata.Employeewisepaymentlists;
     
      setTimeout(() => {
        $('#employeewisepayment_list').DataTable();
      }, 1);
    });
  
  }
  exportExcel() {
    debugger
   
 


    const Employeesalaryreport = this.employeewisepayment_list.map(item => ({
      Department: item.Department_Name || '',
      EmployeeCode: item.User_Code || '',
      EmployeeName: item.Employee_Name || '',
      Designation: item.Designation_Name || '',
      Month: item.Month || '',
      Year: item.Year || '',
      PaymentType:item.Payment_Type || '',
      Salary:item.Total_Salary || '',
   
    }));

    this.excelService.exportAsExcelFile(Employeesalaryreport, 'employee_report');

  }
  }

  
