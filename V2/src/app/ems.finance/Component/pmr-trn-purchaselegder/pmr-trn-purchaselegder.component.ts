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
  selector: 'app-pmr-trn-purchaselegder',
  templateUrl: './pmr-trn-purchaselegder.component.html',
  styleUrls: ['./pmr-trn-purchaselegder.component.scss']
})
export class PmrTrnPurchaselegderComponent {


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
      var url = 'PurchaseLedger/GetPurchaselegder'
      this.NgxSpinnerService.show()
      this.service.get(url).subscribe((result: any) => {
       $('#GetSaleLedger_list').DataTable().destroy();
        this.responsedata = result;
        this.GetSaleLedger_list = this.responsedata.GetPurchaseLedgerfin_list;
        setTimeout(() => {
          $('#GetSaleLedger_list').DataTable()
        }, 1);
        this.NgxSpinnerService.hide()
    
      });
  
    }
    click360(vendorgid: any){
      const key = 'storyboard';
      const param = vendorgid;
      const lspage1 = 'Finance';
      const vendor_gid = AES.encrypt(param,key).toString();
      const lspage = AES.encrypt(lspage1,key).toString();
      this.router.navigate(['/pmr/PmrTrnVendor360',vendor_gid,lspage]);
    }
    onclick(params: any) {
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param, secretKey).toString();
      const lspage1 = 'Finance';
      const lspage = AES.encrypt(lspage1,secretKey).toString();
      this.router.navigate(['/finance/AccRptPurchaseledgerview', encryptedParam])
    }
    OnChangeFinancialYear(){
      this.NgxSpinnerService.show();
      let param = {
        from_date : this.PmrLedgerForm.value.from_date,
        to_date : this.PmrLedgerForm.value.to_date
      }
      var api = 'PurchaseLedger/GetPurchaselegderDatefin';
      this.service.getparams(api,param).subscribe((result: any)=>{
        this.GetSaleLedger_list = result.GetPurchaseLedgerfin_list;
        this.NgxSpinnerService.hide();
      });    
    }
  
  
}