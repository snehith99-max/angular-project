import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
import { encode, decode } from 'js-base64';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Subscription, forkJoin } from 'rxjs';
import flatpickr from 'flatpickr';
interface EmailList {
  s_no: string;
  inbox_id: string;
  from_id: string;
  sent_date: string;
  sent_time: string;
  cc: string;
  subject: string;
  body: string;
  attachement_flag: string;

}

export class IMailTrash {
  gmailmovelist: EmailList[] = []; // Array of Email objects
}
export class IMailFolder {
  label_id: string | undefined;
  gmailmovelist: EmailList[] = []; // Array of Email objects
}
interface IMailform {
  leadbank_gid: string;
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  displayName: any;
  lastname_edit: any;
  name: any;
  email_address: any;
  customer_type: any;
  customertype_edit: any;
  to_mail: any;

}
interface IGmailform {

  leadbank_gid: string;
  gmail_mail_from: string;
  gmail_sub: string;
  //  gmail_to: string;
  gmail_body: string;

  gmail_to_mail: any;
}
interface Email {
  id: any;
  subject: string;
  body: string;

  inbox_id: string;
  from_id: string;
  s_no: string;
  cc: string;
  sent_date: string;
  sent_time: string;
  bcc: string;
  attachement_flag: string;
  read_flag: boolean;
  checked?: boolean;
  isHovered?: boolean;  // Optional isHovered property
  integrated_gmail: string;
  Attachments?: Attachments[];
}
interface Attachments {
  original_filename: string;
  inbox_id: string;
  file_path: string;
  mimeType: string;
}
interface Attachment {
  original_filename: string;
  mimeType: string;
}
interface Comment {
  comments: string;
  s_no: string;
  created_date: string;
  created_by: string;
  isEditing: boolean;
}

