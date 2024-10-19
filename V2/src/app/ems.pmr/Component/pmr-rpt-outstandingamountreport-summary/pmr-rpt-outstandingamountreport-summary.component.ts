import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-pmr-rpt-outstandingamountreport-summary',
  templateUrl: './pmr-rpt-outstandingamountreport-summary.component.html',
  styleUrls: ['./pmr-rpt-outstandingamountreport-summary.component.scss']
})
export class PmrRptOutstandingamountreportSummaryComponent {

  outstandingamountreport_list:any;
  responsedata: any;
  from_date!: string;
  to_date!: string;
  maxDate!:string;
  Getoutstandingamountreportsearch : any [] =[];

  constructor(public service :SocketService,private route:Router,private ToastrService: ToastrService) {  
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    
    const today = new Date();
this.maxDate = today.toISOString().split('T')[0];
  
    this.GetOutstandingAmountReportSummary();
    
  }
  GetOutstandingAmountReportSummary(){
    debugger
    var url = 'PmrRptOutstandingAmountReport/GetOutstandingAmountReportSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#outstandingamountreport_list').DataTable().destroy();
      this.responsedata = result;
      this.outstandingamountreport_list = this.responsedata.Getoutstandingamountreport_list;
      setTimeout(() => {
        $('#outstandingamountreport_list').DataTable();
      }, 1);
    });
  }

  calculateTotal(): number {
    let totalAmount = 0;
    for (const data of this.outstandingamountreport_list) {
      totalAmount += parseFloat(data.invoice_amount.replace(/,/g, ''));
    }
    return totalAmount;
  }

  calculateTotal1(): number {
    let totalAmount = 0;
    for (const data of this.outstandingamountreport_list) {
      totalAmount += parseFloat(data.payment_amount.replace(/,/g, ''));
    }
    return totalAmount;
  }

  
  calculateTotal2(): number {
    let totalAmount = 0;
    for (const data of this.outstandingamountreport_list) {
      totalAmount += parseFloat(data.outstanding_amount.replace(/,/g, ''));
    }
    return totalAmount;
  }
  onrefreshclick(){
    this.from_date = null!;
    this.to_date = null!;
    this.GetOutstandingAmountReportSummary();
  }
  onSearchClick(){
    
    if(this.from_date != null && this.to_date != ""){ 

    }
    else{
      
      this.ToastrService.warning('Kindly Fill all the Mandatory Fields!');
      return;
    }

    var url = 'PmrRptOutstandingAmountReport/GetOutstandingAmountSearch';
    
    let params = {
      from_date: this.from_date,
      to_date: this.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#outstandingamountreport_list').DataTable().destroy();
      this.responsedata = result;
      this.outstandingamountreport_list = this.responsedata.Getoutstandingamountreport_list;
      setTimeout(() => {
        $('#outstandingamountreport_list').DataTable();
      }, 1);
  })
}

}
