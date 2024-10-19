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
  selector: 'app-ims-rpt-stocktransfer-report',
  templateUrl: './ims-rpt-stocktransfer-report.component.html',
  styleUrls: ['./ims-rpt-stocktransfer-report.component.scss']
})
export class ImsRptStocktransferReportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;

  transferqty:string='';
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {
    debugger;
    this.GetImsRptStocktransferreport();
  
}

// // //// Summary Grid//////
GetImsRptStocktransferreport() {
  debugger;
  var url = 'ImsTrnStockTransferSummary/GetImsRptStocktransferreport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stocktransferreport_list;

    const transferqty = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.Transfer_qty.replace(/,/g, '')), 0));
  


    this.transferqty = this.formatNumber(transferqty);
    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
    this.combinedFormData.get("display_field")?.setValue(this.stockreport_list[0].display_field);
    
             
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
