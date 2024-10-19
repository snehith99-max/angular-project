import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';



export class salesorderstatus {
  salesorders_list: string[] = [];
  employee_gid: any;
}

@Component({
  selector: 'app-smr-trn-salesorder-summary',
  templateUrl: './smr-trn-salesorder-summary.component.html',
  styleUrls: ['./smr-trn-salesorder-summary.component.scss'],
  // styles: [`
  // table thead th, 
  // .table tbody td { 
  //  position: relative; 
  // z-index: 0;
  // } 
  // .table thead th:last-child, 

  // .table tbody td:last-child { 
  //  position: sticky; 

  // right: 0; 
  //  z-index: 0; 

  // } 
  // .table td:last-child, 

  // .table th:last-child { 

  // padding-right: 50px; 

  // } 
  // .table.table-striped tbody tr:nth-child(odd) td:last-child { 

  //  background-color: #ffffff; 

  //   } 
  //   .table.table-striped tbody tr:nth-child(even) td:last-child { 
  //    background-color: #f2fafd; 

  // } 
  // `]
})
export class SmrTrnSalesorderSummaryComponent {

  reactiveForm!: FormGroup;
  CourierForm!: FormGroup;
  responsedata: any;
  salesorder_list: any[] = [];
  salesproduct_list: any[] = [];
  salesordertype_list: any[] = [];
  getData: any;
  chart: ApexCharts | null = null;
  salesorderlastsixmonths_list: any[] = [];
  ordertoinvoicecount: any;
  ordercount: any;
  chartOptions: any;
  flag: boolean = true;
  boolean: any;
  pick: Array<any> = [];
  parameterValue1: any;
  parameterValue2: any;
  parameterValue3: any;
  salesorder_gid: any;
  CurObj: salesorderstatus = new salesorderstatus();
  selection = new SelectionModel<salesorderstatus>(true, []);
  products: any[] = [];
  GetCourierService_list: any[] = [];
  company_code: any;
  showOptionsDivId: any;
  MdlCourierService: any;

  constructor(private formBuilder: FormBuilder, 
    private NgxSpinnerService: NgxSpinnerService, public route: ActivatedRoute, 
    
    public service: SocketService, private router: Router, private ToastrService: ToastrService) {
  }


