import { Component, OnInit ,HostListener} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES, format } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";
import { saveAs } from 'file-saver';



interface IWhatsapp {
  //sourceedit_name: any;

  created_date: string;
  customer_name: string;
  displayName: string;
  CompanyName:string;
  ContactpersonName:string;
  mobile: string;
  created_by: string;
  computedDisplayName: string;
  email: string;
  first_letter: string;
  key: string;
  value: string;
  firstName: string;
  lastName: string;
  gender: string;
  identifierValue: string;
  type: string;
  sendtext: string;
  phone: string;
  lastname_edit: string;
  firstname_edit: string;
  displayName_edit: string;
  phone_edit: string;
  customer_type: string;
  customertype_edit: string;


}

@Component({
  selector: 'app-crm-smm-watsapp',
  templateUrl: './crm-smm-watsapp.component.html',
  styleUrls: ['./crm-smm-watsapp.component.scss']
})
export class CrmSmmWatsappComponent implements OnInit {
  
  isReadOnly = true;
  reactiveForm!: FormGroup;
  reactiveMessageForm!: FormGroup;
  reactiveFormContactEdit!: FormGroup;
  reactiveTemplateMessageForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  whatsapp_list: any[] = [];
  whatsappmessage_list: any[] = [];
  whatsappMessagetemplatelist: any[] = [];
  file_list: any[] = [];
  leadbank!: IWhatsapp;
  file!: File;
  searchText = '';
  image_list: any[] = [];
  count_list: any[] = [];
  openDiv: boolean = false;
  filetype: string = "";
  chat_gid: string = "";
  name: any;
  identifier: any;
  initial: any;
  OpenOption: boolean = false;
  absURL: any;
  chatWindow: string = "Default";
  separateDialCode = false;
  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];
  parameterValue1: any;
  firstName: any;
  lastName: any;
  leadbank_gid:any;
  windowInterval: any;
  windowInterval1: any;
  contact_count:any
  contact_id: any;
  customertype_list: any[] = [];
  contactcount_list: any;
  customertype_gid: any;
  templateview_list: any;
  footers:any;
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.displayName.toLowerCase().includes(searchString) || item.value.toLowerCase().includes(searchString);
  }

  constructor(private formBuilder: FormBuilder, private route: Router, private router: Router,
    private ToastrService: ToastrService, public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService) {
    this.leadbank = {} as IWhatsapp;
  }
  downloadImage(data: any) {
    if (data.product_image != null && data.product_image != "") {
      saveAs(data.product_image, data.product_gid + '.png');
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }
  }
  ngOnInit(): void {
    var c_code = localStorage.getItem('c_code');
    if (c_code == "boba_tea") {
      this.windowInterval = window.setInterval(() => {
        this.GetWhatsappSummary();
    }, 30000); // 30 seconds
    }
   else if (c_code == "bobatea") {
    this.windowInterval = window.setInterval(() => {
      this.GetWhatsappSummary();
  }, 30000); // 30 seconds
    }
    else if (c_code == "vcidex") {
      this.windowInterval = window.setInterval(() => {
        this.GetWhatsappSummary();
    }, 30000); // 30 seconds
    }
    else if (c_code == "media") {
      this.windowInterval = window.setInterval(() => {
        this.GetWhatsappSummary();
    }, 30000); // 30 seconds
    }
    else if (c_code == "capwing") {
      this.windowInterval = window.setInterval(() => {
        this.GetWhatsappSummary();
    }, 30000); // 30 seconds
    }
    else{
        this.GetWhatsappSummary();
    }
    this.absURL = window.location.origin

    var url5 = 'SocialMedia/GetContactCount'
  this.service.get(url5).subscribe((result,) => {

    this.contactcount_list = result;
    this.contact_count = this.contactcount_list.contactcount_list[0].contact_count1;
  
   

  });
    this.reactiveForm = new FormGroup({
      customer_name: new FormControl(this.leadbank.customer_name, [
        Validators.required,
      ]),
      
      displayName: new FormControl(this.leadbank.displayName, [
        Validators.required,
      ]),
        
      CompanyName: new FormControl(this.leadbank.CompanyName, [
        Validators.required,
      ]),
        
      ContactpersonName: new FormControl(this.leadbank.ContactpersonName, [
        Validators.required,
      ]),
    
      customer_type: new FormControl(this.leadbank.customer_type, [
        Validators.required,
      ]),
      phone: new FormControl(this.leadbank.phone, [
        Validators.required,]),
      mobile: new FormControl(''),
      value: new FormControl(this.leadbank.value, [
        Validators.required,
      ]),
      gender: new FormControl(''),
      firstName: new FormControl(''),
      lastName: new FormControl('')
    });

    this.reactiveMessageForm = new FormGroup({
      identifierValue: new FormControl(''),
      type: new FormControl(''),
      sendtext: new FormControl(''),
      template_name: new FormControl(''),
      p_name: new FormControl(''),    
      message_id: new FormControl(''),
      contact_id: new FormControl(''),
    });
    this.reactiveTemplateMessageForm = new FormGroup({
      project_id: new FormControl(),
      version: new FormControl(''),
      contact_id: new FormControl(''),
      identifierValue: new FormControl(''),
    });

    this.reactiveFormContactEdit = new FormGroup({
      displayName_edit: new FormControl(this.leadbank.displayName_edit, [
        Validators.required,
        Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces
      ]),

      firstname_edit: new FormControl(this.leadbank.firstname_edit, [
        Validators.required,
        Validators.pattern("^(?!\s*$).+") // Allow letters, numbers, and spaces
      ]),

      lastname_edit: new FormControl(this.leadbank.lastname_edit, [
        Validators.maxLength(300),

      ]),
      phone_edit: new FormControl(this.leadbank.phone_edit, [
        Validators.required,]),

        customertype_edit: new FormControl(this.leadbank.customertype_edit, [
          Validators.required,
        ]),
        contact_id: new FormControl(''),

    });

  //customer type dropdown
  var api3 = 'Whatsapp/GetCustomerTypeSummary'
  this.service.get(api3).subscribe((result: any) => {
    this.responsedata = result;
    this.customertype_list = result.customertype_list2;
  });
    //**Template popup**//
    var api2 = 'Whatsapp/GetMessageTemplatesummary'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.whatsappMessagetemplatelist = result.whatsappMessagetemplatelist;
    });
    // this.reactiveFormContactEdit.controls['phone_edit'].disable();
  }
  

  //   GetWhatsappMessageSummary1() {
  //     var url = 'Whatsapp/GetContactConversation'
  //     this.service.get(url).subscribe((result: any) => {
  //       $('#whatsmessagelist').DataTable().destroy();
  //       this.responsedata = result;

  //   });
  // }
  get customer_name() {
    return this.reactiveForm.get('customer_name')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get value() {
    return this.reactiveForm.get('mobile')!;
  }
  // get firstName() {
  //   return this.reactiveForm.get('customer_name')!;
  // }
  // get lastName() {
  //   return this.reactiveForm.get('mobile')!;
  // }
  get displayName() {
    return this.reactiveForm.get('mobile')!;
  }
  get identifierValue() {
    return this.reactiveMessageForm.get('identifierValue')!;
  }
  get template_name() {
    return this.reactiveMessageForm.get('template_name')!;
  }
  get customer_type() {
    return this.reactiveForm.get('customer_type')!;
  }
  get customertype_edit() {
    return this.reactiveFormContactEdit.get('customertype_edit')!;
  }
  get sendtext() {
    return this.reactiveMessageForm.get('sendtext')!;
  }

  //contact summary//
  GetWhatsappSummary() {
    
    var url = 'Whatsapp/GetContact'
    this.service.get(url).subscribe((result: any) => {
      $('#whatsnamelist').DataTable().destroy();
      this.responsedata = result;
      this.whatsapp_list = this.responsedata.whatscontactlist;
      this.contact_id = this.whatsapp_list[0].whatsapp_gid;

    });
  }
  isLeadingSpace: boolean = false;
 
  checkLeadingSpace(event: any) {
    this.isLeadingSpace = event.target.value.trim() === '';
}

  //Message summary//
  GetWhatsappMessageSummary(whatsapp_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'Whatsapp/GetMessage'
    let param = {
      whatsapp_gid: whatsapp_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#whatsmessagelist').DataTable().destroy();
      if (result != null) {
        this.responsedata = result;
        this.whatsappmessage_list = this.responsedata.whatsmessagelist;
        this.name = this.whatsappmessage_list[0].displayName;
        this.initial = this.whatsappmessage_list[0].first_letter;
        this.identifier = this.whatsappmessage_list[0].identifierValue;
        this.firstName = this.whatsappmessage_list[0].firstName;
        this.lastName = this.whatsappmessage_list[0].lastName;
        this.customertype_gid = this.whatsappmessage_list[0].customertype_gid;
        this.contact_id = this.whatsappmessage_list[0].contact_id;
        this.leadbank_gid = this.whatsappmessage_list[0].leadbank_gid;


      }
      else {
        clearInterval(this.windowInterval1)
      }
    });
  }


  //Create contact//
  public onsubmit(): void {
    if (this.reactiveForm.value.CompanyName != null && 
      this.reactiveForm.value.phone.e164Number != null && 
      this.reactiveForm.value.customer_type != null) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'Whatsapp/CreateContact'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.reactiveForm.get("displayName")?.setValue(null);
          this.reactiveForm.get("phone")?.setValue(null);
          this.ToastrService.warning(result.message)
          this.GetWhatsappSummary();
          this.reactiveForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.reactiveForm.get("displayName")?.setValue(null);
          this.reactiveForm.get("phone")?.setValue(null);
          this.ToastrService.success(result.message)
          this.GetWhatsappSummary();
          this.reactiveForm.reset();
        }
        this.GetWhatsappSummary();
        this.reactiveForm.reset();
      });
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  openModaledit() {
    this.reactiveFormContactEdit.get("displayName_edit")?.setValue(this.name);
    this.reactiveFormContactEdit.get("phone_edit")?.setValue(this.identifier);
    // this.reactiveFormContactEdit.get("firstname_edit")?.setValue(this.firstName);
    // this.reactiveFormContactEdit.get("lastname_edit")?.setValue(this.lastName);
    // this.reactiveFormContactEdit.get("customertype_edit")?.setValue(this.customertype_gid);
    // this.reactiveFormContactEdit.get("contact_id")?.setValue(this.contact_id);

  }
  public onupdatecontact(): void {
    if (this.reactiveFormContactEdit.value.displayName_edit != null) {
      for (const control of Object.keys(this.reactiveFormContactEdit.controls)) {
        this.reactiveFormContactEdit.controls[control].markAsTouched();
      }
      this.reactiveFormContactEdit.value;
      var url = 'Whatsapp/updatewhatsappcontact'
      this.service.post(url, this.reactiveFormContactEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
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
          this.reactiveFormContactEdit.reset();

        }
        this.reactiveFormContactEdit.reset();
        this.GetWhatsappMessageSummary(this.chat_gid);

      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  public onaddlead(): void {
    if (this.reactiveFormContactEdit.value.displayName_edit != null && this.reactiveFormContactEdit.value.customertype_edit != null) {
      for (const control of Object.keys(this.reactiveFormContactEdit.controls)) {
        this.reactiveFormContactEdit.controls[control].markAsTouched();
      }
      this.reactiveFormContactEdit.value;
      var url = 'Whatsapp/UpdateContact'
      this.service.post(url, this.reactiveFormContactEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
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
          this.reactiveFormContactEdit.reset();

        }
        this.reactiveFormContactEdit.reset();
        this.GetWhatsappMessageSummary(this.chat_gid);

      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  onclose() {
    this.reactiveForm.reset();
  }

  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  //Template popup passing identifier value//
  poptemplatemodal(parameter: string) {
    this.reactiveMessageForm.get("identifierValue")?.setValue(parameter);
  }

  //contact to message//
  showResponsiveOutput(gid: string) {
    this.NgxSpinnerService.show();
    this.chat_gid = gid;
    this.chatWindow = "Chat"

    this.GetWhatsappSummary();
    this.GetWhatsappMessageSummary(gid);
    this.GetWhatsappSummary();
    this.NgxSpinnerService.hide();

    // var c_code = localStorage.getItem('c_code');
    // if (c_code == "boba") {
    //   this.windowInterval = window.setInterval(() => {
    //     if (this.chat_gid != null)
    //       this.GetWhatsappMessageSummary(this.chat_gid);
    //     else
    //       clearInterval(this.windowInterval)
    //   }, 1000);
    // }
  }

  i: number = 0;

  onSubmit() {
    if (this.i === 0) {
      this.onMessagesent(this.identifier, this.contact_id);
    }
  }

  // Message send //
  public onMessagesent(gid: string, id: string): void {
    this.reactiveMessageForm.value.identifierValue = gid;
    this.reactiveMessageForm.value.contact_id = id;
    var sendtexts = this.reactiveMessageForm.value.sendtext; 
    if (sendtexts != null && sendtexts != '') {

      var url = 'Whatsapp/WhatsappSend'
      this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
          this.reactiveMessageForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.reactiveMessageForm.reset();
        }
        this.GetWhatsappMessageSummary(this.chat_gid);
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Please Type a Message to Continue')
    }
  }

  // Template sent //
  public onTemplatesent(id: string, version: string): void {

    this.reactiveTemplateMessageForm.get("project_id")?.setValue(id);
    this.reactiveTemplateMessageForm.get("version")?.setValue(version);
    let identifierValue = this.reactiveMessageForm.value.identifierValue;
    let project_id = id;
    this.reactiveTemplateMessageForm.value.identifierValue = identifierValue;
    let param = {
      identifierValue: identifierValue,
      project_id: project_id,
    }

    if (project_id != null) {
      this.reactiveTemplateMessageForm.value.param = param

      var url = 'Whatsapp/WhatsappSend'
      this.service.post(url, this.reactiveTemplateMessageForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
        }
        this.GetWhatsappMessageSummary(this.chat_gid);
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  attachments() {
    this.openDiv = !this.openDiv;
  }

  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  downloadFile(file_path: string, file_name: string): void {
    var params = {
      file_path: file_path,
      file_name: file_name
    }

    this.service.downloadFile(params).subscribe((data: any) => {
      if (data != null) {
        this.service.filedownload1(data);
      }
      else {
        // this.ToastrService.warning("Error in file download")  
      }
    });
  }
  public onup(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
    }
    else {
      this.ToastrService.warning('Kindly select atleast one file!')
    }
  }

  public onupload(): void {
    this.attachments();
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("file_type", this.filetype)
      formData.append("contact_id", this.chat_gid)
      this.NgxSpinnerService.show();
      var url = 'Whatsapp/waSendDocuments'
      this.service.post(url, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappMessageSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)

        }
        this.GetWhatsappMessageSummary(this.chat_gid);
      });
    }

    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  setFileType(data: string) {
    this.filetype = data;
  }

  onClickOption() {
    this.OpenOption = !this.OpenOption;
  }

  getDocument(gid: string) {
    this.chatWindow = "Files"
    var url = '/Whatsapp/waGetDocuments'
    var params = {
      contact_id: gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.image_list = result.wa_images_list
      this.file_list = result.wa_files_list
    });
  }

  backtochat() {
    this.chatWindow = "Chat"
  }

  ngOnDestroy(): void {
    clearInterval(this.windowInterval)
    clearInterval(this.windowInterval1)
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    // Check if the clicked element is not the button or notification area
    if (!event.target || !(event.target as HTMLElement).closest('#notification') && !(event.target as HTMLElement).closest('.sampel')) {
      // Toggle the notification off
      this.openDiv = false;
    }
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
}
