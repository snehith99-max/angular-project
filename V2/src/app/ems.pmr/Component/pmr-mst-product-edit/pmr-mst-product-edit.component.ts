import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router,ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface IProduct {

  product_gid:string;
  producttypename: string;
  productgroupname: string;
  productuomclassname: string;
  productuomname: string;
  product_code:string;
  product_name: string;
  cost_price: string;
  mrp_price: string;
  product_desc: string;
  productuomclass_gid:string;
}
@Component({
  selector: 'app-pmr-mst-product-edit',
  templateUrl: './pmr-mst-product-edit.component.html',
})
export class PmrMstProductEditComponent  {
  product!: IProduct;
  defaultAuth: any = { }; 
  product_gid:any;
  producttype_list: any;
  productgroup_list: any;
  productunitclass_list:any;
  productunit_list: any[] = [];
  tax_list: any[] = [];
  tax1_list:any []=[];
  productform!: FormGroup | any;
  hasError?: boolean;
  selectedproducttype_name: any;
  selectedproductgroup_name: any;
  selectedproductuom_name: any;
  selectedUnits: any;
  // mrp_price:number=0;
  // cost_price:number=0;
  mdltax:any;
  mdlPrdUnitC:any;
  mdlPrdName:any;
  cbotax: any;
  returnUrl?: string;
  reactiveForm!: FormGroup;

  submitted = false;
  Productgid: any;
  Product_list:any;
  // private fields
  responsedata: any;
  editProductSummary_list: any;
  mdlproducttype: any;
  mdltax1:any;


 constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService, private ToastrService: ToastrService,private route:Router,private router: ActivatedRoute,private NgxSpinnerService:NgxSpinnerService ) {
    this.product = {} as IProduct;
  }


  ngOnInit(): void {

 const product_gid = this.router.snapshot.paramMap.get('product_gid');
    this.product_gid = product_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.product_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)


    this.productform = new FormGroup({

      productgroup_name: new FormControl('', Validators.required),
      product_code:new FormControl('',Validators.required),
      sku:new FormControl('',Validators.required),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl(''),
      productuom_name:new FormControl('',Validators.required),
       mrp_price:new FormControl(''),
      producttype_name:new FormControl('',Validators.required),
      tax:new FormControl('',Validators.required),
      tax1:new FormControl(''),
      cost_price:new FormControl('',Validators.required),
      product_gid:new FormControl(''),
    });

    var api = 'PmrMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productgroup_list = this.responsedata.GetProductGroup;
      setTimeout(()=>{  
         $('#productgroup_list').DataTable();
      }, 0.1);
    });

    var api = 'PmrMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });

    var api = 'PmrMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = this.responsedata.taxdtl_list;

    });
    var api = 'PmrMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax1_list = this.responsedata.taxdtl_list;

    });
    var api = 'PmrMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

    });

    // var api = 'PmrMstProduct/GetProductUnitclass';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.productunitclass_list = this.responsedata.GetProductUnitclass;
    

    // });

  
    this.GetEditProductSummary(deencryptedParam)
  } 

  GetEditProductSummary(product_gid: any) {
    debugger
    var url = 'PmrMstProduct/GetEditProductSummary'
    this.NgxSpinnerService.show();
    let param = {product_gid : product_gid}
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata=result;
      this.editProductSummary_list = result.GetEditProductSummary;

      // this.product = result;
      console.log(this.product)
      console.log(this.editProductSummary_list)

      this.productform.get("product_gid")?.setValue(this.editProductSummary_list[0].product_gid);
      this.productform.get("producttype_name")?.setValue(this.editProductSummary_list[0].producttype_name);
      this.productform.get("productgroup_name")?.setValue(this.editProductSummary_list[0].productgroup_name);
      this.productform.get("productuomclassname")?.setValue(this.editProductSummary_list[0].productuomclass_gid);
      this.productform.get("productuom_name")?.setValue(this.editProductSummary_list[0].productuom_name);
      this.selectedproducttype_name = this.editProductSummary_list[0].producttype_name;
      this.selectedproductgroup_name = this.editProductSummary_list[0].productgroup_name;
      // this.selectedUnitclass = this.editProductSummary_list[0].productuomclassname;
      this.selectedproductuom_name = this.editProductSummary_list[0].productuom_name;
      this.productform.get("product_code")?.setValue(this.editProductSummary_list[0].product_code);
      this.productform.get("product_name")?.setValue(this.editProductSummary_list[0].product_name);
      this.productform.get("mrp_price")?.setValue(this.editProductSummary_list[0].mrp_price);
      this.productform.get("cost_price")?.setValue(this.editProductSummary_list[0].cost_price);
      this.productform.get("product_desc")?.setValue(this.editProductSummary_list[0].product_desc);
      // this.productform.get("purchasewarrenty_flag")?.setValue(this.editProductSummary_list[0].purchasewarrenty_flag);
      // this.productform.get("serial_flag")?.setValue(this.editProductSummary_list[0].serial_flag);
      // this.productform.get("batch_flag")?.setValue(this.editProductSummary_list[0].batch_flag);
      // this.productform.get("expirytracking_flag")?.setValue(this.editProductSummary_list[0].expirytracking_flag);
      this.productform.get("sku")?.setValue(this.editProductSummary_list[0].sku);
      this.productform.get("tax")?.setValue([
        this.editProductSummary_list[0].tax_gid, 
        this.editProductSummary_list[0].tax_gid1
      ]);
      this.NgxSpinnerService.hide();
    });
  } 
  get producttype_name() {

    return this.productform.get('producttype_name')!;

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
  get mrp_price() {

    return this.productform.get('mrp_price')!;

  };
  get cost_price() {

    return this.productform.get('cost_price')!;

  };
  get product_code() {

    return this.productform.get('product_code')!;

  };
  
 
  get product_name() {

    return this.productform.get('product_name')!;

  };
  
  get tax() {

    return this.productform.get('tax')!;

  };
  get sku(){
    return this.productform.get('sku')!;
  }
  
  GetOnproductunitName(){
    debugger
    
    let productuomclass_gid = this.productform.get("productuomclassname")?.value;
  let param = {
   productuomclass_gid : productuomclass_gid
 }
 var api = 'PmrMstProduct/GetProductUnit';
 this.service.getparams(api,param).subscribe((result: any) => {
   this.responsedata = result;
   this.productunit_list = this.responsedata.GetProductUnit;

 });
 }
 onclearproduct(){
  // this.selectedUnitclass='';
  this.selectedUnits='';
 }

  public validate(): void {
    debugger
    this.product = this.productform.value;
   
    if (this.product.product_code != null && this.product.product_name != null &&   

       this.product.cost_price != null ) {
      
     console.log(this.productform.value)
     this.NgxSpinnerService.show();
     const api = 'PmrMstProduct/PmrMstProductUpdate';

     this.service.post(api, this.productform.value).subscribe(
      (result: any) => {
  
        if(result.status ==false){

          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();

        }
        else{
          this.ToastrService.success(result.message)
          this.route.navigate(['/pmr/PmrMstProductSummary']);
          this.NgxSpinnerService.hide();
            

        }

      });
  }
}
   }




