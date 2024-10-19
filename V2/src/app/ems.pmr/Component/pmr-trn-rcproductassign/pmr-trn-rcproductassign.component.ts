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
export class IunAssign {
  unassigncustomerchecklist: any[] = [];
  mailmanagement_gid: string = "";
  leadbank_gid: string = "";
  tax_gid:any;
  unmappedproduct_list:any;
  taxsegment_gid: any;

}
export class IunAssignproduct {
  unassignproductchecklist: any[] = [];
}

@Component({
  selector: 'app-pmr-trn-rcproductassign',
  templateUrl: './pmr-trn-rcproductassign.component.html',
  styleUrls: ['./pmr-trn-rcproductassign.component.scss']
})

export class PmrTrnRCproductassignComponent {
  selectedProduct: Product[] = [];
  tax_gid : any;
  CurObj: IunAssign = new IunAssign();
  selection = new SelectionModel<IunAssign>(true, []);
  tax_gid1:any;
  contractproduct_list: any[] = [];
  contractvendor_list:any[]=[];
  pick: Array<any> = [];
  unmappedproduct_list: any[] = [];
  product!: Product;
  encrypt: any;
   taxsegment_key: any;
   ratecontractgid: any;
  response_data:any;
  products: any[] = [];
  taxsegment_gid1:any;
  productFormadd:FormControl|any;
  temptable: any[] = [];
  ratecontract:any;

  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private datePipe: DatePipe, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
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
    });
  }
  GetMappingSummary(ratecontract_gid: any) {
    debugger
    
    var api = 'PmrTrnRateContract/GetcontractProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      ratecontract_gid: ratecontract_gid,
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#contractproduct_list').DataTable().destroy();
      this.response_data = result;
      this.contractproduct_list = this.response_data.contractproduct_list;
      // setTimeout(()=>{  
      //   $('#contractproduct_list').DataTable();
      // }, 1);
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
  assign(){
      debugger
      this.temptable=[];
      let j=0;
      const product = this.contractproduct_list.some(item => item.product_price && item.product_price > 0);
      if (!product) {
        this.ToastrService.warning('Please fill in at least one value in the Product Price.');
        return;
      }
      for(let i=0;i<this.contractproduct_list.length;i++){
              if(this.contractproduct_list[i].product_price !=null && this.contractproduct_list[i].product_price !=0){
              this.temptable.push(this.contractproduct_list[i]);
              j++;
              }
          } 
      var params = { 
        contractassignlist: this.temptable,
        ratecontract_gid:this.ratecontract,
      };
      this.NgxSpinnerService.show();
        var url = 'PmrTrnRateContract/PostMapProduct';  
        this.service.postparams(url,params).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/pmr/PmrTrnRatecontract'])
            this.NgxSpinnerService.hide();
          }
        });     
  }
}

