import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { CurrencyPipe } from '@angular/common';
@Component({
  selector: 'app-acc-trn-fundtransfersummary',
  templateUrl: './acc-trn-fundtransfersummary.component.html',
  styleUrls: ['./acc-trn-fundtransfersummary.component.scss']
})
export class AccTrnFundtransfersummaryComponent {
  approval_flag:any;
  pettycashgid: any;
  transactiondate: any;
  frombranchname: any;
  tobranchname: any;
  GetFundTransfer_list: any;
  bankmaster_list: any;
  parameterValue1: any;
  parameterValue:any;
  responsedata: any;
  branchname_list: any;
  reactiveform: FormGroup | any;
  reactiveformview: FormGroup | any;
  mdlBranchName: any;
  remarks: any;
  from_branch: any;
  to_branch:any;
transaction_date:any;
transaction_amount:any;
  filteredBranches: any;
  showOptionsDivId: any;
  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      created_date: new FormControl(this.getCurrentDate(), Validators.required),
      frombranch: new FormControl(null, Validators.required),
      tobranch: new FormControl(null, Validators.required),
      transfer_amount: new FormControl('', Validators.required),
      remarks: new FormControl(''),
      pettycash_gid: new FormControl(''),
    })
    this.reactiveformview = new FormGroup({
      created_date: new FormControl(''),
      frombranch: new FormControl(''),
      tobranch: new FormControl(''),
      transfer_amount: new FormControl(''),
      remarks: new FormControl(''),
	  reason: new FormControl(''),
      pettycash_gid: new FormControl(''),
    })
  }
  get created_date() {
    return this.reactiveform.get('created_date')!;
  }
  get frombranch() {
    return this.reactiveform.get('frombranch')!;
  }
  get tobranch() {
    return this.reactiveform.get('tobranch')!;
  }
  get transfer_amount() {
    return this.reactiveform.get('transfer_amount')!;
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      this.filteredBranches = result.GetBranchName;

    })

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    
    // this.branchname_list = [
    //   { branch_gid: 'Vcidex Solutions Pvt Ltd', branch_name: 'Vcidex Solutions Pvt Ltd' },
    //   { branch_gid: 'Vcidex Singapore PVT Ltd', branch_name: 'Vcidex Singapore PVT Ltd' },
    //   { branch_gid: 'NISSI CHENNAI', branch_name: 'NISSI CHENNAI' }
    // ];
    this.getsummary();
  }
  filterToBranchOptions(selectedFromBranch: any): void {
    debugger
    if (selectedFromBranch) {
      // Filter branchname_list to exclude selectedFromBranch
      this.filteredBranches = this.branchname_list.filter((branch: { branch_gid: any; }) => branch.branch_gid !== selectedFromBranch);
    }
    else {
      // Reset to show all branches
      this.filteredBranches = [...this.branchname_list];
    }
  }
  formatCurrencyValue() {
    // Format the currency value when the input field value changes
    const numericValue = parseFloat(this.reactiveform.value.transfer_amount);
    const formattedCurrency = this.formatCurrency(numericValue);
    this.transfer_amount.setValue(formattedCurrency, { emitEvent: false });
  }
  
  private formatCurrency(value: number): string {
    // Format the numeric value with comma separators for thousands (no currency symbol)
    if (!isNaN(value)) {
      const stringValue = value.toFixed(2); // Round to two decimal places and convert to string
      const parts = stringValue.split('.'); // Split into integer and decimal parts
      parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); // Add commas for thousands
      return parts.join('.'); // Join parts back together
    }
    return '';
  }
  
  reviewmodal(data:any){
    //console.log(data)
   
    this.pettycashgid = data.pettycash_gid;
    this.transactiondate = data.transaction_date;
    this.frombranchname = data.from_branch;
    this.tobranchname = data.to_branch;
    this.reactiveformview.get("pettycash_gid")?.setValue(data.pettycash_gid);
    this.reactiveformview.get("created_date")?.setValue(data.transaction_date);
    this.reactiveformview.get("remarks")?.setValue(data.remarks);
    this.reactiveformview.get("reason")?.setValue(data.reason);
    this.reactiveformview.get("tobranch")?.setValue(data.to_branch_gid);
    this.approval_flag =data.approval_flag;
    this.transaction_amount =data.transaction_amount;
  }
  getsummary() {
    var url = 'FundTransfer/GetFundTransferSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.GetFundTransfer_list = this.responsedata.GetFundTransfer_list;
      //console.log(this.GetFundTransfer_list)
      $('#GetFundTransfer_list').DataTable().destroy();
      setTimeout(() => {
        $('#GetFundTransfer_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 200, // Number of rows to display per page
            "lengthMenu": [200, 500, 1000, 1500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }
  onsubmit() {
    //console.log(this.reactiveform.value)
    if (this.reactiveform.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'FundTransfer/PostFundTransfer'
      this.service.post(url, this.reactiveform.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });

          this.reactiveform.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.getsummary();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveform.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.getsummary();

        }

      });
    }
    else {

      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  onupdate() {
    if (this.reactiveform.status == 'VALID') {
      //console.log(this.reactiveform.value)
      this.NgxSpinnerService.show();
      var url = 'FundTransfer/UpdateFundTransfer'
      this.service.post(url, this.reactiveform.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveform.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.getsummary();


        }
        else {
          this.reactiveform.reset();
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.getsummary();

        }


      });
    }
    else {

      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  openModaledit(data: any) {
    //console.log(data.pettycash_gid)
    this.pettycashgid = data.pettycash_gid;
    this.transactiondate = data.transaction_date;
    this.frombranchname = data.from_branch;
    this.tobranchname = data.to_branch;
    this.reactiveform.get("pettycash_gid")?.setValue(data.pettycash_gid);
    this.reactiveform.get("created_date")?.setValue(data.transaction_date);
    this.reactiveform.get("remarks")?.setValue(data.remarks);
    this.reactiveform.get("frombranch")?.setValue(data.from_branch_gid);
    this.reactiveform.get("tobranch")?.setValue(data.to_branch_gid);
    this.reactiveform.get("transfer_amount")?.setValue(data.transaction_amount);
  }
  popmodal(parameter: string, param1: string, param2: string) {

    this.remarks = parameter;
    this.from_branch = param1;
    this.to_branch = param2;

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  
  }
  ondelete()
  {
    this.NgxSpinnerService.show();
    var url = 'FundTransfer/DeleteFundTransfers'
    let param = {
      pettycash_gid : this.parameterValue 
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        window.scrollTo({
  
          top: 0, // Code is used for scroll top after event done
  
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
  
      }
      else{
        window.scrollTo({
  
          top: 0, // Code is used for scroll top after event done
  
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.getsummary();
  
  
      }

  
    });
  }
  openModaladd() {
    this.reactiveform.get("created_date")?.setValue(this.getCurrentDate());
  }
  clear() {
    this.reactiveform.reset();
  }
}
