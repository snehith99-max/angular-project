import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IEmployee {
}

@Component({
  selector: 'app-ims-trn-pendingmaterialissue',
  templateUrl: './ims-trn-pendingmaterialissue.component.html',
  styleUrls: ['./ims-trn-pendingmaterialissue.component.scss']
})
export class ImsTrnPendingmaterialissueComponent {
  pendingmaterial_list: any[] = [];
  productsummary_list:any[]=[];
  employee: IEmployee;
  reactiveFormReset: any;
  reactiveFormUpdateUserCode: any;
  responsedata: any;
  employee_list: any;

  constructor(public service :SocketService,private route:Router,public NgxSpinnerService:NgxSpinnerService) {
    this.employee = {} as IEmployee;
  }
 
  ngOnInit(): void {
    this.reactiveFormReset = new FormGroup({
    });
    this.reactiveFormUpdateUserCode = new FormGroup({

    });
   this.GetPendingMaterialIssueSummary();
  } 
  GetPendingMaterialIssueSummary(){
  var api='ImsTrnPendingMaterialIssue/GetPendingMaterialIssueSummary'
  this.NgxSpinnerService.show()
    
  this.service.get(api).subscribe((result:any)=>{
 
    this.responsedata=result;
    this.pendingmaterial_list = this.responsedata.pendingmaterialissue_list;  
   console.log(this.pendingmaterial_list)
    setTimeout(()=>{   
      $('#pendingmaterial_list').DataTable();
    }, 1);
  this.NgxSpinnerService.hide()
});
}
Onclickgrn(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnRaiseMaterialIndent',encryptedParam]) 
}
Getproduct(materialrequisition_gid: any){
  let param = {
    materialrequisition_gid:materialrequisition_gid
  } 
  var url = 'ImsTrnPendingMaterialIssue/GetDetailPopup'
  this.service.getparams(url,param).subscribe((result: any) => {
    debugger
    $('#productsummary_list').DataTable().destroy();
    this.responsedata = result;
    this.productsummary_list = this.responsedata.detialspop_list;
    setTimeout(() => {
      $('#productsummary_list').DataTable();
    }, 1);
  });
}
}
