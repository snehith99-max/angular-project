import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-issue',
  templateUrl: './ims-trn-issue.component.html',
  styleUrls: ['./ims-trn-issue.component.scss']
})
export class ImsTrnIssueComponent {
  issuematerialselect_list: any[] = [];
  responsedata: any;
  productsummary_list:any
  constructor(public service :SocketService,private route:Router,public NgxSpinnerService:NgxSpinnerService) {
  }
   ngOnInit(): void {
   this.GetSummary();
  } 
  GetSummary(){
  var api='ImsTrnIssueMaterial/GetIssueMaterialselect'
  this.NgxSpinnerService.show()
  this.service.get(api).subscribe((result:any)=>{
    this.responsedata=result;
    this.issuematerialselect_list = this.responsedata.issuematerialselect_list;  
   console.log(this.issuematerialselect_list)
    setTimeout(()=>{   
      $('#issuematerialselect_list').DataTable();
    }, 1);
  this.NgxSpinnerService.hide()
});
}
Onclick(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnRequestedissue',encryptedParam]) 
}

Getproduct(materialrequisition_gid: any){
  debugger;
  let param = {
    materialrequisition_gid:materialrequisition_gid
  } 
  var url = 'ImsTrnIssueMaterial/GetDetialViewProduct'
  this.service.getparams(url,param).subscribe((result: any) => {
    debugger
    $('#productsummary_list').DataTable().destroy();
    this.responsedata = result;
    this.productsummary_list = this.responsedata.materialproduct_list;
    setTimeout(() => {
      $('#productsummary_list').DataTable();
    }, 1);
  });
}
}
