import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators,FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
interface Isplit {
  split_qty:string;
  branch_name:string;
  productgroup_name:string;
  product_code: string;
  product_name: string;
  income_qty: string;
  productuom_name: string;
  stock_balance: string;
  uom_name: string;
  stock_gid: string;
  product_gid: string;
  branch_gid: string;
  uom_gid:string;
}

@Component({
  selector: 'app-ims-trn-stocksummary',
  templateUrl: './ims-trn-stocksummary.component.html',
  styleUrls: ['./ims-trn-stocksummary.component.scss'],
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
export class ImsTrnStocksummaryComponent {
  Isplit!: Isplit;

  private unsubscribe: Subscription[] = [];
  file!:File;
  reactiveForm:any;
  responsedata: any;
  parameterValue: any;
  fileInputs: any;
  stocksummary: any[] = [];
  response_data :any;
  parameterValue1: any;
  mdllocationName:any;
  productuom_list:any;
  showOptionsDivId: any; 
  branch_name1:any;
  finyear:any;
  Branchdtl_list:any;
  FinancialYear_List:any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private excelService: ExcelService,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService,) {} 
  
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  ngOnInit(): void {
    this.GetStockSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      branch_gid: new FormControl(''),
      uom_gid: new FormControl(''),
      product_gid: new FormControl(''),
      stock_balance: new FormControl(''),
      split_qty: new FormControl('',Validators.required),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      income_qty: new FormControl('',Validators.required),
      uom_name: new FormControl(''),
      stock_gid: new FormControl(''),
      location_gid: new FormControl(''),
     
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  GetStockSummary(){


    var url = 'ImsTrnOpeningStock/GetBranchDetails'
    this.service.get(url).subscribe((result: any) => {
      this.Branchdtl_list = result.branchdtl_lists;
      this.branch_name1 = this.Branchdtl_list[0].branch_gid;

      var url = 'ImsTrnStockSummary/GetFinancialYear'
      this.service.get(url).subscribe((result: any) => {
        this.FinancialYear_List = result.GetFinancialYear;
        this.finyear = this.FinancialYear_List[0].finyear;

        if(this.FinancialYear_List[0].finyear!=null){

          this.finyear=this.FinancialYear_List[0].finyear;

        }
        else{
          this.finyear=null;
        }
        let params = {
          branch_gid: this.Branchdtl_list[0].branch_gid,
          finyear: this.finyear
        }
 
    var api = 'ImsTrnStockSummary/GetStockSummary';
    this.NgxSpinnerService.show();
    this.service.getparams(api,params).subscribe((result:any) => {
      this.response_data = result;
      this.stocksummary = this.response_data.stocksummary;
      setTimeout(()=>{  
        $('#stocksummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  });
});
}
  
    onamend(product_gid:any,uom_gid:any,branch_gid:any,stock_gid:any){
      debugger
      const secretKey = 'storyboarderp';
      const param1 = (product_gid);
      const param2 = (uom_gid);
      const param3 = (branch_gid);
      const param4 = (stock_gid);

      const product_gid1 = AES.encrypt(param1,secretKey).toString();
      const uom_gid2 = AES.encrypt(param2,secretKey).toString();
      const branch_gid3= AES.encrypt(param3,secretKey).toString();
      const stock_gid4= AES.encrypt(param4,secretKey).toString();
      this.router.navigate(['/ims/ImsTrnStockamend', product_gid1,uom_gid2,branch_gid3,stock_gid4])  
    }
   
    ondamage(product_gid:any,uom_gid:any,branch_gid:any,stock_gid:any){
      debugger
      const secretKey = 'storyboarderp';
      const param1 = (product_gid);
      const param2 = (uom_gid);
      const param3 = (branch_gid);
      const param4 = (stock_gid);

      const product_gid1 = AES.encrypt(param1,secretKey).toString();
      const uom_gid2 = AES.encrypt(param2,secretKey).toString();
      const branch_gid3= AES.encrypt(param3,secretKey).toString();
      const stock_gid4= AES.encrypt(param4,secretKey).toString();
      this.router.navigate(['/ims/ImsTrnStockdamage', product_gid1,uom_gid2,branch_gid3,stock_gid4])  
    }
    get currency_codeedit() {
      return this.reactiveForm.get('currency_codeedit')!;
    }
    get exchange_rateedit() {
      return this.reactiveForm.get('exchange_rateedit')!;
    }

    onproductsplit(product_gid:any){
      debugger
      const secretKey = 'storyboarderp';
      const param1 = (product_gid);
     

      const product_gid1 = AES.encrypt(param1,secretKey).toString();
     
      this.router.navigate(['/ims/ImsTrnProductSplit', product_gid1])  
    }
    get split_qty() {

      return this.reactiveForm.get('split_qty')!;
  
    }

    get income_qty() {

      return this.reactiveForm.get('income_qty')!;
  
    }
  
  onadd()
  {
        this.router.navigate(['/ims/ImsTrnAddstock'])

  }
  

  initForm() {
    this.reactiveForm = this.fb.group({
      split_qty: [ this.reactiveForm.split_qty,  Validators.compose([

          Validators.required,
          ]),
      ],
      income_qty: [ this.reactiveForm.income_qty,  Validators.compose([

        Validators.required,
        ]),
    ]
    });
  }
  
  onclose() {
    window.location.reload();

  }
  

  openModaledit(parameter: string) {
    debugger;
    this.parameterValue1 = parameter
    this.reactiveForm.get("product_code")?.setValue(this.parameterValue1.product_code);
    this.reactiveForm.get("product_name")?.setValue(this.parameterValue1.product_name);
    this.reactiveForm.get("stock_balance")?.setValue(this.parameterValue1.stock_balance);
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue1.product_gid);
    this.reactiveForm.get("uom_gid")?.setValue(this.parameterValue1.uom_gid);
    this.reactiveForm.get("uom_name")?.setValue(this.parameterValue1.productuom_name);
    this.reactiveForm.get("branch_gid")?.setValue(this.parameterValue1.branch_gid);
    this.reactiveForm.get("stock_gid")?.setValue(this.parameterValue1.stock_gid);
    this.reactiveForm.get("location_gid")?.setValue(this.parameterValue1.location_gid);

  

  let product_gid = this.reactiveForm.value.product_gid;
 
  var url = 'ImsTrnStockSummary/GetproductUomName';
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.productuom_list = result.GetUomList;
    this.reactiveForm.get("uom_gid")?.setValue(this.productuom_list[0].productuom_gid);
    this.reactiveForm.get("uom_name")?.setValue(this.productuom_list[0].productuom_name);
    
 
    
  });
}

    public onsubmit(): void {
      debugger
    this.Isplit = this.reactiveForm.value;
  
    if (this.Isplit.split_qty != null    ) {
         
        var params = {    
          split_qty:this.reactiveForm.value.split_qty,
          product_gid:this.reactiveForm.value.product_gid,
          branch_name:this.reactiveForm.value.branch_name,
          productgroup_name:this.reactiveForm.value.productgroup_name,
          product_code:this.reactiveForm.value.product_code,
          productuom_name:this.reactiveForm.value.productuom_name,
          income_qty:this.reactiveForm.value.income_qty,
          stock_balance:this.reactiveForm.value.stock_balance,
          location_gid:this.reactiveForm.value.location_gid,
          stock_gid:this.reactiveForm.value.stock_gid,
          uom_gid:this.reactiveForm.value.uom_gid,
          branch_gid:this.reactiveForm.value.branch_gid,
         }
        var url2 = 'ImsTrnStockSummary/PostSplitstock'
        this.NgxSpinnerService.show()
     this.service.post(url2,params).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide()
            this.ToastrService.warning(result.message)
            
          }
          else {
            this.router.navigate(['/ims/ImsTrnStocksummary']);
            this.NgxSpinnerService.hide()
            this.ToastrService.success(result.message)
            this.reactiveForm.reset();
            this.GetStockSummary();
            
          }
          this.responsedata = result;
        });
      }
    
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      this.NgxSpinnerService.hide()
    }
    
    return;
    
    
  
  
   } 
   OnChangeFinancialYear(){

    let params = {
      branch_gid: this.branch_name1,
      finyear: this.finyear
    }

    var api = 'ImsTrnStockSummary/GetStockSummary';
    this.NgxSpinnerService.show();
    this.service.getparams(api,params).subscribe((result:any) => {
      this.response_data = result;
      this.stocksummary = this.response_data.stocksummary;
      setTimeout(()=>{  
        $('#stocksummary').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });

   }
 
}
 

