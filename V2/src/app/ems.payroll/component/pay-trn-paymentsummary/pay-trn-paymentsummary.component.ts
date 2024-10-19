import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExcelService } from 'src/app/Service/excel.service';

interface IPaymentReport {

}

@Component({
  selector: 'app-pay-trn-paymentsummary',
  templateUrl: './pay-trn-paymentsummary.component.html',
  styleUrls: ['./pay-trn-paymentsummary.component.scss'],
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
export class PayTrnPaymentsummaryComponent {
  reactiveFormSubmit!: FormGroup;
  PaymentReport!: IPaymentReport;
  responsedata: any;
  payment_list: any[] = [];
  expandedRows: any[] = [];
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }
  expandedRows1: any[] = [];
  toggleExpansion1(index: number) {
    this.expandedRows1[index] = !this.expandedRows1[index];
  }
  paymentadd_list: any[] = [];
  paymentadd1_list: any[] = [];
  payadd_list: any[] = [];
  payment_gid: any;
  employee_gid: any;
  exportexcellist:any[]=[];
  parameterValue: any;
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  parameterValue4:any;
  Document_list: any;
  data2:any;
  data:any;
  data1:any;
  paymentexpend_list: any;
  

  constructor(private formBuilder: FormBuilder,private excelService: ExcelService,public NgxSpinnerService:NgxSpinnerService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.PaymentReport = {} as IPaymentReport;
    }
   
ngOnInit(): void {

    this.salarypaymentsummary()
}

salarypaymentsummary(){
     //// Summary Grid//////
     var url = 'PayTrnSalaryPayment/GetSalaryPaymentSummary'
     this.service.get(url).subscribe((result: any) => {
     this.responsedata = result;
     this.payment_list = this.responsedata.paymentlist;
       setTimeout(() => {
         $('#payment_list').DataTable();
         }, );
   });
   
}


// paymentedit(payment_type:any,payment_date:any,payment_year:any,payment_month:any){
//   debugger
//   const secretKey = 'storyboarderp';
 
//    payment_type= payment_type,
//     payment_date=payment_date,
//     payment_year=payment_year,
//     payment_month=payment_month

//   const encryptedParam = AES.encrypt(payment_type,secretKey).toString();
//   const encryptedParam1 = AES.encrypt(payment_date,secretKey).toString();
//   const encryptedParam2 = AES.encrypt(payment_year,secretKey).toString();
//   const encryptedParam3 = AES.encrypt(payment_month,secretKey).toString();
//   this.router.navigate(['/payroll/PayTrnSalarypaymentedit',encryptedParam,encryptedParam1,encryptedParam2,encryptedParam3]);
// }

paymentedit(payment_type: any, payment_date: any, payment_month: any, payment_year: any) {
  debugger
  const secretKey = 'storyboarderp';
  
  // Ensure parameters are assigned correctly
  const encryptedParam = AES.encrypt(payment_type.toString(), secretKey).toString();
  const encryptedParam1 = AES.encrypt(payment_date.toString(), secretKey).toString();
  const encryptedParam2 = AES.encrypt(payment_month.toString(), secretKey).toString();
  const encryptedParam3 = AES.encrypt(payment_year.toString(), secretKey).toString();
  
  // Navigate to the edit page
  this.router.navigate(['/payroll/PayTrnSalarypaymentedit',encryptedParam,encryptedParam1,encryptedParam2,encryptedParam3]);
}

  ondetail(month: any,year:any) {
    debugger;
    var url = 'PayTrnSalaryPayment/GetSalaryPaymentExpand'
    let param = {
      month : month, 
      year : year 
    }
    this.service.getparams(url, param).subscribe((result: any) => {
    this.paymentadd_list = result.getpayment;

      });
  }


 

  
 
ondetail1(month: any, year: any, payment_date: any, payment_type: any) {
  
  function formatDate(inputDate: any) {
    var dateParts = inputDate.split("-");
    var formattedDate = dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0];
    return formattedDate;
}

  var url = 'PayTrnSalaryPayment/GetSalaryPaymentExpand2';
 
  var formattedPaymentDate = formatDate(payment_date);

  let param = {
      month: month,
      year: year,
      payment_date: formattedPaymentDate,
      modeof_payment: payment_type
  };

  this.service.getparams(url, param).subscribe((result: any) => {
      this.paymentexpend_list = result.getpayment1;
  });
}

  makepayment(params: any,params1:any){
    debugger;
    // this.router.navigate(['/payroll/PayTrnMakepayment'])
    const secretKey = 'storyboarderp';
    const param = (params+'+'+params1);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/payroll/PayTrnMakepayment',encryptedParam]) 
  }
 
 
    openModalpayment(parameter: string,parameter1:string) {
   
    this.router.navigate(['/payroll/PayTrnMakepayment'])
   
    this.parameterValue = parameter
    this.parameterValue1 = parameter1

    this.reactiveFormSubmit.get("payment_gid")?.setValue(this.parameterValue1.payment_gid);
    this.reactiveFormSubmit.get("employee_gid")?.setValue(this.parameterValue.employee_gid);
    console.log(this.reactiveFormSubmit)
    const Paymentgid = this.reactiveFormSubmit.value.payment_gid;
    const Employeegid = this.reactiveFormSubmit.value.employee_gid;
    if (this.reactiveFormSubmit.value.payment_gid != null && this.reactiveFormSubmit.value.employee_gid != '') {
      for (const control of Object.keys(this.reactiveFormSubmit.controls)) {
        this.reactiveFormSubmit.controls[control].markAsTouched();
      }
      const params = {
        payment_gid: Paymentgid,
        employee_gid: Employeegid
      };
    
      var url = 'PayTrnSalaryPayment/AssetDocument'
      this.service.getparams(url, params).subscribe((result: any) => {
        this.Document_list = result.Assetcustodian;
     

      
      });
    }

 
  }
  exportExcel(payment_date:any,paidbybank:any,payment_type:any){
    debugger
    var url = "PayTrnSalaryPayment/ExportExcel";
    let params ={
      payment_date:payment_date,
      paidbybank:paidbybank,
      payment_type:payment_type
    }
   this.service.getparams(url,params).subscribe((result:any)=>{
    if(result!=null){
      this.service.filedownload1(result);
    } 
    this.NgxSpinnerService.hide()
   });
 

  }

  pdf(payment_date:any)
  {
    debugger;
    var param={
      
     payment_date:payment_date
    }
    this.NgxSpinnerService.show();
  var url = 'PayTrnSalaryPayment/GetsalarypaymentRpt';
  this.service.getparams(url, param).subscribe((result: any) => {
    if(result!=null){
      this.service.filedownload1(result);
    } 
    this.NgxSpinnerService.hide()

  }
)}

  openModalpaymentedit(){
    this.router.navigate(['/payroll/PayTrnPaymentedit'])
  }
  openModalpaymentdelete(parameter: string,parameter1:string,parameter2: string,parameter3:string,parameter4:string){
    this.parameterValue = parameter
    this.parameterValue1 = parameter1
    this.parameterValue2 = parameter2
    this.parameterValue3 = parameter3
    this.parameterValue4 = parameter4
  }

  ondelete(){
    debugger;

    function formatDate(inputDate: any) {
      var dateParts = inputDate.split("-");
      var formattedDate = dateParts[2] + "-" + dateParts[1] + "-" + dateParts[0];
      return formattedDate;
    }
    console.log(this.parameterValue);
    var url3 = 'PayTrnSalaryPayment/getDeletePayment'

    var formattedPaymentDate = formatDate(this.parameterValue2);
    let params = {
      payment_month:this.parameterValue,
      payment_year: this.parameterValue1,
      payment_date:formattedPaymentDate,
      payment_type:this.parameterValue3,
      paid_bank:this.parameterValue4
  };
    this.service.getparams(url3, params).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning("Cannot Delete the payment");
        // this.NgxSpinnerService.hide();
        this.salarypaymentsummary();
      }
      else {

        this.ToastrService.success("Payment Deleted Successfully")
        // this.NgxSpinnerService.hide();
        this.salarypaymentsummary();
       }
       setTimeout(function() {
        window.location.reload();
    }, 2000);
    });
  }
}
