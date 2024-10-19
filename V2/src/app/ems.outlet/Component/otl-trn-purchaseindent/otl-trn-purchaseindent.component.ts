import { Component, DebugEventListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { FormBuilder, FormGroup } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-otl-trn-purchaseindent',
  templateUrl: './otl-trn-purchaseindent.component.html',
  styleUrls: ['./otl-trn-purchaseindent.component.scss']
})
export class OtlTrnPurchaseindentComponent{
  
  purchaserequest_list: any [] = [];
  responsedata: any;
  quotation_gid: any;
  parameterValue1: any;
  purchaserequisition_gid:any;
  productlistdetailspr:any;
  showOptionsDivId: any; 
  rows: any[] = [];

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: ActivatedRoute,private router: Router,private NgxSpinnerService:NgxSpinnerService) {
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ngOnInit(): void {
    this.GetPurchaseRequisitionSummary();
    
document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  GetPurchaseRequisitionSummary(){
    this.NgxSpinnerService.show();
    var url = 'OtlTrnPurchaseIndent/GetOtlTrnPurchaseIndent'
    this.service.get(url).subscribe((result: any) => {
     $('#purchaserequest_list').DataTable().destroy();
      this.responsedata = result;
      this.purchaserequest_list = this.responsedata.Summary_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#purchaserequest_list').DataTable()
      }, 1);
  
      this.NgxSpinnerService.hide();
    });
  
 }
 onadd()
 {
  this.router.navigate(['/outlet/OtlTrnTraisepurchaseindent'])
 }


  openModaledit()
  {

  }

  openModaladd()
  {

  }

  onview(params:any)
  {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/outlet/OtlTrnPurchaseindentview',encryptedParam]) 
  }
  
  PrintPDF(purchaserequisition_gid: any) {
    // API endpoint URL
    const api = 'PmrTrnPurchaseRequisition/GetPurchaseRequisitionRpt';
    this.NgxSpinnerService.show()
    let param = {
      purchaserequisition_gid:purchaserequisition_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }

  openModaldelete()
  {
    
  }
  Details(parameter: string,purchaserequisition_gid: string){
    this.parameterValue1 = parameter;
    this.purchaserequisition_gid = parameter;
  
    var url='OtlTrnPurchaseIndent/GetProductdetails'
      let param = {
        purchaserequisition_gid : purchaserequisition_gid 
      }
      this.service.getparams(url,param).subscribe((result:any)=>{
      this.responsedata=result;
       this.productlistdetailspr = result.productlistdetail;   
      });
    
  }

}
