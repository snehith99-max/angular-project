import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface addopeningstock{
  branch_name: string;
  branch_gid: string;
  finyear:string;
  productgroup_gid: string;
  productgroup_name: string;
  customerproduct_code: string;
  product_code: string;
  product_gid: string;
  product_name: string;
  cost_price: string;
  product_desc: string;
  reorder_level: string;
  uom_gid: string;
  location_gid: string;
  location_name: string;
  productuom_name: string;
  productuom_gid: string;
}

@Component({
  selector: 'app-ims-mst-reorderleveladd',
  templateUrl: './ims-mst-reorderleveladd.component.html',
  styleUrls: ['./ims-mst-reorderleveladd.component.scss']
})
export class ImsMstReorderleveladdComponent {
  file!: File;
  producttype_list: any[] = [];
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
  mdllocationName:any;
  location_list:any;
  financialYearList:any []=[];
  mdlFinancialYear:any;
  txtproductgroup:any;
  txtproductcode:any;
  txtproductunit:any;
  mdlPrdName:any;
  productsCode:any;
  responsedata: any;
  submitted = false;
  txtprodcutgroup :any;
  result: any;
  productform: FormGroup | any;
  addopeningstock !:addopeningstock;
  fb: any;
  mdlProductName:any;
  mdlBranchName :any;
 uom_gid:any;
 mdlProductdesc:any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService, public service: SocketService, private route: ActivatedRoute,private router:Router) {
    this.addopeningstock = {} as addopeningstock;
  
    this.productform = new FormGroup({
      branch_name: new FormControl('', Validators.required),
      branch_gid: new FormControl(''),
      product_gid :new FormControl(''),
      productgroup_name:new FormControl(''),
      productuomclass_name:new FormControl('',),
      productuom_name:new FormControl(''),
      productuom_gid:new FormControl(''),
      product_code:new FormControl(''),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl(''),
      reorder_level:new FormControl('',Validators.required),
      

    });
  
  }
  ngOnInit(): void {

    var api = 'ImsMstReorderlevel/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.ProductGroup;
    });
    var api = 'ImsMstReorderlevel/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.Producttype;
    });
    var api = 'ImsMstReorderlevel/GetProductUnitclass';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitclass_list = this.responsedata.ProductUnitclass;
    });
   var url = 'ImsMstReorderlevel/GetProductNamDtl'
   this.service.get(url).subscribe((result:any)=>{
     this.product_list = result.ProductNamDropdown;
    });
    // var api = 'PmrMstProduct/GetProductUnit';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.productunit_list = this.responsedata.GetProductUnit;

    // });
    var url = 'ImsMstReorderlevel/GetBranchDtl'
    this.service.get(url).subscribe((result:any)=>{
      this.branch_list = result.BranchDropdown;
      this.productform.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

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
  get reorder_level() {

    return this.productform.get('reorder_level')!;

  };
  get product_name() {

    return this.productform.get('product_name')!;

  }
  get product_desc() {

    return this.productform.get('product_desc')!;

  }


  initForm() {
    this.productform = this.fb.group({
      branch_name: [ this.productform.branch_name,  Validators.compose([

          Validators.required,
          ]),
      ],
      
      productgroup_name: [
        this.productform.productgroup_name,
        Validators.compose([
          Validators.required

        ]),
      ],

      productuomclass_name: [
        this.productform.productuomclass_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      productuom_name: [
        this.productform.productuom_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_code: [
        this.productform.product_code,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_name: [
        this.productform.product_name,
        Validators.compose([
        Validators.required,

        ]),
      ],
      product_desc: [
        this.productform.product_desc,
        Validators.compose([
        Validators.required,

        ]),
      ],

      reorder_level: [
        this.productform.reorder_level,
        Validators.compose([
        Validators.required,

        ]),
      ],
    });
  
  }
 

redirecttolist(){
  this.router.navigate(['/ims/ImsTrnRolsettings']);

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
  
  let product_gid = this.productform.value.product_name.product_gid;
  let param = {
    product_gid: product_gid
    
  }
  var url = 'ImsTrnOpeningStock/GetOnChangeproductName';
  this.service.getparams(url, param).subscribe((result: any) => {
    this.responsedata = result;
    this.productsCode = result.productsCode;
    this.productform.get("product_code")?.setValue(result.ProductsCode[0].product_code);
    this.productform.get("productuom_name")?.setValue(result.ProductsCode[0].productuom_name);
    this.productform.get("productgroup_name")?.setValue(result.ProductsCode[0].productgroup_name);
    this.productform.get("product_desc")?.setValue(result.ProductsCode[0].product_desc);
    this.productform.value.productgroup_gid = result.ProductsCode[0].productgroup_gid;
    this.uom_gid = result.ProductsCode[0].productuom_gid;
    console.log(this.uom_gid)
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

onClearProduct() {
  this.txtproductgroup = '';
 this.txtproductcode='';
 this.txtproductunit='';
}
public validate(): void {
  debugger;

  this.addopeningstock = this.productform.value;

  if (this.addopeningstock.reorder_level != null  && this.addopeningstock.branch_name != null && this.addopeningstock.product_name != null  ) {
       
      var params = {    
        product_gid:this.productform.value.product_name.product_gid,
        branch_gid:this.productform.value.branch_name.branch_gid,
        branch_name:this.productform.value.branch_name.branch_name,
        product_name:this.productform.value.product_name.product_name,
        product_code:this.productform.value.product_code,
        productuom_name:this.productform.value.productuom_name,
        productgroup_name:this.productform.value.productgroup_name,
        display_field:this.productform.value.product_desc,
        reorder_level:this.productform.value.reorder_level,
        uom_gid:this.uom_gid
       }
      var url2 = 'ImsMstReorderlevel/PostROL'
      this.NgxSpinnerService.show()
      this.service.post(url2,params).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide()
          this.ToastrService.warning(result.message)
          
        }
        else {
          this.router.navigate(['/ims/ImsTrnRolsettings']);
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

