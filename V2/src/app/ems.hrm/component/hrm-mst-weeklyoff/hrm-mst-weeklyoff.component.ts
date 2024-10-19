import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-hrm-mst-weeklyoff',
  templateUrl: './hrm-mst-weeklyoff.component.html',
  styleUrls: ['./hrm-mst-weeklyoff.component.scss']
})

export class HrmMstWeeklyoffComponent {
  Weekoff: FormGroup | any;
  employee_gid: any;
  employee_gid1: any[] = [];
  mdlMondayName: any;
  mdltuesdayName: any;
  mdlwednesdayName: any;
  mdlthursdayName: any;
  mdlfridayName: any;
  mdlsaturdayName: any;
  mdlsundayName: any;
  deencryptedParam1: any;
  deencryptedParam: any;
  employee_name: any;

  constructor(
    private SocketService: SocketService,
    private route: Router,
    private router: ActivatedRoute,
    private NgxSpinnerService: NgxSpinnerService,
    public service: SocketService,
    private ToastrService: ToastrService,
    private FormBuilder: FormBuilder)
    {
    this.Weekoff = new FormGroup({
      monday: new FormControl('', [Validators.required]),
      employee_gid: new FormControl(''),
      tuesday: new FormControl('', [Validators.required]),
      wednesday: new FormControl('', [Validators.required]),
      thursday: new FormControl('', [Validators.required]),
      friday: new FormControl('', [Validators.required]),
      saturday: new FormControl('', [Validators.required]),
      sunday: new FormControl('', [Validators.required]),
    });
  }

  ngOnInit() {
    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    const employee_name = this.router.snapshot.paramMap.get('employee_name');

    this.employee_name = employee_name;
    this.employee_gid = employee_gid;

    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.employee_gid, secretKey).toString(enc.Utf8);
    this.deencryptedParam1 = AES.decrypt(this.employee_name, secretKey).toString(enc.Utf8);
    this.Weekoff.get('employee_gid')?.setValue(this.deencryptedParam);
    this.employee_gid1.push(this.Weekoff.value.employee_gid)
  }

  updateweekoff() {
    var url = "WeekOff/updateweekoffemployee";
    var params = {
      employee_gid1: this.employee_gid1,
      monday: this.Weekoff.value.monday,
      tuesday: this.Weekoff.value.tuesday,
      wednesday: this.Weekoff.value.wednesday,
      thursday: this.Weekoff.value.thursday,
      friday: this.Weekoff.value.friday,
      saturday: this.Weekoff.value.saturday,
      sunday: this.Weekoff.value.sunday,
    }

    this.SocketService.postparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.route.navigate(['/hrm/HrmMstWeekoffmanagement'])
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }
  
  backbutton() {
    this.route.navigate(['/hrm/HrmMstWeekoffmanagement'])
    this.Weekoff.reset();
  }
}