import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
export class IShopifyPayment {
  shopifypaymentlists: string[] = [];
}
@Component({
  selector: 'app-smr-trn-shopifypayment',
  templateUrl: './smr-trn-shopifypayment.component.html',
  styleUrls: ['./smr-trn-shopifypayment.component.scss']
})
export class SmrTrnShopifypaymentComponent {
  responsedata: any;
  shopifypaymentsummarylist: any;
  shopifyordercountsummary_list: any;
  order_paidcount: any;
  order_penidngcount: any;
  order_refundedcount: any;
  product_count: any;
  order_partiallycount: any;
  total_order: any;
  pick: Array<any> = [];
  selection3 = new SelectionModel<IShopifyPayment>(true, []);
  CurObj3: IShopifyPayment = new IShopifyPayment();
  shopifyordersummarylist: any;
  shopifyorder: any;
  constructor(public service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService){

  }
  ngOnInit(): void {
    this.GetShopifyOrderSummary();
    this.GetShopifyPaymentSummary();
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
}
