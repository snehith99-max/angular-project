import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-pay-mst-tdsapproval',
  templateUrl: './pay-mst-tdsapproval.component.html',
  styleUrls: ['./pay-mst-tdsapproval.component.scss']
})

export class PayMstTdsapprovalComponent {
  
  response_data: any;
  tdsapprovalsummarylist: any;
  tdsapprovedsummarylist: any;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {
  }

  ngOnInit() {
    var api = 'PayMstTDSapproval/GetTDSApprovalPendingSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.tdsapprovalsummarylist = this.response_data.tdsapprovalpendingsummary_list;

      setTimeout(() => {
        $('#tdsapppendingsum').DataTable();
      }, 1);
    });

    var api = 'PayMstTDSapproval/GetTDSApprovedSummary';
    this.NgxSpinnerService.show();
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.response_data = result;
      this.tdsapprovedsummarylist = this.response_data.tdsapprovedsummary_list;

      setTimeout(() => {
        $('#tdsapprovedsum').DataTable();
      }, 1);
    });
  }

  approvetds(params: any,params1:any) {
    debugger;
    const secretKey = 'storyboarderp';

    const param = (params+ '+' +params1);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/payroll/PayMstTDSApprovalFormDetails', encryptedParam])
  }
}
