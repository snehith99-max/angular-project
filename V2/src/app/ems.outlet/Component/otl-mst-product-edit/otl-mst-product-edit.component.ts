import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


interface IProduct {

  product_gid: string;
  producttypename: string;
  productgroupname: string;
  productuomclassname: string;
  productuomname: string;
  product_code: string;
  product_name: string;
  cost_price: string;
  product_desc: string;
  mrp_price: string;
}
@Component({
  selector: 'app-otl-mst-product-edit',
  templateUrl: './otl-mst-product-edit.component.html',
  styleUrls: ['./otl-mst-product-edit.component.scss']
})
export class OtlMstProductEditComponent {

  isReadOnly = true;
  product!: IProduct;
  defaultAuth: any = {};
  product_gid: any;
  producttype_list: any;
  productgroup_list: any;
  productunitclass_list: any;
  productunit_list: any[] = [];
  productform!: FormGroup | any;
  hasError?: boolean;
  selectedProducttype: any;
  selectedProductgroup: any;
  selectedUnitclass: any;
  selectedUnits: any;
  file!: File;
  returnUrl?: string;
  reactiveForm!: FormGroup;

  submitted = false;
  Productgid: any;
  Product_list: any;
  // private fields
  responsedata: any;
  editProductSummary_list: any;
  remainingChars: any | number = 1000


  constructor(private renderer: Renderer2, private NgxSpinnerService: NgxSpinnerService, private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.product = {} as IProduct;
  }


  ngOnInit(): void {

    const product_gid = this.router.snapshot.paramMap.get('product_gid');
    this.product_gid = product_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.product_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)


    this.productform = new FormGroup({

      product_code: new FormControl(this.product.product_code, [
        Validators.required,

      ]),
      product_name: new FormControl(this.product.product_name, [
        Validators.required,
      ]),
      product_gid: new FormControl(''),

      product_desc: new FormControl(''),



      // productgroupname: new FormControl(this.product.productgroupname, [
      //   Validators.required,

      // ]),


      // productuomname: new FormControl(this.product.productuomname, [
      //   Validators.required,
      // ]),

      cost_price: new FormControl(''),
      mrp_price: new FormControl(''),
      product_image: new FormControl('')

    }


    );

    var api = 'OtlMstProduct/GetProductGroup';
    this.service.get(api).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.productgroup_list = result.GetProductGroup_list;
    });



    var api = 'OtlMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit_list;

    });

    this.GetEditProductSummary(deencryptedParam)
  }

  onChange2(event: any) {
    this.file = event.target.files[0];

  }


  GetEditProductSummary(product_gid: any) {
    debugger
    var url = 'OtlMstProduct/GetEditProductSummary'
    let param = { product_gid: product_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.editProductSummary_list = result.Product_listedit;
      console.log(this.product)
      console.log(this.editProductSummary_list)

      this.productform.get("product_gid")?.setValue(this.editProductSummary_list[0].product_gid);
      this.productform.get("product_code")?.setValue(this.editProductSummary_list[0].product_code);
      this.productform.get("product_name")?.setValue(this.editProductSummary_list[0].product_name);
      this.productform.get("mrp_price")?.setValue(this.editProductSummary_list[0].mrp_price);
      this.productform.get("cost_price")?.setValue(this.editProductSummary_list[0].cost_price);
      this.productform.get("product_desc")?.setValue(this.editProductSummary_list[0].product_desc);
      this.productform.get("product_image")?.setValue(this.editProductSummary_list[0].product_image);
      this.updateRemainingCharsedit();
    });
  }

  // get productgroupname() {
  //   return this.productform.get('productgroupname');
  // }

  // get productuomname() {
  //   return this.productform.get('productuomname');
  // }
  get productcodecontrol() {
    return this.productform.get('product_code');
  }
  get productnamecontrol() {
    return this.productform.get('product_name');
  }
  get costpricecontrol() {
    return this.productform.get('cost_price');
  }
  get mrpcontrol() {
    return this.productform.get('mrp_price');
  }
  get product_name() {
    return this.productform.get('product_name');
  }


  public validate(): void {
    debugger
    var params = {
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name,
      product_desc: this.productform.value.product_desc || '',
      sku: this.productform.value.sku,
      cost_price: this.productform.value.cost_price,
    }

    this.NgxSpinnerService.show();
    var url = 'OtlMstProduct/ProductUpdate';

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
                this.ToastrService.success(result.message)
                this.route.navigate(['/outlet/OtlMstProduct']);
              }
            });
          }
          else {
            this.NgxSpinnerService.hide();
            this.ToastrService.success(result.message)
            this.route.navigate(['/outlet/OtlMstProduct']);
          }

        }
      }
    })

  }
  updateRemainingCharsedit() {
    this.remainingChars = 1000 - this.productform.value.product_desc.length;
  }


}