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
  selector: 'app-smr-rpt-customerreport-view',
  templateUrl: './smr-rpt-customerreport-view.component.html',
  styleUrls: ['./smr-rpt-customerreport-view.component.scss']
})
export class SmrRptCustomerreportViewComponent {

  DetailedReportForm!: FormGroup;
  Getsubbankbook_list: any[]=[];
  Getsubbankbook_list1 : any[] =[];
  accountgid: any;
  customergid: any;
  account_gid: any;
  customer_gid: any;
  customer_id: any;
  totalCredit: any;
  totalDebit: any;
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
    this.customergid = this.route.snapshot.paramMap.get('customergid');
    this.customer_gid = AES.decrypt(this.customergid, key).toString(enc.Utf8);
    this.GetDebtorDetailedReportSummary(this.customer_gid, this.from_date, this.to_date);
    this.GetCustomerName(this.customer_gid)

    this.DetailedReportForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }

  GetCustomerName(customer_gid : any){
    let param = {
      customer_gid : customer_gid
    }
    var url = 'SmrRptCustomerReport/GetCustomerReportSummary';
    this.Service.getparams(url,param).subscribe((result : any)=>{
      this.Getsubbankbook_list1 = result.Getcustomerreport_List;
      this.customer_name = result.Getcustomerreport_List[0].customer;
      this.customer_id = result.Getcustomerreport_List[0].customer_id;
    })
  }
  GetDebtorDetailedReportSummary(customer_gid: any,from_date: any, to_date: any) {
    debugger
    this.spinner.show();
    let param = {
      customer_gid: customer_gid,
      from_date: from_date,
      to_date: to_date
    }
    var summaryapi = 'SmrRptCustomerReport/GetCustomerDetailedReport';
    this.Service.getparams(summaryapi,param).subscribe((result:any)=>{
      this.Getsubbankbook_list = result.Getcustomerreport_List;
      // this.customer_name = result.Getcustomerreport_List[0].customer;
      // this.customer_id = result.Getcustomerreport_List[0].customer_id;
      if (this.Getsubbankbook_list != null) {
        this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.outstanding_amount.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
       
      }
      setTimeout(()=> {
        $('#Getsubbankbook_list').DataTable()
      },1); 
      
    });
    this.spinner.hide();
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
      account_gid : this.account_gid,
      customer_gid : this.customer_gid,
      from_date : this.DetailedReportForm.value.from_date,
      to_date : this.DetailedReportForm.value.to_date
    }
    var searchapi = 'SmrRptCustomerReport/GetCustomerDetailedReport';
    this.Service.getparams(searchapi,param).subscribe((result: any)=>{
      this.Getsubbankbook_list = result.Getcustomerreport_List;
      if (this.Getsubbankbook_list != null) {
        this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.outstanding_amount.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
       
      }
     
    });    
    this.spinner.hide();
  }
  popmodal(remarks: any){
    this.remarks = remarks;
  }
}

