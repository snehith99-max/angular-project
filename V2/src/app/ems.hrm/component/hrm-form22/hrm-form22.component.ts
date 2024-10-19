import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';

interface IForm22 {

  //Form2(Sub-Rule 3)
  company_address: string;  
  typeof_industry: string;  
  numberof_workmen: string;  
  numberofworkmen_form1: string;  
  totalworkdays_year: string;  
  nonpermanent_count: string;  
  permanent_count: string;  
  permanentcount_firstjuly: string;  
  reason_delay: string;
  remarks: string;
  processed_year: string;

  //Form2(Sub-Rule 4)
  postal_address: string;  
  nameaddress_employers: string;  
  serial_number: string;  
  nameaddressofemployee_suspension: string;  
  wagespaid_monthlyemployees: string;  
  departmentanddesignation_last: string;  
  natureof_offence: string;  
  suspension_date: string;  
  commencementenquiry_date: string;
  completionenquiry_date: string;
  revocationsuspension_date: string;
  subsistenceallowence_rate: string;
  subsistenceallowence_paid: string;
  dateofissue_finalorder: string;
  employees_punishment: string;
  remarks_data: string;

   //Form21(Sub-Rule 1)
   registration_number: string;
   occupier_address: string;  
   managingpartner_address: string;  
   managingpartner_address1: string;  
   natureof_industry: string;  
   numberofdaysworked_halfyear: string;
   averageofworkersemployeed_daily: string;  
   adultsmale_count: string;  
   adultsfemale_count: string;  
   adolescentsmale_count: string;  
   adolescentsfemale_count: string;
   childrenmale_count: string;
   childrenfemale_count: string;
   dispatch_date: string;
   


}

@Component({
  selector: 'app-hrm-form22',
  templateUrl: './hrm-form22.component.html',
  styleUrls: ['./hrm-form22.component.scss']
})
export class HrmForm22Component {
  reactiveuserForm!: FormGroup;
  reactivemasterForm!: FormGroup;
  reactivedataForm!: FormGroup;
  responsedata: any;
  companyedit_list: any;
  selectedCompany: any;
  halfyearlysubrule3_list: any;
  form22subrule3edit_list: any;
  form22subrule4edit_list: any;
  selectedFormsubrule3: any;
  selectedFormsubrule4: any;
  form22subrule1edit_list: any;
   formyear: any;
   form_gid:any;
   processed_year:any;

