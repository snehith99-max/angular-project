import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
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
  selector: 'app-ims-rpt-materialissue-report',
  templateUrl: './ims-rpt-materialissue-report.component.html',
  styleUrls: ['./ims-rpt-materialissue-report.component.scss']
})
export class ImsRptMaterialissueReportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {debugger
    this.GetImsRptProductissuereport();
    
  
}

// // //// Summary Grid//////
GetImsRptProductissuereport() {
  debugger
  
  var url = 'ImsRptMaterialIssueReport/GetImsRptMaterialissuereport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.materialissue_list;

    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
             
    })
    this.NgxSpinnerService.hide();
 
}
onview(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage1 = 'Inventory';
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const lspage = AES.encrypt(lspage1,secretKey).toString();
  this.route.navigate(['/ims/ImsTrnIssueMaterialView',encryptedParam,lspage]);
}
PrintPDF(materialissued_gid: any) {
  debugger;
  const api = 'ImsTrnIssueMaterial/GetmaterialissueRpt';
  this.NgxSpinnerService.show()
  let param = {
    materialissued_gid:materialissued_gid
  } 
  this.service.getparams(api,param).subscribe((result: any) => {
    if(result!=null){
      this.service.filedownload1(result);
    }
    this.NgxSpinnerService.hide()
  });
}
}
