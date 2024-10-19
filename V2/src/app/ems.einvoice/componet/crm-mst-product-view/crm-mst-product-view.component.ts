import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';

interface IProduct {
  hsn_desc: string;
  hsn_code: string;
  product_gid: string;
  producttype_name: string;
  productgroup_name: string;
  productuomclass_name: string;
  productuom_name: string;
  product_code: string;
  product_name: string;
  cost_price: string;
  mrp: string;
  product_desc: string;
}

@Component({
  selector: 'app-crm-mst-product-view',
  templateUrl: './crm-mst-product-view.component.html',
  styleUrls: ['./crm-mst-product-view.component.scss']
})

export class CrmMstProductViewComponent {
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
  returnUrl?: string;
  reactiveForm!: FormGroup;
  submitted = false;
  Productgid: any;
  Product_list: any;
  // private fields
  responsedata: any;
  result: any;
  editProductSummary_list: any;
  ToastrService: any;

  constructor(private renderer: Renderer2, private el: ElementRef, public service: SocketService, private route: Router, private router: ActivatedRoute) {
    this.product = {} as IProduct;
  }

  ngOnInit(): void {
    const product_gid = this.router.snapshot.paramMap.get('product_gid');
    this.product_gid = product_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.product_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)

    this.productform = new FormGroup({
      product_code: new FormControl(this.product.product_code, [Validators.required]),
      product_name: new FormControl(this.product.product_name, [Validators.required]),
      product_gid: new FormControl(''),
      product_desc: new FormControl(''),
      producttype_name: new FormControl(this.product.producttype_name, [Validators.required]),
      productgroup_name: new FormControl(this.product.productgroup_name, [Validators.required]),
      productuomclass_name: new FormControl(this.product.productuomclass_name, [Validators.required]),
      productuom_name: new FormControl(this.product.productuom_name, [Validators.required]),
      cost_price: new FormControl(this.product.cost_price, [Validators.required]),
      mrp: new FormControl(this.product.mrp, [Validators.required]),
      hsn_code: new FormControl(this.product.hsn_code, [Validators.required]),
      hsn_desc: new FormControl(this.product.hsn_desc, [Validators.required]),
    });

    var api1 = 'Product/Getproductgroupdropdown';
    this.service.get(api1).subscribe((result: any) => {
      this.productgroup_list = result.Getproductgroupdropdown;
    });

    var api2 = 'Product/Getproducttypedropdown';
    this.service.get(api2).subscribe((result: any) => {
      this.producttype_list = result.Getproducttypedropdown;
    });

    var api3 = 'Product/Getproductunitclassdropdown';
    this.service.get(api3).subscribe((result: any) => {
      this.productunitclass_list = result.Getproductunitclassdropdown;
    });

    var api4 = 'Product/Getproductunitdropdown';
    this.service.get(api4).subscribe((result: any) => {
      this.productunit_list = result.Getproductunitdropdown;
    });
    this.GeteditProductSummary(deencryptedParam)
  }

  get productunitclasscontrol() {
    return this.productform.get('productuomclass_name');
  }

  get productunitscontrol() {
    return this.productform.get('productuom_name');
  }

  get mrpControl() {
    return this.productform.get('mrp');
  }

  get hsncodeControl() {
    return this.productform.get('hsn_code');
  }

  GeteditProductSummary(product_gid: any) {
    var url = 'EinvoiceProduct/editProductSummary'
    let param = { product_gid: product_gid }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.editProductSummary_list = result.editproductsummary_list;
      console.log(this.product)
      console.log(this.editProductSummary_list)

      this.productform.get("product_gid")?.setValue(this.editProductSummary_list[0].product_gid);
      this.productform.get("producttype_name")?.setValue(this.editProductSummary_list[0].producttype_name);
      this.productform.get("productgroup_name")?.setValue(this.editProductSummary_list[0].productgroup_name);
      this.productform.get("productuomclass_name")?.setValue(this.editProductSummary_list[0].productuomclass_name);
      this.productform.get("productuom_name")?.setValue(this.editProductSummary_list[0].productuom_name);

      this.selectedProducttype = this.editProductSummary_list[0].producttype_gid;
      this.selectedProductgroup = this.editProductSummary_list[0].productgroup_gid;
      this.selectedUnitclass = this.editProductSummary_list[0].productuom_gid;
      this.selectedUnits = this.editProductSummary_list[0].productuomclass_gid;
      this.productform.get("product_code")?.setValue(this.editProductSummary_list[0].product_code);
      this.productform.get("product_name")?.setValue(this.editProductSummary_list[0].product_name);
      this.productform.get("mrp")?.setValue(this.editProductSummary_list[0].mrp);

      this.productform.get("cost_price")?.setValue(this.editProductSummary_list[0].cost_price);
      this.productform.get("product_desc")?.setValue(this.editProductSummary_list[0].product_desc);
      this.productform.get("hsn_code")?.setValue(this.editProductSummary_list[0].hsn_code);
      this.productform.get("hsn_desc")?.setValue(this.editProductSummary_list[0].hsn_desc);
    });
  }

  get producttypename() {
    return this.productform.get('producttype_name');
  }
  get productgroupname() {
    return this.productform.get('productgroup_name');
  }
  get productuomclassname() {
    return this.productform.get('productuomclass_name');
  }
  get productuomname() {
    return this.productform.get('productuom_name');
  }
  get productcodecontrol() {
    return this.productform.get('product_code');
  }
  get productnamecontrol() {
    return this.productform.get('product_name');
  }
  get mrpcontrol() {
    return this.productform.get('mrp');
  }
  get costpricecontrol() {
    return this.productform.get('cost_price');
  }

  public validate(): void {
    this.product = this.productform.value;
    console.log(this.productform)
    if (this.product.product_code != null && this.product.product_name != null && this.product.producttype_name != null && this.product.productgroup_name != null
      && this.product.productuomclass_name != null && this.product.productuom_name != null &&
      this.product.cost_price != null && this.product.product_desc != null) {
      console.log(this.productform.value)
      const api = 'Product/UpdatedProduct';
      this.service.post(api, this.productform.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.route.navigate(['/einvoice/CrmMstProduct']);
          this.ToastrService.success(result.message)
        }
        this.responsedata = result;
      });
    }
  }
}