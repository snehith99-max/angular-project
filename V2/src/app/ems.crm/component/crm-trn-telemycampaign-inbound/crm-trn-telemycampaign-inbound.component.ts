import { Component } from '@angular/core';

import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-crm-trn-telemycampaign-inbound',
  templateUrl: './crm-trn-telemycampaign-inbound.component.html',
  styleUrls: ['./crm-trn-telemycampaign-inbound.component.scss']
})
export class CrmTrnTelemycampaignInboundComponent {
  responsedata: any;
  mycallstilescount_list: any[] = [];
  schedule_count: any;
  newleads_count: any;
  followup_count: any;
  prospect_count: any;
  drop_count: any;
  pending_count: any;
  alllead_count: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router, public service: SocketService) {
  }
  ngOnInit(): void{
    this.Getmycallstilescount();
  }
  Getmycallstilescount(){
    var url = 'MyCalls/GetMycallstilescount'
    this.service.get(url).subscribe((result: any) => {
     this.responsedata=result;
     this.mycallstilescount_list = this.responsedata.mycallstilescount_list;
     this.schedule_count = this.mycallstilescount_list[0].schedule_count;
     this.newleads_count = this.mycallstilescount_list[0].newleads_count;
     this.followup_count = this.mycallstilescount_list[0].followup_count;
     this.prospect_count = this.mycallstilescount_list[0].prospect_count;
     this.drop_count = this.mycallstilescount_list[0].drop_count;
     this.pending_count = this.mycallstilescount_list[0].pending_count;
     this.alllead_count = this.mycallstilescount_list[0].alllead_count;
    });
  }
}
