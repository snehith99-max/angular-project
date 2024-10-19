

import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-ims-trn-opendc',
  templateUrl: './ims-trn-opendc.component.html',
  styleUrls: ['./ims-trn-opendc.component.scss'],
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
export class ImsTrnOpendcComponent {
  opndcsummary_list :any[]=[];
  responsedata: any;
  company_code: any;
  product_list:any[]=[];
  directorder_gid:any;
  parameterValue1:any;
  showOptionsDivId:any;


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
    this.GetImsTrnOpenDcSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
}
PrintPDF(directorder_gid: any) {
  // API endpoint URL
  const api = 'ImsTrnOpenDcSummary/GetOpenDCpdf';
  this.NgxSpinnerService.show()
  let param = {
    directorder_gid:directorder_gid
  }
  this.service.getparams(api,param).subscribe((result: any) => {
    if(result!=null){
      this.service.filedownload1(result);
    }
    this.NgxSpinnerService.hide()
  });
}

///Summary ////
GetImsTrnOpenDcSummary() {
  debugger
  var url = 'ImsTrnOpenDcSummary/GetImsTrnOpenDCSummary'
  this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
     this.responsedata = result;
     this.opndcsummary_list = result.opndcsummary_list;
     setTimeout(() => {
       $('#opndcsummary_list').DataTable();
             }, 1);
             this.NgxSpinnerService.hide();
  })
 
 
}
onView(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnViewdc',encryptedParam])
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
