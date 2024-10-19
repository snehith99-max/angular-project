import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';


@Component({
  selector: 'app-pay-mst-bankmaster',
  templateUrl: './pay-mst-bankmaster.component.html',
  styleUrls: ['./pay-mst-bankmaster.component.scss']
})
export class PayMstBankmasterComponent {
  showOptionsDivId: any;
  bankmaster_list: any[] = [];
  responsedata: any;

  constructor(public service: SocketService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    var url = 'PayMstBankMaster/GetBankMasterSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.bankmaster_list = this.responsedata.GetBankMaster_list;
      setTimeout(() => {
        $('#bankmaster_list').DataTable();
      }, 1);
    });
  }
  
  
  
  onadd() {
    this.router.navigate(['/payroll/Paymstbankmasteradd'])
  }

  onedit(params: any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/payroll/Paymstbankmasteredit', encryptedParam])

  }
}
