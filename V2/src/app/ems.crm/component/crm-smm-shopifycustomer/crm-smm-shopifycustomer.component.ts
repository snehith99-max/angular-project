import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';

import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';

interface IProduct {
  product_gid: string;

}

interface IProductPrice {

  shopify_productid: string;
  price: string;
  variant_id: string;
  attribute: string;
}
interface IProductQuantity {

  shopify_productid: string;
  inventory_item_id: string;
  inventory_quantity: string;
}

export class IShopifyCustomer {
  shopifycustomers_lists: string[] = [];
  addtocustomer1: string = "";
  customer_type: string = "";
  source_name: string = "";
}
export class IShopifyProduct {
  shopifyproduct_lists: string[] = [];
}
export class IShopifyOrder {
  shopifyorderlists: string[] = [];
}
export class IShopifyInventory {
  shopifyinventorystocksendlist: string[] = [];
}
export class IShopifyPayment {
  shopifypaymentlists: string[] = [];
}
interface IAssignLead {
  source_name: string;
  customer_type: string;
  addtocustomer1: string;

}
@Component({
  selector: 'app-crm-smm-shopifycustomer',
  templateUrl: './crm-smm-shopifycustomer.component.html',
  styleUrls: ['./crm-smm-shopifycustomer.component.scss']
})
export class CrmSmmShopifycustomerComponent {
  emptyFlag1: boolean = true;
  imageSrc: any;
  sellersPermitFile: any;
  DriversLicenseFile: any;
  InteriorPicFile: any;
  ExteriorPicFile: any;
  //base64s
  sellersPermitString!: string;
  DriversLicenseString!: string;
  InteriorPicString!: string;
  ExteriorPicString!: string;
  //json
  finalJson = {};
  productprice!: IProductPrice;
  currentId: number = 0;
  CurObj: IShopifyCustomer = new IShopifyCustomer();
  CurObj1: IShopifyProduct = new IShopifyProduct();
  CurObj2: IShopifyOrder = new IShopifyOrder();
  CurObj3: IShopifyPayment = new IShopifyPayment();
  CurObj4: IShopifyInventory = new IShopifyInventory();
  selection = new SelectionModel<IShopifyCustomer>(true, []);
  selection1 = new SelectionModel<IShopifyProduct>(true, []);
  selection2 = new SelectionModel<IShopifyOrder>(true, []);
  selection3 = new SelectionModel<IShopifyPayment>(true, []);
  selection4 = new SelectionModel<IShopifyInventory>(true, []);
  selection5 = new SelectionModel<IShopifyProduct>(true, []);
  selection6 = new SelectionModel<IShopifyProduct>(true, []);
  selection7 = new SelectionModel<IShopifyCustomer>(true, []);
  pick: Array<any> = [];
  shopifyordersummarylist: any[] = [];
  shopifypaymentsummarylist: any[] = [];
  productinventorylist: any[] = [];
  shopifycustomer_list: any;
  shopifycustomer: any;
  order_paidcount: any;
  order_penidngcount: any;
  order_refundedcount: any;
  product_count: any;
  total_order: any;
  order_partiallycount: any;
  shopifyorder: any;
  customertotalcount_list: any;
  reactiveFormSubmit!: FormGroup;
  assignleadsubmit!: IAssignLead;
  responsedata: any;
  image: any;
  shopifycustomercount: any;
  customer_assigncount: any;
  customerassignedcount_list: any;
  shopifyordercountsummary_list: any;
  unassign_count: any;
  customerunassignedcount_list: any;
  customer_count: any;
  shopifyproduct_list: any;
  shopifyproduct: any;
  product!: IProduct;
  productquantity!: IProductQuantity;
  file!: File;
  image_path: any;
  imageData !: string;
  private unsubscribe: Subscription[] = [];
  reactiveForm!: FormGroup;
  reactiveFormPrice!: FormGroup;
  reactiveFormQuantity!: FormGroup;
  parameterValue: any;
  parameterValue1: any;
  shopifyproductid: any;
  product_gid1: any;
  product_gid: any;
  products: any[] = [];
  response_data: any;
  customertype_list: any[] = [];
  shopify_customerlist: any[] = [];
  shopify_unassignedcustomerlist: any[] = [];
  ////
  sync_shopify: boolean = false;
  syncshopify_list: any[] = [];
  syncshopify_listcount: any[] = [];
  sync_crmlist: any[] = [];
  sync_crmlistcount: any[] = [];
  sync_crm: boolean = false;
  sync_crm_customer: boolean = false;
  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.assignleadsubmit = {} as IAssignLead;
    this.productprice = {} as IProductPrice;
    this.productquantity = {} as IProductQuantity;

  }


  ngOnInit(): void {
    this.reactiveFormPrice = new FormGroup({
      price: new FormControl(this.productprice.price, [
        Validators.required, Validators.maxLength(10),
      ]),
      attribute: new FormControl(this.productprice.attribute, [
        Validators.required, Validators.maxLength(36),
      ]),
      shopify_productid: new FormControl(),
      variant_id: new FormControl(),
    });
    this.reactiveFormQuantity = new FormGroup({
      inventory_quantity: new FormControl(this.productquantity.inventory_quantity, [
        Validators.required,
      ]),
      shopify_productid: new FormControl(),
      inventory_item_id: new FormControl(),
    });


    this.GetProductSummary();
    this.GetShopifyCustomer();
    this.GetShopifyPaymentSummary();
    this.GetShopifyOrderSummary();
    this.GetProductInventorySummary();
    this.reactiveFormSubmit = new FormGroup({
      customer_type: new FormControl(this.assignleadsubmit.customer_type, [
        Validators.required,
      ]),
      addtocustomer1: new FormControl('N'),
      source_name: new FormControl(),
    });
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");
    //Customer type
    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });

  }
  Getsync_shopify() {
    this.sync_shopify = true;
    this.Getsyncshopify();

  }
  Getsync_shopifycustomer() {
    this.sync_crm_customer = true;
    this.GetShopifyCustomersync();

  }
  Getbacksync_shopify() {
    this.sync_shopify = false;
    this.GetProductSummary();

  }
  Getsyncshopify() {

    var api = 'Product/Getsyncshopify';
    this.service.get(api).subscribe((result: any) => {
      $('#syncshopify_list').DataTable().destroy();
      this.response_data = result;
      this.syncshopify_list = this.response_data.product_list;
      this.syncshopify_listcount = this.response_data.product_list.length;

      setTimeout(() => {
        $('#syncshopify_list').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  Getsync_crm() {
    this.sync_crm = true;
    this.Getsynccrm();

  }
  Getbacksync_crm() {
    this.sync_crm = false;
    this.GetProductSummary();

  }
  Getbacksync_crm_customer() {
    this.sync_crm_customer = false;
    this.GetShopifyCustomer();
  }
  Getsynccrm() {
    var api = 'Product/Getsync_crm';
    this.service.get(api).subscribe((result: any) => {
      $('#sync_crmlist').DataTable().destroy();
      this.response_data = result;
      this.sync_crmlist = this.response_data.product_list;
      this.sync_crmlistcount = this.response_data.product_list.length;

      setTimeout(() => {
        $('#sync_crmlist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  GetProductSummary() {
    var api = 'Product/GetShopifyProductSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.product_list;
      setTimeout(() => {
        $('#product').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
    var url = 'Product/GetShopifyProductdetails'
    this.service.get(url).subscribe((result: any) => {
      this.shopifyproduct = result;
      this.shopifyproduct_list = this.shopifyproduct.products;
    });


    var url3 = 'ShopifyCustomer/GetShopifyOrderCountSummary'
    this.service.get(url3).subscribe((result,) => {
      this.shopifyordercountsummary_list = result;
      this.order_paidcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_paidcount;
      this.order_penidngcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_penidngcount;
      this.order_refundedcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_refundedcount;
      this.product_count = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].product_count;
      this.order_partiallycount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_partiallycount;
      this.total_order = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].total_order;
    });
  }
  GetProductInventorySummary() {

    var api = 'Product/GetShopifyProductInventorySummary';
    this.service.get(api).subscribe((result: any) => {

      $('#productinventorylist').DataTable().destroy();
      this.response_data = result;
      this.productinventorylist = this.response_data.productinventory_list;
      setTimeout(() => {
        $('#productinventorylist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
    var url = 'Product/GetShopifyLocation'
    this.service.get(url).subscribe((result: any) => {

      this.shopifyproduct = result;
      this.shopifyproduct_list = this.shopifyproduct.products;


    });


    var url3 = 'ShopifyCustomer/GetShopifyOrderCountSummary'
    this.service.get(url3).subscribe((result,) => {

      this.shopifyordercountsummary_list = result;
      this.order_paidcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_paidcount;
      this.order_penidngcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_penidngcount;
      this.order_refundedcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_refundedcount;
      this.product_count = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].product_count;
      this.order_partiallycount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_partiallycount;
      this.total_order = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].total_order;


    });
  }
  GetShopifyPaymentSummary() {



    var url = 'ShopifyCustomer/GetShopifyPaymentSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.shopifypaymentsummarylist = this.responsedata.shopifypaymentsummary_list;

      //  console.log(this.shopify_customerlist)
      setTimeout(() => {
        $('#shopifypaymentsummarylist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);

    });
    var url3 = 'ShopifyCustomer/GetShopifyOrderCountSummary'
    this.service.get(url3).subscribe((result,) => {

      this.shopifyordercountsummary_list = result;
      this.order_paidcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_paidcount;
      this.order_penidngcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_penidngcount;
      this.order_refundedcount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_refundedcount;
      this.product_count = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].product_count;
      this.order_partiallycount = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].order_partiallycount;
      this.total_order = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].total_order;


    });
  }
  GetShopifyOrderSummary() {


    var url = 'ShopifyCustomer/GetShopifyOrderSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.shopifyordersummarylist = this.responsedata.shopifyordersummary_list;

      //  console.log(this.shopifyordersummarylist)
      setTimeout(() => {
        $('#shopifyordersummarylist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);

    });
    var url = 'ShopifyCustomer/GetShopifyOrder'
    this.service.get(url).subscribe((result: any) => {

      this.shopifyorder = result;
      // this.shopifyproduct_list = this.shopifyproduct.products;


    });

  }

  isAllSelected7() {
    const numSelected = this.selection7.selected.length;
    const numRows = this.shopify_unassignedcustomerlist.length;
    return numSelected === numRows;
  }

  masterToggle7() {
    this.isAllSelected7() ?
      this.selection7.clear() :
      this.shopify_unassignedcustomerlist.forEach((row: IShopifyCustomer) => this.selection7.select(row));
  }
  GetShopifyCustomersync() {
    var url1 = 'ShopifyCustomer/GetShopifyCustomersUnassignedList'
    this.service.get(url1).subscribe((result: any) => {
      $('#shopify_unassignedcustomerlist').DataTable().destroy();
      this.responsedata = result;
      this.shopify_unassignedcustomerlist = this.responsedata.shopifycustomersunassigned_list;

      //  console.log(this.shopify_customerlist)
      setTimeout(() => {
        $('#shopify_unassignedcustomerlist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);

    });


  }
  GetShopifyCustomer() {
    var url = 'ShopifyCustomer/GetShopifyCustomer'
    this.service.get(url).subscribe((result,) => {
      this.shopifycustomer = result;
      //this.shopifycustomer_list = this.shopifycustomer.customers;
      //console.log(this.shopifycustomer_list)
    });

    var url1 = 'ShopifyCustomer/GetShopifyCustomersList'
    this.service.get(url1).subscribe((result: any) => {
      $('#shopify_customerlist').DataTable().destroy();
      this.responsedata = result;
      this.shopify_customerlist = this.responsedata.shopifycustomers_list;

      //console.log(this.shopify_customerlist)
      setTimeout(() => {
        $('#shopify_customerlist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);

    });
    var url2 = 'ShopifyCustomer/GetCustomerTotalCount'
    this.service.get(url2).subscribe((result,) => {

      this.customertotalcount_list = result;
      this.customer_count = this.customertotalcount_list.customertotalcount_list[0].customer_totalcount;


    });
    var url3 = 'ShopifyCustomer/GetCustomerAssignedCount'
    this.service.get(url3).subscribe((result,) => {

      this.customerassignedcount_list = result;
      this.customer_assigncount = this.customerassignedcount_list.customerassignedcount_list[0].customer_assigncount;


    });
    var url4 = 'ShopifyCustomer/GetCustomerUnassignedCount'
    this.service.get(url4).subscribe((result,) => {

      this.customerunassignedcount_list = result;
      this.unassign_count = this.customerunassignedcount_list.customerunassignedcount_list[0].unassign_count;


    });

  }
  isAllSelected4() {
    const numSelected = this.selection4.selected.length;
    const numRows = this.productinventorylist.length;
    return numSelected === numRows;
  }

  masterToggle4() {
    this.isAllSelected4() ?
      this.selection4.clear() :
      this.productinventorylist.forEach((row: IShopifyInventory) => this.selection4.select(row));
  }
  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.products.length;
    return numSelected === numRows;
  }

  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.products.forEach((row: IShopifyProduct) => this.selection1.select(row));
  }

  isAllSelected2() {
    const numSelected = this.selection2.selected.length;
    const numRows = this.shopifyordersummarylist.length;
    return numSelected === numRows;
  }

  masterToggle2() {
    this.isAllSelected2() ?
      this.selection2.clear() :
      this.shopifyordersummarylist.forEach((row: IShopifyOrder) => this.selection2.select(row));
  }
  isAllSelected3() {
    const numSelected = this.selection3.selected.length;
    const numRows = this.shopifypaymentsummarylist.length;
    return numSelected === numRows;
  }

  masterToggle3() {
    this.isAllSelected3() ?
      this.selection3.clear() :
      this.shopifypaymentsummarylist.forEach((row: IShopifyPayment) => this.selection3.select(row));
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.shopify_customerlist.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.shopify_customerlist.forEach((row: IShopifyCustomer) => this.selection.select(row));
  }
  get customer_type() {
    return this.reactiveFormSubmit.get('customer_type')!;
  }
  get price() {
    return this.reactiveFormPrice.get('price')!;
  }
  get attribute() {
    return this.reactiveFormPrice.get('attribute')!;
  }
  get inventory_quantity() {
    return this.reactiveFormQuantity.get('inventory_quantity')!;
  }

  onsubmitinventory() {
    this.pick = this.selection4.selected
    let list = this.pick
    this.CurObj4.shopifyinventorystocksendlist = list
    if (this.CurObj4.shopifyinventorystocksendlist.length != 0) {

      this.NgxSpinnerService.show();
      var url1 = 'ShopifyCustomer/Sendinventorystock'
      this.service.post(url1, this.CurObj4).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Stock Moving')
        }
        else {
          this.GetProductInventorySummary();
          this.NgxSpinnerService.hide();
          this.selection4.clear();
          this.ToastrService.success('Inventory Stock Moved Sucessfully !')

        }

      });
    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Stock! ")
    }
  }
  onsubmitpayment() {
    this.pick = this.selection3.selected
    let list = this.pick
    this.CurObj3.shopifypaymentlists = list
    if (this.CurObj3.shopifypaymentlists.length != 0) {
      this.NgxSpinnerService.show();
      var url1 = 'ShopifyCustomer/Sendpayment'
      this.service.post(url1, this.CurObj3).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Payment Approve')
        }
        else {
          this.GetShopifyPaymentSummary();
          this.GetShopifyOrderSummary();
          this.NgxSpinnerService.hide();
          this.selection3.clear();
          this.ToastrService.success('Payment Approved Sucessfully!')

        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Approve Payment! ")
    }
  }
  onsubmitorder() {

    this.pick = this.selection2.selected
    let list = this.pick
    this.CurObj2.shopifyorderlists = list
    if (this.CurObj2.shopifyorderlists.length != 0) {
      // console.log(this.CurObj2)
      this.NgxSpinnerService.show();
      var url1 = 'ShopifyCustomer/Sendorder'
      this.service.post(url1, this.CurObj2).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Sending Order')
        }
        else {
          this.GetShopifyOrderSummary();
          this.NgxSpinnerService.hide();
          this.selection2.clear();
          this.ToastrService.success('Order Sented Sucessfully !')
        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Sent Order ! ")
    }
  }
  onsubmitproduct() {

    this.pick = this.selection1.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Sendproductmaster'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();

          this.ToastrService.success('Product Moved Sucessfully!')



        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  isAllSelected5() {
    const numSelected = this.selection5.selected.length;
    const numRows = this.syncshopify_list.length;
    return numSelected === numRows;
  }

  masterToggle5() {
    this.isAllSelected5() ?
      this.selection5.clear() :
      this.syncshopify_list.forEach((row: IShopifyProduct) => this.selection5.select(row));
  }
  Postsyncshopify() {

    this.pick = this.selection5.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Sendproductmaster'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();

          this.ToastrService.success('Product Moved Sucessfully!')



        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  isAllSelected6() {
    const numSelected = this.selection6.selected.length;
    const numRows = this.sync_crmlist.length;
    return numSelected === numRows;
  }

  masterToggle6() {
    this.isAllSelected6() ?
      this.selection6.clear() :
      this.sync_crmlist.forEach((row: IShopifyProduct) => this.selection6.select(row));
  }
  Postsynccrm() {

    this.pick = this.selection6.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Postsynccrm'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();

          this.ToastrService.success('Product Moved Sucessfully!')



        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  OnSubmit() {
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");
    this.pick = this.selection.selected
    let list = this.pick
    // console.log(list)
    this.CurObj.source_name = this.reactiveFormSubmit.value.source_name;
    this.CurObj.addtocustomer1 = this.reactiveFormSubmit.value.addtocustomer1;
    this.CurObj.customer_type = this.reactiveFormSubmit.value.customer_type;
    this.CurObj.shopifycustomers_lists = list
    //  console.log(this.CurObj)

    if (this.CurObj.shopifycustomers_lists.length != 0 && this.reactiveFormSubmit.value.customer_type != null) {
      // console.log(this.CurObj)
      this.NgxSpinnerService.show();
      var url1 = 'ShopifyCustomer/GetLeadmoved'
      this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Lead')
        }
        else {
          this.GetShopifyCustomer();
          this.NgxSpinnerService.hide();
          this.selection.clear();
          this.reactiveFormSubmit.reset();
          this.ToastrService.success('Lead Moved Sucessfully')
          this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");

        }

      });

    }
    else {

      this.ToastrService.warning("Kindly Select Atleast One Record to Move Lead  ! ")
    }
  }

  changeListener($event: any): void {
    this.readThis($event.target);
  }

  readThis(inputValue: any): void {
    var file: File = inputValue.files[0];
    var myReader: FileReader = new FileReader();

    myReader.onloadend = (e) => {
      this.image = myReader.result;
      //console.log('chanti', myReader.result);
    };
    myReader.readAsDataURL(file);
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }

  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Product";
    link.href = "assets/media/Excels/UPLF23092048.xlsx";
    link.click();
  }
  updateprice(data: any) {
   // console.log(data);
    this.parameterValue1 = data

    this.reactiveFormPrice.get("price")?.setValue(this.parameterValue1.price);
    this.reactiveFormPrice.get("shopify_productid")?.setValue(this.parameterValue1.shopify_productid);
    this.reactiveFormPrice.get("variant_id")?.setValue(this.parameterValue1.variant_id);
    this.reactiveFormPrice.get("attribute")?.setValue(this.parameterValue1.option1);
  }
  updatequantity(data: any) {
    //console.log(data);
    this.parameterValue1 = data

    this.reactiveFormQuantity.get("inventory_quantity")?.setValue(this.parameterValue1.inventory_quantity);
    this.reactiveFormQuantity.get("shopify_productid")?.setValue(this.parameterValue1.shopify_productid);
    this.reactiveFormQuantity.get("inventory_item_id")?.setValue(this.parameterValue1.inventory_item_id);
  }
  onupdatequantity() {
   // console.log(this.reactiveFormQuantity.value)
   this.NgxSpinnerService.show();
    var url = 'Product/UpdateShopifyProductQuantity'

    this.service.post(url, this.reactiveFormQuantity.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetProductInventorySummary();
        this.reactiveFormPrice.reset();

      }
      else {
        this.ToastrService.success(result.message)
        this.GetProductInventorySummary();
        this.reactiveFormPrice.reset();

      }
    });
    this.NgxSpinnerService.hide();
  }
  onupdateprice() {
    //console.log(this.reactiveFormPrice.value)
    this.NgxSpinnerService.show();
    var url = 'Product/UpdateShopifyProductPrice'

    this.service.post(url, this.reactiveFormPrice.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetProductSummary();
        this.reactiveFormPrice.reset();

      }
      else {
        this.ToastrService.success(result.message)
        this.GetProductSummary();
        this.reactiveFormPrice.reset();

      }
      this.NgxSpinnerService.hide();
    });
  }
  onclose() {

  }
  downloadImage(data: any) {
    if (data.product_image != null && data.product_image != "" && data.product_image != "0" && data.product_image != 0 && data.product_image != "no") {

      saveAs(data.product_image, data.product_gid + '.png');
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')

    }


  }

  onedit(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/crm/CrmSmmShopifyProductEdit', encryptedParam])
    this.NgxSpinnerService.hide();
  }
  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/crm/CrmMstProductView', encryptedParam])
  }
  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {

      formData.append("file", this.file, this.file.name);

      var api = 'Product/ProductUploadExcels'

      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;

        // this.router.navigate(['/crm/CrmMstProductsummary']);
        window.location.reload();
        this.ToastrService.success("Excel Uploaded Successfully")

      });

    }
  }
  oncloseimage() {
    window.location.reload();
  }
  onadd() {
    this.router.navigate(['/crm/CrmSmmShopifyProductAdd'])

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter


  }

  myModaladddetails(parameter: string) {
    //console.log(parameter)
    this.shopifyproductid = parameter;

    this.emptyFlag1 = true;
  }
  public onuploadimage(): void {
    // console.log("id",this.shopifyproductid)
    // console.log("snehith",this.sellersPermitString)
    let param = {
      shopify_productid: this.shopifyproductid,
      path: this.sellersPermitString
    }

    if (this.sellersPermitString != null && this.sellersPermitString != undefined) {

      this.NgxSpinnerService.show();
      var url = 'Product/UpdateShopifyProductImage'
      this.service.postparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          window.scrollTo({



            top: 0, // Code is used for scroll top after event done



          });
          this.ToastrService.warning(result.message)
        }
        else {
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();

          window.scrollTo({



            top: 0, // Code is used for scroll top after event done



          });
          this.ToastrService.success(result.message)
          this.GetProductSummary();
          // window.location.reload();




        }




      });

    }
    else {
      this.ToastrService.warning("Kindly Select File !")
    }
  }

  addPictures() {
    this.finalJson = {
      "sellersPermitFile": this.ExteriorPicString,
      "DriversLicenseFile": this.DriversLicenseString,
      "InteriorPicFile": this.InteriorPicString,
      "ExteriorPicFile": this.ExteriorPicString
    }
  }
  public picked(event: any, field: any) {
    this.currentId = field;
    let fileList: FileList = event.target.files;
    event = null;
    if (fileList.length > 0) {
      const file: File = fileList[0];
      if (field == 1) {
        this.sellersPermitFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 2) {
        this.DriversLicenseFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 3) {
        this.InteriorPicFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 4) {
        this.ExteriorPicFile = file;
        this.handleInputChange(file); //turn into base64

      }
    }
    else {
      // alert("No file selected");
      this.ToastrService.warning("No file selected")
    }
  }


  handleInputChange(files: any) {
    var file = files;
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      this.ToastrService.warning("Invalid Format")
      // alert('invalid format');
      this.emptyFlag1 = false;
      return;
    }
    this.emptyFlag1 = true;
    reader.onloadend = this._handleReaderLoaded.bind(this);
    reader.readAsDataURL(file);
  }
  _handleReaderLoaded(e: any) {
    let reader = e.target;
    var base64result = reader.result.substr(reader.result.indexOf(',') + 1);
    //this.imageSrc = base64result;
    let id = this.currentId;
    switch (id) {
      case 1:
        this.sellersPermitString = base64result;
        break;
      case 2:
        this.DriversLicenseString = base64result;
        break;
      case 3:
        this.InteriorPicString = base64result;
        break;
      case 4:
        this.ExteriorPicString = base64result;
        break
    }

    this.log();
  }

  log() {
    // for debug
    // console.log('1', this.sellersPermitString);
    // console.log('2', this.DriversLicenseString);
    // console.log('3', this.InteriorPicString);
    // console.log('4', this.ExteriorPicString);
  }
  exportExcel() {
    var api7 = 'Product/GetProductReportExport'
    //console.log(this.file)
    this.service.generateexcel(api7).subscribe((result: any) => {
      this.responsedata = result;
      var phyPath = this.responsedata.productexport_list[0].lspath1;
      var relPath = phyPath.split("src");
      var hosts = window.location.host;
      var prefix = location.protocol + "//";
      var str = prefix.concat(hosts, relPath[1]);
      var link = document.createElement("a");
      var name = this.responsedata.productexport_list[0].lsname.split('.');
      link.download = name[0];
      link.href = str;
      link.click();
    });

  }
  ondelete() {
    // console.log(this.parameterValue);
    this.NgxSpinnerService.show();
    var url = 'Product/Getdeleteproductdetails'
    let param = {
      product_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({



          top: 0, // Code is used for scroll top after event done



        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({



          top: 0, // Code is used for scroll top after event done



        });



        this.ToastrService.success(result.message)

      }
      this.GetProductSummary();



    });
  }


}
