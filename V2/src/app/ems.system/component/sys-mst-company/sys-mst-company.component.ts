
import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

interface ICompany {
  company_gid: string;
  company_code: string;
  company_name: string;
  company_website: string;
  contact_person: string;
  company_phone: string;
  company_mail: string;
  company_address: string;
  country_gid: string;
  country_code: string;
  country_name: string;
  currency_code: string;
  authorised_sign: string;
  file_name: string;
}

@Component({
  selector: 'app-sys-mst-company',
  templateUrl: './sys-mst-company.component.html',
  styleUrls: ['./sys-mst-company.component.scss']
})

export class SysMstCompanyComponent implements OnInit {
  invalidFileFormat: boolean = false;
  selectedFileName : string ='';
  filename : string = '';
  authorised_sign: string | null = null;
  Images: File | null = null;
  file!: File;
  file2!: File;

  company!: ICompany;
  reactiveForm!: FormGroup;
  country_list: any[] = [];
  finyear_list: any[] = [];
  currency_list: any[] = [];
  company_list: any[] = [];
  Getcountrydropdown: any;
  companyform: FormGroup | any;
  responsedata: any;
  form: FormGroup;
  sample: any;
  file_name:any;
  file_name_1:any;
  authorised_sign1: any;

  constructor(private fb: FormBuilder, private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService, private route: Router, private router: ActivatedRoute) {
    this.company = {} as ICompany;
    this.form = this.fb.group({
      authorised_sign: [''],
      file_name: ['']
    });
  }

