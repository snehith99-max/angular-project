import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import {
  FormBuilder, FormControl, FormGroup, Validators, ValidationErrors,
  AbstractControl,
  ValidatorFn
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { Subscriber } from 'rxjs';
interface Ilinkedintextpost {
  linkedin_text: string;
  file_path: string;
}
interface Ilinkedinpollpost {
  linkedin_text: string;
  option1: string;
  option2: string;
  option3: string;
  option4: string;
  poll_duration: string;
  poll_question: string;
}
@Component({
  selector: 'app-crm-smm-linkedaccount',
  templateUrl: './crm-smm-linkedaccount.component.html',
  styleUrls: ['./crm-smm-linkedaccount.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmSmmLinkedaccountComponent {
  responsedata: any;
  accountsummary_list: any;
  currentTab: any = 'pageinfo';
  result: any;
  account_id: any;
  company_name: any;
  company_website: any;
  founded_on: any;
  followers_count: any;
  description: any;
  organizationStatus: any;
  organizationType: any;
  localizedSpecialties: any;
  accountview_list: any;
  linkedintextpost!: Ilinkedintextpost;
  Linkedintextpost!: FormGroup;
  parameterValue1: any;
  file!: File;
  Linkedinmediapost!: FormGroup;
  mediapost!: Ilinkedintextpost;
  maxChar = 120;
  maxChar1 = 30;
  showOption3: boolean = false;
  showOption4: boolean = false;
  maxChars: any;
  pollduration: any;
  linkedinpollpost!: Ilinkedinpollpost;
  Linkedinpollpost!: FormGroup;
  showOptionsDivId: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.linkedintextpost = {} as Ilinkedintextpost;
    this.mediapost = {} as Ilinkedintextpost;
    this.linkedinpollpost = {} as Ilinkedinpollpost;
  }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getaccountdetailssummary();
    this.Linkedintextpost = new FormGroup({
      linkedin_text: new FormControl(this.linkedintextpost.linkedin_text, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ])
    });
    this.Linkedinmediapost = new FormGroup({
      linkedin_text: new FormControl(this.mediapost.linkedin_text, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/)
      ]),
      file_path: new FormControl(this.mediapost.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
    });
    this.Linkedinpollpost = new FormGroup({
      linkedin_text: new FormControl(this.linkedinpollpost.linkedin_text, [
        Validators.pattern(/^(?!\s*$).+/)
      ]),
      poll_question: new FormControl(this.linkedinpollpost.poll_question, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/)
      ]),
      option1: new FormControl(this.linkedinpollpost.option1, [
        Validators.required,
        this.noWhitespaceValidator(),
        Validators.pattern(/^(?!\s*$).+/)

      ]),
      option2: new FormControl(this.linkedinpollpost.option2, [
        Validators.required,
        this.noWhitespaceValidator(),
        Validators.pattern(/^(?!\s*$).+/)

      ]),
      option3: new FormControl(this.linkedinpollpost.option3, [
        Validators.pattern(/^(?!\s*$).+/)

      ]),
      option4: new FormControl(this.linkedinpollpost.option4, [
        Validators.pattern(/^(?!\s*$).+/)

      ]),
      poll_duration: new FormControl(this.linkedinpollpost.poll_duration, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
    });
  }

  get linkedin_text() {
    return this.Linkedintextpost.get('linkedin_text')!;
  }
  get poll_question() {
    return this.Linkedinpollpost.get('poll_question')!;
  }
   get option1() {
    return this.Linkedinpollpost.get('option1')!;
  } 
  get option2() {
    return this.Linkedinpollpost.get('option2')!;
  } 
  get poll_duration() {
    return this.Linkedinpollpost.get('poll_duration')!;
  }
  noWhitespaceValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isWhitespace = (control.value || '').trim().length === 0;
      return isWhitespace ? { whitespace: true } : null;
    };
  }
  addoption() {
    if (!this.showOption3) {
      this.showOption3 = true;
    } else if (!this.showOption4) {
      this.showOption4 = true;
    }
  }
  removeOption(optionNumber: number) {
    if (optionNumber === 3) {
      this.showOption3 = false;
      (document.getElementById('myInput3') as HTMLInputElement).value = ''; // Clear input field
    } else if (optionNumber === 4) {
      this.showOption4 = false;
      (document.getElementById('myInput4') as HTMLInputElement).value = ''; // Clear input field
    }
  }
  summaryrefresh() {
    this.NgxSpinnerService.show();
    var url1 = 'Linkedin/Getlinkedinaccountdetails'
    this.service.get(url1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      window.location.reload();

    });

  }
  Getaccountdetailssummary() {
    this.NgxSpinnerService.show();
    var url = 'Linkedin/Getlinkedinsummary'
    this.service.get(url).subscribe((result: any) => {
      // window.location.reload()
      this.responsedata = result;
      this.accountsummary_list = this.responsedata.accountsummary_list;
      setTimeout(() => {
        $('#accountsummary_list').DataTable();
      }, 100);
      this.NgxSpinnerService.hide();

    });
  }
  viewaccount(account_id: any) {
    var url = 'Linkedin/Getlinkedinaccountview'
    let param = {
      account_id: account_id
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.account_id = result.account_id;
      this.company_name = result.company_name;
      this.company_website = result.company_website;
      this.founded_on = result.founded_on;
      this.followers_count = result.followers_count;
      this.description = result.description;
      this.organizationStatus = result.organizationStatus;
      this.organizationType = result.organizationType;
      this.localizedSpecialties = result.localizedSpecialties;
      this.accountview_list = result.accountview_list;
    });
  }
  showTab(tab: string) {
    this.currentTab = tab;
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  onclose() {

  }
  postimage(account_id: any) {
    this.parameterValue1 = account_id
  }
  public onlinkedintextpost(): void {
    this.NgxSpinnerService.show();
    var url = 'Linkedin/Linkedintextpost'
    let params = {
      linkedin_text: this.Linkedintextpost.value.linkedin_text,
      account_id: this.parameterValue1
    }
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.Linkedintextpost.reset();
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.Linkedintextpost.reset();
      }
    });
  }
  public onlinkedinmediapost(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("linkedin_text", this.Linkedinmediapost.value.linkedin_text);
      formData.append("account_id", this.parameterValue1);
      this.NgxSpinnerService.show();
      var api = 'Linkedin/Linkedinmediapost'
      this.service.postfile(api, formData).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.Linkedinmediapost.reset();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.Linkedinmediapost.reset();

        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image/Video !!")


    }

  }
  public onlinkedinpollpost(): void {
    this.NgxSpinnerService.show();
    var url = 'Linkedin/Linkedinpollpost'
    let params = {
      linkedin_text: this.Linkedinpollpost.value.linkedin_text,
      poll_question: this.Linkedinpollpost.value.poll_question,
      option1: this.Linkedinpollpost.value.option1,
      option2: this.Linkedinpollpost.value.option2,
      option3: this.Linkedinpollpost.value.option3,
      option4: this.Linkedinpollpost.value.option4,
      poll_duration: this.Linkedinpollpost.value.poll_duration,
      account_id: this.parameterValue1

    }
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message);
        this.Linkedinpollpost.reset();
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.Linkedinpollpost.reset();
      }
    });
  }
  postview(account_id: any): void {
    const secretKey = 'storyboarderp';
    account_id = AES.encrypt(account_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmLinkedinpost', account_id]);
  }
  toggleOptions(account_id: any) {
    if (this.showOptionsDivId === account_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_id;
    }
  }
}