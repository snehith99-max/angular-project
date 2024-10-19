import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";
import { NgxSpinnerService } from 'ngx-spinner';
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
  phone: string;
  leadbankcontact_name: string;
  leadbankbranch_name: string;
  address1: string;
  address2: string;
  state: string;
  city: string;
  pincode: string;
  leadbankcontact_nameedit:string;
  phone_edit:string;
  email_edit:string;
  phone1_edit:string;
  phone2_edit:string;
  designation_edit:string;

}

@Component({
  selector: 'app-crm-trn-leadbankcontact',
  templateUrl: './crm-trn-leadbankcontact.component.html',
  styleUrls: ['./crm-trn-leadbankcontact.component.scss']
})
export class CrmTrnLeadbankcontactComponent implements OnInit{
  
  leadbank!: Ileadbank;
  leadbank_gid: any;
  leadbankcontact_gid: any;
  leadaddbranch_list: any[] = [];
  response_data: any;
  branch_list: any;
  branch_list1: any[] = [];
  contactform: FormGroup<{}> | any;
  country_list: any[] = [];
  selectedCountry: any;
  selectedRegion: any;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  contacteditform!: FormGroup | any;
  responsedata:any;
  editContactSummary_list: any;
  parameterValue:any;

  constructor(private fb: FormBuilder, private router: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService,  private route: Router,
    private service: SocketService, private ToastrService: ToastrService) {

    this.leadbank = {} as Ileadbank;
  }
  ngOnInit(): void {

    const leadbank_gid = this.router.snapshot.paramMap.get('leadbank_gid');

    this.leadbank_gid = leadbank_gid;

    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.leadbank_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.leadbank_gid = deencryptedParam;

    this.GetleadbankcontactaddSummary(deencryptedParam);
    this.contactform = new FormGroup({
      leadbankcontact_name: new FormControl(this.leadbank.leadbankcontact_name, [
        Validators.required,
      ]),
    
      leadbankbranch_name: new FormControl('', ),
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

      phone: new FormControl(this.leadbank.phone, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      fax: new FormControl(this.leadbank.fax, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      phone1: new FormControl(this.leadbank.phone1, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      phone2: new FormControl(this.leadbank.phone2, [
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
    this.contacteditform = new FormGroup({
      leadbankcontact_nameedit: new FormControl(this.leadbank.leadbankcontact_nameedit, [
        Validators.required,
      ]),
      phone_edit: new FormControl(this.leadbank.phone_edit, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      phone1_edit: new FormControl(this.leadbank.phone1_edit, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      phone2_edit: new FormControl(this.leadbank.phone2_edit, [
        Validators.required,
        Validators.maxLength(10),
      ]),
      designation_edit: new FormControl(this.leadbank.designation_edit, [
        Validators.required,
        Validators.minLength(1),
      ]),
     
      email_edit: new FormControl(this.leadbank.email_edit, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250),
        Validators.pattern('^([a-z0-9-]+|[a-z0-9-]+([.][a-z0-9-]+)*)@([a-z0-9-]+\.[a-z]{2,20}(\.[a-z]{2})?|\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\]|localhost)$')
      ]),
      
      leadbank_gid: new FormControl(deencryptedParam),
      leadbankcontact_gid: new FormControl(''),
    });

  }
  get leadbankcontact_name() {
    return this.contactform.get('leadbankcontact_name')!;
  }
  get phone() {
    return this.contactform.get('phone')!;
  }

  GetleadbankcontactaddSummary(leadbank_gid: any) {
    let param = {
      leadbank_gid: leadbank_gid
    }
    var api = 'Leadbank/GetleadbankcontactaddSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#leadaddbranch_list').DataTable().destroy();
      this.response_data = result;
      this.leadaddbranch_list = this.response_data.leadbank_list;
      setTimeout(() => {
        $('#leadaddbranch_list').DataTable();
      }, 1);
    });
  }

  onadd() {
    this.leadbank = this.contactform.value;
    if (!this.contactform.value.phone) {
      this.contactform.value.phone = { e164Number: null }; // Or any default value you prefer
    }
    if ( this.leadbank.leadbankcontact_name != null) 
    {
      this.NgxSpinnerService.show();
      var api = 'Leadbank/Postleadbankcontactadd';
      this.service.post(api, this.contactform.value).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        console.log(result);
        if (result.status == false) {
          
          window.location.reload()
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          
          this.ToastrService.warning(result.message)

        }
        else {
          
          this.GetleadbankcontactaddSummary(this.leadbank_gid);
          window.location.reload();
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
   
  GeteditContactSummary(leadbankcontact_gid: any) {
    var url = 'Leadbank/GetleadbankcontacteditsSummary'
    
    let param = {leadbankcontact_gid : leadbankcontact_gid}
    console.log("Leadbankcontact_gid:"+ param);
      
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata=result;
      this.editContactSummary_list = result.leadbank_list;

      this.contacteditform.get("leadbankcontact_gid")?.setValue(this.editContactSummary_list[0].leadbankcontact_gid);
      this.contacteditform.get("leadbankcontact_nameedit")?.setValue(this.editContactSummary_list[0].leadbankcontact_name);
      this.contacteditform.get("phone_edit")?.setValue(this.editContactSummary_list[0].mobile);
      this.contacteditform.get("email_edit")?.setValue(this.editContactSummary_list[0].email);
      this.contacteditform.get("designation_edit")?.setValue(this.editContactSummary_list[0].designation);
      this.contacteditform.get("phone1_edit")?.setValue(this.editContactSummary_list[0].phone1);
      this.contacteditform.get("phone2_edit")?.setValue(this.editContactSummary_list[0].phone2);
    });

  }
 
  get leadbankcontact_nameedit() {
    return this.contacteditform.get('leadbankcontact_nameedit');
  }
  get phone_edit() {
    return this.contacteditform.get('phone_edit');
  }
  get phone1_edit() {
    return this.contacteditform.get('phone1_edit');
  }
  get phone2_edit() {
    return this.contacteditform.get('phone2_edit');
  }
  get designation_edit() {
    return this.contacteditform.get('designation_edit');
  }
  get email_edit() {
    return this.contacteditform.get('email_edit');
  }
  public validate(): void {
    this.leadbank = this.contacteditform.value;
    if (!this.contacteditform.value.phone_edit) {
      this.contacteditform.value.phone_edit = { e164Number: null }; // Or any default value you prefer
    }
    if (this.leadbank.leadbankcontact_nameedit != null && this.leadbank.leadbankcontact_nameedit != '')  
    {
     console.log(this.contacteditform.value)
     this.NgxSpinnerService.show();
     const api = 'Leadbank/UpdateleadbankContactedit';
 
     this.service.post(api,this.contacteditform.value).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        
        this.GetleadbankcontactaddSummary(this.leadbank_gid);
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        
        this.GetleadbankcontactaddSummary(this.leadbank_gid);
          this.ToastrService.success(result.message);
      }
    });
  }
  else {
    window.scrollTo({
      top: 0, // Code is used for scroll top after event done
    });
    
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
  }
}
openModaldelete(parameter: string) {
  this.parameterValue = parameter
}
ondelete() {
  this.NgxSpinnerService.show();
  var url = 'Leadbank/deleteLeadbankcontact'
  let param = {
    leadbankcontact_gid: this.parameterValue
  }
  this.service.getparams(url, param).subscribe((result: any) => {
    if (result.Status == true) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      
       this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message)
      
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      
       this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message)
    }
    this.GetleadbankcontactaddSummary(this.leadbank_gid);    
  });
}
}