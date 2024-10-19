import { Component,ViewChild,Renderer2  } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  ApexAxisChartSeries,
  ApexChart,
  ChartComponent,
  ApexDataLabels,
  ApexPlotOptions,
  ApexYAxis,
  ApexLegend,
  ApexStroke,
  ApexXAxis,
  ApexFill,
  ApexTooltip
} from "ng-apexcharts";
export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  fill: ApexFill;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  legend: ApexLegend;
};
@Component({
  selector: 'app-tsk-mst-dashboard',
  templateUrl: './tsk-mst-dashboard.component.html',
  styleUrls: ['./tsk-mst-dashboard.component.scss']
})
export class TskMstDashboardComponent {
  @ViewChild("chart") chart: ChartComponent | any;
    member_list: any;
    selectedYear!: number;   
    years: number[] = []; 
    allMemberList: any[] = [];
    showList: boolean = false;
    currentHoveredMember: number | null = null;
    public chartOptions: Partial<ChartOptions>|any;
    public chartOptions1: Partial<ChartOptions>|any;

  Date: string='';
  Team_list: any;
  hoverPositionStyle = {};
  mandatory: any;
  non_mandatory: any;
  nice_to_count: any;
  show_stopper: any;
  chart_count: any;
  month_name: any;
  Module_name: any;
  graph_count: any;
  created_year: any;



