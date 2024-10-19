import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { AES } from 'crypto-js';
import { Router } from '@angular/router';


@Component({
  selector: 'app-ims-trn-mrpriceassign-summary',
  templateUrl: './ims-trn-mrpriceassign-summary.component.html',
  styleUrls: ['./ims-trn-mrpriceassign-summary.component.scss']
})
export class ImsTrnMrpriceassignSummaryComponent {

  GetIndentPriceEstimate_list: any[]=[];

  constructor(private serivce: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService,
    private router: Router
  ){}

  ngOnInit(){
    this.GetIndentPriceEstimationSummary();
  }
  GetIndentPriceEstimationSummary(){
    debugger
    this.NgxSpinnerService.show();

    const api = 'IndentPriceEstimation/GetIndentPriceSummary';
    this.serivce.get(api).subscribe((result: any)=>{
      this.GetIndentPriceEstimate_list = result.GetIndentPriceEstimate_list;
      setTimeout(() => {
        $('#GetIndentPriceEstimate_list').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
    });
  }
  Indentview(materialrequisition_gid:any){
    const key = environment.secretKey;
    const param = materialrequisition_gid;
    const materialrequisitiongid = AES.encrypt(param,key).toString();
    this.router.navigate(['/ims/ImsTrnIndentPriceEstimationView',materialrequisitiongid])
  }
}
