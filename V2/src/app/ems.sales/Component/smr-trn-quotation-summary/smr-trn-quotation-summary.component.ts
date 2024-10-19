import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators,} from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-smr-trn-quotation-summary',
  templateUrl: './smr-trn-quotation-summary.component.html',
  styleUrls: ['./smr-trn-quotation-summary.component.scss'],
  styles: []
})
export class SmrTrnQuotationSummaryComponent {

  quoteform:FormControl | any;
  responsedata: any;
  chart: ApexCharts | null = null;
  quotation_list: any[] = [];
  quotationlastsixmonths_list:any[]=[];
  product_list: any[] = [];
  quotrefnolist:any[] = [];
  getData: any;
  amendcount:any;
  approvecount:any;
  company_code: any;
  chartOptions: any;
  showOptionsDivId: any;
  parameterValue1: any;
  parameterValue: any;
  remarks: any;
  flag: boolean = true;
  quotation_gid: any;
  constructor(private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService,public service :SocketService,private route:Router,private ToastrService: ToastrService) {

    this.quoteform = new FormGroup ({
      quotation_date: new FormControl(''),
      customer_name: new FormControl(''),
      quotation_referenceno1: new FormControl(''),
      quotation_gid: new FormControl(''),
      
      
      
      
    });
    

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrTrnQuotation();
    this.getquotationsixmonths();
}

