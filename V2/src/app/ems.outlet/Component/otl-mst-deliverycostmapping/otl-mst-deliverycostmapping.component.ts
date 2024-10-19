import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';


@Component({
  selector: 'app-otl-mst-deliverycostmapping',
  templateUrl: './otl-mst-deliverycostmapping.component.html',
  styleUrls: ['./otl-mst-deliverycostmapping.component.scss']
})
export class OtlMstDeliverycostmappingComponent {
  showOptionsDivId: any;
  reactiveForm!: FormGroup;
  GetDeliverycost_list: any[] = [];
  parameterValue: any;

  constructor(public service: SocketService,
    private router: Router,
    public route: ActivatedRoute, 
    private ToastrService: ToastrService,
    public NgxSpinnerService: NgxSpinnerService,) {

  }


  ngOnInit() {
    this.reactiveForm = new FormGroup({
      deliverybase_cost : new FormControl(''),
    })
this.Getdeliverycost();    
  }
  Getdeliverycost(){
    var summaryapi = 'DeliveryCost/GetDeliveryCostSummary';
    this.service.get(summaryapi).subscribe((result: any) => {
      $('#GetDeliverycost_list').DataTable().destroy();
      this.GetDeliverycost_list = result.GetDeliverycost_list;
      setTimeout(() => {
        $('#GetDeliverycost_list').DataTable();
      }, 1);
    });
    
  }
  onupdatereset() {
    var postapi = 'DeliveryCost/PostDeliveryCost';
    this.service.post(postapi, this.reactiveForm.value).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.reactiveForm.reset();
      }
      else {
        this.Getdeliverycost();
        this.ToastrService.success(result.message);
        this.reactiveForm.reset();
      }
    });
  }
  onclose() {

  }
  toggleOptions(pincode_id: any) {
    if (this.showOptionsDivId === pincode_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = pincode_id;
    }
  }
  ondelete() {
    console.log(this.parameterValue);
    var url = 'Pincode/DeletePincode'
    let param = {
      pincode_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.Getdeliverycost();
      }
      else {
        this.ToastrService.success(result.message)
        this.Getdeliverycost();
      }
      this.Getdeliverycost();
      this.NgxSpinnerService.hide();
    });
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter

  }
  modelassignpin(deliverycost_id:any){
    debugger
    const key = 'storyboard';
    const param = deliverycost_id;
    const deliverycost_id1 = AES.encrypt(param,key).toString();
    this.router.navigate(['/outlet/OtlMstDeliveryAssignPincode', deliverycost_id1]);
  }
}
