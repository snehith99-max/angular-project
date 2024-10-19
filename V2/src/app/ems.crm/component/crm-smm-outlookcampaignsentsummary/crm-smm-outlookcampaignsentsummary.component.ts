import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute } from '@angular/router';
import { AES ,enc} from 'crypto-js';
import { Router } from '@angular/router';
@Component({
  selector: 'app-crm-smm-outlookcampaignsentsummary',
  templateUrl: './crm-smm-outlookcampaignsentsummary.component.html',
  styleUrls: ['./crm-smm-outlookcampaignsentsummary.component.scss']
})
export class CrmSmmOutlookcampaignsentsummaryComponent {
  template_gid: any;
  template_name: any;
  responsedata: any;
  outlooksentcampaign_list: any;
  constructor( public service: SocketService, private router: ActivatedRoute, private route: Router) {
  }
  ngOnInit(): void {
    const template_gid = this.router.snapshot.paramMap.get('template_gid');
    const template_name = this.router.snapshot.paramMap.get('template_name');
    this.template_gid = template_gid;
    this.template_name = template_name;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_gid, secretKey).toString(enc.Utf8);
    const deencryptedParams = AES.decrypt(this.template_name, secretKey).toString(enc.Utf8);
    this.template_gid = deencryptedParam;
    this.template_name = deencryptedParams;
    this.GetCampaignSentSummary();

  }
  GetCampaignSentSummary(){
    debugger
    let param = {
      template_gid: this.template_gid
    }
    var api = 'OutlookCampaign/OutlookCampaignSentSummary'
    this.service.getparams(api,param).subscribe((result: any) => {
      this.responsedata = result;
      this.outlooksentcampaign_list = this.responsedata.outlooksentcampaign_list;
    });
  }
  onback(){
    this.route.navigate(['/crm/CrmSmmOutlookcampaignsummary']);
  }
}
