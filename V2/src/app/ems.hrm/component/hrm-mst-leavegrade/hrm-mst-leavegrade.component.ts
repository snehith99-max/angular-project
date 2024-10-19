import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-hrm-mst-leavegrade',
  templateUrl: './hrm-mst-leavegrade.component.html',
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
export class HrmMstLeavegradeComponent {
  // showOptionsDivId: any;
  responsedata:any;
  Leavegrade_list:any[] = [];
  leavegrade_gid: any;
  Leavetype_list:any[] = [];

constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
  }
  ngOnInit(): void { 

    //// Summary Grid//////
var url = 'LeaveGrade/LeaveGradeSummary'
   
this.service.get(url).subscribe((result: any) => {
this.responsedata = result;
this.Leavegrade_list = this.responsedata.leavegrade_list;
  setTimeout(() => {
    $('#Leavegrade_list').DataTable();
    }, );
});
}
Leavegradeadd(){
this.router.navigate(['/hrm/HrmMstAddLeaveGrade'])
}

openModalAssignEmployee(params:any){
  debugger;
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmTrnLeavegradeassign2employee',encryptedParam])
}
openModalUnssignEmployee(params:any){
  debugger;
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/hrm/HrmTrnLeavegradeunassign2employee',encryptedParam])
}
leavetype(data: any): void {
  debugger;
  var api1 = 'LeaveGrade/Getleavetypepopup';

  
  let params = {
    leavegrade_gid: data.leavegrade_gid,
    leavetype_name:data.leavetype_name,
    total_leavecount:data.total_leavecount,
    available_leavecount:data.available_leavecount,

  };

  this.service.getparams(api1, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Leavetype_list = this.responsedata.Leave_typepopup;
  });
}

}
