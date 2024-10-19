import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
interface Istockreport {

  branch_gid: string;
  branch_name: any;

}
@Component({
  selector: 'app-ims-trn-stockstatement',
  templateUrl: './ims-trn-stockstatement.component.html',
  styleUrls: ['./ims-trn-stockstatement.component.scss']
})
export class ImsTrnStockstatementComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list: any;
  mdlBranchName: any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  showOptionsDivId: any;
  rows: any[] = [];

  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, public service: SocketService, private route: Router, private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
  }

  ngOnInit(): void {
    debugger
    this.GetImsRptStockreport();
    this.reactiveform = new FormGroup({
      branch_name: new FormControl(''),
    })
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  // // //// Summary Grid//////
  GetImsRptStockreport() {
    debugger
    var url = 'ImsRptStockreport/GetImsRptStockstatement'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#stockreport_list').DataTable().destroy();
      this.responsedata = result;
      this.stockreport_list = this.responsedata.stockreport_list;
      setTimeout(() => {
        $('#stockreport_list').DataTable();
      }, 1);
    })
    this.NgxSpinnerService.hide();

  }
  openModaldelete() {
  }
  View(product_gid: any,branch_gid:any) {
    debugger
    const key = 'storyboard';
    const param1 = (product_gid);
      const param2 = (branch_gid);
    const productgid = AES.encrypt(param1, key).toString();
    const branchgid = AES.encrypt(param2, key).toString();
    this.route.navigate(['/ims/ImsTrnStockStatementView', productgid,branchgid]);



  }
}
