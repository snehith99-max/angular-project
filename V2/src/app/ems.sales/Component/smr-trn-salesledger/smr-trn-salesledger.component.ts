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
  selector: 'app-smr-trn-salesledger',
  templateUrl: './smr-trn-salesledger.component.html',
  styleUrls: ['./smr-trn-salesledger.component.scss'],
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
export class SmrTrnSalesledgerComponent {

  responsedata:any;
  GetSaleLedger_list:any[]=[];
  SalesLedgerForm!: FormGroup;
  constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.GetSalesledgersummary();
    this.SalesLedgerForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }


  GetSalesledgersummary(){
    debugger
    var url = 'SmrRptSalesLedger/GetsalesLedgersummary'
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
  click360(customergid: any){
    debugger
    const key = 'storyboard';
    const param = customergid;
    const lspage1 = 'sales';
    const customer_gid = AES.encrypt(param,key).toString();
    const lspage = AES.encrypt(lspage1,key).toString();
    this.router.navigate(['/smr/SmrRptSales360',customer_gid,lspage]);
  }
  onclick(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage1 = 'sales';
    const lspage = AES.encrypt(lspage1,secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesLedgerInvoiceView', encryptedParam,lspage])
  }
  OnChangeFinancialYear(){
    this.NgxSpinnerService.show();
    let param = {
      from_date : this.SalesLedgerForm.value.from_date,
      to_date : this.SalesLedgerForm.value.to_date
    }
    var api = 'SmrRptSalesLedger/GetsalesLedgerdate';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.GetSaleLedger_list = result.GetSaleLedger_list;
      this.NgxSpinnerService.hide();
    });    
  }

  onrefreshclick(){
    this.SalesLedgerForm.value.from_date = null!;
    this.SalesLedgerForm.value.to_date = null!;
    this.GetSalesledgersummary();
  }
  }



