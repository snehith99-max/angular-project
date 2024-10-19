import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-acc-rpt-ledgerassetreport',
  templateUrl: './acc-rpt-ledgerassetreport.component.html',
  styleUrls: ['./acc-rpt-ledgerassetreport.component.scss']
})

export class AccRptLedgerassetreportComponent {
  AssetLedgerReportForm!: FormGroup;
  Getassetledgerreport_list: any[] = [];
  accountgid: any;
  customergid: any;
  account_gid: any;
  customer_gid: any;
  customer_id: any;
  closingbalance: any;
  totalCredit: any;
  totalDebit: any;
  closingamount: any;
  closing_amount: any;
  customer_name: any;
  from_date: any = '';
  to_date: any = '';
  remarks: any;
  Account_name: any;
  Subgroup_name: any;

  constructor(private route: ActivatedRoute, private router: Router, private spinner: NgxSpinnerService, private ToastrService: ToastrService, private Service: SocketService) { }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    const key = 'storyboard';
    this.accountgid = this.route.snapshot.paramMap.get('accountgid');
    this.customergid = this.route.snapshot.paramMap.get('customergid');
    this.account_gid = AES.decrypt(this.accountgid, key).toString(enc.Utf8);
    this.customer_gid = AES.decrypt(this.customergid, key).toString(enc.Utf8);
    this.GetAssetLedgerReportDetails(this.account_gid, this.customer_gid, this.from_date, this.to_date);

    this.AssetLedgerReportForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }

  GetAssetLedgerReportDetails(account_gid: any, customer_gid: any, from_date: any, to_date: any) {
    debugger
    this.spinner.show();

    let param = {
      account_gid: account_gid,
      customer_gid: customer_gid,
      from_date: from_date,
      to_date: to_date
    }

    var summaryapi = 'AccTrnBankbooksummary/GetAssetLedgerReportDetails';
    this.Service.getparams(summaryapi, param).subscribe((result: any) => {
      this.Getassetledgerreport_list = result.Getassetledgerreport_list;
      console.log(this.Getassetledgerreport_list,'list')
      if (this.Getassetledgerreport_list != null) {
      this.customer_name = this.Getassetledgerreport_list[0].customer_name;
      this.customer_id = this.Getassetledgerreport_list[0].customer_id;
      this.Account_name = this.Getassetledgerreport_list[0].MainGroup_name;
      this.Subgroup_name = this.Getassetledgerreport_list[0].subgroup_name;
        this.closingbalance = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.closing_balance.replace(',', '')), 0);
        this.totalCredit = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = this.closingbalance;
        this.closingamount = this.formatValue1(this.closing_amount);
      }
      else{
        this.spinner.hide();
      }
      setTimeout(() => {
        $('#Getassetledgerreport_list').DataTable()
      }, 1);
      this.spinner.hide();
    });
  }

  parseValue1(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue1(value: number): string {
    // Format the number as a string with comma separators and two decimal places
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  OnChangeFinancialYear() {
    this.spinner.show();
    let param = {
      account_gid: this.account_gid,
      customer_gid: this.customer_gid,
      from_date: this.AssetLedgerReportForm.value.from_date,
      to_date: this.AssetLedgerReportForm.value.to_date
    }
    var searchapi = 'AccTrnBankbooksummary/GetAssetLedgerReportDetails';
    this.Service.getparams(searchapi, param).subscribe((result: any) => {
      this.Getassetledgerreport_list = result.Getassetledgerreport_list;
      this.closingbalance = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.closing_balance.replace(',', '')), 0);
        this.totalCredit = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.Getassetledgerreport_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);

        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = this.closingbalance;
        this.closingamount = this.formatValue1(this.closing_amount);
      this.spinner.hide();
    });
  }

  popmodal(remarks: any) {
    this.remarks = remarks;
  }
}