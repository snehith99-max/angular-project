
  import { Component, ElementRef, ViewChild } from '@angular/core';
  import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
    tax_name4: any;
    taxsegment_name: any;
    tax_name: any;
  
  }
  @Component({
    selector: 'app-smr-mst-maptaxsegment2product',
    templateUrl: './smr-mst-maptaxsegment2product.component.html',
    styleUrls: ['./smr-mst-maptaxsegment2product.component.scss']
  })
  export class SmrMstMaptaxsegment2productComponent {
    mdlTaxName4: any;
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
    mrp_price: number = 0;
    cost_price: number = 0;
    selling_price : number = 0;
    discamount: number = 0;
  amount: number = 0;
    taxsegment_list: any;
    tax4_list: any;
    product_gid:any;
    productgid: any;
    taxsegment2prod_list: any;
    product_name: any;
    parameterValue: any;
    ViewProductSummary_list: any;
    get taxsegment_name() {
      return this.cusprodForm.get('taxsegment_name')!;
    }
    get tax_name4() {
      return this.cusprodForm.get('tax_name4')!;
    }
    constructor( private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService,private datePipe: DatePipe,  private route: Router, private router: ActivatedRoute) {
      this.customer = {} as ICustomer;  
      const today = new Date();
      this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
    }
    ngOnInit(): void {

      const product_gid = this.router.snapshot.paramMap.get('product_gid');
      this.productgid = product_gid;  
      const secretKey = 'storyboarderp';    
      const deencryptedParam = AES.decrypt(this.productgid, secretKey).toString(enc.Utf8);
      this.product_gid =deencryptedParam;
      this.GetViewProductSummary(deencryptedParam);
      var url = 'SmrMstTaxSegment/GetTaxSegmentSummary'
      this.service.get(url).subscribe((result: any) => {
        this.taxsegment_list = result.TaxSegmentSummary_list;
      });
      var url = 'SmrMstTaxSegment/GetTax'
      debugger
      this.service.get(url).subscribe((result: any) => {
        this.tax4_list = result.GetTax4Dtl;
      });
      this.getMappedTax(this.product_gid);
      this.cusprodForm = new FormGroup ({
        cost_price:new FormControl (''),
        selling_price:new FormControl (''),
        customer_name:new FormControl(''),
        customer_gid:new FormControl(''),
        amount : new FormControl (''),
        amount1 : new FormControl (''),
  
        discamount : new FormControl (''),
        saleproduct_list:  this.formBuilder.array([0]),
        taxsegment_name: new FormControl(this.customer.taxsegment_name, [
          Validators.required,
  
        ]),
        tax_name4: new FormControl(this.customer.tax_name4, [
          Validators.required,
  
        ]),
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
    }
    GetViewProductSummary(product_gid: any) {
      var url='SmrMstProduct/GetViewProductSummary'
      let param = {
        product_gid : this.product_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
      this.ViewProductSummary_list = result.GetViewProductSummary;   
      this.product_name =this.ViewProductSummary_list[0].product_name;
      this.mrp_price =this.ViewProductSummary_list[0].mrp_price;
      });
    }
 getMappedTax(product_gid:any){
  
  var url = 'SmrMstTaxSegment/GetTaxSegment2ProductSummary'
  var param={
    product_gid:product_gid,
  }
  debugger
  this.service.getparams(url,param).subscribe((result: any) => {
    this.taxsegment2prod_list = result.TaxSegmentSummary_list;
    // this.product_name =result.TaxSegmentSummary_list[0].product_name;
    // this.mrp_price =result.TaxSegmentSummary_list[0].mrp_price;

});
}
ondelete() {
  console.log(this.parameterValue);
  var url = 'SmrMstTaxSegment/DeleteTaxSegment2Product'
  let param = {
    taxsegment2product_gid: this.parameterValue
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    if (result.status == false) {
      this.ToastrService.warning(result.message)
      this.getMappedTax(this.product_gid);
    }
    else {

      this.ToastrService.success(result.message)
      this.getMappedTax(this.product_gid); }
   
    

  });
}

 openModaldelete(parameter: string) {
  this.parameterValue = parameter

}
    onSubmit()
    {
      var url = 'SmrMstTaxSegment/PostTaxSegment2Product';
      this.NgxSpinnerService.show();
        const param = {      
          
          product_gid:this.product_gid,
          tax_gid:this.cusprodForm.value.tax_name4,
          taxsegment_gid:this.cusprodForm.value.taxsegment_name,
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
            this.cusprodForm.reset();
            location.reload();
            //this.route.navigate(['/smr/SmrMstProductSummary'])
          }
        });
    }
    
  close()
  {
    this.route.navigate(['/smr/SmrMstProductSummary']);
  }
  onchangerefresh()
  {
    location.reload();
    this.isref = false;
    this.isdata = true;
   
  }
  
  
  
  
  
  
    }
  
  