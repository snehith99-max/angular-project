import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExcelService } from 'src/app/Service/excel.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pmr-rpt-vendorledgerreport',
  templateUrl: './pmr-rpt-vendorledgerreport.component.html',
  styleUrls: ['./pmr-rpt-vendorledgerreport.component.scss']
})
export class PmrRptVendorledgerreportComponent {

  vendorledger_list :any[]=[];
  responsedata:any;
  VendorLedgerForm: FormGroup | any; 
  GetVendorLedger_list : any []=[];

  constructor(private formBuilder: FormBuilder, private excelService:ExcelService,
    private ToastrService: ToastrService, private router: ActivatedRoute, 
    private route: Router, public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService,) {
    
  }
  ngOnInit(): void{
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.VendorledgerReportSummary();
    this.VendorLedgerForm = new FormGroup({
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }

  OnChangeFinancialYear(){
    this.NgxSpinnerService.show();
    let param = {
      from_date : this.VendorLedgerForm.value.from_date,
      to_date : this.VendorLedgerForm.value.to_date
    }
    var api = 'PmrRptVendorLedgerreport/GetVendorLedgerdate';
    this.service.getparams(api,param).subscribe((result: any)=>{
      $('#vendorledger_list').DataTable().destroy();
      this.vendorledger_list = result.vendorledger_list;
      setTimeout(() => {
        $('#vendorledger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide();
    });    
  }
  

  VendorledgerReportSummary(){
    var url = 'PmrRptVendorLedgerreport/GetVendorledgerReportSummary'
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
    $('#vendorledger_list').DataTable().destroy();
     this.responsedata = result;
     this.vendorledger_list = this.responsedata.vendorledger_list;
     //console.log(this.entity_list)
     setTimeout(() => {
       $('#vendorledger_list').DataTable()
     }, 1);
     this.NgxSpinnerService.hide();

    });
  }
  click360(vendor_gid: any){
    debugger
    const key = 'storyboarderp';
    const param = vendor_gid;
    const vendorgid = AES.encrypt(param,key).toString();
    this.route.navigate(['/pmr/PmrRptVendor360',vendorgid]);
  }
  onrefreshclick(){
    this.VendorLedgerForm.value.from_date = null!;
    this.VendorLedgerForm.value.to_date = null!;
    this.VendorledgerReportSummary();
  }
  onview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrRptVendorLedgerReportView',encryptedParam])
     
  }
}
