import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { DualListComponent } from '../otl-trn-outletmanage/dual-list/dual-list.component'
interface Ioutlet {
  campaign_gid: string;
  campaign_title:string;
  branch:string;
  campaign_description:string;
  employee_name: string;
  employee_gid: string;
  outlet_status:string;
  
}
@Component({
  selector: 'app-otl-trn-outletmanage',
  templateUrl: './otl-trn-outletmanage.component.html',
  styleUrls: ['./otl-trn-outletmanage.component.scss']
})
export class OtlTrnOutletmanageComponent {

  reactiveForm!: FormGroup;
  reactiveFormEdit:FormGroup | any;
  responsedata: any;
  outlet!: Ioutlet;
  campaign_list:any[]=[];
  campaign_gid: any;
  parameterValue1:any;
  ouletmanagergrid_list: any;
  outletemployeegrid_list:any;
  branch_list:any[]=[];
  outletCountList: any [] = [];
  parameterValue: any;
  campaign_name:any;
  branch_name:any;
  private sourceStations: Array<any> = [];
  private confirmedStations: Array<any> = [];
  key!: string;
  key1!: string;
  key2!: string;
  key3!: string;
  keepSorted = true;
  keepSorted1 = true;
  source: Array<any> = [];
  source1: Array<any> = [];
  sourceLeft = true;
  display!: string;
  display1!: string;
  filter = false;
  filter1 = false;
  confirmed: Array<any> = [];
  confirmed1: Array<any> = [];
  disabled = false;
  disabled1 = false;
  format: any = DualListComponent.DEFAULT_FORMAT;
  format1: any = DualListComponent.DEFAULT_FORMAT;
  showOptionsDivId: any;
  rows: any[] = [];


  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public route: ActivatedRoute,private router: Router,public service: SocketService,public NgxSpinnerService:NgxSpinnerService) {
    this.outlet = {} as Ioutlet;
  
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getoutletsummary();
    var url  = 'OutletManage/GetOtlTrnOutletCount';
    this.service.get(url).subscribe((result:any) => {
    this.responsedata = result;
    this.outletCountList = this.responsedata.outletCountList;
    });
    this.reactiveForm = new FormGroup({
      campaign_title: new FormControl(this.outlet.campaign_title, [
        Validators.required,
        Validators.pattern("^(?!\\s*$)[a-zA-Z0-9\\s]*$"),
    ]),
      campaign_description:new FormControl(this.outlet.campaign_description,[
        Validators.required,
        Validators.pattern("^(?!\\s*$)[a-zA-Z0-9\\s]*$"),
      ]),
      branch: new FormControl('', [Validators.required,]),
    });
    
    this.reactiveFormEdit = new FormGroup({
      branch: new FormControl(this.outlet.branch, [
        Validators.required,
      ]),
      campaign_title: new FormControl(this.outlet.campaign_title, [
        Validators.required,
      ]),
      campaign_description: new FormControl(this.outlet.campaign_description, [Validators.required,
        
      ]),
      campaign_gid : new FormControl(''),
      branch_gid : new FormControl(''),
      
    });
    var api1='OutletManage/Getoutletbranch'
    this.service.get(api1).subscribe((result:any)=>{
    this.branch_list = result.branch_list;
    console.log(this.branch_list);
  });
 

  }
  //// Summary //////
  Getoutletsummary() {
    
    var url = 'OutletManage/Getoutletsummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#campaign_list').DataTable().destroy();
      this.responsedata = result;
      debugger
      this.campaign_list = this.responsedata.campaign_list;
      setTimeout(() => {
        $('#campaign_list').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide()
  }
 
  

  get branch() {
    return this.reactiveForm.get('branch')!;
  }
  get branchedit() {
    return this.reactiveFormEdit.get('branch')!;
  }
  get campaign_title() {
    return this.reactiveForm.get('campaign_title')!;
  }
  get editcampaign_title() {
    return this.reactiveFormEdit.get('campaign_title')!;
  }
  get campaign_description() {
    return this.reactiveForm.get('campaign_description')!;
  }
  get editcampaign_description() {
    return this.reactiveFormEdit.get('campaign_description')!;
  }
  

  public onsubmit(): void {
    debugger;
  
    if (this.reactiveForm.value.campaign_title != null && this.reactiveForm.value.campaign_title != '')
      {
  
          this.reactiveForm.value;
          var url = 'OutletManage/PostOutlet'
          this.NgxSpinnerService.show()
          this.service.postparams(url, this.reactiveForm.value).subscribe((result: any) => {
  
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.Getoutletsummary();  
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
            }
            else {
  
              this.reactiveForm.get("branch")?.setValue(null);
              this.reactiveForm.get("campaign_title")?.setValue(null);
              this.reactiveForm.get("campaign_description")?.setValue(null);

              this.ToastrService.success(result.message)
              this.Getoutletsummary();
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
             
  
            }
  
          });
  
        }
        else {
          this.ToastrService.warning('result.message')
        }
  }
  
  
  onclose(){
    this.reactiveForm.reset();
  
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  
  openModaledit(parameter: string) {
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("branch")?.setValue(this.parameterValue1.branch_gid);
    this.reactiveFormEdit.get("campaign_title")?.setValue(this.parameterValue1.campaign_title);
    this.reactiveFormEdit.get("campaign_description")?.setValue(this.parameterValue1.campaign_description);
    this.reactiveFormEdit.get("campaign_gid")?.setValue(this.parameterValue1.campaign_gid);
   

   
  } ;
    onupdate(){
    if (this.reactiveFormEdit.value.branch != null && this.reactiveFormEdit.value.campaign_title != null  && this.reactiveFormEdit.value.campaign_description != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      debugger
      this.reactiveFormEdit.value;
      var url1 = 'OutletManage/PostUpdateoutlet'
      this.service.postparams(url1, this.reactiveFormEdit.value).subscribe((result: any) => {
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.Getoutletsummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.Getoutletsummary();
        }
       
    }); 
  
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModalemployeelist(parameter: string) {
    this.parameterValue1 = parameter;
    console.log(this.parameterValue1)
    this.campaign_name = this.parameterValue1.campaign_title;
    this.branch_name = this.parameterValue1.branch;
    this.employeelist();
  }

 

  openModalmanagerlist(parameter: string) {
    this.parameterValue1 = parameter
    this.campaign_name = this.parameterValue1.campaign_title;
    this.branch_name = this.parameterValue1.branch;
    this.managerlist();
  }
  private useStations() {
    this.key = 'employee_gid';
    this.key1 = 'campaign_gid';
    this.display = 'employee_name';
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
      this.campaign_gid = this.parameterValue1.campaign_gid
    }
  }

  AssignManager(campaign_gid:string){

    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const campaign_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlTrnOutletManageUnassign', campaign_gid1]);

  }

  UnAssignManager(campaign_gid: string) {
    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const campaign_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlTrnOutletManageAssign', campaign_gid1]);
  }

  AssignEmployeee(campaign_gid:string){

    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const campaign_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlTrnOutletManageEmployeeUnassign', campaign_gid1]);

  }

  UnAssignEmployee(campaign_gid: string) {
    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const campaign_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlTrnOutletManageEmployeeAssign', campaign_gid1]);
  }

  managerlist() {
    debugger
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
      campaign_title: this.parameterValue1.campaign_title,
    }
    var url = 'OutletManage/GetotlUnassignedManagerlist'

    this.service.getparams(url, param).subscribe((result: any) => {
      this.sourceStations = result.GetOTLUnassignedManagerlist;
      
      var url1 = 'OutletManage/GetotlAssignedManagerlist'
      this.NgxSpinnerService.show()
      this.service.getparams(url1, param).subscribe((result: any) => {
        debugger
        this.confirmedStations = result.GetOTLAssignedManagerlist;
        this.usemanager();
        this.NgxSpinnerService.show()
      });
    });
  }
  employeelist() {
    
    console.log(this.parameterValue1)
    let param = {
      campaign_gid: this.parameterValue1.campaign_gid,
    }
    var url = 'OutletManage/GetotlUnassignedEmplist'
    this.service.getparams(url, param).subscribe((result: any) => {
    this.sourceStations = result.GetOTLUnassignedEmplist;
      var url1 = 'OutletManage/GetotlAssignedEmplist'
      this.NgxSpinnerService.show()
      this.service.getparams(url1, param).subscribe((result: any) => {
        this.confirmedStations = result.GetOTLAssignedEmplist;
        this.useStations();
        this.NgxSpinnerService.show()
      });
    });
  }
  private usemanager() {
    this.key2 = 'employee_gid';
    this.key3 = 'campaign_gid';
    this.display1 = 'employee_name';
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

  filterBtn1() {
    return (this.filter1 ? 'Hide Filter' : 'Show Filter');
  }

  doDisable1() {
    this.disabled = !this.disabled1;
  }

  disableBtn1() {
    return (this.disabled1 ? 'Enable' : 'Disabled');
  }

  swapDirection() {
    this.sourceLeft = !this.sourceLeft;
    this.format.direction = this.sourceLeft ? DualListComponent.LTR : DualListComponent.RTL;
  }
  openModalinactive(parameter: string){
    this.parameterValue = parameter
  }
  oninactive(){
    console.log(this.parameterValue);
      var url3 = 'OutletManage/GetOutletInactive'
      this.NgxSpinnerService.show()
      this.service.getparams(url3, this.parameterValue).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Outlet Inactivated');
         this.NgxSpinnerService.hide()
        }
        else {
         this.ToastrService.success('Outlet Inactivated Successfully');
         this.NgxSpinnerService.hide()
          }
          this.Getoutletsummary();
      });
     
  }
  openModalactive(parameter: string){
    this.parameterValue = parameter
  }

  onactive(){
    console.log(this.parameterValue);
      var url3 = 'OutletManage/GetOutletActive'
      this.NgxSpinnerService.show()
      
      this.service.getparams(url3, this.parameterValue).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Activating Outlet');
         this.NgxSpinnerService.hide()
        }
        else {
         this.ToastrService.success('Outlet Activated Successfully');
         this.NgxSpinnerService.hide()
          }
          this.Getoutletsummary();
      });
     
  }

  Details(parameter: string,campaign_gid: string){
    this.parameterValue1 = parameter;
    this.campaign_gid = parameter;
    var url = 'OutletManage/Getotlmanagergrid'
    let param = {
      campaign_gid: campaign_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.ouletmanagergrid_list = result.ouletmanagergrid_list;
      this.outlet=this.ouletmanagergrid_list[0].campaign_title;
      console.log(this.ouletmanagergrid_list)
      setTimeout(() => {
        $('#ouletmanagergrid_list').DataTable();
      }, 1);
 
    });
  }
  Detailsemp(parameter: string,campaign_gid: string){
    this.parameterValue1 = parameter;
    this.campaign_gid = parameter;
    let param = {
      campaign_gid: campaign_gid
    }
    var url = 'OutletManage/Getotlemloyeegrid'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.outletemployeegrid_list = result.outletemployeegrid_list;
      this.outlet=this.outletemployeegrid_list[0].campaign_title;
      console.log(this.outletemployeegrid_list)
      setTimeout(() => {
        $('#outletemployeegrid_list').DataTable();
      }, 1);
 
    });
  }
}
