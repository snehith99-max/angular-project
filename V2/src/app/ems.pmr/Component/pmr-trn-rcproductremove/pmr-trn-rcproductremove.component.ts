import { Component } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { HttpClient } from '@angular/common/http';

export class unAssign {
  productgroup_gid: any;
  productgroup_name: any;
  product_gid: any;
  product_name: any;
  productuom_gid: any;
  productuom_name: any;
  sku: any;
  product_desc:any;
  mrp:any;
  contractassignlist:any;
  product_code:any;
  ratecontract_gid:any;
}
@Component({
  selector: 'app-pmr-trn-rcproductremove',
  templateUrl: './pmr-trn-rcproductremove.component.html',
  styleUrls: ['./pmr-trn-rcproductremove.component.scss']
})
export class PmrTrnRCproductremoveComponent {
contractassignlist:any[]=[];
selection = new SelectionModel<unAssign>(true, []);
ProductForm!: FormGroup;
productunassign:any;
ratecontract_gid:any;
response_data:any;
contractvendor_list:any[]=[];
pick:Array<any> = [];
CurObj: unAssign = new unAssign();
  constructor(private http: HttpClient,public NgxSpinnerService:NgxSpinnerService, private fb: FormBuilder, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
  }
  ngOnInit():void{
    this.ProductForm = new FormGroup({
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      ratecontract_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productuom_name: new FormControl(''),
      product_price: new FormControl(''),
    });
    this.ratecontract_gid = this.router.snapshot.paramMap.get('ratecontract_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.ratecontract_gid, secretKey).toString(enc.Utf8);
    this.GetProductunAssignSummary(deencryptedParam);
    this.GetMappingvendor(deencryptedParam);
  }
  GetProductunAssignSummary(ratecontract_gid:any){
    let param = {
      ratecontract_gid: ratecontract_gid,
    }
    var url = 'PmrTrnRateContract/GetProductunAssignSummary'
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#contractassignlist').DataTable().destroy();
      this.contractassignlist = result.unassignproduct_list
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#contractassignlist').DataTable();
      }, 1);
    });
  }
  GetMappingvendor(ratecontract_gid: any) {
    debugger
    
    var api = 'PmrTrnRateContract/Getcontractvendor'
    this.NgxSpinnerService.show()
    let param = {
      ratecontract_gid: ratecontract_gid,
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      this.response_data = result;
      this.contractvendor_list = this.response_data.contractvendor_list;
    });
    this.NgxSpinnerService.hide()
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.contractassignlist.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.contractassignlist.forEach((row: unAssign) => this.selection.select(row));
  }
  onsubmit() {
    debugger
    this.ratecontract_gid = this.router.snapshot.paramMap.get('ratecontract_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.ratecontract_gid, secretKey).toString(enc.Utf8);
    this.pick = this.selection.selected  
    this.CurObj.contractassignlist = this.pick
    this.CurObj.ratecontract_gid=deencryptedParam
      if (this.CurObj.contractassignlist.length === 0) {
      this.ToastrService.warning("Select atleast one product");
      return;
    } 
    else{
      var url = 'PmrTrnRateContract/UnAssignProduct'
      this.NgxSpinnerService.show()
      this.service.post(url, this.CurObj).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetProductunAssignSummary(deencryptedParam);
          this.selection.clear();
          this.NgxSpinnerService.hide()
        }
        else {
          this.ToastrService.success(result.message)
          this.ProductForm.reset();
          this.route.navigate(['/pmr/PmrTrnRatecontract'])
          this.GetProductunAssignSummary(deencryptedParam);
          this.NgxSpinnerService.hide()
        }
      });
    }
    }
    back() {
      this.route.navigate(['/pmr/PmrTrnRatecontract']);
    }

}
