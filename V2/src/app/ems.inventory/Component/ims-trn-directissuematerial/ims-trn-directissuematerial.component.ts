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
  selector: 'app-ims-trn-directissuematerial',
  templateUrl: './ims-trn-directissuematerial.component.html',
  styleUrls: ['./ims-trn-directissuematerial.component.scss']
})
export class ImsTrnDirectissuematerialComponent {
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
  display_field:any;
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
  IssuematerialForm: FormGroup | any;
  productsummary_list: any;
  productform: FormGroup | any;
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

    var api = 'ImsTrnDirectIssueMaterial/Getimslocation';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imslocation_list = this.responsedata.imslocation_list;
    });
    // var api = 'ImsTrnDirectIssueMaterial/Getimscostcenter';
    // this.service.get(api).subscribe((result: any) => {
    //    this.responsedata = result;
    //   this.imscostenter_list = this.responsedata.imscostenter_list;
    // });
    var api = 'ImsTrnDirectIssueMaterial/Getimsdisummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsdirectissue_list = this.responsedata.imsdirectissue_list;
      //this.IssuematerialForm.get("user_firstname")?.setValue(result.imsdirectissue_list[0].user_firstname);
      this.IssuematerialForm.get("branch_name")?.setValue(result.imsdirectissue_list[0].branch_name);
      this.IssuematerialForm.get("department_name")?.setValue(result.imsdirectissue_list[0].department_name);
    });
    var api = 'ImsTrnDirectIssueMaterial/Getimsproducttype';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.imsproducttype_list = this.responsedata.imsproducttype_list;
    });
    var productgroupapi = 'SmrTrnSalesorder/GetProductGroup';
    this.service.get(productgroupapi).subscribe((apiresponse: any) => {
      this.Getproductgroup = apiresponse.Getproductgroup;
    });
    var api = 'PmrTrnPurchaseOrder/Getuser';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.user_list = result.Getuser;
    });
  }
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService, public NgxSpinnerService: NgxSpinnerService) {

    this.IssuematerialForm = new FormGroup({
      branch: new FormControl(''),
      branch_name: new FormControl('', Validators.required),
      employee_name: new FormControl(''),
      department_name: new FormControl(''),
      materialissued_date: new FormControl(this.getCurrentDate(), Validators.required ),
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
      stock_quantity: new FormControl(''),
      location_gid: new FormControl(''),
      location_name: new FormControl(''),
      available_amount: new FormControl(''),
      materialrequisition_remarks: new FormControl(''),
      materialrequisition_reference: new FormControl(''),
    })
    this.productform = new FormGroup({
      tmppurchaseorderdtl_gid: new FormControl(''),
      branch_name: new FormControl(''),
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
    return this.IssuematerialForm.get('branch_name')!;
  }
  get department_name() {
    return this.IssuematerialForm.get('department_name')!;
  }
  get user_firstname() {
    return this.IssuematerialForm.get('user_firstname')!;
  }
  get employee_name() {
    return this.IssuematerialForm.get('employee_name')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
  }
  get producttype_name() {
    return this.productform.get('producttype_name')!;
  }
  OnChangeamount() {
     
    let costcenter_gid = this.IssuematerialForm.get("costcenter_gid")?.value;
    if (costcenter_gid !== null && costcenter_gid !== undefined) {
      {
        let param = {
          costcenter_gid: costcenter_gid
        }
        var url = 'ImsTrnDirectIssueMaterial/DaGetimsavailable';
        this.service.getparams(url, param).subscribe((result: any) => {
          this.IssuematerialForm.get("available_amount")?.setValue(result.available_amount);
        });

      }
    }
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
  IssueMaterial() {
      
    if (this.productform.value.qty_requested > this.productform.value.stock_quantity) {
      this.ToastrService.warning('Requested Quantity should not be higher than available stock!');
      return
    }
    else {
      this.toggleCollapsesection('section3');
      const api = 'ImsTrnDirectIssueMaterial/PostOnIssuematerial';
      this.NgxSpinnerService.show();
      const spinnerTimer = setTimeout(() => {
        this.NgxSpinnerService.hide();
      }, 3000);
      debugger
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
          this.productform.reset();
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

  productSubmit() {
    this.temptable = [];
    this.toggleCollapsesection('section3');
    const api = 'ImsTrnDirectIssueMaterial/PostIssuematerial';
    this.NgxSpinnerService.show();
    const spinnerTimer = setTimeout(() => {
      this.NgxSpinnerService.hide();
    }, 3000);
    let j = 0;
     ;
    for (let i = 0; i < this.IMSProductList1.length; i++) {
      if (this.IMSProductList1[i].qty_requested > this.IMSProductList1[i].stock_quantity) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning('Requested Quantity should not be higher than available stock!');
        return
      }
      else {
        if (this.IMSProductList1[i].qty_requested != null) {
          this.temptable.push(this.IMSProductList1[i]);
          j++;
        }
      }
    }
    const params = {
      type: "Multiple",
      imsproductissue_list: this.temptable,
    };
    if (params.imsproductissue_list === null || params.imsproductissue_list.length === 0) {
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
    var api = 'ImsTrnDirectIssueMaterial/GettmpProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = this.responsedata.tmpproductsummary_list;
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
      tmpproductsummary_list: this.productsummary_list,
      materialissued_date: this.IssuematerialForm.value.materialissued_date,
      branch_name: this.IssuematerialForm.value.branch_name,
      department_name: this.IssuematerialForm.value.department_name,
      user_firstname: this.IssuematerialForm.value.user_firstname,
      employee_name: this.IssuematerialForm.value.employee_name,
      location_name: this.IssuematerialForm.value.location_name,
      costcenter_name: this.IssuematerialForm.value.costcenter_name,
      available_amount: this.IssuematerialForm.value.available_amount,
      materialrequisition_remarks: this.IssuematerialForm.value.materialrequisition_remarks,
      materialrequisition_reference: this.IssuematerialForm.value.materialrequisition_reference,
    };
    var api = 'ImsTrnDirectIssueMaterial/MaterialIssue';
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
        this.router.navigate(['/ims/ImsTrnIssuematerialSummary']);
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
    this.productform.get("productuom_name").setValue('');
    this.productform.get("display_field").setValue('');
    this.productsearch = null;

  }
  onProductSelect(event: any): void {
     
debugger;
    const product_name = this.IMSProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        productuom_name: product_name.productuom_name,
        stock_quantity: product_name.stock_quantity,
        display_field: product_name.display_field,
      });
      
    }
  }

  
  onclearrequestor(){
    this.mdlUserName ='';
    

    }
    OnChangerequestor(){}
}
