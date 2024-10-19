import { Component } from '@angular/core';
import { setRef } from '@fullcalendar/core/internal';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-ims-trn-stocktransfer-branch',
  templateUrl: './ims-trn-stocktransfer-branch.component.html',
  styleUrls: ['./ims-trn-stocktransfer-branch.component.scss'],
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
export class ImsTrnStocktransferBranchComponent {

 

    branchaddsummary: any[]=[];
    branch_gid:any;
      constructor(private serivce: SocketService, 
        private NgxSpinnerService: NgxSpinnerService,
        private route: Router,
        public service: SocketService,
        private router : ActivatedRoute
      ){}
    
    
      ngOnInit() : void {
        this.NgxSpinnerService.show();
        var api = 'ImsTrnStockTransferSummary/GetBranchWiseaddSummary';
        this.service.get(api).subscribe((result: any) => {
          this.branchaddsummary = result.branchaddsummary;
          this.NgxSpinnerService.hide();
        });
      }
    
      stocktransfer(params:any){
        debugger;
        const secretKey = 'storyboarderp';
        const param = (params);
        const encryptedParam = AES.encrypt(param,secretKey).toString();
        this.route.navigate(['/ims/ImsTrnStockTransferBranchWise',encryptedParam])
      }
    
  
}
