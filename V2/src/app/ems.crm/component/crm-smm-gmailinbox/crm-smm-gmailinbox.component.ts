import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';

interface IMailform {
  leadbank_gid: string;
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  displayName: any;
  lastname_edit: any;
  name: any;
  email_address: any;
  customer_type: any;
  customertype_edit: any;
  to_mail: any;

}
interface IGmailform {

  leadbank_gid: string;
  gmail_mail_from: string;
  gmail_sub: string;
  //  gmail_to: string;
  gmail_body: string;

  gmail_to_mail: any;
}
@Component({
  selector: 'app-crm-smm-gmailinbox',
  templateUrl: './crm-smm-gmailinbox.component.html',
  styleUrls: ['./crm-smm-gmailinbox.component.scss']
})
export class CrmSmmGmailinboxComponent {


  response_data: any;
  mailsummary_list: any;
  responsedata: any;
  formDataObject: FormData = new FormData();
  from_mailaddress: any;
  reactiveFormContactEdit!: FormGroup;
  displayName: any;
  customer_type: any;
  customertype_edit: any;
  customertype_list: any[] = [];
  isReadOnly: boolean = true;
  ///
  gmailfiles!: File;
  base64EncodedText: any;
  gmailform!: IGmailform;
  gamilreactiveForm!: FormGroup;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {

    this.gmailform = {} as IGmailform;

  }
  ngOnInit(): void {
    this.shopifyenquiry();
    this.GetMailSummary();

    this.reactiveFormContactEdit = new FormGroup({
      email_address: new FormControl(''),

      displayName: new FormControl(''),
      customer_type: new FormControl(''),
      customertype_edit: new FormControl(''),

    });
    this.gamilreactiveForm = new FormGroup({


      gmail_sub: new FormControl(this.gmailform.gmail_sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      gmail_body: new FormControl(''),
      leadbank_gid: new FormControl(''),
      gmail_to_mail: new FormControl(''),
      gmail_mail_from: new FormControl(''),
      base64EncodedText: new FormControl(''),
    });
    // //customer type dropdown
    var api3 = 'MailCampaign/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list3;
    });
  }

  GetMailSummary() {
    // this.NgxSpinnerService.show();
    var api = 'GmailCampaign/GmailinboxSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.mailsummary_list = this.response_data.gmailtemplatesendsummary_list;
      this.from_mailaddress = this.response_data.gmail_address;
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);

      this.gamilreactiveForm.patchValue({
        gmail_mail_from: this.response_data.gmail_address || '',
      });
    });

  }
  shopifyenquiry() {
    var url = 'GmailCampaign/shopifyenquiry';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
    });
  }
  
  GetMailView(params: any, params1: any) {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const gmail_gid = AES.encrypt(params, secretKey).toString();
    const leadbank_gid = AES.encrypt(params1, secretKey).toString();

    this.router.navigate(['/crm/CrmSmmGmailview', gmail_gid, leadbank_gid])
  }

  onclose(){
    
    this.gamilreactiveForm.get("gmail_sub")?.setValue('');
    this.gamilreactiveForm.get("gmail_to_mail")?.setValue('');
    this.gamilreactiveForm.get("file")?.setValue('');
    this.gamilreactiveForm.get("gmail_body")?.setValue('');

  }

  ongamilChange1(event: any) {

    this.gmailfiles = event.target.files[0];

    let reader = new FileReader();
    reader.onload = (e) => {
      if (e.target && e.target.result) { // Check if e.target exists
        // Cast e.target.result to string
        let base64String = e.target.result as string;
        // Append the base64 content to formDataObject
        //console.log(base64String)
        this.formDataObject.append("gmailfiles", base64String);
      }
    };
    reader.readAsDataURL(this.gmailfiles);
    //console.log(this.gmailfiles)
  }

  
  get gmail_sub() {
    return this.gamilreactiveForm.get('gmail_sub')!;
  }
  get gmail_body() {
    return this.gamilreactiveForm.get('gmail_body')!;
  }

  public onaddmail(): void {

    // console.log(this.gamilreactiveForm.value)
    this.gmailform = this.gamilreactiveForm.value;
    if (this.gmailfiles == null) {
      const originalText =
        'From: ' + this.gmailform.gmail_mail_from + '\r\nTo: ' + this.gmailform.gmail_to_mail + '\r\nSubject:' + this.gmailform.gmail_sub + '\r\nContent-Type: text/html\r\n\r\n' + this.gmailform.gmail_body + '';
      this.base64EncodedText = encodeToBase64(originalText);
      this.gamilreactiveForm.patchValue({
        base64EncodedText: this.base64EncodedText,
      });
      //console.log('Base64 encoded text:',this.base64EncodedText);
      this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/Gmailtext'
      this.service.post(api7, this.gamilreactiveForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
        }
      });
    }
    else if (this.gmailform.gmail_sub != null && this.gmailform.gmail_to_mail != null) {

      this.formDataObject.append("gmailfiles", this.gmailfiles || null);
      this.formDataObject.append("gmail_mail_from", this.gmailform.gmail_mail_from);
      this.formDataObject.append("gmail_sub", this.gmailform.gmail_sub);
      this.formDataObject.append("gmail_to_mail", this.gmailform.gmail_to_mail);
      this.formDataObject.append("gmail_body", this.gmailform.gmail_body);
      this.formDataObject.append("leadbank_gid", this.gmailform.leadbank_gid);

      this.NgxSpinnerService.show();

      var api7 = 'Leadbank360/Gmailupload'
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.gamilreactiveForm.reset();
          
          this.NgxSpinnerService.hide();
          this.GetMailSummary();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
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

  onback() {
    this.router.navigate(['/crm/CrmSmmGmailcampaignsummary']);
  }

  openModaledit(params: any) {
    this.reactiveFormContactEdit.get("email_address")?.setValue(params);
    this.reactiveFormContactEdit.get("displayName")?.setValue(this.displayName);
    // this.reactiveFormContactEdit.get("lastname_edit")?.setValue(this.lastname_edit);
    this.reactiveFormContactEdit.get("customertype_edit")?.setValue(this.customertype_edit);
    this.reactiveFormContactEdit.get("customer_type")?.setValue(this.customer_type);
  }

  public onupdatecontact(): void {
    if (this.reactiveFormContactEdit.value.email_address != null && this.reactiveFormContactEdit.value.displayName != null && this.reactiveFormContactEdit.value.customer_type != null) {
      for (const control of Object.keys(this.reactiveFormContactEdit.controls)) {
        this.reactiveFormContactEdit.controls[control].markAsTouched();
      }
      this.reactiveFormContactEdit.value;
      var url = 'GmailCampaign/GmailAddaslead'
      this.service.post(url, this.reactiveFormContactEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveFormContactEdit.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.reactiveFormContactEdit.reset();

        }
        this.reactiveFormContactEdit.reset();
        this.GetMailSummary();

      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }



}
function encodeToBase64(text: string): string {
  return btoa(text);
}