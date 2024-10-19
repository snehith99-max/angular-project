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


@Component({
  selector: 'app-smr-mst-customereportalpreviewmail',
  templateUrl: './smr-mst-customereportalpreviewmail.component.html',
  styleUrls: ['./smr-mst-customereportalpreviewmail.component.scss']
})
export class SmrMstCustomereportalpreviewmailComponent {
  customer_gid: any;
  eportalemail_id: any;
  MailForm!:FormGroup;
  password: any;
  base64EncodedText:any;
  frommailid: any;
  mailcontent:any;
  default_template:any;
  deencryptedParam:any;
  deencryptedParam1:any;
  deencryptedParam2:any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: false,
    height: '20rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
  showToolbar:false,
enableToolbar:false,
  };

  constructor(private router: Router, public NgxSpinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private service: SocketService,
    private ToastrService: ToastrService) { }

  ngOnInit() {

    this.MailForm = new FormGroup({
      to:new FormControl(''),
      subject: new FormControl('',Validators.required),
      body: new FormControl('',Validators.required),
          });
    debugger
    const customer_gid = this.route.snapshot.paramMap.get('customer_gid');
    const eportalemail_id = this.route.snapshot.paramMap.get('eportalemail_id');
    const password = this.route.snapshot.paramMap.get('password');

    this.customer_gid = customer_gid
    this.eportalemail_id = eportalemail_id
    this.password = password

    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.customer_gid, secretKey).toString(enc.Utf8);
    this.deencryptedParam1 = AES.decrypt(this.eportalemail_id, secretKey).toString(enc.Utf8);
    this.deencryptedParam2 = AES.decrypt(this.password, secretKey).toString(enc.Utf8);

    
    this.mailcontent=`<p>Dear Sir/Madam,</p>
    <p>Greetings,</p>
    <p>Please find below the details of the login credentials for My Orders.</p>
    <p contenteditable="false">Email: ${this.deencryptedParam1 }</p>
    <p contenteditable="false">Password: ${this.deencryptedParam2 }</p>
    <p contenteditable="false"><b>Login URL:</b> <a href="https://myorders.storyboardsystems.com">click here to Login</a></p>
    <p>If you did not receive any credentials, please contact our support team immediately at <a href="mailto:info@bobateacompany.co.uk">info@bobateacompany.co.uk</a>.</p>
    <p>Thank you for your prompt attention to this matter.</p>`;

    this.MailForm.get("to")?.setValue(this.deencryptedParam1)
    this.MailForm.get("subject")?.setValue('Login Crediental For My Orders');
    this.MailForm.get("body")?.setValue(this.mailcontent)


    var url = 'SmrRptInvoiceReport/Getfrommailid';
this.service.get(url).subscribe((result: any) => {
this.frommailid = result.frommailid;
});

var url = 'SmrTrnCustomerSummary/getdefaulttemplate';
this.service.get(url).subscribe((result: any) => {
this.default_template = result.default_template;
});


  }
  get subject(){
    return this.MailForm.get('subject')!;
  }
  get to(){
    return this.MailForm.get('to')!;
  }
 sendemail(){
  var url = "SmrTrnCustomerSummary/eportalmail";

  const originalText =
        'From: ' + this.frommailid + '\r\nTo: ' + this.deencryptedParam1 +'\r\nSubject:' + this.MailForm.value.subject + '\r\nContent-Type: text/html\r\n\r\n' + this.MailForm.value.body + '\r\n' + this.default_template + '';
      this.base64EncodedText = encodeToBase64(originalText);

  let params={
   customer_gid:this.deencryptedParam,
   eportalemail_id:this.deencryptedParam1,
   confirmpassword:this.deencryptedParam2,
   subject:this.MailForm.value.subject,
   body:this.MailForm.value.body,
   fullcontent:this.base64EncodedText
   

  }
    this.NgxSpinnerService.show()
    this.service.postparams(url,params).subscribe((result:any)=>{
      if(result.status == true){
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
        this.router.navigate(['/smr/SmrTrnCustomerSummary'])
      }
      else{
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
    })
 }
 onback(){
  this.router.navigate(['/smr/SmrTrnCustomerSummary'])
 }
}

function encodeToBase64(text: string): string {
  return btoa(text);
}
