// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-pmr-trn-agreementtoinvoicetag',
//   templateUrl: './pmr-trn-agreementtoinvoicetag.component.html',
//   styleUrls: ['./pmr-trn-agreementtoinvoicetag.component.scss']
// })
// export class PmrTrnAgreementtoinvoicetagComponent {

// }
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
  invoicetagsummary_list1:any;
  taxsegment_gid: any;
  renewal_gid:any;

}
export class IunAssignproduct {
  unassignproductchecklist: any[] = [];
}

@Component({
  selector: 'app-pmr-trn-agreementtoinvoicetag',
  templateUrl: './pmr-trn-agreementtoinvoicetag.component.html',
  styleUrls: ['./pmr-trn-agreementtoinvoicetag.component.scss']
})
export class PmrTrnAgreementtoinvoicetagComponent {
  selectedProduct: Product[] = [];
  tax_gid : any;
  CurObj: IunAssign = new IunAssign();
  selection = new SelectionModel<IunAssign>(true, []);
  tax_gid1:any;
  assignproduct_list: any[] = [];
  pick: Array<any> = [];
  invoicetagsummary_list: any[] = [];
  product!: Product;
  encrypt: any;
   taxsegment_key: any;
  tax_gid_key: any;
  response_data:any;
  products: any[] = [];
  selectedTaxSegmentId: string | null = null;
  taxsegment_gid1:any;
  renewal_gid_key1:any;
  renewal_gid_key:any;
  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private datePipe: DatePipe, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
    const renewal_gid = this.router.snapshot.paramMap.get('renewal_gid');
    const key = 'storyboarderp';
    this.renewal_gid_key = renewal_gid;

  const renewal_gid_key1 = AES.decrypt(this.renewal_gid_key, key).toString(enc.Utf8);
    this.GetMappingSummary(renewal_gid_key1);
    this.renewal_gid_key1 = AES.decrypt(this.renewal_gid_key, key).toString(enc.Utf8);
  }
  GetMappingSummary(renewal_gid_key1: any) {
    debugger
    var api = 'PmrTrnPurchaseagreement/Getagreementtoinvoicetag'
    this.NgxSpinnerService.show()
    let param = {
      renewal_gid: renewal_gid_key1,
          };
    this.service.getparams(api, param).subscribe((result: any) => {
      this.response_data = result;
      this.assignproduct_list = this.response_data.invoicetagsummary_list1;
      setTimeout(()=>{  
        $('#assignproduct_list').DataTable();
      }, 1);
    
     
    });
    this.NgxSpinnerService.hide()
  }
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
     this.CurObj.renewal_gid = this.renewal_gid_key1
     this.pick = this.selection.selected  
      this.CurObj.invoicetagsummary_list1 = this.pick
        if (this.CurObj.invoicetagsummary_list1.length === 0) {
        this.ToastrService.warning("Select atleast one Tag Invoice");
        return;
      } 
      this.NgxSpinnerService.show();
        var url = 'PmrTrnPurchaseagreement/PostMappedinvoicetag';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/pmr/PmrTrnPurchaseagreement'])
            this.NgxSpinnerService.hide();

          }
        });     
      this.selection.clear();
  }



}


