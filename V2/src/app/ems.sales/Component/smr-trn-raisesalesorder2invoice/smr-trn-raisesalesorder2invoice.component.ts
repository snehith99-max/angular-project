import { Component } from '@angular/core';
import { FormBuilder, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-trn-raisesalesorder2invoice',
  templateUrl: './smr-trn-raisesalesorder2invoice.component.html',
  styleUrls: ['./smr-trn-raisesalesorder2invoice.component.scss'],
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
export class SmrTrnRaisesalesorder2invoiceComponent {
  reactiveForm!: FormGroup;
  responsedata: any;
  salesorder_list: any[] = [];
  SOService_list : any [] =[];
  salesproduct_list: any[] = [];
  salesordertype_list: any[] = [];
  getData: any;
  delivery_status:any;
  boolean: any;
  pick: Array<any> = [];
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  salesorder_gid: any;
  
  products: any[] = [];
  company_code: any;
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrTrnSalesordersummary();
    this.GetSmrTrnSalesorderServicesummary ();
  }
  GetSmrTrnSalesordersummary() {
    var url = 'SmrTrnSalesorder/GetSmrTrnSalesorder2invoicesummary'
    this.service.get(url).subscribe((result: any) => {
      $('#salesorder_list').DataTable().destroy();
      this.responsedata = result;
      this.salesorder_list = this.responsedata.salesorder_list;
      setTimeout(() => {
        $('#salesorder_list').DataTable();
      }, 1);


    })


  }

  GetSmrTrnSalesorderServicesummary() {
    var url = 'SmrTrnSalesorder/GetSmrTrnSalesorder2invoiceServicessummary'
    this.service.get(url).subscribe((result: any) => {
      $('#SOService_list').DataTable().destroy();
      this.responsedata = result;
      this.SOService_list = this.responsedata.salesorder_list;
      setTimeout(() => {
        $('#SOService_list').DataTable();
      }, 1);


    })


  }
  RaisetoOrder(salesorder_gid: any){

  var url = "SmrTrnSalesorder/checkdeliveryorderforinvoice";
  let params = {
    salesorder_gid:salesorder_gid
  }
  this.service.getparams(url,params).subscribe((result:any)=>{
    if(result.status!=false){

      const key = 'storyboard';
      const param = salesorder_gid;
      const salesordergid = AES.encrypt(param,key).toString();
      this.router.navigate(['/smr/SmrTrndeliveryToInvoice',salesordergid])
  
    }
    else{
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning(result.message);
    }

  });
  
}


RaisetoOrder1(salesorder_gid: any){

  var url = 'SmrTrnSalesorder/checkinvoice'
  let params={
    salesorder_gid:salesorder_gid
  }
  
  this.service.getparams(url,params).subscribe((result:any)=>{
    debugger
    if(result.status == false){
      window.scrollTo({ top: 0, behavior: 'smooth' });
      this.ToastrService.warning(result.message)
    }
    else{

  const key = 'storyboard';
  const param = salesorder_gid;
  const salesordergid = AES.encrypt(param,key).toString();
  this.router.navigate(['/smr/SmrTrnOrderToInvoice',salesordergid])
    }
  });
}
  
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  back(){
    this.router.navigate(['/smr/SmrTrnInvoiceSummary'])
  }

}
