import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-ims-rpt-stockstatus-report',
  templateUrl: './ims-rpt-stockstatus-report.component.html',
  styleUrls: ['./ims-rpt-stockstatus-report.component.scss']
})
export class ImsRptStockstatusReportComponent {
  maxDate!:string;
  responsedata:any;
  GetSaleLedger_list:any[]=[];
  StockStatusform!: FormGroup;
  Quantity:string='';
  constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.GetImsRptStockStatusreport();
    this.StockStatusform = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }


  GetImsRptStockStatusreport(){
    debugger
    var url = 'ImsRptStockStatusReport/GetImsRptStockStatusreport'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     $('#GetSaleLedger_list').DataTable().destroy();
      this.GetSaleLedger_list = result.stocklist;

      const Quantity = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.stock_qty.replace(/,/g, '')), 0));
    this.Quantity = this.formatNumber(Quantity);
      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()
  
    });

  }
  onrefreshclick(){
    window.location.reload();
    this.GetImsRptStockStatusreport();
  }
  OnChangeFinancialYear(){
    this.NgxSpinnerService.show();
    if(this.StockStatusform.value.from_date!=null && this.StockStatusform.value.to_date!=""){
    }
    else{
      this.NgxSpinnerService.hide();
      this.ToastrService.warning("Kindly Fill All The Dates..")
      return;
    }
    let param = {
      from_date : this.StockStatusform.value.from_date,
      to_date : this.StockStatusform.value.to_date
    }
    var api = 'ImsRptStockStatusReport/GetStockStatusdate';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.GetSaleLedger_list = result.stocklist;
      const Quantity = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.stock_qty.replace(/,/g, '')), 0));
      this.Quantity = this.formatNumber(Quantity);
      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide();
    });   
  } 
  
  formatNumber(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }
  roundToTwoDecimal(value: number): number {
    return Math.round(value * 100) / 100;
  }
  }



