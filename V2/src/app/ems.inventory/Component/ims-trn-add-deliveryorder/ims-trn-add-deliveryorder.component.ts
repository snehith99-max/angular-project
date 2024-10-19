import { Component } from '@angular/core';
import { FormBuilder} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-trn-add-deliveryorder',
  templateUrl: './ims-trn-add-deliveryorder.component.html',
  styleUrls: ['./ims-trn-add-deliveryorder.component.scss'],
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
export class ImsTrnAddDeliveryorderComponent {
  responsedata: any;
  adddeliveryorder_list: any[] = [];
  getData: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private router:Router,private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    this.GetImsTrnAddDeliveryorderSummary();
}
//// Summary Grid//////
GetImsTrnAddDeliveryorderSummary() {
  var url = 'ImsTrnDeliveryorderSummary/GetImsTrnAddDeliveryorderSummary'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#adddeliveryorder_list').DataTable().destroy();
    this.responsedata = result;
    this.adddeliveryorder_list = this.responsedata.adddeliveryorder_list;
    setTimeout(() => {
      $('#adddeliveryorder_list').DataTable();
    }, 1);
    this.NgxSpinnerService.hide()


  })
}
raisedeliveryorder(salesorder_gid: any){
    const secretKey = 'storyboarderp';
    const param = (salesorder_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/ims/ImsTrnRaiseDeliveryorder', encryptedParam])
  }
  onview(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/ims/ImsTrnDespatchView',encryptedParam]);
  }
  openModalamend(){}
  onaddinfo(){}



}
