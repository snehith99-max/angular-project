import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { environment } from 'src/environments/environment.development';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hrm-trn-appointmentorder',
  templateUrl: './hrm-trn-appointmentorder.component.html',
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
export class HrmTrnAppointmentorderComponent {
  // showOptionsDivId: any;
  appointmentorder : any;
  response_data : any;
  company_code: any;
  appointmentorder_gid:any;


  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService, public NgxSpinnerService:NgxSpinnerService) {} 

  ngOnInit(): void {
    var api = 'AppointmentOrder/GetappointmentorderSummary';
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.appointmentorder = this.response_data.appointmentorder_list;
      setTimeout(()=>{  
        $('#appointmentorder').DataTable();
      }, 1);
    });
  }

  onedit(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnAppointmentorderedit',encryptedParam]) 
  }

  onhistory(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/Probationhistory',encryptedParam]) 
  }
  
  PrintPDF(appointmentorder_gid: string) {
    // this.company_code = localStorage.getItem('c_code')
    // window.location.href = "http://" + environment.host + "/Print/EMS_print/hrm_crp_appointmentletter.aspx?appointmentorder_gid=" + appointmentorder_gid + "&companycode=" + this.company_code

    const api = 'AppointmentOrder/GetAppointmentorderRpt';
          this.NgxSpinnerService.show()
          let param = {
            appointmentorder_gid:appointmentorder_gid
          } 
          this.service.getparams(api,param).subscribe((result: any) => {
            if(result!=null){
              this.service.filedownload1(result);
            }
            this.NgxSpinnerService.hide()
          });




  }

}
