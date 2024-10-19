import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import * as CryptoJS from 'crypto-js';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-acc-trn-make-payment',
  templateUrl: './acc-trn-make-payment.component.html',
  styleUrls: ['./acc-trn-make-payment.component.scss']
})
export class AccTrnMakePaymentComponent {
  reactiveform: FormGroup | any;
  payment: any;
  decryptedParam: any;
  decryptedParam1: any;
  RecordSummary_list:any;

  private secretKey = 'storyboarderp';
  accountgroup_list = [
    {
      accountgroup_name: 'Bank',
    },
    {
      accountgroup_name: 'Cash',
    },
  ]

  subTable_list = [
    {
      accountgroup_Group: 'tesitng',
      accountgroup_Name: 'Al-Ain',
      Amount: '14,089',
      Remarks: 'the payments details is added ',
      Claim_date: '22-06-2024',
    },
    {
      accountgroup_Group: 'mobirik',
      accountgroup_Name: 'manu',
      Amount: '20,089',
      Remarks: 'the payments details is added ',
      Claim_date: '22-06-2024',
    },
  ]
  responsedata: any;
  Make_payment_list: any;
  Make_Payment_list: any;

  constructor(private route: ActivatedRoute,private formBuilder: FormBuilder, private router: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.reactiveform = new FormGroup({
      Select_payment: new FormControl(null,),
      Bank_name: new FormControl('',),
      Trn_No: new FormControl('',),
      Bank_Payment_Date: new FormControl('',),
    
    })

    

  }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const encryptedParam = params['encryptedParam'];
      const secretKey = 'storyboarderp';
      const bytes = CryptoJS.AES.decrypt(encryptedParam, secretKey);
      const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
      this.decryptedParam = JSON.parse(decryptedData);
      console.log(this.decryptedParam);
    });

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.onview()

    
  }

  GetBankMasterSummary() {
    var url = 'AccTrnRecordExpenseSummary/MakePaymentdetails'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.Make_payment_list = this.responsedata.Make_payment_list;
      // setTimeout(() => {
      //   $('#bankmaster_list').DataTable();
      // }, 1);
    });
  }

  onupdate() {
    this.NgxSpinnerService.show();
    const param = {
      payment_mode: this.reactiveform.get('Select_payment').value || null,
      editbank_name: this.reactiveform.get('Bank_name').value || null,
      transaction_number: this.reactiveform.get('Trn_No').value || null,
      payment_date: this.reactiveform.get('Bank_Payment_Date').value || null,
      expenserequisition_gid: this.decryptedParam
    }

    var url = 'AccTrnRecordExpenseSummary/UpdateMakePayment'

    this.service.post(url,param).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.router.navigate(['/finance/AccTrnRecordExpense']);
      }
     
  }); 
  
    
  }

  onupdate1(){
    this.NgxSpinnerService.show();
    const param = {
      payment_mode: this.reactiveform.get('Select_payment').value || null,
      payment_date: this.reactiveform.get('Bank_Payment_Date').value || null,
      expenserequisition_gid: this.decryptedParam
     }

     var url = 'AccTrnRecordExpenseSummary/UpdateMakePayment'

     this.service.post(url,param).pipe().subscribe((result:any)=>{
       this.responsedata=result;
       if(result.status ==false){
         this.ToastrService.warning(result.message)
         this.NgxSpinnerService.hide();
       }
       else{
         this.ToastrService.success(result.message)
         this.NgxSpinnerService.hide();
         this.router.navigate(['/finance/AccTrnRecordExpense']);
       }
      
   }); 
  
    
  }

  onview() {
    this.NgxSpinnerService.show();
    let param = {
      expenserequisition_gid:this.decryptedParam
    }

    var url = 'AccTrnRecordExpenseSummary/MakePaymentdetails';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.RecordSummary_list = this.responsedata.Make_Payment_Group_list;
      this.Make_Payment_list = this.responsedata.Make_Payment_list;
      this.NgxSpinnerService.hide();
    })
  }


}
