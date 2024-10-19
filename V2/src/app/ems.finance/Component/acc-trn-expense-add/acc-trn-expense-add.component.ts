import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-expense-add',
  templateUrl: './acc-trn-expense-add.component.html',
  styleUrls: ['./acc-trn-expense-add.component.scss']
})

export class AccTrnExpenseAddComponent {
  reactiveform: FormGroup | any;
  Productdetails: FormGroup | any;
  vendordetail_list: any[] = [];
  vendor_list: any[] = [];
  accountgroupname_list: any[] = [];
  accountgroup_lists: any[] = [];
  accountgroup_list = [
    {
      accountgroup_name: '',
      accountgroup_gid: '',
    }
  ]
  checklist: any;
  check_list: { remarks: any, accountgroup_name: any, account_name: any, transaction_amount: any, claim_date: any }[] = [];
  sample: any;
  sam: any;
  mdlacc_name: any;
  mdlvendor: any;

  constructor(private formBuilder: FormBuilder, private router: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.reactiveform = new FormGroup({
      expenserequisition_date: new FormControl(null,[Validators.required]),
      vendor: new FormControl('',[Validators.required]),
      due_date: new FormControl('',),
      contactperson_name: new FormControl('',),
      vendor_gst: new FormControl(''),
      vendor_address: new FormControl('')
    })

    this.Productdetails = new FormGroup({
      Account_grp: new FormControl(null,),
      Account_name: new FormControl(null,),
      Amount: new FormControl(null,),
      remarks: new FormControl(null,),
      claim_date: new FormControl(null,),
    })
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.vendordropdown();
    this.Getaccountgroupnamedropdown();
  }

  checklistClick() {
    this.check_list.push({
      remarks: this.Productdetails.get('remarks').value,
      accountgroup_name: this.Productdetails.get('Account_grp').value,
      account_name: this.Productdetails.get('Account_name').value,
      transaction_amount: this.Productdetails.get('Amount').value,
      claim_date: this.Productdetails.get('claim_date').value
    });

    this.Productdetails.get('remarks').setValue('');
    this.Productdetails.get('Account_grp').setValue('');
    this.Productdetails.get('Account_name').setValue('');
    this.Productdetails.get('Amount').setValue('');
    this.Productdetails.get('claim_date').setValue('');
  }

  vendordropdown() {
    var url = "AccTrnRecordExpenseSummary/getvendordropdown";
    this.service.get(url).subscribe((result: any) => {
      this.vendor_list = result.vendor_list
    });
  }

  Getaccountgroupnamedropdown() {
    var url = "AccTrnRecordExpenseSummary/Getaccountgroupnamedropdown";
    this.service.get(url).subscribe((result: any) => {
      this.accountgroupname_list = result.accountgroupname_list
    });
  }
  
  onchangevendor() {
    debugger
    let vendor_gid = this.reactiveform.get("vendor")?.value;
    var url = "AccTrnRecordExpenseSummary/onchangevendordetails";
    let param = {
      vendor_gid: vendor_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.vendordetail_list = result.vendordetails_list;
      // this.sample = result.vendordetail_list[0].gst_number;
      // this.sam = result.vendordetail_list[0].vendor_address
      console.log(this.sample, this.sam)
      this.reactiveform.get("vendor_gst")?.setValue(result.vendordetails_list[0].gst_number);
      this.reactiveform.get("vendor_address")?.setValue(result.vendordetails_list[0].vendor_address);
    });
  }
  
  onchangeaccountgroup() {
    debugger
    let account_gid = this.Productdetails.get("Account_grp")?.value;
    var url = "AccTrnRecordExpenseSummary/onchangeaccountgroup";
    let param = {
      account_gid: account_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.accountgroup_lists = result.accountgroup_lists;
      this.Productdetails.get("Account_name")?.setValue(result.accountgroup_lists[0].account_name);
      console.log(this.accountgroup_lists, 'groupname:')
    });
  }

  onsubmit() {
    this.NgxSpinnerService.show();
    var url = "AccTrnRecordExpenseSummary/PostExpenseDetailsAdd";

    this.service.postparams(url, this.reactiveform.value).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.router.navigate(['/finance/AccTrnRecordExpense']);
      }
    })

    if (this.check_list.length === 0) {
      this.ToastrService.warning("Select Atleast one Employee to Unassign");
      return;
    }
    var param = {
      expensemuladd_list: this.check_list
    }

    var url1 = "AccTrnRecordExpenseSummary/PostExpenseMulAddDtls";

    this.service.postparams(url1, param).subscribe((result: any) => {
    })
    console.log(this.check_list)
    console.log(this.reactiveform.value)
  }

  delete(index: any) {
    if (index >= 0 && index < this.check_list.length) {
      this.check_list.splice(index, 1);
    }
  }

  get expenserequisition_date() {
    return this.reactiveform.get('expenserequisition_date')!;
  }

  get vendor() {
    return this.reactiveform.get('vendor')!;
  }
}
