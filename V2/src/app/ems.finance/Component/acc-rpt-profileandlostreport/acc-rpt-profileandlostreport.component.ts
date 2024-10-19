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
  selector: 'app-acc-rpt-profileandlostreport',
  templateUrl: './acc-rpt-profileandlostreport.component.html',
  styleUrls: ['./acc-rpt-profileandlostreport.component.scss']
})
export class AccRptProfileandlostreportComponent {
  expenseFlag: boolean = false;
  incomeFlag: boolean = false;
  net_flag: boolean = false;
  income_display: boolean = false;
  expense_display: boolean = false;
  back_flag: boolean  = false;
  responsedata: any;
  GetGstManagement_list:any;
  month:any;
  year:any;
  profitlossexcel_list:any;
  lblNet:any;
  profitlosspdf_list:any;
  htmlContent:any;
  profitlossincome_list:any;
  mainList: any[] = [];
  subList: any[] = [];
  mainList1: any[] = [];
  subList1: any[] = [];
  htmlContent1:any;
  html_code:any;
  html_code2:any;
  html_code3:any;
  content_html:any
  html_income:any;
  html_expense:any;
  profitlossExpense_list:any;
  income_closebal:any;
  expense_closebal:any;
  branchname_list:any;
  reactiveform: FormGroup | any;
  deafultbranch:any;
  net_total:any;
  deafultfin:any;
  GetProfilelossfinyear_list:any;
  income_opening:any;
income_debit:any;
income_credit:any;
income_closing:any;
expense_opening:any;
expense_debit:any;
expense_credit:any;
expense_closing:any;
netTotalFormatted:any;
lspage:any
  @ViewChild('dynamicContentContainer') dynamicContentContainer!: ElementRef;
  @ViewChild('dynamicContentContainer1') dynamicContentContainer1!: ElementRef;
  @ViewChild('dynamicContentContainer2') dynamicContentContainer2!: ElementRef;
  @ViewChild('contentToConvert1') contentToConvert1!: ElementRef;

  constructor(public service: SocketService, private el: ElementRef ,private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer,  private renderer: Renderer2, private router: Router, private ToastrService: ToastrService) {
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
    this.lspage= this.route.snapshot.paramMap.get('lspage');
    if(this.lspage !=null)
    {
     this.back_flag=true;
    }
    else{
      this.back_flag=false;
    }
    this.getsummary();

  }
  
