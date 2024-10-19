import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { FormBuilder } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-acc-trn-bankbook',
  templateUrl: './acc-trn-bankbook.component.html',
  styleUrls: ['./acc-trn-bankbook.component.scss']
})

export class AccTrnBankbookComponent {
  data: any;
  bank_gid: any;
  responsedata: any;
  from_date: any = '';
  to_date: any = '';
  Getsubbankbook_list: any[] = [];
  parameterValue1: any;
  BankBookDeleteForm: FormGroup | any;
  BankBookForm: FormGroup | any;
  FinancialYear_List: any;
  finyear_gid: any;
  from_dateto_date: any;
  finyear: any;
  default_year: any;
  fiyearname_gid: any;
  totalCredit: any;
  totalDebit: any;
  lsbank_gid: any;
  remarks: any;
  showOptionsDivId: any;
  reactiveform: FormGroup | any;
  account_no: any;
  bank_name: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    this.reactiveform = new FormGroup({
      bank_gid: new FormControl(''),
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });

    const bank_gid = this.router.snapshot.paramMap.get('bank_gid');
    this.bank_gid = bank_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.bank_gid, secretKey).toString(enc.Utf8);
    this.bank_gid = deencryptedParam;
    this.lsbank_gid = deencryptedParam;

    this.GetAccTrnBankbooksummary(deencryptedParam, this.from_date, this.to_date)

    const from_dateto_date = this.router.snapshot.paramMap.get('from_dateto_date');
    this.from_dateto_date = from_dateto_date;

    this.BankBookDeleteForm = new FormGroup({
      journal_gid: new FormControl(null),
      journaldtl_gid: new FormControl(null),
      account_gid: new FormControl(null),
    });

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  GetAccTrnBankbooksummary(deencryptedParam: any, from_date: any, to_date: any) {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetSubBankBook'
    let params = {
      bank_gid: deencryptedParam,
      from_date: from_date,
      to_date: to_date
    }

    this.service.getparams(url, params).subscribe((result: any) => {
      $('#Getsubbankbook_list').DataTable().destroy();
      this.responsedata = result;
      this.Getsubbankbook_list = result.Getsubbankbook_list;
      this.bank_name = this.Getsubbankbook_list[0].bank_name;
      this.account_no = this.Getsubbankbook_list[0].account_no;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Getsubbankbook_list').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);

      if (this.Getsubbankbook_list != null) {
        this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

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

  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(params, secretKey).toString();
    this.route.navigate(['/finance/AccTrnBankBookEntry', encryptedParam])
  }

  // add_entry() {
  //   const secretKey = 'storyboarderp';
  //   const bank_gid = AES.encrypt(this.bank_gid, secretKey).toString();
  //   const finyear_gid = this.finyear_gid;
  //   this.route.navigate(['/finance/AccTrnBankBookEntryAdd', bank_gid, finyear_gid]);
  // }

  add_entry() {
    const secretKey = 'storyboarderp';
    const bank_gid = AES.encrypt(this.bank_gid, secretKey).toString();
    const from_date = AES.encrypt(this.from_date, secretKey).toString();
    const to_date = AES.encrypt(this.to_date, secretKey).toString();

    this.route.navigate(['/finance/AccTrnBankBookEntryAdd', bank_gid, from_date, to_date]);
  }

  // OnChangeFinancialYear(finyear: any) {
  //   this.default_year = parseInt(finyear.finyear) + 1;
  //   const deencryptedParam = this.lsbank_gid
  //   this.fiyearname_gid = this.finyear_gid;
  //   this.NgxSpinnerService.show();
    
  //   var url = 'AccTrnBankbooksummary/GetSubBankBook'
    
  //   let params = {
  //     bank_gid: deencryptedParam,
  //     finyear_gid: this.fiyearname_gid
  //   }
    
  //   this.service.getparams(url, params).subscribe((result: any) => {
  //     $('#Getsubbankbook_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.Getsubbankbook_list = result.Getsubbankbook_list;
  //     this.NgxSpinnerService.hide();
  //     setTimeout(() => {
  //       $('#Getsubbankbook_list').DataTable(
  //         {
  //           "pageLength": 50, // Number of rows to display per page
  //           "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
  //         }
  //       );
  //     }, 1);
      
  //     if (this.Getsubbankbook_list != null) {
  //       this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
  //       this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

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

  OnChangeFinancialYear() {
    // this.default_year = parseInt(finyear.finyear) + 1;
    const deencryptedParam = this.lsbank_gid
    // this.fiyearname_gid = this.finyear_gid;
    
    
    this.NgxSpinnerService.show();
    
    var url = 'AccTrnBankbooksummary/GetSubBankBook'     
    let params = {
      bank_gid: deencryptedParam,
      // from_date: from_date,
      // to_date: to_date

      from_date: this.reactiveform.value.from_date,
      to_date: this.reactiveform.value.to_date
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      $('#Getsubbankbook_list').DataTable().destroy();
      this.responsedata = result;
      this.Getsubbankbook_list = result.Getsubbankbook_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#Getsubbankbook_list').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
      
      if (this.Getsubbankbook_list != null) {
        this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
        this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

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

  delete(parameter: any) {
    this.parameterValue1 = parameter
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
    var url = 'AccTrnBankbooksummary/Getdeletebankbookdtls';
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetAccTrnBankbooksummary(this.bank_gid, this.from_date, this.to_date);     /* , this.from_date,this.to_date */
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        this.GetAccTrnBankbooksummary(this.bank_gid, this.from_date, this.to_date);     /* , this.from_date,this.to_date */
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  exportExcel(): void {
    const Getsubbankbook_list = this.Getsubbankbook_list.map(item => ({
      Transaction_Date: item.transaction_date || '',
      Journal_refno: item.journal_refno || '',
      Bank_Name: item.bank_name || '',
      Account_Number: item.account_no || '',
      Account_Name: item.account_desc || '',
      Remarks: item.remarks || '',
      Credit_Amount: item.credit_amount || '',
      Debit_Amount: item.debit_amount || '',
      Closing_Amount: item.closing_amount || '',
    }));

    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(Getsubbankbook_list[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    Getsubbankbook_list.forEach(item => {
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
    link.download = 'Bank Book.xls';
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


















// bank_gid: any;
  // responsedata: any;
  // Getsubbankbook_list: any[] = [];
  // parameterValue1: any;
  // BankBookDeleteForm: FormGroup | any;
  // BankBookForm: FormGroup | any;
  // FinancialYear_List: any;
  // finyear_gid: any;
  // finyear: any;
  // default_year: any;
  // fiyearname_gid: any;
  // totalCredit: any;
  // totalDebit: any;
  // lsbank_gid: any;
  // remarks: any;
  // showOptionsDivId:any;

  // constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {
  // }

  // ngOnInit(): void {
  //   const bank_gid = this.router.snapshot.paramMap.get('bank_gid');
  //   this.bank_gid = bank_gid;
  //   const secretKey = 'storyboarderp';
  //   const deencryptedParam = AES.decrypt(this.bank_gid, secretKey).toString(enc.Utf8);
  //   this.bank_gid = deencryptedParam;
  //   this.lsbank_gid = deencryptedParam;
  //   console.log(deencryptedParam);

  //   const finyear_gid = this.router.snapshot.paramMap.get('finyear_gid');
  //   this.finyear_gid = finyear_gid;

  //   this.BankBookDeleteForm = new FormGroup({
  //     journal_gid: new FormControl(null),
  //     journaldtl_gid: new FormControl(null),
  //     account_gid: new FormControl(null),
  //   });

  //   var url = 'AccTrnBankbooksummary/GetFinancialYear'
  //   this.service.get(url).subscribe((result: any) => {
  //     this.FinancialYear_List = result.GetFinancialYear_List;
  //     // console.log(this.FinancialYear_List[0].finyear_gid)
  //     this.finyear_gid = this.FinancialYear_List[0].finyear_gid;
  //     this.finyear = parseInt(this.FinancialYear_List[0].finyear, 10);
  //     this.default_year = this.finyear + 1;
  //     this.GetAccTrnBankbooksummary(this.bank_gid, this.finyear_gid);
  //   });

  //   document.addEventListener('click', (event: MouseEvent) => {
  //     if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
  //       this.showOptionsDivId = null;
  //     }
  //   });

  // }

  // GetAccTrnBankbooksummary(deencryptedParam: any, finyear_gid: any) {
  //   this.fiyearname_gid = finyear_gid;
  //   this.NgxSpinnerService.show();
  //   var url = 'AccTrnBankbooksummary/GetSubBankBook'
  //   let params = {
  //     bank_gid: deencryptedParam,
  //     finyear_gid: this.finyear_gid
  //   }
  //   this.service.getparams(url, params).subscribe((result: any) => {
  //     $('#Getsubbankbook_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.Getsubbankbook_list = result.Getsubbankbook_list;
  //     this.NgxSpinnerService.hide();
  //     setTimeout(() => {
  //       $('#Getsubbankbook_list').DataTable(
  //         {
  //           "pageLength": 50, // Number of rows to display per page
  //           "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
  //         }
  //       );
  //     }, 1);
  //     if (this.Getsubbankbook_list != null) {
  //     this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
  //     this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

  //     // Format the totals with commas and .00
  //     const formatter = new Intl.NumberFormat('en-US', {
  //       minimumFractionDigits: 2,
  //       maximumFractionDigits: 2
  //     });

  //     this.totalCredit = formatter.format(this.totalCredit);
  //     this.totalDebit = formatter.format(this.totalDebit);
  //   }
     
  //   });
  // }
  // onview(params: any) {

  //   debugger;

  //   const secretKey = 'storyboarderp';
  //   const param = (params);
  //   const encryptedParam = AES.encrypt(params, secretKey).toString();
  //   this.route.navigate(['/finance/AccTrnBankBookEntry', encryptedParam])

  // }

  // add_entry() {
  //   const secretKey = 'storyboarderp';
  //   const bank_gid = AES.encrypt(this.bank_gid, secretKey).toString();
  //   const finyear_gid = this.finyear_gid;
  //   this.route.navigate(['/finance/AccTrnBankBookEntryAdd', bank_gid, finyear_gid]);
  // }

  // OnChangeFinancialYear(finyear: any) {
  //   this.default_year = parseInt(finyear.finyear) + 1;
  //   const deencryptedParam = this.lsbank_gid
  //   this.fiyearname_gid = this.finyear_gid;
  //   this.NgxSpinnerService.show();
  //   var url = 'AccTrnBankbooksummary/GetSubBankBook'
  //   let params = {
  //     bank_gid: deencryptedParam,
  //     finyear_gid: this.fiyearname_gid
  //   }
  //   this.service.getparams(url, params).subscribe((result: any) => {
  //     $('#Getsubbankbook_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.Getsubbankbook_list = result.Getsubbankbook_list;
  //     this.NgxSpinnerService.hide();
  //     setTimeout(() => {
  //       $('#Getsubbankbook_list').DataTable(
  //         {
  //           "pageLength": 50, // Number of rows to display per page
  //           "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
  //         }
  //       );
  //     }, 1);
  //     if (this.Getsubbankbook_list != null) {
  //       this.totalCredit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
  //       this.totalDebit = this.Getsubbankbook_list.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);

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

  // delete(parameter: any) {
  //   this.parameterValue1 = parameter
  //   console.log(this.parameterValue1, 'this.parameterValue1bank');
  //   this.BankBookDeleteForm.get("journal_gid")?.setValue(this.parameterValue1.journal_gid);
  //   this.BankBookDeleteForm.get("journaldtl_gid")?.setValue(this.parameterValue1.journaldtl_gid);
  //   this.BankBookDeleteForm.get("account_gid")?.setValue(this.parameterValue1.account_gid);
  // }

  // submit_delete() {
  //   this.BankBookDeleteForm.value;
  //   let params = {
  //     journal_gid: this.BankBookDeleteForm.value.journal_gid,
  //     journaldtl_gid: this.BankBookDeleteForm.value.journaldtl_gid,
  //     account_gid: this.BankBookDeleteForm.value.account_gid
  //   }
  //   this.NgxSpinnerService.show();
  //   var url = 'AccTrnBankbooksummary/Getdeletebankbookdtls';
  //   this.service.getparams(url, params).subscribe((result: any) => {
  //     this.responsedata = result;
  //     if (result.status == false) {
  //       window.scrollTo({

  //         top: 0, // Code is used for scroll top after event done

  //       });
  //       this.ToastrService.warning(result.message)
  //       this.GetAccTrnBankbooksummary(this.bank_gid, this.finyear_gid);
  //     }
  //     else {
  //       window.scrollTo({

  //         top: 0, // Code is used for scroll top after event done

  //       });
  //       this.ToastrService.success(result.message)
  //       this.GetAccTrnBankbooksummary(this.bank_gid, this.finyear_gid);
  //     }
  //     this.NgxSpinnerService.hide();
  //     // this.ToastrService.success('Opening Balance Added Successfully')
  //   });
  // }

  // exportExcel(): void {
  //   const Getsubbankbook_list = this.Getsubbankbook_list.map(item => ({
  //     Transaction_Date: item.transaction_date || '',
  //     Journal_refno: item.journal_refno || '',
  //     Bank_Name: item.bank_name || '',
  //     Account_Number: item.account_no || '',
  //     Account_Name: item.account_desc || '',
  //     Remarks: item.remarks || '',
  //     Credit_Amount: item.credit_amount || '',
  //     Debit_Amount: item.debit_amount || '',
  //     Closing_Amount: item.closing_amount || '',
  //   }));

  //   // Create a new table element
  //   const table = document.createElement('table');

  //   // Add header row with background color
  //   const headerRow = table.insertRow();
  //   Object.keys(Getsubbankbook_list[0]).forEach(header => {
  //     const cell = headerRow.insertCell();
  //     cell.textContent = header;
  //     cell.style.backgroundColor = '#00317a';
  //     cell.style.color = '#FFFFFF';
  //     cell.style.fontWeight = 'bold';
  //     cell.style.border = '1px solid #000000';
  //   });

  //   // Add data rows
  //   Getsubbankbook_list.forEach(item => {
  //     const dataRow = table.insertRow();
  //     Object.values(item).forEach(value => {
  //       const cell = dataRow.insertCell();
  //       cell.textContent = value;
  //       cell.style.border = '1px solid #000000';
  //     });
  //   });

  //   // Convert the table to a data URI
  //   const tableHtml = table.outerHTML;
  //   const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

  //   // Trigger download
  //   const link = document.createElement('a');
  //   link.href = dataUri;
  //   link.download = 'Bank Book.xls';
  //   document.body.appendChild(link);
  //   link.click();
  //   document.body.removeChild(link);
  // }

  // popmodal(parameter: string) {
  //   this.remarks = parameter;
  // }

  // toggleOptions(data: any) {
  //   if (this.showOptionsDivId === data) {
  //     this.showOptionsDivId = null;
  //   } else {
  //     this.showOptionsDivId = data;
  //   }
  // }
