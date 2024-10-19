import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options';
import { NgxSpinnerService } from 'ngx-spinner';

interface RaisePR
{
  stocktransfer_gid: string;
  stocktransfer_date: string;
  location_name:string;
  locationto_name:string;
  product_gid: string;
  product_name: string;
  productgroup_name: string;
  product_code: string;
  productuom_name: string;
  qty_requested: string;
  stock_quantity: string;
}

@Component({
  selector: 'app-ims-trn-stocktransfer-location',
  templateUrl: './ims-trn-stocktransfer-location.component.html',
  styleUrls: ['./ims-trn-stocktransfer-location.component.scss']
})
export class ImsTrnStocktransferLocationComponent {

  RaiseRequestForm: FormGroup | any; 
  productform: FormGroup | any;
  raisepr!: RaisePR;
  costcenter_list: any [] = [];
  branch_list: any [] = [];
  locationto_list:any[]=[];
  location_list:any[]=[];
  responsedata: any;
  product_list: any [] = [];
  products_list: any [] = [];
  parameterValue: any;
  productcode_list: any;
  productgroup_list: any;
  productunit_list: any;
  productdetails_list: any;
  productsummary_list1: any;
  POproductlist: any;
  Getuserdtl: any [] = [];
  Getcostcenter: any [] = [];
  mdlBranchName: any;
  mdlproduct: any;
  mdlproductdes: any;
  mdlproductuomname: any;
  mdlproductcode: any;
  mdlproductgroup: any;
  mdlUserName: any;
  user_list: any;
  productcodesearch1: any;
  productcodesearch: any;
  productsearch: any;
  IMSProductList1 : any[]=[]
  filteredPOProductList1: any[] = [];
  POProductList1: any[] = [];
  mdllocationName:any;
  mdllocationtoName:any;
  stockgid:any;
  GetPopsummary_list :any[]=[];
  stock_gid:any;
  stock_quantity : any[]=[];
  response_data :any;
  GetPop_list : any[]=[]
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route :ActivatedRoute,private router:Router,public service: SocketService ,public NgxSpinnerService:NgxSpinnerService) {
    this.raisepr = {} as RaisePR;
    
  }
 
  ngOnInit(): void 
  
  {
    this.POproductsummary();
  
    this.RaiseRequestForm = new FormGroup({
      stocktransfer_gid: new FormControl(''),
      stocktranfer_date: new FormControl(this.getCurrentDate()),
      branch_name: new FormControl(''),
      location_name:new FormControl(''),
      locationto_name:new FormControl(''), 
    });
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
//location from

    var api = 'ImsTrnStockTransferSummary/GetLocationTo';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.locationto_list = this.responsedata.LocationTo;
      this.RaiseRequestForm.get("branch_gid")?.setValue(this.locationto_list[0].branch_gid);

    });
