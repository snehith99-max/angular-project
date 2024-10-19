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
  selector: 'app-smr-trn-quotationmail',
  templateUrl: './smr-trn-quotationmail.component.html',
  styleUrls: ['./smr-trn-quotationmail.component.scss']
})
export class SmrTrnQuotationmailComponent {
  
  file!: File;
  file1!: FileList;
  file_name: any;
  frommailid:any;
  responsedata: any;
  formDataObject: FormData = new FormData();
  invoicemailform!: FormGroup;
  GetMailId_list: any[] = [];
  mailform!: IMailform;
  quotation_gid:any;
  templatelist :any[] = [];
  branchList: any[] = [];
  designation_list: any[] = [];
  country_list2: any[] = [];
  mail_form: any;
  template_content: any[] = [];
  GetTemplatelist: any;
  body: any;
  subsymbol:any;
  subcompanyname:any;
  template_gid1:any;
  deencryptedParam:any;
  binaryContent:any;
  result : any
  Templateterm: any;
  files: File[] = [];
  allattchement: any[] = [];
  AutoIDkey: any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '1140px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',

  };
  isDropdownOpen = false;
  mail_list:any[]=[];
  to_mail_list: any;
  mdlCustomer: any;
  invoice_gid: any;
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
debugger
    const quotation_gid = this.route.snapshot.paramMap.get('quotation_gid');
    this.quotation_gid = quotation_gid;
    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.quotation_gid, secretKey).toString(enc.Utf8);
   


    var api='SmrTrnQuotation/GetTemplatelist'
    this.service.get(api).subscribe((result:any)=>{
    this.templatelist = result.templatelist;
    
    this.invoicemailform.get("template_name")?.setValue(this.templatelist[0].template_name);
    
    this.invoicemailform.get("template_content")?.setValue(this.templatelist[0].template_content);
    
    });


    var url = 'SmrRptInvoiceReport/Getfrommailid';
    this.service.get(url).subscribe((result: any) => {
  debugger
    this.frommailid = result.frommailid;
this.invoicemailform.get("employee_emailid")?.setValue(this.frommailid);
});





 const api2 = 'SmrTrnQuotation/GetQuotationRpt';
 this.NgxSpinnerService.show()
 let params = {
  quotation_gid: this.deencryptedParam
 }
 this.service.getparams(api2, params).subscribe((result: any) => {
   if (result != null) {
    this.binaryContent=result;
     const PDF_Blob = this.service.fileattach(result, 'Quotation.pdf');
     this.attach_file_name = 'Quotation.pdf';
     this.formDataObject.append("filename", PDF_Blob, 'Quotation.pdf');
     this.safePdfUrl = URL.createObjectURL(PDF_Blob);
     this.NgxSpinnerService.hide();
   }
   this.NgxSpinnerService.hide()
 });
 
  let param = {
    mail_invoice_gid: this.deencryptedParam
  };
  
  var url = 'SmrTrnQuotation/GetSendMail_MailId';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.to_mail_list = result.GetSendMail_MailIdquot;
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
  
    this.invoicemailform.get("sub")?.setValue( this.subcompanyname +' Quotation ' + this.to_mail_list[0].quotation_reference + ' For ' +this.subsymbol+ + this.to_mail_list[0].total_amount);
  });
  
 

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
        console.log(this.file1[i]);
      }
    }
  
  
  }

  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }
  oncleartemplate(){
    this.invoicemailform.get("template_content")?.setValue(null);
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
    debugger;
    console.log(this.invoicemailform.value)

    this.mailform = this.invoicemailform.value;
    if ( this.mailform.employee_emailid != null && this.mailform.sub != null && this.mailform.to != null) {

     
      const allattchement = "" + JSON.stringify(this.binaryContent) + "";
      this.formDataObject.append("gmailfiles",allattchement);
        this.formDataObject.append("employee_emailid", this.mailform.employee_emailid);
        this.formDataObject.append("sub", this.mailform.sub);
        this.formDataObject.append("to", this.mailform.to);        
        this.formDataObject.append("body", this.mailform.template_content);
        this.formDataObject.append("bcc", this.mailform.bcc);
        this.formDataObject.append("cc", this.mailform.cc);
        this.formDataObject.append("quotation_gid", this.deencryptedParam);


        

        var api7 = 'SmrTrnQuotation/PostMail'
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
            this.router.navigate(['/smr/SmrTrnQuotationSummary']);
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


