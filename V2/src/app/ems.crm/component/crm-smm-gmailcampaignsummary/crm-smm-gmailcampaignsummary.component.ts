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
  template_gid: string = "";
}
@Component({
  selector: 'app-crm-smm-gmailcampaignsummary',
  templateUrl: './crm-smm-gmailcampaignsummary.component.html',
  styleUrls: ['./crm-smm-gmailcampaignsummary.component.scss']
})
export class CrmSmmGmailcampaignsummaryComponent {

  reactiveForm!: FormGroup;
  CurObj: IAssignlead = new IAssignlead();
  selection = new SelectionModel<IAssignlead>(true, []);
  pick: Array<any> = [];
  responsedata: any;
  formData: any = {};
  mailtemplate_list: any;
  template_gid: any;
  mailevent_list: any;
  mailcount_list: any[] = [];
  clicktotal_count: any;
  deliverytotal_count: any;
  showOptionsDivId: any;
  opentotal_count: any;
  mailsendtotal_count: any;
  template_count: any;
  isButtonTrue: boolean = true;
  isButtonFalse: boolean = false;
  mailtemplateview_list: any;
  mail_from: any;
  template_subject: any;
  template_body: any;

  created_date: any;
  template_name: any;
  sanitizedHtml: SafeHtml | undefined;
  body:any;
  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer) {

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetTemplateSummary();


  }


  GetTemplateSummary() {
    this.NgxSpinnerService.show();
    var api3 = 'GmailCampaign/GmailTemplateSummary'
    this.service.get(api3).subscribe((result: any) => {
      $('#mailtemplate_list').DataTable().destroy();
      this.responsedata = result;
      this.mailtemplate_list = this.responsedata.gmailtemplate_list;
      //console.log(this.mailtemplate_list)
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#mailtemplate_list').DataTable();
      }, 1);
    });

  }
  toggleOptions(template_gid: any) {
    if (this.showOptionsDivId === template_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = template_gid;
    }
  }
  public toggleswitch(param: any): void {
    var api = 'GmailCampaign/UpdateGmailTemplateStatus'
    this.service.post(api, param).subscribe((result: any) => {
      if (result.status == false) {
        this.GetTemplateSummary();
      }
      else {
        this.GetTemplateSummary();
      }
    });
   }
  public onsend(params: any): void {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const template_gid = AES.encrypt(params.template_gid, secretKey).toString();

    this.route.navigate(['/crm/CrmSmmGmailcampaignsend', template_gid])

  }


  onadd() {
    this.route.navigate(['/crm/CrmSmmGmailcampaigntemplate']);
  }

  onopen() {
    // this.route.navigate(['/crm/CrmSmmGmailinbox']);
    this.route.navigate(['/crm/CrmSmmMailscompose']);

  }



  public mailstatus(params: any): void {
    const secretKey = 'storyboarderp';
    debugger
    //console.log(params)
    const template_gid = AES.encrypt(params.template_gid, secretKey).toString();

    this.route.navigate(['/crm/CrmSmmGmailcampaignsendstatus', template_gid])
  }

  GetMailView(template_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'GmailCampaign/GmailTemplateView'
    let param = {
      template_gid: template_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.mailtemplateview_list = result.gmailtemplate_list;
      //console.log(this.mailtemplateview_list)
      this.mail_from = this.mailtemplateview_list[0].mail_from;
      this.template_name = this.mailtemplateview_list[0].template_name
      this.body = this.mailtemplateview_list[0].template_body;
      //console.log(this.body)
      this.template_subject = this.mailtemplateview_list[0].template_subject;
      this.created_date = this.mailtemplateview_list[0].created_date;
      const unsafeHtml = this.mailtemplateview_list[0].template_body;
      this.template_body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
      this.NgxSpinnerService.hide();
    });
  }

}
