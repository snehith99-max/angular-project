import { Component, ViewChild } from '@angular/core';
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

import { DomSanitizer, SafeResourceUrl, SafeUrl,SafeHtml  } from '@angular/platform-browser';
export class IAssign {
  gmailsendchecklist: any[] = [];
  mailmanagement_gid: string = "";
  template_gid: string = "";
  leadbank_gid: string = "";

}
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
  template_name?: any | null;

  message?: any | null;
  status?: boolean | null;

}
export interface PageEvent {
  first: number;
  rows: number;
  page: number;
  pageCount: number;
}
@Component({
  selector: 'app-crm-smm-gmailcampaignsend',
  templateUrl: './crm-smm-gmailcampaignsend.component.html',
  styleUrls: ['./crm-smm-gmailcampaignsend.component.scss']
})
export class CrmSmmGmailcampaignsendComponent {
 
  mailtemplate_list: any;
  responsedata: any;
  mailtemplatesendsummary_list: any;
  template_gid: any;
  CurObj: IAssign = new IAssign();
  selection = new SelectionModel<IAssign>(true, []);
  pick: Array<any> = [];
  mailsummarylist: any[] = [];
  reactiveForm: any;
  template_name: any;
  mailtemplateview_list: any;
  mail_from: any;
  sub: any;
  body: any;
  created_date: any;
  filteredData: IAssign[] = [];
  
  sanitizedHtml: SafeHtml | undefined;
  //////////////demo////////////////////
  customer!: Customer;
  selectedCustomer: Customer[] = [];
  sentflag_y: any[] = [];
  sentflag_n: any[] = [];
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService,private sanitizer: DomSanitizer) {
    this.isRowSelectable = this.isRowSelectable.bind(this);
  }
  ngOnInit(): void {
    // this.GetRefreshAccessTockenGenerate();
    debugger
    const template_gid = this.router.snapshot.paramMap.get('template_gid');
    this.template_gid = template_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_gid, secretKey).toString(enc.Utf8);
    this.template_gid = deencryptedParam;
    this.GetMailTemplateSendSummary();
    //this.GetMailView(deencryptedParam);
  }

  // GetRefreshAccessTockenGenerate(): void {
  //   debugger
  //   var api = 'GmailCampaign/DaRefreshAccessTockenGenerate';
  //   this.service.get(api).subscribe((result: any) => {
  //   });
  // }

  //   onPageChange(event: PageEvent) {
  //     this.first = event.first;
  //     this.rows = event.rows;
  // }


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
      this.template_name = this.mailtemplatesendsummary_list[0].template_name;
      console.log(this.template_name)
      this.NgxSpinnerService.hide();
   
    });
  }

  public onsend(): void {
    this.CurObj.template_gid = this.template_gid;
    this.CurObj.gmailsendchecklist = this.selectedCustomer;
    // console.log(this.CurObj)
    if (this.CurObj.gmailsendchecklist.length != 0) {
      var url = 'GmailCampaign/SendGmailTemplate'
      this.service.post(url, this.CurObj).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetMailTemplateSendSummary();
          this.reactiveForm.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.route.navigate(['/crm/CrmSmmGmailcampaignsummary'])
          this.reactiveForm.reset();

        }
        this.GetMailTemplateSendSummary();
        this.reactiveForm.reset();

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }

  onback() {
    this.route.navigate(['/crm/CrmSmmGmailcampaignsummary']);

  }


  GetMailView() {
    this.NgxSpinnerService.show();
    debugger
    var url = 'GmailCampaign/GmailTemplateView';
    let param = {
      template_gid: this.template_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.mailtemplateview_list = result.gmailtemplate_list;
      // this.mail_from = this.mailtemplateview_list[0].mail_from;
      this.template_name = this.mailtemplateview_list[0].template_name;
      const unsafeHtml = this.mailtemplateview_list[0].template_body;
      this.body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
      this.sub = this.mailtemplateview_list[0].template_subject;
      this.created_date = this.mailtemplateview_list[0].created_date;
      this.NgxSpinnerService.hide();
    });
  }

}
