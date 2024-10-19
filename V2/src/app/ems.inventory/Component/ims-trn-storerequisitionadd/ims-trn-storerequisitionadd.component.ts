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
  selector: 'app-ims-trn-storerequisitionadd',
  templateUrl: './ims-trn-storerequisitionadd.component.html',
  styleUrls: ['./ims-trn-storerequisitionadd.component.scss']
})
export class ImsTrnStorerequisitionaddComponent {
  isExpanded: { [key: string]: boolean } = {
    product: false,
    another: false,
    summary: false
  };
  POProductList1: any[] = [];
  productcodesearch1: any;
  productcodesearch: any;
  productcodesearch2: any;
  productsearch: any;
  productrol:any;
  productstock:any;
  product_desc:any;
  Getproductgroup: any;
  productgroup_list: any;
  imslocation_list: any[] = [];
  imscostenter_list: any;
  imsraiseMilist: any;
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
  RaiseSRForm: FormGroup | any;
  productsummary_list: any;
  productform: FormGroup | any;
  responsedata: any;
  mdlproducttype: any;
  parameterValue: string | undefined;
  IMSProductList1: any[] = [];
  filteredPOProductList1: any[] = [];
  qty_req : any;
  show() {
    const toggleBtn = document.getElementById('toggleBtn');
    const collapseContent = document.getElementById('collapseContent');
    toggleBtn?.addEventListener('click', () => {
      collapseContent?.classList.toggle('show');
    });
  }

