import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-crm-smm-gmailcampaignsendstatus',
  templateUrl: './crm-smm-gmailcampaignsendstatus.component.html',
  styleUrls: ['./crm-smm-gmailcampaignsendstatus.component.scss']
})
export class CrmSmmGmailcampaignsendstatusComponent {
  responsedata: any;
  mailtemplatesendsummary_list: any;
  filteredData: any;
  mailevent_list: any;
  mailcount_list: any[] = [];
  clicktotal_count: any;
  deliverytotal_count: any;
  opentotal_count: any;
  template_gid: any;
  to_mail: any;
  sub: any;
  status_delivery: any;
  body: any;
  date: any;
  status_open: any;
  status_click: any;
  template_name: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService) {

  }

  ngOnInit(): void {
    const template_gid = this.router.snapshot.paramMap.get('template_gid');
    this.template_gid = template_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_gid, secretKey).toString(enc.Utf8);
    this.template_gid = deencryptedParam;
    this.GetMailTemplateSendSummary();

  }
  GetMailTemplateSendSummary() {
    //this.NgxSpinnerService.show();
    
    let param = {
      template_gid: this.template_gid,
    }
    var api = 'GmailCampaign/GmailSendStatusSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.mailtemplatesendsummary_list = this.responsedata.gmailtemplatesendsummary_list;
      this.template_name = this.mailtemplatesendsummary_list[0].template_name;

      console.log(this.template_name)
      //this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#mailtemplatesendsummary_list').DataTable();
      }, 1);
    });

  }

  onback() {
    this.route.navigate(['/crm/CrmSmmGmailcampaignsummary']);
  }

  popmodal(product_gid: any) {
    var params = {
      product_gid: product_gid
    }
    debugger
    var url = 'RskMstProductManagement/Getrm';



  }

}
