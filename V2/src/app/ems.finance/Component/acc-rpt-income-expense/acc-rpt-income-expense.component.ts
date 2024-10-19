import { Component, ViewChild } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ApexGrid, ApexMarkers, ApexStroke, ApexTitleSubtitle, ApexXAxis, ChartComponent } from "ng-apexcharts";
import { ApexNonAxisChartSeries, ApexResponsive, ApexChart, ApexFill, ApexDataLabels, ApexLegend, } from "ng-apexcharts";

interface GroupedIncomeSummary {
  account_name: string;
  MainGroup_name: string;
  subgroup_name: string;
  [key: string]: string | number | undefined;
}

interface GroupedExpenseSummary {
  account_name: string;
  MainGroup_name: string;
  subgroup_name: string;
  [key: string]: string | number | undefined;
}

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  fill: ApexFill;
  legend: ApexLegend;
  dataLabels: ApexDataLabels;
  xaxis: ApexXAxis;
  title: ApexTitleSubtitle;
  stroke: ApexStroke;
  markers: ApexMarkers;
  grid: ApexGrid;
};

@Component({
  selector: 'app-acc-rpt-income-expense',
  templateUrl: './acc-rpt-income-expense.component.html',
  styleUrls: ['./acc-rpt-income-expense.component.scss']
})

export class AccRptIncomeExpenseComponent {
  @ViewChild("chart") chart: ChartComponent | any;
  public chartOptions1: Partial<ChartOptions> | any;
  IncomeSummary_list: any[] = [];
  ExpenseSummary_list: any[] = [];
  processedData: any[] = [];
  prevSubGroup: any;
  prevMainGroup: any;
  leadstagechart: any;
  months_detail: any;
  income: any;
  expense: any;
  april: any;
  march: any;
  may: any;
  june: any;
  july: any;
  august: any;
  purchase_april: any;
  purchase_march: any;
  purchase_may: any;
  purchase_june: any;
  purchase_july: any;
  purchase_august: any;
  GetIncome_Expense_list: any[] = [];
  overallchartflag: boolean = false;
  incomeuniqueMonths: string[] = [];
  expenseuniqueMonths: string[] = [];
  groupedIncomeSummary: GroupedIncomeSummary[] = [];
  groupedExpenseSummary: GroupedExpenseSummary[] = [];
  totalincomeByMonth: any = {};
  totalexpenseByMonth: any = {};

  constructor(private service: SocketService,) { }

  ngOnInit(): void {
    var url = 'Income_Expense/GetIncomesummary';
    this.service.get(url).subscribe((result: any) => {
      this.IncomeSummary_list = this.preprocessData(result.IncomeSummary_list);
      this.incomeuniqueMonths = [...new Set(this.IncomeSummary_list.map(item => item.transaction_year))];
      this.groupedIncomeSummary = this.incomegroupBy(this.IncomeSummary_list, 'account_name');
      this.incomemonthlytotal();
    });

    var url2 = 'Income_Expense/GetExpenseSummary';
    this.service.get(url2).subscribe((result: any) => {
      this.ExpenseSummary_list = this.preprocessDatas(result.ExpenseSummary_list);
      this.expenseuniqueMonths = [...new Set(this.ExpenseSummary_list.map(item => item.transaction_year))];
      this.groupedExpenseSummary = this.expensegroupBy(this.ExpenseSummary_list, 'account_name');
      this.expensemonthlytotal();
    });
    this.Getprospectchart();
  }

