import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { DualListComponent } from '../otl-trn-outletmanage/dual-list/dual-list.component'
import { AES } from 'crypto-js';
import {CountryISO,SearchCountryField,} from "ngx-intl-tel-input";
interface Ioutlet {
  campaign_gid: string;
  campaign_title: string;
  branch: string;
  campaign_description: string;
  employee_name: string;
  employee_gid: string;
  outlet_status: string;

}
@Component({
  selector: 'app-otl-mst-branch',
  templateUrl: './otl-mst-branch.component.html',
  styleUrls: ['./otl-mst-branch.component.scss']
})
export class OtlMstBranchComponent {


  responsedata: any;
  outlet!: Ioutlet;
  campaign_list: any[] = [];
  campaign_gid: any;
  parameterValue1: any;
  ouletmanagergrid_list: any;
  outletemployeegrid_list: any;
  branch_list: any[] = [];
  outletCountList: any[] = [];
  parameterValue: any;
  campaign_name: any;
  branch_name: any;
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
  reactiveFormNumber: FormGroup | any;
  branch_data: any;
  EditProd_list: any[] = []


  SearchCountryField = SearchCountryField;
  CountryISO = CountryISO;
  preferredCountries: CountryISO[] = [
    CountryISO.India,
    CountryISO.India
  ];


  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public route: ActivatedRoute, private router: Router, public service: SocketService, public NgxSpinnerService: NgxSpinnerService) {
    this.outlet = {} as Ioutlet;

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.Getoutletsummary();
    var url = 'OutletManage/GetOtlTrnOutletCount';
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.outletCountList = this.responsedata.outletCountList;
    });

    this.reactiveFormNumber = new FormGroup({
      mobile: new FormControl(''),
      outlet_name: new FormControl(''),
      branch_name: new FormControl(''),
      campaign_gid: new FormControl(''),
      branch_gid: new FormControl(''),

    });

    var api1 = 'OutletManage/Getoutletbranch'
    this.service.get(api1).subscribe((result: any) => {
      this.branch_list = result.branch_list;
      console.log(this.branch_list);
    });


  }

  

  // Encryption
  onAssignProduct(campaign_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlMstAssignProduct', encryptedParam])
  }

  onChangePrice(campaign_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlTrnChangeprice', encryptedParam])
  }
  onRemoveProduct(campaign_gid: any) {
    debugger
    const secretKey = 'storyboarderp';
    const param = (campaign_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlTrnUnassignProduct', encryptedParam])
  }



  onAssignpincode(campaign_gid: any) {
    // const key = 'storyboard';
    // const param = campaign_gid;
    // const campaign_gid1 = AES.encrypt(key,param).toString();
    // this.router.navigate(['/outlet/OtlMstAssignPincode',campaign_gid1]);
debugger
    const key = 'storyboard';
    const param = campaign_gid;   
    const branch_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlMstAssignPincode', branch_gid1]);  }
  OnRemovePincode(campaign_gid: any) {
    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const branch_gid1 = AES.encrypt(param, key).toString();
    this.router.navigate(['/outlet/OtlMstRemovePincode', branch_gid1]);
  }

  onNumber(campaign_gid: any) { }

  //// Summary //////
  Getoutletsummary() {

    var url = 'OtlMstBranch/Getoutletsummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#campaign_list').DataTable().destroy();
      this.responsedata = result;
      debugger
      this.campaign_list = this.responsedata.outlet_list;
      setTimeout(() => {
        $('#campaign_list').DataTable();
      }, 1);
    });
    this.NgxSpinnerService.hide()
  }

  openModaledit(campaign_gid: string) {
    debugger
    const key = 'storyboard';
    const param = campaign_gid;
    const branch_gid1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlMstAmendProduct', branch_gid1]);
  }
  toggleOptions(branch_gid: any) {
    if (this.showOptionsDivId === branch_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = branch_gid;
    }
  }
  Openproductunit(data: any) {
    this.branch_data = data;
    this.reactiveFormNumber.get('branch_name')?.setValue(this.branch_data.branch_name);
    this.reactiveFormNumber.get('outlet_name')?.setValue(this.branch_data.outlet_name);

  }


  editnumber(campaign_gid: any) {
    debugger
    this.reactiveFormNumber.get("campaign_gid")?.setValue(campaign_gid);
    var url = 'OtlMstBranch/GetBranchDetails'

    let param = {
      campaign_gid: campaign_gid
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.EditProd_list = result.outlet_list;
      this.responsedata = result;
      this.reactiveFormNumber.get("branch_name")?.setValue(this.EditProd_list[0].campaign_title);
      this.reactiveFormNumber.get("outlet_name")?.setValue(this.EditProd_list[0].branch);

    });
  }

  onupdate() {
    debugger
    //this.number = this.reactiveFormNumber.value;
    const api = 'OtlMstBranch/PostNumberUpdate';
    this.NgxSpinnerService.show()
    let params ={
      mobile : this.reactiveFormNumber.value.mobile.e164Number,
      campaign_gid : this.EditProd_list[0].campaign_gid
    }
    this.service.postparams(api, params).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.reactiveFormNumber.reset()
          this.NgxSpinnerService.hide()
        }
        else {
          this.ToastrService.success(result.message)
          this.reactiveFormNumber.reset()
          this.router.navigate(['/outlet/OtlMstPriceManagement']);
          this.Getoutletsummary();
          this.NgxSpinnerService.hide()
        }
      });
  }
}