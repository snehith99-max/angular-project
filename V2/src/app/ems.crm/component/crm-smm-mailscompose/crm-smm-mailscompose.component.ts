import { Component, OnInit, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { AES, enc } from 'crypto-js';
import { environment } from 'src/environments/environment';
interface IGmailform {
  leadbank_gid: string;
  gmail_mail_from: string;
  gmail_sub: string;
  gmail_body: string;
  gmail_to_mail: any;
  gmail_cc_mail: any;
  gmail_bcc_mail: any;
}
export class IAssign {
  DocumentUploadlist_list: any;


}
@Component({
  selector: 'app-crm-smm-mailscompose',
  templateUrl: './crm-smm-mailscompose.component.html',
  styleUrls: ['./crm-smm-mailscompose.component.scss']
})
export class CrmSmmMailscomposeComponent implements OnInit {
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
  showBccField: boolean = false;
  showCcField: boolean = false;
  gmailfiles!: FileList;
  base64EncodedText: any;
  originalValue:any;
  filename:any;
  gmailform!: IGmailform;
  gamilreactiveForm!: FormGroup;
  suggestedContacts: any[] = [];
  contacts: any[] = [];
  toField: string = '';
  toField_cc: string = '';
  toField_bcc: string = '';
  selectedContacts: { name: string, email: string, leadbank_gid: string }[] = []; // Array to store selected contacts
  allattchement: any[] = [];
  AutoIDkey: any;
  file_name: any;
  suggestedccContacts: any[] = [];
  suggested_bcc_Contacts: any[] = [];
  folder_list: any[] = [];
  selectedContacts_cc: { name: string, email: string, leadbank_gid: string }[] = [];
  selectedContacts_bcc: { name: string, email: string, leadbank_gid: string }[] = [];
  CurObj: IAssign = new IAssign();
  selection = new SelectionModel<IAssign>(true, []);
  folderprt_list: any;
  breadcrumbs: any[] = [{ name: 'Home', id: null }];
  pick: Array<any> = [];
  docupload_gid: any;
  docupload_name: any;
  default_template: any;
  mailidfrom360:any;
  leadbank_gidfrom360:any;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService
  ) {
    this.gmailform = {} as IGmailform;
  }

  ngOnInit(): void {

    this.mailidfrom360=sessionStorage.getItem('CRM_TOMAILID');
    this.leadbank_gidfrom360=sessionStorage.getItem('CRM_LEADBANK_GID_ENQUIRY');
    if(this.mailidfrom360!=null){
    }else{
      this.GetleadSummary();

    }
    var url = 'FileManagement/DocumentUploadSummary';
    this.service.get(url).subscribe((result: any) => {
      $('#folder_lists').DataTable().destroy();
      this.responsedata = result;
      this.folder_list = this.responsedata.DocumentUploadlist_list;
      //console.log('this.folder_list',this.folder_list)
      setTimeout(() => {
        $('#folder_lists').DataTable({
          //code by snehith for customized pagination  
          "pageLength": 50, // Number of rows to display per page
          "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
        });
      }, 1);
    });

    this.GetleadSummary();
    this.gamilreactiveForm = new FormGroup({
      gmail_sub: new FormControl(this.gmailform.gmail_sub, [Validators.required]),
      file: new FormControl(''),
      gmail_body: new FormControl(''),
      leadbank_gid: new FormControl(''),
      gmail_to_mail: new FormControl(''),
      gmail_mail_from: new FormControl(''),
      base64EncodedText: new FormControl(''),
      tomailaddress_list: new FormControl(''),
      gmail_cc_mail: new FormControl(''),
      gmail_bcc_mail: new FormControl(''),
    });
    if(this.mailidfrom360!=null){
      this.gamilreactiveForm.get('gmail_to_mail')?.setValue( this.mailidfrom360);
     }
  }
  GetleadSummary() {
    var api3 = 'GmailCampaign/GetLeaddropdown'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.contacts = this.responsedata.GetLeaddropdown_lists;
      this.from_mailaddress = this.contacts[0].gmail_address;
      this.default_template = this.contacts[0].default_template;
      this.gamilreactiveForm.get('gmail_body')?.setValue(this.default_template);
    });
  }
  openFile(docupload_gid: any, docupload_name: any) {

    if (docupload_gid !== undefined) {

      this.docupload_gid = docupload_gid;
      this.docupload_name = docupload_name;

      this.breadcrumbs = this.breadcrumbs.filter(item => item.name !== null);
      if (docupload_name !== null) {
        this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
      }
      var param = {
        parent_directorygid: this.docupload_gid
      }
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
  get gmail_sub() {
    return this.gamilreactiveForm.get('gmail_sub')!;
  }

  get gmail_body() {
    return this.gamilreactiveForm.get('gmail_body')!;
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
      // const azurePaths = this.pick.map(item => item.azure_path); // Collect azure_path from the picked items
      // const azurePathString = azurePaths.map(path => `${path}`).join('\n'); // Join paths with <br> and wrap each in a <span>
      // const currentGmailBody = this.gamilreactiveForm.value.gmail_body || ''; // Get current gmail_body or an empty string if undefined
      // const formattedString = this.transform(currentGmailBody + '<br>' + azurePathString); // Concatenate and format
      // this.gamilreactiveForm.get("gmail_body")?.setValue(formattedString); // Set the formatted value to gmail_body
      // this.selection.clear(); // Clear the selection

      // // Ensure new text after the formatted string is in black
      // const editorContent = this.gamilreactiveForm.get("gmail_body")?.value;
      // if (editorContent) {
      //   this.gamilreactiveForm.get("gmail_body")?.setValue(editorContent + '<br><br>');
      // }
      const currentBody = this.gamilreactiveForm.get('gmail_body')?.value || '';

      const fillink = this.pick.map(item => {
        const key = environment.secretKey;
        const encrytpvalue = AES.encrypt(item.azure_path, key).toString();
        return `<a href="${encrytpvalue}" download>${item.fileupload_name}</a>`;
      }).join('<br>');  
         
      const updatedBody = currentBody + '<br>' + fillink;

      this.gamilreactiveForm.get("gmail_body")?.setValue(updatedBody + '<br><br>');
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
  onback(){
    window.history.back();
  }
  ongamilChange1($event: any): void {
    this.gmailfiles = $event.target.files;

    if (this.gmailfiles != null && this.gmailfiles.length !== 0) {
      for (let i = 0; i < this.gmailfiles.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.gmailfiles[i]);
        this.file_name = this.gmailfiles[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.gmailfiles[i].name
        });
      }
    }

  }

  generateKey(): string {
    return `AutoIDKey${new Date().getTime()}`;
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
      ['insertImage', 'insertVideo', 'removeFormat', 'toggleEditorMode']
    ]
  };

  toggleBccField() {
    this.showBccField = !this.showBccField;
  }

  toggleCcField() {
    this.showCcField = !this.showCcField;
  }

  summaryreset() {
    this.gamilreactiveForm.get("gmail_sub")?.setValue('');
    this.gamilreactiveForm.get("file")?.setValue('');
    this.gamilreactiveForm.get("gmail_body")?.setValue('');
    this.gamilreactiveForm.get("gmail_to_mail")?.setValue('');
    this.gamilreactiveForm.get("gmail_cc_mail")?.setValue('');
    this.gamilreactiveForm.get("gmail_bcc_mail")?.setValue('');
    this.selectedContacts = [];
    this.selectedContacts_cc = [];
    this.selectedContacts_bcc = [];
  }

  onclose() {
    this.gamilreactiveForm.get("gmail_sub")?.setValue('');
    this.gamilreactiveForm.get("gmail_to_mail")?.setValue('');
    this.gamilreactiveForm.get("file")?.setValue('');
    this.gamilreactiveForm.get("gmail_body")?.setValue('');
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

  public onaddmail(): void {
    this.NgxSpinnerService.show();
    debugger
    this.gmailform = this.gamilreactiveForm.value;
    this.gamilreactiveForm.get('leadbank_gid')?.setValue( this.leadbank_gidfrom360);
    console.log('woimewlkce', this.gamilreactiveForm.value)
    if (this.gmailfiles == null) {
      const to_mailAddresses = this.selectedContacts.map(contact => contact.email);
      const cc_mailaddress = this.selectedContacts_cc.map(contact => contact.email);
      const bcc_mailaddress = this.selectedContacts_bcc.map(contact => contact.email);
      const tomailaddress_list = to_mailAddresses.join(',');
      const CCmailaddress_list = cc_mailaddress.join(',');
      const BCCmailaddress_list = bcc_mailaddress.join(',');
      const trimmedBody = this.gmailform.gmail_body
      .replace(/unsafe:/g, '')  // Remove 'unsafe:'
      .replace(/&#10;/g, '')    // Remove '&#10;'
      .replace(/amp;/g, ''); 

      const key = environment.secretKey;
      const bodyencryptvalue = this.gamilreactiveForm.get('gmail_body')?.value;
      const links = bodyencryptvalue.match(/<a href="(.*?)"/g);
      const file = bodyencryptvalue.match(/download\s*>\s*([^<]+)<\/a>/g);

      
      let result: string = ''; 

      if (links && file) {
        for (let i = 0; i < links.length; i++) {
            const link = links[i];
            const bodysplitvalue = link.match(/<a href="(.*?)"/);
            if (bodysplitvalue) {
                const bodyvalues = AES.decrypt(bodysplitvalue[1], key);
                const originalValue = bodyvalues.toString(enc.Utf8).split('?')[0]; 
                if (file[i]) {
                    const filesplit = file[i].match(/download\s*>\s*([^<]+)<\/a>/);
                    if (filesplit) {
                        const filename = filesplit[1];
                        result += `<a href="${originalValue}" download>${filename}</a><br>`;
                    }
                }
            }
        }
    }
    
    
    const originalText =
      'From: ' + this.gmailform.gmail_mail_from + '\r\nTo: ' + tomailaddress_list + '\r\nCc:' + CCmailaddress_list
       + '\r\nBcc:' + BCCmailaddress_list + '\r\nSubject:' + this.gmailform.gmail_sub + 
       '\r\nContent-Type: text/html\r\n\r\n' + result;
      this.base64EncodedText = encodeToBase64(originalText);
      console.log(originalText, 'sendevent');
      this.gamilreactiveForm.patchValue({
        base64EncodedText: this.base64EncodedText,
        tomailaddress_list: tomailaddress_list,
      });
     
      var api7 = 'Leadbank360/Gmailtext'
      this.service.post(api7, this.gamilreactiveForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message)
          //window.location.reload();
          this.NgxSpinnerService.hide();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.gamilreactiveForm.reset();
          this.selectedContacts = [];
          this.selectedContacts_cc = [];
          this.selectedContacts_bcc = [];
          this.gamilreactiveForm.get("gmail_mail_from")?.setValue(this.from_mailaddress);
          this.ToastrService.success(result.message)
          //window.location.reload();
          this.NgxSpinnerService.hide();
          window.history.back();
        }
      });
    }
    else if (this.gmailform.gmail_sub != null && this.selectedContacts != null) {
      this.NgxSpinnerService.show();
      const allattchement = "" + JSON.stringify(this.allattchement) + "";
      const to_mailAddresses = this.selectedContacts.map(contact => contact.email);
      const cc_mailaddress = this.selectedContacts_cc.map(contact => contact.email);
      const bcc_mailaddress = this.selectedContacts_bcc.map(contact => contact.email);
      const tomailaddress_list = to_mailAddresses.join(',');
      const CCmailaddress_list = cc_mailaddress.join(',');
      const BCCmailaddress_list = bcc_mailaddress.join(',');
      this.formDataObject.append("gmailfiles", allattchement);
      this.formDataObject.append("gmail_mail_from", this.gmailform.gmail_mail_from);
      this.formDataObject.append("gmail_sub", this.gmailform.gmail_sub);
      this.formDataObject.append("gmail_to_mail", tomailaddress_list);
      this.formDataObject.append("gmail_body", this.gmailform.gmail_body);
      this.formDataObject.append("leadbank_gid", this.gmailform.leadbank_gid);
      this.formDataObject.append("gmail_cc_mail", CCmailaddress_list);
      this.formDataObject.append("gmail_bcc_mail", BCCmailaddress_list);



      var api7 = 'Leadbank360/Gmailupload'
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          //window.location.reload();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.gamilreactiveForm.reset();
          this.selectedContacts = [];
          this.selectedContacts_cc = [];
          this.selectedContacts_bcc = [];
          this.gamilreactiveForm.get("gmail_mail_from")?.setValue(this.from_mailaddress);
          //window.location.reload();
          this.NgxSpinnerService.hide();
          window.history.back();
          this.ToastrService.success(result.message)
        }
      });
      this.NgxSpinnerService.hide();
      return;
    }
    else{
      this.ToastrService.warning("Fill mandatory field.");

    }
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!event.target || !(event.target as HTMLElement).closest('#notification') && !(event.target as HTMLElement).closest('.sampel')) {
      this.suggestedContacts = [];
      this.suggestedccContacts = [];
      this.suggested_bcc_Contacts = [];
    }
  }
  onSearch(query: string) {
    query = query.toLowerCase();
    if (query) {
      this.suggestedContacts = this.contacts.filter(contact =>
        contact.name.toLowerCase().includes(query) || contact.email.toLowerCase().includes(query)
      );
      if (this.suggestedContacts.length === 0) {
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
  removeContact(index: number) {
    this.selectedContacts.splice(index, 1);
  }
  removeContact_cc(index: number) {
    this.selectedContacts_cc.splice(index, 1);
  }
  removeContact_bcc(index: number) {
    this.selectedContacts_bcc.splice(index, 1);
  }
  validateEmail(email: string): boolean {
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,20}$/;
    return emailPattern.test(email);
  }
  addNewContact(query: string) {
    const newContact = {
      name: query.split('@')[0],
      email: query,
      leadbank_gid: ''
    };
    this.contacts.push(newContact);
    return newContact;
  }
}

function encodeToBase64(text: string): string {
  return btoa(text);
}
