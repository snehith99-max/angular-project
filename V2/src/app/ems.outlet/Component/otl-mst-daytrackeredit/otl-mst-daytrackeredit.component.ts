import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe } from '@angular/common';
import { Subscription, map, share, timer } from 'rxjs';
interface Idaytracker{
  daytracker_gid:string;
}
@Component({
  selector: 'app-otl-mst-daytrackeredit',
  templateUrl: './otl-mst-daytrackeredit.component.html',
  styleUrls: ['./otl-mst-daytrackeredit.component.scss']
})
export class OtlMstDaytrackereditComponent {
  time = new Date();
  rxTime = new Date();
  currentDayName: any;
  fromDate: any; toDate: any;
  subscription!: Subscription;
  intervalId: any;
  reactiveFormedit!: FormGroup;
  responsedata: any;
  revenue_list: any[] = []
  expense_list: any[] = []
  combined_list:any[]=[]
  outletname_list:any;
  total_revenue:any;
  total_expence:any;
  campaign_title:any;
  daytracker_gid:any;
  constructor(private renderer: Renderer2, 
    private el: ElementRef, 
    public service: SocketService,
    private route: Router, 
    private router: ActivatedRoute, 
    private ToastrService: ToastrService,
    private datePipe: DatePipe,
    private NgxSpinnerService: NgxSpinnerService)  {
      const today = new Date();
      this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
 }
 ngOnInit(): void {
  const daytracker_gid = this.router.snapshot.paramMap.get('daytracker_gid');
  this.daytracker_gid = daytracker_gid;
  const secretKey = 'storyboarderp';
  const deencryptedParam = AES.decrypt(this.daytracker_gid, secretKey).toString(enc.Utf8);
  this.daytracker_gid=deencryptedParam
  console.log(deencryptedParam+"daytracker_gid");
  this.GetEditdaytraker(deencryptedParam);
  this.reactiveFormedit = new FormGroup({
    revenuetotal:new FormControl(''),
    revenue_amount: new FormControl(''),
    expense_amount: new FormControl(''),
    expensetotal:new FormControl(''),
    daytrackerdtl_gid:new FormControl(''),

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
GetEditdaytraker(daytracker_gid:any){
  debugger
  var url = 'DayTracker/GetRevenueEditnewsummary';
  this.NgxSpinnerService.show();
  this.daytracker_gid = daytracker_gid;
  var params = {
    daytracker_gid: daytracker_gid
  };
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    debugger;
    this.revenue_list = this.responsedata.revenue_list;
    for(let i=0;i<this.revenue_list.length;i++){
      this.reactiveFormedit.addControl(`revenue_amount_${i}`, new FormControl(this.revenue_list[i].revenue_amount));
    }
    console.log(this.revenue_list);
  });
  var url = 'DayTracker/GetExpenseEditnewSummary'
  this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.expense_list = this.responsedata.expense_list;
    for(let i=0;i<this.expense_list.length;i++){
      this.reactiveFormedit.addControl(`expense_amount_${i}`, new FormControl(this.expense_list[i].expense_amount));
    }
    console.log(this.expense_list)
  });
  this.NgxSpinnerService.hide();
}
update() {
  debugger;
  var params={ 
    revenue_list : this.revenue_list,
    expense_list: this.expense_list,
    revenue_total: this.reactiveFormedit.value.revenuetotal,
    expense_total: this.reactiveFormedit.value.expensetotal,
    daytracker_gid:this.daytracker_gid,
  }
  console.log(params)

  var url = 'DayTracker/PostDaytrackeredit'
  this.NgxSpinnerService.show();
    this.service.postparams(url,params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.route.navigate(['/outlet/otlmstdaytrackersummary']);
     }
     else{
      this.ToastrService.success(result.message)
      this.route.navigate(['/outlet/otlmstdaytrackersummary']);
     }

    });
}

onclose() {
  this.route.navigate(['/smr/SmrTrnCustomerenquirySummary']);
}

revenue_total() {
  let amount = 0;

  for (let i = 0; i < this.revenue_list.length; i++) {
    let amount1 = parseFloat(this.revenue_list[i].revenue_amount.replace(/,/g, '')) || 0;
    amount += amount1;
  }
  let formattedTotal = amount.toFixed(2);
  this.reactiveFormedit.patchValue({
    revenuetotal: formattedTotal
  });
}

expense_total() {
  let expense = 0;

  for (let i = 0; i < this.expense_list.length; i++) {
    let expense1 = parseFloat(this.expense_list[i].expense_amount.replace(/,/g, '')) || 0;
    expense += expense1;
  }
  let formattedExpense = expense.toFixed(2);
  this.reactiveFormedit.patchValue({
    expensetotal: formattedExpense
  });
}

}


