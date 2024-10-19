
import { Component } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { AngularEditorComponent } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';

interface IMailform {
  employee_emailid: null;
  mail_from: string;
  sub: string;
  to: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  template_content:string;


}

@Component({
  selector: 'app-smr-trn-invoicemail',
  templateUrl: './smr-trn-invoicemail.component.html',
  styleUrls: ['./smr-trn-invoicemail.component.scss']
})
export class SmrTrnInvoicemailComponent {
  
  file!: File;
  file1!: FileList;
  file_name: any;
  responsedata: any;
  formDataObject: FormData = new FormData();
  invoicemailform!: FormGroup;
  GetMailId_list: any[] = [];
  mailform!: IMailform;
  quotation_gid:any;
  gmailfiles!: FileList;
  templatelist :any[] = [];
  branchList: any[] = [];
  designation_list: any[] = [];
  country_list2: any[] = [];
  suggestedContacts: any[] = [];
  mail_form: any;
  template_content: any[] = [];
  Gettomailid: any[] = [];
  Gettomailidcall: {email:string}[] = [];
  GetTemplatelist: any;
  body: any;
  template_gid1: any;
  subcompanyname:any;
  toField: any[]=[];
  mailcontent:any;
  binaryContent:any;
  result : any;
  subsymbol:any;
  frommailid : any
  Templateterm: any;
  selectedContacts: { name: string, email: string,leadbank_gid: string }[] = [];
  files: File[] = [];
  allattchement: any[] = [];
  AutoIDkey: any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '685px',
    defaultFontSize: '2',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',

  };
  isDropdownOpen = false;
  mail_list:any[]=[];
  shopifyInvoice:any;
  to_mail_list: any;
  mdlCustomer: any;
  invoice_gid: any;
  deencryptedParam:any;
  attach_file_name: any;
  safePdfUrl: any;

  constructor(private router: Router,
    public NgxSpinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private fb: FormBuilder, 
    private service: SocketService,
    private ToastrService: ToastrService  ) {
    this.mailform = {} as IMailform;

  }

  ngOnInit(): void {
    
    const invoice_gid = this.route.snapshot.paramMap.get('invoice_gid');
    this.invoice_gid = invoice_gid;
    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.invoice_gid, secretKey).toString(enc.Utf8);

    this.invoicemailform = new FormGroup({
      template_gid:new FormControl(''),
      template_name:new FormControl(''),
      template_content: new FormControl(''),
      employee_emailid: new FormControl(this.mailform.employee_emailid, [
        Validators.required,
      ]),
      sub: new FormControl(this.mailform.sub, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''),
      reply_to: new FormControl(''),
      schedule_time: new FormControl(''),
      to: new FormControl(this.mailform.to, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(250), Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')
      
      ]),
    });
  
    
var api='SmrTrnQuotation/GetTemplatelist'
this.service.get(api).subscribe((result:any)=>{
this.templatelist = result.templatelist;

this.invoicemailform.get("template_name")?.setValue(this.templatelist[0].template_name);

this.invoicemailform.get("template_content")?.setValue(this.templatelist[0].template_content);

});


 const api2 = 'SmrRptInvoiceReport/GetInvoicePDF';
 this.NgxSpinnerService.show()
 let params = {
  invoice_gid:this.deencryptedParam
 }
 this.service.getparams(api2, params).subscribe((result: any) => {
   if (result != null) {
    console.log(result)
    this.binaryContent=result;
    
     const PDF_Blob = this.service.fileattach(result, 'Invoice')
     this.attach_file_name = result.name;
     this.formDataObject.append("filename", PDF_Blob, this.attach_file_name);
     this.safePdfUrl = URL.createObjectURL(PDF_Blob);
     this.NgxSpinnerService.hide();
   }
   this.NgxSpinnerService.hide()
 });