  ngOnInit(): void {

    this.companyform = new FormGroup({
      company_code: new FormControl(this.company.company_code,),
      company_name: new FormControl(this.company.company_name, [Validators.required, Validators.pattern(/^\S.*$/)]),
      company_website: new FormControl(this.company.company_website, [
        Validators.required,
        Validators.pattern(/^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(\/\S*)?$/),  // Regex for URL format
        this.noEmailValidator(),
        this.noPhoneNumberValidator()
      ]),
      contact_person: new FormControl(this.company.contact_person, [Validators.required, Validators.pattern(/^\S.*$/)]),
      company_phone: new FormControl(this.company.company_phone, [Validators.required,Validators.pattern('^[0-9]{10,12}$'),Validators.minLength(10),Validators.maxLength(12)]),
      company_mail: new FormControl(this.company.company_mail,  [Validators.required, Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      company_address: new FormControl(this.company.company_address, [Validators.required,Validators.pattern(/^\S.*$/)]),
      country_code: new FormControl(''),
      country_name: new FormControl(''),
      currency: new FormControl(''),
      currency_code:new FormControl(''),
      authorised_sign: new FormControl(''),
      file_name: new FormControl(''),
    });

    

    var api = 'Company/GetCountrydropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.country_list = this.responsedata.countrylists;
    });

    var api = 'Company/GetCurrencydropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.currency_list = this.responsedata.currencylists;
    });

    var api = 'Company/GetCompany';
    this.service.get(api).subscribe((result: any) => {
      $('#company_list').DataTable().destroy();
      this.responsedata = result;
      this.company_list = this.responsedata.companylists;
      // console.log(this.company_list)
      this.companyform.get("company_code")?.setValue(this.company_list[0].company_code);
      this.companyform.get("company_name")?.setValue(this.company_list[0].company_name);
      this.companyform.get("company_website")?.setValue(this.company_list[0].company_website);
      this.companyform.get("contact_person")?.setValue(this.company_list[0].contact_person);
      this.companyform.get("company_phone")?.setValue(this.company_list[0].company_phone);
      this.companyform.get("company_mail")?.setValue(this.company_list[0].company_mail);
      this.companyform.get("company_address")?.setValue(this.company_list[0].company_address);
      this.companyform.get("country_name")?.setValue(this.company_list[0].country_name);
      this.companyform.get("currency_code")?.setValue(this.company_list[0].currency_code);
      this.authorised_sign1 =  (this.company_list[0].authorised_sign);
      this.companyform.get("authorised_sign")?.setValue(this.company_list[0].authorised_sign);
      console.log(this.companyform)
     });
  }

  validateNumberInput(event: KeyboardEvent): void {
    const charCode = event.which ? event.which : event.keyCode;
    // Only allow digits (0-9), backspace, and delete
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      event.preventDefault(); // Prevent any non-numeric input
    }
  }

  noEmailValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const forbidden = /@/.test(control.value); // Check if '@' exists in the input
      return forbidden ? { email: true } : null;
    };
  }
  
  noPhoneNumberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const phonePattern = /^[+]?[(]?[0-9]{1,4}[)]?[-\s./0-9]*$/;
      const forbidden = phonePattern.test(control.value); // Check for phone number pattern
      return forbidden ? { phoneNumber: true } : null;
    };
  }
  

  getFileName(filePath: string): string {
    return filePath ? filePath.split('/').pop() || '' : '';
  }
  
  onChange(event: any): void {
    this.file = event.target.files[0];
    const validImageTypes = ['image/jpeg', 'image/png', 'image/gif'];
    
    if (this.file && validImageTypes.includes(this.file.type)) {
      this.invalidFileFormat = false;
      this.companyform.get('authorised_sign')?.setValue(this.file);
    } else {
      this.invalidFileFormat = true;
      this.companyform.get('authorised_sign')?.reset();
      event.target.value = ''; // Clear the file input field
    }
    if (this.file) {
      this.file_name = this.file.name;
      this.Images = this.file;
      this.form.patchValue({
        file_name: this.file.name
      });
    }
   else {
      this.file_name = null;
      this.Images = null;
      this.form.patchValue({
        file_name: null
      });
    }
    // setTimeout(function () {
    //   window.location.reload();
    // }, 2000);
  }

  update(){
    debugger
    var api='Company/PostCompany';
    let params={
      company_name:this.companyform.value.company_name,
      company_website:this.companyform.value.company_website,
      contact_person:this.companyform.value.contact_person,
      company_phone:this.companyform.value.company_phone,
      company_mail:this.companyform.value.company_mail,
      company_address:this.companyform.value.company_address,
      country_name:this.companyform.value.country_name,
      currency_code:this.companyform.value.currency_code,
      authorised_sign:this.companyform.value.authorised_sign,
      file_name:this.companyform.value.file_name

    }
    this.service.postparams(api,params).subscribe((result:any) =>{
      if (result.status == true) {
        this.ToastrService.success(result.message)
        // this.route.navigate(['/hrm/HrmTrnMemberdhashboard']);
      }
      else {
       
        this.ToastrService.warning(result.message)
      }
    });
    
  }

  get CompanyNameControl() {
    return this.companyform.get('company_name')!;
  }

  get CompanyWebsiteControl() {
    return this.companyform.get('company_website')!;
  }

  get ContactPersonControl() {
    return this.companyform.get('contact_person')!;
  }

  get CompanyPhonecontrol() {
    return this.companyform.get('company_phone')!;
  }

  get CompanyMailcontrol() {
    return this.companyform.get('company_mail')!;
  }

  get Addresscontrol() {
    return this.companyform.get('company_address')!;
  }

  public validate(): void {
    debugger
    console.log("formData");
    this.company = this.companyform.value;

    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("authorised_sign", this.file, this.file.name);
      formData.append("company_code", this.company.company_code);
      formData.append("company_name", this.company.company_name);
      formData.append("company_website", this.company.company_website);
      formData.append("contact_person", this.company.contact_person);
      formData.append("company_phone", this.company.company_phone);
      formData.append("company_mail", this.company.company_mail);
      formData.append("company_address", this.company.company_address);
      formData.append("country_name", this.company.country_name);
      formData.append("currency_code", this.company.currency_code);


      var api = 'Company/UpdatedCompanylogo'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        $('#company_list').DataTable().destroy();
        if (result.status == false) {
          this.ToastrService.warning('Error While Updating Company Signatory !!')
          this.NgxSpinnerService.hide();
        }
        else {
          this.route.navigate(['/hrm/HrmTrnMemberdhashboard']);
          this.ToastrService.success('Company Signatory Updated Successfully !!')
          this.NgxSpinnerService.hide();
        }
        this.responsedata = result;
      });
    }
    else {
      var api = 'Company/PostCompany'
      this.NgxSpinnerService.show();
      this.service.postparams(api, this.company).subscribe((result: any) => {
        $('#company_list').DataTable().destroy();
        
        if (result.status == false) {
          this.ToastrService.warning('Company Details Updated Unsuccessfully')
          this.NgxSpinnerService.hide();
        }
        else {
          this.route.navigate(['/hrm/HrmTrnMemberdhashboard']);
          this.ToastrService.success('Company Details Updated Successfully')
          this.NgxSpinnerService.hide();
        }
        this.responsedata = result;
      });      
    }
   
    
  }  

  // redirecttolist() {
  //   this.route.navigate(['/einvoice/Dashboard']);
  // }
  back(){
    this.route.navigate(['/hrm/HrmTrnMemberdhashboard']);
  }
}