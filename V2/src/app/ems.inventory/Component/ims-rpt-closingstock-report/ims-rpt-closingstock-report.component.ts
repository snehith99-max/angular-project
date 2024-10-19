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
  selector: 'app-ims-rpt-closingstock-report',
  templateUrl: './ims-rpt-closingstock-report.component.html',
  styleUrls: ['./ims-rpt-closingstock-report.component.scss']
})
export class ImsRptClosingstockReportComponent {

  responsedata:any;
  GetSaleLedger_list:any[]=[];
  mdlBranchName:any;
  location_list:any[]=[];
  closingstock!: FormGroup;
  Stock:string='';
  Issue:string='';
  Amend:string='';
  Damage:string='';
  Adjust:string='';
  transfer:string='';
  Available:string='';
  constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }

  ngOnInit(): void {
    this.GetClosingStockreport();
    this.closingstock = new FormGroup({
      location_name: new FormControl(''),

    })
    
    var url = 'ImsRptClosingStock/GetLocation'
    this.service.get(url).subscribe((result: any) => {
      this.location_list = result.location;  
    });
  }


  GetClosingStockreport(){
    
  var url = 'ImsRptClosingStock/GetClosingStockreport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.GetSaleLedger_list = this.responsedata.closingstock_list;

    const Stock = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.total_Stock_Quantity.replace(/,/g, '')), 0));
    const Issue = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.issued_Quantity.replace(/,/g, '')), 0));
    const Amend = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.amended_Quantity.replace(/,/g, '')), 0));
    const Damage = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.damaged_qty.replace(/,/g, '')), 0));
    const Adjust = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.adjusted_qty.replace(/,/g, '')), 0));
    const transfer = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.transfer_qty.replace(/,/g, '')), 0));
    const Available = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.available_quantity.replace(/,/g, '')), 0));
    this.Stock = this.formatNumber(Stock);
    this.Issue = this.formatNumber(Issue);
    this.Amend = this.formatNumber(Amend);
    this.Damage = this.formatNumber(Damage);
    this.Adjust = this.formatNumber(Adjust);
    this.transfer = this.formatNumber(transfer);
    this.Available = this.formatNumber(Available);
    setTimeout(()=>{  
      $('#GetSaleLedger_list').DataTable();
    }, 1);
    this.NgxSpinnerService.hide()
  
    });

  }
  onrefreshclick(){
    this.closingstock.reset();
    this.GetClosingStockreport();
  }
  OnChangeFinancialYear(){
    debugger;
    this.NgxSpinnerService.show();
    let param = {
      location_gid : this.closingstock.value.location_name
    
    }
    var api = 'ImsRptClosingStock/GetClosingStocklocation';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.GetSaleLedger_list = result.closingstock_list;
      

      if( this.GetSaleLedger_list!=null){

      const Stock = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.total_Stock_Quantity.replace(/,/g, '')), 0));
      const Issue = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.issued_Quantity.replace(/,/g, '')), 0));
      const Amend = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.amended_Quantity.replace(/,/g, '')), 0));
      const Damage = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.damaged_qty.replace(/,/g, '')), 0));
      const Adjust = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.adjusted_qty.replace(/,/g, '')), 0));
      const transfer = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.transfer_qty.replace(/,/g, '')), 0));
      const Available = this.roundToTwoDecimal(this.GetSaleLedger_list.reduce((acc, item) => acc + parseFloat(item.available_quantity.replace(/,/g, '')), 0));
      this.Stock = this.formatNumber(Stock);
      this.Issue = this.formatNumber(Issue);
      this.Amend = this.formatNumber(Amend);
      this.Damage = this.formatNumber(Damage);
      this.Adjust = this.formatNumber(Adjust);
      this.transfer = this.formatNumber(transfer);
      this.Available = this.formatNumber(Available);
      setTimeout(()=>{  
        $('#GetSaleLedger_list').DataTable();
      }, 1);
    }
    else{
      this.ToastrService.warning("No record is found")
    }
      
    });  
    this.NgxSpinnerService.hide();  
  }
  formatNumber(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }
  roundToTwoDecimal(value: number): number {
    return Math.round(value * 100) / 100;
  }
  }



