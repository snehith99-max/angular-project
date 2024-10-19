import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-ims-trn-stocktransfer-summary',
  templateUrl: './ims-trn-stocktransfer-summary.component.html',
  styleUrls: ['./ims-trn-stocktransfer-summary.component.scss'],
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
export class ImsTrnStocktransferSummaryComponent {

  stocktransfersummary: any[] = [];
  renewalsummary_list1: any[] = [];
  stocktransferbranchsummary: any[] = [];


  showOptionsDivId: any;
  paramvalue: any;
  setdaysform!: FormGroup;
  responsedata:any;
  productsummary_list:any;

  constructor(private formBuilder: FormBuilder,
    private ToastrService: ToastrService,
    private router: ActivatedRoute,
    private route: Router,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) { }

  ngOnInit(): void {

    this.setdaysform = new FormGroup({
      setdaysproduct: new FormControl(''),
    })

    var locationapi = 'ImsTrnStockTransferSummary/GetLocationWiseSummary';
    this.service.get(locationapi).subscribe((result: any) => {
      $('#stocktransfersummary').DataTable().destroy();
      this.stocktransfersummary = result.stocktransfersummary;
      
      setTimeout(() => {
        $('#stocktransfersummary').DataTable();
      }, 1);
    });
    var branchapi = 'ImsTrnStockTransferSummary/GetBranchWiseSummary';
    this.service.get(branchapi).subscribe((result: any) => {
      $('#stocktransferbranchsummary').DataTable().destroy();
      this.stocktransferbranchsummary = result.stocktransferbranchsummary;
      
      setTimeout(() => {
        $('#stocktransferbranchsummary').DataTable();
      }, 1);
    });

  }


  toggleOptions(product_gid: any) {
    if (this.showOptionsDivId === product_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = product_gid;
    }
  }
  onadd() {
    this.route.navigate(['/ims/ImsTrnStockTransferBranch'])
  }
  onaddlocation() {
    this.route.navigate(['/ims/ImsTrnStockTransferLocation'])
  }
  branchview(params: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/ims/ImsTrnStockTransferBranchView', encryptedParam])
  }

  locationview(params: any) {
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/ims/ImsTrnStockTransferLocationView', encryptedParam])
  }

  Getproduct(stocktransfer_gid: any){
    debugger;
    let param = {
      stocktransfer_gid:stocktransfer_gid
    } 
    var url = 'ImsTrnStockTransferSummary/GetDetialViewProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.Stockproduct_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }

  Getproductdetials(stocktransfer_gid: any){
    debugger;
    let param = {
      stocktransfer_gid:stocktransfer_gid
    } 
    var url = 'ImsTrnStockTransferSummary/GetDetialProduct'
    this.service.getparams(url,param).subscribe((result: any) => {
      debugger
      $('#productsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.productsummary_list = this.responsedata.Stockdeatails_list;
      setTimeout(() => {
        $('#productsummary_list').DataTable();
      }, 1);
    });
  }
}



