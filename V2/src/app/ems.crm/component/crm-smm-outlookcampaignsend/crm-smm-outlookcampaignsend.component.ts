import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES ,enc} from 'crypto-js';
import { DomSanitizer, SafeResourceUrl, SafeUrl, SafeHtml } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Table } from 'primeng/table';
import { SelectionModel } from '@angular/cdk/collections';
export interface Customer {
  leadbank_gid?: any;
  template_gid?: any | null;
  email?: any | null;
  created_date?: any | null;
  default_phone?: any | null;
  customer_type?: any | null;
  names?: any | null;
  address1?: any | null;
  date?: any | null;
  to_mail?: any | null;
  status_delivery?: any | null;
  status_open?: any | null;
  status_click?: any | null;
  address2?: any | null;
  city?: any | null;
  state?: any | null;
  sub?: any | null;
  to?: any | null;
  body?: any | null;
  direction?: any | null;
  source_gid?: any | null;
  source_name?: any | null;
  lead_status?: any | null;
  message?: any | null;
  status?: boolean | null;
}
export interface PageEvent {
  first: number;
  rows: number;
  page: number;
  pageCount: number;
}
export class IAssign {
  outlookmailsendchecklist: any[] = [];
  mailmanagement_gid: string = "";
  template_gid: string = "";
  leadbank_gid: string = "";

}
@Component({
  selector: 'app-crm-smm-outlookcampaignsend',
  templateUrl: './crm-smm-outlookcampaignsend.component.html',
  styleUrls: ['./crm-smm-outlookcampaignsend.component.scss']
})
export class CrmSmmOutlookcampaignsendComponent {
  responsedata: any;
  outlooktemplatesummary_list: any;
  template_name: any;
  template_gid: any;
  template_body: any;
  template_subject: any;
  created_date: any;
  body: any;
  viewtemplate_name: any;
  viewbody: any;
  viewtemplate_subject: any;
  viewcreated_date: any;
  viewtemplate_body: any;
  mailtemplatesendsummary_list: any;
  selectedCustomer: Customer[] = [];
  CurObj: IAssign = new IAssign();
  selection = new SelectionModel<IAssign>(true, []);
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private sanitizer: DomSanitizer, private route: Router) {
    this.isRowSelectable = this.isRowSelectable.bind(this);
  }
  ngOnInit(): void {
    this.GetTemplateView();
    this.GetMailTemplateSendSummary();
    const template_gid = this.router.snapshot.paramMap.get('template_gid');
    const template_name = this.router.snapshot.paramMap.get('template_name');
    this.template_gid = template_gid;
    this.template_name = template_name;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_gid, secretKey).toString(enc.Utf8);
    const deencryptedParams = AES.decrypt(this.template_name, secretKey).toString(enc.Utf8);
    this.template_gid = deencryptedParam;
    this.template_name = deencryptedParams;
  }

GetTemplateView(){
  let param = {
    template_gid: this.template_gid
  }
  var api = 'OutlookCampaign/SendTemplatePreview';
  this.service.getparams(api,param).subscribe((result: any) => {
    this.responsedata = result;
    this.outlooktemplatesummary_list = this.responsedata.outlooktemplatesummary_list;
  this.viewtemplate_name = this.outlooktemplatesummary_list[0].template_name;
  this.viewbody = this.outlooktemplatesummary_list[0].template_body;
  this.viewtemplate_subject = this.outlooktemplatesummary_list[0].template_subject;
  this.viewcreated_date = this.outlooktemplatesummary_list[0].created_date;
  const unsafeHtml = this.outlooktemplatesummary_list[0].template_body; 
  this.viewtemplate_body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
  });
}

onGlobalFilterChange1(event: Event, dt1: Table): void {
  const inputValue = (event.target as HTMLInputElement).value;
  dt1.filterGlobal(inputValue, 'contains');

}

isRowSelectable(event: any) {
  return !this.isMailidnull(event.data);
}
isMailidnull(customer: any) {
  return customer.email === '' || customer.email === null;
}
GetMailTemplateSendSummary(): void {

  var api = 'GmailCampaign/GmailTemplateSendSummary';
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.mailtemplatesendsummary_list = this.responsedata.gmailtemplatesendsummary_list;
 
  });
}
public onsend(): void {
  this.CurObj.template_gid = this.template_gid;
  this.CurObj.outlookmailsendchecklist = this.selectedCustomer;
  if (this.CurObj.outlookmailsendchecklist.length != 0) {
    var url = 'OutlookCampaign/SendOutlookTemplate'
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetTemplateSummary();
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        this.route.navigate(['/crm/CrmSmmOutlookcampaignsummary'])

      }

    });

  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }

}
onback(){
  this.route.navigate(['/crm/CrmSmmOutlookcampaignsummary']);
}
GetTemplateSummary() {
  var api3 = 'OutlookCampaign/OutlookTemplateSummary'
  this.service.get(api3).subscribe((result: any) => {
    $('#outlooktemplatesummary_list').DataTable().destroy();
    this.responsedata = result;
    this.outlooktemplatesummary_list = this.responsedata.outlooktemplatesummary_list;
    setTimeout(() => {
      $('#outlooktemplatesummary_list').DataTable();
    }, 1);
  });

}
}
