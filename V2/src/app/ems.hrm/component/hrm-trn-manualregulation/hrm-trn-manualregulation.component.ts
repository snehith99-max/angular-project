import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hrm-trn-manualregulation',
  templateUrl: './hrm-trn-manualregulation.component.html',
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class HrmTrnManualregulationComponent {
  attendance: any;
  monthname: any;
  offer_list: any[] = [];
  consider_list: any[] = [];
  branch: any;
  month_name:any; 
  branch_list: any[] = [];
  dayslist: any[] = [];
  responsedata: any;
  reactiveForm!: FormGroup;
  datelist: Date[] = [];
  daydatalist: any[] = [];
  file!: File;
  months: { name: string, value: string }[] = [
    { name: 'January', value: 'January' },
    { name: 'February', value: 'February' },
    { name: 'March', value: 'March' },
    { name: 'April', value: 'April' },
    { name: 'May', value: 'May' },
    { name: 'June', value: 'June' },
    { name: 'July', value: 'July' },
    { name: 'August', value: 'August' },
    { name: 'September', value: 'September' },
    { name: 'October', value: 'October' },
    { name: 'November', value: 'November' },
    { name: 'December', value: 'December' }
  ];
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,
    private router: Router, private ToastrService: ToastrService, private SocketService: SocketService,
    public service: SocketService, public NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {


    this.reactiveForm = new FormGroup({

      attendance: new FormControl(''),
      branch_name: new FormControl('', [Validators.required,]),
      fromdate: new FormControl(''),
      month:new FormControl('', [Validators.required,]),
      year: new FormControl('', [Validators.required,]),
      todate: new FormControl(''),
      attendance_list: new FormControl(''),
      // attendanceList: this.formBuilder.array([]), 

    });
    var url = 'EmployeeOnboard/PopBranch';
    this.SocketService.get(url).subscribe((result: any) => {
      this.branch_list = result.employee;
    });

  }

  get branch_name() {
    return this.reactiveForm.get('branch_name')!;
  }
  get month() {
    return this.reactiveForm.get('month')!;
  }
  search() {
    debugger
    let param = {
      fromdate: this.reactiveForm.value.fromdate,
      todate: this.reactiveForm.value.todate
    }
    var url = 'ManualRegulation/GetManualRegulationsummary'
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.NgxSpinnerService.hide();
      this.offer_list = this.responsedata.manuallist;
      this.dayslist = this.responsedata.dayslist;
      this.daydatalist = this.responsedata.daydatalist;
      console.log('offer', this.offer_list) 
      console.log('daylist', this.dayslist) 
      console.log('dataday', this.daydatalist) 
    });
    
    const fromdate1:Date = new Date(this.reactiveForm.value.fromdate);
    const todate1:Date  = new Date(this.reactiveForm.value.todate);
    this.datelist=[];
    let currentdateloop = new Date (fromdate1);
    while(currentdateloop <= todate1){
      this.datelist.push(new Date(currentdateloop));
      currentdateloop.setDate(currentdateloop.getDate()+1);
    }
   
    // setTimeout(() => {
    //   $('#offer_list').DataTable();
    // });
  }
  updatemanual(data: any) {
    let param = {
      daydatalist: data.daydatalist,
      employee_gid: data.employee_gid,
      fromdate: this.reactiveForm.value.fromdate,
      todate: this.reactiveForm.value.todate,
      attendance_list: this.reactiveForm.value.attendance,
    };

    var url = 'ManualRegulation/updateManualRegulation';
    this.service.postparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
    });
  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Attendance Details";
    window.location.href = "https://"+ environment.host + "/Templates/MonthlyAttendance.xlsx";
    link.click();
  }
  importexcel() {
    debugger;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0, 
      });
      formData.append("file", this.file, this.file.name);
      var api = 'ManualRegulation/AttendanceImport'
      this.NgxSpinnerService.show();
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        debugger;
        this.responsedata = result;       
         if(result.status ==false){
          this.ToastrService.warning(result.message)  
        }
        else{
          this.ToastrService.success(result.message)
        }
         
      });
    }
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }

  updateDateRange() {
    const selectedMonth = this.reactiveForm.get('month')?.value;
    const year = this.reactiveForm.get('year')?.value;
    const monthIndex = this.months.findIndex(m => m.value === selectedMonth);
    const startDate = year + '-' + this.pad(monthIndex + 1) + '-01';
    const endDate = year + '-' + this.pad(monthIndex + 1) + '-' + this.getLastDayOfMonth(monthIndex + 1, year);
   

    this.reactiveForm.patchValue({
      fromdate: startDate,
      todate: endDate
    });
     // Set min and max attributes for date inputs
    const fromDateInput = document.querySelector('input[formControlName="fromdate"]') as HTMLInputElement;
    fromDateInput.min = startDate;
    fromDateInput.max = endDate;

    const toDateInput = document.querySelector('input[formControlName="todate"]') as HTMLInputElement;
    toDateInput.min = startDate;
    toDateInput.max = endDate;

    fromDateInput.addEventListener('change', () => {
      const selectedFromDate = new Date(fromDateInput.value);
      const newMinDate = selectedFromDate.toISOString().split('T')[0]; // Get selected from date in YYYY-MM-DD format
      toDateInput.min = newMinDate;
      toDateInput.value = newMinDate; // Reset "todate" value if it's before new min date
  });
   
  }
  pad(num: number) {
    return num.toString().padStart(2, '0');
}

getLastDayOfMonth(month: number, year: number) {
    return new Date(year, month, 0).getDate();
}
getColorBasedOnCondition(dataValue: string): string {
  {
    switch (dataValue) {
      case 'WH':
        return '#F2F0CE';
      case 'XX':
        return '#A9D979';
      case 'XL':
        return '#705720d1';
      case 'LX':
        return '#705720d1';
      case 'AA':
        return '#EB826C';
      case 'LA':
        return '#CCEA8D';
        case 'AL':
        return '#CCEA8D';
      case 'CL':
        return '#2994C4';
      case 'SL':
        return '#A5CEFF';
        case 'SLH':
          return '#775454';
      case 'PL':
        return '#969696';
      case 'LL':
        return '#969696';
      case 'HL':
        return '#969696s';
      case 'FH':
        return '#EBE550';
      case 'AX':
        return '#F2CA52'; 
        case 'XA':
        return '#F2CA52';     
      case 'NH':
        return '#A9CDD4';
      case 'DD':
        return '#BF9FBA';
        case 'OD':
        return '#BF9FBA';
      case 'DX':
        return '#BF9FBA';
      case 'XD':
        return '#BF9FBA';
      case 'EL':
        return '#969696s';
      case 'CO':
        return '#FA6CB4';      
      default:
        return 'transparent';
    }
  }
}
}
