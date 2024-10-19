import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

interface IWeekoffview {

}

@Component({
  selector: 'app-weekoffview',
  templateUrl: './weekoffview.component.html',
  styleUrls: ['./weekoffview.component.scss']
})

export class WeekoffviewComponent {
  employee_name: any;
  user: any;
  employee_gid: any;
  responsedata: any;
  weekoffview!: IWeekoffview;
  Weekoffview_list: any[] = [];
  Employee_type: any;

  constructor(
    private formBuilder: FormBuilder,
    private ToastrService: ToastrService, route: Router,
    private router: ActivatedRoute,
    public service: SocketService) {
    this.weekoffview = {} as IWeekoffview;
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    this.user = employee_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.user, secretKey).toString(enc.Utf8);
    
    this.getViewWeekoffSummary(deencryptedParam);
    this.employee_gid = deencryptedParam;

    var url = 'WeekOff/GetEmployeename'
    let param = {
      employee_gid: this.employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Employee_type = this.responsedata.Employee_type;
      this.employee_name = this.Employee_type[0].employee_name;
    });
  }

  getViewWeekoffSummary(employee_gid: any) {
    var url = 'WeekOff/getViewWeekoffSummary'
    let param = {
      employee_gid: employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Weekoffview_list = result.weekoffview_list;
    });
  }
}
