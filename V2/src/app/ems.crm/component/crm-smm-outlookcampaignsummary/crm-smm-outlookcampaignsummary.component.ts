import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl, SafeUrl, SafeHtml } from '@angular/platform-browser';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-crm-smm-outlookcampaignsummary',
  templateUrl: './crm-smm-outlookcampaignsummary.component.html',
  styleUrls: ['./crm-smm-outlookcampaignsummary.component.scss'],
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
export class CrmSmmOutlookcampaignsummaryComponent {
  responsedata: any;
  outlooktemplatesummary_list: any;
  isButtonTrue: boolean = true;
  isButtonFalse: boolean = false;
  parametervalue: any;
  template_name: any;
  body: any;
  created_date: any;
  template_subject: any;
  template_body: any;
  showOptionsDivId: any;
  windowInterval: any;
  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer) {
  }
  ngOnInit(): void {
    this.windowInterval = window.setInterval(() => {
      this.GetTemplateSummary();
    }, 3000);
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetTemplateSummary() {
    var api3 = 'OutlookCampaign/OutlookTemplateSummary'
    this.service.get(api3).subscribe((result: any) => {
      $('#outlooktemplatesummary_list').DataTable().destroy();
      this.responsedata = result;
      this.outlooktemplatesummary_list = this.responsedata.outlooktemplatesummary_list;
    });

  }
  onadd(){
    this.route.navigate(['/crm/CrmSmmOutlookcampaigntemplate']);
  }
  onopen(){
      this.route.navigate(['/crm/CrmSmmOutlookinbox']);
  }
  toggleswitch(param: any){
    var api = 'OutlookCampaign/PostCampaignStatus'
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.GetTemplateSummary();
      }
      else {
        this.ToastrService.success(result.message);
        this.GetTemplateSummary();
      }
    });
  }
  onsend(param: any){
    const secretKey = 'storyboarderp';
    const template_gid = AES.encrypt(param.template_gid, secretKey).toString();
    const template_name = AES.encrypt(param.template_name, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmOutlookcampaignsend', template_gid,template_name])
  }
  GetTemplateView(parameter: any){
    this.parametervalue = parameter
    this.template_name = this.parametervalue.template_name
    this.body = this.parametervalue.template_body;
    this.template_subject = this.parametervalue.template_subject;
    this.created_date = this.parametervalue.created_date;
    const unsafeHtml = this.parametervalue.template_body;
    this.template_body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
  }
  sentstatus(param: any){
    const secretKey = 'storyboarderp';
    const template_gid = AES.encrypt(param.template_gid, secretKey).toString();
    const template_name = AES.encrypt(param.template_name, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmOutlookcampaignsentsummary', template_gid,template_name])
  }
  toggleOptions(template_gid: any) {
    if (this.showOptionsDivId === template_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = template_gid;
    }
  }
  ngOnDestroy(): void {
    clearInterval(this.windowInterval)
  }
}
