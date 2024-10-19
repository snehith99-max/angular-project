import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';import { NgxSpinnerService } from 'ngx-spinner';

export class Ipayrundelete {
  payrunlist: string[] = [];
  month: any;
  year:any;
}
interface IPayrunReport {
  
}

@Component({
  selector: 'app-pay-trn-payrunview',
  templateUrl: './pay-trn-payrunview.component.html',
  styleUrls: ['./pay-trn-payrunview.component.scss'],
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
export class PayTrnPayrunviewComponent {
  reactiveForm!: FormGroup;
  payrunmanagement!: Ipayrundelete;

  responsedata: any;
  dept_name: any;
  departmentlist: any[] = [];
  branch_name: any;
  branchlist: any[] = [];
  payrun_list: any[] = [];
  selection = new SelectionModel<Ipayrundelete>(true, []);
  CurObj: Ipayrundelete = new Ipayrundelete();
  pick: Array<any> = [];
  addtionOptions: any[]= [];
  deductionOptions: any[] = [];
  payrunother_list: any[] = [];

  
  PayrunReport!: IPayrunReport;
  branch_gid: any;
  salary_gid:any;
  monthyear: any;
  month:any;
  year:any;
  working_days: any;
  department_gid: any;
  company_code: any;
  payrunadd_list:any;
  payrundeduction_list: any;
  constructor(private formBuilder: FormBuilder,
     private route: ActivatedRoute,
      private router: Router,
      public NgxSpinnerService:NgxSpinnerService,
       private ToastrService: ToastrService,
        public service: SocketService) {
  this.PayrunReport = {} as IPayrunReport;
  }

  ngOnInit(): void {
    const monthyear = this.route.snapshot.paramMap.get('monthyear');
    this.monthyear = monthyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    debugger;
    const [month, year,working_days] = deencryptedParam.split('+');
    this.month = month;
    this.year = year;
    this.working_days = working_days;

 
  this.summary(this.month,this.year);

}
summary(month :any,year :any){
  const param = {
    month :month,
    year:year,
  };

  const url = 'PayTrnSalaryManagement/Getpayrunsummary';

  this.service.getparams(url, param).subscribe((result: any) => {
    this.payrun_list = result.payrunviewlist;
    setTimeout(() => {
      $('#payrun_list').DataTable();
    },1);
  });
}

 getfuncAdd(salary_gid :string){
      var url = 'PayTrnSalaryManagement/Additionalsubsummary'
      let param = {
        salary_gid: salary_gid
       };
      
    this.service.getparams(url,param).subscribe((result: any) => {
      this.addtionOptions = result.addsummary1; 
      debugger;
      console.log(this.addtionOptions)  
      });   
      }

 getfuncdeduct(salary_gid :string){
          var url = 'PayTrnSalaryManagement/Deductsubsummary'
          let param= {
            salary_gid: salary_gid
           };
          
        this.service.getparams(url,param).subscribe((result: any) => {
          this.deductionOptions = result.deductsummary1; 
          debugger;
          console.log(this.deductionOptions)  
          });   
  }

  getfuncother(salary_gid :string){
     var url = 'PayTrnSalaryManagement/othersummary'
     let param = {
     salary_gid: salary_gid
            };
              
    this.service.getparams(url,param).subscribe((result: any) => {
    this.payrunother_list = result.otherssummary1; 
    debugger;
    console.log(this.payrunother_list)  
       });   
     }
     deletepayrun(){
      {
        debugger;
        this.pick = this.selection.selected
        this.CurObj.payrunlist = this.pick
        this.CurObj.month=this.month
        this.CurObj.year = this.year
        if ( this.CurObj.payrunlist.length === 0) {
          this.ToastrService.warning("Please select atleast one employee to delete");
          return;
        } 
      
        var url = 'PayRptPayrunSummary/Deleteforpayrun'
        this.NgxSpinnerService.show();
          this.service.post(url, this.CurObj).subscribe((result: any) => {
            if (result.status == true) {
            this.ToastrService.success(result.message);
            // this.NgxSpinnerService.show();


           }
           else{
            this.ToastrService.warning(result.message);
            // this.NgxSpinnerService.hide();

      
           }      
          }); 
          setTimeout(function() {
            window.location.reload();
        }, 2000);
        }
     }


isAllSelected() {
  const numSelected = this.selection.selected.length;
  const numRows = this.payrun_list.length;
  return numSelected === numRows;
   }
masterToggle() {
  this.isAllSelected() ?
  this.selection.clear() :
  this.payrun_list.forEach((row: Ipayrundelete) => this.selection.select(row));
  }

  payrunedit(params: any){

    const secretKey = 'storyboarderp';

    const param = (params);

    const encryptedParam = AES.encrypt(param,secretKey).toString();

    this.router.navigate(['/payroll/PayTrnPayrunedit',encryptedParam])

  }

 
        
}