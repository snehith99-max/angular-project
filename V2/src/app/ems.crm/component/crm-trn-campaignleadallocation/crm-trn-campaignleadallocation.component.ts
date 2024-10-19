import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface ICampaign {
  schedule_remarks: string;
  company_name: string;
  source_name: string;
  region: string;
  industry: string;
  city: string;
  state: string;
  company: string;
  existing_customer: string;
  campaign_gid: string;
  user_name:string;
  campaign_name:string;
  Rowcount:string;
  campaign_title:string;
  employee_gid: string;
  leadbank_gid: string;
  remarks:string;
  leadbank_name:string;

  
}

interface ISearchEvent{
  region_name: string;
  source_name: string;
  customer_type: string;

}
export class IAssignlead {
  summary_list1: string[] = [];
  leadbank_gid: string = "";
  schedule_remarks: string = "";
  employee_gid: string = "";
  campaign_gid: string = "";
}

@Component({
  selector: 'app-crm-trn-campaignleadallocation',
  templateUrl: './crm-trn-campaignleadallocation.component.html',
  styleUrls: ['./crm-trn-campaignleadallocation.component.scss']
})
export class CrmTrnCampaignleadallocationComponent {
  reactiveFormSubmit!: FormGroup;
  campaign!: ICampaign;
  source_list: any[] = [];
  region_list: any[] = [];
  industry_list: any[] = [];
  assign_list: any[] = [];
  user_list:any[] = [];
  summary_list1: any[] = [];
  selection = new SelectionModel<IAssignlead>(true, []);
  responsedata: any;
  IAssignlead: any;
  CurObj: IAssignlead = new IAssignlead();
  pick: Array<any> = [];
  campaign_gid: any;
  employee_gid: any;
  leadbank_gid: any;
  params: any;
  parameterValue1: any;
  parameterValue2: any;
  campaign_name:any;
  remarks: any;
  leadbank_name:any;
  user_name:any;
  isButtonDisabled = false;
  isAnyCheckboxSelected = false;
  SearchEventForm: FormGroup | any;
  regionnamelist: any[] = [];
  customertype_list: any[] = [];
  SearchEvent!: ISearchEvent;
  

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
    this.campaign = {} as ICampaign;
    this.SearchEvent = {} as ISearchEvent;
  }
  ngOnInit(): void {

    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    const campaign_gid = this.router.snapshot.paramMap.get('campaign_gid');

    this.employee_gid = employee_gid;
    this.campaign_gid = campaign_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.campaign_gid, secretKey).toString(enc.Utf8);

    this.employee_gid = deencryptedParam;
    this.campaign_gid = deencryptedParam1;


    this.reactiveFormSubmit = new FormGroup({
      company_name: new FormControl(this.campaign.company_name, [
      ]),
      source_name: new FormControl(this.campaign.source_name, [
      ]),
      region: new FormControl(this.campaign.region, [
      ]),
      industry: new FormControl(this.campaign.industry, [
      ]),
      city: new FormControl(this.campaign.city, [
      ]),
      state: new FormControl(this.campaign.state, [
      ]),
      company: new FormControl(this.campaign.company, [
      ]),
      existing_customer: new FormControl(this.campaign.existing_customer, [
      ]),
     
      leadbank_name: new FormControl(this.campaign.leadbank_name),
      
      campaign_name: new FormControl(this.campaign.campaign_name
        ),
        user_name: new FormControl(this.campaign.user_name
          ),
  
    });


    this.SearchEventForm = new FormGroup({
      region_name: new FormControl(this.SearchEvent.region_name, [   
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      source_name: new FormControl(this.SearchEvent.source_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      customer_type: new FormControl(this.SearchEvent.customer_type, [
        
      ]),
    });

    var api = 'CampaignSummary/GetSourcedropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = this.responsedata.dropdown_list1;
    });
    var api = 'CampaignSummary/GetRegiondropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.regionnamelist = this.responsedata.dropdown_list1;
    });
    var api = 'CampaignSummary/GetIndustrydropdown'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.industry_list = this.responsedata.dropdown_list1;
    });
    this.GetAssignSummary();
    this.GetUserSummary();

    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });


  }
  get schedule_remarks() {
    return this.reactiveFormSubmit.get('schedule_remarks')!;
  }
  // get leadbank_name() {
  //   return this.reactiveFormSubmit.get('leadbank_name')!;
  // }
  GetUserSummary(){
    let param={
      employee_gid: this.employee_gid,
      campaign_gid: this.campaign_gid,
    }
    var api ='CampaignSummary/GetUserSummary'
    this.service.post(api, param).subscribe((result: any) => {
      $('#user_list').DataTable().destroy();
      this.responsedata = result;
      this.user_list = this.responsedata.marketingteam_list;
      console.log(this.user_list)
      setTimeout(() => {
        $('#user_list').DataTable();
      }, 1);
    });
  }

  
  GetAssignSummary() {
    var api = 'CampaignSummary/GetAssignSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#assign_list').DataTable().destroy();
      this.responsedata = result;
      this.assign_list = this.responsedata.marketingteam_list;
      console.log(this.assign_list)
      setTimeout(() => {
        $('#assign_list').DataTable();
      }, 1);
    });
  }


  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.assign_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.assign_list.forEach((row: IAssignlead) => this.selection.select(row));
  }
  OnSubmit(): void {

    this.selection.selected.forEach(s => s.leadbank_gid);
    this.selection.selected.forEach(s => s.employee_gid);
    this.selection.selected.forEach(s => s.campaign_gid);
    this.selection.selected.forEach(s => s.schedule_remarks);
    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.schedule_remarks = this.reactiveFormSubmit.value.schedule_remarks;
    this.CurObj.employee_gid = this.employee_gid;
    this.CurObj.campaign_gid = this.campaign_gid;
    this.CurObj.summary_list1 = list
    console.log(this.CurObj)
    if (this.CurObj.summary_list1.length != 0) {
      this.isAnyCheckboxSelected = true;
      var url1 = 'CampaignSummary/GetAssignLead'
      this.service.post(url1,this.CurObj).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
        }
        else {
          this.ToastrService.success(result.message)
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.route.navigate(['/crm/CrmMstCampaignsummary']);
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record and Executive  ! ")
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      
    }
  }

  redirecttolist(){
    this.route.navigate(['/crm/CrmMstCampaignsummary']);
  }


  popmodal(parameter: string, parameter1: string) {
    this.parameterValue1 = parameter;
    this.remarks = this.parameterValue1;
    this.leadbank_name = parameter1;
}

  // Inside your component class

// Inside your component class
onCheckboxChange(event: any, data: any): void {
  // Update the selection state
  this.selection.toggle(data);

  // Check if at least one checkbox is selected
  this.isAnyCheckboxSelected = this.selection.hasValue();
}

GetSortedLeads(){
  const selectedRegion = this.SearchEventForm.value.region_name || 'null'
  const selectedSource = this.SearchEventForm.value.source_name || 'null'
  const selectedCustomertype = this.SearchEventForm.value.customer_type || 'null';
  const param = {
    region_name: selectedRegion,
    source: selectedSource,
    customer_type: selectedCustomertype,
  };
  var api = 'CampaignSummary/GetsearchAssignSummary'
  this.service.getparams(api, param).subscribe((result: any) => {
    $('#assign_list').DataTable().destroy();
    this.responsedata = result;
    this.assign_list = this.responsedata.marketingteam_list;
    console.log(this.assign_list)
    setTimeout(() => {
      $('#assign_list').DataTable();
    }, 1);
  });
}
 
}





