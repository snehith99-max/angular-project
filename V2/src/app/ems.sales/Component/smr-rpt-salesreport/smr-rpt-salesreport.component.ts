import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
interface MonthwiseOrderReport {
  year: number;
  month_wise: any; // Adjust the type accordingly
}

@Component({
  selector: 'app-smr-rpt-salesreport',
  templateUrl: './smr-rpt-salesreport.component.html',
  styleUrls: ['./smr-rpt-salesreport.component.scss']
})


export class SmrRptSalesreportComponent {
  responsedata: any;
  GetSaleLedger_list: any[] = [];
  SalesLedgerForm!: FormGroup;
  constructor(private formBuilder: FormBuilder, private router: Router, private ToastrService: ToastrService, public service: SocketService, public NgxSpinnerService: NgxSpinnerService,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.GetSalesledgersummary();
    this.SalesLedgerForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }


  GetSalesledgersummary() {
    debugger
    var url = 'SmrRptSalesReport/GetsalesReportsummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#GetSaleLedger_list').DataTable().destroy();
      this.responsedata = result;
      this.GetSaleLedger_list = this.responsedata.GetSalesReport_list;
      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()

    });

  }
  click360(customergid: any) {
    debugger
    const key = 'storyboard';
    const param = customergid;
    const lspage1 = 'SmrRptSalesreport';
    const customer_gid = AES.encrypt(param, key).toString();
    const lspage = AES.encrypt(lspage1, key).toString();
    this.router.navigate(['/smr/SmrRptSales360', customer_gid,lspage]);
  }
  onclick(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrRptSalesreportview', encryptedParam])
  }
  OnChangeFinancialYear() {
    this.NgxSpinnerService.show();
    let param = {
      from_date: this.SalesLedgerForm.value.from_date,
      to_date: this.SalesLedgerForm.value.to_date
    }
    var api = 'SmrRptSalesReport/GetsalesReportdate';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.GetSaleLedger_list = result.GetSalesReport_list;
      this.NgxSpinnerService.hide();
    });
  }

  onrefreshclick() {
    this.SalesLedgerForm.value.from_date = null!;
    this.SalesLedgerForm.value.to_date = null!;
    this.GetSalesledgersummary();
  }
}