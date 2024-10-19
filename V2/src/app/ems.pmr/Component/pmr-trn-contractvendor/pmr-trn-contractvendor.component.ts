
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
  selector: 'app-pmr-trn-contractvendor',
  templateUrl: './pmr-trn-contractvendor.component.html',
  styleUrls: ['./pmr-trn-contractvendor.component.scss'],
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
export class PmrTrnContractvendorComponent  {
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
  }
  GetContractSummary(){
    var url = 'PmrTrnRateContract/vendorposummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.contract_summarylist = this.responsedata.contract_summarylist;
      setTimeout(() => {
        $('#contract_summarylist').DataTable();
      }, 1);
    });
  }
  onclose(){ 
    this.contractform.reset();
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  RCPurchaseOrder(ratecontract_gid:any,vendor_gid:any){
    debugger
    const secretKey = 'storyboarderp';
    const encryptedratecontract_gid = AES.encrypt(ratecontract_gid, secretKey).toString();
    const encryptedVendorGid = AES.encrypt(vendor_gid, secretKey).toString();
    this.route.navigate(['/pmr/PmrTrnContractPO',encryptedratecontract_gid, encryptedVendorGid]);
  }
}



