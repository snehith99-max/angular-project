import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-acc-rpt-ledgerreport',
  templateUrl: './acc-rpt-ledgerreport.component.html',
  styleUrls: ['./acc-rpt-ledgerreport.component.scss']
})

export class AccRptLedgerreportComponent {
  responsedata: any;
  liabilityaccountname_List: any[] = [];
  assetaccountname_List: any[] = [];
  incomeaccountname_List: any[] = [];
  expenseaccountname_List: any[] = [];
  reactiveform: FormGroup | any;
  Ledgerbook_List: any[] = [];
  totalCredit: any;
  totalDebit: any;
  totalBalance: any;
  noledger_data: any;
  ledger_data: any;
  totalClosingAmount: any;
  nodata_available: any;
  currentTab = 'Income';
  assetreport_List: any[] = [];
  liabilityreport_List: any[] = [];
  incomeledgerreport_List: any[] = [];
  expenseledgerreport_List: any[] = [];
  totalCredit_expense: any;
  totalDebit_expense: any;
  totalCredit_income: any;
  totalDebit_income: any;
  totalCredit_asset: any;
  totalDebit_asset: any;
  totalCredit_Liability: any;
  totalDebit_Liability: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router, private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService) {

  }

  ngOnInit(): void {
    var url = 'AccTrnBankbooksummary/GetLiabilityLedgerNameDropdown'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.liabilityaccountname_List = this.responsedata.GetLiabilityaccountname_List;
    });

    var url = 'AccTrnBankbooksummary/GetAssetLedgerNameDropdown'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.assetaccountname_List = this.responsedata.GetAssetaccountname_List;
    });

    var url = 'AccTrnBankbooksummary/GetIncomeLedgerNameDropdown'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.incomeaccountname_List = this.responsedata.GetIncomeaccountname_List;
    });

    var url = 'AccTrnBankbooksummary/GetExpenseLedgerNameDropdown'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.expenseaccountname_List = this.responsedata.GetExpenseaccountname_List;
    });

    this.reactiveform = new FormGroup({
      accountgroup_name: new FormControl(null),
    });
    this.noledger_data = true;
    this.ledger_data = false;
    this.nodata_available = false;

    this.incomeledgersummary();
  }
  
  showTab(tab: string) {
    this.currentTab = tab;
    if (this.currentTab === 'Expense') {
      this.expenseledgersummary();
    }
    else if (this.currentTab === 'Income') {
      this.incomeledgersummary();
    }
    else if (this.currentTab === 'Liability') {
      this.liabilityledgersummary();
    }
    else if (this.currentTab === 'Asset') {
      this.assetledgersummary();
    }
  }
  incomeledgersummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetIncomeLedgerReportSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.incomeledgerreport_List = this.responsedata.IncomeLedgerReport_list;
      setTimeout(() => {
        $('#incomeledgerreport_List').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();

      if (this.incomeledgerreport_List != null) {
        this.totalCredit_income = this.incomeledgerreport_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(/,/g, '') || '0'), 0);
        this.totalDebit_income = this.incomeledgerreport_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(/,/g, '') || '0'), 0);  
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
        });
  
        this.totalCredit_income = formatter.format(this.totalCredit_income);
        this.totalDebit_income = formatter.format(this.totalDebit_income);
      }
    });
  }

  expenseledgersummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetExpenseLedgerReportSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.expenseledgerreport_List = this.responsedata.ExpenseLedgerReport_list;
      setTimeout(() => {
        $('#expenseledgerreport_List').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();

      if (this.expenseledgerreport_List != null) {
        this.totalCredit_expense = this.expenseledgerreport_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(/,/g, '') || '0'), 0);
        this.totalDebit_expense = this.expenseledgerreport_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(/,/g, '') || '0'), 0);  
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
        });
  
        this.totalCredit_expense = formatter.format(this.totalCredit_expense);
        this.totalDebit_expense = formatter.format(this.totalDebit_expense);
      }
    });
  }

  assetledgersummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetAssetLedgerReportSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.assetreport_List = this.responsedata.GetAssetLedgerReportSummary_List;
      setTimeout(() => {
        $('#assetreport_List').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();

      if (this.assetreport_List != null) {
        this.totalCredit_asset = this.assetreport_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(/,/g, '') || '0'), 0);
        this.totalDebit_asset = this.assetreport_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(/,/g, '') || '0'), 0);  
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
        });
  
        this.totalCredit_asset = formatter.format(this.totalCredit_asset);
        this.totalDebit_asset = formatter.format(this.totalDebit_asset);
      }
    });
  }

  liabilityledgersummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetLiabilityLedgerReportSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.liabilityreport_List = this.responsedata.GetLiabilityLedgerReportSummary_List;
      setTimeout(() => {
        $('#liabilityreport_List').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();

      if (this.liabilityreport_List != null) {
        this.totalCredit_Liability = this.liabilityreport_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(/,/g, '') || '0'), 0);
        this.totalDebit_Liability = this.liabilityreport_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(/,/g, '') || '0'), 0);  
        // Format the totals with commas and .00
        const formatter = new Intl.NumberFormat('en-US', {
          minimumFractionDigits: 2,
        });
  
        this.totalCredit_Liability = formatter.format(this.totalCredit_Liability);
        this.totalDebit_Liability = formatter.format(this.totalDebit_Liability);
      }
    });
  }

  exportExcel(): void {
    const LedgerbookList = this.Ledgerbook_List.map(item => ({
      'Transaction Date': item.transaction_date || '',
      'Reference Number': item.journal_refno || '',
      'Account Description': item.account_desc || '',
      'Type': item.type || '',
      'Debit Amount': item.debit_amount || '',
      'Credit Amount': item.credit_amount || '',
      'Closing Balance': item.closing_amount || '',
    }));

    // Create a new table element
    const table = document.createElement('table');

    // Add header row with background color
    const headerRow = table.insertRow();
    Object.keys(LedgerbookList[0]).forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Add data rows
    LedgerbookList.forEach(item => {
      const dataRow = table.insertRow();
      Object.values(item).forEach(value => {
        const cell = dataRow.insertCell();
        cell.textContent = value;
        cell.style.border = '1px solid #000000';
      });
    });

    const totalsRow1 = table.insertRow();
    const numColumns1 = Object.keys(LedgerbookList[0]).length;

    for (let i = 0; i < numColumns1 - 4; i++) {
      const cell = totalsRow1.insertCell();
      cell.textContent = '';
      cell.style.border = '1px solid #000000';
    }

    const totalLabelCell = totalsRow1.insertCell();
    totalLabelCell.textContent = 'Total:';
    totalLabelCell.style.fontWeight = 'bold';
    totalLabelCell.style.border = '1px solid #000000';


    const totalDebitCell = totalsRow1.insertCell();
    totalDebitCell.textContent = this.totalDebit || '0';
    totalDebitCell.style.fontWeight = 'bold';
    totalDebitCell.style.border = '1px solid #000000';


    const totalCreditCell = totalsRow1.insertCell();
    totalCreditCell.textContent = this.totalCredit || '0';
    totalCreditCell.style.fontWeight = 'bold';
    totalCreditCell.style.border = '1px solid #000000';

    const emptycell = totalsRow1.insertCell();
    emptycell.textContent = ' ';
    emptycell.style.fontWeight = 'bold';
    emptycell.style.border = '1px solid #000000';

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Ledger Book Statement.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  downloadPDF() {
    const doc = new jsPDF('landscape', 'mm', 'a4');

    doc.setFontSize(16);
    doc.setTextColor(0, 51, 153);

    doc.text('Statement Of Accounts', 14, 22);
    doc.addImage('assets/media/logos/storyboardsystem_menu.png', 'PNG', 220, 5, 60, 30);

    let y = 32;
    const lineHeight = 7;

    doc.setFontSize(12);
    doc.setTextColor(0, 0, 0);
    doc.text('Vcidex solutions pvt ltd - 60001841,', 14, y);
    y += lineHeight;
    doc.text('Module 210 NSIC - STP, B24,', 14, y);
    y += lineHeight;
    doc.text('Guindy Industrial Estate, Ekkaduthangal,-600032,', 14, y);
    y += lineHeight;
    doc.text('Tamilnadu, INDIA.', 14, y);
    y += lineHeight;

    doc.setTextColor(0, 51, 153);
    doc.text('GST No: 33AABCV7186L2ZU', 14, y);
    y += lineHeight;    

    const headers = [['S.No', 'Transaction Date', 'Reference', 'Description', 'Type', 'Credit Amount', 'Debit Amount', 'Closing Amount']];

    const data = this.Ledgerbook_List.map((item, index) => [      
      index + 1,
      item.transaction_date,
      item.journal_refno,
      item.account_desc,
      item.type,
      { content: parseFloat(item.credit_amount).toFixed(2), styles: { halign: 'right' } },
      { content: parseFloat(item.debit_amount).toFixed(2), styles: { halign: 'right' } },
      { content: parseFloat(item.closing_amount).toFixed(2), styles: { halign: 'right' } }
    ]);

    autoTable(doc, {
      startY: 70,
      head: headers,
      body: data,
      theme: 'grid',
      styles: {
        fontSize: 10,
        cellPadding: 3
      },
      headStyles: {
        fillColor: [225, 227, 234],
        textColor: [0, 0, 0],
        fontStyle: 'bold',
      }
    });

    const finalY = (doc as any).previousAutoTable.finalY || 70;
    doc.setFontSize(12);
    doc.setTextColor(255, 0, 0);
    doc.text('Note: Kindly Confirm the same within 15 days or else the balance as per our records is treated as correct.', 14, finalY + 10);

    doc.save('Ledgerbook_List.pdf');
  }

  IncomeDetailedReport(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const account_gid = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/finance/AccRptLedgerincomereport', account_gid])
  }

  ExpenseDetailedReport(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const account_gid = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/finance/AccRptLedgerexpensereport', account_gid])
  }

  AssetDetailedReport(account_gid: any, customer_gid: any) {
    const key = 'storyboard';
    const param = account_gid;
    const param1 = customer_gid;
    const accountgid = AES.encrypt(param, key).toString();
    const customergid = AES.encrypt(param1, key).toString();
    this.route.navigate(['/finance/AccRptLedgerassetreport', accountgid, customergid])
  }

  LiabilityDetailedReport(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const account_gid = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/finance/AccRptLedgerliabityreport', account_gid])
  }
}










