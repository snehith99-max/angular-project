import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-acc-trn-cashbook-select',
  templateUrl: './acc-trn-cashbook-select.component.html',
  styleUrls: ['./acc-trn-cashbook-select.component.scss']
})
export class AccTrnCashbookSelectComponent {
  serviceorder: any;
  branch_gid: any;
  lsbranch_gid: any;
  finyear_gid: any;
  FinancialYear_List: any;
  finyear: any;
  default_year: any;
  fiyearname_gid: any;
  totalCredit: any;
  totalDebit: any;
  responsedata: any;
  parameterValue1: any;
  BankBookDeleteForm: FormGroup | any;
  CashBookSelect_list: any[] = [];
  remarks: any;
  showOptionsDivId:any;
  reactiveform: FormGroup | any;
  from_dateto_date: any;
  branch_name: any;
  from_date: any = '';
  to_date: any = '';
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private router: ActivatedRoute, private route: Router, public service: SocketService, private NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);
    const branch_gid = this.router.snapshot.paramMap.get('branch_gid');
    // console.log(termsconditions_gid)
    this.branch_gid = branch_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.branch_gid, secretKey).toString(enc.Utf8);
    // this.lsbranch_gid = deencryptedParam;
    this.branch_gid = deencryptedParam;
    const finyear_gid = this.router.snapshot.paramMap.get('finyear_gid');
    this.finyear_gid = finyear_gid;
    const from_dateto_date = this.router.snapshot.paramMap.get('from_dateto_date');
    this.from_dateto_date = from_dateto_date;

    this.reactiveform = new FormGroup({
      branch_gid: new FormControl(''),
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    this.BankBookDeleteForm = new FormGroup({
      journal_gid: new FormControl(null),
      journaldtl_gid: new FormControl(null),
      account_gid: new FormControl(null),
    });

    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      // console.log(this.FinancialYear_List[0].finyear_gid)
      this.finyear_gid = this.FinancialYear_List[0].finyear_gid;
      this.finyear = parseInt(this.FinancialYear_List[0].finyear, 10);
      this.default_year = this.finyear + 1;
      // this.AccTrnCashbooksummary(this.lsbranch_gid, this.finyear_gid);
      this.AccTrnCashbooksummary(deencryptedParam, this.from_date, this.to_date)
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

  }

  // AccTrnCashbooksummary(lsbranch_gid: any, finyear_gid: any) {
  //   this.NgxSpinnerService.show();
  //   var url1 = 'AccTrnCashBookSummary/GetAccTrnCashbookSelect'
  //   let param = {
  //     branch_gid: lsbranch_gid,
  //     finyear_gid: finyear_gid
  //   }
  //   this.service.getparams(url1, param).subscribe((result: any) => {
  //     $('#CashBookSelect_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.CashBookSelect_list = result.CashBookSelect_list;
  //     this.NgxSpinnerService.hide();
  //     setTimeout(() => {
  //       $('#CashBookSelect_list').DataTable(
  //         {
  //           "pageLength": 50, // Number of rows to display per page
  //           "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
  //         }
  //       );
  //     }, 1);
  //     if (this.CashBookSelect_list != null) {
  //       this.totalCredit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
  //       this.totalDebit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

  //       // Format the totals with commas and .00
  //       const formatter = new Intl.NumberFormat('en-US', {
  //         minimumFractionDigits: 2,
  //         maximumFractionDigits: 2
  //       });

  //       this.totalCredit = formatter.format(this.totalCredit);
  //       this.totalDebit = formatter.format(this.totalDebit);
  //     }

  //   });
  // }

  AccTrnCashbooksummary(deencryptedParam: any, from_date: any, to_date: any) {
    this.NgxSpinnerService.show();
    var url1 = 'AccTrnCashBookSummary/GetAccTrnCashbookSelect'

    let param = {
      branch_gid: deencryptedParam,
      from_date: from_date,
      to_date: to_date
    }

    this.service.getparams(url1, param).subscribe((result: any) => {
      $('#CashBookSelect_list').DataTable().destroy();
      this.responsedata = result;
      this.CashBookSelect_list = result.CashBookSelect_list;
      if(this.CashBookSelect_list == null){
        this.NgxSpinnerService.hide();
      }
      else{
      this.branch_name = this.CashBookSelect_list[0].branch_name;
      this.NgxSpinnerService.hide();
      if (this.CashBookSelect_list != null) {
        this.totalCredit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);
      }
    }
    setTimeout(() => {
      $('#CashBookSelect_list').DataTable(
        {
          "pageLength": 50, // Number of rows to display per page
          "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
        }
      );
    }, 1);
    });
  }

  OnChangeFinancialYear(finyear: any) {
    this.default_year = parseInt(finyear.finyear) + 1;
    const deencryptedParam = this.lsbranch_gid
    this.fiyearname_gid = this.finyear_gid;
    this.NgxSpinnerService.show();
    var url = 'AccTrnCashBookSummary/GetAccTrnCashbookSelect'
    let params = {
      branch_gid: deencryptedParam,
      finyear_gid: this.fiyearname_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#CashBookSelect_list').DataTable().destroy();
      this.responsedata = result;
      this.CashBookSelect_list = result.CashBookSelect_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#CashBookSelect_list').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
      if (this.CashBookSelect_list != null) {
        this.totalCredit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.CashBookSelect_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        });

        this.totalCredit = formatter.format(this.totalCredit);
        this.totalDebit = formatter.format(this.totalDebit);
      }

    });
  }

  add_entry() {
    const secretKey = 'storyboarderp';
    const branch_gid = AES.encrypt(this.lsbranch_gid, secretKey).toString();
    const finyear_gid = this.finyear_gid;
    this.route.navigate(['/finance/AccTrnCashBookEntryAdd', branch_gid, finyear_gid]);
  }

  delete(parameter: any) {
    this.parameterValue1 = parameter
    console.log(this.parameterValue1, 'this.parameterValue1bank');
    this.BankBookDeleteForm.get("journal_gid")?.setValue(this.parameterValue1.journal_gid);
    this.BankBookDeleteForm.get("journaldtl_gid")?.setValue(this.parameterValue1.journaldtl_gid);
    this.BankBookDeleteForm.get("account_gid")?.setValue(this.parameterValue1.account_gid);
  }

  submit_delete() {
    this.BankBookDeleteForm.value;
    let params = {
      journal_gid: this.BankBookDeleteForm.value.journal_gid,
      journaldtl_gid: this.BankBookDeleteForm.value.journaldtl_gid,
      account_gid: this.BankBookDeleteForm.value.account_gid
    }
    this.NgxSpinnerService.show();
    var url = 'AccTrnCashBookSummary/GetDeleteCashBookDtls';
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        // this.AccTrnCashbooksummary(this.lsbranch_gid, this.finyear_gid);
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        // this.AccTrnCashbooksummary(this.lsbranch_gid, this.finyear_gid);
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  exportExcel(): void {
    const CashBookSelectlist = this.CashBookSelect_list.map(item => ({
      Transaction_Date: item.transaction_date || '',
      Journal_refno: item.journal_refno || '',
      Branch_Name: item.branch_name || '',
      Account_Name: item.account_desc || '',
      Remarks: item.remarks || '',
      Credit_Amount: item.credit_amount || '',
      Debit_Amount: item.debit_amount || '',
      Closing_Amount: item.closing_amount || '',

    }));
    // this.excelService.exportAsExcelFile(ProductList , 'Product');
    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(CashBookSelectlist[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    CashBookSelectlist.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Cash Book.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  popmodal(parameter: string) {
    this.remarks = parameter;
  }

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

}
