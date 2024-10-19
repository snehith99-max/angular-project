
import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { HttpParams } from '@angular/common/http';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import { SelectionModel } from '@angular/cdk/collections';
import { Subscription, map, share, timer } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-otl-trn-tradedaytracker',
  templateUrl: './otl-trn-tradedaytracker.component.html',
  styleUrls: ['./otl-trn-tradedaytracker.component.scss']
})
export class OtlTrnTradedaytrackerComponent {
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
  balance_date:any;
  remarks:any;
  trade_date:any
  trade_list:any[]=[]

  constructor(private formBuilder: FormBuilder,

    private el: ElementRef, 
    public service: SocketService,
    private route: Router, 
    private router: ActivatedRoute, 
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService
   ,private datePipe: DatePipe,) {
      const today = new Date();
      this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });

 }
 ngOnInit(): void {
debugger;
  const balance_date = this.router.snapshot.paramMap.get('balance_date');
        this.balance_date = balance_date;
        const secretKey = 'storyboarderp';
        const deencryptedParam = AES.decrypt(this.balance_date, secretKey).toString(enc.Utf8);
        console.log(deencryptedParam+"balance_date");
        this.trade_date=deencryptedParam;

  this.reactiveFormadd = new FormGroup({
    revenuetotal:new FormControl(''),
    revenue_amount: new FormControl(''),
    expense_amount: new FormControl(''),
    expensetotal:new FormControl(''),
    remarks:new FormControl(''),
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
  this.route.navigate(['/outlet/OtlTrnTradedaytracker',encryptedParam])
}
  submit() {
    debugger;
    
    // Check if at least one value has been entered in expense_amount or revenue_amount
    const RevenueAmount = this.revenue_list.some(item => item.revenue_amount && item.revenue_amount > 0);
    const ExpenseAmount = this.expense_list.some(item => item.expense_amount && item.expense_amount > 0);
  
    if (!RevenueAmount && !ExpenseAmount) {
      this.ToastrService.warning('Please fill in at least one value in the revenue or expense.');
      return;
    }
  
    var params = { 
      revenue_list: this.revenue_list,
      expense_list: this.expense_list,
      revenue_total: this.reactiveFormadd.value.revenuetotal,
      expense_total: this.reactiveFormadd.value.expensetotal,
      remarks:this.reactiveFormadd.value.remarks,
      trade_date:this.trade_date,
    };
  
    console.log(params);
  
    var url = 'DayTracker/Posttradedatesubmit';
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.route.navigate(['/outlet/otlmstdaytrackersummary']);
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


