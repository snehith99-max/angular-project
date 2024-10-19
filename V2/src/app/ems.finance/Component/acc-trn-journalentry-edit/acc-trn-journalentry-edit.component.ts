import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-acc-trn-journalentry-edit',
  templateUrl: './acc-trn-journalentry-edit.component.html',
  styleUrls: ['./acc-trn-journalentry-edit.component.scss']
})
export class AccTrnJournalentryEditComponent {
  journal_gid:any;
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
  GetJournalEntry_lists:any;
  selectedbranch:any;
  deletedtl_gid:any;
  GetJournalEntryeditdtl_lists:any;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,  private router: Router, private ToastrService: ToastrService,private route: ActivatedRoute, public service: SocketService) {
    this.reactiveform = new FormGroup({
      remarks: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      created_date: new FormControl(this.getCurrentDate(), Validators.required),
      journal_gid: new FormControl(''),
      journal_refno: new FormControl(''),
      detailsedit: this.formBuilder.array([])
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
    return (this.detailsedit.controls[index] as FormGroup).get('accountGroup')!;
  }
  accountName(index: number) {
    return (this.detailsedit.controls[index] as FormGroup).get('accountName')!;
  }
  debitAmount(index: number) {
    return (this.detailsedit.controls[index] as FormGroup).get('debitAmount')!;
  }
  creditAmount(index: number) {
    return (this.detailsedit.controls[index] as FormGroup).get('creditAmount')!;
  }
  ngOnInit(): void {
    const journal_gid = this.route.snapshot.paramMap.get('journal_gid');
    this.journal_gid = journal_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.journal_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

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
   // this.addjournal();

    this.GetEditJournalSummary(deencryptedParam)
  }
  GetEditJournalSummary(journal_gid: any) {
    //console.log(journal_gid)
    var url = 'JournalEntry/GetJournalEntrySummaryEdit'
    let param = {journal_gid : journal_gid}
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetJournalEntry_lists = result.GetJournalEntryedit_lists;
     // console.log(this.GetJournalEntry_lists)

       this.reactiveform.get("remarks")?.setValue(this.GetJournalEntry_lists[0].remarks);
      this.reactiveform.get("created_date")?.setValue(this.GetJournalEntry_lists[0].transaction_date);
      this.reactiveform.get("journal_gid")?.setValue(this.GetJournalEntry_lists[0].journal_gid);
      this.reactiveform.get("journal_refno")?.setValue(this.GetJournalEntry_lists[0].journal_refno);    
      this.selectedbranch = this.GetJournalEntry_lists[0].branch_gid;
  
      
    });
    var url = 'JournalEntry/GetJournalEntrySummaryEditDtl'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.GetJournalEntryeditdtl_lists = result.GetJournalEntryeditdtl_lists;
      //console.log(this.GetJournalEntryeditdtl_lists)

      //  this.reactiveform.get("remarks")?.setValue(this.GetJournalEntry_lists[0].remarks);
      // this.reactiveform.get("created_date")?.setValue(this.GetJournalEntry_lists[0].transaction_date);
      // this.selectedbranch = this.GetJournalEntry_lists[0].branch_gid;
      for (let i = 0; i < this.GetJournalEntryeditdtl_lists.length; i++) {
        const journalEntry = this.GetJournalEntryeditdtl_lists[i];

      // Create a new form group for each journal entry
      const newFormGroup = this.formBuilder.group({
        accountGroup: [journalEntry.accountGroup, Validators.required],
        accountName: [journalEntry.accountName, Validators.required],
        debitAmount: [journalEntry.debitAmount, Validators.required],
        creditAmount: [journalEntry.creditAmount, Validators.required],
        particulars: [journalEntry.particulars],
        journaldtl_gid: [journalEntry.journaldtl_gid]
      });

      // Push the new form group into the details array
      this.detailsedit.push(newFormGroup);
      };
      
    });
  } 
  Generaterow() {
    return this.formBuilder.group({
      accountGroup: this.formBuilder.control(null, Validators.required),
      accountName: this.formBuilder.control(null, Validators.required),
      debitAmount: this.formBuilder.control('',[
        Validators.required,
        Validators.pattern(/^\d*\.?\d+$/)]),
      creditAmount: this.formBuilder.control('',[
        Validators.required,
        Validators.pattern(/^\d*\.?\d+$/)]),
      particulars: this.formBuilder.control(''),
      journaldtl_gid: this.formBuilder.control(''),

    });
  }
  addjournal() {
    //this.details = this.reactiveform.get("details") as FormArray;

    // let customercode = this.invoiceform.get("customerId")?.value;
    // if ((customercode != null && customercode != '')  || this.isedit) {
    this.detailsedit.push(this.Generaterow());
    // } else {
    //   this.alert.warning('Please select the customer', 'Validation');
    // }
  }
  Removejournal(index: any,item:any) {
  //console.log('remove list',item.value.journaldtl_gid)
    if (this.detailsedit.length != 1) {
      this.deletedtl_gid =item.value.journaldtl_gid;
      //this.detailsedit.removeAt(index);
      // this.summarycalculation();
    
      
    }
    else {
      this.detailsedit.reset();
    }
  }
  ondelete()
  {
    this.NgxSpinnerService.show();
    var url = 'JournalEntry/DeleteJounralDtlEntry'
    let param = {
      journaldtl_gid: this.deletedtl_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });   
        window.location.reload();
        this.ToastrService.success(result.message)
      }

    });

  }
  get detailsedit() {
    return this.reactiveform.get("detailsedit") as FormArray;
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
   // console.log(this.reactiveform.value)
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
      formData.append("details", JSON.stringify(this.reactiveform.value.detailsedit));
      formData.append("journal_gid", this.reactiveform.value.journal_gid);
      this.NgxSpinnerService.show();
      var api = 'JournalEntry/UpdateJournalEntryWithDoc'
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
      var url = 'JournalEntry/UpdateJournalEntry'
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
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message);
           this.router.navigate(['/finance/AccTrnJournalentrySummary'])
          this.reactiveform.reset();

        }
        // this.GetMailTemplateSendSummary();
        // this.reactiveForm.reset();

      });
    }
  }


  changeaccountgroup(i: number) {
    var url = 'JournalEntry/GetAccountNameBaseonGroup'
    let param = {
      accountGroup: this.reactiveform.value.detailsedit[i].accountGroup
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.accountname_list = result.GetAccountNameDetails;
    });

    // console.log(this.reactiveform.value.detailsedit[i].accountGroup)
  }
  onDebitAmountChange(index: number): void {
   console.log(this.reactiveform.value.detailsedit[index].debitAmount);
  if(this.reactiveform.value.detailsedit[index].debitAmount !=0  || this.reactiveform.value.detailsedit[index].debitAmount !=0.00  )
  {
    const detailsArray = this.reactiveform.get('detailsedit') as FormArray;
    const creditAmountControl = detailsArray.at(index).get('creditAmount');
    creditAmountControl?.setValue('0.00' ?? null);  
  }
}
  onCreditAmountChange(index: number): void {
    console.log(this.reactiveform.value.detailsedit[index].creditAmount);
   if(this.reactiveform.value.detailsedit[index].creditAmount !=0  || this.reactiveform.value.detailsedit[index].creditAmount !=0.00  )
   {
     const detailsArray = this.reactiveform.get('detailsedit') as FormArray;
     const debitAmountControl = detailsArray.at(index).get('debitAmount');
     debitAmountControl?.setValue('0.00' ?? null); 
   }
}
 
}