//location to
    var url = 'ImsTrnStockTransferSummary/GetLocation'
    this.service.get(url).subscribe((result: any) => {
      this.location_list = result.Location;
      this.RaiseRequestForm.get("location_gid")?.setValue(result.Location[0].location_gid);

    });

   

    


    // var api = 'PmrTrnPurchaseRequisition/GetProductCode1';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.productcode_list = this.responsedata.GetProductCode1;

    // });


    var api = 'PmrMstProduct/GetProductUnit';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productunit_list = this.responsedata.GetProductUnit;

    });
    // var api = 'PmrMstProduct/GetProductGroup';
    // this.service.get(api).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.productgroup_list = this.responsedata.GetProductGroup;
    //   setTimeout(() => {

    //     $('#productgroup_list').DataTable();

    //   }, 0.1);
      var api = 'PmrTrnPurchaseOrder/Getuser';
      this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.user_list = result.Getuser;
    });
   
    // });
    
    
    
    
    
   
        
    this.productform = new FormGroup({
      product_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      product_name: new FormControl(''),
      productuom_gid: new FormControl(''),
      productgroup_gid: new FormControl(''),
      product_code: new FormControl(''),
      productgroup: new FormControl(''),
      productuom_name: new FormControl(''),
      productname: new FormControl(''),
      qty_requested: new FormControl(''),
      display_field: new FormControl(''),
      total_amount:new FormControl(''),
      stock_quantity:new FormControl(''),
    })
    this.RaiseRequestForm = new FormGroup({
      stocktransfer_gid: new FormControl(''),
      stocktransfer_date: new FormControl(this.getCurrentDate()),
      branch_name: new FormControl(''),
      location_name:new FormControl(''),
      locationto_name:new FormControl(''),
      remarks: new FormControl(''),
    })
    
  }

  getCurrentDate(): string {
    const today = new Date();
    const dd = String(today.getDate()).padStart(2, '0');
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // January is 0!
    const yyyy = today.getFullYear();
   
    return dd + '-' + mm + '-' + yyyy;
  }
  get stocktransfer_date() {
    return this.RaiseRequestForm.get('stocktransfer_date')!;
  }
  get location_name(){
    return this.RaiseRequestForm.get('location_name')!;
  }
  get locationto_name(){
    return this.RaiseRequestForm.get('locationto_name')!;
  }
  get product_name() {
    return this.productform.get('product_name')!;
  }
  get product_code() {
    return this.productform.get('product_code')!;
  }
  get productuom_name() {
    return this.productform.get('productuom_name')!;
  }
  get productgroup_name() {
    return this.productform.get('productgroup_name')!;
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
  onProductSelect(event: any): void {
    
    const product_name = this.IMSProductList1.find(product => product.product_gid === event.product_gid);
    if (product_name) {
      this.productform.patchValue({
        product_code: product_name.product_code,
        productgroup_name: product_name.productgroup_gid,
        productuom_name : product_name.productuom_name
      });
    }
  }
  OnProductCode(event: any) {
    
    const product_code = this.IMSProductList1.find(product => product.product_code === event.product_code);
    if (product_code) {
      this.productform.patchValue({
        product_name: product_code.product_name,
        productgroup_name: product_code.productgroup_name,
        productuom_name : product_code.productuom_namez
      });
    }
  }
  //location from based product search
  productSearch() {

    
    let vendor_gid = this.RaiseRequestForm.get("branch_name")?.value;

    var params = {
      producttype_gid: this.productform.value.producttype_name,
      product_name: this.productform.value.product_name,
    };

    var api = 'ImsTrnDirectIssueMaterial/GetImsProductSummary';
    this.service.getparams(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.IMSProductList1 = this.responsedata.imsproductsummary_list;
      this.filteredPOProductList1 = this.POProductList1;
    });
  }
  onclearproductcode() {
    this.productsearch = null;
    this.productcodesearch = null;
  }

  GetOnChangecostcenter() {
    
    let costcenter_gid = this.RaiseRequestForm.value.costcenter_name
    let param = {
      costcenter_gid: costcenter_gid
    }
    var url = 'PmrTrnPurchaseRequisition/GetOnChangecostcenter';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.RaiseRequestForm.get("available_amount")?.setValue(result.Getcostcenter[0].available_amount);
 
    });
  }

  GetOnChangeProductName(){
    debugger;
    let product_gid = this.productform.value.product_name.product_gid;
    let param = {
      product_gid: product_gid
    }
    var url = 'ImsTrnDirectIssueMaterial/GetImsProduct';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.productform.get("product_code")?.setValue(result.imsproductsummary_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.imsproductsummary_list[0].productuom_name);
      this.productform.get("productgroup_name")?.setValue(result.imsproductsummary_list[0].productgroup_name);
      this.productform.get("display_field")?.setValue(result.imsproductsummary_list[0].display_field);
      this.productform.get("stock_quantity")?.setValue(result.imsproductsummary_list[0].stock_quantity);
      this.productform.value.productgroup_gid = result.imsproductsummary_list[0].productgroup_gid,
        this.productform.value.productuom_gid = result.imsproductsummary_list[0].productuom_gid
    });

  }
  onClearlocation(){
    this.mdlproduct = '';
  }
  GetOnChangeLocation() {
    
    
    debugger;
    let location_gid = this.RaiseRequestForm.value.location_name.location_gid;
    let param = {
      location_gid: location_gid
    }
    var url = 'ImsTrnStockTransferSummary/GetProduct1';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productdetails_list = this.responsedata.GetProduct1;
      this.productform.get("product_name")?.setValue(result.productdetails_list[0].product_name)
    });
      var api = 'ImsTrnStockTransferSummary/GetProductCode1';
      this.service.getparams(api,param).subscribe((result: any) => {
        this.responsedata = result;
        this.productcode_list = this.responsedata.GetProductCode1;
      this.productform.get("product_code")?.setValue(result.productdetails_list[0].product_code)

    });
    var api = 'ImsTrnStockTransferSummary/GetProductgroup';
    this.service.getparams(api,param).subscribe((result: any) => {
      this.responsedata = result;
      this.productgroup_list = this.responsedata.GetProductgroup;
      this.productform.get("productgroup_name")?.setValue(result.productgroup_list[0].productgroup_name)
    });
  }

  
  GetPopsummary() {
    
    
    debugger;
    let location_gid = this.RaiseRequestForm.value.location_name.location_gid;
    let product_gid = this.productform.value.product_name.product_gid;
    let productuom_gid = this.productform.value.productuom_gid;
    let param = {
      location_gid: location_gid,
      product_gid: product_gid,
      productuom_gid:productuom_gid
    }
    var url = 'ImsTrnStockTransferSummary/GetPopsummary';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productdetails_list = this.responsedata.GetPopsummary_list;
      this.productform.get("product_name")?.setValue(result.productdetails_list[0].product_name);
      this.productform.get("product_code")?.setValue(result.productdetails_list[0].product_code);
      this.productform.get("productuom_name")?.setValue(result.productdetails_list[0].productuom_name);
      this.productform.get("stock_qty")?.setValue(result.productdetails_list[0].stock_qty);
      this.productform.get("display_field")?.setValue(result.productdetails_list[0].display_field);
      this.productform.get("stock_gid")?.setValue(result.productdetails_list[0].stock_gid);

      
    });
  }


