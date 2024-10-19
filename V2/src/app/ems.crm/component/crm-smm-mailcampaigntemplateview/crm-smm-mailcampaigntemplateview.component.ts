import { Component, AfterViewInit, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { DomSanitizer, SafeResourceUrl, SafeUrl,SafeHtml  } from '@angular/platform-browser';

@Component({
  selector: 'app-crm-smm-mailcampaigntemplateview',
  templateUrl: './crm-smm-mailcampaigntemplateview.component.html',
  styleUrls: ['./crm-smm-mailcampaigntemplateview.component.scss']
})
export class CrmSmmMailcampaigntemplateviewComponent {
  mailtemplateview_list: any;
  mail_from: any;
  sub: any;
  body: any;
  created_date: any;
  temp_mail_gid:any;
  template_name: any;
  

  sanitizedHtml: SafeHtml | undefined;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService,private sanitizer: DomSanitizer) { }


  ngOnInit(): void {
  
    const temp_mail_gid = this.route.snapshot.paramMap.get('temp_mail_gid');
    this.temp_mail_gid = temp_mail_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.temp_mail_gid, secretKey).toString(enc.Utf8);
    //console.log(deencryptedParam)
    this.GetMailView(deencryptedParam);  
  }
  GetMailView(temp_mail_gid:any) {
    this.NgxSpinnerService.show();
    var url = 'MailCampaign/MailTemplateView';
    let param = {
      temp_mail_gid : temp_mail_gid 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      this.mailtemplateview_list = result.mailtemplate_list;
      this.mail_from = this.mailtemplateview_list[0].mail_from;
      this.template_name = this.mailtemplateview_list[0].template_name
      //this.body = this.mailtemplateview_list[0].body;
      const unsafeHtml = this.mailtemplateview_list[0].body;
      this.body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
      
      this.sub = this.mailtemplateview_list[0].sub;
      this.created_date = this.mailtemplateview_list[0].created_date;
      this.NgxSpinnerService.hide();
    });
}


onback()
{
  this.router.navigate(['/crm/CrmSmmMailcampaignsummary']);
}



}
