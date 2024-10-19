import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-sys-rpt-auditreport',
  templateUrl: './sys-rpt-auditreport.component.html',
  styleUrls: ['./sys-rpt-auditreport.component.scss'],
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
export class SysRptAuditreportComponent {
  auditreport_list: any[] = [];
  responsedata: any;
  showOptionsDivId: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: ActivatedRoute, public NgxSpinnerService:NgxSpinnerService, private router: Router, public service: SocketService) {}

  ngOnInit(): void {
  this.GetAuditReportSummary();
   
 }

  GetAuditReportSummary() {
  var url = 'SysRptAuditReport/GetAuditReportSummary'
  this.NgxSpinnerService.show();
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.auditreport_list = this.responsedata.auditreport_list;
    setTimeout(() => {
      $('#auditreport_list').DataTable();
      this.NgxSpinnerService.hide();
    },1);
  });
  (error: any) => {
    console.error('Error fetching data', error);
    // Hide the spinner in case of an error
    this.NgxSpinnerService.hide();
  }
}

  openModalhistory(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/system/SysRptAudithistory',encryptedParam])
  }
}
