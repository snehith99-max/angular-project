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
  selector: 'app-ims-trn-stockconsumptionreport',
  templateUrl: './ims-trn-stockconsumptionreport.component.html',
  styleUrls: ['./ims-trn-stockconsumptionreport.component.scss']
})
export class ImsTrnStockconsumptionreportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  Quantity:string='';
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
    
}

  ngOnInit(): void {
    debugger;
    this.GetImsRptStockconsumptionreport();
  
}

// // //// Summary Grid//////
GetImsRptStockconsumptionreport() {
  debugger;
  var url = 'ImsTrnStockConsumptionReport/GetImsRptStockconsumptionreport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockconsumptionreport_list;
    const Quantity = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.quantity.replace(/,/g, '')), 0));

    this.Quantity = this.formatNumber(Quantity);
    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
    })
    this.NgxSpinnerService.hide();
 
}
formatNumber(value: number): string {
  return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
roundToTwoDecimal(value: number): number {
  return Math.round(value * 100) / 100;
}
}
