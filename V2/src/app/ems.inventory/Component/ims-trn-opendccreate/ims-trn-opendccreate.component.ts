
import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { NgxSpinnerService } from 'ngx-spinner';

interface CollapseState {
  [key: string]: boolean;
}

@Component({
  selector: 'app-ims-trn-opendccreate',
  templateUrl: './ims-trn-opendccreate.component.html',
  styleUrls: ['./ims-trn-opendccreate.component.scss']
})
export class ImsTrnOpendccreateComponent {
  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
  };
  imslocation_list: any;
  imscostenter_list: any;
  imsdirectissue_list: any;
  imsproducttype_list: any;
  qtyrequested: any;
  sam: boolean = false;
  formBuilder: any;
  arrowfst: boolean = false;
  arrowOne: boolean = false;
  temptable: any[] = [];

  toggleExpand(section: string) {
    this.isExpanded[section] = !this.isExpanded[section];
  }
  showInput: boolean = false;
  inputValue: string = ''
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '33rem',
    minHeight: '0rem',
    placeholder: 'Enter text here...',
    translate: 'no',
  };
  opendcForm: FormGroup | any;
  productsummary_list: any;
  productform: FormGroup | any;
  branch_list:any;
  responsedata: any;
  mdlproducttype: any;
  parameterValue: string | undefined;
  IMSProductList1: any[] = [];
  productgroup_list: any[] = [];
  productcodesearch1: any;
  productcodesearch: any;
  productsearch: any;
  Getproductgroup: any;
  productquantity: any;
  productdiscount_amountvalue : any;
  productdiscount:any;
  data:any
  show() {
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      // Toggle the 'show' class on the collapse content element
      collapseContent?.classList.toggle('show');
    });
  }

  ngOnInit(): void {
    this.show();

    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    this.IMSproductsummary();
    this.productSearch()
    var api = 'ImsTrnOpenDcSummary/GetImsTrnDCBranch';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.branch_list = this.responsedata.dcbranch_list;
    });
    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {

    this.opendcForm = new FormGroup({
      branch: new FormControl('', Validators.required),
      branch_name: new FormControl('', Validators.required),
      user_firstname: new FormControl(''),
      department_name: new FormControl(''),
      directorder_date: new FormControl(this.getCurrentDate(), Validators.required),
      costcenter_name: new FormControl('', Validators.required),
      remarks: new FormControl(''),
      productuom_name: new FormControl(''),
      product_code: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
      quantity: new FormControl(''),
      display_name: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      qty_requested: new FormControl(''),
      stock_quantity: new FormControl(''),
      location_gid: new FormControl(''),
      location_name: new FormControl(''),
      available_amount: new FormControl(''),
      tracker_id: new FormControl('',Validators.required),
      dc_note: new FormControl(''),
      no_of_boxs: new FormControl('',Validators.required),
      mode_of_despatch: new FormControl('',Validators.required),
      shipping_to: new FormControl('',Validators.required),
     
    })
    this.productform = new FormGroup({
      tmppurchaseorderdtl_gid: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      product_gid: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      display_name: new FormControl(''),
      productname: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      quantity: new FormControl(''),
      producttype_name: new FormControl(''),
      producttype_gid: new FormControl(''),
      qty_requested: new FormControl(''),
      stock_quantity: new FormControl(''),
      display_field : new FormControl('')
    })

  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get costcenter_name() {
    return this.opendcForm.get('costcenter_name')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get stock_quantity() {
    return this.productform.get('stock_quantity')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get branch_name() {
    return this.opendcForm.get('branch_name')!;
  }
  get department_name() {
    return this.opendcForm.get('department_name')!;
  }
  get user_firstname() {
    return this.opendcForm.get('user_firstname')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get display_name() {
    return this.productform.get('display_name')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }
  get tracker_id() {
    return this.opendcForm.get('tracker_id')!;
  }
  get customer_mode() {
    return this.opendcForm.get('customer_mode')!;
  }
  get no_of_boxs() {
    return this.opendcForm.get('no_of_boxs')!;
  }
  get dc_no() {
    return this.opendcForm.get('dc_no')!;
  }
  get mode_of_despatch() {
    return this.opendcForm.get('mode_of_despatch')!;
  }
  get shipping_to() {
    return this.opendcForm.get('shipping_to')!;
  }

  productSearch() {
    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
    };
      
    var api = 'ImsTrnDirectIssueMaterial/GetImsProductSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.IMSProductList1 = this.responsedata.imsproductsummary_list;
    
    });
  }
  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }
  AddProduct() {
      
    if (this.productform.value.qty_requested > this.productform.value.stock_quantity) {
      this.ToastrService.warning('Requested Quantity should not be higher than available stock!');
      return
    }
    else {
      this.toggleCollapsesection('section3');
      var url = 'ImsTrnOpenDcSummary/PostDcproduct'
      this.NgxSpinnerService.show();
      const spinnerTimer = setTimeout(() => {
        this.NgxSpinnerService.hide();
      }, 3000);
      const params = {
        product_gid: this.productform.value.product_gid,
        product_code: this.productform.value.product_code,
        product_name: this.productform.value.product_name,
        productgroup_gid: this.productform.value.productgroup_gid,
        productgroup_name: this.productform.value.productgroup_name,
        productuom_gid: this.productform.value.productuom_gid,
        productuom_name: this.productform.value.productuom_name,
        qty_requested: this.productform.value.qty_requested,
        display_field: this.productform.value.display_field,
        stock_quantity: this.productform.value.stock_quantity,
      };
       ;
      this.service.postparams(url, params).subscribe((result: any) => {
        clearTimeout(spinnerTimer);
        if (result.status == false) {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.warning(result.message);
        } else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message);
          this.IMSProductList1 = [];

          this.IMSproductsummary();
          this.productSearch();
          this.productform.reset();
        }
        this.NgxSpinnerService.hide();
      });
      const toggleBtn = document.getElementById('toggleBtn');
      const collapseContent = document.getElementById('collapseContent');
      toggleBtn?.addEventListener('click', () => {
        collapseContent?.classList.toggle('show');
      });
      this.IMSproductsummary();
      this.productSearch();
      this.productform.reset();
    }
  }

  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'ImsTrnDirectIssueMaterial/DeletetmpProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpmaterialrequisition_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide()
      }
      else {

        this.ToastrService.success(result.message)
        this.IMSproductsummary();
        this.productSearch();
        this.NgxSpinnerService.hide()
      }
    });
  }
  IMSproductsummary() {
    debugger
    var api = 'ImsTrnOpenDcSummary/GettmpdcProduct';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.tmpdcproduct_list;
      // this.qtyrequested = this.responsedata.qty_requested;
      // this.productsummary_list.forEach((product: any) => {
      // });
    });
  }
  onSubmit() {
     
    if (this.productsummary_list == null || this.productsummary_list == undefined
    ) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    var params = {
      tmpdcproduct_list: this.productsummary_list,
      directorder_date: this.opendcForm.value.directorder_date,
      branch_name: this.opendcForm.value.branch_name,
      shipping_to: this.opendcForm.value.shipping_to,
      mode_of_despatch: this.opendcForm.value.mode_of_despatch,
      tracker_id: this.opendcForm.value.tracker_id,
      no_of_boxs: this.opendcForm.value.no_of_boxs,
      dc_note: this.opendcForm.value.dc_note,
    };
    var api = 'ImsTrnOpenDcSummary/openDCSubmit';
    this.NgxSpinnerService.show()
    this.service.postparams(api, params).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide()
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide()
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.router.navigate(['/ims/ImsTrnOpendc']);
      }
      this.NgxSpinnerService.hide()
    });

  }

  sample() {
    this.sam = !this.sam;
  }
  arrow() {
    this.arrowfst = !this.arrowfst;
  }
  arrowone() {
    this.arrowOne = !this.arrowOne;
  }
  isCollapsed: CollapseState = {
    section1: true,
    section2: true,
    section3: true,
  };
  toggleCollapse(section: string) {

    this.isCollapsed[section] = !this.isCollapsed[section];
  }

  toggleCollapsesection(section: string) {
    this.isCollapsed[section] = false;
  }

  GetOnChangeProductsGroup() {
      
    let productgroup_gid = this.productform.value.productgroup_name;
    let param = {
      productgroup_gid: productgroup_gid
    };

    var url = 'SmrTrnSalesorder/GetOnChangeProductGroup';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productgroup_list = result.GetCustomer;
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }

  OnProductCode(event: any) {
     
    const product_code = this.IMSProductList1.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        productgroup_name: product_code.productgroup_gid,
        productuom_name: product_code.productuom_name,
        stock_quantity: product_code.stock_quantity,
      });
    }
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
  }
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("productgroup_name").setValue('');
    this.productsearch = null;

  }
  onProductSelect(event: any): void {
     

    const product_name = this.IMSProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        productuom_name: product_name.productuom_name,
        stock_quantity: product_name.stock_quantity,
      });
      
    }
  }
}
