import { Component, OnInit, ViewChild } from "@angular/core";
import { ChartComponent } from "ng-apexcharts";
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { Subscription, Observable, timer } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { DatePipe } from '@angular/common';
import { map, share } from "rxjs/operators";
import { Pipe } from "@angular/core";
import { NgxSpinnerService } from 'ngx-spinner';
import { ApexNonAxisChartSeries, ApexResponsive, ApexChart } from "ng-apexcharts";
import { forkJoin } from 'rxjs';
import { SharedService } from "src/app/layout/services/shared.service";

export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

@Component({
  selector: 'app-acc-trn-financedashboard',
  templateUrl: './acc-trn-financedashboard.component.html',
  styleUrls: ['./acc-trn-financedashboard.component.scss']
})

export class AccTrnFinancedashboardComponent {
  response_data: any;
  taxchart_list: any = {};
  regionflag: boolean = false;
  region_count: any;
  region_name: any;
  crmregionchart: any = {};
  tax_amount: any;
  month_name: any;
  responsedata: any;
  bankbook_list: any[] = [];
  branchname_list: any;
  defaultbranch: any;
  GVPoptransaction_list: any;
  GVdebitDetailTable_list: any[] = [];
  GetExpenseExcel_list: any;
  GetBarChartIncomeexpene_list: any;
  months: any;
  income_amount: any;
  expense_amount: any;
  crmleadchart: any = {};
  GVcreditNeedDataSource_list: any[] = [];
  GVdebitNeedDataSource_list: any[] = [];
  totalItemsIncome: number = 0;
  overal_expense: any;
  overal_income: any;
  totalItems: number = 0;
  CashBook_list: any;
  FinanceDashboardCount_List: any;
  bank_count: any;
  creditcard_count: any;
  bankbook_count: any;
  journalentry_count: any;
  cashbook_count: any;
  tax_count: any;
  totaldebtor_count: any;
  totalcreditor_count: any;
  fundtransfer_count: any;
  fundpending_count: any;
  fundapproved_count: any;
  fundrejected_count: any;

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    public sharedservice: SharedService,
    private datePipe: DatePipe,
    private NgxSpinnerService: NgxSpinnerService) {
    const today = new Date();
  }

  ngOnInit(): void {
    this.Getcrmtilescount();
    this.AccTrnBankbooksummary();
    this.AccTrnCashbooksummary();

    const url = 'AccMstBankMaster/GetBranchName';
    this.service.get(url).subscribe((result: any) => {
      this.branchname_list = result.GetBranchName;
      if (this.branchname_list.length > 0) {
        this.defaultbranch = [this.branchname_list[0].branch_gid];
      }

      let selectedBranches = this.defaultbranch;
      let formattedBranches = selectedBranches.map((branch: any) => `'${branch}'`).join(',');
      let param = {
        branch: formattedBranches,
        from_date: null,
        to_date: null
      };

      const url1 = 'IncomeEpenditureReport/GVPopTransaction';
      const url2 = 'IncomeEpenditureReport/GVcreditNeedDataSource';
      const url3 = 'IncomeEpenditureReport/GVdebitNeedDataSource';
      const url4 = 'IncomeEpenditureReport/GetBarChartIncomeexpene';

      forkJoin([
        this.service.getparams(url1, param),
        this.service.getparams(url2, param),
        this.service.getparams(url3, param),
        this.service.getparams(url4, param)
      ]).subscribe((results: any[]) => {
        // Process results of each request     

        this.responsedata = results[0];
        this.GVPoptransaction_list = this.responsedata.GVPoptransaction_list;
        this.responsedata = results[1];
        this.GVcreditNeedDataSource_list = this.responsedata.GVcreditNeedDataSource_list;
        if (this.GVcreditNeedDataSource_list != null) {
          this.totalItemsIncome = this.GVcreditNeedDataSource_list.length;
          const totalCredit = this.GVcreditNeedDataSource_list.reduce((sum, item) => {
            return sum + this.parseCurrency(item.debit_amount);
          }, 0);
          this.overal_income = this.formatCurrency(totalCredit);
        }

        this.responsedata = results[2];
        this.GVdebitNeedDataSource_list = this.responsedata.GVdebitNeedDataSource_list;
        if (this.GVdebitNeedDataSource_list != null) {
          this.totalItems = this.GVdebitNeedDataSource_list.length;
          this.totalItemsIncome = this.GVdebitNeedDataSource_list.length;
          const totalCredit = this.GVdebitNeedDataSource_list.reduce((sum, item) => {
            return sum + this.parseCurrency(item.debit_amount);
          }, 0);
          this.overal_expense = this.formatCurrency(totalCredit);
        }
        this.responsedata = results[3];
        this.GetBarChartIncomeexpene_list = this.responsedata.GetBarChartIncomeexpene_list;

        if (this.GetBarChartIncomeexpene_list != null && this.GetBarChartIncomeexpene_list.length > 0) {
          this.months = this.GetBarChartIncomeexpene_list.map((entry: { month_name: any }) => entry.month_name);
          this.expense_amount = this.GetBarChartIncomeexpene_list.map((entry: { expense_amount: any }) => entry.expense_amount);
          this.income_amount = this.GetBarChartIncomeexpene_list.map((entry: { income_amount: any }) => entry.income_amount);

          // this.crmleadchart = {
          //   chart: {
          //     type: 'bar',
          //     height: 300,
          //     width: '100%',
          //     foreColor: '#0F0F0F',
          //     fontFamily: 'inherit',
          //     toolbar: {
          //       show: false,
          //     },
          //   },
          //   colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
          //   plotOptions: {
          //     bar: {
          //       horizontal: false,
          //       columnWidth: '50%',
          //       borderRadius: 0,
          //     },
          //   },
          //   dataLabels: {
          //     enabled: false,
          //   },
          //   xaxis: {
          //     categories: this.months,
          //     labels: {
          //       style: {
          //         fontSize: '12px',
          //       },
          //     },
          //   },
          //   yaxis: {
          //     title: {
          //       style: {
          //         fontWeight: 'bold',
          //         fontSize: '14px',
          //         color: '#7FC7D9',
          //       },
          //     },
          //   },
          //   series: [
          //     {
          //       name: 'Income',
          //       data: this.income_amount,
          //       color: '#2d8625',
          //     },
          //     {
          //       name: 'Expense',
          //       data: this.expense_amount,
          //       color: '#f00606',
          //     },
          //   ],
          //   legend: {
          //     position: "bottom",
          //     offsetY: 5
          //   }
          // };

          this.crmleadchart = {
            chart: {
              type: 'bar',
              height: 300,
              width: '100%',
              foreColor: '#0F0F0F',
              fontFamily: 'inherit',
              toolbar: {
                show: false,
              },
            },
            colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
            plotOptions: {
              bar: {
                horizontal: false,
                columnWidth: '50%',
                borderRadius: 0,
              },
            },
            dataLabels: {
              enabled: false,
            },
            xaxis: {
              categories: this.months,
              labels: {
                style: {
                  fontSize: '12px',
                },
              },
            },
            yaxis: {
              title: {
                style: {
                  fontWeight: 'bold',
                  fontSize: '14px',
                  color: '#7FC7D9',
                },
              },
              labels: {
                formatter: function(value: any) {
                  if (value >= 10000000) {
                    return (value / 10000000).toFixed(2) + ' Cr';
                  } else if (value >= 100000) {
                    return (value / 100000).toFixed(2) + ' L';
                  } else if (value >= 1000) {
                    return (value / 1000).toFixed(2) + ' K';
                  } else {
                    return value;
                  }
                },
              },
            },
            series: [
              {
                name: 'Income',
                data: this.income_amount,
                color: '#2d8625',
              },
              {
                name: 'Expense',
                data: this.expense_amount,
                color: '#f00606',
              },
            ],
            legend: {
              position: "bottom",
              offsetY: 5
            }
          };
        }

        this.NgxSpinnerService.hide(); // Hide spinner after all operations are completed
      }, (error) => {
        console.error('Error fetching data', error);
        this.NgxSpinnerService.hide(); // Hide spinner in case of error
      });
    });

    // Count Details
    var api = 'AccTrnBankbooksummary/GetFinanceDashboardCount';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.FinanceDashboardCount_List = this.response_data.getFinanceDashboardCount_List;
      this.bank_count = Number(this.FinanceDashboardCount_List[0].bank_count);
      this.creditcard_count = Number(this.FinanceDashboardCount_List[0].creditcard_count);
      this.bankbook_count = Number(this.FinanceDashboardCount_List[0].bankbook_count);
      this.cashbook_count = Number(this.FinanceDashboardCount_List[0].cashbook_count);
      this.journalentry_count = Number(this.FinanceDashboardCount_List[0].journalentry_count);
      this.tax_count = Number(this.FinanceDashboardCount_List[0].tax_count);
      this.totaldebtor_count = Number(this.FinanceDashboardCount_List[0].totaldebtor_count);
      this.totalcreditor_count = Number(this.FinanceDashboardCount_List[0].totalcreditor_count);
      this.fundtransfer_count = Number(this.FinanceDashboardCount_List[0].fundtransfer_count);
      this.fundpending_count = Number(this.FinanceDashboardCount_List[0].fundpending_count);
      this.fundapproved_count = Number(this.FinanceDashboardCount_List[0].fundapproved_count);
      this.fundrejected_count = Number(this.FinanceDashboardCount_List[0].fundrejected_count);
    });

  }

  AccTrnCashbooksummary() {

    debugger
    this.NgxSpinnerService.show();
    var api = 'AccTrnCashBookSummary/GetAccTrnCashbooksummary';
    this.service.get(api).subscribe((result: any) => {
      $('#CashBook_list').DataTable().destroy();
      this.response_data = result;
      this.CashBook_list = this.response_data.CashBook_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#CashBook_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });

  }

  parseCurrency(value: string): number {
    // Remove any non-numeric characters except for the decimal point
    return parseFloat(value.replace(/[^0-9.-]+/g, ""));
  }

  formatCurrency(amount: number): string {
    // Replace commas with an empty string to remove them
    const amountWithoutCommas = amount.toString().replace(/,/g, '');
    // Use Intl.NumberFormat to format the number
    const formatter = new Intl.NumberFormat('en-IN', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
    // Format the number without commas
    const formattedNumber = formatter.format(parseFloat(amountWithoutCommas));
    // Split the formatted number into integer and fraction parts
    const parts = formattedNumber.split('.');
    // Add commas back to the integer part
    const integerPartWithCommas = parts[0].replace(/\B(?=(\d{2})+(?!\d))/g, ',');
    // Join the integer part with the fraction part and return
    return integerPartWithCommas + '.' + parts[1];
  }

  Getcrmtilescount() {
    var url = 'GLCode/GetTaxChartDetails';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.taxchart_list = this.response_data.taxchart_list;
      console.log(this.taxchart_list, 'this.taxchart_list');
      if (this.taxchart_list.length > 0) {
        this.regionflag = true;
      }
      this.tax_amount = this.taxchart_list.map((entry: { tax_amount: any }) => entry.tax_amount),
        this.month_name = this.taxchart_list.map((entry: { month_name: any }) => entry.month_name),
        this.crmregionchart = {
          chart: {
            type: 'bar',
            height: 355,
            width: '100%',
            background: 'White',
            foreColor: '#0F0F0F',
            fontFamily: 'inherit',
            toolbar: {
              show: false,
            },
          },
          colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: '50%',
              borderRadius: 0,
            },
          },
          dataLabels: {
            enabled: false,
          },
          xaxis: {
            categories: this.month_name,
            labels: {
              style: {

                fontSize: '12px',
              },
            },
          },
          yaxis: {
            title: {
              style: {
                fontWeight: 'bold',
                fontSize: '14px',
                color: '#7FC7D9',
              },
            },
            labels: {
              formatter: function(value: any) {
                if (value >= 10000000) {
                  return (value / 10000000).toFixed(2) + ' Cr';
                } else if (value >= 100000) {
                  return (value / 100000).toFixed(2) + ' L';
                } else if (value >= 1000) {
                  return (value / 1000).toFixed(2) + ' K';
                } else {
                  return value;
                }
              },
            },
          },
          series: [
            {
              name: 'Tax Amount',
              data: this.tax_amount,
              color: '#2d8625',
            },

          ],
          // fill: {
          //   type: "gradient",
          //   gradient: {
          //     shadeIntensity: 1,
          //     opacityFrom: 0.7,
          //     opacityTo: 0.9,
          //     stops: [0, 100]
          //   }
          // }

        }
    });
  }

  AccTrnBankbooksummary() {
    this.NgxSpinnerService.show();
    var url = 'AccTrnBankbooksummary/GetBankBookSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#bankbook_list').DataTable().destroy();
      this.responsedata = result;
      this.bankbook_list = this.responsedata.Getbankbook_list;
      this.NgxSpinnerService.hide();
      // setTimeout(() => {
      //   $('#bankbook_list').DataTable(
      //     {
      //       // code by snehith for customized pagination
      //       "pageLength": 50, // Number of rows to display per page
      //       "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
      //     }
      //   );
      // }, 1);
    });
  }

}
