import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

export class IMakePayReport {
  makepayment_list: string[] = [];
  payment_gid:any;
  bank_name: any;
  payment_date: any;
  bankname_pay : any
  payment_mode: any;
  branch_name: any;
  acct_number: any;
  cheq_number: any;
  user_code:any;
  employee_name:any;
  department_name:any;
  outstanding_amount:any;
  earned_net_salary:any;
}

// interface IMakePaymentReport {
//   bank_name: string;
//   payment_date: string;
//   payment_mode: string;
//   branch_name: string;
//   acct_number: string;
//   cheq_number: string;
//   user_code:string;
//   employee_name:string;
//   department_name:string;
//   outstanding_amount:string;
//   earned_net_salary:string;
// }

@Component({
  selector: 'app-pay-trn-makepayment',
  templateUrl: './pay-trn-makepayment.component.html',
  styleUrls: ['./pay-trn-makepayment.component.scss']
})
export class PayTrnMakepaymentComponent {
  showInput: boolean = false;
  showInput1: boolean = false;

  reactiveForm!: FormGroup;
  selection = new SelectionModel<IMakePayReport>(true, []);
  MakePayReport!: IMakePayReport;
  // paymentmodelist: any[] = [];
  employeebanklist: any[] = [];
  banklist: any[] =[];
  responsedata: any;
  makepayment_list: any[] = [];
  makepayment_list1: any[] = [];
  paymentadd_list: any[] = [];
  monthyear:any;
  payment_mode1:any;
  month:any;
  mdlbankName:any;
  year:any;
  user_gid: any;
  
  
  constructor(private formBuilder: FormBuilder, 
     private route: ActivatedRoute,
     private router: Router,
     private ToastrService: ToastrService,
     public service: SocketService,
     public NgxSpinnerService:NgxSpinnerService,) {
    this.MakePayReport = {} as IMakePayReport;
    }

    ngOnInit(): void 
  {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
      const monthyear = this.route.snapshot.paramMap.get('monthyear');
    this.monthyear = monthyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    const [month, year] = deencryptedParam.split('+');
    this.month = month;
    this.year = year;
    this.onpayment(month, year);
    this.reactiveForm = new FormGroup({
     

            bank_name : new FormControl(this.MakePayReport.bank_name, []),
            bankname_pay: new FormControl (this.MakePayReport.bankname_pay,[]),

            payment_date: new FormControl(this.MakePayReport.payment_date, [Validators.required,]),
              
              payment_mode : new FormControl(this.MakePayReport.payment_mode, [
                Validators.required,
                Validators.minLength(1),
                ]),
                branch_name : new FormControl(this.MakePayReport.branch_name, []),
                acct_number : new FormControl(this.MakePayReport.acct_number, []),
                cheq_number : new FormControl(this.MakePayReport.cheq_number, []),
                monthpayment: new FormControl(''),
                employee_name: new FormControl(''),

                earned_net_salary: new FormControl(''),
                makepayment_list: this.formBuilder.array([]),

        });
       
     
       var api='PayTrnSalaryPayment/GetEmployeeBankDtl'
        this.service.get(api).subscribe((result:any)=>{
        this.employeebanklist = result.getemployeebankdtl;
        //console.log(this.employeebanklist)
       });

       var api='PayTrnSalaryPayment/GetEmployeePayFromBank'
       this.service.get(api).subscribe((result:any)=>{
        this.banklist = result.employeebankdtl;
       });

    }

    onPaymentModeChange(event: any): void {
      this.showTextBox(event); 
  
      const selectedMode = event.target.value;
      if (selectedMode === 'Cash') {
        this.onpayment(this.month, this.year);
      }
    }

  onpayment(month: any,year:any) {
  var url = 'PayTrnSalaryPayment/GetMakePaymentSummary'
  let param = {
    month : month, 
    year : year, 
  }
  this.NgxSpinnerService.show();
  this.service.getparams(url, param).subscribe((result: any) => {
  this.NgxSpinnerService.hide();
  this.makepayment_list = result.makepaymentlist;
  for(let i=0;i<this.makepayment_list.length;i++){
    this.reactiveForm.addControl(`earned_net_salary_${i}`, new FormControl(this.makepayment_list[i].earned_net_salary));
  }
  // setTimeout(() => {
  //   $('#makepayment_list').DataTable();
  //   }, );
    });
}

  showTextBox(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInput = target.value === 'Cheque';
    this.showInput1 = target.value === 'NEFT';
   
   }

  
  
  // Validation
 
  get payment_date() {
    return this.reactiveForm.get('payment_date')!;
  }

  get payment_mode() {
    return this.reactiveForm.get('payment_mode');
  }

  getonchangebankname(){
    debugger
    let bankname = this.reactiveForm.value.bank_name;

    let params={
      bankname:bankname,
      month:this.month,
      year:this.year
    }
    var url="PayTrnSalaryPayment/getonchangebankname";
    this.service.getparams(url,params).subscribe((result:any)=>{
      this.makepayment_list = result.makepaymentlist;
    });
  }
  

  submit() {
    debugger;
    const selectedData = this.selection.selected; 
    if (selectedData.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to payment");
      return;
    } 
    for (const data of selectedData) {
      this.makepayment_list1.push(data);
    } 
   var params={ 
       month: this.month, 
       year: this.year,
       payment_date:this.reactiveForm.value.payment_date,
       payment_type: this.reactiveForm.value.payment_mode,
       bankname_pay: this.reactiveForm.value.bankname_pay,
       bank_name: this.reactiveForm.value.bank_name,
       bank_branch: this.reactiveForm.value.branch_name,
       account_no: this.reactiveForm.value.acct_number,
       cheque_number: this.reactiveForm.value.cheq_number,
       employee_gid: this.reactiveForm.value.employee_name,
       payment_list:this.makepayment_list1
    }
    console.log(params)
    var url = 'PayTrnSalaryPayment/PostMakePayment'; 
      this.service.postparams(url,params).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning('Error While Adding Payment')
       }
       else{
        this.ToastrService.success(result.message)
        this.router.navigate(['/payroll/PayTrnPaymentsummary'])  
       }
      });
      this.selection.clear();
    }
    isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.makepayment_list.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.makepayment_list.forEach((row: IMakePayReport) => this.selection.select(row));
    }
   

 onback(){
  this.router.navigate(['/payroll/PayTrnPaymentsummary']) 
  }

  getPaymentLabel(): string {
    return `${this.month} ${this.year}`;
  }


}