import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
interface ILoanview {
  repayment_duration: string;
  repayment_gid: string;
  repayment_remarks: string;
}

@Component({
  selector: 'app-pay-trn-loanview',
  templateUrl: './pay-trn-loanview.component.html',
  styleUrls: ['./pay-trn-loanview.component.scss']
})
export class PayTrnLoanviewComponent {
  loanview!: ILoanview;
  loan: any;
  repayment: any;
  parameterValue1: any;
  ViewLoanSummary_list:any [] = [];
  loanrepay_list:any [] = [];
  reactiveFormEdit!: FormGroup;
  loan_gid: any;

  responsedata: any;

  constructor(private formBuilder: FormBuilder,private ToastrService: ToastrService, route:Router,private router:ActivatedRoute,public service :SocketService) {
    this.loanview= {} as ILoanview;
   }

  ngOnInit(): void {
    
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    
    flatpickr('.date-picker', options);

   const loan_gid =this.router.snapshot.paramMap.get('loan_gid');
    this.loan = loan_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.loan,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.getViewLoanSummary(deencryptedParam);
    this.loan_gid=deencryptedParam;
    this.getRepaymentLoanSummary(deencryptedParam);


    this.reactiveFormEdit = new FormGroup({
      repayment_duration: new FormControl(this.loanview.repayment_duration, [Validators.required,]),
      repayment_gid: new FormControl(this.loanview.repayment_gid, [Validators.pattern(/^\S.*$/)]),
      repayment_remarks: new FormControl(this.loanview.repayment_remarks, [Validators.required,Validators.pattern(/^\S.*$/)]),
      loan_gid: new FormControl(''),
      // repayment_gid: new FormControl(''),
    
    });

  }
  getViewLoanSummary(loan_gid: any) {
    var url='PayTrnLoanSummary/getViewLoanSummary'
    let param = {
      loan_gid : loan_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.ViewLoanSummary_list = result.getViewLoanSummary;   
    });
}

get repayment_duration() {
  return this.reactiveFormEdit.get('repayment_duration')!;
}

get repayment_gid() {
  return this.reactiveFormEdit.get('repayment_gid')!;
}

get repayment_remarks() {
  return this.reactiveFormEdit.get('repayment_remarks')!;
}


openModaledit(parameter: string){
  debugger;
  this.parameterValue1 = parameter
  this.reactiveFormEdit.get("repayment_duration")?.setValue(this.parameterValue1.repayment_duration);
  this.reactiveFormEdit.get("repayment_gid")?.setValue(this.parameterValue1.repayment_gid);
  this.reactiveFormEdit.get("repayment_remarks")?.setValue(this.parameterValue1.repayment_remarks);
}

onupdate(): void {
  debugger;
  if (this.reactiveFormEdit.value.repayment_duration != null && this.reactiveFormEdit.value.repayment_duration != '') {
    for (const control of Object.keys(this.reactiveFormEdit.controls)) {
      this.reactiveFormEdit.controls[control].markAsTouched();
    }
  
  let param= {
    repayment_duration: this.reactiveFormEdit.value.repayment_duration,
    repayment_gid: this.reactiveFormEdit.value.repayment_gid,
    repayment_remarks: this.reactiveFormEdit.value.repayment_remarks
  }

  this.reactiveFormEdit.value;
  var url = 'PayTrnLoanSummary/getUpdatedmonth'

  this.service.postparams(url, param).pipe().subscribe((result: { status: boolean; message: string | undefined; }) => {
            this.responsedata = result;
            if (result.status == false) {
              this.ToastrService.warning('Error While Updating Due Date')
              
            }
            else {
              this.ToastrService.success('Due Date Updated Successfully')
              
            }

         
                // Display a notification
                // new Notification("Due Date Updated Successfully", {});
    
                // Reload the page after a short delay (you can adjust the delay as needed)
                setTimeout(function() {
                    window.location.reload();
                }, 2000); // 2000 milliseconds = 2 seconds
       

          });
        }
}

onclose(){

}

getRepaymentLoanSummary(loan_gid: any) {
  var url='PayTrnLoanSummary/getLoanrepaySummary'
  let param = {
    loan_gid : loan_gid 
  }
  this.service.getparams(url,param).subscribe((result:any)=>{
  this.responsedata=result;
  this.loanrepay_list = result.getRepayViewLoanSummary;   
  });
}
}
