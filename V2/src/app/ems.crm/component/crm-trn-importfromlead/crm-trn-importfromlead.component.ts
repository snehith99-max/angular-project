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
import { Table } from 'primeng/table';


export class IContactList {
  contacts_list: any[] = [];
  project_id: string = "";
  sendtext: string = "";
}
interface ICampaign {
  schedule_remarks: string;
  company_name: string;
  source_name: string;
  leadbank_name: string;
  mobile:string;
  customer_type:string;
  region_name:string;


}
export class IAssignlead {
  summary_list1: string[] = [];
  leadbank_gid: string = "";
  schedule_remarks: string = "";
  employee_gid: string = "";
  campaign_gid: string = "";
}
interface ILeadbank{
  region_name: string;
  source_name: string;
  customer_type: string;

}
export interface Customer {

  whatsapp_gid?: any;
  id?: any | null;
  wkey?: any | null;
  mobile?: any | null;
  leadbank_name?: any | null;
  first_letter?: any | null;
  firstName?: any | null;
  lastName?: any | null;
  gender?: any | null;
  created_date?: any | null;
  created_by?: any | null;
  read_flag?: any | null;
  last_seen?: any | null;
  customer_type?: any | null;
  source_name?: any | null;
  lead_status?: any | null;
  sendcampaign_flag?: any | null;

}
@Component({
  selector: 'app-crm-trn-importfromlead',
  templateUrl: './crm-trn-importfromlead.component.html',
  styleUrls: ['./crm-trn-importfromlead.component.scss']
})
export class CrmTrnImportfromleadComponent {
  reactiveFormSubmit!: FormGroup;
  campaign!: ICampaign;
  Contactlist: any[] = [];
  source_list: any[] = [];
  region_list: any[] = [];
  industry_list: any[] = [];
  assign_list: any[] = [];
  user_list: any[] = [];
  summary_list1: any[] = [];
  selection = new SelectionModel<IContactList>(true, []);
  responsedata: any;
  IAssignlead: any;
  CurObj: IContactList = new IContactList();
  pick: Array<any> = [];
  campaign_gid: any;
  employee_gid: any;
  leadbank_gid: any;
  mobile: any;
  parameterValue1: any;
  parameterValue2: any;
  campaign_name: any;
  contacts_list: any[] = [];
  bulkMessageForm: FormGroup | any;
  remarks: any;
  leadbank_name: any;
  user_name: any;
  isButtonDisabled = false;
  isAnyCheckboxSelected = false;
  leadbank!: ILeadbank;
  regionnamelist: any[] = [];
  customertype_list: any[] = [];
  customer!: Customer;
  selectedCustomer: Customer[] = [];
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
    this.campaign = {} as ICampaign;
    this.leadbank = {} as ILeadbank;

  }
  ngOnInit(): void {

    this.reactiveFormSubmit = new FormGroup({
      leadbank_name: new FormControl(this.campaign.leadbank_name, [
      ]),
      source_name: new FormControl(this.campaign.source_name, [
      ]),
      region_name: new FormControl(this.campaign.region_name, [
      ]),
      customer_type: new FormControl(this.campaign.customer_type, [
      ]),   mobile: new FormControl(this.campaign.mobile, [
      ]),
  
    });
    this.bulkMessageForm = new FormGroup({
      cboTemplate: new FormControl(null, Validators.required),
      id: new FormControl(),
      region_name: new FormControl(this.leadbank.region_name, [   
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      source_name: new FormControl(this.leadbank.source_name, [
       
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),
      customer_type: new FormControl(this.leadbank.customer_type, [
        
      ]),
    });

    //this.GetAssignSummary();
    this.GetUserSummary();
 
    var api1 = 'Leadbank/Getsourcedropdown'
    this.service.get(api1).subscribe((result: any) => {
      this.responsedata = result;
      this.source_list = result.source_list;
      //console.log(this.source_list)
    });

    var api2 = 'Leadbank/Getregiondropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.regionnamelist = result.regionname_list;
    });

    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });
  }
 
  GetUserSummary() {
    debugger
    var api = 'Whatsapp/GetLeadContact'
    this.service.get(api).subscribe((result: any) => {
      $('#user_list').DataTable().destroy();
      this.responsedata = result;
      this.Contactlist = this.responsedata.whatsleadlist;
      // console.log(this.user_list)
      setTimeout(() => {
        $('#user_list').DataTable();
      }, 1);
    });
  }




  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.contacts_list.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.contacts_list.forEach((row: IContactList) => this.selection.select(row));
  }
  OnSubmit(): void {
debugger
this.CurObj.contacts_list =  this.selectedCustomer
if (this.CurObj.contacts_list.length == 0) {
  this.ToastrService.warning("Kindly select atleast 1 Lead")
  return;
}
this.NgxSpinnerService.show();

      var url1 = 'Whatsapp/PostImportExcel'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {
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
        }
        this.NgxSpinnerService.hide();
        this.selectedCustomer = [];
      });
  }

  redirecttolist() {
    this.route.navigate(['/crm/CrmMstCampaignsummary']);
  }


  onGlobalFilterChange(event: Event, dt: Table): void {
    const inputValue = (event.target as HTMLInputElement).value;
    dt.filterGlobal(inputValue, 'contains');
  }

  // Inside your component class

  // Inside your component class
  onCheckboxChange(event: any, data: any): void {
    // Update the selection state
    this.selection.toggle(data);

    // Check if at least one checkbox is selected
    this.isAnyCheckboxSelected = this.selection.hasValue();

  }
  GetPaymentSummary(){
    const selectedRegion = this.bulkMessageForm.value.region_name || 'null'
    const selectedSource = this.bulkMessageForm.value.source_name || 'null'
    const selectedCustomertype = this.bulkMessageForm.value.customer_type || 'null';

    const params = {
      region_name: selectedRegion,
      source_name: selectedSource,
      customer_type: selectedCustomertype,

    };
    this.NgxSpinnerService.show();
    const url = 'Whatsapp/Getimportcontact';

    this.service.getparams(url, params).subscribe((result) => {
      this.responsedata = result;
      this.Contactlist = this.responsedata.whatsleadlist;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Contactlist').DataTable();
      },1);
    });
  }
}
