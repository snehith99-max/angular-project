import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { FormControl, FormGroup } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-acc-rpt-ledgerexpensereport',
  templateUrl: './acc-rpt-ledgerexpensereport.component.html',
  styleUrls: ['./acc-rpt-ledgerexpensereport.component.scss']
})

export class AccRptLedgerexpensereportComponent {

  maxDate!: string;
  account_gid: any;
  responsedata: any;
  totalCredit: any;
  totalDebit: any;
  closing_amount: any;
  closingamount: any;
  from_date: any = '';
  to_date: any = '';

  ExpenseDetailedForm!: FormGroup;
  ExpenseLedgerReportView_List: any[] = [];
  vebdordetails: any[] = []
  vendor_code: any;
  vendor_companyname: any;
  ExpenseReportMonthwise_List: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
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

    this.ExpenseDetailedForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    this.GetAccTrnExpenseReportView(deencryptedParam, this.from_date, this.to_date);
  }

  GetAccTrnExpenseReportView(account_gid: any, from_date: any, to_date: any) {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetExpenseLedgerReportDetailsList'
    let param = {
      account_gid: account_gid,
      from_date: from_date,
      to_date: to_date
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.ExpenseLedgerReportView_List = result.GetExpenseLedgerReportView_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#ExpenseLedgerReportView_List').DataTable();
      }, 1);

      if (this.ExpenseLedgerReportView_List != null) {
        this.closingamount = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.closingbalance.replace(',', '')), 0);
        this.totalCredit = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = this.closingamount;
        this.closingamount = this.formatValue1(this.closing_amount);
      }
    });
  }

  GetVendorName(account_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetExpenseLedgerReportDetails'

    let param = {
      account_gid: account_gid,
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.vebdordetails = result.GetExpenseLedgerReportDetails_List;
      this.vendor_code = this.vebdordetails[0].vendor_code
      this.vendor_companyname = this.vebdordetails[0].vendor_companyname
      this.NgxSpinnerService.hide();
    });
  }

  onSearchClick() {
    this.NgxSpinnerService.show();

    var url = 'AccTrnBankbooksummary/GetExpenseReportMonthwise'
    let params = {
      account_gid: this.account_gid,
      from_date: this.ExpenseDetailedForm.value.from_date,
      to_date: this.ExpenseDetailedForm.value.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#ExpenseReportMonthwise_List').DataTable().destroy();
      this.responsedata = result;
      this.ExpenseLedgerReportView_List = result.GetExpenseReportMonthwise_List;
      if (this.ExpenseLedgerReportView_List != null) {
        this.closingamount = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.closingbalance.replace(',', '')), 0);
        this.totalCredit = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.ExpenseLedgerReportView_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = this.closingamount;
        this.closingamount = this.formatValue1(this.closing_amount);
      }
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#ExpenseReportMonthwise_List').DataTable();
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
