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
  selector: 'app-ims-rpt-stockagereport',
  templateUrl: './ims-rpt-stockagereport.component.html',
  styleUrls: ['./ims-rpt-stockagereport.component.scss']
})
export class ImsRptStockagereportComponent {
  stockreport_list1: any[] = [];
  stockreport_list2: any[] = [];
  stockreport_list3: any[] = [];
  stockreport_list4: any[] = [];
  stockreport_list: any[] = [];

  thirtyavailableqty:string='';
  thirtyunitprice:string='';
  sixtyavailableqty:string='';
  sixtyunitprice:string='';
  nintyavailableqty:string='';
  nintyunitprice:string='';
  onetwentyavailableqty:string='';
  onetwentyprice:string='';
  availableqty:string='';
  unitprice:string='';


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

  ngOnInit(): void {
    debugger;
    this.GetImsRptStockagereport30();
    this.GetImsRptStockagereport60();
    this.GetImsRptStockagereport90();
    this.GetImsRptStockagereport120();
    this.GetImsRptStockagereport();
  
}

// // //// Summary Grid//////
GetImsRptStockagereport30() {
  debugger;
  var url = 'ImsRptStockAgeReport/GetImsRptStockagereport30'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list1 = this.responsedata.stockage_list;

    const thirtyavailableqty = this.roundToTwoDecimal(this.stockreport_list1.reduce((acc, item) => acc + parseFloat(item.stock_quantity.replace(/,/g, '')), 0));
    const thirtyunitprice = this.roundToTwoDecimal(this.stockreport_list1.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));


    this.thirtyavailableqty = this.formatNumber(thirtyavailableqty);
    this.thirtyunitprice = this.formatNumber(thirtyunitprice);
    setTimeout(()=>{  
      $('#stockreport_list1').DataTable();
    }, 1);
    })
    this.NgxSpinnerService.hide();
 
}

GetImsRptStockagereport60() {
  debugger;
  var url = 'ImsRptStockAgeReport/GetImsRptStockagereport60'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list2 = this.responsedata.stockage_list;
    const sixtyavailableqty = this.roundToTwoDecimal(this.stockreport_list2.reduce((acc, item) => acc + parseFloat(item.stock_quantity.replace(/,/g, '')), 0));
    const sixtyunitprice = this.roundToTwoDecimal(this.stockreport_list2.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));


    this.sixtyavailableqty = this.formatNumber(sixtyavailableqty);
    this.sixtyunitprice = this.formatNumber(sixtyunitprice);
    setTimeout(()=>{  
      $('#stockreport_list2').DataTable();
    }, 1);
    })
    this.NgxSpinnerService.hide();
 
}

GetImsRptStockagereport90() {
  debugger;
  var url = 'ImsRptStockAgeReport/GetImsRptStockagereport90'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list3 = this.responsedata.stockage_list;

    const nintyavailableqty = this.roundToTwoDecimal(this.stockreport_list3.reduce((acc, item) => acc + parseFloat(item.stock_quantity.replace(/,/g, '')), 0));
    const nintyunitprice = this.roundToTwoDecimal(this.stockreport_list3.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));


    this.nintyavailableqty = this.formatNumber(nintyavailableqty);
    this.nintyunitprice = this.formatNumber(nintyunitprice);
    setTimeout(()=>{  
      $('#stockreport_list3').DataTable();
    }, 1);
    })
    this.NgxSpinnerService.hide();
 
}

GetImsRptStockagereport120() {
  debugger;
  var url = 'ImsRptStockAgeReport/GetImsRptStockagereport120'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list4 = this.responsedata.stockage_list;

    const onetwentyavailableqty = this.roundToTwoDecimal(this.stockreport_list4.reduce((acc, item) => acc + parseFloat(item.stock_quantity.replace(/,/g, '')), 0));
    const onetwentyprice = this.roundToTwoDecimal(this.stockreport_list4.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));


    this.onetwentyavailableqty = this.formatNumber(onetwentyavailableqty);
    this.onetwentyprice = this.formatNumber(onetwentyprice);
    setTimeout(()=>{  
      $('#stockreport_list4').DataTable();
    }, 1);
    })
    this.NgxSpinnerService.hide();
 
}

GetImsRptStockagereport() {
  debugger;
  var url = 'ImsRptStockAgeReport/GetImsRptStockagereport'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockage_list;

    const availableqty = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.stock_quantity.replace(/,/g, '')), 0));
    const unitprice = this.roundToTwoDecimal(this.stockreport_list.reduce((acc, item) => acc + parseFloat(item.product_price.replace(/,/g, '')), 0));


    this.availableqty = this.formatNumber(availableqty);
    this.unitprice = this.formatNumber(unitprice);
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
