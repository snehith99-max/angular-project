import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment.development';
import { SocketService } from '../../../ems.utilities/services/socket.service';

import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-ims-mst-product-add',
  templateUrl: './ims-mst-product-add.component.html',
  styleUrls: ['./ims-mst-product-add.component.scss']
})
export class ImsMstProductAddComponent {
  producttype_list: any[] = [];
  productgroup_list: any[] = [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlproducttype:any;
  mdlPrdUnitC:any;
  mdlPrdUnit:any;
  mdlPrdName:any;
  mdlproductgroup:any;
  cbotax: any;
  mdlproductunit:any;
  submitted = false;
  isReadOnly = true;
  defaultProductCode: any;
  mdltax:any;
  mdltax1:any;
  tax_list:any[]=[];
  tax1_list:any []=[];
  // private fields
  private unsubscribe: Subscription[] = []; 
  responsedata: any;
  result: any;
  productform: FormGroup<{ product_code: FormControl<any>; product_name: FormControl<any>; product_desc: FormControl<any>; mrp: FormControl<any>; cost_price: FormControl<any>; }> | any;
  

  constructor(
    private fb: FormBuilder,
    public NgxSpinnerService:NgxSpinnerService,

    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
  ) 
  {
    this.productform = new FormGroup({
      productgroup_name: new FormControl('', Validators.required),
      product_code:new FormControl(''),
      sku:new FormControl('',Validators.required),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl(''),
      productuom_name:new FormControl('',Validators.required),
       mrp_price:new FormControl(''),
      producttype_name:new FormControl('',Validators.required),
      tax:new FormControl('',Validators.required),
      tax1:new FormControl(''),
      cost_price:new FormControl('',Validators.required),
      // expirytracking_flag:new FormControl('N',Validators.required),
      // batch_flag:new FormControl('N',Validators.required),
      // serial_flag:new FormControl('N',Validators.required),
      // purchasewarrenty_flag:new FormControl('N',Validators.required),

    });
  }
  ngOnDestroy(): void {

  }
  ngOnInit(): void {

    var api = 'PmrMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup;
      // setTimeout(()=>{  

      //   $('#productgroup_list').DataTable();

      // }, 0.1) ;
    });
    var api = 'PmrMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });
   
    var api = 'PmrMstProduct/GetProductUnitclass';
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.productunitclass_list = this.responsedata.GetProductUnitclass;

  });
  var api = 'PmrMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = this.responsedata.taxdtl_list;

    });
    var api = 'SmrMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax1_list = this.responsedata.taxdtl_list;

    });
    var api = 'SmrMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

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
  get sku(){
    return this.productform.get('sku')!;
  }
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
  
  



  initForm() {
    this.productform = this.fb.group({
      producttype_name: [ this.productform.producttype_name,  Validators.compose([

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
      mrp_price: [
        this.productform.mrp_price,
        Validators.compose([
        Validators.required,

        ]),
      ],
      sku: [
        this.productform.sku,
        Validators.compose([
        Validators.required,

        ]),
      ],   tax: [
        this.productform.tax,
        Validators.compose([
        Validators.required,

        ]),
      ],
      tax1:new FormControl(''),
      

      
       batch_flag: new FormControl(''),
       serial_flag: new FormControl(''),
       product_desc:new FormControl(''),
       expirytracking_flag: new FormControl(''),
       purchasewarrenty_flag: new FormControl(''),

       cost_price: [
        this.productform.cost_price,
        Validators.compose([
          Validators.required

        ]),
      ],
    });
  
  }
  GetOnproductunitName(){
    
    
    let productuomclass_gid = this.productform.value.productuomclass_name;
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
  this.mdlPrdUnit='';
  this.mdlPrdUnitC='';
 }
 
onadd() {
  debugger;
  if(this.productform.value.producttype_name!=null &&
     this.productform.value.productgroup_name!=null &&
       this.productform.value.productuom_name!=null &&
         this.productform.value.product_name !='' 
        ){
  var api = 'PmrMstProduct/PostProduct';
  this.NgxSpinnerService.show();
  this.service.post(api, this.productform.value).subscribe(
    (result: any) => {

      if (result.status == true) {
        this.ToastrService.success(result.message)
       
        this.router.navigate(['ims/ImsMstProductSummary']);
        this.NgxSpinnerService.hide();
        

      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
  
      }
        
        

      }
    ,(error: any) => {
      if (error.status === 401)
        this.router.navigate(['pages/401'])
      else if (error.status === 404)
        this.router.navigate(['pages/404'])
    });
  }
  else{
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
  
}
redirecttolist(){
  this.router.navigate(['/ims/ImsMstProductSummary']);

}
productunitclass() {
  let productuomclass_gid = this.productform.get("productuomclass_name")?.value;
   
   let param = {
    productuomclass_gid : productuomclass_gid
  }
    var url = 'PmrMstProduct/GetOnChangeProductUnitClass';
  this.service.getparams(url,param).subscribe((result:any)=>{    
    this.responsedata=result;
    
     this.productunit_list = this.responsedata.GetProductUnit;
    
  });
}
}


