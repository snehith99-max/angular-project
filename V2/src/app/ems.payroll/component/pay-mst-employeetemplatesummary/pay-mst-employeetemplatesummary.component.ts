import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-pay-mst-employeetemplatesummary',
  templateUrl: './pay-mst-employeetemplatesummary.component.html',
  styleUrls: ['./pay-mst-employeetemplatesummary.component.scss'],
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
export class PayMstEmployeetemplatesummaryComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  salarygrade_list: any[] = [];
  addtional_component: any[] = [];
  responsedata: any;

  constructor(public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    
  }
  ngOnInit(): void {
    var api1='PayMstEmployeesalarytemplate/GetEmployeesalarytemplateSummary'
    
    this.service.get(api1).subscribe((result:any)=>{
   
      this.responsedata=result;
      this.salarygrade_list = this.responsedata.salarygrade_list;  
     console.log(this.salarygrade_list)
      setTimeout(()=>{   
        $('#salarygrade_list').DataTable();
      }, 1);
    
   
  });
  }

  showAdlCompOnEnter(data: any): void {
    debugger;
    var api1 = 'PayMstEmployeesalarytemplate/GetComponentpopup';
    let params = {
        employee2salarygradetemplate_gid: data.employee2salarygradetemplate_gid,
        salarygradetype: "Addition"
    };
    this.service.getparams(api1, params).subscribe((result: any) => {
        this.responsedata = result;
        this.addtional_component = this.responsedata.AddtionalComponent;
    });
}
showAdlCompOnEnterdeduction(data: any): void {
  debugger;
  var api1 = 'PayMstEmployeesalarytemplate/GetComponentpopup';
  let params = {
      employee2salarygradetemplate_gid: data.employee2salarygradetemplate_gid,
      salarygradetype: "Deduction"
  };
  this.service.getparams(api1, params).subscribe((result: any) => {
      this.responsedata = result;
      this.addtional_component = this.responsedata.AddtionalComponent;
  });
}
showAdlCompOnEnterother(data: any): void {
  debugger;
  var api1 = 'PayMstEmployeesalarytemplate/GetComponentpopup';
  let params = {
      employee2salarygradetemplate_gid: data.employee2salarygradetemplate_gid,
      salarygradetype: "Others"
  };
  this.service.getparams(api1, params).subscribe((result: any) => {
      this.responsedata = result;
      this.addtional_component = this.responsedata.AddtionalComponent;
  });
}

onview(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/payroll/PayMstViewEmployeetosalarygrade',encryptedParam]) 
}
onedit(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/payroll/PayMstemp2salarygrade',encryptedParam]) 
}



  onback(){

  }
  
}
