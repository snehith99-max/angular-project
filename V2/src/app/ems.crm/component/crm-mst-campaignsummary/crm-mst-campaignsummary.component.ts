
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { DualListComponent } from './dual-list/dual-list.component';
import { ManagerListComponent } from './manager-list/manager-list.component';
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
  selector: 'app-crm-mst-campaignsummary',
  templateUrl: './crm-mst-campaignsummary.component.html',
  styleUrls: ['./crm-mst-campaignsummary.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})


export class CrmMstCampaignsummaryComponent {
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
  keepSorted = true;
  keepSorted1 = true;
  key!: string;
  key1!: string;
  key2!: string;
  key3!: string;
  display!: string;
  display1!: string;
  filter = false;
  source: Array<any> = [];
  confirmed: Array<any> = [];
  filter1 = false;
  source1: Array<any> = [];
  confirmed1: Array<any> = [];
  userAdd = '';
  disabled = false;
  disabled1 = false;
  sourceLeft = true;
  format: any = DualListComponent.DEFAULT_FORMAT;
  campaign_name: any;

  format1: any = ManagerListComponent.DEFAULT_FORMAT;

  private sourceStations: Array<any> = [];


  private confirmedStations: Array<any> = [];
  selectedBranch: any;
  showOptionsDivId: any;


  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
    this.team = {} as ITeam;
  }
  ngOnInit(): void {
    this.GetTeamSummary();
    this.GetMarketingTeamCount();

    this.reactiveForm = new FormGroup({

      team_name: new FormControl(this.team.team_name, [
        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)
        


      ]),
      team_prefix: new FormControl(this.team.team_prefix, [
        Validators.required,


      ]),
      description: new FormControl('', [

        Validators.maxLength(300)


      ]),
      branch: new FormControl(this.team.branch, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      team_manager: new FormControl(this.team.team_manager, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      mail_id: new FormControl(this.team.mail_id, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern("^([a-z0-9-]+|[a-z0-9-]+([.][a-z0-9-]+)*)@([a-z0-9-]+\.[a-z]{2,20}(\.[a-z]{2})?|\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\]|localhost)$")

      ]),

    });
    this.reactiveFormEdit = new FormGroup({

      team_name_edit: new FormControl(this.team.team_name_edit, [
        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)


      ]),
      team_prefix_edit: new FormControl(this.team.team_prefix_edit, [
        Validators.required,


      ]),
      description_edit: new FormControl('', [
        Validators.maxLength(250)


      ]),
      branch_edit: new FormControl(this.team.branch_edit, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      mail_id_edit: new FormControl(this.team.mail_id_edit, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern("^([a-z0-9-]+|[a-z0-9-]+([.][a-z0-9-]+)*)@([a-z0-9-]+\.[a-z]{2,20}(\.[a-z]{2})?|\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\]|localhost)$")

      ]),
      campaign_gid: new FormControl(''),
    });

    var api = 'CampaignSummary/Getbranchdropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.branch_list1;
    });

    var api = 'CampaignSummary/Getteammanagerdropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.team_list = this.responsedata.team_list;
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetTeamSummary() {
    this.NgxSpinnerService.show();
    var api = 'CampaignSummary/GetTeamSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#team_list1').DataTable().destroy();
      this.responsedata = result;
      this.team_list1 = this.responsedata.marketingteam_list;
      this.NgxSpinnerService.hide();
      //console.log(this.responsedata.marketingteam_list)
      setTimeout(() => {
        $('#team_list1').DataTable();
      }, 1);
    });
  }


  get team_name() {
    return this.reactiveForm.get('team_name')!;
  }
  // get description() {
  //   return this.reactiveForm.get('description')!;
  // }
  get branch() {
    return this.reactiveForm.get('branch')!;
  }
  get team_manager() {
    return this.reactiveForm.get('team_manager')!;
  }
  get mail_id() {
    return this.reactiveForm.get('mail_id')!;
  }
  get team_prefix() {
    return this.reactiveForm.get('team_prefix')!;
  }
  get TeamCount() {
    return this.reactiveFormEdit.get('TeamCount')!;
  }

  get team_name_edit() {
    return this.reactiveFormEdit.get('team_name_edit')!;
  }
  // get description_edit() {
  //   return this.reactiveFormEdit.get('description_edit')!;
  // }
  get branch_edit() {
    return this.reactiveFormEdit.get('branch_edit')!;
  }
  get mail_id_edit() {
    return this.reactiveFormEdit.get('mail_id_edit')!;
  }
  get team_prefix_edit() {
    return this.reactiveFormEdit.get('team_prefix_edit')!;
  }



  public onsubmit(): void {
    if (this.reactiveForm.value.team_name != null && this.reactiveForm.value.team_prefix != null && this.reactiveForm.value.branch != null && this.reactiveForm.value.team_manager != null && this.reactiveForm.value.mail_id != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'CampaignSummary/Postmarketingteam'
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
        this.GetMarketingTeamCount();
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

  ondetail(campaign_gid: any) {
    var url = 'CampaignSummary/GetMarketingTeamDetailTable'
    let param = {
      campaign_gid: campaign_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#marketingteam_list1').DataTable().destroy();
      this.responsedata = result;
      this.campaign_name = result.campaign_name

      this.marketingteam_list1 = result.marketingteam_list1;
      console.log(this.marketingteam_list1)
      setTimeout(() => {
        $('#marketingteam_list1').DataTable();
      }, 1);

    });
  }

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("team_name_edit")?.setValue(this.parameterValue1.team_name);
    this.reactiveFormEdit.get("team_prefix_edit")?.setValue(this.parameterValue1.team_prefix);
    this.reactiveFormEdit.get("description_edit")?.setValue(this.parameterValue1.description);
    this.reactiveFormEdit.get("branch_edit")?.setValue(this.parameterValue1.branch);
    this.selectedBranch = this.parameterValue1.branch_gid;
    this.reactiveFormEdit.get("mail_id_edit")?.setValue(this.parameterValue1.mail_id);
    this.reactiveFormEdit.get("campaign_gid")?.setValue(this.parameterValue1.campaign_gid);

  }
  openModalemployee(parameter: string) {
    this.parameterValue1 = parameter;
    this.teamname = this.parameterValue1.team_name;
    this.branch_name = this.parameterValue1.branch;
    //console.log(this.parameterValue1)
    this.teamemployee();
  }
  openModalmanager(parameter: string) {
    this.parameterValue1 = parameter
    this.teamname = this.parameterValue1.team_name;
    this.branch_name = this.parameterValue1.branch;
    this.campaign_gid = this.parameterValue1.campaign_gid;
    this.teammanager();
  }
  private useStations() {
    this.key = 'employee_gid';
    this.key1 = 'campaign_gid';
    this.display = 'employee_name'; // [ 'vendor_companyname', 'vendor_gid' ];
    this.keepSorted = true;
    //console.log(this.confirmedStations)
    if (this.confirmedStations === null) {
      this.source = this.sourceStations
      this.confirmed = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed = this.confirmedStations;
      this.source = this.sourceStations

    }
    // else if(this.sourceStations===null){
    //    this.source = '0'
    //   this.confirmed = [this.confirmedStations];
    // }
    else {
      this.source = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed = this.confirmedStations;
      this.campaign_gid = this.parameterValue1.campaign_gid
    }
  }
  private usemanager() {
    this.key2 = 'employee_gid';
    this.key3 = 'campaign_gid';
    this.display1 = 'employee_name'; // [ 'vendor_companyname', 'vendor_gid' ];
    this.keepSorted1 = true;
    //console.log(this.confirmedStations)
    if (this.confirmedStations === null) {
      this.source1 = this.sourceStations
      this.confirmed1 = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed1 = this.confirmedStations;
      this.source1 = this.sourceStations

    }
    // else if(this.sourceStations===null){
    //    this.source = '0'
    //   this.confirmed = [this.confirmedStations];
    // }
    else {
      this.source1 = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed1 = this.confirmedStations;

    }
  }


  teamemployee() {
debugger
    this.NgxSpinnerService.show();
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
      campaign_location: this.parameterValue1.campaign_location,
    }
    var url = 'CampaignSummary/GetUnassignedlist'

    this.service.getparams(url, param).subscribe((result: any) => {
      // this.responsedata=result;
      this.sourceStations = result.GetUnassignedlist;
      var url1 = 'CampaignSummary/GetAssignedlist'
      this.service.getparams(url1, param).subscribe((result: any) => {
        // this.responsedata=result;
        this.confirmedStations = result.GetAssignedlist;
        this.useStations();
        this.NgxSpinnerService.hide();
      });
    });


  }

  teammanager() {
    this.NgxSpinnerService.show();
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
      campaign_location: this.parameterValue1.campaign_location,
    };

    var url = 'CampaignSummary/GetManagerUnassignedlist';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.sourceStations = result.GetManagerUnassignedlist || [];
      var url1 = 'CampaignSummary/GetManagerAssignedlist';
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.confirmedStations = result.GetManagerAssignedlist || [];
        this.usemanager();
        this.NgxSpinnerService.hide();
      });
    });
  }


  filterBtn() {
    return (this.filter ? 'Hide Filter' : 'Show Filter');
  }

  doDisable() {
    this.disabled = !this.disabled;
  }

  disableBtn() {
    return (this.disabled ? 'Enable' : 'Disabled');
  }

  swapDirection() {
    this.sourceLeft = !this.sourceLeft;
    this.format.direction = this.sourceLeft ? DualListComponent.LTR : DualListComponent.RTL;

  }

  filterBtn1() {
    return (this.filter1 ? 'Hide Filter' : 'Show Filter');
  }

  doDisable1() {
    this.disabled = !this.disabled1;
  }

  disableBtn1() {
    return (this.disabled1 ? 'Enable' : 'Disabled');
  }

  swapDirection1() {
    this.sourceLeft = !this.sourceLeft;
    this.format1.direction = this.sourceLeft ? ManagerListComponent.LTR : ManagerListComponent.RTL;

  }
  ////////Update popup////////
  public onupdate(): void {
    if (this.reactiveFormEdit.value.team_name_edit != null && this.reactiveFormEdit.value.team_prefix_edit != null && this.reactiveFormEdit.value.branch_edit != null && this.reactiveFormEdit.value.mail_id_edit != null) {

      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;
      var url = 'CampaignSummary/Updatedmarketingteam'

      this.service.post(url, this.reactiveFormEdit.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetTeamSummary();
          this.reactiveFormEdit.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveFormEdit.get("team_name_edit")?.setValue(null);
          this.reactiveFormEdit.get("branch_edit")?.setValue(null);
          this.reactiveFormEdit.get("mail_id_edit")?.setValue(null);
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide();
          this.GetTeamSummary();
          this.reactiveFormEdit.reset();
        }
        this.GetTeamSummary();
        this.reactiveFormEdit.reset();
      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.reactiveFormEdit.reset();
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    var url = 'CampaignSummary/DeleteTeam'
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
      this.GetMarketingTeamCount();
    });
  }

  onassign(params: any) {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const campaign_gid = AES.encrypt(params.campaign_gid, secretKey).toString();
    const employee_gid = AES.encrypt(params.employee_gid, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnCampaignleadallocation', campaign_gid, employee_gid])
  }
  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormEdit.reset();
  }
  onexpand(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const campaign_gid = AES.encrypt(params, secretKey).toString();
    this.route.navigate(['/crm/CrmMstCampaignexpand', campaign_gid])
  }
  getmanagers(campaign_gid: any) {
    var param = {
      campaign_gid: campaign_gid,
    }
    var url = 'CampaignSummary/GetManagers';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.assignedmanagers_list = result.popup_list;
    });
  }
  getemployees(campaign_gid: any) {
    var param1 = {
      campaign_gid: campaign_gid,
    }
    var url = 'CampaignSummary/GetEmployees';
    this.service.getparams(url, param1).subscribe((result: any) => {
      this.assignedemployees_list = result.popup_list;
    });
  }
  getleadslist(campaign_gid: any) {
    var param = {
      campaign_gid: campaign_gid,
    }
    var url = 'CampaignSummary/GetAssignlead';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.assignedlead_list = result.popup_list;
    });
  }
  GetMarketingTeamCount(){
    var api = 'CampaignSummary/GetMarketingTeamCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.LeadBankCountList = this.responsedata.MarketingTeamCount_List;

    });
  }
  getListFromManagerList(data:any){
    this.team_list1 = data;
}
toggleOptions(campaign_gid: any) {
  if (this.showOptionsDivId === campaign_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = campaign_gid;
  }
}
}
