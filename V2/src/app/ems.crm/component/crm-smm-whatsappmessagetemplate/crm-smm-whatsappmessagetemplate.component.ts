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


interface IWhatsappMessage {
  platform: string;
  localae: string;
  template_name: string;
  category: string;
  category_change: string;
  message_type: string;
  name: string;
  type: string;
  description: string;
  body: string;
  footer: string;
  project_id: string;
  value1: string;
  id: string;
  template_id: string;
  p_type: string;
  p_name: string;
  template_body: string;
  whatsappTemplateName: string;
  test:any;

}

@Component({
  selector: 'app-crm-smm-whatsappmessagetemplate',
  templateUrl: './crm-smm-whatsappmessagetemplate.component.html',
  styleUrls: ['./crm-smm-whatsappmessagetemplate.component.scss']
})
export class CrmSmmWhatsappmessagetemplateComponent {

  file!: File;
  image_path: any;
  responsedata: any;
  template_list: any[] = [];
  templateview_list: any[] = [];
  TemplateForm!: FormGroup;
  reactiveForm!: FormGroup;
  reactiveFormadd!: FormGroup;
  reactiveMessageForm!: FormGroup;
  reactiveFormpublish!: FormGroup;
  whatsappmessage!: IWhatsappMessage;
  parameterValue:any;
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  parameterValue4: any;
  parameterValue5: any;
  openDiv: boolean = false;
  filetype: string = "";
  footers: any;
  // config: AngularEditorConfig = {
  //   editable: true,
  //   spellcheck: true,
  //   height: '15rem',
  //   width: '100%',
  //   placeholder: 'Enter text here...',
  //   translate: 'no',
  //   defaultParagraphSeparator: 'p',
  //   defaultFontName: 'Arial',

