import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

export class IPincode {
  branch_gid: any;
  pincodeassing: any;
}
@Component({
  selector: 'app-otl-mst-assignpincode',
  templateUrl: './otl-mst-assignpincode.component.html',
  styleUrls: ['./otl-mst-assignpincode.component.scss']
})
export class OtlMstAssignpincodeComponent {
  responsedata: any;
  holidayunemployee_list: any[] = [];
  selection = new SelectionModel<IPincode>(true, []);
  leave_name: any;
  leave_code: any;
  branch_gid: any;
  Leaveassign_type: any;
  pick: Array<any> = [];
  GetOtlPincodeSummaryAssign: any[] = [];
  CurObj: IPincode = new IPincode();
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
    var api = 'OtlMstBranch/GetPincodeSummaryAssign';
    this.NgxSpinnerService.show();
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.GetOtlPincodeSummaryAssign = this.responsedata.GetOtlPincodeSummaryAssign;
      setTimeout(() => {
        $('#GetOtlPincodeSummaryAssign').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });

  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetOtlPincodeSummaryAssign.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.GetOtlPincodeSummaryAssign.forEach((row: IPincode) => this.selection.select(row));
  }


  unassign() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.pincodeassing = this.pick
    this.CurObj.branch_gid = this.branch_gid
    if (this.CurObj.pincodeassing.length === 0) {
      this.ToastrService.warning("Select atleast one branch");
      return;
    }

    debugger
    this.NgxSpinnerService.show();
    var url = 'OtlMstBranch/PostPincodeassing';
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/outlet/OtlMstPriceManagement'])
        this.NgxSpinnerService.hide();

      }
    });
    this.selection.clear();
  }
}
