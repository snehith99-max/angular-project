// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-smr-trn-renewalassign',
//   templateUrl: './smr-trn-renewalassign.component.html',
//   styleUrls: ['./smr-trn-renewalassign.component.scss']
// })
// export class SmrTrnRenewalassignComponent {

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
  unmappedproduct_list:any;
  taxsegment_gid: any;
  campaign_gid1:any;
  campaign_gid:any;
  employee_gid1:any;
  employee_gid:any;
  unmappedrenewal_list:any;


}
export class IunAssignproduct {
  unassignproductchecklist: any[] = [];
}

@Component({
  selector: 'app-smr-trn-renewalassign',
  templateUrl: './smr-trn-renewalassign.component.html',
  styleUrls: ['./smr-trn-renewalassign.component.scss']
})
export class SmrTrnRenewalassignComponent {
  selectedProduct: Product[] = [];
  tax_gid : any;
  CurObj: IunAssign = new IunAssign();
  selection = new SelectionModel<IunAssign>(true, []);
  tax_gid1:any;
  assignproduct_list: any[] = [];
  pick: Array<any> = [];
  unmappedproduct_list: any[] = [];
  unmappedrenewal_list:any;
  product!: Product;
  encrypt: any;
   taxsegment_key: any;
  tax_gid_key: any;
  response_data:any;
  products: any[] = [];
  selectedTaxSegmentId: string | null = null;
  taxsegment_gid1:any;
  campaign_gid1:any;
  campaign_gid:any;
  employee_gid1:any;
  employee_gid:any;
  campaign_gid_key:any;
  employee_gid_key:any;

  constructor(private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, private datePipe: DatePipe, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
  debugger
  this.GetMappingSummary();
const campaign_gid = this.router.snapshot.paramMap.get('campaign_gid1');
const employee_gid = this.router.snapshot.paramMap.get('employee_gid1');
const key = 'storyboarderp';
 this.campaign_gid_key = campaign_gid;
this.employee_gid_key = employee_gid;
this.campaign_gid1 = AES.decrypt(this.campaign_gid_key, key).toString(enc.Utf8);
this.employee_gid1 = AES.decrypt(this.employee_gid_key, key).toString(enc.Utf8);
  }
GetMappingSummary() {
    var api = 'SmrTrnRenewalteamsummary/GetSmrRenewalassignlist'
    this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result: any) => {
    this.response_data = result;
    this.assignproduct_list = this.response_data.unmappedrenewal_list;
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
    this.CurObj.campaign_gid = this.campaign_gid1
    this.pick = this.selection.selected  
    this.CurObj.unmappedrenewal_list = this.pick
     this.CurObj.employee_gid=this.employee_gid1
        if (this.CurObj.unmappedrenewal_list.length === 0) {
        this.ToastrService.warning("Select atleast one Product");
        return;
      } 
  
      debugger
      this.NgxSpinnerService.show();
        var url = 'SmrTrnRenewalteamsummary/Getassignrenewal';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/smr/SmrTrnRenevalsummary'])
            this.NgxSpinnerService.hide();
          }
        });     
      this.selection.clear();
  }

}


