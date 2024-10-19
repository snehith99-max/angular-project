import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-hrm-mst-addshifttype',
  templateUrl: './hrm-mst-addshifttype.component.html',
})

export class HrmMstAddshifttypeComponent {
  reactiveFormadd!: FormGroup;
  weekdays_list: any[] = [];
  responsedata: any;

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    public service: SocketService,
    private router: Router,
    public NgxSpinnerService: NgxSpinnerService,) {

    this.reactiveFormadd = new FormGroup({
      shift_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      email_list: new FormControl(''),
      grace_time: new FormControl('', [Validators.required]),
      login_scheduler: new FormControl('', [Validators.required]),
      entrycutoff_time: new FormControl('', [Validators.required]),
      overnight_flag: new FormControl('N'),
      inovernight_flag: new FormControl('M'),
      outovernight_flag: new FormControl('M'),
      logout_schedular: new FormControl('', [Validators.required]),
      existcutoff_time: new FormControl('', [Validators.required]),
      logout_overnight_flag: new FormControl('N'),
      logout_inovernight_flag: new FormControl('M'),
      logout_outovernight_flag: new FormControl('M'),
      logintime: new FormControl(''),
      logouttime: new FormControl(''),
      Ot_cutoff: new FormControl(''),
      weekdays_list: this.formBuilder.array([])
    });
  }

  ngOnInit(): void {
    var url = 'ShiftType/GetWeekdaysummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.weekdays_list = this.responsedata.weekday_list;
      console.log(this.weekdays_list);
      this.triggerGetOptions();

      this.weekdays_list.forEach((item) => {
        item.logintime = ('');
        item.logouttime = ('');
        item.Ot_cutoff = ('');
      });
    });
  }

  triggerGetOptions(): void {
    for (let i = 0; i < this.weekdays_list.length; i++) {
      const data = this.weekdays_list[i];
    }
  }

  // submit() {
  //   var params = {
  //     weekday_list: this.weekdays_list,
  //     login_scheduler: this.reactiveFormadd.value.login_scheduler,
  //     entrycutoff_time: this.reactiveFormadd.value.entrycutoff_time,
  //     overnight_flag: this.reactiveFormadd.value.overnight_flag,
  //     inovernight_flag: this.reactiveFormadd.value.inovernight_flag,
  //     outovernight_flag: this.reactiveFormadd.value.outovernight_flag,
  //     logout_schedular: this.reactiveFormadd.value.logout_schedular,
  //     grace_time: this.reactiveFormadd.value.grace_time,
  //     email_list: this.reactiveFormadd.value.email_list,
  //     shift_name: this.reactiveFormadd.value.shift_name,
  //     logintime: this.reactiveFormadd.value.logintime,
  //     logouttime: this.reactiveFormadd.value.logouttime,
  //     Ot_cutoff: this.reactiveFormadd.value.Ot_cutoff,
  //   }

  //   var url = 'ShiftType/Shiftweekdaystime'
  //   this.NgxSpinnerService.show();
  //   this.service.postparams(url, params).subscribe((result: any) => {
  //     this.NgxSpinnerService.hide();
  //     if (result.status == false) {
  //       this.ToastrService.warning(result.message)
  //       this.router.navigate(['/hrm/HrmMstShiftTypeSummary']);
  //     }
  //     else {
  //       this.ToastrService.success(result.message)
  //       this.router.navigate(['/hrm/HrmMstShiftTypeSummary']);
  //     }
  //   });
  // }

  submit() {
    var params = {

      shift_name: this.reactiveFormadd.value.shift_name,
      email_list: this.reactiveFormadd.value.email_list,
      grace_time: this.reactiveFormadd.value.grace_time,

      weekday_list: this.weekdays_list,
      logintime: this.reactiveFormadd.value.logintime,
      logouttime: this.reactiveFormadd.value.logouttime,
      Ot_cutoff: this.reactiveFormadd.value.Ot_cutoff,

      login_scheduler: this.reactiveFormadd.value.login_scheduler,
      entrycutoff_time: this.reactiveFormadd.value.entrycutoff_time,
      overnight_flag: this.reactiveFormadd.value.overnight_flag,
      inovernight_flag: this.reactiveFormadd.value.inovernight_flag,
      outovernight_flag: this.reactiveFormadd.value.outovernight_flag,
      
      logout_schedular: this.reactiveFormadd.value.logout_schedular,
      existcutoff_time: this.reactiveFormadd.value.existcutoff_time,
      logout_overnight_flag: this.reactiveFormadd.value.logout_overnight_flag,
      logout_inovernight_flag: this.reactiveFormadd.value.logout_inovernight_flag,
      logout_outovernight_flag: this.reactiveFormadd.value.logout_outovernight_flag,      
    }

    var url = 'ShiftType/Shiftweekdaystime'
    this.NgxSpinnerService.show();
    this.service.postparams(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.router.navigate(['/hrm/HrmMstShiftTypeSummary']);
      }
      else {
        this.ToastrService.success(result.message)
        this.router.navigate(['/hrm/HrmMstShiftTypeSummary']);
      }
    });
  }

  get shift_name() {
    return this.reactiveFormadd.get('shift_name')!;
  }

  get grace_time() {
    return this.reactiveFormadd.get('grace_time')!;
  }
}