  Getprospectchart() {
    var url = 'Income_Expense/IncomeExpenseGraph';
    this.service.get(url).subscribe((result: any) => {
      this.GetIncome_Expense_list = result.GetIncome_Expense_list;
      if (this.GetIncome_Expense_list.length > 0) {
        this.overallchartflag = true;
      }

      // Separate sales and purchase values
      const salesData = this.GetIncome_Expense_list
        .filter((entry: { source: string }) => entry.source === 'Sales')
        .map((entry: { transaction_date: any, transaction_amount: any }) => ({
          transaction_date: entry.transaction_date || 0,
          transaction_amount: entry.transaction_amount
        }));
      const purchaseData = this.GetIncome_Expense_list
        .filter((entry: { source: string }) => entry.source === 'Purchase')
        .map((entry: { transaction_date: any, transaction_amount: any }) => ({
          transaction_date: entry.transaction_date || 0,
          transaction_amount: entry.transaction_amount
        }));
      this.months_detail = Array.from(new Set(this.GetIncome_Expense_list.map((entry: { transaction_date: any }) => entry.transaction_date)));
     
      this.income = this.months_detail.map((month: string) => {
        const data = salesData.find(entry => entry.transaction_date === month);
        return data ? data.transaction_amount : 0; // If no sales data for this month, return 0
      });
      
      this.expense = this.months_detail.map((month: string) => {
        const data = purchaseData.find(entry => entry.transaction_date === month);
        return data ? data.transaction_amount : 0; // If no purchase data for this month, return 0
      });
      
      this.leadstagechart = {
        chart: {
          type: 'bar',
          height: 280,
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
          categories: this.months_detail,
          labels: {
            style: {
              fontSize: '12px',
            },
          },
        },
        yaxis: {
          labels: {
            // formatter: (value: number) => {
            //   return '₹' + value.toLocaleString(); // Format amount with commas and currency symbol
            // }

            formatter: function (value: any) {
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

          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Income',
            data: this.income,
            color: '#2D8625',
            // color: '#62cff4',
          },
          {
            name: 'Expense',
            data: this.expense,
            color: '#F00606',
            // color: '#2c67f2',
          },
        ],
        legend: {
          position: "bottom",
          offsetY: 1
        }
      };
    });
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
   preprocessDatas(data: any[]): any[] {
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

  toggleVisibility(item: any) {
    item.visible = !item.visible;
  }  

  getIncomeRowSpan(mainGroupName: string): number {
    return this.groupedIncomeSummary.filter(d => d.MainGroup_name === mainGroupName).length;
  }

  getIncomeRowSpanForSubgroup(subgroupName: string): number {
    return this.groupedIncomeSummary.filter(d => d.subgroup_name === subgroupName).length;
  }

  getIncomeAmountForMonth(accountName: string, month: string): number | null {
    const record = this.IncomeSummary_list.find(item =>
      item.account_name === accountName && item.transaction_year === month
    );
    return record ? record.transaction_amount : null;
  }

  incomegroupBy(arr: any[], key: string): GroupedIncomeSummary[] {
    const groupedObj = arr.reduce((acc, curr) => {
      (acc[curr[key]] = acc[curr[key]] || []).push(curr);
      return acc;
    }, {} as Record<string, any[]>);

    return Object.keys(groupedObj).map(account_name => {
      const records = groupedObj[account_name];
      const result: GroupedIncomeSummary = {
        account_name,
        MainGroup_name: records[0].MainGroup_name,
        subgroup_name: records[0].subgroup_name,
      };

      this.incomeuniqueMonths.forEach(month => {
        result[month] = records.find((record: { transaction_year: string; }) => record.transaction_year === month)?.transaction_amount || 0.00;
      });

      return result;
    });
  }

  incomemonthlytotal() {
    this.totalincomeByMonth = {};
    this.groupedIncomeSummary.forEach(data => {
      this.incomeuniqueMonths.forEach(month => {
        if (!this.totalincomeByMonth[month]) {
          this.totalincomeByMonth[month] = 0.00;
        }
        this.totalincomeByMonth[month] += parseFloat(String(data[month] ?? '0').replace(/,/g, '')) || 0;
      });
    });
  }

  getExpenseRowSpan(mainGroupName: string): number {
    return this.groupedExpenseSummary.filter(d => d.MainGroup_name === mainGroupName).length;
  }  

  getExpenseRowSpanForSubgroup(subgroupName: string): number {
    return this.groupedExpenseSummary.filter(d => d.subgroup_name === subgroupName).length;
  } 

  getExpenseAmountForMonth(accountName: string, month: string): number | null {
    const record = this.ExpenseSummary_list.find(item =>
      item.account_name === accountName && item.transaction_year === month
    );
    return record ? record.transaction_amount : null || 0.00 ;
  } 

  expensegroupBy(arr: any[], key: string): GroupedExpenseSummary[] {
    const groupedObj = arr.reduce((acc, curr) => {
      (acc[curr[key]] = acc[curr[key]] || []).push(curr);
      return acc;
    }, {} as Record<string, any[]>);

    return Object.keys(groupedObj).map(account_name => {
      const records = groupedObj[account_name];
      const result: GroupedExpenseSummary = {
        account_name,
        MainGroup_name: records[0].MainGroup_name,
        subgroup_name: records[0].subgroup_name,
      };

      this.expenseuniqueMonths.forEach(month => {
        result[month] = records.find((record: { transaction_year: string; }) => record.transaction_year === month)?.transaction_amount || 0.00;
      });

      return result;
    });
  }

  expensemonthlytotal() {
    this.totalexpenseByMonth = {};
    this.groupedExpenseSummary.forEach(data => {
      this.expenseuniqueMonths.forEach(month => {
        if (!this.totalexpenseByMonth[month]) {
          this.totalexpenseByMonth[month] = 0.00;
        }
        this.totalexpenseByMonth[month] += parseFloat(String(data[month] ?? '0').replace(/,/g, '')) || 0;
      });
    });
  }
}

















// console.log(this.IncomeSummary_list,'hello');



// if (this.IncomeSummary_list != null) {
//   this.march = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_march.replace(',', '')), 0);
//   this.april = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_april.replace(',', '')), 0);
//   this.may = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_may.replace(',', '')), 0);
//   this.june = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_june.replace(',', '')), 0);
//   this.july = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_july.replace(',', '')), 0);
//   this.august = result.IncomeSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_august.replace(',', '')), 0);
//   const formatter = new Intl.NumberFormat('en-US', {
//     minimumFractionDigits: 2,
//     maximumFractionDigits: 2
//   });
//   this.march = formatter.format(this.march);
//   this.april = formatter.format(this.april);
//   this.may = formatter.format(this.may);
//   this.june = formatter.format(this.june);
//   this.july = formatter.format(this.july);
//   this.august = formatter.format(this.august);
// }
// setTimeout(() => {
//   $('IncomeSummary_list').DataTable();
// }, 1);


// //this.debtorreport_list.reduce((total: any, data: any) => total + parseFloat(data.credit_amount.replace(',', '')), 0);
// this.purchase_march = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_march.replace(',', '')), 0);
// this.purchase_april = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_april.replace(',', '')), 0);
// this.purchase_may = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_may.replace(',', '')), 0);
// this.purchase_june = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_june.replace(',', '')), 0);
// this.purchase_july = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_july.replace(',', '')), 0);
// this.purchase_august = result.ExpenseSummary_list.reduce((total: any, data: any) => total + parseFloat(data.credit_august.replace(',', '')), 0);
// const formatter = new Intl.NumberFormat('en-US', {
//   minimumFractionDigits: 2,
//   maximumFractionDigits: 2
// });
// this.purchase_march = formatter.format(this.purchase_march);
// this.purchase_april = formatter.format(this.purchase_april);
// this.purchase_may = formatter.format(this.purchase_may);
// this.purchase_june = formatter.format(this.purchase_june);
// this.purchase_july = formatter.format(this.purchase_july);
// this.purchase_august = formatter.format(this.purchase_august);

// setTimeout(() => {
//   $('IncomeSummary_list').DataTable();
// }, 1);

// Getprospectchart() {
//   var url = 'Income_Expense/IncomeExpenseGraph';
//   this.service.get(url).subscribe((result: any) => {
//     this.GetIncome_Expense_list = result.GetIncome_Expense_list;
//     if (this.GetIncome_Expense_list.length > 0) {
//       this.overallchartflag = true;
//     }
//     this.months_detail= this.GetIncome_Expense_list
//     .map((entry: { transaction_date: any }) => entry.transaction_date || 0);
//     this.income= this.GetIncome_Expense_list
//     .filter((entry: { source: string }) => entry.source === 'Purchase')
//     .map((entry: { transaction_date: any }) => entry.transaction_date || 0);
//     this.expense= this.GetIncome_Expense_list
//     .filter((entry: { source: string }) => entry.source === 'Sales')
//     .map((entry: { transaction_date: any }) => entry.transaction_date || 0);
//     this.months_detail = this.GetIncome_Expense_list.map((entry: { transaction_date: any }) => entry.transaction_date),
//       this.income = this.GetIncome_Expense_list.map((entry: { transaction_amount : any }) => entry.transaction_amount),
//       this.expense = this.GetIncome_Expense_list.map((entry: { transaction_amount: any }) => entry.transaction_amount),
//     this.leadstagechart = {
//       chart: {
//         type: 'bar',
//         height: 280,
//         width: '100%',
//         background: 'White',
//         foreColor: '#0F0F0F',
//         fontFamily: 'inherit',
//         toolbar: {
//           show: false,
//         },
//       },
//       colors: ['#7FC7D9', '#FFD54F', '#66BB6A', '#EF5350'],
//       plotOptions: {
//         bar: {
//           horizontal: false,
//           columnWidth: '50%',
//           borderRadius: 0,
//         },
//       },
//       dataLabels: {
//         enabled: false,
//       },
//       xaxis: {
//         categories: this.months_detail,
//         labels: {
//           style: {

//             fontSize: '12px',
//           },
//         },
//       },
//       yaxis: {
//         title: {
//           style: {
//             fontWeight: 'bold',
//             fontSize: '14px',
//             color: '#7FC7D9',
//           },
//         },
//       },
//       series: [
//         {
//           name: 'Income',
//           data: this.income,
//           color: '#62cff4',
//         },
//         {
//           name: 'Expense',
//           data: this.expense,
//           color: '#2c67f2',
//         },
//       ],
//       legend: {
//         position: "bottom",
//         offsetY: 1
//       }
//     };
//   });
// }