// accountnamechange() {
//   let account_gid = this.reactiveform.get("accountgroup_name")?.value;

//   let param = {
//     account_gid: account_gid
//   }
//   this.NgxSpinnerService.show();
//   var url = 'AccTrnBankbooksummary/GetLedgerBookReport';
//   this.service.getparams(url, param).subscribe((result: any) => {
//     $('#Ledgerbook_List').DataTable().destroy();
//     this.responsedata = result;
//     this.Ledgerbook_List = this.responsedata.GetLedgerbook_List;
//     this.NgxSpinnerService.hide();
//     // setTimeout(() => {
//     //   $('#Ledgerbook_List').DataTable(
//     //     {
//     //       // code by snehith for customized pagination
//     //       "pageLength": 50, // Number of rows to display per page
//     //       "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
//     //     }
//     //   );
//     // }, 1);

//     if (this.Ledgerbook_List != null) {
//       this.totalCredit = this.Ledgerbook_List.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
//       this.totalDebit = this.Ledgerbook_List.reduce((total: any, data: any) => total + parseFloat(data.debit_amount.replace(',', '')), 0);
//       this.totalClosingAmount = this.Ledgerbook_List.reduce((total: any, data: any) => total + parseFloat(data.closing_amount.replace(',', '')), 0);

//       // Format the totals with commas and .00
//       const formatter = new Intl.NumberFormat('en-US', {
//         minimumFractionDigits: 2,
//         maximumFractionDigits: 2
//       });

