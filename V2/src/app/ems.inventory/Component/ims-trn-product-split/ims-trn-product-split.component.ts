import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators,FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
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
  incoming_quantity:string;
  split_quantity:string;
  display_field : string;
  product_unit_name : string;
  product_uom_gid : string;
}

@Component({
  selector: 'app-ims-trn-product-split',
  templateUrl: './ims-trn-product-split.component.html',
  styleUrls: ['./ims-trn-product-split.component.scss']
})
export class ImsTrnProductSplitComponent {
  Isplit!: Isplit;
  reactiveForm: FormGroup | any;
  response_data: any;
  productsplitsummary : any[]=[];
  product_unit_list: any[] = [];
  product_gid : any;
  responsedata: any;
  mdlunitName: any;

  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private excelService: ExcelService,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService:NgxSpinnerService,) {} 
  
  ngOnInit(): void {
    const product_gid = this.route.snapshot.paramMap.get('product_gid');
  const secretKey = 'storyboarderp';
  this.product_gid = product_gid;
  const product_gid1 = AES.decrypt(this.product_gid, secretKey).toString(enc.Utf8);



    this.GetStockSummary(product_gid1);
   // this.GetOnChangeUnit();
    this.reactiveForm = new FormGroup({
      stock_gid : new FormControl(''),
      product_gid : new FormControl(''),
      reference_gid : new FormControl(''),
      file: new FormControl(''),
      branch_gid: new FormControl(''),
      uom_gid: new FormControl(''),
      stock_balance: new FormControl(''),
      split_qty: new FormControl(''),
      product_name: new FormControl(''),
      product_code: new FormControl(''),
      income_qty: new FormControl(''),
      uom_name: new FormControl(''),
      location_gid: new FormControl(''),
      product_group: new FormControl(''),
      sku: new FormControl(''),
      productuom_name: new FormControl(''),
      incoming_quantity :new FormControl(''),
      split_quantity : new FormControl(''),
      product_unit_name:new FormControl(''),
      display_field : new FormControl(''),
      product_uom_gid : new FormControl(''),

    });

    debugger
    var url = 'ImsTrnProductSplit/GetOnChangeUnit';
  this.service.get(url).subscribe((result: any) => {
    this.responsedata = result;
    this.product_unit_list = this.responsedata.GetLocation;
    this.reactiveForm.get("product_unit_name")?.setValue(result.product_unit_list.productuom_name);
    this.reactiveForm.get("product_uom_gid")?.setValue(result.product_unit_list.product_uom_gid);
     // this.stockform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
  });
  }
  

  GetStockSummary(product_gid1 :any){
    debugger 
    
    var api = 'ImsTrnStockSummary/GetProductSplitSummary';
    
    var params = {
      product_gid : product_gid1
    }
    this.service.getparams(api,params).subscribe((result:any) => {
      this.response_data = result;
      this.productsplitsummary = this.response_data.stocksummary;
      this.reactiveForm.get("stock_gid")?.setValue(this.productsplitsummary[0].stock_gid);
      this.reactiveForm.get("branch_gid")?.setValue(this.productsplitsummary[0].branch_gid);
      this.reactiveForm.get("product_code")?.setValue(this.productsplitsummary[0].product_code);
      this.reactiveForm.get("product_group")?.setValue(this.productsplitsummary[0].productgroup_name);
      this.reactiveForm.get("product_name")?.setValue(this.productsplitsummary[0].product_name);
      this.reactiveForm.get("sku")?.setValue(this.productsplitsummary[0].sku);
      this.reactiveForm.get("stock_balance")?.setValue(this.productsplitsummary[0].stock_balance);
      this.reactiveForm.get("productuom_name")?.setValue(this.productsplitsummary[0].productuom_name);
      this.reactiveForm.get("reference_gid")?.setValue(this.productsplitsummary[0].reference_gid);
      this.reactiveForm.get("product_gid")?.setValue(this.productsplitsummary[0].product_gid);
      this.reactiveForm.get("display_field")?.setValue(this.productsplitsummary[0].display_field);
      this.reactiveForm.get("uom_gid")?.setValue(this.productsplitsummary[0].uom_gid);
      this.reactiveForm.get("product_unit_name")?.setValue(this.productsplitsummary[0].product_unit_name)
    });
}

// GetOnChangeUnit() {
//   debugger
  
//   let branch_gid = this.reactiveForm.value.branch_name.branch_gid;
//   let param = {
//     branch_gid: branch_gid
//   }

//   debugger
//   var url = 'ImsTrnProductSplit/GetOnChangeUnit';
//   this.service.getparams(url, param).subscribe((result: any) => {
//     this.responsedata = result;
//     this.product_unit_list = this.responsedata.GetLocation;
//     this.reactiveForm.get("product_unit_name")?.setValue(result.location_list[0].product_unit_name);
//     this.reactiveForm.get("product_uom_gid")?.setValue(result.location_list[0].product_uom_gid);
//      // this.stockform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
//   });
// }


get product_unit_name() {

  return this.reactiveForm.get('product_unit_name')!;

}

onClearUom(){
  this.mdlunitName = '';
}


validate_split(){
  debugger
 
      var params = {    
        product_gid:this.reactiveForm.value.product_gid,
        product_group:this.reactiveForm.value.product_group,
        product_name:this.reactiveForm.value.product_name,
        productgroup_name:this.reactiveForm.value.productgroup_name,
        product_code:this.reactiveForm.value.product_code,
        productuom_name:this.reactiveForm.value.productuom_name,
        sku:this.reactiveForm.value.sku,
        stock_balance:this.reactiveForm.value.stock_balance,
        incoming_quantity:this.reactiveForm.value.incoming_quantity,
        split_quantity:this.reactiveForm.value.split_quantity,
        branch_gid:this.reactiveForm.value.branch_gid,
        product_unit_text:this.reactiveForm.value.product_unit_text,
        stock_gid : this.reactiveForm.value.stock_gid,
        reference_gid : this.reactiveForm.value.reference_gid,
        display_field : this.reactiveForm.value.display_field,
        uom_gid : this.reactiveForm.value.product_unit_name.product_uom_gid
       }
       console.log(params)
        var url3 = 'ImsTrnProductSplit/GetProductSplit';
        this.NgxSpinnerService.show();
        this.service.post(url3,params).subscribe((result: any) => {
          if (result.status == false) {
            this.NgxSpinnerService.hide()
            this.ToastrService.warning(result.message)
            
          }
          else {
            this.router.navigate(['/ims/ImsTrnProductSplit']);
            this.NgxSpinnerService.hide()
            this.ToastrService.success(result.message)
            this.router.navigate(["/ims/ImsTrnStocksummary"]);
            this.reactiveForm.reset();
          }
          this.responsedata = result;
        });
      
    
    
    
  } 
}
