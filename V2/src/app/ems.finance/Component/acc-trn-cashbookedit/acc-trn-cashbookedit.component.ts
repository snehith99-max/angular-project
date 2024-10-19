import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, MinLengthValidator, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute } from '@angular/router';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

// interface IVendor {
//   branch_code: string;
//   openning_balance: string;
//   externalgl_code: string;
//   gl_code: string;
//   transaction_date: string;
//   remarks: string;
//   branch_name: string;

// }

@Component({
  selector: 'app-acc-trn-cashbookedit',
  templateUrl: './acc-trn-cashbookedit.component.html',
  styleUrls: ['./acc-trn-cashbookedit.component.scss']
})
export class AccTrnCashbookeditComponent {

  // vendor!: IVendor;
  reactiveForm: FormGroup | any;
  country_list: any[] = [];
  currency_list: any[] = [];
  tax_list: any[] = [];
  responsedata: any;
  selectedVendorcode: any;
  branch_code: any;
  CashBookedit_list: any;
  GetAccount_List: any;
  cashbookdtl_List: any;
  branch_gid: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService) {
    // this.vendor = {} as IVendor;
    // this.formvalidator();
  }

  // formvalidator() {
  //   this.reactiveForm = this.FormBuilder.group({
  //     parent_name: ['Cash on Hand']
  //   })
  // }

  ngOnInit(): void {

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    const branch_code = this.router.snapshot.paramMap.get('branch_gid');
    // console.log(termsconditions_gid)
    this.branch_code = branch_code;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.branch_code, secretKey).toString(enc.Utf8);
    this.branch_gid = deencryptedParam;
    // console.log(deencryptedParam);
    this.GetCashBookedit(deencryptedParam);

    this.reactiveForm = new FormGroup({
      branch_code: new FormControl(null),
      branch_name: new FormControl(null),
      parent_name: new FormControl(null),
      gl_code: new FormControl(null),      
      externalgl_code: new FormControl(null,Validators.required),
      transaction_date: new FormControl(null,Validators.required),
      openning_balance: new FormControl(null,Validators.required),     
      remarks: new FormControl(null),
      branch_gid:new FormControl(deencryptedParam),
    });

    var url = 'AccTrnBankbooksummary/GetAccountNameList'
    this.service.get(url).subscribe((result: any) => {
      this.GetAccount_List = result.GetAccount_List;
    });
  }

  GetCashBookedit(branch_gid: any) {
    debugger
    this.NgxSpinnerService.show();
    var url1 = 'AccTrnCashBookSummary/GetCashBookDtlEdit'
    let param = {
      branch_gid: branch_gid
    }
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.cashbookdtl_List = result.Getcashbookdtl_List;
      this.NgxSpinnerService.hide();
      console.log(this.cashbookdtl_List)      
      this.reactiveForm.get("openning_balance")?.setValue(this.cashbookdtl_List[0].openning_balance);
      this.reactiveForm.get("externalgl_code")?.setValue(this.cashbookdtl_List[0].externalgl_code);
      this.reactiveForm.get("transaction_date")?.setValue(this.cashbookdtl_List[0].transaction_date);
      this.reactiveForm.get("remarks")?.setValue(this.cashbookdtl_List[0].remarks);
    });
  }
  // onChange2(event: any) {
  //   this.reactiveForm = event.target.files[0];
  // }
  get externalgl_code() {
    return this.reactiveForm.get('externalgl_code')!;
  }
  get transaction_date() {
    return this.reactiveForm.get('transaction_date')!;
  }
  get openning_balance() {
    return this.reactiveForm.get('openning_balance')!;
  }


  update() {   
    this.reactiveForm.value;  
    if (this.reactiveForm.status == 'VALID') { 
      this.NgxSpinnerService.show();
      var url = 'AccTrnCashBookSummary/UpdateCashBookDtls';
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetCashBookedit(this.branch_gid);
        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)          
          this.route.navigate(['/finance/AccTrnCashbooksummary']);
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else{}

  }

  back() {
    this.route.navigate(['/finance/AccTrnCashbooksummary']);
  }

  // public validate(): void {
  //   debugger
  //   console.log(this.reactiveForm.value)
  //   // this.vendor = this.reactiveForm.value;
  //   // debugger
  //   // if (this.vendor.openning_balance != null && this.vendor.transaction_date != null &&
  //   //   this.vendor.externalgl_code != null && this.vendor.branch_name != null
  //   //   && this.vendor.remarks != null) {
  //   //   let formData = new FormData();
  //   //   if (this.reactiveForm != null && this.reactiveForm != undefined) {

  //   //     formData.append("branch_code", this.vendor.branch_code);
  //   //     formData.append("openning_balance", this.vendor.openning_balance);
  //   //     formData.append("externalgl_code", this.vendor.externalgl_code);
  //   //     formData.append("gl_code", this.vendor.gl_code);
  //   //     formData.append("transaction_date", this.vendor.transaction_date);
  //   //     formData.append("branch_name", this.vendor.branch_name);
  //   //     formData.append("remarks", this.vendor.remarks);

  //   //     var api = 'AccTrnCashBookSummary/PostCashBookUpdate'
  //   //     this.service.postfile(api, formData).subscribe((result: any) => {
  //   //       this.responsedata = result;
  //   //       if (result.status == false) {
  //   //         this.ToastrService.warning(result.message)
  //   //       }
  //   //       else {
  //   //         this.route.navigate(['/finance/AccTrnCashbooksummary']);
  //   //         this.ToastrService.success(result.message)
  //   //       }
  //   //     });

  //   //   }
  //   //   // else{
  //   //   //   var api7='AccTrnCashBookSummary/PostCashBookUpdate'

  //   //   //     this.service.post(api7,this.vendor).subscribe((result:any) => {

  //   //   //       if(result.status ==false){
  //   //   //         this.ToastrService.warning(result.message)
  //   //   //       }
  //   //   //       else{
  //   //   //         this.route.navigate(['/finance/AccTrnCashbooksummary']);
  //   //   //         this.ToastrService.success(result.message)
  //   //   //       }
  //   //   //       this.responsedata=result;
  //   //   //     });
  //   //   // }
  //   // }
  //   // else {
  //   //   this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   // }
  //   // return;


  // } 

}

