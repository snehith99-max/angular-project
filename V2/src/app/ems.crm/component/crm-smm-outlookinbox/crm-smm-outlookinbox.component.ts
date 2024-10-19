import { Component, OnInit, ElementRef, ViewChild,HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { DomSanitizer, SafeResourceUrl, SafeUrl, SafeHtml } from '@angular/platform-browser';
import { __values } from 'tslib';
interface Imailform {

  leadbank_gid: string;
  from_mail: string;
  mail_sub: string;
  mail_body: string;
  to_mail: any;
}
@Component({
  selector: 'app-crm-smm-outlookinbox',
  templateUrl: './crm-smm-outlookinbox.component.html',
  styleUrls: ['./crm-smm-outlookinbox.component.scss']
})
export class CrmSmmOutlookinboxComponent {
  response_data: any;
  outlooksentMail_list: any;
  outlookreactiveForm!: FormGroup;
  from_mailaddress: any;
  isReadOnly: boolean = true;
  ComposeOutlookMail_list: any;
  mailfiles!: FileList;
  formDataObject: FormData = new FormData();
  base64EncodedText: any;
  mailform!: Imailform;
  
  AutoIDkey: any;  
  file_name: any;
  allattchement: any[]=[];
  parametervalue: any;
  Receiver_mailid: any;
  body: any;
  template_subject: any;
  template_body: any;
  sent_date: any;
  showOptionsDivId: any;
  responsedata: any;
  contacts: any[] = [];
  suggestedContacts: any[] = [];
  suggestedccContacts: any[] = [];
  suggested_bcc_Contacts: any[] = [];
  toField: string = '';
  toField_cc: string = '';
  toField_bcc: string = '';
  selectedContacts: { name: string, email: string,leadbank_gid: string }[] = [];
  selectedContacts_cc: { name: string, email: string,leadbank_gid: string }[] = [];
  selectedContacts_bcc: { name: string, email: string,leadbank_gid: string }[] = [];
  showBccField: boolean = false;
  showCcField: boolean = false;
  Sender_mailid: any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer) {
    this.mailform = {} as Imailform;
  }
  ngOnInit(): void {
    this.GetMailSummary();
    this.GetOutlookMail();
    this.GetleadSummary();
    this.outlookreactiveForm = new FormGroup({


      from_mail: new FormControl(''),
      mail_sub: new FormControl(''),
      to_mail: new FormControl(''),
      cc_mail: new FormControl(''),
      bcc_mail: new FormControl(''),
      mail_body: new FormControl(''),
      file: new FormControl(''),
      leadbank_gid: new FormControl(''),
      tomailaddress_list: new FormControl(''),
      cc_mailids: new FormControl(''),
      bcc_mailids: new FormControl(''),
      // base64EncodedText: new FormControl(''),
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetMailSummary() {
    // this.NgxSpinnerService.show();
    var api = 'OutlookCampaign/OutlookMailSentSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.outlooksentMail_list = this.response_data.outlooksentMail_list;
      setTimeout(() => {
        $('#outlooksentMail_list').DataTable();
      }, 1);
    });

  }
  openModaledit(params: any) {

  }
  GetMailView(params: any, params1: any) {

  }
  public onaddmail(): void {
    this.mailform = this.outlookreactiveForm.value;
    if (this.mailfiles == null) {
      // const originalText =
      //   'From: ' + this.mailform.from_mail + '\r\nTo: ' + this.mailform.to_mail + '\r\nSubject:' + this.mailform.mail_sub + '\r\nContent-Type: text/html\r\n\r\n' + this.mailform.mail_body + '';
      // this.base64EncodedText = encodeToBase64(originalText);
      // this.outlookreactiveForm.patchValue({
      //   base64EncodedText: this.base64EncodedText,
      // });
      //console.log('Base64 encoded text:',this.base64EncodedText);
      this.NgxSpinnerService.show();
      var api7 = 'OutlookCampaign/SendOutlookMail';
      const emailAddresses = this.selectedContacts.map(contact => contact.email);
      const cc_mailaddress = this.selectedContacts_cc.map(contact => contact.email);
      const bcc_mailaddress = this.selectedContacts_bcc.map(contact => contact.email);
      const tomailaddress_list = emailAddresses.join(',');
      const cc_mailids = cc_mailaddress.join(',');
      const bcc_mailids = bcc_mailaddress.join(',');
      this.outlookreactiveForm.patchValue({
        to_mail: tomailaddress_list,
        cc_mail: cc_mailids,
        bcc_mail: bcc_mailids
      });
      this.service.post(api7, this.outlookreactiveForm.value).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.outlookreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
          this.GetOutlookMail();
          this.selectedContacts=[];
          this.selectedContacts_cc=[];
          this.selectedContacts_bcc=[];
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.outlookreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
          this.GetOutlookMail();
          this.selectedContacts=[];
          this.selectedContacts_cc=[];
          this.selectedContacts_bcc=[];
        }
      });
    }
    else if (this.mailform.mail_sub != null && this.mailform.to_mail != null) {
      const allattchement = "" + JSON.stringify(this.allattchement) + "";
      const emailAddresses = this.selectedContacts.map(contact => contact.email);
      const cc_mailaddress = this.selectedContacts_cc.map(contact => contact.email);
      const bcc_mailaddress = this.selectedContacts_bcc.map(contact => contact.email);
      const tomailaddress_list = emailAddresses.join(',');
      const cc_mailids = cc_mailaddress.join(',');
      const bcc_mailids = bcc_mailaddress.join(',');
      this.formDataObject.append("from_mail", this.mailform.from_mail);
      this.formDataObject.append("mail_sub", this.mailform.mail_sub);
      this.formDataObject.append("to_mail", tomailaddress_list);
      this.formDataObject.append("cc_mail", cc_mailids);
      this.formDataObject.append("bcc_mail", bcc_mailids);
      this.formDataObject.append("mail_body", this.mailform.mail_body);
      this.formDataObject.append("mailfiles",allattchement);

      this.NgxSpinnerService.show();

      var api7 = 'OutlookCampaign/SendOutlookMailwithfiles'
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.outlookreactiveForm.reset();
          
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
          this.GetOutlookMail();
          window.location.reload();
          this.selectedContacts=[];
          this.selectedContacts_cc=[];
          this.selectedContacts_bcc=[];

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.outlookreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
          this.GetOutlookMail();
          window.location.reload();
          this.selectedContacts=[];
          this.selectedContacts_cc=[];
          this.selectedContacts_bcc=[];
          
        }
      });
      this.NgxSpinnerService.hide();


      return;

    }
  }
  config_compose_mail: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '120px',
    minHeight: '0rem',
    width: '1013px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'insertImage',
        'insertVideo',
        'removeFormat',
        'toggleEditorMode'
      ]
    ]

  };
  ongamilChange1($event: any): void {

     this.mailfiles = $event.target.files;
    // let reader = new FileReader();
    // reader.onload = (e) => {
    //   if (e.target && e.target.result) { 
    //     let base64String = e.target.result as string;
    //     this.formDataObject.append("mailfiles", base64String);
    //   }
    // };
    // reader.readAsDataURL(this.mailfiles);
    if (this.mailfiles != null && this.mailfiles.length !== 0) {
      for (let i = 0; i < this.mailfiles.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.mailfiles[i]);
        this.file_name = this.mailfiles[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.mailfiles[i].name
        });
        console.log(this.mailfiles[i].name);
      }}
  }
  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }
  onclose(){
    
    this.outlookreactiveForm.get("mail_sub")?.setValue('');
    this.outlookreactiveForm.get("to_mail")?.setValue('');
    this.outlookreactiveForm.get("file")?.setValue('');
    this.outlookreactiveForm.get("mail_body")?.setValue('');
    this.selectedContacts=[];
    this.selectedContacts_cc=[];
    this.selectedContacts_bcc=[];
    window.location.reload();

  }
  onback() {
    this.router.navigate(['/crm/CrmSmmOutlookcampaignsummary']);
  }
  GetOutlookMail(){
    var api = 'OutlookCampaign/ComposeOutlookMail'
    this.service.get(api).subscribe((result: any) =>{
      this.response_data = result;
      this.ComposeOutlookMail_list = this.response_data.ComposeOutlookMail_list
      this.outlookreactiveForm.get("from_mail")?.setValue(this.ComposeOutlookMail_list[0].employee_emailid)
    });
  }
  GetTemplateView(parameter: any){
    this.parametervalue = parameter
    this.Sender_mailid = this.parametervalue.from_mailaddress
    this.Receiver_mailid = this.parametervalue.to_mailaddress
    this.body = this.parametervalue.mail_body;
    this.template_subject = this.parametervalue.mail_subject;
    this.sent_date = this.parametervalue.sent_date;
    const unsafeHtml = this.parametervalue.mail_body;
    this.template_body = this.sanitizer.bypassSecurityTrustHtml(unsafeHtml);
  }
  toggleOptions(mail_gid: any) {
    if (this.showOptionsDivId === mail_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = mail_gid;
    }
  }
  toggleBccField() {
    this.showBccField = !this.showBccField;
  }

  toggleCcField() {
    this.showCcField = !this.showCcField;
  }
  GetleadSummary() {
    var api3 = 'OutlookCampaign/RecipientMailList'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.contacts = this.responsedata.RecipientMailList_list;
    });

  }
  onSearch(query: string) {
    query = query.toLowerCase();
    if (query) {
      this.suggestedContacts = this.contacts.filter(contact =>
        contact.name.toLowerCase().includes(query) || contact.email.toLowerCase().includes(query)
      );
      if (this.suggestedContacts.length === 0) {
        // If no suggestions, add the new contact to the suggested list
        const newContact = this.addNewContact(query);
        this.suggestedContacts.push(newContact);
      }
    } else {
      this.suggestedContacts = [];
    }
  }
  onSearch_cc(query: string) {
    query = query.toLowerCase();
    if (query) {
      this.suggestedccContacts = this.contacts.filter(contact =>
        contact.name.toLowerCase().includes(query) || contact.email.toLowerCase().includes(query)
      );
      if (this.suggestedccContacts.length === 0) {
        // If no suggestions, add the new contact to the suggested list
        const newContact = this.addNewContact(query);
        this.suggestedccContacts.push(newContact);
      }
    } else {
      this.suggestedccContacts = [];
    }
  }
  
  onSearch_bcc(query: string) {
    query = query.toLowerCase();
    if (query) {
      this.suggested_bcc_Contacts = this.contacts.filter(contact =>
        contact.name.toLowerCase().includes(query) || contact.email.toLowerCase().includes(query)
      );
      if (this.suggested_bcc_Contacts.length === 0) {
        // If no suggestions, add the new contact to the suggested list
        const newContact = this.addNewContact(query);
        this.suggested_bcc_Contacts.push(newContact);
      }
    } else {
      this.suggested_bcc_Contacts = [];
    }
  }
  selectContact(contact: any, inputElement: HTMLInputElement) {
    if (!this.validateEmail(contact.email)) {
      this.ToastrService.warning('Invalid email address');
      return;
    }
    let existingContact = this.contacts.find(c => c.email === contact.email);
    if (!existingContact) {
      contact = this.addNewContact(contact.name);
    }
    this.selectedContacts.push(contact);
    this.toField = '';
    inputElement.value = '';
    this.suggestedContacts = [];
  }
  
  selectccContact(contact: any, inputElement: HTMLInputElement) {
    if (!this.validateEmail(contact.email)) {
      this.ToastrService.warning('Invalid email address');
      return;
    }
    let existingContact = this.contacts.find(c => c.email === contact.email);
    if (!existingContact) {
      contact = this.addNewContact(contact.name);
    }
    this.selectedContacts_cc.push(contact);
    this.toField_cc = '';
    inputElement.value = '';
    this.suggestedccContacts = [];
  }
  
  select_bcc_Contact(contact: any, inputElement: HTMLInputElement) {
    if (!this.validateEmail(contact.email)) {
      this.ToastrService.warning('Invalid email address');
      return;
    }
    let existingContact = this.contacts.find(c => c.email === contact.email);
    if (!existingContact) {
      contact = this.addNewContact(contact.name);
    }
    this.selectedContacts_bcc.push(contact);
    this.toField_bcc = '';
    inputElement.value = '';
    this.suggested_bcc_Contacts = [];
  }
  validateEmail(email: string): boolean {
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    return emailPattern.test(email);
  }
  addNewContact(query: string) {
    const newContact = {
      name: query.split('@')[0], // Use the part before '@' as the name
      email: query,
      leadbank_gid: '' // Assign a default or generate a unique ID
    };
    this.contacts.push(newContact);
    return newContact;
  }
  removeContact(index: number) {
    this.selectedContacts.splice(index, 1);
  }
  removeContact_cc(index: number) {
    this.selectedContacts_cc.splice(index, 1);
  }
  removeContact_bcc(index: number) {
    this.selectedContacts_bcc.splice(index, 1);
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    // Check if the clicked element is not the button or notification area
    if (!event.target || !(event.target as HTMLElement).closest('#notification') && !(event.target as HTMLElement).closest('.sampel')) {
      // Toggle the notification off
      this.suggestedContacts = [];
      this.suggestedccContacts = [];
      this.suggested_bcc_Contacts = [];
    }
  }
}
