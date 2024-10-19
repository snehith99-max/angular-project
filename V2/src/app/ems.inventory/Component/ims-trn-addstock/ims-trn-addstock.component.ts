import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface addstock{
  branch_name: string;
  branch_gid: string;
  productgroup_gid: string;
  productgroup_name: string;
  product_code: string;
  product_gid: string;
  product_name: string;
  cost_price: string;
  product_desc: string;
  stock_qty: string;
  uom_gid: string;
  location_gid: string;
  location_name: string;
  productuom_name: string;
  productuom_gid: string;
}

@Component({
  selector: 'app-ims-trn-addstock',
  templateUrl: './ims-trn-addstock.component.html',
  styleUrls: ['./ims-trn-addstock.component.scss']
})
export class ImsTrnAddstockComponent {
  file!: File;
  producttype_list: any[] = [];
  location_list: any[] = [];
  productgroup_list: any[] = [];
  branch_list : any [] =[];
  GetLocation :any[]= [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  product_list: any[] = [];
  productcode_list: any[] = [];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlPrdType:any;
  mdlPrdUnitC:any;
  mdlPrdUnit:any;
  txtproductgroup:any;
  txtproductcode:any;
  txtproductunit:any;
  mdlPrdName:any;
  mdllocationName:any;
  productsCode:any;
  responsedata: any;
  submitted = false;
  txtprodcutgroup :any;
  financialYearList:any []=[];
  result: any;
  stockform: FormGroup | any;
  addstock !:addstock;
  fb: any;
  mdlProductName:any;
  mdlBranchName :any;
  mdlFinancialYear:any;
 uom_gid:any;
 MdlProductdesc:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) {
    this.addstock = {} as addstock;
  
    this.stockform = new FormGroup({
      branch_name: new FormControl('', Validators.required),
      finyear:new FormControl('',Validators.required),
      branch_gid: new FormControl(''),
      product_gid :new FormControl(''),
      productgroup_name:new FormControl(''),
      productuomclass_name:new FormControl('',),
      productuom_name:new FormControl(''),
      productuom_gid:new FormControl(''),
      product_code:new FormControl(''),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl(''),
      location_name :new FormControl(''),
      location_gid :new FormControl(''),
      stock_qty:new FormControl('',Validators.required),
      cost_price:new FormControl('',Validators.required),
      

    });
  
  }
  ngOnInit(): void {

    var api = 'ImsTrnStockSummary/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup;
    
    });
    var api = 'ImsTrnStockSummary/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });

    var api = 'ImsTrnStockSummary/GetProductUnitclass';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitclass_list = this.responsedata.GetProductUnitclass;

    });
          
   var url = 'ImsTrnStockSummary/GetProductNamDtl'
   this.service.get(url).subscribe((result:any)=>{
     this.product_list = result.GetProductNamDtl;
    });

    
    var url = 'ImsTrnStockSummary/GetBranchDtl'
    this.service.get(url).subscribe((result:any)=>{
      this.branch_list = result.GetBranchDtl;
      this.stockform.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

     });
     debugger;
     var url = 'ImsTrnStockSummary/GetFinancialYear'
     this.service.get(url).subscribe((result:any)=>{
       this.financialYearList = result.GetFinancialYear;
       //this.productform.get("finyear")?.setValue(result.GetFinancialYear[0].finyear);
       console.log(this.financialYearList)
      });
    var api = 'ImsTrnStockSummary/GetImsTrnOpeningstockAdd';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productcode_list = this.responsedata.stockadd_list;

    });
  }

  get branch_name() {

    return this.stockform.get('branch_name')!;

  };
  get productgroup_name() {

    return this.stockform.get('productgroup_name')!;

  };
  get productuomclass_name() {

    return this.stockform.get('productuomclass_name')!;

  };
  get productuom_name() {

    return this.stockform.get('productuom_name')!;

  };
  get productuom_gid() {

    return this.stockform.get('productuom_gid')!;

  };
  get product_code() {

    return this.stockform.get('product_code')!;

  };
  get stock_qty() {

    return this.stockform.get('stock_qty')!;

  };
  get product_name() {

    return this.stockform.get('product_name')!;

  }
  get location_name() {

    return this.stockform.get('location_name')!;

  }
  get finyear() {

    return this.stockform.get('finyear')!;

  }
  get product_desc() {

    return this.stockform.get('product_desc')!;

  }
  get cost_price() {

    return this.stockform.get('cost_price')!;

  }




  initForm() {
    this.stockform = this.fb.group({
      branch_name: [ this.stockform.branch_name,  Validators.compose([

          Validators.required,
          ]),
      ],
      location_name: [
        this.stockform.location_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      productgroup_name: [
        this.stockform.productgroup_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      finyear: [
        this.stockform.finyear,
        Validators.compose([
          Validators.required

        ]),
      ],
      productuomclass_name: [
        this.stockform.productuomclass_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      productuom_name: [
        this.stockform.productuom_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_code: [
        this.stockform.product_code,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_name: [
        this.stockform.product_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_desc: [
        this.stockform.product_desc,
        Validators.compose([
        Validators.required,

        ]),
      ],

      stock_qty: [
        this.stockform.stock_qty,
        Validators.compose([
        Validators.required,

        ]),
      ],
      

      cost_price: [
        this.stockform.cost_price,
        Validators.compose([
          Validators.required

        ]),
      ],
    });
  
  }
 

redirecttolist(){
  this.router.navigate(['/ims/ImsTrnStocksummary']);

}
GetOnChangeProductgroupName (){
  let productgroup_gid = this.stockform.value.productgroup_name.productgroup_gid;
  let param = {
    productgroup_gid: productgroup_gid
  }
  var url = 'ImsTrnOpeningStock/GetonchangeProductNamDtl'
   this.service.getparams(url,param).subscribe((result:any)=>{
     this.product_list = result.GetProduct_name;
    });
}

GetOnChangeProductsName(){
  
  let product_gid = this.stockform.value.product_name.product_gid;
  let param = {
    product_gid: product_gid
    
  }
  var url = 'ImsTrnStockSummary/GetOnChangeproductName';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.productsCode = result.ProductsCode;
    this.stockform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
    this.stockform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
    this.stockform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
    this.stockform.get("product_desc")?.setValue(result.ProductsCode[0].product_desc);
    this.stockform.value.productgroup_gid = result.ProductsCode[0].productgroup_gid;
    this.uom_gid = result.ProductsCode[0].productuom_gid;
    console.log(this.uom_gid)
  });
}
onClearCustomer() {
 
  this.txtprodcutgroup = '';
  this.txtproductcode='';
  this.txtproductunit='';
  this.MdlProductdesc='';
  this.mdlProductName='';
}
onClearlocation(){
  this.mdllocationName = '';
}
GetOnChangeLocation() {
  debugger
  
  let branch_gid = this.stockform.value.branch_name.branch_gid;
  let param = {
    branch_gid: branch_gid
  }
  var url = 'ImsTrnStockSummary/GetOnChangeLocation';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.location_list = this.responsedata.GetLocation;
    this.stockform.get("location_name")?.setValue(result.location_list[0].location_name);
    this.stockform.get("location_gid")?.setValue(result.location_list[0].location_gid);
     // this.stockform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
  });
}

onClearProduct() {
  this.txtproductgroup = '';
 this.txtproductcode='';
 this.txtproductunit='';
}
public validate(): void {
  debugger
  this.addstock = this.stockform.value;

  if (this.addstock.stock_qty != null && this.addstock.cost_price != null && this.addstock.branch_name != null && this.addstock.product_name != null  ) {
       
      var params = {    
        product_gid:this.stockform.value.product_name.product_gid,
        branch_gid:this.stockform.value.branch_name.branch_gid,
        branch_name:this.stockform.value.branch_name.branch_name,
        product_name:this.stockform.value.product_name.product_name,
        product_code:this.stockform.value.product_code,
        productuom_name:this.stockform.value.productuom_name,
        productgroup_name:this.stockform.value.productgroup_name,
        location_name:this.stockform.value.location_name.location_name,
        location_gid:this.stockform.value.location_name.location_gid,
        unit_price:this.stockform.value.cost_price,
        display_field:this.stockform.value.product_desc,
        stock_qty:this.stockform.value.stock_qty,
        finyear:this.stockform.value.finyear.finyear,
        uom_gid:this.uom_gid
       }
      var url2 = 'ImsTrnStockSummary/Poststock'
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



}

