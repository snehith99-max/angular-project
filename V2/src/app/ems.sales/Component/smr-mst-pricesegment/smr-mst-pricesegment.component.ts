import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { DualListComponent } from './dual-list/dual-list.component';

interface IPricesegment {
  pricesegment_gid: string;
  pricesegment_name: string;
  pricesegment_code: any;
  pricesegmentedit_name: string;
  pricesegmentedit_code: any;
  discountpercentage: any;
  editdiscountpercentage: any;
}

@Component({
  selector: 'app-smr-mst-pricesegment',
  templateUrl: './smr-mst-pricesegment.component.html',
  styleUrls: ['./smr-mst-pricesegment.component.scss'],
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

export class SmrMstPricesegmentComponent {
  reactiveFormReset!: FormGroup;
  reactiveForm!: FormGroup;
  responsedata: any;
  showOptionsDivId: any;
  pricesegment_list: any;
  pricesegment!: IPricesegment;
  reactiveFormEdit: FormGroup | any;
  parameterValue1: any;
  pricesegmentgrid_list: any[] = [];
  team_list1: any[] = [];
  pricesegmentcustomer_list:any[] = [];
  pricesegmentproduct_list:any[] = [];
  team_list: any[] = [];
  campaign_gid: any;
  branch_list: any[] = [];
  dual_list: any;
  marketingteam_list1: any[] = [];
  teamname: any;
  branch_name: any;
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
  format: any = DualListComponent.DEFAULT_FORMAT;
  private sourceStations: Array<any> = [];
  private confirmedStations: Array<any> = [];
  selectedBranch: any;
  format1: any;
  pricesegment_gid: any;
  employee_gid: any;

  constructor(private formBuilder: FormBuilder, private router: Router, private ToastrService: ToastrService, public service: SocketService, public route: ActivatedRoute) {
    this.pricesegment = {} as IPricesegment;
  }

  data: any;
  parameterValue: any;

  onedit() { }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrMstPricesegmentSummary();

    ///form values for add popup///
    this.reactiveForm = new FormGroup({
      pricesegment_prefix: new FormControl('', [Validators.required,]),
      pricesegment_name: new FormControl(this.pricesegment.pricesegment_name, [Validators.required,]),
      pricesegment_code: new FormControl(this.pricesegment.pricesegment_code, [Validators.required,]),
      discount_percentage: new FormControl(this.pricesegment.discountpercentage,[Validators.required])
    });
    ///form values for edit///
    this.reactiveFormEdit = new FormGroup({
      editpricesegment_prefix: new FormControl('',[Validators.required]),
      pricesegmentedit_name: new FormControl('',[Validators.required]),
      pricesegmentedit_code: new FormControl('',[Validators.required]),
      pricesegment_gid: new FormControl(''),
      editdiscount_percentage: new FormControl('',[Validators.required])
    });
  }
  ////////////Get Summary for price segment//////////////////////
  GetSmrMstPricesegmentSummary() {
    var url = 'SmrMstPricesegmentSummary/GetSmrMstPricesegmentSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#pricesegment_list').DataTable().destroy();
      this.responsedata = result;
      this.pricesegment_list = this.responsedata.pricesegment_list;
      setTimeout(() => {
        $('#pricesegment_list').DataTable();
      }, 1);
    });
  }

  containsNonAlphabeticCharacters(value: string): boolean {
    return /[A-Za-z]/.test(value);
  }
  containsSpaces(value: string): boolean {
    return /\s/.test(value);
  }
  containsOnlyNumbers(value: string): boolean {
    return /[0-9.]+/.test(value);
  }
  /////////For Add PopUp/////////
  get pricesegment_name() {
    return this.reactiveForm.get('pricesegment_name')!;
  }
  get pricesegment_code() {
    return this.reactiveForm.get('pricesegment_code')!;
  }
  get pricesegment_prefix() {
    return this.reactiveForm.get('pricesegment_prefix')!;
  }
  get discount_percentage() {
    return this.reactiveForm.get('discount_percentage')!;
  }
  
  assignedproducts(pricesegment_gid:string){
    var url = "SmrMstPricesegmentSummary/assignedpricesegmentproducts";
    let params = {
      pricesegment_gid:pricesegment_gid
    }
    this.service.getparams(url,params).subscribe((result:any)=>{
      this.pricesegmentproduct_list = result.pricesegmentproduct_list
    });

  }
  assignedcustomers(pricesegment_gid:string){
    var url = "SmrMstPricesegmentSummary/assignedpricesegmentscustomers";
    let params = {
      pricesegment_gid:pricesegment_gid
    }
    this.service.getparams(url,params).subscribe((result:any)=>{
      this.pricesegmentcustomer_list = result.pricesegmentcustomer_list
    });
  }
  onsubmit() {
    if (this.reactiveForm.value.pricesegment_prefix != null && this.reactiveForm.value.pricesegment_name != null && this.reactiveForm.value.pricesegment_code != null) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'SmrMstPricesegmentSummary/PostPriceSegment'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetSmrMstPricesegmentSummary();
          this.reactiveForm.reset();
        }
        else {
          this.reactiveForm.get("pricesegment_code")?.setValue(null);
          this.reactiveForm.get("pricesegment_name")?.setValue(null);
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();

          this.GetSmrMstPricesegmentSummary();
        }
      });
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }

  /////EDIT POPUP/////
  get pricesegmentedit_name() {
    return this.reactiveFormEdit.get('pricesegmentedit_name')!;
  }
  get pricesegmentedit_code() {
    return this.reactiveFormEdit.get('pricesegmentedit_code')!;
  }
  get editpricesegment_prefix() {
    return this.reactiveFormEdit.get('editpricesegment_prefix')!;
  }
  get editdiscount_percentage() {
    return this.reactiveFormEdit.get('editdiscount_percentage')!;
  }

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("editpricesegment_prefix")?.setValue(this.parameterValue1.pricesegment_prefix);
    this.reactiveFormEdit.get("pricesegmentedit_code")?.setValue(this.parameterValue1.pricesegment_code);
    this.reactiveFormEdit.get("pricesegmentedit_name")?.setValue(this.parameterValue1.pricesegment_name);
    this.reactiveFormEdit.get("pricesegment_gid")?.setValue(this.parameterValue1.pricesegment_gid);
    this.reactiveFormEdit.get("editdiscount_percentage")?.setValue(this.parameterValue1.discount_percentage);
  };

  ////////////Update popup////////
  public onupdate(): void {
    this.reactiveFormEdit.value;
    var url = 'SmrMstPricesegmentSummary/UpdatedPriceSegment'
    this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.GetSmrMstPricesegmentSummary();
      }
      else {
        this.ToastrService.success(result.message)
        this.GetSmrMstPricesegmentSummary();
      }
    });
  }
  ////////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url = 'SmrMstPricesegmentSummary/deletePriceSegmentSummary'
    let param = {
      pricesegment_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      this.GetSmrMstPricesegmentSummary();
    });
  }
  ////Expandable Grid////
  ondetail(pricesegment_gid: any) {
    var url = 'SmrMstPricesegmentSummary/GetPricesegmentgrid'
    let param = {
      pricesegment_gid: pricesegment_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.pricesegmentgrid_list = result.pricesegmentgrid_list;
      console.log(this.pricesegmentgrid_list)
      setTimeout(() => {
        $('#pricesegmentgrid_list').DataTable();
      }, 1);
    });
  }

  onclose() {
    this.reactiveForm.reset();
  }

  assignproducts(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstProductAssign', encryptedParam])
  }
  unassignproducts(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstProductunAssign', encryptedParam])
  }
  //Assign//
  openModalemployee(parameter: string) {
    this.parameterValue1 = parameter;
    this.teamname = this.parameterValue1.pricesegment_name;
    this.branch_name = this.parameterValue1.pricesegment_code;
    this.pricesegment_gid = this.parameterValue1.pricesegment_gid;
    //console.log(this.parameterValue1)
    this.teamemployee();
  }

  // openModalemploye(parameter: string) {
  //   this.parameterValue1 = parameter;
  //   this.teamname = this.parameterValue1.pricesegment_name;
  //   this.branch_name = this.parameterValue1.pricesegment_code;
  //   console.log(this.parameterValue1)
  //   this.teamemployee();
  // }

  getListFromManagerList(data:any){ 
    this.team_list1 = data; 
  }

  teamemployee() {
    let param = {
      pricesegment_gid: this.parameterValue1.pricesegment_gid,
      pricesegment_name: this.parameterValue1.pricesegment_name,
    }

    var url = 'SmrMstPricesegmentSummary/GetUnassignedlists'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.sourceStations = result.GetUnassignedlists;
      this.useStations();
    });

    var url1 = 'SmrMstPricesegmentSummary/GetAssignedlists'
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.responsedata = result;
      this.confirmedStations = result.GetAssignedlists;
      this.useStations();
    });
  }

  // private useStations() {
  //   this.key = 'employee_gid';
  //   this.key2 = 'pricesegment_gid';
  //   this.display = 'employee_name'; // [ 'vendor_companyname', 'vendor_gid' ];
  //   this.keepSorted = true;
  //   console.log(this.confirmedStations)
  //   if (this.confirmedStations === null) {
  //     this.source = this.sourceStations
  //     this.confirmed = this.confirmedStations;
  //   }
  //   else if (this.sourceStations === null) {
  //     this.confirmed = this.confirmedStations;
  //     this.source = this.sourceStations

  //   }
  //   else if(this.sourceStations===null){
  //      this.source = '0'
  //     this.confirmed = [this.confirmedStations];
  //   }
  //   else {
  //     this.source = [...this.sourceStations, ...this.confirmedStations];
  //     this.confirmed = this.confirmedStations;
  //     this.employee_gid = this.parameterValue1.employee_gid
  //   }
  // }

  private useStations() {
    debugger;
    this.key = 'employee_gid';
    this.key1 = this.pricesegment_gid;
    this.display = 'employee_name'; // [ 'vendor_companyname', 'vendor_gid' ];
    this.keepSorted = true;
 
    console.log(this.key1)
    if (this.confirmedStations === null) {
      this.source = this.sourceStations
      this.confirmed = this.confirmedStations;
    }
    else if (this.sourceStations === null) {
      this.confirmed = this.confirmedStations;
      this.source = this.sourceStations
 
    }
    // else if(this.sourceStations===null){
    //    this.source = '0'
    //   this.confirmed = [this.confirmedStations];
    // }
    else {
      this.source = [...this.sourceStations, ...this.confirmedStations];
      this.confirmed = this.confirmedStations;
      this.pricesegment_gid = this.parameterValue1.pricesegment_gid
    }
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
    this.format.direction = this.sourceLeft ? DualListComponent.LTR : DualListComponent.RTL;
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
    this.format1.direction = this.sourceLeft ? DualListComponent.LTR : DualListComponent.RTL;
  }

  assign(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/smr/SmrMstpriceassigncustomer',encryptedParam]) 
  } 
  Unassign(params:any){
    debugger;
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/smr/SmrMstPriceunassigncustomer',encryptedParam]) 
  }
  toggleOptions(pricesegment_gid: any) {
    debugger
    if (this.showOptionsDivId === pricesegment_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = pricesegment_gid;
    }
  } 
}