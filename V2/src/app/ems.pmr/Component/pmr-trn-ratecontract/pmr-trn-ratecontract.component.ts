
import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { AES } from 'crypto-js';
interface product
{
  vendor_companyname:string,
  agreement_date:string,
  expairy_date:string,
  branch_gid:string,
}
@Component({
  selector: 'app-pmr-trn-ratecontract',
  templateUrl: './pmr-trn-ratecontract.component.html',
  styleUrls: ['./pmr-trn-ratecontract.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class PmrTrnRatecontractComponent  {
  contract_summarylist:any;
  responsedata: any;
  contractform:FormControl|any;
  product!: product;
  mdlbranch:any;
  Imsvendor_list:string[]=[];
  showOptionsDivId:any;
  parameterValue1: any;
  parameterValue:any;
  parameterValue2:any;
  constructor(private renderer: Renderer2,public NgxSpinnerService:NgxSpinnerService,private ToastrService: ToastrService,private el: ElementRef, public service: SocketService, private route: Router, private router: ActivatedRoute) {
    this.product = {} as product;
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetContractSummary()
    this.GetVendor()
    this.contractform = new FormGroup({
      vendor_companyname:new FormControl(''),
      agreement_date:new FormControl(''),
      expairy_date:new FormControl(''),
    });
  }
  GetContractSummary(){
    var url = 'PmrTrnRateContract/RateContractsummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.contract_summarylist = this.responsedata.contract_summarylist;
      setTimeout(() => {
        $('#contract_summarylist').DataTable();
      }, 1);
    });
  }
  GetVendor(){
var url='PmrTrnRateContract/Imsvendorcontract'
this.service.get(url).subscribe((result: any) => {
  this.responsedata = result; 
  this.Imsvendor_list = this.responsedata.Imsvendorrate_list;
});
  }
  get agreement_date() {
    return this.contractform.get('agreement_date')!;
  }
  get expairy_date() {
    return this.contractform.get('expairy_date')!;
  }
  get vendor_companyname() {
    return this.contractform.get('vendor_companyname')!;
  }
  onclose(){ 
    this.contractform.reset();
  }
  onadd(){
      this.route.navigate(['/pmr/PmrTrnCreatecontract']);
    }
  onsubmit() {
    debugger
    if (
      this.contractform.value.expairy_date != null) {
      for (const control of Object.keys(this.contractform.controls)) {
        this.contractform.controls[control].markAsTouched();
      }
      this.contractform.value;
      var url = 'PmrTrnRateContract/Postratecontract';
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.contractform.value)
        .subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.contractform.reset();
            this.GetContractSummary();
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.contractform.reset();
            this.GetContractSummary();
          }
        });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  onMapProd(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnrcproductassign',encryptedParam]);
  }
  openunassignproduct(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnRCproductremove',encryptedParam]);
  }
  onamend(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnRCproductamend',encryptedParam]);
  }
}



