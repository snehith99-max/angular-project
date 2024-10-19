import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service'; 
import { ToastrService } from 'ngx-toastr';
import { RouterTestingHarness } from '@angular/router/testing';
import { AES, enc } from 'crypto-js';
import { Subscription, map, share, timer } from 'rxjs';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';

interface  ICustomer

{

}
@Component({
  selector: 'app-smr-trn-customer-product-price',
  templateUrl: './smr-trn-customer-product-price.component.html',
  styleUrls: ['./smr-trn-customer-product-price.component.scss']
})
export class SmrTrnCustomerProductPriceComponent {
  @ViewChild('Inbox') tableRef!: ElementRef;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  subscription!: Subscription;
  currentDayName: any;
  fromDate: any; toDate: any;
  salesproduct_list: any[] = [];
  saleproduct_list: any[] = [];
  cusprodForm!: FormGroup;
  customer!: ICustomer;
  productasgn: any;
  customer_name:any;
  customer_type:any;
  responsedata: any;
  customer_gid:any;
  data: any;
  isref:any;
  isdata:any;
  lspage:any;
  mrp_price: number = 0;
  cost_price: number = 0;
  selling_price : number = 0;
  discamount: number = 0;
amount: number = 0;

  constructor( private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService,private datePipe: DatePipe,  private route: Router, private router: ActivatedRoute) {
    this.customer = {} as ICustomer;
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
  }
  ngOnInit(): void {

    this.cusprodForm = new FormGroup ({
      cost_price:new FormControl (''),
      selling_price:new FormControl (''),
      customer_name:new FormControl(''),
      customer_gid:new FormControl(''),
      amount : new FormControl (''),
      amount1 : new FormControl (''),

      discamount : new FormControl (''),
      saleproduct_list:  this.formBuilder.array([0]),
    });
debugger
    this.productasgn = this.router.snapshot.paramMap.get('customer_gid');
    const lspage1 = this.router.snapshot.paramMap.get('lspage');
    this.lspage = lspage1;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productasgn, secretKey).toString(enc.Utf8);
    this.GetSmrTrnProductAssignSummary(deencryptedParam);
    this.customer_gid=deencryptedParam;
   console.log(deencryptedParam);

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
  }

  GetSmrTrnProductAssignSummary(customer_gid: any) {       
  debugger
  this.isdata = true;
  this.isref = false;
    let param = {
      customer_gid: customer_gid,
    }
    var url = 'SmrTrnCustomerSummary/GetProductAssignSummary'
    this.service.getparams(url,param).subscribe((result: any) => {
      $('#salesproduct_list').DataTable().destroy();
     this.responsedata=result;
     this.salesproduct_list = result.product_list
     for(let i=0;i<this.salesproduct_list.length;i++){
      this.cusprodForm.addControl(`selling_price_${i}`, new FormControl(this.salesproduct_list[i].selling_price));
    }

     setTimeout(() => {
      $('#salesproduct_list').DataTable();
    }, 1);
    });
    var url = 'SmrTrnCustomerSummary/Getcustomername'
    this.service.getparams(url,param).subscribe((result: any) => {
      this.saleproduct_list = result.GetCustomerList
      this.customer_name=this.saleproduct_list[0].customer_name
      this.customer_type=this.saleproduct_list[0].customer_type
    });
  }
  
  onchangeamount()
  {
    debugger
if(this.cusprodForm.value.amount1=="Discount")
{
  for (let i=0;i<this.salesproduct_list.length;i++)
  {
    let price =this.salesproduct_list[i].product_price;
    let amount=this.cusprodForm.value.amount;
    let total=((Number(price))*Number(amount)/100)
    let onselling_price = Number(price) - Number(total);
    this.salesproduct_list[i].selling_price=onselling_price.toFixed(2);
    this.isdata = false;
    this.isref = true;

  }

}
else if (this.cusprodForm.value.discamount=="Margin")
{
  for (let i=0;i<this.salesproduct_list.length;i++)
    {
      let price1 =this.salesproduct_list[i].cost_price;
      let amount=this.cusprodForm.value.amount;
      let total1=((Number(price1))*Number(amount)/100)
      let oncost_price = Number(price1) + Number(total1);
      this.salesproduct_list[i].cost_price=oncost_price.toFixed(2);
      this.isdata = false;
      this.isref = true;

    }

}
  }
  onSubmit()
  {
    var url = 'SmrTrnCustomerSummary/Customerprice';
    this.NgxSpinnerService.show();
      const param = {        
        salesproduct_list:this.salesproduct_list,
         customer_gid:this.customer_gid
      };
      debugger;
      console.log(param)
    
      this.service.postparams(url, param).subscribe((result: any) => {   
        this.NgxSpinnerService.hide();
        if (result.status === false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
        } else {
          this.ToastrService.success(result.message);
          this.NgxSpinnerService.hide();
          if(this.lspage == '/smr/SmrTrnCustomerRetailer')
        {
          this.route.navigate(['/smr/SmrTrnCustomerRetailer']);
          this.NgxSpinnerService.hide()
        }
        else if(this.lspage == '/smr/SmrTrnCustomerDistributor')
        {
          this.route.navigate(['/smr/SmrTrnCustomerDistributor']);
          this.NgxSpinnerService.hide()
        }
        else if(this.lspage == '/smr/SmrTrnCustomerCorporate')
        {
          this.route.navigate(['/smr/SmrTrnCustomerCorporate']);
          this.NgxSpinnerService.hide()
        }
        else if(this.lspage == '/smr/SmrTrnCustomerSummary')
        {
          this.route.navigate(['/smr/SmrTrnCustomerSummary']);
        }
        }
      });
  }
  
close()
{
  if(this.lspage == '/smr/SmrTrnCustomerRetailer')
    {
      this.route.navigate(['/smr/SmrTrnCustomerRetailer']);

    }
    else if(this.lspage == '/smr/SmrTrnCustomerDistributor')
    {
      this.route.navigate(['/smr/SmrTrnCustomerDistributor']);

    }
    else if(this.lspage == '/smr/SmrTrnCustomerCorporate')
    {
      this.route.navigate(['/smr/SmrTrnCustomerCorporate']);

    }
  else
  {
  this.route.navigate(['/smr/SmrTrnCustomerSummary']);
  }
}
onchangerefresh()
{
  location.reload();
  this.isref = false;
  this.isdata = true;
 
}






  }