		  PrintPDF(quotation_gid: any) {
    // API endpoint URL
    const api = 'SmrTrnQuotation/GetQuotationRpt';
    this.NgxSpinnerService.show()
    let param = {
      quotation_gid:quotation_gid
    } 
    this.service.getparams(api,param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
 


//// Summary Grid//////
GetSmrTrnQuotation() {
  debugger
  var url = 'SmrTrnQuotation/GetSmrTrnQuotation'
  this.service.get(url).subscribe((result: any) => {
    $('#quotation_list').DataTable().destroy();
    this.responsedata = result;
    this.quotation_list = this.responsedata.quotation_list;
    setTimeout(() => {
      $('#quotation_list').DataTable();
    }, 1);


  })
  
  
}
getquotationsixmonths(){
  var url = 'SmrTrnQuotation/Getquotationsixmonthschart'
  this.service.get(url).subscribe((result:any)=>{
    this.quotationlastsixmonths_list = result.quotationlastsixmonths_list;
    this.amendcount = result.amendedcount;
    this.approvecount = result.approvecount;
    if (this.quotationlastsixmonths_list == null) {
      this.flag = false;
    }
    const categories = this.quotationlastsixmonths_list.map((entry: { months: any; }) => entry.months);
      const data = this.quotationlastsixmonths_list.map((entry: { quotationamount: any; }) => entry.quotationamount);
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
        // legend: {
        //      horizontalAlign: "left"
        //    },
        series: [
          {
            name: 'Quotation Value',
            color: '#87CEEB',
            data: data,
          },
        ],
      };
    
      // this.chartOptions = {
      //   chart: {
      //     type: "area",
      //     height: 300,
      //     width: '100%',
      //     background: 'White',
      //     foreColor: '#0F0F0F',
      //     fontFamily: 'inherit',
      //     toolbar: {
      //       show: false,
      //     },
      //   },
      //   plotOptions: {
      //     bar: {
      //       horizontal: false,
      //       columnWidth: '25%',
      //       borderRadius: 3,
      //     },
      //   },
      //   dataLabels: {
      //     enabled: false,
      //   },
      //   stroke: {
      //     show: true,
      //     width: 2,
      //     colors: ['transparent'],
      //   },
      //   xaxis: {
      //     categories: categories,
      //     labels: {
      //       style: {
      //         fontWeight: 'bold',
      //         fontSize: '14px',
      //       },
      //     },
      //   },
      //   yaxis: {
      //     title: {
      //       style: {
      //         fontWeight: 'bold',
      //         fontSize: '14px',
      //         color: '#FF0000',
      //       },
      //     },
      //     tickAmount: 8,
      //   },
      //   series: [
      //     {
      //       name: 'Sales Amount',
      //       color: '#9b98b8',
      //       data: data,
      //     },
      //   ],
      // };
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

GetProductdetails() {
  debugger
  var url = 'SmrTrnQuotation/GetProductdetails'
  this.service.get(url).subscribe((result: any) => {
    $('#product_list').DataTable().destroy();
    this.responsedata = result;
    this.product_list = this.responsedata.product_list;
    setTimeout(() => {
      $('#product_list').DataTable();
    }, 1);


  })
}


Mail(params : string)
  {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/smr/SmrTrnQuotationmail',encryptedParam])
  }
  
  onedit(quotation_gid:any,customer_gid:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const lspage = 'SmrTrnQuoteeditnew'
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
    this.route.navigate(['/smr/SmrTrnQuoteeditnew',encryptedParam,encryptedParam2,lspage]) 
  }
  onview(quotation_gid:any,customer_gid:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const lspage = 'SrmTrnNewquotationview'
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
    this.route.navigate(['/smr/SmrTrnquotationviewNew',encryptedParam,encryptedParam2,lspage]) 
  }
  
  openModalamend(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.route.navigate(['/smr/SmrTrnAmendQuotation',encryptedParam]) 
  }
onaddinfo(){}
onadd(params: any){
  const secretKey = 'storyboarderp';

  const param = (params);

  const encryptedParam = AES.encrypt(param,secretKey).toString() ;
  this.route.navigate(['/smr/SmrTrnQuoteToOrder',encryptedParam]);
}
Details(parameter: string,quotation_gid: string){
  this.parameterValue1 = parameter;
  this.quotation_gid = parameter;

  var url='SmrTrnQuotation/GetProductdetails'
    let param = {
      quotation_gid : quotation_gid 
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.responsedata=result;
    this.product_list = result.product_list;   
    });
  
}

get quotation_referenceno1() {
  return this.quoteform.get('quotation_referenceno1')!;
}
///Change Quotation Reference No///
GetQuoteRefNodetails(){
  debugger
  var url ='SmrTrnQuotation/GetQuotRefNodetails'
  this.service.get(url).subscribe((result: any) => {
    $('#quotrefnolist').DataTable().destroy();
    this.responsedata = result;
    this.quotrefnolist = this.responsedata.quotrefnolist;
    setTimeout(() => {
      $('#quotrefnolist').DataTable();
    }, 1);

});
}


QuotRefNopopup(){
  

  // var url='SmrTrnQuotation/GetQuotRefNodetails'
  // let param = {
  //   quotation_gid:quotation_gid
  // }
  // this.service.getparams(url,param).subscribe((result:any)=>{
  //   this.responsedata=result;
  //   this.quotrefnolist =result.quotrefnolist
  //   this.quoteform.get("quotation_referenceno1")?.setValue(this.quotrefnolist[0].quotation_referenceno1);
  //   // this.quoteform.get("quotation_gid")?.setValue(this.quotrefno_list[0].quotation_gid);  
  // });

}

onhistory(quotation_gid:any,customer_gid:any){
  const secretKey = 'storyboarderp';
    const param = (quotation_gid);
    const param2 = (customer_gid);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnQuotationHistory',encryptedParam,encryptedParam2]) 
}


openModaldelete(param: string){
this.parameterValue= param
}

onupdate(){
// debugger
//   var params = {
//     quotation_referenceno1:this.quoteform.value.quotation_referenceno1,
//     quotation_gid:this.quoteform.value.quotation_gid,
//     customer_name:this.quoteform.value.customer_name,
//     quotation_date:this.quoteform.value.quotation_date

//   }
//       var url = 'SmrTrnQuotation/PostUpdatedQuotationRefno'

//       this.service.postparams(url,params).subscribe((result:any)=>{
//         this.responsedata=result;
//         this.quotrefnolist =result.quotrefnolist
//         this.quoteform.get("quotation_referenceno1")?.setValue(this.quotrefnolist[0].quotation_referenceno1);

//         if(result.status ==false){
//           this.ToastrService.warning(result.message)
          
//         }
//         else{
//           this.ToastrService.success(result.message)
         
//         }
//     }); 

  }
  

  onclose(){}
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}









