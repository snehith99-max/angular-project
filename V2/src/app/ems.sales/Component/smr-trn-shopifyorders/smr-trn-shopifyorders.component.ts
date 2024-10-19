import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';

export class IShopifyOrder {
  shopifyorderlists: string[] = [];
}
@Component({
  selector: 'app-smr-trn-shopifyorders',
  templateUrl: './smr-trn-shopifyorders.component.html',
  styleUrls: ['./smr-trn-shopifyorders.component.scss']
})
export class SmrTrnShopifyordersComponent {

  shopifyordersummarylist: any[] = [];
  responsedata:any;
  shopifyorder:any;
  pick: Array<any> = [];
  selection2 = new SelectionModel<IShopifyOrder>(true, []);
  CurObj2: IShopifyOrder = new IShopifyOrder();
  shopifyordercountsummary_list:any;
  order_paidcount:any;
  order_penidngcount:any;
  order_refundedcount:any;
  product_count:any;
  order_partiallycount:any;
  total_order:any;
  showOptionsDivId:any;
  customer_address:any;
  customer_contact_person:any;
  currentTab:any;
  notassigned_list:any;
  assigned_list:any;




  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService) {

  }

  ngOnInit():void{
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
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
    this.GetShopifyOrderSummary();
  }
  GetShopifyOrderSummary() {


    var url = 'ShopifyCustomer/GetShopifyOrderSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.responsedata = result;
      this.shopifyordersummarylist = this.responsedata.shopifyordersummary_list;
      // this.assigned_list=this.shopifyordersummarylist.filter(item => item.status_flag==='Assigned');
      // this.notassigned_list=this.shopifyordersummarylist.filter(item => item.status_flag==='Not Assign');
      setTimeout(() => {
        $('#product1').DataTable({
          "pageLength": 50, // Number of rows to display per page
          "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
        });
      }, 1);
    });
    var url = 'ShopifyCustomer/GetShopifyOrder'
    this.service.get(url).subscribe((result: any) => {
      this.shopifyorder = result;
    });

  }
  toggleOptions(salesorder_gid: any) {
    if (this.showOptionsDivId === salesorder_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = salesorder_gid;
    }
  }
  popmodal(parameter: any,customer_contact_person:any) {
    this.customer_address = parameter,
    this.customer_contact_person = customer_contact_person
    
  } 
  

  onview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.route.navigate(['/smr/SmrTrnShopifyordersview',encryptedParam])
  }
  
  showTab(tab: string) {
    this.currentTab = tab;
  }
  

}
