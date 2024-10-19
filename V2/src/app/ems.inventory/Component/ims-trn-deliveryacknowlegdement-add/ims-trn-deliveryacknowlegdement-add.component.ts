import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-deliveryacknowlegdement-add',
  templateUrl: './ims-trn-deliveryacknowlegdement-add.component.html',
  styleUrls: ['./ims-trn-deliveryacknowlegdement-add.component.scss'],
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
export class ImsTrnDeliveryacknowlegdementAddComponent {

  deliveryadd_list:any[]=[];
  deliverycus_list:any[]=[];
  directorder_gid:any;
  product_list:any[]=[];
  parameterValue1: any;
  responsedata: any;
  showOptionsDivId: any; 

  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
    this.GetImsTrnDeliveryAcknowledgementSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
}
// // //// Summary Grid//////
GetImsTrnDeliveryAcknowledgementSummary() {
  debugger
  var url = 'ImsTrnDeliveryAcknowledgement/GetImsTrnDeliveryAcknowledgementAdd'
  this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
     this.responsedata = result;
     this.deliveryadd_list = result.deliveryadd_list;

     
     setTimeout(() => {
       $('#deliveryadd_list').DataTable();
             }, 1);
             this.NgxSpinnerService.hide()


  })
 
 
}
onadd(params:any){
  debugger
  const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnDeliveryacknowledgementUpdate',encryptedParam])
  


}
splitIntoLines(text: string, lineLength: number): string[] {
  const lines = [];
  for (let i = 0; i < text.length; i += lineLength) {
    lines.push(text.substr(i, lineLength));
  }
  return lines;
}
Details(parameter: string,directorder_gid: string){
  this.parameterValue1 = parameter;
  this.directorder_gid = parameter;

  var url='ImsTrnDeliveryAcknowledgement/GetProductdetails'
    let param = {
      directorder_gid : directorder_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.product_list = result.product_list;   
    });
  
}

}
