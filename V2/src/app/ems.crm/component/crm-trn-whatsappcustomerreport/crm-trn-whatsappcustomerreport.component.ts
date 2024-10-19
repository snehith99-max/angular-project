import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface Icustomerreport {
 
}

@Component({
  selector: 'app-crm-trn-whatsappcustomerreport',
  templateUrl: './crm-trn-whatsappcustomerreport.component.html',
  styleUrls: ['./crm-trn-whatsappcustomerreport.component.scss']
})
export class CrmTrnWhatsappcustomerreportComponent {
  isReadOnly = true;
  private unsubscribe: Subscription [] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  showOptionsDivId: any;
  parameterValue1: any;
  campaignlist:any;
  customerreport_list: any[] = [];
  Getcampaignlists: any[] = [];
  customerreport!: Icustomerreport;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
    this.customerreport = {} as Icustomerreport;
  }
  ngOnInit(): void {

    this.GetCustomerreport();
  }
  GetCustomerreport(){
    this.NgxSpinnerService.show();
    var api = 'Whatsapp/Getcustomerreport'
    this.service.get(api).subscribe((result: any) => {
      $('#customerreport_list').DataTable().destroy();
    this.responsedata = result;
    this.customerreport_list = this.responsedata.customerreport;
    this.NgxSpinnerService.hide();
    setTimeout(() => {
    $('#customerreport_list').DataTable();
      }, 1);
    });
    }

popmodal(parameter: string) {
  this.campaignlist = parameter
  this.Getcampaignlist(this.campaignlist);
} 
Getcampaignlist(campaignlist:any){
  this.NgxSpinnerService.show();
  let param = {
    contact_id : campaignlist
  }
  var url = 'Whatsapp/Getcampaignlist';
  this.service.getparams(url,param).subscribe((apiresponse: any) => {
    this.Getcampaignlists = apiresponse.campaignlists;
    this.NgxSpinnerService.hide();
  });
}
formatNumber(value: number): string {
  if (value >= 100000) {
    const num = value / 100000;
    if (num % 1 === 0) {
        return num.toFixed(0) + 'L';
    } else {
        return num.toFixed(1) + 'L';
    }
  } else if (value >= 1000) {
      const num = value / 1000;
      if (num % 1 === 0) {
          return num.toFixed(0) + 'k';
      } else {
          return num.toFixed(1) + 'k';
      }
  } else {
      return value.toString();
  }
}
}