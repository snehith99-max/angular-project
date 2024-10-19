import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-acc-trn-gstmanagement-summary',
  templateUrl: './acc-trn-gstmanagement-summary.component.html',
  styleUrls: ['./acc-trn-gstmanagement-summary.component.scss']
})
export class AccTrnGstmanagementSummaryComponent {
  responsedata: any;
  GetGstManagement_list:any;
  month:any;
  year:any;
  constructor(public service: SocketService, private route: Router, private NgxSpinnerService: NgxSpinnerService,  private router: Router, private ToastrService: ToastrService) {
  }

  ngOnInit(): void {
   
    var url = 'GstManagement/GstManagementSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.GetGstManagement_list = this.responsedata.GetGstManagement_list;
    //console.log(this.GetGstManagement_list)
      setTimeout(() => {
        $('#GetGstManagement_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 200, // Number of rows to display per page
            "lengthMenu": [200, 500, 1000, 1500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  ingst(param1: any, param2: any) {
    this.month = param1;
    this.year = param2
    const secretKey = 'storyboarderp';
    const lsmonth = AES.encrypt(this.month, secretKey).toString();
    const lsyear = AES.encrypt(this.year, secretKey).toString();

    this.route.navigate(['/finance/AccTrnGstInSummary', lsmonth, lsyear]);
  }
  outgst(param1: any, param2: any) {
    this.month = param1;
    this.year = param2
    const secretKey = 'storyboarderp';
    const lsmonth = AES.encrypt(this.month, secretKey).toString();
    const lsyear = AES.encrypt(this.year, secretKey).toString();

    this.route.navigate(['/finance/AccTrnGstOutSummary', lsmonth, lsyear]);
  }
  fillinggst(param1: any, param2: any) {
    this.month = param1;
    this.year = param2
    const secretKey = 'storyboarderp';
    const lsmonth = AES.encrypt(this.month, secretKey).toString();
    const lsyear = AES.encrypt(this.year, secretKey).toString();

    this.route.navigate(['/finance/AccTrnGstFillingSummary', lsmonth, lsyear]);
  }
}
