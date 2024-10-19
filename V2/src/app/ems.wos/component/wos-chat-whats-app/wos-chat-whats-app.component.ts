import { Component, OnInit, HostListener, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES, format } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';

import {
  CountryISO,
  SearchCountryField,

} from "ngx-intl-tel-input";
import { saveAs } from 'file-saver';
import { environment } from 'src/environments/environment';
export class IContact {
  whatsapp_gid: any;
  value: any;
  wosbulktemplatesend: any;

}

interface IWhatsapp {
  //sourceedit_name: any;
  created_date: string;
  customer_name: string;
  displayName: string;
  CompanyName: string;
  ContactpersonName: string;
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
  last_message: any;

}

@Component({
  selector: 'app-wos-chat-whats-app',
  templateUrl: './wos-chat-whats-app.component.html',
  styleUrls: ['./wos-chat-whats-app.component.scss']
})
export class WosChatWhatsAppComponent implements OnInit, OnDestroy {
  CurObj: IContact = new IContact();
  pick: Array<any> = [];
  isReadOnly = true;
  reactiveForm!: FormGroup;
  reactiveMessageForm!: FormGroup;
  reactiveFormContactEdit!: FormGroup;
  reactiveTemplateMessageForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  woscontactsummary_List: any[] = [];
  wostotalcontact_list: any[] = [];
  wosmsgchatsummary_list: any[] = [];
  whatsappMessagetemplatelist: any[] = [];///dummy delete it
  wosmsgtemplate_list: any[] = [];
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
  leadbank_gid: any;
  windowInterval: any;
  windowInterval1: any;
  contact_count: any
  contact_id: any;
  customertype_list: any[] = [];
  contactcount_list: any;
  customertype_gid: any;
  templateview_list: any;
  footers: any;
  selection = new SelectionModel<IContact>(true, []);
  env_UR = environment.URL_FILEPATH;

  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.displayName.toLowerCase().includes(searchString) || item.value.toLowerCase().includes(searchString);
  }

  constructor(private formBuilder: FormBuilder, private route: Router, private router: Router,
    private ToastrService: ToastrService, public service: SocketService,
    private NgxSpinnerService: NgxSpinnerService) {
    this.leadbank = {} as IWhatsapp;
    this.Getwoscontctsummary();

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
    this.windowInterval = window.setInterval(() => {
      this.Getwoscontctsummary();
    }, 25000);


    //this.absURL = window.location.origin
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

  }

  get customer_name() {
    return this.reactiveForm.get('customer_name')!;
  }
  get mobile() {
    return this.reactiveForm.get('mobile')!;
  }
  get value() {
    return this.reactiveForm.get('mobile')!;
  }

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
  Getwoscontctsummary() {

    var url = 'WhatsApporderSummary/Getwoscontctsummary'
    this.service.get(url).subscribe((result: any) => {
      $('#woscontactsummary_List').DataTable().destroy();
      this.responsedata = result;
      this.woscontactsummary_List = this.responsedata.woscontactsummary_List;
      this.contact_count = result.contact_count;

    });
  }

  Getwosmsgtemplatesummary() {

    var url = 'WhatsApporderSummary/Getwosmsgtemplatesummary'
    this.service.get(url).subscribe((result: any) => {
      $('#woscontactsummary_List').DataTable().destroy();
      this.responsedata = result;
      this.wosmsgtemplate_list = this.responsedata.wosmsgtemplate_list;
    });
  }
  isLeadingSpace: boolean = false;

  checkLeadingSpace(event: any) {
    this.isLeadingSpace = event.target.value.trim() === '';
  }

  //Message summary//
  GetWhatsappchatSummary(whatsapp_gid: any) {
    this.NgxSpinnerService.show();
    var url = 'WhatsApporderSummary/GetWhatsappchatSummary'
    let param = {
      whatsapp_gid: whatsapp_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#wosmsgchatsummary_list').DataTable().destroy();
      if (result != null) {
        this.responsedata = result;
        this.wosmsgchatsummary_list = this.responsedata.wosmsgchatsummary_list;
        this.name = result.displayName;
        this.initial = result.first_letter;
        this.identifier = result.identifierValue;
        this.firstName = result.firstName;
        this.lastName = result.lastName;
        this.customertype_gid = result.customertype_gid;
        this.contact_id = result.contact_id;
        this.leadbank_gid = result.leadbank_gid;


      }
      else {
        clearInterval(this.windowInterval1)
      }
    });
  }


  //Create contact//
  public oncontactcreate(): void {
    if (this.reactiveForm.value.CompanyName != null &&
      this.reactiveForm.value.phone.e164Number != null) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'WhatsApporderSummary/woscontactcreate'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.Getwoscontctsummary();
          this.reactiveForm.reset();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveForm.get("displayName")?.setValue(null);
          this.reactiveForm.get("phone")?.setValue(null);
          this.ToastrService.success(result.message)
          this.Getwoscontctsummary();
          this.reactiveForm.reset();
        }
        this.Getwoscontctsummary();
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
      var url = 'WhatsApporderSummary/updatewoscontact'
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
        this.GetWhatsappchatSummary(this.chat_gid);

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
        this.GetWhatsappchatSummary(this.chat_gid);

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
    this.selection.clear();
  }

  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  //Template popup passing identifier value//
  poptemplatemodal(parameter: string) {
    this.reactiveMessageForm.get("identifierValue")?.setValue(parameter);
    this.Getwosmsgtemplatesummary();
  }

  //contact to message//
  showResponsiveOutput(gid: string) {
    this.NgxSpinnerService.show();
    this.chat_gid = gid;
    this.chatWindow = "Chat"

    this.Getwoscontctsummary();
    this.GetWhatsappchatSummary(gid);
    this.NgxSpinnerService.hide();

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

      var url = 'WhatsApporderSummary/wosmsgsend'
      this.service.post(url, this.reactiveMessageForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappchatSummary();
          this.reactiveMessageForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.reactiveMessageForm.reset();
        }
        this.GetWhatsappchatSummary(this.chat_gid);
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

      var url = 'WhatsApporderSummary/wosmsgsend'
      this.service.post(url, this.reactiveTemplateMessageForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappchatSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
        }
        this.GetWhatsappchatSummary(this.chat_gid);
      });
    }

    else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Error Occured !! ')
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
      var url = 'WhatsApporderSummary/wosdocumentssend'
      this.service.post(url, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          //this.GetWhatsappchatSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)

        }
        this.GetWhatsappchatSummary(this.chat_gid);
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
    var url = 'WhatsApporderSummary/wosgetfilesummary'
    var params = {
      contact_id: gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.image_list = result.wosimages_list
      this.file_list = result.wosfiles_list
    });
  }

  backtochat() {
    this.chatWindow = "Chat"
  }

  ngOnDestroy(): void {
    if (this.windowInterval) {
      clearInterval(this.windowInterval);
    }
    if (this.windowInterval1) {
      clearInterval(this.windowInterval1);
    }
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
  importexcel() {
    debugger
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0,
      });
      formData.append("file", this.file, this.file.name);
      var api = 'WhatsApporderSummary/wosContactImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();

        }
        else {
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();

        }
      });
    }
  }
  onChange3(event: any) {
    this.file = event.target.files[0];
  }
  downloadfileformat() {
    debugger;
    let link = document.createElement("a");

    link.download = "Watsapp contact Details";
    window.location.href = environment.URL_FILEPATH1 + "Templates/WhatsApp Contacts.xlsx";

    link.click();
  }

  exportExcel(): void {
    const WhatsappContactList = this.woscontactsummary_List.map(item => ({
      displayName: item.displayName || '',
      value: item.value || '',
    }));
    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(WhatsappContactList[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    WhatsappContactList.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'WhatsappContactList.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.wostotalcontact_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.wostotalcontact_list.forEach((row: IContact) => this.selection.select(row));
  }

  Gettotalcontactlist() {
    this.NgxSpinnerService.show();
    var url = 'WhatsApporderSummary/Gettotalcontactlist'
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#wostotalcontact_list').DataTable().destroy();
      this.responsedata = result;
      this.wostotalcontact_list = this.responsedata.wostotalcontact_list;
      this.contact_count = result.contact_count;

    });
  }
  bulktemplatesend() {
    this.NgxSpinnerService.show();
    this.pick = this.selection.selected
    this.CurObj.wosbulktemplatesend = this.pick
    if (this.CurObj.wosbulktemplatesend.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Select atleast one record");
      return;
    }
    var url = 'WhatsApporderSummary/Postbulktemplatesend';
    this.service.post(url, this.CurObj).subscribe((result: any) => {
      if (result.status === false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      } else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
      }
    });
    this.selection.clear();
  }

}