  constructor(private NgxSpinnerService:NgxSpinnerService,private SocketService: SocketService){
    this.Date = new Date().toString();
this.populateYears()
// this.chartOptions = {
//   series: [
//     {
//       name: "Show Stopper",
//       data: [44, 55, 57, 56, 61, 58, 63, 60, 66]
//     },
//     {
//       name: "Critical Mandatory",
//       data: [76, 85, 101, 98, 87, 105, 91, 114, 94]
//     },
//     {
//       name: "Non-Critical Mandatory",
//       data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
//     },
//     {
//       name: "Nice To Have",
//       data: [35, 41, 36, 26, 45, 48, 52, 53, 41]
//     }
//   ],
//   chart: {
//     type: "bar",
//     height: 290
//   },
//   plotOptions: {
//     bar: {
//       horizontal: false,
//       columnWidth: "55%",
//       endingShape: "rounded"
//     }
//   },
//   dataLabels: {
//     enabled: false
//   },
//   stroke: {
//     show: true,
//     width: 2,
//     colors: ["transparent"]
//   },
//   xaxis: {
//     categories: [
//       "Feb",
//       "Mar",
//       "Apr",
//       "May",
//       "Jun",
//       "Jul",
//       "Aug",
//       "Sep",
//       "Oct"
//     ]
//   },
//   yaxis: {
//     title: {
//       text: "$ (thousands)"
//     }
//   },
//   fill: {
//     opacity: 1
//   },
//   tooltip: {
//     y: {
//       formatter: function(val: string) {
//         return "$ " + val + " thousands";
//       }
//     }
//   },
//   colors: ["#adabd3", "#61ad7791", "#f488777a","#bfcfa9"]
// };
    this.chartOptions1 = {
      series: [
        {
          name: "Live",
          data: [31, 40, 28, 51, 42, 109, 100]
        }
      ],
      chart: {
        height: 290,
        type: "area",
        toolbar: {
          show: false
        }
      },
      colors: ['#7579ad'],
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: "smooth"
      },
      xaxis: {
        categories: [
          "2024",
          "2023",
          "2022",
          "2021",
          "2020",
          "2019",
          "2018",
        ]
      },
      tooltip: {
        x: {
          format: "MM/yyyy"
        }
      }
    };
    
  }

  public generateData(baseval: number, count: number, yrange: { max: number; min: number; }) {
    var i = 0;
    var series = [];
    while (i < count) {
      var x = Math.floor(Math.random() * (750 - 1 + 1)) + 1;
      var y =
        Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;
      var z = Math.floor(Math.random() * (75 - 15 + 1)) + 15;

      series.push([x, y, z]);
      baseval += 86400000;
      i++;
    }
    return series;
  }
  linechart() {
    const params = {
      MonthYear: this.selectedYear
    };
    const api = 'TskTrnTaskManagement/linechartcount';
    this.SocketService.getparams(api, params).subscribe((result: any) => {
      this.chart_count = result.chart_count;
      if (this.chart_count?.length > 0) {
        const show_stopper_count = this.chart_count.map((item: any) => item.show_stopper_count);
        const critical_mandatory_count = this.chart_count.map((item: any) => item.critical_mandatory_count);
        const critical_non_mandatory_count = this.chart_count.map((item: any) => item.critical_non_mandatory_count);
        const nice_to_have_count = this.chart_count.map((item: any) => item.nice_to_have_count);
        this.month_name = this.chart_count.map((item: any) => item.month_name);
  
        this.chartOptions = {
          series: [
            {
              name: "Show Stopper",
              data: show_stopper_count
            },
            {
              name: "Critical Mandatory",
              data: critical_mandatory_count
            },
            {
              name: "Non-Critical Mandatory",
              data: critical_non_mandatory_count
            },
            {
              name: "Nice To Have",
              data: nice_to_have_count
            }
          ],
          chart: {
            type: "bar",
            height: 290
          },
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: "55%",
              endingShape: "rounded"
            }
          },
          dataLabels: {
            enabled: false
          },
          stroke: {
            show: true,
            width: 2,
            colors: ["transparent"]
          },
          xaxis: {
            categories: this.month_name
          },
          yaxis: {
            // title: {
            //   text: "$ (thousands)"
            // }
          },
          fill: {
            opacity: 1
          },
          tooltip: {
            y: {
              formatter: function(val: string) {
                return  val;
              }
            }
          },
          colors: ["#adabd3", "#61ad7791", "#f488777a","#bfcfa9"]
        };
      } else {
        this.chartOptions = {
          series: [],
          chart: {
            type: "bar",
            height: 290
          },
          plotOptions: {
            bar: {
              horizontal: false,
              columnWidth: "55%",
              endingShape: "rounded"
            }
          },
          dataLabels: {
            enabled: false
          },
          stroke: {
            show: true,
            width: 2,
            colors: ["transparent"]
          },
          xaxis: {
            categories: []
          },
          yaxis: {
            // title: {
            //   text: "$ (thousands)"
            // }
          },
          fill: {
            opacity: 1
          },
          tooltip: {
            y: {
              formatter: function(val: string) {
                return val;
              }
            }
          },
          colors: ["#adabd3", "#61ad7791", "#f488777a","#bfcfa9"]
        };
      }
    }, (error) => {
      console.error('Error fetching data:', error);
    });
  }
  graphchart(){
    const params = {
      module_gid: this.Module_name
    };
    const api = 'TskTrnTaskManagement/graphchartcount';
    this.SocketService.getparams(api, params).subscribe((result: any) => {
      this.graph_count = result.graph_count;
      if (this.graph_count?.length > 0) {
        const total_count = this.graph_count.map((item: any) => item.total_count);
        this.created_year = this.graph_count.map((item: any) => item.created_year);
        this.chartOptions1 = {
          series: [
            {
              name: "Live",
              data: total_count
            }
          ],
          chart: {
            height: 290,
            type: "area",
            toolbar: {
              show: false
            }
          },
          colors: ['#7579ad'],
          dataLabels: {
            enabled: false
          },
          stroke: {
            curve: "smooth"
          },
          xaxis: {
            categories: this.created_year
          },
          tooltip: {
            x: {
              format: "MM/yyyy"
            }
          }
        };
      } else {
        this.chartOptions1 = {
          series: [],
          chart: {
            height: 290,
            type: "area",
            toolbar: {
              show: false
            }
          },
          colors: ['#7579ad'],
          dataLabels: {
            enabled: false
          },
          stroke: {
            curve: "smooth"
          },
          xaxis: {
            categories: []
          },
          tooltip: {
            x: {
              format: "MM/yyyy"
            }
          }
        };
      }
    }, (error) => {
      console.error('Error fetching data:', error);
    });
  }
  ngOnInit(){
    var url = 'TskTrnTaskManagement/allmember';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.memberdropdown_list != null) {
        $('#employee_list').DataTable().destroy();
        this.member_list = result.memberdropdown_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#employee_list').DataTable();
        }, 1);
      }
      else {
        this.member_list = result.memberdropdown_list;
        setTimeout(() => {
          var table = $('#employee_list').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#employee_list').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
    });
    this.NgxSpinnerService.show()

    var url = 'TskMstCustomer/TeamSummary';
    this.SocketService.get(url).subscribe((result: any) => {
      this.Team_list = result.team_list;
        // this.Module_name = this.Team_list.find(team => team.team_name === 'Finance');

      this.NgxSpinnerService.hide()
    });
    this.graphchart()
    this.linechart()
    this.count()
  }
  private populateYears(): void {
    const currentYear = new Date().getFullYear();
    const startYear = 2000; // Change this as per your requirement

    for (let year = currentYear; year >= startYear; year--) {
        this.years.push(year);
    }

    // Set default selected year to the current year
    this.selectedYear = currentYear;
  }
  // showMemberDetails(assigned_member_gid: number) {
  //   this.currentHoveredMember = assigned_member_gid;
  //   const params = { assigned_member_gid: assigned_member_gid };
  //   const url = 'TskTrnTaskManagement/allmemebrview';

  //   this.SocketService.getparams(url, params).subscribe((result: any) => {
  //     if (result.allmember_list && result.allmember_list.length > 0) {
  //       this.allMemberList = result.allmember_list;
  //     }
  //     else {
  //       this.allMemberList = []; // Handle empty or undefined list
  //   }
  //   });
  // }
  showMemberDetails(event: MouseEvent, assigned_member_gid: number) {
    this.currentHoveredMember = assigned_member_gid;
    const params = { assigned_member_gid: assigned_member_gid };
    const url = 'TskTrnTaskManagement/allmemebrview';

    // Fetch member details
    this.SocketService.getparams(url, params).subscribe((result: any) => {
      if (result.allmember_list && result.allmember_list.length > 0) {
        this.allMemberList = result.allmember_list;
      } else {
        this.allMemberList = []; // Handle empty or undefined list
      }
    });

    // Calculate the position of the hover list based on the click event
    const target = event.target as HTMLElement;
    const rect = target.getBoundingClientRect();
    this.hoverPositionStyle = {
      position: 'fixed',
      top: `${rect.top + rect.height}px`, // Position below the clicked element
      left: `${rect.left}px`, // Align to the left of the clicked element
      zIndex: 1000, // Ensure it appears above other content
    };
  }
//   hideMemberDetails() {
//     this.currentHoveredMember = null;
//     this.allMemberList = [];
  
// }
hideMemberDetails() {
  this.currentHoveredMember = null;
  this.hoverPositionStyle = {}; // Reset the hover position style
}
count(){
  var url = 'TskTrnTaskManagement/Mandatorysummary';
this.SocketService.get(url).subscribe((result: any) => {
  this.mandatory=result.mandatory
});
var url = 'TskTrnTaskManagement/nonmandatorytsummary';
this.SocketService.get(url).subscribe((result: any) => {
  this.non_mandatory=result.non_mandatory
});
var url = 'TskTrnTaskManagement/nicetohavesummary';
this.SocketService.get(url).subscribe((result: any) => {
  this.nice_to_count=result.nice_to_count
});
var url = 'TskTrnTaskManagement/showstoppersummary';
this.SocketService.get(url).subscribe((result: any) => {
  this.show_stopper=result.show_stopper
});
}
}
