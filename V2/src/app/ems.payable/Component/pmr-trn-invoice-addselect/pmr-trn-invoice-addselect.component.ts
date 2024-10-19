import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup,FormControl } from '@angular/forms';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-pmr-trn-invoice-addselect',
  templateUrl: './pmr-trn-invoice-addselect.component.html',
  styleUrls: ['./pmr-trn-invoice-addselect.component.scss']
})
export class PmrTrnInvoiceAddselectComponent {
  invoice_list:any;
  response_data :any;
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  invoice:any;
  showOptionsDivId:any;
  rows:any[]=[];
  serviceinvoice_list:any[]=[];


  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,) {} 
  ngOnInit(): void {
    this.GetPmrTrnInvoiceAddSelectSummary();
    this.GetPmrTrnInvoiceserviceSelectSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),

     
    });
  }
  GetPmrTrnInvoiceAddSelectSummary() {
   
    const api = 'PmrTrnInvoice/GetPmrTrnInvoiceAddSelectSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.invoice_list = this.response_data.invoice_list;
      setTimeout(() => {  
        $('#invoice_list').DataTable({
          order: [] 
        });
      }, 1);
 
    });
  }

  GetPmrTrnInvoiceserviceSelectSummary() {
   
    const api = 'PmrTrnInvoice/GetPmrTrnInvoiceServiceSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.serviceinvoice_list = this.response_data.Serviceinvoice_list;
      setTimeout(() => {  
        $('#serviceinvoice_list').DataTable({
          order: [] 
        });
      }, 1);
 
    });
  }


  onview(vendor_gid:any, purchaseorder_gid:any, purchaseorder_from:any, grn_gid:any){
    debugger;
  const secretKey = 'storyboarderp';
    
  const param1 = (purchaseorder_gid);
  const param2 = (vendor_gid);
  const param3 = (grn_gid);

  const purchaseorder_gid1 = AES.encrypt(param1,secretKey).toString();
  const vendor_gid2 = AES.encrypt(param2,secretKey).toString();
  const grn_gid3= AES.encrypt(param3,secretKey).toString();

  //const encryptedParam = AES.encrypt(param1,secretKey).toString();
  this.router.navigate(['/payable/PblTrnInvoiceaddselectgrndetailsComponent', purchaseorder_gid1,vendor_gid2,grn_gid3])  
  // }
}

// onviewservice( purchaseorder_gid:any){
// debugger;
// const secretKey = 'storyboarderp';  
// const param1 = (purchaseorder_gid);
// const purchaseorder_gid1 = AES.encrypt(param1,secretKey).toString();
// //const encryptedParam = AES.encrypt(param1,secretKey).toString();
// this.router.navigate(['/payable/PblTrnServiceInvoiceAdd', purchaseorder_gid1])  
// // }
onviewservice(params:any){
  debugger
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.router.navigate(['/payable/PblTrnServiceInvoiceAdd',encryptedParam])
   
}



back(){
  this.router.navigate(['/payable/PmrTrnInvoice'])
}

}
