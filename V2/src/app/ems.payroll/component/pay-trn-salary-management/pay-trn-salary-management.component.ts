import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-pay-trn-salary-management',
  templateUrl: './pay-trn-salary-management.component.html',
  styleUrls: ['./pay-trn-salary-management.component.scss'],
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
export class PayTrnSalaryManagementComponent {
  responsedata: any;
  salary_list: any;

  constructor(
       private formBuilder: FormBuilder,
       private route: ActivatedRoute,
       private router: Router,
       private ToastrService: ToastrService,
       public service: SocketService) {   
  }
    
  ngOnInit(): void {
  

       
       var url = 'PayTrnSalaryManagement/GetEmployeeSalaryManagement'
       this.service.get(url).subscribe((result: any) => {
   
         this.responsedata = result;
         this.salary_list = this.responsedata.employeesalarylist;
         setTimeout(() => {
           $('#salary_list').DataTable();
         }, );   
   
       });
     }
    
     mgtEmp(params:any,params1: any,params2: any){
      debugger;
      const secretKey = 'storyboarderp';
      const param = (params+'+'+params1+'+'+params2);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payroll/PayTrnEmployeeselect',encryptedParam]) 
    }
    manageleave(params:any,params1: any,params2: any){
      debugger;
      const secretKey = 'storyboarderp';
      const param = (params+'+'+params1+'+'+params2);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payroll/PayTrnLeavegenarate',encryptedParam]) 
    }
    payrunview(params:any,params1: any,params2: any){
      debugger;
      const secretKey = 'storyboarderp';
      const param = (params+'+'+params1+'+'+params2);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.router.navigate(['/payroll/PayTrnPayrunview',encryptedParam]) 
      
      
    }


}
