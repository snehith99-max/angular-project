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
  selector: 'app-ims-rpt-stockreport',
  templateUrl: './ims-rpt-stockreport.component.html',
  styleUrls: ['./ims-rpt-stockreport.component.scss']
})
export class ImsRptStockreportComponent {
  stockreport_list: any[] = [];
  responsedata: any;
  getData: any;
  branch_list :any;
  mdlBranchName:any;
  reactiveform: FormGroup | any;
  stockreport: Istockreport;
  combinedFormData: any;
  GetOrderForLastSixMonths_List: any;
  flag: boolean = true;
  chartOptions: any;
  chart: ApexCharts | null = null;
  
  productprice:string='';
  stockvalue:string='';
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService: NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
}
  ngOnInit(): void {debugger
    this.GetImsRptStockreport();
    this.reactiveform = new FormGroup({
      branch_name: new FormControl(''),
    })
    var url = 'ImsRptStockreport/GetBranch'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.branch_list;  
    });
}
GetImsRptStockreport() {
  debugger
  var url = 'ImsRptStockreport/GetImsRptStockstatement'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#stockreport_list').DataTable().destroy();
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockreport_list;
    const productprice = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));
    const stockvalue = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.stock_value.replace(/,/g, '')), 0));
    this.productprice = this.formatNumber(productprice);
    this.stockvalue = this.formatNumber(stockvalue);
    setTimeout(() => {
      $('#stockreport_list').DataTable();
    }, 1);
  })
  this.NgxSpinnerService.hide();
}
onclearbranch(){
  this.mdlBranchName = null;
}
onSearchClick() {
  debugger;
  // const branch_gid =this.mdlBranchName;
  let param = {
    branch_gid:this.reactiveform.value.branch_name
  }
  var url = 'ImsRptStockreport/GetImsRptStockreport'
  this.NgxSpinnerService.show()
  this.service.getparams(url,param).subscribe((result: any) => {
     $('#stockreport_list').DataTable().destroy();
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockreport_list;
    const productprice = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));
    const stockvalue = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.stock_value.replace(/,/g, '')), 0));
    this.productprice = this.formatNumber(productprice);
    this.stockvalue = this.formatNumber(stockvalue);
    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
    this.combinedFormData.get("display_field")?.setValue(this.stockreport_list[0].display_field);     
    })
    this.NgxSpinnerService.hide();
}
onrefreshclick(){
  debugger
  var url = 'ImsRptStockreport/GetImsRptStockstatement'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#stockreport_list').DataTable().destroy();
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockreport_list;
    const productprice = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));
    const stockvalue = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.stock_value.replace(/,/g, '')), 0));
    this.productprice = this.formatNumber(productprice);
    this.stockvalue = this.formatNumber(stockvalue);
    setTimeout(() => {
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
