import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { AES } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import { SelectionModel } from '@angular/cdk/collections';
import { Subscription, map, share, timer } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-otl-mst-daytrackeradd',
  templateUrl: './otl-mst-daytrackeradd.component.html',
  styleUrls: ['./otl-mst-daytrackeradd.component.scss']
})
export class OtlMstDaytrackeraddComponent {
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  fromDate: any; toDate: any;
  subscription!: Subscription;
  intervalId: any;
  reactiveFormadd!: FormGroup;
  responsedata: any;
  revenue_list: any[] = []
  expense_list: any[] = []
  combined_list:any[]=[]
  outletname_list:any;
  total_revenue:any;
  total_expence:any;
  campaign_title:any;
  revenuetotal:any;
  expensetotal:any;
  remarks:any;
  trade_list:any[]=[];
  showTradeList: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
     public service: SocketService,
     private router: Router,
     public NgxSpinnerService:NgxSpinnerService,private datePipe: DatePipe,) {
      const today = new Date();
      this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });

 }
 ngOnInit(): void {

  this.reactiveFormadd = new FormGroup({
    revenuetotal:new FormControl(''),
    revenue_amount: new FormControl(''),
    expense_amount: new FormControl(''),
    expensetotal:new FormControl(''),
    remarks:new FormControl(''),
    leave:new FormControl('N'),
  });
  var url = 'DayTracker/Tradesummary';
    this.service.get(url).subscribe((result: any) => {
      debugger
      this.responsedata = result;
      this.trade_list = this.responsedata.trade_list;
      console.log(this.trade_list);
      this.triggerGetOptions();
      this.trade_list.forEach((item) => {
        item.trade_date = ('');
      });
      this.showTradeList = this.trade_list.some(item => item.notification === 'Y');
    });
  var url = 'DayTracker/Revenuesummary'
  this.service.get(url).subscribe((result: any) => {
debugger
    this.responsedata = result;
    this.revenue_list = this.responsedata.revenue_list;
    console.log(this.revenue_list)
    this.triggerGetOptions();
    this.revenue_list.forEach((item) => {
      item.revenue_amount = ('');
     
    });
  });
  var url = 'DayTracker/GetExpenseSummary'
  this.service.get(url).subscribe((result: any) => {

    this.responsedata = result;
    this.expense_list = this.responsedata.expense_list;
    console.log(this.expense_list)
    this.triggerGetOptions1();
    this.expense_list.forEach((item) => {
      item.expense_amount = ('');
    });

  });
  let yesterday = new Date();
  yesterday.setDate(yesterday.getDate() - 1);
  this.fromDate = this.datePipe.transform(yesterday, 'dd-MM-yyyy');
  this.toDate = this.datePipe.transform(new Date(), 'dd-MM-yyyy');

  this.intervalId = setInterval(() => {
    this.time = new Date();
  }, 1000);   
  this.subscription = timer(0, 1000)
    .pipe(
      map(() => new Date()),
      share()
    )
    .subscribe(time => {
      let hour = this.rxTime.getHours();
      let minuts = this.rxTime.getMinutes();
      let seconds = this.rxTime.getSeconds();       
      let NewTime = hour + ":" + minuts + ":" + seconds
      this.rxTime = time;
    });
    var url = 'DayTracker/GetOutletname'
    this.service.get(url).subscribe((result: any) => {
      this.outletname_list = result.outletname_list
      this.campaign_title=this.outletname_list[0].campaign_title
    });

}
triggerGetOptions(): void {
  for (let i = 0; i < this.revenue_list.length; i++) {
    const data = this.revenue_list[i];
  }
}
triggerGetOptions1(): void {
  for (let i = 0; i < this.expense_list.length; i++) {
    const data = this.expense_list[i];
  }
}
trade(params:any){
  debugger
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.router.navigate(['/outlet/OtlTrnTradedaytracker',encryptedParam])
}
onCheckboxChange(event: Event) {
  const isChecked = (event.target as HTMLInputElement).checked;
  this.reactiveFormadd.patchValue({ leave: isChecked ? 'Y' : 'N' });
}
  submit() {
    debugger;
    if(this.reactiveFormadd.value.leave ==='N'){
      const RevenueAmount = this.revenue_list.some(item => item.revenue_amount && item.revenue_amount > 0);
      const ExpenseAmount = this.expense_list.some(item => item.expense_amount && item.expense_amount > 0);
      if (!RevenueAmount && !ExpenseAmount) {
        this.ToastrService.warning('Please fill in at least one value in the revenue or expense.');
        return;
      }
    }
    if(this.trade_list.length==null||this.trade_list.length==0){
      this.ToastrService.warning('Expense and Revenue Amount Already Added!.');
      return;
    }
    // if (this.reactiveFormadd.value.leave === "Y") {
    //   this.revenue_list.forEach((item, index) => {
    //     this.revenue_list[index] = { ...item, value: 0.00 }; 
    //   });
    //   this.expense_list.forEach((item, index) => {
    //     this.expense_list[index] = { ...item, value: 0.00 }; 
    //   });
    // }
    var params = { 
      revenue_list: this.revenue_list,
      expense_list: this.expense_list,
      revenue_total: this.reactiveFormadd.value.revenuetotal,
      expense_total: this.reactiveFormadd.value.expensetotal,
      leave: this.reactiveFormadd.value.leave,
      remarks:this.reactiveFormadd.value.remarks,
      trade_date:this.trade_list[0].balance_date,
      previous_date:this.trade_list[0].previous_date,
    };
    console.log(params);
    var url = 'DayTracker/PostDaytrackersubmit';
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.router.navigate(['/outlet/otlmstdaytrackersummary']);
    });
  }
revenue_total() {
  let amount=0;

  for(let i=0;i<this.revenue_list.length;i++){
       let amount1=this.revenue_list[i].revenue_amount ||0;
       amount= Number(amount)+Number(amount1);
  }
   this.reactiveFormadd.patchValue({
    revenuetotal: amount.toFixed(2),
  
  });
}
expense_total() {
let expense=0;

for (let i=0;i<this.expense_list.length;i++){
  let expense1 =this.expense_list[i].expense_amount ||0;
  expense=Number(expense)+Number(expense1);
}
 this.reactiveFormadd.patchValue({
  expensetotal:expense.toFixed(2)
 });
}
}


