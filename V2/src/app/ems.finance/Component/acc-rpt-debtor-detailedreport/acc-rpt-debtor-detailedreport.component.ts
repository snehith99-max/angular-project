import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-acc-rpt-debtor-detailedreport',
  templateUrl: './acc-rpt-debtor-detailedreport.component.html',
})
export class AccRptDebtorDetailedreportComponent {

  DetailedReportForm!: FormGroup;
  Getsubbankbook_list: any[]=[];
  accountgid: any;
  customergid: any;
  account_gid: any;
  customer_gid: any;
  customer_id: any;
  totalCredit: any;
  tds: any;
  totalDebit: any;
  totalBalance: any;
  closingamount: any;
  closing_amount: any;
  customer_name: any;
  from_date: any = '';
  to_date: any = '';
  remarks: any;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private spinner : NgxSpinnerService,
    private ToastrService: ToastrService,
    private Service : SocketService
  ) { }

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
    this.GetDebtorDetailedReportSummary(this.account_gid,this.customer_gid, this.from_date, this.to_date);

    this.DetailedReportForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }
  GetDebtorDetailedReportSummary(account_gid: any, customer_gid: any,from_date: any, to_date: any) {
    this.spinner.show();
    let param = {
      account_gid: account_gid,
      customer_gid: customer_gid,
      from_date: from_date,
      to_date: to_date
    }
    var summaryapi = 'AccTrnBankbooksummary/GetDebtorDetailedReport';
    this.Service.getparams(summaryapi,param).subscribe((result:any)=>{
      this.Getsubbankbook_list = result.Getsubbankbook_list;
      if(this.Getsubbankbook_list == null){
        this.spinner.hide();
      }
      else{
      this.customer_name = result.Getsubbankbook_list[0].customer_name;
      this.customer_id = result.Getsubbankbook_list[0].customer_id;
debugger
      if (this.Getsubbankbook_list != null ) {
        this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.tds = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.tds.replace(',', '')), 0);
        this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
        this.totalBalance = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.closing_balance.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);
        this.totalBalance = formatter.format(this.totalBalance);

  
        let totcrd = this.parseValue1(this.totalCredit);
        let totdeb = this.parseValue1(this.totalDebit);

        this.closing_amount = totdeb - totcrd;

        this.closingamount = this.formatValue1(this.closing_amount);
      }
    }
      setTimeout(()=> {
        $('#Getsubbankbook_list').DataTable()
      },1); 
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
    if (this.DetailedReportForm.value.from_date == "" || this.DetailedReportForm.value.to_date == "") {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Please Enter From and To Date!');
      return;
    }
    // Validate that toDate does not exceed fromDate
    // Convert date strings from DD-MM-YYYY to a format JavaScript can parse
      const fromDateParts = this.DetailedReportForm.value.from_date.split('-');
      const toDateParts = this.DetailedReportForm.value.to_date.split('-');

      const fromDate = new Date(`${fromDateParts[2]}-${fromDateParts[1]}-${fromDateParts[0]}`);
      const toDate = new Date(`${toDateParts[2]}-${toDateParts[1]}-${toDateParts[0]}`);

    // Validate that toDate does not exceed fromDate
    if (toDate < fromDate) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('To Date cannot be earlier than From Date!');
      return;
    }
 
    this.spinner.show();
    let param = {
      account_gid : this.account_gid,
      customer_gid : this.customer_gid,
      from_date : this.DetailedReportForm.value.from_date,
      to_date : this.DetailedReportForm.value.to_date
    }
    var searchapi = 'AccTrnBankbooksummary/GetDebtorDetailedReport';
    this.Service.getparams(searchapi,param).subscribe((result: any)=>{
      this.Getsubbankbook_list = result.Getsubbankbook_list;
    if (this.Getsubbankbook_list != null ) {
      this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
      this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
      this.totalBalance = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.closing_balance.replace(',', '')), 0);

      // Format the totals with commas and .00
      const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      });

      this.totalCredit = formatter.format(this.totalCredit);
      this.totalDebit = formatter.format(this.totalDebit);
      this.totalBalance = formatter.format(this.totalBalance);


      let totcrd = this.parseValue1(this.totalCredit);
      let totdeb = this.parseValue1(this.totalDebit);

      this.closing_amount = totdeb - totcrd;

      this.closingamount = this.formatValue1(this.closing_amount);
    }  
    else{
      this.totalCredit = "0.00";
      this.totalDebit = "0.00";
      this.totalBalance = "0.00";
    } 
    setTimeout(()=> {
      $('#Getsubbankbook_list').DataTable()
    },1); 
    this.spinner.hide();
  });
  }
  popmodal(remarks: any){
    this.remarks = remarks;
  }
}
