import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';


import { DomSanitizer, SafeResourceUrl, SafeUrl, SafeHtml } from '@angular/platform-browser';
export class IAssignlead {
  mailtemplate_list: string[] = [];
  temp_mail_gid: string = "";
}

@Component({
  selector: 'app-crm-smm-mailcampaignsummary',
  templateUrl: './crm-smm-mailcampaignsummary.component.html',
  styleUrls: ['./crm-smm-mailcampaignsummary.component.scss']
})
export class CrmSmmMailcampaignsummaryComponent {
  reactiveForm!: FormGroup;
  CurObj: IAssignlead = new IAssignlead();
  selection = new SelectionModel<IAssignlead>(true, []);
  pick: Array<any> = [];
  responsedata: any;
  formData: any = {};
  mailtemplate_list: any;
  temp_mail_gid: any;
  mailevent_list: any;
  mailcount_list: any[] = [];
  clicktotal_count: any;
  deliverytotal_count: any;
  opentotal_count: any;
  mailsendtotal_count: any;
  template_count: any;
  isButtonTrue: boolean = true;
  isButtonFalse: boolean = false;
  mailtemplateview_list: any;
  mail_from: any;
  sub: any;
  showOptionsDivId: any;
  body: any;
  created_date: any;
  template_name: any;
  sanitizedHtml: SafeHtml | undefined;
  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer) {

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetTemplateSummary();
    this.GetMailEventCount();
    this.GetMailEventClick();
    this.GetMailEventDelivery();
    this.GetMailEventOpen();

  }


  GetTemplateSummary() {
    this.NgxSpinnerService.show();
    var api3 = 'MailCampaign/TemplateSummary'
    this.service.get(api3).subscribe((result: any) => {
      $('#mailtemplate_list').DataTable().destroy();
      this.responsedata = result;
      this.mailtemplate_list = this.responsedata.mailtemplate_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#mailtemplate_list').DataTable();
      }, 1);
    });

  }
  public toggleswitch(param: any): void {
    var api = 'MailCampaign/UpdateMailTemplateStatus'
    this.service.post(api, param).subscribe((result: any) => {
      if (result.status == false) {
        this.GetTemplateSummary();
      }
      else {
        this.GetTemplateSummary();
      }
    });
  }
  GetMailEventCount() {
    var api2 = 'MailCampaign/GetMailEventCount'
    this.service.get(api2).subscribe((result: any) => {
      this.mailcount_list = result.mailcount_list;
      this.clicktotal_count = this.mailcount_list[0].clicktotal_count;
      this.opentotal_count = this.mailcount_list[0].opentotal_count;
      this.deliverytotal_count = this.mailcount_list[0].deliverytotal_count;
      this.mailsendtotal_count = this.mailcount_list[0].mailsendtotal_count;
      this.template_count = this.mailcount_list[0].template_count;
    });
  }
  toggleOptions(temp_mail_gid: any) {
    if (this.showOptionsDivId === temp_mail_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = temp_mail_gid;
    }
  }
  GetMailEventOpen() {
    var api = 'MailCampaign/GetMailEventOpen'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.mailevent_list = this.responsedata.mailevent_list;
    });
  }
  GetMailEventClick() {
    var api = 'MailCampaign/GetMailEventClick'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.mailevent_list = this.responsedata.mailevent_list;
    });
  }
  GetMailEventDelivery() {
    var api = 'MailCampaign/GetMailEventDelivery'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.mailevent_list = this.responsedata.mailevent_list;
    });
  }

  public onsend(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();

    this.route.navigate(['/crm/CrmSmmMailcampaignsend', temp_mail_gid])

  }


  onadd() {
    this.route.navigate(['/crm/CrmSmmMailcampaigntemplate']);
  }

  onopen() {
    this.route.navigate(['/crm/CrmSmmEmailmanagement']);
  }

  public ontemplateview(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmMailcampaigntemplateview', temp_mail_gid])
  }

  public mailstatus(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const temp_mail_gid = AES.encrypt(params.temp_mail_gid, secretKey).toString();

    this.route.navigate(['/crm/CrmSmmMailcampaignsendstatus', temp_mail_gid])
  }

  GetMailView(temp_mail_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'MailCampaign/MailTemplateView';
    let param = {
      temp_mail_gid: temp_mail_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.mailtemplateview_list = result.mailtemplate_list;
      this.mail_from = this.mailtemplateview_list[0].mail_from;
      this.template_name = this.mailtemplateview_list[0].template_name
      const unsafeHtml = this.mailtemplateview_list[0].body;
      this.body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
      this.sub = this.mailtemplateview_list[0].sub;
      this.created_date = this.mailtemplateview_list[0].created_date;
      this.NgxSpinnerService.hide();
    });
  }


}