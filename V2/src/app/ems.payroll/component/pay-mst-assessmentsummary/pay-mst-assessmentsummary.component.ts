import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pay-mst-assessmentsummary',
  templateUrl: './pay-mst-assessmentsummary.component.html',
  styleUrls: ['./pay-mst-assessmentsummary.component.scss'],
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
export class PayMstAssessmentsummaryComponent {
  
  response_data: any;
  assessmentsummarylist: any;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit() {
    var api = 'PayMstAssessmentSummary/Getassessmentsummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.assessmentsummarylist = this.response_data.assessmentsummary_list;

      setTimeout(() => {
        $('#assesssum').DataTable();
      }, 1);
    });
  }

  assignemp(params:any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/payroll/PayTrnAssignemployee2form16', encryptedParam])
  }

  generate(params:any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/payroll/PayTrnGenerateform16', encryptedParam])
  }  
}
