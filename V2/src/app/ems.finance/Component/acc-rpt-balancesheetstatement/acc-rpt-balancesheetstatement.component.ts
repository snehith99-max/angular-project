import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, finalize } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-acc-rpt-balancesheetstatement',
  templateUrl: './acc-rpt-balancesheetstatement.component.html',
  styleUrls: ['./acc-rpt-balancesheetstatement.component.scss']
})
export class AccRptBalancesheetstatementComponent {
  FinancialYear_List: any[] = [];
  finyear: any;
  Branchdtl_list: any[] = [];
  branch_name: any;
  Liability_list: any[] = [];
  Asset_list: any[] = [];
  processedData: any[] = [];
  prevMainGroup: any;
  prevSubGroup: any;
  processedLiability_List: any[] = [];
  processedAsset_List: any[] = [];
  currentTab = 'Liability';
  totalCredit: any;
  totalDebit: any;
  difference_value: any;

  constructor(public service: SocketService, private router: Router, private NgxSpinnerService: NgxSpinnerService) {

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
    this.GetLiabilityAssetSummary();
  }
  GetLiabilityAssetSummary() {
    this.NgxSpinnerService.show();
    var url = 'AccMstOpeningbalance/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name = this.Branchdtl_list[0].branch_gid;

      var url = 'AccTrnBankbooksummary/GetFinancialYear'
      this.service.get(url).subscribe((result: any) => {
        this.FinancialYear_List = result.GetFinancialYear_List;
        this.finyear = this.FinancialYear_List[0].finyear;

        let params = {
          entity_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.FinancialYear_List[0].finyear
        }
        this.service.getparams('BSIEReports/GetLiabilitySummary', params).pipe(
          catchError(error => {
            console.error('Error fetching Liability balance (credit):', error);
            return [];
          }),
          
          finalize(() => {
            this.NgxSpinnerService.hide();
            this.calculateDifference();
          })
        ).subscribe((result: any) => {
          this.Liability_list = result?.Liability_list || [];
          this.totalCredit = this.calculateTotal(this.Liability_list);
          if (this.Liability_list.length <= 0) {
            this.processedLiability_List = [];
          }
          else {
            this.processedLiability_List = this.preprocessData(this.Liability_list);
          }
        });
        // Fetch opening balance data for debit amounts
        this.service.getparams('BSIEReports/GetAssetSummary', params).pipe(catchError(error => {
          console.error('Error fetching Asset balance (debit):', error);
          return [];
        }),
        finalize(() => {
          this.calculateDifference();
        })
        ).subscribe((result: any) => {
          this.Asset_list = result?.Asset_list || [];
          this.totalDebit = this.calculateTotals(this.Asset_list);
          if (this.Asset_list.length <= 0) {
            this.processedAsset_List = [];
          }
          else {
            this.processedAsset_List = this.preprocessData(this.Asset_list);
          }
        });
      });
    });

  }
  OnSearch() {
    let params = {
      entity_gid: this.branch_name,
      finyear: this.finyear
    }
    this.NgxSpinnerService.show();
    this.service.getparams('BSIEReports/GetLiabilitySummary', params).pipe(
      catchError(error => {
        console.error('Error fetching opening balance (credit):', error);
        return [];
      }),
      
      finalize(() => {
        this.NgxSpinnerService.hide();
        this.calculateDifference();
      })
    ).subscribe((result: any) => {
      this.Liability_list = result?.Liability_list || [];
      this.totalCredit = this.calculateTotal(this.Liability_list);
      if (this.Liability_list.length <= 0) {
        this.processedLiability_List = [];
      }
      else {
        this.processedLiability_List = this.preprocessData(this.Liability_list);
      }
    });
    // Fetch Asset Summary
    this.service.getparams('BSIEReports/GetAssetSummary', params).pipe(catchError(error => {
      console.error('Error fetching opening balance (debit):', error);
      return [];
    }),
    
    finalize(() => {
      this.calculateDifference();
    })
    ).subscribe((result: any) => {
      this.Asset_list = result?.Asset_list || [];
      this.totalDebit = this.calculateTotals(this.Asset_list);
      if (this.Asset_list.length <= 0) {
        this.processedAsset_List = [];
      }
      else {
        this.processedAsset_List = this.preprocessData(this.Asset_list);
      }
    });
  }
  LedgerView(params: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const account_gid = AES.encrypt(param, secretKey).toString();
    const finyear = AES.encrypt(this.finyear, secretKey).toString();
    const branch = AES.encrypt(this.branch_name, secretKey).toString();
    this.router.navigate(['/finance/AccRptBalancesheetreportview', account_gid, finyear, branch])
  }
  preprocessData(data: any[]): any[] {
    this.processedData = [];

    let mainGroupSpan = 1;
    let subGroupSpan = 1;
    this.prevMainGroup = null;
    this.prevSubGroup = null;

    data.forEach((item, index) => {
      // Check MainGroup_name
      if (this.prevMainGroup !== item.MainGroup_name) {
        if (this.prevMainGroup !== null) {
          // Set the rowspan for the previous MainGroup_name
          for (let i = index - mainGroupSpan; i < index; i++) {
            this.processedData[i].mainGroupSpan = mainGroupSpan;
          }
        }
        mainGroupSpan = 1;
        this.prevMainGroup = item.MainGroup_name;
      } else {
        mainGroupSpan++;
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
      const amount = parseFloat(item?.credit_amount?.replace(/,/g, '') || '0');
      return sum + amount;
    }, 0);
    return this.formatAmount(total);
  }

  calculateTotals(dataList: any[]): string {
    const total = dataList.reduce((sum: number, item: any) => {
      const amount = parseFloat(item?.debit_amount?.replace(/,/g, '') || '0');
      return sum + amount;
    }, 0);
    return this.formatAmount(total);
  }
}
