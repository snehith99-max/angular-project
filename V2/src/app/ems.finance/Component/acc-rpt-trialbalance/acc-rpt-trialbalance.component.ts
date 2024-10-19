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

interface CustomOptions extends Options {
  dpi?: number;
}

@Component({
  selector: 'app-acc-rpt-trialbalance',
  templateUrl: './acc-rpt-trialbalance.component.html',
  styleUrls: ['./acc-rpt-trialbalance.component.scss']
})

export class AccRptTrialbalanceComponent {
  expenseFlag: boolean = false;
  incomeFlag: boolean = false;
  net_flag: boolean = false;
  income_display: boolean = false;
  expense_display: boolean = false;
  back_flag: boolean = false;
  responsedata: any;
  GetGstManagement_list: any;
  month: any;
  year: any;
  profitlossexcel_list: any;
  lblNet: any;
  profitlosspdf_list: any;
  htmlContent: any;
  profitlossincome_list: any;
  mainList: any[] = [];
  subList: any[] = [];
  mainList1: any[] = [];
  subList1: any[] = [];
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
  GetProfilelossfinyear_list: any;
  income_opening: any;
  income_debit: any;
  income_credit: any;
  income_closing: any;
  expense_opening: any;
  expense_debit: any;
  expense_credit: any;
  expense_closing: any;
  netTotalFormatted: any;
  lspage: any
  @ViewChild('dynamicContentContainer') dynamicContentContainer!: ElementRef;
  @ViewChild('dynamicContentContainer1') dynamicContentContainer1!: ElementRef;
  @ViewChild('dynamicContentContainer2') dynamicContentContainer2!: ElementRef;
  @ViewChild('contentToConvert1') contentToConvert1!: ElementRef;

  constructor(public service: SocketService, private el: ElementRef, private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer, private renderer: Renderer2, private router: Router, private ToastrService: ToastrService) {
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
    // debugger
    this.lspage = this.route.snapshot.paramMap.get('lspage');
    if (this.lspage != null) {
      this.back_flag = true;
    }
    else {
      this.back_flag = false;
    }
    this.getsummary();
  }

  getsummary() {
    var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      this.deafultbranch = this.branchname_list[0].branch_gid;

      var url = 'ProfitLossReport/GetProfilelossfinyear'
      this.service.get(url).subscribe((result: any) => {
        this.GetProfilelossfinyear_list = result.GetProfilelossfinyear_list;
        this.deafultfin = this.GetProfilelossfinyear_list[0].finyear_gid;

        let param = {
          branch: this.deafultbranch,
          year_gid: this.deafultfin
        }

        var url3 = 'AccTrailBalanceReport/GetTrialBalanceSummary'
        this.service.getparams(url3, param).subscribe((result: any) => {
          this.responsedata = result;
          debugger

          this.mainList = (result.parent_folders || []).map((item: any) => ({ ...item, visible: false }));
          if (this.mainList.length != 0) {
            const totals = this.sumOfAllProperties(this.mainList);
            this.expense_display = true;
            this.expense_debit = totals.totalDebitAmount;
            this.expense_credit = totals.totalCreditAmount;
          }
          else {
            this.expense_display = false;
            this.expense_closing = 0.00;
          }

          this.subList = result.sub_folders1.map((item: any) => ({ ...item, visible: false }));
          this.addItemsFromTargetList();
        });

      });
    });
  }

  getsubmitsummary() {
    let param = {
      branch: this.reactiveform.value.frombranch,
      year_gid: this.reactiveform.value.finyear
    };

    this.NgxSpinnerService.show(); // Show spinner at the beginning 

    var url3 = 'AccTrailBalanceReport/GetTrialBalanceSummary'
    this.service.getparams(url3, param).subscribe((result: any) => {
      this.responsedata = result;
      debugger

      this.mainList = (result.parent_folders || []).map((item: any) => ({ ...item, visible: false }));
      if (this.mainList.length != 0) {
        debugger
        const totals = this.sumOfAllProperties(this.mainList);
        this.expense_display = true;
        this.expense_debit = totals.totalDebitAmount;
        this.expense_credit = totals.totalCreditAmount;
      }
      else {
        this.expense_display = false;
        this.expense_closing = 0.00;
      }

      this.subList = result.sub_folders1.map((item: any) => ({ ...item, visible: false }));
      this.addItemsFromTargetList();
      this.NgxSpinnerService.hide(); // Show spinner at the beginning
    });
  }

  // parseValue1(value: any): number {
  //   // Convert the value to a number, removing any non-numeric characters (e.g., commas)
  //   return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  // }

  // formatValue1(value: number): string {
  //   // Format the number as a string with comma separators and two decimal places
  //   return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  // }

  sumOfAllProperties(list: any[]): {
    totalDebitAmount: string,
    totalCreditAmount: string
  }
  {
    const result = list.reduce((acc, account) => {
      acc.totalDebitAmount += this.parseValue(account.debit_amount);
      acc.totalCreditAmount += this.parseValue(account.credit_amount);
      return acc;
    }, {
      totalDebitAmount: 0,
      totalCreditAmount: 0,
    });

    return {
      totalDebitAmount: this.formatValue(result.totalDebitAmount),
      totalCreditAmount: this.formatValue(result.totalCreditAmount)
    };
  }

  parseValue(value: any): number {
    return parseFloat(value.toString().replace(/[^0-9.-]+/g, '')) || 0;
  }

  formatValue(value: number): string {
    return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
  }

  addItemsFromTargetList() {
    this.subList.forEach((targetItem: any) => {
      this.recursivelyAddItems(targetItem, this.mainList);
    });
  }

  addItemsFromTargetList1() {
    this.subList1.forEach((targetItem: any) => {
      this.recursivelyAddItems1(targetItem, this.mainList1);
    });
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
      if (!sourceList[matchingIndex1].subfolders2) {
        sourceList[matchingIndex1].subfolders2 = [];
      }
      sourceList[matchingIndex1].subfolders2.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.subfolders2 && sourceItem.subfolders2.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.subfolders2);
        }
      });
    }
  }

  recursivelyAddItems(targetItem: any, sourceList: any[]) {
    const matchingIndex = sourceList.findIndex(sourceItem => sourceItem.account_gid === targetItem.accountgroup_gid);
    if (matchingIndex !== -1) {
      if (!sourceList[matchingIndex].sub_folders1) {
        sourceList[matchingIndex].sub_folders1 = [];
      }
      sourceList[matchingIndex].sub_folders1.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.sub_folders1 && sourceItem.sub_folders1.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.sub_folders1);
        }
      });
    }
  }

  onroute(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = 'TB';
    const encryptedParam1 = AES.encrypt(lspage, secretKey).toString();
    this.router.navigate(['/finance/AccRptProfitandLostDetails', encryptedParam, encryptedParam1])
  }
}
