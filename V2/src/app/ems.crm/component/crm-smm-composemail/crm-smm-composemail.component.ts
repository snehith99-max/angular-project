import { Component } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
interface IMailform {
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;



}

@Component({
  selector: 'app-crm-smm-composemail',
  templateUrl: './crm-smm-composemail.component.html',
  styleUrls: ['./crm-smm-composemail.component.scss']
})
export class CrmSmmComposemailComponent {
  file!: File;


 
  mailsummary_list: any;
  to_address: any;
  reactiveForm!: FormGroup;
  mailid_list: any[] = [];
  mailform!: IMailform;

  branchList: any[] = [];
  designation_list: any[] = [];
  country_list2: any[] = [];
  response_data: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router,

  ) {
    this.mailform = {} as IMailform;

  }

  ngOnInit(): void {

    this.reactiveForm = new FormGroup({

      mail_from: new FormControl(this.mailform.mail_from, [
        Validators.required,
      ]),
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''),
      reply_to: new FormControl(''),
      schedule_time: new FormControl(''),
      to: new FormControl(this.mailform.to, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')
        // emailValidator(),
      ]),
    })
  }



  onChange1(event: any) {
    this.file = event.target.files[0];

  }
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '120px',
    minHeight: '0rem',
    width: '630px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
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

  };
  isDropdownOpen = false;
  GetMailSummary(deencryptedParam: any) {
    let params = {
      leadbank_gid: deencryptedParam
    }
    var api = 'MailCampaign/GetIndividualMailSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.response_data = result;
      this.mailsummary_list = this.response_data.mailsummary_list;
      this.to_address = this.mailsummary_list[0].to;
      this.reactiveForm.patchValue({
        to: this.mailsummary_list[0]?.to || '',
        leadbank_gid: deencryptedParam,
        // sub: this.mailsummary_list[0]?.sub || '',
        // body: this.mailsummary_list[0]?.body || '',
      });
      setTimeout(() => {
        $('#mailsummary_list').DataTable();
      }, 1);
    });
  }
  sendMessage() {
    // Add your send message logic here
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  get mail_from() {
    return this.reactiveForm.get('mail_from')!;
  }
  get to() {
    return this.reactiveForm.get('to')!;
  }
  get sub() {
    return this.reactiveForm.get('sub')!;
  }
  get reply_to() {
    return this.reactiveForm.get('reply_to')!;
  }
  get cc() {
    return this.reactiveForm.get('cc')!;
  }
  get bcc() {
    return this.reactiveForm.get('bcc')!;
  }
  get schedule_time() {
    return this.reactiveForm.get('schedule_time')!;
  }


  onback() {
    this.route.navigate(['/crm/CrmSmmEmailmanagement'])

  } 
  public onadd(): void {
    console.log(this.reactiveForm.value)

    this.mailform = this.reactiveForm.value;
    // this.service.Profileupload(this.reactiveForm.value).subscribe(result => {  
    //   this.responsedata=result;
    // });   
    if (this.mailform.mail_from != null && this.mailform.sub != null && this.mailform.to != null) {
      //let formData = new FormData();
      // if (this.file != null && this.file != undefined) {



      // formData.append("file", this.file, this.file.name);
      // formData.append("mail_from", this.mailform.mail_from);
      // formData.append("sub", this.mailform.sub);
      // formData.append("to", this.mailform.to);
      // formData.append("body", this.mailform.body);
      // formData.append("bcc", this.mailform.to);
      // formData.append("cc", this.mailform.body);
      // formData.append("reply_to", this.mailform.body);




      //console.log(this.file)
      //   var api7 = 'Mailmanagement/mailmanagementupload'
      //   //console.log(this.file)
      //   this.service.postfile(api7, formData).subscribe((result: any) => {

      //     if (result.status == true) {
      //       this.ToastrService.warning(result.message)
      //     }
      //     else {
      //       window.scrollTo({

      //         top: 0, // Code is used for scroll top after event done

      //       });
      //       this.route.navigate(['/crm/CrmCampaignMailmanagementsummary']);
      //       this.ToastrService.success("Mail sent Successfully")
      //     }
      //   });

      // }
      // else {
      var api7 = 'MailCampaign/MailSend'
      //console.log(this.file)
      this.service.post(api7, this.mailform).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.route.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
        }
        this.response_data = result;
      });
    }

    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

    // console.info('Name:', this.employee);
    return;


  }
  onclose() {
    this.reactiveForm.reset();
    this.reactiveForm.reset();


  }
  onChange2(event: any) {
    this.file = event.target.files[0];
    // var api='Employeelist/EmployeeProfileUpload'
    // //console.log(this.file)
    //   this.service.EmployeeProfileUpload(api,this.file).subscribe((result:any) => {
    //     this.responsedata=result;
    //   });
  }
  public schedulemail(): void {
      console.log(this.reactiveForm.value)
   
      this.mailform = this.reactiveForm.value;
      var api7 = 'MailCampaign/ScheduledMailSend'
      //console.log(this.file)
      this.service.post(api7, this.mailform).subscribe((result: any) => {
   
        if (result.status == false) {
          window.scrollTo({
   
            top: 0, // Code is used for scroll top after event done
   
          });
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
   
            top: 0, // Code is used for scroll top after event done
   
          });
          this.route.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
        }
        this.response_data = result;
      });
    }

}
