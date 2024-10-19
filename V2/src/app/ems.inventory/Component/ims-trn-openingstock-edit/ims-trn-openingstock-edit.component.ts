import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface addopeningstock {

  branch_name: string;
  branch_gid: string;
  productgroup_gid: string;
  productgroup_name: string;
  customerproduct_code: string;
  product_code: string;
  product_gid: any;
  product_name: string;
  productuom_name: string;
  productuom_gid: string;
  cost_price: string;
  opening_stock: string;
  location_name: string;
  product_desc: string;
  stock_gid: string;
  product_status: string;
  finyear: string;

}


@Component({
  selector: 'app-ims-trn-openingstock-edit',
  templateUrl: './ims-trn-openingstock-edit.component.html',
  styleUrls: ['./ims-trn-openingstock-edit.component.scss']
})
export class ImsTrnOpeningstockEditComponent {
  producttype_list: any[] = [];
  productgroup_list: any[] = [];
  branch_list: any[] = [];
  GetLocation: any[] = [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  product_list: any[] = [];
  productcode_list: any[] = [];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlPrdType: any;
  mdlPrdUnitC: any;
  mdlPrdUnit: any;
  mdlPrdName: any;
  GetproductsCode: any;
  responsedata: any;
  submitted = false;
  result: any;
  productform: FormGroup | any;
  addopeningstock !: addopeningstock;
  fb: any;
  mdlProductName: any;
  mdlBranchName: any;
  uom_gid: any;
  stock_gid: any;
  editProductSummary_list: any;
  mdlFinancialYear: any;
  financialYearList: any[] = [];
  mdllocationName:any;
  location_list:any[]=[];
  txtprodcutgroup :any;
  txtproductcode:any;
  txtproductunit:any;
  mdlProductdesc:any;
  productsCode:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, public NgxSpinnerService: NgxSpinnerService, private router: ActivatedRoute, private route: Router) {
    this.addopeningstock = {} as addopeningstock;

    this.productform = new FormGroup({
      branch_name: new FormControl('',Validators.required),
      product_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      productuom_name: new FormControl('',Validators.required),
      productuom_gid: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl('',Validators.required),
      product_desc: new FormControl(''),
      location_name: new FormControl('',Validators.required),
      opening_stock: new FormControl(''),
      location_gid :new FormControl(''),
      cost_price: new FormControl(''),
      stock_gid: new FormControl(''),
      product_status: new FormControl(''),
      branch_gid: new FormControl(''),
      finyear: new FormControl('', Validators.required),

    });

  }
  ngOnInit(): void {

    var api = 'ImsTrnStockSummary/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup;
    });

    var url = 'ImsTrnOpeningStock/GetProductNamDtl'
   this.service.get(url).subscribe((result:any)=>{
     this.product_list = result.GetProductNamDtl;
    });

    var url = 'ImsTrnStockSummary/GetBranchDtl'
    this.service.get(url).subscribe((result:any)=>{
      this.branch_list = result.GetBranchDtl;
      this.productform.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

     });
    var url = 'ImsTrnStockSummary/GetFinancialYear'
    this.service.get(url).subscribe((result: any) => {
      this.financialYearList = result.GetFinancialYear;
      //this.productform.get("finyear")?.setValue(result.GetFinancialYear[0].finyear);
      console.log(this.financialYearList)
    });
    const stock_gid = this.router.snapshot.paramMap.get('stock_gid');
    this.stock_gid = stock_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.stock_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

    debugger
    this.GetEditOpeningStockSummary(deencryptedParam)
  }


  GetEditOpeningStockSummary(stock_gid: any) {
    var url = 'ImsTrnOpeningStock/GetEditOpeningStockSummary'
    debugger
    let param = { stock_gid: stock_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.editProductSummary_list = result.GetEditOpeningStock;

      // this.product = result;
      console.log(this.addopeningstock)
      console.log(this.editProductSummary_list)
      this.productform.get("stock_gid")?.setValue(result.GetEditOpeningStock[0].stock_gid);
      this.productform.get("product_name")?.setValue(result.GetEditOpeningStock[0].product_name);
      this.productform.get("product_code")?.setValue(result.GetEditOpeningStock[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.GetEditOpeningStock[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.GetEditOpeningStock[0].productgroup_name);
      this.productform.get("branch_name")?.setValue(result.GetEditOpeningStock[0].branch_name);
      this.productform.get("location_name")?.setValue(result.GetEditOpeningStock[0].location_name);
      this.productform.get("product_desc")?.setValue(result.GetEditOpeningStock[0].product_desc);
      this.productform.get("opening_stock")?.setValue(result.GetEditOpeningStock[0].opening_stock);
      this.productform.get("cost_price")?.setValue(result.GetEditOpeningStock[0].cost_price);
      this.productform.get("product_gid")?.setValue(result.GetEditOpeningStock[0].product_gid);
      this.productform.get("productuom_gid")?.setValue(result.GetEditOpeningStock[0].productuom_gid);
      this.productform.get("branch_gid")?.setValue(result.GetEditOpeningStock[0].branch_gid);
      this.productform.get("product_status")?.setValue(result.GetEditOpeningStock[0].product_status);
      this.productform.get("finyear")?.setValue(result.GetEditOpeningStock[0].financial_year);

    });
  }

  get branch_name() {

    return this.productform.get('branch_name')!;

  };
  get productgroup_name() {

    return this.productform.get('productgroup_name')!;

  };
  get productuomclass_name() {

    return this.productform.get('productuomclass_name')!;

  };
  get productuom_name() {

    return this.productform.get('productuom_name')!;

  };
  get productuom_gid() {

    return this.productform.get('productuom_gid')!;

  };
  get product_code() {

    return this.productform.get('product_code')!;

  };
  get opening_stock() {

    return this.productform.get('opening_stock')!;

  };
  get cost_price() {

    return this.productform.get('cost_price')!;

  };
  get product_name() {

    return this.productform.get('product_name')!;

  }
  get location_name() {

    return this.productform.get('location_name')!;

  }
  get finyear() {
    return this.productform.get('finyear')!;
  }



  initForm() {
    this.productform = this.fb.group({

      product_desc: [
        this.productform.product_desc,
        Validators.compose([
          Validators.required,

        ]),
      ],
      stock_gid: [
        this.productform.stock_gid,
        Validators.compose([
          Validators.required,

        ]),
      ],
      product_gid: [
        this.productform.product_gid,
        Validators.compose([
          Validators.required,

        ]),
      ],

      opening_stock: [
        this.productform.opening_stock,
        Validators.compose([
          Validators.required,

        ]),
      ],
      finyear: [
        this.productform.finyear,
        Validators.compose([
          Validators.required,
        ]),
      ],

      cost_price: [
        this.productform.cost_price,
        Validators.compose([
          Validators.required,

        ]),
      ],
      product_status: [
        this.productform.product_status,
        Validators.compose([
          Validators.required,

        ]),
      ],
    });

  }
  GetOnChangeProductgroupName (){
    let productgroup_gid = this.productform.value.productgroup_name.productgroup_gid;
    let param = {
      productgroup_gid: productgroup_gid
    }
    var url = 'ImsTrnOpeningStock/GetonchangeProductNamDtl'
     this.service.getparams(url,param).subscribe((result:any)=>{
       this.product_list = result.GetProduct_name;
      });
  }
  
  GetOnChangeProductsName(){
    debugger;
    let product_gid = this.productform.value.product_name.product_gid;
    let param = {
      product_gid: product_gid
      
    }
    var url = 'ImsTrnOpeningStock/GetOnChangeproductName';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productsCode = result.ProductsCode;
      this.productform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
      this.productform.get("product_desc")?.setValue(result.ProductsCode[0].product_desc);
    });
  }
  onClearCustomer() {
 
    this.txtprodcutgroup = '';
    this.txtproductcode='';
    this.txtproductunit='';
    this.mdlProductdesc='';
    this.mdlProductName='';
  }
  onClearlocation(){
    this.mdllocationName = '';
  }
  redirecttolist() {
    this.route.navigate(['/ims/ImsTrnOpeningstockSummary']);

  }
  GetOnChangeLocation() {
    debugger
    let branch_gid = this.productform.value.branch_name.branch_gid;
    let param = {
      branch_gid: branch_gid
    }
    var url = 'ImsTrnStockSummary/GetOnChangeLocation';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.location_list = this.responsedata.GetLocation;
      this.productform.get("location_name")?.setValue(result.location_list[0].location_name);
      this.productform.get("location_gid")?.setValue(result.location_list[0].location_gid);
       // this.productform.value.productuom_gid = result.GetProductsCode[0].productuom_gid
    });
  }

  public validate(): void {
    debugger;

    const productName = typeof this.productform.value.product_name === 'object' && this.productform.value.product_name !== null 
  ? this.productform.value.product_name.product_name 
  : this.productform.value.product_name;


  const locationName = typeof this.productform.value.location_name === 'object' && this.productform.value.location_name !== null 
  ? this.productform.value.location_name.location_name 
  : this.productform.value.location_name;

  const branchName = typeof this.mdlBranchName === 'object' && this.mdlBranchName !== null 
  ? this.productform.value.branch_name.branch_gid
  : this.productform.value.branch_gid;


  const finyear = typeof this.productform.value.finyear === 'object' && this.productform.value.finyear !== null 
  ? this.productform.value.finyear.finyear 
  : this.productform.value.finyear;

    this.addopeningstock = this.productform.value;
    var params = {


        branch_gid:branchName,
        finyear:finyear,
        product_name:productName,
        product_code:this.productform.value.product_code,
        productuom_name:this.productform.value.productuom_name,
        productgroup_name:this.productform.value.productgroup_name,
        location_name:locationName,
        cost_price: this.productform.value.cost_price,
        product_desc: this.productform.value.product_desc,
        opening_stock: this.productform.value.opening_stock,
        stock_gid: this.productform.value.stock_gid,
        product_status: this.productform.value.product_status,
        productuom_gid: this.productform.value.productuom_gid,
        opening_qty: this.editProductSummary_list[0].opening_qty,
        stock_qty:this.editProductSummary_list[0].stock_qty,
    }
    if (this.addopeningstock.opening_stock != null) {
      debugger;
      var api = 'ImsTrnOpeningStock/PostOpeningStockUpdate';
      this.NgxSpinnerService.show();
      this.service.post(api,params).subscribe(
        (result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.success(result.message)
            this.route.navigate(['/ims/ImsTrnOpeningstockSummary']);
          }
          this.NgxSpinnerService.hide();
        });
    }
  }
}

