import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IMailform {
  template_subject: string;
  template_body: string;
  template_name: string;

}
@Component({
  selector: 'app-crm-smm-gmailcampaigntemplate',
  templateUrl: './crm-smm-gmailcampaigntemplate.component.html',
  styleUrls: ['./crm-smm-gmailcampaigntemplate.component.scss']
})
export class CrmSmmGmailcampaigntemplateComponent {
  file!: File;
  responsedata: any;
  reactiveForm!: FormGroup;
  mailform!: IMailform;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '20rem',
    width: '100%',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'removeFormat',
        'insertVideo',

      ]
    ], sanitize: false,
    

  };

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
    this.mailform = {} as IMailform;

  }

  ngOnInit(): void {

    this.reactiveForm = new FormGroup({
      template_subject: new FormControl(this.mailform.template_subject, [
        Validators.required,
        Validators.pattern("^(?!\s*$).+"),

      ]),

      template_body: new FormControl(this.mailform.template_body, [
        Validators.required,

      ]),
      template_name: new FormControl(this.mailform.template_name, [
        Validators.required,
        Validators.pattern("^(?!\s*$).+")
      ]),
    })

  }

  get template_name() {
    return this.reactiveForm.get('template_name')!;
  }
  get template_subject() {
    return this.reactiveForm.get('template_subject')!;
  }

  public submit(): void {
    if (this.reactiveForm.value.template_name != null && this.reactiveForm.value.template_subject != null && this.reactiveForm.value.template_body != null) {
      this.reactiveForm.value;
      this.NgxSpinnerService.show();
      var url = 'GmailCampaign/GmailSaveTemplate'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();
          this.route.navigate(['/crm/CrmSmmGmailcampaignsummary']);

        }
        this.reactiveForm.reset();

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    this.NgxSpinnerService.hide();
  }

  onback() {
    this.route.navigate(['/crm/CrmSmmGmailcampaignsummary']);

  }
}
