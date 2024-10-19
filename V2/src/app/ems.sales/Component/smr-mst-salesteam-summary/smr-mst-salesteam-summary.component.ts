import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SalesTeamDualListComponent } from '../smr-mst-salesteam-summary/dual-list/dual-list.component';
import { NgxSpinnerService } from 'ngx-spinner';


interface Isalesteam {
  mail_id: string;
  team_name: any;
  branch_gid: string;
  employee_name: string;
  employee_gid: string;
  description: string;
  branch_name: any;
  campaign_description: string;
  campaign_location: string;
  campaign_mailid: string;
  campaign_title: string;
  campaign_gid: string;
  team_prefix: string;

}

@Component({
  selector: 'app-smr-mst-salesteam-summary',
  templateUrl: './smr-mst-salesteam-summary.component.html',
  styleUrls: ['./smr-mst-salesteam-summary.component.scss'],
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

export class SmrMstSalesteamSummaryComponent {
  reactiveForm!: FormGroup;
  reactiveFormEdit: FormGroup | any;
  responsedata: any;
  salesteam_list: any;
  assignedmanagers_list: any;
  assignedemployees_list: any;
  salesteamgrid_list: any;
  MdlManager: any;
  getData: any;
  salesteam!: Isalesteam;
  campaign_gid: any;
  branch_list: any[] = [];
  Getemployee: any[] = [];
  data: any;
  showOptionsDivId: any;
  parameterValue: any;
  editsalesteam_list: any;
  keepSorted = true;
  source: Array<any> = [];
  key!: string;
  key1!: string;
  display!: string;
  filter = false;
  confirmed: Array<any> = [];
  format: any = SalesTeamDualListComponent.DEFAULT_FORMAT;
  disabled = false;
  campaign_name: any;
  keepSorted1 = true;
  source1: Array<any> = [];
  key2!: string;
  key3!: string;
  display1!: string;
  filter1 = false;
  confirmed1: Array<any> = [];
  format1: any = SalesTeamDualListComponent.DEFAULT_FORMAT;
  disabled1 = false;

  parameterValue1: any;
  private confirmedStations: Array<any> = [];
  private sourceStations: Array<any> = [];
  sourceLeft = true;
  teamname: any;
  branchname: any;
  team: any;
  LeadBankCountList: any[] = [];
  rows:any []=[];

  leadchartcountlist: any[] = [];
  leadchartcount: any = {};
  saleschartcountlist: any[] = [];
  saleschartcount: any = {};
  quotationcountlist: any[] = [];
  quotationchartcount: any = {};
  enquirychartcountlist: any[] = [];
  enquirychartcount: any = {};
  response_data: any;
  teamactivitysummary_list: any[] = [];
  selectedChartType: any;
  chartscountsummary_list: any[] = [];
  teamCountVisible: boolean = false;

  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, public service: SocketService, private router: Router, private ToastrService: ToastrService) {
    this.salesteam = {} as Isalesteam;

    this.reactiveForm = new FormGroup({
      branch_name: new FormControl(''),
      branch_gid: new FormControl(''),
      employee_name: new FormControl(''),
      employee_gid: new FormControl(''),
      description: new FormControl(''),
      team_prefix: new FormControl(''),
    })
    this.reactiveFormEdit = new FormGroup({
      campaign_title: new FormControl(''),
      campaign_description: new FormControl(''),
      campaign_location: new FormControl(''),
      campaign_mailid: new FormControl(''),
      campaign_gid: new FormControl(''),

    })

  }


  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrMstSalesteamSummary();

    var url = 'SmrMstSalesteamSummary/GetSmrTrnTeamCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.LeadBankCountList = this.responsedata.teamcountlist;

    });

    if (this.LeadBankCountList && this.LeadBankCountList.length > 0) {
      this.teamCountVisible = true;
    }

    this.reactiveForm = new FormGroup({
      mail_id: new FormControl(this.salesteam.mail_id, [Validators.required,]),
      team_name: new FormControl(this.salesteam.team_name, [Validators.required,]),
      team_prefix: new FormControl(this.salesteam.team_prefix, [Validators.required,]),
      employee_name: new FormControl(this.salesteam.employee_name, [Validators.required,]),
      branch_name: new FormControl(this.salesteam.branch_name, [Validators.required,]),
      description: new FormControl(''),
    });
    this.reactiveFormEdit = new FormGroup({
      campaign_mailid: new FormControl(this.salesteam.campaign_mailid, [
        Validators.required,
      ]),
      campaign_title: new FormControl(this.salesteam.campaign_title, [
        Validators.required,
      ]),
      campaign_description: new FormControl(this.salesteam.campaign_description, [

      ]),
      campaign_location: new FormControl(this.salesteam.campaign_location, [
        Validators.required,
      ]),
      team_prefix: new FormControl(this.salesteam.team_prefix, [
        Validators.required,
      ]),
      campaign_gid: new FormControl(''),

      campaign_manager: new FormControl(''),
    });

    var url = 'SmrTrnSalesorder/GetBranchDtl'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.GetBranchDtl;
    });

    var url = 'SmrMstSalesteamSummary/Getemployee'
    this.service.get(url).subscribe((result: any) => {
      this.Getemployee = result.Getemployee;
    });
  }
  toggleOptions(campaign_gid: any) {
    if (this.showOptionsDivId === campaign_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = campaign_gid;
    }
  }
  //// Summary //////
  GetSmrMstSalesteamSummary() {

    var url = 'SmrMstSalesteamSummary/GetSmrMstSalesteamSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#salesteam_list').DataTable().destroy();
      this.responsedata = result;
      this.salesteam_list = this.responsedata.salesteam_list;
      setTimeout(() => {
        $('#salesteam_list').DataTable();
      }, 1);
    })
  }
  /////////For Add PopUp/////////
  get mail_id() {
    return this.reactiveForm.get('mail_id')!;
  }
  get team_name() {
    return this.reactiveForm.get('team_name')!;
  }
  get team_prefix() {
    return this.reactiveForm.get('team_prefix')!;
  }
  get branch_name() {
    return this.reactiveForm.get('branch_name')!;
  }
  get employee_name() {
    return this.reactiveForm.get('employee_name')!;
  }
  get campaign_mailid() {
    return this.reactiveFormEdit.get('campaign_mailid')!;
  }
  get campaign_title() {
    return this.reactiveFormEdit.get('campaign_title')!;
  }
  get team_prefixedit() {
    return this.reactiveFormEdit.get('team_prefix')!;
  }
  get campaign_location() {
    return this.reactiveFormEdit.get('campaign_location')!;
  }
  get description() {
    return this.reactiveFormEdit.get('campaign_description')!;
  }
  get employeeedit_name() {
    return this.reactiveFormEdit.get('campaign_manager')!;
  }
  public onsubmit(): void {

    if (this.reactiveForm.value.team_name != null && this.reactiveForm.value.mail_id != null && this.reactiveForm.value.team_prefix != null) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'SmrMstSalesteamSummary/PostSalesteam'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetSmrMstSalesteamSummary();
        }
        else {
          this.reactiveForm.get("team_name")?.setValue(null);
          this.reactiveForm.get("team_prefix")?.setValue(null);
          this.reactiveForm.get("mail_id")?.setValue(null);
          this.reactiveForm.get("branch_name")?.setValue(null);
          this.reactiveForm.get("employee_name")?.setValue(null);
          this.reactiveForm.get("description")?.setValue(null);
          this.ToastrService.success(result.message)
          this.GetSmrMstSalesteamSummary();
        }
      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  onassign(params: any) {

  }
  openModalexpand(params: any) {

  }
  redirecttolist() {
    this.router.navigate(['/smr/SmrMstSalesteamSummary']);
  }
  ////Expandable Grid////
  ondetail(campaign_gid: any) {
    debugger
    var url = 'SmrMstSalesteamSummary/GetSmrMstSalesteamgrid'
    let param = {
      campaign_gid: campaign_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesteamgrid_list = result.salesteamgrid_list;
      console.log(this.salesteamgrid_list)
      setTimeout(() => {
        $('#salesteamgrid_list').DataTable();
      }, 1);
    });
  }

  onclose() {
    this.reactiveForm.reset();
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

  filterBtn1() {
    return (this.filter1 ? 'Hide Filter' : 'Show Filter');
  }

  doDisable1() {
    this.disabled = !this.disabled1;
  }

  disableBtn1() {
    return (this.disabled1 ? 'Enable' : 'Disabled');
  }

  swapDirection() {
    this.sourceLeft = !this.sourceLeft;
    this.format.direction = this.sourceLeft ? SalesTeamDualListComponent.LTR : SalesTeamDualListComponent.RTL;
  }

  private useStations() {
    this.key = 'employee_gid';
    this.key1 = 'campaign_gid';
    this.display = 'employee_name';
    this.keepSorted = true;

    if (this.confirmedStations === null) {
      this.source = this.sourceStations
      this.confirmed = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed = this.confirmedStations;
      this.source = this.sourceStations

    }
    else {
      this.source = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed = this.confirmedStations;
      this.campaign_gid = this.parameterValue1.campaign_gid
    }
  }

  private usemanager() {
    this.key2 = 'employee_gid';
    this.key3 = 'campaign_gid';
    this.display1 = 'employee_name';
    this.keepSorted1 = true;

    if (this.confirmedStations === null) {
      this.source1 = this.sourceStations
      this.confirmed1 = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed1 = this.confirmedStations;
      this.source1 = this.sourceStations
    }
    else {
      this.source1 = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed1 = this.confirmedStations;
    }
  }

  employeelist() {

    this.NgxSpinnerService.show();
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
      campaign_location: this.parameterValue1.campaign_location,
    }

    var url = 'SmrMstSalesteamSummary/GetUnassignedEmplist'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.sourceStations = result.GetUnassignedEmplist;

      var url1 = 'SmrMstSalesteamSummary/GetAssignedEmplist'
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.confirmedStations = result.GetAssignedEmplist;
        this.useStations();
        this.NgxSpinnerService.hide();
      });
    });
  }

  managerlist() {
    this.NgxSpinnerService.show();
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
      campaign_location: this.parameterValue1.campaign_location,
    }
    var url = 'SmrMstSalesteamSummary/GetUnassignedManagerlist'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.sourceStations = result.GetUnassignedManagerlist;

      var url1 = 'SmrMstSalesteamSummary/GetAssignedManagerlist'
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.confirmedStations = result.GetAssignedManagerlist;
        this.usemanager();
        this.NgxSpinnerService.hide();
      });
    });
  }

  openModalemployeelist(parameter: string) {
    this.parameterValue1 = parameter;
    console.log(this.parameterValue1)
    this.teamname = this.parameterValue1.campaign_title;
    this.branchname = this.parameterValue1.branch_name;
    this.employeelist();
  }

  openModalmanagerlist(parameter: string) {
    this.parameterValue1 = parameter
    this.teamname = this.parameterValue1.campaign_title;
    this.branchname = this.parameterValue1.branch_name;
    this.managerlist();
  }

  openModaledit(campaign_gid: any) {
    debugger
    var url = 'SmrMstSalesteamSummary/GetEditSalesTeamSummary'

    let param = {

      campaign_gid: campaign_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.editsalesteam_list = result.editsalesteam_list;

      this.reactiveFormEdit.get("campaign_mailid")?.setValue(this.editsalesteam_list[0].campaign_mailid);
      this.reactiveFormEdit.get("campaign_title")?.setValue(this.editsalesteam_list[0].campaign_title);
      this.reactiveFormEdit.get("campaign_location")?.setValue(this.editsalesteam_list[0].campaign_location);
      this.reactiveFormEdit.get("campaign_description")?.setValue(this.editsalesteam_list[0].campaign_description);
      this.reactiveFormEdit.get("campaign_gid")?.setValue(this.editsalesteam_list[0].campaign_gid);
      this.reactiveFormEdit.get("team_prefix")?.setValue(this.editsalesteam_list[0].team_prefix);
      this.MdlManager = this.editsalesteam_list[0].campaign_manager;
    });
  }


  public onupdate(): void {
    if (this.reactiveFormEdit.value.campaign_mailid != null && this.reactiveFormEdit.value.campaign_title != null && this.reactiveFormEdit.value.campaign_gid != null && this.reactiveFormEdit.value.campaign_location != null && this.reactiveFormEdit.value.campaign_description != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      //console.log(this.reactiveFormEdit.value)
      var url = 'SmrMstSalesteamSummary/PostUpdateSalesTeam'

      this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetSmrMstSalesteamSummary();
        }
        else {
          this.ToastrService.success(result.message)
          this.GetSmrMstSalesteamSummary();
        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  Details(parameter: string, campaign_gid: string) {
    this.parameterValue1 = parameter;
    this.campaign_gid = parameter;

    var url = 'SmrMstSalesteamSummary/GetSmrMstSalesteamgrid'
    let param = {
      campaign_gid: campaign_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesteamgrid_list = result.salesteamgrid_list;
      this.campaign_name = result.campaign_name;
      this.team = this.salesteam_list[0].campaign_title;
      console.log(this.salesteamgrid_list)
      setTimeout(() => {
        $('#salesteamgrid_list').DataTable();
      }, 1);

    });
  }

  toggleStatus(data: any) {

    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }
  openModalinactive(parameter: string) {
    this.parameterValue = parameter
  }
  oninactive() {
    console.log(this.parameterValue);
    var url3 = 'SmrMstSalesteamSummary/GetTeamInactive'
    this.NgxSpinnerService.show()
    this.service.getparams(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning('Error While Team Inactivated');
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success('Team Inactivated Successfully');
        this.NgxSpinnerService.hide()
      }
      this.GetSmrMstSalesteamSummary();
    });

  }
  openModalactive(parameter: string) {
    this.parameterValue = parameter
  }

  onactive() {
    console.log(this.parameterValue);
    var url3 = 'SmrMstSalesteamSummary/GetTeamActive'
    this.NgxSpinnerService.show()

    this.service.getparams(url3, this.parameterValue).subscribe((result: any) => {

      if (result.status == false) {
        this.ToastrService.warning('Error While Activating Team');
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.success('Team Activated Successfully');
        this.NgxSpinnerService.hide()
      }
      this.GetSmrMstSalesteamSummary();
    });
  }

  getmanagers(campaign_gid: any) {
    var param = {
      campaign_gid: campaign_gid,
    }
    var url = 'SmrMstSalesteamSummary/GetManagers';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.assignedmanagers_list = result.countpopuplist;
    });
  }
  getemployee(campaign_gid: any) {
    var param1 = {
      campaign_gid: campaign_gid,
    }
    var url = 'SmrMstSalesteamSummary/GetEmployee';
    this.service.getparams(url, param1).subscribe((result: any) => {
      this.assignedemployees_list = result.countemplist;
    });
  }
  getListFromManagerList(data: any) {
    this.salesteam_list = data;
  }

  getListFromEmployeeList(data: any) {
    this.salesteam_list = data;
  }

}
