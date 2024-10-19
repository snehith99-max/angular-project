import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { CustomerAssignDualListComponent } from './customer-assign-dual-list/customer-assign-dual-list.component';
import { VendorAssignDualListComponent } from './vendor-assign-dual-list/vendor-assign-dual-list.component';
import { AES } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
interface ITaxsegment {
  tax_name4: any;
  taxsegment_name: string;
  taxsegment_code: string;
  taxsegment_gid: string;
  taxsegment_description: string;
  taxsegment_name_edit: string;
  taxsegment_code_edit: string;
  taxsegment_description_edit: string
  active_flag: string;
  active_flag_edit: string;
}

@Component({
  selector: 'app-smr-mst-taxsegment',
  templateUrl: './smr-mst-taxsegment.component.html',
  styleUrls: ['./smr-mst-taxsegment.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class SmrMstTaxsegmentComponent {

  showOptionsDivId: any; 
  teamname: any;
  branch_name: any;
  isReadOnly = true;
  private unsubscribe: Subscription[] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  tax_list: any[] = [];
  parameterValue1: any;
  taxsegment_list: any[] = [];
  taxsegment!: ITaxsegment;
  smrcustomer_list: any;

  team_list1: any[] = [];
  team_list: any[] = [];
  campaign_gid: any;
  branch_list: any[] = [];
  marketingteam_list1: any[] = [];
  LeadBankCountList: any[] = [];
  assignedmanagers_list: any[] = [];
  assignedemployees_list: any[] = [];
  assignedlead_list: any[] = [];


  GetInterState_list: any [] =[];

  tab = 1;
  keepSorted = true;
  keepSorted1 = true;
  key!: string;
  key1!: string;
  key2!: string;
  key3!: string;
  display!: string;
  display1!: string;
  filter = false;
  source: Array<any> = [];
  confirmed: Array<any> = [];
  filter1 = false;
  source1: Array<any> = [];
  confirmed1: Array<any> = [];
  userAdd = '';
  disabled = false;
  disabled1 = false;
  sourceLeft = true;
  format: any = CustomerAssignDualListComponent.DEFAULT_FORMAT;
  campaign_name: any;
  page: any='tax';


  format1: any = CustomerAssignDualListComponent.DEFAULT_FORMAT;



  private sourceStations: Array<any> = [];


  private confirmedStations: Array<any> = [];
  selectedBranch: any;
  taxsegment_gid: any;
  TaxSegmentCustomer_list: any;
  TaxSegmentVendor_list: any;
  TaxSegmentTotalCustomer_list: any;
  selection1: any;


  constructor(private formBuilder: FormBuilder,
     private ToastrService: ToastrService,
      public service: SocketService, private router: Router,
      public route: ActivatedRoute, 
      private NgxSpinnerService: NgxSpinnerService) {
    this.taxsegment = {} as ITaxsegment;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    var api = 'SmrMstTaxSegment/GetCustomerCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.TaxSegmentCustomer_list = this.responsedata.TaxSegmentCustomer_list;

    });
    var api = 'SmrMstTaxSegment/GetVendorrCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.TaxSegmentVendor_list = this.responsedata.TaxSegmentVendor_list;

    });

 



    //  this.GetSmrTrnCustomerSummary();
    this.GetSmrTrnTotalCustomerSummary();
    this.fetchTaxDetails();
    // Form values for Add popup/////
    this.GetTaxSegmentSummary();
    this.reactiveForm = new FormGroup({

      taxsegment_name: new FormControl(this.taxsegment.taxsegment_name, [
        Validators.required,
        
      ]),
      taxsegment_description: new FormControl(this.taxsegment.taxsegment_description, [
       
      ]),
      taxsegment_code: new FormControl(this.taxsegment.taxsegment_code, [
        Validators.required,
       
      ]),
      // tax_name4: new FormControl(this.taxsegment.tax_name4, [
      //   Validators.required,
      //   Validators.pattern(/^(?!\s*$).+/),
      // ]),

      active_flag: new FormControl('Y', [
        Validators.required,
      ]),

    });
    this.reactiveFormEdit = new FormGroup({

      taxsegment_name_edit: new FormControl(this.taxsegment.taxsegment_name_edit, [

        Validators.required,
        
        //Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces

      ]),

      taxsegment_code_edit: new FormControl(this.taxsegment.taxsegment_code_edit, [
        Validators.required,
       

      ]),
      taxsegment_description_edit: new FormControl(this.taxsegment.taxsegment_description_edit, [

       

      ]),
      taxsegment_gid: new FormControl(''),
      active_flag_edit: new FormControl(''),

    });


  }
  // isAllSelected1() {
  //   const numSelected = this.selection1.selected.length;
  //   const numRows = this.TaxSegmentTotalCustomer_list.length;
  //   return numSelected === numRows;
  // }
  // masterToggle1() {
  //   this.isAllSelected1() ?
  //     this.selection1.clear() :
  //    this.TaxSegmentTotalCustomer_list.forEach((row: IMapProduct) => this.selection1.select(row));
  // }

  //  GetSmrTrnCustomerSummary() {
  //     var url = 'SmrTrnCustomerSummary/GetSmrTrnCustomerSummary'
  //     this.NgxSpinnerService.show();
  //     this.service.get(url).subscribe((result: any) => {

  //       $('#smrcustomer_list').DataTable().destroy();
  //        this.responsedata = result;
  //        this.smrcustomer_list = this.responsedata.smrcustomer_list;
  //        //console.log(this.entity_list)
  //        setTimeout(() => {
  //          $('#smrcustomer_list').DataTable()
  //        }, 1);
  //        this.NgxSpinnerService.hide();

  //     }); 
  //   }
  GetSmrTrnTotalCustomerSummary() {
    var url = 'SmrMstTaxSegment/GetTotalCustomerSummary'
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => {
      $('#TaxSegmentTotalCustomer_list').DataTable().destroy();
      this.responsedata = result;
      this.TaxSegmentTotalCustomer_list = this.responsedata.TaxSegmentTotalCustomer_list;
      this.NgxSpinnerService.hide();

      //console.log(this.taxsegment_list)
      setTimeout(() => {
        $('#TaxSegmentTotalCustomer_list').DataTable();
      }, 1);
    });
  }

  filterBtn() {
    return (this.filter ? 'Hide Filter' : 'Show Filter');
  }

  doDisable() {
    this.disabled = !this.disabled;
  }

  disableBtn() {
    return (this.disabled ? 'Enable' : 'Disabled');
  }

  swapDirection() {
    this.sourceLeft = !this.sourceLeft;
    this.format.direction = this.sourceLeft ? CustomerAssignDualListComponent.LTR : CustomerAssignDualListComponent.RTL;

  }

  filterBtn1() {
    return (this.filter1 ? 'Hide Filter' : 'Show Filter');
  }

  doDisable1() {
    this.disabled = !this.disabled1;
  }

  disableBtn1() {
    return (this.disabled1 ? 'Enable' : 'Disabled');
  }

  swapDirection1() {
    this.sourceLeft = !this.sourceLeft;
    // this.format1.direction = this.sourceLeft ? ManagerListComponent.LTR : ManagerListComponent.RTL;

  }

  private fetchTaxDetails(): void {
    const url = 'SmrTrnSalesorder/GetTax4Dtl';
    this.service.get(url).subscribe((result: any) => {
      // Map the fetched tax details into the desired format
      this.tax_list = result.GetTax4Dtl.map((tax: any) => ({
        tax_name4: `${tax.tax_name4} | ${tax.percentage}%`,
        value: tax.tax_gid
      }));
    });
  }
  Details(productuomclass_gid: any) {

    // this.reactiveFormadd.get("productuomclass_gid")?.setValue(productuomclass_gid);
    // var url = 'SmrMstProductUnit/GetSalesProductUnitSummarygrid'
    // let param = {
    //   productuomclass_gid: productuomclass_gid
    // }
    // this.service.getparams(url, param).subscribe((result: any) => {
    //   this.responsedata = result;
    //   this.salesproductunitgrid_list =  this.responsedata.salesproductunitgrid_list;



    // });
  }
  openModalunAssignCustomer(taxsegment_gid:string){

    debugger
    const key = 'storyboard';
    const param = taxsegment_gid;
    const taxsegment_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/smr/SmrMstunAssignCustomer', taxsegment_gid1]);

  }
  openModalAssignCustomer(taxsegment_gid: string) {
    debugger
    const key = 'storyboard';
    const param = taxsegment_gid;
    const taxsegment_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/smr/SmrMstAssignCustomer', taxsegment_gid1]);
  }
  teamemployee() {

   
    let param = {
      taxsegment_gid: this.parameterValue1.taxsegment_gid,
      customer_gid: ''
    };
    var urlUnassigned = 'SmrMstTaxSegment/GetCustomerUnassignedlist';
    //var urlAssigned = 'SmrMstTaxSegment/GetCustomerAssignedlist';
    this.NgxSpinnerService.show();

    this.service.getparams(urlUnassigned, param).subscribe((resultUnassigned: any) => {
      this.sourceStations = resultUnassigned.GetCustomerUnassignedlist;
      //this.service.getparams(urlAssigned, param).subscribe((resultAssigned: any) => {
        //this.confirmedStations = resultAssigned.GetCustomerassignedlist;
        this.useStations();
        this.NgxSpinnerService.hide();
      });
    //});
  }

  teamassignemployee(){
    debugger
    
    let param = {
      taxsegment_gid: this.parameterValue1.taxsegment_gid,
      customer_gid: ''
    };
    var urlAssigned = 'SmrMstTaxSegment/GetCustomerAssignedlist';
    this.NgxSpinnerService.show();
    this.service.getparams(urlAssigned, param).subscribe((resultAssigned: any) => {
      this.confirmedStations = resultAssigned.GetCustomerassignedlist;
      if(this.confirmedStations == null){
        this.confirmedStations = [];
      }
      else{
        this.confirmedStations = resultAssigned.GetCustomerassignedlist;
      }
      this.useStations();
      this.NgxSpinnerService.hide();
    });
  }
  private useStations() {
    this.key = 'customer_gid';
    this.key1 = 'taxsegment_gid';
    this.display = 'customer_name';
    this.keepSorted = true;
    if (this.confirmedStations === null) {
      this.source = this.sourceStations
      this.confirmed = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed = this.confirmedStations;
      this.source = this.sourceStations

    }
    else {
      this.source = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed = this.confirmedStations;
      this.taxsegment_gid = this.parameterValue1.taxsegment_gid
    }
  }
  //Vendor Assign

  openModalAssignVendor(parameter: string) {
    debugger
    this.parameterValue1 = parameter;
    this.teamname = this.parameterValue1.taxsegment_name;

    this.VendorAssignUnassign();
  }
  VendorAssignUnassign() {

    this.NgxSpinnerService.show();
    let param = {
      taxsegment_gid: this.parameterValue1.taxsegment_gid,
      vendor_gid: ''
    };
    var urlUnassigned = 'SmrMstTaxSegment/GetVendorUnassignedlist';
    var urlAssigned = 'SmrMstTaxSegment/GetVendorAssignedlist';

    this.service.getparams(urlUnassigned, param).subscribe((resultUnassigned: any) => {
      this.sourceStations = resultUnassigned.GetVendorUnassignedlist;
      this.service.getparams(urlAssigned, param).subscribe((resultAssigned: any) => {
        this.confirmedStations = resultAssigned.GetVendorUnassignedlist;
        this.useStationsVendor();
        this.NgxSpinnerService.hide();
      });
    });
  }

  private useStationsVendor() {
    this.key2 = 'vendor_gid';
    this.key3 = 'taxsegment_gid';
    this.display1 = 'vendor_companyname';
    this.keepSorted1 = true;

    if (this.confirmedStations === null) {
      this.source1 = this.sourceStations
      this.confirmed1 = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed1 = this.confirmedStations;
      this.source1 = this.sourceStations
    }
    else {
      this.source1 = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed1 = this.confirmedStations;
    }

    // if (this.sourceStations && this.sourceStations.length > 0) {
    //   this.source = this.sourceStations.map(item => ({ customer_gid: item.customer_gid, customer_name: item.customer_name }));
    // } else {
    //   this.source = [];
    // }

    // if (this.confirmedStations && this.confirmedStations.length > 0) {
    //   const confirmedCustomers = this.confirmedStations.map(item => ({ customer_gid: item.customer_gid, customer_name: item.customer_name }));
    //   // Concatenate the confirmed customers to the source array
    //   this.source = this.source.concat(confirmedCustomers);
    //   // Assign confirmed customers separately to the confirmed array
    //   this.confirmed = confirmedCustomers;
    // } else {
    //   this.confirmed = [];
    //   this.taxsegment_gid = this.parameterValue1.taxsegment_gid;
    // }
  }


  GetTaxSegmentSummary() {
    this.NgxSpinnerService.show();
    var api = 'SmrMstTaxSegment/GetTaxSegmentSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#taxsegment_list').DataTable().destroy();
      this.responsedata = result;
      this.taxsegment_list = this.responsedata.TaxSegmentSummary_list;
      this.NgxSpinnerService.hide();

      //console.log(this.taxsegment_list)
      setTimeout(() => {
        $('#taxsegment_list').DataTable();
      }, 1);
    });
  }
  getListFromEmployeeList(data: any) {
    this.taxsegment_list = data;
  }


  get taxsegment_name() {
    return this.reactiveForm.get('taxsegment_name')!;
  }
  get taxsegment_code() {
    return this.reactiveForm.get('taxsegment_code')!;
  }
  get taxsegment_description() {
    return this.reactiveForm.get('taxsegment_description')!;
  }

  get taxsegment_name_edit() {
    return this.reactiveFormEdit.get('taxsegment_name_edit')!;
  }
  get taxsegment_code_edit() {
    return this.reactiveFormEdit.get('taxsegment_code_edit')!;
  }
  get taxsegment_description_edit() {
    return this.reactiveFormEdit.get('taxsegment_description_edit')!;
  }

  public onsubmit(): void {
    if (this.reactiveForm.valid) {
      // const selectedTaxes = this.reactiveForm.value.tax_name4.map((tax: any) => ({
      //   tax_gid: tax.value,
      //   percentage: parseFloat(tax.tax_name4.split('|')[1].trim()), // Extract percentage from the tax_name4 string
      //   tax_name: tax.tax_name4.split('|')[0].trim() // Extract tax name from the tax_name4 string
      // }));

      const params = {
        // tax_list: selectedTaxes,        
        taxsegment_name: this.reactiveForm.value.taxsegment_name,
        taxsegment_description: this.reactiveForm.value.taxsegment_description,
        taxsegment_code: this.reactiveForm.value.taxsegment_code,
        active_flag: this.reactiveForm.value.active_flag
      }
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      this.NgxSpinnerService.show();
      var url = 'SmrMstTaxSegment/PostTaxSegment'
      this.service.post(url, params).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetTaxSegmentSummary();
          this.reactiveForm.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.GetTaxSegmentSummary();
          this.reactiveForm.reset();

        }
        this.NgxSpinnerService.hide();
        this.GetTaxSegmentSummary();
        this.reactiveForm.reset();

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("taxsegment_name_edit")?.setValue(this.parameterValue1.taxsegment_name);
    this.reactiveFormEdit.get("taxsegment_code_edit")?.setValue(this.parameterValue1.taxsegment_code);
    this.reactiveFormEdit.get("taxsegment_description_edit")?.setValue(this.parameterValue1.taxsegment_description);
    this.reactiveFormEdit.get("taxsegment_gid")?.setValue(this.parameterValue1.taxsegment_gid);
    this.reactiveFormEdit.get("active_flag_edit")?.setValue(this.parameterValue1.active_flag);
  }
  ////////Update popup////////
  public onupdate(): void {
    if (this.reactiveFormEdit.value.taxsegment_name_edit != null && this.reactiveFormEdit.value.taxsegment_code_edit != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;
      this.NgxSpinnerService.show();
      var url = 'SmrMstTaxSegment/UpdatedTaxSegmentSummary'

      this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetTaxSegmentSummary();
          this.reactiveFormEdit.reset();

        }
        else {
          this.ToastrService.success(result.message)
          this.GetTaxSegmentSummary();
          this.reactiveFormEdit.reset();

        }
        this.NgxSpinnerService.show();
        this.GetTaxSegmentSummary();

      });

    }
    else {

      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter

  }
  ondelete() {
    console.log(this.parameterValue);
    this.NgxSpinnerService.show();
    var url = 'SmrMstTaxSegment/deleteTaxSegmentSummary'
    let param = {
      taxsegment_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.reactiveFormEdit.reset();

      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.success(result.message)
        this.GetTaxSegmentSummary();
        this.reactiveFormEdit.reset();

      }
      this.NgxSpinnerService.hide();
      this.GetTaxSegmentSummary();
      this.reactiveFormEdit.reset();

    });
  }

  onclose() {
    this.reactiveForm.reset();
    this.reactiveFormEdit.reset();

  }

  routerpage(page: any) {
    
    if (page == 'withinstatecustomer') {
      this.page = 'withinstatecustomer';
    }
    else if (page == 'intersate') {
      this.page = 'intersate';
    }
    else if (page == 'totalcustomer'){
      this.page = 'totalcustomer';
    }
    else if (page == 'overseas'){
      this.page = 'overseas';
    }
    else if (page == 'unassign'){
      this.page = 'unassign';
    }
    else if (page == 'other'){
      this.page = 'other';
    }
  }
  
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}
