import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { VendorAssignDualList1Component } from './vendor-assign-dual-list1/vendor-assign-dual-list1.component';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';

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
  selector: 'app-pmr-mst-taxsegment',
  templateUrl: './pmr-mst-taxsegment.component.html',
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
export class PmrMstTaxsegmentComponent {
  filter1 = false;
  format1: any = VendorAssignDualList1Component.DEFAULT_FORMAT;
  TaxSegmentVendor_list: any[] = [];
  taxsegment_list: any[] = [];
  parameterValue: any;
  taxsegment!: ITaxsegment;
  parameterValue1: any;
  reactiveFormEdit!: FormGroup;
  reactiveForm!: FormGroup;
  page: any = 'tax';
  active_flag: any;
  responsedata: any;
  isReadOnly = true;
  teamname: any;
  sourceStations: any;
  confirmedStations: any;
  key!: string;
  key1!: string;
  reside_user: string = 'N';
  key2!: string;
  // showOptionsDivId:any
  key3!: string;
  display!: string;
  display1!: string;
  source1: any;
  confirmed1: any;
  keepSorted = true;
  keepSorted1 = true;
  showOptionsDivId: any; 
  rows: any[] = [];


  constructor(public NgxSpinnerService: NgxSpinnerService,
    public service: SocketService,private router: Router,
    public ToastrService: ToastrService,
  ) {
    this.taxsegment = {} as ITaxsegment;
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }

  ngOnInit(): void {
    this.GetTaxSegCount()
    this.reactiveForm = new FormGroup({

      taxsegment_name: new FormControl(this.taxsegment.taxsegment_name, [
        Validators.required,
      
      ]),
      taxsegment_description: new FormControl(''),
      taxsegment_code: new FormControl(this.taxsegment.taxsegment_code, [
        Validators.required,
       
      ]),
      active_flag: new FormControl('Y', [
        Validators.required,
      ]),

    });
    

    this.reactiveFormEdit = new FormGroup({
      taxsegment_gid : new FormControl(''),
      taxsegment_name_edit: new FormControl(this.taxsegment.taxsegment_name_edit, [

        Validators.required,
        //Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces

      ]),

      taxsegment_code_edit: new FormControl(this.taxsegment.taxsegment_code_edit, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      taxsegment_description_edit: new FormControl(''),
      active_flag_edit: new FormControl(''),

    });

    this.GetTaxSegmentSummary();
    
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }


  GetTaxSegCount(){
    var api = 'PmrTaxSegment/GetVendorrCount'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.TaxSegmentVendor_list = this.responsedata.TaxSegment_Vendorlist;

    });
  }
  public onsubmit(): void {
    
    if (this.reactiveForm.valid) {


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
      var url = 'PmrTaxSegment/PostPmrTaxSegment'
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
  
 

  GetTaxSegmentSummary() {
    this.NgxSpinnerService.show();
    var api = 'PmrTaxSegment/GetTaxSegmentSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#taxsegment_list').DataTable().destroy();
      this.responsedata = result;
      this.taxsegment_list = this.responsedata.PmrTaxSegment_list;
      this.NgxSpinnerService.hide();

      //console.log(this.taxsegment_list)
      setTimeout(() => {
        $('#taxsegment_list').DataTable();
      }, 1);
    });
  }

  public onupdate(): void {
    if (this.reactiveFormEdit.value.taxsegment_name_edit != null && this.reactiveFormEdit.value.taxsegment_code_edit != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;
      this.NgxSpinnerService.show();
      var url = 'PmrTaxSegment/UpdatedTaxSegmentSummary'

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
    var url = 'PmrTaxSegment/deleteTaxSegmentSummary'
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
  AssignVendor(taxsegment_gid:string){

    debugger
    const key = 'storyboard';
    const param = taxsegment_gid;
    const taxsegment_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/pmr/PmrMstAssignVendor', taxsegment_gid1]);

  }
  UnAssignVendor(taxsegment_gid: string) {
    debugger
    const key = 'storyboard';
    const param = taxsegment_gid;
    const taxsegment_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/pmr/PmrMstunAssignVendor', taxsegment_gid1]);
  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("taxsegment_name_edit")?.setValue(this.parameterValue1.taxsegment_name);
    this.reactiveFormEdit.get("taxsegment_code_edit")?.setValue(this.parameterValue1.taxsegment_code);
    this.reactiveFormEdit.get("taxsegment_description_edit")?.setValue(this.parameterValue1.taxsegment_description);
    this.reactiveFormEdit.get("taxsegment_gid")?.setValue(this.parameterValue1.taxsegment_gid);
    this.reactiveFormEdit.get("active_flag_edit")?.setValue(this.parameterValue1.active_flag);
  }
  routerpage(page: any) {

    if (page == 'withstate') {
      this.page = 'withstate';
    }
    else if (page == 'interstate') {
      this.page = 'interstate';
    }
    else if (page == 'total') {
      this.page = 'total';
    }
    else if (page == 'overseas') {
      this.page = 'overseas';
    }
    else if (page == 'unassign') {
      this.page = 'unassign';
    }
    else if (page == 'other') {
      this.page = 'other';
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
}
