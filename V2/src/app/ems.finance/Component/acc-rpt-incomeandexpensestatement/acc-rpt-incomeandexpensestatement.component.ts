import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { catchError, finalize } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-acc-rpt-incomeandexpensestatement',
  templateUrl: './acc-rpt-incomeandexpensestatement.component.html',
  styleUrls: ['./acc-rpt-incomeandexpensestatement.component.scss']
})
export class AccRptIncomeandexpensestatementComponent {
  FinancialYear_List: any[] = [];
  finyear: any;
  Branchdtl_list: any[] = [];
  branch_name: any; 
  IncomeSummary_list: any[] = [];
  ExpenseSummary_list: any[] = [];
  processedIncome_List: any[] = [];
  processedExpense_List: any[] = [];
  processedData: any[] = [];
  prevMainGroup: any;
  prevSubGroup: any;
  currentTab = 'Expense';
  totalCredit: any;
  totalDebit: any;
  difference_value: any;
  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService){

  }
  ngOnInit(): void {
    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear;
    });
    var url = 'AccMstOpeningbalance/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name = this.Branchdtl_list[0].branch_gid;
    });
    this.GetIncomeExpenseSummary();
  }
  GetIncomeExpenseSummary(){
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.FinancialYear_List = result.GetFinancialYear_List;
      this.finyear = this.FinancialYear_List[0].finyear;
      let param = {
        finyear: this.FinancialYear_List[0].finyear
      }
    this.service.getparams('BSIEReports/GetIncomeSummary',param).pipe(
      catchError(error => {
        console.error('Error fetching Income Summary (Closing Balance):', error);
        return [];
      }),
      
      finalize(() => {
        this.NgxSpinnerService.hide();
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.IncomeSummary_list = result?.Income_list || [];
      this.totalCredit = this.calculateTotal(this.IncomeSummary_list);
      if (this.IncomeSummary_list.length <= 0) {
        this.processedIncome_List = [];
      }
      else {
        this.processedIncome_List = this.preprocessData(this.IncomeSummary_list);
      }
    });
    this.service.getparams('BSIEReports/GetExpenseSummary',param).pipe(
      catchError(error => {
        console.error('Error fetching Expense Summary (Closing Balance):', error);
        return [];
      }),
      
      finalize(() => {
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.ExpenseSummary_list = result?.Expense_list || [];
      this.totalDebit = this.calculateTotals(this.ExpenseSummary_list);
      if (this.ExpenseSummary_list.length <= 0) {
        this.processedExpense_List = [];
      }
      else {
        this.processedExpense_List = this.preprocessData(this.ExpenseSummary_list);
        console.log(this.processedExpense_List,'Expense_list')
      }
    });
  });
  }
 
  OnSearch(){
    let param = {
      finyear: this.finyear
    }
    this.NgxSpinnerService.show();
    this.service.getparams('BSIEReports/GetIncomeSummary',param).pipe(
      catchError(error => {
        console.error('Error fetching Income Summary (Closing Balance):', error);
        return [];
      }),
      
      finalize(() => {
        this.NgxSpinnerService.hide();
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.IncomeSummary_list = result?.Income_list || [];
      this.totalCredit = this.calculateTotal(this.IncomeSummary_list);
      if (this.IncomeSummary_list.length <= 0) {
        this.processedIncome_List = [];
      }
      else {
        this.processedIncome_List = this.preprocessData(this.IncomeSummary_list);
      }
    });
    this.service.getparams('BSIEReports/GetExpenseSummary',param).pipe(
      catchError(error => {
        console.error('Error fetching Expense Summary (Closing Balance):', error);
        return [];
      }),
      
      finalize(() => {
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.ExpenseSummary_list = result?.Expense_list || [];
      this.totalDebit = this.calculateTotals(this.ExpenseSummary_list);
      if (this.ExpenseSummary_list.length <= 0) {
        this.processedExpense_List = [];
      }
      else {
        this.processedExpense_List = this.preprocessData(this.ExpenseSummary_list);
      }
    });

  }
  preprocessData(data: any[]): any[] {
    this.processedData = [];
    let mainGroupSpan = 1;
    let subGroupSpan = 1;
    this.prevMainGroup = null;
    this.prevSubGroup = null;
    let mainGroupBalance = 0;
  
    data.forEach((item, index) => {
      // Parse Amount to ensure it's treated as a number
      const amount = parseFloat(item.transaction_amount.toString().replace(/,/g, ''));
  
      // Check MainGroup_name
      if (this.prevMainGroup !== item.MainGroup_name) {
        if (this.prevMainGroup !== null) {
          // Set the rowspan for the previous MainGroup_name
          for (let i = index - mainGroupSpan; i < index; i++) {
            this.processedData[i].mainGroupSpan = mainGroupSpan;
          }
          // Set balance for the first row of the main group
          this.processedData[index - mainGroupSpan].balance = mainGroupBalance.toLocaleString('en-US', { minimumFractionDigits: 1, maximumFractionDigits: 2 });
        }
        // Reset counters
        mainGroupSpan = 1;
        subGroupSpan = 1;
        mainGroupBalance = amount; // Start new balance with current amount
        this.prevMainGroup = item.MainGroup_name;
      } else {
        mainGroupSpan++;
        mainGroupBalance += amount; // Add to current main group balance
      }
  
      // Check subgroup_name
      if (this.prevSubGroup !== item.subgroup_name) {
        if (this.prevSubGroup !== null) {
          // Set the rowspan for the previous subgroup_name
          for (let i = index - subGroupSpan; i < index; i++) {
            this.processedData[i].subGroupSpan = subGroupSpan;
          }
        }
        subGroupSpan = 1;
        this.prevSubGroup = item.subgroup_name;
      } else {
        subGroupSpan++;
      }
  
      this.processedData.push({ ...item });
    });
  
    // Set the rowspan for the last MainGroup_name and subgroup_name
    for (let i = data.length - mainGroupSpan; i < data.length; i++) {
      this.processedData[i].mainGroupSpan = mainGroupSpan;
    }
    for (let i = data.length - subGroupSpan; i < data.length; i++) {
      this.processedData[i].subGroupSpan = subGroupSpan;
    }
    
    // Set balance for the last group
    this.processedData[data.length - mainGroupSpan].balance = mainGroupBalance.toLocaleString('en-US', { minimumFractionDigits: 1, maximumFractionDigits: 2 });
  
    return this.processedData;
  }
  
 showTab(tab: string) {
  this.currentTab = tab;
}
formatAmount(value: number): string {
  const formatter = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
  return formatter.format(value);
}
calculateDifference() {
  const credit = parseFloat(this.totalCredit.replace(/,/g, '') || '0');
  const debit = parseFloat(this.totalDebit.replace(/,/g, '') || '0');
  const diff = credit - debit;
  this.difference_value = this.formatAmount(diff);
}
calculateTotal(dataList: any[],): string {
  const total = dataList.reduce((sum: number, item: any) => {
    const amount = parseFloat(item?.transaction_amount?.replace(/,/g, '') || '0');
    return sum + amount;
  }, 0);
  return this.formatAmount(total);
}

calculateTotals(dataList: any[]): string {
  const total = dataList.reduce((sum: number, item: any) => {
    const amount = parseFloat(item?.transaction_amount?.replace(/,/g, '') || '0');
    return sum + amount;
  }, 0);
  return this.formatAmount(total);
}
}
