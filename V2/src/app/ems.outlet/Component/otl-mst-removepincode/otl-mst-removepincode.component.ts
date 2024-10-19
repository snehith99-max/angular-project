import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable, ReplaySubject } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

export class IBranch {
  branch_gid: any;
  branchunassign: any;
  product_gid: any;
}
@Component({
  selector: 'app-otl-mst-removepincode',
  templateUrl: './otl-mst-removepincode.component.html',
  styleUrls: ['./otl-mst-removepincode.component.scss']
})
export class OtlMstRemovepincodeComponent {
  responsedata: any;
  Getassignpincode_code: any[] = [];
  selection = new SelectionModel<IBranch>(true, []);
  leave_name: any;
  leave_code: any;
  branch_gid: any;
  Leaveassign_type: any;
  pick: Array<any> = [];
  products: any[] = [];
  CurObj: IBranch = new IBranch();
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private NgxSpinnerService: NgxSpinnerService
  ) { }
  ngOnInit(): void {
debugger
    const branch_gid = this.router.snapshot.paramMap.get('branch_gid1');
    this.branch_gid = branch_gid;
    const secretKey = 'storyboard';
    const deencryptedParam = AES.decrypt(this.branch_gid, secretKey).toString(enc.Utf8);
    this.branch_gid = deencryptedParam

    this.GetProductSummary(this.branch_gid);
  }

  // summary
  GetProductSummary(branch_gid: any) {
    let param = { branch_gid: branch_gid }
    var api = 'OtlMstBranch/GetAssignedPincodeSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.Getassignpincode_code = result.Getassignpincode_code;
      setTimeout(() => {
        $('#Getassignpincode_code').DataTable();
      }, 1);
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.products.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.products.forEach((row: IBranch) => this.selection.select(row));
  }


  unassign() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.branchunassign = this.pick
    this.CurObj.branch_gid = this.branch_gid
    if (this.CurObj.branchunassign.length === 0) {
      this.ToastrService.warning("Select atleast one branch");
      return;
    }

    debugger
    this.NgxSpinnerService.show();
    var url = 'OtlMstBranch/RemovePincode';
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message);

      } else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/outlet/OtlMstPriceManagement'])
        this.NgxSpinnerService.hide();

      }
    });
    this.selection.clear();
  }
}