  getsummary()
  {
//  console.log('bran',localStorage.getItem('deafultbranch'))
//   console.log('fin',localStorage.getItem('deafultfin'))
var url = 'AccMstBankMaster/GetBranchName'
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      this.deafultbranch =this.branchname_list[0].branch_gid; 
      // localStorage.removeItem('deafultbranch');
      // localStorage.setItem('deafultbranch', this.deafultbranch.toString());
      var url = 'ProfitLossReport/GetProfilelossfinyear'
      this.service.get(url).subscribe((result: any) => {
        this.GetProfilelossfinyear_list = result.GetProfilelossfinyear_list;
        this.deafultfin =this.GetProfilelossfinyear_list[0].finyear_gid; 
        // localStorage.removeItem('deafultfin');
        // localStorage.setItem('deafultfin', this.deafultfin.toString());
        let param = {
          branch: this.deafultbranch,
          year_gid:this.deafultfin
          }
         // var url1 = 'ProfitLossReport/GetProfitLossExportExcel'
          // this.service.getparams(url1, param).subscribe((result: any) => {
          //   this.responsedata = result;
          //   this.profitlossexcel_list = this.responsedata.profitlossexcel_list;
          //  this.html_code =this.profitlossexcel_list[0].html_content;
          //  this.htmlContent = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent());
          // });
          var url2 = 'ProfitLossReport/GetProfitLossIncome'
          this.service.getparams(url2, param).subscribe((result: any) => {
            this.responsedata = result;
            this.profitlossincome_list = this.responsedata.profitlossincome_list;
            if( this.profitlossincome_list !=null &&  this.profitlossincome_list!='')
            {
            this.html_code2 = this.profitlossincome_list[0].html_content;
            this.income_closebal = this.profitlossincome_list[0].income_closebal;
            this.html_income = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent1());
           // console.log('inc',this.income_closebal)
           if(this.html_income !="" &&  this.html_income !=null)
           {

             this.incomeFlag =true;
           }
           else 
           {
            this.income_closebal ='0.00';
            this.html_income="";
             this.incomeFlag =false;
           }
          }
          else {
            this.income_closebal ='0.00';
            this.html_income="";
          this.incomeFlag =false;
          }
           var url3 = 'ProfitLossReport/GetProfitLossExpense'
           this.service.getparams(url3, param).subscribe((result: any) => {
             this.responsedata = result;
             this.profitlossExpense_list = this.responsedata.profitlossExpense_list;
             if( this.profitlossExpense_list !=null &&  this.profitlossExpense_list!='')
             {
             this.expense_closebal = this.profitlossExpense_list[0].expense_closebal;      
             this.html_code3 = this.profitlossExpense_list[0].html_content;
             this.html_expense = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent2());
            // console.log('exp',this.expense_closebal)
            if(this.html_expense !="" &&  this.html_expense !=null)
            {          
              this.expenseFlag =true;
            }
            else 
            {
              this.expense_closebal ='0.00';
              this.html_expense="";
              this.expenseFlag =false;
            }
           
          }
          else 
          {
            
            this.expense_closebal ='0.00';
            this.html_expense="";
            this.expenseFlag =false;
          }
          if((this.income_closebal !=null && this.income_closebal !="" && this.income_closebal !=undefined) || (this.expense_closebal !=null && this.expense_closebal !=""  && this.expense_closebal !=undefined))
          {

          
          const  income_total:number  = parseFloat(this.income_closebal);
          const  expense_total:number  = parseFloat(this.expense_closebal);
          this.net_total = expense_total -  (income_total < 0 ? -income_total : income_total);
          const formatter = new Intl.NumberFormat('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
          });
    
          this.net_total = formatter.format(this.net_total);
          this.net_flag =true;
        }
        else 
        {
          this.net_flag=false;

          
        }
          
      });
    });
    var url3 = 'ProfitLossReport/GetSummaryExpense'
      this.service.getparams(url3, param).subscribe((result: any) => {
        this.responsedata = result;
        debugger
       // this.mainList =  result.GetSummaryExpenseparent.map((item: any) => ({ ...item, visible: false })) ;
        //this.subList = result.GetSummaryExpensechild.map((item: any) => ({ ...item, visible: false }));
        this.mainList = (result.parentfolders || []).map((item: any) => ({ ...item, visible: false }));
        if(this.mainList.length !=0)
        {
          const totals = this.sumOfAllProperties(this.mainList);
          this.expense_display = true;
          this.expense_opening=totals.totalOpeningBalance;
          this.expense_debit=totals.totalDebitAmount;
          this.expense_credit=totals.totalCreditAmount;
          this.expense_closing=totals.totalClosingBalance;
        }
        else 
        {
          this.expense_display = false;
          this.expense_closing=0.00;
           
        }
        //this.mainList = result.parentfolders.map((item: any) => ({ ...item, visible: false }));
      //  console.log('main',this.mainList)
        this.subList = result.subfolders1.map((item: any) => ({ ...item, visible: false }));
        this.addItemsFromTargetList();
        var url4 = 'ProfitLossReport/GetSummaryIncome'
        this.service.getparams(url4, param).subscribe((result: any) => {
          this.responsedata = result;
         // debugger
        //this.mainList1 = result.parentfoldersincome.map((item: any) => ({ ...item, visible: false }));
        this.mainList1 = (result.parentfoldersincome || []).map((item: any) => ({ ...item, visible: false }));
        if(this.mainList1.length !=0)
        {
          this.income_display = true;
          const totals = this.sumOfAllProperties(this.mainList1);
        
        this.income_opening=totals.totalOpeningBalance;
        this.income_debit=totals.totalDebitAmount;
        this.income_credit=totals.totalCreditAmount;
        this.income_closing=totals.totalClosingBalance;
        }
        else
        {
          this.income_display = false;
          this.income_closing=0.00;
        }
        let expenseClosing = this.parseValue1(this.expense_closing);
        let incomeClosing =  this.parseValue1(this.income_closing);
        if (incomeClosing > expenseClosing) {
          this.lblNet = "Net Profit:";
          this.net_total = incomeClosing - (expenseClosing < 0 ? -expenseClosing : expenseClosing);
          this.netTotalFormatted = this.formatValue1(this.net_total);
        } else {
          this.lblNet = "Net Loss:";
          this.net_total = expenseClosing - (incomeClosing < 0 ? -incomeClosing : incomeClosing);
          this.netTotalFormatted = this.formatValue1(this.net_total);
        }
       
        // console.log('this.mainList1 ',this.mainList1 )
        this.subList1 = result.subfolders2.map((item: any) => ({ ...item, visible: false }));
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
}
getsubmitsummary() {
  let param = {
    branch: this.reactiveform.value.frombranch,
    year_gid: this.reactiveform.value.finyear
  };

  this.NgxSpinnerService.show(); // Show spinner at the beginning

  var url2 = 'ProfitLossReport/GetProfitLossIncome';
  this.service.getparams(url2, param).subscribe((result: any) => {
    this.responsedata = result;
    this.profitlossincome_list = this.responsedata.profitlossincome_list;
    if (this.profitlossincome_list != null && this.profitlossincome_list != '') {
      this.html_code2 = this.profitlossincome_list[0].html_content;
      this.income_closebal = this.profitlossincome_list[0].income_closebal;
      this.html_income = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent1());
      this.incomeFlag = this.html_income != "" && this.html_income != null;
    } else {
      this.html_income = "";
      this.income_closebal = '0.00';
      this.incomeFlag = false;
    }

    var url3 = 'ProfitLossReport/GetProfitLossExpense';
    this.service.getparams(url3, param).subscribe((result: any) => {
      this.responsedata = result;
      this.profitlossExpense_list = this.responsedata.profitlossExpense_list;
      if (this.profitlossExpense_list != null && this.profitlossExpense_list != '') {
        this.expense_closebal = this.profitlossExpense_list[0].expense_closebal;
        this.html_code3 = this.profitlossExpense_list[0].html_content;
        this.html_expense = this.sanitizer.bypassSecurityTrustHtml(this.getHtmlContent2());
        this.expenseFlag = this.html_expense != "" && this.html_expense != null;
      } else {
        this.html_expense = "";
        this.expense_closebal = '0.00';
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

      var url4 = 'ProfitLossReport/GetSummaryExpense';
      this.service.getparams(url4, param).subscribe((result: any) => {
        this.responsedata = result;
        this.mainList = (result.parentfolders || []).map((item: any) => ({ ...item, visible: false }));
        if (this.mainList.length != 0) {
          const totals = this.sumOfAllProperties(this.mainList);
          this.expense_display = true;
          this.expense_opening = totals.totalOpeningBalance;
          this.expense_debit = totals.totalDebitAmount;
          this.expense_credit = totals.totalCreditAmount;
          this.expense_closing = totals.totalClosingBalance;
        } else {
          this.expense_display = false;
          this.expense_closing = 0.00;
        }
        this.subList = result.subfolders1.map((item: any) => ({ ...item, visible: false }));
        this.addItemsFromTargetList();

        var url5 = 'ProfitLossReport/GetSummaryIncome';
        this.service.getparams(url5, param).subscribe((result: any) => {
          this.responsedata = result;
          this.mainList1 = (result.parentfoldersincome || []).map((item: any) => ({ ...item, visible: false }));
          if (this.mainList1.length != 0) {
            this.income_display = true;
            const totals = this.sumOfAllProperties(this.mainList1);
            this.income_opening = totals.totalOpeningBalance;
            this.income_debit = totals.totalDebitAmount;
            this.income_credit = totals.totalCreditAmount;
            this.income_closing = totals.totalClosingBalance;
          } else {
            this.income_display = false;
            this.income_closing = 0.00;
          }

          let expenseClosing = this.parseValue1(this.expense_closing);
          let incomeClosing = this.parseValue1(this.income_closing);
          if (incomeClosing > expenseClosing) {
            this.lblNet = "Net Profit:";
            this.net_total = incomeClosing - (expenseClosing < 0 ? -expenseClosing : expenseClosing);
            this.netTotalFormatted = this.formatValue1(this.net_total);
          } else {
            this.lblNet = "Net Loss:";
            this.net_total = expenseClosing - (incomeClosing < 0 ? -incomeClosing : incomeClosing);
            this.netTotalFormatted = this.formatValue1(this.net_total);
          }

         

          this.subList1 = result.subfolders2.map((item: any) => ({ ...item, visible: false }));
          this.addItemsFromTargetList1();

          this.NgxSpinnerService.hide(); // Hide spinner after all operations are completed
        }, (error) => {
          console.error('Error fetching summary income data', error);
          this.NgxSpinnerService.hide(); // Hide spinner in case of error
        });
      }, (error) => {
        console.error('Error fetching summary expense data', error);
        this.NgxSpinnerService.hide(); // Hide spinner in case of error
      });
    }, (error) => {
      console.error('Error fetching profit/loss expense data', error);
      this.NgxSpinnerService.hide(); // Hide spinner in case of error
    });
  }, (error) => {
    console.error('Error fetching profit/loss income data', error);
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
  onsubmit()
  {
// console.log(this.reactiveform.value)

 this.getsubmitsummary();
  }
  navigateTo(path: string, param1: string, param2: string): void {
    this.router.navigate([path, param1, param2]);
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
      if (!sourceList[matchingIndex].subfolders1) {
        sourceList[matchingIndex].subfolders1 = [];
      }
      sourceList[matchingIndex].subfolders1.push({ ...targetItem, visible: false });
    } else {
      sourceList.forEach(sourceItem => {
        if (sourceItem.subfolders1 && sourceItem.subfolders1.length > 0) {
          this.recursivelyAddItems(targetItem, sourceItem.subfolders1);
        }
      });
    }
  }
 


  onroute(params: any) {
    //debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage ='PL';
    const encryptedParam1 = AES.encrypt(lspage, secretKey).toString();
    this.router.navigate(['/finance/AccRptProfitandLostDetails', encryptedParam,encryptedParam1])
  }
  onback()
  {
      this.router.navigate(['/finance/AccRptBalanceSheeet'])
  }
}
