import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, FormArray } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { param } from 'jquery';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';


export class IMakePayReport {
  salaryedit_list: string[] = [];
  payment_gid:any;
  user_code: any;
  payment_date: any;
  employee_name: any;
  branch_name: any;
  department_name: any;
  designation_name: any;
  payment_type:any;
  net_salary:any;
}

@Component({
  selector: 'app-pay-trn-salarypaymentsummaryedit',
  templateUrl: './pay-trn-salarypaymentsummaryedit.component.html',
  styleUrls: ['./pay-trn-salarypaymentsummaryedit.component.scss']
})
export class PayTrnSalarypaymentsummaryeditComponent {
  data:any;
  responsedata: any;
  payment_date:any;
  payment_month:any;
  payment_year:any;
  MakePayReport!: IMakePayReport;
  payment_type:any;
  selection = new SelectionModel<IMakePayReport>(true, []);
  salaryeditform!:FormGroup;
  salaryedit_list:any[]=[];
  salaryedit_list1:any[]=[];

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, private router: ActivatedRoute, private route: Router, public service: SocketService, ) {
    this.MakePayReport = {} as IMakePayReport;


    this.salaryeditform = new FormGroup({
      product_gid : new FormControl(''),
      net_salary: new FormControl(''),
      payment_date: new FormControl('')
    });
  
  }
  ngOnInit(): void {
    
    const options: Options = {
      dateFormat: 'd-m-Y' ,    
    };
    flatpickr('.date-picker', options);
   
    try {
      debugger
    
    const paymentdate = this.router.snapshot.paramMap.get('payment_date');
    const paymenttype = this.router.snapshot.paramMap.get('payment_type');
    const paymentmonth= this.router.snapshot.paramMap.get('payment_month');
    const paymentyear = this.router.snapshot.paramMap.get('payment_year');
    

    this.payment_date= paymentdate;
     this.payment_type= paymenttype;
     this.payment_month= paymentmonth;
     this.payment_year= paymentyear;

     const secretKey = 'storyboarderp';
     const deencryptedParam = AES.decrypt(this.payment_date,secretKey).toString(enc.Utf8);
     const deencryptedParam1 = AES.decrypt(this.payment_type,secretKey).toString(enc.Utf8);
     const deencryptedParam2 = AES.decrypt(this.payment_month,secretKey).toString(enc.Utf8);
     const deencryptedParam3 = AES.decrypt(this.payment_year,secretKey).toString(enc.Utf8);

      

      this.getsalarypaymentedit(deencryptedParam, deencryptedParam1, deencryptedParam2, deencryptedParam3)
      
    } catch (error) {
      console.error("Error decrypting parameter:", error);
    }
}
get net_salary ()
{
  return this.salaryeditform.get('net_salary')!;
}
getsalarypaymentedit(payment_date:any,payment_type:any,payment_month:any,payment_year:any){
debugger

function formatDate(inputDate: any) {
  var dateParts = inputDate.split("-");
  var formattedDate = dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0];
  return formattedDate;
}
var formattedPaymentDate = formatDate(payment_date);
  var url='PayTrnSalaryPayment/getsalarypaymentedit'
  this.NgxSpinnerService.show()
  let params = {
    payment_date : formattedPaymentDate,
    payment_type : payment_type,
    payment_month : payment_month,
    payment_year : payment_year,
  }
  this.service.getparams(url,params).subscribe((result:any)=>{
    this.salaryedit_list = result.salaryedit_list;

    this.salaryeditform.get("payment_gid")?.setValue(this.salaryedit_list[0].payment_gid); 
    this.salaryeditform.get("payment_date")?.setValue(this.salaryedit_list[0].payment_date); 
     
    this.NgxSpinnerService.hide()
  });

}

updatepaymentedit(){
  debugger
  const selectedData = this.selection.selected; 
    if (selectedData.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to payrun");
      return;
    } 
    for (const data of selectedData) {
      this.salaryedit_list1.push(data);
    } 
    const net_salary = this.salaryedit_list[0].net_salary
    var params={ 
      net_salary:net_salary,
      salaryedit_list:this.salaryedit_list1 
   }
   var url='PayTrnSalaryPayment/salarypaymentupdate';
   this.service.postparams(url,params).subscribe((result:any)=>{
    if (result.status == false) {
      this.ToastrService.warning('Error While Updating Payment')
   }
   else{
    this.ToastrService.success("Payment updated Successfully")
    this.route.navigate(['/payroll/PayTrnPaymentsummary'])  
   }

   });
   this.selection.clear();

}
onback(){
  this.route.navigate(['/payroll/PayTrnPaymentsummary'])
}

isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.salaryedit_list.length;
  return numSelected === numRows;
}
masterToggle() {
  this.isAllSelected() ?
    this.selection.clear() :
    this.salaryedit_list.forEach((row: IMakePayReport) => this.selection.select(row));
}
}
