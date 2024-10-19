import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pbl-rpt-ageing-report',
  templateUrl: './pbl-rpt-ageing-report.component.html',
  styleUrls: ['./pbl-rpt-ageing-report.component.scss']
})
export class PblRptAgeingReportComponent {

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
    //payment_amount.replace(/,/g, '')
    
    var productapi = 'PmrRptAgeingReport/Get30purchaseinvoicereport';
    this.service.get(productapi).subscribe((result: any) => {
      this.thirtydays_list = result.thirtydays_list;
      const thirtytotalInvoiceAmount = this.roundToTwoDecimal(this.thirtydays_list.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const thirtytotalPaidAmount = this.roundToTwoDecimal(this.thirtydays_list.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const thirtytotalOutstandingAmount = this.roundToTwoDecimal(this.thirtydays_list.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.thirtytotalInvoiceAmount = this.formatNumber(thirtytotalInvoiceAmount);
    this.thirtytotalPaidAmount = this.formatNumber(thirtytotalPaidAmount);
    this.thirtytotalOutstandingAmount = this.formatNumber(thirtytotalOutstandingAmount);
      setTimeout(() => {
        $('#thirtydays_list').DataTable();
      }, 1);
    });
    var Getupdaysapi = 'PmrRptAgeingReport/Get30to60purchaseinvoicereport';
    this.service.get(Getupdaysapi).subscribe((result: any) => {
      this.thirtytosixty_list = result.thirtytosixty_list;
      const thirtytosixtytotalInvoiceAmount = this.roundToTwoDecimal(this.thirtytosixty_list.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalPaidAmount = this.roundToTwoDecimal(this.thirtytosixty_list.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const thirtytosixtytotalOutstandingAmount = this.roundToTwoDecimal(this.thirtytosixty_list.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.thirtytosixtytotalInvoiceAmount = this.formatNumber(thirtytosixtytotalInvoiceAmount);
    this.thirtytosixtytotalPaidAmount = this.formatNumber(thirtytosixtytotalPaidAmount);
    this.thirtytosixtytotalOutstandingAmount = this.formatNumber(thirtytosixtytotalOutstandingAmount);
      setTimeout(() => {
        $('#thirtytosixty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'PmrRptAgeingReport/Get60to90purchaseinvoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.sixtytoninty_list = result.sixtytoninty_list;
      const sixtytonintytotalInvoiceAmount = this.roundToTwoDecimal(this.sixtytoninty_list.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const sixtytonintytotalPaidAmount = this.roundToTwoDecimal(this.sixtytoninty_list.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const sixtytonintytotalOutstandingAmount = this.roundToTwoDecimal(this.sixtytoninty_list.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.sixtytonintytotalInvoiceAmount = this.formatNumber(sixtytonintytotalInvoiceAmount);
    this.sixtytonintytotalPaidAmount = this.formatNumber(sixtytonintytotalPaidAmount);
    this.sixtytonintytotalOutstandingAmount = this.formatNumber(sixtytonintytotalOutstandingAmount);
      setTimeout(() => {
        $('#sixtytoninty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'PmrRptAgeingReport/Get90to120purchaseinvoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.nintytoonetwenty_list = result.nintytoonetwenty_list;
      const nintytoonetwentytotalInvoiceAmount = this.roundToTwoDecimal(this.nintytoonetwenty_list.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const nintytoonetwentytotalPaidAmount = this.roundToTwoDecimal(this.nintytoonetwenty_list.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const nintytoonetwentytotalOutstandingAmount = this.roundToTwoDecimal(this.nintytoonetwenty_list.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.nintytoonetwentytotalInvoiceAmount = this.formatNumber(nintytoonetwentytotalInvoiceAmount);
    this.nintytoonetwentytotalPaidAmount = this.formatNumber(nintytoonetwentytotalPaidAmount);
    this.nintytoonetwentytotalOutstandingAmount = this.formatNumber(nintytoonetwentytotalOutstandingAmount);
      setTimeout(() => {
        $('#nintytoonetwenty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'PmrRptAgeingReport/Get120to180purchaseinvoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.onetwentytooneeighty_list = result.onetwentytooneeighty_list;
      const onetwentytooneeightytotalInvoiceAmount = this.roundToTwoDecimal(this.onetwentytooneeighty_list.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const onetwentytooneeightytotalPaidAmount = this.roundToTwoDecimal(this.onetwentytooneeighty_list.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const onetwentytooneeightytotalOutstandingAmount = this.roundToTwoDecimal(this.onetwentytooneeighty_list.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.onetwentytooneeightytotalInvoiceAmount = this.formatNumber(onetwentytooneeightytotalInvoiceAmount);
    this.onetwentytooneeightytotalPaidAmount = this.formatNumber(onetwentytooneeightytotalPaidAmount);
    this.onetwentytooneeightytotalOutstandingAmount = this.formatNumber(onetwentytooneeightytotalOutstandingAmount);
      setTimeout(() => {
        $('#onetwentytooneeighty_list').DataTable();
      }, 1);
    });
    var Getexpirysapi = 'PmrRptAgeingReport/Getallpurchaseinvoicereport';
    this.service.get(Getexpirysapi).subscribe((result: any) => {
      this.All_lists = result.All_lists;
      const AlltotalInvoiceAmount = this.roundToTwoDecimal(this.All_lists.reduce((acc, item) => acc + parseFloat(item.invoice_amount.replace(/,/g, '')), 0));
    const AlltotalPaidAmount = this.roundToTwoDecimal(this.All_lists.reduce((acc, item) => acc + parseFloat(item.paid_amount.replace(/,/g, '')), 0));
    const AlltotalOutstandingAmount = this.roundToTwoDecimal(this.All_lists.reduce((acc, item) => acc + parseFloat(item.outstanding_amount.replace(/,/g, '')), 0));

    this.AlltotalInvoiceAmount = this.formatNumber(AlltotalInvoiceAmount);
    this.AlltotalPaidAmount = this.formatNumber(AlltotalPaidAmount);
    this.AlltotalOutstandingAmount = this.formatNumber(AlltotalOutstandingAmount);
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

