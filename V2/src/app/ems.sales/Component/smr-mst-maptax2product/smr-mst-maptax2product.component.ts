
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
  selector: 'app-smr-mst-maptax2product',
  templateUrl: './smr-mst-maptax2product.component.html',
  styleUrls: ['./smr-mst-maptax2product.component.scss']
})

export class SmrMstMaptax2productComponent {
  // CurObj: IunAssignproduct = new IunAssignproduct();
  selectedProduct: Product[] = [];
  tax_gid : any;
  CurObj: IunAssign = new IunAssign();
  selection = new SelectionModel<IunAssign>(true, []);
  tax_gid1:any;
  assignproduct_list: any[] = [];
  pick: Array<any> = [];
  unmappedproduct_list: any[] = [];
  // selection = new SelectionModel<IunAssignproduct>(true, []);
  product!: Product;
  encrypt: any;
   taxsegment_key: any;
  tax_gid_key: any;
  response_data:any;
  products: any[] = [];
  selectedTaxSegmentId: string | null = null;
  taxsegment_gid1:any;


  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private datePipe: DatePipe, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
debugger
    const tax_gid = this.router.snapshot.paramMap.get('tax_gid');
    const taxsegment_gid = this.router.snapshot.paramMap.get('taxsegment_gid');
    // this.encrypt = tax_gid;
    const key = 'storyboarderp';
    this.tax_gid_key = tax_gid;
  this.taxsegment_key = taxsegment_gid;

  const tax_gid1 = AES.decrypt(this.tax_gid_key, key).toString(enc.Utf8);
  const taxsegment_gid1 = AES.decrypt(this.taxsegment_key, key).toString(enc.Utf8);;
    // this.tax_gid = AES.decrypt(this.encrypt, key).toString(enc.Utf8);
    this.GetMappingSummary(tax_gid1,taxsegment_gid1);
    this.tax_gid1 = AES.decrypt(this.tax_gid_key, key).toString(enc.Utf8);
    this.taxsegment_gid1 = AES.decrypt(this.taxsegment_key, key).toString(enc.Utf8);

    // var taxsegmentapi = 'SmrMstTaxSegment/GetTaxSegmentSummary';
    // this.service.get(taxsegmentapi).subscribe((apiresponse: any) => {
    //   this.unmappedproduct_list = apiresponse.unmappedproduct_list;
    // });
    // if (this.unmappedproduct_list.length > 0) {
    //   this.selectedTaxSegmentId = this.unmappedproduct_list[0].taxsegment_gid;
    //   this.GetTaxsegmentassignedsummary(this.selectedTaxSegmentId);
    // }
  }
  // onTabClick(taxsegment_gid: any): void {
  //   debugger
  //   this.selectedTaxSegmentId = taxsegment_gid;
  //   this.GetTaxsegmentassignedsummary(this.selectedTaxSegmentId);
  // }
  // GetTaxsegmentassignedsummary(taxsegment_gid: any) {
  //   let param = { taxsegment_gid: taxsegment_gid }
  //   var assignedproductapi = 'SmrMstTaxSegment/GetTaxSegment2ProductSummary';
  //   this.service.getparams(assignedproductapi, param).subscribe((apiresponse: any) => {
  //     this.assignproduct_list = apiresponse.unmappedproduct_list;
  //   });
  // }


  GetMappingSummary(tax_gid1: any,taxsegment_gid1: any) {
    debugger
    var api = 'SmrMstTaxSummary/GetTaxSegment2ProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tax_gid: tax_gid1,
      taxsegment_gid: taxsegment_gid1,
      
      // stock_gid: stock_gid1
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      this.response_data = result;
      this.assignproduct_list = this.response_data.unmappedproduct_list;
      setTimeout(()=>{  
        $('#assignproduct_list').DataTable();
      }, 1);
    
     
    });
    this.NgxSpinnerService.hide()
  }
  


  // isAllSelected() {
  //   const numSelected = this.selection.selected.length;
  //   const numRows = this.assignproduct_list.length;
  //   return numSelected === numRows;
  // }
  // masterToggle() {
  //   this.isAllSelected() ?
  //     this.selection.clear() :
  //     this.assignproduct_list.forEach((row: IunAssignproduct) => this.selection.select(row));
  // }
  // onsend() {
  //   debugger
  //   this.CurObj.unassignproductchecklist = this.selectedProduct;
  //   if (this.CurObj.unassignproductchecklist.length != 0) {
  //     var unassigncustomer = 'SmrMstTaxSegment/UnAssignProduct';
  //     this.NgxSpinnerService.show();
  //     this.service.post(unassigncustomer, this.CurObj).subscribe((postresponse: any) => {
  //       if (postresponse == false) {
  //         window.scrollTo({
  //           top: 0,
  //         });
  //         this.ToastrService.warning(postresponse.message);
  //       }
  //       else {
  //         window.scrollTo({
  //           top: 0,
  //         });
  //         this.ToastrService.success(postresponse.message);
  //         this.route.navigate(['/smr/SmrMstTaxSummary']);
  //       }
  //     });
  //     this.NgxSpinnerService.hide();
  //   }
  // }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assignproduct_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.assignproduct_list.forEach((row: IunAssign) => this.selection.select(row));
  }
  assign(){
    debugger;
     this.CurObj.taxsegment_gid = this.taxsegment_gid1
      this.pick = this.selection.selected  
      this.CurObj.unmappedproduct_list = this.pick
      this.CurObj.tax_gid=this.tax_gid1

        if (this.CurObj.unmappedproduct_list.length === 0) {
        this.ToastrService.warning("Select atleast one Product");
        return;
      } 
  
      debugger
      this.NgxSpinnerService.show();
        var url = 'SmrMstTaxSummary/PostMappedProducts';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/smr/SmrMstTaxsummary'])
            this.NgxSpinnerService.hide();

          }
        });     
      this.selection.clear();
  }



}

