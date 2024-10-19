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
  selector: 'app-ims-rpt-grndetailreport',
  templateUrl: './ims-rpt-grndetailreport.component.html',
  styleUrls: ['./ims-rpt-grndetailreport.component.scss']
})
export class ImsRptGrndetailreportComponent {
  maxDate!:string;
  responsedata:any;
  GetSaleLedger_list:any[]=[];
  GRNform!: FormGroup;
  branch_list:any[]=[];
  vendor_list:any[]=[];
  mdlBranchName:any;
  mdlvendorname:any;
  constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.GetImsRptGrndetailreport();
    this.GRNform = new FormGroup({
      vendor_name: new FormControl(''),
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    var url = 'ImsRptGrnDetailReport/GetVendor'
    this.service.get(url).subscribe((result: any) => {
      this.vendor_list = result.vendor_lists;  
    });
  }


  GetImsRptGrndetailreport(){
    debugger
    var url = 'ImsRptGrnDetailReport/GetImsRptGrndetailreport'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     $('#GetSaleLedger_list').DataTable().destroy();
      this.GetSaleLedger_list = result.grn_lists;

      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()
  
    });

  }
  onrefreshclick(){
    this.GRNform.reset();
    this.GetImsRptGrndetailreport();
  }
  OnChangeFinancialYear(){
    debugger;
    this.NgxSpinnerService.show();
    let param = {
      vendor_gid : this.GRNform.value.vendor_name,
      from_date : this.GRNform.value.from_date,
      to_date : this.GRNform.value.to_date
    }
    var api = 'ImsRptGrnDetailReport/GetImsRptGrndetailreportsearch';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.GetSaleLedger_list = result.grn_lists;
      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide();
    });    
  }
  

  onclearbranch(){
    this.GetSaleLedger_list=[]
  }
  }