  ngOnInit(): void {

    this.CourierForm = new FormGroup({
      warehouse : new FormControl(''),
      courierservice_id : new FormControl(''),
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    var url = 'SmrTrnSalesorder/GetCourierSerive';
    this.service.get(url).subscribe((result:any)=>{
      this.GetCourierService_list = result.GetCourierService_list
    });
    this.GetSmrTrnSalesordersummary();
    this.getsalesordersixmonths();
    this.salesordertypecount();
    this.company_code = localStorage.getItem('c_code');
  }

  PrintPDF(salesorder_gid: string) {
    this.GetSmrTrnSalesordersummary();
  }
  Delivery(salesorder_gid: any) {
    const secretKey = 'storyboarderp';
    const param = (salesorder_gid);
    
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    

    this.router.navigate(['/ims/ImsTrnRaiseDeliveryorder', encryptedParam])
  }
  //// Summary Grid//////
  GetSmrTrnSalesordersummary() {
    var url = 'SmrTrnSalesorder/GetSmrTrnSalesordersummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#salesorder_list').DataTable().destroy();
      this.responsedata = result;
      this.salesorder_list = this.responsedata.salesorder_list;
      this.NgxSpinnerService.hide()
      this.CourierForm.get('warehouse')?.setValue('Main Warehouse');
      setTimeout(() => {
        $('#salesorder_list').DataTable();
      }, 1);


    })

  }
  filedownload(file_path: any, file_name:any) {
    debugger
    if (file_path != null && file_path !="" && file_path != undefined)  {
      let param = { file_path: file_path, file_name: file_name }
      this.service.post('SmrTrnSalesorder/DownloadDocument', param).subscribe((result: any) => {
        if (result.status == true) {
          this.service.filedownload1(result);
        }
      });
      // /this.service.downloadfile(file_path,"sales");
    }
    else {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      this.ToastrService.warning("No file has been uploaded for this order");
    }
  }
  getsalesordersixmonths() {
    var url = 'SmrTrnSalesorder/Getsalesordersixmonthschart'
    this.service.get(url).subscribe((result: any) => {
      this.salesorderlastsixmonths_list = result.salesorderlastsixmonths_list;
      this.ordertoinvoicecount = result.ordertoinvoicecount;
      this.ordercount = result.ordercount;
      if (this.salesorderlastsixmonths_list == null) {
        this.flag = false;
      }
      const categories = this.salesorderlastsixmonths_list.map((entry: { months: any; }) => entry.months);
      const data = this.salesorderlastsixmonths_list.map((entry: { orderamount: any; }) => entry.orderamount);
      // this.chartOptions = {
      //   series: [
      //     {

      //       name: "Order Value",
      //       data: data
      //     }
      //   ],  
      //   chart: {
      //     type: "bar",
      //     height: 220,
      //     zoom: {
      //       enabled: false
      //     },
      //     width: '100%',
      //   background: 'White',
      //   foreColor: '#0F0F0F',
      //   fontFamily: 'inherit',
      //   toolbar: {
      //     show: false,
      //   },
      //   },
      //   dataLabels: {
      //     enabled: false
      //   },
      //   plotOptions: {
      //     bar: {
      //       horizontal: false,
      //       columnWidth: '25%',
      //       borderRadius: 3,
      //     },
      //   },
      //   stroke: {
      //     show: true,
      //     width: 2,
      //     colors: ['transparent'],
      //   },


      //   labels: categories,
      //   xaxis: {
      //     type: "months"
      //   },
      //   yaxis: {
      //     opposite: true,
      //     labels: {
      //       formatter: (value: number) => {
      //         return  value.toLocaleString(); 
      //       },
      //     },
      //   },
      //   legend: {
      //     horizontalAlign: "left"
      //   }
      // };

      this.chartOptions = {
        chart: {
          type: "bar",
          height: 240,
          width: '100%',
          background: 'White',
          foreColor: '#0F0F0F',
          fontFamily: 'inherit',
          toolbar: {
            show: false,
          },
        },
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: '15%',
            borderRadius: 3,
          },
        },
        dataLabels: {
          enabled: false,
        },
        stroke: {
          show: true,
          width: 2,
          colors: ['transparent'],
        },
        xaxis: {
          categories: categories,
          labels: {
            style: {
              fontWeight: 'bold',
              fontSize: '14px',
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
          tickAmount: 8,
          labels: {
            formatter: (value: number) => {
              return value.toLocaleString();
            },
          }
        },
        legend: {
          horizontalAlign: "right"
        },
        series: [
          {
            name: 'Order Value',
            color: '#87CEEB',
            data: data,
          },
        ],
      };
    })
    this.renderChart()
  }
  private renderChart(): void {
    if (this.chart) {
      this.chart.updateOptions(this.chartOptions); // Update existing chart with new options
    } else {
      this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      this.chart.render();
    }
  }
  salesordertypecount() {
    var url = 'SmrTrnSalesorder/GetSmrTrnSalesordercount'
    this.service.get(url).subscribe((result: any) => {
      $('#salesordertype_list').DataTable().destroy();

      this.salesordertype_list = result.salesordertype_list;
      setTimeout(() => {
        $('#salesordertype_list').DataTable();
      }, 1);


    })


  }
  onview(salesorder_gid: any, customer_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const salesordergid = (salesorder_gid);
    const customergid = (customer_gid);
    const lspage1 = "SmrTrnSalesorderview";
    const leadbank_gid1 = "";
    const leadbank_gid = AES.encrypt(leadbank_gid1, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    const encryptedParam = AES.encrypt(salesordergid, secretKey).toString();
    const encryptedParam2 = AES.encrypt(customergid, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesorderviewNew', encryptedParam, encryptedParam2, leadbank_gid, lspage]);
  }
  onhistory(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesOrderHistory', encryptedParam])
  }

  onedit(params: any) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesOrderEdit', encryptedParam])
  }
  approval(salesorder_gid: any) {
    debugger
    const secretKey = 'storyboard';
    const param = salesorder_gid;
    const salesorder_gid1 = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnSalesOrderApproval', salesorder_gid1]);
  }
  add() {
    this.router.navigate(['/smr/SmrTrnRaiseSalesOrderNew'])
  }
  openModaledit() { }
  openModaldelete() { }
  onattach() { }
  openModalshop() { }

  openModalcancel(parameter: string) {
    this.parameterValue1 = parameter
  }

  openModalmakepayment() {

  }

  openModalmakeapprove() {

  }
  onsubmitsalesupdate() {
    this.pick = this.selection.selected
    let list = this.pick
    this.CurObj.salesorders_list = list
    if (this.CurObj.salesorders_list.length != 0) {

      this.NgxSpinnerService.show();
      var url1 = 'SmrTrnSalesorder/Getsalesonupdate'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Upadating Status!')
        }
        else {
          this.GetSmrTrnSalesordersummary();
          this.NgxSpinnerService.hide();
          this.selection.clear();
          this.ToastrService.success('Status Updated Successfully!')

        }

      });
    }
    else {

      this.ToastrService.warning("Error While Upadating Status!")
    }
  }


  oncancel() {
    console.log(this.parameterValue1);
    var url3 = 'SmrTrnSalesorder/getCancelSalesOrder'
    this.service.getid(url3, this.parameterValue1).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Cancelling Order')
        this.GetSmrTrnSalesordersummary();

      }
      else {
        this.ToastrService.success('Order Cancelled Successfully')
        this.GetSmrTrnSalesordersummary();
        window.location.reload();
        this.salesordertypecount();
      }

    });
  }
  openModalupdate(parameter: string) {
    this.parameterValue2 = parameter
  }
  onupdate() {
    console.log(this.parameterValue2);
    var url3 = 'SmrTrnSalesorder/getupdate'
    this.service.getid(url3, this.parameterValue2).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning('Error While Upadating Status!')
        this.GetSmrTrnSalesordersummary();

      }
      else {
        this.ToastrService.success('Status Updated Successfully!')
        this.GetSmrTrnSalesordersummary();
        window.location.reload();
      }

    });
  }

  Details(parameter: string, salesorder_gid: string) {
    this.parameterValue1 = parameter;
    this.salesorder_gid = parameter;

    var url = 'SmrTrnSalesorder/GetSalesProductdetails'
    let param = {
      salesorder_gid: salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesproduct_list = result.salesproduct_list;
    });


  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.salesorder_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.salesorder_list.forEach((row: salesorderstatus) => this.selection.select(row));
  }

  posttomintsoft(data: any) {
    
    this.salesorder_gid = data;

  }
  
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  RaisetoOrder(salesorder_gid: any) {
    debugger
    var url = 'SmrTrnSalesorder/checkinvoice'
    let params={
      salesorder_gid:salesorder_gid
    }
    debugger
    this.service.getparams(url,params).subscribe((result:any)=>{
      debugger
      if(result.status == false){
        window.scrollTo({ top: 0, behavior: 'smooth' });
        this.ToastrService.warning(result.message)
      }
      else{
        const key = 'storyboard';
    const param = salesorder_gid;
    const salesordergid = AES.encrypt(param, key).toString();
    this.router.navigate(['/smr/SmrTrnOrderToInvoice', salesordergid])
      }

    });
    
  }
  onclose(){

  }
  OnUpdatePost(){
  this.NgxSpinnerService.show();
    var url = 'Mintsoft/CreateOrder'
    let param = {
      salesorder_gid: this.salesorder_gid,
      CourierServiceId: this.CourierForm.value.courierservice_id
    }

    this.service.post(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      this.GetSmrTrnSalesordersummary();

    })
  }

  raisepurchaseorder(salesorder_gid: any) {
    const secretKey = 'storyboarderp';
    const param = (salesorder_gid);    
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrTrnRaisePurchaseorder', encryptedParam])
  }

}



