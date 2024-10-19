import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-crm-mst-customer',
  templateUrl: './crm-mst-customersummary.component.html',
  styleUrls: ['./crm-mst-customersummary.component.scss']
})

export class CrmMstCustomerSummaryComponent {
  private unsubscribe: Subscription[] = []; // Read more: => https://brianflove.com/2016/12/11/anguar-2-unsubscribe-observables/
  customerlist: any[] = [];
  response_data :any;
  parameterValue: any; 
  responsedata:any;

  constructor(private fb: FormBuilder, private router: Router, private service: SocketService, private ToastrService: ToastrService) { }

  ngOnInit(): void {
    var api = 'EinvoiceCustomer/GetCustomerSummary';
    this.service.get(api).subscribe((result:any) => {
      this.response_data = result;
      this.customerlist = this.response_data.CustomerSummary_list;       
      setTimeout(()=>{  
        $('#customersummary').DataTable();
      }, 1);
    });  
  }

  onedit(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/einvoice/CrmMstCustomerEdit',encryptedParam])
  }

  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/einvoice/CrmMstCustomerview',encryptedParam]) 
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'EinvoiceCustomer/Deletecustomer'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if ( result.status == false) {
       this.ToastrService.warning(result.message)
      }
      else {
       this.ToastrService.success(result.message)
        }
        // window.location.reload();
    });
  }  
  

  onadd()
  {
    this.router.navigate(['/einvoice/CrmMstCustomerAdd']);
  }
}


