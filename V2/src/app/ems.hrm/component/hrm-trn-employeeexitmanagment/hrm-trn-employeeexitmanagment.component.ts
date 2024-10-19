import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-hrm-trn-employeeexitmanagment',
  templateUrl: './hrm-trn-employeeexitmanagment.component.html',
  styleUrls: ['./hrm-trn-employeeexitmanagment.component.scss']
})
export class HrmTrnEmployeeexitmanagmentComponent {

  GetExitmanagament_list: any[] = [];

  constructor(public service: SocketService,
    public route: Router
  ) { }

  ngOnInit(): void {
    debugger
    var api = 'ExitManagement/GetExitmanagementSummary';
    this.service.get(api).subscribe((result: any) => {
      this.GetExitmanagament_list = result.GetExitmanagament_list
    });
  }
  Onview360(exitemployee_gid: any) {
    const key = 'storyboarderp';
    const param = (exitemployee_gid);
    const exitemployee_gid1 = AES.encrypt(param, key).toString();
    this.route.navigate(['/hrm/HrmExitmanagement360', exitemployee_gid1])
  }

}
