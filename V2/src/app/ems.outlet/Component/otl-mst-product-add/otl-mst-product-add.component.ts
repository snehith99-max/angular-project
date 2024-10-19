import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment.development';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface IProduct {
  product_gid: string;
  product_name : string;
}
@Component({
  selector: 'app-otl-mst-product-add',
  templateUrl: './otl-mst-product-add.component.html',
  styleUrls: ['./otl-mst-product-add.component.scss']
})
export class OtlMstProductAddComponent {

  producttype_list: any[] = [];
  productgroup_list: any[] = [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlproducttype: any;
  mdlPrdUnitC: any;
  mdlPrdUnit: any;
  mdlPrdName: any;
  mdlproductgroup: any;
  mdlproductunit: any;
  submitted = false;
  isReadOnly = true;
  defaultProductCode: any;
  mdltax: any;
  tax_list: any[] = [];
  file!: File;
  private unsubscribe: Subscription[] = [];
  responsedata: any;
  result: any;
  product!: IProduct;
  productform: FormGroup<{ product_code: FormControl<any>; product_name: FormControl<any>; product_desc: FormControl<any>; mrp: FormControl<any>; cost_price: FormControl<any>;product_image : FormControl<any>; }> | any;


  constructor(
    private fb: FormBuilder,
    public NgxSpinnerService: NgxSpinnerService,

    private route: ActivatedRoute,
    private router: Router,
    private service: SocketService,
    private ToastrService: ToastrService,
  ) {
    this.productform = new FormGroup({
      //productgroup_name: new FormControl('', Validators.required),
      product_code: new FormControl(''),
      sku: new FormControl('', Validators.required),
      product_name: new FormControl('', Validators.required),
      product_desc: new FormControl(''),
     // productuom_name: new FormControl('', Validators.required),
      mrp_price: new FormControl(''),
     // producttype_name: new FormControl('', Validators.required),
     // tax: new FormControl('', Validators.required),
      cost_price: new FormControl('', Validators.required),
      product_image: new FormControl(''),

    });
  }
  ngOnDestroy(): void {

  }
  ngOnInit(): void {

    var api = 'OtlMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup_list;
      // setTimeout(()=>{  

      //   $('#productgroup_list').DataTable();

      // }, 0.1) ;
    });
    var api = 'OtlMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype_list;

    });

    var api = 'OtlMstProduct/GetProductUnitclass';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitclass_list = this.responsedata.GetProductUnitclass_list;

    });
    var api = 'OtlMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = this.responsedata.tax_list;

    });
    var api = 'OtlMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit_list;

    });

  }

  // get producttype_name() {

  //   return this.productform.get('producttype_name')!;

  // };
  // get productgroup_name() {

  //   return this.productform.get('productgroup_name')!;

  // };
  // get productuomclass_name() {

  //   return this.productform.get('productuomclass_name')!;

  // };
  // get productuom_name() {

  //   return this.productform.get('productuom_name')!;

  // };

  // get tax() {

  //   return this.productform.get('tax')!;

  // };


  get mrp_price() {

    return this.productform.get('mrp_price')!;

  };
  get sku() {
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

  




  initForm() {
    this.productform = this.fb.group({
      producttype_name: [this.productform.producttype_name, Validators.compose([

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
      tax: [
        this.productform.tax,
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
      ], 




      batch_flag: new FormControl(''),
      serial_flag: new FormControl(''),
      product_desc: new FormControl(''),
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
  GetOnproductunitName() {


    let productuomclass_gid = this.productform.value.productuomclass_name;
    let param = {
      productuomclass_gid: productuomclass_gid
    }
    var api = 'OtlMstProduct/GetProductUnit';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

    });
  }
  onclearproduct() {
    this.mdlPrdUnit = '';
    this.mdlPrdUnitC = '';
  }

  onadd() {
    debugger  
    var params={
      product_code: this.productform.value.product_code,
      product_name:  this.productform.value.product_name,
      product_desc:this.productform.value.product_desc || '',
      sku:this.productform.value.sku ,
      cost_price :this.productform.value.cost_price ,
    }

    this.NgxSpinnerService.show();
    var url = 'OtlMstProduct/PostProduct';

    this.service.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.show();
      if (result.status == true) {
        {
          if (this.file != null && this.file != undefined) {
            let formData = new FormData();
            this.product = this.productform.value;
            formData.append("file", this.file, this.file.name);
            formData.append("product_name", this.product.product_name);
            var api = 'OtlMstProduct/GetProductImage'
      
            this.service.postfile(api, formData).subscribe((result: any) => {
              this.responsedata = result;
              if (result.status == false) {
                this.NgxSpinnerService.hide();
                this.ToastrService.warning(result.message)
              }
              else {
                this.productform.reset();
                this.NgxSpinnerService.hide();
                this.router.navigate(['/outlet/OtlMstProduct']);
                this.ToastrService.success(result.message)
                
              }
            });
            this.NgxSpinnerService.hide();
          }
         else{
          this.NgxSpinnerService.hide();
          this.router.navigate(['/outlet/OtlMstProduct']);
          this.ToastrService.success(result.message)
         }
        }
      }
  })
  
    
}
  redirecttolist() {
    this.router.navigate(['/outlet/OtlMstProduct']);

  }
  productunitclass() {
    let productuomclass_gid = this.productform.get("productuomclass_name")?.value;

    let param = {
      productuomclass_gid: productuomclass_gid
    }
    var url = 'OtlMstProduct/GetOnChangeProductUnitClass';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;

      this.productunit_list = this.responsedata.GetProductUnit_list;

    });
  }
  onChange2(event: any) {
    this.file = event.target.files[0];

  }
}


