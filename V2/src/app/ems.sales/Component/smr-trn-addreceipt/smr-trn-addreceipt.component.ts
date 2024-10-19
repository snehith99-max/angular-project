import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { environment } from '../../../../environments/environment.development';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-smr-trn-addreceipt',
  templateUrl: './smr-trn-addreceipt.component.html',
  styleUrls: ['./smr-trn-addreceipt.component.scss'],
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
export class SmrTrnAddreceiptComponent {
  addreceipt : any[]=[];
  response_data : any;


  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    
    var api = 'SmrReceipt/GetAddReceiptSummary';
    this.NgxSpinnerService.show();

    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.addreceipt = this.response_data.receiptaddsummary_list;
      setTimeout(() => {
        $('#addreceipt').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide();

  }

  
  makereceipt(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnInvoiceReceipt', encryptedParam])
  }

}
