import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IMailform {
  sub: string;
  body: string;
  template_name: string;

}
@Component({
  selector: 'app-crm-smm-mailcampaigntemplate',
  templateUrl: './crm-smm-mailcampaigntemplate.component.html',
  styleUrls: ['./crm-smm-mailcampaigntemplate.component.scss']
})
export class CrmSmmMailcampaigntemplateComponent {
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

      ]
    ], sanitize: false,
    

  };

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService) {
    this.mailform = {} as IMailform;

  }

  ngOnInit(): void {

    this.reactiveForm = new FormGroup({
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
        Validators.pattern("^(?!\s*$).+"),

      ]),

      body: new FormControl(this.mailform.body, [
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
  get sub() {
    return this.reactiveForm.get('sub')!;
  }

  public submit(): void {
   
    if (this.reactiveForm.value.template_name != null && this.reactiveForm.value.sub != null && this.reactiveForm.value.body != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      this.NgxSpinnerService.show();
      var url = 'MailCampaign/SaveTemplate'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveForm.get("template_name")?.setValue(null);
          this.reactiveForm.get("body")?.setValue(null);
          this.reactiveForm.get("sub")?.setValue(null);
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveForm.get("template_name")?.setValue(null);
          this.reactiveForm.get("body")?.setValue(null);
          this.reactiveForm.get("sub")?.setValue(null);
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();
          this.route.navigate(['/crm/CrmSmmMailcampaignsummary']);

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
    this.route.navigate(['/crm/CrmSmmMailcampaignsummary']);

  }
}

