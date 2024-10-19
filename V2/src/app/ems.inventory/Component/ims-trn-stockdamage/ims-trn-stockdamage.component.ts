import { Component, numberAttribute } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormRecord, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { DefaultGlobalConfig, ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FrozenColumn } from 'primeng/table';
import { TabHeadingDirective } from 'ngx-bootstrap/tabs';
import { ReplaySubject } from 'rxjs';
import { __values } from 'tslib';


@Component({
  selector: 'app-ims-trn-stockdamage',
  templateUrl: './ims-trn-stockdamage.component.html',
  styleUrls: ['./ims-trn-stockdamage.component.scss']
})
export class ImsTrnStockdamageComponent { 
  product_gid_key: any;
  uom_gid_key: any;
  branch_gid_key: any;
  stock_gid_key: any;
  response_data:any;
  stockdamagesummary : any[] = [];
  
  constructor(private formBuilder: FormBuilder,
  private ToastrService: ToastrService,
  private router: ActivatedRoute,
  private route: Router,
  public service: SocketService,
  public NgxSpinnerService: NgxSpinnerService) {
   
  
}

  ngOnInit(): void {
  const product_gid = this.router.snapshot.paramMap.get('product_gid');
  const uom_gid = this.router.snapshot.paramMap.get('uom_gid');
  const branch_gid = this.router.snapshot.paramMap.get('branch_gid');
  const stock_gid = this.router.snapshot.paramMap.get('stock_gid');
  const secretKey = 'storyboarderp';

  this.product_gid_key = product_gid;
  this.uom_gid_key = uom_gid;
  this.branch_gid_key = branch_gid;
  this.stock_gid_key = stock_gid;
  const product_gid1 = AES.decrypt(this.product_gid_key, secretKey).toString(enc.Utf8);
  const uom_gid1 = AES.decrypt(this.uom_gid_key, secretKey).toString(enc.Utf8);;
  const branch_gid1 = AES.decrypt(this.branch_gid_key, secretKey).toString(enc.Utf8);
  const stock_gid1 = AES.decrypt(this.stock_gid_key, secretKey).toString(enc.Utf8);

   this.GetdamageSummary(product_gid1,uom_gid1,branch_gid1);
 

 
}

GetdamageSummary(product_gid1: any,uom_gid1: any,branch_gid1: any) {
  debugger
  var api = 'ImsTrnStockSummary/GetDamageStockSummary'
  this.NgxSpinnerService.show()
  let param = {
    product_gid: product_gid1,
    uom_gid: uom_gid1,
    branch_gid: branch_gid1,
    // stock_gid: stock_gid1
  };
  this.service.getparams(api, param).subscribe((result: any) => {
    this.response_data = result;
    this.stockdamagesummary = this.response_data.Getdamagestock;
    setTimeout(()=>{  
      $('#stockdamagesummary').DataTable();
    }, 1);
  
   
  });
  this.NgxSpinnerService.hide()
}

onDamaged(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnAddDamagedstock',encryptedParam]) 
}
}
