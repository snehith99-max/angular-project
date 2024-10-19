import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
interface Istockreport {
 
  branch_gid: string;
  branch_name: any;

}
@Component({
  selector: 'app-ims-rpt-productissue-report',
  templateUrl: './ims-rpt-productissue-report.component.html',
  styleUrls: ['./ims-rpt-productissue-report.component.scss']
})
export class ImsRptProductissueReportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  Request:string='';
  Issue:string='';
  Balance:string='';
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {debugger
    this.GetImsRptProductissuereport();
    
  
}

// // //// Summary Grid//////
GetImsRptProductissuereport() {
  debugger
  
  var url = 'ImsRptProductIssueReport/GetImsRptProductissuereport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.product_issuelist;

    const Request = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.qty_requested.replace(/,/g, '')), 0));
    const Issue = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.qty_issued.replace(/,/g, '')), 0));
    const Balance = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.balance_qty.replace(/,/g, '')), 0));
    this.Request = this.formatNumber(Request);
    this.Issue = this.formatNumber(Issue);
    this.Balance = this.formatNumber(Balance);

    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
             
    })
    this.NgxSpinnerService.hide();
 
}

openModaldelete(){

}
formatNumber(value: number): string {
  return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
roundToTwoDecimal(value: number): number {
  return Math.round(value * 100) / 100;
}
}
