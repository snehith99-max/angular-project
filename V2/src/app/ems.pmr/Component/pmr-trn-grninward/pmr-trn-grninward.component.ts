import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { environment } from '../../../../environments/environment.development';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormControl, FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-pmr-trn-grninward',
  templateUrl: './pmr-trn-grninward.component.html',
  styles: [`
    table thead th, 
    .table tbody td { 
     position: relative; 
    z-index: 0;
    } 
    .table thead th:last-child, 
    
    .table tbody td:last-child { 
     position: sticky; 
    
    right: 0; 
     z-index: 0; 
    
    } 
    .table td:last-child, 
    
    .table th:last-child { 
    
    padding-right: 50px; 
    
    } 
    .table.table-striped tbody tr:nth-child(odd) td:last-child { 
    
     background-color: #ffffff; 
      
      } 
      .table.table-striped tbody tr:nth-child(even) td:last-child { 
       background-color: #f2fafd; 
    
    } 
    `]
})
export class PmrTrnGrninwardComponent {

  ASNForm!: FormGroup;

  GrnInward_lists: any[] = [];
  GetASN_list: any[] = [];
  responsedata: any;
  MdlASN: any;
  company_code: any;
  showOptionsDivId: any;
  grn_gid: any;
  rows: any[] = [];
  grnproduct_lists:any;
  chart: ApexCharts | null = null;
  chartOptions: any;
  flag: boolean = true;
  salesorderlastsixmonths_list:any[]=[];
  ordertoinvoicecount:any;
  ordercount:any;

  constructor(public service: SocketService, private route: Router, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {

    this.ASNForm = new FormGroup({
      warehouse: new FormControl(''),
      goodsintypes_id: new FormControl(''),
      supplier_name: new FormControl(''),
    });

    var url = 'PmrTrnGrnInward/GetGoodInTypesMintSoft';
    this.service.get(url).subscribe((result: any) => {
      this.GetASN_list = result.GetGoodInType_list;
    })


    this.GetGrnInwardSummary();
   this.getpurchaseordersixmonths();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetGrnInwardSummary() {
    debugger;
    var url = 'PmrTrnGrnInward/GetGrnInwardSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#GrnInward_lists').DataTable().destroy();
      this.responsedata = result;
      this.GrnInward_lists = this.responsedata.GetGrnInward_lists;
      setTimeout(() => {
        $('#GrnInward_lists').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()
    });

  }
  getpurchaseordersixmonths(){
    var url = 'PmrTrnGrnInward/GetGRNsixmonthschart'
    this.service.get(url).subscribe((result:any)=>{
      this.salesorderlastsixmonths_list = result.GRNlastsixmonths_list;
      this.ordertoinvoicecount = result.ordertogrncount;
      this.ordercount = result.ordercount;
      if (this.salesorderlastsixmonths_list == null) {
        this.flag = false;
      }
      const categories = this.salesorderlastsixmonths_list.map((entry: { months: any; }) => entry.months);
        const data = this.salesorderlastsixmonths_list.map((entry: { ordercount: any; }) => entry.ordercount);
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
                      return  value.toLocaleString(); 
                    },
          }
        },
          legend: {
               horizontalAlign: "right"
             },
          series: [
            {
              name: 'GRN Count',
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
      this.chart.updateOptions(this.chartOptions); 
    } else {
      this.chart = new ApexCharts(document.getElementById('chart'), this.chartOptions);
      this.chart.render();
    }
  }
  onview(params: any) {

    const secretKey = 'storyboarderp';
    const param = (params);
    console.log(param)
    const lspage1 = 'Inv';
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    const lspage = AES.encrypt(lspage1, secretKey).toString();
    this.route.navigate(['./pmr/PmrTrnGrninwardView', encryptedParam, lspage])
  }

  addpurchasereturn(grn_gid: any, vendor_gid: any) {
    const key = 'storyboard';
    const param = grn_gid;
    const param1 = vendor_gid;
    const lspage1 = 'GRN';
    const grngid = AES.encrypt(param,key).toString();
    const vendorgid = AES.encrypt(param1,key).toString();
    const lspage = AES.encrypt(lspage1, key).toString();
    this.route.navigate(['/ims/ImsTrnPurchaseReturnAdd', grngid, vendorgid,lspage])
  }


  PrintPDF(grn_gid: any) {
    debugger
    let param = { grn_gid: grn_gid }
    this.NgxSpinnerService.show();
    var PDFapi = 'PmrTrnGrn/GetGRNPDF';
    this.service.getparams(PDFapi, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
      this.NgxSpinnerService.hide();
    });
  }
  filedownload(file_path: any, file_name:any) {
    debugger
    if (file_path != null && file_path !="") {
      let param = { file_path: file_path, file_name: file_name }
      this.service.post('PmrTrnGrnInward/DownloadDocument', param).subscribe((result: any) => {
        if (result.status == true) {
          this.service.filedownload1(result);
        }
      });
      // /this.service.downloadfile(file_path,"sales");
    }
    else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning("No file has been uploaded for this order");
    }
  }


  onadd() {
    this.route.navigate(['./pmr/PmrTrnGrninwardadd'])
  }
  splitIntoLines(text: string, lineLength: number): string[] {
    const lines = [];
    for (let i = 0; i < text.length; i += lineLength) {
      lines.push(text.substr(i, lineLength));
    }
    return lines;
  }
  posttomintsoft(grn_gid: any) {
    debugger
    const supplier = this.GrnInward_lists.find((supplier: { grn_gid: any, vendor_companyname: string }) => supplier.grn_gid === grn_gid);

    if (supplier) {
      const supplier_name = supplier.vendor_companyname;
      this.ASNForm.get('supplier_name')?.setValue(supplier_name);
      this.grn_gid = grn_gid;
    }
  }
  OnUpdatePost() {
    this.NgxSpinnerService.show();
    var url = 'PmrTrnGrnInward/CreateASN';
    let param =
    {
      grn_gid: this.grn_gid,
      goodsintypes_id: this.ASNForm.value.goodsintypes_id,
      supplier: this.ASNForm.value.supplier_name,
    };
    this.service.post(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      } else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      this.GetGrnInwardSummary();
    });
    this.GetGrnInwardSummary();
  }
  onclose() {

  }

  Getproduct(grn_gid: any){
    let param = {
      grn_gid:grn_gid
    } 
    var url = 'PmrTrnGrn/GetGRNViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#grnproduct_lists').DataTable().destroy();
      this.responsedata = result;
      this.grnproduct_lists = this.responsedata.grnproduct_lists;
      setTimeout(() => {
        $('#grnproduct_lists').DataTable();
      }, 1);
    });
  }
}
