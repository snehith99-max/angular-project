import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-ims-trn-opendc-addselect',
  templateUrl: './ims-trn-opendc-addselect.component.html',
  styleUrls: ['./ims-trn-opendc-addselect.component.scss'],
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
export class ImsTrnOpendcAddselectComponent {

  opendcadd_list:any[]=[];
  responsedata: any;
  GetOpendcView_list:any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    this.GetImsTrnOpenDcAddSummary();
}
GetImsTrnOpenDcAddSummary(){
  debugger
  var url = 'ImsTrnOpenDcSummary/GetImsTrnOpenDcAddSummary'
  this.NgxSpinnerService.show()
   this.service.get(url).subscribe((result: any) => {
     this.responsedata = result;
     this.opendcadd_list = result.opendcadd_list;
     setTimeout(() => {
       $('#opendcadd_list').DataTable();
             }, 1);
      this.NgxSpinnerService.hide()
  })
}
onadd(params:any){
  debugger
  const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/ims/ImsTrnOpendcaddselectUpdate',encryptedParam])
}
splitIntoLines(text: string, lineLength: number): string[] {
  const lines = [];
  for (let i = 0; i < text.length; i += lineLength) {
    lines.push(text.substr(i, lineLength));
  }
  return lines;
}
Getproduct(salesorder_gid: any){
  let param = {
    salesorder_gid:salesorder_gid
  } 
  var url = 'ImsTrnOpenDcSummary/GetOpendcViewProduct'
  this.service.getparams(url,param).subscribe((result: any) => {
    debugger
    $('#GetOpendcView_list').DataTable().destroy();
    this.responsedata = result;
    this.GetOpendcView_list = this.responsedata.GetOpendcView_list;
    setTimeout(() => {
      $('#GetOpendcView_list').DataTable();
    }, 1);
  });
}
}
