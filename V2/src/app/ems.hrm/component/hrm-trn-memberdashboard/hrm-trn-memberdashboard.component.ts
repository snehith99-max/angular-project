import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface MonthlyChartDataItem {
  monthname: string;
  countPresentm: string;
  countAbsentm: string;
}
@Component({
  selector: 'app-hrm-trn-memberdashboard',
  templateUrl: './hrm-trn-memberdashboard.component.html',
  styleUrls: ['./hrm-trn-memberdashboard.component.scss']
})
export class HrmTrnMemberdashboardComponent {
  totalDuration:any;

  chartOptions1: any = {};
  chartOptions2: any = {};
  chartOptions3: any = {};
  defaultChartOptions: any = {};
  response_data: any;
  isTableVisible: boolean = false;

  // Declare and initialize variables
  totalDays: number = 0;
  countPresent: number = 0;
  countAbsent: number = 0;
  countLeave: number = 0;
  countholiday: number = 0;
  countWeekOff: number = 0;

  DashboardCount_List: any[] = [];
  DashboardQuotationAmt_List: any;
  noquotation: any;
  year: any;
  event_list: any[] = [];
  noquotation_status: any;
  show :boolean= false;
  monthlyflag :boolean= false;
  Date: string;
  onduty_details: any;
  responsedata: any;
  loginsummary_list: any;
  salarydetails_list: any;
  additionsalarydetail_list: any;
  deductionsalarydetail_list: any;
  othersalarydetail_list: any;
  logoutsummary_list: any;
  ondutysummary_list: any;
  compoffsummary_list: any;
  permissionsummary_list: any;
  punchlogin_time: any;
  login_time_audit: any;
  logout_time_audit: any;
  applyLoginReqForm!: FormGroup;
  applyLogoutReqForm!: FormGroup;
  applyODForm!: FormGroup;
  applycompoffForm!: FormGroup;
  applypermissionForm!: FormGroup;
  emptyFlag: boolean = false;
  noleadstatus: any;
  login_time: any;
  update_flag: any;
  logout_time: any;
  showInputs: boolean = false;

