import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-smr-rpt-ageingreport',
  templateUrl: './smr-rpt-ageingreport.component.html',
  styleUrls: ['./smr-rpt-ageingreport.component.scss']
})
export class SmrRptAgeingreportComponent {



  thirtydays_list: any[] = [];
  thirtytosixty_list: any[] = [];
  sixtytoninty_list: any[] = [];
  nintytoonetwenty_list: any[] = [];
  onetwentytooneeighty_list: any[] = [];
  All_lists: any[] = [];

  
  thirtytotalInvoiceAmount: string = '';
  thirtytotalPaidAmount: string = '';
  thirtytotalOutstandingAmount: string = '';

  thirtytosixtytotalInvoiceAmount: string = '';
  thirtytosixtytotalPaidAmount: string = '';
  thirtytosixtytotalOutstandingAmount: string = '';

  sixtytonintytotalInvoiceAmount: string = '';
  sixtytonintytotalPaidAmount: string = '';
  sixtytonintytotalOutstandingAmount: string = '';

  nintytoonetwentytotalInvoiceAmount: string = '';
  nintytoonetwentytotalPaidAmount: string = '';
  nintytoonetwentytotalOutstandingAmount: string = '';

  onetwentytooneeightytotalInvoiceAmount: string = '';
  onetwentytooneeightytotalPaidAmount: string = '';
  onetwentytooneeightytotalOutstandingAmount: string = '';

  AlltotalInvoiceAmount: string = '';
  AlltotalPaidAmount: string = '';
  AlltotalOutstandingAmount: string = '';

  paramvalue: any;
  

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private router: ActivatedRoute,
    private route: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    debugger
    var productapi = 'SmrRptAgeingreport/Get30invoicereport';
    this.service.get(productapi).subscribe((result: any) => {
      this.thirtydays_list = result.thirtydays_list;
      debugger
          this.thirtytotalInvoiceAmount = this.formatNumber(result.invoice_amount30);
    this.thirtytotalPaidAmount = this.formatNumber(result.paid_amount30);
    this.thirtytotalOutstandingAmount =  this.formatNumber(result.outstanding_amount30);
      setTimeout(() => {
        $('#thirtydays_list').DataTable();
      }, 1);
    });
    var Getupdaysapi = 'SmrRptAgeingreport/Get30to60invoicereport';
    this.service.get(Getupdaysapi).subscribe((result: any) => {
      this.thirtytosixty_list = result.thirtytosixty_list;
     
    this.thirtytosixtytotalInvoiceAmount = this.formatNumber(result.invoice_amount60);
    this.thirtytosixtytotalPaidAmount = this.formatNumber(result.paid_amount60);
    this.thirtytosixtytotalOutstandingAmount = this.formatNumber(result.outstanding_amount60);
      setTimeout(() => {
        $('#thirtytosixty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'SmrRptAgeingreport/Get60to90invoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.sixtytoninty_list = result.sixtytoninty_list;
     
    this.sixtytonintytotalInvoiceAmount = this.formatNumber(result.invoice_amount90);
    this.sixtytonintytotalPaidAmount =  this.formatNumber(result.paid_amount90);
    this.sixtytonintytotalOutstandingAmount = this.formatNumber(result.outstanding_amount90);
      setTimeout(() => {
        $('#sixtytoninty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'SmrRptAgeingreport/Get90to120invoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.nintytoonetwenty_list = result.nintytoonetwenty_list;
      
    this.nintytoonetwentytotalInvoiceAmount =this.formatNumber(result.invoice_amount120);
    this.nintytoonetwentytotalPaidAmount =  this.formatNumber(result.paid_amount120);
    this.nintytoonetwentytotalOutstandingAmount = this.formatNumber(result.outstanding_amount120);
      setTimeout(() => {
        $('#nintytoonetwenty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'SmrRptAgeingreport/Get120to180invoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.onetwentytooneeighty_list = result.onetwentytooneeighty_list;
     
    this.onetwentytooneeightytotalInvoiceAmount = this.formatNumber(result.invoice_amount180);
    this.onetwentytooneeightytotalPaidAmount =  this.formatNumber(result.paid_amount180);
    this.onetwentytooneeightytotalOutstandingAmount = this.formatNumber(result.outstanding_amount180);
      setTimeout(() => {
        $('#onetwentytooneeighty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'SmrRptAgeingreport/Getallinvoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.All_lists = result.All_lists;
     
    this.AlltotalInvoiceAmount = this.formatNumber(result.invoice_amountall);
    this.AlltotalPaidAmount =  this.formatNumber(result.paid_amountall);
    this.AlltotalOutstandingAmount = this.formatNumber(result.outstanding_amountall);
      setTimeout(() => {
        $('#All_lists').DataTable();
      }, 1);
    });
  }


  formatNumber(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  roundToTwoDecimal(value: number): number {
    return Math.round(value * 100) / 100;
  }
 
  

}
