import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-sys-rpt-audithistory',
  templateUrl: './sys-rpt-audithistory.component.html',
  styleUrls: ['./sys-rpt-audithistory.component.scss']
})
export class SysRptAudithistoryComponent {
  auditreporthistory_list: any[] = [];
  responsedata: any;
  user_gid: any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: ActivatedRoute, public NgxSpinnerService:NgxSpinnerService, private router: Router, public service: SocketService) {}

  ngOnInit(): void {
    debugger;  
    const user_gid = this.route.snapshot.paramMap.get('user_gid');
    this.user_gid = user_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.user_gid, secretKey).toString(enc.Utf8);
    this.GetAuditReportHistorySummary(deencryptedParam);
    console.log(deencryptedParam)
  }

  GetAuditReportHistorySummary(user_gid: any) {
    var url = 'SysRptAuditReport/GetAuditReportHistorySummary'
    const params = { user_gid: user_gid };
    this.NgxSpinnerService.show();
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.auditreporthistory_list = this.responsedata.audithistoryreport_list;
      setTimeout(() => {
        $('#auditreporthistory_list').DataTable();
        this.NgxSpinnerService.hide();
      },1);
    });
    (error: any) => {
      console.error('Error fetching data', error);
      // Hide the spinner in case of an error
      this.NgxSpinnerService.hide();
    }
  }

  back() {
    this.router.navigate(['/system/SysRptAuditreport'])
  }

}
