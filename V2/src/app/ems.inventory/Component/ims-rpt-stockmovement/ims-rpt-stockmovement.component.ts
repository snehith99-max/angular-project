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
  selector: 'app-ims-rpt-stockmovement',
  templateUrl: './ims-rpt-stockmovement.component.html',
  styleUrls: ['./ims-rpt-stockmovement.component.scss']
})
export class ImsRptStockmovementComponent {
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
  Getstock_list:any[]=[];
  response_data: any;
  flag2: boolean = false;
  stockchart: any = {};
  grn_count: any;
  delivery_count:any;
  month: any;
  to_date!: string;
  maxDate!:string;
  from_date!: string;
  Getlastsixmonthstock_list:any[]=[];


  constructor(private formBuilder: FormBuilder, public NgxSpinnerService: NgxSpinnerService, public service: SocketService, private route: Router, private ToastrService: ToastrService) {
    this.stockreport = {} as Istockreport;
  }

  ngOnInit(): void {
    debugger
    this.GetImsRptStockreport();
    this.GetStockStatus();
    var url = 'ImsRptStockreport/GetBranch'
    this.service.get(url).subscribe((result: any) => {
      this.branch_list = result.branch_list;  
    });
    var url = 'ImsRptStockreport/Getlastsixmonthstock'
    this.service.get(url).subscribe((result: any) => {
      this.Getlastsixmonthstock_list = result.Getlastsixmonthstock_list;  
    });
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
    var url = 'ImsRptStockreport/GetImsRptStockmovement'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#stockreport_list').DataTable().destroy();
      this.responsedata = result;
      console.log(this.responsedata.stockreport_list);
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
    this.route.navigate(['/ims/ImsRptMovementview', productgid,branchgid]);
  }
  GetStockStatus() {
    debugger
    var url = 'ImsRptStockreport/GetstockStatus';
    this.service.get(url).subscribe((result: any) => {
      this.response_data = result;
      this.Getstock_list = this.response_data.Getstock_list;
      if (this.Getstock_list.length > 0) {
        this.flag2 = true;
      }
      this.grn_count = this.Getstock_list.map((entry: { grn_count: any }) => entry.grn_count)
      this.delivery_count = this.Getstock_list.map((entry: { delivery_count: any }) => entry.delivery_count)
      this.month = this.Getstock_list.map((entry: { month: any }) => entry.month)
      this.stockchart = {
        chart: {
          type: 'bar',
          height: 250,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: true,
          },
        },
        colors: ['#7FC7D9', '#FFD54F'],
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '50%',
            borderRadius: 0,
          },
        },
        dataLabels: {
          enabled: false,
        },
        xaxis: {
          categories: this.month,
          labels: {
            style: {

              fontSize: '12px',
            },
          },
        },
        yaxis: {
          title: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
              color: '#7FC7D9',
            },
          },
        },
        series: [
          {
            name: 'Stock In',
            data: this.grn_count,
            color: '#3D9DD9',
          },
          {
            name: 'Stock Out',
            data: this.delivery_count,
            color: '#9EBF95',
          },
        ],
        legend: {
          position: "top",
          offsetY: 5
        }
      };
    });
  }
  onrefreshclick(){
    window.location.reload();
    this.GetImsRptStockreport();
  }
  OnChangeFinancialYear(){
    debugger;
    this.NgxSpinnerService.show();
    if(this.reactiveform.value.branch_name !=null){
    }
    else{
      this.NgxSpinnerService.hide();
      this.ToastrService.warning("Select the Branch Name..")
      return;
    }
    let param = {
      branch_gid : this.reactiveform.value.branch_name,
    }
    var api = 'ImsRptStockreport/GetImsRptMovementreportsearch';
    this.service.getparams(api,param).subscribe((result: any)=>{
      this.stockreport_list = result.stockreport_list;
      setTimeout(() => {
        $('#stockreport_list').DataTable()
      }, 1);
      this.NgxSpinnerService.hide();
    });    
  }
  onclearbranch(){
    this.mdlBranchName = null;
  }
}
