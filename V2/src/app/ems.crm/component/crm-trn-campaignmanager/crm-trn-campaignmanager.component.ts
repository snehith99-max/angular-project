import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { param } from 'jquery';
export class ICampaign {
  campaign_gid: string = "";
  campaign_list: string[] = [];
  assign_user: string = "";
  team_name: string = "";
  team_member: string = "";
  schedule_remarks: string = "";
  schedule_date: string = "";
  schedule_time: string = "";
  schedule_type: string = "";

}
interface ITransfer {
  team_name: string;
  team_member: string;
  schedule_date:string;
  schedule_time:string;
  schedule_type:string;
  schedule_remarks:string;
}
@Component({
  selector: 'app-crm-trn-campaignmanager',
  templateUrl: './crm-trn-campaignmanager.component.html',
  styleUrls: ['./crm-trn-campaignmanager.component.scss']
})
export class CrmTrnCampaignmanagerComponent  {
  employee_list: any;
  selectedDate:Date;
  selectedTime: Date = new Date();
  reactiveFormSchedule!: FormGroup;
  assign_user: any;
  reactiveFormTransfer!: FormGroup;
  ScheduleType = [
    { type: 'Call Log', },
    { type: 'Meeting', },
    { type: 'Mail Log', },
  ];
  CampaignmanagerSummary_list: any;
  team_list: any;
  campaign_title: any;
  user_name: any;
  pick: Array<any> = [];
  selectedItems: string[] = [];
  assignlist: string[] = [];
  CurObj: ICampaign = new ICampaign();
  parameterValue1: any;
  parameterValue2: any;
  internal_notes:any;
  leadbank_name:any;
  teamname_list: any;
  selection = new SelectionModel<ICampaign>(true, []);
  ICampaign: any;
  transfer!: ITransfer;
  constructor(private router: ActivatedRoute, private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router) {
    this.transfer = {} as ITransfer;
    this.selectedDate = new Date();
  }

