import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Table } from 'primeng/table';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
export interface Customer {

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



export class IunAssign {
  unassigncustomerchecklist: any[] = [];
  mailmanagement_gid: string = "";
  template_gid: string = "";
  leadbank_gid: string = "";
  customer_gid:any;
  GetCustomerassignedlist:any;


}

@Component({
  selector: 'app-smr-mst-assigncustomer',
  templateUrl: './smr-mst-assigncustomer.component.html',
  styleUrls: ['./smr-mst-assigncustomer.component.scss']
})
export class SmrMstAssigncustomerComponent {
  assigncustomer_list: any[] = [];
  customer!: Customer;
  selectedCustomer: Customer[] = [];
  pick:Array<any> = [];
  taxsegment_gid: any;
  customer_gid: any;
  encryptparam: any;
  CurObj: IunAssign = new IunAssign();
     selection = new SelectionModel<IunAssign>(true, []);

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private router: ActivatedRoute,
    private route: Router,
    private NgxSpinnerService: NgxSpinnerService,
    private sanitizer: DomSanitizer) {
    // this.isRowSelectable = this.isRowSelectable.bind(this);
  }

  ngOnInit(): void {    
    const taxsegment_gid = this.router.snapshot.paramMap.get('taxsegment_gid1');

    this.encryptparam = taxsegment_gid;
   
    const key = 'storyboard';
    this.taxsegment_gid = AES.decrypt(this.encryptparam, key).toString(enc.Utf8)

    let param = { taxsegment_gid: this.taxsegment_gid }

    this.NgxSpinnerService.show();
    var customerassign = 'SmrMstTaxSegment/GetCustomerAssignedlist';
    this.service.getparams(customerassign, param).subscribe((apiresponse: any) => {
      this.assigncustomer_list = apiresponse.GetCustomerassignedlist;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#assigncustomer_list').DataTable();
      }, 1);

    });
  }

 
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assigncustomer_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ? 
      this.selection.clear() :
      this.assigncustomer_list.forEach((row: IunAssign) => this.selection.select(row));
  }
  unassign(){
      this.pick = this.selection.selected  
      this.CurObj.GetCustomerassignedlist = this.pick
      this.CurObj.customer_gid=this.customer_gid
        if (this.CurObj.GetCustomerassignedlist.length === 0) {
        this.ToastrService.warning("Select atleast one customer");
        return;
      } 
  
      this.NgxSpinnerService.show();
        var url = 'SmrMstTaxSegment/UnassignedCustomer';  
        this.service.post(url, this.CurObj).subscribe((result: any) => {
          if (result.status === false) {
            this.ToastrService.warning(result.message);
            
          } else {
            this.ToastrService.success(result.message);
            this.route.navigate(['/smr/SmrMstTaxsegment'])
            this.NgxSpinnerService.hide();

          }
        });     
      this.selection.clear();
  }

}
