import { Component } from '@angular/core';
import { FormBuilder,FormGroup,FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


export type ChartOptions1 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};

@Component({
  selector: 'app-hrm-dashboard',
  templateUrl: './hrm-dashboard.component.html',
  styleUrls: ['./hrm-dashboard.component.scss']
})

export class HrmDashboardComponent {

  chartOptions1: any = {};
  Date: string;

  responsedata: any;
  totalactiveemployeecount: any;
  presentemployee_count: any;
  absentemployee_count: any;
  leaveemployee_count: any;
  totalemployee_count: any;
  totalactiveemployeelist: any;
  employeepresentlist: any;
  employeeabsentlist: any;
  employeeleavelist: any;

  todaybirthdaycount: any;
  today_birthdaycount: any;
  todaybirthdaylist: any;

  upcomingbirthdaycount: any;
  upcoming_birthdaycount: any;
  upcomingbirthdaylist: any;

  totalexperience: any;
  workanniversary_count: any;
  workanniversarylist: any;

  totalprobationemployee: any;
  probationemployee_count: any;
  probationemployeelist: any;

  empcountbylocation: any;
  empbranch_count: any;
  emp_count: any;

  total_activeemployees: any;
  pendingleavecount: any;
  pendingleave_count: any;
  applydate:any;
  leavetodate:any;
  leavefromdate:any;
  total_pendinglist: any;
  total_loginpendinglist: any;
  total_logoutpendinglist: any;
  total_odpendinglist: any;
  total_permissionpendinglist: any;
  total_compoffpendinglist: any;
  remarks:any;
  reason:any;
  parametevalue:any
  employeename:any;
  departmentname:any;
  leavename:any;
  empStatistics_list: any[] = [];
  empStatistics_listflag: boolean = false;
  emp_joining_count: any;
  emp_exit_count: any;
  months: any;
  empStatisticschart: any = {};
  empActivecount_list: any;
  employee_count: any;
  empActivecountchart: any = {};
  empActivecountflag: boolean = false;
  currentTab = 'leave';
  reactiveFormadd: FormGroup;
  leave_historylist: any;
  login_historylist: any;
  logout_historylist: any;
  compoff_historylist: any;
  od_historylist: any;
  permission_HistoryList: any;

   constructor(private fb: FormBuilder, private route: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService, private router: Router, private service: SocketService,private ToastrService: ToastrService) {
    this.Date = new Date().toString();
    this.reactiveFormadd = new FormGroup({
      remarks: new FormControl('')
      })
  }

