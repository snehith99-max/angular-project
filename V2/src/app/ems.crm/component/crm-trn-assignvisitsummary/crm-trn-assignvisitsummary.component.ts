import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
export class IAssign {
  summary_list: string[] = [];
  schedulelog_gid: string = "";
  schedule_remarks: string = "";
  executive: string = "";
  campaign_title: string = "";
  leadbank_gid: string = "";


}
interface IAssignvisit {
  campaign_title: string;
  campaign_gid: string;
  schedule_remarks: string;
  executive: string;
  leadbank_gid: string;




}
@Component({
  selector: 'app-crm-trn-assignvisitsummary',
  templateUrl: './crm-trn-assignvisitsummary.component.html',
  styleUrls: ['./crm-trn-assignvisitsummary.component.scss']
})
export class CrmTrnAssignvisitsummaryComponent {
  assignvisitlist: any[] = [];
  summary_list: any[] = [];
  breadcrumb_lists: any[] = [];

  responsedata: any;

  reactiveForm!: FormGroup;
  marketingteamdropdown_list: any[] = [];
  reactiveFormSubmit!: FormGroup;
  executive_list: any[] = [];
  pick: Array<any> = [];
  CurObj: IAssign = new IAssign();
  assignvisitsubmit!: IAssignvisit;
  selection = new SelectionModel<IAssign>(true, []);
  IAssign: any;
  visittilecount_list: any;
  todayvisit: any;
  totalvisit: any;
  expired: any;
  upcoming: any;
  leadbank_gid: any;
  parametervalue: any;
  leadbankname: any;
  leadbank: any;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) {
    this.assignvisitsubmit = {} as IAssignvisit;

  }


  ngOnInit(): void {
    this.Getassignvisitsummary();
    this.Getvisittilecount();


    this.reactiveFormSubmit = new FormGroup({

      schedule_remarks: new FormControl(this.assignvisitsubmit.schedule_remarks, [
        Validators.required,

      ]),
      executive: new FormControl(this.assignvisitsubmit.executive, [
        Validators.required,

      ]),

      campaign_title: new FormControl(),

      campaign_gid: new FormControl(),

      leadbank_gid: new FormControl(),
      schedulelog_gid: new FormControl(),



    });

    var api6 = 'Assignvisit/Getmarketingteamdropdown'
    this.service.get(api6).subscribe((result: any) => {
      this.responsedata = result;
      this.marketingteamdropdown_list = this.responsedata.marketingteamdropdown_list;
    });

  }
  Getassignvisitsummary() {
    var url = 'Assignvisit/GetassignvisitSummaryToday'
    this.service.get(url).subscribe((result: any) => {
      $('#assignvisitlist').DataTable().destroy();
      this.responsedata = result;
      this.assignvisitlist = this.responsedata.assignvisitlist;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#assignvisitlist').DataTable();
      }, 1);


    });


  }
  marketingteam() {
    let campaign_gid = this.reactiveFormSubmit.get("campaign_gid")?.value;

    let params = {
      campaign_gid: campaign_gid
    }
    var url = 'Assignvisit/Getmarketingteamdropdownonchange'

    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;

      this.executive_list = this.responsedata.Getexecutedropdown;

    });


  }
  get schedule_remarks() {
    return this.reactiveFormSubmit.get('schedule_remarks')!;
  }

  get executive() {
    return this.reactiveFormSubmit.get('executive')!;
  }

  get campaign_title() {
    return this.reactiveFormSubmit.get('campaign_title')!;
  }

  get campaign_gid() {
    return this.reactiveFormSubmit.get('campaign_gid')!;
  }
  OnSubmit() {
    this.leadbank = this.reactiveFormSubmit.value;
    if (this.reactiveFormSubmit.value.executive != null && this.reactiveFormSubmit.value.campaign_gid != null) {
      var url1 = 'Assignvisit/GetAssignassignvisit'
      this.service.post(url1, this.leadbank).pipe().subscribe((result: any) => {

        if (result.status == false) {


          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
          this.Getassignvisitsummary();
          this.Getvisittilecount();
          this.reactiveFormSubmit.reset();


        }

      });

    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record and Excutive  ! ")
    }
  }

  Getvisittilecount() {
    var url = 'Assignvisit/Getvisittilecount'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.visittilecount_list = this.responsedata.visittilecount_list;
      this.todayvisit = this.visittilecount_list[0].todayvisit;
      this.upcoming = this.visittilecount_list[0].upcoming;
      this.expired = this.visittilecount_list[0].expired;
      this.totalvisit = this.visittilecount_list[0].totalvisit;
    });
  }
  onclose() {
    this.reactiveFormSubmit.reset()
  }
  openModalassign(parameter: string) {
    this.parametervalue = parameter
    this.reactiveFormSubmit.get("leadbank_gid")?.setValue(this.parametervalue.leadbank_gid);
    this.reactiveFormSubmit.get("schedule_remarks")?.setValue(this.parametervalue.schedule_remarks);
    this.reactiveFormSubmit.get("schedulelog_gid")?.setValue(this.parametervalue.schedulelog_gid);
    this.leadbankname=this.parametervalue.leadbank_name
  }
}
