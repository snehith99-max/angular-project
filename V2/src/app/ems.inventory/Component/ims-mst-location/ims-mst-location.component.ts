import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
interface product
{
  branch_name:string,
  location_code:string,
  location_name:string,
  branch_gid:string,
}
@Component({
  selector: 'app-ims-mst-location',
  templateUrl: './ims-mst-location.component.html',
  styleUrls: ['./ims-mst-location.component.scss'],
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
export class ImsMstLocationComponent  {
  imslocation_list:any;
  responsedata: any;
  Locationform:FormControl|any;
  Locationformedit:FormControl|any;
  product!: product;
  mdlbranch:any;
  Imsbranch_list:string[]=[];
  showOptionsDivId:any;
  parameterValue1: any;
  parameterValue:any;
  parameterValue2:any;
  constructor(private renderer: Renderer2,public NgxSpinnerService:NgxSpinnerService,private ToastrService: ToastrService,private el: ElementRef, public service: SocketService, private route: Router, private router: ActivatedRoute) {
    this.product = {} as product;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetLocationSummary()
    this.Getlocationbranch()
    this.Locationform = new FormGroup({
      branch_name:new FormControl(''),
      location_code:new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      location_name:new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
    });
    this.Locationformedit = new FormGroup({
      branch_name:new FormControl(''),
      location_code:new FormControl(''),
      location_name:new FormControl('',[Validators.required,Validators.pattern(/^\S.*$/)]),
      location_gid:new FormControl(''), 
    });
  }
  GetLocationSummary(){
    var url = 'ImsMstLocation/Imslocationsummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.imslocation_list = this.responsedata.locationsummary_list;
      setTimeout(() => {
        $('#imslocation_list').DataTable();
      }, 1);
    });
  }
  Getlocationbranch(){
var url='ImsMstLocation/Imslocationbranch'
this.service.get(url).subscribe((result: any) => {
  this.responsedata = result;
  this.Imsbranch_list = this.responsedata.locationbranch_list;
});
  }
  get location_code() {
    return this.Locationform.get('location_code')!;
  }
  get location_name() {
    return this.Locationform.get('location_name')!;
  }
  get branch_name() {
    return this.Locationform.get('branch_name')!;
  }
  get location_gid() {
    return this.Locationform.get('location_gid')!;
  }
  onclose(){ 
    this.Locationform.reset();
  }
  onsubmit() {
    debugger
    if (this.Locationform.value.location_name !== "" && 
    this.Locationform.value.location_name[0] !== " ") {
      for (const control of Object.keys(this.Locationform.controls)) {
        this.Locationform.controls[control].markAsTouched();
      }
      this.Locationform.value;
      var url = 'ImsMstLocation/PostLocation';
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.Locationform.value)
        .subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.Locationform.reset();
            this.GetLocationSummary();
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.Locationform.reset();
            this.GetLocationSummary();
          }
        });
    } else {
      this.ToastrService.warning('Please enter a valid location name. It cannot be start with a space. !! ');
    }
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.Locationformedit.get("location_code")?.setValue(this.parameterValue1.location_code);
    this.Locationformedit.get("location_name")?.setValue(this.parameterValue1.location_name);
    this.Locationformedit.get("branch_name")?.setValue(this.parameterValue1.branch_gid);
    this.Locationformedit.get("location_gid")?.setValue(this.parameterValue1.location_gid);
  }
  onUpdate(){
    if (
      this.Locationformedit.value.location_name != null) {
      this.Locationformedit.value;
      var url = 'ImsMstLocation/Editlocation';
      this.NgxSpinnerService.show();
      this.service.post(url, this.Locationformedit.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
          this.Locationformedit.reset();
          this.GetLocationSummary();
        } else {
          
          this.ToastrService.success(result.message);
          this.NgxSpinnerService.hide();
          this.Locationformedit.reset();
          this.GetLocationSummary();
        }
        
      });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  onclose2(){
    this.Locationformedit.reset();
  }
  openModaldelete(parameter: string) {
    this.parameterValue2 = parameter
  }
  ondelete(){
    var url = 'ImsMstLocation/Deletelocationsummary';
    let param = {
      location_gid: this.parameterValue2
    };
    this.NgxSpinnerService.show();
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        this.GetLocationSummary();
        this.ToastrService.warning(result.message);
      } else {
        this.NgxSpinnerService.hide();
        this.GetLocationSummary();
        this.ToastrService.success(result.message);
      }
    });
  }
}



