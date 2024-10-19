import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-sbc-mst-productmodule',
  templateUrl: './sbc-mst-productmodule.component.html',
  styleUrls: ['./sbc-mst-productmodule.component.scss']
})
export class SbcMstProductmoduleComponent {
   Productmodule: FormGroup | any;
  module_list:any;
  mdlmodule_name:any;
  responsedata:any;
  product_list:any;
  parameterValue: any;
  productmodule_name:any;

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
    this.Productmodule = new FormGroup({
      productmodule_name: new FormControl('', [Validators.required]),     
    })
  }
  ngOnInit(): void {
    this.GetproductmoduleSummary();
    var api = 'Dynamicdb/GetModuledropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.module_list = this.responsedata.Modulelists;
    });
  }
  GetproductmoduleSummary() {
    var url = 'Productmodule/GetproductmoduleSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#product_list').DataTable().destroy();
      this.responsedata = result;
      this.product_list = this.responsedata.productlists;
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#product_list').DataTable();
      }, 1);
    })
  }
  addProduct(){
    let params={
      productmodule_name:this.Productmodule.value.module_name,
      
    }
    this.NgxSpinnerService.show();
    var url = 'Productmodule/PostProductmodule';
    this.SocketService.postparams(url, this.Productmodule.value).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.Productmodule.reset();
        this.NgxSpinnerService.hide();
        this.GetproductmoduleSummary()

      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  }
  onclose(){

  }
  ondelete(){
    var url = 'Productmodule/DeleteProductmodule';
    let params = {
      productmodule_gid : this.parameterValue
    }
    this.SocketService.getparams(url, params).subscribe((result:any) => {
    if(result.status == true){
    this.ToastrService.success(result.message);
    this.GetproductmoduleSummary();
    }
    else {
    this.ToastrService.warning(result.message);
    this.GetproductmoduleSummary();
    }  
    })
    }
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    }
  

}