  ngOnInit(): void {
    this.Getdaywisechart();
    var api = 'HRDashboard/GetTotalActiveEmployeeCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.totalactiveemployeecount = this.responsedata.TotalActiveEmployeeCount;
      this.presentemployee_count = this.totalactiveemployeecount[0].present_count;
      this.absentemployee_count = this.totalactiveemployeecount[0].absent_count;
      this.leaveemployee_count = this.totalactiveemployeecount[0].leave_count;
      this.totalemployee_count = this.totalactiveemployeecount[0].employee_count;
    });

    var api = 'HRDashboard/GetEmployeePresentList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.employeepresentlist = this.responsedata.TotalActiveEmployeeList;
    });

    var api = 'HRDashboard/GetEmployeeAbsentList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.employeeabsentlist = this.responsedata.TotalActiveEmployeeList;
    });

    var api = 'HRDashboard/GetEmployeeLeaveList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.employeeleavelist = this.responsedata.TotalActiveEmployeeList;
    });

    var api = 'HRDashboard/GetTotalActiveEmployeeList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.totalactiveemployeelist = this.responsedata.TotalActiveEmployeeList;
    });

    var api = 'HRDashboard/GetTodayBirthdayCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.todaybirthdaycount = this.responsedata.TodayBirthdayCount;
      this.today_birthdaycount = this.todaybirthdaycount[0].today_birthdaycount;
    });

    var api = 'HRDashboard/GetTodayBirthdayList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.todaybirthdaylist = this.responsedata.TodayBirthdayList;
    });

    var api = 'HRDashboard/GetUpcomingBirthdayCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.upcomingbirthdaycount = this.responsedata.UpcomingBirthdayCount;
      this.upcoming_birthdaycount = this.upcomingbirthdaycount[0].upcoming_birthdaycount;
    });

    var api = 'HRDashboard/GetUpcomingBirthdayList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.upcomingbirthdaylist = this.responsedata.UpcomingBirthdayList;
    });

    var api = 'HRDashboard/GetWorkAnniversaryCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.totalexperience = this.responsedata.WorkAnniversaryCount;
      this.workanniversary_count = this.totalexperience[0].workanniversarycount;
    });

    var api = 'HRDashboard/GetWorkAnniversaryList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.workanniversarylist = this.responsedata.WorkAnniversaryList;
      console.log(this.workanniversarylist)
    });



    var api = 'HRDashboard/GetOnProbationCount';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.totalprobationemployee = this.responsedata.OnProbationCount;
      this.probationemployee_count = this.totalprobationemployee[0].total_probationemployee;
    });

    var api = 'HRDashboard/GetOnProbationList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.probationemployeelist = this.responsedata.OnProbationList;
    });

    var api = 'HRDashboard/GetTotalActiveEmployees';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_activeemployees = this.responsedata.TotalActiveEmployees;
    });

    // var api = 'HRDashboard/GetToDoListCount';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.pendingleavecount = this.responsedata.ToDoListCount;
    //   this.pendingleave_count = this.pendingleavecount[0].pending_leaves;
    //   this.applydate = this.pendingleavecount[0].leave_applydate;
    //   this.leavetodate = this.pendingleavecount[0].leave_todate;
    //   this.leavefromdate = this.pendingleavecount[0].leave_fromdate;
    //   this.remarks = this.pendingleavecount[0].leave_remarks;
    //   this.reason = this.pendingleavecount[0].leave_reason;
    //   this.employeename = this.pendingleavecount[0].user_firstname;
    //   this.departmentname = this.pendingleavecount[0].department_name;
    //   this.leavename = this.pendingleavecount[0].leavetype_name;

      

    // });

    var api = 'HRDashboard/GetToDoList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_pendinglist = this.responsedata.ToDoList;
    });

    var api = 'HRDashboard/GetToDoLoginList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_loginpendinglist = this.responsedata.ToDoLoginList;
    });

    var api = 'HRDashboard/GetToDoLogoutList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_logoutpendinglist = this.responsedata.ToDoLogoutList;
    });

    var api = 'HRDashboard/GetToDoODList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_odpendinglist = this.responsedata.ToDoODList;
    });

    var api = 'HRDashboard/GetToDoPermissionList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_permissionpendinglist = this.responsedata.ToDoPermissionList;
    });

    var api = 'HRDashboard/GetToDoCompoffList';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.total_compoffpendinglist = this.responsedata.ToDoCompOffList;
    });
    

    this.GetempStatistics();
    this.GetempActivecount();
  }
  
  mymodal(leave_gid:string){
    this.parametevalue=leave_gid
    let param={
      leave_gid:this.parametevalue

    }
    var api = 'HRDashboard/GetToDoListCount';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.responsedata = result;
      this.pendingleavecount = this.responsedata.ToDoListCount;
      this.pendingleave_count = this.pendingleavecount[0].leave_noofdays;
      this.applydate = this.pendingleavecount[0].leave_applydate;
      this.leavetodate = this.pendingleavecount[0].leave_todate;
      this.leavefromdate = this.pendingleavecount[0].leave_fromdate;
      this.remarks = this.pendingleavecount[0].leave_remarks;
      this.reason = this.pendingleavecount[0].leave_reason;
      this.employeename = this.pendingleavecount[0].user_firstname;
      this.departmentname = this.pendingleavecount[0].department_name;
      this.leavename = this.pendingleavecount[0].leavetype_name;     

    });
  } 

  Getdaywisechart() {
    var url = 'HRDashboard/GetEmpCountbyLocation';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.empcountbylocation = this.responsedata.EmpCountbyLocation;

      const categories = this.empcountbylocation.map((entry: { branch_name: any; }) => entry.branch_name);
      const data = this.empcountbylocation.map((entry: { employee_count: any; }) => entry.employee_count);

      this.chartOptions1 = {
        chart: {
          type: 'bar',
          height: 250,
          background: 'White',
          foreColor: '#0F0F0F',
          toolbar: {
            show: false,
          },
        },

        plotOptions: {
          bar: {
            columnWidth: '25%',
          },
        },

        dataLabels: {
          enabled: false,
        },

        xaxis: {
          categories: categories,
          labels: {
            style: {
              fontWeight: 'bold',
              fontSize: '8px',
            },
          },
        },

        yaxis: {
          title: {
            text: 'Employee Count',
            style: {
              fontWeight: 'bold',
              fontSize: '8px',
              color: '#0F0F0F',
            },
          },
        },

        series: [
          {
            name: 'Employee_Count',
            data: data,
            color: '#b950eb',
          },
        ],
      };

    })
  }

  GetempStatistics() {
    var url = 'HRDashboard/GetempStatistics';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.empStatistics_list = this.responsedata.empStatistics_list;
      if (this.empStatistics_list.length > 0) {
        this.empStatistics_listflag = true;
      }
      this.emp_joining_count = this.empStatistics_list.map((entry: { emp_joining_count: any }) => entry.emp_joining_count),
        this.emp_exit_count = this.empStatistics_list.map((entry: { emp_exit_count: any }) => entry.emp_exit_count),
        this.months = this.empStatistics_list.map((entry: { months: any }) => entry.months),

        this.empStatisticschart = {
          chart: {
            type: 'line',
            height: 260,
            width: '100%',
            background: 'White',
            foreColor: '#0F0F0F',
            fontFamily: 'inherit',
            toolbar: {
              show: false,
            },
            offsetY: 0
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
                fontSize: '10px',
                tick: 'false'
              },
            },
          },
          yaxis: [{
            title: {
              style: {
                fontWeight: 'bold',
                fontSize: '8px',
                color: '#7FC7D9',
              },
            },
            tickAmount: 4,
            labels: {
              formatter: function (value: any) {
                return value.toFixed(0);
              }
            }
          }],
          series: [
            {
              name: 'Joining',
              data: this.emp_joining_count,
              color: '#50cd89'
            },
            {
              name: 'Exit',
              data: this.emp_exit_count,
              color: '#f1416c'
            },
          ],

          stroke: {
            curve: 'smooth',
            width: 2,
          },
          legend: {
            position: "top",
            offsetY: 2,
            fontSize: '10px',
          }
        };
    });
  }

  GetempActivecount() {
    var url = 'HRDashboard/GetempActivecount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.empActivecount_list = this.responsedata.empActivecount_list;
      if (this.empActivecount_list.length > 0) {
        this.empActivecountflag = true;
      }
      this.employee_count = this.empActivecount_list.map((entry: { employee_count: any }) => entry.employee_count),
        this.months = this.empActivecount_list.map((entry: { months: any }) => entry.months),

        this.empActivecountchart = {
          chart: {
            type: 'line',
            height: 260,
            width: '100%',
            background: 'White',
            foreColor: '#0F0F0F',
            fontFamily: 'inherit',
            toolbar: {
              show: false,
            },
            offsetY: 0
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
            enabled: true,
          },
          xaxis: {
            categories: this.months,
            labels: {
              style: {
                fontSize: '10px',
                tick: 'false'
              },
            },
          },
          yaxis: [{
            title: {
              style: {
                fontWeight: 'bold',
                fontSize: '8px',
                color: '#7FC7D9',
              },
            },
            tickAmount: 3,
            labels: {
              formatter: function (value: any) {
                return value.toFixed(0);
              }
            }
          }],
          series: [
            {
              name: 'Overall Count',
              data: this.employee_count,
              color: '#0A5C36'
            },
          ],

          stroke: {
            curve: 'smooth',
            width: 2,
          },
          legend: {
            position: "top",
            offsetY: 2,
            fontSize: '10px',
          }
        };
    });
  }

  approve(){
    this.NgxSpinnerService.show()
    let param={
      leave_gid:this.parametevalue,
      remarks: this.reactiveFormadd.value.remarks
     
    }
    var url = 'HRDashboard/Approveevent';  
    this.service.post(url, param).subscribe((result: any) => {      
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();

      }
      else {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();
        this.router.navigate(['/hrm/HrmDashboard']);
        this.ngOnInit()
      }
    });
  }

  
  reject(){
    this.NgxSpinnerService.show();

    let param={
      leave_gid:this.parametevalue,
      
    }
    var url = 'HRDashboard/Rejectleaveevent';  
    this.service.post(url, param).subscribe((result: any) => {      
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()


      }
    });
  }
 
  leavehistory(){
    var api = 'HRDashboard/GetLeaveHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.leave_historylist = this.responsedata.Leave_HistoryList;
    });
  }

  loginhistory(){
    var api = 'HRDashboard/GetLoginHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.login_historylist = this.responsedata.Login_HistoryList;
    });
  }

  logouthistory(){
    var api = 'HRDashboard/GetLogoutHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.logout_historylist = this.responsedata.Logout_HistoryList;
    });
  }

  odhistory(){
    var api = 'HRDashboard/GetOdHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.od_historylist = this.responsedata.od_HistoryList;
    });
  }

  compoffhistory(){
    var api = 'HRDashboard/GetCompoffHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.compoff_historylist = this.responsedata.Compoff_HistoryList;
    });
  }

  permissionhistory(){
    var api = 'HRDashboard/GetPermissionHistoryDetails';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.permission_HistoryList = this.responsedata.permission_HistoryList;
    });
  }

  mymodallogin(attendancelogintmp_gid:any )
  {
    this.parametevalue=attendancelogintmp_gid
   
    let param={
      attendancelogintmp_gid:this.parametevalue,
     

    }
    var api = 'HRDashboard/LoginApprove';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()


      }
    });
  }

  Loginreject(attendancelogintmp_gid:any )
  {
    this.parametevalue=attendancelogintmp_gid
   
    let param={
      attendancelogintmp_gid:this.parametevalue,
     

    }
    var api = 'HRDashboard/Loginreject';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()


      }
    });
  }

  mymodallogout(attendancetmp_gid:any ){
    this.parametevalue=attendancetmp_gid
   
    let param={
      attendancetmp_gid:this.parametevalue    

    }
    var api = 'HRDashboard/LogoutApprove';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });

  }

  Logoutreject(attendancetmp_gid:any ){
    this.parametevalue=attendancetmp_gid
   
    let param={
      attendancetmp_gid:this.parametevalue    

    }
    var api = 'HRDashboard/Logoutreject';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });

  }

  mymodalcompoff(compensatoryoff_gid:any){
    this.parametevalue=compensatoryoff_gid
   
    let param={
      compensatoryoff_gid:this.parametevalue,
     }
     var api = 'HRDashboard/CompoffApprove';
     this.service.post(api,param).subscribe((result: any) => {
       if (result.status == false) {
         this.ToastrService.warning(result.message)
         this.NgxSpinnerService.hide()
 
       }
       else {
         this.ToastrService.success(result.message);
         this.router.navigate(['/hrm/HrmDashboard'])
         this.NgxSpinnerService.hide()
         this.ngOnInit()
       }
     });
 
  }

  Compoffreject(compensatoryoff_gid:any){
    this.parametevalue=compensatoryoff_gid
   
    let param={
      compensatoryoff_gid:this.parametevalue,
     }
     var api = 'HRDashboard/Compoffreject';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });
  }
  ApproveOD(ondutytracker_gid:any ){
    this.parametevalue=ondutytracker_gid
   
    let param={
      ondutytracker_gid:this.parametevalue    

    }
    var api = 'HRDashboard/ApproveOD';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });

  }

  RejectOD(ondutytracker_gid:any ){
    this.parametevalue=ondutytracker_gid
   
    let param={
      ondutytracker_gid:this.parametevalue    

    } 
    var api = 'HRDashboard/RejectOD';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });
  }

  mymodalpermission(permissiondtl_gid:any){
    this.parametevalue=permissiondtl_gid
   
    let param={
      permissiondtl_gid:this.parametevalue    

    }
    var api = 'HRDashboard/Permissiontapprove';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });
  }

  permissionreject(permissiondtl_gid:any){
    this.parametevalue=permissiondtl_gid
   
    let param={
      permissiondtl_gid:this.parametevalue    

    }
    var api = 'HRDashboard/PermissionReject';
    this.service.post(api,param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()

      }
      else {
        this.ToastrService.success(result.message);
        this.router.navigate(['/hrm/HrmDashboard'])
        this.NgxSpinnerService.hide()
        this.ngOnInit()
      }
    });
  }

 
showTab(tab: string) {
    this.currentTab = tab;
  }
  showTab1(tab: string) {
    this.currentTab = tab;
  }
  showTab2(tab: string) {
    this.currentTab = tab;
  }
  showTab3(tab: string) {
    this.currentTab = tab;
  }
  showTab4(tab: string) {
    this.currentTab = tab;
  }
  showTab5(tab: string) {
    this.currentTab = tab;
  }

  close(){
    this.reactiveFormadd.reset();
  }
  
  close1(){

  }

  close2(){

  }

  close3(){

  }

  close4(){
    
  }

  close5(){

  }

  close6(){

  }

}