//       this.totalCredit = formatter.format(this.totalCredit);
//       this.totalDebit = formatter.format(this.totalDebit);
//       this.totalClosingAmount = formatter.format(this.totalClosingAmount);

//       this.noledger_data = false;
//       this.ledger_data = true;
//       this.nodata_available = false;
//     }
//     else {
//       this.nodata_available = true;
//       this.noledger_data = false;
//       this.ledger_data = false;
//     }
//   });
// }


// const totalsRow = table.insertRow();
// const numColumns = Object.keys(LedgerbookList[0]).length;

// for (let i = 0; i < numColumns - 4; i++) {
//   const cell = totalsRow.insertCell();
//   cell.textContent = '';
//   cell.style.border = '1px solid #000000';
// }

// const openingLabelCell = totalsRow.insertCell();
// openingLabelCell.textContent = 'Opening:';
// openingLabelCell.style.fontWeight = 'bold';
// openingLabelCell.style.border = '1px solid #000000';

// const openingAmountCell = totalsRow.insertCell();
// openingAmountCell.textContent = this.totalClosingAmount || '0';
// openingAmountCell.style.fontWeight = 'bold';
// openingAmountCell.style.border = '1px solid #000000';

// const closingLabelCell = totalsRow.insertCell();
// closingLabelCell.textContent = 'Closing:';
// closingLabelCell.style.fontWeight = 'bold';
// closingLabelCell.style.border = '1px solid #000000';

// const closingAmountCell = totalsRow.insertCell();
// closingAmountCell.textContent = this.totalClosingAmount || '0';
// closingAmountCell.style.fontWeight = 'bold';
// closingAmountCell.style.border = '1px solid #000000';

// doc.text('SHEENLAC PAINTS LIMITED,', 150, 32);
// doc.text('No 124, Developed Plots,', 150, 37);
// doc.text('Industrial Estate,Ambattur,', 150, 42);
// doc.text('Chennai - 600098.', 150, 47);
// doc.text('GST No: GST TIN 33AASCS5073J1ZV', 150, 52);
// doc.text('Phone: 044-43949900', 150, 57);

// this.Ledgerbook_List.sort((a, b) => new Date(a.transaction_date).getTime() - new Date(b.transaction_date).getTime());

// const firstDateString = this.Ledgerbook_List[0]?.transaction_date || '';
// const lastDateString = this.Ledgerbook_List[this.Ledgerbook_List.length - 1]?.transaction_date || '';


// const statementPeriodText = `Statement Of The Account For The Period Of ${firstDateString} To ${lastDateString}`;

// doc.setFontSize(10);
// doc.setTextColor(0, 0, 0);
// doc.text(statementPeriodText, 14, y);
// { content: index + 1, styles: { halign: 'center' } },// S.No (index + 1)
