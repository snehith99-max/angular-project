import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-ims-mst-assignproduct',
  templateUrl: './ims-mst-assignproduct.component.html',
  styleUrls: ['./ims-mst-assignproduct.component.scss']
})
export class ImsMstAssignproductComponent  {
  imsbin_list:any;
  responsedata: any;
  showOptionsDivId:any;
  Imsform:FormControl|any;
  binformadd:FormControl|any;
  location_name:any;
  parameterValue:any;
  imsbinadd_list:any;
  assignedbin_list:string|any;
  locationgid:any;
  constructor(private renderer: Renderer2,public NgxSpinnerService:NgxSpinnerService,private ToastrService: ToastrService,private el: ElementRef, public service: SocketService, private route: Router, private router: ActivatedRoute) {
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetbinSummary()
    this.binformadd = new FormGroup({
      branch_name:new FormControl(''),
      bin_number:new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      location_name:new FormControl(''),
      location_gid:new FormControl(''),
    });
  }
  GetbinSummary(){
    var url = 'ImsMstBin/Imsbinsummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.imsbin_list = this.responsedata.imsbin_summarylist;
      setTimeout(() => {
        $('#imbin_list').DataTable();
      }, 1);
    });
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  Managebin(parameter: string){
    this.parameterValue = parameter;
    debugger;
    var params = {
      location_gid:this.parameterValue.location_gid,
    }
    var url = 'ImsMstBin/Imsbinaddsummary'
    this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.imsbinadd_list = this.responsedata.imsbinadd_list;
    this.binformadd.get("location_gid")?.setValue(this.imsbinadd_list[0].location_gid);
    });
    const locationgid=this.parameterValue.location_gid
    this.Getassignedbin(locationgid);
  }
  Getassignedbin(locationgid:any){
    var params = {
      location_gid:locationgid,
    }
    var url = 'ImsMstBin/Imsbinadd'
    this.service.getparams(url,params).subscribe((result: any) => {
    this.responsedata = result;
    this.assignedbin_list = this.responsedata.assignedbin_list;
    });
  }
  get bin_number() {
    return this.binformadd.get('bin_number')!;
  }
  onsubmit(){
    debugger
    if (this.binformadd.value.bin_number !== null && 
      this.binformadd.value.bin_number.trim() !== "") {
      for (const control of Object.keys(this.binformadd.controls)) {
        this.binformadd.controls[control].markAsTouched();
      }
      this.binformadd.value;
      var url = 'ImsMstBin/PostBin';
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.binformadd.value)
        .subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.binformadd.reset();
            this.GetbinSummary();
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.binformadd.reset();
            this.GetbinSummary();
          }
        });
    } else {
      this.ToastrService.warning('Kindly Fill Mandatory Field!! ');
    }
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  onclose(){
    
  }

  ondelete(){
    this.NgxSpinnerService.show();
    var url = 'ImsMstBin/Deletebin'
    this.NgxSpinnerService.show();
    let param = {
      bin_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        this.GetbinSummary();
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
    });
  }
}



