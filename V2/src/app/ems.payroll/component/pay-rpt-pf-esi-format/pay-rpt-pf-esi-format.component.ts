import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pay-rpt-pf-esi-format',
  templateUrl: './pay-rpt-pf-esi-format.component.html',
  styleUrls: ['./pay-rpt-pf-esi-format.component.scss'],
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
export class PayRptPfEsiFormatComponent {

  pfformat_list: any[]=[];
  responsedata: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute, private router: Router ) {
  }
  ngOnInit(): void {

    var url = 'PayTrnRptPFandESIFormat/GetPFandESISummary'
    this.service.get(url).subscribe((result: any) => {
     
      this.responsedata = result;
      this.pfformat_list = this.responsedata.PFList_format;
      setTimeout(() => {
        $('#pfformat_list').DataTable()
      }, 1); 
      
  
    });
    
  }

  onpf(params:any,params1: any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params+'+'+params1);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/payroll/PayRptPfReport',encryptedParam])
  }

  onesi(params:any,params1: any){
  const secretKey = 'storyboarderp';
  const param = (params+'+'+params1);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/payroll/PayRptESIReport',encryptedParam])
   }
}