  form22!: IForm22;


  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    
       ) 
    {
    this.form22 = {} as IForm22;


    // Form2(sub-rule3)
    this.reactiveuserForm = new FormGroup({
      company_address: new FormControl(this.form22.company_address, []),
      typeof_industry: new FormControl(this.form22.typeof_industry, []),
      numberof_workmen: new FormControl(this.form22.numberof_workmen, []),
      numberofworkmen_form1: new FormControl(this.form22.numberofworkmen_form1, []),
      totalworkdays_year: new FormControl(this.form22.totalworkdays_year, []),
      nonpermanent_count: new FormControl(this.form22.nonpermanent_count, []),
      permanent_count: new FormControl(this.form22.permanent_count, []),
      permanentcount_firstjuly: new FormControl(this.form22.permanentcount_firstjuly, []),
      reason_delay: new FormControl(this.form22.reason_delay, []),
      remarks: new FormControl(this.form22.remarks, []),
      processed_year: new FormControl(this.form22.processed_year, []),
      form_gid: new FormControl(),
      
      
    });

     // Form2(sub-rule4)
     this.reactivemasterForm = new FormGroup({
      postal_address: new FormControl(this.form22.postal_address, []),
      nameaddress_employers: new FormControl(this.form22.nameaddress_employers, []),
      serial_number: new FormControl(this.form22.serial_number, []),
      nameaddressofemployee_suspension: new FormControl(this.form22.nameaddressofemployee_suspension, []),
      wagespaid_monthlyemployees: new FormControl(this.form22.wagespaid_monthlyemployees, []),
      departmentanddesignation_last: new FormControl(this.form22.departmentanddesignation_last, []),
      natureof_offence: new FormControl(this.form22.natureof_offence, []),
      suspension_date: new FormControl(this.form22.suspension_date, []),
      commencementenquiry_date: new FormControl(this.form22.commencementenquiry_date, []),
      completionenquiry_date: new FormControl(this.form22.completionenquiry_date, []),
      revocationsuspension_date: new FormControl(this.form22.revocationsuspension_date, []),
      subsistenceallowence_rate: new FormControl(this.form22.subsistenceallowence_rate, []),
      subsistenceallowence_paid: new FormControl(this.form22.subsistenceallowence_paid, []),
      dateofissue_finalorder: new FormControl(this.form22.dateofissue_finalorder, []),
      employees_punishment: new FormControl(this.form22.employees_punishment, []),
      remarks_data: new FormControl(this.form22.remarks_data, []),
      form_gid: new FormControl(),
      
      
    });

      // Form21(sub-rule1)
      this.reactivedataForm = new FormGroup({
        registration_number: new FormControl(this.form22.registration_number, []),
        occupier_address: new FormControl(this.form22.occupier_address, []),
        managingpartner_address: new FormControl(this.form22.managingpartner_address, []),
        managingpartner_address1: new FormControl(this.form22.managingpartner_address1, []),
        natureof_industry: new FormControl(this.form22.natureof_industry, []),
        numberofdaysworked_halfyear: new FormControl(this.form22.numberofdaysworked_halfyear, []),
        averageofworkersemployeed_daily: new FormControl(this.form22.averageofworkersemployeed_daily, []),
        adultsmale_count: new FormControl(this.form22.adultsmale_count, []),
        adultsfemale_count: new FormControl(this.form22.adultsfemale_count, []),
        adolescentsmale_count: new FormControl(this.form22.adolescentsmale_count, []),
        adolescentsfemale_count: new FormControl(this.form22.adolescentsfemale_count, []),
        childrenmale_count: new FormControl(this.form22.childrenmale_count, []),
        childrenfemale_count: new FormControl(this.form22.childrenfemale_count, []),
        dispatch_date: new FormControl(this.form22.dispatch_date, []),
        form_gid: new FormControl(),
        
      });
  }

  // Form2(sub-rule3)
  get company_address() {
    return this.reactiveuserForm.get('company_address')!;
  }
  get typeof_industry() {
    return this.reactiveuserForm.get('typeof_industry')!;
  }
  get numberof_workmen() {
    return this.reactiveuserForm.get('numberof_workmen')!;
  }
  get numberofworkmen_form1() {
    return this.reactiveuserForm.get('numberofworkmen_form1')!;
  }
  get totalworkdays_year() {
    return this.reactiveuserForm.get('totalworkdays_year')!;
  }
  get permanent_count() {
    return this.reactiveuserForm.get('permanent_count')!;
  }
  get nonpermanent_count() {
    return this.reactiveuserForm.get('nonpermanent_count')!;
  }
  get permanentcount_firstjuly() {
    return this.reactiveuserForm.get('permanentcount_firstjuly')!;
  }
  get reason_delay() {
    return this.reactiveuserForm.get('reason_delay')!;
  }
  get remarks() {
    return this.reactiveuserForm.get('remarks')!;
  }
 

  // Form2(sub-rule4)
  get postal_address() {
    return this.reactivemasterForm.get('postal_address')!;
  }
  get nameaddress_employers() {
    return this.reactivemasterForm.get('nameaddress_employers')!;
  }
  get serial_number() {
    return this.reactivemasterForm.get('serial_number')!;
  }
  get nameaddressofemployee_suspension() {
    return this.reactivemasterForm.get('nameaddressofemployee_suspension')!;
  }
  get wagespaid_monthlyemployees() {
    return this.reactivemasterForm.get('wagespaid_monthlyemployees')!;
  }
  get departmentanddesignation_last() {
    return this.reactivemasterForm.get('departmentanddesignation_last')!;
  }
  get natureof_offence() {
    return this.reactivemasterForm.get('natureof_offence')!;
  }
  get suspension_date() {
    return this.reactivemasterForm.get('suspension_date')!;
  }
  get commencementenquiry_date() {
    return this.reactivemasterForm.get('commencementenquiry_date')!;
  }
 get completionenquiry_date() {
     return this.reactivemasterForm.get('completionenquiry_date')!;
   }
  get revocationsuspension_date() {
    return this.reactivemasterForm.get('revocationsuspension_date')!;
  }
  get subsistenceallowence_rate() {
    return this.reactivemasterForm.get('subsistenceallowence_rate')!;
  }
  get subsistenceallowence_paid() {
    return this.reactivemasterForm.get('subsistenceallowence_paid')!;
  }
  get dateofissue_finalorder() {
    return this.reactivemasterForm.get('dateofissue_finalorder')!;
  }
  get employees_punishment() {
    return this.reactivemasterForm.get('employees_punishment')!;
  }
  get remarks_data() {
    return this.reactivemasterForm.get('remarks_data')!;
  }

  // Form21(sub-rule1)
  get registration_number() {
    return this.reactivedataForm.get('registration_number')!;
  }
  get occupier_address() {
    return this.reactivedataForm.get('occupier_address')!;
  }
  get managingpartner_address() {
    return this.reactivedataForm.get('managingpartner_address')!;
  }
  get managingpartner_address1() {
    return this.reactivedataForm.get('managingpartner_address1')!;
  }
  get natureof_industry() {
    return this.reactivedataForm.get('natureof_industry')!;
  }
  get numberofdaysworked_halfyear() {
    return this.reactivedataForm.get('numberofdaysworked_halfyear')!;
  }
  get averageofworkersemployeed_daily() {
    return this.reactivedataForm.get('averageofworkersemployeed_daily')!;
  }
  get adultsmale_count() {
    return this.reactivedataForm.get('adultsmale_count')!;
  }
  get adultsfemale_count() {
    return this.reactivedataForm.get('adultsfemale_count')!;
  }
  get adolescentsmale_count() {
    return this.reactivedataForm.get('adolescentsmale_count')!;
  }
  get adolescentsfemale_count() {
    return this.reactivedataForm.get('adolescentsfemale_count')!;
  }
  get childrenmale_count() {
    return this.reactivedataForm.get('childrenmale_count')!;
  }
  get childrenfemale_count() {
    return this.reactivedataForm.get('childrenfemale_count')!;
  }
  get dispatch_date() {
    return this.reactivedataForm.get('dispatch_date')!;
  }
 
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);

    const formyear = this.router.snapshot.paramMap.get('formyear');
    this.formyear = formyear;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.formyear, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
   
    const [form_gid, processed_year] = deencryptedParam.split('+');
    this.form_gid = form_gid;
    this.processed_year = processed_year;

    // var url = 'HrmForm22/GetHalfyearlysubrule3Summary'
    // this.service.get(url).subscribe((result: any) => {

    //   this.responsedata = result;
    //   this.halfyearlysubrule3_list = this.responsedata.halfyearlysubrule3_list;
    // });
    this.GetEditForm22SubRule3(form_gid, processed_year);
    this.GetEditForm22SubRule4(form_gid, processed_year);
    this.GetEditForm22SubRule1(form_gid, processed_year);
   
  }

  GetEditForm22SubRule3(form_gid: any,processed_year: any) {

    var url = 'HrmForm22/GetEditForm22SubRule3'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.form22subrule3edit_list = result.GetEditForm22SubRule3;
      console.log(this.form22subrule3edit_list)
      this.selectedFormsubrule3 = this.form22subrule3edit_list[0].form_gid;
      this.reactiveuserForm.get("company_address")?.setValue(this.form22subrule3edit_list[0].company_address);
      this.reactiveuserForm.get("typeof_industry")?.setValue(this.form22subrule3edit_list[0].typeof_industry);
      this.reactiveuserForm.get("numberof_workmen")?.setValue(this.form22subrule3edit_list[0].numberof_workmen);
      this.reactiveuserForm.get("numberofworkmen_form1")?.setValue(this.form22subrule3edit_list[0].numberofworkmen_form1);
      this.reactiveuserForm.get("totalworkdays_year")?.setValue(this.form22subrule3edit_list[0].totalworkdays_year);
      this.reactiveuserForm.get("nonpermanent_count")?.setValue(this.form22subrule3edit_list[0].nonpermanent_count);
      this.reactiveuserForm.get("permanent_count")?.setValue(this.form22subrule3edit_list[0].permanent_count);
      this.reactiveuserForm.get("permanentcount_firstjuly")?.setValue(this.form22subrule3edit_list[0].permanentcount_firstjuly);
      this.reactiveuserForm.get("reason_delay")?.setValue(this.form22subrule3edit_list[0].reasons);
      this.reactiveuserForm.get("remarks")?.setValue(this.form22subrule3edit_list[0].remarks);
      
      this.reactiveuserForm.get("form_gid")?.setValue(this.form22subrule3edit_list[0].form_gid);
    
     
    });
  }

  GetEditForm22SubRule4(form_gid: any,processed_year: any) {
    debugger;
    var url = 'HrmForm22/GetEditForm22SubRule4'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.form22subrule4edit_list = result.GetEditForm22SubRule4;
      console.log(this.form22subrule4edit_list)
      this.selectedFormsubrule4 = this.form22subrule4edit_list[0].form_gid;
      this.reactivemasterForm.get("postal_address")?.setValue(this.form22subrule4edit_list[0].postal_address);
      this.reactivemasterForm.get("nameaddress_employers")?.setValue(this.form22subrule4edit_list[0].managingpartner_address);
      this.reactivemasterForm.get("serial_number")?.setValue(this.form22subrule4edit_list[0].serial_number);
      this.reactivemasterForm.get("nameaddressofemployee_suspension")?.setValue(this.form22subrule4edit_list[0].nameaddressofemployee_suspension);
      this.reactivemasterForm.get("wagespaid_monthlyemployees")?.setValue(this.form22subrule4edit_list[0].wagespaid_monthlyemployees);
      this.reactivemasterForm.get("departmentanddesignation_last")?.setValue(this.form22subrule4edit_list[0].departmentanddesignation_last);
      this.reactivemasterForm.get("natureof_offence")?.setValue(this.form22subrule4edit_list[0].natureof_offence);
      this.reactivemasterForm.get("suspension_date")?.setValue(this.form22subrule4edit_list[0].suspension_date);
      this.reactivemasterForm.get("commencementenquiry_date")?.setValue(this.form22subrule4edit_list[0].commencementenquiry_date);
      this.reactivemasterForm.get("completionenquiry_date")?.setValue(this.form22subrule4edit_list[0].completionenquiry_date);
      this.reactivemasterForm.get("revocationsuspension_date")?.setValue(this.form22subrule4edit_list[0].revocationsuspension_date);
      this.reactivemasterForm.get("subsistenceallowence_rate")?.setValue(this.form22subrule4edit_list[0].subsistenceallowence_rate);
      this.reactivemasterForm.get("subsistenceallowence_paid")?.setValue(this.form22subrule4edit_list[0].subsistenceallowence_paid);
      this.reactivemasterForm.get("dateofissue_finalorder")?.setValue(this.form22subrule4edit_list[0].dateofissue_finalorder);
      this.reactivemasterForm.get("employees_punishment")?.setValue(this.form22subrule4edit_list[0].employees_punishment);
      this.reactivemasterForm.get("remarks_data")?.setValue(this.form22subrule4edit_list[0].remarks);
      
      this.reactivemasterForm.get("form_gid")?.setValue(this.form22subrule4edit_list[0].form_gid);
    
     
    });
  }

  GetEditForm22SubRule1(form_gid: any,processed_year: any) {

    debugger;
    var url = 'HrmForm22/GetEditForm22SubRule1'
    let param = {
      form_gid: form_gid,
      processed_year: processed_year,
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata=result;
      this.form22subrule1edit_list = result.GetEditForm22SubRule1;
      console.log(this.form22subrule1edit_list)
      this.selectedFormsubrule4 = this.form22subrule1edit_list[0].form_gid;
      this.reactivedataForm.get("registration_number")?.setValue(this.form22subrule1edit_list[0].registration_number);
      this.reactivedataForm.get("occupier_address")?.setValue(this.form22subrule1edit_list[0].occupier_address);
      this.reactivedataForm.get("managingpartner_address")?.setValue(this.form22subrule1edit_list[0].managingpartner_address);
      this.reactivedataForm.get("managingpartner_address1")?.setValue(this.form22subrule1edit_list[0].managingpartner_address1);
      this.reactivedataForm.get("natureof_industry")?.setValue(this.form22subrule1edit_list[0].natureof_industry);
      this.reactivedataForm.get("numberofdaysworked_halfyear")?.setValue(this.form22subrule1edit_list[0].numberofdaysworked_halfyear);
      this.reactivedataForm.get("averageofworkersemployeed_daily")?.setValue(this.form22subrule1edit_list[0].averageofworkersemployeed_daily);
      this.reactivedataForm.get("adultsmale_count")?.setValue(this.form22subrule1edit_list[0].adultsmale_count);
      this.reactivedataForm.get("adultsfemale_count")?.setValue(this.form22subrule1edit_list[0].adultsfemale_count);
      this.reactivedataForm.get("adolescentsmale_count")?.setValue(this.form22subrule1edit_list[0].adolescentsmale_count);
      this.reactivedataForm.get("adolescentsfemale_count")?.setValue(this.form22subrule1edit_list[0].adolescentsfemale_count);
      this.reactivedataForm.get("childrenmale_count")?.setValue(this.form22subrule1edit_list[0].childrenmale_count);
      this.reactivedataForm.get("childrenfemale_count")?.setValue(this.form22subrule1edit_list[0].childrenfemale_count);
      this.reactivedataForm.get("dispatch_date")?.setValue(this.form22subrule1edit_list[0].dispatch_date);
      
      this.reactivedataForm.get("form_gid")?.setValue(this.form22subrule4edit_list[0].form_gid);
    
     
    });
  }



  CompanySummary() {

    var url = 'HrmForm22/CompanySummary'
   
    this.service.get(url).subscribe((result: any) => {
    this.responsedata=result;
      this.companyedit_list = result.company_list;
      console.log(this.companyedit_list)
      console.log(this.companyedit_list[0].company_gid)
    
      this.selectedCompany = this.companyedit_list[0].company_gid;
      this.reactiveuserForm.get("name_address")?.setValue(this.companyedit_list[0].company_address);
      this.reactiveuserForm.get("company_gid")?.setValue(this.companyedit_list[0].company_gid);
    
     
    });
  }

  
  form2subrule3submit() {
    
    this.reactiveuserForm.get("form_gid")?.setValue(this.form_gid);

     if (this.reactiveuserForm.value.company_address != null && this.reactiveuserForm.value.company_address != '') {
      this.reactiveuserForm.value;
      var url = 'HrmForm22/Form2SubRule3Submit'
      this.service.post(url, this.reactiveuserForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }
    
  }
   

  onuserback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }
  
  form2subrule4submit() {

    this.reactivemasterForm.get("form_gid")?.setValue(this.form_gid);

    if (this.reactivemasterForm.value.postal_address != null && this.reactivemasterForm.value.postal_address != '') {
      this.reactivemasterForm.value;
      var url = 'HrmForm22/Form2SubRule4Submit'
      this.service.post(url, this.reactivemasterForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }
    
  }

  onmasterback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }

  form21subrule1submit() {

    this.reactivedataForm.get("form_gid")?.setValue(this.form_gid);

    if (this.reactivedataForm.value.registration_number != null && this.reactivedataForm.value.registration_number != '') {
      this.reactivedataForm.value;
      var url = 'HrmForm22/Form21SubRule1Submit'
      this.service.post(url, this.reactivedataForm.value).subscribe((result: any) => {
        if (result.status == false) {
        this.ToastrService.warning(result.message)
        }
        else {
        
          this.ToastrService.success(result.message)
        }
        
      });
    }
   
    

  }

  ondataback(){
    this.route.navigate(['/hrm/HrmTrnStatutoryforms']) 
  }
}
