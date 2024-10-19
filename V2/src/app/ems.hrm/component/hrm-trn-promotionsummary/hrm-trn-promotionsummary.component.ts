import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-hrm-trn-promotionsummary',
  templateUrl: './hrm-trn-promotionsummary.component.html',
  styleUrls: ['./hrm-trn-promotionsummary.component.scss']
})
export class HrmTrnPromotionsummaryComponent {
  promotion_managementlist: any[] = [];
  parameterValue: any;
  responsedata: any;

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, private router: Router, public service: SocketService){}

  ngOnInit(){

    this.GetPromotionSummary();
    
  }
  GetPromotionSummary() {
    var url = 'HrmTrnPromotionManagement/GetPromotionSummary'
    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.promotion_managementlist = this.responsedata.Promotionsummarylist;
      setTimeout(() => {
        $('#promotion_managementlist').DataTable();
      },);
    });
  }

  history(params:any){
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param,secretKey).toString();
    this.router.navigate(['/hrm/HrmTrnPromotionhistory',encryptedParam])
  }
 ////////Delete popup////////
 openModaldelete(parameter: string) {
  this.parameterValue = parameter
}

     ondelete(){
      console.log(this.parameterValue);
        var url = 'HrmTrnPromotionManagement/DeletePromotion'
        this.service.getid(url, this.parameterValue).subscribe((result: any) => {
          $('#promotion_managementlist').DataTable().destroy();
          if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.success(result.message)
            this.GetPromotionSummary();
          }
        });
     }

}
