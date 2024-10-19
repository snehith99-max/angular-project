import { Component } from '@angular/core';

import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Pipe } from '@angular/core';
@Component({
  selector: 'app-smr-rpt-ouststandingamount-report',
  templateUrl: './smr-rpt-ouststandingamount-report.component.html',
  styleUrls: ['./smr-rpt-ouststandingamount-report.component.scss']
})
export class SmrRptOuststandingamountReportComponent {
  response_data: any;
  outstandingamountlist:any[]=[];
  totalInvoiceAmount: number = 0;
  totalPaidAmount: number = 0;
  from_date!: string;
  to_date!: string;
  maxDate!:string;
  totalOutstandingAmount: number = 0;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
  }
  ngOnInit(): void {
    var api = 'OutStandingAmount/GetOutstandingAmountReportSummary';
    this.NgxSpinnerService.show();
    let params = {
      from_date:"",
      to_date: ""
    }
    this.service.getparams(api,params).subscribe((result: any) => {
      $('#outstandingamountlist').DataTable().destroy();
      this.response_data = result;
      this.outstandingamountlist = this.response_data.GetOutsrandingAmountResult;
      this.totalInvoiceAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.invoice_amount), 0));
    this.totalPaidAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.paid_amount), 0));
    this.totalOutstandingAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.outstanding_amount), 0));
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#outstandingamountlist').DataTable();
      }, 1);
    });
    const today = new Date();
    this.maxDate = today.toISOString().split('T')[0];
  }
  roundToTwoDecimal(value: number): number {
    return Math.round(value * 100) / 100;
  }
  onSearchClick(){
    var api = 'OutStandingAmount/GetOutstandingAmountReportSummary';
    this.NgxSpinnerService.show();
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(api,params).subscribe((result: any) => {
      this.response_data = result;
      $('#outstandingamountlist').DataTable().destroy();
      this.outstandingamountlist = this.response_data.GetOutsrandingAmountResult;
      this.totalInvoiceAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.invoice_amount), 0));
    this.totalPaidAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.paid_amount), 0));
    this.totalOutstandingAmount = this.roundToTwoDecimal(this.outstandingamountlist.reduce((acc, item) => acc + parseFloat(item.outstanding_amount), 0));
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#outstandingamountlist').DataTable();
      }, 1);
    });
  }
  onrefreshclick() {
    this.from_date = null!;
    this.to_date = null!;
    this.ngOnInit();
  }

 
  }



