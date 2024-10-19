import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';

@Component({
  selector: 'app-acc-trn-fundtransferapproval',
  templateUrl: './acc-trn-fundtransferapproval.component.html',
  styleUrls: ['./acc-trn-fundtransferapproval.component.scss']
})
export class AccTrnFundtransferapprovalComponent {
  GetFundTransfer_list: any;
  bankmaster_list: any;
  parameterValue1: any;
  responsedata: any;
  branchname_list: any;
  reactiveform: FormGroup | any;
  mdlBranchName: any;
  remarks: any;
  from_branch: any;
  to_branch: any;
  pettycash_gid:any;
transaction_date:any;
transaction_amount:any;
approval_flag:any;
  showOptionsDivId: any;
  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService) {
    this.reactiveform = new FormGroup({
      pettycashgid: new FormControl(''),
      remarks: new FormControl(''),
      reason: new FormControl(''),
      status_flag: new FormControl(''),
    })
 
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.branchname_list = [
      { branch_gid: 'Vcidex Solutions Pvt Ltd', branch_name: 'Vcidex Solutions Pvt Ltd' },
      { branch_gid: 'Vcidex Singapore PVT Ltd', branch_name: 'Vcidex Singapore PVT Ltd' },
      { branch_gid: 'NISSI CHENNAI', branch_name: 'NISSI CHENNAI' }
    ];
  this.getsummary();

  document.addEventListener('click', (event: MouseEvent) => {
    if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
      this.showOptionsDivId = null;
    }
  });
  
  }

  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
  getsummary()
  {
    var url = 'FundTransferApproval/GetFundTransferApprovalSummary'
    this.service.get(url).subscribe((result: any) => {

      this.responsedata = result;
      this.GetFundTransfer_list = this.responsedata.GetFundTransferApproval_list;
      $('#GetFundTransfer_list').DataTable().destroy();
      console.log(this.GetFundTransfer_list)
      setTimeout(() => {
        $('#GetFundTransfer_list').DataTable(
          {
            // code by snehith for customized pagination
            "pageLength": 200, // Number of rows to display per page
            "lengthMenu": [200, 500, 1000, 1500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();

    return dd + '-' + mm + '-' + yyyy;
  }

  popmodal(parameter: string, param1: string, param2: string) {

    this.remarks = parameter;
    this.from_branch = param1;
    this.to_branch = param2;

  }

  clear() {
    this.reactiveform.reset();
  }
  reviewmodal(data:any){
    //console.log(data)
   
this.pettycash_gid= data.pettycash_gid;
this.transaction_date= data.transaction_date;
this.transaction_amount= data.transaction_amount;
this.remarks= data.remarks;
this.from_branch= data.from_branch;
// this.from_branch= data.from_branch;
this.to_branch= data.to_branch;
this.reactiveform.get("remarks")?.setValue(data.remarks);
this.reactiveform.get("reason")?.setValue(data.reason);
this.reactiveform.get("pettycashgid")?.setValue(data.pettycash_gid);

this.approval_flag = data.approval_flag
  }
  onreview(para :any )
{
  console.log(para)
    this.reactiveform.get("status_flag")?.setValue(para);
    //console.log(this.reactiveform.value)
    this.NgxSpinnerService.show();
    var url = 'FundTransferApproval/FundTransferApprovalStatus'
    this.service.post(url, this.reactiveform.value).subscribe((result: any) => {

      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });

        this.reactiveform.reset();
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)

      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.reactiveform.reset();
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.getsummary();

      }

    });

}
}
