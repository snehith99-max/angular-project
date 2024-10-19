import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-hrm-mst-holidaygrade-management',
  templateUrl: './hrm-mst-holidaygrade-management.component.html',
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
export class HrmMstHolidaygradeManagementComponent {
  responsedata:any;
  // showOptionsDivId: any;
  Holidaygrade_list1:any[] = [];
  parameterValue: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
  }
  ngOnInit(): void { 
    debugger

    //// Summary Grid//////
var url = 'HolidayGradeManagement/HolidayGradeSummary'
   
this.service.get(url).subscribe((result: any) => {
this.responsedata = result;
this.Holidaygrade_list1 = this.responsedata.holidaysummary_list;
  setTimeout(() => {
    $('#Holidaygrade_list1').DataTable();
    }, );
});
}

HolidayGrade(){
  this.router.navigate(['/hrm/HrmMstAddHolidaygrademanagement'])
}
Holidayassign(){
  this.router.navigate(['/hrm/HrmTrnAddHolidayAssign'])
}

view(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmMstAssignedholidaygradeview',encryptedParam]) 
}

openModaldelete(parameter: string) {
  this.parameterValue = parameter
}
Assign(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmMstHolidayAssignEmployee',encryptedParam]) 
} 
Unassign(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmMstHolidayUnAssignEmployee',encryptedParam]) 
}
edit(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmMstEditHolidayGrade',encryptedParam]) 
}
}
