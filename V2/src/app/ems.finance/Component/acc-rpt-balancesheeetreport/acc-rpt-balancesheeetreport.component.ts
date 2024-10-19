import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { jsPDF, jsPDFOptions } from 'jspdf';
import "jspdf-autotable";
import html2canvas from "html2canvas";
import { DomSanitizer } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { Options } from 'html2canvas';
import { NgSelectComponent } from '@ng-select/ng-select';
import autoTable from 'jspdf-autotable';


interface CustomOptions extends Options {
  dpi?: number;
}
@Component({
  selector: 'app-acc-rpt-balancesheeetreport',
  templateUrl: './acc-rpt-balancesheeetreport.component.html',
  styleUrls: ['./acc-rpt-balancesheeetreport.component.scss']
})
export class AccRptBalancesheeetreportComponent {
  @ViewChild('select') select!: NgSelectComponent;
  expenseFlag: boolean = false;
  incomeFlag: boolean = false;
  net_flag: boolean = false;
  responsedata: any;
  GetGstManagement_list: any;
  month: any;
  lblNet: any;
  Balancesheetoverallnetvalue: any;
  year: any;
  profitlossexcel_list: any;
  profitlosspdf_list: any;
  htmlContent: any;
  profitlossincome_list: any;
  Balancesheetpdf_list: any;
  lblpandlvalue: any;
  lblpandl: any;
  htmlContent1: any;
  html_code: any;
  html_code2: any;
  html_code3: any;
  content_html: any
  html_income: any;
  html_expense: any;
  profitlossExpense_list: any;
  income_closebal: any;
  expense_closebal: any;
  branchname_list: any;
  reactiveform: FormGroup | any;
  deafultbranch: any;
  net_total: any;
  deafultfin: any;
  BalanceSheetexcel_list: any;
  mainList: any[] = [];
  subList: any[] = [];
  mainList1: any[] = [];
  subList1: any[] = [];
  liability_opening: any;
  liability_debit: any;
  liability_credit: any;
  liability_closing: any;
  asset_opening: any;
  asset_debit: any;
  asset_credit: any;
  asset_closing: any;
  netTotalFormatted: any;
  GetProfilelossfinyear_list: any;
  liability_display: boolean = false;
  asset_display: boolean = false;
  @ViewChild('dynamicContentContainer') dynamicContentContainer!: ElementRef;
  @ViewChild('dynamicContentContainer1') dynamicContentContainer1!: ElementRef;
  @ViewChild('dynamicContentContainer2') dynamicContentContainer2!: ElementRef;
  @ViewChild('contentToConvert1') contentToConvert1!: ElementRef;
  constructor(public service: SocketService, private el: ElementRef, private route: Router, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer, private renderer: Renderer2, private router: Router, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      frombranch: new FormControl(null, Validators.required),
      finyear: new FormControl(null, Validators.required),

    })
  }
  get frombranch() {
    return this.reactiveform.get('frombranch')!;
  }
  get finyear() {
    return this.reactiveform.get('finyear')!;
  }
  ngOnInit(): void {

    this.getsummary();

  }

  getsummary() {
    //  console.log('bran',localStorage.getItem('deafultbranch'))
    //   console.log('fin',localStorage.getItem('deafultfin'))
    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      this.deafultbranch = this.branchname_list[0].branch_gid;
      // localStorage.removeItem('deafultbranch');
      // localStorage.setItem('deafultbranch', this.deafultbranch.toString());
      var url = 'ProfitLossReport/GetProfilelossfinyear'
      this.service.get(url).subscribe((result: any) => {
        this.GetProfilelossfinyear_list = result.GetProfilelossfinyear_list;
        this.deafultfin = this.GetProfilelossfinyear_list[0].finyear_gid;
        // localStorage.removeItem('deafultfin');
        // localStorage.setItem('deafultfin', this.deafultfin.toString());
        let param = {
          branch: this.deafultbranch,
          year_gid: this.deafultfin
        }
        // var url1 = 'ProfitLossReport/GetProfitLossExportExcel'
        // this.service.getparams(url1, param).subscribe((result: any) => {
        //   this.responsedata = result;
        //   this.profitlossexcel_list = this.responsedata.profitlossexcel_list;
        //  this.html_code =this.profitlossexcel_list[0].html_content;
        //  this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent());
        // });
        var url = 'BalanceSheetReport/GetNetAmountDetails';
        this.service.getparams(url, param).subscribe((result: any) => {
          $('#Balancesheetoverallnetvalue').DataTable().destroy();
          this.responsedata = result;
          this.Balancesheetoverallnetvalue = this.responsedata.Balancesheetoverallnetvalue;
          //console.log(this.Balancesheetoverallnetvalue[0].lblpandlvalue)
          this.netTotalFormatted = this.Balancesheetoverallnetvalue[0].lblpandlvalue;
          this.lblNet = this.Balancesheetoverallnetvalue[0].lblpandl;

          //console.log(this.Balancesheetoverallnetvalue[0].lblpandl)
        });
        var url2 = 'BalanceSheetReport/GetBalanceSheetAsset'
        this.service.getparams(url2, param).subscribe((result: any) => {
          this.responsedata = result;
          this.profitlossincome_list = this.responsedata.BalanceSheetasset_list;
          if (this.profitlossincome_list != null && this.profitlossincome_list != '') {
            this.html_code2 = this.profitlossincome_list[0].html_content;
            this.income_closebal = this.profitlossincome_list[0].income_closebal;
            this.html_income = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent1());
            // console.log('inc',this.income_closebal)
            if (this.html_income != "" && this.html_income != null) {

              this.incomeFlag = true;
            }
            else {
              this.income_closebal = '0.00';
              this.html_income = "";
              this.incomeFlag = false;
            }
          }
          else {
            this.income_closebal = '0.00';
            this.html_income = "";
            this.incomeFlag = false;
          }
          var url3 = 'BalanceSheetReport/GetBalanceSheetLiability'
          this.service.getparams(url3, param).subscribe((result: any) => {
            this.responsedata = result;
            this.profitlossExpense_list = this.responsedata.BalanceSheetliability_list;
            if (this.profitlossExpense_list != null && this.profitlossExpense_list != '') {
              this.expense_closebal = this.profitlossExpense_list[0].expense_closebal;
              this.html_code3 = this.profitlossExpense_list[0].html_content;
              this.html_expense = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent2());
              // console.log('exp',this.expense_closebal)
              if (this.html_expense != "" && this.html_expense != null) {
                this.expenseFlag = true;
              }
              else {
                this.expense_closebal = '0.00';
                this.html_expense = "";
                this.expenseFlag = false;
              }

            }
            else {

              this.expense_closebal = '0.00';
              this.html_expense = "";
              this.expenseFlag = false;
            }
            if ((this.income_closebal != null && this.income_closebal != "" && this.income_closebal != undefined) || (this.expense_closebal != null && this.expense_closebal != "" && this.expense_closebal != undefined)) {

              const income_total: number = parseFloat(this.income_closebal);
              const expense_total: number = parseFloat(this.expense_closebal);
              this.net_total = expense_total - (income_total < 0 ? -income_total : income_total);
              const formatter = new Intl.NumberFormat('en-US', {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
              });

              this.net_total = formatter.format(this.net_total);
              this.net_flag = true;

            }
            else {
              this.net_flag = false;


            }

          });
        });
        var url3 = 'BalanceSheetReport/GetSummaryLiability'
        this.service.getparams(url3, param).subscribe((result: any) => {
          this.responsedata = result;
          debugger
          // this.mainList =  result.GetSummaryExpenseparent.map((item: any) => ({ ...item, visible: false })) ;
          //this.subList = result.GetSummaryExpensechild.map((item: any) => ({ ...item, visible: false }));
          this.mainList = (result.parentfoldersliability || []).map((item: any) => ({ ...item, visible: false }));
          if (this.mainList.length != 0) {
            const totals = this.sumOfAllProperties(this.mainList);
            this.liability_display = true;
            this.liability_opening = totals.totalOpeningBalance;
            this.liability_debit = totals.totalDebitAmount;
            this.liability_credit = totals.totalCreditAmount;
            this.liability_closing = totals.totalClosingBalance;
          }
          else {
            this.liability_display = false;
            this.liability_closing = 0.00;

          }
          //this.mainList = result.parentfolders.map((item: any) => ({ ...item, visible: false }));
          //  console.log('main',this.mainList)
          this.subList = result.subfolders3.map((item: any) => ({ ...item, visible: false }));
          this.addItemsFromTargetList();
          var url4 = 'BalanceSheetReport/GetSummaryAsset'
          this.service.getparams(url4, param).subscribe((result: any) => {
            this.responsedata = result;
            // debugger
            //this.mainList1 = result.parentfoldersasset.map((item: any) => ({ ...item, visible: false }));
            this.mainList1 = (result.parentfoldersasset || []).map((item: any) => ({ ...item, visible: false }));
            if (this.mainList1.length != 0) {
              this.asset_display = true;
              const totals = this.sumOfAllProperties(this.mainList1);

              this.asset_opening = totals.totalOpeningBalance;
              this.asset_debit = totals.totalDebitAmount;
              this.asset_credit = totals.totalCreditAmount;
              this.asset_closing = totals.totalClosingBalance;
            }
            else {
              this.asset_display = false;
              this.asset_closing = 0.00;
            }
            let liabilityClosing = this.parseValue1(this.liability_closing);
            let assetClosing = this.parseValue1(this.asset_closing);
            // if (liabilityClosing < assetClosing) {
            //   this.lblNet = "Net Profit:";
            //   this.net_total = assetClosing - (liabilityClosing < 0 ? -liabilityClosing : liabilityClosing);
            //   this.netTotalFormatted = this.formatValue1(this.net_total);
            // } else {
            //   this.lblNet = "Net Loss:";
            //   this.net_total = liabilityClosing - (assetClosing < 0 ? -assetClosing : assetClosing);
            //   this.netTotalFormatted = this.formatValue1(this.net_total);
            // }
            // console.log('this.mainList1 ',this.mainList1 )
            this.subList1 = result.subfolders4.map((item: any) => ({ ...item, visible: false }));
            // console.log('this.subList1',this.subList1 )
            this.addItemsFromTargetList1();
            //debugger





          });

        });
      });
    });



  }
  ngAfterViewInit() {
    if (this.dynamicContentContainer) {
      this.dynamicContentContainer.nativeElement.addEventListener('click', (event: Event) => {
        const target = event.target as HTMLElement;
        if (target.tagName === 'A' && target.classList.contains('button-link')) {
          const path = target.getAttribute('data-path');
          const param1 = target.getAttribute('data-param1');
          const param2 = target.getAttribute('data-param2');
          if (path && param1 && param2) {
            this.navigateTo(path, param1, param2);
          }
        }
      });
    }
    if (this.dynamicContentContainer1) {
      this.dynamicContentContainer1.nativeElement.addEventListener('click', (event: Event) => {
        const target = event.target as HTMLElement;
        if (target.tagName === 'A' && target.classList.contains('button-link')) {
          const path = target.getAttribute('data-path');
          const param1 = target.getAttribute('data-param1');
          const param2 = target.getAttribute('data-param2');
          if (path && param1 && param2) {
            this.navigateTo(path, param1, param2);
          }
        }
      });
    }
    if (this.dynamicContentContainer2) {
      this.dynamicContentContainer2.nativeElement.addEventListener('click', (event: Event) => {
        const target = event.target as HTMLElement;
        if (target.tagName === 'A' && target.classList.contains('button-link')) {
          const path = target.getAttribute('data-path');
          const param1 = target.getAttribute('data-param1');
          const param2 = target.getAttribute('data-param2');
          if (path && param1 && param2) {
            this.navigateTo(path, param1, param2);
          }
        }
      });
    }
  }
  getHtmlContent1(): string {
    return this.html_code2
  }
  getHtmlContent2(): string {
    return this.html_code3
  }
  getHtmlContent(): string {
    return this.html_code
    //     return `
    //     <table width='99%' align='center'>
    //     <tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>
    // <td style='font-size:16px;font-family:calibri;' align='center' width='23%'>Accountgroup Name</td>
    //       <td style='font-size:16px;font-family:calibri;' align='center' width='18%'>Opening Balance</td>
    //       <td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Credit</td>
    //       <td style='font-size:16px;font-family:calibri;' align='center' width='20%'>Debit</td>
    //       <td style='font-size:16px;font-family:calibri;' align='center' width='19%'>Closing Balance</td>
    //     </tr>
    //     <tr>
    //     <td style='font-size:small;font-weight:bold;color:black;' align='left'>Other Income</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>0.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>0.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>31,750.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>-31,750.00</td>
    //     </tr>
    //   </table>
    // <table width='99%' align='center'>
    //   <tr style='font-family:Arial; font-size:14px; color:white;' bgcolor='#4E7DB6'>
    //       <td width='23%'></td>
    //       <td width='18%'></td>
    //       <td width='20%'></td>
    //       <td width='20%'></td>
    //       <td width='19%'></td>
    //     </tr>
    // <tr>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='left'>
    //       <a class="button-link" type="button" data-path="/finance/AccRptProfileandLostDetails" data-param1="FCOA000153" data-param2="PL">Exchange Gain</a>

    //       </td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>0.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>0.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>31,750.00</td>
    //       <td style='font-size:small;font-weight:bold;color:black;' align='right'>-31,750.00</td>
    //     </tr>
    //     </table>
    //     `;
  }

  exportExcel(): void {
    // Create a new table element
    const table = document.createElement('table');

    // Add title row
    const titleRowTop = table.insertRow();
    const titleCellTop = titleRowTop.insertCell();
    titleCellTop.colSpan = 2; // Span across all header columns
    titleCellTop.textContent = 'Balance Sheet Report';
    titleCellTop.style.textAlign = 'center';
    titleCellTop.style.color = 'green';
    titleCellTop.style.fontWeight = 'bolder';
    titleCellTop.style.fontSize = '20px';
    titleCellTop.style.padding = '10px 0';
    titleCellTop.style.border = '1px solid #000';

    // Add "Net Profit" row
    const titleProfitRow = table.insertRow();
    const titleProfitCell = titleProfitRow.insertCell();
    titleProfitCell.colSpan = 1;
    titleProfitCell.textContent = 'Net Profit:';
    titleProfitCell.style.color = '#00317a';
    titleProfitCell.style.fontWeight = 'bold';
    titleProfitCell.style.fontSize = '16px';
    titleProfitCell.style.padding = '10px 0';
    titleProfitCell.style.border = '1px solid #000000';

    // const emptyProfitCell = titleProfitRow.insertCell();
    // emptyProfitCell.colSpan = 3; 
    // emptyProfitCell.style.border = '1px solid #000000';

    const titleNetTotalCell = titleProfitRow.insertCell();
    titleNetTotalCell.colSpan = 1;
    titleNetTotalCell.textContent = this.netTotalFormatted;
    titleNetTotalCell.style.color = '#00317a';
    titleNetTotalCell.style.fontWeight = 'bold';
    titleNetTotalCell.style.fontSize = '16px';
    titleNetTotalCell.style.padding = '10px 0';
    titleNetTotalCell.style.border = '1px solid #000000';

    // Add break rows
    for (let i = 0; i < 1; i++) {
      const breakRow = table.insertRow();
      const breakCell = breakRow.insertCell();
      breakCell.colSpan = 5;
      breakCell.innerHTML = '&nbsp;'; // Add a non-breaking space
    }

    // Add Liability title row
    const titleRow = table.insertRow();
    const titleCell = titleRow.insertCell();
    titleCell.colSpan = 2; // Span across all header columns
    titleCell.textContent = 'Liability';
    titleCell.style.color = '#00317a';
    titleCell.style.fontWeight = 'bold';
    titleCell.style.fontSize = '16px';
    titleCell.style.padding = '10px 0';
    titleCell.style.border = '1px solid #000000';

    // Add header row with background color 'Opening Balance', 'Credit', 'Debit', 'opening_balance', 'credit_amount', 'debit_amount', 
    const headers = ['Account Group', 'Amount'];
    const headerRow = table.insertRow();
    headers.forEach(header => {
      const cell = headerRow.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Function to add rows recursively
    const addRows = (list: any[], level = 0, isMainList = false) => {
      list.forEach(item => {
        // Add main list row
        const dataRow = table.insertRow();
        const accountCell = dataRow.insertCell();
        accountCell.textContent = ' '.repeat(level * 4) + (item.account_name || '');
        accountCell.style.border = '1px solid #000000';

        // Highlight main list rows
        if (isMainList) {
          accountCell.style.backgroundColor = '#d9edf7';
          accountCell.style.fontWeight = 'bolder';
        }

        ['closing_balance'].forEach(key => {
          const cell = dataRow.insertCell();
          cell.textContent = item[key] || '';
          cell.style.border = '1px solid #000000';

          if (isMainList) {
            cell.style.backgroundColor = '#d9edf7';
            cell.style.fontWeight = 'bolder';
          }
        });

        // Add subfolders rows
        if (item.subfolders3 && item.subfolders3.length > 0) {
          addRows(item.subfolders3, level + 1, false);
        }
      });
    };

    addRows(this.mainList, 0, true);

    // Add totals row for Liability
    const totalsRow = table.insertRow();
    const emptyCell = totalsRow.insertCell();
    emptyCell.colSpan = 1;
    emptyCell.style.border = '1px solid #000000';



    // const totalOpeningCell = totalsRow.insertCell();
    // totalOpeningCell.textContent = this.liability_opening;
    // totalOpeningCell.style.color = 'maroon';
    // totalOpeningCell.style.fontSize = '14px';
    // totalOpeningCell.style.border = '1px solid #000000';
    // totalOpeningCell.style.fontWeight = 'bolder';
    // totalOpeningCell.style.textAlign = 'right';

    // const totalCreditCell = totalsRow.insertCell();
    // totalCreditCell.textContent = this.liability_credit;
    // totalCreditCell.style.color = 'maroon';
    // totalCreditCell.style.fontSize = '14px';
    // totalCreditCell.style.border = '1px solid #000000';
    // totalCreditCell.style.textAlign = 'right';
    // totalCreditCell.style.fontWeight = 'bolder';

    // const totalDebitCell = totalsRow.insertCell();
    // totalDebitCell.textContent = this.liability_debit;
    // totalDebitCell.style.color = 'maroon';
    // totalDebitCell.style.fontSize = '14px';
    // totalDebitCell.style.border = '1px solid #000000';
    // totalDebitCell.style.textAlign = 'right';
    // totalDebitCell.style.fontWeight = 'bolder';

    const totalClosingCell = totalsRow.insertCell();
    totalClosingCell.textContent = this.liability_closing;
    totalClosingCell.style.color = 'maroon';
    totalClosingCell.style.fontSize = '14px';
    totalClosingCell.style.border = '1px solid #000000';
    totalClosingCell.style.textAlign = 'right';
    totalClosingCell.style.fontWeight = 'bolder';

    // Add break rows
    for (let i = 0; i < 1; i++) {
      const breakRow = table.insertRow();
      const breakCell = breakRow.insertCell();
      breakCell.colSpan = 5;
      breakCell.innerHTML = '&nbsp;'; // Add a non-breaking space
    }

    // Add Asset title row
    const titleRowAss = table.insertRow();
    const titleCellAss = titleRowAss.insertCell();
    titleCellAss.colSpan = 2; // Span across all header columns
    titleCellAss.textContent = 'Asset';
    titleCellAss.style.color = '#00317a';
    titleCellAss.style.fontWeight = 'bold';
    titleCellAss.style.fontSize = '16px';
    titleCellAss.style.padding = '10px 0';
    titleCellAss.style.border = '1px solid #000000';

    // Add header row with background color 'Opening Balance', 'Debit', 'Credit', 
    const headersAss = ['Account Group', 'Amount'];
    const headerRowAss = table.insertRow();
    headersAss.forEach(header => {
      const cell = headerRowAss.insertCell();
      cell.textContent = header;
      cell.style.backgroundColor = '#00317a';
      cell.style.color = '#FFFFFF';
      cell.style.fontWeight = 'bold';
      cell.style.border = '1px solid #000000';
    });

    // Function to add rows recursively 'opening_balance', 'debit_amount',  'credit_amount',
    const addRows1 = (list: any[], level = 0, isMainList = false) => {
      list.forEach(item => {
        // Add main list row
        const dataRow = table.insertRow();
        const accountCell = dataRow.insertCell();
        accountCell.textContent = ' '.repeat(level * 4) + (item.account_name || '');
        accountCell.style.border = '1px solid #000000';

        // Highlight main list rows
        if (isMainList) {
          accountCell.style.backgroundColor = '#d9edf7';
          accountCell.style.fontWeight = 'bolder';
        }

        ['closing_balance'].forEach(key => {
          const cell = dataRow.insertCell();
          cell.textContent = item[key] || '';
          cell.style.border = '1px solid #000000';

          if (isMainList) {
            cell.style.backgroundColor = '#d9edf7';
            cell.style.fontWeight = 'bolder';
          }
        });

        // Add subfolders rows
        if (item.subfolders4 && item.subfolders4.length > 0) {
          addRows1(item.subfolders4, level + 1, false);
        }
      });
    };

    addRows1(this.mainList1, 0, true);

    const totalsRowAssets = table.insertRow();
    const emptyCellAsset = totalsRowAssets.insertCell();
    emptyCellAsset.colSpan = 1;
    emptyCellAsset.style.border = '1px solid #000000';



    // const totalOpeningCellAsset = totalsRowAssets.insertCell();
    // totalOpeningCellAsset.textContent = this.asset_opening;
    // totalOpeningCellAsset.style.color = 'maroon';
    // totalOpeningCellAsset.style.fontSize = '14px';
    // totalOpeningCellAsset.style.border = '1px solid #000000';
    // totalOpeningCellAsset.style.fontWeight = 'bolder';
    // totalOpeningCellAsset.style.textAlign = 'right';


    // const totalDebitCellAsset = totalsRowAssets.insertCell();
    // totalDebitCellAsset.textContent = this.asset_debit;
    // totalDebitCellAsset.style.color = 'maroon';
    // totalDebitCellAsset.style.fontSize = '14px';
    // totalDebitCellAsset.style.border = '1px solid #000000';
    // totalDebitCellAsset.style.textAlign = 'right';
    // totalDebitCellAsset.style.fontWeight = 'bolder';

    // const totalCreditCellAsset = totalsRowAssets.insertCell();
    // totalCreditCellAsset.textContent = this.asset_credit;
    // totalCreditCellAsset.style.color = 'maroon';
    // totalCreditCellAsset.style.fontSize = '14px';
    // totalCreditCellAsset.style.border = '1px solid #000000';
    // totalCreditCellAsset.style.textAlign = 'right';
    // totalCreditCellAsset.style.fontWeight = 'bolder';


    const totalClosingCellAsset = totalsRowAssets.insertCell();
    totalClosingCellAsset.textContent = this.asset_closing;
    totalClosingCellAsset.style.color = 'maroon';
    totalClosingCellAsset.style.fontSize = '14px';
    totalClosingCellAsset.style.border = '1px solid #000000';
    totalClosingCellAsset.style.textAlign = 'right';
    totalClosingCellAsset.style.fontWeight = 'bolder';

    // Convert the table to a data URI
    const tableHtml = table.outerHTML;
    const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

    // Trigger download
    const link = document.createElement('a');
    link.href = dataUri;
    link.download = 'Balance Sheet Report.xls';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }



  downloadPDF() {
    const doc = new jsPDF('landscape', 'mm', 'a4');

    doc.setFontSize(18);
    doc.setTextColor('green');

    doc.text('Balance Sheet Report', 120, 12);
    doc.text('Net Profit : ' + this.netTotalFormatted, 14, 22);
    doc.setFontSize(18);
    doc.setTextColor(0, 51, 153);

    doc.text('Liability', 14, 32);

    const headers = [['S.No', 'Account Name', 'Opening Balance', 'Credit Amount', 'Debit Amount', 'Closing Amount']];

    const data = this.flattenData(this.mainList);
    const options = {
      startY: 35,
      head: headers,
      body: data,
      theme: 'grid',
      styles: {
        fontSize: 10,
        cellPadding: 3,
      },
      columnStyles: {
        0: { fontStyle: 'bold', textColor: 'white', fillColor: [16, 44, 87] },

      },

      headStyles: {
        fillColor: [225, 227, 234],
        textColor: [0, 0, 0],
        fontStyle: 'bold',
      },
    };
    (doc as any).autoTable(options);
    // doc.setFontSize(12);
    // doc.setTextColor('maroon');
    // doc.text('Total : ' ,50, 50);
    // doc.text(this.liability_opening, 126, 50);
    // doc.text(this.liability_credit, 172, 50);
    // doc.text(this.liability_debit, 210, 50);
    // doc.text(this.liability_closing, 258, 50);
    doc.addPage();
    doc.setFontSize(18);
    doc.setTextColor(0, 51, 153);
    doc.text('Asset', 14, 32);

    const datas = this.flattenDataAsset(this.mainList1);

    const options1 = {
      startY: 35,
      head: headers,
      body: datas,
      theme: 'grid',
      styles: {
        fontSize: 10,
        cellPadding: 3,
      },
      columnStyles: {
        0: { fontStyle: 'bold', textColor: 'white', fillColor: [16, 44, 87] },

      },

      headStyles: {
        fillColor: [225, 227, 234],
        textColor: [0, 0, 0],
        fontStyle: 'bold',
      },
    };

    (doc as any).autoTable(options1);
    // doc.setFontSize(12);
    // doc.setTextColor('maroon');
    // doc.text('Total : ' ,50, 100);
    // doc.text(this.asset_opening, 140, 100);
    // doc.text(this.asset_debit, 183, 100);
    // doc.text(this.asset_credit, 223, 100);
    // doc.text(this.asset_closing, 258, 100);

    const finalY = (doc as any).previousAutoTable.finalY || 70;
    doc.setFontSize(12);
    doc.setTextColor(255, 0, 0);
    doc.text(
      'Note: Kindly Confirm the same within 15 days or else the balance as per our records is treated as correct.',
      14,
      finalY + 13
    );

    doc.save('balance_sheet_report.pdf');
  }

  flattenData(data: any[], parentIndex = '', level = 0): any[] {
    return data.reduce((acc, item, index) => {
      const currentIndex = parentIndex ? `${parentIndex}.${index + 1}` : `${index + 1}`;
      const row = [
        currentIndex,
        ' '.repeat(level * 4) + item.account_name,
        { content: item.opening_balance, styles: { halign: 'right' } },
        { content: item.credit_amount, styles: { halign: 'right' } },
        { content: item.debit_amount, styles: { halign: 'right' } },
        { content: item.closing_balance, styles: { halign: 'right' } },
      ];

      acc.push(row);

      if (item.subfolders3 && item.subfolders3.length > 0) {
        acc = acc.concat(this.flattenData(item.subfolders3, currentIndex, level + 1));
      }

      return acc;
    }, []);
  }

  flattenDataAsset(data: any[], parentIndex = '', level = 0): any[] {
    return data.reduce((acc, item, index) => {
      const currentIndex = parentIndex ? `${parentIndex}.${index + 1}` : `${index + 1}`;
      const row = [
        currentIndex,
        ' '.repeat(level * 4) + item.account_name,
        { content: item.opening_balance, styles: { halign: 'right' } },
        { content: item.debit_amount, styles: { halign: 'right' } },
        { content: item.credit_amount, styles: { halign: 'right' } },
        { content: item.closing_balance, styles: { halign: 'right' } },
      ];

      acc.push(row);

      if (item.subfolders4 && item.subfolders4.length > 0) {
        acc = acc.concat(this.flattenData(item.subfolders4, currentIndex, level + 1));
      }

      return acc;
    }, []);
  }






  getsubmitsummary() {
    let param = {
      branch: this.reactiveform.value.frombranch,
      year_gid: this.reactiveform.value.finyear
    };

    this.NgxSpinnerService.show(); // Show spinner at the beginning
    var url = 'BalanceSheetReport/GetNetAmountDetails';
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#Balancesheetoverallnetvalue').DataTable().destroy();
      this.responsedata = result;
      this.Balancesheetoverallnetvalue = this.responsedata.Balancesheetoverallnetvalue;
      //console.log(this.Balancesheetoverallnetvalue[0].lblpandlvalue)
      this.netTotalFormatted = this.Balancesheetoverallnetvalue[0].lblpandlvalue;
      this.lblNet = this.Balancesheetoverallnetvalue[0].lblpandl;

      //console.log(this.Balancesheetoverallnetvalue[0].lblpandl)
    });
    var url2 = 'BalanceSheetReport/GetBalanceSheetAsset';
    this.service.getparams(url2, param).subscribe((result: any) => {
      this.responsedata = result;
      this.profitlossincome_list = this.responsedata.BalanceSheetasset_list;
      if (this.profitlossincome_list != null && this.profitlossincome_list != '') {
        this.html_code2 = this.profitlossincome_list[0].html_content;
        this.income_closebal = this.profitlossincome_list[0].income_closebal;
        this.html_income = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent1());
        this.incomeFlag = this.html_income != "" && this.html_income != null;
      } else {
        this.income_closebal = null;
        this.html_income = "";
        this.incomeFlag = false;
      }

      var url3 = 'BalanceSheetReport/GetBalanceSheetLiability';
      this.service.getparams(url3, param).subscribe((result: any) => {
        this.responsedata = result;
        this.profitlossExpense_list = this.responsedata.BalanceSheetliability_list;
        if (this.profitlossExpense_list != null && this.profitlossExpense_list != '') {
          this.expense_closebal = this.profitlossExpense_list[0].expense_closebal;
          this.html_code3 = this.profitlossExpense_list[0].html_content;
          this.html_expense = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent2());
          this.expenseFlag = this.html_expense != "" && this.html_expense != null;
        } else {
          this.expense_closebal = '0.00';
          this.html_expense = "";
          this.expenseFlag = false;
        }

        if ((this.income_closebal != null && this.income_closebal != "" && this.income_closebal != undefined) || (this.expense_closebal != null && this.expense_closebal != "" && this.expense_closebal != undefined)) {
          const income_total: number = parseFloat(this.income_closebal);
          const expense_total: number = parseFloat(this.expense_closebal);
          this.net_total = expense_total - (income_total < 0 ? -income_total : income_total);
          const formatter = new Intl.NumberFormat('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
          });
          this.net_total = formatter.format(this.net_total);
          this.net_flag = true;
        } else {
          this.net_flag = false;
        }

        var url4 = 'BalanceSheetReport/GetSummaryLiability';
        this.service.getparams(url4, param).subscribe((result: any) => {
          this.responsedata = result;
          this.mainList = (result.parentfoldersliability || []).map((item: any) => ({ ...item, visible: false }));
          if (this.mainList.length != 0) {
            const totals = this.sumOfAllProperties(this.mainList);
            this.liability_display = true;
            this.liability_opening = totals.totalOpeningBalance;
            this.liability_debit = totals.totalDebitAmount;
            this.liability_credit = totals.totalCreditAmount;
            this.liability_closing = totals.totalClosingBalance;
          } else {
            this.liability_display = false;
            this.liability_closing = 0.00;
          }
          this.subList = result.subfolders3.map((item: any) => ({ ...item, visible: false }));
          this.addItemsFromTargetList();

          var url5 = 'BalanceSheetReport/GetSummaryAsset';
          this.service.getparams(url5, param).subscribe((result: any) => {
            this.responsedata = result;
            this.mainList1 = (result.parentfoldersasset || []).map((item: any) => ({ ...item, visible: false }));
            if (this.mainList1.length != 0) {
              this.asset_display = true;
              const totals = this.sumOfAllProperties(this.mainList1);
              this.asset_opening = totals.totalOpeningBalance;
              this.asset_debit = totals.totalDebitAmount;
              this.asset_credit = totals.totalCreditAmount;
              this.asset_closing = totals.totalClosingBalance;
            } else {
              this.asset_display = false;
              this.asset_closing = 0.00;
            }

            let liabilityClosing = this.parseValue1(this.liability_closing);
            let assetClosing = this.parseValue1(this.asset_closing);

            this.subList1 = result.subfolders4.map((item: any) => ({ ...item, visible: false }));
            this.addItemsFromTargetList1();

            this.NgxSpinnerService.hide(); // Hide spinner after all operations are completed
          }, (error) => {
            console.error('Error fetching summary asset data', error);
            this.NgxSpinnerService.hide(); // Hide spinner in case of error
          });
        }, (error) => {
          console.error('Error fetching summary liability data', error);
          this.NgxSpinnerService.hide(); // Hide spinner in case of error
        });
      }, (error) => {
        console.error('Error fetching balance sheet liability data', error);
        this.NgxSpinnerService.hide(); // Hide spinner in case of error
      });
    }, (error) => {
      console.error('Error fetching balance sheet asset data', error);
      this.NgxSpinnerService.hide(); // Hide spinner in case of error
    });
  }

  parseValue1(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue1(value: number): string {
    // Format the number as a string with comma separators and two decimal places
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  sumOfAllProperties(list: any[]): {
    totalOpeningBalance: string,
    totalDebitAmount: string,
    totalCreditAmount: string,
    totalClosingBalance: string
  } {
    const result = list.reduce((acc, account) => {
      acc.totalOpeningBalance += this.parseValue(account.opening_balance);
      acc.totalDebitAmount += this.parseValue(account.debit_amount);
      acc.totalCreditAmount += this.parseValue(account.credit_amount);
      acc.totalClosingBalance += this.parseValue(account.closing_balance);
      return acc;
    }, {
      totalOpeningBalance: 0,
      totalDebitAmount: 0,
      totalCreditAmount: 0,
      totalClosingBalance: 0
    });

    return {
      totalOpeningBalance: this.formatValue(result.totalOpeningBalance),
      totalDebitAmount: this.formatValue(result.totalDebitAmount),
      totalCreditAmount: this.formatValue(result.totalCreditAmount),
      totalClosingBalance: this.formatValue(result.totalClosingBalance)
    };
  }

  parseValue(value: any): number {
    // Convert the value to a number, removing any non-numeric characters (e.g., commas)
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue(value: number): string {
    // Format the number as a string with comma separators
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }
  addItemsFromTargetList() {
    this.subList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.mainList);
      // this.recursivelyAddItems(targetItem, this.mainListTwo);
      // this.recursivelyAddItems(targetItem, this.assetfolder);
      // this.recursivelyAddItems(targetItem, this.liabilityfolder);
    });

    //debugger

    // this.subList.forEach((targetItem: any) => {
    //   this.recursivelyAddItems(targetItem, this.mainListTwo);
    // });
  }
  addItemsFromTargetList1() {
    this.subList1.forEach((targetItem: any) => {
      this.recursivelyAddItems1(targetItem, this.mainList1);
      // this.recursivelyAddItems(targetItem, this.mainListTwo);
      // this.recursivelyAddItems(targetItem, this.assetfolder);
      // this.recursivelyAddItems(targetItem, this.liabilityfolder);
    });

    //debugger

    // this.subList.forEach((targetItem: any) => {
    //   this.recursivelyAddItems(targetItem, this.mainListTwo);
    // });
  }
  toggleVisibility(item: any) {
    item.visible = !item.visible;

  }
  toggleVisibility1(item: any) {
    item.visible = !item.visible;

  }
  recursivelyAddItems1(targetItem: any, sourceList: any[]) {
    const matchingIndex1 = sourceList.findIndex(sourceItem => sourceItem.account_gid === targetItem.accountgroup_gid);
    if (matchingIndex1 !== -1) {
      if (!sourceList[matchingIndex1].subfolders4) {
        sourceList[matchingIndex1].subfolders4 = [];
      }
      sourceList[matchingIndex1].subfolders4.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.subfolders4 && sourceItem.subfolders4.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.subfolders4);
        }
      });
    }
  }
  recursivelyAddItems(targetItem: any, sourceList: any[]) {
    const matchingIndex = sourceList.findIndex(sourceItem => sourceItem.account_gid === targetItem.accountgroup_gid);
    if (matchingIndex !== -1) {
      if (!sourceList[matchingIndex].subfolders3) {
        sourceList[matchingIndex].subfolders3 = [];
      }
      sourceList[matchingIndex].subfolders3.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.subfolders3 && sourceItem.subfolders3.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.subfolders3);
        }
      });
    }
  }
  onsubmit() {
    // console.log(this.reactiveform.value)

    this.getsubmitsummary();
  }
  navigateTo(path: string, param1: string, param2: string): void {
    this.router.navigate([path, param1, param2]);
  }



  onroute(params: any) {
    //debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = 'BS';
    const encryptedParam1 = AES.encrypt(lspage, secretKey).toString();
    this.router.navigate(['/finance/AccRptProfitandLostDetails', encryptedParam, encryptedParam1])
  }
  onprofit() {
    const lspage = 'PL';
    this.router.navigate(['/finance/AccRptProfitandLost', lspage])
  }

}
