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
import { SelectionModel } from '@angular/cdk/collections';

interface Imailform {

  leadbank_gid: string;
  from_mail: string;
  mail_sub: string;
  mail_body: string;
  to_mail: any;
}
export class IAssign {
  DocumentUploadlist_list: any;


}
@Component({
  selector: 'app-crm-smm-outlookmailcompose',
  templateUrl: './crm-smm-outlookmailcompose.component.html',
  styleUrls: ['./crm-smm-outlookmailcompose.component.scss']
})
export class CrmSmmOutlookmailcomposeComponent {
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
  folder_list: any[] = [];
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
  CurObj: IAssign = new IAssign();
  selection = new SelectionModel<IAssign>(true, []);
  folderprt_list: any;
  breadcrumbs: any[] = [{ name: 'Home', id: null }];
  pick: Array<any> = [];
  docupload_gid: any;
  docupload_name: any;
  mailidfrom360:any;
  leadbank_gidfrom360:any;
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer) {
    this.mailform = {} as Imailform;
  }
  ngOnInit(): void {
    this.mailidfrom360=sessionStorage.getItem('CRM_TOMAILID');
    this.leadbank_gidfrom360=sessionStorage.getItem('CRM_LEADBANK_GID_ENQUIRY');
    if(this.mailidfrom360!=null){
    
    }else{
      sessionStorage.removeItem('CRM_TOMAILID');
      this.GetleadSummary();

    }
    this.GetOutlookMail();
    var url = 'FileManagement/DocumentUploadSummary';
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
      $('#folder_lists').DataTable().destroy();
      this.responsedata = result;
      this.folder_list = this.responsedata.DocumentUploadlist_list;
      //console.log('this.folder_list',this.folder_list)
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#folder_lists').DataTable({
          //code by snehith for customized pagination  
          "pageLength": 50, // Number of rows to display per page
          "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
        });
      }, 1);
    });
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
    if(this.mailidfrom360!=null){
    this.outlookreactiveForm.get('to_mail')?.setValue( this.mailidfrom360);
    }
  }
  openFile(docupload_gid: any, docupload_name: any) {

    if (docupload_gid !== undefined) {

      this.docupload_gid = docupload_gid;
      this.docupload_name = docupload_name;


      // Filter out entries with null name
      this.breadcrumbs = this.breadcrumbs.filter(item => item.name !== null);

      // Add to breadcrumbs only if docupload_name is not null
      if (docupload_name !== null) {
        this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
      }


      // this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
      var param = {
        parent_directorygid: this.docupload_gid
      }
      // console.log("Open folder:");
      // console.log(param);


      var url = 'FileManagement/FolderDtls';
      this.NgxSpinnerService.show();
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.Folder_list != null) {
          $('#folder_lists').DataTable().destroy();
          this.folder_list = result.Folder_list;
          this.NgxSpinnerService.hide();
          setTimeout(() => {
            $('#folder_lists').DataTable({
              //code by snehith for customized pagination  
              "pageLength": 50, // Number of rows to display per page
              "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
            });
          }, 1);
        }
        else {
          this.folder_list = result.Folder_list;
          setTimeout(() => {
            var table = $('#folder_lists').DataTable({
              //code by snehith for customized pagination  
              "pageLength": 50, // Number of rows to display per page
              "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
            });
          }, 1);
          this.NgxSpinnerService.hide();
          $('#folder_lists').DataTable().destroy();
          this.NgxSpinnerService.hide();
        }

      })
    }


  }
  navigateToFolder(crumb: any) {
    const index = this.breadcrumbs.indexOf(crumb);
    this.breadcrumbs = this.breadcrumbs.slice(0, index);
    if (crumb.name == "Home") {
      this.breadcrumbs = [{ name: 'Home', id: null }];
      var url = 'FileManagement/DocumentUploadSummary';
      this.NgxSpinnerService.show();
      this.service.get(url).subscribe((result: any) => {
        $('#folder_lists').DataTable().destroy();
        this.responsedata = result;
        this.folder_list = this.responsedata.DocumentUploadlist_list;
        //console.log('this.folder_list',this.folder_list)
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#folder_lists').DataTable({
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          });
        }, 1);
      });
    }
    else {
      this.openFile(crumb.id, crumb.name);
    }
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.folder_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.folder_list.forEach((row: IAssign) => this.selection.select(row));
  }
  sharedocument() {
    this.pick = this.selection.selected;
    this.CurObj.DocumentUploadlist_list = this.pick;
    if (this.CurObj.DocumentUploadlist_list.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("Kindly Select Atleast One Record.");
      this.breadcrumbs = [{ name: 'Home', id: null }];
      var url = 'FileManagement/DocumentUploadSummary';
      //this.NgxSpinnerService.show();
      this.service.get(url).subscribe((result: any) => {
        $('#folder_lists').DataTable().destroy();
        this.responsedata = result;
        this.folder_list = this.responsedata.DocumentUploadlist_list;
        //console.log('this.folder_list',this.folder_list)
        //this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#folder_lists').DataTable({
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          });
        }, 1);
      });
    }
    else {
      const azurePaths = this.pick.map(item => item.azure_path); // Collect azure_path from the picked items
      const azurePathString = azurePaths.map(path => `${path}`).join('\n'); // Join paths with <br> and wrap each in a <span>
      const currentGmailBody = this.outlookreactiveForm.value.mail_body || ''; // Get current gmail_body or an empty string if undefined
      const formattedString = this.transform(currentGmailBody + '<br>' + azurePathString); // Concatenate and format
      this.outlookreactiveForm.get("mail_body")?.setValue(formattedString); // Set the formatted value to gmail_body
      this.selection.clear(); // Clear the selection

      // Ensure new text after the formatted string is in black
      const editorContent = this.outlookreactiveForm.get("mail_body")?.value;
      if (editorContent) {
        this.outlookreactiveForm.get("mail_body")?.setValue(editorContent + '<br><br>');
      }
      this.breadcrumbs = [{ name: 'Home', id: null }];
      var url = 'FileManagement/DocumentUploadSummary';
      //this.NgxSpinnerService.show();
      this.service.get(url).subscribe((result: any) => {
        $('#folder_lists').DataTable().destroy();
        this.responsedata = result;
        this.folder_list = this.responsedata.DocumentUploadlist_list;
        //console.log('this.folder_list',this.folder_list)
        //this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#folder_lists').DataTable({
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          });
        }, 1);
      });

    }
  }
  transform(text: string): string {
    const urlPattern = /(https?:\/\/[^\s]+)/g;
  
    // Replace URLs with anchor tags and add a <br> after each URL
    let formattedText = text.replace(urlPattern, (match) => {
      return `<a class="url" href="${match}" target="_blank">${match}</a><br>`;
    });
  
    // Replace newlines with <br><br>
    formattedText = formattedText.replace(/\n/g, '<br><br>');
  
    return formattedText + '<br><br>';
  }
  
  getIconClass(fileName: string): string {
    if (!fileName) {
      return 'fas fa-file text-muted'; // default icon if fileName is empty or null
    }

    const fileExtension = fileName.split('.').pop()?.toLowerCase(); // add optional chaining operator (?.)
    switch (fileExtension) {
      case 'pdf':
        return 'fas fa-file-pdf text-danger';
      case 'docx':
        return 'fas fa-file-word text-primary';
      case 'xlsx':
        return 'fas fa-file-excel text-success';
      case 'csv':
        return 'fas fa-file-csv text-info'; // added icon for CSV files
      case 'jpg':
      case 'jpeg':
      case 'png':
        return 'fas fa-file-image text-warning';
      case 'zip':
        return 'fas fa-file-archive text-secondary';
      case 'txt':
        return 'fas fa-file-text text-muted';
      case 'pptx':
        return 'fas fa-file-powerpoint text-primary';
      case 'p3':
        return 'fas fa-file-audio text-secondary';
      case 'p4':
        return 'fas fa-file-video text-secondary';
      default:
        return 'fas fa-file text-muted';
    }
  }
  onclosepop() {
    this.selection.clear();
    this.breadcrumbs = [{ name: 'Home', id: null }];
    var url = 'FileManagement/DocumentUploadSummary';
    //this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
      $('#folder_lists').DataTable().destroy();
      this.responsedata = result;
      this.folder_list = this.responsedata.DocumentUploadlist_list;
      //console.log('this.folder_list',this.folder_list)
      //this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#folder_lists').DataTable({
          //code by snehith for customized pagination  
          "pageLength": 50, // Number of rows to display per page
          "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
        });
      }, 1);
    });
  }

  GetOutlookMail(){
    var api = 'OutlookCampaign/ComposeOutlookMail'
    this.service.get(api).subscribe((result: any) =>{
      this.response_data = result;
      this.ComposeOutlookMail_list = this.response_data.ComposeOutlookMail_list
      this.outlookreactiveForm.get("from_mail")?.setValue(this.ComposeOutlookMail_list[0].employee_emailid);
      this.from_mailaddress =this.ComposeOutlookMail_list[0].employee_emailid;
    });
  }
  public onaddmail(): void {
    this.mailform = this.outlookreactiveForm.value;
    this.outlookreactiveForm.get('leadbank_gid')?.setValue(this.leadbank_gidfrom360);
  
    const emailAddresses = this.selectedContacts.map(contact => contact.email);
    const cc_mailaddress = this.selectedContacts_cc.map(contact => contact.email);
    const bcc_mailaddress = this.selectedContacts_bcc.map(contact => contact.email);
    
    let tomailaddress_list = emailAddresses.join(',');
    const cc_mailids = cc_mailaddress.join(',');
    const bcc_mailids = bcc_mailaddress.join(',');
  
    // Set default value if to_mail is null
    if (!tomailaddress_list || tomailaddress_list.trim() === "") {
      tomailaddress_list = this.outlookreactiveForm.value.to_mail
    }
  
    if (this.mailfiles == null) {
      this.outlookreactiveForm.patchValue({
        to_mail: tomailaddress_list,
        cc_mail: cc_mailids,
        bcc_mail: bcc_mailids
      });
      
      this.NgxSpinnerService.show();
      var api7 = 'OutlookCampaign/SendOutlookMail';
      
      this.service.post(api7, this.outlookreactiveForm.value).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          window.scrollTo({ top: 0 });
          this.ToastrService.warning(result.message);
        } else {
          window.scrollTo({ top: 0 });
          this.ToastrService.success(result.message);
          this.outlookreactiveForm.reset();
          this.selectedContacts = [];
          this.selectedContacts_cc = [];
          this.selectedContacts_bcc = [];
          this.outlookreactiveForm.get("from_mail")?.setValue(this.from_mailaddress);
        }
        this.NgxSpinnerService.hide();
      });
    } else if (this.mailform.mail_sub != null && tomailaddress_list != null) {
      this.NgxSpinnerService.show();
      const allattchement = JSON.stringify(this.allattchement);
  
      // Using FormData to append file and email data
      this.formDataObject.append("from_mail", this.mailform.from_mail);
      this.formDataObject.append("mail_sub", this.mailform.mail_sub);
      this.formDataObject.append("to_mail", tomailaddress_list);
      this.formDataObject.append("cc_mail", cc_mailids);
      this.formDataObject.append("bcc_mail", bcc_mailids);
      this.formDataObject.append("mail_body", this.mailform.mail_body);
      this.formDataObject.append("mailfiles", allattchement);
  
      var api7 = 'OutlookCampaign/SendOutlookMailwithfiles';
      
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.response_data = result;
        if (result.status == false) {
          window.scrollTo({ top: 0 });
          this.ToastrService.warning(result.message);
        } else {
          window.scrollTo({ top: 0 });
          this.ToastrService.success(result.message);
          this.outlookreactiveForm.reset();
          this.selectedContacts = [];
          this.selectedContacts_cc = [];
          this.selectedContacts_bcc = [];
          this.outlookreactiveForm.get("from_mail")?.setValue(this.from_mailaddress);
        }
        this.NgxSpinnerService.hide();
      });
    } else {
      this.ToastrService.warning("Fill mandatory field.");
    }
  }
  
  config_compose_mail: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '120px',
    minHeight: '0rem',
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
  summaryreset() {
    this.outlookreactiveForm.get("mail_sub")?.setValue('');
    this.outlookreactiveForm.get("to_mail")?.setValue('');
    this.outlookreactiveForm.get("file")?.setValue('');
    this.outlookreactiveForm.get("mail_body")?.setValue('');
    this.selectedContacts=[];
    this.selectedContacts_cc=[];
    this.selectedContacts_bcc=[];
  }
  onback(){
    window.history.back();
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
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,20}$/;
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
