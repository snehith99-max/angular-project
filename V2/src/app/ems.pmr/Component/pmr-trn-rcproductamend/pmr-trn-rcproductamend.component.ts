
import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { RouterTestingHarness } from '@angular/router/testing';
import { AES, enc } from 'crypto-js';
import { Subscription, map, share, timer } from 'rxjs';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { Table } from 'primeng/table';

export interface Product {

  productgroup_name?: any | null;
  product_code?: any | null;
  product?: any | null;
  SKU?: any | null;
  description?: any | null;
  mrp_price?: any | null;
  taxsegment_gid?: any | null;
  taxsegment_name?: any | null;
  message?: any | null;
  status?: boolean | null;
}
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
export class IunAssignproduct {
  unassignproductchecklist: any[] = [];
}

@Component({
  selector: 'app-pmr-trn-rcproductamend',
  templateUrl: './pmr-trn-rcproductamend.component.html',
  styleUrls: ['./pmr-trn-rcproductamend.component.scss']
})

export class PmrTrnRCproductamendComponent {
  selectedProduct: Product[] = [];
  tax_gid : any;
  CurObj: unAssign = new unAssign();
  selection = new SelectionModel<unAssign>(true, []);
  tax_gid1:any;
  contractproduct_list: any[] = [];
  pick: Array<any> = [];
  unmappedproduct_list: any[] = [];
  product!: Product;
  encrypt: any;
   taxsegment_key: any;
   ratecontractgid: any;
   showOptionsDivId:any;
  response_data:any;
  contractvendor_list:any[]=[];
  contractamend_list:any[]=[];
  products: any[] = [];
  taxsegment_gid1:any;
  productFormadd:FormControl|any;
  temptable: any[] = [];
  ratecontract:any;
  productname:any;
  productgroup:any;
  productcode:any;

  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private datePipe: DatePipe, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    debugger
    const ratecontractgid = this.router.snapshot.paramMap.get('ratecontract_gid');
    const key = 'storyboarderp';
    this.ratecontractgid = ratecontractgid;
    const ratecontract_gid = AES.decrypt(this.ratecontractgid, key).toString(enc.Utf8);
    this.GetMappingSummary(ratecontract_gid);
    this.GetMappingvendor(ratecontract_gid);
    this.ratecontract=ratecontract_gid;
    this.productFormadd = new FormGroup({
      product_price:new FormControl(''),
      remarks:new FormControl(''),
    });
  }
  GetMappingSummary(ratecontract_gid: any) {
    debugger
    var api = 'PmrTrnRateContract/GetProductunAssignSummary'
    this.NgxSpinnerService.show()
    let param = {
      ratecontract_gid: ratecontract_gid,
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#contractproduct_list').DataTable().destroy();
      this.response_data = result;
      this.contractproduct_list = this.response_data.unassignproduct_list;
      for(let i=0;i<this.contractproduct_list.length;i++){
        this.productFormadd.addControl(`product_price_${i}`, new FormControl(this.contractproduct_list[i].product_price));
      }
      setTimeout(()=>{  
        $('#contractproduct_list').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide()
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
  toggleOptions(product_gid: any) {
    if (this.showOptionsDivId === product_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = product_gid;
    }
  }
  onMapProd(params:any){
      debugger
      let productgid = params.product_gid;
      this.productname = params.product_name;
      this.productgroup = params.productgroup_name;
      this.productcode = params.product_code;

      var api = 'PmrTrnRateContract/Getcontractamend'
      this.NgxSpinnerService.show()
      let param = {
        ratecontract_gid: this.ratecontract,
        product_gid:productgid,
      };
      this.service.getparams(api, param).subscribe((result: any) => {
        this.response_data = result;
        this.contractamend_list = this.response_data.contractamend_list;
      }); 
      this.NgxSpinnerService.hide()
    }
    isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.contractproduct_list.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.contractproduct_list.forEach((row: unAssign) => this.selection.select(row));
    }
  onsubmit(){
      debugger
      this.pick = this.selection.selected  
      this.CurObj.contractassignlist = this.pick
      this.CurObj.ratecontract_gid = this.ratecontract
      if (this.CurObj.contractassignlist.length === 0) {
        this.ToastrService.warning("Select atleast one product");
        return;
      } 
      const product = this.contractproduct_list.some(item => item.product_price && item.product_price > 0);
      if (!product) {
        this.ToastrService.warning('Please fill in at least one value in the Product Price.');
        return;
      }
      this.NgxSpinnerService.show();
        var url = 'PmrTrnRateContract/PostAmendProduct';  
        this.service.postparams(url,this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/pmr/PmrTrnRatecontract'])
            this.NgxSpinnerService.hide();
          }
        });     
      this.selection.clear();
  }
}

