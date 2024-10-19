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


@Component({
  selector: 'app-hrm-mst-assignedholidaygradeview',
  templateUrl: './hrm-mst-assignedholidaygradeview.component.html',
})
export class HrmMstAssignedholidaygradeviewComponent {
  Holidayassign_type: any;
  holidaygrade_code: any;
  holidaygrade_name: any;
  responsedata: any;
  Holidaygradeview_list: any[] = [];
  holidaygrade_gid: any;

  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit(): void {

    //// Summary Grid//////
    const holidaygrade_gid = this.router.snapshot.paramMap.get('holidaygrade_gid');
    this.holidaygrade_gid = holidaygrade_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.holidaygrade_gid, secretKey).toString(enc.Utf8);
    this.holidaygrade_gid = deencryptedParam
    let param = {
      holidaygrade_gid: deencryptedParam
    }

    //// Summary Grid//////
    var url = 'HolidayGradeManagement/HolidayGradeViewSummary'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Holidaygradeview_list = this.responsedata.holidayview_list;
      setTimeout(() => {
        $('#Holidaygradeview_list').DataTable();
      },);
    });
    var url = 'HolidayGradeManagement/Holidayassign'
    let param1 = {
      holidaygrade_gid: this.holidaygrade_gid
    }
    this.service.getparams(url, param1).subscribe((result: any) => {
      this.responsedata = result;
      this.Holidayassign_type = this.responsedata.Holidayassign_type;
      this.holidaygrade_code = this.Holidayassign_type[0].holidaygrade_code;
      this.holidaygrade_name = this.Holidayassign_type[0].holidaygrade_name;
    });
  }

}
