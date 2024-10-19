import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { encode, decode } from 'js-base64';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';


interface Email {
  mail_subject: string;
  to_mailaddress: string;
  checked?: boolean;
  isHovered?: boolean;
}
@Component({
  selector: 'app-crm-smm-mailsent',
  templateUrl: './crm-smm-mailsent.component.html',
  styleUrls: ['./crm-smm-mailsent.component.scss']
})
export class CrmSmmMailsentComponent {
  responsedata: any;
  gmailsenditemsummary_list: any[] = [];
  showOptions: boolean = false;
  selectedEmail: Email | null = null;
  itemsPerPage: number = 200;
  currentPage: number = 1;
  paginatedEmails: any[] = [];
  emails: Email[] = [];
  filteredEmails: Email[] = this.emails;
  totalItems: number = 0;
  searchTerm: string = '';
  itemsPerPageOptions: number[] = [200, 500, 1000, 1500];
  mailtoaddressinitial: any;
  mailtoaddress: any;
  from_mailaddress:any;
  mailsubject: any;
  mailbody: any;
  maildate: any;
  mailattachement: any;
  mail_gid: any;
  mailtime: any;
  allattchement_list: any[]=[];
  hoveredAttachment: any;
  selectedAction = '';
  timeoutId: any;
  listvalue:boolean=false;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) {
    this.filteredEmails = this.emails;
  }
  ngOnInit(): void {
    this.Getsenditemsummary();




  }
  Getsenditemsummary() {
    this.NgxSpinnerService.show();
    var api = 'GmailCampaign/GmailSenditemSummary'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.gmailsenditemsummary_list = this.responsedata.gmailsenditemsummary_list;
      if(this.gmailsenditemsummary_list.length <0){
        this.listvalue=true;
      }
      this.emails = this.responsedata.gmailsenditemsummary_list;
      this.filteredEmails = this.gmailsenditemsummary_list;
      //this.totalItems = this.filteredEmails.length;
      const firstEmail = this.filteredEmails[0];
      this.selectEmail(firstEmail);
      this.NgxSpinnerService.hide();
    });
  }
  toggleCheck(email: Email, event: Event) {
    email.checked = !email.checked;
    event.stopPropagation();
  }
  selectEmail(email: any) {
    this.selectedEmail = email;
    this.mailtoaddressinitial = email.to_mailaddress.charAt(0).toUpperCase();
    this.from_mailaddress = email.from_mailaddress;
    this.mailtoaddress = email.to_mailaddress;
    this.mailsubject = email.mail_subject;
    this.mailbody = email.mail_body;
    this.maildate = email.sent_date;
    this.mailtime = email.sent_time;
    this.mailattachement = email.sent_date;
    this.mail_gid = email.gmail_gid;
this.Getattachement(this.mail_gid );
  }
Getattachement(mail_gid:any){
  var url = 'GmailCampaign/Getattachement'
  let param = {
    mail_gid : mail_gid 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
   this.allattchement_list=result.allattchement_list;
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


onMouseOver(attachment: any, index: number) {
  this.hoveredAttachment = attachment;
}

onMouseLeave() {
  this.hoveredAttachment = null;
}

updatePage() {
  // Paginate filtered emails based on current page and itemsPerPage
  const startIndex = (this.currentPage - 1) * this.itemsPerPage;
  this.paginatedEmails = this.filteredEmails.slice(startIndex, startIndex + this.itemsPerPage);
  //console.log(this.paginatedEmails)
}

pageChanged(event: any): void {
  // Handle page change event
  this.currentPage = event.page;
  this.updatePage();
}

onItemsPerPageChange() {
  // Handle change in items per page
  this.currentPage = 1; // Reset to first page when items per page changes
  this.updatePage();
}

downloadAttachment(attachment: any) {
  debugger
  if (attachment.document_path && attachment.document_path.trim() !== '') {
    let link = document.createElement("a");
    link.download = attachment.document_name;
    link.href = attachment.document_path;
    link.addEventListener('error', (event) => {
      console.error('Error occurred during file download:', event);
      this.ToastrService.error('Failed to download the file');
    });
    link.click();
  } else {
    window.scrollTo({ top: 0 });
    this.ToastrService.warning('No File Found');
  }
}

  searchEmails() {
    if (this.searchTerm === null || this.searchTerm === "") {
      this.Getsenditemsummary();
    }
    else {
      this.filteredEmails = this.gmailsenditemsummary_list.filter(email =>
        email.to_mailaddress.toLowerCase().includes(this.searchTerm.toLowerCase()) || email.mail_subject.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
      this.gmailsenditemsummary_list = this.filteredEmails;
    }
  }




  selectAction(action: string) {
    clearTimeout(this.timeoutId);
    // your action logic here
  }


  onbacks(){
    window.history.back();
  }
}
