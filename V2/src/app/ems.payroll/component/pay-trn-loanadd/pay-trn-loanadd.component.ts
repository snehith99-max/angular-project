import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';
interface ILoanadd {
  loan_gid: string;
  employee_gid: string;
  employee: string;
  loan_name: string;
  loan_amount: string;
  remarks: string;
  loan_advance: string;
  date: string;
  cheque_no: string;
  bank_name: string;
  bank: string;
  transaction_refno: string;
  branch_name: string;
  duration_period: string;
}
@Component({
  selector: 'app-pay-trn-loanadd',
  templateUrl: './pay-trn-loanadd.component.html',
  styleUrls: ['./pay-trn-loanadd.component.scss']
})
export class PayTrnLoanaddComponent {
  showInput: boolean = false;
  showInput1: boolean = false;
  showInput2: boolean = false;
  showInput3: boolean = false;
  showInput4: boolean = false;
  showInput5: boolean = false;
  inputValue: string = ''
  reactiveForm: any;
  employeelist: any[] = [];
  loan_refno: any; 
  employeegid:any;

  bankdetailslist: any[] = [];


 

 
  Loanadd!: ILoanadd;
  repayamtpermonth: number = 0;
  durationperiod: number = 0;
  loanamount:number=0
  paymentmode: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,public NgxSpinnerService:NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.Loanadd = {} as ILoanadd;
    this.reactiveForm = new FormGroup({
    
      loan_refno: new FormControl(''),
      loan_name: new FormControl(this.Loanadd.loan_name, [Validators.required,Validators.pattern(/^\S.*$/)]),
      loan_amount: new FormControl(this.Loanadd.loan_amount, [Validators.required,Validators.pattern(/^\S.*$/)]),
      loan_advance: new FormControl(this.Loanadd.loan_advance, []), 
      date: new FormControl(this.Loanadd.date, []), 
      duration_period: new FormControl(this.Loanadd.duration_period, [Validators.required,Validators.pattern(/^\S.*$/)]),
      branch_name: new FormControl(this.Loanadd.branch_name, [Validators.pattern(/^\S.*$/)]),
      transaction_refno: new FormControl(this.Loanadd.transaction_refno, []), 
      type: new FormControl('loan'),
      repay_amt: new FormControl(''),
      repaymentstartdate: new FormControl('',[Validators.required]),
      repayamtpermonth: new FormControl(''),
      remarks: new FormControl('',[Validators.pattern(/^\S.*$/)]),
      employee : new FormControl(this.Loanadd.employee, []),
      bank : new FormControl(this.Loanadd.bank, []),
      
          loan_gid: new FormControl(''),
          employee_gid: new FormControl(''),
          payment_mode: new FormControl('',[Validators.required]),
          bank_name: new FormControl('',[Validators.pattern(/^\S.*$/)]),
          payment_date: new FormControl(''),

          cheque_no: new FormControl('',[Validators.pattern(/^\d{6}$/),Validators.pattern(/^\S.*$/)]),
          transaction_no: new FormControl('',[Validators.pattern(/^\S.*$/)]),
          card_name: new FormControl(''),
          dd_no: new FormControl(''),
         

    });
  }
  pendingamountcalc() {
    this.repayamtpermonth = this.loanamount / this.durationperiod;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };

    flatpickr('.date-picker', options);
 
  
        
        var api='PayTrnLoanSummary/GetEmployeeDtl'
        this.service.get(api).subscribe((result:any)=>{
          debugger;
        this.employeelist = result.GetEmployeeDtl;
        this.loan_refno = result.loan_refno;
        this.reactiveForm.get("loan_refno")?.setValue(this.loan_refno);
        //console.log(this.employeelist)
        });
 
        var api='PayTrnLoanSummary/GetBankDetail'
        this.service.get(api).subscribe((result:any)=>{
        this.bankdetailslist = result.GetBankNameDtl;
        //console.log(this.bankdetailslist)
        });
 
      }

    
    get employee() {
      return this.reactiveForm.get('employee')!;
    }
    get loan_name() {
      return this.reactiveForm.get('loan_name')!;
    }
   
    get loan_amount() {
      return this.reactiveForm.get('loan_amount')!;
    }
  
    get payment_mode() {
      return this.reactiveForm.get('payment_mode')!;
    }
    get repaymentstartdate() {
      return this.reactiveForm.get('repaymentstartdate')!;
    }
    get remarks() {
      return this.reactiveForm.get('remarks')!;
    }
    get loan_advance() {
      return this.reactiveForm.get('loan_advance')!;
    }
    get date() {
      return this.reactiveForm.get('date')!;
    }

    get cheque_no() {
      return this.reactiveForm.get('cheque_no')!;
    }
    
    get bank_name() {
      return this.reactiveForm.get('bank_name')!;
    }

    get branch_name() {
      return this.reactiveForm.get('branch_name')!;
    }
    get bank() {
      return this.reactiveForm.get('bank')!;
    }
    get transaction_refno() {
      return this.reactiveForm.get('transaction_refno')!;
    }
    get duration_period() {
      return this.reactiveForm.get('duration_period')!;
    }


 onconfirm(): void {

  var api7 = 'PayTrnLoanSummary/PostLoan'
  this.NgxSpinnerService.show();

  this.service.post(api7, this.reactiveForm.value).subscribe((result: any) => {
    debugger;
    
    if (result.status == false) {
      this.ToastrService.warning('Error While Adding Loan')
      this.NgxSpinnerService.hide();
    }
    else {
      this.router.navigate(['/payroll/PayTrnLoansummary']);
      this.ToastrService.success('Loan Added Successfully')
      this.NgxSpinnerService.hide();
    }
  });

 
 }

 oncancel()
  {
    this.router.navigate(['/payroll/PayTrnLoansummary']);
  }


  showTextBox(selectedValue: string) {
    this.showInput = selectedValue === 'Cash';
    this.showInput1 = selectedValue === 'Cheque';
    this.showInput2 = selectedValue === 'DD';
    this.showInput3 = selectedValue === 'NEFT';
}


}
