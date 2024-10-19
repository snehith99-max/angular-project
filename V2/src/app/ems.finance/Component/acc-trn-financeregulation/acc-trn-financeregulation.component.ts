import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-acc-trn-financeregulation',
  templateUrl: './acc-trn-financeregulation.component.html',
  styleUrls: ['./acc-trn-financeregulation.component.scss']
})
export class AccTrnFinanceregulationComponent {
  dateform: FormGroup | any;
  currentTab = 'Customer';  
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService
  ) {
    this.dateform = new FormGroup({
    todate: new FormControl(''),
    
    });
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);    
  }
  
  showTab(tab: string) {
      this.currentTab = tab;      
    }
    onPostClick(){
      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/RepostSalesJournals';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });

    }
     onreceiptClick(){
      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/RepostReceiptJournals';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }
    onpurchaseClick(){
      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/DaPostPurchaseJournals';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }
    onpayablesClick(){

      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/DaPostPBLPaymentJournals';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }

    onEmployeeClick(){

      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/DaPostEmployeeSalary';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }
    onEmployeePaymentClick(){

      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/DaPostEmployeeSalary';
      this.service.post(url, this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }

    onPostdebtorClick(){
      const selectedDate = this.dateform.value.todate;
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/PostDebtor';
      this.service.post(url,this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }
    onPostVendorClick(){
      this.NgxSpinnerService.show();
      const url = 'FinanceJournalEntryRegulation/PostCreditor';
      this.service.post(url,this.dateform.value).pipe().subscribe({
        next: (result: any) => {
          if (result.status === true) {
            this.dateform.reset();
            this.ToastrService.success(result.message);
          } else {
            this.dateform.reset();
            this.ToastrService.warning(result.message);
          }
          this.NgxSpinnerService.hide();
        },
        error: (error) => {
          this.NgxSpinnerService.hide();
          this.ToastrService.error('An unexpected error occurred. Please try again later.');
        }
      });
    }
    
    

 
}