  ngOnInit(): void {
    this.show();
    this.productSearch();
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);
    var api = 'ImsTrnRasieMI/GetMIsummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsraiseMilist = this.responsedata.imsraiseMilist;
      this.RaiseSRForm.get("user_firstname")?.setValue(result.imsraiseMilist[0].user_firstname);
      this.RaiseSRForm.get("branch_name")?.setValue(result.imsraiseMilist[0].branch_name);
      this.RaiseSRForm.get("department_name")?.setValue(result.imsraiseMilist[0].department_name);
    });
    var api = 'ImsTrnRasieMI/GetMIproducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsproducttype_list = this.responsedata.imsmiproducttype_list;
    });
    var productgroupapi = 'ImsTrnStoreRequisition/GetProductrolGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.getrolproductgroup_list;
    });
    this.IMSproductsummary();
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
  }
  onclearproduct() {
    this.productform.get("product_code").setValue('');
    this.productform.get("productgroup_name").setValue('');
    this.productform.get("productuom_name").setValue('');
    this.productform.get("product_desc").setValue('');
    this.productform.get("reorder_level").setValue('');
    this.productform.get("available_quantity").setValue('');
    this.productsearch = null;

  }
  onProductSelect(event: any): void {
    debugger;

    const product_name = this.IMSProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        productuom_name : product_name.productuom_name,
        product_desc : product_name.product_desc,
        reorder_level : product_name.reorder_level,
        available_quantity : product_name.available_quantity,

      });
    }
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {

    this.RaiseSRForm = new FormGroup({
      branch: new FormControl(''),
      branch_name: new FormControl(''),
      user_firstname: new FormControl(''),
      department_name: new FormControl(''),
      storerequisition_date: new FormControl(this.getCurrentDate(), Validators.required),
      costcenter_name: new FormControl(''),
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
      location_gid: new FormControl(''),
      location_name: new FormControl(''),
      storerequisition_remarks: new FormControl(''),
      materialrequisition_reference: new FormControl(''),
      priority: new FormControl('N'),
      reorder_level: new FormControl(''),
      available_quantity: new FormControl(''),
      expected_date: new FormControl(this.getCurrentDate(), Validators.required),
    })
    this.productform = new FormGroup({


      product_gid: new FormControl(''),

      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name : new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      quantity: new FormControl(''),
      product_desc: new FormControl(''),
      qty_requested: new FormControl(''),
      reorder_level : new FormControl(''),
      available_quantity : new FormControl(''),
    })

  }
  GetOnChangeProductsGroup() {
    debugger
    let productgroup_gid = this.productform.value.productgroup_name;
    let param = {
      productgroup_gid: productgroup_gid
    };

    var url = 'ImsTrnStoreRequisition/GetOnrolProductGroup';
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productgroup_list = result.getrolproduct_list;
      this.NgxSpinnerService.hide();
    });
    this.NgxSpinnerService.hide();
  }
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get costcenter_name() {
    return this.RaiseSRForm.get('costcenter_name')!;
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
    return this.RaiseSRForm.get('branch_name')!;
  }
  get department_name() {
    return this.RaiseSRForm.get('department_name')!;
  }
  get user_firstname() {
    return this.RaiseSRForm.get('user_firstname')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get available_quantity() {
    return this.productform.get('available_quantity')!;
  }
  get reorder_level() {
    return this.productform.get('reorder_level')!;
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
  OnChangeamount() {
    debugger
    let costcenter_gid = this.RaiseSRForm.get("costcenter_gid")?.value;
    if (costcenter_gid !== null && costcenter_gid !== undefined) {
      {
        let param = {
          costcenter_gid: costcenter_gid
        }
        var url = 'ImsTrnDirectIssueMaterial/DaGetimsavailable';
        this.service.getparams(url, param).subscribe((result: any) => {
          this.RaiseSRForm.get("available_amount")?.setValue(result.available_amount);
        });

      }
    }
  }
  productSearch() {

    // debugger
    // let vendor_gid = this.RaiseSRForm.get("branch_name")?.value;

    // var params = {
    //   producttype_gid: this.productform.value.producttype_name,
    //   product_name: this.productform.value.product_name,
    //   vendor_gid: vendor_gid
    // };

    var api = 'ImsTrnStoreRequisition/GetrolProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.IMSProductList1 = this.responsedata.getrolproduct1_list;
      this.filteredPOProductList1 = this.POProductList1;
    });
  }

  OnProductCode(event: any) {
    debugger
    const product_code = this.IMSProductList1.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        productgroup_name: product_code.productgroup_gid,
        productuom_name : product_code.productuom_name,
        reorder_level : product_code.reorder_level,
        available_quantity : product_code.available_quantity
      });
    }
  }
  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }
  SRProduct() {
    debugger
    this.toggleCollapsesection('section3');
    const api = 'ImsTrnStoreRequisition/PostOnSRproduct';
    this.NgxSpinnerService.show();
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    console.log(this.productform.value)
    if(this.productform.value.qty_requested!=0){

    
    const params = {
      product_gid: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name,
      productuom_name : this.productform.value.productuom_name,
      productgroup_gid: this.productform.value.productgroup_gid,
      productgroup_name: this.productform.value.productgroup_name,
      qty_requested: this.productform.value.qty_requested,
      product_desc: this.productform.value.product_desc,
      reorder_level: this.productform.value.reorder_level,
    };
    debugger;
    this.service.postparams(api, params).subscribe((result: any) => {
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
    this.productform.reset()
  }
  else{
    this.NgxSpinnerService.hide();
    this.ToastrService.warning("Requested Qty must be greater than zero");
  }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'ImsTrnStoreRequisition/DeletetmpProductSummary'
    this.NgxSpinnerService.show()
    let param = {
      tmpsr_gid: this.parameterValue
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
    debugger;
    var api = 'ImsTrnStoreRequisition/tmprolProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.tmpsrproduct_list;
      this.qtyrequested = this.responsedata.qty_requested.toFixed(2);
      this.productsummary_list.forEach((product: any) => {
      });
    });
  }


  onSubmit() {
    debugger
    if (this.productsummary_list == null || this.productsummary_list == undefined
    ) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
    var params = {
      tmpsrproduct_list: this.productsummary_list,
      storerequisition_date: this.RaiseSRForm.value.storerequisition_date,
      branch_name: this.RaiseSRForm.value.branch_name,
      department_name: this.RaiseSRForm.value.department_name,
      user_firstname: this.RaiseSRForm.value.user_firstname,
      storerequisition_remarks: this.RaiseSRForm.value.storerequisition_remarks,
    };
    var api = 'ImsTrnStoreRequisition/PostSR';
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
        this.router.navigate(['/ims/ImsTrnStorerequisition']);
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


}
