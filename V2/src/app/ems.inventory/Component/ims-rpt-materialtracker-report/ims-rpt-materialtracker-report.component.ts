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
  selector: 'app-ims-rpt-materialtracker-report',
  templateUrl: './ims-rpt-materialtracker-report.component.html',
  styleUrls: ['./ims-rpt-materialtracker-report.component.scss']
})
export class ImsRptMaterialtrackerReportComponent {
  stockreport_list: any[] = [];
  stockreport_list1:any[]=[];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  requestqty:string='';
  issueqty:string='';
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {debugger
    this.GetImsRptmaterialreport();
    this.reactiveform = new FormGroup({
      branch_name: new FormControl(''),

    })
    
    var url = 'ImsRptMaterialTracker/GetBranch'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.branchlist;  
    });
  
}

// // //// Summary Grid//////
GetImsRptmaterialreport() {
  debugger

 
 
}

OnChangeBranch(branch_name :any) {
  const branch_gid =branch_name.branch_gid;
  let param = {
    branch_gid: branch_gid
  }
  var url = 'ImsRptMaterialTracker/GetImsRptmaterialreport'
  this.NgxSpinnerService.show()
  this.service.getparams(url,param).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.materialtrackerlist;
    const requestqty = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.qty_requested.replace(/,/g, '')), 0));
    const issueqty = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.qty_issued.replace(/,/g, '')), 0));
    this.requestqty = this.formatNumber(requestqty);
    this.issueqty = this.formatNumber(issueqty);
    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
   })
    var url = 'ImsRptMaterialTracker/GetImsRptmaterialporeport'
    this.NgxSpinnerService.show()
    this.service.getparams(url,param).subscribe((result: any) => {
      this.responsedata = result;
      this.stockreport_list1 = this.responsedata.materialtracker_list1;
      setTimeout(()=>{  
        $('#stockreport_list1').DataTable();
      }, 1);
             
    })
    this.NgxSpinnerService.hide();
 
}
onclearbranch(){
  this.stockreport_list=[]
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
