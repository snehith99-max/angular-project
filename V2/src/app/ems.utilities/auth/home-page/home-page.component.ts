import { Component, HostListener } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})



export class HomePageComponent {
  show:boolean =true
  branches :any
  response: any;
  option: boolean = false
  formmobileconfig: FormGroup | any;
    enable_kot: any;
  constructor(private NgxSpinnerService: NgxSpinnerService,private service: SocketService, private router: Router,private ToastrService: ToastrService){
   
  }
  
  ngOnInit() {
  this.NgxSpinnerService.show()
  var api = 'WhatsApporderSummary/whatsappdashboard'
  this.service.get(api).subscribe((result:any) => {
  this.response = result;
  this.NgxSpinnerService.hide();
  }
  )
  
  this.prdtldtl()
  this.Getkotscreensum()
  
  
   
  }
  
  prdtldtl(){
  var api1 = 'WhatsApporderSummary/whatsappproductdtl'
  this.service.get(api1).subscribe((result:any) => {
  this.branches = result;
  
  
  
  }
  )
  
  this.formmobileconfig = new FormGroup({
    manager_number: new FormControl('',[
      Validators.required,
      Validators.pattern(/^[0-9+]+$/)
    ]),
    msgsend_manger: new FormControl('', [
      Validators.required,
    ]),
    owner_number: new FormControl('',[
      Validators.pattern(/^[0-9+]+$/)
    ]),
    msgsend_owner: new FormControl(''),
    branch_gid: new FormControl(''), 
  
  });
  
  
  
  }
  
  optionopen(){
  this.option = !this.option
  }
  
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const targetElement = event.target as HTMLElement;
    const isClickedOutside = !targetElement.closest('.icon_menu') && !targetElement.closest('.fa-ellipsis-vertical');
  
    if (isClickedOutside) {
      this.option = false;
    }
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
  
  onclose(){
    this.formmobileconfig.reset();
  }
    
  
  pushvalue(branches: any) {
    if (branches) {
      this.formmobileconfig.get("branch_gid")?.setValue(branches.branch_gid || '');
      this.formmobileconfig.get("manager_number")?.setValue(branches.manager_number || '');
      this.formmobileconfig.get("msgsend_manger")?.setValue(branches.msgsend_manger || '');
      this.formmobileconfig.get("owner_number")?.setValue(branches.owner_number || '');
      this.formmobileconfig.get("msgsend_owner")?.setValue(branches.msgsend_owner || '');
    } 
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
     
     }
   });
  
  }
  
  
  }