  // };
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
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private sanitizer: DomSanitizer,
    private route: Router, public service: SocketService) {
    this.whatsappmessage = {} as IWhatsappMessage;
  }

  ngOnInit(): void {


    this.GetMessageTemplatesummary();

    this.reactiveForm = new FormGroup({
      platform: new FormControl(this.whatsappmessage.platform, [
        Validators.required,
      ]),
      localae: new FormControl(this.whatsappmessage.localae, [
      ]),
      template_name: new FormControl(this.whatsappmessage.template_name, [
      ]),
      category: new FormControl(this.whatsappmessage.category, [
      ]),
      // category_change: new FormControl(this.whatsappmessage.category_change, [
      // ]),
      message_type: new FormControl(this.whatsappmessage.message_type, [
      ]),
      type: new FormControl(this.whatsappmessage.type, [
      ]),
     
      project_id: new FormControl(this.whatsappmessage.project_id, [
      ]),
      template_id: new FormControl(this.whatsappmessage.template_id, [
      ]),
      p_type: new FormControl(this.whatsappmessage.p_type, [
      ]),
      p_name: new FormControl(this.whatsappmessage.p_name, [
      ]),
      template_body: new FormControl(this.whatsappmessage.template_body, [
      ]),
      whatsappTemplateName: new FormControl(this.whatsappmessage.whatsappTemplateName, [
      ]),
      body: new FormControl(this.whatsappmessage.body, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      footer: new FormControl(this.whatsappmessage.footer, [
        Validators.required,
      ]),
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      id: new FormControl(this.whatsappmessage.id, [
      ]),
    });


    this.TemplateForm = new FormGroup({
      name: new FormControl(this.whatsappmessage.name, [
        this.noWhitespaceValidator(),
      ]),
      description: new FormControl(this.whatsappmessage.description, [
        this.noWhitespaceValidator(),
      ]),
    });
  }
  test="www.vcidex.com"
  get platform() {
    return this.reactiveForm.get('platform')!;
  }
  get localae() {
    return this.reactiveForm.get('localae')!;
  }
  get template_name() {
    return this.reactiveForm.get('template_name')!;
  }
  get category() {
    return this.reactiveForm.get('category')!;
  }
  // get value() {
  //   return this.reactiveForm.get('value1')!;
  // }
  // get category_change() {
  //   return this.reactiveForm.get('category_change')!;
  // }
  get message_type() {
    return this.reactiveForm.get('message_type')!;
  }
  get project_type() {
    return this.reactiveForm.get('type')!;
  }
  get project_name() {
    return this.reactiveForm.get('name')!;
  }
  get project_description() {
    return this.reactiveForm.get('description')!;
  }
  get body() {
    return this.reactiveForm.get('body')!;
  }
  // get footer() {
  //   return this.reactiveForm.get('footer')!;
  // }
  // get image() {
  //   return this.reactiveForm.get('image')!;
  // }
  get template_id() {
    return this.reactiveForm.get('template_id')!;
  }
  get name() {
    return this.TemplateForm.get('name')!;

  }
  get description() {
    return this.TemplateForm.get('description')!;
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  onclose() {
    this.reactiveMessageForm.reset();
  }

  onprojectcreate() {
    this.whatsappmessage = this.TemplateForm.value;
    if (this.whatsappmessage.name != null
      && this.whatsappmessage.description != null) {
      console.log(this.TemplateForm.value)
      //  this.leadbank.region_name != null,this.leadbank.country_name != null  &&
      var api = 'Whatsapp/CreateProject';
      this.service.post(api, this.whatsappmessage).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
        }
        else {
          // this.GetleadbranchaddSummary(this.leadbank_gid);
          // this.route.navigate(['/crm/CrmTrnLeadBankbranch',this.leadbank_gid]);
          window.location.reload()
          this.ToastrService.success(result.message)
        }
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  //// Summary Grid//////
  GetMessageTemplatesummary() {
    var url = 'Whatsapp/GetMessageTemplatesummary'
    this.service.get(url).subscribe((result: any) => {
      $('#template_list').DataTable().destroy();
      this.responsedata = result;
      this.template_list = this.responsedata.whatsappMessagetemplatelist;
      //console.log(this.source_list)
      setTimeout(() => {
        $('#template_list').DataTable();
      }, 1);


    });
  }
  attachments() {
    this.openDiv = !this.openDiv;
  }
  setFileType(data: string) {
    this.filetype = data;
  }

  openModaledit(parameter: string, p_name: string,template_body:string) {
    this.parameterValue1 = parameter,
    this.parameterValue3 = p_name.toLowerCase().replace(/[^a-zA-Z0-9]/g, '');
    this.parameterValue5 = template_body
    this.reactiveForm.get("project_id")?.setValue(this.parameterValue1);
    this.reactiveForm.get("p_name")?.setValue(this.parameterValue3);
    this.reactiveForm.get("body")?.setValue(this.parameterValue5);
}
  openModalpublish(id: string, template_id: string, p_name: string, p_type: string  ) {
    this.parameterValue1 = id,
      this.parameterValue2 = template_id,
      this.parameterValue3 = p_name,
      this.parameterValue4 = p_type
   
    this.reactiveForm.get("project_id")?.setValue(this.parameterValue1);
    this.reactiveForm.get("template_id")?.setValue(this.parameterValue2);
    this.reactiveForm.get("p_name")?.setValue(this.parameterValue3);
    this.reactiveForm.get("p_type")?.setValue(this.parameterValue4);

  }
  convertHtmlToPlainText(html: string): string {
    const tempElement = document.createElement('div');
    tempElement.innerHTML = html;
    // Use a DOMSanitizer to make sure the content is safe
    const sanitizedContent: SafeHtml = this.sanitizer.bypassSecurityTrustHtml(tempElement.innerText || tempElement.textContent || ''); 
    // Extract plain text from the sanitized content
    return sanitizedContent.toString();
  }
  onsubmit() {
    let formData = new FormData();
    this.whatsappmessage = this.reactiveForm.value;
    this.plainTextBody = this.convertHtmlToPlainText(this.whatsappmessage.body);
    const removeSafeValue = 'SafeValue must use \\[property\\]=binding:';
    const removeXSS = '\\(see https://g.co/ng/security#xss\\)';
    const regexSafeValue = new RegExp(removeSafeValue, 'g');
    const regexXSS = new RegExp(removeXSS, 'g');
    this.text = this.plainTextBody.replace(regexSafeValue, '').replace(regexXSS, '');
  
    // Replace various line break formats (Windows, Unix, Mac)
    this.text1 = this.text.replace(/(\r\n|\r|\n)/g, '');
  
    if (this.whatsappmessage.body != null) {
      if (this.file != null) {
      formData.append("file", this.file , this.file.name);
      formData.append("file_type", this.filetype);
      formData.append("body", this.text1);
      formData.append("p_name", this.reactiveForm.value.p_name);
      formData.append("project_id", this.reactiveForm.value.project_id);
      formData.append("footer", this.reactiveForm.value.footer);
      console.log(this.reactiveForm.value);
  
      var api = 'Whatsapp/CreateTemplate';
      this.service.post(api, formData).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          window.location.reload();
          this.ToastrService.warning(result.message);
        } else {
          window.location.reload();
          this.ToastrService.success(result.message);
        }
      });
    }
    else{
      var param={
        body : this.text1,
        whatsapptemplate : this.reactiveForm.value
      }
      var api = 'Whatsapp/Createtexttemplate';
      this.service.postparams(api, param).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          window.location.reload();
          this.ToastrService.warning(result.message);
        } else {
          window.location.reload();
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
  
  
  onpublish() {
    this.whatsappmessage = this.reactiveForm.value;
    if (this.whatsappmessage.p_name != null
      && this.whatsappmessage.p_type != null) {
      console.log(this.reactiveForm.value)
      var api = 'Whatsapp/PublishTemplate';
      this.service.post(api, this.whatsappmessage).subscribe((result: any) => {
        console.log(result);
        if (result.status == false) {
          window.location.reload()
          this.ToastrService.warning(result.message)
        }
        else {
         window.location.reload()
          this.ToastrService.success(result.message)
        }
      });
    }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {

    console.log(this.parameterValue);
    var url = 'Whatsapp/DeleteCampaign'
    let param = {
      project_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
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
        this.ToastrService.success(result.message)
        this.GetMessageTemplatesummary();
      }
    });
  }
  
  GetTemplateview(project_id: any) {
    
    var url = 'Whatsapp/GetMessageTemplateview'
    let param = {
      project_id: project_id
    }
 
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#templateview_list').DataTable().destroy();
   
      this.responsedata = result;
      this.templateview_list = this.responsedata.whatsappMessagetemplatelist;
      this.footers = this.responsedata.whatsappMessagetemplatelist[0].footer;
      //console.log(this.source_list)


    });
  }
  noWhitespaceValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isWhitespace = (control.value || '').trim().length === 0;
      return isWhitespace ? { whitespace: true } : null;
    };
  }
  
  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  
}