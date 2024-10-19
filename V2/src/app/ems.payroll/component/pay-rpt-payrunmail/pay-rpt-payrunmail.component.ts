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
  employee_emailid: string;
  subject: string;
  to_emailid: string;
  body: string;
  bcc: any;
  cc: any;
  reply_to: any;
  attach_file_name:string;
  PDF_Blob:string;

}

@Component({
  selector: 'app-pay-rpt-payrunmail',
  templateUrl: './pay-rpt-payrunmail.component.html',
  styleUrls: ['./pay-rpt-payrunmail.component.scss']
})
export class PayRptPayrunmailComponent {
  safePdfUrl: string ='';
  attach_file_name : string ='';
  mailform:IMailform;
  salary_gid:any;
  formDataObject: FormData = new FormData();
  month:any;
  year:any;
  to_emailid1:any;
  MailForm!:FormGroup;
  GetMailId_list:any[]=[];

  constructor(private router: Router, public NgxSpinnerService: NgxSpinnerService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private service: SocketService,
    private ToastrService: ToastrService) {
    this.mailform = {} as IMailform;

  }
  ngOnInit(): void {
    this.MailForm = new FormGroup({
     
      employee_emailid: new FormControl(this.mailform.employee_emailid, [
        Validators.required,
      ]),
      subject: new FormControl(this.mailform.subject, [
        Validators.required,
      ]),

      file: new FormControl(''),
      body: new FormControl(''),
      bcc: new FormControl(''),
      cc: new FormControl(''), 
      reply_to: new FormControl(''),
      schedule_time: new FormControl(''),
      to_emailid: new FormControl(''),
    });
    const salary_gid = this.route.snapshot.paramMap.get('salary_gid');
    const month = this.route.snapshot.paramMap.get('month');
    const year = this.route.snapshot.paramMap.get('year');
    const to_emailid1 = this.route.snapshot.paramMap.get('to_emailid1');
    this.salary_gid = salary_gid
    this.month = month
    this.year = year
    this.to_emailid1 = to_emailid1
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salary_gid, secretKey).toString(enc.Utf8);
    const deencryptedParam1 = AES.decrypt(this.month, secretKey).toString(enc.Utf8);
    const deencryptedParam2 = AES.decrypt(this.year, secretKey).toString(enc.Utf8);
    const deencryptedParam3 = AES.decrypt(this.to_emailid1, secretKey).toString(enc.Utf8);
    this.MailForm.get("to_emailid")?.setValue(deencryptedParam3);

    //from dropdown
    var api3 = 'PayRptPayrunSummary/GetMailId'
    this.service.get(api3).subscribe((result: any) => {
      this.GetMailId_list = result.GetMailId_list;
    });

    //pdf file attach while page initialize
    const api = 'PayRptPayrunSummary/GetPayslipRpt';
    this.NgxSpinnerService.show()
    let param = {
      salary_gid: deencryptedParam,
      month: deencryptedParam1,
      year: deencryptedParam2
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        const PDF_Blob = this.service.fileattach(result, 'Payslip.pdf');
        this.attach_file_name = 'Payslip.pdf';
        this.formDataObject.append("filename", PDF_Blob, 'Payslip.pdf');
        this.safePdfUrl = URL.createObjectURL(PDF_Blob);
      }
      this.NgxSpinnerService.hide()
    });

  }
  get employee_emailid() {
    return this.MailForm.get('employee_emailid')!;
  }
  get to_emailid() {
    return this.MailForm.get('to_emailid')!;
  }
  get subject() {
    return this.MailForm.get('subject')!;
  }
  get reply_to() {
    return this.MailForm.get('reply_to')!;
  }
  get cc() {
    return this.MailForm.get('cc')!;
  }
  get bcc() {
    return this.MailForm.get('bcc')!;
  }

  onadd(){
    debugger
    this.mailform = this.MailForm.value
    if(this.mailform.employee_emailid != null && this.mailform.to_emailid != null && this.mailform.subject){
      this.formDataObject.append("employee_emailid",this.mailform.employee_emailid);
      this.formDataObject.append("to_emailid",this.mailform.to_emailid);
      this.formDataObject.append("body","Please find the attached payslip for your review");
      this.formDataObject.append("subject",this.mailform.subject);
      this.formDataObject.append("bcc",this.mailform.bcc);
      this.formDataObject.append("cc",this.mailform.cc);

      console.log(this.formDataObject);
      var url="PayRptPayrunSummary/postpayrunmail";
      this.NgxSpinnerService.show();
      this.service.post(url,this.formDataObject).subscribe((result:any)=>{
        if(result.status == false){
          window.scrollTo({
            top:0,
          })
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide()
        }
        else{
          window.scrollTo({
            top:0,
          });
          this.router.navigate(['/payroll/PayRptEmployeesalaryreport'])
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide()
        }
      });
    }
  }
  onback(){
    this.router.navigate(['/payroll/PayRptEmployeesalaryreport']);
  }
}
