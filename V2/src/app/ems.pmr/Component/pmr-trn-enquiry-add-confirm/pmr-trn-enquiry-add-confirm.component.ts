import { Component, OnInit, OnDestroy, ChangeDetectorRef, Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { NgbTimepickerModule, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgModel } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
// import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AngularEditorConfig } from '@kolkov/angular-editor';
export class IEmployee {
  enquiryaddprodeed_list: string[] = [];
  employee_gid:any;
}

@Component({
  selector: 'app-pmr-trn-enquiry-add-confirm',
  templateUrl: './pmr-trn-enquiry-add-confirm.component.html',
  styleUrls: ['./pmr-trn-enquiry-add-confirm.component.scss']
})

export class PmrTrnEnquiryAddConfirmComponent    {
  enquiryaddconfirm_list: any[] = [];
  GetEnquiryaddConfirm1:any[]=[];
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '1150px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',

  };
  enquiryaddprodeed_list1: any[] = [];
  select_list: any[] = [];
  detailsdtl_list: any[] = [];
  reactiveForm!: FormGroup;
  selection = new SelectionModel<IEmployee>(true, []);
  monthyear: any;
  month:any;
  year:any;
  working_days: any;
  purchaserequisition_gid:any;
  data: any;
  Enquiry_Date: any;
  
  


  
  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    public NgxSpinnerService:NgxSpinnerService,
    
    
       )
   { 
  
   }
   ngOnInit(): void {
    const purchaserequisition_gid = this.router.snapshot.paramMap.get('purchaserequisition_gid');
    this.purchaserequisition_gid = purchaserequisition_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.purchaserequisition_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

    this.GetEnquiryaddconfirm(deencryptedParam);
    this.GetEnquiryaddconfirm1(deencryptedParam);
    this.reactiveForm = this.formBuilder.group({
      msGetGID3: new FormControl(''),
      employee_mobileno : new FormControl (''),
      Enquiry_Date: new FormControl(this.getCurrentDate()),
      employee_name: new FormControl(''),
      employee_emailid: new FormControl(''),
      template_content: new FormControl(''),
      remarks: new FormControl(''),
     
    });
   
    

  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
   
    return dd + '-' + mm + '-' + yyyy;
  }
  GetEnquiryaddconfirm(param1 :any) {
    this.NgxSpinnerService.show();
    var url = 'PmrTrnRequestforQuote/GetEnquiryaddConfirm';
   
    let param = {
      purchaserequisition_gid : param1 
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      this.enquiryaddconfirm_list = result.GetEnquiryaddConfirm;
      this.NgxSpinnerService.hide();
    });
  }
  GetEnquiryaddconfirm1(param1: any) {
    this.NgxSpinnerService.show();
    var url = 'PmrTrnRequestforQuote/GetEnquiryaddConfirm1';
    let param = {
      purchaserequisition_gid: param1
    };
    this.service.getparams(url, param).subscribe((result: any) => {
  
      this.GetEnquiryaddConfirm1 = result.GetEnquiryaddConfirm1;
      this.reactiveForm.get("employee_name")?.setValue(this.GetEnquiryaddConfirm1[0].employee_name);
      this.reactiveForm.get("msGetGID3")?.setValue(this.GetEnquiryaddConfirm1[0].msGetGID3);
      this.reactiveForm.get("employee_emailid")?.setValue(this.GetEnquiryaddConfirm1[0].employee_emailid);
      this.reactiveForm.get("employee_mobileno")?.setValue(this.GetEnquiryaddConfirm1[0].employee_mobileno);
      // this.reactiveForm.get("Enquiry_Date")?.setValue(this.GetEnquiryaddConfirm1[0].Enquiry_Date);
      const Enquiry_Date = new Date(this.GetEnquiryaddConfirm1[0].Enquiry_Date);
      const formattedDate = Enquiry_Date.toISOString().substring(0, 10);
      this.reactiveForm.get("Enquiry_Date")?.setValue(formattedDate);

      this.NgxSpinnerService.hide();
  
  
    });
  }
  



  submit() {
    const secretKey = 'storyboarderp';
    const purchaserequisition_gid = AES.encrypt(this.purchaserequisition_gid, secretKey).toString();
   

    const selectedData = this.selection.selected; // Get the selected items
   
    for (const data of selectedData) {
      this.enquiryaddconfirm_list.push(data);
 }

   var params={
    Enquiry_Date: this.reactiveForm.value.Enquiry_Date,
    msGetGID3: this.reactiveForm.value.msGetGID3,
    employee_name: this.reactiveForm.value.employee_name,
    employee_mobileno: this.reactiveForm.value.employee_mobileno,
    employee_emailid: this.reactiveForm.value.employee_emailid,
    remarks: this.reactiveForm.value.remarks,

    enquiryaddconfirm_list :this.enquiryaddconfirm_list
   }
   var url = 'PmrTrnRequestforQuote/GetEnquiryproceedConfirm'
  this.NgxSpinnerService.show();
    this.service.postparams(url,params).subscribe((result: any) => {
      
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
     }
     else{
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.route.navigate(['/pmr/PmrTrnRequestForQuoteSummary']);
     }
    });

  }
 
}