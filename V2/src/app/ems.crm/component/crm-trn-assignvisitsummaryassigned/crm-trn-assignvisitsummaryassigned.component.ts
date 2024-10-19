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
  campaign_title: string= "";




}
interface IAssignvisit {
  campaign_title: string;
  campaign_gid: string;
  schedule_remarks: string;
  executive: string;




}

@Component({
  selector: 'app-crm-trn-assignvisitsummaryassigned',
  templateUrl: './crm-trn-assignvisitsummaryassigned.component.html',
  styleUrls: ['./crm-trn-assignvisitsummaryassigned.component.scss']
})
export class CrmTrnAssignvisitsummaryassignedComponent {

  assignvisitlist: any[] = [];
  summary_list: any[] = [];
  breadcrumb_lists: any[] = [];

  responsedata: any;

  reactiveForm!: FormGroup;
  marketingteamdropdown_list: any[] = [];
  lead_dropdown: any[] = [];
  reactiveFormSubmit!: FormGroup;
  executive_list: any[] = [];
  pick: Array<any> = [];
  CurObj: IAssign = new IAssign();
  assignvisitsubmit!: IAssignvisit;
  selection = new SelectionModel<IAssign>(true, []);
  IAssign: any;
  Assignedstatus: any;
  visittilecount_list: any;
  todayvisit: any;
  totalvisit: any;
  expired: any;
  upcoming: any;
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



    });

    var api6='Assignvisit/Getmarketingteamdropdown'
    this.service.get(api6).subscribe((result:any)=>{
      this.responsedata=result;
      this.marketingteamdropdown_list = this.responsedata.marketingteamdropdown_list;   
        });
        var api6='Assignvisit/Getleadbankdropdown'
        this.service.get(api6).subscribe((result:any)=>{
          this.responsedata=result;
          this.lead_dropdown = this.responsedata.lead_dropdown;   
            });
     
  }
  Getassignvisitsummary() {
    var url = 'Assignvisit/Getassignvisitsummary'
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
      this.responsedata=result;

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


  
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assignvisitlist.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.assignvisitlist.forEach((row: IAssign) => this.selection.select(row));
  }

  
  onclose() {
    this.reactiveFormSubmit.reset()
  }
  Getvisittilecount(){
    debugger
     var url = 'Assignvisit/Getvisittilecount'
     this.service.get(url).subscribe((result: any) => {
       debugger
      this.responsedata=result;
      this.visittilecount_list = this.responsedata.visittilecount_list;   
      this.todayvisit = this.visittilecount_list[0].todayvisit;
      this.upcoming = this.visittilecount_list[0].upcoming;
      this.expired = this.visittilecount_list[0].expired;
      this.totalvisit = this.visittilecount_list[0].totalvisit;
     });
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
}
