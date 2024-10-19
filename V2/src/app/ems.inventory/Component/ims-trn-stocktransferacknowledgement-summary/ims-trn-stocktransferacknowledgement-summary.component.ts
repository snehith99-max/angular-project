import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
@Component({
  selector: 'app-ims-trn-stocktransferacknowledgement-summary',
  templateUrl: './ims-trn-stocktransferacknowledgement-summary.component.html',
  styleUrls: ['./ims-trn-stocktransferacknowledgement-summary.component.scss'],
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
export class ImsTrnStocktransferacknowledgementSummaryComponent {
  responsedata:any;
  stockreport_list:any[]=[];
  stock_list:any[]=[];
  StocktransferAckForm!: FormGroup;
  combinedFormData: any;
  constructor(private formBuilder: FormBuilder, private router: Router,private ToastrService: ToastrService, public service: SocketService,public NgxSpinnerService:NgxSpinnerService,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.GetImsRptStocktransferapproval();
    this.StocktransferAckForm = new FormGroup({
      ref_no:new FormControl(''),
      from_date: new FormControl(''),
      to_date: new FormControl(''),
    });
  }

// // //// Summary Grid//////
GetImsRptStocktransferapproval() {
  debugger;
  var url = 'ImsTrnStockTransferSummary/GetImsRptStocktransferacknowlege'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stocktransferapproval_list;
    setTimeout(()=>{  
      $('#stockreport_list').DataTable();
    }, 1);
    this.combinedFormData.get("display_field")?.setValue(this.stockreport_list[0].display_field);         
    })
    debugger;
    var url = 'ImsTrnStockTransferSummary/GetImsRptStocktransferacknowlegelocation'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.stock_list= this.responsedata.stocktransferapproval_list;
    setTimeout(()=>{  
      $('#stock_list').DataTable();
    }, 1);
    this.combinedFormData.get("display_field")?.setValue(this.stock_list[0].display_field);         
    })
    this.NgxSpinnerService.hide();
 
}
OnChangeFinancialYear(){
  debugger;
  this.NgxSpinnerService.show();
  let param = {
    ref_no:this.StocktransferAckForm.value.ref_no,
    from_date : this.StocktransferAckForm.value.from_date,
    to_date : this.StocktransferAckForm.value.to_date
  }
  var api = 'ImsTrnStockTransferSummary/GetStocktransferAckdate';
  this.service.getparams(api,param).subscribe((result: any)=>{
    this.stockreport_list = result.stocktransferapproval_list;
    this.NgxSpinnerService.hide();
  });    
}
onrefreshclick(){
  debugger
  var url = 'ImsRptStockreport/GetImsRptStockstatement'
  this.NgxSpinnerService.show()
  this.service.get(url).subscribe((result: any) => {
    $('#stockreport_list').DataTable().destroy();
    this.responsedata = result;
    this.stockreport_list = this.responsedata.stockreport_list;
    setTimeout(() => {
      $('#stockreport_list').DataTable();
    }, 1);
  })
  this.NgxSpinnerService.hide();
}
ApproavalView(params: any) {
  debugger;
  const secretKey = 'storyboarderp';
  const param = (params);
  const encryptedParam = AES.encrypt(param, secretKey).toString();
  this.router.navigate(['/ims/ImsTrnStockTransferAcknowledgementView', encryptedParam])
}
  }



