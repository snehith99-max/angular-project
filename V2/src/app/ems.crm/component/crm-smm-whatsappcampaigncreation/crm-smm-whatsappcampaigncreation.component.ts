import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators,ValidationErrors,
  AbstractControl,
  ValidatorFn } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { NgxSpinnerService } from 'ngx-spinner';

interface IWhatsappTemplate {
  name: string;
  description: string;
  body: string;
  footer: string;
}
@Component({
  selector: 'app-crm-smm-whatsappcampaigncreation',
  templateUrl: './crm-smm-whatsappcampaigncreation.component.html',
  styleUrls: ['./crm-smm-whatsappcampaigncreation.component.scss']
})
export class CrmSmmWhatsappcampaigncreationComponent {
  reactiveForm!: FormGroup;
  file!: File;
  image_path: any;
  filetype: string = "";
  template_name:any;
  WhatsappTemplate!: IWhatsappTemplate;
  config = {
    uiColor: '#ffffff',
    toolbarGroups: [
      //{ name: 'clipboard', groups: ['clipboard', 'undo'] },
    //{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
    { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
    { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
  //  { name: 'document', groups: ['mode', 'document', 'doctools'] },
    { name: 'styles' },
    { name: 'colors' },
    // { name: 'links' }, 
    { name: 'insert' },],
    skin: 'kama',
    resize_enabled: false,
    removePlugins: 'elementspath,save,magicline',
    extraPlugins: 'divarea,emoji,smiley,justify,indentblock,colordialog',
    // font: 'Arial/Arial, Helvetica, sans-serif; Calibri/Calibri, sans-serif; Times New Roman/Times New Roman, Times, serif; Verdana/Verdana, Geneva, sans-serif; Tahoma/Tahoma, Geneva, sans-serif; Courier New/Courier New, Courier, monospace',
    colorButton_foreStyle: {
       element: 'font',
       attributes: { 'color': '#(color)' }
    },
    height: 188,
    removeDialogTabs: 'image:advanced;link:advanced',
    removeButtons: 'Subscript,Superscript,Anchor,Source,Table',
    format_tags: 'p;h1;h2;h3;pre;div'
 }
  plainTextBody: string = '';
  text:any;
  text1:any;
  showimage:boolean=true;
  showdocument:boolean=false
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private sanitizer: DomSanitizer,
    private route: Router, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
    this.WhatsappTemplate = {} as IWhatsappTemplate;
  }
  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      name: new FormControl(this.WhatsappTemplate.name, [
        Validators.required,
      ]),
      description: new FormControl(this.WhatsappTemplate.description, [
      ]),
      body: new FormControl(this.WhatsappTemplate.body, [
      ]),
      footer: new FormControl(this.WhatsappTemplate.footer, [
      ]),
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
    });
  }
  get name() {
    return this.reactiveForm.get('name')!;
  }
  get description() {
    return this.reactiveForm.get('description')!;
  }
  get body() {
    return this.reactiveForm.get('body')!;
  }
  get footer() {
    return this.reactiveForm.get('footer')!;
  }

  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  
  setFileType(data: string) {
    this.filetype = data;
  }
  convertHtmlToPlainText(html: string): string {
    const tempElement = document.createElement('div');
    tempElement.innerHTML = html;
    // Use a DOMSanitizer to make sure the content is safe
    const sanitizedContent: SafeHtml = this.sanitizer.bypassSecurityTrustHtml(tempElement.innerText || tempElement.textContent || ''); 
    // Extract plain text from the sanitized content
    return sanitizedContent.toString();
  }
  show(){
    this.showimage=true
    this.showdocument=false

  }
  showno(){
    this.showdocument=true
    this.showimage=false

  }
  onsubmit() {
    if (this.reactiveForm.value.name != null) {
    let formData = new FormData();
    this.WhatsappTemplate = this.reactiveForm.value;
    this.plainTextBody = this.convertHtmlToPlainText(this.WhatsappTemplate.body);
    const removeSafeValue = 'SafeValue must use \\[property\\]=binding:';
    const removeXSS = '\\(see https://g.co/ng/security#xss\\)';
    const regexSafeValue = new RegExp(removeSafeValue, 'g');
    const regexXSS = new RegExp(removeXSS, 'g');
    this.text = this.plainTextBody.replace(regexSafeValue, '').replace(regexXSS, '');
    // Replace various line break formats (Windows, Unix, Mac)
    this.text1 = this.text.replace(/(\r\n|\r|\n)/g, '');
    this.template_name = this.WhatsappTemplate.name.toLowerCase().replace(/[^a-zA-Z0-9]/g, '');
  
      if (this.file != null) {
      formData.append("file", this.file , this.file.name);
      formData.append("file_type", this.filetype);
      formData.append("body", this.text1);
      formData.append("template_name", this.template_name);
      formData.append("description", this.reactiveForm.value.description);
      formData.append("p_name", this.reactiveForm.value.name);
      formData.append("footer", this.reactiveForm.value.footer);
      console.log(this.reactiveForm.value);
      this.NgxSpinnerService.show();
      var api = 'Whatsapp/PostTemplateCreation';
      this.service.post(api, formData).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.route.navigate(['/crm/CrmSmmWhatsappcampaign'])
          this.ToastrService.warning(result.message);
        } else {
          this.NgxSpinnerService.hide();
          this.route.navigate(['/crm/CrmSmmWhatsappcampaign'])
          this.ToastrService.success(result.message);
        }
      });
    }
    else{

  let param = {
    body : this.text1,
    template_name : this.template_name,
    description : this.reactiveForm.value.description,
    p_name : this.reactiveForm.value.name,
    footer : this.reactiveForm.value.footer
}
      var api = 'Whatsapp/PostTextTemplateCreation';
      this.service.post(api, param).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          this.route.navigate(['/crm/CrmSmmWhatsappcampaign'])
          this.ToastrService.warning(result.message);
        } else {
          this.route.navigate(['/crm/CrmSmmWhatsappcampaign'])
          this.ToastrService.success(result.message);
        }
      });
    }
    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  
}
