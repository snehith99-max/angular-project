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
  selector: 'app-ims-rpt-grnreport',
  templateUrl: './ims-rpt-grnreport.component.html',
  styleUrls: ['./ims-rpt-grnreport.component.scss']
})
export class ImsRptGrnreportComponent {
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
    this.GetImsRptGrnreport();
    this.GRNform = new FormGroup({
      branch_name: new FormControl(''),
      vendor_name: new FormControl(''),
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
    var url = 'ImsRptGrnReport/GetBranch'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.branch;  
    });
    var url = 'ImsRptGrnReport/GetVendor'
    this.service.get(url).subscribe((result: any) => {
      this.vendor_list = result.vendor_list;  
    });
  }


  GetImsRptGrnreport(){
    debugger
    var url = 'ImsRptGrnReport/GetImsRptGrnreport'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
     $('#GetSaleLedger_list').DataTable().destroy();
      this.GetSaleLedger_list = result.grn_list;

      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide()
  
    });

  }
  onrefreshclick(){
    window.location.reload();
    this.GetImsRptGrnreport();
  }
  OnChangeFinancialYear(){
    debugger;
    this.NgxSpinnerService.show();
    if(this.GRNform.value.branch_name || this.GRNform.value.vendor_name || this.GRNform.value.from_date!=null && this.GRNform.value.to_date!=""){
    }
    else{
      this.NgxSpinnerService.hide();
      this.ToastrService.warning("Kindly Fill All The Dates..")
      return;
    }
    let param = {
      branch_gid : this.GRNform.value.branch_name,
      vendor_gid : this.GRNform.value.vendor_name,
      from_date : this.GRNform.value.from_date,
      to_date : this.GRNform.value.to_date
    }
    var api = 'ImsRptGrnReport/GetImsRptGrnreportsearch';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.GetSaleLedger_list = result.grn_list;
      setTimeout(() => {
        $('#GetSaleLedger_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide();
    });    
  }
  onview(params: any) {

    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const lspage1 = 'Inventory';
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1,secretKey).toString();
    this.router.navigate(['./pmr/PmrTrnGrninwardView', encryptedParam,lspage])
  }

  PrintPDF(grn_gid: any) {
    debugger
    let param = { grn_gid: grn_gid}
   this.NgxSpinnerService.show();
   var PDFapi = 'PmrTrnGrn/GetGRNPDF';
   this.service.getparams(PDFapi, param).subscribe((result : any)=>{
    if(result!=null){
      this.service.filedownload1(result);
    }
    else{
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
    }
    this.NgxSpinnerService.hide();
   });
  }

  onclearbranch(){
    this.GetSaleLedger_list=[]
  }
  }