var url = 'SmrRptInvoiceReport/Getfrommailid';
this.service.get(url).subscribe((result: any) => {
  debugger
this.frommailid = result.frommailid;
this.invoicemailform.get("employee_emailid")?.setValue(this.frommailid);
});






  let param = {
    mail_invoice_gid: this.deencryptedParam
  };
  
  var url = 'SmrRptInvoiceReport/GetSendMail_MailId';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.to_mail_list = result.GetSendMail_MailId;
    this.subcompanyname = result.subcompanyname;
    this.subsymbol = result.subsymbol;
  
    this.invoicemailform.get("to")?.setValue(this.to_mail_list[0].to_customer_email);

    if(this.to_mail_list[0].to_customer_email == this.to_mail_list[0].cc_contact_emails)
    {
      this.invoicemailform.get("cc")?.setValue("");
    }
  else
  {
    this.invoicemailform.get("cc")?.setValue(this.to_mail_list[0].cc_contact_emails);
  }
  
    this.invoicemailform.get("sub")?.setValue( this.subcompanyname +' Invoice ' + this.to_mail_list[0].invoice_reference + ' For ' +this.subsymbol+ + this.to_mail_list[0].total_amount + ' due on ' + this.to_mail_list[0].due_date);
  });
  
 

    

  





    this.mailcontent = `<p>Dear Sir/Madam,</p>
    
    <p>Please find below the details of your invoice from BOBATEACOMPANY LTD.</p>
    <p>Full details, including payment terms, are included in the attached PDF.</p><br>
    <p>If you have any questions, please don't hesitate to contact us.</p><br>`;
    

    // this.invoicemailform.get("template_content")?.setValue(this.mailcontent);

  }


  
  onChange2($event: any): void {
    this.file1 = $event.target.files;
  
    if (this.file1 != null && this.file1.length !== 0) {
      for (let i = 0; i < this.file1.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.file1[i]);
         this.file_name = this.file1[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.file1[i].name
        });
        
      }
    }
  
  
  }

  

  generateKey(): string {
    return `AutoIDKey${new Date().getTime()}`;
  }

  GetOnChangeTerms() {
    
    
    let template_name = this.invoicemailform.value.template_name;
    for(let i=0; this.templatelist.length; i++){
      if(this.templatelist[i].template_name === template_name){
       this.template_gid1 = this.templatelist[i].template_gid;
       break;
      }
    }
    let param = {
      template_gid: this.template_gid1
    }
    var url = 'SmrTrnQuotation/GetTemplate';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.invoicemailform.get("template_content")?.setValue(result.templatelist[0].template_content);
      this.invoicemailform.value.template_gid = result.templatelist[0].template_gid
    });
  }
  
  oncleartemplate(){
    this.invoicemailform.get("template_content")?.setValue(null);
  }
 
  sendMessage() {
   
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  get employee_emailid() {
    return this.invoicemailform.get('employee_emailid')!;
  }
  get to() {
    return this.invoicemailform.get('to')!;
  }
  get sub() {
    return this.invoicemailform.get('sub')!;
  }
  get reply_to() {
    return this.invoicemailform.get('reply_to')!;
  }
  get cc() {
    return this.invoicemailform.get('cc')!;
  }
  get bcc() {
    return this.invoicemailform.get('bcc')!;
  }
  public onadd(): void {
    
    console.log(this.invoicemailform.value)

    this.mailform = this.invoicemailform.value;
    if ( this.mailform.employee_emailid != null && this.mailform.sub != null && this.mailform.to != null) {

       const allattchement = "" + JSON.stringify(this.binaryContent) + "";
       this.formDataObject.append("gmailfiles",allattchement);
        this.formDataObject.append("employee_emailid", this.mailform.employee_emailid);
        this.formDataObject.append("sub", this.mailform.sub);
        this.formDataObject.append("to", this.mailform.to);      
        this.formDataObject.append("cc", this.mailform.cc);  
        this.formDataObject.append("body", this.mailform.template_content);
        this.formDataObject.append("invoice_gid", this.deencryptedParam);
        this.formDataObject.append("bcc", this.mailform.bcc);
      


        console.log(this.formDataObject)

        var api7 = 'SmrRptInvoiceReport/PostMail'
        this.NgxSpinnerService.show()
        this.service.post(api7, this.formDataObject).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            window.scrollTo({

              top: 0,
            });
            this.router.navigate(['/smr/SmrTrnInvoiceSummary']);
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
          }
        });
   //}    
    }
    else {
      window.scrollTo({

        top: 0, 
      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    return;
  }
 
  
  public isAttachmentSelected(): boolean {
    return this.file1 != null && this.file1.length >= 0;
}
  

}
