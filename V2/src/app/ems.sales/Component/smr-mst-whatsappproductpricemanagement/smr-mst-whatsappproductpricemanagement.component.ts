import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { CountryISO, SearchCountryField, } from "ngx-intl-tel-input";
interface mobileconfig
{
  manager_number:string;
  owner_number:string;
  msgsend_manger:boolean;
  msgsend_owner:boolean;
}
@Component({
  selector: 'app-smr-mst-whatsappproductpricemanagement',
  templateUrl: './smr-mst-whatsappproductpricemanagement.component.html'
})
export class SmrMstWhatsappproductpricemanagementComponent {
  responsedata: any;
  branchsum_list: any;
  total_products: any;
  enable_kot: any;
  formmobileconfig: FormGroup | any;
  mobileconfig!: mobileconfig;
  parameterValue: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public route: ActivatedRoute, private router: Router, public service: SocketService, public NgxSpinnerService: NgxSpinnerService) {
    this.mobileconfig = {} as mobileconfig;

  }
  ngOnInit(): void {
    this.Getoutletsummary();
    this.Getkotscreensum();
    this.formmobileconfig = new FormGroup({
      manager_number: new FormControl(this.mobileconfig.manager_number,[
        Validators.required,
        Validators.pattern(/^[0-9+]+$/)
      ]),
      msgsend_manger: new FormControl(this.mobileconfig.msgsend_manger, [
        Validators.required,
      ]),
      owner_number: new FormControl(this.mobileconfig.owner_number,[
        Validators.pattern(/^[0-9+]+$/)
      ]),
      msgsend_owner: new FormControl(''),
      branch_gid: new FormControl(''),

    });
  }

  Getoutletsummary() {
    this.NgxSpinnerService.show();
    var url = 'SmrMstWhatsappproductpricemanagement/Getwabranchsummary'
    this.service.get(url).subscribe((result: any) => {
      $('#branchsum_list').DataTable().destroy();
      this.responsedata = result;
      this.branchsum_list = this.responsedata.branchsum_list;
      this.total_products =this.responsedata.total_products;
      setTimeout(() => {
        $('#branchsum_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide()

    });
  }
  WaAssignProduct(branch_gid: any) {
    const secretKey = 'storyboarderp';
    const param = (branch_gid);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/smr/SmrMstWaassignproduct', encryptedParam])

  }

  Waupdateprice(branch_gid: string) {
    debugger
    const key = 'storyboard';
    const param = branch_gid;
    const branch_gid1 = AES.encrypt(param, key).toString();
    this.router.navigate(['/smr/SmrMstWaproductpriceupdate', branch_gid1]);
  }
  WaProduct(branch_gid : any){
    debugger
    const key = 'storyboard';
    const param = branch_gid;
    const branchgid = AES.encrypt(param,key).toString();
    this.router.navigate(['/smr/SmrMstBranchwhatsappproductsummary',branchgid]);
  }
  Getkotscreensum(){
    this.NgxSpinnerService.show();
    var url = 'CampaignService/Getkotscreensum';
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.enable_kot = result.enable_kot;
    });
  }
  openModalupdate(parameter: string) {
    this.parameterValue = parameter
    this.formmobileconfig.get("branch_gid")?.setValue(this.parameterValue.branch_gid);
    this.formmobileconfig.get("manager_number")?.setValue(this.parameterValue.manager_number);
    this.formmobileconfig.get("msgsend_manger")?.setValue(this.parameterValue.msgsend_manger);
    this.formmobileconfig.get("owner_number")?.setValue(this.parameterValue.owner_number);
    this.formmobileconfig.get("msgsend_owner")?.setValue(this.parameterValue.msgsend_owner);

  }

  onupdatemobileconfig(){
     this.NgxSpinnerService.show();
    var url = 'SmrMstWhatsappproductpricemanagement/updatemobileconfig'
    this.service.post(url, this.formmobileconfig.value).subscribe((result: any) => {

      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
        this.formmobileconfig.reset();
      }
      else {

        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.Getoutletsummary();
      }
    });

  }
  onclose(){
    this.formmobileconfig.reset();
  }
  toggleswitch(shopstatus: any,branch_gid:any) {
    this.NgxSpinnerService.show();
    var api = 'WhatsApporderSummary/wosshopenable';
    let params = {
      shopstatus: shopstatus,
      branch_gid:branch_gid
    }
    this.service.post(api, params).subscribe((result: any) => {
      this.responsedata = result;
      this.NgxSpinnerService.hide();

      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.Getoutletsummary();
        this.ToastrService.warning(result.message);


      }
      else {
        window.scrollTo({
          top: 0, 
        });
        this.Getoutletsummary();
        this.ToastrService.success(result.message);

      }

    });
  }
}
