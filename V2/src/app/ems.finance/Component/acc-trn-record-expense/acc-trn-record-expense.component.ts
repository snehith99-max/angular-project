import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-acc-trn-record-expense',
  templateUrl: './acc-trn-record-expense.component.html',
  styleUrls: ['./acc-trn-record-expense.component.scss']
})
export class AccTrnRecordExpenseComponent {

  payment:any

  record_data = [
    {
      Expense_date:'20/06/2024',
      vendor:'Nizar Ahamed',
      vendor_gst:'9238023949332',
      contact_person:'naim',
      due_date:'20/07/2024'
    },
    {
      Expense_date:'20/06/2024',
      vendor:'Logapriya',
      vendor_gst:'8234584580933',
      contact_person:'naim',
      due_date:'20/07/2024'
    },
    {
      Expense_date:'18/06/2024',
      vendor:'Sam',
      vendor_gst:'823234580923',
      contact_person:'guna',
      due_date:'20/07/2024'
    },
    {
      Expense_date:'12/06/2024',
      vendor:'Hari',
      vendor_gst:'340209190020',
      contact_person:'Abdul',
      due_date:'20/07/2024'
    },
    {
      Expense_date:'2/06/2024',
      vendor:'Gautham',
      vendor_gst:'9238023949332',
      contact_person:'naim',
      due_date:'20/07/2024'
    },
    {
      Expense_date:'20/06/2024',
      vendor:'Nizar Ahamed',
      vendor_gst:'9238023949332',
      contact_person:'sam andrew',
      due_date:'20/07/2024'
    },
    

  ]

  accountgroup_list = [
    {
      accountgroup_name: 'Bank',
    },
    {
      accountgroup_name: 'Cash',
    },
  ]
  showOptionsDivId: any;
  responsedata: any;
  RecordSummary_list: any;
  parametervalue: any;

  constructor(public service: SocketService, private router: Router, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService,) {
  }

  ngOnInit(): void {
   
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    

    this.GetBankMasterSummary()
  }

  GetBankMasterSummary() {
    var url = 'AccTrnRecordExpenseSummary/RecordExpenseSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.RecordSummary_list = this.responsedata.record_expense_list;
      // setTimeout(() => {
      //   $('#bankmaster_list').DataTable();
      // }, 1);
    });
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

  onroute(params: any) {
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(JSON.stringify(params), secretKey).toString();
    this.router.navigate(['/finance/AccTrnMakePayment', encryptedParam]);

    console.log(params);
}

openModaldelete(gid: any) {
  this.parametervalue = gid
}

ondelete() {
  this.NgxSpinnerService.show();
  var params = {
    expenserequisition_gid: this.parametervalue
  }
  var url = 'AccTrnRecordExpenseSummary/deleteexpensesummary';
  this.service.getparams(url, params).subscribe((result: any) => {
    if (result.status == true) {
      this.NgxSpinnerService.hide();
      this.ToastrService.success(result.message);
      this.ngOnInit();
    }
    else {
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();
    }
  })
} 


}
