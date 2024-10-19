import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';



@Component({
  selector: 'app-hrm-trn-probationperiod',
  templateUrl: './hrm-trn-probationperiod.component.html',
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
export class HrmTrnProbationperiodComponent {
  // showOptionsDivId: any;
  employee:any[] = [];
  employee_list: any[] = [];
  response_data :any;


  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,) {} 

  ngOnInit(): void {
    var api = 'Probationperiod/GetProbationperiodSummary';
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.employee = this.response_data.employee_list;
      setTimeout(()=>{  
        $('#employee').DataTable();
      }, 1);
    });
  }

  onhistory(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/Probationhistory',encryptedParam]) 
  }

  onleaveupdate(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/Probationleaveupdate',encryptedParam]) 
  }






























}
