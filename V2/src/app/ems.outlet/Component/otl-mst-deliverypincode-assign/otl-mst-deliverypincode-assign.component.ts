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
  deliverycost_id: any;
  deliverypincodeassing: any;
}
@Component({
  selector: 'app-otl-mst-deliverypincode-assign',
  templateUrl: './otl-mst-deliverypincode-assign.component.html',
  styleUrls: ['./otl-mst-deliverypincode-assign.component.scss']
})
export class OtlMstDeliverypincodeAssignComponent {
  responsedata: any;
  holidayunemployee_list: any[] = [];
  selection = new SelectionModel<IPincode>(true, []);
  leave_name: any;
  leave_code: any;
  deliverycost_id: any;
  Leaveassign_type: any;
  pick: Array<any> = [];
  GetPincodeSummaryAssign: any[] = [];
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
    this.GetPincodeSummary();
    const deliverycost_id = this.router.snapshot.paramMap.get('deliverycost_id1');
    this.deliverycost_id = deliverycost_id;
    const secretKey = 'storyboard';
    const deencryptedParam = AES.decrypt(this.deliverycost_id,secretKey).toString(enc.Utf8);
    this.deliverycost_id = deencryptedParam;
    
  }

  GetPincodeSummary() {

    var api = 'DeliveryCost/GetPincodeSummaryAssign';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.GetPincodeSummaryAssign = this.responsedata.GetPincodeSummaryAssign;
      setTimeout(() => {
        $('#GetPincodeSummaryAssign').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });

  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.GetPincodeSummaryAssign.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.GetPincodeSummaryAssign.forEach((row: IPincode) => this.selection.select(row));
  }


  unassign() {
    debugger;
    this.pick = this.selection.selected
    this.CurObj.deliverypincodeassing = this.pick
    this.CurObj.deliverycost_id = this.deliverycost_id
    if (this.CurObj.deliverypincodeassing.length === 0) {
      this.ToastrService.warning("Select atleast one branch");
      return;
    }

    debugger
    this.NgxSpinnerService.show();
    var url = 'DeliveryCost/PostAssignPincode2deliverycost';
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message);

      } else {
        this.ToastrService.success(result.message);
        this.route.navigate(['/outlet/OtlMstDeliveryCostMapping'])
        this.NgxSpinnerService.hide();

      }
    });
    this.selection.clear();
  }
}