  attendanceMarked: boolean = false;
  punch_flag: any;
  parametervalue: any;
  labels_value: any;
  series_Value: any;
  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService) {
    this.Date = new Date().toString();

    this.applyLoginReqForm = new FormGroup({
      loginreq_date: new FormControl('', [Validators.required]),
      logintime: new FormControl('', [Validators.required]),
      loginreq_reason: new FormControl('', [Validators.required]),
    })

    this.applyLogoutReqForm = new FormGroup({
      logoutattendence_date: new FormControl('', [Validators.required]),
      logouttime: new FormControl('', [Validators.required]),
      logouttime_reason: new FormControl('', [Validators.required]),
    })

    this.applyODForm = new FormGroup({
      od_date: new FormControl('', [Validators.required]),
      od_fromhr: new FormControl('', [Validators.required]),
      od_tohr: new FormControl('', [Validators.required]),
      Onduty_period: new FormControl(''),
      od_session: new FormControl(''),
      od_reason: new FormControl('', [Validators.required]),
    })

    this.applycompoffForm = new FormGroup({
      actualworking_date: new FormControl('', [Validators.required]),
      compensatoryoff_applydate: new FormControl('', [Validators.required]),
      compoff_reason: new FormControl('', [Validators.required]),

    })

    this.applypermissionForm = new FormGroup({
      permission_date: new FormControl('', [Validators.required]),
      permission_fromhr: new FormControl('', [Validators.required]),
      permission_tohr: new FormControl('', [Validators.required]),
      permission_reason: new FormControl('', [Validators.required]),
    })

    HrmTrnMemberdashboardComponent.constructor(); {
      setInterval(() => {
        this.Date = new Date().toString();
      }, 1000);
    }    
  }

  togglesalarydetailtablevisiiblity() {
    this.isTableVisible = !this.isTableVisible;
  }

  get loginreq_date() {
    return this.applyLoginReqForm.get('loginreq_date')!;
  }

  get logintime() {
    return this.applyLoginReqForm.get('logintime')!;
  }

  get loginreq_reason() {
    return this.applyLoginReqForm.get('loginreq_reason')!;
  }

  get logoutattendence_date() {
    return this.applyLoginReqForm.get('logoutattendence_date')!;
  }

  get logouttime() {
    return this.applyLoginReqForm.get('logouttime')!;
  }

  get logouttime_reason() {
    return this.applyLoginReqForm.get('logouttime_reason')!;
  }

  get od_date() {
    return this.applyLoginReqForm.get('od_date')!;
  }

  get od_fromhr() {
    return this.applyLoginReqForm.get('od_fromhr')!;
  }

  get od_tohr() {
    return this.applyLoginReqForm.get('od_tohr')!;
  }

  get od_reason() {
    return this.applyLoginReqForm.get('od_reason')!;
  }

  get actualworking_date() {
    return this.applycompoffForm.get('actualworking_date')!;
  }

  get compensatoryoff_applydate() {
    return this.applycompoffForm.get('compensatoryoff_applydate')!;
  }

  get compoff_reason() {
    return this.applycompoffForm.get('compoff_reason')!;
  }

  get permission_date() {
    return this.applypermissionForm.get('permission_date')!;
  }

  get permission_fromhr() {
    return this.applypermissionForm.get('permission_fromhr')!;
  }

  get permission_tohr() {
    return this.applypermissionForm.get('permission_tohr')!;
  }

  get permission_reason() {
    return this.applypermissionForm.get('permission_reason')!;
  }
  showTextBox1(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInputs = target.value === 'Half' ;
  }

  ngOnInit(): void {
    this.defaultChartOptions = {
      series: [0],
      chart: {
        type: 'donut',
        width: 360,
      },
      plotOptions: {
        donut: {
          customScale: 0.8,
          dataLabels: {
            offset: 0, 
            minAngleToShowLabel: 10,
            enabled: true,
          },
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 220
            },
            legend: {
              position: "top",
            },
          },
        },
      ],
      // legend: {
      //   show: false,
      // },
      // labels: ["No Records are found"],
    };
    
    this.salarydetailsummary();
    // this.chartOptions2 = getbarChartOptions(350);

    setInterval(() => {
      this.Date = new Date().toString();
    }, 1000);

    var api = 'hrmTrnDashboard/monthlyAttendence';
    this.service.getdtl(api).subscribe((result: any) => {
      this.responsedata = result;
      this.DashboardCount_List = this.responsedata.last6MonthAttendence_list;

      if (this.DashboardCount_List.length>0) {
        this.show = true;
      }

      this.totalDays = Number(this.DashboardCount_List[0].totalDays);
      this.countPresent = Number(this.DashboardCount_List[0].countPresent);
      this.countAbsent = Number(this.DashboardCount_List[0].countAbsent);
      this.countLeave = Number(this.DashboardCount_List[0].countLeave);
      this.countholiday = Number(this.DashboardCount_List[0].countholiday);
      this.countWeekOff = Number(this.DashboardCount_List[0].countWeekOff);

           this.chartOptions1 = {

        series: [this.countPresent, this.countAbsent, this.countLeave, this.countholiday, this.countWeekOff],
        labels: ["Present", "Absent", "Leave", "Holiday", "WeekOff"],

        colors: ['#FFD54F', '#EF5350', '#66BB6A', '#7FC7D9', '#eba7ff'], 

        // Use a set of colors for better combinations

        chart: {
          width: 430,
          type: "donut"
        },
        responsive: [
          {
            breakpoint: 480,
            options: {
              chart: {
                width: 150
              },
              legend: {
                position: "bottom"
              }
            }
          }
        ],
        fill: {
          type: "gradient"
        },
      };  
   
    this.monthlyattendance()
    this.punchinoutcall()

    this.loginsummary()
    this.logoutsummary();
    this.ondutysummary();
    this.compoffsummary();
    this.permissionsummary();
    });
  }

  monthlyattendance(){
    const monthlyChartData: MonthlyChartDataItem[] = this.responsedata.last6MonthAttendence_list;
      
    if (this.DashboardCount_List.length>0) {
      this.monthlyflag = true;
    }
    const formattedMonthlyChartData = {
      present: monthlyChartData.map(item => Number(item.countPresentm)),
      absent: monthlyChartData.map(item => Number(item.countAbsentm)),
    };     

    this.chartOptions2 = {
      chart: {
        type: 'bar',
        height: 300,
        width: '100%',
        background: 'White',
        foreColor: '#0F0F0F',
        toolbar: {
          show: false, // Set to false to hide the toolbar/menu icon
        },
      },
      colors: ['#66BB6A', '#FFD54F', '#66BB6A', '#EF5350', '#FFFF33'], // Use a set of colors for better combinations
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '1%', // Adjust the width of the bars
          borderRadius: 0, // Add some border radius for a more modern look
        },
      },
      dataLabels: {
        enabled: false, // Disable data labels for a cleaner look
      },
      stroke: {
        show: true,
        width: 2,
        colors: ['transparent'],
      },
      xaxis: {
        categories: monthlyChartData.map(item => item.monthname),
        axisBorder: { show: false, },
        axisTicks: { show: false, },         
         labels: {
          style: {
            fontWeight: 'bold',
            fontSize: '14px',
            //colors: ['#FF5733', '#33FF57', '#5733FF', '#FFFF33'], // Set different colors for each label
          },
        },
      },
      yaxis: {
        title: {
          text: 'Weeks',
          style: {
            fontWeight: 'bold',
            fontSize: '14px',
            color: '#0F0F0F', // Set a different color for the y-axis title
          },
        },
      },
      series: [
        {
          name: 'Present',
          data: formattedMonthlyChartData.present,
        },
        {
          name: 'Absent',
          data: formattedMonthlyChartData.absent,
        },
       
      ],
    };

  } 




  loginsummary() {
    var api = 'hrmTrnDashboard/loginSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.loginsummary_list = this.responsedata.loginsummary_list;

      setTimeout(() => {
        $('#login').DataTable();
      }, 1);
    });
  }

  logoutsummary() {
    var api = 'hrmTrnDashboard/logoutSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.logoutsummary_list = this.responsedata.logoutsummary_list;

      setTimeout(() => {
        $('#logout').DataTable();
      }, 1);
    });
  }

  onTabSelected(tabName: string) {
    debugger
    console.log(`Tab selected: ${tabName}`);
    // Add your custom logic here
    this.ondutysummary();
  }

  ondutysummary() {
    var api = 'hrmTrnDashboard/ondutySummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.ondutysummary_list = this.responsedata.onduty_details;

      setTimeout(() => {
        $('#onduty').DataTable();
      }, 1);
    });
  }

  compoffsummary() {
    var api = 'hrmTrnDashboard/compOffSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.compoffsummary_list = this.responsedata.compoffSummary_details;

      setTimeout(() => {
        $('#compoff').DataTable();
      }, 1);
    });
  }

  permissionsummary() {
    var api = 'hrmTrnDashboard/permissionSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.permissionsummary_list = this.responsedata.permissionSummary_details;

      setTimeout(() => {
        $('#permission').DataTable();
      }, 1);
    });
  }

  punchinoutcall() {
    debugger
    var api = 'hrmTrnDashboard/Punchinlogin';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      
      this.punchlogin_time = this.responsedata.punchinlogin;
      if (this.punchlogin_time != null) {
        this.login_time = this.punchlogin_time[0].login_time;
        this.logout_time = this.punchlogin_time[0].logout_time;
        if (this.punchlogin_time[0].update_flag == "Y") {
          this.punch_flag = 'punchin';
        }
        else if (this.punchlogin_time[0].update_flag == "N") {
          this.punch_flag = 'punchout';
        }
        else if (this.punchlogin_time[0].update_flag == "C") {
          this.punch_flag = 'Completed';
        }
        // else {
        //   // this.punch_flag = 'Raise Request';
        //   this.punch_flag = 'punchin';
        // }
      }
      else {
        // this.punch_flag = 'Raise Request';
        this.punch_flag = 'punchin';
      }
    });
  }

  punchin() {
    var param = {
      login_time_audit: this.login_time_audit
    }
    this.NgxSpinnerService.show();
    var url = 'hrmTrnDashboard/punchIn';
    this.service.post(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.punchinoutcall()
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
    });
  }

  punchout() {
    debugger
    var param = {
      login_time_audit: this.login_time_audit
    };
    this.NgxSpinnerService.show();
    const url = 'hrmTrnDashboard/punchOut';
    this.service.post(url,param).subscribe((result: any) => {
      if (result.status) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.punchinoutcall()
      } else {
        this.ToastrService.warning(result.message);
      }
      this.NgxSpinnerService.hide();
    });
    window.location.reload()
  }

  onapplylogin() {
    debugger;
    var url = 'hrmTrnDashboard/applyLoginReq';
    this.service.post(url, this.applyLoginReqForm.value).subscribe((result: any) => {
      
      if (result.status == true) {
        this.ToastrService.success(result.message);
        
        this.loginsummary()
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.applyLoginReqForm.reset()
    });
  }  

  onapplylogout() {
    var url = 'hrmTrnDashboard/applyLogoutReq';
    this.service.post(url, this.applyLogoutReqForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.logoutsummary();
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.applyLogoutReqForm.reset()
    });
  }  

  onapplyod() {
    debugger
    var url = 'hrmTrnDashboard/applyonduty';
    this.service.post(url, this.applyODForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
      
        this.ondutysummary();
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.applyODForm.reset()
    });
  }

  onapplycompoff() {
    var url = 'hrmTrnDashboard/applyCompoffReq';
    this.service.post(url, this.applycompoffForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.compoffsummary();
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.applycompoffForm.reset()
    });
  }

  onapplypermission() {
    var url = 'hrmTrnDashboard/applyPermission';
    this.service.post(url, this.applypermissionForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.permissionsummary();
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.applypermissionForm.reset()
    });
  }

  delete(parameter: any) {
    this.parametervalue = parameter
  }

  logindelete() {
    this.NgxSpinnerService.show();
    var params = {
      attendancelogintmp_gid: this.parametervalue
    }
    var url = 'hrmTrnDashboard/DeleteLoginRequisition';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }

  logoutdelete() {
    this.NgxSpinnerService.show();
    var params = {
      attendancetmp_gid: this.parametervalue
    }
    var url = 'hrmTrnDashboard/DeleteLogoutRequisition';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }

  oddelete() {
    this.NgxSpinnerService.show();
    var params = {
      ondutytracker_gid: this.parametervalue
    }
    var url = 'hrmTrnDashboard/DeleteOD';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }

  compoffdelete() {
    this.NgxSpinnerService.show();
    var params = {
      compensatoryoff_gid: this.parametervalue
    }
    var url = 'hrmTrnDashboard/DeleteCompoff';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }

  permissiondelete(){
    this.NgxSpinnerService.show();
    var params = {
      permissiondtl_gid: this.parametervalue
    }
    var url = 'hrmTrnDashboard/DeletePermission';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }

  salarydetailsummary() {
    var url = 'hrmTrnDashboard/GetsalarydetailSummary';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.salarydetails_list = this.response_data.salarydetails_list;
      this.additionsalarydetail_list = this.response_data.additionsalarydetail_list;
      this.deductionsalarydetail_list = this.response_data.deductionsalarydetail_list;
      this.othersalarydetail_list = this.response_data.othersalarydetail_list;

      setTimeout(() => {
        $('#salarydetails_list').DataTable();
      }, 1);
    });
  }

  PrintPDF(salary_gid: string, month: string, year: string) {
    const api = 'PayRptPayrunSummary/GetPayslipRpt';
    this.NgxSpinnerService.show()
    let param = {
      salary_gid: salary_gid,
      month: month,
      year: year
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  
  myprofile() {
    this.router.navigate(['/hrm/Employeeprofile'])
  }

  myleave() {
    this.router.navigate(['/hrm/HrmMyLeave'])
  }

  approveleave() {
    this.router.navigate(['/hrm/HrmApproveLeave'])
  }

  companypolicies() {
    this.router.navigate(['/hrm/HrmTrnCompanyPolicy'])
  }

  onclose() {
     this.applyLogoutReqForm.reset();
  }

  onclose1(){
    this.applyODForm.reset();
  }

  onclose2(){
    this.applycompoffForm.reset();
  }

  onclose3(){
    this.applypermissionForm.reset();
  }

  onclose4(){
    this.applyLoginReqForm.reset();
  }

   calculateDuration() {
    const fromTime = this.applyODForm.get('od_fromhr')?.value;
    const toTime = this.applyODForm.get('od_tohr')?.value;

    if (fromTime && toTime) {
      // Create Date objects with a fixed date
      const from = new Date(`1970-01-01T${fromTime}:00`);
      const to = new Date(`1970-01-01T${toTime}:00`);
      
      // Calculate the duration in milliseconds
      let durationMs = to.getTime() - from.getTime();
      
      // If duration is negative, assume the 'to' time is on the next day
      if (durationMs < 0) {
        durationMs += 24 * 60 * 60 * 1000; // Add 24 hours in milliseconds
      }

      // Convert milliseconds to hours and minutes
      const durationHours = Math.floor(durationMs / (1000 * 60 * 60));
      const durationMinutes = Math.floor((durationMs % (1000 * 60 * 60)) / (1000 * 60));

      this.totalDuration = `${durationHours} hours ${durationMinutes} minutes`;
    } else {
      this.totalDuration = '';
    }
  }

}

function getbarChartOptions(height: number) {
  const labelColor = '#000';
  const borderColor = '#e6ccb2';
  const baseColor = '#89DA59';
  const secondaryColor = '#FF420E'

  return {
  };

  
}