  ngOnInit() {
    this.reactiveFormTransfer = new FormGroup({

      team_name: new FormControl(this.transfer.team_name, [
        Validators.required,

      ]),
      team_member: new FormControl(this.transfer.team_member, [
        Validators.required,

      ]),




    });
    this.reactiveFormSchedule = new FormGroup({

      schedule_date: new FormControl(this.transfer.schedule_date, [
        Validators.required,

      ]),
      schedule_time: new FormControl(this.transfer.schedule_time, [
        Validators.required,

      ]),
      schedule_type: new FormControl(this.transfer.schedule_type, [
        Validators.required,

      ]),
      schedule_remarks: new FormControl(),




    });
    var api1 = 'MarketingManager/GetTeamNamedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.teamname_list = result.GetTeamNamedropdown;
      //console.log(this.branch_list)
    });
    const options: Options = {
      dateFormat: 'd-m-Y',

    };

    flatpickr('.date-picker', options);
    this.router.params.subscribe(params => {
      const campaigngid = params['campaign_gid'];
      const assignto = params['assign_to'];
      const stage = params['stage'];

      const secretKey = 'storyboarderp';

      const campaign_gid = AES.decrypt(campaigngid, secretKey).toString(enc.Utf8);
      const assign_to = AES.decrypt(assignto, secretKey).toString(enc.Utf8);
      this.assign_user = AES.decrypt(assignto, secretKey).toString(enc.Utf8);
      const stages = AES.decrypt(stage, secretKey).toString(enc.Utf8);
      let param = {
        campaign_gid: campaign_gid,
        assign_to: assign_to,
        stages: stages,
      }
      // console.log(param)
      this.GetCampaignmanagerSummary(param);
      let para = {
        campaign_gid: campaign_gid,
        assign_to: assign_to,
      }
      var url1 = 'MarketingManager/GetCampaignmanagerTeam'

      this.service.getparams(url1, para).subscribe((result: any) => {
        // this.responsedata=result;
        this.team_list = result.GetCampaignmanagerTeam;
        this.campaign_title = this.team_list[0].campaign_title;
        this.user_name = this.team_list[0].user_firstname;

      });

      // Use the id and category parameters as needed in your component
    });
  }
  GetCampaignmanagerSummary(param: any) {
    var url = 'MarketingManager/GetCampaignmanagerSummary'

    this.service.getparams(url, param).subscribe((result: any) => {
      $('#CampaignmanagerSummary_list').DataTable().destroy();
      // this.responsedata=result;
      this.CampaignmanagerSummary_list = result.GetCampaignmanagerSummary;
      setTimeout(() => {
        $('#CampaignmanagerSummary_list').DataTable();
      }, 1);

    });
  }

  get team_name() {
    return this.reactiveFormTransfer.get('team_name')!;
  }
  get team_member() {
    return this.reactiveFormTransfer.get('team_member')!;
  }
  get schedule_type() {
    return this.reactiveFormSchedule.get('schedule_type')!;
  }
  get schedule_date() {
    return this.reactiveFormSchedule.get('schedule_date')!;
  }
  get schedule_time() {
    return this.reactiveFormSchedule.get('schedule_time')!;
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.CampaignmanagerSummary_list.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.CampaignmanagerSummary_list.forEach((row: ICampaign) => this.selection.select(row));
  }

  onsubmitschedule() {

    this.selection.selected.forEach(s => s.campaign_gid);


    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.schedule_date = this.reactiveFormSchedule.value.schedule_date;
    this.CurObj.schedule_time = this.reactiveFormSchedule.value.schedule_time;
    this.CurObj.schedule_type = this.reactiveFormSchedule.value.schedule_type;
     this.CurObj.schedule_remarks = this.reactiveFormSchedule.value.schedule_remarks;
    this.CurObj.assign_user = this.assign_user
    this.CurObj.campaign_list = list
    if (this.CurObj.campaign_list.length != 0) {
      var url1 = 'MarketingManager/GetCampaignSchedule'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {


          this.ToastrService.warning('Error While Lead  Schedule')
        }
        else {
          this.ToastrService.success('Lead Schedule Successfully')
          window.location.reload();

        }

      });

    }
    else {
      this.ToastrService.warning("Kindly Check Atleast One Record ! ")
    }
  }
  OnTransfer() {

    this.selection.selected.forEach(s => s.campaign_gid);
    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.team_name = this.reactiveFormTransfer.value.team_name;
    this.CurObj.team_member = this.reactiveFormTransfer.value.team_member;
    this.CurObj.assign_user = this.assign_user
    this.CurObj.campaign_list = list

    if (this.CurObj.campaign_list.length != 0) {
      var url1 = 'MarketingManager/GetCampaignMoveToTransfer'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {


          this.ToastrService.warning('Error While Lead  Transfer')
        }
        else {
          this.ToastrService.success('Lead Transfer Successfully')
          window.location.reload();

        }

      });

    }
    else {
      this.ToastrService.warning("Kindly Check Atleast One Record ! ")
    }


  }
  OnBin() {

    this.selection.selected.forEach(s => s.campaign_gid);


    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.assign_user = this.assign_user
    this.CurObj.campaign_list = list

    if (this.CurObj.campaign_list.length != 0) {
      var url1 = 'MarketingManager/GetCampaignMoveToBin'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {


          this.ToastrService.warning('Error While Lead Moved to MyBin')
        }
        else {
          this.ToastrService.success('Lead Moved to MyBin Successfully')
          window.location.reload();

        }

      });

    }
    else {
      this.ToastrService.warning("Kindly Check Atleast One Record ! ")
    }


  }
  onclosetransfer() {
 this.reactiveFormTransfer.reset();
  }
  oncloseschedule(){
    this.reactiveFormSchedule.reset();
  }
  teamname() {
    let team_gid = this.reactiveFormTransfer.get("team_name")?.value;
    let param = {
      team_gid: team_gid
    }

    var url = 'MarketingManager/GetTeamEmployeedropdown'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.employee_list = result.GetTeamEmployeedropdown;

    });
   

  }
  popmodal(parameter: string, parameter1: string) {
      
    this.internal_notes = parameter;
    this.leadbank_name = parameter1;
}
onmodal(params: any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.route.navigate(['/crm/CrmTrn360view', encryptedParam])
}
}
