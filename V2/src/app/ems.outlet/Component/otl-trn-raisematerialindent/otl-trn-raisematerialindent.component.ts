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
  selector: 'app-otl-trn-raisematerialindent',
  templateUrl: './otl-trn-raisematerialindent.component.html',
  styleUrls: ['./otl-trn-raisematerialindent.component.scss']
})
export class OtlTrnRaisematerialindentComponent {
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
  mdlUserName: any;
  user_list: any;

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
  RaiseMIForm: FormGroup | any;
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
      // Toggle the 'show' class on the collapse content element
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
    this.IMSproductsummary();
    var api = 'ImsTrnRasieMI/GetMIsummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsraiseMilist = this.responsedata.imsraiseMilist;
      // this.RaiseMIForm.get("user_firstname")?.setValue(result.imsraiseMilist[0].user_firstname);
      this.RaiseMIForm.get("branch_name")?.setValue(result.imsraiseMilist[0].branch_name);
      this.RaiseMIForm.get("department_name")?.setValue(result.imsraiseMilist[0].department_name);
    });
    var api = 'ImsTrnRasieMI/GetMIproducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsproducttype_list = this.responsedata.imsmiproducttype_list;
    });
    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });
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
    debugger

    const product_name = this.IMSProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        product_unit : product_name.productuom_name,
        
      });
    }
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {

    this.RaiseMIForm = new FormGroup({
      branch: new FormControl(''),
      branch_name: new FormControl(''),
      // user_firstname: new FormControl(''),
      employee_name: new FormControl(''),
      department_name: new FormControl(''),
      materialissued_date: new FormControl(this.getCurrentDate(), Validators.required),
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
      materialrequisition_remarks: new FormControl(''),
      materialrequisition_reference: new FormControl(''),
      priority: new FormControl('N'),
      expected_date: new FormControl(this.getCurrentDate(), Validators.required),
    })
    this.productform = new FormGroup({


      product_gid: new FormControl(''),

      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productcode: new FormControl(''),
      productgroup: new FormControl(''),
      product_unit: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_name: new FormControl(''),
      quantity: new FormControl(''),
      display_field: new FormControl(''),
      qty_requested: new FormControl(''),
    })

  }
  GetOnChangeProductsGroup() {
    debugger
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
  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
    return dd + '-' + mm + '-' + yyyy;
  }
  get costcenter_name() {
    return this.RaiseMIForm.get('costcenter_name')!;
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
    return this.RaiseMIForm.get('branch_name')!;
  }
  get department_name() {
    return this.RaiseMIForm.get('department_name')!;
  }
  // get user_firstname() {
  //   return this.RaiseMIForm.get('user_firstname')!;
  // }
  get employee_name() {
    return this.RaiseMIForm.get('employee_name')!;
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
  OnChangeamount() {
    debugger
    let costcenter_gid = this.RaiseMIForm.get("costcenter_gid")?.value;
    if (costcenter_gid !== null && costcenter_gid !== undefined) {
      {
        let param = {
          costcenter_gid: costcenter_gid
        }
        var url = 'ImsTrnDirectIssueMaterial/DaGetimsavailable';
        this.service.getparams(url, param).subscribe((result: any) => {
          this.RaiseMIForm.get("available_amount")?.setValue(result.available_amount);
        });

      }
    }
  }
  productSearch() {

    debugger
    let vendor_gid = this.RaiseMIForm.get("branch_name")?.value;

    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
      vendor_gid: vendor_gid
    };

    var api = 'ImsTrnRasieMI/GetMIProductSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.IMSProductList1 = this.responsedata.imsMIproductsummarylist;
      this.filteredPOProductList1 = this.POProductList1;
    });
    var api = 'PmrTrnPurchaseOrder/Getuser';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.user_list = result.Getuser;
    });
  }

  OnProductCode(event: any) {
    debugger
    const product_code = this.IMSProductList1.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_gid,
        productgroup_name: product_code.productgroup_gid,
        product_unit : product_code.productuom_name,
       
      });
    }
  }
  searchOnChange(event: KeyboardEvent) {
    if (event.key !== 'Enter') {
      this.productSearch();
    }
  }
  MIProduct() {


    this.toggleCollapsesection('section3');
    const api = 'ImsTrnRasieMI/PostOnmiproduct';
    this.NgxSpinnerService.show();
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    console.log(this.productform.value)
    const params = {
      product_gid: this.productform.value.product_name,
      product_code: this.productform.value.product_code,
      product_name: this.productform.value.product_name,
      product_unit : this.productform.value.product_unit,
      productgroup_gid: this.productform.value.productgroup_gid,
      productgroup_name: this.productform.value.productgroup_name,
      qty_requested: this.productform.value.qty_requested,
      display_field: this.productform.value.display_field,

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

  productSubmit() {
    this.temptable = [];
    this.toggleCollapsesection('section3');
    const api = 'ImsTrnRasieMI/PostMIProduct';
    this.NgxSpinnerService.show();
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    let j = 0;
    debugger;
    for (let i = 0; i < this.IMSProductList1.length; i++) {
      if (this.IMSProductList1[i].qty_requested != null) {
        this.temptable.push(this.IMSProductList1[i]);
        j++;
      }
    }
    const params = {
      type: "Multiple",
      imsproductMI_list: this.temptable,
    };
    if (params.imsproductMI_list === null || params.imsproductMI_list.length === 0) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('Please Enter the Quantity!');
      return;
    }
    else {
      console.log(params)
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
    }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    var url = 'ImsTrnRasieMI/DeletetmpProductSummary'
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
    debugger;
    var api = 'ImsTrnRasieMI/tmpProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.tmpproductlist;
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
      tmpproductsummary_list: this.productsummary_list,
      materialissued_date: this.RaiseMIForm.value.materialissued_date,
      expected_date: this.RaiseMIForm.value.expected_date,
      branch_name: this.RaiseMIForm.value.branch_name,
      department_name: this.RaiseMIForm.value.department_name,
      // user_firstname: this.RaiseMIForm.value.user_firstname,
      employee_name: this.RaiseMIForm.value.employee_name,
      location_name: this.RaiseMIForm.value.location_name,
      costcenter_name: this.RaiseMIForm.value.costcenter_name,
      available_amount: this.RaiseMIForm.value.available_amount,
      materialrequisition_remarks: this.RaiseMIForm.value.materialrequisition_remarks,
      materialrequisition_reference: this.RaiseMIForm.value.materialrequisition_reference,
      priority: this.RaiseMIForm.value.priority,

    };
    var api = 'ImsTrnRasieMI/MaterialIndent';
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
        this.router.navigate(['/outlet/OtlTrnMaterialindent']);
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
  onclearrequestor(){
    this.mdlUserName ='';
    

    }
    OnChangerequestor(){}


}
