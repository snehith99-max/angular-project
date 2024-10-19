import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
interface addopeningstock {
  reorder_level: null;

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
  product_desc: string;
  rol_gid: string;


}


@Component({
  selector: 'app-ims-trn-reorderleveledit',
  templateUrl: './ims-trn-reorderleveledit.component.html',
  styleUrls: ['./ims-trn-reorderleveledit.component.scss']
})
export class ImsTrnReorderleveleditComponent {
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
  productsCode: any;
  responsedata: any;
  submitted = false;
  result: any;
  productform: FormGroup | any;
  addopeningstock !: addopeningstock;
  fb: any;
  mdlProductName: any;
  mdlBranchName: any;
  uom_gid: any;
  rol_gid: any;
  editProductSummary_list: any;
  mdlFinancialYear: any;
  financialYearList: any[] = [];
  txtprodcutgroup :any;
  txtproductcode :any;
  txtproductunit :any;
  mdlProductdesc :any;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService, public NgxSpinnerService: NgxSpinnerService, private router: ActivatedRoute, private route: Router) {
    this.addopeningstock = {} as addopeningstock;

    this.productform = new FormGroup({
      branch_name: new FormControl(''),
      product_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      productuom_name: new FormControl(''),
      productuom_gid: new FormControl(''),
      product_code: new FormControl(''),
      product_name: new FormControl(''),
      product_desc: new FormControl(''),
      reorder_level: new FormControl(''),
      rol_gid: new FormControl(''),
      branch_gid: new FormControl(''),


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
    var url = 'ImsMstReorderlevel/GetBranchDtl'
    this.service.get(url).subscribe((result:any)=>{
      this.branch_list = result.BranchDropdown;
      this.productform.get("branch_gid")?.setValue(result.GetBranchDtl[0].branch_gid);

     });
   
    const rol_gid = this.router.snapshot.paramMap.get('rol_gid');
    this.rol_gid = rol_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.rol_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

    debugger
    this.GetEditROLSummary(deencryptedParam)
  }


  GetEditROLSummary(rol_gid: any) {
    var url = 'ImsMstReorderlevel/GetEditROLSummary'
    debugger
    let param = { rol_gid: rol_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.editProductSummary_list = result.EditRol_list;

      // this.product = result;
      console.log(this.addopeningstock)
      console.log(this.editProductSummary_list)
      this.productform.get("rol_gid")?.setValue(result.EditRol_list[0].rol_gid);
      this.productform.get("product_name")?.setValue(result.EditRol_list[0].product_name);
      this.productform.get("product_code")?.setValue(result.EditRol_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.EditRol_list[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.EditRol_list[0].productgroup_name);
      this.productform.get("branch_name")?.setValue(result.EditRol_list[0].branch_name);
      this.productform.get("product_desc")?.setValue(result.EditRol_list[0].product_desc);
      this.productform.get("reorder_level")?.setValue(result.EditRol_list[0].reorder_level);
      this.productform.get("product_gid")?.setValue(result.EditRol_list[0].product_gid);
      this.productform.get("productuom_gid")?.setValue(result.EditRol_list[0].productuom_gid);
      this.productform.get("branch_gid")?.setValue(result.EditRol_list[0].branch_gid);
     

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




  initForm() {
    this.productform = this.fb.group({

      product_desc: [
        this.productform.product_desc,
        Validators.compose([
          Validators.required,

        ]),
      ],
      rol_gid: [
        this.productform.rol_gid,
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

      reorder_level: [
        this.productform.reorder_level,
        Validators.compose([
          Validators.required,

        ]),
      ],

    });

  }


  redirecttolist() {
    this.route.navigate(['/ims/ImsTrnRolsettings']);

  }
  GetOnChangeProductgroupName (){
    let productgroup_gid = this.productform.value.productgroup_name.productgroup_gid;
    let param = {
      productgroup_gid: productgroup_gid
    }
    var url = 'ImsTrnOpeningStock/GetonchangeProductNamDtl'
     this.service.getparams(url,param).subscribe((result:any)=>{
       this.product_list = result.Product_names;
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
  onClearProduct() {
    this.txtprodcutgroup = '';
   this.txtproductcode='';
   this.txtproductunit='';
  }
  public validate(): void {
    debugger;
    this.addopeningstock = this.productform.value;
    
  const productname = typeof this.mdlProductName === 'object' && this.mdlProductName !== null 
  ? this.productform.value.product_name.product_name
  : this.productform.value.product_name;
    var params = {
      branch_gid: this.productform.value.branch_gid,
      //product_gid: this.productform.value.product_gid,
      branch_name: this.productform.value.branch_name,
      product_name: productname,
      product_code: this.productform.value.product_code,
      productuom_name: this.productform.value.productuom_name,
      productgroup_name: this.productform.value.productgroup_name,
      product_desc: this.productform.value.product_desc,
      reorder_level: this.productform.value.reorder_level,
      rol_gid: this.productform.value.rol_gid,
      productuom_gid: this.productform.value.productuom_gid,
    }
    if (this.addopeningstock.reorder_level != null) {
      debugger;
      const api = 'ImsMstReorderlevel/PostROLUpdate';
      this.NgxSpinnerService.show();
      this.service.post(api,params).subscribe(
        (result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.success(result.message)
            this.route.navigate(['/ims/ImsTrnRolsettings']);
          }
          this.NgxSpinnerService.hide();
        });
    }
  }
}

