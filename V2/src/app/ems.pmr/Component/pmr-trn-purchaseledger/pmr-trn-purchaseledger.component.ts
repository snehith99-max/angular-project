import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-pmr-trn-purchaseledger',
  templateUrl: './pmr-trn-purchaseledger.component.html',
  styleUrls: ['./pmr-trn-purchaseledger.component.scss'],
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
export class PmrTrnPurchaseledgerComponent {


    responsedata:any;
    GetSaleLedger_list:any[]=[];
    PmrLedgerForm!: FormGroup;
  
    constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
    }
  
    ngOnInit(): void {
      this.GetSalesledgersummary();
      const options: Options = {
        dateFormat: 'd-m-Y',
      };
      flatpickr('.date-picker', options);
      this.GetSalesledgersummary();
      this.PmrLedgerForm = new FormGroup({
        from_date: new FormControl(''),
        to_date: new FormControl(''),
      });
    }
  
  
    GetSalesledgersummary(){
      
      var url = 'PmrRptPurchaseLegder/GetPurchaselegderSummary'
      this.NgxSpinnerService.show()
      this.service.get(url).subscribe((result: any) => {
       $('#GetSaleLedger_list').DataTable().destroy();
        this.responsedata = result;
        this.GetSaleLedger_list = this.responsedata.GetSaleLedger_list;
        setTimeout(() => {
          $('#GetSaleLedger_list').DataTable()
        }, 1);
        this.NgxSpinnerService.hide()
    
      });
  
    }
    click360(vendorgid: any){
      
      const key = 'storyboard';
      const param = vendorgid;
      const lspage1 = 'debtorReport';
      const vendor_gid = AES.encrypt(param,key).toString();
      const lspage = AES.encrypt(lspage1,key).toString();
      this.router.navigate(['/pmr/PmrTrnVendor360',vendor_gid,lspage]);
    }
    onclick(invoice_gid_param: any) {
      debugger;
      const Key = 'storyboarderp';
      const param = invoice_gid_param;
      const lspage2 = 'purchaseReport';
      const invoice_gid = AES.encrypt(param, Key).toString();
      const lspage3 = AES.encrypt(lspage2,Key).toString();
      this.router.navigate(['/payable/PmrTrnLedgerInvoiceView', invoice_gid,lspage3])
    }
    OnChangeFinancialYear(){
      
      if(this.PmrLedgerForm.value.from_date != null && this.PmrLedgerForm.value.to_date != ""){ 

      }
      else{
        this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
        return;
      }
      let param = {
        from_date : this.PmrLedgerForm.value.from_date,
        to_date : this.PmrLedgerForm.value.to_date
      }
      this.NgxSpinnerService.show();
      var api = 'PmrRptPurchaseLegder/GetPurchaselegderDate';
      this.service.getparams(api,param).subscribe((result: any)=>{
        this.GetSaleLedger_list = result.GetSaleLedger_list;
        this.NgxSpinnerService.hide();
      });    
    }
  
    onrefreshclick(){
      this.PmrLedgerForm.value.from_date = null!;
      this.PmrLedgerForm.value.to_date = null!;
      this.GetSalesledgersummary();
    }
  
}