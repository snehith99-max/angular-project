import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { format } from 'crypto-js';
// import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';


@Component({
  selector: 'app-hrm-attendance-configuration',
  templateUrl: './hrm-attendance-configuration.component.html',
})

export class HrmAttendanceConfigurationComponent {
  attendaceconfigform: FormGroup | any;
  showInput1: boolean = false;
  showInputs: boolean = false;
  showInputs1: boolean = false;
  responsedata: any;
  attendanceconfiglist: any;
  showNO:any;
  showYES:any;
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService
  ) {
    this.attendaceconfigform = new FormGroup({
      totalshifthours: new FormControl(''),
      halfmineligiblehours: new FormControl(''),
      halfmaxeligiblehours: new FormControl(''),
      weekoff_salary: new FormControl(''),
      holiday_salary: new FormControl(''),
      totalpermissionallowed: new FormControl(''),
      otavailed: new FormControl('N'),
      attendance_allowance_flag:new FormControl('N'),
      otminhours: new FormControl(''),
      otmaxhours: new FormControl(''),
      allowed_leave :new FormControl(''),
      allowance_amount : new FormControl(''),
    });
  }

  showTextBox(event: Event) {
    debugger;
    const target = event.target as HTMLInputElement;
    this.showInputs = target.value === 'Y';
  }
  showTextBox1(event: Event) {
    const target = event.target as HTMLInputElement;
    this.showInputs1 = target.value === 'Y';
  }

  ngOnInit(): void {
    this.GetAttendanceConfiguration();
    
  } 

  submit() {
    const api = 'HrmMstConfiguration/PostHrmconfig'
    this.service.post(api, this.attendaceconfigform.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.router.navigate(['/hrm/HrmConfiguration']);
        this.ToastrService.success(result.message)
      }
    });
  }

  GetAttendanceConfiguration() {
    var api = 'HrmMstConfiguration/GetAttendanceConfiguration';    
    this.service.get(api).subscribe((result: any) => {
      this.responsedata=result;
      this.attendanceconfiglist = result;

      this.attendaceconfigform.get("totalshifthours")?.setValue(this.attendanceconfiglist.totalshifthours);
      this.attendaceconfigform.get("halfmineligiblehours")?.setValue(this.attendanceconfiglist.halfmineligiblehours);
      this.attendaceconfigform.get("halfmaxeligiblehours")?.setValue(this.attendanceconfiglist.halfmaxeligiblehours);
      this.attendaceconfigform.get("weekoff_salary")?.setValue(this.attendanceconfiglist.weekoff_salary);
      this.attendaceconfigform.get("holiday_salary")?.setValue(this.attendanceconfiglist.holiday_salary);
      this.attendaceconfigform.get("totalpermissionallowed")?.setValue(this.attendanceconfiglist.totalpermissionallowed);
      this.attendaceconfigform.get("otminhours")?.setValue(this.attendanceconfiglist.otminhours);
      this.attendaceconfigform.get("otmaxhours")?.setValue(this.attendanceconfiglist.otmaxhours);
      this.attendaceconfigform.get("otavailed")?.setValue(this.attendanceconfiglist.otavailed);
      this.attendaceconfigform.get("attendance_allowance_flag")?.setValue(this.attendanceconfiglist.attendance_allowance_flag);
      this.attendaceconfigform.get("allowed_leave")?.setValue(this.attendanceconfiglist.allowed_leave);
      this.attendaceconfigform.get("allowance_amount")?.setValue(this.attendanceconfiglist.allowance_amount);

if(this.attendanceconfiglist.otavailed == "Y")
{
  this.showInputs = true;   
}
if(this.attendanceconfiglist.attendance_allowance_flag=="Y")
{
  this.showInputs1= true;
}
    
      
    });
  }

  redirecttolist() {
    this.router.navigate(['/hrm/HrmTrnMemberdhashboard'])
  }
}