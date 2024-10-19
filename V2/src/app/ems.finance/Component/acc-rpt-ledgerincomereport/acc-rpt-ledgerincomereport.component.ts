import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { FormControl, FormGroup } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-acc-rpt-ledgerincomereport',
  templateUrl: './acc-rpt-ledgerincomereport.component.html',
  styleUrls: ['./acc-rpt-ledgerincomereport.component.scss']
})
export class AccRptLedgerincomereportComponent {

  IncomeDetailedForm!: FormGroup;
  IncomeLedgerReportView_List: any[] = [];
  
  account_gid: any;
  responsedata: any;
  journal_gid: any;
  totalCredit: any;
  closingbalance: any;
  totalDebit: any;
  closing_amount: any;
  closingamount: any;
  customerdetails: any[] = []
  customer_code: any;
  customer_name: any;
  from_date: any = '';
  to_date: any = '';
  maxDate!: string;
  IncomeReportMonthwise_List: any;

  constructor(public service: SocketService, 
    private router: ActivatedRoute, 
    private route: Router, 
    private NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    const today = new Date();
    this.maxDate = today.toISOString().split('T')[0];
    flatpickr('.date-picker', options);

    const account_gid = this.router.snapshot.paramMap.get('account_gid');
    this.account_gid = account_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.account_gid, secretKey).toString(enc.Utf8);
    this.account_gid = deencryptedParam;
    console.log(deencryptedParam)

    this.IncomeDetailedForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    this.GetCustomerName(deencryptedParam);
    this.GetAccTrnIncomeReportView(deencryptedParam, this.from_date, this.to_date);
  }

  GetAccTrnIncomeReportView(account_gid: any, from_date: any, to_date: any) {
    debugger
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetIncomeLedgerReportDetailsList'
    let param = {
      account_gid: account_gid,
      from_date: from_date,
      to_date: to_date
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.IncomeLedgerReportView_List = result.GetIncomeLedgerReportView_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#IncomeLedgerReportView_List').DataTable();
      }, 1);

      if (this.IncomeLedgerReportView_List != null) {
        this.closingbalance = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.closingbalance.replace(',', '')), 0);
        this.totalCredit = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount =  this.closingbalance;
        this.closingamount = this.formatValue1(this.closing_amount);
      }
    });
  }

  GetCustomerName(account_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetIncomeLedgerReportDetails'

    let param = {
      account_gid: account_gid,
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.customerdetails = result.GetIncomeLedgerReportDetails_List;
      this.customer_code = this.customerdetails[0].customer_code
      this.customer_name = this.customerdetails[0].customer_name
      this.NgxSpinnerService.hide();
    });
  }

  onSearchClick() {
    this.NgxSpinnerService.show();

    var url = 'AccTrnBankbooksummary/GetIncomeReportMonthwise'
    let params = {
      account_gid: this.account_gid,
      from_date: this.IncomeDetailedForm.value.from_date,
      to_date: this.IncomeDetailedForm.value.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#IncomeReportMonthwise_List').DataTable().destroy();
      this.responsedata = result;
      this.IncomeLedgerReportView_List = result.GetIncomeReportMonthwise_List;
      if (this.IncomeLedgerReportView_List != null) {
        this.closingbalance = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.closingbalance.replace(',', '')), 0);
        this.totalCredit = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.IncomeLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount =  this.closingbalance;
        this.closingamount = this.formatValue1(this.closing_amount);
      }
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#IncomeReportMonthwise_List').DataTable();
      }, 1);
    });
  } 

  parseValue1(value: any): number {
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue1(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }
}
