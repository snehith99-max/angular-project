import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-hrm-trn-de-promotionhistory',
  templateUrl: './hrm-trn-de-promotionhistory.component.html',
  styleUrls: ['./hrm-trn-de-promotionhistory.component.scss']
})
export class HrmTrnDePromotionhistoryComponent {
  promotion_historylist: any[] = [];
  responsedata: any;
  employee_gid: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: ActivatedRoute, public NgxSpinnerService:NgxSpinnerService, private router: Router, public service: SocketService) {}

  ngOnInit(){  
    const employee_gid = this.route.snapshot.paramMap.get('employee_gid');
    this.employee_gid = employee_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee_gid, secretKey).toString(enc.Utf8);
    this.GetPromotionHistorySummary(deencryptedParam);
    console.log(deencryptedParam)
    
  }
  GetPromotionHistorySummary(employee_gid: any) {
    var url = 'HrmTrnDePromote/GetDePromotionHistorySummary'
    const params = { employee_gid: employee_gid };
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.promotion_historylist = this.responsedata.DePromotionHistorysummary_list;
      setTimeout(() => {
        $('#promotion_historylist').DataTable();
      },);
    });
  }

}
