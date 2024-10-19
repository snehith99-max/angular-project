import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-ims-trn-purchasereturn-addselect',
  templateUrl: './ims-trn-purchasereturn-addselect.component.html',
  styleUrls: ['./ims-trn-purchasereturn-addselect.component.scss']
})
export class ImsTrnPurchasereturnAddselectComponent {

  combinedFormData!: FormGroup;
  branch_list: any[] = [];
  branchaddresslist: any[] = [];
  GetGRNPurchaseReturn_list: any[] = [];
  mdlBranchName: any;

  constructor(private service: SocketService,
    private router : Router
  ) { }

  ngOnInit(): void {

    this.combinedFormData = new FormGroup({
      branch_name: new FormControl(''),
    })

    var url = 'PurchaseReturn/GetBranchPurchaseReturn'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetPurchaseReturnbranch_list;
      const firstBranch = this.branch_list[0];
      const branchName = firstBranch.branch_gid;
      this.combinedFormData.get('branch_name')?.setValue(branchName);
    });
    this.GetPurchaseReturnGRNSummary();
  }
  GetPurchaseReturnGRNSummary() {
    debugger
    let param = { branch_gid: this.combinedFormData.value.branch_name }
    var GRNsummaryapi = 'PurchaseReturn/GetPurchaseReturnGRN';
    $('#GetGRNPurchaseReturn_list').DataTable().destroy();
    this.service.getparams(GRNsummaryapi, param).subscribe((result: any) => {
      this.GetGRNPurchaseReturn_list = result.GetGRNPurchaseReturn_list;
      setTimeout(() => {
        $('#GetGRNPurchaseReturn_list').DataTable();
      }, 1);
    })
  }
  get branch_name() {
    return this.combinedFormData.get('branch_name')!;
  }
  addpurchasereturn(grn_gid: any, vendor_gid: any) {
    const key = 'storyboard';
    const param = grn_gid;
    const param1 = vendor_gid;
    const lspage1 = 'GRN QC';
    const grngid = AES.encrypt(param,key).toString();
    const vendorgid = AES.encrypt(param1,key).toString();
    const lspage = AES.encrypt(lspage1,key).toString();
    this.router.navigate(['/ims/ImsTrnPurchaseReturnAdd', grngid, vendorgid,lspage])
  }
}

