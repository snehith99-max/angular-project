import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../../../environments/environment.development';
import { SocketService } from '../../../ems.utilities/services/socket.service';

import { FormsModule } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-smr-mst-productadd',
  templateUrl: './smr-mst-productadd.component.html',
  styleUrls: ['./smr-mst-productadd.component.scss']
})
export class SmrMstProductaddComponent implements OnInit, OnDestroy{
  isReadOnly = true;
  defaultProductCode: any;
  producttype_list: any[] = [];
  productgroup_list: any[] = [];
  productunitclass_list: any[] = [];
  productunit_list: any[] = [];
  tax_list:any[]=[];
  rbo_status: any[] = [];
  Form: FormGroup | any;
  hasError?: boolean;
  returnUrl?: string;
  mdlPrdType:any;
  mdlPrdUnitC:any;
  mdltax:any;
  mdlPrdUnit:any;
  mdlPrdName:any;

  submitted = false;


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
      producttype_name: new FormControl(''),
      productgroup_name:new FormControl('', Validators.required),
      productuomclass_name:new FormControl(''),
      productuom_name:new FormControl('',Validators.required),
      product_code:new FormControl(''),
      product_name:new FormControl('',Validators.required),
      product_desc:new FormControl(''),
      mrp_price:new FormControl('',Validators.required),
      cost_price:new FormControl(''),
      expirytracking_flag:new FormControl('N',Validators.required),
      batch_flag:new FormControl('N',Validators.required),
      serial_flag:new FormControl('N',Validators.required),
      purchasewarrenty_flag:new FormControl('N',Validators.required),
      sku: new FormControl(''),
      tax:new FormControl('',[Validators.required])
    });
  }
  ngOnDestroy(): void {

  }

  ngOnInit(): void {

    var api = 'SmrMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup;
      setTimeout(()=>{  

        $('#productgroup_list').DataTable();

      }, 0.1);
    });
    var api = 'SmrMstProduct/GetProducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.producttype_list = this.responsedata.GetProducttype;

    });
    var api = 'SmrMstProduct/gettaxdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.tax_list = this.responsedata.taxdtl_list;

    });


    var api = 'SmrMstProduct/GetProductUnitclass';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitclass_list = this.responsedata.GetProductUnitclass;

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
  get product_code() {

    return this.productform.get('product_code')!;

  };
 
  get mrp_price() {

    return this.productform.get('mrp_price')!;

  };
 
  
  get product_name() {

    return this.productform.get('product_name')!;

  }
  get tax() {

    return this.productform.get('tax')!;

  }




  initForm() {
    this.productform = this.fb.group({
      producttype_name: [ this.productform.producttype_name
      ],
      productgroup_name: [
        this.productform.productgroup_name,
        Validators.compose([
          Validators.required

        ]),
      ],
      // productuomclass_name: [
      //   this.productform.productuomclass_name,
      //   Validators.compose([
      //     Validators.required

      //   ]),
      // ],
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
       batch_flag: new FormControl(''),
       serial_flag: new FormControl(''),
       expirytracking_flag: new FormControl(''),
       purchasewarrenty_flag: new FormControl(''),

      cost_price: [
        this.productform.cost_price],
    });
  
  }
 
onadd() {

  if (
    this.productform.value.productgroup_name != null &&
    this.productform.value.productuom_name != null &&
    this.productform.value.product_name != null
    ) {
  var api = 'SmrMstProduct/PostSalesProduct';
  this.NgxSpinnerService.show()
  this.service.post(api, this.productform.value).subscribe(
    (result: any) => {

      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
      this.router.navigate(['smr/SmrMstProductSummary']);
        
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
  
      }
      });
  }

  else {
  
    window.scrollTo({

      top: 0, // Code is used for scroll top after event done

    });
    this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  }
  
  
  
}
redirecttolist(){
  this.router.navigate(['/smr/SmrMstProductSummary']);
}
// productunitclass() {
//   let productuomclass_gid = this.productform.get("productuomclass_name")?.value;
   
//    let param = {
//     productuomclass_gid : productuomclass_gid
//   }
//     var url = 'SmrMstProduct/GetOnChangeProductUnitClass';
//   this.service.getparams(url,param).subscribe((result:any)=>{    
//     this.responsedata=result;
    
//      this.productunit_list = this.responsedata.GetProductUnit;
    
//   });
// }
}

