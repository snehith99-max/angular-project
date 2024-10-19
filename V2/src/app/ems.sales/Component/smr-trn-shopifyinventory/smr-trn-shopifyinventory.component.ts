import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
export class IShopifyInventory {
  shopifyinventorystocksendlist: string[] = [];
}
interface IProductQuantity {

  shopify_productid: string;
  inventory_item_id: string;
  inventory_quantity: string;
}
interface IProductPrice {

  shopify_productid: string;
  price: string;
  variant_id: string;
  attribute: string;
}
@Component({
  selector: 'app-smr-trn-shopifyinventory',
  templateUrl: './smr-trn-shopifyinventory.component.html',
  styleUrls: ['./smr-trn-shopifyinventory.component.scss']
})
export class SmrTrnShopifyinventoryComponent {
  response_data: any;
  productinventorylist: any;
  shopifyproduct: any;
  shopifyproduct_list: any;
  shopifyordercountsummary_list: any;
  product_count: any;
  reactiveFormQuantity!: FormGroup;
  reactiveFormPrice!: FormGroup;
  pick: Array<any> = [];
  selection4 = new SelectionModel<IShopifyInventory>(true, []);
  CurObj4: IShopifyInventory = new IShopifyInventory();
  productquantity!: IProductQuantity;
  productprice!: IProductPrice;
  parameterValue1: any;
  responsedata: any;
  order_paidcount: any;
  order_penidngcount: any;
  order_refundedcount: any;
  order_partiallycount: any;
  total_order: any;
  products: any;
  showOptionsDivId:any;
  constructor(private NgxSpinnerService: NgxSpinnerService,  private ToastrService: ToastrService, public service: SocketService) {
    this.productquantity = {} as IProductQuantity;
    this.productprice = {} as IProductPrice;
  }
  ngOnInit(): void {
    this.reactiveFormQuantity = new FormGroup({
      inventory_quantity: new FormControl(this.productquantity.inventory_quantity, [
        Validators.required,
      ]),
      shopify_productid: new FormControl(),
      inventory_item_id: new FormControl(),
    });
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
    this.GetProductInventorySummary();
    this.GetProductSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  get inventory_quantity() {
    return this.reactiveFormQuantity.get('inventory_quantity')!;
  }
  get price() {
    return this.reactiveFormPrice.get('price')!;
  }
  get attribute() {
    return this.reactiveFormPrice.get('attribute')!;
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
  updatequantity(data: any) {
    //console.log(data);
    this.parameterValue1 = data

    this.reactiveFormQuantity.get("inventory_quantity")?.setValue(this.parameterValue1.inventory_quantity);
    this.reactiveFormQuantity.get("shopify_productid")?.setValue(this.parameterValue1.shopify_productid);
    this.reactiveFormQuantity.get("inventory_item_id")?.setValue(this.parameterValue1.inventory_item_id);
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
  toggleOptions(inventory_item_id: any) {
    if (this.showOptionsDivId === inventory_item_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = inventory_item_id;
    }
  }
}
