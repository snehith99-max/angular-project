import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';

import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

interface ILeadmaster {

  leadbank_gid: string;
  leadbank_name: string;
  contact_details: string;
  region_city_state: string;
  source: string;
  remarks: string;
  status: string;
  assigned_to: string;
}
@Component({
  selector: 'app-crm-trn-leadmastersummary',
  templateUrl: './crm-trn-leadmastersummary.component.html',
  styleUrls: ['./crm-trn-leadmastersummary.component.scss'],
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
export class CrmTrnLeadmastersummaryComponent {

  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  remarks: any;
  leadbank_name: any;
  leadmaster_list: any[] = [];
  GetRegisterLeadSummary_list2: any[] = [];
  GetRegisterLeadSummary_list1: any[] = [];
  CustomerTypeCountList: any[] = [];
  RegisterLeadCountList: any[] = [];
  firstCustomertype :any;
  firstCustomertype2 :any;
  firstCustomertype3 :any;
  customertype_list : any[] = [];
  showOptionsDivId: any;
  formData: any = {};
  isSubmitting = false;
  leadmaster!: ILeadmaster;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.leadmaster = {} as ILeadmaster;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    //this.GetRegisterLeadSummary();
    this.GetRegisterLeadSummary1();
    //this.GetRegisterLeadSummary2();
    this.GetCustomerTypeSummary();
    // Form values for Add popup/////

    this.reactiveForm = new FormGroup({
      leadbank_gid: new FormControl(this.leadmaster.leadbank_gid, [
        Validators.required,
      ]),
      leadbank_name: new FormControl(this.leadmaster.leadbank_name, [
        Validators.required,
      ]),
    });

    var api = 'registerlead/GetRegisterLeadCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.RegisterLeadCountList = this.responsedata.RegisterLeadCount_List;
    });
    var api = 'registerlead/GetCustomerTypeCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.CustomerTypeCountList = this.responsedata.RegisterLeadCount_List;
    });
  }

  GetRegisterLeadSummary1() {
    this.NgxSpinnerService.show();
    var api3 = 'registerlead/GetRegisterLeadSummary1'
    this.service.get(api3).subscribe((result: any) => {
      $('#GetRegisterLeadSummary_list1').DataTable().destroy();
      this.responsedata = result;
      this.GetRegisterLeadSummary_list1 = this.responsedata.GetRegisterLeadSummary_list1;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      // setTimeout(() => {
      //   $('#GetRegisterLeadSummary_list1').DataTable();
      // }, 1);
    });
  }
  GetRegisterLeadSummary2() {
    this.NgxSpinnerService.show();
    var api3 = 'registerlead/GetRegisterLeadSummary2'
    this.service.get(api3).subscribe((result: any) => {
      $('#GetRegisterLeadSummary_list2').DataTable().destroy();
      this.responsedata = result;
      this.GetRegisterLeadSummary_list2 = this.responsedata.GetRegisterLeadSummary_list2;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      // setTimeout(() => {
      //   $('#GetRegisterLeadSummary_list2').DataTable();
      // }, 1);
    });
  }
  toggleOptions(leadbank_gid: any) {
    if (this.showOptionsDivId === leadbank_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = leadbank_gid;
    }
  }
  GetRegisterLeadSummary() {
    this.NgxSpinnerService.show();
    var api3 = 'registerlead/GetRegisterLeadSummary'
    this.service.get(api3).subscribe((result: any) => {
      $('#leadmaster_list').DataTable().destroy();
      this.responsedata = result;
      this.leadmaster_list = this.responsedata.GetRegisterLeadSummary_list;
      this.NgxSpinnerService.hide();
      //console.log(this.source_list)
      // setTimeout(() => {
      //   $('#leadmaster_list').DataTable();
      // }, 1);
    });
  }
  GetCustomerTypeSummary() {
    var api = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = this.responsedata.customertype_list1;    
      this.firstCustomertype = this.customertype_list[0].customer_type1;
      this.firstCustomertype2 = this.customertype_list[1].customer_type1;
      this.firstCustomertype3 = this.customertype_list[2].customer_type1;  
    });
  }
  //Delete //
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    this.NgxSpinnerService.show();
    console.log(this.parameterValue);
    var url = 'registerlead/deleteregisterleadSummary'
    let param = {
      leadbank_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
       
        this.ToastrService.warning(result.message)
        setTimeout(() => {
          this.NgxSpinnerService.hide();
          window.location.reload();
        }, 2000);
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        window.location.reload()
        this.ToastrService.success(result.message)
      }
      this.GetRegisterLeadSummary();
      setTimeout(() => {
        this.NgxSpinnerService.hide();
        window.location.reload();
      }, 2000);
    });

  }
  //delete 1//
  onadd() {
    const secretKey = 'storyboarderp';
    const lspage1 = 'LeadBankdistributor';
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnLeadbankadd', lspage]);
  }

  onview(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = "Registerleaddistributor";
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    console.log(encryptedParam);
    this.NgxSpinnerService.hide();
    this.route.navigate(['/crm/CrmTrnLeadbankview', encryptedParam, lspage])
  }
  onedit(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const lspage1 = "Registerleaddistributor";
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    this.NgxSpinnerService.hide();
    this.route.navigate(['/crm/CrmTrnLeadbankedit', encryptedParam, lspage])
  }
  popmodal(parameter: string, parameter1: string) {

    this.remarks = parameter;
    this.leadbank_name = parameter1;
  }


}