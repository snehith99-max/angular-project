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
  GetOTLAssignedEmplist:any;
  campaign_gid: any;


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
  selector: 'app-otl-trn-outletmanage-employeeassign',
  templateUrl: './otl-trn-outletmanage-employeeassign.component.html',
  styleUrls: ['./otl-trn-outletmanage-employeeassign.component.scss']
})
export class OtlTrnOutletmanageEmployeeassignComponent {

  unassignemployee_list: any[] = [];
  vendor!: Vendor;
  selectedCustomer: Vendor[] = [];
  pick:Array<any> = [];
  campaign_gid: any;
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
      const campaign_gid = this.router.snapshot.paramMap.get('campaign_gid');
  
      this.encryptparam = campaign_gid;
     
      const key = 'storyboard';
      this.campaign_gid = AES.decrypt(this.encryptparam, key).toString(enc.Utf8)
  
      let param = { campaign_gid: this.campaign_gid }
      this.NgxSpinnerService.show();
      var vendorassign = 'OutletManage/GetotlUnassignedEmplist';
      this.service.getparams(vendorassign, param).subscribe((apiresponse: any) => {
        this.unassignemployee_list = apiresponse.GetOTLUnassignedEmplist;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#assigncustomer_list').DataTable();
        }, 1);
  
      });
    }
  
   
    isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.unassignemployee_list.length;
      return numSelected === numRows;
    }
    masterToggle() {
      this.isAllSelected() ?
        this.selection.clear() :
        this.unassignemployee_list.forEach((row: IunAssign) => this.selection.select(row));
    }
    unassign(){
      debugger;
        this.pick = this.selection.selected  
        this.CurObj.GetOTLAssignedEmplist = this.pick
        this.CurObj.campaign_gid=this.campaign_gid
          if (this.CurObj.GetOTLAssignedEmplist.length === 0) {
          this.ToastrService.warning("Select atleast one manager");
          return;
        } 
    
        debugger
        this.NgxSpinnerService.show();
          var url = 'OutletManage/PostOutUnassignedEmplist';  
          this.service.post(url, this.CurObj).subscribe((result: any) => {
            if (result.status === false) {
              this.ToastrService.warning(result.message);
              
            } else {
              this.ToastrService.success(result.message);
              this.route.navigate(['/outlet/otltrnoutletmanage'])
              this.NgxSpinnerService.hide();
  
            }
          });     
        this.selection.clear();
    }
    

}
