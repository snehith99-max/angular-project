import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupName, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner'; import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-hrm-mst-addholidaygrade',
  templateUrl: './hrm-mst-addholidaygrade.component.html',
  styleUrls: ['./hrm-mst-addholidaygrade.component.scss']
})

export class HrmMstAddholidaygradeComponent {
  Holidaygradecode_list: any[] = [];
  reactiveFormadd!: FormGroup;
  responsedata: any;
  parameterValue: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
    this.reactiveFormadd = new FormGroup({
      Holiday_date: new FormControl('', [Validators.required]),
      holiday_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      holiday_type: new FormControl(''),
      holiday_remarks: new FormControl(''),
    });
  }
  
  ngOnInit(): void {
    this.holidaygradesummary()

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
  }

  holidaygradesummary() {
    var url = 'HolidayGradeManagement/Addholidaysummary'
    this.service.get(url).subscribe((result: any) => {
      $('#Holidaygradecode_list').DataTable().destroy();
      this.responsedata = result;
      this.Holidaygradecode_list = this.responsedata.holidaygrade1_list;
      setTimeout(() => {
        $('#Holidaygradecode_list').DataTable();
      },);
    });
  }

  Addholiday() {
    var params = {
      Holiday_date: this.reactiveFormadd.value.Holiday_date,
      holiday_name: this.reactiveFormadd.value.holiday_name,
      holiday_type: this.reactiveFormadd.value.holiday_type,
      holiday_remarks: this.reactiveFormadd.value.holiday_remarks,
    }
    if (this.reactiveFormadd.value.Holiday_date != null && this.reactiveFormadd.value.Holiday_date != '') {
      for (const control of Object.keys(this.reactiveFormadd.controls)) {
        this.reactiveFormadd.controls[control].markAsTouched();
      }
      this.reactiveFormadd.value;
    var url = 'HolidayGradeManagement/AddHolidayGradesubmit'
    this.service.postparams(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Adding Holiday')
        this.holidaygradesummary()
        this.router.navigate(['/hrm/HrmMstAddHolidaygrademanagement']);
      }
      else {
        this.ToastrService.success('Holiday Added Sucessfully')
        this.holidaygradesummary()
        this.router.navigate(['/hrm/HrmMstAddHolidaygrademanagement']);

      }
    });

  }
  else {
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
    this.reactiveFormadd.reset();
   
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  
  get Holiday_date() {
    return this.reactiveFormadd.get('Holiday_date')!;
  }
  get holiday_name() {
    return this.reactiveFormadd.get('holiday_name')!;
  }

  ondelete() {
    console.log(this.parameterValue);
    var url3 = 'HolidayGradeManagement/Deleteholiday'
    this.NgxSpinnerService.show();
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
      $('#Holidaygradecode_list').DataTable().destroy();
      if (result.status == false) {
       this.ToastrService.warning(result.message)
       this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.holidaygradesummary()
      }
    });
  //   setTimeout(function() {
  //     window.location.reload();
  // }, 2000); // 2000 milliseconds = 2 seconds
  }
}