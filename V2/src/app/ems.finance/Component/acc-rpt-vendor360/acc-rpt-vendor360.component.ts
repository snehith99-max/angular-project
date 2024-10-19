import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Subscription, timer } from "rxjs";
import { map, share } from "rxjs/operators";
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { saveAs } from 'file-saver';
import { Location } from '@angular/common';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";

import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-acc-rpt-vendor360',
  templateUrl: './acc-rpt-vendor360.component.html',
  styleUrls: ['./acc-rpt-vendor360.component.scss']
})
export class AccRptVendor360Component {

  vendor_gid: any;
  fromDate: any;
  toDate: any;
  currentDayName: string;
  time = new Date();
  rxTime = new Date();
  intervalId: any;
  customer_gid: any;
  subscription!: Subscription;
  responsedata : any;
  vencount_list : any [] =[];
  vendordetailslist: any[]=[];
  GetPurchase_list : any[]=[];
  flag2: boolean = false;
  invoice_count : any;
  PO_count: any;
  purchasechart: any = {};
  paymentchart:any={};
  purchase_months : any;
  paymentchartflag:boolean=false;
  Getpaymentchart_list : any[]=[]
  series_Value: any;
  labels_value: any;
  cancelled_payment: any;
  approved_payment: any;
  completed_payment: any;


  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private datePipe: DatePipe, private NgxSpinnerService: NgxSpinnerService, private location: Location) {
    const today = new Date();
    this.currentDayName = today.toLocaleDateString('en-US', { weekday: 'long' });
  }

  ngOnInit() : void{
    const vendor_gid = this.route.snapshot.paramMap.get('vendor_gid');
    this.vendor_gid = vendor_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.vendor_gid, secretKey).toString(enc.Utf8);
    this.vendor_gid = deencryptedParam;
    this.Getcustomerdetails(deencryptedParam);
    this.GetCount(deencryptedParam);
    this.GetPurchaseStatus(deencryptedParam);
    this.getpaymentchart(deencryptedParam)

    // date and time
    let yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    this.fromDate = this.datePipe.transform(yesterday, 'dd-MM-yyyy');
    this.toDate = this.datePipe.transform(new Date(), 'dd-MM-yyyy');

    this.intervalId = setInterval(() => {
      this.time = new Date();
    }, 1000);

    this.subscription = timer(0, 1000).pipe(map(() => new Date()),share()).subscribe(time => {
        let hour = this.rxTime.getHours();
        let minuts = this.rxTime.getMinutes();
        let seconds = this.rxTime.getSeconds();
        let NewTime = hour + ":" + minuts + ":" + seconds
        this.rxTime = time;
      });
  }

  // tiles count
  GetCount(deencryptedParam : any) {
    var url = 'Vendor360/GetTilesCount';
    let param = {
      vendor_gid: this.vendor_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.vencount_list = this.responsedata.VendorTilesCount;

    });
  }

  // vendor details
  Getcustomerdetails(deencryptedParam : any) {
    debugger
    var url = 'Vendor360/GetVendorDetails'
    let param = {
      vendor_gid: this.vendor_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.vendordetailslist = this.responsedata.vendordetails;
    });
  }

  // purchase count
  GetPurchaseStatus(deencryptedParam : any) {
    var url = 'Vendor360/GetPurchasetatus';
    let param ={
      vendor_gid : this.vendor_gid
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetPurchase_list = this.responsedata.purchasecount;
     
      if (this.GetPurchase_list.length > 0) {
        this.flag2 = true;
      }
      this.PO_count = this.GetPurchase_list.map((entry: { po_count: any }) => entry.po_count)
      this.invoice_count = this.GetPurchase_list.map((entry: { invoice_count: any }) => entry.invoice_count)
      this.purchase_months = this.GetPurchase_list.map((entry: { Months: any }) => entry.Months)
    
      this.purchasechart = {
        chart: {
          type: 'bar',
          height: 300,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
        },
        colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.purchase_months,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Purchase Order',
            data: this.PO_count,
            color: '#3D9DD9',
          },
          {
            name: 'Invoice',
            data: this.invoice_count,
            color: '#9EBF95',
          },
        ],

        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
  }

  //payment chart
  getpaymentchart(deencryptedParam : any){
    var url = 'Vendor360/GetPaymentCount';
    let param ={
      vendor_gid : this.vendor_gid
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      this.responsedata = result;
      this.Getpaymentchart_list= this.responsedata.paymentcount_list;
      if ( this.Getpaymentchart_list.length === 0) {
        this.paymentchartflag = true;
      }
      this.cancelled_payment =  Number ( this.Getpaymentchart_list[0].cancelled_payment);
      this.approved_payment =  Number(this.Getpaymentchart_list[0].approved_payment);
      this.completed_payment =  Number(this.Getpaymentchart_list[0].completed_payment);
      this.series_Value = [ this.approved_payment,this.completed_payment ,this.cancelled_payment  ];
      this.labels_value = ['Payment Approved','Payment Completed','Payment Cancelled'];
      this.paymentchart = {
        series: this.series_Value,
        labels: this.labels_value,
        chart: {
          width: 430,
          type: "pie"
        },
        colors: ['#E2D686','#26C485','#F96666'], // Update colors as needed
        fill: {
          type: "solid"
        },
      };

    });
  }
}
