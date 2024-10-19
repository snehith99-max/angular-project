
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute,Router } from '@angular/router';
import { AES, enc } from 'crypto-js';

import { NgxSpinnerService } from 'ngx-spinner';

interface ITeam {
  branch: string;
  team: string;
  description: string;
  team_name: string;
  team_manager: string;
  mail_id: string;
  team_name_edit: string;
  branch_edit: string;
  description_edit: string;
  mail_id_edit: string;
  campaign_gid: string;
  assign_employee: string;
  assign_manager: string;
  no_of_leads_assigned: string;
  team_count: string;
  team_prefix:string;
  team_prefix_edit:string;

}
@Component({
  selector: 'app-smr-trn-assignrenewalagreement',
  templateUrl: './smr-trn-assignrenewalagreement.component.html',
  styleUrls: ['./smr-trn-assignrenewalagreement.component.scss']
})
export class SmrTrnAssignrenewalagreementComponent{
  form: FormGroup = new FormGroup({
    campagin_gid: new FormControl(null),


  });
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;

  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  unassigned_list: any;
  team!: ITeam;
  team_list1: any[] = [];
  team_list: any[] = [];
  campaign_gid: any;
  branch_list: any[] = [];
  marketingteam_list1: any[] = [];
  LeadBankCountList: any[] = [];
  assignedmanagers_list: any[] = [];
  assignedemployees_list: any[] = [];
  assignedlead_list: any[] = [];
  teamname: any;
  branch_name: any;
  tab = 1;
  renewal_gid_key:any;
  sourceLeft = true;
  campaign_name: any;
  renewal_gid1:any;
  selectedBranch: any;
  showOptionsDivId: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService,private router: ActivatedRoute) {
    this.team = {} as ITeam;
  }
  ngOnInit(): void {
    this.GetTeamSummary();
    const renewal_gid = this.router.snapshot.paramMap.get('renewal_gid');
    const key = 'storyboarderp';
    this.renewal_gid_key = renewal_gid;
    this.renewal_gid1 = AES.decrypt(this.renewal_gid_key, key).toString(enc.Utf8);

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    
  }
  GetTeamSummary() {
    this.NgxSpinnerService.show();
    var api = 'SmrTrnRenewalsummary/GetassignTeamSummary';
    let param = {}
    this.service.get(api).subscribe((result: any) => {
      $('#team_list1').DataTable().destroy();
      this.responsedata = result;
      this.team_list1 = this.responsedata.renewalassignteam_list;
      this.NgxSpinnerService.hide();
    console.log(this.responsedata.renewalassignteam_list)
      setTimeout(() => {
        $('#team_list1').DataTable();
      }, 1);
    });
  }
  getListFromManagerList(data:any){
    this.team_list1 = data;
}
getmanagers(campaign_gid: any) {
  var param = {
    campaign_gid: campaign_gid,
  }
  var url = 'SmrTrnRenewalteamsummary/GetManagers';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.assignedmanagers_list = result.popup_list;
  });
}
getemployees(campaign_gid: any) {
  var param1 = {
    campaign_gid: campaign_gid,
  }
  var url = 'SmrTrnRenewalteamsummary/GetEmployees';
  this.service.getparams(url, param1).subscribe((result: any) => {
    this.assignedemployees_list = result.popup_list;
  });
}
openModaldelete(parameter: string) {
  this.parameterValue = parameter
}
ondelete() {
  var url = 'SmrTrnRenewalteamsummary/DeleteTeam'
  let param = {
    campaign_gid: this.parameterValue
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    if (result.status == false) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });

      this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message)
     
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });

      this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message)
    }
    this.GetTeamSummary();
    //this.GetMarketingTeamCount();
  });
}
  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormEdit.reset();
  }
  public onsubmit(): void {
    if (this.reactiveForm.value.team_name != null && this.reactiveForm.value.team_prefix != null && this.reactiveForm.value.branch != null && this.reactiveForm.value.team_manager != null && this.reactiveForm.value.mail_id != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'SmrTrnRenewalteamsummary/PostRenewalteam'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetTeamSummary();
          this.reactiveForm.reset();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveForm.get("team_name")?.setValue(null);
          this.reactiveForm.get("branch")?.setValue(null);
          this.reactiveForm.get("team_manager")?.setValue(null);
          this.reactiveForm.get("mail_id")?.setValue(null);
          this.ToastrService.success(result.message)
          this.GetTeamSummary();
          this.reactiveForm.reset();
        }
        this.GetTeamSummary();
        this.reactiveForm.reset();
      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.reactiveForm.reset();
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  onassign(campaign_gid: any) {
    const secretKey = 'storyboarderp';
    const encryptedRenewalGid = AES.encrypt(this.renewal_gid1, secretKey).toString();
    const encryptedCampaignGid = AES.encrypt(campaign_gid, secretKey).toString();
    this.route.navigate(['/smr/SmrTrnRenewalemployee', encryptedCampaignGid, encryptedRenewalGid]);
  }




}

