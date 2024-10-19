import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
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
  selector: 'app-crm-smm-shopifycustomerassigned',
  templateUrl: './crm-smm-shopifycustomerassigned.component.html',
  styleUrls: ['./crm-smm-shopifycustomerassigned.component.scss']
})
export class CrmSmmShopifycustomerassignedComponent {
  CurObj: IShopifyCustomer = new IShopifyCustomer();
  selection = new SelectionModel<IShopifyCustomer>(true, []);
  pick: Array<any> = [];
  shopifycustomer_list : any;
  shopifycustomer:any;
  customertotalcount_list:any;
  reactiveFormSubmit!: FormGroup;
  assignleadsubmit!: IAssignLead;
   responsedata: any;
   image:any;
   shopifycustomercount: any;
   customer_assigncount: any;
   customerassignedcount_list: any;
   unassign_count:any;
   customerunassignedcount_list:any;
customer_count: any;

  shopify_customerlist: any[] = [];
  constructor(private formBuilder: FormBuilder, private route:Router,private router: Router,private ToastrService: ToastrService, public service: SocketService) {
    this.assignleadsubmit = {} as IAssignLead;
  }


  ngOnInit(): void {
   
     this.GetShopifyCustomer();
    this.reactiveFormSubmit = new FormGroup({

      customer_type: new FormControl(this.assignleadsubmit.customer_type, [
        Validators.required,

      ]),
      addtocustomer1: new FormControl('N'),
      
     
      source_name: new FormControl(),
     


    });
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");
   }
   GetShopifyCustomer(){
  //   var url='ShopifyCustomer/GetShopifyCustomer'
  //  this.service.get(url).subscribe((result,) => {
  //    this.shopifycustomer = result;
  //    this.shopifycustomer_list=this.shopifycustomer.customers;
  //    console.log(this.shopifycustomer_list)
   
    var url1 = 'ShopifyCustomer/GetShopifyCustomersAssignedList'
    this.service.get(url1).subscribe((result: any) => {
      this.responsedata = result;
    this.shopify_customerlist = this.responsedata.shopifycustomersassigned_list;

      //  console.log(this.shopify_customerlist)
      setTimeout(() => {
        $('#shopify_customerlist').DataTable();
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
  // var url = 'ShopifyCustomer/GetShopifyCustomer'
  // this.service.get(url).subscribe((result: any) => {

  //   this.shopifycustomer = result;
  //   this.shopifycustomer_list = this.shopifycustomer.customers;
   
  //   // console.log(this.shopifycustomer_list)

  // });
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
  OnSubmit() {

    // this.selection.selected.forEach(s => s.schedulelog_gid);
    // this.selection.selected.forEach(s => s.schedule_remarks);
    // this.selection.selected.forEach(s => s.executive);
    this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");
    this.pick = this.selection.selected
    let list = this.pick
    // console.log(list)
    this.CurObj.source_name = this.reactiveFormSubmit.value.source_name;
    this.CurObj.addtocustomer1 = this.reactiveFormSubmit.value.addtocustomer1;
    this.CurObj.customer_type = this.reactiveFormSubmit.value.customer_type;
   this.CurObj.shopifycustomers_lists = list
  //  console.log(this.CurObj)
   if (this.CurObj.shopifycustomers_lists.length != 0 && this.reactiveFormSubmit.value.customer_type != null  ) {
    // console.log(this.CurObj)
    var url1 = 'ShopifyCustomer/GetLeadmoved'
    this.service.post(url1, this.CurObj).pipe().subscribe((result: any) => {

      if (result.status == false) {


        this.ToastrService.warning('Error While Occured Moving Lead')
      }
      else {
       
        this.GetShopifyCustomer();
        this.reactiveFormSubmit.reset();
        window.location.reload();
        this.ToastrService.success('Lead Moved Sucessfully')
        this.reactiveFormSubmit.get("source_name")?.setValue("Shopify");


      }

    });

  }
  else{
    this.ToastrService.warning("Kindly Select Atleast One Record to Move lead  ! ")
  }
}

changeListener($event:any): void {
  this.readThis($event.target);
}

readThis(inputValue: any): void {
  var file: File = inputValue.files[0];
  var myReader: FileReader = new FileReader();

  myReader.onloadend = (e) => {
    this.image = myReader.result;
    console.log('chanti', myReader.result);
  };
  myReader.readAsDataURL(file);
}
}
