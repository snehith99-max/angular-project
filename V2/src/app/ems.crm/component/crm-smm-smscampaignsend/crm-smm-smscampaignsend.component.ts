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
  customerdetailslist: any[] = [];
  template_id: any;
  leadbank_gid: string = "";
}
export interface Customer {
  leadbank_gid?: any;
  template_id?: any | null;
  names?: any;
  email?: any | null;
  customer_type?: any | null;
  address1?: any | null;
  default_phone?: any | null;

}
@Component({
  selector: 'app-crm-smm-smscampaignsend',
  templateUrl: './crm-smm-smscampaignsend.component.html',
  styleUrls: ['./crm-smm-smscampaignsend.component.scss']
})
export class CrmSmmSmscampaignsendComponent {
  selection = new SelectionModel<IContactList>(true, []);
  reactiveForm!: FormGroup;
  CurObj: IContactList = new IContactList();
  responsedata: any;
  pick: Array<any> = [];
  // customerdetailslist: any;
  customerdetailslist: any[] = [];

  template_id: any;
  selectedCustomer: Customer[] = [];
  customer!: Customer;
  campagin_title: any;
  campagin_message: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
  }
  ngOnInit(): void {
    debugger
    const template_id = this.router.snapshot.paramMap.get('template_id');
    this.template_id = template_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_id, secretKey).toString(enc.Utf8);
    this.template_id = deencryptedParam;
    this.GetSmsCampaignSummary();
  }
  templateview() {
   
    let params = {
      template_id: this.template_id
    }
    var url = 'SmsCampaign/smstemplatepreview'
    this.service.getparams(url, params).subscribe((result: any) => {
      this.campagin_title = result.template_previewlist[0].campagin_title;
      this.campagin_message = result.template_previewlist[0].campagin_message;

    });
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.customerdetailslist.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.customerdetailslist.forEach((row: IContactList) => this.selection.select(row));
  }
  onGlobalFilterChange(event: Event, dt: Table): void {
    const inputValue = (event.target as HTMLInputElement).value;
    dt.filterGlobal(inputValue, 'contains');
  }
  GetSmsCampaignSummary() {
    this.NgxSpinnerService.show();
    var api = 'SmsCampaign/SmsLeadCustomerDetails';
    this.service.get(api).subscribe((result: any) => {
      // $('#mailtemplatesendsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.customerdetailslist = this.responsedata.smsleadcustomerdetails_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#customerdetailslist').DataTable();
      }, 1);
    });

  }
  onsend(): void {
    this.CurObj.customerdetailslist = this.selectedCustomer
    this.CurObj.template_id = this.template_id
    if (this.CurObj.template_id == undefined) {
      this.ToastrService.warning("Kindly select a template to send")
      return;
    }
    if (this.CurObj.customerdetailslist.length == 0) {
      this.ToastrService.warning("Kindly select atleast 1 contact")
      return;
    }
    this.NgxSpinnerService.show();
    var url = 'SmsCampaign/smstemplatesendsummarylist'
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.route.navigate(['/crm/CrmSmmSmscampaign']);

      }
      this.NgxSpinnerService.hide();
      this.selectedCustomer = [];
    });
  }

  onback() {
    this.route.navigate(['/crm/CrmSmmSmscampaign']);

  }
}
