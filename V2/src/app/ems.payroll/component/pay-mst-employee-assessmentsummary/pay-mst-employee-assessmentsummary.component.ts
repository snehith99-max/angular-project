import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pay-mst-employee-assessmentsummary',
  templateUrl: './pay-mst-employee-assessmentsummary.component.html',
  styleUrls: ['./pay-mst-employee-assessmentsummary.component.scss'],
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

export class PayMstEmployeeAssessmentsummaryComponent {

  response_data: any;
  employeeassessmentsummarylist: any = [];

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeAssessmentSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.employeeassessmentsummarylist = this.response_data.employeeassessmentsummary_list;

      setTimeout(() => {
        $('#financial_list').DataTable();
      }, 1);
    });
  }  

  fillform(params: any) {
    debugger;
    const secretKey = 'storyboarderp';

    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/payroll/PayMstEmployeeform16details', encryptedParam])
  }

  back() {
    this.router.navigate(['/payroll/PayMstEmpAssessmentsummary'])
  }

//   PrintPDF(assessment_gid: string,employee_gid: string) {
  
//     const api = 'PayMstAssessmentSummary/GetTdsPDF';
//           this.NgxSpinnerService.show()
//           let param = {
//             assessment_gid:assessment_gid,
//             employee_gid:employee_gid
//           } 
//           this.service.getparams(api,param).subscribe((result: any) => {
//             if(result!=null){
//               this.service.filedownload1(result);
//             }
//             this.NgxSpinnerService.hide()
//           });
// }
}