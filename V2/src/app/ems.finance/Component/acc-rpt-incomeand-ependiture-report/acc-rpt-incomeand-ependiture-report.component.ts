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
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { ChartComponent } from "ng-apexcharts";
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
import { forkJoin } from 'rxjs';
interface CustomOptions extends Options {
  dpi?: number;
}

@Component({
  selector: 'app-acc-rpt-incomeand-ependiture-report',
  templateUrl: './acc-rpt-incomeand-ependiture-report.component.html',
  styleUrls: ['./acc-rpt-incomeand-ependiture-report.component.scss']
})
export class AccRptIncomeandEpenditureReportComponent {
  months: any;
  income_amount: any;
  expense_amount: any;
  crmleadchart: any = {};
  GVcreditNeedDataSource_list: any[] = [];
  responsedata: any;
  GVcreditDetailTable_list: any[] = [];
  GetIncomeExcel_list: any;
  GVdebitNeedDataSource_list: any[] = [];
  overal_expense: any;
  overal_income: any;
  GVPoptransaction_list: any;
  GVdebitDetailTable_list: any[] = [];
  GetExpenseExcel_list: any;
  currentPage: number = 1; // Current page number
  itemsPerPage: number = 10; // Number of items to display per page
  totalItems: number = 0;
  itemsPerPageOptions: number[] = [10, 25, 50, 100];

