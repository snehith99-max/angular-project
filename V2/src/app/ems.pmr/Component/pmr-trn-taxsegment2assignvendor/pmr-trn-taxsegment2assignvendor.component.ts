import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
export class IunAssign {
  unassignvendorchecklist: any[] = [];
  template_gid: string = "";
  Taxsegment2assignvendorList:any;
  taxsegment_gid: any;


}
export interface Vendor {

  email?: any | null;
  names?: any | null;
  address?: any | null;
  customer_city?: any | null;
  customer_state?: any | null;
  direction?: any | null;
  source_gid?: any | null;
  source_name?: any | null;
  taxsegment_name?: any | null;
  customer_country?: any | null;

  message?: any | null;
  status?: boolean | null;

}
@Component({
  selector: 'app-pmr-trn-taxsegment2assignvendor',
  templateUrl: './pmr-trn-taxsegment2assignvendor.component.html',
})
export class PmrTrnTaxsegment2assignvendorComponent {
  unassignvendor_list: any[] = [];
  vendor!: Vendor;
  selectedCustomer: Vendor[] = [];
  pick:Array<any> = [];
  taxsegment_gid: any;
  vendor_gid: any;
  encryptparam: any;
  CurObj: IunAssign = new IunAssign();
     selection = new SelectionModel<IunAssign>(true, []);

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private router: ActivatedRoute,
    private route: Router,
    private NgxSpinnerService: NgxSpinnerService,
    private sanitizer: DomSanitizer){
      // this.isRowSelectable = this.isRowSelectable.bind(this);
    }
    ngOnInit(): void {
      debugger
      const taxsegment_gid = this.router.snapshot.paramMap.get('taxsegment_gid');
  
      this.encryptparam = taxsegment_gid;
     
      const key = 'storyboard';
      this.taxsegment_gid = AES.decrypt(this.encryptparam, key).toString(enc.Utf8)
  
      let param = { taxsegment_gid: this.taxsegment_gid }
      this.NgxSpinnerService.show();
      var vendorassign = 'PmrTaxSegment/GetTaxSegmentUnAssignVendorSummary';
      this.service.getparams(vendorassign, param).subscribe((apiresponse: any) => {
        this.unassignvendor_list = apiresponse.Taxsegment2unassignvendorList;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#unassignvendor_list').DataTable();
        }, 1);
  
      });
    }
  
   
    // isAllSelected() {
    //   const numSelected = this.selection.selected.length;
    //   const numRows = this.unassignvendor_list.length;
    //   return numSelected === numRows;
    // }
    // masterToggle() {
    //   this.isAllSelected() ?
    //     this.selection.clear() :
    //     this.unassignvendor_list.forEach((row: IunAssign) => this.selection.select(row));
    // }
    masterToggle() {
      const table = $('#unassignvendor_list').DataTable();
      const filteredData = table.rows({ search: 'applied' }).data().toArray();
   
      if (this.isAllSelected()) {
        this.selection.clear();
      }
       else {
        filteredData.forEach((row: any) => {
          const vendorCode = row[1];
          const vendor = this.unassignvendor_list.find(item => item.vendor_code === vendorCode);
          if (vendor) this.selection.select(vendor);
        });
      }
    }
   
    isAllSelected() {
      const table = $('#unassignvendor_list').DataTable();
      const filteredDataLength = table.rows({ search: 'applied' }).data().length;
      const numSelected = this.selection.selected.length;
      return numSelected === filteredDataLength;
    }

    unassign(){
      debugger;
        this.pick = this.selection.selected  
        this.CurObj.Taxsegment2assignvendorList = this.pick
        this.CurObj.taxsegment_gid=this.taxsegment_gid
          if (this.CurObj.Taxsegment2assignvendorList.length === 0) {
          this.ToastrService.warning("Select atleast one vendor");
          return;
        } 
    
        debugger
        this.NgxSpinnerService.show();
          var url = 'PmrTaxSegment/PostPmrTaxSegmentunassign2Vendor';  
          this.service.post(url, this.CurObj).subscribe((result: any) => {
            if (result.status === false) {
              this.ToastrService.warning(result.message);
              
            } else {
              this.ToastrService.success(result.message);
              this.route.navigate(['/pmr/PmrMstTaxSegment'])
              this.NgxSpinnerService.hide();
  
            }
          });     
        this.selection.clear();
    }

}
