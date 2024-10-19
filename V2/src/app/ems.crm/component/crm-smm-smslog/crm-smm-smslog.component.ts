import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-crm-smm-smslog',
  templateUrl: './crm-smm-smslog.component.html',
  styleUrls: ['./crm-smm-smslog.component.scss']
})
export class CrmSmmSmslogComponent {
  responsedata: any;
  template_id: any;
  smscampaignlog: any;
  campagin_title: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,
    private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    debugger
    const template_id = this.router.snapshot.paramMap.get('template_id');
    this.template_id = template_id;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.template_id, secretKey).toString(enc.Utf8);
    this.template_id = deencryptedParam;
    this.Getsmscampaignlog();
  }
  //// Summary Grid//////
  Getsmscampaignlog() {
    this.NgxSpinnerService.show();
    let param = {
      template_id: this.template_id,
    }
    var api = 'SmsCampaign/Getsmscampaignlog'
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#smscampaignlog').DataTable().destroy();
      this.responsedata = result;
      this.smscampaignlog = this.responsedata.smscampaignlog;
      this.campagin_title = this.smscampaignlog[0].campagin_title;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#smscampaignlog').DataTable();
      }, 1);


    });
  }

}
