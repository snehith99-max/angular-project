import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { Table } from 'primeng/table';

export class IunAssign {
  unassigncustomerchecklist: any[] = [];
  mailmanagement_gid: string = "";
  leadbank_gid: string = "";
  tax_gid:any;
  unmappedproduct_list:any;
  taxsegment_gid: any;

}



@Component({
  selector: 'app-pmr-mst-tax-un-map2-product',
  templateUrl: './pmr-mst-tax-un-map2-product.component.html',
})
export class PmrMstTaxUnMap2ProductComponent {
  GetUnunassignproduct_list1: any[] = [];
  product_gid: any;
  taxsegment_name:any;
  mdltaxsegment:any;
  unmappedproduct_list: any[] = [];
  taxsegmentdtl_list: any[] = [];
  tax_gid: any;
  cusprodForm!: FormGroup;
  selectedProducts: string[] = [];
  deencryptedParam: any;
  taxsegment_gid: any;
 
  tax_gid_key:any;
  taxsegment_key:any;
  tax_gid1:any;
  taxsegment_gid1:any;
 
 
  CurObj: IunAssign = new IunAssign();
  selection = new SelectionModel<IunAssign>(true, []);
  unassignproduct_list: any[] = [];
  pick: Array<any> = [];
  encrypt: any;
  response_data:any;
  products: any[] = [];
  selectedTaxSegmentId: string | null = null;


  constructor(public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    public ToastrService: ToastrService,
    private route: Router, private router: ActivatedRoute,
  ) {}
  
  ngOnInit(): void {
  debugger
    const tax_gid = this.router.snapshot.paramMap.get('tax_gid');
    const taxsegment_gid = this.router.snapshot.paramMap.get('taxsegment_gid');
    const key = 'storyboarderp';
    this.tax_gid_key = tax_gid;
  this.taxsegment_key = taxsegment_gid;
  
  const tax_gid1 = AES.decrypt(this.tax_gid_key, key).toString(enc.Utf8);
  const taxsegment_gid1 = AES.decrypt(this.taxsegment_key, key).toString(enc.Utf8);;
       this.GetMappingSummary(tax_gid1,taxsegment_gid1);
    this.tax_gid1 = AES.decrypt(this.tax_gid_key, key).toString(enc.Utf8);
    this.taxsegment_gid1 = AES.decrypt(this.taxsegment_key, key).toString(enc.Utf8);
  

   
  }






  GetMappingSummary(tax_gid1: any,taxsegment_gid1: any) {
    debugger
    var api = 'PmrMstTax/GetTaxUnmapping'
    this.NgxSpinnerService.show()
    let param = {
      tax_gid: tax_gid1,
      taxsegment_gid: taxsegment_gid1,
      
     
    };
    this.service.getparams(api, param).subscribe((result: any) => {
      this.response_data = result;
      this.unassignproduct_list = this.response_data.unmappedproduct_list;
      setTimeout(()=>{  
        $('#unassignproduct_list').DataTable();
      }, 1);
    
     
    });
    this.NgxSpinnerService.hide()
  }


  // isAllSelected() {
  //   const numSelected = this.selection.selected.length;
  //   const numRows = this.unassignproduct_list.length;
  //   return numSelected === numRows;
  // }
  // masterToggle() {
  //   this.isAllSelected() ?
  //     this.selection.clear() :
  //     this.unassignproduct_list.forEach((row: IunAssign) => this.selection.select(row));
  // }
  masterToggle() {
    const table = $('#unassignproduct_list').DataTable();
    const filteredData = table.rows({ search: 'applied' }).data().toArray();
 
    if (this.isAllSelected()) {
      this.selection.clear();
    }
     else {
      filteredData.forEach((row: any) => {
        const productcode = row[3];
        const product = this.unassignproduct_list.find(item => item.product_code === productcode);
        if (product) this.selection.select(product);
      });
    }
  }
 
  isAllSelected() {
    const table = $('#unassignproduct_list').DataTable();
    const filteredDataLength = table.rows({ search: 'applied' }).data().length;
    const numSelected = this.selection.selected.length;
    return numSelected === filteredDataLength;
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
        var url = 'PmrMstTax/PostUnMappedProducts';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/pmr/PmrMstTaxSummary'])
            this.NgxSpinnerService.hide();

          }
        });     
      this.selection.clear();
  }
}