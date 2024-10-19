import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';

import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
interface Ileadbank {
  country_name: string;
  region_name: string;
  leadbank_gid: string;
  leadbankcontact_gid: string;
  area_code2: string;
  phone2: string;
  country_code1: string;
  area_code1: string;
  phone1: string;
  fax_area_code: string;
  fax_country_code: string;
  fax: string;
  designation: string;
  email: string;
  mobile: string;
  leadbankcontact_name: string;
  leadbankbranch_name: string;
  address1: string;
  address2: string;
  state: string;
  city: string;
  pincode: string;
}

@Component({
  selector: 'app-crm-trn-leadbankbranch',
  templateUrl: './crm-trn-leadbankbranch.component.html',
  styleUrls: ['./crm-trn-leadbankbranch.component.scss']
})
export class CrmTrnLeadbankbranchComponent implements OnInit {
  leadbank!: Ileadbank;
  leadbank_gid: any;
  leadbankcontact_gid: any;
  leadaddbranch_list: any[] = [];
  response_data: any;
  branch_list: any;
  region_list: any[] = [];
  branchform: FormGroup<{}> | any;
  country_list: any[] = [];
  selectedCountry: any;
  selectedRegion: any;

  constructor(private fb: FormBuilder, private router: ActivatedRoute, private route: Router,
    private service: SocketService, private ToastrService: ToastrService) {

    this.leadbank = {} as Ileadbank;
  }
  ngOnInit(): void {

    const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');

    this.leadbank_gid = leadbank_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

    this.GetleadbranchaddSummary(deencryptedParam);

    this.branchform = new FormGroup({
      leadbankcontact_name: new FormControl(this.leadbank.leadbankcontact_name, [
        Validators.required,
      ]),

      leadbankbranch_name: new FormControl('', 
      Validators.required
        ),
      address2: new FormControl(''),
      state: new FormControl(''),
      pincode: new FormControl(''),
      city: new FormControl(this.leadbank.city, [
        Validators.required,
      ]),
      region_name: new FormControl(this.leadbank.region_name, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
      ]),

      mobile: new FormControl(this.leadbank.mobile, [
        Validators.required,
        Validators.maxLength(10),
      ]),

      designation: new FormControl(this.leadbank.designation, [
        Validators.required,
        Validators.minLength(1),
      ]),
      country_name: new FormControl(this.leadbank.country_name, [
        Validators.minLength(1),
      ]),
      address1: new FormControl(this.leadbank.address1, [
        Validators.maxLength(1000),
      ]),
      email: new FormControl(this.leadbank.email, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.pattern('^([a-z0-9-]+|[a-z0-9-]+([.][a-z0-9-]+)*)@([a-z0-9-]+\.[a-z]{2,20}(\.[a-z]{2})?|\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\]|localhost)$')
      ]),
      
      leadbank_gid: new FormControl(deencryptedParam),
      leadbankcontact_gid: new FormControl(''),
    });
    

    var api2 = 'registerlead/Getregiondropdown1'
    this.service.get(api2).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.region_list = this.response_data.Getregiondropdown1;
      setTimeout(() => {
        $('#product').DataTable();
      }, 1);
    });

    var api2 = 'registerlead/Getcountrynamedropdown'
    this.service.get(api2).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      console.log("countrydropdown value:"+this.response_data);
      
      this.country_list = this.response_data.Getcountrynamedropdown;
      setTimeout(() => {
        $('#product').DataTable();
      }, 1);
    });

  }
  get leadbank_name() {
    return this.branchform.get('leadbank_name')!;
  }

  GetleadbranchaddSummary(leadbank_gid: any) {
    let param = {
      leadbank_gid: leadbank_gid
    }
    var api = 'registerlead/GetleadbranchaddSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#leadaddbranch_list').DataTable().destroy();
      this.response_data = result;
      this.leadaddbranch_list = this.response_data.leadaddbranch_list;
      setTimeout(() => {
        $('#leadaddbranch_list').DataTable();
      }, 1);
    });
  }

  onedit(param1: any, param2: any) {
    const secretKey = 'storyboarderp';
    console.log(param1);
    console.log(param2);
    const leadbank_gid = AES.encrypt(param1, secretKey).toString();
    const leadbankcontact_gid = AES.encrypt(param2, secretKey).toString();
    this.route.navigate(['/crm/CrmTrnLeadbankbranchedit', leadbank_gid, leadbankcontact_gid]);
  }


  onadd() {
    this.leadbank = this.branchform.value;

    if (this.leadbank.leadbankbranch_name != null && this.leadbank.leadbankcontact_name != null &&
      this.leadbank.designation != null && this.leadbank.address1 != null
      && this.leadbank.state != null && this.leadbank.city != null && this.leadbank.pincode != null) {
      console.log(this.branchform.value)
      //  this.leadbank.region_name != null,this.leadbank.country_name != null  &&
      var api = 'registerlead/Addbranchlead';

      this.service.post(api, this.branchform.value).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)

        }
        else {
          this.GetleadbranchaddSummary(this.leadbank_gid);
          // this.route.navigate(['/crm/CrmTrnLeadBankbranch',this.leadbank_gid]);
          window.location.reload()
          this.ToastrService.success(result.message)
        }
      });
    }
  }

  
}