interface Emailreply {
  s_no: string;
  reply_id: string;
  inbox_id: string;
  from_id: string;
  cc: string;
  sent_date: string;
  sent_time: string;
  bcc: string;
  subject: string;
  body: string;
  attachement_flag: string;
  both_body: string;
  attachments: EmailAttachment[];
}
interface EmailFwd {
  s_no: string;
  reply_id: string;
  inbox_id: string;
  to_id: string;
  cc: string;
  sent_date: string;
  sent_time: string;
  bcc: string;
  subject: string;
  body: string;
  attachement_flag: string;
  both_body: string;
  attachments: EmailAttachment[];
}
interface EmailAttachment {
  s_no: string;
  inbox_id: string;
  original_filename: string;
  modified_filename: string;
  file_path: string;
}
@Component({
  selector: 'app-crm-smm-gmaildirectinboxsummary',
  templateUrl: './crm-smm-gmaildirectinboxsummary.component.html',
  styleUrls: ['./crm-smm-gmaildirectinboxsummary.component.scss']
})
export class CrmSmmGmaildirectinboxsummaryComponent implements OnInit {
  @ViewChild('scrollContainer') scrollContainer!: ElementRef;
  CurObj: IMailTrash = new IMailTrash();
  CurObj1: IMailFolder = new IMailFolder();
  @ViewChild('fileInput') fileInput!: { nativeElement: { value: string; }; };
  isReply: boolean = false;
  isForward: boolean = false;
  isForwardFwd: boolean = false;
  searchTerm: string = '';
  globalreplyid: any;
  golbalemail_id: any;
  response_data: any;
  mailsummary_list: any;
  globlaattachement_flag: any;
  responsedata: any;
  formDataObject: FormData = new FormData();
  from_mailaddress: any;
  reactiveFormContactEdit!: FormGroup;
  displayName: any;
  customer_type: any;
  customertype_edit: any;
  customertype_list: any[] = [];
  isReadOnly: boolean = true;
  gmailapiinboxatatchement_list: any[] = [];
  inboxforward: boolean = false;
  replyforward: boolean = false;
  inboxcustomer_list: any[] = [];
  GetEmailId_lists: any[] = [];
  globalfromemail_address: any;
  ///
  first!: SafeHtml;
  second: any;
  htmlbodycontent: any;
  seconds!: SafeHtml;
  replysubject!: SafeHtml;
  replybody!: SafeHtml;
  gmailfiles!: File;
  base64EncodedText: any;
  gmailform!: IGmailform;
  gamilreactiveForm!: FormGroup;
  showCheck: Email | null = null;
  showMenu = true;
  isEmailTabActive: boolean = true;
  isCommentTabActive: boolean = false;
  email = '';
  email_id: any;
  message = '';
  showOptionsDivId: any;
  selectedFileNames: string[] = [];
  showOptions: boolean = false;
  showOptions1: boolean = false;
  hover = '';
  selectedAction = '';
  timeoutId: any;
  condition!: string;
  file!: File;
  globalFromId: string = '';
  globalinboxid: string = '';
  globalforwardid: string = '';
  emailBody: string = '';
  forwardto: string = '';
  replytoid: string = '';
  replyccid: string = '';
  replybccid: string = '';
  globalbody: any;
  replyForm!: FormGroup;
  @ViewChild('emailTab') emailTab!: ElementRef;
  attachments: File[] = [];
  paginatedEmails: any[] = [];
  Getbussinessverticledropdown_list: any[] = [];
  totalItems: number = 0;
  itemsPerPage: number = 200;
  currentPage: number = 1;
  subject: any;
  itemsPerPageOptions: number[] = [200, 500, 1000, 1500]; // Options for "Show entries" dropdown
  newComment: string = '';
  editnewComment: string = '';
  gmailcomments_list: Comment[] = [];
  reactiveForm!: FormGroup;
  reactiveFormFolder!: FormGroup;
  folders: any;
  gmailapiinboxsummary_lists: any;
  movetofolder_list: EmailList[] = [];
  showSpinner: boolean = true;
  encryptparam: any;
  trash: boolean = true;
  inbox_id: any;
  private routerEventsSubscription: Subscription = new Subscription;
  trashBackground: string = 'white';
  constructor(private fb: FormBuilder, private elementRef: ElementRef, private sanitizer: DomSanitizer, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.filteredEmails = this.emails;
    this.gmailform = {} as IGmailform;

  }
  onbacks() {
    window.history.back();
  }
  addComment() {
    // if (this.newComment.trim()) {
    //   //this.gmailcomments_list.unshift({ comments: this.newComment, isEditing: false });
    //   this.newComment = '';
    //   this.renderComments();
    // }
    var param = {
      inbox_id: this.globalinboxid,
      comments: this.newComment,
    };
    if (this.newComment != null && this.newComment != "") {


      var url = 'GmailCampaign/PostGmailComments'
      this.service.post(url, param).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.newComment = '';
          //this.GetnotesSummary();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.newComment = '';
          this.ToastrService.success(result.message)
          //this.GetnotesSummary();
          var url3 = 'GmailCampaign/GetGmailComments';
          this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
          this.service.getparams(url3, param).subscribe(
            (result: any) => {
              this.gmailcomments_list = result.gmailcomments_list;
              //console.log(this.selectedReplmails); // Logging the fetched emails
              this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
            },
            (error) => {
              console.error('Error fetching reply emails:', error);
              this.NgxSpinnerService.hide(); // Hide spinner on error
            }
          );
        }
      });
    }
    else {
      this.ToastrService.warning('Comment Is Required!!')
    }
  }

  editComment(index: number) {
    // Close any currently open edit fields
    this.gmailcomments_list.forEach((comment, i) => {
      if (i !== index) {
        comment.isEditing = false;
      }
    });

    // Open the selected edit field
    this.gmailcomments_list[index].isEditing = true;
    this.editnewComment = this.gmailcomments_list[index].comments;
  }

  saveComment(index: number) {
    this.gmailcomments_list[index].isEditing = false;
    // console.log(this.gmailcomments_list[index].s_no)
    // console.log(this.editnewComment)
    if (this.editnewComment != "" && this.editnewComment != undefined && this.editnewComment != null) {
      let param = {
        s_no: this.gmailcomments_list[index].s_no,
        comments: this.editnewComment
      };
      this.NgxSpinnerService.show();
      var url = 'GmailCampaign/UpdatedGmailComments';
      this.service.post(url, param).subscribe((result: any) => {
        if (result.status === false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        } else {
          var params = {
            inbox_id: this.globalinboxid
          };
          var url3 = 'GmailCampaign/GetGmailComments';
          // this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
          this.service.getparams(url3, params).subscribe(
            (result: any) => {
              this.gmailcomments_list = result.gmailcomments_list;
              //console.log(this.selectedReplmails); // Logging the fetched emails
              this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
            },
            (error) => {
              console.error('Error fetching reply emails:', error);
              this.NgxSpinnerService.hide(); // Hide spinner on error
            }
          );
          this.ToastrService.success(result.message)
        }
        // this.GetnotesSummary();
      });
    }
    else {

      this.ToastrService.warning('Comment Is Required!! ')

    }
  }

  deleteComment(index: number) {
    this.gmailcomments_list[index].s_no;
    var url = 'GmailCampaign/deleteGmailComments'
    let param = {
      s_no: this.gmailcomments_list[index].s_no,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message)
      } else {
        var params = {
          inbox_id: this.globalinboxid
        };
        var url3 = 'GmailCampaign/GetGmailComments';
        // this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
        this.service.getparams(url3, params).subscribe(
          (result: any) => {
            this.gmailcomments_list = result.gmailcomments_list;
            //console.log(this.selectedReplmails); // Logging the fetched emails
            this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
          },
          (error) => {
            console.error('Error fetching reply emails:', error);
            this.NgxSpinnerService.hide(); // Hide spinner on error
          }
        );
        this.ToastrService.success(result.message)
      }
      //this.GetnotesSummary();

    });
  }




  ngOnInit(): void {
    const today = new Date();
    const minDate = today;
    const Options = {
      enableTime: true,
      dateFormat: 'Y-m-d H:i:S',
      minDate: minDate,
      defaultDate: today,
      minuteIncrement: 1
    };
    flatpickr('.date-picker', Options);

    const inbox_gid = this.route.snapshot.paramMap.get('inbox_gid');
    if (inbox_gid) {
      this.encryptparam = inbox_gid;

      const key = 'storyboard';
      this.inbox_id = AES.decrypt(this.encryptparam, key).toString(enc.Utf8)
    }
    // Show the spinner and load the data
    setTimeout(() => {
      // Assuming this is where you load your data
      this.filteredEmails = this.emails; // Replace with actual data loading logic
      this.NgxSpinnerService.show();

      // Hide the spinner after 2 seconds
      setTimeout(() => {
        this.NgxSpinnerService.show();
      }, 2000); // Hides the spinner after 2 seconds (2000 milliseconds)
    }, 2000); // Show the spinner and load data after 2 seconds (2000 milliseconds)

    setTimeout(() => {
      this.trash = false
    }, 5000)
    this.loadData();
    // this.NgxSpinnerService.show();
    this.routerEventsSubscription = this.router.events.subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        // Get the current route path from the event, excluding query parameters
        const currentPath = event.urlAfterRedirects.split('?')[0];

        if (currentPath === '/crm/CrmSmmGmailDirectInboxSummary') {
          this.NgxSpinnerService.show(); // Show spinner when the route matches
        } else {
          this.NgxSpinnerService.show(); // Hide spinner when the route does not match
        }
      }
    });
    this.NgxSpinnerService.show();
    if (this.inbox_id) {
      this.NgxSpinnerService.show();
    }


    // var api = 'GmailCampaign/GetMailFolderDetails'
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.folders = this.responsedata.GetMailFolderDetails_list;
    //   // console.log(this.folders)
    // });
    // var api = 'GmailCampaign/GmailAPIDirectly'
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.gmailapiinboxsummary_lists = this.responsedata.gmailapiinboxsummary_lists;
    //    console.log(this.gmailapiinboxsummary_lists)
    // });
    this.GetMailSummary();
    this.Getbussinessverticledropdown();

    this.reactiveForm = new FormGroup({
      appointment_timing: new FormControl(''),
      leadname_gid: new FormControl(null),
      bussiness_verticle: new FormControl(null),
      remarks: new FormControl(null),
      lead_title: new FormControl(null),
      email_id: new FormControl(null),
      inbox_id: new FormControl(null),
    });
    this.replyForm = new FormGroup({
      emailBody: new FormControl(''),
      inbox_id: new FormControl(''),
      orginal_body: new FormControl('')
    });
    this.reactiveFormContactEdit = new FormGroup({
      email_address: new FormControl(''),

      displayName: new FormControl(''),
      customer_type: new FormControl(''),
      customertype_edit: new FormControl(''),

    });
    this.reactiveFormFolder = new FormGroup({
      label_id: new FormControl(null, Validators.required),

    });
    this.gamilreactiveForm = new FormGroup({


      gmail_sub: new FormControl(this.gmailform.gmail_sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      gmail_body: new FormControl(''),
      leadbank_gid: new FormControl(''),
      gmail_to_mail: new FormControl(''),
      gmail_mail_from: new FormControl(''),
      base64EncodedText: new FormControl(''),
    });
    // //customer type dropdown

  }
  ngOnDestroy(): void {
    // Clean up subscription to avoid memory leaks
    if (this.routerEventsSubscription) {
      this.routerEventsSubscription.unsubscribe();
    }
  }

  loadData(): void {
    // Show the spinner and load the data
    setTimeout(() => {
      // Assuming this is where you load your data
      this.filteredEmails = this.emails; // Replace with actual data loading logic
      this.NgxSpinnerService.show();

      // Hide the spinner after 2 seconds
      setTimeout(() => {
        this.NgxSpinnerService.show();
      }, 4000); // Hides the spinner after 2 seconds (2000 milliseconds)
    }, 4000); // Show the spinner and load data after 2 seconds (2000 milliseconds)

  }

  refreshscreen() {
    this.GetMailSummary();
  }
  Getbussinessverticledropdown() {
    var api = 'AppointmentManagement/Getbussinessverticledropdown';
    this.service.get(api).subscribe((result: any) => {
      this.Getbussinessverticledropdown_list = result.Getbussinessverticledropdown_list;
    });
  }
  toggleTab(tab: string) {
    if (tab === 'email') {
      this.isEmailTabActive = true;
      this.isCommentTabActive = false;
    } else if (tab === 'comment') {
      this.isEmailTabActive = false;
      this.isCommentTabActive = true;
      // console.log(this.globalinboxid)
      let param = {
        inbox_id: this.globalinboxid
      }
      var url3 = 'GmailCampaign/GetGmailComments';
      this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
      this.service.getparams(url3, param).subscribe(
        (result: any) => {
          this.gmailcomments_list = result.gmailcomments_list;
          //console.log(this.selectedReplmails); // Logging the fetched emails
          this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
        },
        (error) => {
          console.error('Error fetching reply emails:', error);
          this.NgxSpinnerService.hide(); // Hide spinner on error
        }
      );
    }
  }
  //   {
  //     sent_time: '10:00 AM',
  //     from_id: 'support@hubspot.com',
  //     subject: 'Welcome to HubSpot',
  //     sent_date: '2023-06-11',
  //     body: 'Thank you for signing up for our service. Let us know if you have any questions.',
  //     attachement_flag: 'N',
  //     attachments: []
  //   },
  //   {
  //     sent_time: '11:00 AM',
  //     from_id: 'noreply@calendar.com',
  //     subject: 'Reminder: Meeting at 3 PM',
  //     sent_date: '2023-06-11',
  //     body: 'Don\'t forget about our meeting at 3 PM today.',
  //     attachement_flag: 'N',
  //     attachments: []
  //   },
  //   {
  //     sent_time: '09:00 AM',
  //     from_id: 'billing@company.com',
  //     subject: 'Your Invoice is Ready',
  //     sent_date: '2023-06-10',
  //     body: 'Your invoice for the month of May is ready.',
  //     attachement_flag: 'Y',
  //     attachments: [
  //       {
  //         original_filename: 'invoice_may.pdf',
  //         mimeType: 'application/pdf'
  //       }
  //     ]
  //   },
  //   {
  //     sent_time: '08:00 AM',
  //     from_id: 'newsletter@website.com',
  //     subject: 'Weekly Newsletter',
  //     sent_date: '2023-06-09',
  //     body: 'Here is your weekly newsletter.',
  //     attachement_flag: 'N',
  //     attachments: []
  //   },
  //   {
  //     sent_time: '07:00 AM',
  //     from_id: 'security@website.com',
  //     subject: 'Password Reset',
  //     sent_date: '2023-06-08',
  //     body: 'Click here to reset your password.',
  //     attachement_flag: 'N',
  //     attachments: []
  //   },
  //   {
  //     sent_time: '11:00 AM',
  //     from_id: 'admin@company.com',
  //     subject: 'Meeting Follow-Up',
  //     sent_date: '2023-06-07',
  //     body: 'Thank you for attending the meeting. Here are the minutes.',
  //     attachement_flag: 'Y',
  //     attachments: [
  //       {
  //         original_filename: 'meeting_minutes.pdf',
  //         mimeType: 'application/pdf'
  //       }
  //     ]
  //   }
  // ];
  selectedReplmails: Emailreply[] = [];
  selectedFwdmails: EmailFwd[] = [];
  selectedReplymails: any;
  selectedFilesBase64: string[] = [];

  onFilesSelected(event: any) {
    this.attachments = event.target.files;
    this.selectedFileNames = [];
    for (let i = 0; i < this.attachments.length; i++) {
      this.selectedFileNames.push(this.attachments[i].name);
    }
  }
  // @HostListener('document:click', ['$event1'])
  // handleOutsideClick1(event1: MouseEvent) {
  //   if (!this.elementRef.nativeElement.contains(event1.target)) {
  //     this.showOptions1 = false;
  //   }
  // }
  hideMenu1() {
    this.timeoutId = setTimeout(() => {
      this.showOptions1 = false;
    }, 500); // adjust the delay as needed
  }
  @HostListener('document:click', ['$event'])
  handleOutsideClick(event: MouseEvent) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.showOptions = false;
      this.showOptions1 = false;
    }
  }
  hideMenu() {
    this.timeoutId = setTimeout(() => {
      this.showOptions = false;
    }, 500); // adjust the delay as needed
  }


  selectAction(action: string) {
    clearTimeout(this.timeoutId);
    // your action logic here
  }
  closeEmailInput() {
    this.email = '';
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  // emails: Email[] = [
  //   { subject: 'Welcome to HubSpot', body: 'Thank you for signing up for our service. Let us know if you have any questions.', sender: 'support@hubspot.com', date: new Date('2023-06-11T10:00:00Z') },
  //   { subject: 'Reminder: Meeting at 3 PM', body: 'Don\'t forget about our meeting at 3 PM today.', sender: 'noreply@calendar.com', date: new Date('2023-06-11T09:00:00Z') },
  //   { subject: 'Your Invoice is Ready', body: 'Your invoice for the month of May is ready.', sender: 'billing@company.com', date: new Date('2023-06-10T10:00:00Z') },
  //   { subject: 'Weekly Newsletter', body: 'Here is your weekly newsletter.', sender: 'newsletter@website.com', date: new Date('2023-06-09T08:00:00Z') },
  //   { subject: 'Password Reset', body: 'Click here to reset your password.', sender: 'security@website.com', date: new Date('2023-06-08T07:00:00Z') },
  //   { subject: 'Meeting Follow-Up', body: 'Thank you for attending the meeting. Here are the minutes.', sender: 'admin@company.com', date: new Date('2023-06-07T11:00:00Z') },
  // ];

  emails: Email[] = [];
  filteredEmails: Email[] = this.emails;
  onclose2() {
    this.showOptions1 = false;
    this.reactiveForm.reset();
  }
  Onsubmit() {
    debugger
    // this.reactiveForm.value;
    this.showOptions1 = false;
    if (this.reactiveForm.value.bussiness_verticle != null &&
      this.reactiveForm.value.lead_title != null) {
      this.NgxSpinnerService.show();
      var url = 'GmailCampaign/PostcreateAppointments';
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.reactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.reactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  // get appointment_timing() {
  //   return this.reactiveForm.get('appointment_timing')!;
  // }
  get leadname_gid() {
    return this.reactiveForm.get('leadname_gid')!;
  }
  get bussiness_verticle() {
    return this.reactiveForm.get('bussiness_verticle')!;
  }
  get lead_title() {
    return this.reactiveForm.get('lead_title')!;
  }


  onadd() {

    this.router.navigate(['/crm/CrmSmmMailscompose']);
  }

  onclickaddopportunity(email: any) {

    this.email_id = email.from_id;
    this.reactiveForm.get("email_id")?.setValue(this.email_id);
    this.reactiveForm.get("inbox_id")?.setValue(email.inbox_id);
  }
  searchEmails() {
    // Filter emails based on search term
    this.filteredEmails = this.emails.filter(email =>
      email.subject.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      email.from_id.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
    this.totalItems = this.filteredEmails.length;
    this.updatePage();
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


  onChange(event: { target: { files: any[]; }; }) {
    this.file = event.target.files[0];
  }
  selectedEmail: Email | null = null;
  emailsFromSender: Email[] = [];
  gmailapiinboxsummary_list: any;
  GetMailSummary() {
    this.NgxSpinnerService.show();
    // var api = 'GmailCampaign/GmailAPIDirectly'
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.gmailapiinboxsummary_lists = this.responsedata.gmailapiinboxsummary_lists;
    //    console.log(this.gmailapiinboxsummary_lists)
    // });

    var api1 = 'GmailCampaign/GmailAPIDirectly';

    this.service.get(api1).subscribe(
      (result: any) => {
        // this.NgxSpinnerService.show();
        this.response_data = result;
        if (this.response_data == null) {
          this.NgxSpinnerService.hide();
        }
        this.emails = this.response_data.gmailapiinboxsummary_lists || []; // Ensure emails is an array
        this.filteredEmails = this.emails;
        //console.log('notificatio route',this.inbox_id)

        if (this.filteredEmails && this.filteredEmails.length > 0) {
          if (this.inbox_id) {
            // Find the email with matching inbox_id
            const matchingEmail = this.filteredEmails.find(email => email.inbox_id === this.inbox_id);

            if (matchingEmail) {
              // If found, move it to the beginning of the array
              this.filteredEmails = [
                matchingEmail,
                ...this.filteredEmails.filter(email => email.inbox_id !== this.inbox_id)
              ];
            } else {
              // If not found, sort by date
              this.filteredEmails.sort((a: Email, b: Email) => {
                return this.parseDateString(b.sent_date).getTime() - this.parseDateString(a.sent_date).getTime();
              });
            }
          } else {
            // If no inbox_id, sort by date
            this.filteredEmails.sort((a: Email, b: Email) => {
              return this.parseDateString(b.sent_date).getTime() - this.parseDateString(a.sent_date).getTime();
            });
          }

          const firstEmail = this.filteredEmails[0];
          this.totalItems = this.filteredEmails.length;
          this.updatePage();
          this.selectEmail(firstEmail);
          this.NgxSpinnerService.hide();

          // Run background API call asynchronously
          setTimeout(() => {
            this.runBackgroundApiCall();
          }, 0);
        } else {
          this.runBackgroundApiCall();
          this.NgxSpinnerService.hide();
        }
      },
      (error) => {
        // Handle the error
        console.error(error);
        this.NgxSpinnerService.hide(); // Hide spinner on error
      }
    );
  }
  parseDateString(dateString: string): Date {
    return new Date(Date.parse(dateString));
  }

  async runBackgroundApiCall() {
    var api2 = 'GmailCampaign/GmailAPIInboxMailLoad';
    try {
      const result = await this.service.get(api2).toPromise();
      // console.log(result);
    } catch (error) {
      //console.error(error);
    }
  }
  removeFontSizeAttributes(html: string): string {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');

    // Select all elements with font-size attribute and remove them
    const elementsWithFontSize = doc.querySelectorAll('*[style*="font-size"]');
    elementsWithFontSize.forEach(element => {
      element.removeAttribute('style');
    });

    // Serialize the updated DOM back to string
    return new XMLSerializer().serializeToString(doc);
  }
  validateEmails(emails: string): boolean {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/; // Basic email validation regex
    const emailAddresses = emails.split(',').map(email => email.trim()); // Split by comma and trim whitespace

    return emailAddresses.every(email => emailRegex.test(email));
  }
  selectEmail(email: Email) {
    this.selectedEmail = email;
    this.replytoid = this.getEmailFromId(this.selectedEmail.from_id);
    this.replyccid = this.selectedEmail.cc ? this.getEmailFromId(this.selectedEmail.cc) : '';
    this.replybccid = this.selectedEmail.bcc ? this.getEmailFromId(this.selectedEmail.bcc) : '';
    // debugger
    // const input = "Ala snehith <alasnehith@gmail.com>, alasnehith99@gmail.com,Sweety snehith  <sweetysnehith143@gmail.com>";
    // const output = extractNamesAndEmails(input);
    // console.log(output);
    // debugger
    // const input = "Ala snehith <alasnehith@gmail.com>, alasnehith99@gmail.com,Sweety snehith  <sweetysnehith143@gmail.com>";
    // const output = extractNamesAndEmails(input);
    // console.log(output);  //"alasnehith@gmail.com, alasnehith99@gmail.com, sweetysnehith143@gmail.com"
    // this.isEmailTabActive = true;
    this.isCommentTabActive = false;
    this.isForward = false;
    this.isReply = false;
    if (this.scrollContainer && this.scrollContainer.nativeElement) {
      const scrollContainerEl = this.scrollContainer.nativeElement;
      requestAnimationFrame(() => {
        scrollContainerEl.scrollTop = 0; // Scroll to the top
      });
    }
    this.globalinboxid = this.selectedEmail.inbox_id;
    this.emailsFromSender = this.emails.filter(e => e.from_id === email.from_id);
    const decodedString = decode(this.selectedEmail.body);
    let truncatedHtml = decodedString.substr(0, decodedString.length - 10);

    // Check if the truncatedHtml ends with an open tag and close it if needed
    if (truncatedHtml.lastIndexOf('<') > truncatedHtml.lastIndexOf('>')) {
      // Find the last open tag and close it properly
      const lastOpenTagIndex = truncatedHtml.lastIndexOf('<');
      const lastCloseTagIndex = truncatedHtml.lastIndexOf('>');
      if (lastCloseTagIndex < lastOpenTagIndex) {
        truncatedHtml += '</' + truncatedHtml.slice(lastOpenTagIndex + 1).split(' ')[0].replace('>', '') + '>';
      }
    }
    // function extractNamesAndEmails(value: string): string {
    //   // Split the input by commas to handle multiple email addresses
    //   const entries = value.split(',').map(entry => entry.trim());

    //   // Function to extract name or email from each entry
    //   const result = entries.map((entry, index) => {
    //     const emailPattern = /<([^>]+)>/; // Pattern to find emails in angle brackets
    //     const match = emailPattern.exec(entry);

    //     if (match) {
    //       // Extract the email part from the angle brackets
    //       return match[1];
    //     } else {
    //       // Extract the email part without angle brackets
    //       const emailPatternWithoutBrackets = /\b[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\b/;
    //       const matchWithoutBrackets = emailPatternWithoutBrackets.exec(entry);
    //       return matchWithoutBrackets ? matchWithoutBrackets[0] : '';
    //     }
    //   }).filter(item => item !== ''); // Remove empty strings

    //   return result.join(', ');
    // }
    // Remove all font-size attributes from HTML
    truncatedHtml = this.removeFontSizeAttributes(truncatedHtml);

    // Sanitize and assign to 'this.second'
    this.seconds = this.sanitizer.bypassSecurityTrustHtml(truncatedHtml) as SafeHtml;
    this.htmlbodycontent = this.seconds;
    //this.second = this.htmlbodycontent.changingThisBreaksApplicationSecurity;

    // Get the HTML content
    let htmlContent = this.htmlbodycontent.changingThisBreaksApplicationSecurity;

    // Remove the <title> tag and its content using a regular expression
    htmlContent = htmlContent.replace(/<title[^>]*>([\s\S]*?)<\/title>/gi, '');

    // Assign the sanitized HTML content
    this.second = htmlContent;
    this.globalbody = this.seconds;
    let param = {
      inbox_id: this.selectedEmail.inbox_id
    };
    let param2 = {
      email_id: email.from_id && email.from_id.trim() ? email.from_id : null
    };

    this.golbalemail_id = email.from_id;
    let attachmentObservable = null;
    debugger
    if (this.selectedEmail.read_flag) {
      const emailIndex = this.paginatedEmails.findIndex(e => e.inbox_id === email.inbox_id);
      if (emailIndex !== -1) {
        this.paginatedEmails[emailIndex].read_flag = false;  // Set read_flag to true
      }
      this.runBackgroundInboxStatus(this.selectedEmail.inbox_id);
    }

    if (this.selectedEmail?.Attachments) {
      this.gmailapiinboxatatchement_list = this.selectedEmail.Attachments.map((attachment) => {
        const mimeType = this.getMimeType(attachment.original_filename);
        const base64FilePath = attachment.file_path;

        // Add the data URI prefix based on the MIME type
        const dataUri = `data:${mimeType};base64,${base64FilePath}`;

        return {
          ...attachment,
          file_path: dataUri // Update file_path with the data URI
        };
      });
    } else {
      this.gmailapiinboxatatchement_list = [];
    }

    // debugger
    // console.log(this.gmailapiinboxatatchement_list)
    // if (this.selectedEmail.attachement_flag != 'N') {
    //   var url = 'GmailCampaign/GetInboxAttchement';
    //   attachmentObservable = this.service.getparams(url, param);
    // }

    // var url1 = 'GmailCampaign/GetReplyMail';
    // var replyMailObservable = this.service.getparams(url1, param);

    // var url3 = 'GmailCampaign/GetForwardMail';
    // var forwardMailObservable = this.service.getparams(url3, param);

    // this.NgxSpinnerService.show(); // Show spinner before fetching data
    // // var api = 'GmailCampaign/GetEmailId'
    // // this.service.get(api).subscribe((result: any) => {
    // //   this.responsedata = result;
    // //   this.GetEmailId_lists = this.responsedata.GetEmailId_lists;
    // //   if (this.GetEmailId_lists.length != 0) {
    // //     this.globalfromemail_address = this.GetEmailId_lists[0].gmail_address;
    // //   }
    // // });

    // var url4 = 'GmailCampaign/GetInboxCustomerDetails';
    // this.service.getparams(url4, param2).subscribe(
    //   (result: any) => {
    //     this.inboxcustomer_list = result.inboxcustomer_list;

    //     if (this.inboxcustomer_list.every(customer =>
    //       customer.sales_order_count === 0 &&
    //       customer.invoice_count === 0 &&
    //       (!customer.customercontact_name || customer.customercontact_name === '') &&
    //       (!customer.email || customer.email === '') &&
    //       (!customer.address1 || customer.address1 === '') &&
    //       (!customer.customer_name || customer.customer_name === '')
    //     )) {
    //       this.inboxcustomer_list = []; // Set entire list to null
    //       this.NgxSpinnerService.hide();
    //     } else {
    //       // Iterate through the list to assign null to specific fields
    //       this.inboxcustomer_list.forEach(customer => {
    //         if (customer.sales_order_count === 0) {
    //           customer.sales_order_count = null;
    //         }
    //         if (customer.invoice_count === 0) {
    //           customer.invoice_count = null;
    //         }
    //         if (!customer.customercontact_name || customer.customercontact_name === '') {
    //           customer.customercontact_name = null;
    //         }
    //         if (!customer.email || customer.email === '') {
    //           customer.email = null;
    //         }
    //         if (!customer.address1 || customer.address1 === '') {
    //           customer.address1 = null;
    //         }
    //         if (!customer.customer_name || customer.customer_name === '') {
    //           customer.customer_name = null;
    //         }
    //       });
    //       this.NgxSpinnerService.hide();
    //     }
    //   },
    //   (error) => {
    //     console.error('Error fetching inbox customer details:', error);
    //     this.NgxSpinnerService.hide(); // Hide spinner on error
    //   }
    // );
    // if (this.selectedEmail.read_flag === 'true' || this.selectedEmail.read_flag === 'True' || this.selectedEmail.read_flag === '' || this.selectedEmail.read_flag === null) {
    //   var url5 = 'GmailCampaign/GmailInboxStatusUpdate';
    //   this.service.post(url5, param).subscribe(
    //     (result: any) => {

    //     },
    //     (error) => {
    //       console.error('Error fetching inbox customer details:', error);
    //       this.NgxSpinnerService.hide(); // Hide spinner on error
    //     }
    //   );
    // }
    // // Combine observables
    // const observables = [];
    // if (attachmentObservable) {
    //   observables.push(attachmentObservable);
    // }
    // observables.push(replyMailObservable);
    // observables.push(forwardMailObservable);

    // forkJoin(observables).subscribe(
    //   (results: any[]) => {
    //     // Handle attachment results if applicable
    //     if (this.selectedEmail && this.selectedEmail.attachement_flag != 'N') {
    //       const attachmentResult = results.shift();
    //       this.gmailapiinboxatatchement_list = attachmentResult.gmailapiinboxatatchement_list;
    //     }

    //     // Handle reply mail results
    //     const replyMailResult = results.shift();
    //     this.selectedReplmails = replyMailResult.gmailapireply_list;
    //     if (this.selectedReplmails) {
    //       this.selectedReplmails.forEach((mail: any) => {
    //         mail.subject = this.getDecodeValue(mail.subject);
    //         mail.body = this.getDecodeValue(mail.body);
    //       });
    //     }

    //     // Handle forward mail results
    //     const forwardMailResult = results.shift();
    //     this.selectedFwdmails = forwardMailResult.gmailapiforward_list;
    //     if (this.selectedFwdmails) {
    //       this.selectedFwdmails.forEach((mail: any) => {
    //         mail.subject = this.getDecodeValue(mail.subject);
    //         mail.body = this.getDecodeValue(mail.body);
    //       });
    //     }

    //     this.NgxSpinnerService.hide(); // Hide spinner after fetching all data
    //   },
    //   (error) => {
    //     console.error('Error fetching data:', error);
    //     this.NgxSpinnerService.hide(); // Hide spinner on error
    //   }
    // );
  }
  async runBackgroundInboxStatus(inbox_id: any) {
    var api2 = 'GmailCampaign/GmailInboxStatusUpdateBack';
    try {
      let param = {
        inbox_id: inbox_id
      };
      const result = await this.service.post(api2, param).toPromise();
      // console.log(result);
    } catch (error) {
      //console.error(error);
    }
  }
  getMimeType(originalFilename: string): string {
    const extension = originalFilename.split('.').pop()?.toLowerCase(); // Get the file extension

    switch (extension) {
      case 'png':
        return 'image/png';
      case 'jpg':
      case 'jpeg':
        return 'image/jpeg';
      case 'gif':
        return 'image/gif';
      case 'pdf':
        return 'application/pdf';
      case 'txt':
        return 'text/plain';
      case 'doc':
        return 'application/msword';
      case 'docx':
        return 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
      // Add more types as needed
      default:
        return 'application/octet-stream'; // Fallback for unknown types
    }
  }


  tagEmail(email: any) {
    //console.log('tag',email)
    const key = 'storyboard';
    const param = email.inbox_id;
    const inbox_id = AES.encrypt(param, key).toString();
    const lspage = 'GI';
    const encryptedParam1 = AES.encrypt(lspage, key).toString();
    this.router.navigate(['/crm/CrmSmmGmailTagCustomer', inbox_id, encryptedParam1]);

  }

  untagEmail(email: any) {
    //console.log('untag',email)
    const key = 'storyboard';
    const param = email.inbox_id;
    const inbox_id = AES.encrypt(param, key).toString();
    const lspage = 'GI';
    const encryptedParam1 = AES.encrypt(lspage, key).toString();
    this.router.navigate(['/crm/CrmSmmGmailUntagCustomer', inbox_id, encryptedParam1]);

  }

  actionmenu1() {
    this.showOptions1 = false;
  }

  getDecodeValue(encodedValue: string): string {
    try {
      return atob(encodedValue);
    } catch (error) {
      console.error('Error decoding Base64:', error);
      return encodedValue; // Return original value on error
    }
  }
  getEmailFromId(fromId: string): string {
    const emailRegex = /<(.*?)>/;
    const match = fromId.match(emailRegex);
    this.globalFromId = match ? match[1] : fromId;
    return match ? match[1] : fromId;
  }
  wrapText(text: string, length: number): string[] {
    if (text.length <= length) {
      return [text];
    }

    const words = text.split(' ');
    const lines = [];
    let currentLine = '';

    for (const word of words) {
      if ((currentLine + ' ' + word).trim().length <= length) {
        currentLine += (currentLine ? ' ' : '') + word;
      } else {
        lines.push(currentLine);
        currentLine = word;
      }
    }

    if (currentLine) {
      lines.push(currentLine);
    }

    return lines;
  }
  replyButtonClicked(selectedlist: any) {
    this.isReply = true;
    this.isForward = false;
    this.replytoid = this.getEmailFromId(selectedlist.from_id);
    this.replyccid = selectedlist.cc ? this.getEmailFromId(selectedlist.cc) : '';
    this.replybccid = selectedlist.bcc ? this.getEmailFromId(selectedlist.bcc) : '';

    if (this.scrollContainer && this.scrollContainer.nativeElement) {
      const scrollContainerEl = this.scrollContainer.nativeElement;
      requestAnimationFrame(() => {
        scrollContainerEl.scrollTop = scrollContainerEl.scrollHeight;
      });
    }

  }
  replyToEmail() {
    // Show the spinner at the beginning
    this.NgxSpinnerService.show();

    if (!this.globalFromId || !this.globalinboxid) {
      // If globalFromId or globalinboxid is missing, show warning, hide spinner and return
      window.scrollTo({ top: 0 });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields!');
      this.NgxSpinnerService.hide();
      return;
    }

    if (!this.emailBody) {
      // If emailBody is missing, show warning, hide spinner and return
      window.scrollTo({ top: 0 });
      this.ToastrService.warning('Email Body is Required!');
      this.NgxSpinnerService.hide();
      return;
    }

    // At this point, globalFromId, globalinboxid, and emailBody are all present
    if (this.attachments && this.attachments.length > 0) {
      // Attachments are present, proceed with sending email with attachments
      this.sendEmailWithAttachments();
    } else {
      // No attachments selected, proceed with sending email without attachments
      this.sendEmailWithoutAttachments();
    }
  }

  sendEmailWithAttachments() {
    const formData = new FormData();
    formData.append('inbox_id', this.globalinboxid);
    formData.append('emailBody', this.emailBody);
    formData.append('original_body', this.globalbody.changingThisBreaksApplicationSecurity);
    if (this.replytoid) {
      if (!this.validateEmails(this.replytoid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid To Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }

    if (this.replyccid) {
      if (!this.validateEmails(this.replyccid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid Cc Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }

    if (this.replybccid) {
      if (!this.validateEmails(this.replybccid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid Bcc Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }
    formData.append('replytoid', this.replytoid);
    formData.append('replyccid', this.replyccid);
    formData.append('replybccid', this.replybccid);
    // Append attachments to FormData if available
    if (this.attachments && this.attachments.length > 0) {
      for (let i = 0; i < this.attachments.length; i++) {
        formData.append(`attachments[${i}]`, this.attachments[i], this.attachments[i].name);
      }
    }
    if (this.replytoid != null && this.replytoid != "" && this.replytoid != '') {
      var url = 'GmailCampaign/ReplyAllWithAttachment';
      this.service.postfile(url, formData).subscribe(
        (result: any) => {
          // Hide the spinner after the response is received
          this.NgxSpinnerService.hide();

          if (result.status == false) {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            this.selectedFileNames = [];
            this.fileInput.nativeElement.value = '';
            this.ToastrService.warning(result.message);
          } else {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            // Fetch updated reply emails list
            this.attachments = [];
            this.selectedFileNames = [];
            this.fileInput.nativeElement.value = '';
            this.fetchReplyEmailsList();
            this.emailBody = '';
            this.ToastrService.success(result.message);
          }
        },
        (error) => {
          console.error('Error sending reply email:', error);
          // Hide the spinner in case of an error
          this.NgxSpinnerService.hide();
        }
      );
    }
    else {
      this.NgxSpinnerService.hide();
      this.ToastrService.warning('To Email is Required!');
    }
  }

  sendEmailWithoutAttachments() {
    // Prepare form data to send without attachments
    var param = {
      inbox_id: this.globalinboxid,
      emailBody: this.emailBody,
      orginal_body: this.globalbody.changingThisBreaksApplicationSecurity,
      replytoid: this.replytoid,
      replyccid: this.replyccid,
      replybccid: this.replybccid,
    };
    if (this.replytoid) {
      if (!this.validateEmails(this.replytoid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid To Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }

    if (this.replyccid) {
      if (!this.validateEmails(this.replyccid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid Cc Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }

    if (this.replybccid) {
      if (!this.validateEmails(this.replybccid)) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Invalid Bcc Email Format!');
        return; // or throw an error, or do something else to handle the error
      }
    }
    if (this.replytoid != null && this.replytoid != "" && this.replytoid != '') {
      var url = 'GmailCampaign/ReplyInboxMail';
      this.service.post(url, param).subscribe(
        (result: any) => {
          // Hide the spinner after the response is received
          this.NgxSpinnerService.hide();

          if (result.status == false) {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            this.emailBody = '';
            this.ToastrService.warning(result.message);
          } else {
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done

            });
            // Fetch updated reply emails list
            this.fetchReplyEmailsList();
            this.emailBody = '';
            this.ToastrService.success(result.message);
          }
        },
        (error) => {
          console.error('Error sending reply email:', error);
          // Hide the spinner in case of an error
          this.NgxSpinnerService.hide();
        }
      );
    }
    else {
      this.NgxSpinnerService.hide();
      this.ToastrService.warning('To Email is Required!');
    }
  }
  forwardToEmail() {
    // console.log('list',selectedEmail)
    // console.log('globalinboxid',this.globalinboxid)
    // console.log('globalreplyid',this.globalreplyid)
    // console.log('emailBody',this.emailBody)
    // console.log('forwardto',this.forwardto)
    // console.log('globlaattachement_flag',this.globlaattachement_flag)
    this.NgxSpinnerService.show();
    var param = {
      inbox_id: this.globalinboxid,
      reply_id: this.globalreplyid,
      emailBody: this.emailBody,
      forwardto: this.forwardto,
      attachement_flag: this.globlaattachement_flag
    };
    if (this.globlaattachement_flag == "N") {
      if (this.forwardto != null && this.forwardto != undefined && this.forwardto != "") {
        var url = 'GmailCampaign/ReplyOrForwardInboxMail';
        this.service.post(url, param).subscribe(
          (result: any) => {
            // Hide the spinner after the response is received
            this.NgxSpinnerService.hide();

            if (result.status == false) {
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done

              });
              this.emailBody = '';
              this.forwardto = '';
              var params = {
                inbox_id: this.globalinboxid,
              }
              var url3 = 'GmailCampaign/GetForwardMail';
              this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
              this.service.getparams(url3, params).subscribe(
                (result: any) => {
                  this.selectedFwdmails = result.gmailapiforward_list;
                  console.log(this.selectedFwdmails); // Logging the fetched emails

                  // Decode subjects and bodies
                  if (this.selectedFwdmails) {
                    this.selectedFwdmails.forEach((mail: any) => {
                      mail.subject = this.getDecodeValue(mail.subject);
                      mail.body = this.getDecodeValue(mail.body);
                    });
                  }
                  this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
                },
                (error) => {
                  console.error('Error fetching reply emails:', error);
                  this.NgxSpinnerService.hide(); // Hide spinner on error
                }
              );
              this.ToastrService.warning(result.message);
            } else {
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done

              });
              // Fetch updated reply emails list
              //this.fetchReplyEmailsList();
              this.forwardto = '';
              this.emailBody = '';
              this.ToastrService.success(result.message);
            }
          },
          (error) => {
            console.error('Error sending reply email:', error);
            // Hide the spinner in case of an error
            this.NgxSpinnerService.hide();
          }
        );
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('To Email is Required !! ')
      }
    }
    else {
      if (this.forwardto != null && this.forwardto != undefined && this.forwardto != "") {
        var url = 'GmailCampaign/ReplyOrForwardInboxMailWithAttach';
        this.service.post(url, param).subscribe(
          (result: any) => {
            // Hide the spinner after the response is received
            this.NgxSpinnerService.hide();

            if (result.status == false) {
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done

              });
              this.emailBody = '';
              this.forwardto = '';
              this.ToastrService.warning(result.message);
            } else {
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done

              });
              // Fetch updated reply emails list
              //this.fetchReplyEmailsList();
              var params = {
                inbox_id: this.globalinboxid,
              }
              var url3 = 'GmailCampaign/GetForwardMail';
              this.NgxSpinnerService.show(); // Show spinner before fetching reply mails
              this.service.getparams(url3, params).subscribe(
                (result: any) => {
                  this.selectedFwdmails = result.gmailapiforward_list;
                  // console.log(this.selectedFwdmails); // Logging the fetched emails

                  // Decode subjects and bodies
                  if (this.selectedFwdmails) {
                    this.selectedFwdmails.forEach((mail: any) => {
                      mail.subject = this.getDecodeValue(mail.subject);
                      mail.body = this.getDecodeValue(mail.body);
                    });
                  }
                  this.NgxSpinnerService.hide(); // Hide spinner after fetching reply mails
                },
                (error) => {
                  console.error('Error fetching reply emails:', error);
                  this.NgxSpinnerService.hide(); // Hide spinner on error
                }
              );
              this.forwardto = '';
              this.emailBody = '';
              this.ToastrService.success(result.message);
            }
          },
          (error) => {
            console.error('Error sending reply email:', error);
            // Hide the spinner in case of an error
            this.NgxSpinnerService.hide();
          }
        );
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('To Email is Required !! ')
      }
    }

  }
  fetchReplyEmailsList() {
    // Fetch the list of reply emails after sending or replying
    let param = { inbox_id: this.globalinboxid };
    var url = 'GmailCampaign/GetReplyMail';
    this.service.getparams(url, param).subscribe(
      (result: any) => {
        this.selectedReplmails = result.gmailapireply_list;
        // Decode subjects and bodies
        if (this.selectedReplmails) {
          this.selectedReplmails.forEach((mail: any) => {
            mail.subject = this.getDecodeValue(mail.subject);
            mail.body = this.getDecodeValue(mail.body);
          });
        }
      },
      (error) => {
        console.error('Error fetching reply emails:', error);
      }
    );
  }

  forwardButtonClicked(slectedlist: any, inbox: string) {
    this.isForward = true;
    this.isReply = false;
    this.isForwardFwd = false;
    if (this.scrollContainer && this.scrollContainer.nativeElement) {
      const scrollContainerEl = this.scrollContainer.nativeElement;
      requestAnimationFrame(() => {
        scrollContainerEl.scrollTop = scrollContainerEl.scrollHeight;
      });
    }

    debugger
    if (inbox == "inbox") {
      this.globalinboxid = slectedlist.inbox_id;
      this.globlaattachement_flag = slectedlist.attachement_flag;
      this.subject = slectedlist.subject;
      // console.log('inbox_id',slectedlist.inbox_id)
      // console.log('attachement_flag',slectedlist.attachement_flag)
      this.globalreplyid = 'No';
      // console.log('globalreplyid',this.globalreplyid)
    }
    else {
      this.globalinboxid = slectedlist.inbox_id;
      this.globlaattachement_flag = slectedlist.attachement_flag;
      // console.log('inbox_id',slectedlist.inbox_id)
      // console.log('attachement_flag',slectedlist.attachement_flag)
      this.globalreplyid = slectedlist.reply_id;
      this.subject = slectedlist.subject;
      //console.log('globalreplyid',this.globalreplyid)
    }

  }
  forwardofButton(slectedlist: any, inbox: string) {
    this.isForwardFwd = true;
    this.isReply = false;
    this.isForward = false;
    if (this.scrollContainer && this.scrollContainer.nativeElement) {
      const scrollContainerEl = this.scrollContainer.nativeElement;
      requestAnimationFrame(() => {
        scrollContainerEl.scrollTop = scrollContainerEl.scrollHeight;
      });
    }

    //console.log(slectedlist)
    this.subject = slectedlist.subject;
    this.globalinboxid = slectedlist.inbox_id;
    this.globlaattachement_flag = slectedlist.attachement_flag;
    this.globalreplyid = slectedlist.reply_id;
    this.globalforwardid = slectedlist.forward_id;

  }

  forwardToFwdEmail() {
    debugger;

    const param = {
      inbox_id: this.globalinboxid,
      reply_id: this.globalreplyid,
      emailBody: this.emailBody,
      forwardto: this.forwardto,
      attachement_flag: this.globlaattachement_flag,
      forward_id: this.globalforwardid
    };

    if (this.forwardto && this.forwardto.trim() !== "") {
      const url = this.globlaattachement_flag === "N" ? 'GmailCampaign/ForwardOfFwdMail' : 'GmailCampaign/ForwardOfFwdMailWithAttach';
      this.NgxSpinnerService.show(); // Show spinner before starting the API calls

      this.service.post(url, param).subscribe(
        (result: any) => {
          if (!result.status) {
            window.scrollTo({ top: 0 }); // Scroll to top after event done
            this.emailBody = '';
            this.forwardto = '';

            // Fetch the updated forward emails list
            const params = { inbox_id: this.globalinboxid };
            const url3 = 'GmailCampaign/GetForwardMail';
            this.NgxSpinnerService.show(); // Show spinner before fetching forward mails

            this.service.getparams(url3, params).subscribe(
              (result: any) => {
                this.selectedFwdmails = result.gmailapiforward_list;
                //console.log(this.selectedFwdmails); // Logging the fetched emails

                // Decode subjects and bodies
                if (this.selectedFwdmails) {
                  this.selectedFwdmails.forEach((mail: any) => {
                    mail.subject = this.getDecodeValue(mail.subject);
                    mail.body = this.getDecodeValue(mail.body);
                  });
                }
                this.NgxSpinnerService.hide(); // Hide spinner after fetching forward mails
              },
              (error) => {
                console.error('Error fetching forward emails:', error);
                this.NgxSpinnerService.hide(); // Hide spinner on error
              }
            );

            this.ToastrService.warning(result.message);
          } else {
            window.scrollTo({ top: 0 }); // Scroll to top after event done
            this.emailBody = '';
            this.forwardto = '';

            // Fetch the updated forward emails list
            const params = { inbox_id: this.globalinboxid };
            const url3 = 'GmailCampaign/GetForwardMail';
            this.NgxSpinnerService.show(); // Show spinner before fetching forward mails

            this.service.getparams(url3, params).subscribe(
              (result: any) => {
                this.selectedFwdmails = result.gmailapiforward_list;
                //console.log(this.selectedFwdmails); // Logging the fetched emails

                // Decode subjects and bodies
                if (this.selectedFwdmails) {
                  this.selectedFwdmails.forEach((mail: any) => {
                    mail.subject = this.getDecodeValue(mail.subject);
                    mail.body = this.getDecodeValue(mail.body);
                  });
                }
                this.NgxSpinnerService.hide(); // Hide spinner after fetching forward mails
              },
              (error) => {
                console.error('Error fetching forward emails:', error);
                this.NgxSpinnerService.hide(); // Hide spinner on error
              }
            );

            this.ToastrService.success(result.message);
          }
        },
        (error) => {
          console.error('Error sending forward email:', error);
          this.NgxSpinnerService.hide(); // Hide spinner on error
        }
      );
    } else {
      this.ToastrService.warning('To Mail is Required!!');
    }
  }

  replyforwardbtn() {
    this.isForward = !this.isForward;
    this.isReply = false;
  }
  // replyButtonClicked() {
  //   this.emailTab.nativeElement.scrollIntoView({ behavior: 'mooth' });
  // }
  // searchEmails() {
  //   this.filteredEmails = this.emails.filter(email => {
  //     return email.subject.toLowerCase().includes(this.searchTerm.toLowerCase()) || email.from_id.toLowerCase().includes(this.searchTerm.toLowerCase());
  //   });
  // }
  getSenderInitial(sender: string): string {
    return sender.charAt(0).toUpperCase(); // Get the first letter and capitalize it
  }
  toggleCheck(email: Email, event: Event) {
    email.checked = !email.checked;
    event.stopPropagation();
  }

  moveToFolder() {

    const checkedEmails = this.emails.filter(email => email.checked);
    if (checkedEmails && checkedEmails.length > 0) {
      //console.log('Checked emails TO FOLDER:', checkedEmails);
    }
    this.movetofolder_list = checkedEmails;

  }

  moveToTrash() {
    const checkedEmails = this.emails.filter(email => email.checked);
    if (checkedEmails && checkedEmails.length > 0) {
      const selectedEmails: EmailList[] = checkedEmails.map(email => ({
        s_no: email.s_no,
        inbox_id: email.inbox_id,
        from_id: email.from_id,
        sent_date: email.sent_date,
        sent_time: email.sent_time,
        cc: email.cc,
        subject: email.subject,
        body: email.body,
        attachement_flag: email.attachement_flag,
      }));

      this.CurObj.gmailmovelist = selectedEmails;
      //console.log('Selected emails:', this.CurObj);
      // Perform further actions with selectedEmails
    }
    if (this.CurObj.gmailmovelist.length != 0) {
      this.NgxSpinnerService.show();
      var url1 = 'GmailCampaign/MovetoTrash'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          this.GetMailSummary();
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)

        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Trash! ")
    }
  }

  tagcustomer() {
    const checkedEmails = this.emails.filter(email => email.checked);
    if (checkedEmails && checkedEmails.length > 0) {
      //console.log('Checked emails TO FOLDER:', checkedEmails);
    }
  }
  untagcustomer() {
    const checkedEmails = this.emails.filter(email => email.checked);
    if (checkedEmails && checkedEmails.length > 0) {
      //console.log('Checked emails TO FOLDER:', checkedEmails);
    }
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

  hoveredAttachment: any;

  onMouseOver(attachment: any, index: number) {
    this.hoveredAttachment = attachment;
  }

  onMouseLeave() {
    this.hoveredAttachment = null;
  }

  downloadAttachment(attachment: any) {
    if (attachment.file_path && attachment.file_path.trim() !== '') {
      const pathParts = attachment.file_path.split('/');
      const filenameWithExtension = pathParts[pathParts.length - 1];
      const filename = filenameWithExtension.split('.')[0];

      let link = document.createElement("a");
      link.download = attachment.original_filename;
      link.href = attachment.file_path;

      // Add event listener to handle download completion or failure
      link.addEventListener('error', (event) => {
        console.error('Error occurred during file download:', event);
        // Handle error (e.g., show error message to the user)
        this.ToastrService.error('Failed to download the file');
      });

      // Trigger download
      link.click();
    } else {
      // Scroll to top of the page (optional)
      window.scrollTo({ top: 0 });

      // Display warning message when no file path is provided
      this.ToastrService.warning('No File Found');
    }
  }

  get label_id() {
    return this.reactiveFormFolder.get('label_id')!;
  }
  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  menuItemClicked(option: string) {
    //console.log('Selected option:', option);
    // Add your logic for each menu item click here
  }
  submitCheckedEmails() {
    const checkedEmails = this.emails.filter(email => email.checked);
    //console.log('Checked emails:', checkedEmails);
    // Add your custom logic here to handle the checked emails
  }
  GetMailView(params: any, params1: any) {
    const secretKey = 'storyboarderp';
    //console.log(params)
    const gmail_gid = AES.encrypt(params, secretKey).toString();
    const leadbank_gid = AES.encrypt(params1, secretKey).toString();

    this.router.navigate(['/crm/CrmSmmGmailview', gmail_gid, leadbank_gid])
  }

  onclose() {

    this.reactiveFormFolder.reset();
  }
  onmovefolder() {
    debugger
    // this.movetofolder_list;
    // this.reactiveFormFolder.value.label_id
    if (this.movetofolder_list != null && this.movetofolder_list != undefined && this.movetofolder_list.length != 0) {
      this.CurObj1.gmailmovelist = this.movetofolder_list;
      this.CurObj1.label_id = this.reactiveFormFolder.value.label_id;
      if (this.reactiveFormFolder.status == 'VALID') {
        this.NgxSpinnerService.show();
        var url1 = 'GmailCampaign/MovetoFolder'
        this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

          if (result.status == false) {
            this.reactiveFormFolder.reset();
            this.NgxSpinnerService.hide();
            this.ToastrService.warning(result.message)
          }
          else {
            this.GetMailSummary();
            this.reactiveFormFolder.reset();
            // this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)

          }

        });
      }
      else {

        this.ToastrService.warning("Kindly Select Mandatory Field ")
      }
    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Folder! ")
    }
  }
  ongamilChange1(event: any) {

    this.gmailfiles = event.target.files[0];

    let reader = new FileReader();
    reader.onload = (e) => {
      if (e.target && e.target.result) { // Check if e.target exists
        // Cast e.target.result to string
        let base64String = e.target.result as string;
        // Append the base64 content to formDataObject
        //console.log(base64String)
        this.formDataObject.append("gmailfiles", base64String);
      }
    };
    reader.readAsDataURL(this.gmailfiles);
    //console.log(this.gmailfiles)
  }


  get gmail_sub() {
    return this.gamilreactiveForm.get('gmail_sub')!;
  }
  get gmail_body() {
    return this.gamilreactiveForm.get('gmail_body')!;
  }

  public onaddmail(): void {

    // console.log(this.gamilreactiveForm.value)
    this.gmailform = this.gamilreactiveForm.value;
    if (this.gmailfiles == null) {
      const originalText =
        'From: ' + this.gmailform.gmail_mail_from + '\r\nTo: ' + this.gmailform.gmail_to_mail + '\r\nSubject:' + this.gmailform.gmail_sub + '\r\nContent-Type: text/html\r\n\r\n' + this.gmailform.gmail_body + '';
      this.base64EncodedText = encodeToBase64(originalText);
      this.gamilreactiveForm.patchValue({
        base64EncodedText: this.base64EncodedText,
      });
      //console.log('Base64 encoded text:',this.base64EncodedText);
      this.NgxSpinnerService.show();
      var api7 = 'Leadbank360/Gmailtext'
      this.service.post(api7, this.gamilreactiveForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
        }
      });
    }
    else if (this.gmailform.gmail_sub != null && this.gmailform.gmail_to_mail != null) {

      this.formDataObject.append("gmailfiles", this.gmailfiles || null);
      this.formDataObject.append("gmail_mail_from", this.gmailform.gmail_mail_from);
      this.formDataObject.append("gmail_sub", this.gmailform.gmail_sub);
      this.formDataObject.append("gmail_to_mail", this.gmailform.gmail_to_mail);
      this.formDataObject.append("gmail_body", this.gmailform.gmail_body);
      this.formDataObject.append("leadbank_gid", this.gmailform.leadbank_gid);

      this.NgxSpinnerService.show();

      var api7 = 'Leadbank360/Gmailupload'
      this.service.post(api7, this.formDataObject).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.gamilreactiveForm.reset();

          this.NgxSpinnerService.hide();
          this.GetMailSummary();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          // this.router.navigate(['/crm/CrmSmmEmailmanagement']);
          this.ToastrService.success(result.message)
          this.gamilreactiveForm.reset();
          this.NgxSpinnerService.hide();
          this.GetMailSummary();
        }
      });
      this.NgxSpinnerService.hide();


      return;

    }
  }



  config_compose_mail: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '120px',
    minHeight: '0rem',
    width: '420px',
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

  onback() {
    this.router.navigate(['/crm/CrmSmmGmailcampaignsummary']);
  }
  actionmenu() {
    this.showOptions = false;
  }

  openModaledit(params: any) {
    this.reactiveFormContactEdit.get("email_address")?.setValue(params);
    this.reactiveFormContactEdit.get("displayName")?.setValue(this.displayName);
    // this.reactiveFormContactEdit.get("lastname_edit")?.setValue(this.lastname_edit);
    this.reactiveFormContactEdit.get("customertype_edit")?.setValue(this.customertype_edit);
    this.reactiveFormContactEdit.get("customer_type")?.setValue(this.customer_type);
  }

  public onupdatecontact(): void {
    if (this.reactiveFormContactEdit.value.email_address != null && this.reactiveFormContactEdit.value.displayName != null && this.reactiveFormContactEdit.value.customer_type != null) {
      for (const control of Object.keys(this.reactiveFormContactEdit.controls)) {
        this.reactiveFormContactEdit.controls[control].markAsTouched();
      }
      this.reactiveFormContactEdit.value;
      var url = 'GmailCampaign/GmailAddaslead'
      this.service.post(url, this.reactiveFormContactEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveFormContactEdit.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.reactiveFormContactEdit.reset();

        }
        this.reactiveFormContactEdit.reset();
        this.GetMailSummary();

      });

    }
    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  active() {
    this.trash = !this.trash;
  }


}
function encodeToBase64(text: string): string {
  return btoa(text);
}


