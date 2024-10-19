import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-journalentry-add',
  templateUrl: './acc-trn-journalentry-add.component.html',
  styleUrls: ['./acc-trn-journalentry-add.component.scss']
})
export class AccTrnJournalentryAddComponent {
  file!: File;
  reactiveform: FormGroup | any;
  journaldetail !: FormArray;
  productdetails_list: any;
  bankmaster_list: any;
  accounttype_list: any;
  accountgroup_list: any;
  branchname_list: any;
  accountname_list: any;
  mdlAccountType: any;
  mdlAccountGroup: any;
  mdlBranchName: any;
  credit_amount: any;
  debit_amount:any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,  private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.reactiveform = new FormGroup({
      remarks: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      created_date: new FormControl(this.getCurrentDate(), Validators.required),
      details: this.formBuilder.array([])
    })
  }


  get created_date() {
    return this.reactiveform.get('created_date')!;
  }
  get branch_name() {
    return this.reactiveform.get('branch_name')!;
  }
  // get accountGroup() {
  //   return this.reactiveform.details.get('accountGroup')!;
  // }
  // get accountName() {
  //   return this.details.get('accountName')!;
  // }
  accountGroup(index: number) {
    return (this.details.controls[index] as FormGroup).get('accountGroup')!;
  }
  accountName(index: number) {
    return (this.details.controls[index] as FormGroup).get('accountName')!;
  }
  debitAmount(index: number) {
    return (this.details.controls[index] as FormGroup).get('debitAmount')!;
  }
  creditAmount(index: number) {
    return (this.details.controls[index] as FormGroup).get('creditAmount')!;
  }
  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    ////Drop Down///
    // var url = 'AccMstBankMaster/GetAccountType'
    // this.service.get(url).subscribe((result: any) => {
    //   this.accounttype_list = result.GetAccountType;
    // });
    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
    });
    var url = 'JournalEntry/GetAccountGroupDetails'
    this.service.get(url).subscribe((result: any) => {
      this.accountgroup_list = result.GetAccountGroupDetails;
    });
    var url = 'JournalEntry/GetAccountNameDetails'
    this.service.get(url).subscribe((result: any) => {
      this.accountname_list = result.GetAccountNameDetails;
    });
    // this.accountgroup_list = [
    //   { account_gid: 'FCOA1909110064', account_name: 'Shobikaa Impex Pvt Ltd' },
    //   { account_gid: 'FCOA2104200013', account_name: 'Shri Ram Kumar' },
    //   { account_gid: 'FCOA2104200014', account_name: 'Shriaura Technolgies' },
    //   { account_gid: 'FCOA1709250040', account_name: 'Shubadrishtiganapathi' },
    //   { account_gid: 'FCOA1709250076', account_name: 'Signetique' },
    //   { account_gid: 'FCOA1710260120 ', account_name: 'Silicon Radio House' }
    // ];
    this.addjournal();

    // var url = 'AccMstBankMaster/GetBranchName'
    // this.service.get(url).subscribe((result: any) => {
    //   this.branchname_list = result.GetBranchName;
    // });
  }
  Generaterow() {
    return this.formBuilder.group({
      accountGroup: this.formBuilder.control(null, Validators.required),
      accountName: this.formBuilder.control(null, Validators.required),
      debitAmount: this.formBuilder.control('',[
        Validators.required,
        Validators.pattern(/^\d*\.?\d+$/)]),
      creditAmount: this.formBuilder.control('', [
        Validators.required,
        Validators.pattern(/^\d*\.?\d+$/)]),
      particulars: this.formBuilder.control(''),

    });
  }
  addjournal() {
    //this.details = this.reactiveform.get("details") as FormArray;

    // let customercode = this.invoiceform.get("customerId")?.value;
    // if ((customercode != null && customercode != '')  || this.isedit) {
    this.details.push(this.Generaterow());
    // } else {
    //   this.alert.warning('Please select the customer', 'Validation');
    // }
  }
  Removejournal(index: any) {

    if (this.details.length != 1) {
      this.details.removeAt(index);
      // this.summarycalculation();
    }
    else {
      this.details.reset();
    }
  }
  get details() {
    return this.reactiveform.get("details") as FormArray;
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  redirecttolist() {
    this.router.navigate(['/finance/AccTrnJournalentrySummary'])
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
    const selectedFile: File = event.target.files[0];
    if (selectedFile) {
      //console.log('Selected file:', selectedFile);
      
      // Add your file handling logic here (e.g., validate file type, upload file)
      // Example: Validate file type
      const allowedFileTypes = ['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'image/png', 'image/jpeg', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/pdf', 'text/plain'];
      const isValidFileType = allowedFileTypes.includes(selectedFile.type);

      if (!isValidFileType) {
        this.ToastrService.warning('Invalid File Format,Kindly Select a Valid File Format !!')     
        return;
      }
    }
  }
  onsubmit() {

    if (this.file != null && this.file != undefined) {
      const selectedFile: File = this.file;
      const allowedFileTypes = ['application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', 'image/png', 'image/jpeg', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'application/pdf', 'text/plain'];
      const isValidFileType = allowedFileTypes.includes(selectedFile.type);
      if (!isValidFileType) {
        this.ToastrService.warning('Invalid File Format,Kindly Select a Valid File Format !!')     
        return;
      }
      else 
      {
      let formData = new FormData();
      //console.log(this.file)
      formData.append("file", this.file, this.file.name);
      formData.append("branch_name", this.reactiveform.value.branch_name);
      formData.append("created_date", this.reactiveform.value.created_date);
      formData.append("remarks", this.reactiveform.value.remarks);
      formData.append("details", JSON.stringify(this.reactiveform.value.details));
      this.NgxSpinnerService.show();
      var api = 'JournalEntry/PostJournalEntryWithDoc'
      this.service.postfile(api, formData).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.router.navigate(['/finance/AccTrnJournalentrySummary'])
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
        }
      });
    }
    }
    else {

     // console.log(this.reactiveform.value)
     this.NgxSpinnerService.show();
      var url = 'JournalEntry/PostJournalEntry'
      this.service.post(url, this.reactiveform.value).subscribe((result: any) => {
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          // this.GetMailTemplateSendSummary();
          // this.reactiveForm.reset();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.reactiveform.reset();
          this.ToastrService.success(result.message);
           this.router.navigate(['/finance/AccTrnJournalentrySummary'])
           this.NgxSpinnerService.hide();

        }
        // this.GetMailTemplateSendSummary();
        // this.reactiveForm.reset();

      });
    }
  }


  changeaccountgroup(i: number) {
    var url = 'JournalEntry/GetAccountNameBaseonGroup'
    let param = {
      accountGroup: this.reactiveform.value.details[i].accountGroup
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.accountname_list = result.GetAccountNameDetails;
    });

    // console.log(this.reactiveform.value.details[i].accountGroup)
  }
  onDebitAmountChange(index: number): void {
   console.log(this.reactiveform.value.details[index].debitAmount);
  if(this.reactiveform.value.details[index].debitAmount !=0  || this.reactiveform.value.details[index].debitAmount !=0.00  )
  {
    const detailsArray = this.reactiveform.get('details') as FormArray;
    const creditAmountControl = detailsArray.at(index).get('creditAmount');
    creditAmountControl?.setValue('0.00' ?? null);  
  }
}
  onCreditAmountChange(index: number): void {
    console.log(this.reactiveform.value.details[index].creditAmount);
   if(this.reactiveform.value.details[index].creditAmount !=0  || this.reactiveform.value.details[index].creditAmount !=0.00  )
   {
     const detailsArray = this.reactiveform.get('details') as FormArray;
     const debitAmountControl = detailsArray.at(index).get('debitAmount');
     debitAmountControl?.setValue('0.00' ?? null); 
   }
}
 
}
