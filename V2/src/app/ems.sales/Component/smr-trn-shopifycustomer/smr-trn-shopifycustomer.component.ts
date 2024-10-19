import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
export class IShopifyCustomer {
  shopifycustomers_lists: string[] = [];
  addtocustomer1: string = "";
  customer_type: string = "";
  source_name: string = "";
}
interface IAssignLead {
  source_name: string;
  customer_type: string;
  addtocustomer1: string;

}
@Component({
  selector: 'app-smr-trn-shopifycustomer',
  templateUrl: './smr-trn-shopifycustomer.component.html',
  styleUrls: ['./smr-trn-shopifycustomer.component.scss']
})
export class SmrTrnShopifycustomerComponent {
  sync_crm_customer: boolean = false;
  selection = new SelectionModel<IShopifyCustomer>(true, []);
  selection7 = new SelectionModel<IShopifyCustomer>(true, []);
  CurObj: IShopifyCustomer = new IShopifyCustomer();
  pick: Array<any> = [];
  responsedata: any;
  shopify_unassignedcustomerlist: any;
  customertotalcount_list: any;
  customer_count: any;
  shopify_customerlist: any[] = [];
  customertype_list: any[] = [];
  reactiveFormSubmit!: FormGroup;
  assignleadsubmit!: IAssignLead;
  customerassignedcount_list: any;
  customer_assigncount: any;
  customerunassignedcount_list: any;
  unassign_count: any;
  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,){
    this.assignleadsubmit = {} as IAssignLead;
  }
  ngOnInit(): void {
    this.reactiveFormSubmit = new FormGroup({
      customer_type: new FormControl(this.assignleadsubmit.customer_type, [
        Validators.required,
      ]),
      addtocustomer1: new FormControl('N'),
      source_name: new FormControl(),
    });
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");

    var url2 = 'ShopifyCustomer/GetCustomerTotalCount'
    this.service.get(url2).subscribe((result,) => {
      this.customertotalcount_list = result;
      this.customer_count = this.customertotalcount_list.customertotalcount_list[0].customer_totalcount;
    });
    var api3 = 'Leadbank/GetCustomerTypeSummary'
    this.service.get(api3).subscribe((result: any) => {
      this.responsedata = result;
      this.customertype_list = result.customertype_list1;
    });
    this.GetShopifyCustomer();
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
  Getsync_shopifycustomer() {
    this.sync_crm_customer = true;
    this.GetShopifyCustomersync();

  }
  Getbacksync_crm_customer() {
    this.sync_crm_customer = false;
    this.GetShopifyCustomer();
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
  OnSubmit() {
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");
    this.pick = this.selection.selected
    let list = this.pick
    // console.log(list)
    this.CurObj.source_name = this.reactiveFormSubmit.value.source_name;
    this.CurObj.addtocustomer1 = this.reactiveFormSubmit.value.addtocustomer1;
    this.CurObj.shopifycustomers_lists = list
    //  console.log(this.CurObj)

    if (this.CurObj.shopifycustomers_lists.length != 0) {
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
  GetShopifyCustomer() {
    var url = 'ShopifyCustomer/GetShopifyCustomer'
    this.service.get(url).subscribe((result,) => {
      //this.shopifycustomer = result;
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
}