  currentPageIncome: number = 1;
  itemsPerPageIncome: number = 10;
  totalItemsIncome: number = 0;
  itemsPerPageOptionsIncome: number[] = [10, 25, 50, 100];
  GetBarChartIncomeexpene_list: any;
  reactiveform!: FormGroup;
  branchname_list: any;
  deafultbranch: any;
  defaultbranch: any;
  constructor(private formBuilder: FormBuilder, public service: SocketService, private el: ElementRef, private route: Router, private NgxSpinnerService: NgxSpinnerService, private sanitizer: DomSanitizer, private renderer: Renderer2, private router: Router, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      from_date: new FormControl(this.getPreviousSixMonthsDate(), Validators.required),
      to_date: new FormControl(this.getCurrentDate(), Validators.required),
      frombranch: new FormControl('', [Validators.required,]),
    })
  }
  get to_date() {
    return this.reactiveform.get('to_date')!;
  }
  get from_date() {
    return this.reactiveform.get('from_date')!;
  }
  get frombranch() {
    return this.reactiveform.get('frombranch')!;
  }

  ngOnInit(): void {
    this.NgxSpinnerService.show(); // Show spinner at the beginning

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

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
        from_date: this.reactiveform.value.from_date,
        to_date: this.reactiveform.value.to_date
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

        setTimeout(() => {
          $('#GVdebitNeedDataSource_list').DataTable({
            "pageLength": 10,
            "lengthMenu": [10, 25, 50, 100],
          });
        }, 1);

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
              position: 'bottom',
              offsetY: 5,
            },
          };
        }

        this.NgxSpinnerService.hide(); // Hide spinner after all operations are completed
      }, (error) => {
        console.error('Error fetching data', error);
        this.NgxSpinnerService.hide(); // Hide spinner in case of error
      });
    });
  }

  getCurrentDate(): string {
    const today = new Date();
    return this.formatDate(today);
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

  getPreviousSixMonthsDate(): string {
    const today = new Date();
    today.setMonth(today.getMonth() - 6);
    today.setDate(1); // Ensure the day is the 1st
    return this.formatDate(today);
  }

  formatDate(date: Date): string {
    const dd = String(date.getDate()).padStart(2, '0');
    const mm = String(date.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = date.getFullYear();
    return `${dd}-${mm}-${yyyy}`;
  }


  onsubmit(): void {
    this.NgxSpinnerService.show();

    if (this.reactiveform.valid) {
      let selectedBranches = this.reactiveform.value.frombranch;
      let formattedBranches = selectedBranches.map((branch: any) => `'${branch}'`).join(',');

      let param = {
        branch: formattedBranches,
        from_date: this.reactiveform.value.from_date,
        to_date: this.reactiveform.value.to_date
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
          const totalCredit = this.GVdebitNeedDataSource_list.reduce((sum, item) => {
            return sum + this.parseCurrency(item.debit_amount);
          }, 0);
          this.overal_expense = this.formatCurrency(totalCredit);
        }

        setTimeout(() => {
          $('#GVdebitNeedDataSource_list').DataTable({
            "pageLength": 10,
            "lengthMenu": [10, 25, 50, 100],
          });
        }, 1);

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
              position: 'bottom',
              offsetY: 5,
            },
          };
          
        }

        this.NgxSpinnerService.hide();
      }, (error) => {
        console.error('Error fetching data', error);
        this.NgxSpinnerService.hide();
      });
    } else {
      this.NgxSpinnerService.hide();
    }
  }
  // Income Pagination Methods
  get pagedItemsIncome(): any[] {
    if (!this.GVcreditNeedDataSource_list) {
      return [];
    }
    const startIndexIncome = (this.currentPageIncome - 1) * this.itemsPerPageIncome;
    const endIndexIncome = startIndexIncome + this.itemsPerPageIncome;
    return this.GVcreditNeedDataSource_list.slice(startIndexIncome, endIndexIncome);
  }

  onItemsPerPageChangeIncome(): void {
    this.currentPageIncome = 1;
  }

  pageChangedIncome(event: any): void {
    this.currentPageIncome = event.page;
  }

  get startIndexIncome(): number {
    return (this.currentPageIncome - 1) * this.itemsPerPageIncome;
  }

  get endIndexIncome(): number {
    const end = this.startIndexIncome + this.itemsPerPageIncome;
    return end > this.totalItemsIncome ? this.totalItemsIncome : end;
  }

  get pagedItems(): any[] {
    if (!this.GVdebitNeedDataSource_list) {
      return [];
    }
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.GVdebitNeedDataSource_list.slice(startIndex, endIndex);
  }
  onItemsPerPageChange(): void {
    this.currentPage = 1; // Reset to the first page when items per page changes
  }
  pageChanged(event: any): void {
    this.currentPage = event.page;
  }
  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }

  get endIndex(): number {
    const end = this.startIndex + this.itemsPerPage;
    return end > this.totalItems ? this.totalItems : end;
  }
  parseCurrency(value: string): number {
    // Remove any non-numeric characters except for the decimal point
    return parseFloat(value.replace(/[^0-9.-]+/g, ""));
  }

  // transactions: any[] = [
  //   { year: 2022, month: 'November', income_amount: 1661663.00, expense_amount: 1000000.00, net_amount: 661663.00 },
  //   { year: 2022, month: 'December', income_amount: 1800000.00, expense_amount: 1200000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'January', income_amount: 1500000.00, expense_amount: 900000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'February', income_amount: 2000000.00, expense_amount: 1300000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'March', income_amount: 2200000.00, expense_amount: 1500000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'April', income_amount: 1800000.00, expense_amount: 1000000.00, net_amount: 800000.00 },

  //   { year: 2022, month: 'November', income_amount: 1661663.00, expense_amount: 1000000.00, net_amount: 661663.00 },
  //   { year: 2022, month: 'December', income_amount: 1800000.00, expense_amount: 1200000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'January', income_amount: 1500000.00, expense_amount: 900000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'February', income_amount: 2000000.00, expense_amount: 1300000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'March', income_amount: 2200000.00, expense_amount: 1500000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'April', income_amount: 1800000.00, expense_amount: 1000000.00, net_amount: 800000.00 },
  //   { year: 2022, month: 'November', income_amount: 1661663.00, expense_amount: 1000000.00, net_amount: 661663.00 },
  //   { year: 2022, month: 'December', income_amount: 1800000.00, expense_amount: 1200000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'January', income_amount: 1500000.00, expense_amount: 900000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'February', income_amount: 2000000.00, expense_amount: 1300000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'March', income_amount: 2200000.00, expense_amount: 1500000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'April', income_amount: 1800000.00, expense_amount: 1000000.00, net_amount: 800000.00 },

  //   { year: 2022, month: 'November', income_amount: 1661663.00, expense_amount: 1000000.00, net_amount: 661663.00 },
  //   { year: 2022, month: 'December', income_amount: 1800000.00, expense_amount: 1200000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'January', income_amount: 1500000.00, expense_amount: 900000.00, net_amount: 600000.00 },
  //   { year: 2023, month: 'February', income_amount: 2000000.00, expense_amount: 1300000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'March', income_amount: 2200000.00, expense_amount: 1500000.00, net_amount: 700000.00 },
  //   { year: 2023, month: 'April', income_amount: 1800000.00, expense_amount: 1000000.00, net_amount: 800000.00 },
  //   { year: 2023, month: 'May', income_amount: 1900000.00, expense_amount: 1100000.00, net_amount: 800000.00 },
  //   { year: 2023, month: 'June', income_amount: 2100000.00, expense_amount: 1300000.00, net_amount: 800000.00 },
  //   { year: 2023, month: 'July', income_amount: 2200000.00, expense_amount: 1400000.00, net_amount: 800000.00 },
  //   { year: 2023, month: 'August', income_amount: 2300000.00, expense_amount: 1500000.00, net_amount: 800000.00 }
  // ];

  toggleExpand(data: any): void {
    this.GVcreditNeedDataSource_list.forEach(row => {
      if (row !== data) {
        row.isExpand = false; // Collapse all other rows
      }
    });
    data.isExpand = !data.isExpand; // Toggle the clicked 
    // console.log(data.month)
    if (data.isExpand) {
      let selectedBranches = this.reactiveform.value.frombranch;
      let formattedBranches = selectedBranches.map((branch: any) => `'${branch}'`).join(',');
      let param = {
        branch: formattedBranches,
        from_date: this.reactiveform.value.from_date,
        to_date: this.reactiveform.value.to_date,
        month: data.month,
        year: data.year,
      }

      var url2 = 'IncomeEpenditureReport/GVcreditDetailTable'
      this.service.getparams(url2, param).subscribe((result: any) => {
        $('#GVcreditDetailTable_list').DataTable().destroy();
        this.responsedata = result;
        this.GVcreditDetailTable_list = this.responsedata.GVcreditDetailTable_list;
        //console.log(this.GVcreditDetailTable_list)
      });
    }
  }
  // toggleExpand1(data: any): void {
  //   this.GVdebitNeedDataSource_list.forEach(row => {
  //     if (row !== data) {
  //       row.isExpand1 = false; // Collapse all other rows
  //     }
  //   });
  //   data.isExpand1 = !data.isExpand1; // Toggle the clicked 
  //   console.log(data.month)

  //   let param = {
  //     branch: 'HBHM2312121',
  //     month: data.month,
  //     year: data.year,
  //   }

  //   var url2 = 'IncomeEpenditureReport/GVdebitDetailTable'
  //   this.service.getparams(url2, param).subscribe((result: any) => {
  //     $('#GVdebitDetailTable_list').DataTable().destroy();
  //     this.responsedata = result;
  //     this.GVdebitDetailTable_list = this.responsedata.GVdebitDetailTable_list;
  //     //console.log(this.GVcreditDetailTable_list)
  //   });
  // }
  toggleExpand1(data: any): void {
    // Collapse all other rows
    this.pagedItems.forEach(row => {
      if (row !== data) {
        row.isExpand1 = false;
      }
    });

    // Toggle the clicked row
    data.isExpand1 = !data.isExpand1;

    // If the row is now expanded, fetch the new data
    if (data.isExpand1) {
      // console.log(data.month);

      let selectedBranches = this.reactiveform.value.frombranch;
      let formattedBranches = selectedBranches.map((branch: any) => `'${branch}'`).join(',');
      let param = {
        branch: formattedBranches,
        from_date: this.reactiveform.value.from_date,
        to_date: this.reactiveform.value.to_date,
        month: data.month,
        year: data.year,
      }

      var url2 = 'IncomeEpenditureReport/GVdebitDetailTable';
      this.service.getparams(url2, param).subscribe((result: any) => {
        $('#GVdebitDetailTable_list').DataTable().destroy();
        this.responsedata = result;
        this.GVdebitDetailTable_list = this.responsedata.GVdebitDetailTable_list;
        //this.totalItemsIncome = this.GVdebitDetailTable_list.length;
      });
    }
  }

}