onclearproduct(){
  this.mdlproduct=""
  this.mdlproductuomname=""
  this.mdlproductcode=""
  this.mdlproductdes=""
  this.mdlproductgroup=""
  this.productcodesearch1=""
  this.productsearch=""

}
 // product submit stock transfer
  productSubmit(){
    
    console.log(this.productform.value)
    var params = {    
    
      product_name: this.productform.value.product_name.product_name,
      product_gid: this.productform.value.product_name.product_gid,
      qty_requested: this.productform.value.qty_requested,
      productgroup_gid: this.productform.value.productgroup_gid,
      productgroup_name: this.productform.value.productgroup_name,
      product_code: this.productform.value.product_code,
      productuom_gid: this.productform.value.productuom_gid,
      productuom_name: this.productform.value.productuom_name, 
      display_field: this.productform.value.display_field,   
      stock_quantity: this.productform.value.stock_quantity,   
    }
    console.log(params)
    var api = 'ImsTrnStockTransferSummary/PostOnAddpr';
    this.NgxSpinnerService.show();
    this.service.post(api, params).subscribe((result: any) => {
    if(result.status ==false){
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();
    }
    else{
      
      this.ToastrService.success(result.message)
      this.productform.reset();
      this.NgxSpinnerService.hide();
      this.IMSProductList1 = [];
      this.POproductsummary()
      
    }
    },
    );

  }
  

  
  onclose1(){
    this.productform.reset();
  }

  onclose()
  {
    this.router.navigate(['/pmr/PmrTrnPurchaseRequisition']);  

  }
  POproductsummary() {
    
    var api = 'ImsTrnStockTransferSummary/GetProductSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list1 = this.responsedata.productsummary_list1;
      
      this.productSearch();
    });
  
}
openModaldelete(parameter: string){
  this.parameterValue = parameter
}
ondelete(){
  var url = 'ImsTrnStockTransferSummary/GetDeletePrProductSummary'
  this.NgxSpinnerService.show();
  let param = {
    tmpstocktransfer_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if(result.status ==false){
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();
    }
    else{
      
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      
    }
    
    this.POproductsummary();


  });
}
GetStockQuantity(){
debugger
let stock_gid = this.productdetails_list[0].stock_gid;
let product_gid =this.productdetails_list[0].product_gid;
  var api = 'ImsTrnStockTransferSummary/GetStockQuantity';
        let param = {
         stock_gid:stock_gid,
         product_gid:product_gid
        }
        this.service.getparams(api,param).subscribe((result:any) => {
          this.GetPop_list = result.GetPop_list;
          this.productform.get("qty_requested")?.setValue(result.GetPop_list[0].stock_quantity);
        });
      
  
}
OnSubmitPop(){
  debugger;
  
  var params = {
    location_gid : this.RaiseRequestForm.value.location_name.location_gid,
    stock_gid : this.productdetails_list[0].stock_gid,
    product_gid:this.productdetails_list[0].product_gid,
    productuom_gid:this.productdetails_list[0].uom_gid,
    stock_qty:this.productdetails_list[0].stock_qty,
    issued_qty:this.productform.value.total_amount,
  }
      var url='ImsTrnStockTransferSummary/PopSubmit'
      this.service.post(url, params).subscribe((result: any) => {
       if(result.status == false){
        this.GetStockQuantity();
       }
      });
      
}
//Stock Location Transfer Submit
onSubmit() {
  debugger;
  
    if( this.productsummary_list1 == null || this.productsummary_list1 == undefined 
      ){
      window.scrollTo({
        top: 0, 
      });
      this.ToastrService.warning('Atleast One Product Must Be Added!');
      return
    }
  
  console.log(this.RaiseRequestForm.value)
    var params = {
      
      location_name:this.RaiseRequestForm.value.location_name.location_name,
      locationto_name:this.RaiseRequestForm.value.locationto_name.locationto_name,
      stocktransfer_date:this.RaiseRequestForm.value.stocktransfer_date,
      remarks: this.RaiseRequestForm.value.remarks,  
      location_gid : this.RaiseRequestForm.value.location_name.location_gid,
      locationto_gid : this.RaiseRequestForm.value.locationto_name.locationto_gid,
      stock_gid : this.productdetails_list[0].stock_gid,
      product_gid:this.productdetails_list[0].product_gid
    }
    this.NgxSpinnerService.show();
    var url='ImsTrnStockTransferSummary/PostLocationstocktransfer'
  
    this.service.postparams(url, params).subscribe((result: any) => {
      
      if(result.status ==false){
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      
      }
      else{
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.router.navigate(['/ims/ImsTrnStockTransfer']);
        
      }
    
     
    },
    );
  }
  
  onclearrequestor(){
    this.mdlUserName ='';
    

    }
    OnChangerequestor(){}
  
}




function ProductsGroup() {
  throw new Error('Function not implemented.');
}
